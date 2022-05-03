using Aki.Launcher.Helpers;
using Aki.Launcher.Interfaces;
using Aki.Launcher.Models.Launcher;
using Aki.Launcher.ViewModels.Dialogs;
using Aki.Launcher.ViewModels.Notifications;
using Avalonia.Controls.Notifications;
using Splat;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aki.Launcher.Models
{
    public class GameStarterFrontend : IGameStarterFrontend
    {
        private WindowNotificationManager notificationManager => Locator.Current.GetService<WindowNotificationManager>();

        public async Task CompletePatchTask(IAsyncEnumerable<PatchResultInfo> task)
        {
            notificationManager.Show(new AkiNotificationViewModel(null, "", $"{LocalizationProvider.Instance.patching} ..."));

            var iter = task.GetAsyncEnumerator();
            while (await iter.MoveNextAsync())
            {
                var info = iter.Current;
                if (!info.OK)
                {
                    if(info.Status == ByteBanger.PatchResultType.InputChecksumMismatch)
                    {
                        var result = await DialogHost.DialogHost.Show(new ConfirmationDialogViewModel(null, LocalizationProvider.Instance.file_mismatch_dialog_message));

                        if(result != null && result is bool confirmation && !confirmation)
                        {
                            notificationManager.Show(new AkiNotificationViewModel(null, "", LocalizationProvider.Instance.failed_core_patch, NotificationType.Error));
                            throw new TaskCanceledException();
                        }
                    }
                    else
                    {
                        notificationManager.Show(new AkiNotificationViewModel(null, "", LocalizationProvider.Instance.failed_core_patch, NotificationType.Error));
                        throw new TaskCanceledException();
                    }
                }
            }
        }
    }
}
