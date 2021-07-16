using System;
using Ariadne.Kernel.Math;

namespace Ariadne.Kernel
{
    /// <summary>
    /// Class of the node
    /// </summary>
    class Node
    {
        /// <summary>
        /// Parent model
        /// </summary>
        private object _parentModel = null;

        /// <summary>
        /// A set of references to node parent elements
        /// </summary>
        private ElementSet _parentElements = new ElementSet();

        /// <summary>
        /// Node identifier
        /// </summary>
        public int ID { get; protected set; }

        /// <summary>
        /// ID of the reference coordinate system
        /// </summary>
        public int RefCSysID { get; protected set; }

        /// <summary>
        /// ID of the analysis coordinate system
        /// </summary>
        public int AnalysisCSysID { get; protected set; }

        /// <summary>
        /// Coordinates
        /// </summary>
        public Vector3D Coords { get; protected set; } 

        /// <summary>
        /// Constructor by parameters
        /// </summary>
        /// <param name="parameters">Node parameters</param>
        public Node(NodeParams parameters)
        {
            // TODO: Implement a class
            ID = parameters.ID;
            RefCSysID = parameters.RefCSysID;
            AnalysisCSysID = parameters.AnalysisCSysID;
            Coords = new Vector3D(parameters.Coords);
            ParentElementIDs = IntSet.FromArray(parameters.ParentElementIDs);
            // Type = parameters.TypeName;
        }

        /// <summary>
        /// Method to return the type of the node
        /// </summary>
        /// <returns>Type of node</returns>
        public NodeType GetNodeType() 
        { 
            return NodeType.General; 
        }

        /// <summary>
        /// Set of parent element IDs forming the node
        /// </summary>
        public IntSet ParentElementIDs { get; protected set; }

        /// <summary>
        /// The method attempts to populate a set of elements for the current node
        /// </summary>
        /// <returns>Returns true if the result is successful, otherwise - false</returns>
        private bool TryLinkElementsToNodes()
        {
            if (_parentModel == null)
                throw new ArgumentNullException("The parent model of node is null");

            if (!(_parentModel is Model))
                throw new InvalidCastException("The parent model object is not the model type");

            if (ParentElementIDs == null || ParentElementIDs.Count <= 0)
                throw new ArgumentNullException("Set of element IDs is null or empty");

            foreach (var modelElement in ((Model)_parentModel).Elements)
            {
                foreach (var elementID in ParentElementIDs)
                {
                    if (elementID == modelElement.ID)
                        _parentElements.Add(modelElement.ID, modelElement);
                }
            }

            return true;
        }

        /// <summary>
        /// The method updates the content of the node, binds the elements through a specific model,
        /// which is passed by reference. This method is called when constructing a specific model 
        /// through the reflection mechanism. Be careful and call the method only in the context of 
        /// a specific model constructor.
        /// </summary>
        /// <param name="parentModel">Reference to specific parent model</param>
        /// <returns>Returns true if the result is successful, otherwise - false</returns>
        protected bool Update(ref object parentModel)
        {
            _parentModel = parentModel;

            return TryLinkElementsToNodes();
        }
    }

    /// <summary>
    /// Class of the node creator
    /// </summary>
    class NodeCreator
    {
        /// <summary>
        /// Node parameters
        /// </summary>
        NodeParams nodeParameters;

        /// <summary>
        /// Protected base constructor for implementing abstract parameter passing when creating a specific node class.
        /// </summary>
        /// <param name="parameters">Node parameters</param>
        protected NodeCreator(NodeParams parameters) 
        {
            nodeParameters = parameters;
        }

        /// <summary>
        /// A virtual function for building nodes. Must be implemented in each specific class and returns the constructed node
        /// The same factory method
        /// </summary>
        /// <returns>Some constructed node</returns>
        public Node BuildNode()
        {
            return new Node(nodeParameters);
        }

        /// <summary>
        /// Returns the specific creator of the node using the data contained in the passed parameters
        /// </summary>
        /// <param name="parameters">Node parameters</param>
        /// <returns>A specific instance of the node creator</returns>
        public static NodeCreator GetNodeCreatorByParams(NodeParams parameters)
        {
            NodeCreator creator = null;
            var type = parameters.TypeName;

            // TODO: Hide the implementation of selecting a specific creator, so that the registration of a specific type occurs somewhere in a separate place
            if (type == null)
            {
                creator = new NodeCreator(parameters);
            }
            else if (type != null)
            {
                // Do nothing
            }

            // TODO: Insert new type of nodes

            return creator;
        }
    }

    /// <summary>
    /// The basic structure of the generalized parameters of the node
    /// </summary>
    struct NodeParams
    {
        /// <summary>
        /// Node identifier
        /// </summary>
        public int ID;

        /// <summary>
        /// ID of the analysis coordinate system
        /// </summary>
        public int AnalysisCSysID;

        /// <summary>
        /// ID of the reference coordinate system
        /// </summary>
        public int RefCSysID;

        /// <summary>
        /// Coordinates of the node in the global coordinate system
        /// </summary>
        public float[] Coords;

        /// <summary>
        /// Name of the node type
        /// </summary>
        public string TypeName;

        /// <summary>
        /// Array of parent element IDs
        /// </summary>
        public int[] ParentElementIDs;
    }

    /// <summary>
    /// Enumeration of all available types of nodes
    /// </summary>
    enum NodeType
    {
        General,
    }
}
