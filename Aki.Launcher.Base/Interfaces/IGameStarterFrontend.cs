using System.Collections.Generic;
using System.Threading.Tasks;
using Aki.Launcher.Models.Launcher;

namespace Aki.Launcher.Interfaces
{
    public interface IGameStarterFrontend
    {
        Task CompletePatchTask(IAsyncEnumerable<PatchResultInfo> task);
    }
}