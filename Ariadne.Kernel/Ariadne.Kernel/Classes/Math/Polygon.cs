namespace Ariadne.Kernel.Math
{
    /// <summary>
    /// Abstract class for hiding the implementation of the Polygon class
    /// </summary>
    abstract class Polygon
    {
        /// <summary>
        /// Polygon specific data
        /// </summary>
        protected MathNet.Spatial.Euclidean.Plane _polygon;

        /// <summary>
        /// Virtual method to return the type of the polygon
        /// </summary>
        /// <returns>Type of polygon</returns>
        public abstract PolygonType GetPolygonType();
    }

    /// <summary>
    /// Enumeration of all available types of polygons
    /// </summary>
    enum PolygonType
    {
        Polygon3D,
    }

    /// <summary>
    /// Class of a polygon in three-dimensional space
    /// </summary>
    class Polygon3D : Polygon
    {
        /// <summary>
        /// Default constructor. Creates a zero polygon
        /// </summary>
        private Polygon3D()
        {
            var p1 = new MathNet.Spatial.Euclidean.Point3D(0.0f, 0.0f, 0.0f);
            var p2 = new MathNet.Spatial.Euclidean.Point3D(0.0f, 0.0f, 0.0f);
            var p3 = new MathNet.Spatial.Euclidean.Point3D(0.0f, 0.0f, 0.0f);

            _polygon = MathNet.Spatial.Euclidean.Plane.FromPoints(p1, p2, p3);
        }

        /// <summary>
        /// Constructor by points
        /// </summary>
        /// <param name="p1">First point</param>
        /// <param name="p2">Second point</param>
        /// <param name="p3">Third point</param>
        public Polygon3D(Vector3D p1, Vector3D p2, Vector3D p3)
        {
            var point1 = new MathNet.Spatial.Euclidean.Point3D(p1.X, p1.Y, p1.Z);
            var point2 = new MathNet.Spatial.Euclidean.Point3D(p2.X, p2.Y, p2.Z);
            var point3 = new MathNet.Spatial.Euclidean.Point3D(p3.X, p3.Y, p3.Z);
            _polygon = MathNet.Spatial.Euclidean.Plane.FromPoints(point1, point2, point3);
        }

        /// <summary>
        /// Get the distance to the point along the Normal
        /// </summary>
        /// <param name="point">Point</param>
        /// <returns>Distance</returns>
        public float SignedDistanceTo(Vector3D point)
        {
            var p = new MathNet.Spatial.Euclidean.Point3D(point.X, point.Y, point.Z);
            return (float)_polygon.SignedDistanceTo(p);
        }

        /// <summary>
        /// Returns the polygon type
        /// </summary>
        /// <returns>Polygon type</returns>
        public override PolygonType GetPolygonType()
        {
            return PolygonType.Polygon3D;
        }

        /// <summary>
        /// The method returns representations of the specific polygon as a string
        /// </summary>
        /// <returns>A string that represents the current object</returns>
        public override string ToString()
        {
            return GetPolygonType().ToString();
        }
    }
 }
