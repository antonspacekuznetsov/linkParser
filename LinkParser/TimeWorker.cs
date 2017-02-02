using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkParser
{

    //Класс работы с получением текущей даты и время
    class TimeWorker:ITimer
    {
        private DateTime _stopTime, _startTime;
        public void Start()
        {
            _startTime = DateTime.Now;
        }
        public void Stop()
        {
            _stopTime = DateTime.Now;
        }

        public DateTime TimeStart
        {
            get { return _startTime; }
        }

        public DateTime TimeStop
        {
            get { return _stopTime; }
        }

    }
}
