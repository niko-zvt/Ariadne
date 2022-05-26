using System.Runtime.InteropServices;

namespace Ariadne.Kernel.CGAL
{
    /// <summary>
    /// CGAL Point3D
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CGAL_Point
    {
        public float X, Y, Z;

        public CGAL_Point(float x, float y, float z)
        {
            X = x; Y = y; Z = z;
        }
    }
}
