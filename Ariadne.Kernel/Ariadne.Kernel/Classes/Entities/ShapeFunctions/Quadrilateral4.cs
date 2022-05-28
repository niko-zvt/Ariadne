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

        private Quadrilateral4()
        {
            _functors.Add(new Functor(N1));
            _functors.Add(new Functor(N2));
            _functors.Add(new Functor(N3));
            _functors.Add(new Functor(N4));
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
        public float N1(float u, float v, float w = 0.0f)
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
        public float N2(float u, float v, float w = 0.0f)
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
        public float N3(float u, float v, float w = 0.0f)
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
        public float N4(float u, float v, float w = 0.0f)
        {
            if (Math.Utils.IsNaturalCoordinate(u, v))
                return 0.25f * (1 + u) * (1 - v);

            return float.NaN;
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