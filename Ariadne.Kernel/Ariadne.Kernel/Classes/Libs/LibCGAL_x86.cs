using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Ariadne.Kernel.CGAL
{
    internal class LibCGAL_x86 : ILibraryImport
    {
        [DllImport("Ariadne.CGAL.x86", CallingConvention = CallingConvention.StdCall, ExactSpelling = false, EntryPoint = "_GetOptimalOrientedBoundingBox@12")]
        private static extern int GetOptimalOrientedBoundingBox(CGAL_Point[] points, int size);

        public int GetOOBB(CGAL_Point[] points, int size)
        {
            return GetOptimalOrientedBoundingBox(points, size);
        }
    }
}

