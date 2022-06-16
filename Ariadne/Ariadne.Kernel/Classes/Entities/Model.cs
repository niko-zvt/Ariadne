﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Ariadne.Kernel.Math;

namespace Ariadne.Kernel
{
    /// <summary>
    /// The class implements the main entity of the model for data manipulation
    /// </summary>
    public class Model
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

        /// <summary>
        /// The method returns the stresses at the node in the form of a 3x3 matrix
        /// </summary>
        /// <param name="nodeID">Node ID</param>
        /// <param name="stress">Stress matrix</param>
        /// <param name="location">Point location</param>
        /// <returns>Returns true if the result is successful, otherwise - false</returns>
        public bool GetStressInNode(int nodeID, out Matrix3x3 stress, out Vector3D location)
        {
            stress = new Matrix3x3();
            location = Nodes[nodeID].Coords;

            if (Results == null || Results.Count <= 0)
                return false;

            var result = Results[9];
            if (result == null || !(result is ExternalResult))
                return false;

            var data = ((ExternalResult)result).GetData();
            if (data == null || !(data is object[,]))
                return false;

            var array = (object[,])data;

            int length = array.GetLength(0);

            for (int i = 0; i < length; i++)
            {
                /*
                 * In the array of results of the FeResPost library,
                 * the node ID is listed under the index [i, 1].
                 * If it is an element, then the object is NULL
                 */
                var nID = array[i, 1];
                if (nID is int @int && (!(nID is int) || @int == nodeID))
                {
                    /* 
                    * In the array of results of the FeResPost library,
                    * the stress components is listed under the indexes [i, 5] - [i, 10].
                    * If it is a number, then the object is FLOAT
                    */
                    var Sxx = (array[i, 5] is float || array[i, 5] is double) ? (float)array[i, 5] : 0.0;
                    var Syy = (array[i, 6] is float || array[i, 6] is double) ? (float)array[i, 6] : 0.0;
                    var Szz = (array[i, 7] is float || array[i, 7] is double) ? (float)array[i, 7] : 0.0;

                    var Sxy = (array[i, 8] is float || array[i, 8] is double) ? (float)array[i, 8] : 0.0;
                    var Syz = (array[i, 9] is float || array[i, 9] is double) ? (float)array[i, 9] : 0.0;
                    var Szx = (array[i, 10] is float || array[i, 10] is double) ? (float)array[i, 10] : 0.0;

                    stress = new Matrix3x3(Sxx, Syy, Szz, Sxy, Syz, Szx);

                    return true;
                }

                continue;
            }

            return false;
        }

        /// <summary>
        /// The method returns the stresses at the element in the form of a 3x3 matrix
        /// </summary>
        /// <param name="elementID">Element ID</param>
        /// <param name="stress">Stress matrix</param>
        /// <param name="location">Point location</param>
        /// <returns>Returns true if the result is successful, otherwise - false</returns>
        public bool GetStressInElement(int elementID, out Matrix3x3 stress, out Vector3D location)
        {
            stress = new Matrix3x3();
            location = Elements[elementID].Coords;

            if (Results == null || Results.Count <= 0)
                return false;

            var result = Results[9];
            if (result == null || !(result is ExternalResult))
                return false;

            var data = ((ExternalResult)result).GetData();
            if (data == null || !(data is object[,]))
                return false;

            var array = (object[,])data;

            int length = array.GetLength(0);

            for (int i = 0; i < length; i++)
            {
                /*
                 * In the array of results of the FeResPost library,
                 * the element ID is listed under the index [i, 0] and
                 * the node ID is listed under the index [i, 1].
                 */
                var eID = array[i, 0];
                var nID = array[i, 1];

                if (eID is int @int &&
                    (!(eID is int) || @int == elementID))
                {
                    if (nID is int)
                        continue;

                    /* 
                     * In the array of results of the FeResPost library,
                     * the stress components is listed under the indexes [i, 5] - [i, 10].
                     * If it is a number, then the object is FLOAT
                     */
                    var Sxx = (array[i, 5] is float || array[i, 5] is double) ? (float)array[i, 5] : 0.0;
                    var Syy = (array[i, 6] is float || array[i, 6] is double) ? (float)array[i, 6] : 0.0;
                    var Szz = (array[i, 7] is float || array[i, 7] is double) ? (float)array[i, 7] : 0.0;

                    var Sxy = (array[i, 8] is float || array[i, 8] is double) ? (float)array[i, 8] : 0.0;
                    var Syz = (array[i, 9] is float || array[i, 9] is double) ? (float)array[i, 9] : 0.0;
                    var Szx = (array[i, 10] is float || array[i, 10] is double) ? (float)array[i, 10] : 0.0;

                    stress = new Matrix3x3(Sxx, Syy, Szz, Sxy, Syz, Szx);

                    return true;
                }

                continue;
            }

            return false;
        }

