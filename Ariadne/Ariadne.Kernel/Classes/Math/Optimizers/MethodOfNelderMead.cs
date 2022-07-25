using System;

namespace Ariadne.Kernel.Math.Optimizers
{
    /// <summary>
    /// The Nelder–Mead method (also downhill simplex method, amoeba method, or polytope method)
    /// is a commonly applied numerical method used to find the minimum or maximum of an objective function 
    /// in a multidimensional space. 
    /// 
    /// It is a direct search method (based on function comparison) and is often applied to nonlinear optimization problems
    /// for which derivatives may not be known. 
    /// 
    /// However, the Nelder–Mead technique is a heuristic search method that can converge to non-stationary points
    /// on problems that can be solved by alternative methods.
    /// </summary>
    public class MethodOfNelderMead : Optimizer2D, IIterativeOptimizer
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
        /// Search interval.
        /// </summary>
        private (float Lower, float Upper) _interval = (0, 0);

        /// <summary>
        /// Demension.
        /// </summary>
        private const uint _N = 2;

        /// <summary>
        /// The golden section search.
        /// </summary>
        /// <param name="function">Optimization function.</param>
        /// <param name="interval">Search interval.</param>
        /// <param name="tolerance">Tolerance.</param>
        /// <param name="maxIterations">Maximum number of iterations.</param>
        public MethodOfNelderMead(Func<float, float, float> function, (float Lower, float Upper) interval, float tolerance = Utils.LinearTolerance, uint maxIterations = 100000)
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
        protected override float[] Optimize(bool isMax)
        {
            var N = _N;
            Random rnd = new Random();
            float[][] simplex = new float[N + 1][];

            // Generate N + 1 initial arrays.
            for (int array = 0; array <= N; array++)
            {
                simplex[array] = new float[N];
                for (int index = 0; index < N; index++)
                {
                    simplex[array][index] = GetRandomInInterval(rnd);
                }
            }

            // Init centroid
            float[] centroid = GetCentroid(simplex);

            const float alpha = 1f;
            const float gamma = 2f;
            const float rho = 0.5f;
            const float sigma = 0.5f;

            // Infinite loop until convergence
            while (true)
            {
                // Evaluation
                float[] functionValues = new float[N + 1];
                int[] indices = new int[N + 1];
                for (int vertex_of_simplex = 0; vertex_of_simplex <= N; vertex_of_simplex++)
                {
                    functionValues[vertex_of_simplex] = Function(simplex[vertex_of_simplex]);
                    indices[vertex_of_simplex] = vertex_of_simplex;
                }

                // Order
                Array.Sort(functionValues, indices);

                // Check for convergence
                if (System.Math.Abs(Function(centroid) - functionValues[0]) < _eps)
                {
                    break;
                }

                // Find centroid of the simplex excluding the vertex with highest function value.
                for (int index = 0; index < N; index++)
                {
                    centroid[index] = 0;
                    for (int vertex_of_simplex = 0; vertex_of_simplex <= N; vertex_of_simplex++)
                    {
                        if (vertex_of_simplex != indices[N])
                        {
                            centroid[index] += simplex[vertex_of_simplex][index] / N;
                        }
                    }
                }

                //Reflection
                float[] reflection_point = new float[N];
                for (int index = 0; index < N; index++)
                {
                    reflection_point[index] = centroid[index] + alpha * (centroid[index] - simplex[indices[N]][index]);
                }

                double reflection_value = Function(reflection_point);

                if (reflection_value >= functionValues[0] & reflection_value < functionValues[N - 1])
                {
                    simplex[indices[N]] = reflection_point;
                    continue;
                }

                // Expansion
                if (reflection_value < functionValues[0])
                {
                    float[] expansion_point = new float[N];
                    for (int index = 0; index < N; index++)
                    {
                        expansion_point[index] = centroid[index] + gamma * (reflection_point[index] - centroid[index]);
                    }
                    float expansion_value = Function(expansion_point);

                    if (expansion_value < reflection_value)
                    {
                        simplex[indices[N]] = expansion_point;
                    }
                    else
                    {
                        simplex[indices[N]] = reflection_point;
                    }
                    continue;
                }

                // Contraction
                float[] contraction_point = new float[N];
                for (int index = 0; index < N; index++)
                {
                    contraction_point[index] = centroid[index] + rho * (simplex[indices[N]][index] - centroid[index]);
                }

                float contraction_value = Function(contraction_point);

                if (contraction_value < functionValues[N])
                {
                    simplex[indices[N]] = contraction_point;
                    continue;
                }

                //Shrink
                float[] best_point = simplex[indices[0]];
                for (int vertex_of_simplex = 0; vertex_of_simplex <= N; vertex_of_simplex++)
                {
                    for (int index = 0; index < N; index++)
                    {
                        simplex[vertex_of_simplex][index] = best_point[index] + sigma * (simplex[vertex_of_simplex][index] - best_point[index]);
                    }
                }

                if (_maxIterations <= 0)
                    return new float[] { float.NaN, float.NaN};
                _maxIterations--;
            }

            return centroid;
        }

        /// <summary>
        /// Get number of maximum allowable iterations.
        /// </summary>
        /// <returns>Count of iterations.</returns>
        public uint GetCountIterations()
        {
            return _maxIterations;
        }

        /// <summary>
        /// Optimization function.
        /// </summary>
        /// <param name="values">Values.</param>
        /// <returns>Result.</returns>
        private float Function(float[] values)
        {
            if (values.Length != 2)
                throw new ArgumentException("Array count > 2");
            return _f(values[0], values[1]);
        }

        /// <summary>
        /// Get centroid of current simplex.
        /// </summary>
        /// <param name="simplex">Simplex.</param>
        /// <returns>Centroid.</returns>
        private float[] GetCentroid(float[][] simplex)
        {
            var N = _N;
            var centroid = new float[N];
            for (int index = 0; index < N; index++)
            {
                centroid[index] = 0;
                for (int vertex_of_simplex = 0; vertex_of_simplex <= N; vertex_of_simplex++)
                {
                    centroid[index] += simplex[vertex_of_simplex][index] / N;
                }
            }

            return centroid;
        }

        /// <summary>
        /// Get random value in interval.
        /// </summary>
        /// <param name="rnd">Random object.</param>
        /// <returns>Random value.</returns>
        private float GetRandomInInterval(Random rnd)
        {
            var mu1 = _interval.Lower;
            var mu2 = _interval.Upper;
            var l = (float)rnd.NextDouble();
            var l1 = 0.0f;
            var l2 = 1.0f;

            return (l * (mu1 - mu2) + l1 * mu2 - l2 * mu1) / (l1 - l2);
        }
    }
}
