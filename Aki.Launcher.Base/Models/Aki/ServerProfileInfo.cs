/* ServerProfileInfo.cs
 * License: NCSA Open Source License
 * 
 * Copyright: Merijn Hendriks
 * AUTHORS:
 * waffle.lord
 */

namespace Aki.Launcher.Models.Aki
{
    public class ServerProfileInfo
    {
        public string username { get; set; }
        public string nickname { get; set; }
        public string side { get; set; }
        public int currlvl { get; set; }
        public long currexp { get; set; }
        public long prevexp { get; set; }
        public long nextlvl { get; set; }
        public int maxlvl { get; set; }
        public AkiData akiData { get; set; }
    }
}
