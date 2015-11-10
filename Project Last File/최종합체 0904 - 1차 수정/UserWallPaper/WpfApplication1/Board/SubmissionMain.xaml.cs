using System;
using System.Collections;
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
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Threading;

namespace UserWallPaper
{
    /// <summary>
    /// SubmissionMain.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SubmissionMain : Window
    {
        ArrayList submissionarraylist, teacherarraylist, studentarraylist;
        Service service;
        Thread thread;
        string name, type, id;
        int maxcount, flag, nowstate;

        private int PageIndex = 1;
        private int MaxBoardCount = 30;

        public SubmissionMain(Service s, string n, string t, string i)
        {
            InitializeComponent();

            service = s;
            name = n;
            type = t;
            id = i;

            submissionarraylist = new ArrayList();
            teacherarraylist = new ArrayList();
            studentarraylist = new ArrayList();

            SetMenuList();
            SetList();

            thread = new Thread(new ThreadStart(GetList));
            thread.Start();
        }

        private void SetMenuList()
        {
            if (type == "Teacher")
            {
                StudentTab.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                TeacherTab.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void SetList()
        {
            boardlist.Items.Clear();

            submissionarraylist.Clear();
            teacherarraylist.Clear();
            studentarraylist.Clear();

            if (flag == 0)
                SetSubmission();
            else if (flag == 1)
                SetTeacher();
            else if (flag == 2)
                SetStudent();

            string[][] array = new string[maxcount][];
            string[] boardinfo = new string[8];

            if (maxcount >= MaxBoardCount)
            {
                for (int i = 0; i < MaxBoardCount; i++)
                {
                    if (flag == 0)
                        boardinfo = submissionarraylist[i].ToString().Split('\a');
                    else if (flag == 1)
                        boardinfo = teacherarraylist[i].ToString().Split('\a');
                    else if (flag == 2)
                        boardinfo = studentarraylist[i].ToString().Split('\a');

                    array[i] = boardinfo;
                    boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][0], Title = array[i][2], Date = array[i][7] });
                }
            }
            else
            {
                for (int i = 0; i < maxcount; i++)
                {
                    if (flag == 0)
                        boardinfo = submissionarraylist[i].ToString().Split('\a');
                    else if (flag == 1)
                        boardinfo = teacherarraylist[i].ToString().Split('\a');
                    else if (flag == 2)
                        boardinfo = studentarraylist[i].ToString().Split('\a');

                    array[i] = boardinfo;
                    boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][0], Title = array[i][2], Date = array[i][7] });
                }
            }

            if (flag == 0)
                submissionarraylist.Reverse(); //==> 게시판 정보를 빼올때 알맞은놈을 가져오기 위함
            else if (flag == 1)
                teacherarraylist.Reverse(); //==> 게시판 정보를 빼올때 알맞은놈을 가져오기 위함
            else if (flag == 2)
                studentarraylist.Reverse(); //==> 게시판 정보를 빼올때 알맞은놈을 가져오기 위함
        }

        private void SetSubmission()
        {
            submissionarraylist = service.readsugesstsubmissionList();
            submissionarraylist.Reverse();
            maxcount = submissionarraylist.Count;
        }

        private void SetTeacher()
        {
            teacherarraylist = service.readsubmissionlist(id);
            teacherarraylist.Reverse();
            maxcount = teacherarraylist.Count;
        }

        private void SetStudent()
        {
            studentarraylist = service.ReadStudentSumitList(id);
            studentarraylist.Reverse();
            maxcount = studentarraylist.Count;
        }

        private void readcheck_Unchecked(object sender, RoutedEventArgs e)
        {
            nowstate = 0;
            SetList();
        }

        private void readcheck_Checked(object sender, RoutedEventArgs e)
        {
            nowstate = 1;

            boardlist.Items.Clear();

            submissionarraylist.Clear();
            teacherarraylist.Clear();
            studentarraylist.Clear();

            if (flag == 0)
                SetSubmission();
            else if (flag == 1)
                SetTeacher();
            else if (flag == 2)
                SetStudent();

            string[][] array = new string[maxcount][];
            string[] boardinfo = new string[8];

            if (maxcount >= MaxBoardCount)
            {
                for (int i = 0; i < MaxBoardCount; i++)
                {
                    if (flag == 0)
                        boardinfo = submissionarraylist[i].ToString().Split('\a');
                    else if (flag == 1)
                        boardinfo = teacherarraylist[i].ToString().Split('\a');
                    else if (flag == 2)
                        boardinfo = studentarraylist[i].ToString().Split('\a');

                    if (service.ReadSubmissionBoardCheck(id, boardinfo[2], boardinfo[7]) == true)
                    {
                        array[i] = boardinfo;
                        boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][0], Title = array[i][2], Date = array[i][7] });
                    }
                }
            }
            else
            {
                for (int i = 0; i < maxcount; i++)
                {
                    if (flag == 0)
                        boardinfo = submissionarraylist[i].ToString().Split('\a');
                    else if (flag == 1)
                        boardinfo = teacherarraylist[i].ToString().Split('\a');
                    else if (flag == 2)
                        boardinfo = studentarraylist[i].ToString().Split('\a');

                    if (service.ReadSubmissionBoardCheck(id, boardinfo[2], boardinfo[7]) == true)
                    {
                        array[i] = boardinfo;
                        boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][0], Title = array[i][2], Date = array[i][7] });
                    }
                }
            }

            if (flag == 0)
                submissionarraylist.Reverse(); //==> 게시판 정보를 빼올때 알맞은놈을 가져오기 위함
            else if (flag == 1)
                teacherarraylist.Reverse(); //==> 게시판 정보를 빼올때 알맞은놈을 가져오기 위함
            else if (flag == 2)
                studentarraylist.Reverse(); //==> 게시판 정보를 빼올때 알맞은놈을 가져오기 위함
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (searchname.Text != "")
            {
                nowstate = 2;

                boardlist.Items.Clear();

                submissionarraylist.Clear();
                teacherarraylist.Clear();
                studentarraylist.Clear();

                if (flag == 0)
                    SetSubmission();
                else if (flag == 1)
                    SetTeacher();
                else if (flag == 2)
                    SetStudent();

                string[][] array = new string[maxcount][];
                string[] boardinfo = new string[8];

                if (maxcount >= MaxBoardCount)
                {
                    for (int i = 0; i < MaxBoardCount; i++)
                    {
                        if (flag == 0)
                            boardinfo = submissionarraylist[i].ToString().Split('\a');
                        else if (flag == 1)
                            boardinfo = teacherarraylist[i].ToString().Split('\a');
                        else if (flag == 2)
                            boardinfo = studentarraylist[i].ToString().Split('\a');

                        if (searchname.Text == boardinfo[0])
                        {
                            array[i] = boardinfo;
                            boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][0], Title = array[i][2], Date = array[i][7] });
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < maxcount; i++)
                    {
                        if (flag == 0)
                            boardinfo = submissionarraylist[i].ToString().Split('\a');
                        else if (flag == 1)
                            boardinfo = teacherarraylist[i].ToString().Split('\a');
                        else if (flag == 2)
                            boardinfo = studentarraylist[i].ToString().Split('\a');

                        if (searchname.Text == boardinfo[0])
                        {
                            array[i] = boardinfo;
                            boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][0], Title = array[i][2], Date = array[i][7] });
                        }
                    }
                }

                try
                {
                    if (array[0] == null)
                    {
                        SetList();
                        System.Windows.MessageBox.Show(searchname.Text + "님의 게시글이 없습니다.");
                        return;
                    }
                }
                catch 
                {
                    SetList();
                    System.Windows.MessageBox.Show(searchname.Text + "님의 게시글이 없습니다.");
                    return;
                }

                if (flag == 0)
                    submissionarraylist.Reverse(); //==> 게시판 정보를 빼올때 알맞은놈을 가져오기 위함
                else if (flag == 1)
                    teacherarraylist.Reverse(); //==> 게시판 정보를 빼올때 알맞은놈을 가져오기 위함
                else if (flag == 2)
                    studentarraylist.Reverse(); //==> 게시판 정보를 빼올때 알맞은놈을 가져오기 위함
            }
            else
                System.Windows.Forms.MessageBox.Show("작성자 이름을 입력해주세요.");
        }

        private void Write_Board(object sender, RoutedEventArgs e)
        {
            string mainlabel = "과제 제시";

            System.Windows.Application.Current.Properties["Suggest"] = true;
            SubmissionConnect missionsuggest = new SubmissionConnect(service, name, mainlabel, 4, id);
            missionsuggest.ShowDialog();
        }

        private void SelectList_Click(object sender, SelectionChangedEventArgs e)
        {
            if (boardlist.SelectedIndex != -1)
            {
                string str = null;
                int sendflag = 4;
                var selectindex = (boardlist.SelectedItem as BoardInfo).Number - 1;

                if (flag == 0)
                    str = submissionarraylist[selectindex].ToString();
                else if (flag == 1)
                {
                    str = teacherarraylist[selectindex].ToString();
                    sendflag = 5;
                }
                else if (flag == 2)
                {
                    str = studentarraylist[selectindex].ToString();
                    sendflag = 5;
                }

                if (type == "Student")
                    System.Windows.Application.Current.Properties["SubmissionSend"] = true;

                string[] submistlist = new string[8];
                submistlist = str.Split('\a');

                string mainlabel = "게시물 확인";
                SubmissionConnect missionsuggest = new SubmissionConnect(service, name, mainlabel, submistlist, sendflag, id);
                missionsuggest.DataContext = boardlist.SelectedItem;
                missionsuggest.ShowDialog();

                service.ReadSubmissionBoardWrite(id, submistlist[2], submistlist[7]); // 읽은파일 기록 표시
            }

            boardlist.SelectedIndex = -1;
        }

        private void GetList()
        {
            while (true)
            {
                if (System.Windows.Application.Current.Properties["SubmissionSugesstAdd"] != null && (bool)System.Windows.Application.Current.Properties["SubmissionSugesstAdd"] == true)
                {
                    System.Windows.Application.Current.Properties["SubmissionSugesstAdd"] = false;
                    Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
                    {
                        SetList();
                    }));
                }
                else if (System.Windows.Application.Current.Properties["SubmissionSubitAdd"] != null && (bool)System.Windows.Application.Current.Properties["SubmissionSubitAdd"] == true)
                {
                    System.Windows.Application.Current.Properties["SubmissionSubitAdd"] = false;
                    Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
                    {
                        SetList();
                    }));
                }
                else if (System.Windows.Application.Current.Properties["SubmissionDelete"] != null && (bool)System.Windows.Application.Current.Properties["SubmissionDelete"] == true)
                {
                    System.Windows.Application.Current.Properties["SubmissionDelete"] = false;
                    Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
                    {
                        SetList();
                    }));
                }
            }
        }

        private void Page_Change(object sender, RoutedEventArgs e)
        {
            if (maxcount <= MaxBoardCount)
            {
                return;
            }

            submissionarraylist.Clear();
            teacherarraylist.Clear();
            studentarraylist.Clear();

            if (flag == 0)
                SetSubmission();
            else if (flag == 1)
                SetTeacher();
            else if (flag == 2)
                SetStudent();

            string[][] array = new string[maxcount][];
            string[] boardinfo = new string[8];

            string flagbutton = ((System.Windows.Controls.Button)sender).Content.ToString();

            switch (flagbutton)
            {
                case ">":
                    if (maxcount > (PageIndex * MaxBoardCount))
                    {
                        boardlist.Items.Clear();
                        if (maxcount >= ((PageIndex * MaxBoardCount) + MaxBoardCount))
                        {
                            for (int i = PageIndex * MaxBoardCount; i < ((PageIndex * MaxBoardCount) + MaxBoardCount); i++)
                            {
                                if (flag == 0)
                                    boardinfo = submissionarraylist[i].ToString().Split('\a');
                                else if (flag == 1)
                                    boardinfo = teacherarraylist[i].ToString().Split('\a');
                                else if (flag == 2)
                                    boardinfo = studentarraylist[i].ToString().Split('\a');

                                if (nowstate == 0)
                                {
                                    array[i] = boardinfo;
                                    boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][0], Title = array[i][2], Date = array[i][7] });
                                }
                                else if (nowstate == 1)
                                {
                                    if (service.ReadStudentBoardCheck(id, boardinfo[0], boardinfo[5]) == true)
                                    {
                                        array[i] = boardinfo;
                                        boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][0], Title = array[i][2], Date = array[i][7] });
                                    }
                                }
                                else if (nowstate == 2)
                                {
                                    if (searchname.Text == boardinfo[2])
                                    {
                                        array[i] = boardinfo;
                                        boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][0], Title = array[i][2], Date = array[i][7] });
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = PageIndex * MaxBoardCount; i < maxcount; i++)
                            {
                                if (flag == 0)
                                    boardinfo = submissionarraylist[i].ToString().Split('\a');
                                else if (flag == 1)
                                    boardinfo = teacherarraylist[i].ToString().Split('\a');
                                else if (flag == 2)
                                    boardinfo = studentarraylist[i].ToString().Split('\a');

                                if (nowstate == 0)
                                {
                                    array[i] = boardinfo;
                                    boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][0], Title = array[i][2], Date = array[i][7] });
                                }
                                else if (nowstate == 1)
                                {
                                    if (service.ReadStudentBoardCheck(id, boardinfo[0], boardinfo[5]) == true)
                                    {
                                        array[i] = boardinfo;
                                        boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][0], Title = array[i][2], Date = array[i][7] });
                                    }
                                }
                                else if (nowstate == 2)
                                {
                                    if (searchname.Text == boardinfo[2])
                                    {
                                        array[i] = boardinfo;
                                        boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][0], Title = array[i][2], Date = array[i][7] });
                                    }
                                }
                            }
                        }

                        PageIndex += 1;
                    }
                    else
                    {
                        return;
                    }
                    break;
                case "<":
                    if (PageIndex > 1)
                    {
                        boardlist.Items.Clear();

                        PageIndex -= 1;

                        for (int i = ((PageIndex * MaxBoardCount) - MaxBoardCount); i < (PageIndex * MaxBoardCount); i++)
                        {
                            if (flag == 0)
                                boardinfo = submissionarraylist[i].ToString().Split('\a');
                            else if (flag == 1)
                                boardinfo = teacherarraylist[i].ToString().Split('\a');
                            else if (flag == 2)
                                boardinfo = studentarraylist[i].ToString().Split('\a');

                            if (nowstate == 0)
                            {
                                array[i] = boardinfo;
                                boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][0], Title = array[i][2], Date = array[i][7] });
                            }
                            else if (nowstate == 1)
                            {
                                if (service.ReadStudentBoardCheck(id, boardinfo[0], boardinfo[5]) == true)
                                {
                                    array[i] = boardinfo;
                                    boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][0], Title = array[i][2], Date = array[i][7] });
                                }
                            }
                            else if (nowstate == 2)
                            {
                                if (searchname.Text == boardinfo[2])
                                {
                                    array[i] = boardinfo;
                                    boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][0], Title = array[i][2], Date = array[i][7] });
                                }
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                    break;
                case "<<":
                    PageIndex = 2;
                    if (PageIndex > 1)
                    {
                        boardlist.Items.Clear();

                        PageIndex -= 1;

                        for (int i = ((PageIndex * MaxBoardCount) - MaxBoardCount); i < (PageIndex * MaxBoardCount); i++)
                        {
                            if (flag == 0)
                                boardinfo = submissionarraylist[i].ToString().Split('\a');
                            else if (flag == 1)
                                boardinfo = teacherarraylist[i].ToString().Split('\a');
                            else if (flag == 2)
                                boardinfo = studentarraylist[i].ToString().Split('\a');

                            if (nowstate == 0)
                            {
                                array[i] = boardinfo;
                                boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][0], Title = array[i][2], Date = array[i][7] });
                            }
                            else if (nowstate == 1)
                            {
                                if (service.ReadStudentBoardCheck(id, boardinfo[0], boardinfo[5]) == true)
                                {
                                    array[i] = boardinfo;
                                    boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][0], Title = array[i][2], Date = array[i][7] });
                                }
                            }
                            else if (nowstate == 2)
                            {
                                if (searchname.Text == boardinfo[2])
                                {
                                    array[i] = boardinfo;
                                    boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][0], Title = array[i][2], Date = array[i][7] });
                                }
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                    break;
                case ">>":
                    PageIndex = (maxcount / MaxBoardCount);
                    if (maxcount > (PageIndex * MaxBoardCount))
                    {
                        boardlist.Items.Clear();

                        if (maxcount >= ((PageIndex * MaxBoardCount) + MaxBoardCount))
                        {
                            for (int i = PageIndex * MaxBoardCount; i < ((PageIndex * MaxBoardCount) + MaxBoardCount); i++)
                            {
                                if (flag == 0)
                                    boardinfo = submissionarraylist[i].ToString().Split('\a');
                                else if (flag == 1)
                                    boardinfo = teacherarraylist[i].ToString().Split('\a');
                                else if (flag == 2)
                                    boardinfo = studentarraylist[i].ToString().Split('\a');

                                if (nowstate == 0)
                                {
                                    array[i] = boardinfo;
                                    boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][0], Title = array[i][2], Date = array[i][7] });
                                }
                                else if (nowstate == 1)
                                {
                                    if (service.ReadStudentBoardCheck(id, boardinfo[0], boardinfo[5]) == true)
                                    {
                                        array[i] = boardinfo;
                                        boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][0], Title = array[i][2], Date = array[i][7] });
                                    }
                                }
                                else if (nowstate == 2)
                                {
                                    if (searchname.Text == boardinfo[2])
                                    {
                                        array[i] = boardinfo;
                                        boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][0], Title = array[i][2], Date = array[i][7] });
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = PageIndex * MaxBoardCount; i < maxcount; i++)
                            {
                                if (flag == 0)
                                    boardinfo = submissionarraylist[i].ToString().Split('\a');
                                else if (flag == 1)
                                    boardinfo = teacherarraylist[i].ToString().Split('\a');
                                else if (flag == 2)
                                    boardinfo = studentarraylist[i].ToString().Split('\a');

                                if (nowstate == 0)
                                {
                                    array[i] = boardinfo;
                                    boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][0], Title = array[i][2], Date = array[i][7] });
                                }
                                else if (nowstate == 1)
                                {
                                    if (service.ReadStudentBoardCheck(id, boardinfo[0], boardinfo[5]) == true)
                                    {
                                        array[i] = boardinfo;
                                        boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][0], Title = array[i][2], Date = array[i][7] });
                                    }
                                }
                                else if (nowstate == 2)
                                {
                                    if (searchname.Text == boardinfo[2])
                                    {
                                        array[i] = boardinfo;
                                        boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][0], Title = array[i][2], Date = array[i][7] });
                                    }
                                }
                            }
                        }

                        PageIndex += 1;
                    }
                    else
                    {
                        return;
                    }
                    break;
            }

            pageNumber.Content = PageIndex;

            if (flag == 0)
                submissionarraylist.Reverse(); //==> 게시판 정보를 빼올때 알맞은놈을 가져오기 위함
            else if (flag == 1)
                teacherarraylist.Reverse(); //==> 게시판 정보를 빼올때 알맞은놈을 가져오기 위함
            else if (flag == 2)
                studentarraylist.Reverse(); //==> 게시판 정보를 빼올때 알맞은놈을 가져오기 위함
        }

        #region 탭 컨트롤 변환 함수
        private void Submission_Click(object sender, RoutedEventArgs e)
        {
            flag = 0;
            searchname.Text = "";
            BoardName.Content = "과제 확인 / 과제 제출 게시판";
            Sugesstbutton.Visibility = System.Windows.Visibility.Hidden;
            SetList();
        }

        private void TeacherTab_Click(object sender, RoutedEventArgs e)
        {
            flag = 1;
            searchname.Text = "";
            BoardName.Content = "과제 제시 / 과제 제출 확인 게시판";
            Sugesstbutton.Visibility = System.Windows.Visibility.Visible;
            SetList();
        }

        private void StudentTab_Click(object sender, RoutedEventArgs e)
        {
            flag = 2;
            searchname.Text = "";
            BoardName.Content = "과제 제출 확인 게시판";
            Sugesstbutton.Visibility = System.Windows.Visibility.Hidden;
            SetList();
        }
        #endregion

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
            thread.Abort();
            this.Close();
        }
    }
}
