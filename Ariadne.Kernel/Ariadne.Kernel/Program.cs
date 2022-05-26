﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

namespace Ariadne.Kernel
{
    class Program
    {
        // Paths to the sample files
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

            // Test OOBB
            var points = new List<Math.Vector3D>();
            points.Add(new Math.Vector3D(1.0, 0.0, 0.0));
            points.Add(new Math.Vector3D(2.0, 0.0, 0.0));
            points.Add(new Math.Vector3D(3.0, 0.0, 0.0));
            points.Add(new Math.Vector3D(4.0, 0.0, 0.0));
            var test = Math.OOBoundingBox.CreateByPoints(points);
        }
    }
}
