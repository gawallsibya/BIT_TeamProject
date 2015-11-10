using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using UserWallPaper.Xml;
using System.IO;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace UserWallPaper
{
    /// <summary>
    /// BookmarkControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BookmarkControl : UserControl
    {
        MainWindow parent;
        public BookmarkControl()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            var v = VisualTreeHelper.GetParent(this);
            while (true)
            {
                v = VisualTreeHelper.GetParent(v);
                if (v is MainWindow)
                    break;
            }
            parent = (MainWindow)v;
        }

        public ImageSource Source
        {
            get { return image.Source; }
            set { image.Source = value; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Content.ToString() == "추가")
            {
                string[] output = new string[4];

                output[0] = parent.FileName;
                output[1] = System.IO.Path.GetFileName(parent.FileName);
                output[2] = parent.Page;
                output[3] = memo.Text;

                BookmarkXml.WriteXml(output);
                Bookmark_ReadList();
                memo.Text = "";
            }
            
            addtogle.IsChecked = false;
        }

        public event RoutedEventHandler PopupChecked;
        private void myToggle_Checked(object sender, RoutedEventArgs e)
        {
            PopupChecked(Popup, e);
            if(Popup.IsOpen)
                Bookmark_ReadList();
        }

        private void Popup_Closed(object sender, EventArgs e)
        {
            myToggle.IsChecked = false;
            if((bool)addtogle.IsChecked)
                addtogle.IsChecked = false;
        }

        private void listview_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string path = (listview.SelectedItem as BookInfo).Path;
                string page = (listview.SelectedItem as BookInfo).Page;

                parent.Page = page;

                parent.FileOpen(path);

                this.Popup.IsOpen = false;
            }
            catch
            {
                return;
            }
        }

        private void BookMarkDelete_Click(object sender, RoutedEventArgs e)
        {
            string path = (listview.SelectedItem as BookInfo).Path;
            string page = (listview.SelectedItem as BookInfo).Page;
            BookmarkXml.DeleteXml(path, page);
        }

        private void BookMarkDeleteAll_Click(object sender, RoutedEventArgs e)
        {
            string path = (listview.SelectedItem as BookInfo).Path;
            string page = (listview.SelectedItem as BookInfo).Page;
            BookmarkXml.DeleteAllXml(path, page);
        }

        private void Bookmark_ReadList()
        {
            listview.Items.Clear();

            ArrayList bookmarks = BookmarkXml.ReadXml();

            for (int i = 0; i < bookmarks.Count; i++)
            {
                string str = bookmarks[i].ToString();
                string[] bookinfo = new string[4];
                bookinfo = str.Split('\a');
                listview.Items.Add(new BookInfo { Path = bookinfo[0], Name = bookinfo[1], Page = bookinfo[2], Memo = bookinfo[3] });
            }   
        }

        private void addToggle_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)addtogle.IsChecked)
            {
                addpopup.IsOpen = true;
                IntPtr handle = PopupChildEnable.GetHwnd(addpopup);
                PopupChildEnable.SetFocus(handle);
            }
            else addpopup.IsOpen = false;
        }       
    }

    public class BookInfo
    {
        string path;
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        string page;
        public string Page
        {
            get { return page; }
            set { page = value; }
        }

        string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        string memo;
        public string Memo
        {
            get { return memo; }
            set { memo = value; }
        }
    }
}
