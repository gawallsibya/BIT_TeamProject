using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Threading;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using WPFSpark;
using UserWallPaper.ServiceReference1;
using System.Windows.Threading;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace UserWallPaper
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WindowsStyle : Window
    {
        private delegate void SetProgCallBack(string copydirectory, string userselectdirectory);
        private delegate void deletetempfileCallBack();

        Service service;
        string name, type, id, info, Server_File, USB_Path = "SchoolInteligentUSB\\FileRoom\\";
        string usbpath;
        ArrayList NoUSBlist;

        #region 사용자 독 움직이기 변수
        Point m_start;
        Vector m_startOffset;
        #endregion

        #region 사용자 폴더 및 파일 함수에 사용하는 변수
        private ArrayList filelist, directorylist, writexml;
        private ArrayList userfile, userdocument;
        int count = 24;
        Thread thread;
        #endregion

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

        #region 배경화면 변수
        //=====================================================================================================
        System.Collections.Generic.List<BitmapImage> ImageList { get; set; }
        public System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();
        public Random Ran = new Random();
        public bool IsImage1 { get; set; }

        public string WallPaper_Para_Default = null;
        //=====================================================================================================
        #endregion

        DispatcherTimer t = new DispatcherTimer();

        BackgroundWorker bw = new BackgroundWorker();

        private static EventWaitHandle ewh;

        Thread th;

        private string file;
        ProgressBar pb;
        string path;

        string to;

        public WindowsStyle(Service s, string info)
        {
            InitializeComponent();

            service = s;
            service.Ws = this;
            SetUser(info);

            usbpath = (string)Application.Current.Properties["USBDrivePath"];

            System.Windows.Media.Animation.DoubleAnimation da = new System.Windows.Media.Animation.DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(1)));
            WallPaper1.BeginAnimation(Grid.OpacityProperty, da);

            ImageList = new System.Collections.Generic.List<BitmapImage>(); //배경화면 리스트 저장

            LoadWindows(); //load the windows

            XmlRead();

            HideOrShow();

            SetUSerLinkList();

            DownloadedFileCheck();

            try
            {
                WindowsXml useruixml = new WindowsXml();

                NoUSBlist = new ArrayList();
                NoUSBlist = service.NoUSBRead();
                multiFileName = new string[NoUSBlist.Count];

                if (NoUSBlist.Count != useruixml.NoUsbDownLoadXML_Read().Count)
                {
                    NoUSB_AutoDownLoad();
                }
            }
            catch
            {
            }

            thread = new Thread(new ThreadStart(UserIconTheading));
            thread.Start();

            foreach (string serial in (string[])Application.Current.Properties["Members"])
            {
                if ((string)Application.Current.Properties["User"] == serial)
                {
                    Application.Current.Properties.Remove("User");
                    continue;
                }

                string name = service.MyName(serial);
                MenuItem item = new MenuItem();
                item.Click += item_Click;
                item.Header = name;
                item.Name = "s_" + serial;
                ((MenuItem)WallPaper1.ContextMenu.Items[8]).Items.Add(item);
            }
            Application.Current.Properties.Remove("Members");

            ewh = new EventWaitHandle(false, EventResetMode.AutoReset);

            th = new Thread(new ThreadStart(DownloadThread));
            th.Start();

            //===================================================
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
            {
                pb = new ProgressBar();
                pb.Show();
            }));

            FileInfo fileInfo = new FileInfo(path);

            if (fileInfo.Exists)
            {
                using (FileStream stream = new FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    using (StreamWithProgress uploadStreamWithProgress = new StreamWithProgress(stream))
                    {
                        uploadStreamWithProgress.ProgressChanged += uploadStreamWithProgress_ProgressChanged;
                        service.UploadFile(fileInfo.Name, fileInfo.Length, 0, uploadStreamWithProgress, 7);
                    }
                }
            }

            service.SendRequest(to, fileInfo.Name);
        }

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

        public void DownloadThread()
        {
            while (true)
            {
                if (service.ExistFile())
                    ewh.Set();
                ewh.WaitOne();

                Stream stream = service.GetFile(file, 0);

                Stream output = new FileStream(service.FilePath, FileMode.Create);

                StorageFileInfo[] files = null;

                string tempdirectroy = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + "Temp";

                files = service.List(null, 0);
                double fileSize = 0;
                foreach (var f in files)
                {
                    if (this.file == f.VirtualPathk__BackingField)
                        fileSize = (double)f.Sizek__BackingField;
                }
                //프로그래스바 ㄱㄱ

                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
                {
                    pb = new ProgressBar(stream, output, fileSize, tempdirectroy, service.FilePath);
                    pb.getfile += GetFile;
                    pb.Show();

                    if (!pb.IsActive)
                        service.DeleteFile(this.file, -1);
                }));
            }
        }

        public void item_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog()
            {
                Title = "Select a file to upload",
                RestoreDirectory = true,
                CheckFileExists = true
            };

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (((MenuItem)sender).Header.ToString() != "전체")
                    to = ((MenuItem)sender).Name.Replace("s_", "");
                else to = "TotalSend";

                path = dlg.FileName;

                if (bw.IsBusy != true)
                {
                    // Start the asynchronous operation.
                    bw.RunWorkerAsync();
                }
            }
        }

        #region NoUSB 자동 다운로드

        public string DownFileNameSplit(string DownFileName)
        {
            char[] dot = { '.' };
            string[] dotarray = DownFileName.Split(dot);
            return dotarray[0];
        }

        string[] multiFileName;
        public void NoUSB_AutoDownLoad()
        {
            string[] array = new string[3];
            string Receive_CompactFile;

            if (NoUSBlist.Count != 0)
            {
                for (int i = 0; i < NoUSBlist.Count; i++)
                {
                    Receive_CompactFile = NoUSBlist[i].ToString();
                    array = Receive_CompactFile.Split('\a');

                    if (id == array[0])
                    {
                        char[] delimiterChars = { '\\' };
                        string[] words = array[1].Split(delimiterChars);
                        Server_File = words[words.Length - 1];

                        multiFileName[i] = Server_File;
                    }
                }

                if (id == array[0])
                {
                    if (System.IO.File.Exists(usbpath + USB_Path + id + " - " + Server_File))        //USB_Path = "SchoolInteligentUSB\\FileRoom\\"
                        return;

                    else
                    {
                        string UpdateFile = id + " - " + Server_File;

                        System.Windows.Forms.DialogResult dr = System.Windows.Forms.MessageBox.Show("서버에 업데이트된 파일이 있습니다. 받으시겠습니까?", "알림", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Information);

                        if (dr == System.Windows.Forms.DialogResult.OK)
                        {
                            DownloadFile(multiFileName);

                            WindowsXml useruixml = new WindowsXml();
                            useruixml.NoUsbDownLoadXML_Write(UpdateFile);

                            Process.Start(usbpath + "SchoolInteligentUSB\\FileRoom");

                            MessageBox.Show("다운로드가 완료되었습니다.");
                        }
                    }
                }
            }
            else
                return;
        }

        public void DeleteFile(string Delete_path)
        {
            if (System.IO.File.Exists(Delete_path))
            {
                try
                {
                    System.IO.File.Delete(Delete_path);
                }
                catch
                {
                    return;
                }
            }
        }

        string DownTemp = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Temp\\";
        private void DownloadFile(string[] p)
        {
            for (int i = 0; i < p.Count(); i++)
            {
                p[i] = id + " - " + p[i];
            }

            string realpath = usbpath + @"SchoolInteligentUSB\FileRoom";

            //프로그래스바 ㄱㄱ
            MultiProgressBar mbar = new MultiProgressBar(service, p, 6, realpath);
            mbar.getfile += GetFile;
            mbar.ShowDialog();
        }

        private void GetFile(string copydirectory, string userselectdirectory)
        {
            if (this.Dispatcher.Thread != Thread.CurrentThread)
            {
                SetProgCallBack dele = new SetProgCallBack(GetFile);
                Dispatcher.Invoke(dele, new object[] { copydirectory, userselectdirectory });
            }
            else
            {
                string path = copydirectory;
                string realpath = userselectdirectory;

                DirectoyProgress bar = new DirectoyProgress(copydirectory, userselectdirectory, 1);
                bar.deletefile += deletecompletefile;
                bar.Show();
            }
        }

        private void deletecompletefile()
        {
            if (this.Dispatcher.Thread != Thread.CurrentThread)
            {
                deletetempfileCallBack dele = new deletetempfileCallBack(deletecompletefile);
                Dispatcher.Invoke(dele);
            }
            else
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + "Temp";
                Directory.Delete(path, true);
            }
        }
        #endregion


        public void CreateFolder(string path)
        {
            if (Directory.Exists(usbpath + path) == false)
                Directory.CreateDirectory(usbpath + path);
        }

        #region 최초 윈도우 시작 셋팅
        private void UserWindows_Loaded(object sender, RoutedEventArgs e)
        {
            SetOnDesktop(this);

            this.Left = 0;
            this.Top = 0;
            this.WindowState = WindowState.Maximized;

            WindowsXml windowxml = new WindowsXml();


            if(windowxml.WallpaperLoad() != null)
            Img1.Source = new BitmapImage(new Uri(windowxml.WallpaperLoad(), UriKind.Absolute));
        }

        private void SetUser(string i)
        {
            string[] userinfo = new string[3];
            info = i;
            userinfo = i.Split('\a');

            name = userinfo[0];
            type = userinfo[1];
            id = userinfo[2];
            MainName.Text = name;
        }
        #endregion

        #region 사용자 독
        #region 사용자 독 숨기고 보이기
        private void HideOrShow()
        {
            ShowLinkDock.MouseLeave += delegate
            {
                DoubleAnimation da = new DoubleAnimation(0, new Duration(TimeSpan.FromSeconds(3)));
                usergrid.BeginAnimation(Grid.OpacityProperty, da);
            };

            ShowLinkDock.MouseEnter += delegate
            {
                DoubleAnimation da = new DoubleAnimation(1, new Duration(TimeSpan.FromSeconds(0.2)));
                usergrid.BeginAnimation(Grid.OpacityProperty, da);
            };
        }
        #endregion

        #region 사용자 독 움직이기
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            m_start = e.GetPosition(UserWindows);
            m_startOffset = new Vector(tt.X, tt.Y);
            usergrid.CaptureMouse();
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (usergrid.IsMouseCaptured)
            {
                Vector offset = Point.Subtract(e.GetPosition(UserWindows), m_start);

                tt.X = m_startOffset.X + offset.X;
                tt.Y = m_startOffset.Y + offset.Y;
            }
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            usergrid.ReleaseMouseCapture();
        }
        #endregion

        #region 사용자 링크
        private void SetUSerLinkList()
        {
            UserLinkListXML userlinklistxml = new UserLinkListXML();
            ArrayList userlinklist = userlinklistxml.LoadUserLink();
            MainStack.Children.Clear();

            ParentsPanel.Width = 5;
            MainStack.Width = 5;
            iconbackboard.Width = 25;
            iconbackboard2.Width = 40;
            iconground.Width = 0;

            for (int i = 0; i < userlinklist.Count; i++)
            {
                string linkpath = Convert.ToString(userlinklist[i]);

                string[] userlink = new string[2];
                userlink = linkpath.Split('\a');

                string ExeName = userlink[1].Substring(userlink[1].LastIndexOf("\\") + 1);

                string pullpath = userlink[1].TrimEnd('\\');
                pullpath = userlink[1].Remove(userlink[1].LastIndexOf('\\') + 1);

                try
                {
                    DirectoryInfo di = new DirectoryInfo(pullpath);

                    foreach (FileInfo file in di.GetFiles())
                    {
                        if (string.Compare(ExeName, file.Name) == 0)
                        {
                            ParentsPanel.Width += 55;
                            MainStack.Width += 55;
                            iconbackboard.Width += 55;
                            iconbackboard2.Width += 55;
                            iconground.Width += 55;

                            UserSetUtil usercreatelink = new UserSetUtil(userlink);

                            Thickness newMargin = new Thickness(5);
                            usercreatelink.Margin = newMargin;

                            MainStack.Children.Add(usercreatelink);

                            break;
                        }
                    }
                }
                catch
                {
                }
            }

            if (Application.Current.Properties["DeleteUserlnk"] != null)
            {
                Application.Current.Properties.Remove("DeleteUserlnk");
            }
            else if (Application.Current.Properties["AddUserlnk"] != null)
            {
                Application.Current.Properties.Remove("AddUserlnk");
            }

        }

        private void CreateLinkFile(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog() { DereferenceLinks = false };

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GetlnkPath getlnkpath = new GetlnkPath();
                string path = getlnkpath.GetTargetPath(dlg.FileName);
                string[] name = new string[2];
                name = dlg.SafeFileName.Split('.');

                if (path == null)
                {
                    path = dlg.FileName;
                }

                UserLinkListXML userlinklistxml = new UserLinkListXML();
                if (userlinklistxml.SaveUserLink(name[0], path)) { }
                else
                    System.Windows.Forms.MessageBox.Show("동일한 바로가기가 존재합니다");

                SetUSerLinkList();
            }
        }
        #endregion
        #endregion

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
            viewer.Owner = this;
            //viewer.Icon = new BitmapImage(new Uri(@"../../PNG/4.png",UriKind.Relative));
            viewer.Show();
        }

        private void Submission_Click(object sender, MouseButtonEventArgs e)
        {
            SubmissionMain submission = new SubmissionMain(service, name, type, id);
            submission.Owner = this;
            //submission.Icon = new BitmapImage(new Uri(@"../../PNG/3.png", UriKind.Relative));
            submission.Show();
        }

        private void FileRoom_Click(object sender, MouseButtonEventArgs e)
        {
            FileRoom fileroom = new FileRoom(service, name, type);
            fileroom.Owner = this;
            //fileroom.Icon = new BitmapImage(new Uri(@"../../PNG/2.png", UriKind.Relative));
            fileroom.Show();
        }

        private void StudentBoard_Click(object sender, MouseButtonEventArgs e)
        {
            StudentBoard studentboard = new StudentBoard(service, name, id);
            studentboard.Owner = this;
            //studentboard.Icon = new BitmapImage(new Uri(@"../../PNG/3.png", UriKind.Relative));
            studentboard.Show();
        }

        #endregion

        #region 최초 파일 가져오기
        private void LoadWindows()
        {
            filelist = new ArrayList();
            directorylist = new ArrayList();

            CreateFolder(@"SchoolInteligentUSB");
            CreateFolder(@"SchoolInteligentUSB\WindowFolder");

            DirectoryInfo di = new DirectoryInfo(usbpath + @"SchoolInteligentUSB\WindowFolder");

            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                directorylist.Add(dir);
            }

            foreach (FileInfo file in di.GetFiles())
            {
                filelist.Add(file);
            }
        }

        private void CreateUserICon()
        {
            userfile = new ArrayList();
            userdocument = new ArrayList();

            for (int i = 0; i < directorylist.Count; i++)
            {
                Application.Current.Properties["DirectoryInfo"] = directorylist[i];

                UserFolder IconInfo = new UserFolder();

                userdocument.Add(IconInfo);

                Application.Current.Properties.Remove("DirectoryInfo");
            }

            for (int j = 0; j < filelist.Count; j++)
            {
                Application.Current.Properties["FileInfo"] = filelist[j];

                UserFile IconInfo = new UserFile();

                userfile.Add(IconInfo);

                Application.Current.Properties.Remove("FileInfo");
            }

            wallpaperline1.Children.Clear();
        }
        #endregion

        #region 사용자 폴더 및 파일 셋팅
        private void XmlWrite()
        {
            writexml = new ArrayList();

            foreach (UIElement u in wallpaperline1.FluidElements)
            {
                if (u is UserFile)
                    writexml.Add(((UserFile)u).IconName);
                else if (u is UserFolder)
                    writexml.Add(((UserFolder)u).IconName);
            }

            WindowsXml useruixml = new WindowsXml();
            useruixml.WindowsWriteXml(writexml);

            writexml.Clear();
        }

        private void XmlRead()
        {
            WindowsXml useruixml = new WindowsXml();

            if (useruixml.WindowsLoadXml() == null)
            {
                AddUserInfo();
            }
            else
            {
                string[] useroderlist = (string[])useruixml.WindowsLoadXml();

                if (useroderlist.Length == filelist.Count + directorylist.Count)
                {
                    CreateUserICon();

                    for (int i = 0; i < useroderlist.Length; i++)
                    {
                        for (int j = 0; j < userdocument.Count; j++)
                        {
                            UserFolder folderInfo = (UserFolder)userdocument[j];

                            if (string.Compare(useroderlist[i], folderInfo.IconName) == 0)
                            {
                                SetAddPanel();

                                wallpaperline1.Children.Add((UserFolder)userdocument[j]);
                                break;
                            }
                        }

                        for (int k = 0; k < userfile.Count; k++)
                        {
                            UserFile fileInfo = (UserFile)userfile[k];

                            if (string.Compare(useroderlist[i], fileInfo.IconName) == 0)
                            {
                                SetAddPanel();

                                wallpaperline1.Children.Add((UserFile)userfile[k]);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    wallpaperline1.Children.Clear();

                    AddUserInfo();
                }
            }
        }

        private void UserIconTheading()
        {
            while (true)
            {
                DirectoryInfo di = new DirectoryInfo(usbpath + @"SchoolInteligentUSB\WindowFolder");
                int directorylistcount = di.GetDirectories().Length + di.GetFiles().Length;

                Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
                {
                    if (wallpaperline1.Children.Count != directorylistcount)
                    {
                        ResetUserInfo();
                        SetSubPanel();
                    }

                    if (Application.Current.Properties["DeleteUserlnk"] != null)
                    {
                        SetUSerLinkList();
                    }
                    else if (Application.Current.Properties["AddUserlnk"] != null)
                    {
                        SetUSerLinkList();
                    }

                    if (Application.Current.Properties["FirstSeting"] != null)
                    {
                        XmlWrite();
                        Application.Current.Properties.Remove("FirstSeting");
                    }

                    if (Application.Current.Properties["Resetingicon"] != null)
                    {
                        XmlWrite();
                        Application.Current.Properties.Remove("Resetingicon");
                    }

                    if (Application.Current.Properties["DragAndDrop"] != null)
                    {
                        XmlWrite();
                        Application.Current.Properties.Remove("DragAndDrop");
                    }

                    if (Application.Current.Properties["DirectoryRename"] != null)
                    {
                        XmlWrite();
                        Application.Current.Properties.Remove("DirectoryRename");
                    }
                }));
            }
        }

        private void AddUserInfo()
        {
            LoadWindows();
            CreateUserICon();

            for (int i = 0; i < userdocument.Count; i++)
            {
                SetAddPanel();

                wallpaperline1.Children.Add((UserFolder)userdocument[i]);
            }

            for (int j = 0; j < userfile.Count; j++)
            {
                SetAddPanel();

                wallpaperline1.Children.Add((UserFile)userfile[j]);
            }

            Application.Current.Properties["FirstSeting"] = true;
        }

        private void ResetUserInfo()
        {
            LoadWindows();
            CreateUserICon();

            WindowsXml useruixml = new WindowsXml();

            string[] useroderlist = (string[])useruixml.WindowsLoadXml();

            if (useroderlist.Length < userdocument.Count + userfile.Count)
            {
                for (int i = 0; i < useroderlist.Length; i++)
                {
                    for (int j = 0; j < userdocument.Count; j++)
                    {
                        UserFolder folderInfo = (UserFolder)userdocument[j];

                        if (string.Compare(useroderlist[i], folderInfo.IconName) == 0)
                        {
                            SetAddPanel();

                            wallpaperline1.Children.Add((UserFolder)userdocument[j]);

                            userdocument.RemoveAt(j);
                            break;
                        }
                    }

                    for (int k = 0; k < userfile.Count; k++)
                    {
                        UserFile fileInfo = (UserFile)userfile[k];

                        if (string.Compare(useroderlist[i], fileInfo.IconName) == 0)
                        {
                            SetAddPanel();

                            wallpaperline1.Children.Add((UserFile)userfile[k]);

                            userfile.RemoveAt(k);
                            break;
                        }
                    }
                }

                if (userdocument.Count != 0)
                {
                    wallpaperline1.Children.Add((UserFolder)userdocument[0]);
                    XmlWrite();
                }
                else if (userfile.Count != 0)
                {
                    wallpaperline1.Children.Add((UserFile)userfile[0]);
                    XmlWrite();
                }
            }
            else if (useroderlist.Length > userdocument.Count + userfile.Count)
            {
                for (int i = 0; i < useroderlist.Length; i++)
                {
                    for (int j = 0; j < userdocument.Count; j++)
                    {
                        UserFolder folderInfo = (UserFolder)userdocument[j];

                        if (string.Compare(useroderlist[i], folderInfo.IconName) == 0)
                        {
                            SetAddPanel();

                            wallpaperline1.Children.Add((UserFolder)userdocument[j]);

                            userdocument.RemoveAt(j);
                            break;
                        }
                    }

                    for (int k = 0; k < userfile.Count; k++)
                    {
                        UserFile fileInfo = (UserFile)userfile[k];

                        if (string.Compare(useroderlist[i], fileInfo.IconName) == 0)
                        {
                            SetAddPanel();

                            wallpaperline1.Children.Add((UserFile)userfile[k]);

                            userfile.RemoveAt(k);
                            break;
                        }
                    }
                }
            }
        }

        private void SetAddPanel()
        {
            if (wallpaperline1.Children.Count == count)
            {
                WindowCanvas.Width += 170;
                wallpaperline1.Width += 170;
                count += 4;
            }
        }

        private void SetSubPanel()
        {
            int subcount = count - wallpaperline1.Children.Count;

            if (subcount == 4 && count > wallpaperline1.Children.Count)
            {
                WindowCanvas.Width -= 170;
                wallpaperline1.Width -= 170;
                count -= 4;
            }
        }
        #endregion

        #region 새폴더 생성 / 파일 가져오기 / 폴더 가져오기
        string copydirectoypath;
        string userdirectoypath;
        string directoryname;

        private void CreateNewFile(object sender, RoutedEventArgs e)
        {
            int i = 2;
            string newfolder = "새 폴더";

            DirectoryInfo di = new DirectoryInfo(usbpath + @"SchoolInteligentUSB\WindowFolder");

            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                if (string.Compare(newfolder, dir.Name) == 0)
                {
                    newfolder = "새 폴더";
                    newfolder = newfolder + " " + "(" + i + ")";
                    i++;
                }
            }

            Directory.CreateDirectory(usbpath + @"SchoolInteligentUSB\WindowFolder\" + newfolder);
            Application.Current.Properties["Resetingicon"] = true;
        }

        private void GetFile(object sender, RoutedEventArgs e)
        {
            string path, filename;

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "All(*.*)|*.*";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                path = dlg.FileName;
                filename = path.Substring(path.LastIndexOf("\\") + 1);
                try
                {
                    var size = new FileInfo(path).Length;

                    Stream output = new FileStream(usbpath + @"SchoolInteligentUSB\WindowFolder\" + filename, FileMode.Create);

                    Stream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

                    ProgressBar bar = new ProgressBar(fileStream, output, size);
                    bar.Show();
                }
                catch (System.IO.FileNotFoundException ex)
                {
                    MessageBox.Show(ex.StackTrace);
                }

                Application.Current.Properties["Resetingicon"] = true;
            }
            else
                return;
        }

        private void GetDirectory(object sender, RoutedEventArgs e)
        {
            userdirectoypath = usbpath + @"SchoolInteligentUSB\WindowFolder\";

            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                if (dialog.SelectedPath != null)
                {
                    copydirectoypath = dialog.SelectedPath;
                    directoryname = copydirectoypath.Substring(copydirectoypath.LastIndexOf("\\") + 1);

                    DirectoyProgress directorycopy = new DirectoyProgress(copydirectoypath, userdirectoypath + directoryname, 1);
                    directorycopy.Show();
                }
                else
                    return;

                Application.Current.Properties["Resetingicon"] = true;
            }
            else
                return;
        }
        #endregion

        #region 배경화면
        #region 배경화면 슬라이드
        private void WallPaper_Slide()
        {
            Timer.Interval = TimeSpan.FromSeconds(150);
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Start();
            GetImage_Function();
        }

        private void GetImage_Function()
        {
            string[] Files = System.IO.Directory.GetFiles(WallPaper_Para_Default);
            foreach (string Path in Files)
            {
                try
                {
                    ImageList.Add(new BitmapImage(new Uri(Path, UriKind.Absolute)));
                }
                catch (Exception) { }
            }
            Img1.Source = ImageList[0];
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            if (IsImage1)
            {
                Img1.Source = ImageList[Ran.Next(0, ImageList.Count - 1)];
                (Resources["Img1Animation"] as Storyboard).Begin(this);
            }
            else
            {
                Img2.Source = ImageList[Ran.Next(0, ImageList.Count - 1)];
                (Resources["Img2Animation"] as Storyboard).Begin(this);
            }
            IsImage1 = !IsImage1;
        }
        #endregion

        #region 배경화면 변경
        private void WallPaper_One_Change(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            WindowsXml windowxml = new WindowsXml();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                if (ImageList.Count > 1)
                {
                    ImageList.Clear();
                    Img1.Source = new BitmapImage(new Uri(dialog.FileName, UriKind.Absolute));
                    
                    windowxml.WallpaperWrite(dialog.FileName);
                }
                else
                {
                    Img1.Source = new BitmapImage(new Uri(dialog.FileName, UriKind.Absolute));

                    windowxml.WallpaperWrite(dialog.FileName);
                }

            }
        }

        private void WallPaper_Change(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            WallPaper_Para_Default = dialog.SelectedPath;

            //다이얼로그에서 ok 일때만 리스트 초기화, 바탕화면 추가 실행
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                if (ImageList.Count > 1)
                {
                    ImageList.Clear();
                    WallPaper_Slide();
                }
                else
                    WallPaper_Slide();
            }
        }
        #endregion
        #endregion

        private void DownloadedFileCheck()
        {
            ArrayList downfilelist = new ArrayList();
            string[] fileinfo;
            foreach (string str in service.GetUploadedXmlInfo(2))
            {
                fileinfo = new string[2];
                fileinfo = str.Split('\a');

                if (Xml.DownloadedXml.DownloadedFileCheck(fileinfo[0].ToString(), fileinfo[1].ToString()) != null)
                {
                    downfilelist.Add(Xml.DownloadedXml.DownloadedFileCheck(fileinfo[0].ToString(), fileinfo[1].ToString()));
                }
            }

            if (downfilelist.Count != 0)
            {
                StorageFileInfo[] files = null;
                files = service.List(null, 2);
                //   double fileSize = 0;
                string[] Time = new string[downfilelist.Count];
                for (int i = 0; i < downfilelist.Count; i++)
                {
                    foreach (var file in files)
                    {
                        if (downfilelist[i].ToString() == file.VirtualPathk__BackingField)
                        {
                            Time[i] = file.Timek__BackingField.ToString();
                        }
                    }
                }
                //프로그래스바 ㄱㄱ
                string realpath = usbpath + @"SchoolInteligentUSB\FileRoom";
                MultiProgressBar mbar = new MultiProgressBar(service, (string[])downfilelist.ToArray(typeof(string)), 2, realpath);
                mbar.Name = "dd";
                mbar.Closed += mbar_Closed;
                mbar.Show();
                for (int i = 0; i < downfilelist.Count; i++)
                {
                    Xml.DownloadedXml.DownloadedFileWrite(downfilelist[i].ToString(), Time[i]);
                }
            }
            else
            {
                bar_Closed(new DirectoyProgress() { Name="dd"}, null);
            }
        }

        void mbar_Closed(object sender, EventArgs e)
        {
            string path = ((MultiProgressBar)sender).CopyDirectory;
            string realpath = ((MultiProgressBar)sender).RealPath;

            DirectoyProgress bar = new DirectoyProgress(path, realpath, 1);
            if(((MultiProgressBar)sender).Name != null)
                bar.Name = ((MultiProgressBar)sender).Name;
            bar.Closed += bar_Closed;
            bar.Show();
        }

        void bar_Closed(object sender, EventArgs e)
        {
            if (((DirectoyProgress)sender).Name != "dd")
            {
                try
                {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + "Temp";
                Directory.Delete(path, true);
                                }
                catch{ }
            }
            else
            {
                try
                {
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + "Temp";
                    Directory.Delete(path, true);
                }
                catch{ }
                    
                DownloadedBookCheck();
            }
        }

        private void DownloadedBookCheck()
        {
            ArrayList downfilelist = new ArrayList();
            string[] fileinfo;
            foreach (string str in service.GetUploadedXmlInfo(1))
            {
                fileinfo = new string[2];
                fileinfo = str.Split('\a');

                if (Xml.DownloadedXml.DownloadedBookCheck(fileinfo[0].ToString(), fileinfo[1].ToString()) != null)
                {
                    downfilelist.Add(Xml.DownloadedXml.DownloadedBookCheck(fileinfo[0].ToString(), fileinfo[1].ToString()));
                }
            }

            if (downfilelist.Count != 0)
            {
                StorageFileInfo[] files = null;
                files = service.List(null, 1);
                //   double fileSize = 0;
                string[] Time = new string[downfilelist.Count];
                for (int i = 0; i < downfilelist.Count; i++)
                {
                    foreach (var file in files)
                    {
                        if (downfilelist[i].ToString() == file.VirtualPathk__BackingField)
                        {
                            Time[i] = file.Timek__BackingField.ToString();
                        }
                    }
                }
                //프로그래스바 ㄱㄱ
                string realpath = usbpath + @"SchoolInteligentUSB\BookRoom";
                MultiProgressBar mbar = new MultiProgressBar(service, (string[])downfilelist.ToArray(typeof(string)), 1, realpath);
                mbar.Closed += mbar_Closed;
                mbar.Show();
                for (int i = 0; i < downfilelist.Count; i++)
                {
                    Xml.DownloadedXml.DownloadedBookWrite(downfilelist[i].ToString(), Time[i]);
                }
            }
        }

        public void FileRoomTextChange()
        {
            txt_fileroom.Text = "새로운 파일이 있습니다. 파일을 확인하여 주세요.";

            t.Interval = new TimeSpan(0, 0, 1);
            t.Tick += new EventHandler(t_Tick);
            t.Start();
        }

        void t_Tick(object sender, EventArgs e)
        {
            if (txt_fileroom.Foreground == Brushes.Red)
                txt_fileroom.Foreground = Brushes.White;
            else if (txt_fileroom.Foreground == Brushes.White)
                txt_fileroom.Foreground = Brushes.Red;
        }

        #region 종료 & 모드 전환
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            XmlWrite();

            Application.Current.Properties.Remove("USBDrivePath");

            service.Dispose();
            Environment.Exit(0);
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            thread.Abort();
            bw.Dispose();
            this.Close();
        }
        #endregion
    }
}
