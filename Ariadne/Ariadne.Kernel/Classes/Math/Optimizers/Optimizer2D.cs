using System;

namespace Ariadne.Kernel.Math
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

    /// <summary>
    /// Coordinate descent is an optimization algorithm that successively minimizes along coordinate directions to find the minimum of a function.
    /// </summary>
    public class CoordinateDescentAlgorithm : Optimizer2D, IIterativeOptimizer
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

        private (float x, float y) _initApprox = (0, 0);

        /// <summary>
        /// Coordinate descent algorithm
        /// </summary>
        /// <param name="function">Optimization function.</param>
        /// <param name="tolerance">Tolerance.</param>
        /// <param name="maxIterations">Maximum number of iterations.</param>
        public CoordinateDescentAlgorithm(Func<float, float, float> function, (float x, float y) initialApprox, float tolerance = Utils.LinearTolerance, uint maxIterations = 100000)
        {
            _f = function;
            _maxIterations = maxIterations;
            _initApprox = initialApprox;
        }

        protected override float[] Optimize(bool isMax)
        {
            var x = new float[] { _initApprox.x, _initApprox.y };
            var result = false;
            var stopCondition = _f(x[0], x[1]);

            for (uint currentIteration = _maxIterations; currentIteration > 0; currentIteration--)
            {
                // Repeat for each dimension (2D)
                for (uint i = 0; i < 2; i++)
                {
                    // 1. Fix the values ​​of all variables except x_i
                    Func<float, float> f = param =>
                    {
                        if (i == 0)
                            return _f(param, x[1]);
                        else
                            return _f(x[0], param);
                    };

                    // 2. Carry out one-dimensional optimization on the variable x_i,
                    // by any one-dimensional optimization method
                    var optimizer = new GoldenSectionSearch(f, (-1, 1));
                    var results = new float[] { };
                    result = isMax ? optimizer.FindMax(out results) : optimizer.FindMin(out results);
                    if (result is not true)
                        break;

                    x[i] = results[0];

                    // 3. Check the termination criterion is met (state options below),
                    // then return the current values
                    stopCondition = _f(x[0], x[1]);
                    if (stopCondition < _eps)
                    {
                        return x;
                    }
                }

                if (result is not true)
                    break;
            }

            return new float[] { float.NaN, float.NaN };
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
