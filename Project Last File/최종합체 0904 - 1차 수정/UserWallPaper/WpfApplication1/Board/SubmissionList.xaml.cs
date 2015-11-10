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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UserWallPaper
{
    /// <summary>
    /// SubmissionList.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SubmissionList : UserControl
    {
        Service service;
        string[] submissioninfo;
        string name, type, id, title, filename;

        public SubmissionList(Service s, string str, string n, string t, string i)
        {
            InitializeComponent();

            service = s;
            name = n;
            type = t;
            id = i;

            submissioninfo = new string[8];
            submissioninfo = str.Split('\a');
            SugesstTitle.Content = submissioninfo[2];
            title = submissioninfo[2];
            SugesstDate.Content = submissioninfo[7];
            SugesstSubject.Content = submissioninfo[1];
            SugesstTeacherName.Content = submissioninfo[0];
            SugesstLastDate.Content = submissioninfo[3];
            filename = submissioninfo[4];

            if (type == "Teacher")
            {
                DeleteSubmission.IsEnabled = true;
            }
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string mainlabel = "과제 확인";

            System.Windows.Application.Current.Properties["SubmissionSend"] = true;
            SubmissionConnect connectsubmission = new SubmissionConnect(service, name, mainlabel, submissioninfo, 4, id);
            connectsubmission.ShowDialog();
            service.ReadSubmissionBoardWrite(id, SugesstTitle.Content.ToString(), SugesstDate.Content.ToString()); // 읽은파일 기록 표시
            this.Opacity = 0.6;
        }

        private void Submission_Click(object sender, RoutedEventArgs e)
        {
            string mainlabel = "과제 확인";

            System.Windows.Application.Current.Properties["SubmissionSend"] = true;
            SubmissionConnect connectsubmission = new SubmissionConnect(service, name, mainlabel, submissioninfo, 4, id);
            connectsubmission.ShowDialog();
        }

        private void SubmissionDelete_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.DialogResult dialogResult = System.Windows.Forms.MessageBox.Show("게시물을 삭제하시겠습니까?", "게시물 삭제", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (dialogResult == System.Windows.Forms.DialogResult.Yes)
            {
                service.DeleteSubmission(id, title);
                if (filename != "")
                {
                    service.BoardDeleteFile(filename, 4);
                }
                System.Windows.Application.Current.Properties["SubmissionSugesstDelete"] = true;
            }
            else if (dialogResult == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
        }
    }
}
