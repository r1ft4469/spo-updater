using Aki.Launch.Models.Aki;
using Aki.Launcher.Helpers;
using Aki.Launcher.Models.Launcher;
using Avalonia;
using ReactiveUI;
using Splat;
using System.IO;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace Aki.Launcher.ViewModels
{
    public class ConnectServerViewModel : ViewModelBase
    {
        private bool noAutoLogin = false;

        public ConnectServerModel connectModel { get; set; } = new ConnectServerModel()
        {
            InfoText = LocalizationProvider.Instance.server_connecting
        };
        
        public ConnectServerViewModel(IScreen Host, bool NoAutoLogin = false) : base(Host)
        {
            noAutoLogin = NoAutoLogin;

            this.WhenActivated((CompositeDisposable disposables) =>
            {
                Task.Run(async () =>
                {
                   await ConnectServer();
                });
            });
        }

        public async Task ConnectServer()
        {
            await ServerManager.LoadDefaultServerAsync(LauncherSettingsProvider.Instance.Server.Url);

            bool connected = ServerManager.PingServer();

            connectModel.ConnectionFailed = !connected;

            connectModel.InfoText = connected ? LocalizationProvider.Instance.ok : string.Format(LocalizationProvider.Instance.server_unavailable_format_1, LauncherSettingsProvider.Instance.Server.Name);

            if (connected)
            {
                AkiVersion version = Locator.Current.GetService<AkiVersion>("akiversion");

                version.ParseVersionInfo(ServerManager.GetVersion());

                NavigateTo(new LoginViewModel(HostScreen, noAutoLogin));
            }
        }

        public void RetryCommand()
        {
            connectModel.InfoText = LocalizationProvider.Instance.server_connecting;

            connectModel.ConnectionFailed = false;

            Task.Run(async () =>
            {
                await ConnectServer();
            });
        }

        public void UpdateCommand()
        {            
            var strExeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            if (!File.Exists(strExeFilePath + "Server.exe"))
            {
                var patchCmdText = "/C git\\patch.bat";
                System.Diagnostics.Process.Start("CMD.exe",patchCmdText);

                return;
            }
            else
            {
                if (File.Exists(strExeFilePath + "git\\install"))
                {
                    var patchCmdText = "/K git\\README.bat";
                    System.Diagnostics.Process.Start("CMD.exe", patchCmdText);

                    return;
                }
            }

            var serverCmdText = "/C Server.exe";
            System.Diagnostics.Process.Start("CMD.exe",serverCmdText);
        }
    }
}
