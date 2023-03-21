using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCmd.Infrastructure;
namespace NetCmd.Defaults
{
    internal sealed class ConsoleEntry : IEntry
    {
        public string CommandName { get; } = "console";
        public string HelpText { get; } = "It configures console on your settings!\nIt has 3 parameters:\n" +
            " 1 - Text color\n 2 - Background color\n 3 - Title\n Optional parameters value is _";
        public int ParameterCount { get; } = 3;
        public ConsoleColor HelpColor { get; } = ConsoleColor.DarkGray;
        public void React(string[] args)
        {
            if (args[0] != "_")
            {
                if (int.TryParse(args[0], out int tc))
                {
                    Console.ForegroundColor = (ConsoleColor)tc;
                }
            }
            if (args[1] != "_")
            {
                if (int.TryParse(args[1], out int bc))
                {
                    Console.BackgroundColor = (ConsoleColor)bc;
                }
            }
            if (args[2] != "_")
            {
                Console.Title = args[2];
            }
            StartupProgresser.Current.Notify(CommandName, 2);
        }
        public void Print(string msg)
        {
            throw new NotImplementedException();
        }
    }
}
