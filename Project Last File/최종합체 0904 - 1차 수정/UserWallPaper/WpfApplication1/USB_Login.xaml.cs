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
using System.IO;

namespace UserWallPaper
{
    /// <summary>
    /// USB_Login.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class USB_Login : Window
    {
        Service service;

        public USB_Login(Service s)
        {
            InitializeComponent();

            service = s;
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (service.Login(ID.Text, PASSWORD.Password))
            {
                string str = service.FindtoID(ID.Text);
                string[] userinfo = new string[3];
                userinfo = str.Split('\a');

                string MyDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\SIU\\" + ID.Text + "\\" + DateTime.Today.ToShortDateString(); //download폴더 수정흔적

                DirectoryInfo di = new DirectoryInfo(MyDocumentsPath); //download폴더 수정흔적
                if (di.Exists == false)
                {
                    di.Create();
                }

                string serial = userinfo[1];

                service.Join(serial);
                NoUSBWindowsStyle windowsstyle = new NoUSBWindowsStyle(service, str, ID.Text, MyDocumentsPath); //download폴더 수정흔적

                windowsstyle.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("일치하는 ID, PW가 없습니다");
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            this.Close();
        }
    }
}
