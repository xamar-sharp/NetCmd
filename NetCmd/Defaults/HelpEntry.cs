using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCmd.Infrastructure;
namespace NetCmd.Defaults
{
    public sealed class HelpEntry : IEntry
    {
        public int ParameterCount { get; } = 1;
        public string CommandName { get; } = "help";
        public string HelpText { get; } = "Ohhh it`s COMMAND for ... COMMAND hints?";
        public ConsoleColor HelpColor { get; } = ConsoleColor.Cyan;
        public void React(string[] args)
        {
            if (String.IsNullOrWhiteSpace(args[0]))
            {
                IStartup.Current.ReportError(CommandName + ":" + "First parameter must be $<command-name> OR $all");
                return;
            }
            if (args[0] == "all")
            {
                Print("Format of input to console:\n<command-name> $<first-param> [$<second-param> $<third-param>...]\n");
                var entries = IStartup.Current.Builder.GetAllEntries();
                foreach (IEntry entry in entries)
                {
                    Console.ForegroundColor = entry.HelpColor;
                    Print(entry.CommandName + ": " + entry.HelpText);
                    Console.ResetColor();
                }
            }
            else
            {
                var entry = IStartup.Current.Builder.GetEntry(args[0]);
                Console.ForegroundColor = entry.HelpColor;
                Print(entry.CommandName + ": " + entry.HelpText);
                Console.ResetColor();
            }
        }
        public void Print(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
