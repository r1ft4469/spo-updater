using Aki.Launcher.Helpers;
using Aki.Launcher.MiniCommon;
using Aki.Launcher.Models;
using Aki.Launcher.Models.Launcher;
using Avalonia;
using ReactiveUI;
using System.Threading.Tasks;
using Aki.Launcher.Attributes;
using Aki.Launcher.ViewModels.Dialogs;
using Avalonia.Threading;
using System.Reactive.Disposables;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading;

namespace Aki.Launcher.ViewModels
{
    [RequireLoggedIn]
    public class ProfileViewModel : ViewModelBase
    {
        public string CurrentUsername { get; set; }
        private bool _UpdateAvailable { get; set; } = false;
        private string _CurrentEdition;
        public string CurrentEdition
        {
            get => _CurrentEdition;
            set => this.RaiseAndSetIfChanged(ref _CurrentEdition, value);
        }

        public string CurrentID { get; set; }

        public ProfileInfo ProfileInfo { get; set; } = AccountManager.SelectedProfileInfo;

        public ImageHelper SideImage { get; } = new ImageHelper();

        private GameStarter gameStarter = new GameStarter(new GameStarterFrontend());

        private ProcessMonitor monitor { get; set; }

        public ProfileViewModel(IScreen Host) : base(Host)
        {
            this.WhenActivated((CompositeDisposable disposables) =>
            {
                Task.Run(() =>
                {
                    GameVersionCheck();
                });
            });

            // cache and load side image if profile has a side
            if(AccountManager.SelectedProfileInfo != null && AccountManager.SelectedProfileInfo.Side != null)
            {
                ImageRequest.CacheSideImage(AccountManager.SelectedProfileInfo.Side);
                SideImage.Path = AccountManager.SelectedProfileInfo.SideImage;
                SideImage.Touch();
            }

            monitor = new ProcessMonitor("EscapeFromTarkov", 1000, aliveCallback: GameAliveCallBack, exitCallback: GameExitCallback);

            CurrentUsername = AccountManager.SelectedAccount.username;

            CurrentEdition = AccountManager.SelectedAccount.edition;

            CurrentID = AccountManager.SelectedAccount.id;
        }

