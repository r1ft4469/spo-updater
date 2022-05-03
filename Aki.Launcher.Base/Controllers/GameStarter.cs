/* GameStarter.cs
 * License: NCSA Open Source License
 * 
 * Copyright: Merijn Hendriks
 * AUTHORS:
 * waffle.lord
 * reider123
 * Merijn Hendriks
 */


using Aki.Launcher.Helpers;
using Aki.Launcher.MiniCommon;
using Aki.Launcher.Models.Launcher;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aki.Launcher.Controllers;
using Aki.Launcher.Interfaces;

namespace Aki.Launcher
{
    public class GameStarter
    {
        private readonly IGameStarterFrontend _frontend;
        private readonly bool _showOnly;
        private readonly string _originalGamePath;
        private readonly string _gamePath;
        private readonly string[] _excludeFromCleanup;

        private const string registrySettings = @"Software\Battlestate Games\EscapeFromTarkov";

        public GameStarter(IGameStarterFrontend frontend, string gamePath = null, string originalGamePath = null,
            bool showOnly = false, string[] excludeFromCleanup = null)
        {
            _frontend = frontend;
            _showOnly = showOnly;
            _gamePath = gamePath ?? LauncherSettingsProvider.Instance.GamePath ?? Environment.CurrentDirectory;
            _originalGamePath = originalGamePath ?? LauncherSettingsProvider.Instance.OriginalGamePath;
            _excludeFromCleanup = excludeFromCleanup ?? LauncherSettingsProvider.Instance.ExcludeFromCleanup;
        }

        public async Task<GameStarterResult> LaunchGame(ServerInfo server, AccountInfo account)
        {
            // setup directories
            if (IsInstalledInLive())
            {
                return GameStarterResult.FromError(-1);
            }

            SetupGameFiles();

            if (IsPiratedCopy() > 1)
            {
                return GameStarterResult.FromError(-2);
            }

            if (account.wipe)
            {
                RemoveRegistryKeys();
                CleanTempFiles();
            }

            //create nlog.dll.nlog
            if(!NLogCreator.Create())
            {
                return GameStarterResult.FromError(-7);
            }


            // check game path
            var clientExecutable = Path.Join(_gamePath, "EscapeFromTarkov.exe");

            if (!File.Exists(clientExecutable))
            {
                return GameStarterResult.FromError(-6);
            }


            // apply patches
            ProgressReportingPatchRunner patchRunner = new ProgressReportingPatchRunner(_gamePath);

            try
            {
                await _frontend.CompletePatchTask(patchRunner.PatchFiles());
            }
            catch (TaskCanceledException)
            {
                return GameStarterResult.FromError(-4);
            }
            
            //start game
            var args =
                $"-force-gfx-jobs native -token={account.id} -config={Json.Serialize(new ClientConfig(server.backendUrl))}";

            if (_showOnly)
            {
                Console.WriteLine($"{clientExecutable} {args}");
            }
            else
            {
                var clientProcess = new ProcessStartInfo(clientExecutable)
                {
                    Arguments = args,
                    UseShellExecute = false,
                    WorkingDirectory = _gamePath,
                };

                Process.Start(clientProcess);
            }

            return GameStarterResult.FromSuccess();
        }

