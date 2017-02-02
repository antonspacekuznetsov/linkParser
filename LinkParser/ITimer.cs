using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkParser
{
    interface ITimer
    {
        void Start();
        void Stop();
        DateTime TimeStart { get; }
        DateTime TimeStop { get; }
    }
}
