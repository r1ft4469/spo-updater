/* NotificationQueue.cs
 * License: NCSA Open Source License
 * 
 * Copyright: Merijn Hendriks
 * AUTHORS:
 * waffle.lord
 */


using Aki.Launcher.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Timers;

namespace Aki.Launcher.Models.Launcher.Notifications
{
    public class NotificationQueue : INotifyPropertyChanged, IDisposable
    {
        public Timer queueTimer = new Timer();
        private Timer animateChangeTimer = new Timer(230);
        private Timer animateCloseTimer = new Timer(230);

        public ObservableCollection<NotificationItem> queue { get; set; } = new ObservableCollection<NotificationItem>();

        private bool _ShowBanner;
        public bool ShowBanner
        {
            get => _ShowBanner;
            set
            {
                if (_ShowBanner != value)
                {
                    _ShowBanner = value;
                    RaisePropertyChanged(nameof(ShowBanner));
                }
            }
        }

        public NotificationQueue(int ShowTimeInMiliseconds)
        {
            ShowBanner = false;
            queueTimer.Interval = ShowTimeInMiliseconds;
            queueTimer.Elapsed += QueueTimer_Elapsed;

            animateChangeTimer.Elapsed += AnimateChange_Elapsed;
            animateCloseTimer.Elapsed += AnimateCloseTimer_Elapsed;
        }

        private void AnimateCloseTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            animateCloseTimer.Stop();

            queue.Clear();
            queueTimer.Stop();
        }

        public void CloseQueue()
        {
            ShowBanner = false;
            animateCloseTimer.Start();
        }

        private void CheckAndShowNotifications()
        {
            if (!queueTimer.Enabled)
            {
                ShowBanner = true;
                queueTimer.Start();
            }
        }

        public void Enqueue(string Message, bool AutowNext = false, bool NoDefaultButton = false)
        {
            if (queue.Where(x => x.Message == Message).Count() == 0)
            {
                if (NoDefaultButton)
                {
                    queue.Add(new NotificationItem(Message));
                }
                else
                {
                    queue.Add(new NotificationItem(Message, LocalizationProvider.Instance.ok, () => { }));
                }

                CheckAndShowNotifications();

                if (AutowNext && queue.Count == 2)
                {
                    Next(true);
                }
            }
        }

        public void Enqueue(string Message, string ButtonText, Action ButtonAction, bool AllowNext = false)
        {
            if (queue.Where(x => x.Message == Message && x.ButtonText == ButtonText).Count() == 0)
            {
                queue.Add(new NotificationItem(Message, ButtonText, ButtonAction));
                CheckAndShowNotifications();

                if (AllowNext && queue.Count == 2)
                {
                    Next(true);
                }
            }
        }

        public void Next(bool ResetTimer = false)
        {
            if (queue.Count - 1 <= 0)
            {
                CloseQueue();
                return;
            }

            if (ResetTimer)
            {
                queueTimer.Stop();
                queueTimer.Start();
            }

            ShowBanner = false;
            animateChangeTimer.Start();
        }

        private void QueueTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Next();
        }

        private void AnimateChange_Elapsed(object sender, ElapsedEventArgs e)
        {
            animateChangeTimer.Stop();

            if (queue.Count > 0)
            {
                queue.RemoveAt(0);
            }

            ShowBanner = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public void Dispose()
        {
            queueTimer.Dispose();
            animateChangeTimer.Dispose();
            animateCloseTimer.Dispose();
        }
    }
}