        bool IsInstalledInLive()
        {
            var value0 = false;

            try
            {
                var value4 = new FileInfo[]
                {
                    new FileInfo(Path.Combine(_originalGamePath, @"Launcher.exe")),
                    new FileInfo(Path.Combine(_originalGamePath, @"Server.exe")),
                    new FileInfo(Path.Combine(_originalGamePath, @"EscapeFromTarkov_Data\Managed\0Harmony.dll")),
                    new FileInfo(Path.Combine(_originalGamePath, @"EscapeFromTarkov_Data\Managed\NLog.dll.nlog")),
                    new FileInfo(Path.Combine(_originalGamePath, @"EscapeFromTarkov_Data\Managed\Nlog.Aki.Loader.dll")),
                };
                var value5 = new FileInfo[]
                {
                    new FileInfo(Path.Combine(_originalGamePath, @"EscapeFromTarkov_Data\Managed\Assembly-CSharp.dll.bak")),
                    new FileInfo(Path.Combine(_originalGamePath, @"EscapeFromTarkov_Data\Managed\Assembly-CSharp.dll")),
                };
                var value6 = new DirectoryInfo(Path.Combine(_originalGamePath, @"Aki_Data"));

                foreach (var value in value4)
                {
                    if (File.Exists(value.FullName))
                    {
                        File.Delete(value.FullName);
                        value0 = true;
                    }
                }

                if (File.Exists(value5[0].FullName))
                {
                    File.Delete(value5[1].FullName);
                    File.Move(value5[0].FullName, value5[1].FullName);
                    value0 = true;
                }

                if (Directory.Exists(value6.FullName))
                {
                    RemoveFilesRecurse(value6);
                    value0 = true;
                }
            }
            catch
            {
            }

            return value0;
        }

        void SetupGameFiles()
        {
            var files = new []
            {
                GetFileForCleanup("BattlEye"),
                GetFileForCleanup("Logs"),
                GetFileForCleanup("ConsistencyInfo"),
                GetFileForCleanup("EscapeFromTarkov_BE.exe"),
                GetFileForCleanup("Uninstall.exe"),
                GetFileForCleanup("UnityCrashHandler64.exe"),
                GetFileForCleanup("WinPixEventRuntime.dll")
            };

            foreach (var file in files)
            {
                if (file == null)
                {
                    continue;
                }

                if (Directory.Exists(file))
                {
                    RemoveFilesRecurse(new DirectoryInfo(file));
                }

                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }

        private string GetFileForCleanup(string fileName)
        {
            if (_excludeFromCleanup.Contains(fileName))
            {
                LogManager.Instance.Info($"Excluded {fileName} from file cleanup");
                return null;
            }
            
            return Path.Combine(_gamePath, fileName);
        }

        int IsPiratedCopy()
        {
            var value0 = 0;
            
            try
            {
                var value4 = new FileInfo[3]
                {
                    new FileInfo(Path.Combine(_originalGamePath, "Uninstall.exe")),
                    new FileInfo(Path.Combine(_originalGamePath, @"BattlEye", "BEClient_x64.dll")),
                    new FileInfo(Path.Combine(_originalGamePath, @"BattlEye", "BEService_x64.dll"))
                };

                value0 = value4.Length;

                foreach (var value in value4)
                {
                    if (File.Exists(value.FullName))
                    {
                        --value0;
                    }
                }
            }
            catch
            {
                value0 = 5;
            }

            return value0;
        }

        /// <summary>
        /// Remove the registry keys
        /// </summary>
        /// <returns>returns true if the keys were removed. returns false if an exception occured</returns>
		public bool RemoveRegistryKeys()
        {
            try
            {
                var key = Registry.CurrentUser.OpenSubKey(registrySettings, true);

                foreach (var value in key.GetValueNames())
                {
                    key.DeleteValue(value);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Clean the temp folder
        /// </summary>
        /// <returns>returns true if the temp folder was cleaned succefully or doesn't exist. returns false if something went wrong.</returns>
		public bool CleanTempFiles()
        {
            var rootdir = new DirectoryInfo(Path.Combine(Path.GetTempPath(), @"Battlestate Games\EscapeFromTarkov"));

            if (!rootdir.Exists)
            {
                return true;
            }

            return RemoveFilesRecurse(rootdir);
        }

        bool RemoveFilesRecurse(DirectoryInfo basedir)
        {
            if (!basedir.Exists)
            {
                return true;
            }

            try
            {
                // remove subdirectories
                foreach (var dir in basedir.EnumerateDirectories())
                {
                    RemoveFilesRecurse(dir);
                }

                // remove files
                var files = basedir.GetFiles();

                foreach (var file in files)
                {
                    file.IsReadOnly = false;
                    file.Delete();
                }

                // remove directory
                basedir.Delete();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
