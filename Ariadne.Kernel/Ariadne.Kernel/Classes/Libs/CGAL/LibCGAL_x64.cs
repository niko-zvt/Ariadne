using Ariadne.Kernel.Libs;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Ariadne.Kernel.CGAL
{
    internal class LibCGAL_x64 : ILibCGAL
    {
        [DllImport("Ariadne.CGAL.x64", CallingConvention = CallingConvention.StdCall, ExactSpelling = false, EntryPoint = "GetOptimalOrientedBoundingBox")]
        private static extern int GetOptimalOrientedBoundingBox([In] CGAL_Point[] points, [In] int size, Notification notification);
        public bool GetOOBB(CGAL_Point[] points, int size, Notification notification)
        {
            int res = GetOptimalOrientedBoundingBox(points, size, notification);

            if (res != 0)
                return false;

            return true;
        }

        [DllImport("Ariadne.CGAL.x64", CallingConvention = CallingConvention.StdCall, ExactSpelling = false, EntryPoint = "GetAxisAlignedBoundingBox")]
        private static extern int GetAxisAlignedBoundingBox([In] CGAL_Point[] points, [In] int size, Notification notification);
        public bool GetAABB(CGAL_Point[] points, int size, Notification notification)
        {
            int res = GetAxisAlignedBoundingBox(points, size, notification);

            if (res != 0)
                return false;

            return true;
        }

        [DllImport("Ariadne.CGAL.x64", CallingConvention = CallingConvention.StdCall, ExactSpelling = false, EntryPoint = "IsPointBelongToElement")]
        private static extern int IsPointBelongToElement([In] CGAL_Point point, [In] CGAL_Point[] meshPoints, [In] int size, Notification notification);
        public bool IsBelongToMesh(CGAL_Point point, CGAL_Point[] meshPoints, int size, Notification notification)
        {
            int res = IsPointBelongToElement(point, meshPoints, size, notification);

            if (res != 0)
                return false;

            return true;
        }
    }
}
