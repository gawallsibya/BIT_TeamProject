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
    /// USB_Rejoin.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class USB_Rejoin : Window
    {
        Service service;
        public USB_Rejoin(Service s, string sl)
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
                        if (service.Rejoin(serial.Text, id.Text, pw.Text))
                        {
                            MessageBox.Show("재등록완료!");
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
    }
}
