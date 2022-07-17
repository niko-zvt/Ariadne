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
            _globalFunctor = new GlobalFunctor(GlobalShape);
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

        public Vector3D GlobalShape(Vector3D point, MatrixNxM coeffs)
        {
            var sizeA = coeffs.GetSize()[0];
            var sizeB = coeffs.GetSize()[1];
            if (sizeA != 6 || sizeB != 3)
                throw new System.ArgumentOutOfRangeException("Matrix is not a 6x2!");

            // TODO: Check calculation by Maple!
            //throw new System.ArithmeticException("ERROR!");

            var nX = (coeffs.GetValueAt(0, 0) * 1) + 
                     (coeffs.GetValueAt(1, 0) * point.X) + 
                     (coeffs.GetValueAt(2, 0) * point.Y) + 
                     (coeffs.GetValueAt(3, 0) * point.X * point.X) +
                     (coeffs.GetValueAt(4, 0) * point.X * point.Y) +
                     (coeffs.GetValueAt(5, 0) * point.Y * point.Y);

            var nY = (coeffs.GetValueAt(0, 1) * 1) +
                     (coeffs.GetValueAt(1, 1) * point.X) +
                     (coeffs.GetValueAt(2, 1) * point.Y) +
                     (coeffs.GetValueAt(3, 1) * point.X * point.X) +
                     (coeffs.GetValueAt(4, 1) * point.X * point.Y) +
                     (coeffs.GetValueAt(5, 1) * point.Y * point.Y);

            var nZ = 0;

            return new Vector3D(nX, nY, nZ);
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