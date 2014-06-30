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
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: Hydro$ense.exe input.xls(x)");
                Console.WriteLine("Where: input.xls(x) contains the problem definition to run");
            }

            ModelInput m = new ModelInput();
            m.ReadHardcoded();
            //m.ReadFromExcel(args[0]);

            TracySolver solver = new TracySolver();
            solver.Solve(m);

            Console.ReadKey();

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());
        }
    }
}
