/* ClientConfig.cs
 * License: NCSA Open Source License
 * 
 * Copyright: Merijn Hendriks
 * AUTHORS:
 * Merijn Hendriks
 */


namespace Aki.Launcher
{
    public class ClientConfig
    {
        public string BackendUrl;
        public string Version;

        public ClientConfig()
        {
            BackendUrl = "http://127.0.0.1:6969";
            Version = "live";
        }

        public ClientConfig(string backendUrl)
        {
            BackendUrl = backendUrl;
            Version = "live";
        }
    }
}
