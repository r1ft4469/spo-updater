using Aki.Launcher.ViewModels.Notifications;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace Aki.Launcher.Views.Notifications
{
    public partial class AkiNotificationView : ReactiveUserControl<AkiNotificationViewModel>
    {
        public AkiNotificationView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
