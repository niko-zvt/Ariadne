// Copyright 2022 Nikolay V. Zhivotenko
// Licensed under the Apache License, Version 2.0
// E-mail: niko.zvt@gmail.com

using Ariadne.Kernel.Math;
using System.Collections.Generic;

namespace Ariadne.Kernel
{
    /// <summary>
    /// Class representing 3-node triangle shape function
    /// </summary>
    sealed class Triangle3 : ShapeFunction
    {
        /// <summary>
        /// Lazy instance holder
        /// </summary>
        private static readonly System.Lazy<Triangle3> instanceHolder = new System.Lazy<Triangle3>(() => new Triangle3());

        /// <summary>
        /// Default constructor
        /// </summary>
        private Triangle3()
        {
            _localFunctors.Add(new ShapeFunctor(LocalN1));
            _localFunctors.Add(new ShapeFunctor(LocalN2));
            _localFunctors.Add(new ShapeFunctor(LocalN3));
            _globalFunctor = new GlobalShapeFunctor(FindUVCoords);
        }

        /// <summary>
        /// Instance of Triangle3 shape function
        /// </summary>
        public static Triangle3 GetFunction
        {
            get { return instanceHolder.Value; }
        }

        /// <summary>
        /// First shape function
        /// </summary>
        /// <param name="u">Parameter U</param>
        /// <param name="v">Parameter V</param>
        /// <returns>Value of shape function</returns>
        public float LocalN1(float u, float v, float w = 0.0f)
        {
            if (Math.Utils.IsNaturalCoordinate(u, v) &&
                u >= 0 && v >= 0 && (u + v) <= 1)
                return u;

            return float.NaN;
        }

        /// <summary>
        /// Second shape function
        /// </summary>
        /// <param name="u">Parameter U</param>
        /// <param name="v">Parameter V</param>
        /// <returns>Value of shape function</returns>
        public float LocalN2(float u, float v, float w = 0.0f)
        {
            if (Math.Utils.IsNaturalCoordinate(u, v) && 
                u >= 0 && v >= 0 && (u + v)<= 1 )
                return 1.0f - u - v;

            return float.NaN;
        }

        /// <summary>
        /// Third shape function
        /// </summary>
        /// <param name="u">Parameter U</param>
        /// <param name="v">Parameter V</param>
        /// <returns>Value of shape function</returns>
        public float LocalN3(float u, float v, float w = 0.0f)
        {
            if (Math.Utils.IsNaturalCoordinate(u, v) &&
                u >= 0 && v >= 0 && (u + v) <= 1)
                return v;

            return float.NaN;
        }

        /// <summary>
        /// Find the UV-coords by solving the two-dimensional optimization problem.
        /// </summary>
        /// <param name="point">Target point in XYZ-space.</param>
        /// <param name="nodalCoords">Nodal coordinates from a specific CQUAD4 element.</param>
        /// <returns>UV-coords.</returns>
        private Vector3D FindUVCoords(Vector3D point, List<Vector3D> nodalCoords)
        {
            // 1. Check size
            if (nodalCoords.Count != Size)
                throw new System.ArgumentOutOfRangeException("In triangle3 shape function count of nodes != 3");

            // 2. Find optimum UV-coords
            var uvw = new Vector3D();
            var area = GetArea(nodalCoords[0], nodalCoords[1], nodalCoords[2]);
            var area12P = GetArea(nodalCoords[0], nodalCoords[1], point);
            var area13P = GetArea(nodalCoords[0], nodalCoords[2], point);
            var area23P = GetArea(nodalCoords[1], nodalCoords[2], point);
            uvw.X = area23P / area;
            uvw.Y = area12P / area;
            uvw.Z = area13P / area;

            return uvw;
        }

        /// <summary>
        /// Returns the shape function type
        /// </summary>
        /// <returns>Shape function type</returns>
        public override ShapeFunctionType GetShapeFunctionType()
        {
            return ShapeFunctionType.Triangle_3;
        }

        /// <summary>
        /// Get area of triangle.
        /// </summary>
        /// <param name="v1">First vertex.</param>
        /// <param name="v2">Second vertex.</param>
        /// <param name="v3">Third vertex.</param>
        /// <returns>Area.</returns>
        private float GetArea(Vector3D v1, Vector3D v2, Vector3D v3)
        {
            var a = (v3 - v2).Length;
            var b = (v2 - v1).Length;
            var c = (v3 - v1).Length;
            var p = 0.5f * (a + b + c);
            var area = (float)System.Math.Sqrt(p * (p - a) * (p - b) * (p - c));
            return area;
        }
    }
}