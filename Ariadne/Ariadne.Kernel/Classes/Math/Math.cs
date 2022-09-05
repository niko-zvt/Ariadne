using Ariadne.Kernel.Libs;
using System;
using System.Text.Json;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Optimization;

namespace Ariadne.Kernel.Math
{
    /// <summary>
    /// Static class for implementing a set of general mathematical functions
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Global linear tolerance
        /// </summary>
        public const float LinearTolerance = 1e-6f;

        /// <summary>
        /// Global angular tolerance
        /// </summary>
        public const float AngularTolerance = 0.001f;

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
        /// Intersection type
        /// </summary>
        public enum IntersectionType
        {
            Null,
            Point,
            Segment,
            Line
        }

        /// <summary>
        /// Dimension type
        /// </summary>
        public enum DimensionType
        {
            D0 = 0,
            D1 = 1,
            D2 = 2,
            D3 = 3,
            D4 = 4,
            DN = int.MaxValue
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
            var cgal = LibraryImport.SelectCGAL();
            var result = cgal.CGAL_IsBelongToGrid(point, meshByPoints);
            return result;
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

        /// <summary>
        /// Calculate point on Legendre-Gauss Interval
        /// </summary>
        /// <param name="point">Point in 3D space</param>
        /// <param name="startPoint">Start point in 3D space</param>
        /// <param name="endPoint">End point in 3D space</param>
        /// <returns>Point on Legendre-Gauss interval</returns>
        public static Vector3D CalculatePointOnLegendreGaussInterval(Vector3D point, Vector3D startPoint, Vector3D endPoint)
        {
            Vector3D result = new Vector3D();
            result.X = CalculatePointOnLegendreGaussInterval(point.X, startPoint.X, endPoint.X);
            result.Y = CalculatePointOnLegendreGaussInterval(point.Y, startPoint.Y, endPoint.Y);
            result.Z = CalculatePointOnLegendreGaussInterval(point.Z, startPoint.Z, endPoint.Z);
            return result;
        }

        /// <summary>
        /// Calculate value on Legendre-Gauss interval
        /// </summary>
        /// <param name="x">Value</param>
        /// <param name="xmin">Min value</param>
        /// <param name="xmax">Max value</param>
        /// <returns>Value on Legendre-Gauss interval</returns>
        public static float CalculatePointOnLegendreGaussInterval(float x, float xmin, float xmax)
        {
            var k = (x - xmin) / (xmax - xmin);
            return 2 * k - 1;
        }

        /// <summary>
        /// Calculate angle between vectors
        /// </summary>
        /// <param name="v1">First vector</param>
        /// <param name="v2">Second vector</param>
        /// <returns>Angle between vectors</returns>
        public static float CalculateAngleBetweenVectors(Vector3D v1, Vector3D v2)
        {
            var cos = (v1.DotProduct(v2)) / (v1.Length * v2.Length);
            var angle = System.Math.Acos(cos);
            return (float)angle;
        }

        /// <summary>
        /// Calculate the intersection of two segments.
        /// </summary>
        /// <param name="A1">Start point of the first segment</param>
        /// <param name="A2">End point of the first segment</param>
        /// <param name="B1">Start point of the second segment</param>
        /// <param name="B2">End point of the second segment</param>
        /// <param name="intersectionPoints">List of intersection points</param>
        /// <returns>Intersection type</returns>
        public static IntersectionType CalculateIntersectionOfTwoSegments(Vector3D A1, Vector3D A2, Vector3D B1, Vector3D B2, out List<Vector3D> intersectionPoints)
        {
            var result = LibraryImport.SelectCGAL().CGAL_GetIntersectionOfTwoSegments(A1, A2, B1, B2, out intersectionPoints);
            return result;
        }

        /// <summary>
        /// Calculate the intersection of two lines.
        /// </summary>
        /// <param name="A1">Start point of the first line</param>
        /// <param name="A2">End point of the first line</param>
        /// <param name="B1">Start point of the second line</param>
        /// <param name="B2">End point of the second line</param>
        /// <param name="intersectionPoints">List of intersection points</param>
        /// <returns>Intersection type</returns>
        public static IntersectionType CalculateIntersectionOfTwoLines(Vector3D A1, Vector3D A2, Vector3D B1, Vector3D B2, out List<Vector3D> intersectionPoints)
        {
            var result = LibraryImport.SelectCGAL().CGAL_GetIntersectionOfTwoLines(A1, A2, B1, B2, out intersectionPoints);
            return result;
        }

        public static bool CalculateTranformationOfPoint(Vector3D point, CoordinateSystem sourceCS, CoordinateSystem targetCS, out Vector3D tPoint)
        {
            var result = LibraryImport.SelectCGAL().CGAL_TransformPoint(point, sourceCS, targetCS, out tPoint);
            return result;
        }

