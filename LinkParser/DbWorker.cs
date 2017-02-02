using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Configuration;

namespace LinkParser
{
   class DbWorker: IDBSaver
    {
       private ISmtpSender _errorSend;
       public DbWorker(ISmtpSender sender)
        {
            _errorSend = sender;
        }

       public void SaveToDB(ref List<LinkInfo> parts, DateTime start, DateTime stop)
        {
            Console.WriteLine("Try to write data to BD, it may take some time...");
            using (var context = new LinkInfoEntities())
            {
                RunningInfo Idznach = new RunningInfo { amount = parts.Count, StartTime = start, EndTime = stop, Login = Environment.UserName };
                context.RunningInfoes.Add(Idznach);
                context.SaveChanges();
                foreach (LinkInfo line in parts)
                {
                    context.Link.Add(new Link { StatusCode = line.Code, Link1 = line.Url, Type = line.Type, RunningInfoId = Idznach.Id });
                }
                context.SaveChanges();
            }
            parts.Clear();//После добавления списка в БД очищаем его что бы не воспользоваться им, а взять данные для HTML из БД
        }


        public void GetDataFromDB(string sqlExpression,  ref List<LinkInfoCombo> iParts, int priznak)
        {
            if (sqlExpression == "")
                return;
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionToDB"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand(sqlExpression, conn);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            switch(priznak)
                            {
                                case 0:
                                    iParts.Add(new LinkInfoCombo { Url = reader[0].ToString(), Code = int.Parse(reader[1].ToString()), Type = int.Parse(reader[2].ToString()) });

                                        break;
                                case 1:
                                        iParts.Add(new LinkInfoCombo { StartTime = (DateTime)reader[0], EndTime = (DateTime)reader[1], Login = reader[2].ToString(), amount = int.Parse(reader[3].ToString()) });
                                        break;
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    _errorSend.SendMail("Can't connect to database!");
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
