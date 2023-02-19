using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NetCmd.Infrastructure;
namespace NetCmd.Defaults
{
    public sealed class DirectoryIteratorEntry : IEntry
    {
        public int ParameterCount { get; } = 1;
        public string CommandName { get; } = ">";
        public string HelpText { get; } = "Command for changing current working directory!\nIt has 1 parameters:\n 1 - Full path of directory to iterate";
        public void React(string[] args)
        {
            if (Directory.Exists(args[0]))
                IStartup.Current.CurrentDirectory = args[0];
            else
                Print("Such directory not exists!");
            StartupProgresser.Current.Notify(CommandName, 2);
        }
        public void Print(string msg)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
