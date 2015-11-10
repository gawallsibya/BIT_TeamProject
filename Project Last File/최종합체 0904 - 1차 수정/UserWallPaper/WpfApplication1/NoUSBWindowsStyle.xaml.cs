using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.ComponentModel;
using UserWallPaper.ServiceReference1;

namespace UserWallPaper
{
    /// <summary>
    /// NoUSBWindowsStyle.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NoUSBWindowsStyle : Window
    {

        #region 바탕화면 변수
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindow(IntPtr hWnd, int uCmd);

        [DllImport("User32.dll")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        private const int GW_CHILD = 5;

        public static void SetOnDesktop(Window window)
        {
            //IntPtr hWnd = new WindowInteropHelper(window).Handle;
            //IntPtr hWndProgMan = FindWindow("Progman", "Program Manager");
            //SetParent(hWnd, hWndProgMan);

            IntPtr Progmanhwnd = IntPtr.Zero;
            IntPtr hwd1, hwd2, hwd3;
            Progmanhwnd = (IntPtr)FindWindow("Progman", "Program Manager");
            hwd1 = GetWindow(Progmanhwnd, GW_CHILD);
            hwd2 = GetWindow(hwd1, GW_CHILD);
            hwd3 = GetWindow(hwd2, GW_CHILD);

            SetParent(new WindowInteropHelper(window).Handle, hwd2);
        }
        #endregion

        private ArrayList directorylist;

        Service service;
        string name, type, id;
        string userinfo;
        string documentspath; //download폴더 수정흔적
        BackgroundWorker bw = new BackgroundWorker();

        ProgressBar pb;


        public NoUSBWindowsStyle(Service s, string info, string i, string path) //download폴더 수정흔적
        {
            InitializeComponent();

            service = s;
            id = i;
            userinfo = info;
            documentspath = path; //download폴더 수정흔적
            setinfo(info);

            System.Windows.Media.Animation.DoubleAnimation da = new System.Windows.Media.Animation.DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(1.5)));
            WallPaper1.BeginAnimation(Grid.OpacityProperty, da);

            LoadWindows();

            Thread.Sleep(10);

            //===================================================
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
        }

        private void UserWindows_Loaded(object sender, RoutedEventArgs e)
        {
            SetOnDesktop(this);

            this.Left = 0;
            this.Top = 0;
            this.WindowState = WindowState.Maximized;
        }

        private void setinfo(string info)
        {
            string[] userinfo = new string[3];
            userinfo = info.Split('\a');

            name = userinfo[0];
            type = userinfo[2];
            MainName.Text = userinfo[0];
        }

