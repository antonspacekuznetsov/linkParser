using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkParser
{
    interface IRegularEx
    {
        bool RegExRequest(string line, string pattern);
    }
}
