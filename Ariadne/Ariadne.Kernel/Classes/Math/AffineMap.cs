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
        private Matrix3x3 _R;

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
            _R = new Matrix3x3(1, 1, 1);
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
            _R = R;
            _T = T;
            _S = new Vector3D();
            _I = 1.0f;
        }

        public AffineMap3D(Matrix3x3 R, Vector3D T, Vector3D S, float I) : this(R, T)
        {
            _S = S;
            _I = I;
        }

        public AffineMap3D(float[,] array)
        {
            if (array.GetLength(0) == 4 || array.GetLength(1) == 4)
            {
                _R = new Matrix3x3(array[0, 0], array[0, 1], array[0, 2],
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

        public AffineMap3D(CoordinateSystem cs)
        {
            _R = new Matrix3x3() { XX = cs.XAxis.X, XY = cs.YAxis.X, XZ = cs.ZAxis.X,
                                   YX = cs.XAxis.Y, YY = cs.YAxis.Y, YZ = cs.ZAxis.Y,  
                                   ZX = cs.XAxis.Z, ZY = cs.YAxis.Z, ZZ = cs.ZAxis.Z };

            _T = cs.Origin;
            _S = new Vector3D();
            _I = 1.0f;
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
            //    // TODO: _R = Rotate(anotherMap._R);
            //    _R = _R * anotherMap._R; // ERROR!!!
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
            return _R * point + _T;
        }

        public static AffineMap3D GetChangeBasisMap(CoordinateSystem sourceCS, CoordinateSystem targetCS)
        {
            return GetChangeBasisMap(sourceCS, targetCS, sourceCS.Origin - targetCS.Origin);
        }

        protected static AffineMap3D GetChangeBasisMap(CoordinateSystem sourceCS, CoordinateSystem targetCS, Vector3D translation)
        {
            var AB = new double[,] { { sourceCS.XAxis.X, sourceCS.XAxis.Y, sourceCS.XAxis.Z, targetCS.XAxis.X, targetCS.XAxis.Y, targetCS.XAxis.Z },
                                     { sourceCS.YAxis.X, sourceCS.YAxis.Y, sourceCS.YAxis.Z, targetCS.YAxis.X, targetCS.YAxis.Y, targetCS.YAxis.Z },
                                     { sourceCS.ZAxis.X, sourceCS.ZAxis.Y, sourceCS.ZAxis.Z, targetCS.ZAxis.X, targetCS.ZAxis.Y, targetCS.ZAxis.Z } };

            var CD = new Math.ReducedRowEchelonForm(AB).Result;

            if (CD.GetLength(0) != 3 || CD.GetLength(1) != 6)
                throw new System.ArgumentOutOfRangeException("CD is not a 3x6 matrix!");

            var D = new double[,] { { CD[0,3], CD[0,4], CD[0,5] },
                                    { CD[1,3], CD[1,4], CD[1,5] },
                                    { CD[2,3], CD[2,4], CD[2,5] } };


            var R = (new Matrix3x3(D)).Inverse() as Matrix3x3;

            var T = new Vector3D();
            if (translation != null)
                T = translation;

            var S = new Vector3D();
            var I = 1.0f;

            return new AffineMap3D(R, T, S, I);
        }

        public static Vector3D ChangeBasis(Vector3D point, CoordinateSystem sourceCS, CoordinateSystem targetCS)
        {
            var map = GetChangeBasisMap(sourceCS, targetCS);
            return map.TransformPoint(point);
        }

        public static Vector3D GlobalToLocalCS(Vector3D globalPoint, CoordinateSystem localCS)
        {
            var rotation = (new Matrix3x3() { XX = localCS.XAxis.X, XY = localCS.YAxis.X, XZ = localCS.ZAxis.X,
                                              YX = localCS.XAxis.Y, YY = localCS.YAxis.Y, YZ = localCS.ZAxis.Y,  
                                              ZX = localCS.XAxis.Z, ZY = localCS.YAxis.Z, ZZ = localCS.ZAxis.Z }
                           ).Inverse() as Matrix3x3;

            var translation = -1.0f * rotation * localCS.Origin;

            var map = new AffineMap3D(rotation, translation);
            return map.TransformPoint(globalPoint);
        }

        public static Vector3D LocalToGlobalCS(Vector3D localPoint, CoordinateSystem localCS)
        {
            var map = new AffineMap3D(localCS);
            return map.TransformPoint(localPoint);
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
            var matrix = new HValue[,] { { (HValue)_R.XX, (HValue)_R.XY,   (HValue)_R.XZ,   (HValue)_T.X   },
                                         { (HValue)_R.YX, (HValue)_R.YY,   (HValue)_R.YZ,   (HValue)_T.Y   },
                                         { (HValue)_R.ZX, (HValue)_R.ZY,   (HValue)_R.ZZ,   (HValue)_T.Z   },
                                         { (HValue)_S.X,  (HValue)_S.Y,    (HValue)_S.Z,    (HValue)_I   } };
            
            var value = Utils.CalculateDeterminant(matrix);

            if (value.IsVector == true)
                return float.NaN;

            return value.Float;
        }

        public override AffineMap InvMap()
        {
            var array = new float[,] { { _R.XX, _R.XY, _R.XZ, _T.X   },
                                       { _R.YX, _R.YY, _R.YZ, _T.Y   },
                                       { _R.ZX, _R.ZY, _R.ZZ, _T.Z   },
                                       { _S.X, _S.Y,   _S.Z,  _I   } };

            MatrixNxM matrix = new MatrixNxM(array);
            var invArray = matrix.Inverse().ToArray();

            return new AffineMap3D(invArray);
        }
    }
}
