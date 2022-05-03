/* LogManager.cs
 * License: NCSA Open Source License
 * 
 * Copyright: Merijn Hendriks
 * AUTHORS:
 * Merijn Hendriks
 */


using System;
using System.IO;

namespace Aki.Launcher.Controllers
{
    /// <summary>
    /// LogManager
    /// </summary>
    public class LogManager
    {
        private static LogManager _instance;
        public static LogManager Instance => _instance ?? (_instance = new LogManager());
        private string filepath;

        public LogManager()
        {
            filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "user", "logs");
        }

        public void Write(string text)
        {
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            string filename = Path.Combine(filepath, "launcher.log");
            File.WriteAllLines(filename, new[] { $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]{text}" });
        }

        public void Debug(string text) => Write($"[Debug]{text}");

        public void Info(string text) => Write($"[Info]{text}");

        public void Warning(string text) => Write($"[Warning]{text}");

        public void Error(string text) => Write($"[Error]{text}");
    }
}
