using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCmd.Infrastructure
{
    internal interface IUIMessager
    {
        void Print(string msg);
    }
}
