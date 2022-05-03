/* ServerInfo.cs
 * License: NCSA Open Source License
 * 
 * Copyright: Merijn Hendriks
 * AUTHORS:
 * Merijn Hendriks
 */


namespace Aki.Launcher
{
    public class ServerInfo
    {
        public string backendUrl;
        public string name;
        public string[] editions;

        public ServerInfo()
        {
            backendUrl = "http://127.0.0.1:6969";
            name = "Local SPT-AKI Server";
            editions = new string[0];
        }
    }
}
