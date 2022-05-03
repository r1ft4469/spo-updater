/* EditionCollection.cs
 * License: NCSA Open Source License
 * 
 * Copyright: Merijn Hendriks
 * AUTHORS:
 * Merijn Hendriks
 */


using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Aki.Launcher.Models.Launcher
{
    public class EditionCollection : INotifyPropertyChanged
    {
        private bool _HasSelection;
        public bool HasSelection
        {
            get => _HasSelection;
            set
            {
                if(_HasSelection != value)
                {
                    _HasSelection = value;
                    RaisePropertyChanged(nameof(HasSelection));
                }
            }
        }
        private int _SelectedEditionIndex;
        public int SelectedEditionIndex
        {
            get => _SelectedEditionIndex;
            set
            {
                if (_SelectedEditionIndex != value)
                {
                    _SelectedEditionIndex = value;
                    RaisePropertyChanged(nameof(SelectedEditionIndex));
                }
            }
        }

        private string _SelectedEdition;
        public string SelectedEdition
        {
            get => _SelectedEdition;
            set
            {
                if (_SelectedEdition != value)
                {
                    _SelectedEdition = value;
                    HasSelection = _SelectedEdition != null;
                    RaisePropertyChanged(nameof(SelectedEdition));
                }
            }
        }
        public ObservableCollection<string> AvailableEditions { get; private set; } = new ObservableCollection<string>(ServerManager.SelectedServer.editions);

        public EditionCollection()
        {
            SelectedEditionIndex = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
