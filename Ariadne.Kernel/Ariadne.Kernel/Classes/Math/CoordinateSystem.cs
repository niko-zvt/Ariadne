namespace Ariadne.Kernel.Math
{
    /// <summary>
    /// Abstract class for hiding the implementation of the CoordinateSystem class
    /// </summary>
    abstract class CoordinateSystem
    {
        /// <summary>
        /// Coordinate system specific data
        /// </summary>
        protected MathNet.Spatial.Euclidean.CoordinateSystem _coordSys;

        /// <summary>
        /// Virtual method to return the type of the coordinate system
        /// </summary>
        /// <returns>Type of coordinate system</returns>
        public abstract CoordinateSystemType GetCoordinateSystemType();
    }

    /// <summary>
    /// Enumeration of all available types of coordinate systems
    /// </summary>
    enum CoordinateSystemType
    {
        GlobalCS,
        LocalCS,
    }

    /// <summary>
    /// Class singleton of a global coordinate system in three-dimensional space
    /// </summary>
    sealed class GlobalCSys : CoordinateSystem
    {
        /// <summary>
        /// Lazy instance holder
        /// </summary>
        private static readonly System.Lazy<GlobalCSys> instanceHolder = new System.Lazy<GlobalCSys>(() => new GlobalCSys());

        /// <summary>
        /// Private constructor.
        /// </summary>
        private GlobalCSys()
        {
            _coordSys = new MathNet.Spatial.Euclidean.CoordinateSystem();
        }

        /// <summary>
        /// Instance of global coordinate system
        /// </summary>
        public static GlobalCSys Instance
        {
            get { return instanceHolder.Value; }
        }

        /// <summary>
        /// X-axis of global coordinate system
        /// </summary>
        public Vector3D XAxis
        {
            get
            {
                return new Vector3D(_coordSys.XAxis.X,
                                    _coordSys.XAxis.Y,
                                    _coordSys.XAxis.Z);
            }
        }

        /// <summary>
        /// Y-axis of global coordinate system
        /// </summary>
        public Vector3D YAxis
        {
            get
            {
                return new Vector3D(_coordSys.YAxis.X,
                                    _coordSys.YAxis.Y,
                                    _coordSys.YAxis.Z);
            }
        }

        /// <summary>
        /// Z-axis of global coordinate system
        /// </summary>
        public Vector3D ZAxis
        {
            get
            {
                return new Vector3D(_coordSys.ZAxis.X,
                                    _coordSys.ZAxis.Y,
                                    _coordSys.ZAxis.Z);
            }
        }

        /// <summary>
        /// Original point location of global coordinate system
        /// </summary>
        public Vector3D Origin
        {
            get
            {
                return new Vector3D(_coordSys.Origin.X,
                                    _coordSys.Origin.Y,
                                    _coordSys.Origin.Z);
            }
        }

        /// <summary>
        /// Returns the coordinate system type
        /// </summary>
        /// <returns>Cordinate system type</returns>
        public override CoordinateSystemType GetCoordinateSystemType()
        {
            return CoordinateSystemType.GlobalCS;
        }
    }

    /// <summary>
    /// Class of a local coordinate system in three-dimensional space
    /// </summary>
    sealed class LocalCSys : CoordinateSystem
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public LocalCSys()
        {
            _coordSys = new MathNet.Spatial.Euclidean.CoordinateSystem();
        }

        /// <summary>
        /// Constructor along the axis and origin point.
        /// </summary>
        /// <param name="xAxis">Vector of X-axis</param>
        /// <param name="yAxis">Vector of Y-axis</param>
        /// <param name="zAxis">Vector of Z-axis</param>
        /// <param name="origin">Origin point</param>
        public LocalCSys(Vector3D xAxis, Vector3D yAxis, Vector3D zAxis, Vector3D origin)
        {
            var oX = ((MathNet.Spatial.Euclidean.Vector3D)xAxis).Normalize();
            var oY = ((MathNet.Spatial.Euclidean.Vector3D)yAxis).Normalize();
            var oZ = ((MathNet.Spatial.Euclidean.Vector3D)zAxis).Normalize();
            var O = (MathNet.Spatial.Euclidean.Point3D)zAxis;
            _coordSys = new MathNet.Spatial.Euclidean.CoordinateSystem(O, oX, oY, oZ);
        }

        /// <summary>
        /// X-axis of local coordinate system
        /// </summary>
        public Vector3D XAxis
        {
            get
            {
                return new Vector3D(_coordSys.XAxis.X,
                                    _coordSys.XAxis.Y,
                                    _coordSys.XAxis.Z);
            }
        }

        /// <summary>
        /// Y-axis of local coordinate system
        /// </summary>
        public Vector3D YAxis
        {
            get
            {
                return new Vector3D(_coordSys.YAxis.X,
                                    _coordSys.YAxis.Y,
                                    _coordSys.YAxis.Z);
            }
        }

        /// <summary>
        /// Z-axis of local coordinate system
        /// </summary>
        public Vector3D ZAxis
        {
            get
            {
                return new Vector3D(_coordSys.ZAxis.X,
                                    _coordSys.ZAxis.Y,
                                    _coordSys.ZAxis.Z);
            }
        }

        /// <summary>
        /// Original point location of local coordinate system
        /// </summary>
        public Vector3D Origin
        {
            get
            {
                return new Vector3D(_coordSys.Origin.X,
                                    _coordSys.Origin.Y,
                                    _coordSys.Origin.Z);
            }
        }

        /// <summary>
        /// Returns the coordinate system type
        /// </summary>
        /// <returns>Cordinate system type</returns>
        public override CoordinateSystemType GetCoordinateSystemType()
        {
            return CoordinateSystemType.LocalCS;
        }
    }
}
