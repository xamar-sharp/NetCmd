using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCmd.Infrastructure
{
    internal interface IEntry : IUIMessager
    {
        public int ParameterCount { get; }
        public string CommandName { get; }
        public string HelpText { get; }
        public void React(string[] paramsRaw);
    }
}
