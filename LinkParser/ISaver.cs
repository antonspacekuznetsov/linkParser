using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkParser
{
    interface ISaver
    {
        //void Save(ref List<LinkInfo> parts);
        void GenerateHTML(IDBSaver dbsaver);
    }
}
