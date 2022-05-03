using Aki.Launcher.Helpers;
using Aki.Launcher.Models;
using Aki.Launcher.ViewModels;
using ReactiveUI;

namespace Aki.Launcher.Attributes
{
    public class RequireLoggedIn : NavigationPreCondition
    {
        public override NavigationPreConditionResult TestPreCondition(IScreen Host)
        {
            AccountStatus status = AccountManager.Login(AccountManager.SelectedAccount.username, AccountManager.SelectedAccount.password);

            if (status == AccountStatus.OK) return NavigationPreConditionResult.FromSuccess();

            return NavigationPreConditionResult.FromError(LocalizationProvider.Instance.login_failed, new ConnectServerViewModel(Host));
        }
    }
}
