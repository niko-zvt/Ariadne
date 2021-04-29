using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ariadne.Kernel
{
    /// <summary>
    /// The class implements the main entity of the model for data manipulation
    /// </summary>
    class Model
    {
        /// <summary>
        /// Materials contained in the model
        /// </summary>
        public MaterialSet Materials { get; private set; }

        /// <summary>
        /// Properties contained in the model
        /// </summary>
        public PropertySet Properties { get; private set; }

        /// <summary>
        /// Nodes contained in the model
        /// </summary>
        public NodeSet Nodes { get; private set; }

        /// <summary>
        /// Elements contained in the model
        /// </summary>
        public ElementSet Elements { get; private set; }

        /// <summary>
        /// Results contained in the model
        /// </summary>
        public ResultSet Results { get; private set; }

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="materials">Set of materials</param>
        /// <param name="properties">Set of properties</param>
        /// <param name="nodes">Set of nodes</param>
        /// <param name="elements">Set of elements</param>
        /// <param name="results">Set of results</param>
        private Model(MaterialSet materials, PropertySet properties, NodeSet nodes, ElementSet elements, ResultSet results)
        {
            // Initializing
            Materials = materials;
            Properties = properties;
            Nodes = nodes;
            Elements = elements;
            Results = results;

            // Updating
            UpdateElements();
            UpdateNodes();
        }

        /// <summary>
        /// Method for creating a specific model according to data sets
        /// </summary>
        /// <param name="materials">Set of materials</param>
        /// <param name="properties">Set of properties</param>
        /// <param name="nodes">Set of nodes</param>
        /// <param name="elements">Set of elements</param>
        /// <param name="results">Set of results</param>
        /// <returns>Specific model</returns>
        public static Model CreateBySets(MaterialSet materials, PropertySet properties, NodeSet nodes, ElementSet elements, ResultSet results)
        {
            // Return new model
            return new Model(materials, properties, nodes, elements, results);
        }

        /// <summary>
        /// Method for creating a specific model according to database
        /// </summary>
        /// <param name="database">Database</param>
        /// <returns>Specific model</returns>
        public static Model CreateByDatabase(DB database)
        {
            // Check database
            if (database != null && !database.IsValid())
                return null;

            // Get all materials
            var materialIDs = database.GetAllMaterialIDs();
            var materials = database.BuildMaterials(materialIDs);

            // Get all properties
            var propertyIDs = database.GetAllPropertyIDs();
            var properties = database.BuildProperties(propertyIDs);

            // Get all nodes
            var nodeIDs = database.GetAllNodeIDs();
            var nodes = database.BuildNodes(nodeIDs);

            // Get all elements
            var elementIDs = database.GetAllElementIDs();
            var elements = database.BuildElements(elementIDs);

            // Get all results
            var results = database.BuildAllResults();

            // Return new model
            return Model.CreateBySets(materials, properties, nodes, elements, results);
        }

        /// <summary>
        /// The method updates all the elements contained in the model
        /// </summary>
        /// <returns>Returns true if the result is successful, otherwise - false</returns>
        private bool UpdateElements()
        {
            if (Elements == null)
                return false;

            var results = new List<bool>();

            foreach (var element in Elements)
            {
                if (element == null)
                    continue;

                // The update occurs through the reflection mechanism.
                // Since each element inherits a predefined protected method.
                Type elementType = element.GetType();
                MethodInfo callUpdateMethod = elementType.GetMethod("Update", BindingFlags.Instance | BindingFlags.NonPublic);
                if (callUpdateMethod != null)
                {
                    var result = (bool)callUpdateMethod.Invoke(element, new object[] { this });
                    results.Add(result);
                }
            }

            if (results.Count <= 0 || results.Find(x => x == false))
                return false;

            return true;
        }

        /// <summary>
        /// The method updates all the nodes contained in the model
        /// </summary>
        /// <returns>Returns true if the result is successful, otherwise - false</returns>
        private bool UpdateNodes()
        {
            if (Nodes == null)
                return false;

            var results = new List<bool>();

            foreach (var node in Nodes)
            {
                if (node == null)
                    continue;

                // The update occurs through the reflection mechanism.
                // Since each node inherits a predefined protected method.
                Type nodeType = node.GetType();
                MethodInfo callUpdateMethod = nodeType.GetMethod("Update", BindingFlags.Instance | BindingFlags.NonPublic);
                if (callUpdateMethod != null)
                {
                    var result = (bool)callUpdateMethod.Invoke(node, new object[] { this });
                    results.Add(result);
                }
            }

            if (results.Count <= 0 || results.Find(x => x == false))
                return false;

            return true;
        }
    }
}
