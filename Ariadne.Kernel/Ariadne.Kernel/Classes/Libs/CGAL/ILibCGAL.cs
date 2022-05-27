using Ariadne.Kernel.CGAL;
using System.Runtime.InteropServices;

namespace Ariadne.Kernel.Libs
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public delegate void Notification(string value);

    public interface ILibCGAL : ILibraryImport
    {
        bool GetOOBB(CGAL_Point[] points, int size, Notification notification);
        bool GetAABB(CGAL_Point[] points, int size, Notification notification);

        bool IsBelongToMesh(CGAL_Point point, CGAL_Point[] meshPoints, int size, Notification notification);
    }
}