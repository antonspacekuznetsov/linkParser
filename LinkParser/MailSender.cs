using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Mail;
namespace LinkParser
{
    //the class send mail then occure error in programm, for example occure exception

    class MailSender:ISmtpSender
    {
        private SmtpClient _client;
        private MailMessage _mail;
        private int _allow;
        public MailSender()
        {
            int port;
            _mail = new MailMessage(ConfigurationManager.AppSettings["MailFrom"], ConfigurationManager.AppSettings["MailTo"], ConfigurationManager.AppSettings["MailSubject"], ConfigurationManager.AppSettings["MailBody"]);
            _client = new SmtpClient(ConfigurationManager.AppSettings["MailServer"]);
            int.TryParse(ConfigurationManager.AppSettings["MailPort"], out port);
            _client.Port = port;
            int.TryParse(ConfigurationManager.AppSettings["Allow"], out _allow);
        }

        public void SendMail(string message)
        {

            if(_allow==1)
                try
                {
                    _mail.Body = message;
                    _client.Send(_mail);
                }
                catch (Exception e)
                {
                    Console.WriteLine("+ERROR+" + e.Message);
                }
        }
    }
}
