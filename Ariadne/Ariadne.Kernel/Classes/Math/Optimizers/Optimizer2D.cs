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
        /// Find min value.
        /// </summary>
        /// <param name="results">Coords of min value.</param>
        /// <returns>True - if success, otherwise - false.</returns>
        public override bool FindMin(out float[] results)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Find max value.
        /// </summary>
        /// <param name="results">Coords of max value.</param>
        /// <returns>True - if success, otherwise - false.</returns>
        public override bool FindMax(out float[] results)
        {
            throw new NotImplementedException();
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
        /// Coordinate descent algorithm
        /// </summary>
        /// <param name="function">Optimization function.</param>
        /// <param name="tolerance">Tolerance.</param>
        /// <param name="maxIterations">Maximum number of iterations.</param>
        public CoordinateDescentAlgorithm(Func<float, float, float> function, float tolerance = Utils.LinearTolerance, uint maxIterations = 100000)
        {
            _f = function;
            _maxIterations = maxIterations;
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
