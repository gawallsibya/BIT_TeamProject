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

namespace UserWallPaper
{
    /// <summary>
    /// UserControl3.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BrushControl : UserControl
    {
        public BrushControl()
        {
            InitializeComponent();
        }

        public ImageSource Source
        {
            get { return image.Source; }
            set { image.Source = value; }
        }

        public event RoutedEventHandler ButtonClick;
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ButtonClick(this, e);
        }

        public event RoutedEventHandler PopupChecked;
        private void myToggle_Checked(object sender, RoutedEventArgs e)
        {
            PopupChecked(Popup, e);
        }

        private void Popup_Closed(object sender, EventArgs e)
        {
            myToggle.IsChecked = false;
        }
    }
}