        private async Task GameVersionCheck()
        {
            string compatibleGameVersion = ServerManager.GetCompatibleGameVersion();

            if (compatibleGameVersion == "") return;

            // get the product version of the exe
            string gameVersion = FileVersionInfo.GetVersionInfo(Path.Join(LauncherSettingsProvider.Instance.GamePath, "EscapeFromTarkov.exe")).FileVersion;

            if (gameVersion == null) return;

            // if the compatible version isn't the same as the game version show a warning dialog
            if(compatibleGameVersion != gameVersion)
            {
                WarningDialogViewModel warning = new WarningDialogViewModel(null,
                                                     string.Format(LocalizationProvider.Instance.game_version_mismatch_format_2, gameVersion, compatibleGameVersion),
                                                     LocalizationProvider.Instance.i_understand);
                Dispatcher.UIThread.InvokeAsync(async() =>
                {
                    await ShowDialog(warning);
                });
            }

            
            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                UpdateStatus();
            });
        }

        public void LogoutCommand()
        {
            AccountManager.Logout();

            NavigateTo(new ConnectServerViewModel(HostScreen, true));
        }

        public void ChangeWindowState(Avalonia.Controls.WindowState? State, bool Close = false)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                if (Application.Current.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
                {
                    if (Close)
                    {
                        desktop.ShutdownMode = Avalonia.Controls.ShutdownMode.OnMainWindowClose;
                        desktop.Shutdown();
                    }
                    else
                    {
                        desktop.MainWindow.WindowState = State ?? Avalonia.Controls.WindowState.Normal;
                    }
                }
            });
        }

        public async Task StartGameCommand()
        {
            LauncherSettingsProvider.Instance.AllowSettings = false;

            AccountStatus status = await AccountManager.LoginAsync(AccountManager.SelectedAccount.username, AccountManager.SelectedAccount.password);

            LauncherSettingsProvider.Instance.AllowSettings = true;

            switch (status)
            {
                case AccountStatus.NoConnection:
                    NavigateTo(new ConnectServerViewModel(HostScreen));
                    return;
            }

            LauncherSettingsProvider.Instance.GameRunning = true;

            GameStarterResult gameStartResult = await gameStarter.LaunchGame(ServerManager.SelectedServer, AccountManager.SelectedAccount);

            if (gameStartResult.Succeeded)
            {
                monitor.Start();

                switch (LauncherSettingsProvider.Instance.LauncherStartGameAction)
                {
                    case LauncherAction.MinimizeAction:
                        {
                            ChangeWindowState(Avalonia.Controls.WindowState.Minimized);
                            break;
                        }
                    case LauncherAction.ExitAction:
                        {
                            ChangeWindowState(null, true);
                            break;
                        }
                }
            }
            else
            {
                SendNotification("", gameStartResult.Message, Avalonia.Controls.Notifications.NotificationType.Error);
                LauncherSettingsProvider.Instance.GameRunning = false;
            }
        }

        public async Task ChangeEditionCommand()
        {
            var result = await ShowDialog(new ChangeEditionDialogViewModel(null));

            if(result != null && result is string edition)
            {
                AccountStatus status = await AccountManager.WipeAsync(edition);

                switch (status)
                {
                    case AccountStatus.OK:
                        {
                            CurrentEdition = AccountManager.SelectedAccount.edition;
                            SendNotification("", LocalizationProvider.Instance.account_updated);
                            break;
                        }
                    case AccountStatus.NoConnection:
                        {
                            NavigateTo(new ConnectServerViewModel(HostScreen));
                            break;
                        }
                    default:
                        {
                            SendNotification("", LocalizationProvider.Instance.edit_account_update_error);
                            break;
                        }
                }
            }
        }

        public async Task CopyCommand(object parameter)
        {
            if (Application.Current.Clipboard != null && parameter != null && parameter is string text)
            {
                await Application.Current.Clipboard.SetTextAsync(text);
                SendNotification("", $"{text} {LocalizationProvider.Instance.copied}", Avalonia.Controls.Notifications.NotificationType.Success);
            }
        }

        public async Task RemoveProfileCommand()
        {
            ConfirmationDialogViewModel confirmation = new ConfirmationDialogViewModel(null, string.Format(LocalizationProvider.Instance.profile_remove_question_format_1, AccountManager.SelectedAccount.username));

            var result = await ShowDialog(confirmation);

            if (result is bool b && !b) return;

            AccountStatus status = await AccountManager.RemoveAsync();

            switch(status)
            {
                case AccountStatus.OK:
                    {
                        SendNotification("", LocalizationProvider.Instance.profile_removed);

                        LauncherSettingsProvider.Instance.Server.AutoLoginCreds = null;

                        LauncherSettingsProvider.Instance.SaveSettings();

                        NavigateTo(new ConnectServerViewModel(HostScreen));
                        break;
                    }
                case AccountStatus.UpdateFailed:
                    {
                        SendNotification("", LocalizationProvider.Instance.profile_removal_failed);
                        break;
                    }
                case AccountStatus.NoConnection:
                    {
                        SendNotification("", LocalizationProvider.Instance.no_servers_available);
                        NavigateTo(new ConnectServerViewModel(HostScreen));
                        break;
                    }
            }
        }

        private void UpdateProfileInfo()
        {
            AccountManager.UpdateProfileInfo();
            ImageRequest.CacheSideImage(AccountManager.SelectedProfileInfo.Side);
            ProfileInfo.UpdateDisplayedProfile(AccountManager.SelectedProfileInfo);
            if (ProfileInfo.SideImage != SideImage.Path)
            {
                SideImage.Path = ProfileInfo.SideImage;
                SideImage.Touch();
            }
        }


        //pull profile every x seconds
        private int aliveCallBackCountdown = 60;
        private void GameAliveCallBack(ProcessMonitor monitor)
        {
            aliveCallBackCountdown--;

            if (aliveCallBackCountdown <= 0)
            {
                aliveCallBackCountdown = 60;
                UpdateProfileInfo();
            }
        }

        private void GameExitCallback(ProcessMonitor monitor)
        {
            monitor.Stop();

            LauncherSettingsProvider.Instance.GameRunning = false;

            //Make sure the call to MainWindow happens on the UI thread.
            switch (LauncherSettingsProvider.Instance.LauncherStartGameAction)
            {
                case LauncherAction.MinimizeAction:
                    {
                        ChangeWindowState(Avalonia.Controls.WindowState.Normal);

                        break;
                    }
            }

            UpdateProfileInfo();
        }

        public void VerifyCommand()
        {
            NavigateTo(new ConnectServerViewModel(HostScreen, true));

            var strCmdText = "/C git\\verify.bat";
            System.Diagnostics.Process.Start("CMD.exe", strCmdText);
        }

        public void UpdateCommand()
        {
            if (_UpdateAvailable)
            {
                NavigateTo(new ConnectServerViewModel(HostScreen, true));

                var strCmdText = "/C git\\update.bat";
                System.Diagnostics.Process.Start("CMD.exe",strCmdText);
            }
            else
            {
                SendNotification("", "No Update Available");
            }            
        }

        private async Task UpdateStatus()
        {
            var strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location + "git\\";

            if (File.Exists(strExeFilePath + "git.exe"))
            {
                NavigateTo(new ConnectServerViewModel(HostScreen));
                UpdateCommand();
                return;
            }

            if (File.Exists(strExeFilePath + "release.json") && File.Exists(strExeFilePath + "release.new.json"))
            {
                string newJson = "";
                using (StreamReader r = new StreamReader(strExeFilePath + "release.new.json"))
                {
                    newJson = r.ReadToEnd();
                    r.Close();
                }

                string oldJson = "";
                using (StreamReader r = new StreamReader(strExeFilePath + "release.json"))
                {
                    oldJson = r.ReadToEnd();
                    r.Close();
                }

                var newID = JsonConvert.DeserializeObject<Commit>(newJson).sha;
                var oldID = JsonConvert.DeserializeObject<Commit>(oldJson).sha;

                if (newID != oldID)
                {
                    _UpdateAvailable = true;
                    string notesJson = "";
                    using (StreamReader r = new StreamReader(strExeFilePath + "notes.json"))
                    {
                        notesJson = r.ReadToEnd();
                        r.Close();
                    }

                    var notes = JsonConvert.DeserializeObject<ReleaseNotes>(notesJson).body;
                    SendNotification("SPO Update Available", $"{notes}");   
                }

                return;
            }
        }
    }
}
