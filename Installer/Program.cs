using System;
using Microsoft.Win32;
using System.IO;
using System.Net;
using System.IO.Compression;
using System.Diagnostics;
using System.Windows.Forms;

namespace Installer
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("SPO Installer");
            Console.WriteLine("-----------------------------------------");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey("Software\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\EscapeFromTarkov"))
            {
                if (key != null)
                {
                    var EFTLoc = key.GetValue("InstallLocation").ToString();
                    if (EFTLoc != null)
                    {
                        Console.WriteLine($"Detected EFT Install Location at : {EFTLoc}");
                        var installLoc = SelectInstall();
                        if (installLoc == null)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[Error] Please Select a Folder.");
                            return;
                        }
                        if (CheckInstallLoc(EFTLoc, installLoc))
                        {
                            Console.WriteLine($"Insalling SPO to : {installLoc}");
                            var inputDir = new DirectoryInfo(EFTLoc);
                            var outputDir = new DirectoryInfo(installLoc);
                            CopyFolder(inputDir, outputDir);
                            DownloadLauncher(outputDir);
                            ExtractLauncher(outputDir);
                            RunLauncher(outputDir);
                        }
                    }
                    else
                    {
                        Console.WriteLine("[Error] Cannot Find EFT Install.");
                        Console.WriteLine("[Error] Install a Retail Copy of EFT.");
                    }
                }
            }
        }

        private static bool CheckInstallLoc(string EFTLoc, string InstallLoc)
        {
            if (InstallLoc == EFTLoc.ToString())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error] Cannot Install SPO in the EFT Folder");
                Console.WriteLine("[Error] Please Choose a empty Directory to install SPO to.");
                Console.ReadKey();
                return false;
            }

            var filePaths = Directory.GetFiles(InstallLoc);
            if (filePaths.Length > 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error] Please Choose a empty Directory to install SPO to.");
                Console.ReadKey();
                return false;
            }

            return CheckFreeSpace(InstallLoc[0].ToString());
        }

        private static bool CheckFreeSpace(string drive)
        {
            // Create a DriveInfo instance of the D:\ drive
            DriveInfo dDrive = new DriveInfo(drive);
            const double BytesInGB = 1073741824;

            // When the drive is accessible..
            if (dDrive.IsReady)
            {
                Console.WriteLine($"Checking Free Space on Drive: {dDrive.AvailableFreeSpace / BytesInGB} GB");

                if (dDrive.AvailableFreeSpace / BytesInGB < 27)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Error] Not Enough Free Space on Disk.");
                    Console.WriteLine("[Error] Make Sure there is atleast 27 GB Free.");
                    Console.ReadKey();
                    return false;
                }
            }

            return true;
        }

        private static void RunLauncher(DirectoryInfo target)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = target.FullName + "\\Aki.Launcher.exe";
            startInfo.WorkingDirectory = target.FullName;
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            Process.Start(startInfo);
            Console.WriteLine($"Starting Launcher ...");
        }

        private static void DownloadLauncher(DirectoryInfo target)
        {
            Console.WriteLine($"Downloading Launcher ...");
            bool downloaded = false;
            int retries = 0;
            int maxRetries = 5;
            while (!downloaded && retries < maxRetries)
            {
                try
                {
                    string url = "https://github.com/r1ft4469/spo-updater/releases/download/Beta12/Install.zip";
                    string savePath = target.FullName + "\\Install.zip";
                    WebClient client = new WebClient();
                    client.DownloadFile(url, savePath);
                    downloaded = true;
                }
                catch (Exception ex)
                {
                    retries++;
                }
            }


            if (!downloaded)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error] Could not Download the Launcher");
                Console.WriteLine("[Error] Are You Connected to the Internet?");
                Console.ReadKey();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Cleaning up Broken Install ...");
                foreach (FileInfo file in target.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in target.GetDirectories())
                {
                    dir.Delete(true);
                }
                Application.Exit();
            }
        }

        private static void ExtractLauncher(DirectoryInfo target)
        {
            Console.WriteLine($"Extracting Launcher ...");
            string zipPath = target.FullName + "\\Install.zip";
            string extractPath = target.FullName;

            ZipFile.ExtractToDirectory(zipPath, extractPath);

            File.Delete(zipPath);
        }

        public static void CopyFolder(DirectoryInfo source, DirectoryInfo target)
        {
            var tmpColor = Console.ForegroundColor;
            foreach (DirectoryInfo dir in source.GetDirectories())
                CopyFolder(dir, target.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in source.GetFiles())
            {
                var _destination = new FileInfo(Path.Combine(target.FullName, file.Name));

                if (_destination.Exists) _destination.Delete();

                Console.ForegroundColor = ConsoleColor.Blue;
                file.CopyTo(_destination, x => Console.Write($"\r{file.Name} [{x}%]"));
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\r    *    {file.Name}");
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            Console.ForegroundColor = tmpColor;
        }

        private static string SelectInstall()
        {
            Console.WriteLine($"Choose directory to install SPO to :");
            FolderDialog.ISelect select = new FolderDialog.Select();
            select.InitialFolder = "C:\\";
            select.ShowDialog();
            return select.Folder;
        }
    }
}
