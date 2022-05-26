using System;
using System.Globalization;
using System.IO;
using System.Threading;

namespace Ariadne.Kernel
{
    class Program
    {
        // Paths to the sample files
        private const string pathToLibs = @"..\..\..\..\Libs\";
        private const string pathToDAT = @"..\..\..\..\Examples\Ex-000\model-000.dat";
        private const string pathToOP2 = @"..\..\..\..\Examples\Ex-000\model-000.op2";
        //private const string pathToSES = @"..\..\..\..\Examples\Ex-000\model-000.ses";
        //private const string pathToXDB = @"..\..\..\..\Examples\Ex-000\model-000.xdb";

        static void Main(string[] args)
        {
            // Configuring the standard output stream
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            StreamWriter stdout = new StreamWriter(Console.OpenStandardOutput());
            stdout.AutoFlush = true;

            // Create database
            var database = DB.Create(pathToDAT, pathToOP2);

            // Create model
            var model = Model.CreateByDatabase(database);

            // Test methods
            var resultNode = model.GetStressInNode(39, out var nStress, out var nCoords);
            var resultElement = model.GetStressInElement(39, out var eStress, out var eCoords);
            var resultPoint = model.GetStressInPoint(new Math.Vector3D(1.0, 1.0, 0.0), out var pStress);

            Directory.SetCurrentDirectory(pathToLibs);
            CGAL.ILibraryImport cgal = CGAL.LibraryImport.Select();

            CGAL.CGAL_Point[] points = new CGAL.CGAL_Point[]
            {
                new CGAL.CGAL_Point(1.5f, 0.0f, 0.0f),
                new CGAL.CGAL_Point(0.0f, 1.5f, 0.0f),
                new CGAL.CGAL_Point(0.0f, 0.0f, 1.5f)
            };
            var test = cgal.GetOOBB(points, points.Length);
            stdout.WriteLine("C++ say: {0}", test);
        }
    }
}
