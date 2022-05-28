using Ariadne.Kernel.Libs;
using System;
using System.Collections.Generic;

namespace Ariadne.Kernel.Math
{
    /// <summary>
    /// Static class for implementing a set of general mathematical functions
    /// </summary>
    static class Utils
    {
        /// <summary>
        /// Global tolerance
        /// </summary>
        public const float Tolerance = 0.000001f;

        /// <summary>
        /// Point location type
        /// </summary>
        public enum LocationType
        {
            Vertex,
            Edge,
            Facet,
            Cell,
            OutsideConvexHull,
            OutsideAffineHull
        }

        /// <summary>
        /// Calculate the principal invariants
        /// </summary>
        /// <param name="squareMatrix">Second-rank tensor / 3x3 matrix</param>
        /// <returns>The principal invariants</returns>
        public static Vector3D CalculatePrincipalInvariants(Matrix3x3 squareMatrix)
        {
            if (!squareMatrix.IsSquare())
                throw new ArgumentException("Input matrix is not square");

            //Principal invariants
            var I1 = squareMatrix.Trace();
            var I2 = 0.5 * (System.Math.Pow(I1, 2) - squareMatrix.SquareTrace());
            var I3 = squareMatrix.Determinant();

            return new Vector3D(I1, I2, I3);
        }

        /// <summary>
        /// Calculate the main invariants
        /// </summary>
        /// <param name="squareMatrix">Second-rank tensor / 3x3 matrix</param>
        /// <returns>The main invariants</returns>
        public static Vector3D CalculateMainInvariants(Matrix3x3 squareMatrix)
        {
            if (!squareMatrix.IsSquare())
                throw new ArgumentException("Input matrix is not square");

            var principalInvariants = CalculatePrincipalInvariants(squareMatrix);

            var I1 = principalInvariants.X;
            var I2 = principalInvariants.Y;
            var I3 = principalInvariants.Z;

            var J1 = I1;
            var J2 = I1 * I1 - 2 * I2;
            var J3 = I1 * I1 * I1 - 3 * I1 * I2 + 3 * I3;

            return new Vector3D(J1, J2, J3);
        }

        /// <summary>
        /// Find the roots of the cubic equation A + B*x + C*x^2 + x^3 = 0 by vector components {A, B, C}
        /// </summary>
        /// <param name="vector">Vector</param>
        /// <returns>The three roots of the cubic equation</returns>
        public static Vector3D FindCubicRoots(Vector3D vector)
        {
            var roots = MathNet.Numerics.RootFinding.Cubic.RealRoots(vector.X, vector.Y, vector.Z);
            return new Vector3D(roots.Item1, roots.Item2, roots.Item3);
        }

        /// <summary>
        /// Find the roots of the cubic equation A + B*x + C*x^2 + x^3 = 0 by float values
        /// </summary>
        /// <param name="a">Coefficient A</param>
        /// <param name="b">Coefficient B</param>
        /// <param name="c">Coefficient C</param>
        /// <returns>The three roots of the cubic equation</returns>
        public static Vector3D FindCubicRoots(float a, float b, float c)
        {
            var roots = MathNet.Numerics.RootFinding.Cubic.RealRoots(a, b, c);
            return new Vector3D(roots.Item1, roots.Item2, roots.Item3);
        }

        /// <summary>
        /// Find the roots of the cubic equation A + B*x + C*x^2 + x^3 = 0 by double values
        /// </summary>
        /// <param name="a">Coefficient A</param>
        /// <param name="b">Coefficient B</param>
        /// <param name="c">Coefficient C</param>
        /// <returns>The three roots of the cubic equation</returns>
        public static Vector3D FindCubicRoots(double a, double b, double c)
        {
            var roots = MathNet.Numerics.RootFinding.Cubic.RealRoots((float)a, (float)b, (float)c);
            return new Vector3D(roots.Item1, roots.Item2, roots.Item3);
        }

