using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCmd.Infrastructure
{
    public sealed class RemoteEntry
    {
        public string CommandName { get; set; }
        public int ParameterCount { get; set; }
        public byte[] AssemblyData { get; set; }
    }
}
