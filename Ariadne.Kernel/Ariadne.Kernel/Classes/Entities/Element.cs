using System;
using System.Collections.Generic;
using Ariadne.Kernel.Math;

namespace Ariadne.Kernel
{
    /// <summary>
    /// Abstract class of the element
    /// </summary>
    abstract class Element
    {
        /// <summary>
        /// Parent model
        /// </summary>
        private object _parentModel = null;

        /// <summary>
        /// Bounding box of element
        /// </summary>
        private BoundingBox _boundingBox = null;

        /// <summary>
        /// Protected constructor with parameters
        /// </summary>
        /// <param name="parameters">Element parameters</param>
        protected Element(ElementParams parameters)
        {
            // Initializing
            ID = parameters.ID;
            Dim = parameters.Dim;
            CornerNodeIDs = IntSet.FromArray(parameters.CornerNodes);
            NodeIDs = IntSet.FromArray(parameters.Nodes);
            PropertyID = parameters.PropertyID;
            Coords = new Vector3D(parameters.Coords);
        }

        /// <summary>
        /// Virtual method to return the type of the element
        /// </summary>
        /// <returns>Type of element</returns>
        public abstract ElementType GetElementType();

        /// <summary>
        /// Virtual method to return the result of validate the element
        /// </summary>
        /// <returns>Returns true if the element is valid, otherwise - false</returns>
        public abstract bool IsValid();

        /// <summary>
        /// Virtual method for checking whether a point belongs to a element
        /// </summary>
        /// <returns>true if point belong to element; otherwise return - false</returns>
        public abstract bool IsPointBelong(Vector3D point);

        /// <summary>
        /// Element identifier
        /// </summary>
        public int ID { get; protected set; }

        /// <summary>
        /// Dimension of the element
        /// </summary>
        public int Dim { get; protected set; }

        /// <summary>
        /// Coordinates
        /// </summary>
        public Vector3D Coords { get; protected set; }

        /// <summary>
        /// Identifier of the element property
        /// </summary>
        public int PropertyID { get; protected set; }

        /// <summary>
        /// Set of node IDs forming the element
        /// </summary>
        public IntSet NodeIDs { get; protected set; }

        /// <summary>
        /// Set of the element's corner node IDs
        /// </summary>
        public IntSet CornerNodeIDs { get; protected set; }

        /// <summary>
        /// Shape function of element
        /// </summary>
        public ShapeFunction ShapeFunction { get; protected set; }

        /// <summary>
        /// The method returns a reference to a set of element nodes
        /// </summary>
        /// <returns>Reference to a set of element nodes</returns>
        public NodeSet GetNodes()
        {
            if (_parentModel == null)
                throw new ArgumentNullException("The parent model of element is null");

            if (!(_parentModel is Model))
                throw new InvalidCastException("The parent model object is not the model type");

            if (NodeIDs == null || NodeIDs.Count <= 0)
                throw new ArgumentNullException("Set of node IDs is null or empty");

            var nodes = new NodeSet();
            foreach (var modelNode in ((Model)_parentModel).Nodes)
            {
                foreach (var nodeID in NodeIDs)
                {
                    if (nodeID == modelNode.ID)
                        nodes.Add(modelNode.ID, modelNode);
                }
            }

            return nodes;
        }

        /// <summary>
        /// The method returns a reference to a set of element corner nodes
        /// </summary>
        /// <returns>Reference to a set of element corner nodes</returns>
        public NodeSet GetCornerNodes()
        {
            if (_parentModel == null)
                throw new ArgumentNullException("The parent model of element is null");

            if (!(_parentModel is Model))
                throw new InvalidCastException("The parent model object is not the model type");

            if (NodeIDs == null || NodeIDs.Count <= 0)
                throw new ArgumentNullException("Set of node IDs is null or empty");

            var cornerNodes = new NodeSet();
            foreach (var modelNode in ((Model)_parentModel).Nodes)
            {
                foreach (var cornerNodeID in CornerNodeIDs)
                {
                    if (cornerNodeID == modelNode.ID)
                        cornerNodes.Add(modelNode.ID, modelNode);
                }
            }

            return cornerNodes;
        }

        /// <summary>
        /// The method returns a reference to a bounding box of element
        /// </summary>
        /// <returns>Reference to a bounding box of element</returns>
        public ref BoundingBox GetBoundingBoxAsRef()
        {
            return ref _boundingBox;
        }

        /// <summary>
        /// The method tries to form a bounding box for the current element
        /// </summary>
        /// <returns>Returns true if the result is successful, otherwise - false</returns>
        private bool TryBuildBoundingBoxesToElements()
        {
            if (_parentModel == null)
                throw new ArgumentNullException("The parent model of element is null");

            if (!(_parentModel is Model))
                throw new InvalidCastException("The parent model object is not the model type");

            if (NodeIDs == null || NodeIDs.Count <= 0)
                throw new ArgumentNullException("Set of node IDs is null or empty");

            _boundingBox = AABoundingBox.CreateByNodeSet(GetCornerNodes());

            if(_boundingBox == null)
                throw new ArgumentNullException("Bounding box of element is null");

            return true;
        }

