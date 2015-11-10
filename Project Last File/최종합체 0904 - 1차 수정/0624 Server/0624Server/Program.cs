using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Configuration;
using System.IO;

namespace _0624Server
{
    class Program
    {
        public void CreateFolder(string path)
        {
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);
        }
        static void Main(string[] args)
        {
            //Address
            Uri uri = new Uri(ConfigurationManager.AppSettings["addr"]);
            //Contract-> Setting
            //Binding -> App.Config
            ServiceHost host = new ServiceHost(typeof(SIUService), uri);

            Program P = new Program();

            P.CreateFolder(@"C:\SchoolInteligentUSB");
            P.CreateFolder(@"C:\SchoolInteligentUSB\UserInfoDirectory");
            P.CreateFolder(@"C:\SchoolInteligentUSB\UserPDFDirectory");                             //0
            P.CreateFolder(@"C:\SchoolInteligentUSB\BookRoomDirectory");                            //1
            P.CreateFolder(@"C:\SchoolInteligentUSB\FileRoomDirectory");                            //2
            P.CreateFolder(@"C:\SchoolInteligentUSB\StudentBoardDirectory");                        //3

            P.CreateFolder(@"C:\SchoolInteligentUSB\SubmissionDirectory");                          //4
            P.CreateFolder(@"C:\SchoolInteligentUSB\SubmissionDirectory\SugesstSubmissionList");    //5
            P.CreateFolder(@"C:\SchoolInteligentUSB\SubmissionDirectory\SubmitSubmission");         //6

            P.CreateFolder(@"C:\SchoolInteligentUSB\NoUSBDirectory");                               //7
            P.CreateFolder(@"C:\SchoolInteligentUSB\Transfer\");  

            //오픈
            host.Open();
            Console.WriteLine("서비스를 시작합니다. {0}", uri.ToString());
            Console.WriteLine("종료하시려면 엔터를 눌러주세요..");
            Console.ReadLine();
            //서비스
            host.Abort();
            host.Close();

            //using (ServiceHost host = new ServiceHost(typeof(SIUService)))
            //{
            //    host.Open();
            //    Console.WriteLine("Service Started!");

            //    foreach (Uri address in host.BaseAddresses)
            //        Console.WriteLine("Listening on " + address);
            //    Console.WriteLine("Press any key to close...");
            //    Console.ReadKey();

            //    host.Close();
            //}
        }
    }
}
