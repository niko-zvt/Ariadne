using System;
using System.Collections.Generic;
using System.IO;
using FeResPost;

namespace Ariadne.Kernel
{
    /// <summary>
    /// Class for hiding the database implementation of the FeResPost library
    /// </summary>
    public class DB
    {
        /// <summary>
        /// Enumeration specifying the correspondence between the keys
        /// of the old Result object and the keys of the new Result object.
        /// </summary>
        private enum FromToMethod
        {
            CentersToCorners,
            CentersToNodes,
            NodesToCenters,
            NodesToCorners,
            CornersToCenters,
            CornersToNodes,
            MergeLayers,
            MergeLayersKeepId,
            MergeSubLayers,
            MergeSubLayersKeepId,
            MergeAll,
            CentersToElemsAndNodes,
            CornersToElemsAndNodes,
            CentersToCornerNodes,
            ElemsAndNodesToCenters,
            ElemsAndNodesToNodes,
            NodesToElemsAndNodes
        }

        /// <summary>
        /// Enumeration used to specify the way values associated to
        /// different keys are merged into a single value (if this happens).
        /// </summary>
        private enum RemappingMethod
        {
            average,
            sum,
            min,
            max,
            NONE
        }

        /// <summary>
        /// Nastran database
        /// </summary>
        private NastranDb _externalNastranDB;

        /// <summary>
        /// Private constructor of database object
        /// </summary>
        /// <param name="pathToBDF">Path to the model file</param>
        /// <param name="pathToOP2">Path to the results OUTPUT2 file</param>
        /// <param name="pathToXDB">Path to the results database file</param>
        /// <param name="pathToSES">Path to the Patran session file</param>
        private DB(string pathToBDF, string pathToOP2 = null, string pathToXDB = null, string pathToSES = null)
        {
            // TODO: Use TRY-CATCH
            var isNull = String.IsNullOrEmpty(pathToBDF);
            var isEmpty = !File.Exists(pathToBDF);
            if (isNull || isEmpty)
                throw new ArgumentException("Parameter cannot be null or empty", nameof(pathToBDF));

            _externalNastranDB = new NastranDb
            {
                Name = Path.GetFileNameWithoutExtension(pathToBDF)
            };

            _externalNastranDB.readBdf(pathToBDF);

            if (!String.IsNullOrEmpty(pathToXDB) && File.Exists(pathToXDB))
            {
                _externalNastranDB.readXdb(pathToXDB);
            }

            if (!String.IsNullOrEmpty(pathToSES) && File.Exists(pathToSES))
            {
                _externalNastranDB.readGroupsFromPatranSession(pathToSES);
            }

            if (!String.IsNullOrEmpty(pathToOP2) && File.Exists(pathToOP2))
            {
                _externalNastranDB.readOp2(pathToOP2, "Results");
                // TODO: WTF?!
                //_externalNastranDB.generateCoordResults();
                //_externalNastranDB.generateCoordResults("Fake Coords Case", "No SubCase", "coords");
            }
        }

        /// <summary>
        /// Create a database
        /// </summary>
        /// <param name="pathToBDF">Path to the model file</param>
        /// <param name="pathToOP2">Path to the results OUTPUT2 file</param>
        /// <param name="pathToXDB">Path to the results database file</param>
        /// <param name="pathToSES">Path to the Patran session file</param>
        /// <returns>Database or null</returns>
        public static DB Create(string pathToBDF, string pathToOP2 = null, string pathToXDB = null, string pathToSES = null)
        {
            // TODO: Use TRY-CATCH
            var database = new DB(pathToBDF, pathToOP2, pathToXDB, pathToSES);
            return database.IsValid() ? database : null;
        }

