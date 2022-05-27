using Ariadne.Kernel.CGAL;
using System;
using System.IO;

namespace Ariadne.Kernel.Libs
{
    public static class LibraryImport
    {
        private const string pathToLibs =  "Libs\\";
        public static ILibCGAL SelectCGAL()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory + pathToLibs);
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
