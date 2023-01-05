using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using NetCmd.Infrastructure;
namespace NetCmd.Defaults
{
    internal class DnsEntry : IEntry
    {
        private readonly IProgresser _progresser;
        public int ParameterCount { get; } = 2;
        public string CommandName { get; } = "dns-request";
        public string HelpText { get; } = "dns-request is command for DNS recursive Requests!\n It has 2 parameters:\n 1 - domain to request, example: github.com \n 2 - count of records for catch, example: 2";
        public DnsEntry()
        {
            _progresser = StartupProgresser.Current;
        }
        public void React(string[] paramsRaw)
        {
            Print("Welcome to the DNS-client!");
            _progresser.Notify(CommandName, 0.01);
            IPHostEntry entry = Dns.GetHostEntry(paramsRaw[0]);
            if (int.TryParse(paramsRaw[1], out int results))
            {
                var addresses = entry.AddressList;
                addresses = addresses.Length <= results ? addresses : addresses.Take(results).ToArray();
                for (int x = 0; x < addresses.Length; x++)
                {
                    Print(addresses[x].ToString());
                    _progresser.Notify(CommandName, (x + 1) / (double)addresses.Length);
                }
            }
            else
            {
                IStartup.Current.ReportError("dns-request: Invalid second argument. It must be positive number!");
            }
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
