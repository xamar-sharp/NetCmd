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
            this.WithEntry(new DnsEntry()).WithEntry(new InstallEntry()).WithEntry(new HelpEntry()).
                WithEntry(new HttpEntry()).WithEntry(new StateEntry()).WithEntry(new LoaderEntry()).
                WithEntry(new ProcessStartEntry()).WithEntry(new DeadEntry()).WithEntry(new ReaderEntry()).
                WithEntry(new DirectoryIteratorEntry()).WithEntry(new DriveEntry()).WithEntry(new SpeechEntry()).
                WithEntry(new ConsoleEntry()).WithEntry(new AscIIDrawEntry());
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
