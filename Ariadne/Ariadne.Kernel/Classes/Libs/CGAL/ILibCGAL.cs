using Ariadne.Kernel.CGAL;
using Ariadne.Kernel;
using Ariadne.Kernel.Math;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Ariadne.Kernel.Libs
{
    /// <summary>
    /// The essence of notifications, which transmits any information from the C++ library via JSON strings
    /// </summary>
    /// <param name="JSONvalue">JSON string</param>
    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public delegate void Notification(string JSONvalue);

    /// <summary>
    /// External interface of the CGAL library
    /// </summary>
    public interface ILibCGAL : ILibraryImport
    {
        /// <summary>
        /// The method calculate optimal oriented bounding box.
        /// </summary>
        /// <param name="points">Points for which OOBB will be calculate.</param>
        /// <param name="oobb">Optimal oriented bounding box.</param>
        /// <returns>
        /// - true in the case, when the result is valid.
        /// </returns>
        public bool CGAL_GetOOBB(List<Vector3D> points, out OOBoundingBox oobb);

        /// <summary>
        /// The method calculate axis-aligned bounding box for global coordinate system.
        /// </summary>
        /// <param name="points">Points for which AABB will be calculate.</param>
        /// <param name="aabb">Axis-aligned bounding box.</param>
        /// <returns>
        /// - true in the case, when the result is valid.
        /// </returns>
        public bool CGAL_GetAABB(List<Vector3D> points, out AABoundingBox aabb);

        /// <summary>
        /// The method determines whether a point belongs to a grid created on the basis of a point cloud.
        /// </summary>
        /// <param name="point">Point</param>
        /// <param name="pointCloudOfGrid">Point cloud of grid</param>
        /// <returns>
        /// Location type:<br/>
        ///  - Vertex    - if the point lies at the vertex of the grid.<br/>
        ///  - Edge      - if the point lies at the edge of the grid.<br/>
        ///  - Facet     - if the point lies at the facet of the grid.<br/>
        ///  - Cell      - if the point lies inside the cell.<br/>
        ///  - OutsideConvexHull - if the point lies at the outside convex hull.<br/>
        ///  - OutsideAffineHull - if the point lies at the outside affine hull.<br/>
        /// </returns>
        public Utils.LocationType CGAL_IsBelongToGrid(Vector3D point, List<Vector3D> pointCloudOfGrid);

        /// <summary>
        /// The method determines the intersection of two segments defined by the start and end points.
        /// </summary>
        /// <param name="A1">Start point of first segment.</param>
        /// <param name="A2">End point of first segment.</param>
        /// <param name="B1">Start point of second segment.</param>
        /// <param name="B2">End point of second segment.</param>
        /// <param name="intersectionPoints">List of intersection points.</param>
        /// <returns>
        /// Intersection type:<br/>
        ///  - Null    - if there are no intersection points.<br/>
        ///  - Point   - if the intersection is a point.<br/>
        ///  - Segment - if the intersection is a segment.<br/>
        /// </returns>
        public Utils.IntersectionType CGAL_GetIntersectionOfTwoSegments(Vector3D A1, Vector3D A2, Vector3D B1, Vector3D B2, out List<Vector3D> intersectionPoints);

        /// <summary>
        /// The method determines the intersection of two lines defined by the start and end points.
        /// </summary>
        /// <param name="A1">Start point of first line.</param>
        /// <param name="A2">End point of first line.</param>
        /// <param name="B1">Start point of second line.</param>
        /// <param name="B2">End point of second line.</param>
        /// <param name="intersectionPoints">List of intersection points.</param>
        /// <returns>
        /// Intersection type:<br/>
        ///  - Null    - if there are no intersection points.<br/>
        ///  - Point   - if the intersection is a point.<br/>
        ///  - Line    - if the intersection is a line.<br/>
        /// </returns>
        public Utils.IntersectionType CGAL_GetIntersectionOfTwoLines(Vector3D A1, Vector3D A2, Vector3D B1, Vector3D B2, out List<Vector3D> intersectionPoints);

        /// <summary>
        /// The method calculate affine transformation of point (Source To Target CS)
        /// </summary>
        /// <param name="point">Point</param>
        /// <param name="sourceCS">Source coordinate system</param>
        /// <param name="targetCS">Target coordinate system</param>
        /// <returns>
        /// - true in the case, when the result is valid.
        /// </returns>
        public bool CGAL_TransformPoint(Vector3D point, CoordinateSystem sourceCS, CoordinateSystem targetCS, out Vector3D transformPoint);
    }
}