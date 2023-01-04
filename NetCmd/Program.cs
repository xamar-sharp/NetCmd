using System;
using System.Threading.Tasks;
using NetCmd.Infrastructure;
using NetCmd.Defaults;
namespace NetCmd
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            (await new EntryBuilder().AddPackage(new NeToolsPackage())).Build().Run();
        }
    }
}
