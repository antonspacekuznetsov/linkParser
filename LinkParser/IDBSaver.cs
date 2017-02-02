using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkParser
{
    interface IDBSaver
    {
        void SaveToDB(ref List<LinkInfo> parts, DateTime start, DateTime stop);
        void GetDataFromDB(string sqlExpression, ref List<LinkInfoCombo> iParts, int priznak);

    }
}