        private void LoadWindows()
        {
            directorylist = new ArrayList();

            string path = documentspath.Remove(documentspath.LastIndexOf("\\") + 1);

            DirectoryInfo di = new DirectoryInfo(path);

            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                Application.Current.Properties["DirectoryInfo"] = dir;

                NoUSBWindowFolder IconInfo = new NoUSBWindowFolder();

                Thickness margin = new Thickness(10);
                IconInfo.Margin = margin;

                wallpaperline1.Children.Add(IconInfo);

                Application.Current.Properties.Remove("DirectoryInfo");
            }
        }

        #region 사용자 지정 아이콘
        private void MyComputer_Click(object sender, MouseButtonEventArgs e)
        {
            Process ps = new Process();
            ps.StartInfo.FileName = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}";
            ps.Start();
        }

        private void MyFolder_Click(object sender, MouseButtonEventArgs e)
        {
            Process ps = new Process();
            ps.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ps.Start();
        }

        private void Recyclebin_Click(object sender, MouseButtonEventArgs e)
        {
            Process ps = new Process();
            ps.StartInfo.FileName = "::{645FF040-5081-101B-9F08-00AA002F954E}";
            ps.Start();
        }

        private void Viewer_Click(object sender, MouseButtonEventArgs e)
        {
            MainWindow viewer = new MainWindow(service);
            viewer.ShowDialog();
        }

        private void Submission_Click(object sender, MouseButtonEventArgs e)
        {
            SubmissionMain submission = new SubmissionMain(service, name, type, id);
            submission.ShowDialog();
        }

        private void FileRoom_Click(object sender, MouseButtonEventArgs e)
        {
            FileRoom fileroom = new FileRoom(service, name, type);
            fileroom.ShowDialog();
        }

        private void StudentBoard_Click(object sender, MouseButtonEventArgs e)
        {
            StudentBoard studentboard = new StudentBoard(service, name, id);
            studentboard.ShowDialog();
        }

        #endregion

        #region 종료

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties.Remove("USBDrivePath");
            //===================================================
            System.Windows.Forms.DialogResult dr = System.Windows.Forms.MessageBox.Show("오늘 작성한 폴더를 서버에 업로드 하시겠습니까?.", "알림", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Information);

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                //폴더 압축
                FileCompact();

                string[] stringArray = new string[2];

                stringArray[0] = id;
                stringArray[1] = zipPath_Convert;

                service.NoUSBWrite(stringArray);

                //파일전송
                if (bw.IsBusy != true)
                {
                    // Start the asynchronous operation.
                    bw.RunWorkerAsync();
                }
            }
            else
            {
                Environment.Exit(0);
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                this.Close();
            }
        }

        private string Replace_OverlapFile(string path)
        {
            FileInfo fInfo = new FileInfo(path);//file은 static 멤버를 갖고있고 fileinfo는 객체를 생성해야함
            if (fInfo.Exists)
            {
                //true면 같은파일명이 존재한다.
                int idx = 1;
                string ext = fInfo.Extension; //해당파일의 확장자만 가져옴
                path = path.Replace(ext, "");
                string newFileName = "";
                do
                {
                    newFileName = path + "(" + (idx++) + ")" + ext;
                    fInfo = new FileInfo(newFileName);
                } while (fInfo.Exists);
                path = newFileName;
            }
            return path;
        }

        #region 비로그인시 폴더 압축
        //비로그인시 폴더 압축
        string startPath, zipPath, zipPath_Convert;
        private void FileCompact()
        {
            startPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SIU\\" + id + "\\" + DateTime.Today.ToShortDateString();
            zipPath = startPath;

            string savepath = Replace_OverlapFile(zipPath + ".zip");

            FileInfo file = new FileInfo(zipPath + ".zip");
            if (file.Exists)     //파일이 해당경로에 존재하는지 여부 체크
            {
                //zipPath_Convert = zipPath + "-추가" + ".zip";
                zipPath_Convert = savepath;
                ZipFile.CreateFromDirectory(startPath, zipPath_Convert);
            }
            else
            {
                zipPath_Convert = zipPath + ".zip";
                ZipFile.CreateFromDirectory(startPath, zipPath_Convert);
            }
        }
        #endregion

        #region No USB  작성한 파일 서버에 업로드
        //ProgressBar pb;
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.pb.Value = e.ProgressPercentage;
        }

        public void uploadStreamWithProgress_ProgressChanged(object sender, StreamWithProgress.ProgressChangedEventArgs e)
        {
            if (e.Length != 0)
            {
                bw.ReportProgress((int)(e.BytesRead * 100 / e.Length));
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
            {
                pb = new ProgressBar();
                pb.Show();
            }));

            //NoUSB 폴더에 저장
            FileInfo fileInfo = new FileInfo(zipPath_Convert);
            if (fileInfo.Exists)
            {
                using (FileStream stream = new FileStream(zipPath_Convert, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    using (StreamWithProgress uploadStreamWithProgress = new StreamWithProgress(stream))
                    {
                        uploadStreamWithProgress.ProgressChanged += uploadStreamWithProgress_ProgressChanged;
                        service.UploadFile(id + " - " + fileInfo.Name, fileInfo.Length, 0, uploadStreamWithProgress, 6);
                    }
                }
            }
            service.Dispose();

            if (System.IO.Directory.Exists(startPath))
            {
                try
                {
                    System.IO.Directory.Delete(startPath, true);
                }

                catch
                {
                    return;
                }
            }

            if (System.IO.File.Exists(zipPath + ".zip"))
            {
                try
                {
                    System.IO.File.Delete(zipPath + ".zip");
                }

                catch
                {
                    return;
                }
            }

            Environment.Exit(0);
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            this.Close();
        }
        #endregion

        #endregion

    }
}
