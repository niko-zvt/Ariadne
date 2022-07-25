namespace Ariadne.Kernel.Math.Optimizers
{
    /// <summary>
    /// An abstract class representing any optimizers of functions.
    /// </summary>
    public abstract class Optimizer
    {
        /// <summary>
        /// Find min value.
        /// </summary>
        /// <param name="results">Coords of min value.</param>
        /// <returns>True - if success, otherwise - false.</returns>
        public abstract bool FindMin(out float[] results);

        /// <summary>
        /// Find max value.
        /// </summary>
        /// <param name="results">Coords of max value.</param>
        /// <returns>True - if success, otherwise - false.</returns>
        public abstract bool FindMax(out float[] results);
    }

    /// <summary>
    /// Iterative optimizer interface.
    /// </summary>
    public interface IIterativeOptimizer
    {
        /// <summary>
        /// Get number of maximum allowable iterations.
        /// </summary>
        /// <returns>Count of iterations.</returns>
        public abstract uint GetCountIterations();
    }
}
