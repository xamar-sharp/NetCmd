using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using NetCmd.Infrastructure;
namespace NetCmd.Defaults
{
    internal class InstallEntry : IEntry
    {
        private static readonly HttpClient _client;
        static InstallEntry()
        {
            ServicePointManager.DefaultConnectionLimit = Environment.ProcessorCount;
            ServicePointManager.DnsRefreshTimeout = 300;
            _client = new HttpClient(new SocketsHttpHandler() { AutomaticDecompression = DecompressionMethods.All }) { Timeout = TimeSpan.FromSeconds(300) };
        }
        public InstallEntry()
        {
            _progresser = StartupProgresser.Current;
        }
        private readonly IProgresser _progresser;
        public int ParameterCount { get; } = 1;
        public string CommandName { get; } = "remote-pack";
        public string HelpText { get; } = "remote-pack is command for dynamically load commands!\nIt has 1 parameters:\n1 - URI for such command-model as \n{'CommandName':'remote-command','ParameterCount':2,'AssemblyData':'Binary data for dll with command-class. He must contains [void React(string[] params)] method!'}\n'}";
        public ConsoleColor HelpColor { get; } = ConsoleColor.DarkYellow;
        public void React(string[] args)
        {
            Print("Welcome to the NetCmd Installer module!");
            if (!Uri.IsWellFormedUriString(args[0], UriKind.Absolute))
            {
                IStartup.Current.ReportError(CommandName + ":" + "This string is not well formed uri -> " + args[0]);
                _progresser.Notify(CommandName, 2);
                return;
            }
            _progresser.Notify(CommandName, 0.01);
            //ONLY ON IN LIBRARY
            var response = _client.GetAsync(args[0], HttpCompletionOption.ResponseContentRead).GetAwaiter().GetResult();
            _progresser.Notify(CommandName, 0.5);
            if (!response.IsSuccessStatusCode)
            {
                IStartup.Current.ReportError(CommandName + ":" + "Server sent failed status code -> " + response.StatusCode);
                _progresser.Notify(CommandName, 2);
                return;
            }
            long length = response.Content.Headers.ContentLength.Value;
            Print($"Downloaded {length / 1024}kb module!");
            try
            {
                RemoteEntry pack = JsonConvert.DeserializeObject<RemoteEntry>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                _progresser.Notify(CommandName, 0.7);
                var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\NetCmd\\";
                var filePath = path + pack.CommandName + "_" + pack.ParameterCount + ".dll";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                File.WriteAllBytes(filePath, pack.AssemblyData);
                Print($"Module was successfully saved to FileSystem -> " + filePath);
                _progresser.Notify(CommandName, 2);
            }
            catch (Exception ex)
            {
                IStartup.Current.ReportError(CommandName + ":" + "Error in remote model OR in saving .dll of module into your FileSystem -> " + ex.Message);
                _progresser.Notify(CommandName, 2);
                return;
            }
        }
        public void Print(string msg)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
