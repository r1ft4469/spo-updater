/* ProfileInfo.cs
 * License: NCSA Open Source License
 * 
 * Copyright: Merijn Hendriks
 * AUTHORS:
 * waffle.lord
 */

using Aki.Launch.Models.Aki;
using Aki.Launcher.Helpers;
using Aki.Launcher.MiniCommon;
using Aki.Launcher.Models.Aki;
using System;
using System.ComponentModel;
using System.IO;

namespace Aki.Launcher.Models.Launcher
{
    public class ProfileInfo : INotifyPropertyChanged
    {
        private string _Username;
        public string Username
        {
            get => _Username;
            set
            {
                if(_Username != value)
                {
                    _Username = value;
                    RaisePropertyChanged(nameof(Username));
                }
            }
        }

        private string _Nickname;
        public string Nickname
        {
            get => _Nickname;
            set
            {
                if (_Nickname != value)
                {
                    _Nickname = value;
                    RaisePropertyChanged(nameof(Nickname));
                }
            }
        }

        private string _SideImage;
        public string SideImage
        {
            get => _SideImage;
            set
            {
                if (_SideImage != value)
                {
                    _SideImage = value;
                    RaisePropertyChanged(nameof(SideImage));
                }
            }
        }

        private string _Side;
        public string Side
        {
            get => _Side;
            set
            {
                if (_Side != value)
                {
                    _Side = value;
                    RaisePropertyChanged(nameof(Side));
                }
            }
        }

        private string _Level;
        public string Level
        {
            get => _Level;
            set
            {
                if (_Level != value)
                {
                    _Level = value;
                    RaisePropertyChanged(nameof(Level));
                }
            }
        }

        private int _XPLevelProgress;
        public int XPLevelProgress
        {
            get => _XPLevelProgress;
            set
            {
                if (_XPLevelProgress != value)
                {
                    _XPLevelProgress = value;
                    RaisePropertyChanged(nameof(XPLevelProgress));
                }
            }
        }

        private long _CurrentXP;
        public long CurrentExp
        {
            get => _CurrentXP;
            set
            {
                if (_CurrentXP != value)
                {
                    _CurrentXP = value;
                    RaisePropertyChanged(nameof(CurrentExp));
                }
            }
        }

        private long _RemainingExp;
        public long RemainingExp
        {
            get => _RemainingExp;
            set
            {
                if (_RemainingExp != value)
                {
                    _RemainingExp = value;
                    RaisePropertyChanged(nameof(RemainingExp));
                }
            }
        }

        private long _NextLvlExp;
        public long NextLvlExp
        {
            get => _NextLvlExp;
            set
            {
                if (_NextLvlExp != value)
                {
                    _NextLvlExp = value;
                    RaisePropertyChanged(nameof(NextLvlExp));
                }
            }
        }

        private bool _HasData;
        public bool HasData
        {
            get => _HasData;
            set
            {
                if (_HasData != value)
                {
                    _HasData = value;
                    RaisePropertyChanged(nameof(HasData));
                }
            }
        }

        public string MismatchMessage => VersionMismatch ? LocalizationProvider.Instance.profile_version_mismath : null;

        private bool _VersionMismatch;
        public bool VersionMismatch
        {
            get => _VersionMismatch;
            set
            {
                if(_VersionMismatch != value)
                {
                    _VersionMismatch = value;
                    RaisePropertyChanged(nameof(VersionMismatch));
                }
            }
        }

        private AkiData _Aki;
        public AkiData Aki
        {
            get => _Aki;
            set
            {
                if(_Aki != value)
                {
                    _Aki = value;
                    RaisePropertyChanged(nameof(Aki));
                }
            }
        }

        public void UpdateDisplayedProfile(ProfileInfo PInfo)
        {
            if (PInfo.Side == null || string.IsNullOrWhiteSpace(PInfo.Side) || PInfo.Side == "unknown") return;

            HasData = true;
            Nickname = PInfo.Nickname;
            Side = PInfo.Side;
            SideImage = PInfo.SideImage;
            Level = PInfo.Level;
            CurrentExp = PInfo.CurrentExp;
            NextLvlExp = PInfo.NextLvlExp;
            RemainingExp = PInfo.RemainingExp;
            XPLevelProgress = PInfo.XPLevelProgress;

            Aki = PInfo.Aki;
        }

        /// <summary>
        /// Checks if the aki versions are compatible (non-major changes)
        /// </summary>
        /// <param name="CurrentVersion"></param>
        /// <param name="ExpectedVersion"></param>
        /// <returns></returns>
        private bool CompareVersions(string CurrentVersion, string ExpectedVersion)
        {
            if (ExpectedVersion == "") return false;

            AkiVersion v1 = new AkiVersion(CurrentVersion);
            AkiVersion v2 = new AkiVersion(ExpectedVersion);

            // check 'X'.x.x
            if (v1.Major != v2.Major) return false;

            // check x.'X'.x
            if(v1.Minor != v2.Minor) return false;

            //otherwise probably good
            return true;
        }

        public ProfileInfo(ServerProfileInfo serverProfileInfo)
        {
            Username = serverProfileInfo.username;
            Nickname = serverProfileInfo.nickname;
            Side = serverProfileInfo.side;

            Aki = serverProfileInfo.akiData;

            if (Aki != null)
            {
                VersionMismatch = !CompareVersions(Aki.version, ServerManager.GetVersion());
            }

            SideImage = Path.Combine(ImageRequest.ImageCacheFolder, $"side_{Side.ToLower()}.png");

            if (Side != null && !string.IsNullOrWhiteSpace(Side) && Side != "unknown")
            {
                HasData = true;
            }
            else
            {
                HasData = false;
            }

            Level = serverProfileInfo.currlvl.ToString();
            CurrentExp = serverProfileInfo.currexp;

            //check if player is max level
            if (Level == serverProfileInfo.maxlvl.ToString())
            {
                NextLvlExp = 0;
                XPLevelProgress = 100;
                return;
            }

            NextLvlExp = serverProfileInfo.nextlvl;
            RemainingExp = NextLvlExp - CurrentExp;

            long currentLvlTotal = NextLvlExp - serverProfileInfo.prevexp;

            XPLevelProgress = (int)Math.Floor((((double)currentLvlTotal) - RemainingExp) / currentLvlTotal * 100);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
