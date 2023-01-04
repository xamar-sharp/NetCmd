using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NetCmd.Defaults;
namespace NetCmd.Infrastructure
{
    internal class EntryBuilder
    {
        private readonly IList<IEntry> _entries;
        private readonly HttpClient _client;
        public EntryBuilder()
        {
            _entries = new List<IEntry>(4);
            _client = new HttpClient(new SocketsHttpHandler() { AutomaticDecompression = DecompressionMethods.All }) { Timeout = TimeSpan.FromSeconds(300) };
        }
        public EntryBuilder AddCommand(IEntry entry)
        {
            _entries.Add(entry);
            return this;
        }
        public async Task<EntryBuilder> AddPackage(IEntryPackage package)
        {
            var entries = await package.ProvideEntriesAsync();
            for(int x = 0; x < entries.Count; x++)
            {
                AddCommand(entries[x]);
            }
            return this;
        }
        public async Task DownloadCommandAsync(string url)
        {
            _entries.Add(JsonConvert.DeserializeObject<IEntry>(await _client.GetStringAsync(url)));
        }
        public IStartup Build()
        {
            return new Startup(this, _entries);
        }
    }
}