        /// <summary>
        /// The method returns the stresses at the point in the form of a 3x3 matrix
        /// </summary>
        /// <param name="location">Point location</param>
        /// <param name="stress">Stress matrix</param>
        /// <returns>Returns true if the result is successful, otherwise - false</returns>
        public bool GetStressInPoint(Vector3D location, out Matrix3x3 stress)
        {
            stress = new Matrix3x3();

            if (Results == null || Results.Count <= 0)
                return false;

            var result = Results[9];
            if (result == null || !(result is ExternalResult))
                return false;

            var data = ((ExternalResult)result).GetData();
            if (data == null || !(data is object[,]))
                return false;

            var IsElementIDFound = GetElementIDFromPoint(location, out int eID);
            if (IsElementIDFound == false)
                return false;

            var IsStressFound = GetStressInElementByIDAndPoint(eID, location, out stress);
            if (IsStressFound == false)
                return false;

            return true;
        }

        /// <summary>
        /// The method returns ID element by the point location
        /// </summary>
        /// <param name="location">Point location</param>
        /// <param name="elementID">Element ID</param>
        /// <returns>Returns true if the result is successful, otherwise - false</returns>
        public bool GetElementIDFromPoint(Vector3D location, out int elementID)
        {
            elementID = -1;

            // Create list of distances
            List<(int ID, float Distance)> listOfDistances = new List<(int, float)> ();
            foreach (var node in Nodes)
            {
                var distance = (new Vector3D(location, node.Coords)).Length;
                listOfDistances.Add((node.ID, distance));
            }
            listOfDistances.Sort((s1, s2) => s1.Distance.CompareTo(s2.Distance));

            // We go through the list of distances and check elements
            foreach(var tuple in listOfDistances)
            {
                var elementIDs = Nodes.GetByID(tuple.ID).ParentElementIDs;
                foreach (var parentElementID in elementIDs)
                {
                    var res = CheckPointBelongElement(parentElementID, location);
                    if (res == true)
                    {
                        elementID = parentElementID;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// The method returns the stress matrix by ID element and point location.
        /// The point MUST BELONG to the element. Use parameter "forceCheck = true" if you are unsure if the point actually belongs to the element.
        /// </summary>
        /// <param name="elementID">Element ID</param>
        /// <param name="location">Point location</param>
        /// <param name="stress">Stress matrix</param>
        /// <param name="forceCheck">If the parameter is true, then an explicit check is made that the point belongs to the element.</param>
        /// <returns>Returns true if the result is successful, otherwise - false</returns>
        private bool GetStressInElementByIDAndPoint(int elementID, Vector3D location, out Matrix3x3 stress, bool forceCheck = false)
        {
            stress = null;

            if (forceCheck == true && CheckPointBelongElement(elementID, location) == false)
                return false;

            var element = Elements.GetByID(elementID);
            if (element == null)
                return false;

            var elementNodes = new NodeSet();
            foreach(var nodeID in element.NodeIDs)
            {
                elementNodes.Add(nodeID, Nodes.GetByID((int)nodeID));
            }
            if (elementNodes.Count <= 0)
                return false;

            var nodesData = new List<(int NodeID, Vector3D NodeLocation, Matrix3x3 NodeStress)>();
            foreach (var node in elementNodes)
            {
                var nodeResult = GetStressInNode(node.ID, out var nodeStress, out var nodeLocation);
                nodesData.Add(new (node.ID, nodeLocation, nodeStress));
            }

            if(nodesData.Count != elementNodes.Count)
                return false;

            // TODO: Calculate the stress through the shape function
            // 1. Calculate UV-coords of point
            // 2. Calculate the stress through the shape function

            return true;
        }

        /// <summary>
        /// The method tries to check whether the point belongs to the specified element
        /// </summary>
        /// <param name="elementID">Element ID</param>
        /// <param name="location">Point location</param>
        /// <returns>Returns true if the result is successful, otherwise - false</returns>
        private bool CheckPointBelongElement(int elementID, Vector3D location)
        {
            var element = Elements.GetByID(elementID);
            var result = false;

            if (element != null)
                result = element.IsPointBelong(location);

            return result;
        }
    }
}