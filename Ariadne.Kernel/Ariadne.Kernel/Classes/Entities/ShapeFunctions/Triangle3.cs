using Ariadne.Kernel.Math;

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

        private Triangle3()
        {
            _localFunctors.Add(new Functor(LocalN1));
            _localFunctors.Add(new Functor(LocalN2));
            _localFunctors.Add(new Functor(LocalN3));
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
        /// Returns the shape function type
        /// </summary>
        /// <returns>Shape function type</returns>
        public override ShapeFunctionType GetShapeFunctionType()
        {
            return ShapeFunctionType.Triangle_3;
        }
    }
}