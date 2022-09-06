// Copyright 2022 Nikolay V. Zhivotenko
// Licensed under the Apache License, Version 2.0
// E-mail: niko.zvt@gmail.com

using System.Runtime.InteropServices;

namespace Ariadne.Kernel.CGAL
{
    /// <summary>
    /// CGAL Point3D
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CGAL_Vector3D
    {
        public float X, Y, Z;

        public CGAL_Vector3D(float x, float y, float z)
        {
            X = x; Y = y; Z = z;
        }
    }

    /// <summary>
    /// CGAL LCS
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CGAL_LCS
    {
        public CGAL_Vector3D O, X, Y, Z;

        public CGAL_LCS(CGAL_Vector3D o, CGAL_Vector3D x, CGAL_Vector3D y, CGAL_Vector3D z)
        {
            O = o; X = x; Y = y; Z = z;
        }
    }
}
