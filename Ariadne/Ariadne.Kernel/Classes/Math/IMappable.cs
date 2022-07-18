namespace Ariadne.Kernel.Math
{
    public interface IMappable
    {
        public AffineMap3D GetMapToGlobalCS();

        public AffineMap3D GetMapToLocalCS();

        //public object GetMapTo(CoordinateSystem coordinateSystem);
    }
}
