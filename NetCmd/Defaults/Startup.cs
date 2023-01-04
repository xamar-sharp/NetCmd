using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using NetCmd.Infrastructure;
namespace NetCmd.Defaults
{
    internal sealed class Startup : IStartup
    {
        private readonly IList<IEntry> _commands;
        public EntryBuilder Builder { get; }
        public double CurrentProgress { get; set; }
        public string CurrentCommand { get; set; }
        public Startup(EntryBuilder builder, IList<IEntry> entries)
        {
            IStartup.Current = this;
            Builder = builder;
            _commands = entries;
            CurrentCommand = "none";
            CurrentProgress = 0;
        }
        public void Run()
        {
            Print("Welcome to NetCmd! NetCmd is simple-dynamic CLI for your life ^_^!");
            while (true)
            {
                try
                {
                    string rawText = Console.ReadLine();
                    int lastIndex = rawText.IndexOf(" ");
                    string cmdName = rawText.Substring(0, lastIndex);
                    IEntry command = _commands.FirstOrDefault(ent => ent.CommandName == cmdName);
                    if (command is null)
                    {
                        ReportError("Input command not exists!");
                    }
                    else
                    {
                        var parameters = rawText.Substring(lastIndex);
                        string[] paramsRaw = parameters.Split(" $").Skip(1).Take(command.ParameterCount + 1).ToArray();
                        new Thread((obj) =>
                        {
                            command.React(obj as string[]);
                        }).Start(paramsRaw);
                        CurrentCommand = command.CommandName;
                    }
                }
                catch
                {
                    ReportError("Syntax error.");
                }
            }
        }
        public void ReportError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
        public void ProgressUI()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Current command: {CurrentCommand} - {CurrentProgress*100}%");
            Console.ResetColor();
        }
        public void Print(string msg)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