        /// <summary>
        /// Calculate intersection of bisector and base.
        /// </summary>
        /// <param name="vertex">Vertex of bisector</param>
        /// <param name="startBase">Start point of the base</param>
        /// <param name="endBase">End point of the base</param>
        /// <returns>Intersect point or NULL.</returns>
        public static Vector3D CalculateIntersectionOfBisectorAndBase(Vector3D vertex, Vector3D startBase, Vector3D endBase)
        {
            // (A) - Vertex 
            // (B) - Start of base
            // (C) - End of base
            // (D) - Intersection Point
            //
            // AD - Bisector
            // BC - Base
            //
            //                  (A)
            //                 / | \
            //                /  |  \
            //               /   |   \
            //              /    |    \
            //            (B)___(D)___(C)
            //

            var AB = (startBase - vertex).Length;
            var AC = (endBase - vertex).Length;
            var CB = (endBase - startBase).Length;

            var k = 1 + (AC / AB);
            var BD = CB / k;
            var CD = (BD * AC) / AB;

            var error = System.Math.Abs(CB - (BD + CD));
            if (error > LinearTolerance)
                throw new System.ArithmeticException("The intersection of bisector and base is incorrect!");

            var weight = BD / CB;
            if (weight < 0 || weight > 1)
                throw new System.ArithmeticException("The weight of the base is incorrect!");

            return startBase * (1 - weight) + endBase * weight;
        }

        public static HValue CalculateDeterminant(HValue[,] m)
        {
            var length = m.Length;
            var rank = m.Rank;

            if (rank != 2)
                throw new System.ArgumentException("Array rank != 2!");

            var isSquare = m.GetLength(0) == m.GetLength(1);

            if (length == 4 && isSquare)
            {
                return m[0, 0] * m[1, 1] - m[0, 1] * m[1, 0];
            }
            else if (length == 9 && isSquare)
            {
                return m[0, 0] * m[1, 1] * m[2, 2] - m[0, 0] * m[1, 2] * m[2, 1] - m[0, 1] * m[1, 0] * m[2, 2] + m[0, 1] * m[1, 2] * m[2, 0] + m[0, 2] * m[1, 0] * m[2, 1] - m[0, 2] * m[1, 1] * m[2, 0];
            }
            else if (length == 16 && isSquare)
            {
                return  m[0, 0] * m[1, 1] * m[2, 2] * m[3, 3] - m[0, 0] * m[1, 1] * m[2, 3] * m[3, 2] - m[0, 0] * m[1, 2] * m[2, 1] * m[3, 3]
                    + m[0, 0] * m[1, 2] * m[2, 3] * m[3, 1] + m[0, 0] * m[1, 3] * m[2, 1] * m[3, 2] - m[0, 0] * m[1, 3] * m[2, 2] * m[3, 1]
                    - m[0, 1] * m[1, 0] * m[2, 2] * m[3, 3] + m[0, 1] * m[1, 0] * m[2, 3] * m[3, 2] + m[0, 1] * m[1, 2] * m[2, 0] * m[3, 3]
                    - m[0, 1] * m[1, 2] * m[2, 3] * m[3, 0] - m[0, 1] * m[1, 3] * m[2, 0] * m[3, 2] + m[0, 1] * m[1, 3] * m[2, 2] * m[3, 0]
                    + m[0, 2] * m[1, 0] * m[2, 1] * m[3, 3] - m[0, 2] * m[1, 0] * m[2, 3] * m[3, 1] - m[0, 2] * m[1, 1] * m[2, 0] * m[3, 3]
                    + m[0, 2] * m[1, 1] * m[2, 3] * m[3, 0] + m[0, 2] * m[1, 3] * m[2, 0] * m[3, 1] - m[0, 2] * m[1, 3] * m[2, 1] * m[3, 0]
                    - m[0, 3] * m[1, 0] * m[2, 1] * m[3, 2] + m[0, 3] * m[1, 0] * m[2, 2] * m[3, 1] + m[0, 3] * m[1, 1] * m[2, 0] * m[3, 2]
                    - m[0, 3] * m[1, 1] * m[2, 2] * m[3, 0] - m[0, 3] * m[1, 2] * m[2, 0] * m[3, 1] + m[0, 3] * m[1, 2] * m[2, 1] * m[3, 0];
            }
            else
            {
                throw new System.IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Method of checking the float number is NaN.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>true - if value is NaN, overwise - false.</returns>
        public static unsafe bool IsNaN(float f)
        {
            int binary = *(int*)(&f);
            return ((binary & 0x7F800000) == 0x7F800000) && ((binary & 0x007FFFFF) != 0);
        }
    }
}
