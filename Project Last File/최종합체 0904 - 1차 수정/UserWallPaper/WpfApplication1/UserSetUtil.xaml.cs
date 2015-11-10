using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;

namespace UserWallPaper
{
    /// <summary>
    /// UserSetUtil.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserSetUtil : UserControl
    {
        private string path, name;

        public UserSetUtil(string[] link)
        {
            InitializeComponent();

            path = link[1];
            name = link[0];

            SetProperties();
        }

        private void SetProperties()
        {
            userfile.Source = getIcon(path);
            FileName.Text = name;
        }

        public System.Windows.Media.ImageSource getIcon(string filename)       //File 경로에서 Image 추출
        {
            System.Windows.Media.ImageSource icon;

            using (System.Drawing.Icon sysicon = System.Drawing.Icon.ExtractAssociatedIcon(filename))
            {
                icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                sysicon.Handle,
                System.Windows.Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
            return icon;
        }

        private void CreateLinkFile(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog() { DereferenceLinks = false };

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                GetlnkPath getlnkpath = new GetlnkPath();
                string path = getlnkpath.GetTargetPath(dlg.FileName);
                string[] name = new string[2];
                name = dlg.SafeFileName.Split('.');

                if (path == null)
                {
                    path = dlg.FileName;
                }

                UserLinkListXML userlinklistxml = new UserLinkListXML();
                if (userlinklistxml.SaveUserLink(name[0], path)) { }
                else
                    System.Windows.Forms.MessageBox.Show("동일한 바로가기가 존재합니다");

                Application.Current.Properties["AddUserlnk"] = true;
            }
        }

        private void Delete_Userlnk(object sender, RoutedEventArgs e)
        {
            UserLinkListXML userlinklistxml = new UserLinkListXML();
            userlinklistxml.DeleteUserLink(name, path);

            Application.Current.Properties["DeleteUserlnk"] = true;
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Process ps = new Process();
            ps.StartInfo.FileName = path;
            ps.Start();
        }
    }
}
