using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using NetCmd.Infrastructure;
namespace NetCmd.Defaults
{
    public sealed class ProcessStartEntry : IEntry
    {
        public int ParameterCount { get; } = 1;
        public string CommandName { get; } = "exec";
        public string HelpText { get; } = "Execute any file if extension supports!\n It has 1 parameters:\n 1 - Filename with extension in current working directory to execute!";
        public void React(string[] args)
        {
            string execPath = Path.Combine(IStartup.Current.CurrentDirectory, args[0]);
            if (File.Exists(execPath))
            {
                try
                {
                    switch (Path.GetExtension(execPath))
                    {
                        case ".class":
                            //Only on Windows Platform! Input console stream will be locked because main multithreading system!
                            var processJava = Process.Start(new ProcessStartInfo() {UseShellExecute=false, FileName = "cmd", RedirectStandardInput = true });
                            processJava.StandardInput.WriteLine($"cd {IStartup.Current.CurrentDirectory}");
                            processJava.StandardInput.WriteLine($"java {Path.GetFileNameWithoutExtension(args[0])}");
                            processJava.WaitForExit();
                            break;
                        case ".c":
                            //Only on Windows Platform! Input stream will be locked
                            var processC = Process.Start(new ProcessStartInfo() { UseShellExecute = false, FileName = "cmd", RedirectStandardInput = true });
                            processC.StandardInput.WriteLine($"cd {IStartup.Current.CurrentDirectory}");
                            string futurePath = Path.GetFileNameWithoutExtension(args[0])+".exe";
                            processC.StandardInput.WriteLine($"gcc {args[0]} -o {futurePath}");
                            processC.StandardInput.WriteLine($"{futurePath}");
                            processC.WaitForExit();
                            break;
                        default:
                            Process.Start(execPath);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    IStartup.Current.ReportError($"I can`t execute this file: {execPath}.\n Details: {ex.Message}");
                }
            }
            else
            {
                Print(CommandName + ": " + "Such file not exists - " + execPath);
            }
            StartupProgresser.Current.Notify(CommandName, 2);
        }
        public void Print(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
