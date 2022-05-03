﻿/* GameStarterResult.cs
 * License: NCSA Open Source License
 * 
 * Copyright: Merijn Hendriks
 * AUTHORS:
 * Merijn Hendriks
 * waffle.lord
 */

using Aki.Launcher.Helpers;

namespace Aki.Launcher.Models.Launcher
{
    public class GameStarterResult
    {
        public bool Succeeded => Message == null;
        public string Message { get; } = null;
        protected GameStarterResult(int ServerStatus)
        {
            switch (ServerStatus)
            {
                case 1:
                    break;
                case -1:
                    Message = LocalizationProvider.Instance.installed_in_live_game_warning;
                    break;

                case -2:
                    Message = LocalizationProvider.Instance.no_official_game_warning;
                    break;

                case -3:
                    Message = LocalizationProvider.Instance.failed_to_receive_patches;
                    break;
                case -4:
                    Message = LocalizationProvider.Instance.failed_core_patch;
                    break;
                case -5:
                    Message = LocalizationProvider.Instance.failed_mod_patch;
                    break;
                case -6:
                    Message = LocalizationProvider.Instance.eft_exe_not_found_warning;
                    break;
                case -7:
                    Message = LocalizationProvider.Instance.nlog_modify_failed;
                    break;
                default:
                    Message = LocalizationProvider.Instance.login_failed;
                    break;
            }
        }

        public static GameStarterResult FromSuccess() =>
            new GameStarterResult(1);
        public static GameStarterResult FromError(int ServerStatus) =>
            new GameStarterResult(ServerStatus);
    }
}
