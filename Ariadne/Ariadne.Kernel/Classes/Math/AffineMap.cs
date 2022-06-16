namespace Ariadne.Kernel.Math
{
    /// <summary>
    /// Abstract class for hiding the implementation of the affine map class.
    /// </summary>
    abstract class AffineMap
    {
        /// <summary>
        /// Virtual method to return the type of the affine map.
        /// </summary>
        /// <returns>Type of affine map.</returns>
        public abstract AffineMapType GetAffineMapType();
    }

    /// <summary>
    /// Enumeration of all available types of affine map.
    /// </summary>
    enum AffineMapType
    {
        AffineMap2D, 
        AffineMap3D
    }

    /// <summary>
    /// Class of the affine map in three-dimensional space.
    /// </summary>
    class AffineMap3D : AffineMap
    {
        /// <summary>
        /// Transformation matrix.
        /// </summary>
        private Matrix3x3 _M;

        /// <summary>
        /// Translation vector.
        /// </summary>
        private Vector3D _T;

        /// <summary>
        /// Homogeneous components.
        /// </summary>
        private Vector3D _S;

        /// <summary>
        /// Homogeneous component.
        /// </summary>
        private float _I;

        /// <summary>
        /// Default constructor. 
        /// </summary>
        public AffineMap3D()
        {
            _M = new Matrix3x3(1, 1, 1);
            _T = new Vector3D();
            _S = new Vector3D();
            _I = 1.0f;
        }

        /// <summary>
        /// Constructor by transformation matrix and translation vector.
        /// </summary>
        /// <param name="R">Transformation matrix.</param>
        /// <param name="T">Translation vector.</param>
        public AffineMap3D(Matrix3x3 R, Vector3D T)
        {
            _M = R;
            _T = T;
            _S = new Vector3D();
            _I = 1.0f;
        }

        /// <summary>
        /// Constructor by coordinate systems
        /// </summary>
        /// <param name="sourceCS">Source CSys</param>
        /// <param name="targetCS">Target CSys</param>
        public AffineMap3D(CoordinateSystem sourceCS, CoordinateSystem targetCS)
        {
            _T = sourceCS.Origin - targetCS.Origin;
            throw new System.NotImplementedException();
            // TODO: _M = Calculate M 
            _S = new Vector3D();
            _I = 1.0f;
        }

        /// <summary>
        /// Constructor by transformation vectors and translation vector.
        /// </summary>
        /// <param name="i">First transformation vector.</param>
        /// <param name="j">Second transformation vector.</param>
        /// <param name="k">Third transformation vector.</param>
        /// <param name="T">Translation vector.</param>
        public AffineMap3D(Vector3D i, Vector3D j, Vector3D k, Vector3D T)
        {
            _M = new Matrix3x3(i, j, k);
            _T = T;
            _S = new Vector3D();
            _I = 1.0f;
        }

        /// <summary>
        /// Transform point
        /// </summary>
        /// <param name="point">Target point</param>
        /// <returns>Result point after affine mapping</returns>
        public Vector3D TransformPoint(Vector3D point)
        {
            var rPoint = _M * point;
            var trPoint = rPoint + _T;
            return trPoint;
        }

        public static Vector3D TransformPoint(Vector3D point, CoordinateSystem sourceCS, CoordinateSystem targetCS)
        {
            var pX = point.X;
            var pY = point.Y;
            var pZ = point.Z;

            var m1 = new HValue[,] { { (HValue)new Vector3D(), (HValue)targetCS.XAxis,   (HValue)targetCS.YAxis,   (HValue)targetCS.ZAxis   },
                                    { (HValue)pX, (HValue)sourceCS.XAxis.X, (HValue)sourceCS.YAxis.X, (HValue)sourceCS.ZAxis.X },
                                    { (HValue)pY, (HValue)sourceCS.XAxis.Y, (HValue)sourceCS.YAxis.Y, (HValue)sourceCS.ZAxis.Y },
                                    { (HValue)pZ, (HValue)sourceCS.XAxis.Z, (HValue)sourceCS.YAxis.Z, (HValue)sourceCS.ZAxis.Z } };

            var m2 = new HValue[,] { { (HValue)sourceCS.XAxis.X, (HValue)sourceCS.YAxis.X, (HValue)sourceCS.ZAxis.X },
                                      { (HValue)sourceCS.XAxis.Y, (HValue)sourceCS.YAxis.Y, (HValue)sourceCS.ZAxis.Y },
                                      { (HValue)sourceCS.XAxis.Z, (HValue)sourceCS.YAxis.Z, (HValue)sourceCS.ZAxis.Z } };

            var firstD = Utils.CalculateDeterminant(m1);
            var secondD = Utils.CalculateDeterminant(m2);

            var vec = -1 * (firstD / secondD);

            if (vec.IsVector == false)
                throw new System.ArithmeticException();

            if (vec.Vector.Size != 3)
                throw new System.IndexOutOfRangeException();

            return new Vector3D(vec.Vector.GetValueAt(0), vec.Vector.GetValueAt(1), vec.Vector.GetValueAt(2));
        }

        /// <summary>
        /// Method return the type of the affine map.
        /// </summary>
        /// <returns>Type of affine map.</returns>
        public override AffineMapType GetAffineMapType()
        {
            return AffineMapType.AffineMap3D;
        }
    }
}
