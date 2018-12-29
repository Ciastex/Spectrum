﻿using System;

namespace Spectrum.Bootstrap
{
    public class Log
    {
        public static void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            WriteMessage('i', message);
            Console.ResetColor();
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            WriteMessage('!', message);
            Console.ResetColor();
        }

        public static void Exception(Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            WriteMessage('e', e.Message);

            if (!string.IsNullOrEmpty(e.StackTrace))
                Console.WriteLine(e.StackTrace);

            Console.ResetColor();
        }

        private static void WriteMessage(char symbol, string message)
        {
            Console.WriteLine($"[{symbol}][{DateTime.Now}] {message}");
        }
    }
}