        /// <summary>
        /// Build a set of specific materials by their IDs
        /// </summary>
        /// <param name="ids">Set of material IDs</param>
        /// <returns>Set of materials</returns>
        public MaterialSet BuildMaterials(IntSet ids)
        {
            var materials = new MaterialSet();

            if (!IsValid())
                return materials;

            List<MaterialCreator> creators = new List<MaterialCreator>();
            MaterialParams parameters;

            foreach (int id in ids)
            {
                // FeResPost User Manual:
                // "Note that Nastran model may contain several material cards sharing a common ID, or several MPC cards with the same ID.
                // When this case occurs, an exception is thrown.
                // Therefore, for MPC or material cards, it is advised to use “fillCards” method instead of “fillCard”.
                // An exception is also thown when no FEM item matching specified ID is found."

                // fillCard(...) -> fillCards(...)

                var cards = _externalNastranDB.fillCards("Material", id);

                if (cards != null && cards is object[] @objects)
                {
                    foreach (var card in @objects)
                    {
                        if (card != null && card is object[] @fields &&
                            fields[0] != null && fields[0] is string @string)
                        {
                            parameters.ID = id;
                            parameters.TypeName = MaterialCreator.GetMaterialTypeNameBySubtype(@string);
                            parameters.SubtypeName = @string;
                            parameters.Data = card;

                            var creator = MaterialCreator.GetMaterialCreatorByParams(parameters);
                            creators.Add(creator);
                        }
                    }
                }
            }

            foreach (var creator in creators)
            {
                var material = creator.BuildMaterial();
                materials.Add(material);
            }

            return materials;
        }

        /// <summary>
        /// Build a set of specific properties by their IDs
        /// </summary>
        /// <param name="ids">Set of property IDs</param>
        /// <returns>Set of properties</returns>
        public PropertySet BuildProperties(IntSet ids)
        {
            var properties = new PropertySet();

            if (!IsValid())
                return properties;

            List<PropertyCreator> creators = new List<PropertyCreator>();
            PropertyParams parameters;

            foreach (int id in ids)
            {
                var card = _externalNastranDB.fillCard("Property", id);

                if (card != null && card[0] is string @string)
                {
                    parameters.ID = id;
                    parameters.TypeName = @string;
                    parameters.Data = card;

                    var creator = PropertyCreator.GetPropertyCreatorByParams(parameters);
                    creators.Add(creator);
                }
            }

            foreach (var creator in creators)
            {
                var property = creator.BuildProperty();
                properties.Add(property.ID, property);
            }

            return properties;
        }

        /// <summary>
        /// Build a set of specific nodes by their IDs
        /// </summary>
        /// <param name="ids">Set of node IDs</param>
        /// <returns>Set of nodes</returns>
        public NodeSet BuildNodes(IntSet ids)
        {
            var nodes = new NodeSet();

            if (!IsValid())
                return nodes;

            List<NodeCreator> creators = new List<NodeCreator>();
            NodeParams parameters;

            foreach (int id in ids)
            {
                parameters.ID = id;
                parameters.TypeName = null; // TODO: Node type
                parameters.RefCSysID = _externalNastranDB.getNodeRcId(id);
                parameters.AnalysisCSysID = _externalNastranDB.getNodeAcId(id);
                parameters.Coords = _externalNastranDB.getNodeCoords(id);
                parameters.ParentElementIDs = GetParentElementIDsForNode(id);

                var creator = NodeCreator.GetNodeCreatorByParams(parameters);
                creators.Add(creator);
            }

            foreach (var creator in creators)
            {
                var node = creator.BuildNode();
                nodes.Add(node.ID, node);
            }

            return nodes;
        }

        /// <summary>
        /// Build a set of specific elements by their IDs
        /// </summary>
        /// <param name="ids">Set of element IDs</param>
        /// <returns>Set of elements</returns>
        public ElementSet BuildElements(IntSet ids)
        {
            var elements = new ElementSet();

            if (!IsValid())
                return elements;

            List<ElementCreator> creators = new List<ElementCreator>();
            ElementParams parameters;

            foreach (int id in ids)
            {
                parameters.ID = id;
                parameters.TypeName = _externalNastranDB.getElementTypeName(id);
                parameters.Nodes = _externalNastranDB.getElementNodes(id);
                parameters.Dim = _externalNastranDB.getElementDim(id);
                parameters.CornerNodes = _externalNastranDB.getElementCornerNodes(id);
                parameters.PropertyID = _externalNastranDB.getElementPropertyId(id);
                parameters.Coords = GetElementCoords(id);

                var creator = ElementCreator.GetElementCreatorByParams(parameters);
                creators.Add(creator);
            }
   
            foreach (var creator in creators)
            {
                var element = creator.BuildElement();
                elements.TryAdd(element.ID, element);
            }

            return elements;
        }

