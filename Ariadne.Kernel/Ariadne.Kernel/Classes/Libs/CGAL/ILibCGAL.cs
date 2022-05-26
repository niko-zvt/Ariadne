using Ariadne.Kernel.CGAL;
using System.Runtime.InteropServices;

namespace Ariadne.Kernel.Libs
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public delegate void Notification(string value);

    public interface ILibCGAL : ILibraryImport
    {
        bool GetOOBB(CGAL_Point[] points, int size, Notification notification);
    }
}