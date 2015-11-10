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
using System.Threading;
using System.IO;

namespace UserWallPaper
{
    /// <summary>
    /// ProgressBar.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ProgressBar : Window
    {
        public delegate void GetFileDelegate(string copydirectory, string userselectdirectory);

        private delegate void SetProgCallBack(int vv);
        private delegate void ExitCallBack();

        public event GetFileDelegate getfile;

        private Stream sSrc;
        private Stream sDest;
        private double sFileSize;
        private Thread t;
        private string copydirectory, userselectdirectory;

        public ProgressBar()
        {
            InitializeComponent();
        }

        public ProgressBar(Stream _src, Stream _dest, double _fileSize)
        {
            InitializeComponent();
            sSrc = _src;
            sDest = _dest;
            sFileSize = _fileSize;

            t = new Thread(new ThreadStart(Copy));
            t.Start();
        }

        public ProgressBar(Stream _src, Stream _dest, double _fileSize, string sourcedirectory, string realdirectroy)
        {
            InitializeComponent();
            sSrc = _src;
            sDest = _dest;
            sFileSize = _fileSize;
            copydirectory = sourcedirectory;
            userselectdirectory = realdirectroy;

            t = new Thread(new ThreadStart(Copy));
            t.Start();
        }

        void Copy()
        {
            //FileStream fsDest = new FileStream(sDest, FileMode.Create, FileAccess.Write);

            byte[] byteBuffer = new byte[4096];
            int iByteSize = 0;
            int iRunningByteTotal = 0;
            int seekPointer = 0;

            while ((iByteSize = sSrc.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
            {
                sDest.Write(byteBuffer, 0, iByteSize);
                iRunningByteTotal += iByteSize;

                seekPointer++;

                // 진행률
                double dIndex = (int)(iRunningByteTotal);

                double dTotal = (double)byteBuffer.Length;

                double dProgressPercentage = (dIndex * 100 / sFileSize);

                int iProgressPercentage = (int)(dProgressPercentage);

                SetProgBar(iProgressPercentage);
            }
            sSrc.Dispose();
            sDest.Dispose();

            this.Exit();
        }

        private void Exit()
        {
            ExitCallBack dele = new ExitCallBack(Close);
            Dispatcher.Invoke(dele);

            if (getfile != null)
            {
                getfile(copydirectory, userselectdirectory);
            }
        }

        public double Value
        {
            get { return this.ProgressBar1.Value; }
            set
            {
                this.ProgressBar1.Value = value;
                if (value >= 100)
                {
                    Thread.Sleep(500);
                    this.Close();
                    System.Windows.Application.Current.Properties["RefreshFileList"] = true;
                }
            }
        }

        private void SetProgBar(int valCurrent)
        {
            if (this.Dispatcher.Thread != Thread.CurrentThread)
            {
                SetProgCallBack dele = new SetProgCallBack(SetProgBar);
                Dispatcher.Invoke(dele, new object[] { valCurrent });
            }
            else
                this.ProgressBar1.Value = valCurrent;
        }

        private void Move(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}
