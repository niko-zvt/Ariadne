using System;
using System.Collections.Generic;
using System.IO;
using FeResPost;

namespace Ariadne.Kernel
{
    /// <summary>
    /// Class for hiding the database implementation of the FeResPost library
    /// </summary>
    class DB
    {
        /// <value>Nastran database</value>
        private NastranDb db { get; set; }

        /// <summary>
        /// Private constructor of database object
        /// </summary>
        /// <param name="pathToBDF">Path to the model file</param>
        /// <param name="pathToOP2">Path to the results OUTPUT2 file</param>
        /// <param name="pathToXDB">Path to the results database file</param>
        /// <param name="pathToSES">Path to the Patran session file</param>
        private DB(string pathToBDF, string pathToOP2 = null, string pathToXDB = null, string pathToSES = null)
        {
            /// TODO: Use TRY-CATCH
            if (String.IsNullOrEmpty(pathToBDF) || !File.Exists(pathToBDF))
                throw new ArgumentException("Parameter cannot be null or empty", nameof(pathToBDF));

            db = new NastranDb();
            db.Name = Path.GetFileNameWithoutExtension(pathToBDF);
            db.readBdf(pathToBDF);

            if (!String.IsNullOrEmpty(pathToXDB) && File.Exists(pathToXDB))
            {
                db.readXdb(pathToXDB);
            }

            if (!String.IsNullOrEmpty(pathToSES) && File.Exists(pathToSES))
            {
                db.readGroupsFromPatranSession(pathToSES);
            }

            if (!String.IsNullOrEmpty(pathToOP2) && File.Exists(pathToOP2))
            {
                db.readOp2(pathToOP2, "Results");
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
            /// TODO: Use TRY-CATCH
            DB database = new DB(pathToBDF, pathToOP2, pathToXDB, pathToSES);
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
                var card = db.fillCard("Material", id);

                if (card != null && card[0] is string)
                {
                    parameters.ID = id;
                    parameters.TypeName = MaterialCreator.GetMaterialTypeNameBySubtype((string)card[0]);
                    parameters.SubtypeName = (string)card[0];
                    parameters.Data = card;

                    var creator = MaterialCreator.GetMaterialCreatorByParams(parameters);
                    creators.Add(creator);
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
                var card = db.fillCard("Property", id);

                if (card != null && card[0] is string)
                {
                    parameters.ID = id;
                    parameters.TypeName = (string)card[0];
                    parameters.Data = card;

                    var creator = PropertyCreator.GetPropertyCreatorByParams(parameters);
                    creators.Add(creator);
                }
            }

            foreach (var creator in creators)
            {
                var property = creator.BuildProperty();
                properties.Add(property);
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
                parameters.RefCSysID = db.getNodeRcId(id);
                parameters.AnalysisCSysID = db.getNodeAcId(id);
                parameters.Coords = db.getNodeCoords(id);
                
                var creator = NodeCreator.GetNodeCreatorByParams(parameters);
                creators.Add(creator);
            }

            foreach (var creator in creators)
            {
                var node = creator.BuildNode();
                nodes.Add(node);
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
                parameters.TypeName = db.getElementTypeName(id);
                parameters.Nodes = db.getElementNodes(id);
                parameters.Dim = db.getElementDim(id);
                parameters.CornerNodes = db.getElementCornerNodes(id);
                parameters.PropertyID = db.getElementPropertyId(id);

                var creator = ElementCreator.GetElementCreatorByParams(parameters);
                creators.Add(creator);
            }
   
            foreach (var creator in creators)
            {
                var element = creator.BuildElement();
                elements.Add(element);
            }

            return elements;
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
                var iters = db.iter_materialId();
                foreach (var iter in iters)
                {
                    if (iter is int)
                    {
                        var mID = (int)iter;
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
                var iters = db.iter_propertyId();
                foreach(var iter in iters)
                {
                    if(iter is int)
                    {
                        var pID = (int)iter;
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
                var iters = db.iter_nodeId();
                foreach (var iter in iters)
                {
                    if (iter is int)
                    {
                        var eID = (int)iter;
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
                nbrNodes = db.getNbrNodes();
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
                var iters = db.iter_elemId();
                foreach (var iter in iters)
                {
                    if(iter is int)
                    {
                        var eID = (int)iter;
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
                nbrElements = db.getNbrElements();
            }

            return nbrElements;
        }

        /// <summary>
        /// Checking that the database is valid
        /// </summary>
        /// <returns>
        /// - true in the case, when the database is valid
        /// </returns>
        public bool IsValid()
        {
            return db == null ? false : true;
        }

        /// <summary>
        /// Temporary database return function
        /// </summary>
        /// <returns>Nastran database or null</returns>
        public NastranDb TEMP_GetDataBase()
        {
            // TODO: Remove this feature
            return IsValid() ? db : null;
        }
    }

}
