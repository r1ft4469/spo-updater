using Avalonia;
using ReactiveUI;
using System.Reactive.Disposables;
using Aki.Launcher.Models;
using Aki.Launcher.MiniCommon;
using System.IO;
using Splat;
using Aki.Launch.Models.Aki;
using Aki.Launcher.Helpers;

namespace Aki.Launcher.ViewModels
{
    public class MainWindowViewModel : ReactiveObject, IActivatableViewModel, IScreen
    {
        public AkiVersion VersionInfo { get; set; } = new AkiVersion();
        public RoutingState Router { get; } = new RoutingState();
        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        public ImageHelper Background { get; } = new ImageHelper()
        {
            Path = Path.Join(ImageRequest.ImageCacheFolder, "bg.png")
        };

        public MainWindowViewModel()
        {
            Locator.CurrentMutable.RegisterConstant<ImageHelper>(Background, "bgimage");

            Locator.CurrentMutable.RegisterConstant<AkiVersion>(VersionInfo, "akiversion");

            LauncherSettingsProvider.Instance.AllowSettings = true;

            this.WhenActivated((CompositeDisposable disposables) =>
            {
                Router.Navigate.Execute(new ConnectServerViewModel(this));
            });
        }

        public void CloseCommand()
        {
            if (Application.Current.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktopApp)
            {
                desktopApp.MainWindow.Close();
            }
        }

        public void MinimizeCommand()
        {
            if (Application.Current.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktopApp)
            {
                desktopApp.MainWindow.WindowState = Avalonia.Controls.WindowState.Minimized;
            }
        }

        public void GoToSettingsCommand()
        {
            LauncherSettingsProvider.Instance.AllowSettings = false;

            Router.Navigate.Execute(new SettingsViewModel(this));
        }
    }
}
