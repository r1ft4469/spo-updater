using Aki.Launcher.ViewModels.Dialogs;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;

namespace Aki.Launcher.Views.Dialogs
{
    public partial class ChangeEditionDialogView : ReactiveUserControl<ChangeEditionDialogViewModel>
    {
        public ChangeEditionDialogView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
