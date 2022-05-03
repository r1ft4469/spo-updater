using Aki.Launcher.Helpers;
using Aki.Launcher.Models;
using Aki.Launcher.ViewModels;
using ReactiveUI;

namespace Aki.Launcher.Attributes
{
    public class RequireServerConnected : NavigationPreCondition
    {
        public override NavigationPreConditionResult TestPreCondition(IScreen Host)
        {
            if (ServerManager.PingServer()) return NavigationPreConditionResult.FromSuccess();

            string error = string.Format(LocalizationProvider.Instance.server_unavailable_format_1, LauncherSettingsProvider.Instance.Server.Name);

            return NavigationPreConditionResult.FromError(error, new ConnectServerViewModel(Host));
        }
    }
}
