using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace LinkParser
{
    class RegExWorker:IRegularEx
    {
        public RegExWorker()
        {

        }
        public bool RegExRequest(string line, string pattern)
        {
            if (Regex.IsMatch(line, pattern))
                return true;
            else
                return false;
        }
    }
}
