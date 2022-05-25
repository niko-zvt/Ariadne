﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Ariadne.Kernel.CGAL
{
    internal class LibCGAL_x64 : ILibraryImport
    {
        [DllImport("Ariadne.CGAL.x64", CallingConvention = CallingConvention.StdCall, ExactSpelling = false, EntryPoint = "GetOptimalOrientedBoundingBox")]
        private static extern int GetOOBB(int start, int count, Notification notification);

        public int GetOptimalOrientedBoundingBox(int start, int count, Notification notification)
        {
            return GetOOBB(start, count, notification);
        }
    }
}
