using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCmd.Defaults
{
    internal sealed class RenderMap
    {
        public static char SetupOutput(string specialCode)
        {
            if(specialCode.Length > 3)
            {
                specialCode = specialCode[0..1];
            }
            if (specialCode.StartsWith('#'))
            {
                return specialCode[1];
            }
            switch (specialCode)
            {
                case "BL":
                    Console.BackgroundColor = ConsoleColor.Black;
                    return ' ';
                case "B":
                    Console.BackgroundColor = ConsoleColor.Blue;
                    return ' ';
                case "C":
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    return ' ';
                case "G":
                    Console.BackgroundColor = ConsoleColor.Green;
                    return ' ';
                case "GR":
                    Console.BackgroundColor = ConsoleColor.Gray;
                    return ' ';
                case "Y":
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    return ' ';
                case "R":
                    Console.BackgroundColor = ConsoleColor.Red;
                    return ' ';
                case "W":
                    Console.BackgroundColor = ConsoleColor.White;
                    return ' ';
                case "M":
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    return ' ';
                case "DB":
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    return ' ';
                case "DC":
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    return ' ';
                case "DM":
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    return ' ';
                case "DG":
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    return ' ';
                case "DA":
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    return ' ';
                case "DR":
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    return ' ';
                case "NL":
                    return '\n';
                default:
                    return specialCode[0];
            }
        }
    }
}
