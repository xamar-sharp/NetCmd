using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using NetCmd.Infrastructure;
namespace NetCmd.Defaults
{
    internal class LoaderEntry : IEntry
    {
        private readonly IProgresser _progresser;
        public int ParameterCount { get; } = 2;
        public string CommandName { get; } = "load";
        public string HelpText { get; } = "It`s command for loading simple web-file into your filesystem!\nIt has 2 parameters:\n1 - URI to file for load\n2 - Relative filesystem path (from current working directory) - loading location";
        public ConsoleColor HelpColor { get; } = ConsoleColor.DarkCyan;
        private static readonly WebClient _client = new WebClient();
        public LoaderEntry()
        {
            _progresser = StartupProgresser.Current;
        }
        public async void React(string[] paramsRaw)
        {
            if(!Uri.IsWellFormedUriString(paramsRaw[0],UriKind.RelativeOrAbsolute))
            {
                IStartup.Current.ReportError(CommandName + ": Argument is`nt valid URI string!");
                _progresser.Notify(CommandName, 2);
                return;
            }
            Print($"{DateTime.Now}: Loading started...");
            _progresser.Notify(CommandName, 0.01);
            try
            {
                await _client.DownloadFileTaskAsync(paramsRaw[0], Path.Combine(IStartup.Current.CurrentDirectory,paramsRaw[1]));
            }
            catch
            {
                Print($"{DateTime.Now}: Error in loading process of file: {paramsRaw[0]}!");
                _progresser.Notify(CommandName, 2);
                return;
            }
            _progresser.Notify(CommandName, 1);
            Print($"{DateTime.Now}: Web-file {paramsRaw[0]} saved locally to your filesystem: {paramsRaw[1]}");
            _progresser.Notify(CommandName, 2);
        }
        public void Print(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
