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
        public EntryBuilder()
        {
            _entries = new List<IEntry>(4);
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
        public IEntry GetEntry(string entryName)
        {
            return _entries.FirstOrDefault(e => e.CommandName == entryName);
        }
        public IList<IEntry> GetAllEntries()
        {
            return _entries;
        }
        public IStartup Build()
        {
            return new Startup(this, _entries);
        }
    }
}
