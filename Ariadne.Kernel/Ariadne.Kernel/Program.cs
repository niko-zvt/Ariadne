using System;
using System.Globalization;
using System.IO;
using System.Threading;

namespace Ariadne.Kernel
{
    class Program
    {
        // Paths to the sample files
        private const string pathToDAT = @"..\..\..\Examples\Ex-000\model-000.dat";
        private const string pathToOP2 = @"..\..\..\Examples\Ex-000\model-000.op2";
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

            Console.WriteLine(model.Results.ToString());

            // Print results
            database.TEMP_PrintResult(ref stdout, ref model);
        }
    }
}
