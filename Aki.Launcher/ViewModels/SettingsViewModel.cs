using Aki.Launcher.Helpers;
using Aki.Launcher.Models;
using Aki.Launcher.Models.Launcher;
using Aki.Launcher.ViewModels.Dialogs;
using Avalonia;
using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Aki.Launcher.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public LocaleCollection Locales { get; set; } = new LocaleCollection();

        private GameStarter gameStarter = new GameStarter(new GameStarterFrontend());

        public SettingsViewModel(IScreen Host) : base(Host)
        {
            if(Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow.Closing += MainWindow_Closing;
            }
        }

        private void MainWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            LauncherSettingsProvider.Instance.SaveSettings();
        }

        public void GoBackCommand()
        {
            if (Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow.Closing -= MainWindow_Closing;
            }

            LauncherSettingsProvider.Instance.AllowSettings = true;
            LauncherSettingsProvider.Instance.SaveSettings();

            NavigateBack();
        }

        public void CleanTempFilesCommand()
        {
            bool filesCleared = gameStarter.CleanTempFiles();

            if (filesCleared)
            {
                SendNotification("", LocalizationProvider.Instance.clean_temp_files_succeeded, Avalonia.Controls.Notifications.NotificationType.Success);
            }
            else
            {
                SendNotification("", LocalizationProvider.Instance.clean_temp_files_failed, Avalonia.Controls.Notifications.NotificationType.Error);
            }
        }

        public void RemoveRegistryKeysCommand()
        {
            bool regKeysRemoved = gameStarter.RemoveRegistryKeys();

            if (regKeysRemoved)
            {
                SendNotification("", LocalizationProvider.Instance.remove_registry_keys_succeeded, Avalonia.Controls.Notifications.NotificationType.Success);
            }
            else
            {
                SendNotification("", LocalizationProvider.Instance.remove_registry_keys_failed, Avalonia.Controls.Notifications.NotificationType.Error);
            }
        }

        public async Task ClearGameSettingsCommand()
        {

            bool BackupAndRemove(string backupFolderPath, FileInfo file)
            {
                file.Refresh();

                //if for some reason the file no longer exists /shrug
                if (!file.Exists)
                {
                    return false;
                }

                //create backup dir and copy file
                Directory.CreateDirectory(backupFolderPath);

                string newFilePath = Path.Combine(backupFolderPath, $"{file.Name}_{DateTime.Now.ToString("MM-dd-yyyy_hh-mm-ss-tt")}.bak");

                File.Copy(file.FullName, newFilePath);

                //copy check
                if (!File.Exists(newFilePath))
                {
                    return false;
                }

                //delete old file
                file.Delete();

                //delete check
                if (file.Exists)
                {
                    return false;
                }

                return true;
            }

            string EFTSettingsFolder = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Escape from Tarkov");
            string backupFolderPath = Path.Combine(EFTSettingsFolder, "Backups");

            if (Directory.Exists(EFTSettingsFolder))
            {
                FileInfo local_ini = new FileInfo(Path.Combine(EFTSettingsFolder, "local.ini"));
                FileInfo shared_ini = new FileInfo(Path.Combine(EFTSettingsFolder, "shared.ini"));

                string Message = string.Format(LocalizationProvider.Instance.clear_game_settings_warning, backupFolderPath);
                ConfirmationDialogViewModel confirmDelete = new ConfirmationDialogViewModel(null, Message, LocalizationProvider.Instance.clear_game_settings, LocalizationProvider.Instance.cancel);

                var confirmation = await ShowDialog(confirmDelete);

                if (confirmation is bool proceed && !proceed)
                {
                    SendNotification("", LocalizationProvider.Instance.clear_game_settings_failed, Avalonia.Controls.Notifications.NotificationType.Error);
                    return;
                }

                bool localSucceeded = BackupAndRemove(backupFolderPath, local_ini);
                bool sharedSucceeded = BackupAndRemove(backupFolderPath, shared_ini);

                //if one fails, I'm considering it bad. Send failed notification.
                if (!localSucceeded || !sharedSucceeded)
                {
                    SendNotification("", LocalizationProvider.Instance.clear_game_settings_failed, Avalonia.Controls.Notifications.NotificationType.Error);
                    return;
                }
            }

            SendNotification("", LocalizationProvider.Instance.clear_game_settings_succeeded, Avalonia.Controls.Notifications.NotificationType.Success);
        }

        public void OpenGameFolderCommand()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = Path.EndsInDirectorySeparator(LauncherSettingsProvider.Instance.GamePath) ? LauncherSettingsProvider.Instance.GamePath : LauncherSettingsProvider.Instance.GamePath + Path.DirectorySeparatorChar,
                UseShellExecute = true,
                Verb = "open"
            });
        }

        public async Task SelectGameFolderCommand()
        {
            
            OpenFolderDialog dialog = new OpenFolderDialog();

            dialog.Directory = Assembly.GetExecutingAssembly().Location;

            if (Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
            {
                string? result = await dialog.ShowAsync(desktop.MainWindow);

                if (result != null)
                {
                    LauncherSettingsProvider.Instance.GamePath = result;
                }
            }
        }
    }
}
