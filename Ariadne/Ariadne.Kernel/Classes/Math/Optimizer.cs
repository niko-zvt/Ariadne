using System;

namespace Ariadne.Kernel.Math
{
    public abstract class Optimizer
    {
        protected uint _maxIterations = 0;
        public abstract void FindMin(out float[] result);
    }
}
