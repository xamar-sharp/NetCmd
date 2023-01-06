using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using NetCmd.Infrastructure;
namespace NetCmd.Defaults
{
    internal class HttpEntry : IEntry
    {
        public string CommandName { get; } = "http-request";
        public int ParameterCount { get; } = 5;
        public string HelpText { get; } = "http-request is command for HTTP/HTTPS Requests!\n It has 6 parameters:\n 1 - URI for request, example: https://github.com/xamar-sharp/ \n 2 - HTTP METHOD for request (GET, POST, PUT, DELETE, PATCH)"+
            "\n 3 - Timeout for connection to server in milliseconds, example: 3000"+ "\n 4 - Request headers separates by format: <key>=<value>;<key1>=<value1>...\nAuthorization header value format must be: <Bearer|other auth scheme...><whitespace><auth-data>!!!\nExample: Accept=application/json;Authorization=Bearer any-jwt-string"+
            "\n 5 - Request body in JSON format, example: {\"account\":\"xamar-sharp\",\"tech\":true}"+ "\n 6 - Your proxy via (host:port), example: proxy.net:43300\nIf you don`t want have parameter in your request, you can enter _ in parameter value!!!\nOnly 3,4,5,6 parameters is optional!!!\n";
        private readonly IProgresser _progresser;
        public HttpEntry()
        {
            _progresser = StartupProgresser.Current;
        }
        public async void React(string[] args)
        {
            Print("Welcome to embedded HTTP module!");
            _progresser.Notify(CommandName, 0.01);
            HttpClientHandler handler = new HttpClientHandler();
            if (args[5] != "_")
            {
                string[] credentials = args[5].Split(":").Take(2).ToArray();
                handler.UseProxy = true;
                if (!int.TryParse(credentials[1], out int port))
                {
                    IStartup.Current.ReportError(CommandName + ":" + " Sixth parameter, after : - port is`nt integer number!");
                    _progresser.Notify(CommandName, 2);
                    return;
                }
                handler.Proxy = new WebProxy(credentials[0], Convert.ToInt32(port));
                Print($"Proxy activated on {credentials[0]}:{port}");
            }
            _progresser.Notify(CommandName, 0.1);
            handler.AutomaticDecompression = DecompressionMethods.All;
            using HttpClient client = new HttpClient(handler);
            if (args[2] != "_")
            {
                if (!int.TryParse(args[2], out int ms))
                {
                    IStartup.Current.ReportError(CommandName + ":" + " Third parameter - timeout, is`nt integer number!");
                    _progresser.Notify(CommandName, 2);
                    return;
                }
                client.Timeout = TimeSpan.FromMilliseconds(ms);
            }
            if (args[3] != "_")
            {
                string[] headers = args[3].Split(new char[] { '=', ';' });
                if (headers.Length > 0 && headers.Length % 2 == 0)
                {
                    int authIndex = headers.ToList().IndexOf("Authorization");
                    if (headers.Contains("Authorization"))
                    {
                        string[] auth = headers[authIndex + 1].Split(" ").Take(2).ToArray();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(auth[0], auth[1]);
                    }
                    if (authIndex != -1)
                    {
                        var inter = headers.ToList();
                        inter.RemoveAt(authIndex);
                        inter.RemoveAt(authIndex + 1);
                        headers = inter.ToArray();
                    }
                    for (int x = 0; x < headers.Length; x += 2)
                    {
                        client.DefaultRequestHeaders.Add(headers[x], headers[x + 1]);
                    }
                    Print("Headers initialized!");
                }
            }
            if (!Uri.IsWellFormedUriString(args[0], UriKind.Absolute))
            {
                IStartup.Current.ReportError(CommandName + ":" + "First parameter is`nt valid absolute URI!");
                _progresser.Notify(CommandName, 2);
                return;
            }
            _progresser.Notify(CommandName, 0.2);
            HttpResponseMessage response;
            switch (args[1])
            {
                case "GET":
                    response = await client.GetAsync(args[0], HttpCompletionOption.ResponseContentRead);
                    _progresser.Notify(CommandName, 0.8);
                    DisplayResponseMessage(response);
                    break;
                case "POST":
                    response = await client.PostAsync(args[0], new StringContent(args[4], Encoding.UTF8, "application/json"));
                    _progresser.Notify(CommandName, 0.8);
                    DisplayResponseMessage(response);
                    break;
                case "PUT":
                    response = await client.PostAsync(args[0], new StringContent(args[4], Encoding.UTF8, "application/json"));
                    _progresser.Notify(CommandName, 0.8);
                    DisplayResponseMessage(response);
                    break;
                case "DELETE":
                    response = await client.DeleteAsync(args[0]);
                    _progresser.Notify(CommandName, 0.8);
                    DisplayResponseMessage(response);
                    break;
                case "PATCH":
                    response = await client.PatchAsync(args[0], new StringContent(args[4], Encoding.UTF8, "application/json"));
                    _progresser.Notify(CommandName, 0.8);
                    DisplayResponseMessage(response);
                    break;
                default:
                    IStartup.Current.ReportError(CommandName + ":" + "Second parameter is`nt valid HTTP METHOD!");
                    _progresser.Notify(CommandName, 2);
                    return;
            }
            _progresser.Notify(CommandName, 2);
        }
        public async void DisplayResponseMessage(HttpResponseMessage msg)
        {
            Print(msg.IsSuccessStatusCode ? "SUCCESSFULLY" : "FAILED");
            Print("Status code: " + msg.StatusCode);
            Print("Response Body:");
            Print(await msg.Content.ReadAsStringAsync());
            Print("\nResponse headers:");
            bool isfirst = true;
            foreach(var header in msg.Headers)
            {
                Print(header.Key + "=" + header.Value.Aggregate((first, sec) =>
                {
                    if (isfirst)
                    {
                        isfirst = false;
                        return first + "," + sec;
                    }
                    return "," + first + "," + sec;
                }));
            }
            isfirst = true;
            foreach(var cheader in msg.Content.Headers)
            {
                Print(cheader.Key + "=" + cheader.Value.Aggregate((first, sec) =>
                {
                    if (isfirst)
                    {
                        isfirst = false;
                        return first + "," + sec;
                    }
                    return "," + first + "," + sec;
                }));
            }
            _progresser.Notify(CommandName, 1);
        }
        public void Print(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
