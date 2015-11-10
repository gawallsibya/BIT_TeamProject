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

namespace UserWallPaper
{
    #region Data 클래스(자료실 자료 표시)
    public class Data
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

    /// <summary>
    /// SubmissionConnect.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SubmissionConnect : Window
    {
        private delegate void SetProgCallBack(string copydirectory, string userselectdirectory);
        private delegate void deletetempfileCallBack();

        Service service;
        string[] submissioninfo;
        string TeacherID;
        string usbpath, userid, name, filename;
        string title, subject, lastdate; //게시판 쓰기에 사용되는 변수
        int flag;
        Data filedata;
        string path = null;
        string deletefilepath;

        BackgroundWorker bw = new BackgroundWorker();
        ProgressBar pb;

        public SubmissionConnect(Service s, string name, string MainLabel, int pathflag, string i)
        {
            InitializeComponent();

            service = s;
            flag = pathflag;
            namecontent.Text = name;
            BoardName.Content = MainLabel;
            userid = i;

            usbpath = (string)System.Windows.Application.Current.Properties["USBDrivePath"];

            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
        }

        public SubmissionConnect(Service s, string n, string MainLabel, string[] Submissionlist, int pathflag, string i)
        {
            InitializeComponent();

            service = s;
            name = n;
            flag = pathflag;
            userid = i;

            WriteButton.IsEnabled = false;
            namecontent.Text = name;
            BoardName.Content = MainLabel;
            SetList(Submissionlist);

            usbpath = (string)System.Windows.Application.Current.Properties["USBDrivePath"];

            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            bw.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
        }

        private void SetList(string[] submission)
        {
            TeacherID = submission[6];
            title = submission[2];
            filename = submission[4];
            subject = submission[1];
            lastdate = submission[3];
            namecontent.Text = submission[0];
            Subject_Name.Text = submission[1];
            titlecontent.Text = submission[2];
            datecontent.Text = submission[3];
            datatext.Text = submission[4];
            descripton.Text = submission[5];
            WriteButton.IsEnabled = false;
            WriteButton.Background = Brushes.LightGray;

            if (datatext.Text != "")
            {
                upload.IsEnabled = false;
                upload.Background = Brushes.LightGray;
                download.IsEnabled = true;
                download.Background = Brushes.White;
                RefreshFileList();
            }
            else
            {
                upload.IsEnabled = false;
                upload.Background = Brushes.LightGray;
                download.IsEnabled = false;
                download.Background = Brushes.LightGray;
            }

            if (System.Windows.Application.Current.Properties["SubmissionSend"] != null)
            {
                DeleteButton.Visibility = System.Windows.Visibility.Hidden;
                clear_text.Visibility = System.Windows.Visibility.Visible;
            }

            if (userid == submission[6])
            {
                clear_text.Visibility = System.Windows.Visibility.Hidden;
                DeleteButton.Visibility = System.Windows.Visibility.Visible;
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

        private void Write_Submission(object sender, RoutedEventArgs e)
        {
            BoardName.Content = "과제 작성 및 제출";

            WriteButton.IsEnabled = true;
            WriteButton.Background = Brushes.White;

            namecontent.Text = name;
            Subject_Name.Text = subject;
            titlecontent.Text = "";
            datecontent.Text = lastdate;
            datatext.Text = "";
            descripton.Text = "";

            clear_text.Visibility = System.Windows.Visibility.Hidden;
            WriteButton.IsEnabled = true;
            upload.IsEnabled = true;
            upload.Background = Brushes.White;
            download.IsEnabled = false;
            flag = 5;
        }

        private void SetDate(object sender, RoutedEventArgs e)
        {
            clder.Visibility = Visibility.Visible;
            maingrid.IsHitTestVisible = false;
        }

        private void clder_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            datecontent.Text = clder.SelectedDate.Value.ToString("yy년 MM월 dd일");
            clder.Visibility = Visibility.Hidden;
            maingrid.IsHitTestVisible = true;
        }

        private void Write_Out_Click(object sender, RoutedEventArgs e)
        {
            submissioninfo = new string[8];

            submissioninfo[0] = namecontent.Text;
            submissioninfo[1] = Subject_Name.Text;
            submissioninfo[2] = titlecontent.Text;
            submissioninfo[3] = datecontent.Text;
            submissioninfo[4] = datatext.Text;
            submissioninfo[5] = descripton.Text;
            submissioninfo[6] = userid;
            submissioninfo[7] = System.DateTime.Now.ToString("yy년 MM월 dd일 hh시 mm분");

            if (System.Windows.Application.Current.Properties["Suggest"] != null)
            {
                service.writesugesstsubmission(submissioninfo);
                System.Windows.Application.Current.Properties["SubmissionSugesstAdd"] = true;
                System.Windows.Application.Current.Properties.Remove("Suggest");
                this.Close();
            }

            else if (System.Windows.Application.Current.Properties["SubmissionSend"] != null)
            {
                service.writesubmissionlist(TeacherID, submissioninfo);
                service.WriteStudentSumitList(userid, submissioninfo);
                System.Windows.Application.Current.Properties["SubmissionSubitAdd"] = true;
                System.Windows.Application.Current.Properties.Remove("SubmissionSend");
                this.Close();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("게시물을 삭제하시겠습니까?", "게시물 삭제", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                if (System.Windows.Application.Current.Properties["SubmissionSend"] != null)
                {
                    service.DeleteSumitSubmission(userid, title);
                    service.DeleteStudentSumit(userid, title);
                }
                else
                    service.DeleteSubmission(userid, title);
                if (filename != "")
                {
                    service.BoardDeleteFile(filename, 4);
                }
                System.Windows.Application.Current.Properties["SubmissionDelete"] = true;

                this.Close();
            }
            else if (dialogResult == System.Windows.Forms.DialogResult.No)
            {
                return;
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
