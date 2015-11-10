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
using System.Windows.Shapes;

namespace UserWallPaper
{
    /// <summary>
    /// StudentBoardList.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StudentBoardList : UserControl
    {
        Service service;
        string[] boardinfo;
        string id, name, mainlabel, filename;

        public StudentBoardList(Service s, string str, string n, string i)
        {
            InitializeComponent();
            service = s;
            name = n;
            id = i;

            boardinfo = new string[6];
            boardinfo = str.Split('\a');
            Title.Content = boardinfo[0];
            UserName.Content = boardinfo[2];
            SetLastDate.Content = boardinfo[5];
            mainlabel = boardinfo[0];
            filename = boardinfo[3];

            if (id == boardinfo[1])
            {
                DeleteBoard.IsEnabled = true;
            }
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //StudentBoardConnet connectsubmission = new StudentBoardConnet(service, boardinfo, mainlabel, 3);
            //connectsubmission.ShowDialog();
            //service.ReadStudentBoardWrite(id, Title.Content.ToString(), SetLastDate.Content.ToString()); // 읽은파일 기록 표시
            //this.Opacity = 0.6;
        }

        private void StudentBoard_Click(object sender, RoutedEventArgs e)
        {
            //StudentBoardConnet connectsubmission = new StudentBoardConnet(service, boardinfo, mainlabel, 3);
            //connectsubmission.ShowDialog();
        }

        private void StudentBoardDelete_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("게시물을 삭제하시겠습니까?", "게시물 삭제", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                service.DeleteBoard(id, mainlabel);
                if (filename != "")
                {
                    service.BoardDeleteFile(filename, 3);
                }
                System.Windows.Application.Current.Properties["StudentBoardDelete"] = true;
            }
            else if (dialogResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
        }
    }
}
