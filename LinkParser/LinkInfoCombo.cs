using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkParser
{
    //обертка над RunningInfo и ListInfo
    class LinkInfoCombo
    {
        public string Url { get; set; }
        public int Code { get; set; }
        public int Type { get; set; }
        public int Id { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public string Login { get; set; }
        public int amount { get; set; }

    }
}
