using Aki.Launcher.Attributes;
using Aki.Launcher.Helpers;
using Aki.Launcher.MiniCommon;
using Aki.Launcher.Models;
using Aki.Launcher.Models.Aki;
using Aki.Launcher.Models.Launcher;
using Aki.Launcher.ViewModels.Dialogs;
using Avalonia.Controls.Notifications;
using ReactiveUI;
using Splat;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;

namespace Aki.Launcher.ViewModels
{
    [RequireServerConnected]
    public class LoginViewModel : ViewModelBase
    {
        public ObservableCollection<ProfileInfo> ExistingProfiles { get; set; } = new ObservableCollection<ProfileInfo>();

        public LoginModel Login { get; set; } = new LoginModel();

        public ReactiveCommand<Unit, Unit> LoginCommand { get; set; }

        public LoginViewModel(IScreen Host, bool NoAutoLogin = false) : base(Host)
        {
            //setup reactive commands
            LoginCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                AccountStatus status = await AccountManager.LoginAsync(Login);

                switch (status)
                {
                    case AccountStatus.OK:
                        {
                            if (LauncherSettingsProvider.Instance.UseAutoLogin && LauncherSettingsProvider.Instance.Server.AutoLoginCreds != Login)
                            {
                                LauncherSettingsProvider.Instance.Server.AutoLoginCreds = Login;
                            }

                            LauncherSettingsProvider.Instance.SaveSettings();
                            NavigateTo(new ProfileViewModel(HostScreen));
                            break;
                        }
                    case AccountStatus.LoginFailed:
                        {
                            // Create account if it doesn't exist
                            if (!string.IsNullOrWhiteSpace(Login.Username))
                            {
                                var result = await ShowDialog(new RegisterDialogViewModel(null, Login.Username));

                                if(result != null && result is string edition)
                                {
                                    AccountStatus registerResult = await AccountManager.RegisterAsync(Login.Username, Login.Password, edition);

                                    switch(registerResult)
                                    {
                                        case AccountStatus.OK:
                                            {
                                                if (LauncherSettingsProvider.Instance.UseAutoLogin && LauncherSettingsProvider.Instance.Server.AutoLoginCreds != Login)
                                                {
                                                    LauncherSettingsProvider.Instance.Server.AutoLoginCreds = Login;
                                                }

                                                LauncherSettingsProvider.Instance.SaveSettings();
                                                SendNotification(LocalizationProvider.Instance.profile_created, Login.Username, NotificationType.Success);
                                                NavigateTo(new ProfileViewModel(HostScreen));
                                                break;
                                            }
                                        case AccountStatus.RegisterFailed:
                                            {
                                                SendNotification("", LocalizationProvider.Instance.registration_failed, NotificationType.Error);
                                                break;
                                            }
                                        case AccountStatus.NoConnection:
                                            {
                                                NavigateTo(new ConnectServerViewModel(HostScreen));
                                                break;
                                            }
                                        default:
                                            {
                                                SendNotification("", registerResult.ToString(), NotificationType.Error);
                                                break;
                                            }
                                    }

                                    return;
                                }
                            }

                            SendNotification("", LocalizationProvider.Instance.login_failed, NotificationType.Error);

                            break;
                        }
                    case AccountStatus.NoConnection:
                        {
                            NavigateTo(new ConnectServerViewModel(HostScreen));
                            break;
                        }
                }
            });

            //cache and touch background image
            var backgroundImage = Locator.Current.GetService<ImageHelper>("bgimage");

            ImageRequest.CacheBackgroundImage();

            backgroundImage.Touch();

            //handle auto-login
            if(LauncherSettingsProvider.Instance.UseAutoLogin && LauncherSettingsProvider.Instance.Server.AutoLoginCreds != null && !NoAutoLogin)
            {
                Task.Run(() =>
                {
                    Login = LauncherSettingsProvider.Instance.Server.AutoLoginCreds;
                    LoginCommand.Execute();
                });

                return;
            }

            Task.Run(() =>
            {
                GetExistingProfiles();
            });
        }

        public void LoginProfileCommand(object parameter)
        {
            if (parameter == null) return;

            Task.Run(() =>
            {
                if (parameter is string username)
                {
                    Login.Username = username;
                    LoginCommand.Execute();
                }
            });
        }

        public void GetExistingProfiles()
        {
            ServerProfileInfo[] existingProfiles = AccountManager.GetExistingProfiles();

            if(existingProfiles != null)
            {
                ExistingProfiles.Clear();

                foreach(ServerProfileInfo profile in existingProfiles)
                {
                    ProfileInfo profileInfo = new ProfileInfo(profile);

                    ExistingProfiles.Add(profileInfo);

                    ImageRequest.CacheSideImage(profileInfo.Side);

                    ImageHelper sideImage = new ImageHelper() { Path = profileInfo.SideImage };
                    sideImage.Touch();
                }
            }
        }

        public void UpdateCommand()
        {
            AccountManager.Logout();
            
            var strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            var strCmdText = "/C update.bat";
            System.Diagnostics.Process.Start("CMD.exe",strCmdText);
        }
    }
}