        /// <summary>
        /// Calculate the stress invariants
        /// </summary>
        /// <param name="squareMatrix">Second-rank tensor / 3x3 matrix</param>
        /// <returns>The stress invariants</returns>
        public static Vector3D CalculateStressInvariants(Matrix3x3 squareMatrix)
        {
            if (!squareMatrix.IsSquare())
                throw new ArgumentException("Input matrix is not square");

            var Sxx = squareMatrix.XX;
            var Sxy = squareMatrix.XY;
            var Sxz = squareMatrix.XZ;

            var Syx = squareMatrix.YX;
            var Syy = squareMatrix.YY;
            var Syz = squareMatrix.YZ;

            var Szx = squareMatrix.ZX;
            var Szy = squareMatrix.ZY;
            var Szz = squareMatrix.ZZ;

            var I1 = Sxx + Syy + Szz;
            var I2 = (Sxx * Syy) + (Syy * Szz) + (Szz * Sxx) + ((Sxy * Sxy) + (Syz * Syz) + (Szx * Szx));
            var I3 = (Sxx * Syy * Szz) - (Sxx * Syz * Syz) - (Syy * Szx * Szx) - (Szz * Sxy * Sxy) + (2 * Sxy * Syz * Szx);

            return new Vector3D(I1, I2, I3);
        }

        /// <summary>
        /// The function calculates the position of a point relative to the mesh given by a set of points
        /// </summary>
        /// <param name="point">Points</param>
        /// <param name="meshByPoints">Mesh by points</param>
        /// <returns>Point location type</returns>
        public static LocationType CalculatePositionRelativelyMesh(Vector3D point, List<Vector3D> meshByPoints)
        {
            // 1. Load CGAL lib
            var cgal = LibraryImport.SelectCGAL();

            // 2. Create CGAL points
            CGAL.CGAL_Point targetPoint = new CGAL.CGAL_Point(point.X, point.Y, point.Z);
            CGAL.CGAL_Point[] meshPoints = new CGAL.CGAL_Point[meshByPoints.Count];
            int index = 0;
            foreach (var currentPoint in meshByPoints)
            {
                meshPoints[index] = new CGAL.CGAL_Point(currentPoint.X, currentPoint.Y, currentPoint.Z);
                index++;
            }

            // 3. Calculate
            var jsonResult = string.Empty;
            var result = cgal.IsBelongToMesh(targetPoint, meshPoints, meshByPoints.Count, str => { jsonResult = str; });

            if (string.IsNullOrEmpty(jsonResult) || result == false)
                throw new System.Exception("CGAL lib is fail! IsBelongToMesh().");

            if (jsonResult == "VERTEX") 
            { 
                return LocationType.Vertex; 
            }
            else if (jsonResult == "EDGE")
            {
                return LocationType.Edge;
            }
            else if (jsonResult == "FACET")
            {
                return LocationType.Facet;
            }
            else if (jsonResult == "CELL") 
            { 
                return LocationType.Cell; 
            }
            else if (jsonResult == "OUTSIDE_CONVEX_HULL")
            {
                return LocationType.OutsideConvexHull;
            }
            else if (jsonResult == "OUTSIDE_AFFINE_HULL")
            { 
                return LocationType.OutsideAffineHull;
            }
            else
            {
                throw new System.Exception("CGAL lib is fail! IsBelongToMesh().");
            }
        }

        /// <summary>
        /// The function checks if the parameter belongs to natural coordinates
        /// </summary>
        /// <param name="u">Parameter</param>
        /// <returns>true if parameter belong to natural coordinates; otherwise return - false</returns>
        public static bool IsNaturalCoordinate(float u)
        {
            if (u < -1 || u > 1)
                return false;

            return true;
        }

        /// <summary>
        /// The function checks if the parameters belongs to natural coordinates
        /// </summary>
        /// <param name="u">First parameter</param>
        /// <param name="v">Second parameter</param>
        /// <returns>true if parameters belong to natural coordinates; otherwise return - false</returns>
        public static bool IsNaturalCoordinate(float u, float v)
        {
            if (!IsNaturalCoordinate(u) || 
                !IsNaturalCoordinate(v))
                return false;
            return true;
        }

        /// <summary>
        /// The function checks if the parameters belongs to natural coordinates
        /// </summary>
        /// <param name="u">First parameter</param>
        /// <param name="v">Second parameter</param>
        /// <param name="w">Third parametr</param>
        /// <returns>true if parameters belong to natural coordinates; otherwise return - false</returns>
        public static bool IsNaturalCoordinate(float u, float v, float w)
        {
            if (!IsNaturalCoordinate(u) || 
                !IsNaturalCoordinate(v) || 
                !IsNaturalCoordinate(w))
                return false;
            return true;
        }

    }
}
