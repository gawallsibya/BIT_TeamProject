using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using UserWallPaper.ServiceReference1;
using System.ComponentModel;
using System.Windows.Shell;

namespace UserWallPaper
{
    /// <summary>
    /// FileRoom.xaml에 대한 상호 작용 논리
    /// </summary>

    #region Data 클래스(자료실 자료 표시)
    public class BookData
    {
        string path;
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        DateTime dt;
        public DateTime Date
        {
            get { return dt; }
            set { dt = value; }
        }

        string filesize;
        public string FileSize
        {
            get { return filesize; }
            set { filesize = value; }
        }
    }
    #endregion

    public partial class FileRoom : Window
    {
        private delegate void SetProgCallBack(string copydirectory, string userselectdirectory);
        private delegate void deletetempfileCallBack();

        ObservableCollection<BookData> mDataList;
        Service service;
        int flag;
        string name, type, usbpath;

        Thread filethread;

        BackgroundWorker bw = new BackgroundWorker();

        public ObservableCollection<BookData> DataList
        {
            get
            {
                return mDataList;
            }
        }

        public FileRoom(Service s, string n, string t)
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Window_Loaded);

            filethread = new Thread(new ThreadStart(thread));
            filethread.Start();

            mDataList = new ObservableCollection<BookData>();

            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);

            SetFileRoom(s, n, t);

            usbpath = (string)System.Windows.Application.Current.Properties["USBDrivePath"];
        }

        private void SetFileRoom(Service s, string n, string t)
        {
            service = s;
            flag = 2;
            name = n;
            type = t;

            if (type != "Teacher")
            {
                FileUpload.IsEnabled = false;
                FileDelete.IsEnabled = false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RefreshFileList();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void RefreshFileList()
        {
            StorageFileInfo[] files = null;

            files = service.List(null, flag);

            FileList.Items.Clear();

            foreach (var file in files)
            {
                IconOnCanvas iconOnCanva = new IconOnCanvas();
                string[] str = file.VirtualPathk__BackingField.Split('.');
                iconOnCanva.Icontxtblock.Text = str[str.Length - 1];
                iconOnCanva.IconTextBox.Text = file.VirtualPathk__BackingField;
                iconOnCanva.ChangeTextBackgroundColor();
                float fileSize = (float)file.Sizek__BackingField / 1024.0f;
                string suffix = "KB";

                if (fileSize > 1000.0f)
                {
                    fileSize /= 1024.0f;
                    suffix = "MB";
                }

                iconOnCanva.FileName.Text = "이름 : " + file.VirtualPathk__BackingField;
                iconOnCanva.Size.Text = "크기 : " + string.Format("{0:0.0} {1}", fileSize, suffix);
                iconOnCanva.Date.Text = "업로드 날짜 : " + Convert.ToString(file.Timek__BackingField);

                FileList.Items.Add(iconOnCanva);
            }
        }

        private void DownloadButton_Click(object sender, EventArgs e)
        {
            if (FileList.SelectedItems.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("You must select a file to download");
            }
            else
            {
                string[] path = new string[FileList.SelectedItems.Count];
                for (int i = 0; i < FileList.SelectedItems.Count; i++)
                {
                    path[i] = (FileList.SelectedItems[i] as IconOnCanvas).IconTextBox.Text;
                }
                DownloadFile(path);
            }
        }

        private void DownloadFile(string[] filepath)
        {
            string npath = "";
            if (flag == 2)
            {
                npath = @"SchoolInteligentUSB\FileRoom";
                CreateFolder(npath);
            }
            else
            {
                npath = @"SchoolInteligentUSB\BookRoom";
                CreateFolder(npath);
            }

            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog()
            {
                ShowNewFolderButton = true,     
                Description = 
            "Select the directory that you want to use as the default.",
                SelectedPath = System.IO.Path.Combine(usbpath, npath),
             //   RootFolder = new DirectoryInfo(System.IO.Path.Combine(usbpath, npath)),
                
            };
            dlg.ShowDialog();

                //프로그래스바 ㄱㄱ
            MultiProgressBar mbar = new MultiProgressBar(service, filepath, flag, dlg.SelectedPath);
            mbar.getfile += GetFile;
            mbar.Show();
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

        private void UploadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                Title = "Select a file to upload",
                RestoreDirectory = true,
                CheckFileExists = true
            };

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = dlg.FileName;

                if (bw.IsBusy != true)
                {
                    // Start the asynchronous operation.
                    bw.RunWorkerAsync();
                }
            }
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
                        service.UploadFile(fileInfo.Name, fileInfo.Length, 0, uploadStreamWithProgress, flag);
                    }
                }
            }
        }

        ProgressBar pb;
        string path = null;

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

        private void thread()
        {
            while (true)
            {
                if (System.Windows.Application.Current.Properties["RefreshFileList"] != null && (bool)System.Windows.Application.Current.Properties["RefreshFileList"] == true)
                {
                    System.Windows.Application.Current.Properties["RefreshFileList"] = false;
                    Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
                    {
                        RefreshFileList();
                    }));
                }
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (FileList.SelectedItems.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("You must select a file to delete");
            }
            else
            {
                System.Windows.Controls.ListBox lb = new System.Windows.Controls.ListBox();
                for (int i = 0; i < FileList.SelectedItems.Count; i++)
                {
                    string virtualPath = (FileList.SelectedItems[i] as IconOnCanvas).IconTextBox.Text;

                    service.DeleteFile(virtualPath, flag);
                }
            }

            RefreshFileList();
        }

        #region 탭 컨트롤 변환 함수
        private void FileRoom_Click(object sender, RoutedEventArgs e)
        {
            flag = 2;
            BoardName.Content = "참고 자료 게시판";
            RefreshFileList();
        }

        private void BookRoom_Click(object sender, RoutedEventArgs e)
        {
            flag = 1;
            BoardName.Content = "교재 자료 게시판";
            RefreshFileList();
        }
        #endregion

        public void CreateFolder(string path)
        {
            if (Directory.Exists(usbpath + path) == false)
                Directory.CreateDirectory(usbpath + path);
        }

        private void Window_Move(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void ThisMinimize(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void ThisClose(object sender, RoutedEventArgs e)
        {
            filethread.Abort();
            bw.Dispose();
            this.Close();
        }
    }
}
