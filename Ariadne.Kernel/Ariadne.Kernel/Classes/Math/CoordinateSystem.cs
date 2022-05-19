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
        /// <returns>Type of plane</returns>
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
        public LocalCSys()
        {
            _coordSys = new MathNet.Spatial.Euclidean.CoordinateSystem();
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
