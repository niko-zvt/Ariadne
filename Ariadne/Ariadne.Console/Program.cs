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
            var streams = new List<StreamWriter>();
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            StreamWriter stdout = new StreamWriter(System.Console.OpenStandardOutput());
            stdout.AutoFlush = true;
            streams.Add(stdout);

            // File path
            var fullPathToFile = AppDomain.CurrentDomain.BaseDirectory + pathToFile;

            // Create FeResPost database
            var database = Kernel.DB.Create(fullPathToFile + FileFormat.DAT.ToString(),
                                            fullPathToFile + FileFormat.OP2.ToString(),
                                            fullPathToFile + FileFormat.XDB.ToString(),
                                            fullPathToFile + FileFormat.SES.ToString());

            // Create model
            var model = Kernel.Model.CreateByDatabase(database);


            // Test vectors
            var testVectors = new List<Vector3D>()
            {
                new Vector3D(8, 16, 0),             //CQUAD4 eID = 26
                new Vector3D(-30.29, 16.39, 0.0),   //CQUAD4 eID = 52
                new Vector3D(-29.8, -33.49, 0.0),   //CTRIA3 eID = 126
                new Vector3D(37.75, 0.2, 0.0),      //VOID VOLUME
                new Vector3D(7.89, -50, 0.0)        //VOID VOLUME
            };

            // Test UVW-coords
            var step = 0.25f;
            var uvwCoords = GetTestListOfUVWCoords(step);
            
            // RunTests
            RunTestForAllNodes(in model, streams);
            //RunTestForAllElements(in model, streams);
            // RunTestForHandmadeVectors(model, streams, testVectors);
            // RunTestForAllElementsInUVPoints(model, streams, uvwCoords);
        }

        static private bool RunTestForAllNodes(in Kernel.Model model, List<StreamWriter> streams)
        {
            var results = new List<bool>();

            foreach (var stream in streams)
                stream.WriteLine($"Result\tID\tType\tX\tY\tZ\tSxx\tSxy\tSxz\tSyx\tSyy\tSyz\tSzx\tSzy\tSzz");

            var sep = "##";

            foreach (var node in model.Nodes)
            {
                var result = model.GetStressInNode(node.ID, out var stress, out var coords);

                foreach (var stream in streams)
                    stream.WriteLine($"{result}{sep}{node.ID}{sep}{node.GetNodeType()}{sep}{coords.X}{sep}{coords.Y}{sep}{coords.Z}{sep}{stress.XX}{sep}{stress.YY}{sep}{stress.XY}");

                results.Add(result);
            }

            return !results.Any(value => value == false);
        }

        static private bool RunTestForAllElements(in Kernel.Model model, List<StreamWriter> streams)
        {
            var results = new List<bool>();

            foreach (var stream in streams)
                stream.WriteLine($"Result\tID\tType\tX\tY\tZ\tSxx\tSxy\tSxz\tSyx\tSyy\tSyz\tSzx\tSzy\tSzz");

            var sep = "##";

            foreach (var element in model.Elements)
            {
                var result = model.GetStressInElement(element.ID, out var stress, out var coords);

                foreach (var stream in streams)
                    stream.WriteLine($"{result}{sep}{element.ID}{sep}{element.GetElementType()}{sep}{coords.X}{sep}{coords.Y}{sep}{coords.Z}{sep}{stress.XX}{sep}{stress.YY}{sep}{stress.XY}");

                results.Add(result);
            }

            return !results.Any(value => value == false);
        }

        static private bool RunTestForHandmadeVectors(Kernel.Model model, List<StreamWriter> streams, List<Vector3D> vectors)
        {
            var results = new List<bool>();

            foreach (var stream in streams)
                stream.WriteLine($"ID\tType\tX\tY\tZ\tSxx\tSxy\tSxz\tSyx\tSyy\tSyz\tSzx\tSzy\tSzz");

            foreach (var vector in vectors)
            {
                var result = model.GetStressInPoint(vector, out var stress);
                if (result)
                {
                    var elementIsExist = model.GetElementIDFromPoint(vector, out var id);
                    if (elementIsExist)
                    {
                        var type = model.Elements.GetByID(id).GetElementType();
                        foreach (var stream in streams)
                            stream.WriteLine($"{id}\t{type}\t{vector.X}\t{vector.Y}\t{vector.Z}\t{stress.XX}\t{stress.XY}\t{stress.XZ}\t{stress.YX}\t{stress.YY}\t{stress.YZ}\t{stress.ZX}\t{stress.ZY}\t{stress.ZZ}");
                    }
                }
                results.Add(result);
            }

            return !results.Any(value => value == false);
        }

        static private bool RunTestForAllElementsInUVPoints(Kernel.Model model, List<StreamWriter> streams, List<Vector3D> uvwCoords)
        {
            var results = new List<bool>();

            foreach (var stream in streams)
                stream.WriteLine($"Result\tID\tType\tU\tV\tW\tX\tY\tZ\tSxx\tSxy\tSxz\tSyx\tSyy\tSyz\tSzx\tSzy\tSzz");

            foreach (var element in model.Elements)
            {
                foreach(var uvw in uvwCoords)
                {
                    var location = element.GetPointByUVWCoords(uvw);                  
                    if(location.IsValid())
                    {
                        var result = model.GetStressInPoint(location, out var stress);
                        foreach (var stream in streams)
                            stream.WriteLine($"{element.ID}\t{element.GetElementType()}\t{uvw.X}\t{uvw.Y}\t{uvw.Z}\t{location.X}\t{location.Y}\t{location.Z}\t{stress.XX}\t{stress.XY}\t{stress.XZ}\t{stress.YX}\t{stress.YY}\t{stress.YZ}\t{stress.ZX}\t{stress.ZY}\t{stress.ZZ}");
                        results.Add(result);
                    }
                }
            }

            return !results.Any(value => value == false);
        }

        static private List<Vector3D> GetTestListOfUVWCoords(float step)
        {
            var uvwCoords = new List<Vector3D>();

            for (float u = -1; u <= 1; u += step)
            {
                for (float v = -1; v <= 1; v += step)
                {
                    for (float w = -1; w <= 1; w += step)
                    {
                        uvwCoords.Add(new Vector3D(u, v, w));
                    }
                }
            }

            return uvwCoords;
        }
    }
}
