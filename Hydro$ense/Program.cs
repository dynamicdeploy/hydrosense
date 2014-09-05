using System;
using System.IO;
using System.Windows.Forms;

namespace HydroSense
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: Hydro$ense.exe input.xls(x) output.xls(x)");
                Console.WriteLine("Where: input.xls(x) contains the problem definition to run");
                Console.WriteLine("       output.xls(x) file the program will write output to");
            }

            Console.Write("Reading model input...");
            ModelInput m = new ModelInput();
            m.ReadHardcoded();
            //m.ReadFromExcel(args[0]);
            Console.WriteLine("Done");

            Console.WriteLine("Solving...");
            TracySolver solver = new TracySolver();
            solver.Solve(m);

            Console.WriteLine("Writing output...");
            ModelOutput mOut = new ModelOutput(m);
            //mOut.ToExcel(args[1]);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());
        }
    }
}
