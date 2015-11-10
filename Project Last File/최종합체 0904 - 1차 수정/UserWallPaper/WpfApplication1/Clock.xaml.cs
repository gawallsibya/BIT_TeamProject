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
    /// Clock.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Clock : Window
    {
        public Clock()
        {
            InitializeComponent();
            StartTimer();
        }

        public void StartTimer()
        {
            System.Windows.Threading.DispatcherTimer myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000); // 100 Milliseconds
            myDispatcherTimer.Tick += new EventHandler(Each_Tick);
            myDispatcherTimer.Start();
            today.Text = DateTime.Now.ToString("yyyy년 MM월 dd일");
        }

        // Fires every 1000 miliseconds while the DispatcherTimer is active.
        public void Each_Tick(object o, EventArgs sender)
        {
            hourText.Text = twoDigit(DateTime.Now.Hour.ToString());
            hourText1.Text = twoDigit(DateTime.Now.Hour.ToString());
            minuteText.Text = twoDigit(DateTime.Now.Minute.ToString());
            minuteText1.Text = twoDigit(DateTime.Now.Minute.ToString());
            secondText.Text = twoDigit(DateTime.Now.Second.ToString());
            secondText1.Text = twoDigit(DateTime.Now.Second.ToString());
        }

        string twoDigit(string input)
        {
            if (input.Length < 2)
                return "0" + input;
            else
                return input;
        }

        private void Move(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void ClockClose(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}


