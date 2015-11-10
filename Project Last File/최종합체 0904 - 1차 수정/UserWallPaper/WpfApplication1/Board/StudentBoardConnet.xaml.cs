using System;
using System.Collections;
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

namespace UserWallPaper
{
    /// <summary>
    /// StudentBoardConnet.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StudentBoardConnet : Window
    {
        private delegate void SetProgCallBack(string copydirectory, string userselectdirectory);
        private delegate void deletetempfileCallBack();

        ArrayList studentboardlist;
        Service service;
        Data filedata;
        int flag;
        string usbpath, userid, name;
        string path = null;
        string deletefilepath;

        BackgroundWorker bw = new BackgroundWorker();
        ProgressBar pb;

        public StudentBoardConnet(Service s, string n, string i, string mainlabel, int f)
        {
            InitializeComponent();

            service = s;
            Setinfo(n, i, mainlabel, f);

            usbpath = (string)System.Windows.Application.Current.Properties["USBDrivePath"];

            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
        }

        public StudentBoardConnet(Service s, string[] info, string mainlabel, string id, int f)
        {
            InitializeComponent();

            service = s;
            Setinfo2(info, mainlabel, id, f);

            usbpath = (string)System.Windows.Application.Current.Properties["USBDrivePath"];

            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
        }

        private void Setinfo(string n, string i, string m, int f)
        {
            flag = f;
            name = n;
            userid = i;
            UserName.Text = n;
            BoardName.Content = m;
        }

        private void Setinfo2(string[] info, string m, string id, int f)
        {
            flag = f;
            BoardName.Content = m;
            userid = id;

            id = info[1];
            UserTitle.Text = info[0];
            UserTitle.IsReadOnly = true;
            UserName.Text = info[2];
            datatext.Text = info[3];
            datatext.IsReadOnly = true;
            descripton.Text = info[4];
            descripton.IsReadOnly = true;

            if (userid == info[1])
            {
                DeleteButton.Visibility = System.Windows.Visibility.Visible;
            }

            upload.IsEnabled = false;
            upload.Background = Brushes.LightGray;
            WriteButton.IsEnabled = false;
            WriteButton.Background = Brushes.LightGray;

            if (datatext.Text != "")
            {
                download.IsEnabled = true;
                download.Background = Brushes.White;
                RefreshFileList();
            }
        }

        private void RefreshFileList()
        {
            StorageFileInfo[] files = null;
            files = service.List(null, flag);

            foreach (var file in files)
            {
                float fileSize = (float)file.Sizek__BackingField / 1024.0f;
                string suffix = "Kb";

                if (fileSize > 1000.0f)
                {
                    fileSize /= 1024.0f;
                    suffix = "Mb";
                }

                if (datatext.Text == file.VirtualPathk__BackingField)
                {
                    filedata = new Data { Path = file.VirtualPathk__BackingField, Date = file.Timek__BackingField, FileSize = string.Format("{0:0.0} {1}", fileSize, suffix) };
                }
            }
        }

        private void Upload_Click(object sender, RoutedEventArgs e)
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
                datatext.Text = path.Substring(path.LastIndexOf("\\") + 1);

                if (bw.IsBusy != true)
                {
                    // Start the asynchronous operation.
                    bw.RunWorkerAsync();
                }
            }
        }

        private void DownLoad_Click(object sender, RoutedEventArgs e)
        {
            //System.Windows.Controls.ListViewItem item = (System.Windows.Controls.ListViewItem)FileList.SelectedItems[0];
            // Strip off 'Root' from the full path
            string path = (filedata).Path;

            CreateFolder(@"SchoolInteligentUSB\FileRoom");

            SaveFileDialog dlg = new SaveFileDialog()
            {
                RestoreDirectory = true,
                OverwritePrompt = true,
                Title = "Save as...",
                FileName = System.IO.Path.GetFileName(path),
                InitialDirectory = usbpath + @"SchoolInteligentUSB\FileRoom"
            };

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(dlg.FileName))
                {
                    // Get the file from the server

                    string tempdirectroy = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + "Temp";
                    Directory.CreateDirectory(tempdirectroy);
                    string filename = dlg.FileName.Substring(dlg.FileName.LastIndexOf("\\") + 1);

                    Stream output = new FileStream(tempdirectroy + "\\" + filename, FileMode.Create);

                    //Stream output = new FileStream(dlg.FileName, FileMode.Create);

                    Stream downloadStream = service.GetFile(path, flag);
                    // 파일 사이즈 정보 가져오기
                    StorageFileInfo[] files = null;
                    files = service.List(null, flag);
                    double fileSize = 0;
                    foreach (var file in files)
                    {
                        if (path == file.VirtualPathk__BackingField)
                            fileSize = (double)file.Sizek__BackingField;
                    }
                    deletefilepath = tempdirectroy;
                    //프로그래스바 ㄱㄱ
                    ProgressBar bar = new ProgressBar(downloadStream, output, fileSize, deletefilepath, dlg.FileName);
                    bar.getfile += GetFile;
                    bar.Show();
                }
            }
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
                //string path = new FileInfo(copydirectory).Directory.ToString();
                string realpath = userselectdirectory;

                DirectoyProgress bar = new DirectoyProgress(copydirectory, realpath, 0);
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
                Directory.Delete(deletefilepath, true);
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

        private void Write_Out_Click(object sender, RoutedEventArgs e)
        {
            studentboardlist = new ArrayList(service.StudentBoardRead());
            string[] boardinfo = new string[6];

            boardinfo[0] = userid;
            boardinfo[1] = UserTitle.Text;
            boardinfo[2] = UserName.Text;
            boardinfo[3] = datatext.Text;
            boardinfo[4] = descripton.Text;
            boardinfo[5] = System.DateTime.Now.ToString("yy년 MM월 dd일 hh시 mm분");
            service.StudentBoardWrite(boardinfo);
            System.Windows.Application.Current.Properties["StudentBoardAdd"] = true;
            this.Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("게시물을 삭제하시겠습니까?", "게시물 삭제", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                service.DeleteStudentBoard(userid, UserTitle.Text);
                if (datatext.Text != "")
                {
                    service.BoardDeleteFile(datatext.Text, 3);
                }
                System.Windows.Application.Current.Properties["StudentBoardDelete"] = true;

                this.Close();
            }
            else if (dialogResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
        }

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

        private void CloseForm(object sender, RoutedEventArgs e)
        {
            bw.Dispose();
            this.Close();
        }
    }
}
