// Copyright 2022 Nikolay V. Zhivotenko
// Licensed under the Apache License, Version 2.0
// E-mail: niko.zvt@gmail.com

using System;
using Ariadne.Kernel;
using Ariadne.Kernel.Math;

namespace Ariadne.Kernel.Math.Optimizers
{
    /// <summary>
    /// The golden section search is a method of searching for the extremum of a real function of one variable on a given segment.
    /// The method is based on the principle of dividing the segment in the proportions of the golden section.
    /// It is one of the simplest computational methods for solving optimization problems.
    /// </summary>
    public class MethodOfGoldenSection : Optimizer1D, IIterativeOptimizer
    {
        /// <summary>
        /// The total number of iterations.
        /// In general, it is a counter of the maximum allowable number of iterations for iterative optimizers.
        /// </summary>
        protected uint _maxIterations;

        /// <summary>
        /// Tolerance.
        /// </summary>
        private float _eps = Utils.LinearTolerance;

        /// <summary>
        /// Golden ratio proportion.
        /// </summary>
        private readonly float _phi = (1 + (float)System.Math.Sqrt(5)) / 2;

        /// <summary>
        /// Search interval.
        /// </summary>
        private (float Lower, float Upper) _interval = (0, 0);

        /// <summary>
        /// The golden section search.
        /// </summary>
        /// <param name="function">Optimization function.</param>
        /// <param name="interval">Search interval.</param>
        /// <param name="tolerance">Tolerance.</param>
        /// <param name="maxIterations">Maximum number of iterations.</param>
        public MethodOfGoldenSection(Func<float, float> function, (float Lower, float Upper) interval, float tolerance = Utils.LinearTolerance, uint maxIterations = 100000)
        {
            _f = function;
            _interval = interval;
            _eps = tolerance;
            _maxIterations = maxIterations;
        }

        /// <summary>
        /// Optimize method.
        /// </summary>
        /// <returns>Optimum coords.</returns>
        protected override float Optimize(bool isMax)
        {
            var x = float.NaN;

            for (uint currentIteration = _maxIterations; currentIteration > 0; currentIteration--)
            {
                var x1 = _interval.Upper - ((_interval.Upper - _interval.Lower) / _phi);
                var x2 = _interval.Lower + ((_interval.Upper - _interval.Lower) / _phi);

                var y1 = _f(x1);
                var y2 = _f(x2);

                var check = isMax ? (y1 <= y2) : (y1 >= y2);

                if (check)
                    _interval.Lower = x1;
                else
                    _interval.Upper = x2;

                if ((float)System.Math.Abs(_interval.Upper - _interval.Lower) < _eps)
                {
                    x = (_interval.Lower + _interval.Upper) / 2;
                    break;
                }
            }

            return x;
        }

        /// <summary>
        /// Get number of maximum allowable iterations.
        /// </summary>
        /// <returns>Count of iterations.</returns>
        public uint GetCountIterations()
        {
            return _maxIterations;
        }
    }
}
