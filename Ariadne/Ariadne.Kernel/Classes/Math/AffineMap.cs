using Ariadne.Kernel.Libs;
using System;

namespace Ariadne.Kernel.Math
{
    /// <summary>
    /// Abstract class for hiding the implementation of the affine map class.
    /// </summary>
    public abstract class AffineMap
    {
        /// <summary>
        /// Virtual method to return the type of the affine map.
        /// </summary>
        /// <returns>Type of affine map.</returns>
        public abstract AffineMapType GetAffineMapType();

        /// <summary>
        /// Check degenerate transformation.
        /// </summary>
        /// <returns>True if transformation is degenerated.</returns>
        public abstract bool Degenerated();

        /// <summary>
        /// Calculate determinant of the map.
        /// </summary>
        /// <returns>Determinant.</returns>
        public abstract float Determinant();

        /// <summary>
        /// Create new affine map with reverse transformation.
        /// </summary>
        /// <returns>New affine map with reverse transformation.</returns>
        public abstract AffineMap InvMap();

        /// <summary>
        /// Multiply affine maps.
        /// </summary>
        /// <param name="affineMap">Another affine map.</param>
        /// <returns>New affine map.</returns>
        public abstract AffineMap Multiply(AffineMap affineMap);
    }

    /// <summary>
    /// Enumeration of all available types of affine map.
    /// </summary>
    public enum AffineMapType
    {
        AffineMap2D, 
        AffineMap3D
    }

    /// <summary>
    /// Class of the affine map in three-dimensional space.
    /// </summary>
    public class AffineMap3D : AffineMap
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

        public AffineMap3D(Matrix3x3 R, Vector3D T, Vector3D S, float I) : this(R, T)
        {
            _S = S;
            _I = I;
        }

        /// <summary>
        /// Constructor by coordinate systems
        /// </summary>
        /// <param name="sourceCS">Source CSys</param>
        /// <param name="targetCS">Target CSys</param>
        public AffineMap3D(CoordinateSystem sourceCS, CoordinateSystem targetCS)
        {
            throw new System.NotImplementedException();
            
            // TODO: CGAL Calculate
            // _T = sourceCS.Origin - targetCS.Origin;
            // _M = Calculate M 
            //_S = new Vector3D();
            //_I = 1.0f;
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

        public AffineMap3D(float[,] array)
        {
            if (array.GetLength(0) == 4 || array.GetLength(1) == 4)
            {
                _M = new Matrix3x3(array[0, 0], array[0, 1], array[0, 2],
                                   array[1, 0], array[1, 1], array[1, 2],
                                   array[2, 0], array[2, 1], array[2, 2]);
                
                _T = new Vector3D(array[0, 3], array[1, 3], array[2, 3]);

                _S = new Vector3D(array[3, 0], array[3, 1], array[3, 2]);

                _I = array[3, 3];
            }
            else
            {
                throw new System.ArgumentException("Size of array != 16!");
            }
        }

        /// <summary>
        /// Multiply affine maps.
        /// </summary>
        /// <param name="affineMap">Another affine map.</param>
        /// <returns>New affine map.</returns>
        public override AffineMap Multiply(AffineMap affineMap)
        {
            throw new NotImplementedException();

            // CGAL
            //if (affineMap is AffineMap3D)
            //{
            //    var anotherMap = affineMap as AffineMap3D;
            //    // TODO: _M = Rotate(anotherMap._M);
            //    _M = _M * anotherMap._M; // ERROR!!!
            //    _T.X += anotherMap._T.X;
            //    _T.Y += anotherMap._T.Y;
            //    _T.Z += anotherMap._T.Z;
            //}
            //throw new ArgumentException("Affine map is not a AffineMap3D!");
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
            var result = LibraryImport.SelectCGAL().CGAL_TransformPoint(point, sourceCS, targetCS, out var transformPoint);

            if (result == false)
                return new Vector3D(float.NaN);
            else
                return transformPoint;
        }

        /// <summary>
        /// Method return the type of the affine map.
        /// </summary>
        /// <returns>Type of affine map.</returns>
        public override AffineMapType GetAffineMapType()
        {
            return AffineMapType.AffineMap3D;
        }

        /// <summary>
        /// Check degenerate transformation.
        /// </summary>
        /// <returns>True if transformation is degenerated.</returns>
        public override bool Degenerated()
        {
            return System.Math.Abs(Determinant()) < Utils.LinearTolerance * Utils.LinearTolerance * Utils.LinearTolerance;
        }

        /// <summary>
        /// Calculate determinant of the map
        /// </summary>
        /// <returns>Determinant</returns>
        public override float Determinant()
        {
            var matrix = new HValue[,] { { (HValue)_M.XX, (HValue)_M.XY,   (HValue)_M.XZ,   (HValue)_T.X   },
                                         { (HValue)_M.YX, (HValue)_M.YY,   (HValue)_M.YZ,   (HValue)_T.Y   },
                                         { (HValue)_M.ZX, (HValue)_M.ZY,   (HValue)_M.ZZ,   (HValue)_T.Z   },
                                         { (HValue)_S.X,  (HValue)_S.Y,    (HValue)_S.Z,    (HValue)_I   } };
            
            var value = Utils.CalculateDeterminant(matrix);

            if (value.IsVector == true)
                return float.NaN;

            return value.Float;
        }

        public override AffineMap InvMap()
        {
            var array = new float[,] { { _M.XX, _M.XY, _M.XZ, _T.X   },
                                       { _M.YX, _M.YY, _M.YZ, _T.Y   },
                                       { _M.ZX, _M.ZY, _M.ZZ, _T.Z   },
                                       { _S.X, _S.Y,   _S.Z,  _I   } };

            MatrixNxM matrix = new MatrixNxM(array);
            var invArray = matrix.Inverse().ToArray();

            return new AffineMap3D(invArray);
        }
    }
}
