/* NotificationItem.cs
 * License: NCSA Open Source License
 * 
 * Copyright: Merijn Hendriks
 * AUTHORS:
 * waffle.lord
 */


using System;
using System.ComponentModel;

namespace Aki.Launcher.Models.Launcher.Notifications
{
    public class NotificationItem : INotifyPropertyChanged
    {
        private string _Message;
        public string Message
        {
            get => _Message;
            set
            {
                if (_Message != value)
                {
                    _Message = value;
                    RaisePropertyChanged(nameof(Message));
                }
            }
        }

        private string _ButtonText;
        public string ButtonText
        {
            get => _ButtonText;
            set
            {
                if (_ButtonText != value)
                {
                    _ButtonText = value;
                    RaisePropertyChanged(nameof(ButtonText));
                }
            }
        }

        private bool _HasButton;
        public bool HasButton
        {
            get => _HasButton;
            set
            {
                if (_HasButton != value)
                {
                    _HasButton = value;
                    RaisePropertyChanged(nameof(HasButton));
                }
            }
        }

        public Action ItemAction = null;

        public NotificationItem(string Message)
        {
            this.Message = Message;
            ButtonText = string.Empty;
            HasButton = false;
        }

        public NotificationItem(string Message, string ButtonText, Action ItemAction)
        {
            this.Message = Message;
            this.ButtonText = ButtonText;
            HasButton = true;
            this.ItemAction = ItemAction;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
