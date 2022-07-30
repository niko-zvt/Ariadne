using System.Globalization;
using Ariadne.Kernel.Math;

namespace Ariadne.Console
{
    class Program
    {
        // File formats and paths to the sample files
        enum FileFormat { DAT, OP2, SES, XDB }
        private const string example = "000";
        private const string pathToFile = $"Examples\\Ex-{example}\\model-{example}.";

        delegate bool FuncCalculate(Vector3D coords, out Matrix3x3 stress);

        static void Main(string[] args)
        {
            // Configuring the standard output stream
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            StreamWriter stdout = new StreamWriter(System.Console.OpenStandardOutput());
            stdout.AutoFlush = true;

            // File path
            var fullPathToFile = AppDomain.CurrentDomain.BaseDirectory + pathToFile;

            // Create FeResPost database
            var database = Kernel.DB.Create(fullPathToFile + FileFormat.DAT.ToString(),
                                            fullPathToFile + FileFormat.OP2.ToString(),
                                            fullPathToFile + FileFormat.XDB.ToString(),
                                            fullPathToFile + FileFormat.SES.ToString());

            // Create model
            var isForceRemappingResults = false; // TODO: Error in FeResPost
            var model = Kernel.Model.CreateByDatabase(database, isForceRemappingResults);

            // Test methods
            var stress = new Matrix3x3();
            var resultNode = model.GetStressInNode(39, out stress, out var nCoords);
            resultNode = model.GetStressInNode(87, out stress, out nCoords);
            var resultElement = model.GetStressInElement(39, out stress, out var eCoords);
            resultElement = model.GetStressInElement(126, out stress, out eCoords);

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
