using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCmd.Infrastructure
{
    internal interface IStartup : IUIMessager
    {
        static IStartup Current { get; internal set; }
        string CurrentDirectory { get; set; }
        double CurrentProgress { get; set; }
        string CurrentCommand { get; set; }
        EntryBuilder Builder { get; }
        void ProgressUI();
        void ReportError(string msg);
        void Run();
    }
}