        /// <summary>
        /// Build a set of all specific results
        /// </summary>
        /// <returns>Set of results</returns>
        public ResultSet BuildAllResults(bool isForceRemappingResults)
        {
            var results = new ResultSet();

            if (IsValid() == false)
                return results;

            var resultCreators = new List<ResultCreator>();
            var parameters = new ResultParams();

            string[] lcNames = _externalNastranDB.getResultLoadCaseNames();
            string[] scNames = _externalNastranDB.getResultSubCaseNames();
            string[] resNames = _externalNastranDB.getResultTypeNames();

            // TODO: Remove nesting and design a normal class of results
            foreach (string lcName in lcNames)
            {
                foreach (string scName in scNames)
                {
                    foreach (string resName in resNames)
                    {
                        var resultID = ResultID.CreateByNames(lcName, scName, resName);
                        parameters.ID = resultID;
                        parameters.TypeName = "ExternalResult";
                        var result = _externalNastranDB.getResultCopy(lcName, scName, resName);

                        if (isForceRemappingResults) 
                        {
                            var fromToMethod = FromToMethod.CentersToElemsAndNodes;
                            var remappingMethod = RemappingMethod.NONE;
                            var database = _externalNastranDB as DataBase;
                            var remappingResult = DeriveResultByRemapping(fromToMethod, remappingMethod, ref result, ref database);
                            result = remappingResult;
                        }

                        parameters.Data = (result.modifyRefCoordSys(_externalNastranDB, 0)).getData(); // Result in GlobalCSys

                        ResultCreator resultCreator = ResultCreator.GetResultCreatorByParams(parameters);
                        resultCreators.Add(resultCreator);
                    }
                }
            }

            int idx = 0;
            foreach (var creator in resultCreators)
            {
                var result = creator.BuildResult();
                results.Add(idx, result);
                idx++;
            }

            return results;
        }

        /// <summary>
        /// Returns a full set of materials
        /// </summary>
        /// <returns>Full set of materials or empty set</returns>
        public IntSet GetAllMaterialIDs()
        {
            var mIDs = new IntSet();

            if (this.IsValid())
            {
                var iters = _externalNastranDB.iter_materialId();
                foreach (var iter in iters)
                {
                    if (iter is int @int)
                    {
                        var mID = @int;
                        mIDs.Add(mID);
                    }
                }
            }

            return mIDs;
        }

        /// <summary>
        /// Returns the number of materials in the database
        /// </summary>
        /// <returns>Number of materials</returns>
        public int GetNbrMaterials()
        {
            var nbrMaterials = 0;

            if (IsValid())
            {
                nbrMaterials = GetAllMaterialIDs().Count;
            }

            return nbrMaterials;
        }

        /// <summary>
        /// Returns a full set of properties
        /// </summary>
        /// <returns>Full set of properties or empty set</returns>
        public IntSet GetAllPropertyIDs()
        {
            var pIDs = new IntSet();

            if(this.IsValid())
            {
                var iters = _externalNastranDB.iter_propertyId();
                foreach(var iter in iters)
                {
                    if(iter is int @int)
                    {
                        var pID = @int;
                        pIDs.Add(pID);
                    }
                }
            }

            return pIDs;
        }

        /// <summary>
        /// Returns the number of properties in the database
        /// </summary>
        /// <returns>Number of properties</returns>
        public int GetNbrProperties()
        {
            var nbrProperties = 0;

            if (IsValid())
            {
                nbrProperties = GetAllPropertyIDs().Count;
            }

            return nbrProperties;
        }

        /// <summary>
        /// Returns a full set of nodes
        /// </summary>
        /// <returns>Full set of nodes or empty set</returns>
        public IntSet GetAllNodeIDs()
        {
            var nIDs = new IntSet();

            if (this.IsValid())
            {
                var iters = _externalNastranDB.iter_nodeId();
                foreach (var iter in iters)
                {
                    if (iter is int @int)
                    {
                        var eID = @int;
                        nIDs.Add(eID);
                    }
                }
            }

            return nIDs;
        }

        /// <summary>
        /// Returns the number of nodes in the database
        /// </summary>
        /// <returns>Number of nodes</returns>
        public int GetNbrNodes()
        {
            var nbrNodes = 0;

            if (IsValid())
            {
                nbrNodes = _externalNastranDB.getNbrNodes();
            }

            return nbrNodes;
        }

        /// <summary>
        /// Returns a full set of elements
        /// </summary>
        /// <returns>Full set of elements or empty set</returns>
        public IntSet GetAllElementIDs()
        {
            var eIDs = new IntSet();
            
            if(this.IsValid())
            {
                var iters = _externalNastranDB.iter_elemId();
                foreach (var iter in iters)
                {
                    if(iter is int @int)
                    {
                        var eID = @int;
                        eIDs.Add(eID);
                    }
                }
            }

            return eIDs;
        }

