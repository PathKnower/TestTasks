using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Task1Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Invalid number of arguments.");
                return;
            }
            int id = int.Parse(args[2]);

            Process oldProcess = null;
            try
            {
                oldProcess = Process.GetProcessById(id);
            }
            catch (ArgumentException) {}

            try
            {
                if (oldProcess != null && !oldProcess.HasExited)
                    oldProcess.WaitForExit(5000);

                File.Copy(args[0], args[1], true);
                Process.Start(args[1]);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(args[0]);
                Console.WriteLine(args[1]);
                Console.WriteLine(args[2]);

                Console.ReadLine();
            }
            
        }
    }
}
