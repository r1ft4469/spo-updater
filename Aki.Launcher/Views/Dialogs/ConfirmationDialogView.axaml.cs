using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Aki.Launcher.Views.Dialogs
{
    public partial class ConfirmationDialogView : UserControl
    {
        public ConfirmationDialogView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