        /// <summary>
        /// The method returns a reference to the parent model of the element
        /// </summary>
        /// <returns>Reference to the parent model</returns>
        public ref object GetParentModelRef()
        {
            return ref _parentModel;
        }

        /// <summary>
        /// The method updates the content of the element, binds the nodes through a specific model,
        /// which is passed by reference. This method is called when constructing a specific model 
        /// through the reflection mechanism. Be careful and call the method only in the context of 
        /// a specific model constructor.
        /// </summary>
        /// <param name="parentModel">Reference to specific parent model</param>
        /// <returns>Returns true if the result is successful, otherwise - false</returns>
        protected bool Update(in object parentModel)
        {
            _parentModel = parentModel;

            return TryBuildBoundingBoxesToElements();
        }

        /// <summary>
        /// The method checks whether the point is inside the Element
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns>Returns true if point inside the element, otherwise - false</returns>
        protected bool IsPointInsideElement(Vector3D point)
        {
            var corners = GetCornerNodes();
            if (corners.Count == 0)
                return false;

            var mesh = new List<Vector3D>();
            foreach (var corner in corners)
            {
                mesh.Add(corner.Coords);
            }

            var result = Utils.CalculatePositionRelativelyMesh(point, mesh);
            if (result == Utils.LocationType.Vertex ||
                result == Utils.LocationType.Edge ||
                result == Utils.LocationType.Facet ||
                result == Utils.LocationType.Cell)
                return true;

            return false;
        }
    }

    /// <summary>
    /// Abstract class of the element creator
    /// </summary>
    abstract class ElementCreator
    {
        /// <summary>
        /// Protected base constructor for implementing abstract parameter passing when creating a specific element class.
        /// </summary>
        /// <param name="parameters">Element parameters</param>
        protected ElementCreator(ElementParams parameters) { }

        /// <summary>
        /// A virtual function for building elements. Must be implemented in each specific class and returns the constructed element
        /// The same factory method
        /// </summary>
        /// <returns>Some constructed element</returns>
        public abstract Element BuildElement();

        /// <summary>
        /// Returns the specific creator of the element using the data contained in the passed parameters
        /// </summary>
        /// <param name="parameters">Element parameters</param>
        /// <returns>A specific instance of the element creator</returns>
        public static ElementCreator GetElementCreatorByParams(ElementParams parameters)
        {
            ElementCreator creator = null;
            var type = parameters.TypeName;

            // TODO: Hide the implementation of selecting a specific creator, so that the registration of a specific type occurs somewhere in a separate place
            if (type == "CQUAD4")
            {
                creator = new CQUAD4Creator(parameters);
            }
            else if (type == "CTRIA3")
            {
                creator = new CTRIA3Creator(parameters);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Unknown type of element");
            }

            // TODO: Insert new type of elements

            return creator;
        }
    }

    /// <summary>
    /// The basic structure of the generalized parameters of the element
    /// </summary>
    struct ElementParams
    {
        /// <summary>
        /// Element identifier
        /// </summary>
        public int ID;

        /// <summary>
        /// Dimension of the element
        /// </summary>
        public int Dim;

        /// <summary>
        /// Name of the element type
        /// </summary>
        public string TypeName;

        /// <summary>
        /// Identifier of the element property
        /// </summary>
        public int PropertyID;

        /// <summary>
        /// Array of nodes forming the element
        /// </summary>
        public int[] Nodes;

        /// <summary>
        /// Array of the element's corner nodes
        /// </summary>
        public int[] CornerNodes;

        /// <summary>
        /// Array of the element's coordinates
        /// </summary>
        public float[] Coords;
    }

    /// <summary>
    /// Enumeration of all available types of elements
    /// </summary>
    enum ElementType
    {
        CELAS1,
        CELAS2,
        CELAS3,
        CELAS4,
        CDAMP1,
        CDAMP2,
        CDAMP3,
        CDAMP4,
        CMASS1,
        CMASS2,
        CMASS3,
        CMASS4,
        CBAR,
        CBEAM,
        CBEND,
        CONROD,
        CROD,
        CTUBE,
        CVISC,
        CSHEAR,
        CRAC2D,
        CQUAD4,
        CQUAD8,
        CQUADR,
        CTRIA3,
        CTRIA6,
        CTRIAR,
        CPLSTN3,
        CPLSTN4,
        CPLSTN6,
        CPLSTN8,
        CPLSTS3,
        CPLSTS4,
        CPLSTS6,
        CPLSTS8,
        CTETRA,
        CPYRAM,
        CHEXA,
        CPENTA,
        CRAC3D,
        CTRAX3,
        CQUADX4,
        CTRAX6,
        CQUADX8,
        CBUSH,
        SBUSH1D,
        CWELD,
        CGAP,
        CONM1,
        CONM2,
        CHBDYP,
        CHBDYG,
        RBAR,
        RBE1,
        RBE2,
        RBE3,
        RROD,
        RTRPLT,
    }
}
