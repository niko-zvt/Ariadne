﻿using Ariadne.Kernel.Libs;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Ariadne.Kernel.CGAL
{
    internal class LibCGAL_x86 : ILibCGAL
    {
        [DllImport("Ariadne.CGAL.x86", CallingConvention = CallingConvention.StdCall, ExactSpelling = false, EntryPoint = "_GetOptimalOrientedBoundingBox@12")]
        private static extern int GetOptimalOrientedBoundingBox([In] CGAL_Point[] points, [In] int size, Notification notification);

        public bool GetOOBB(CGAL_Point[] points, int size, Notification notification)
        {
            int res = GetOptimalOrientedBoundingBox(points, size, notification);

            return true;
        }
    }
}
