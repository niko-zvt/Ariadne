// Copyright 2022 Nikolay V. Zhivotenko
// Licensed under the Apache License, Version 2.0
// E-mail: niko.zvt@gmail.com

using Ariadne.Kernel.CGAL;
using System;
using System.IO;

namespace Ariadne.Kernel.Libs
{
    /// <summary>
    /// Libraries importer class
    /// </summary>
    public static class LibraryImport
    {
        /// <summary>
        /// Local path to Lib directory.
        /// </summary>
        private const string pathToLibs =  "Libs\\";

        /// <summary>
        /// Select C++ CGAL library
        /// </summary>
        /// <returns>Library</returns>
        /// <exception cref="System.Exception"></exception>
        public static ILibCGAL SelectCGAL()
        {
            // Full path to Lib directory
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory + pathToLibs);

            // Check IntPtr size for determine the bit depth of the application
            if (IntPtr.Size == 4) 
            {
                // 32-bit application
                throw new System.Exception("Attention for 32-bit application: x86 platform not supported!");
                //return new LibCGAL_x86();
            }
            else 
            {
                // 64-bit application
                return new LibCGAL_x64();
            }
        }
    }
}
