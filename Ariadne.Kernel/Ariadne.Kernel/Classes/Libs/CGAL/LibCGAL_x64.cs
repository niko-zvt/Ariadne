using Ariadne.Kernel.Libs;
using Ariadne.Kernel.Math;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace Ariadne.Kernel.CGAL
{
    /// <summary>
    /// 64-bit version of C++ CGAL library
    /// </summary>
    internal class LibCGAL_x64 : ILibCGAL
    {
        #region "CPP_CGAL_DECLARATION"

        /// <summary>
        /// Get optimal oriented bounding box
        /// </summary>
        /// <param name="points">Point cloud</param>
        /// <param name="size">Size of point cloud</param>
        /// <param name="notification">Optimal oriented bounding box as JSON string</param>
        /// <returns>
        /// - true in the case, when the result is valid
        /// </returns>
        [DllImport("Ariadne.CGAL.x64", CallingConvention = CallingConvention.StdCall, ExactSpelling = false, EntryPoint = "GetOptimalOrientedBoundingBox")]
        private static extern int GetOptimalOrientedBoundingBox([In] CGAL_Point[] points, [In] int size, Notification notification);

        /// <summary>
        /// Get axis-aligned bounding box
        /// </summary>
        /// <param name="points">Point cloud</param>
        /// <param name="size">Size of point cloud</param>
        /// <param name="notification">Axis-aligned bounding box as JSON string</param>
        /// <returns>
        /// - true in the case, when the result is valid
        /// </returns>
        [DllImport("Ariadne.CGAL.x64", CallingConvention = CallingConvention.StdCall, ExactSpelling = false, EntryPoint = "GetAxisAlignedBoundingBox")]
        private static extern int GetAxisAlignedBoundingBox([In] CGAL_Point[] points, [In] int size, Notification notification);

        /// <summary>
        /// The method determines whether a point belongs to a grid created on the basis of a point cloud.
        /// </summary>
        /// <param name="point">Point</param>
        /// <param name="meshPoints">Point cloud of mesh</param>
        /// <param name="size">Size of point cloud</param>
        /// <param name="notification">Belonging of a point as a JSON string</param>
        /// <returns>
        /// <para> - true in the case, when the result is valid.</para>
        /// JSON contain:<br/>
        ///  - VERTEX    - if the point lies at the vertex of the grid.<br/>
        ///  - EDGE      - if the point lies at the edge of the grid.<br/>
        ///  - FACET     - if the point lies at the facet of the grid.<br/>
        ///  - CELL      - if the point lies inside the cell.<br/>
        ///  - OUTSIDE_CONVEX_HULL - if the point lies at the outside convex hull.<br/>
        ///  - OUTSIDE_AFFINE_HULL - if the point lies at the outside affine hull.<br/>
        /// </returns>
        [DllImport("Ariadne.CGAL.x64", CallingConvention = CallingConvention.StdCall, ExactSpelling = false, EntryPoint = "IsPointBelongToGrid")]
        private static extern int IsPointBelongToGrid([In] CGAL_Point point, [In] CGAL_Point[] meshPoints, [In] int size, Notification notification);

        /// <summary>
        /// The method determines the intersection of two segments defined by the start and end points.
        /// </summary>
        /// <param name="A1">Start point of first segment.</param>
        /// <param name="A2">End point of first segment.</param>
        /// <param name="B1">Start point of second segment.</param>
        /// <param name="B2">End point of second segment.</param>
        /// <param name="notification">Intersection type as JSON string.</param>
        /// <returns>
        /// <para> - true in the case, when the result is valid.</para>
        /// JSON contain:<br/>
        ///  - NULL    - if there are no intersection points.<br/>
        ///  - POINT   - if the intersection is a point.<br/>
        ///  - SEGMENT - if the intersection is a segment.<br/>
        /// </returns>
        [DllImport("Ariadne.CGAL.x64", CallingConvention = CallingConvention.StdCall, ExactSpelling = false, EntryPoint = "SegmentsIntersection")]
        private static extern int SegmentsIntersection([In] CGAL_Point A1, [In] CGAL_Point A2, [In] CGAL_Point B1, [In] CGAL_Point B2, Notification notification);

        #endregion

        #region "CGAL_INTERFACE_IMPLEMENTATION"

        /// <summary>
        /// The method calculate optimal oriented bounding box.
        /// </summary>
        /// <param name="points">Points for which OOBB will be calculate.</param>
        /// <param name="oobb">Optimal oriented bounding box.</param>
        /// <returns>
        /// - true in the case, when the result is valid.
        /// </returns>
        public bool CGAL_GetOOBB(List<Vector3D> points, out OOBoundingBox oobb)
        {
            int countOfPoints = points.Count;
            CGAL.CGAL_Point[] cgalPoints = new CGAL.CGAL_Point[countOfPoints];
            int index = 0;
            foreach (var point in points)
            {
                cgalPoints[index] = new CGAL.CGAL_Point(point.X, point.Y, point.Z);
                index++;
            }

            var jsonString = string.Empty;
            var result = GetOptimalOrientedBoundingBox(cgalPoints, countOfPoints, str => { jsonString = str; });

            if (result!= 0)
                throw new System.Exception("CGAL lib is fail!");

            if (string.IsNullOrEmpty(jsonString))
                throw new System.ArgumentNullException("AABB GetBoundingBoxByJSON - JsonAttribute string is string.Empty or null");

            // 1. Find all points
            const int countAABBPoints = 8;
            string[] jsonBoxes = jsonString.Split('\n');

            List<Vector3D> boxPoints = new List<Vector3D>();
            foreach (var jsonBox in jsonBoxes)
            {
                if (!string.IsNullOrEmpty(jsonBox))
                    boxPoints.Add(JsonSerializer.Deserialize<Vector3D>(jsonBox));
            }

            if (boxPoints.Count != countAABBPoints)
                throw new System.ArgumentOutOfRangeException("AABB GetBoundingBoxByJSON - JSON contains less/greater than 8 points!");

            // TODO: Implementation
            throw new System.NotImplementedException();

            // 1. Find LCS
            //CoordinateSystem cs = GlobalCSys.Instance;

            // 3. Find AABB
            //var minPoint = new Vector3D();
            //var maxPoint = new Vector3D();
            //AABoundingBox aabb = AABoundingBox.CreateByPoints(minPoint, maxPoint);

            //return new OOBoundingBox(cs, aabb);
        }

        /// <summary>
        /// The method calculate axis-aligned bounding box for global coordinate system.
        /// </summary>
        /// <param name="points">Points for which AABB will be calculate.</param>
        /// <param name="aabb">Axis-aligned bounding box.</param>
        /// <returns>
        /// - true in the case, when the result is valid.
        /// </returns>
        public bool CGAL_GetAABB(List<Vector3D> points, out AABoundingBox aabb)
        {

            CGAL.CGAL_Point[] cgalPoints = new CGAL.CGAL_Point[points.Count];
            int index = 0;
            foreach (var point in points)
            {
                cgalPoints[index] = new CGAL.CGAL_Point(point.X, point.Y, point.Z);
                index++;
            }

            var jsonStr = string.Empty;
            var result = GetAxisAlignedBoundingBox(cgalPoints, points.Count, str => { jsonStr = str; });

            if (result!= 0)
                throw new System.Exception("CGAL lib is fail!");

            // 2. Deserialize JSON box from C++ lib
            if (string.IsNullOrEmpty(jsonStr))
                throw new System.ArgumentNullException("OOBB GetBoundingBoxByJSON - JsonAttribute string is string.Empty or null");

            const int countOOBBPoints = 2;
            string[] jsonBoxes = jsonStr.Split('\n');

            List<Vector3D> boxPoints = new List<Vector3D>();
            foreach (var jsonBox in jsonBoxes)
            {
                if (!string.IsNullOrEmpty(jsonBox))
                    boxPoints.Add(JsonSerializer.Deserialize<Vector3D>(jsonBox));
            }

            if (boxPoints.Count != countOOBBPoints)
                throw new System.ArgumentOutOfRangeException("OOBB GetBoundingBoxByJSON - JSON contains less/greater than 2 points!");

            aabb = AABoundingBox.CreateByPoints(points[0], points[1]);

            return true;
        }

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
        public Utils.LocationType CGAL_IsBelongToGrid(Vector3D point, List<Vector3D> pointCloudOfGrid)
        {
            CGAL.CGAL_Point targetPoint = new CGAL.CGAL_Point(point.X, point.Y, point.Z);
            int countOfPoints = pointCloudOfGrid.Count;
            CGAL.CGAL_Point[] meshPoints = new CGAL.CGAL_Point[countOfPoints];
            
            int index = 0;
            foreach (var currentPoint in pointCloudOfGrid)
            {
                meshPoints[index] = new CGAL.CGAL_Point(currentPoint.X, currentPoint.Y, currentPoint.Z);
                index++;
            }

            var jsonResult = string.Empty;
            var result = IsPointBelongToGrid(targetPoint, meshPoints, countOfPoints, str => { jsonResult = str; });

            if (result != 0 || string.IsNullOrEmpty(jsonResult))
                throw new System.Exception("CGAL lib is fail! IsBelongToMesh().");

            if (jsonResult == "VERTEX")
            {
                return Utils.LocationType.Vertex;
            }
            else if (jsonResult == "EDGE")
            {
                return Utils.LocationType.Edge;
            }
            else if (jsonResult == "FACET")
            {
                return Utils.LocationType.Facet;
            }
            else if (jsonResult == "CELL")
            {
                return Utils.LocationType.Cell;
            }
            else if (jsonResult == "OUTSIDE_CONVEX_HULL")
            {
                return Utils.LocationType.OutsideConvexHull;
            }
            else if (jsonResult == "OUTSIDE_AFFINE_HULL")
            {
                return Utils.LocationType.OutsideAffineHull;
            }
            else
            {
                throw new System.Exception("CGAL lib is fail! IsBelongToMesh().");
            }
        }

        /// <summary>
        /// The method determines the intersection of two segments defined by the start and end points.
        /// </summary>
        /// <param name="A1">Start point of first segment.</param>
        /// <param name="A2">End point of first segment.</param>
        /// <param name="B1">Start point of second segment.</param>
        /// <param name="B2">End point of second segment.</param>
        /// <param name="intersectionPoints">List of ntersection points.</param>
        /// <returns>
        /// Intersection type:<br/>
        ///  - Null    - if there are no intersection points.<br/>
        ///  - Point   - if the intersection is a point.<br/>
        ///  - Segment - if the intersection is a segment.<br/>
        /// </returns>
        public Utils.IntersectionType CGAL_GetIntersectionOfTwoSegments(Vector3D A1, Vector3D A2, Vector3D B1, Vector3D B2, out List<Vector3D> intersectionPoints)
        {
            // 1. Run CGAL
            CGAL_Point A_start = new CGAL_Point(A1.X, A1.Y, A1.Z);
            CGAL_Point A_end = new CGAL_Point(A2.X, A2.Y, A2.Z);
            CGAL_Point B_start = new CGAL_Point(B1.X, B1.Y, B1.Z);
            CGAL_Point B_end = new CGAL_Point(B2.X, B2.Y, B2.Z);

            var jsonString = string.Empty;
            int result = SegmentsIntersection(A_start, A_end, B_start, B_end, str => { jsonString = str; });

            if (result == 1 || string.IsNullOrEmpty(jsonString))
                throw new System.Exception("CGAL lib is fail!");

            // 2. Deserialize JSON points from C++ lib
            var type = Utils.IntersectionType.Null;
            string[] jsonStrings = jsonString.Split('\n');
            intersectionPoints = new List<Vector3D>();
            foreach (var str in jsonStrings)
            {
                if (string.IsNullOrEmpty(str) == true)
                    continue;

                if (str.Contains("IntersectionType"))
                {
                    if (str.Contains("NULL"))
                    {
                        type = Utils.IntersectionType.Null;
                    }
                    else if (str.Contains("POINT"))
                    {
                        type = Utils.IntersectionType.Point;
                    }
                    else if (str.Contains("SEGMENT"))
                    {
                        type = Utils.IntersectionType.Segment;
                    }
                    else
                    {
                        throw new System.FormatException("Intersection type is invalid!");
                    }
                    continue;
                }
                else
                {
                    intersectionPoints.Add(JsonSerializer.Deserialize<Vector3D>(str));
                }
            }
            return type;
        }

        #endregion
    }
}
