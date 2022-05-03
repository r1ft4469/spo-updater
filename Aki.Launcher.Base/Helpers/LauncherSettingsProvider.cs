/* LauncherSettingsProvider.cs
 * License: NCSA Open Source License
 * 
 * Copyright: Merijn Hendriks
 * AUTHORS:
 * waffle.lord
 * Merijn Hendriks
 */


using Aki.Launcher.MiniCommon;
using Aki.Launcher.Models.Launcher;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Aki.Launcher.Helpers
{
    public static class LauncherSettingsProvider
    {
        public static string DefaultSettingsFileLocation = Path.Join(Environment.CurrentDirectory, "user", "launcher", "config.json");
        public static Settings Instance { get; } = Json.Load<Settings>(DefaultSettingsFileLocation) ?? new Settings();
    }

    public class Settings : INotifyPropertyChanged
    {
        private const string registryInstall = @"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\EscapeFromTarkov";
        
        public bool FirstRun { get; set; } = true;

        public void SaveSettings()
        {
            Json.SaveWithFormatting(LauncherSettingsProvider.DefaultSettingsFileLocation, this, Formatting.Indented);
        }

        public string DefaultLocale { get; set; } = "English";

        private bool _IsAddingServer;
        [JsonIgnore]
        public bool IsAddingServer
        {
            get => _IsAddingServer;
            set
            {
                if (_IsAddingServer != value)
                {
                    _IsAddingServer = value;
                    RaisePropertyChanged(nameof(IsAddingServer));
                }
            }
        }

        private bool _AllowSettings;
        [JsonIgnore]
        public bool AllowSettings
        {
            get => _AllowSettings;
            set
            {
                if (_AllowSettings != value)
                {
                    _AllowSettings = value;
                    RaisePropertyChanged(nameof(AllowSettings));
                }
            }
        }

        private bool _GameRunning;
        [JsonIgnore]
        public bool GameRunning
        {
            get => _GameRunning;
            set
            {
                if (_GameRunning != value)
                {
                    _GameRunning = value;
                    RaisePropertyChanged(nameof(GameRunning));
                }
            }
        }

        private LauncherAction _LauncherStartGameAction;
        public LauncherAction LauncherStartGameAction
        {
            get => _LauncherStartGameAction;
            set
            {
                if (_LauncherStartGameAction != value)
                {
                    _LauncherStartGameAction = value;
                    RaisePropertyChanged(nameof(LauncherStartGameAction));
                }
            }
        }

        private bool _UseAutoLogin;
        public bool UseAutoLogin
        {
            get => _UseAutoLogin;
            set
            {
                if (_UseAutoLogin != value)
                {
                    _UseAutoLogin = value;
                    RaisePropertyChanged(nameof(UseAutoLogin));
                }
            }
        }

        private string _GamePath;
        public string GamePath
        {
            get => _GamePath;
            set
            {
                if (_GamePath != value)
                {
                    _GamePath = value;
                    RaisePropertyChanged(nameof(GamePath));
                }
            }
        }

        private string _OriginalGamePath;

        public string OriginalGamePath
        {
            get => _OriginalGamePath ??= DetectOriginalGamePath();
            set
            {
                if (_OriginalGamePath != value)
                {
                    _OriginalGamePath = value;
                    RaisePropertyChanged(nameof(OriginalGamePath));
                }
            }
        }

        private string[] _ExcludeFromCleanup;

        public string[] ExcludeFromCleanup
        {
            get => _ExcludeFromCleanup ??= Array.Empty<string>();
            set
            {
                if (_ExcludeFromCleanup != value)
                {
                    _ExcludeFromCleanup = value;
                    RaisePropertyChanged(nameof(ExcludeFromCleanup));
                }
            }
        }

        private static string DetectOriginalGamePath()
        {
            // We can't detect the installed path on non-Windows
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return null;

            var uninstallStringValue = Registry.LocalMachine.OpenSubKey(registryInstall, false)
                ?.GetValue("UninstallString");
            var info = (uninstallStringValue is string key) ? new FileInfo(key) : null;
            return info?.DirectoryName;
        }

        public ServerSetting Server { get; set; } = new ServerSetting();

        public Settings()
        {
            if (!File.Exists(LauncherSettingsProvider.DefaultSettingsFileLocation))
            {
                LauncherStartGameAction = LauncherAction.MinimizeAction;
                UseAutoLogin = true;
                GamePath = Environment.CurrentDirectory;

                Server = new ServerSetting { Name = "SPT-AKI", Url = "http://127.0.0.1:6969" };
                SaveSettings();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
