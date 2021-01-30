using System;
using System.Globalization;
using System.IO;
using System.Threading;

namespace Ariadne.Kernel
{
    class Program
    {
        private const string pathToDAT = @"model.dat";
        private const string pathToOP2 = @"model.op2";
        private const string pathToSES = @"model.ses";
        private const string pathToXDB = @"model.xdb";
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            StreamWriter stdout = new StreamWriter(Console.OpenStandardOutput());
            stdout.AutoFlush = true;
            
            var database = DB.Create(pathToDAT, pathToOP2);

            // Get all nodes
            var nodeIDs = database.GetAllNodeIDs();
            var nodes = database.BuildNodes(nodeIDs);

            // Get all elements
            var elementIDs = database.GetAllElementIDs();
            var elements = database.BuildElements(elementIDs);           
        }
    }
}
