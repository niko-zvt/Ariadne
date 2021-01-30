namespace Ariadne.Kernel
{
    /// <summary>
    /// Abstract class of the element
    /// </summary>
    abstract class Element
    {
        /// <summary>
        /// Protected constructor with parameters
        /// </summary>
        /// <param name="parameters">Element parameters</param>
        protected Element(ElementParams parameters)
        {
            ID = parameters.ID;
            Dim = parameters.Dim;
            CornerNodeIDs = IntSet.FromArray(parameters.CornerNodes);
            NodeIDs = IntSet.FromArray(parameters.Nodes);
            PropertyID = parameters.PropertyID;
        }

        /// <summary>
        /// Element identifier
        /// </summary>
        public int ID { get; protected set; }

        /// <summary>
        /// Dimension of the element
        /// </summary>
        public int Dim { get; protected set; }

        /// <summary>
        /// Identifier of the element property
        /// </summary>
        public int PropertyID { get; protected set; }

        /// <summary>
        /// Set of nodes forming the element
        /// </summary>
        public IntSet NodeIDs { get; protected set; }

        /// <summary>
        /// Set of the element's corner nodes
        /// </summary>
        public IntSet CornerNodeIDs { get; protected set; }

        /// <summary>
        /// Virtual method to return the type of the element
        /// </summary>
        /// <returns>Type of element</returns>
        public abstract ElementType GetElementType();
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

            // TODO: Insert new type of elements

            return creator;
        }
    }

    /// <summary>
    /// The basic structure of the generalized parameters of the element
    /// </summary>
    struct ElementParams
    {
        /// <value>Element identifier</value>
        public int ID;

        /// <value>Dimension of the element</value>
        public int Dim;

        /// <value>Name of the element type</value>
        public string TypeName;

        /// <value>Identifier of the element property</value>
        public int PropertyID;

        /// <value>Array of nodes forming the element</value>
        public int[] Nodes;

        /// <value>Array of the element's corner nodes</value>
        public int[] CornerNodes;
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
