using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using Ariadne.Kernel.Libs;

namespace Ariadne.Kernel.Math
{
    /// <summary>
    /// Abstract class for hiding the implementation of the BoundingBox class
    /// </summary>
    abstract class BoundingBox
    {
        /// <summary>
        /// Virtual method for checking whether a point belongs to a bounding box
        /// </summary>
        /// <returns>true if point belong to bounding box; otherwise return - false</returns>
        public abstract bool IsPointBelong(Vector3D point);

        /// <summary>
        /// Virtual method to return the type of the bounding box
        /// </summary>
        /// <returns>Type of plane</returns>
        public abstract BoundingBoxType GetBoundingBoxType();
    }

    /// <summary>
    /// Enumeration of all available types of bounding boxes
    /// </summary>
    enum BoundingBoxType
    {
        AABB, // Axis-aligned bounding box
        OOBB, // Optimal oriented bounding box
    }

    /// <summary>
    /// Class of an axis-aligned bounding box in three-dimensional space
    /// </summary>
    sealed class AABoundingBox : BoundingBox
    {
        private Vector3D _minPoint;
        private Vector3D _maxPoint;

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="coordinateSystem">Coordinate system</param>
        /// <param name="minPoint">Min point</param>
        /// <param name="maxPoint">Max point</param>
        private AABoundingBox(Vector3D minPoint, Vector3D maxPoint)
        {
            _minPoint = minPoint;
            _maxPoint = maxPoint;
        }

        /// <summary>
        /// Create AABB by set of nodes
        /// </summary>
        /// <param name="nodes">Set of nodes</param>
        /// <returns>Axis-aligned bounding box</returns>
        public static AABoundingBox CreateByNodeSet(ref NodeSet nodes)
        {
            List<Vector3D> points = new List<Vector3D>();
            foreach (Node node in nodes)
            {
                points.Add(node.Coords);
            }
            return CreateByPoints(points);
        }

        /// <summary>
        /// Create AABB by two points
        /// </summary>
        /// <param name="minPoint">Min point</param>
        /// <param name="maxPoint">Max point</param>
        /// <returns>Axis-aligned bounding box</returns>
        public static AABoundingBox CreateByPoints(Vector3D minPoint, Vector3D maxPoint)
        {
            return new AABoundingBox(minPoint, maxPoint);
        }

        /// <summary>
        /// Create AABB by list of points
        /// </summary>
        /// <param name="points">List of points</param>
        /// <returns>Axis-aligned bounding box</returns>
        /// <exception cref="System.Exception">CGAL lib is fail.</exception>
        public static AABoundingBox CreateByPoints(List<Vector3D> points)
        {
            // 1. Find AABB in C++ lib
            var cgal = LibraryImport.SelectCGAL();
            CGAL.CGAL_Point[] cgalPoints = new CGAL.CGAL_Point[points.Count];

            int index = 0;
            foreach (var point in points)
            {
                cgalPoints[index] = new CGAL.CGAL_Point(point.X, point.Y, point.Z);
                index++;
            }

            var jsonBox = string.Empty;
            var result = cgal.GetAABB(cgalPoints, points.Count, str => { jsonBox = str; });

            if (string.IsNullOrEmpty(jsonBox))
                throw new System.Exception("CGAL lib is fail!");

            // 2. Deserialize JSON box from C++ lib
            return GetBoundingBoxByJSON(jsonBox);
        }

        /// <summary>
        /// The method gets the bounding box after deserializing it from the JSON representation
        /// </summary>
        /// <param name="jsonString">JSON representation for bounding box (From CGAL lib)</param>
        /// <returns>Optimal oriented bounding box</returns>
        private static AABoundingBox GetBoundingBoxByJSON(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
                throw new System.ArgumentNullException("OOBB GetBoundingBoxByJSON - JsonAttribute string is string.Empty or null");

            const int countOOBBPoints = 2;
            string[] jsonBoxes = jsonString.Split('\n');

            List<Vector3D> points = new List<Vector3D>();
            foreach(var jsonBox in jsonBoxes)
            {
                if(!string.IsNullOrEmpty(jsonBox))
                    points.Add(JsonSerializer.Deserialize<Vector3D>(jsonBox));
            }

            if (points.Count != countOOBBPoints)
                throw new System.ArgumentOutOfRangeException("OOBB GetBoundingBoxByJSON - JSON contains less/greater than 2 points!");

            return new AABoundingBox(points[0], points[1]);
        }

        /// <summary>
        /// Method for checking whether a point belongs to a bounding box
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns>true if point belong to bounding box; otherwise return - false</returns>
        public override bool IsPointBelong(Vector3D point)
        {
            if ((point.X >= _minPoint.X && point.Y >= _minPoint.Y && point.Z >= _minPoint.Z) &&
                (point.X <= _maxPoint.X && point.Y <= _maxPoint.Y && point.Z <= _maxPoint.Z))
                return true;
            else
                return false;
        }

