/* LoginRequestData.cs
 * License: NCSA Open Source License
 * 
 * Copyright: Merijn Hendriks
 * AUTHORS:
 * Merijn Hendriks
 */


namespace Aki.Launcher
{
    public struct LoginRequestData
    {
        public string username;
        public string password;

        public LoginRequestData(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}
