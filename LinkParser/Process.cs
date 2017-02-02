using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
namespace LinkParser
{
    class Process
    {

        private ISmtpSender _sender;
        private IRegularEx _reqrequest;
        private IParser _parser;
        private ISaver _saver;
        private IDBSaver _dbsaver;
        private ITimer _gettime;
        private NetRequest _net;
        private string _link;

        public Process(string link)
        {
            _sender = new MailSender();
            _reqrequest = new RegExWorker();
            _net = new NetRequest(_sender);
            _saver = new FileWriter(_sender);
            _parser = new DataParser(_sender,_net);
            _dbsaver = new DbWorker(_sender);
            _gettime = new TimeWorker();
            _link = link;
        }
        //Главная исполнительная процедура, выполняется основной цикл работы 
        public void Runner()
        {

            int deep;//степень вложенности
            int amount=0;//общее количество обработанных ссылок
            Int32.TryParse(ConfigurationManager.AppSettings["deep"], out deep);
            List <LinkInfo> parts = new List<LinkInfo>();

            _gettime.Start();//Записываем время запуска
            parts.Add(new LinkInfo() { Url = _link, Code = _net.GetStatusCode(_link), Type=1 });//ининциализация, добавляем первую ссылку 
            
            //Цикл прохода по вложенным ссылкам
            for (int i = 0; i < deep; ++i)
            {
                int countList = parts.Count;
                int j = amount;
                for (; j < countList; j++)
                {
                    _parser.Parse(ref parts, parts[j].Url);
                    amount++;
                }
            }

            _gettime.Stop();// Время остановки обработки ссылок
            _dbsaver.SaveToDB(ref parts, _gettime.TimeStart, _gettime.TimeStop);//сохраняем найденные ссылки в БД
            _saver.GenerateHTML(_dbsaver);//Запускаем генерацию HTML файла

        }
    }
}
