using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using NetCmd.Infrastructure;
namespace NetCmd.Defaults
{
    internal class NeToolsPackage : IEntryPackage
    {
        private readonly IList<IEntry> _networkTools;
        public NeToolsPackage()
        {
            _networkTools = new List<IEntry>(4);
            this.WithEntry(new DnsEntry()).WithEntry(new InstallEntry());
        }
        public IEntryPackage WithEntry(IEntry entry)
        {
            _networkTools.Add(entry);
            return this;
        }
        public async Task<IList<IEntry>> ProvideEntriesAsync()
        {
            await Task.Yield();
            return _networkTools;
        }
    }
}
