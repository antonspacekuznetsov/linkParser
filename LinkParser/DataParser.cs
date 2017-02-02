using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace LinkParser
{
    class DataParser:IParser
    {
        private ISmtpSender _errorSend;
        private NetRequest _net;
        private IRegularEx _regex;
        private int _type;
        public DataParser(ISmtpSender sender, NetRequest net)
        {
            _errorSend = sender;
            _net = net;
            _regex = new RegExWorker();
            _type = 77;
        }
        //Метод нахождения ссылок из HTML кода с использованием регулярных выражений
        public void Parse(ref List<LinkInfo> parts, string link)
        {
            Match m;
            int code;
            string backlnk, responseFromServer = _net.GetHtmlCode(link), pattern = "(?:href|src)\\s*=\\s*(?:[\"'](?<1>[^\"']*)[\"']|(?<1>\\S+))";
            if (responseFromServer == "")
                return;
            try
            {
                m = Regex.Match(responseFromServer, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromSeconds(1));
                while (m.Success)
                {
                    backlnk = Cheker(link, m.Groups[1].ToString());
                    if (backlnk != "")
                    {
                        code = _net.GetStatusCode(backlnk);
                        parts.Add(new LinkInfo() {Url=backlnk, Code=code, Type=_type});
                        DisplayInfo.ShowLink(backlnk, code, parts.Count);
                        }
                    m = m.NextMatch();
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                _errorSend.SendMail(e.Message);
                DisplayInfo.ShowInfo("The matching operation timed out.");
            }

            catch (ArgumentNullException e)
            {
                _errorSend.SendMail(e.Message);
                DisplayInfo.ShowInfo(e.Message.ToString());
            }

        }

        //функция очистки ссылки от параметров, страниц и т.д. in=> http://google.ru/main.php?t=5 out=> http://google.ru
        //returned cleared link
        private string Clearlink(string _link)
        {
            string goodlink = "";
            int t = 0;
            for (int i = 0; i < _link.Length; i++)
            {
                if (_link[i] == '/')
                    t++;
                if (t >= 3)
                    break;
                goodlink += _link[i];
            }

            return goodlink;
        }
        //Метод проверки найденной ссылки
        private string Cheker(string _link, string foundlnk)
        {
            string goodLnk = foundlnk;
            _link = Clearlink(_link);

            if (!_regex.RegExRequest(foundlnk, ("^http.?")) || !_regex.RegExRequest(foundlnk, ("^https.?")))
            {
                if (_regex.RegExRequest(foundlnk, ("^mailto.*")))
                    return goodLnk = "";
                if (_regex.RegExRequest(foundlnk, ("^//.*")))
                {
                    goodLnk = "http:" + foundlnk;
                    _type = 1;
                }
                else
                {
                    if (!_regex.RegExRequest(foundlnk, ("^/.*")))
                        goodLnk = _link + "/" + foundlnk;
                    else
                        goodLnk = _link + foundlnk;
                    _type = 0;
                }
            }

            return goodLnk;
        }
    }
}
