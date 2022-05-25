using System;

namespace Ariadne.Kernel.CGAL
{
    public static class LibraryImport
    {
        public static ILibraryImport Select()
        {
            if (IntPtr.Size == 4) // 32-bit application
            {
                return new LibCGAL_x86();
            }
            else // 64-bit application
            {
                return new LibCGAL_x64();
            }
        }
    }
}
