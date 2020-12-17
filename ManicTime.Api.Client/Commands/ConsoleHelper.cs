using System;

namespace ManicTime.Api.Client.Commands
{
    public static class ConsoleHelper
    {
        public static string ReadValue(string label)
        {
            Console.Write(label);
            return Console.ReadLine();
        }

        public static string ReadMaskedValue(string label)
        {
            Console.Write(label);
            string password = string.Empty;
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    Console.Write("\b \b");
                    password = password[..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    password += keyInfo.KeyChar;
                }
            } while (keyInfo.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return password;
        }
    }
}
