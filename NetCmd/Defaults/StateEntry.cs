using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCmd.Infrastructure;
namespace NetCmd.Defaults
{
    internal class StateEntry : IEntry
    {
        private readonly IProgresser _progresser;
        public int ParameterCount { get; } = 0;
        public string CommandName { get; } = "state";
        public string HelpText { get; } = "It`s command for display current working task and her progress percentage!";
        public ConsoleColor HelpColor { get; } = ConsoleColor.Gray;
        public StateEntry()
        {
            _progresser = StartupProgresser.Current;
        }
        public void React(string[] paramsRaw)
        {
            IStartup.Current.ProgressUI();
        }
        public void Print(string message)
        {
            Console.ForegroundColor = HelpColor;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
