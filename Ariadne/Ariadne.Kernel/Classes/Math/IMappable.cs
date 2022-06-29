namespace Ariadne.Kernel.Math
{
    public interface IMappable
    {
        public AffineMap GetMapToGlobalCS();

        public object GetMapTo(CoordinateSystem coordinateSystem);
    }
}
