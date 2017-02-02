using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
namespace LinkParser
{
    class NetRequest
    {
        private string _responseFromServer;
        private int _code;
        private ISmtpSender _errorSend;
        private IRegularEx _regrequest;
        public NetRequest(ISmtpSender send)
        {
            _responseFromServer = "";
            _code = 0;
            _errorSend = send;
 
        }

        public string GetHtmlCode(string _link)
        {
             _regrequest = new RegExWorker();
            if (_regrequest.RegExRequest(_link, ".*\\.(doc|w3x|docx|pdf|rar|zip|png|js|jpeg|jpg|gif|css|djvu|iso|exe|ppt|pptx|ico|xml)$"))
                return "";

            try
            {
                WebRequest request = WebRequest.Create(_link);
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                            _responseFromServer = "";
                            _responseFromServer = reader.ReadToEnd();
                    }
                    response.Close();
                }
            }
            catch (WebException e)
            {
                _errorSend.SendMail(_link + " " + e.Message);
            }
            catch (UriFormatException e)
            {
                Console.WriteLine(e.Message);
                _errorSend.SendMail(e.Message);
            }
           return _responseFromServer;
        }

        public int GetStatusCode(string _link)
        {
            try
            {
                WebRequest request = WebRequest.Create(_link);
                using (WebResponse response = request.GetResponse())
                {
                    using (Stream dataStream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        _code = (int)(((HttpWebResponse)response).StatusCode);
                    }
                    response.Close();
                }
            }
            catch (WebException e)
            {
                    _errorSend.SendMail(_link + " " + e.Message);
                    Console.WriteLine(_link + " " + e.Message);
                    try
                    {
                        _code = (int)(((HttpWebResponse)e.Response).StatusCode);
                    }
                    catch (NullReferenceException ex)
                    {
                        _errorSend.SendMail(ex.Message);
                        _code = -1;
                    }
            }
            catch (UriFormatException e)
            {
                Console.WriteLine(e.Message);
                _errorSend.SendMail(e.Message);
            }

           return _code;
        }
    }
}
