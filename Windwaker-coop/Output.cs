using System;
using System.Windows.Media;
using Windwaker_coop.Services;

namespace Windwaker_coop
{
    static class Output
    {
        private static Color MapColor(ConsoleColor color)
        {
            return color switch
            {
                ConsoleColor.Red => Colors.Red,
                ConsoleColor.Green => Colors.LimeGreen,
                ConsoleColor.Blue => Colors.DodgerBlue,
                ConsoleColor.Yellow => Colors.Gold,
                ConsoleColor.Gray => Colors.Gray,
                ConsoleColor.Magenta => Colors.Magenta,
                ConsoleColor.DarkMagenta => Colors.DarkMagenta,
                ConsoleColor.Cyan => Colors.Cyan,
                _ => Colors.White,
            };
        }

        public static void error(string message)
        {
            LogService.Instance.AddLog("Error: " + message, Colors.Red);
        }

        //Level 1 - decent stuff to know, level 2 - deep stuff
        public static void debug(string message, byte level)
        {
            if (level <= Program.config.debugLevel)
            {
                if (level == 1)
                    LogService.Instance.AddLog(message, Colors.Magenta);
                else if (level == 2)
                    LogService.Instance.AddLog(message, Colors.DarkMagenta);
                else
                {
                    error("Invalid debug level");
                    return;
                }
            }
        }

        public static void text(string message, ConsoleColor color = ConsoleColor.White, bool newLine = true)
        {
            LogService.Instance.AddLog(message, MapColor(color));
        }

        public static void clear()
        {
            LogService.Instance.Clear();
        }
    }
}
