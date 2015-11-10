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
    /// USB_Join.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class USB_Join : Window
    {
        USB_Member member;
        USB_Rejoin rejoin;

        Service service;

        string userinfo;

        public string Serial
        {
            get;
            set;
        }

        public USB_Join(Service s, string serial)
        {
            InitializeComponent();

            service = s;
            Serial = serial;
            userinfo = service.FindUserInfo(serial);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            switch (((Button)sender).Content.ToString())
            {
                case "USB 등록":
                    {
                        this.Hide();
                        member = new USB_Member(service, Serial);
                        if (member.ShowDialog() == true)
                        {
                            Application.Current.Properties["Members"] = service.Join(Serial);
                            Application.Current.Properties["User"] = Serial;

                            string userinfo = service.FindUserInfo(Serial);
                            WindowsStyle window = new WindowsStyle(service, userinfo);
                            window.Show();

                            service.First_Files();
                            service.First_FileRoomFiles();
                        }
                        else
                            this.Show();
                    } break;
                case "USB 재등록":
                    {
                        this.Hide();
                        rejoin = new USB_Rejoin(service, Serial);
                        if (rejoin.ShowDialog() == true)
                        {
                            Application.Current.Properties["Members"] = service.Join(Serial);
                            Application.Current.Properties["User"] = Serial;

                            userinfo = service.FindUserInfo(Serial);

                            WindowsStyle window = new WindowsStyle(service, userinfo);
                            window.Show();

                            service.First_Files();
                            service.First_FileRoomFiles();
                        }
                        else
                            this.Show();
                    } break;
            }
        }
    }
}
