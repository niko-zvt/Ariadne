// Copyright 2022 Nikolay V. Zhivotenko
// Licensed under the Apache License, Version 2.0
// E-mail: niko.zvt@gmail.com

using System;

namespace Ariadne.Kernel.Math.Optimizers
{
    /// <summary>
    /// An abstract class representing optimizers of functions of a single variable.
    /// </summary>
    public abstract class Optimizer1D : Optimizer
    {
        /// <summary>
        /// Optimization function.
        /// </summary>
        protected Func<float, float> _f = null;

        /// <summary>
        /// Optimize method. 
        /// </summary>
        /// <param name="isMax">True - if you need to find the maximum, otherwise the minimum.</param>
        /// <returns>Optimum coords.</returns>
        protected abstract float Optimize(bool isMax);

        /// <summary>
        /// Find min value.
        /// </summary>
        /// <param name="results">Coords of min value.</param>
        /// <returns>True - if success, otherwise - false.</returns>
        public override bool FindMin(out float[] results)
        {
            results = new float[] { };

            var value = Optimize(false);

            if (value == float.NaN)
                return false;

            results = new float[] { value };
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

            var value = Optimize(true);

            if (value == float.NaN)
                return false;

            results = new float[] { value };
            return true;
        }
    }
}
