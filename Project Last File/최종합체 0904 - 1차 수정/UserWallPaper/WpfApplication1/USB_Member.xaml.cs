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
using System.Windows.Shapes;

namespace UserWallPaper
{
    /// <summary>
    /// USB_Member.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class USB_Member : Window
    {
        Service service;

        public USB_Member(Service s, string sl)
        {
            InitializeComponent();

            service = s;
            serial.Text = sl;
        }

        private void Border_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Content.ToString())
            {
                case "확인":
                    {
                        if (service.Save(serial.Text, id.Text, pw.Text, name.Text, sort.SelectionBoxItem.ToString(), number_text.Text))
                        {
                            MessageBox.Show("등록완료!");
                            DialogResult = true;
                        }
                        else
                        {
                            MessageBox.Show("ID 혹은 학번(사번)이 같습니다!");

                        }
                    } break;
                case "취소": this.Close(); break;
            }
        }

        private void ComboBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {
            switch (((ComboBoxItem)sender).Content.ToString())
            {
                case "Teacher": number.Content = "사번"; break;
                case "Student": number.Content = "학번"; break;
            }
        }

        private void pw_GotFocus(object sender, RoutedEventArgs e)
        {

        }
    }
}
