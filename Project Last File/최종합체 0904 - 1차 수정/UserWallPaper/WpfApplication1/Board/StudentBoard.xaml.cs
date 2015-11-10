using System;
using System.Collections;
using System.Collections.ObjectModel;
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
    public partial class StudentBoard : Window
    {
        ArrayList studentboardlist;
        Service service;
        Thread thread;
        string name, id;
        int nowstate;

        private int PageIndex = 1;
        private int MaxBoardCount = 30;

        public StudentBoard(Service s, string n, string i)
        {
            InitializeComponent();

            service = s;
            name = n;
            id = i;

            studentboardlist = new ArrayList();

            SetBoardList();

            thread = new Thread(new ThreadStart(GetList));
            thread.Start();
        }

        private void SetBoardList()
        {
            boardlist.Items.Clear();
            studentboardlist.Clear();

            studentboardlist = service.StudentBoardRead();
            studentboardlist.Reverse();
            string[][] array = new string[studentboardlist.Count][];
            string[] boardinfo = new string[6];
            int maxcount = studentboardlist.Count;

            if (studentboardlist.Count >= MaxBoardCount)
            {
                for (int i = 0; i < MaxBoardCount; i++)
                {
                    boardinfo = studentboardlist[i].ToString().Split('\a');

                    array[i] = boardinfo;
                    boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][2], Title = array[i][0], Date = array[i][5] });
                }
            }
            else
            {
                for (int i = 0; i < studentboardlist.Count; i++)
                {
                    boardinfo = studentboardlist[i].ToString().Split('\a');

                    array[i] = boardinfo;
                    boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][2], Title = array[i][0], Date = array[i][5] });
                }
            }
            studentboardlist.Reverse(); //==> 게시판 정보를 빼올때 알맞은놈을 가져오기 위함
        }

        private void Page_Change(object sender, RoutedEventArgs e)
        {
            if (studentboardlist.Count <= MaxBoardCount)
            {
                return;
            }

            studentboardlist.Clear();
            studentboardlist = service.StudentBoardRead();
            studentboardlist.Reverse();
            string[][] array = new string[studentboardlist.Count][];
            string[] boardinfo = new string[6];
            int maxcount = studentboardlist.Count;

            string flag = ((System.Windows.Controls.Button)sender).Content.ToString();

            switch (flag)
            {
                case ">":
                    if (studentboardlist.Count > (PageIndex * MaxBoardCount))
                    {
                        boardlist.Items.Clear();
                        if (studentboardlist.Count >= ((PageIndex * MaxBoardCount) + MaxBoardCount))
                        {
                            for (int i = PageIndex * MaxBoardCount; i < ((PageIndex * MaxBoardCount) + MaxBoardCount); i++)
                            {
                                boardinfo = studentboardlist[i].ToString().Split('\a');

                                if (nowstate == 0)
                                {
                                    array[i] = boardinfo;
                                    boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][2], Title = array[i][0], Date = array[i][5] });
                                }
                                else if (nowstate == 1)
                                {
                                    if (service.ReadStudentBoardCheck(id, boardinfo[0], boardinfo[5]) == true)
                                    {
                                        array[i] = boardinfo;
                                        boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][2], Title = array[i][0], Date = array[i][5] });
                                    }
                                }
                                else if (nowstate == 2)
                                {
                                    if (searchname.Text == boardinfo[2])
                                    {
                                        array[i] = boardinfo;
                                        boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][2], Title = array[i][0], Date = array[i][5] });
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = PageIndex * MaxBoardCount; i < studentboardlist.Count; i++)
                            {
                                boardinfo = studentboardlist[i].ToString().Split('\a');

                                if (nowstate == 0)
                                {
                                    array[i] = boardinfo;
                                    boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][2], Title = array[i][0], Date = array[i][5] });
                                }
                                else if (nowstate == 1)
                                {
                                    if (service.ReadStudentBoardCheck(id, boardinfo[0], boardinfo[5]) == true)
                                    {
                                        array[i] = boardinfo;
                                        boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][2], Title = array[i][0], Date = array[i][5] });
                                    }
                                }
                                else if (nowstate == 2)
                                {
                                    if (searchname.Text == boardinfo[2])
                                    {
                                        array[i] = boardinfo;
                                        boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][2], Title = array[i][0], Date = array[i][5] });
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
                            boardinfo = studentboardlist[i].ToString().Split('\a');

                            if (nowstate == 0)
                            {
                                array[i] = boardinfo;
                                boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][2], Title = array[i][0], Date = array[i][5] });
                            }
                            else if (nowstate == 1)
                            {
                                if (service.ReadStudentBoardCheck(id, boardinfo[0], boardinfo[5]) == true)
                                {
                                    array[i] = boardinfo;
                                    boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][2], Title = array[i][0], Date = array[i][5] });
                                }
                            }
                            else if (nowstate == 2)
                            {
                                if (searchname.Text == boardinfo[2])
                                {
                                    array[i] = boardinfo;
                                    boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][2], Title = array[i][0], Date = array[i][5] });
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
                            boardinfo = studentboardlist[i].ToString().Split('\a');

                            if (nowstate == 0)
                            {
                                array[i] = boardinfo;
                                boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][2], Title = array[i][0], Date = array[i][5] });
                            }
                            else if (nowstate == 1)
                            {
                                if (service.ReadStudentBoardCheck(id, boardinfo[0], boardinfo[5]) == true)
                                {
                                    array[i] = boardinfo;
                                    boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][2], Title = array[i][0], Date = array[i][5] });
                                }
                            }
                            else if (nowstate == 2)
                            {
                                if (searchname.Text == boardinfo[2])
                                {
                                    array[i] = boardinfo;
                                    boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][2], Title = array[i][0], Date = array[i][5] });
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
                    PageIndex = (studentboardlist.Count / MaxBoardCount);
                    if (studentboardlist.Count > (PageIndex * MaxBoardCount))
                    {
                        boardlist.Items.Clear();

                        if (studentboardlist.Count >= ((PageIndex * MaxBoardCount) + MaxBoardCount))
                        {
                            for (int i = PageIndex * MaxBoardCount; i < ((PageIndex * MaxBoardCount) + MaxBoardCount); i++)
                            {
                                boardinfo = studentboardlist[i].ToString().Split('\a');

                                if (nowstate == 0)
                                {
                                    array[i] = boardinfo;
                                    boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][2], Title = array[i][0], Date = array[i][5] });
                                }
                                else if (nowstate == 1)
                                {
                                    if (service.ReadStudentBoardCheck(id, boardinfo[0], boardinfo[5]) == true)
                                    {
                                        array[i] = boardinfo;
                                        boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][2], Title = array[i][0], Date = array[i][5] });
                                    }
                                }
                                else if (nowstate == 2)
                                {
                                    if (searchname.Text == boardinfo[2])
                                    {
                                        array[i] = boardinfo;
                                        boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][2], Title = array[i][0], Date = array[i][5] });
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = PageIndex * MaxBoardCount; i < studentboardlist.Count; i++)
                            {
                                boardinfo = studentboardlist[i].ToString().Split('\a');

                                if (nowstate == 0)
                                {
                                    array[i] = boardinfo;
                                    boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][2], Title = array[i][0], Date = array[i][5] });
                                }
                                else if (nowstate == 1)
                                {
                                    if (service.ReadStudentBoardCheck(id, boardinfo[0], boardinfo[5]) == true)
                                    {
                                        array[i] = boardinfo;
                                        boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][2], Title = array[i][0], Date = array[i][5] });
                                    }
                                }
                                else if (nowstate == 2)
                                {
                                    if (searchname.Text == boardinfo[2])
                                    {
                                        array[i] = boardinfo;
                                        boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][2], Title = array[i][0], Date = array[i][5] });
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
            studentboardlist.Reverse(); //==> 게시판 정보를 빼올때 알맞은놈을 가져오기 위함
        }

        private void SelectList_Click(object sender, SelectionChangedEventArgs e)
        {
            if (boardlist.SelectedIndex != -1)
            {
                var selectindex = (boardlist.SelectedItem as BoardInfo).Number - 1;
                string str = studentboardlist[selectindex].ToString();

                string[] submistlist = new string[6];
                submistlist = str.Split('\a');

                string mainlabel = "게시물 확인";
                StudentBoardConnet boardcheck = new StudentBoardConnet(service, submistlist, mainlabel, id, 3);
                boardcheck.DataContext = boardlist.SelectedItem;
                boardcheck.ShowDialog();

                service.ReadStudentBoardWrite(id, submistlist[0], submistlist[5]); // 읽은파일 기록 표시
            }

            boardlist.SelectedIndex = -1;
        }

        private void readcheck_Unchecked(object sender, RoutedEventArgs e)
        {
            nowstate = 0;
            SetBoardList();
        }

        private void readcheck_Checked(object sender, RoutedEventArgs e)
        {
            boardlist.Items.Clear();
            studentboardlist.Clear();

            nowstate = 1;

            studentboardlist = service.StudentBoardRead();
            studentboardlist.Reverse();
            string[][] array = new string[studentboardlist.Count][];
            string[] boardinfo = new string[6];
            int maxcount = studentboardlist.Count;

            if (studentboardlist.Count >= MaxBoardCount)
            {
                for (int i = 0; i < MaxBoardCount; i++)
                {
                    boardinfo = studentboardlist[i].ToString().Split('\a');

                    if (service.ReadStudentBoardCheck(id, boardinfo[0], boardinfo[5]) == true)
                    {
                        array[i] = boardinfo;
                        boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][2], Title = array[i][0], Date = array[i][5] });
                    }
                }
            }
            else
            {
                for (int i = 0; i < studentboardlist.Count; i++)
                {
                    boardinfo = studentboardlist[i].ToString().Split('\a');

                    if (service.ReadStudentBoardCheck(id, boardinfo[0], boardinfo[5]) == true)
                    {
                        array[i] = boardinfo;
                        boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][2], Title = array[i][0], Date = array[i][5] });
                    }
                }
            }

            studentboardlist.Reverse(); //==> 게시판 정보를 빼올때 알맞은놈을 가져오기 위함
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (searchname.Text != "")
            {
                boardlist.Items.Clear();
                studentboardlist.Clear();

                nowstate = 2;

                studentboardlist = service.StudentBoardRead();
                studentboardlist.Reverse();
                string[][] array = new string[studentboardlist.Count][];
                string[] boardinfo = new string[6];
                int maxcount = studentboardlist.Count;

                if (studentboardlist.Count >= MaxBoardCount)
                {
                    for (int i = 0; i < MaxBoardCount; i++)
                    {
                        boardinfo = studentboardlist[i].ToString().Split('\a');

                        if (searchname.Text == boardinfo[2])
                        {
                            array[i] = boardinfo;
                            boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][2], Title = array[i][0], Date = array[i][5] });
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < studentboardlist.Count; i++)
                    {
                        boardinfo = studentboardlist[i].ToString().Split('\a');

                        if (searchname.Text == boardinfo[2])
                        {
                            array[i] = boardinfo;
                            boardlist.Items.Add(new BoardInfo { Number = maxcount - i, Name = array[i][2], Title = array[i][0], Date = array[i][5] });
                        }
                    }
                }

                if (array[0] == null)
                {
                    System.Windows.MessageBox.Show(searchname.Text + "님의 게시글이 없습니다.");
                    return;
                }

                studentboardlist.Reverse(); //==> 게시판 정보를 빼올때 알맞은놈을 가져오기 위함
            }
            else
                System.Windows.Forms.MessageBox.Show("사용자 이름을 입력해주세요.");
        }

        private void Write_Board(object sender, RoutedEventArgs e)
        {
            string mainlabel = "게시판 글쓰기";

            StudentBoardConnet writeboard = new StudentBoardConnet(service, name, id, mainlabel, 3);
            writeboard.ShowDialog();
        }

        private void GetList()
        {
            while (true)
            {
                if (System.Windows.Application.Current.Properties["StudentBoardAdd"] != null && (bool)System.Windows.Application.Current.Properties["StudentBoardAdd"] == true)
                {
                    System.Windows.Application.Current.Properties["StudentBoardAdd"] = false;
                    Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate { SetBoardList(); }));
                }
                else if (System.Windows.Application.Current.Properties["StudentBoardDelete"] != null && (bool)System.Windows.Application.Current.Properties["StudentBoardDelete"] == true)
                {
                    System.Windows.Application.Current.Properties["StudentBoardDelete"] = false;
                    Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate { SetBoardList(); }));
                }
            }
        }

        private void StudentBoard_Click(object sender, RoutedEventArgs e)
        {
            nowstate = 0;
            searchname.Text = "";
            SetBoardList();
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
            thread.Abort();
            this.Close();
        }
    }

    public class BoardInfo
    {
        int number;
        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        string title;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        string date;
        public string Date
        {
            get { return date; }
            set { date = value; }
        }

        string filename;
        public string FileName
        {
            get { return filename; }
            set { filename = value; }
        }
    }
}
