using System.ComponentModel;

namespace Aki.Launch.Models.Aki
{
    public class AkiVersion : INotifyPropertyChanged
    {
        public int Major;
        public int Minor;
        public int Build;

        public bool HasTag => Tag != null;

        private string _Tag = null;
        public string Tag
        {
            get => _Tag;
            set
            {
                if(_Tag != value)
                {
                    _Tag = value;
                    RaisePropertyChanged(nameof(Tag));
                    RaisePropertyChanged(nameof(HasTag));
                }
            }
        }

        public void ParseVersionInfo(string AkiVersion)
        {
            if (AkiVersion.Contains('-'))
            {
                string[] versionInfo = AkiVersion.Split('-');

                AkiVersion = versionInfo[0];

                Tag = versionInfo[1];
                return;
            }

            string[] splitVersion = AkiVersion.Split('.');

            if (splitVersion.Length == 3)
            {
                int.TryParse(splitVersion[0], out Major);
                int.TryParse(splitVersion[1], out Minor);
                int.TryParse(splitVersion[2], out Build);
            }
        }

        public AkiVersion() { }

        public AkiVersion(string AkiVersion)
        {
            ParseVersionInfo(AkiVersion);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