        /// <summary>
        /// Returns the number of elements in the database
        /// </summary>
        /// <returns>Number of elements</returns>
        public int GetNbrElements()
        {
            var nbrElements = 0;

            if(IsValid())
            {
                nbrElements = _externalNastranDB.getNbrElements();
            }

            return nbrElements;
        }

        /// <summary>
        /// Get the coordinates center of the element by its ID
        /// </summary>
        /// <param name="eID">Element ID</param>
        /// <returns>The coordinates center of the element</returns>
        private float[] GetElementCoords(int eID)
        {
            var coords = new float[3] { 0.0f, 0.0f, 0.0f };

            var typeName = _externalNastranDB.getElementTypeName(eID);
            var cornerNodeIDs = _externalNastranDB.getElementCornerNodes(eID);
            var cornerNodes = BuildNodes(IntSet.FromArray(cornerNodeIDs));

            var cornerNodesCount = cornerNodes.Count;

            // TODO: Think about implementation
            if (typeName == "CQUAD4")
            {
                if (cornerNodesCount != 4)
                    throw new ArgumentException("The parameter is different from 4 nodes", nameof(cornerNodes.Count));
            }
            else if (typeName == "CTRIA3")
            {
                if (cornerNodes.Count != 3)
                    throw new ArgumentException("The parameter is different from 3 nodes", nameof(cornerNodes.Count));
            }
            else
            {
                // DO NOTHING
            }

            foreach (var node in cornerNodes)
            {
                if (node != null)
                {
                    var nodeCoords = node.Coords;
                    coords[0] += nodeCoords.X;
                    coords[1] += nodeCoords.Y;
                    coords[2] += nodeCoords.Z;
                }
            }

            coords[0] = (float)(coords[0] / cornerNodesCount);
            coords[1] = (float)(coords[1] / cornerNodesCount);
            coords[2] = (float)(coords[2] / cornerNodesCount);

            return coords;
        }

        /// <summary>
        /// Get node's parent element IDs
        /// </summary>
        /// <param name="nID">Node ID</param>
        /// <returns>Parent element IDs for node</returns>
        private int[] GetParentElementIDsForNode(int nID)
        {
            var elementIDs = GetAllElementIDs();

            if (elementIDs == null || elementIDs.Count <= 0)
                throw new NullReferenceException("Element IDs is null or empty");

            var parentElementIDs = new IntSet();

            foreach (var elementID in elementIDs)
            {
                var nodeIDs = _externalNastranDB.getElementNodes(elementID);
                var cornerNodeIDs = _externalNastranDB.getElementCornerNodes(elementID);

                foreach (var nodeID in nodeIDs)
                {
                    if (nodeID == nID)
                        parentElementIDs.Add(elementID);
                }

                foreach (var cornerNodeID in cornerNodeIDs)
                {
                    if (cornerNodeID == nID)
                        parentElementIDs.Add(elementID);
                }
            }

            return parentElementIDs.ToArray();
        }

        /// <summary>
        /// Method returns a Result object obtained by remapping the values of the Result object to which the method is applied.
        /// </summary>
        /// <param name="resultToRemapping">Result object to remapping.</param>
        /// <param name="fromToMethod">From-To method. See enum.</param>
        /// <param name="remappingMethod">Remapping method. See enum.</param>
        /// <param name="dataBase">DataBase object used by the method to recover the association of node and elements.
        /// This association is often needed to perform the remapping</param>
        /// <returns></returns>
        private static FeResPost.Result DeriveResultByRemapping(FromToMethod fromToMethod, RemappingMethod remappingMethod, ref FeResPost.Result resultToRemapping, ref DataBase dataBase)
        {
            var strFromToMethod = fromToMethod.ToString();
            var strRemappingMethod = remappingMethod.ToString();
            var result = resultToRemapping.deriveByRemapping(strFromToMethod, strRemappingMethod, dataBase);
            return result;
        }

        /// <summary>
        /// Checking that the database is valid
        /// </summary>
        /// <returns>
        /// - true in the case, when the database is valid
        /// </returns>
        public bool IsValid()
        {
            return _externalNastranDB != null;
        }

        /// <summary>
        /// Temporary database return function
        /// </summary>
        /// <returns>Nastran database or null</returns>
        // TODO: Delete this method
        public NastranDb TEMP_GetDataBase()
        {
            // TODO: Remove this feature
            return IsValid() ? _externalNastranDB : null;
        }
    }
}
