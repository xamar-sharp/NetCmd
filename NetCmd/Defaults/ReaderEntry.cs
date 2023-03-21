using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NetCmd.Infrastructure;
using System.IO;
using System.IO.Compression;
namespace NetCmd.Defaults
{
    internal class ReaderEntry : IEntry
    {
        public int ParameterCount { get; } = 1;
        public string CommandName { get; } = "open";
        public string HelpText { get; } = "Command for formatted reading of any file.\nIt has 1 parameter:\n1 - URI to file for load";
        public ConsoleColor HelpColor { get; } = ConsoleColor.Green;
        private readonly IProgresser _progresser;
        public ReaderEntry()
        {
            _progresser = StartupProgresser.Current;
        }
        public async void React(string[] args)
        {
            if (!File.Exists(args[0]))
            {
                IStartup.Current.ReportError(CommandName + ": " + args[0] + " not exists!");
            }
            else
            {
                Print("Target file: " + args[0] + "\n\n");
                _progresser.Notify(CommandName, 0.01);
                switch (Path.GetExtension(args[0]))
                {
                    case ".zip":
                        string future = Path.Combine(Environment.CurrentDirectory, Path.GetFileNameWithoutExtension(args[0]));
                        _progresser.Notify(CommandName, 0.1);
                        ZipFile.ExtractToDirectory(args[0], Environment.CurrentDirectory);
                        _progresser.Notify(CommandName, 0.5);
                        FileInfo[] added = new DirectoryInfo(future).GetFiles("*", SearchOption.AllDirectories);
                        _progresser.Notify(CommandName, 0.7);
                        foreach (FileInfo fileSystem in added)
                        {
                            Print(fileSystem.FullName + "   " + fileSystem.Attributes + "   " + (fileSystem as FileInfo).Length + "bytes");
                        }
                        Directory.Delete(future, true);
                        break;
                    default:
                        Print(await File.ReadAllTextAsync(args[0]));
                        break;
                }
            }
            _progresser.Notify(CommandName, 1);
            _progresser.Notify(CommandName, 2);
        }
        public void Print(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
