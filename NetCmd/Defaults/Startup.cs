﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Runtime.Loader;
using System.Reflection;
using NetCmd.Infrastructure;
namespace NetCmd.Defaults
{
    internal sealed class Startup : IStartup
    {
        private readonly IList<IEntry> _commands;
        public EntryBuilder Builder { get; }
        public double CurrentProgress { get; set; }
        public string CurrentCommand { get; set; }
        private IDictionary<string, KeyValuePair<int,KeyValuePair<object, MethodInfo>>> _externalCommandsCache;
        private readonly AssemblyLoadContext _ctx;
        public Startup(EntryBuilder builder, IList<IEntry> entries)
        {
            IStartup.Current = this;
            Builder = builder;
            CurrentCommand = "none";
            CurrentProgress = 0;
            _commands = entries;
            _ctx = new AssemblyLoadContext("Module_Loader", false);
            _externalCommandsCache = new Dictionary<string, KeyValuePair<int,KeyValuePair<object, MethodInfo>>>(4);
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
                    var parameters = rawText.Substring(lastIndex);
                    string[] paramsRaw;
                    if (command is null)
                    {
                        if (_externalCommandsCache.TryGetValue(cmdName, out KeyValuePair<int,KeyValuePair<object, MethodInfo>> data))
                        {
                            paramsRaw = parameters.Split(" $").Skip(1).Take(data.Key).ToArray();
                            CurrentCommand = cmdName;
                            Console.WriteLine();
                            data.Value.Value.Invoke(data.Value.Key, new object[] { paramsRaw });
                            CurrentCommand = "none";
                            continue;
                        }
                        var dirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "NetCmd");
                        if (Directory.Exists(dirPath))
                        {
                            string filePath = Directory.GetFiles(dirPath, "*", SearchOption.TopDirectoryOnly).FirstOrDefault(f => Path.GetFileName(f).StartsWith(cmdName) && f.EndsWith(".dll"));
                            if (filePath is not null)
                            {
                                string fileName = Path.GetFileName(filePath);
                                string rawParamCount = fileName.Substring(cmdName.Length + 1, fileName.LastIndexOf(".") - (cmdName.Length + 1));
                                int paramCount = Convert.ToInt32(rawParamCount);
                                paramsRaw = parameters.Split(" $").Skip(1).Take(paramCount).ToArray();
                                Assembly asm = _ctx.LoadFromAssemblyPath(filePath);
                                Type moduleType = asm.GetType(asm.GetName().Name + "." + KebabToPascalCase(cmdName) + "Entry");
                                object moduleInst = Activator.CreateInstance(moduleType);
                                MethodInfo external = moduleType.GetMethod("React", BindingFlags.Public | BindingFlags.Instance);
                                _externalCommandsCache.Add(cmdName, new KeyValuePair<int,KeyValuePair<object,MethodInfo>>(paramCount,new KeyValuePair<object, MethodInfo>(moduleInst, external)));
                                CurrentCommand = cmdName;
                                Console.WriteLine();
                                external.Invoke(moduleInst, new object[] { paramsRaw });
                                CurrentCommand = "none";
                                continue;
                            }
                        }
                        ReportError("Input command not exists!");
                    }
                    else
                    {
                        paramsRaw = parameters.Split(" $").Skip(1).Take(command.ParameterCount + 1).ToArray();
                        new Thread((obj) =>
                        {
                            command.React(obj as string[]);
                        }).Start(paramsRaw);
                        CurrentCommand = command.CommandName;
                        Console.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    ReportError("Syntax error.");
                }
            }
        }
        private string KebabToPascalCase(string kebab)
        {
            IList<int> indexes = new List<int>(4) { 0 };
            int offset = 0;
            for (int x = 0; x < kebab.Length; x++)
            {
                if (kebab[x] == '-')
                {
                    indexes.Add(x - offset);
                    if (indexes.Count >= 2)
                    {
                        offset++;
                    }
                }
            }
            var raw = kebab.Replace("-", "");
            var pascal = String.Empty;
            for (int x = 0; x < raw.Length; x++)
            {
                if (indexes.Contains(x))
                {
                    pascal += Char.ToUpper(raw[x]);
                }
                else
                {
                    pascal += raw[x];
                }
            }
            return pascal;
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
            Console.WriteLine($"Current command: {CurrentCommand} - {CurrentProgress * 100}%");
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
