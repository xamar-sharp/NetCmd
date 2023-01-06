using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using NetCmd.Infrastructure;
namespace NetCmd.Defaults
{
    internal class DeadEntry : IEntry
    {
        public int ParameterCount { get; } = 2;
        public string CommandName { get; } = "die";
        public string HelpText { get; } = "It`s command for NetCmd death *_*";
        public DeadEntry()
        {
        }
        public void React(string[] paramsRaw)
        {
            Print("\'(*_*)\'");
            Process.GetCurrentProcess().Kill();
        }
        public void Print(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
