/* MenuBarItem.cs
 * License: NCSA Open Source License
 * 
 * Copyright: Merijn Hendriks
 * AUTHORS:
 * waffle.lord
 */

using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Aki.Launcher.Models.Launcher
{
    public class MenuBarItem : INotifyPropertyChanged
    {
        private string _Name;
        public string Name
        {
            get => _Name;
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    RaisePropertyChanged(nameof(Name));
                }
            }
        }

        private bool _IsSelected;
        public bool IsSelected
        {
            get => _IsSelected;
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    RaisePropertyChanged(nameof(IsSelected));
                }
            }
        }

        private Action _ItemAction;
        public Action ItemAction
        {
            get => _ItemAction;
            set
            {
                if (_ItemAction != value)
                {
                    _ItemAction = value;
                    RaisePropertyChanged(nameof(ItemAction));
                }
            }
        }

        public Func<Task<bool>> CanUseAction = async () => await Task.FromResult(true);

        public Action OnFailedToUseAction = null;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
