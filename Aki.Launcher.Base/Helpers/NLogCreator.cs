/* NLogCreator.cs
 * License: NCSA Open Source License
 * 
 * Copyright: Merijn Hendriks
 * AUTHORS:
 * waffle.lord
 */

using Aki.Launcher.MiniCommon;
using System.Text;

namespace Aki.Launcher.Helpers
{
    public static class NLogCreator
    {
        public static string NLogFilePath = VFS.Combine(LauncherSettingsProvider.Instance.GamePath, "EscapeFromTarkov_Data/Managed/Nlog.dll.nlog");
        
        private static string nlogFileContents = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
<nlog xmlns = ""http://www.nlog-project.org/schemas/NLog.xsd"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <targets> 
    <target name = ""Aki.Loader"" xsi:type=""Aki.Loader"" /> 
  </targets> 
</nlog>";

        public static bool Create()
        {
            if (VFS.Exists(NLogFilePath))
            {
                return true;
            }

            VFS.WriteFile(NLogFilePath, nlogFileContents, false, Encoding.UTF8);
            return VFS.Exists(NLogFilePath);
        }
    }
}
