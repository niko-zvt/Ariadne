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
        AABB,
        OOBB,
    }

    /// <summary>
    /// Class of an axis-aligned bounding box in three-dimensional space
    /// </summary>
    sealed class AABoundBox : BoundingBox
    {
        private Vector3D _minPoint;
        private Vector3D _maxPoint;

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="coordinateSystem">Coordinate system</param>
        /// <param name="minPoint">Min point</param>
        /// <param name="maxPoint">Max point</param>
        public AABoundBox(Vector3D minPoint, Vector3D maxPoint)
        {
            _minPoint = minPoint;
            _maxPoint = maxPoint;
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
        private AABoundBox _box;
        private CoordinateSystem _coordinateSystem;

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="coordinateSystem">Coordinate system of bounding box</param>
        /// <param name="box">Axis-aligned bounding box</param>
        private OOBoundingBox(CoordinateSystem coordinateSystem, AABoundBox box)
        {
            _box = box;
            _coordinateSystem = coordinateSystem;
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
        /// <param name="jsonBox">JSON representation for bounding box (From CGAL lib)</param>
        /// <returns>Optimal oriented bounding box</returns>
        private static OOBoundingBox GetBoundingBoxByJSON(string jsonBox)
        {
            // TODO: Fix Debug
            System.Console.WriteLine(jsonBox);

            // 1. Find CSYS
            CoordinateSystem cs = GlobalCSys.Instance;

            // 3. Find AABB
            var minPoint = new Vector3D();
            var maxPoint = new Vector3D();
            AABoundBox aabb = new AABoundBox(minPoint, maxPoint);

            return new OOBoundingBox(cs, aabb);
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