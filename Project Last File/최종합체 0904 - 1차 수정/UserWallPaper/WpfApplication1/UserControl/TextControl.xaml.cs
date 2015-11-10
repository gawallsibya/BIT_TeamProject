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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using System.Threading;
using System.Windows.Threading;
using UserWallPaper.Xml;
using Kent.Boogaart.Windows.Controls;
using System.Diagnostics;

namespace UserWallPaper
{
    /// <summary>
    /// UserControl1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TextControl : UserControl
    {
        MainWindow parent;
        public TextControl()
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
        public string ID
        {
            get;
            set;
        }

        public string Text
        {
            get { return text.Text; }
            set { text.Text = value; }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Popup.IsOpen = false;
        }

        public event TextChangedEventHandler Text_Changed;
        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (Text_Changed != null)
                Text_Changed(this, e);
        }

        private void myToggle_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)myToggle.IsChecked)
            {
                this.Popup.IsOpen = true;
                this.text.Text = NoteXml.Load(ID, parent.Page);

                IntPtr handle = PopupChildEnable.GetHwnd(this.Popup);
                PopupChildEnable.SetFocus(handle);
            }
            else
            {
                this.Popup.IsOpen = false;
            }

            if (parent.Service != null)
            {
                parent.Service.Memo(ID);
            }
        }

        private void Popup_Closed(object sender, EventArgs e)
        {
            myToggle.IsChecked = false;
        }

        private void text_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            text.FontSize = e.NewSize.Height / 7;
        }

        MenuItem link = null;

        private void text_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (text.SelectedText == "")
            {
                ((MenuItem)context.Items[0]).IsEnabled = false;
                ((MenuItem)context.Items[1]).IsEnabled = false;
            }
            else
            {
                ((MenuItem)context.Items[0]).IsEnabled = true;
                ((MenuItem)context.Items[1]).IsEnabled = true;

                try { new Uri(text.SelectedText); }
                catch { return; }

                link = new MenuItem();
                link.Header = text.SelectedText + "(으)로 이동...";
                link.Command = UserCommands.LinkCommand;
                link.Click += Link_Move;
                context.Items.Add(link);
            }
        }

        private void MenuItem_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void Cut(object sender, ExecutedRoutedEventArgs e)
        {
            Clipboard.SetText(text.SelectedText);

            text.SelectedText = "";
        }

        private void Copy(object sender, ExecutedRoutedEventArgs e)
        {
            Clipboard.SetText(text.SelectedText);
        }

        private void Paste(object sender, ExecutedRoutedEventArgs e)
        {
            if (Clipboard.GetText() != "")
                text.Text = text.Text.Insert(text.SelectionStart, Clipboard.GetText());
        }

        private void Link_Move(object sender, RoutedEventArgs e)
        {
            try
            {
                Uri u = new Uri(text.SelectedText);
                Process.Start(u.AbsoluteUri);
                e.Handled = true;
            }
            catch
            {

            }
        }

        private void text_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            if (link != null)
                context.Items.Remove(link);
        }
    }
    public static class UserCommands
    {
        public static RoutedUICommand CutCommand { get; private set; }
        public static RoutedUICommand CopyCommand { get; private set; }
        public static RoutedUICommand PasteCommand { get; private set; }
        public static RoutedUICommand LinkCommand { get; private set; }

        static UserCommands()
        {
            CutCommand = new RoutedUICommand("잘라내기", "Cut", typeof(UserCommands),
             new InputGestureCollection() { new KeyGesture(Key.X, ModifierKeys.Control, "Ctrl + X") });
            CopyCommand = new RoutedUICommand("복사", "Copy", typeof(UserCommands),
             new InputGestureCollection() { new KeyGesture(Key.C, ModifierKeys.Control, "Ctrl + C") });
            PasteCommand = new RoutedUICommand("붙여넣기", "Paste", typeof(UserCommands),
             new InputGestureCollection() { new KeyGesture(Key.V, ModifierKeys.Control, "Ctrl + V") });

            LinkCommand = new RoutedUICommand("링크", "Link", typeof(UserCommands),
             new InputGestureCollection() { new KeyGesture(Key.G, ModifierKeys.Control, "Ctrl + G") });
        }
    }
}
