using System;
using System.Windows.Forms;

namespace HydroSense
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ModelInput m = new ModelInput();
            m.ReadHardcoded();

            TracySolver solver = new TracySolver();
            solver.Solve(m);
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
