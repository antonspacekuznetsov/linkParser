using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LinkParser
{
    class Program
    {
        
        static void Main(string[] args)
        {
            IRegularEx request = new RegExWorker();
                if (args.Length != 1)
                    DisplayInfo.ShowInfo("The program must have one parametr only");
                if (!request.RegExRequest(args[0], "\\."))
                    DisplayInfo.ShowInfo("Your link is not correct it have'not the \".\" please try again");
                if (!request.RegExRequest(args[0], "(http://?)"))
                    args[0] = "http://" + args[0];
 

            Process proc = new Process(args[0]);
            proc.Runner();//Запускаем процесс обработки ссылок

            Console.Write("Sucssessful. Press any key to exit...!");
            Console.ReadKey();
        }
    }
}
