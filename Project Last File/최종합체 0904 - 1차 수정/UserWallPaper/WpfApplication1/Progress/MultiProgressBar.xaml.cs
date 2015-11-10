using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;
using System.ComponentModel;
using UserWallPaper.ServiceReference1;

namespace UserWallPaper
{
    /// <summary>
    /// MultiProgressBar.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MultiProgressBar : Window
    {
        public delegate void GetFileDelegate(string copydirectory, string userselectdirectory);
        public event GetFileDelegate getfile;

        private Service service;
        private int pathflag;
        private int nowdownloadflag;

        BackgroundWorker bw = new BackgroundWorker();
        string[] filepath;
        private string copydirectory, realpath;
        long totalsize;

        public string CopyDirectory
        {
            get { return this.copydirectory; }
        }

        public string RealPath
        {
            get { return this.realpath; }
        }

        public MultiProgressBar()
        {
            InitializeComponent();
        }

        public MultiProgressBar(Service s, int flag)
        {
            InitializeComponent();
            pathflag = flag;
            settitle();
            nowdownloadflag = 0;
            service = s;
            totalsize = service.GetTotalSize(pathflag);
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;

            this.Loaded += MultiProgressBar_Loaded;
        }

        public MultiProgressBar(Service s, string[] _filepath, int _flag, string _realpath)
        {
            InitializeComponent();
            service = s;
            filepath = new string[_filepath.Count()];
            nowdownloadflag = 1;
            filepath = _filepath;
            pathflag = _flag;
            realpath = _realpath;
            totalsize = service.GetTotalFileSize(pathflag, filepath);
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            this.txtFile.Text = "Copying " + idx + " of " + filepath.Count() + " files";
            bw.RunWorkerAsync();
        }

        private void settitle()
        {
            if (pathflag == 1)
            {
                Title.Content = "교재를 다운 받는 중";
            }
            else
            {
                Title.Content = "참고 자료를 다운 받는 중";
            }
        }


        void MultiProgressBar_Loaded(object sender, RoutedEventArgs e)
        {
             bw.RunWorkerAsync();
        }

        static int idx = 0;
        void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            string usbpath = (string)System.Windows.Application.Current.Properties["USBDrivePath"];

            if (pathflag == 1)
            {
                DirectoryInfo di = new DirectoryInfo(usbpath + @"SchoolInteligentUSB\BookRoom");
                if (di.Exists != true)
                {
                    di.Create();
                }
            }
            else
            {
                DirectoryInfo di = new DirectoryInfo(usbpath + @"SchoolInteligentUSB\FileRoom");
                if (di.Exists != true)
                {
                    di.Create();
                }
            }
            //---------------------------------------------------------------------------------------  요거
            copydirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + "Temp";
            DirectoryInfo ndi = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + "Temp");  //c:에 저장할 폴더 임시로 만듬
            if (ndi.Exists != true)
            {
                ndi.Create();
            }
            //----------------------------------------------------------------------------------------

            string fileName = "";
            Stream inputStream;
            long startlength = 0;

           // long totalsize = service.GetTotalSize(pathflag);

            long length = 0; // 한 파일 사이즈를 최종 사이즈까지 더한다.

            while (totalsize > length)
            {
                long original = 0;


                if (nowdownloadflag == 0)
                {
                    original = service.DownloadFile(idx++, ref fileName, ref startlength, out inputStream, pathflag); //한파일 사이즈
                }
                else
                {
                    original = service.DownloadFiles(filepath, idx++, pathflag, ref fileName, ref startlength, out inputStream); //한파일 사이즈
                }

                length += original;

                string tempdirectroy = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + "Temp";

                using (FileStream writeStream = new System.IO.FileStream(tempdirectroy + "\\" + Path.GetFileName(fileName), FileMode.OpenOrCreate, FileAccess.Write))
                {
                    int chunkSize = 2048;
                    byte[] buffer = new byte[chunkSize];

                    do
                    {
                        int bytesRead = inputStream.Read(buffer, 0, chunkSize);
                        if (bytesRead == 0) break;

                        writeStream.Write(buffer, 0, bytesRead);

                        Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
                        {
                            //this.pb.Value = (int)(writeStream.Position * 100 / totalsize);
                            this.pb.Value = (int)(writeStream.Position * 100 / original);

                            this.total.Value = (int)(length * 100 / totalsize);
                        }));
                    }
                    while (true);

                    writeStream.Close();
                }

                inputStream.Dispose();
            }
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            idx = 0;
            this.Close();

            if (getfile != null)
            {
                getfile(copydirectory, realpath);
            }
        }
    }
}
