using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace TestTask1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                if (File.Exists("updater.exe"))
                    File.Delete("updater.exe");
            }
            catch (Exception) {}

            if (args != null && args.Length == 1 && args[0] == "ShowVersion")
            {
                var version = Form1.GetFileVersion(System.Reflection.Assembly.GetExecutingAssembly().Location);
                if (version == null)
                    return;
                MessageBox.Show("Programm version: " + version?.ProductVersion, "Program version", MessageBoxButtons.OK);
            }
            else
                Application.Run(new Form1());
        }
    }
}