        /// <summary>
        /// The method checks the bounding box for singularity
        /// </summary>
        /// <returns>true if the current bounding box is singular; otherwise, false.</returns>
        public bool IsSingular()
        {
            return _minPoint.Equals(_maxPoint);
        }

        /// <summary>
        /// Returns the bounding box type
        /// </summary>
        /// <returns>Bounding box type</returns>
        public override BoundingBoxType GetBoundingBoxType()
        {
            return BoundingBoxType.AABB;
        }
    }

    /// <summary>
    /// Class of a optimal oriented bounding box in three-dimensional space
    /// </summary>
    sealed class OOBoundingBox : BoundingBox
    {
        private AABoundingBox _box;
        private CoordinateSystem _coordinateSystem;

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="coordinateSystem">Coordinate system of bounding box</param>
        /// <param name="box">Axis-aligned bounding box</param>
        private OOBoundingBox(CoordinateSystem coordinateSystem, AABoundingBox box)
        {
            _box = box;
            _coordinateSystem = coordinateSystem;
        }

        /// <summary>
        /// Create OOBB by set of nodess
        /// </summary>
        /// <param name="nodes">Set of nodes</param>
        /// <returns>Optimal oriented bounding box</returns>
        public static OOBoundingBox CreateByNodeSet(ref NodeSet nodes)
        {
            List<Vector3D> points = new List<Vector3D>();
            foreach(Node node in nodes)
            {
                points.Add(node.Coords);
            }
            return CreateByPoints(points);
        }

        /// <summary>
        /// Create OOBB by list of points
        /// </summary>
        /// <param name="points">List of points</param>
        /// <returns>Optimal oriented bounding box</returns>
        /// <exception cref="System.Exception">CGAL lib is fail.</exception>
        public static OOBoundingBox CreateByPoints(List<Vector3D> points)
        {
            // 1. Find OOBB in C++ lib
            var cgal = LibraryImport.SelectCGAL();
            CGAL.CGAL_Point[] cgalPoints = new CGAL.CGAL_Point[points.Count];

            int index = 0;
            foreach(var point in points)
            {
                cgalPoints[index] = new CGAL.CGAL_Point(point.X, point.Y, point.Z);
                index++;
            }

            var jsonBox = string.Empty;
            var result = cgal.GetOOBB(cgalPoints, points.Count, str => { jsonBox = str; } );

            if (string.IsNullOrEmpty(jsonBox))
                throw new System.Exception("CGAL lib is fail!");

            // 2. Deserialize JSON box from C++ lib
            return GetBoundingBoxByJSON(jsonBox);
        }

        /// <summary>
        /// The method gets the bounding box after deserializing it from the JSON representation
        /// </summary>
        /// <param name="jsonString">JSON representation for bounding box (From CGAL lib)</param>
        /// <returns>Optimal oriented bounding box</returns>
        private static OOBoundingBox GetBoundingBoxByJSON(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
                throw new System.ArgumentNullException("AABB GetBoundingBoxByJSON - JsonAttribute string is string.Empty or null");

            // 1. Find all points
            const int countAABBPoints = 8;
            string[] jsonBoxes = jsonString.Split('\n');

            List<Vector3D> points = new List<Vector3D>();
            foreach (var jsonBox in jsonBoxes)
            {
                if (!string.IsNullOrEmpty(jsonBox))
                    points.Add(JsonSerializer.Deserialize<Vector3D>(jsonBox));
            }

            if (points.Count != countAABBPoints)
                throw new System.ArgumentOutOfRangeException("AABB GetBoundingBoxByJSON - JSON contains less/greater than 8 points!");

            // TODO: Implementation
            throw new System.NotImplementedException();

            // 1. Find CSYS
            //CoordinateSystem cs = GlobalCSys.Instance;

            // 3. Find AABB
            //var minPoint = new Vector3D();
            //var maxPoint = new Vector3D();
            //AABoundingBox aabb = AABoundingBox.CreateByPoints(minPoint, maxPoint);

            //return new OOBoundingBox(cs, aabb);
        }

        /// <summary>
        /// Method for checking whether a point belongs to a bounding box
        /// </summary>
        /// <param name="point">Global point</param>
        /// <returns>true if point belong to bounding box; otherwise return - false</returns>
        public override bool IsPointBelong(Vector3D point)
        {
            // TODO: Global to local point
            throw new System.NotImplementedException();
            //point -> localPoint
            //return _box.IsPointBelong(localPoint);
        }

        /// <summary>
        /// Returns the bounding box type
        /// </summary>
        /// <returns>Bounding box type</returns>
        public override BoundingBoxType GetBoundingBoxType()
        {
            return BoundingBoxType.OOBB;
        }
    }

}