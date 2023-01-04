using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetCmd.Infrastructure;
namespace NetCmd.Defaults
{
    internal class StartupProgresser : IProgresser
    {
        public static StartupProgresser Current { get => new StartupProgresser(); }
        private StartupProgresser()
        {

        }
        public void Notify(string cmdName, double progress)
        {
            var startup = IStartup.Current;
            if (progress > 1)
            {
                startup.CurrentCommand = "none";
                startup.CurrentProgress = -1;
            }
            else
            {
                startup.CurrentCommand = cmdName;
                startup.CurrentProgress = progress;
                startup.ProgressUI();
            }
        }
    }
}
