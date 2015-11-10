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
using System.Windows.Interactivity;
using System.Windows.Shapes;
using System.Diagnostics;
using Microsoft.VisualBasic;

namespace WPFSpark
{
    /// <summary>
    /// UserFile.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserFile : UserControl
    {
        public string path;

        public UserFile()
        {
            InitializeComponent();

            DataContext = this;

            SetProperties(); //set the window properties
        }

        public string IconName
        {
            get;
            set;
        }

        public ImageSource Icon
        {
            get;
            set;
        }


        private void SetProperties()
        {
            if (Application.Current.Properties["FileInfo"] != null)
            {
                FileInfo fileinfo = (FileInfo)Application.Current.Properties["FileInfo"];
                path = fileinfo.FullName;

                Icon = getIcon(fileinfo.FullName);
                IconName = System.IO.Path.GetFileName(fileinfo.FullName);

                IconName = StringCut(IconName);
            }
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

        private void Delete_File(object sender, RoutedEventArgs e)
        {
            Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(path,
                Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);

            Application.Current.Properties["Resetingicon"] = true;
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
