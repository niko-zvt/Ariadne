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

        delegate bool FuncCalculate(Vector3D coords, out Matrix3x3 stress);

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
            var stress = new Matrix3x3();
            var resultNode = model.GetStressInNode(39, out stress, out var nCoords);
            var resultElement = model.GetStressInElement(39, out stress, out var eCoords);

            var testVectors = new List<Vector3D>()
            {
                new Vector3D(8, 16, 0),             //CQUAD4 eID = 26
                new Vector3D(-30.29, 16.39, 0.0),   //CQUAD4 eID = 52
                new Vector3D(-29.8, -33.49, 0.0),   //CTRIA3 eID = 126
                new Vector3D(37.75, 0.2, 0.0),      //VOID VOLUME
                new Vector3D(7.89, -50, 0.0)        //VOID VOLUME
            };

            stdout.WriteLine($"ID\tType\tX\tY\tZ\tSxx\tSxy\tSxz\tSyx\tSyy\tSyz\tSzx\tSzy\tSzz");
            foreach (var vector in testVectors)
            {
                var result = model.GetStressInPoint(vector, out stress);
                if (result)
                {
                    model.GetElementIDFromPoint(vector, out var id);
                    var type = model.Elements.GetByID(id).GetElementType();
                    stdout.WriteLine($"{id}\t{type}\t{vector.X}\t{vector.Y}\t{vector.Z}\t{stress.XX}\t{stress.XY}\t{stress.XZ}\t{stress.YX}\t{stress.YY}\t{stress.YZ}\t{stress.ZX}\t{stress.ZY}\t{stress.ZZ}");
                }
            }
          

        }
    }
}
