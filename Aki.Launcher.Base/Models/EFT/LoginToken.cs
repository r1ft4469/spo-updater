/* LoginToken.cs
 * License: NCSA Open Source License
 * 
 * Copyright: Merijn Hendriks
 * AUTHORS:
 * Merijn Hendriks
 */


namespace Aki.Launcher
{
    public struct LoginToken
    {
        public string username;
        public string password;
        public bool toggle;
        public long timestamp;

        public LoginToken(string username, string password)
        {
            this.username = username;
            this.password = password;
            toggle = true;
            timestamp = 0;
        }
    }
}
