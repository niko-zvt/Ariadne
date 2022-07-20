using System;

namespace Ariadne.Kernel.Math
{
    public class Optimizer2D : Optimizer
    {
        private Func<float, float, float> _f;

        public Optimizer2D(Func<float, float, float> function)
        {
            _maxIterations = 0;
            _f = function;
        }

        public override void FindMin(out float[] result)
        {
            throw new NotImplementedException();
        }
    }
}
