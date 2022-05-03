/* ConnectServerModel.cs
 * License: NCSA Open Source License
 * 
 * Copyright: Merijn Hendriks
 * AUTHORS:
 * Merijn Hendriks
 */


using System.ComponentModel;

namespace Aki.Launcher.Models.Launcher
{
    public class ConnectServerModel : INotifyPropertyChanged
    {
        private string _InfoText;
        public string InfoText
        {
            get => _InfoText;
            set
            {
                if (_InfoText != value)
                {
                    _InfoText = value;
                    RaisePropertyChanged(nameof(InfoText));
                }
            }
        }

        private bool _ConnectionFailed;
        public bool ConnectionFailed
        {
            get => _ConnectionFailed;
            set
            {
                if(_ConnectionFailed != value)
                {
                    _ConnectionFailed = value;
                    RaisePropertyChanged(nameof(ConnectionFailed));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
