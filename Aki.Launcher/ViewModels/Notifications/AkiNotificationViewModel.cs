using Avalonia.Controls.Notifications;
using Avalonia.Media;
using ReactiveUI;

namespace Aki.Launcher.ViewModels.Notifications
{
    public class AkiNotificationViewModel : ViewModelBase
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public IBrush BarColor { get; set; }

        public AkiNotificationViewModel(IScreen Host, string Title, string Message, NotificationType Type = NotificationType.Information) : base(Host)
        {
            this.Title = Title;
            this.Message = Message;

            switch(Type)
            {
                case NotificationType.Information:
                    {
                        BarColor = new SolidColorBrush(Colors.DodgerBlue);
                        break;
                    }
                case NotificationType.Warning:
                    {
                        BarColor = new SolidColorBrush(Colors.Gold);
                        break;
                    }
                case NotificationType.Success:
                    {
                        BarColor = new SolidColorBrush(Colors.ForestGreen);
                        break;
                    }
                case NotificationType.Error:
                    {
                        BarColor = new SolidColorBrush(Colors.IndianRed);
                        break;
                    }
                default:
                    {
                        BarColor = new SolidColorBrush(Colors.Gray);
                        break;
                    }
            }
        }
    }
}
