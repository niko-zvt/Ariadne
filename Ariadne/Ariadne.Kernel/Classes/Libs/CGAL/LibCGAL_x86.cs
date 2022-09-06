// Copyright 2022 Nikolay V. Zhivotenko
// Licensed under the Apache License, Version 2.0
// E-mail: niko.zvt@gmail.com

using Ariadne.Kernel.Libs;
using System.Runtime.InteropServices;

namespace Ariadne.Kernel.CGAL
{
    //internal class LibCGAL_x86 : ILibCGAL
    //{
    //    [DllImport("Ariadne.CGAL.x86", CallingConvention = CallingConvention.StdCall, ExactSpelling = false, EntryPoint = "_GetOptimalOrientedBoundingBox@12")]
    //    private static extern int GetOptimalOrientedBoundingBox([In] CGAL_Vector3D[] points, [In] int size, Notification notification);
    //    public bool GetOOBB(CGAL_Vector3D[] points, int size, Notification notification)
    //    {
    //        int res = GetOptimalOrientedBoundingBox(points, size, notification);
    //        if (res != 0)
    //            return false;
    //        return true;
    //    }
    //    [DllImport("Ariadne.CGAL.x86", CallingConvention = CallingConvention.StdCall, ExactSpelling = false, EntryPoint = "_GetAxisAlignedBoundingBox@12")]
    //    private static extern int GetAxisAlignedBoundingBox([In] CGAL_Vector3D[] points, [In] int size, Notification notification);
    //    public bool GetAABB(CGAL_Vector3D[] points, int size, Notification notification)
    //    {
    //        int res = GetAxisAlignedBoundingBox(points, size, notification);
    //        if (res != 0)
    //            return false;
    //        return true;
    //    }
    //    [DllImport("Ariadne.CGAL.x86", CallingConvention = CallingConvention.StdCall, ExactSpelling = false, EntryPoint = "_IsPointBelongToElement@12")]
    //    private static extern int IsPointBelongToElement([In] CGAL_Vector3D point, [In] CGAL_Vector3D[] meshPoints, [In] int size, Notification notification);
    //    public bool IsBelongToMesh(CGAL_Vector3D point, CGAL_Vector3D[] meshPoints, int size, Notification notification)
    //    {
    //        int res = IsPointBelongToElement(point, meshPoints, size, notification);
    //        if (res != 0)
    //            return false;
    //        return true;
    //    }
    //}
}

