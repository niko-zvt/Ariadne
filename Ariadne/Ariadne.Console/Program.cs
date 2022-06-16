using System.Globalization;
using Ariadne.Kernel.Math;

namespace Ariadne.Console
{
    class Program
    {
        // Paths to the sample files
        private const string pathToDAT = @"Examples\Ex-000\model-000.dat";
        private const string pathToOP2 = @"Examples\Ex-000\model-000.op2";
        //private const string pathToSES = @"Examples\Ex-000\model-000.ses";
        //private const string pathToXDB = @"Examples\Ex-000\model-000.xdb";

        static void Main(string[] args)
        {
            // Configuring the standard output stream
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            StreamWriter stdout = new StreamWriter(System.Console.OpenStandardOutput());
            stdout.AutoFlush = true;

            // Create FeResPost database
            var fullPathToDAT = AppDomain.CurrentDomain.BaseDirectory + pathToDAT;
            var fullPathToOP2 = AppDomain.CurrentDomain.BaseDirectory + pathToOP2;
            var database = Kernel.DB.Create(fullPathToDAT, fullPathToOP2);

            // Create model
            var model = Kernel.Model.CreateByDatabase(database);

            // Test methods
            var resultNode = model.GetStressInNode(39, out var nStress, out var nCoords);
            var resultElement = model.GetStressInElement(39, out var eStress, out var eCoords);

            var shapeTest = model.GetStressInPoint(new Vector3D(8, 16, 0), out var pStressTest);            //CQUAD4 26
            var resultCQUAD4 = model.GetStressInPoint(new Vector3D(-30.29, 16.39, 0.0), out var pStress1);  //CQUAD4 eID = 52
            var resultCTRIA3 = model.GetStressInPoint(new Vector3D(-29.8, -33.49, 0.0), out var pStress2);  //CTRIA3 eID = 126
            var resultVOID1 = model.GetStressInPoint(new Vector3D(37.75, 0.2, 0.0), out var pStress3);      //VOID VOLUME
            var resultVOID2 = model.GetStressInPoint(new Vector3D(7.89, -50, 0.0), out var pStress4);       //VOID VOLUME
        }
    }
}
