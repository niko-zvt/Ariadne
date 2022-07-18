using Ariadne.Kernel.Math;
using System.Collections.Generic;

namespace Ariadne.Kernel
{
    /// <summary>
    /// Class representing 4-node quadrilateral shape function
    /// </summary>
    sealed class Quadrilateral4 : ShapeFunction
    {
        /// <summary>
        /// Lazy instance holder
        /// </summary>
        private static readonly System.Lazy<Quadrilateral4> instanceHolder = new System.Lazy<Quadrilateral4>(() => new Quadrilateral4());

        /// <summary>
        /// Default constructor
        /// </summary>
        private Quadrilateral4()
        {
            _localFunctors.Add(new Functor(LocalN1));
            _localFunctors.Add(new Functor(LocalN2));
            _localFunctors.Add(new Functor(LocalN3));
            _localFunctors.Add(new Functor(LocalN4));
            _globalFunctor = new GlobalFunctor(CalculateUV);
        }

        /// <summary>
        /// Instance of Quadrilateral4 shape function
        /// </summary>
        public static Quadrilateral4 GetFunction
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
            if (Math.Utils.IsNaturalCoordinate(u, v))
                return 0.25f * (1 - u) * (1 - v);

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
            if (Math.Utils.IsNaturalCoordinate(u, v))
                return 0.25f * (1 - u) * (1 + v);

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
            if (Math.Utils.IsNaturalCoordinate(u, v))
                return 0.25f * (1 + u) * (1 + v);

            return float.NaN;
        }

        /// <summary>
        /// Fourth shape function
        /// </summary>
        /// <param name="u">Parameter U</param>
        /// <param name="v">Parameter V</param>
        /// <returns>Value of shape function</returns>
        public float LocalN4(float u, float v, float w = 0.0f)
        {
            if (Math.Utils.IsNaturalCoordinate(u, v))
                return 0.25f * (1 + u) * (1 - v);

            return float.NaN;
        }

        public Vector3D CalculateUV(Vector3D point, NodeSet nodes)
        {
            // TODO: Calculate UV from
            // x = N1*x1 + N2*x2 + N3*x3 + N4*x4;
            // y = N1*y1 + N2*y2 + N3*y3 + N4*y4;
            // z = 0;
            // Ni = 1/4 * (1+r*ri) * (1+s*si);
            // Newton-Raphson method

            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Returns the shape function type
        /// </summary>
        /// <returns>Shape function type</returns>
        public override ShapeFunctionType GetShapeFunctionType()
        {
            return ShapeFunctionType.Quadrilateral_4;
        }
    }
}