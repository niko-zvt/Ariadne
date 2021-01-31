using System;
using System.Globalization;
using System.IO;
using System.Threading;

namespace Ariadne.Kernel
{
    class Program
    {
        private const string pathToDAT = @"E:\Ariadne\00_Test_Problem\SolverOutput\PlateWithEllipse\model-000.dat";
        private const string pathToOP2 = @"E:\Ariadne\00_Test_Problem\SolverOutput\PlateWithEllipse\model-000.op2";
        private const string pathToSES = @"E:\Ariadne\00_Test_Problem\SolverOutput\PlateWithEllipse\model-000.ses";
        private const string pathToXDB = @"E:\Ariadne\00_Test_Problem\SolverOutput\PlateWithEllipse\model-000.xdb";
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            StreamWriter stdout = new StreamWriter(Console.OpenStandardOutput());
            stdout.AutoFlush = true;
            
            var database = DB.Create(pathToDAT, pathToOP2);

            // Get all materials
            var materialIDs = database.GetAllMaterialIDs();
            var materials = database.BuildMaterials(materialIDs);

            // Get all properties
            var propertyIDs = database.GetAllPropertyIDs();
            var properties = database.BuildProperties(propertyIDs);

            // Get all nodes
            var nodeIDs = database.GetAllNodeIDs();
            var nodes = database.BuildNodes(nodeIDs);

            // Get all elements
            var elementIDs = database.GetAllElementIDs();
            var elements = database.BuildElements(elementIDs);
        }
    }
}
