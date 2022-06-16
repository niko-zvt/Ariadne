using System.Text.Json;
using System.Collections.Generic;
using Ariadne.Kernel.Libs;

namespace Ariadne.Kernel.Math
{
    /// <summary>
    /// Abstract class for hiding the implementation of the BoundingBox class
    /// </summary>
    public abstract class BoundingBox
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
    public enum BoundingBoxType
    {
        AABB, // Axis-aligned bounding box
        OOBB, // Optimal oriented bounding box
    }

    /// <summary>
    /// Class of an axis-aligned bounding box in three-dimensional space
    /// </summary>
    public sealed class AABoundingBox : BoundingBox
    {
        /// <summary>
        /// Minimum defining point AABB.
        /// </summary>
        private Vector3D _minPoint;

        /// <summary>
        /// Maximum defining point AABB.
        /// </summary>
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
        public static AABoundingBox CreateByNodeSet(NodeSet nodes)
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
            if (LibraryImport.SelectCGAL().CGAL_GetAABB(points, out var aabb) == true)
                return aabb;
            else
                return null;
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
    public sealed class OOBoundingBox : BoundingBox
    {
        /// <summary>
        /// Axis-aligned bounding box
        /// </summary>
        private AABoundingBox _box;

        /// <summary>
        /// Local coordinate system
        /// </summary>
        private LocalCSys _coordinateSystem;

        /// <summary>
        /// Private constructor
        /// </summary>
        /// <param name="coordinateSystem">Coordinate system of bounding box</param>
        /// <param name="box">Axis-aligned bounding box</param>
        private OOBoundingBox(LocalCSys coordinateSystem, AABoundingBox box)
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
            if (LibraryImport.SelectCGAL().CGAL_GetOOBB(points, out var oobb) == true)
                return oobb;
            else
                return null;
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