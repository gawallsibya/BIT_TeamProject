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
    /// Calendar.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Calendar : Window
    {
        public Calendar()
        {
            InitializeComponent();
            ShowCalendar();
        }

        private void ShowCalendar()
        {
            week.Text = DateTime.Now.ToString("dddd");
            date.Text = DateTime.Now.ToString("dd");
            year.Text = DateTime.Now.ToString("yyyy년 MM월");
        }

        private void Move(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void CalendarClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
