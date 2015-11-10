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
using Microsoft.VisualBasic;

namespace UserWallPaper
{
    /// <summary>
    /// UserFolder.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NoUSBWindowFolder : UserControl
    {
        string path, usbpath;

        public NoUSBWindowFolder()
        {
            InitializeComponent();
            usbpath = (string)Application.Current.Properties["USBDrivePath"];
            SetProperties(); //set the window properties
        }

        private void SetProperties()
        {
            if (Application.Current.Properties["DirectoryInfo"] != null)
            {
                DirectoryInfo directoryinfo = (DirectoryInfo)Application.Current.Properties["DirectoryInfo"];
                path = directoryinfo.FullName;

                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri(@"\ImageFolder\UserFolder.png", UriKind.Relative);
                logo.EndInit(); // Getting exception here

                seticon.Source = logo;
                seticonname.Text = System.IO.Path.GetFileName(directoryinfo.FullName);
            }
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Process ps = new Process();
            ps.StartInfo.FileName = path;
            ps.Start();
        }

        private void Delete_Directory(object sender, RoutedEventArgs e)
        {
            Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(path,
                Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
        }

        private void Rename_Directory(object sender, RoutedEventArgs e)
        {
            RenameBox.Text = seticonname.Text;

            seticonname.Visibility = Visibility.Hidden;
            RenameBox.Visibility = Visibility.Visible;
            Keyboard.Focus(RenameBox);
            RenameBox.SelectAll();
        }

        private void RenameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (seticonname.Text == RenameBox.Text)
                {
                    seticonname.Text = RenameBox.Text;
                    RenameBox.Visibility = Visibility.Hidden;
                    seticonname.Visibility = Visibility.Visible;
                }
                else if (RenameBox.Text != "")
                {
                    seticonname.Text = RenameBox.Text;
                    RenameBox.Visibility = Visibility.Hidden;
                    seticonname.Visibility = Visibility.Visible;

                    ReName(RenameBox.Text);
                }
                else
                {
                    MessageBox.Show("폴더 명을 입력해주세요.");
                }
            }
        }

        private void ReName(string rename)
        {
            string renamepath = usbpath + @"SchoolInteligentUSB\WindowFolder\" + rename;

            Directory.Move(path, renamepath);

            path = renamepath;
        }
    }
}
