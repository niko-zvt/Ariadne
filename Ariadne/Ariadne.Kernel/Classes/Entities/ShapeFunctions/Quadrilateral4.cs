// Copyright 2022 Nikolay V. Zhivotenko
// Licensed under the Apache License, Version 2.0
// E-mail: niko.zvt@gmail.com

using Ariadne.Kernel.Math;
using System.Collections.Generic;

namespace Ariadne.Kernel
{
    /// <summary>
    /// Class representing 4-node quadrilateral shape function.
    /// </summary>
    sealed class Quadrilateral4 : ShapeFunction
    {
        /// <summary>
        /// Lazy instance holder.
        /// </summary>
        private static readonly System.Lazy<Quadrilateral4> instanceHolder = new System.Lazy<Quadrilateral4>(() => new Quadrilateral4());

        /// <summary>
        /// Default constructor.
        /// </summary>
        private Quadrilateral4()
        {
            _localFunctors.Add(new ShapeFunctor(LocalN1));
            _localFunctors.Add(new ShapeFunctor(LocalN2));
            _localFunctors.Add(new ShapeFunctor(LocalN3));
            _localFunctors.Add(new ShapeFunctor(LocalN4));
            _globalFunctor = new GlobalShapeFunctor(FindUVWCoords);
        }

        /// <summary>
        /// Instance of Quadrilateral4 shape function.
        /// </summary>
        public static Quadrilateral4 GetFunction
        {
            get { return instanceHolder.Value; }
        }

        /// <summary>
        /// First shape function.
        /// </summary>
        /// <param name="u">Parameter U.</param>
        /// <param name="v">Parameter V.</param>
        /// <returns>Value of shape function.</returns>
        public float LocalN1(float u, float v, float w = 0.0f)
        {
            if (Math.Utils.IsNaturalCoordinate(u, v))
                return 0.25f * (1 - u) * (1 - v);

            return float.NaN;
        }

        /// <summary>
        /// Second shape function.
        /// </summary>
        /// <param name="u">Parameter U.</param>
        /// <param name="v">Parameter V.</param>
        /// <returns>Value of shape function.</returns>
        public float LocalN2(float u, float v, float w = 0.0f)
        {
            if (Math.Utils.IsNaturalCoordinate(u, v))
                return 0.25f * (1 - u) * (1 + v);

            return float.NaN;
        }

        /// <summary>
        /// Third shape function.
        /// </summary>
        /// <param name="u">Parameter U.</param>
        /// <param name="v">Parameter V.</param>
        /// <returns>Value of shape function.</returns>
        public float LocalN3(float u, float v, float w = 0.0f)
        {
            if (Math.Utils.IsNaturalCoordinate(u, v))
                return 0.25f * (1 + u) * (1 + v);

            return float.NaN;
        }

        /// <summary>
        /// Fourth shape function.
        /// </summary>
        /// <param name="u">Parameter U.</param>
        /// <param name="v">Parameter V.</param>
        /// <returns>Value of shape function.</returns>
        public float LocalN4(float u, float v, float w = 0.0f)
        {
            if (Math.Utils.IsNaturalCoordinate(u, v))
                return 0.25f * (1 + u) * (1 - v);

            return float.NaN;
        }

        /// <summary>
        /// Find the UV-coords by solving the two-dimensional optimization problem.
        /// </summary>
        /// <param name="point">Target point in XYZ-space.</param>
        /// <param name="nodalCoords">Nodal coordinates from a specific CQUAD4 element.</param>
        /// <returns>UV-coords.</returns>
        private Vector3D FindUVWCoords(Vector3D point, List<Vector3D> nodalCoords)
        {
            // 1. Check size
            if (nodalCoords.Count != Size)
                throw new System.ArgumentOutOfRangeException("In quadrilateral4 shape function count of nodes != 4");

            // 2. Preparing the optimization function
            System.Func<float, float, float> f = (u,v) =>
            {
                var uvw = new Vector3D(u, v, 0);
                var probPoint = GetPointByUVW(uvw, nodalCoords);
                return (probPoint - point).Length;
            };

            // 3. Preparing the optimizer
            var initParams = (-1, 1);
            var optimizer = new Math.Optimizers.MethodOfNelderMead(f, initParams);
            
            // 4. Find optimum UV-coords
            var result = optimizer.FindMin(out var results);
            if (result == false || results.Length != SpaceDimension)
                throw new System.ArgumentException("Optimizer fail!");

            // 5. Return UV-coords
            var uv = new Vector3D(results[0], results[1], 0);
            return uv;
        }

        /// <summary>
        /// Returns the shape function type.
        /// </summary>
        /// <returns>Shape function type.</returns>
        public override ShapeFunctionType GetShapeFunctionType()
        {
            return ShapeFunctionType.Quadrilateral_4;
        }
    }
}