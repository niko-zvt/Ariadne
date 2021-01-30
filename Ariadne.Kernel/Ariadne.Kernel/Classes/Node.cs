namespace Ariadne.Kernel
{
    /// <summary>
    /// Class of the node
    /// </summary>
    class Node
    {
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
        // TODO: Change data type
        public float[] Coords { get; protected set; } 

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
            Coords = parameters.Coords;
            //Type = parameters.TypeName;
        }

        /// <summary>
        /// Method to return the type of the node
        /// </summary>
        /// <returns>Type of node</returns>
        public NodeType GetNodeType() 
        { 
            return NodeType.General; 
        }
    }

    /// <summary>
    /// Class of the node creator
    /// </summary>
    class NodeCreator
    {
        /// <value>Node parameters</value>
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
        /// <value>Node identifier</value>
        public int ID;

        /// <value>ID of the analysis coordinate system</value>
        public int AnalysisCSysID;

        /// <value>ID of the reference coordinate system</value>
        public int RefCSysID;

        /// <value>Coordinates of the node in the global coordinate system</value>
        public float[] Coords;

        /// <value>Name of the node type</value>
        public string TypeName;
    }

    /// <summary>
    /// Enumeration of all available types of nodes
    /// </summary>
    enum NodeType
    {
        General,
    }
}
