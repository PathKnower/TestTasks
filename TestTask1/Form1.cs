using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Threading;

namespace TestTask1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if (Directory.Exists("new"))
            {
                FileInfo newApp = new FileInfo("new/test.exe");

                if (newApp.Exists)
                {
                    var newVersion = GetFileVersion(newApp.FullName);
                    if (newVersion == null)
                        return;

                    string currentAppLocation = Assembly.GetExecutingAssembly().Location;
                    var appVersion = GetFileVersion(currentAppLocation);
                    if (appVersion == null)
                        return;

                    bool needUpdate = false;

                    if (newVersion.ProductMajorPart > appVersion.ProductMajorPart)
                        needUpdate = true;
                    if (newVersion.ProductMinorPart > appVersion.ProductMinorPart)
                        needUpdate = true;
                    if (newVersion.ProductBuildPart > appVersion.ProductBuildPart)
                        needUpdate = true;

                    if (needUpdate)
                    {
                        File.WriteAllBytes("updater.exe", Properties.Resources.Task1Updater);
                        ProcessStartInfo updaterStartInfo = new ProcessStartInfo("updater.exe")
                        {
                            Arguments = $"{newApp.FullName} {currentAppLocation} {Process.GetCurrentProcess().Id}"
                        };
                        Process.Start(updaterStartInfo);

                        Application.Exit();
                    }
                    else
                        MessageBox.Show("Application is already at newest version");
                }
                else
                    MessageBox.Show("File not found");
            }
            else
                MessageBox.Show("Directory not found");
        }

        private void ShowFileVersionButton_Click(object sender, EventArgs e)
        {
            var version = GetFileVersion(Assembly.GetExecutingAssembly().Location);
            if (version == null)
                return;

            MessageBox.Show("Program version: " + version.ProductVersion);
        }

        internal static FileVersionInfo GetFileVersion(string path)
        {
            try
            {
                return FileVersionInfo.GetVersionInfo(path);
            }
            catch (Exception)
            {
                MessageBox.Show("Error accuring while read file version");
                return null;
            }

        }
        
    }
}
