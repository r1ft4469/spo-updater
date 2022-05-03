using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aki.Launcher.Interfaces;
using Aki.Launcher.Models.Launcher;

namespace Aki.Launcher.CLI.Helpers
{
    public class GameStarterFrontendCLI: IGameStarterFrontend
    {
        public async Task CompletePatchTask(IAsyncEnumerable<PatchResultInfo> task)
        {
            var iter = task.GetAsyncEnumerator();
            while (await iter.MoveNextAsync())
            {
                var info = iter.Current;
                if (!info.OK)
                {
                    Console.WriteLine($"Error applying patches: {info.Status.ToString()}");
                    Console.Write("Ignore and continue [y/N]: ");
                    if ((Console.ReadLine() ?? "").ToLower() != "y")
                        Environment.Exit(1);
                }
            }
        }
    }
}