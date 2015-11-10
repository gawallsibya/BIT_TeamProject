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
using System.Windows.Interactivity;
using System.Diagnostics;

namespace WPFSpark
{
    /// <summary>
    /// UserFolder.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserFolder : UserControl
    {
        public string path;
        public string usbpath;
        public string oldname;

        public UserFolder()
        {
            InitializeComponent();

            DataContext = this;
            usbpath = (string)Application.Current.Properties["USBDrivePath"];
            SetProperties(); //set the window properties
        }

        public string IconName
        {
            get;
            set;
        }

        private void SetProperties()
        {
            if (Application.Current.Properties["DirectoryInfo"] != null)
            {
                DirectoryInfo directoryinfo = (DirectoryInfo)Application.Current.Properties["DirectoryInfo"];
                path = directoryinfo.FullName;

                oldname = System.IO.Path.GetFileName(directoryinfo.FullName);

                IconName = System.IO.Path.GetFileName(directoryinfo.FullName);

                IconName = StringCut(IconName);
            }
        }

        private void Delete_Directory(object sender, RoutedEventArgs e)
        {
            Microsoft.VisualBasic.FileIO.FileSystem.DeleteDirectory(path,
                Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);

            Application.Current.Properties["Resetingicon"] = true;
        }

        private void Rename_Directory(object sender, RoutedEventArgs e)
        {
            RenameBox.Text = oldname;

            seticonname.Visibility = Visibility.Hidden;
            RenameBox.Visibility = Visibility.Visible;
            Keyboard.Focus(RenameBox);
            RenameBox.SelectAll();
        }

        private void RenameBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (oldname == RenameBox.Text)
                {
                    RenameBox.Visibility = Visibility.Hidden;
                    seticonname.Visibility = Visibility.Visible;
                }
                else if (RenameBox.Text != "")
                {
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

            IconName = StringCut(rename);
            seticonname.Text = rename;

            System.Windows.Application.Current.Properties["DirectoryRename"] = true;
        }

        public string StringCut(string value)
        {
            if (value == null) return "";

            string ReturnString = (string)value;

            if (ReturnString.Length > 15)
            {
                ReturnString = string.Format("{0}...", ReturnString.Substring(0, 15));
            }
            return ReturnString;
        }
    }
}
