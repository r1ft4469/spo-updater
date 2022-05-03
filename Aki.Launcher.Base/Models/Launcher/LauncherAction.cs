/* LauncherAction.cs
 * License: NCSA Open Source License
 * 
 * Copyright: Merijn Hendriks
 * AUTHORS:
 * waffle.lord
 */


using Aki.Launcher.Converters;
using System.ComponentModel;

namespace Aki.Launcher.Models.Launcher
{
    [TypeConverter(typeof(EnumToLocaleStringConverter))]
    public enum LauncherAction
    {
        MinimizeAction,
        DoNothingAction,
        ExitAction
    }
}
