/* ProgressInfo.cs
 * License: NCSA Open Source License
 * 
 * Copyright: Merijn Hendriks
 * AUTHORS:
 * waffle.lord
 */

namespace Aki.Launcher.Models.Launcher
{
    public class ProgressInfo
    {
        public int Percentage { get; private set; }
        public string Message { get; private set; }

        public ProgressInfo(int Percentage, string Message)
        {
            this.Percentage = Percentage;
            this.Message = Message;
        }
    }
}
