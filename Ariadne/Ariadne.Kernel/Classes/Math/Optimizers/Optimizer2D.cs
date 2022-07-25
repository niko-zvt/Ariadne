using System;

namespace Ariadne.Kernel.Math.Optimizers
{
    /// <summary>
    /// An abstract class representing optimizers of functions of two variables.
    /// </summary>
    public abstract class Optimizer2D : Optimizer
    {
        /// <summary>
        /// Optimization function.
        /// </summary>
        protected Func<float, float, float> _f;

        /// <summary>
        /// Optimize method. 
        /// </summary>
        /// <param name="isMax">True - if you need to find the maximum, otherwise the minimum.</param>
        /// <returns>Optimum coords.</returns>
        protected abstract float[] Optimize(bool isMax);

        /// <summary>
        /// Find min value.
        /// </summary>
        /// <param name="results">Coords of min value.</param>
        /// <returns>True - if success, otherwise - false.</returns>
        public override bool FindMin(out float[] results)
        {
            results = new float[] { };

            var values = Optimize(false);

            foreach (var value in values)
            {
                if (value == float.NaN)
                    return false;
            }

            results = values;
            return true;
        }

        /// <summary>
        /// Find max value.
        /// </summary>
        /// <param name="results">Coords of max value.</param>
        /// <returns>True - if success, otherwise - false.</returns>
        public override bool FindMax(out float[] results)
        {
            results = new float[] { };

            var values = Optimize(true);

            foreach (var value in values)
            {
                if (value == float.NaN)
                    return false;
            }

            results = values;
            return true;
        }
    }
}
