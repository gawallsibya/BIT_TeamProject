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

namespace UserWallPaper
{
    /// <summary>
    /// Setting.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Setting : Window
    {
        public Setting()
        {
            InitializeComponent();

            DataContext = this;

            sub.Focus();
        }

        public string Sub
        {
            get;
            set;
        }
        public string Book
        {
            get;
            set;
        }
        public string StartTime
        {
            get;
            set;
        }
        public string EndTime
        {
            get;
            set;
        }

        public void SetData(string day, ArrayList time)
        {
            this.day.Content = day;
            this.starttime.ItemsSource = time;
            this.endtime.ItemsSource = time;

            this.button.Content = "등록";
        }

        public void SetData(string day, ArrayList time,string s, int start,int end)
        {
            this.day.Content = day;
            this.starttime.ItemsSource = time;
            this.endtime.ItemsSource = time;

            this.sub.Text = s;
            this.button.Content = "수정";

            this.starttime.SelectedIndex = start;
            this.endtime.SelectedIndex = end;
        }

        private void book_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.Filter = "PDF파일(*.pdf)|*.pdf";

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ((TextBox)sender).Text = dlg.SafeFileName;
                Book = dlg.FileName;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Content.ToString())
            {
                case "등록":
                case "수정":
                    {
                        Sub = sub.Text;
                        StartTime = starttime.SelectedItem.ToString();
                        EndTime = endtime.SelectedItem.ToString();

                        if (Sub == "")
                        {
                            MessageBox.Show("과목명을 입력해주세요");
                            return;
                        }
                        else if (StartTime == EndTime)
                        {
                            MessageBox.Show("시작시간과 끝시간은 같을 수 없습니다!");
                            return;
                        }
                        else if (starttime.SelectedIndex >= endtime.SelectedIndex)
                        {
                            MessageBox.Show("시작시간이 끝시간을 초과하였습니다!");
                            return;
                        }
                        DialogResult = true;
                    } break;
                case "취소": this.Close();  break;
            }
        }

        
    }
}
