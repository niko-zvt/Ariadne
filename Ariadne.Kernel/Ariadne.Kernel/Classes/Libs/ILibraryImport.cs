using System.Runtime.InteropServices;

namespace Ariadne.Kernel.CGAL
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public delegate void Notification(string value);

    public interface ILibraryImport
    {
        int GetOptimalOrientedBoundingBox(int start, int count, Notification notification);
    }
}
