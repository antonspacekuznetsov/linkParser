using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkParser
{
    interface IParser
    {
        void Parse(ref List<LinkInfo> parts, string link);
    }
}
