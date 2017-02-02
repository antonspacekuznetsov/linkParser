using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace LinkParser
{
    //Класс работы с файлами на жестком диске
    class FileWriter:ISaver
    {
        private static string _filepath;//путь + имя к файлу, задан в файле app.config
        private ISmtpSender _errorSend;

        public FileWriter(ISmtpSender send)
        {
           FileCheker(ConfigurationManager.AppSettings["path"], ConfigurationManager.AppSettings["filename"]);
           _errorSend = send;
        }
        //Функция проверки на наличие файла и создания 
        private void FileCheker(string path, string name)
        {

            int t = 0;
            //В том случаее  если файл с таким именем уже существует
            //то запускаем цикл в котором лобавляем к имени файла значение +1 For example myfile.txt is exists! try... myfile0.txt is exists! try... myfile1.txt is not exists! OK then create this file with that name
            if (File.Exists(path + "\\" + name + ConfigurationManager.AppSettings["fileformat"]))
            {
                //цикл будет выполнятся до тех пор пока файл с таким именем не будет существовать
                while (File.Exists(path + "\\" + name + t.ToString() + ConfigurationManager.AppSettings["fileformat"]))
                {
                    t++;
                }
                _filepath = path + "\\" + name + t.ToString() + ConfigurationManager.AppSettings["fileformat"];
                try
                {
                    //Создаем файл
                    using (FileStream fs = File.Create(_filepath)) { }
                }
                catch (Exception ex)
                {
                    _errorSend.SendMail(ex.Message);
                    DisplayInfo.ShowInfo(ex.Message.ToString());
                }

            }
                //Если условие не выполнилось и файл с таким именем не существует то просто создаем файл с именем из app.config
            else
            {
                _filepath = path + "\\" + name + ConfigurationManager.AppSettings["fileformat"];
                try
                {
                    using (FileStream fs = File.Create(_filepath)) { }
                }
                catch (Exception ex)
                {
                    _errorSend.SendMail(ex.Message);
                    DisplayInfo.ShowInfo(ex.Message.ToString());
                }
            }
        }
        //Метод построчного сохранения данных в файл формата csv из списка ListInfo
        private void Save(ref List<LinkInfoCombo> iParts)
        {

            int StatusCode=0;
            try
            {
                foreach(string line in File.ReadLines(ConfigurationManager.AppSettings["HTMLpath"]))
                {
                    File.AppendAllText(_filepath, line + "\r\n", Encoding.UTF8);

                    if (line.Contains("metka1"))
                    {
                        foreach (LinkInfoCombo lnk in iParts)
                        {
                            if(lnk.amount!=0)
                            File.AppendAllText(_filepath, "<tr><td>" + lnk.StartTime + "</td><td>" + lnk.EndTime + "</td><td>" + lnk.Login + "</td><td>" + lnk.amount + "</td></tr>\r\n", Encoding.UTF8);
                        }
                    }
                    if(line.Contains("metka2"))
                    {
                        foreach (LinkInfoCombo lnk in iParts)
                        {
                            if (lnk.Code != StatusCode)
                                File.AppendAllText(_filepath, "<tr><td><H1>Status Code " + lnk.Code + "</h1></td></tr>\r\n", Encoding.UTF8);
                            File.AppendAllText(_filepath, "<tr><td><a href=\"" + lnk.Url + "\">" + lnk.Url + " </a>" + "</td>" + "<td>" + lnk.Code + "</td>" + "<td>" + ((lnk.Type == 1) ? "abs" : "otn") + "</td><tr>\r\n", Encoding.UTF8);
                            StatusCode = lnk.Code;
                        }
                    }
                }
                DisplayInfo.ShowDataWritten();
            }
            catch (Exception ex)
            {
                _errorSend.SendMail(ex.Message);
                DisplayInfo.ShowInfo(ex.Message.ToString());
            }
        }
        //Метод генерирования HTML файла с инфой о ссылках взятую из БД
        public void GenerateHTML(IDBSaver dbsaver)
        {
            List<LinkInfoCombo> iParts=new List<LinkInfoCombo>();
            Console.WriteLine("Starting generate HTML file...");
            dbsaver.GetDataFromDB("SELECT Link, StatusCode, Type from LinkInfo.dbo.Link where RunningInfoId=(SELECT TOP 1 [Id] FROM [LinkInfo].[dbo].[RunningInfo] ORDER BY Id DESC) ORDER BY StatusCode ASC", ref iParts, 0);
            dbsaver.GetDataFromDB("SELECT TOP 1 StartTime, EndTime, Login, amount FROM [LinkInfo].[dbo].[RunningInfo] ORDER BY Id DESC", ref iParts, 1);
            Save(ref iParts);
        }
    }
}
