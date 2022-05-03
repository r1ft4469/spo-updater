using System;

namespace Aki.Launcher.CLI.Helpers
{
    public static class CLIHelper
    {
        /// <summary>
        /// Read a password line from the console, masking the typed characters with asterisks.
        /// </summary>
        /// <returns>The entered text.</returns>
        public static string ReadPassword()
        {
            int numStarsPrinted = 0;
            var value = "";

            for (;;)
            {
                var key = Console.ReadKey(intercept: true);
                if (key.Key == ConsoleKey.Enter)
                    break;

                if (key.Key == ConsoleKey.Backspace)
                {
                    if (value.Length > 0)
                        value = value.Substring(0, value.Length - 1);
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    value += key.KeyChar;
                }

                var starsToPrint = value.Length;
                var delta = starsToPrint - numStarsPrinted;

                while (delta > 0)
                {
                    Console.Write("*");
                    --delta;
                }

                while (delta < 0)
                {
                    Console.Write("\b \b");
                    ++delta;
                }

                numStarsPrinted = starsToPrint;
            }
            
            Console.WriteLine("");
            return value;
        }
    }
}