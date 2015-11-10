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
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Threading;
using System.Threading;

namespace UserWallPaper
{
    /// <summary>
    /// DirectoyProgress.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DirectoyProgress : Window
    {
        public delegate void deletetempfile();
        public event deletetempfile deletefile;

        private delegate void UpdateProgressDelegate(int nPercent);
        private CopyFileExWrapper cfewEngine = new CopyFileExWrapper();
        private Thread t;
        private string SourcePath, DestinationPath;
        private int flag;

        public DirectoyProgress()
        {
            InitializeComponent();
        }

        public DirectoyProgress(string s, string d, int settype)
        {
            InitializeComponent();

            SourcePath = s;
            DestinationPath = d;
            flag = settype;

            t = new Thread(new ThreadStart(CopyAll));
            t.Start();
        }

        public void CopyAll()
        {
            Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(delegate
            {
                Directory.CreateDirectory(DestinationPath);

                List<string> lstSource = new List<string>();

                //Now Create all of the directories
                foreach (string dirPath in Directory.GetDirectories(SourcePath, "*.*", SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

                //Copy all the files
                foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
                try
                {
                    lstSource.Add(newPath);
                    //File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath));
                }
                catch { }

                if (btnCopy.Content.ToString() == "Copy")
                {
                    if (flag == 0)
                    {
                        string pullpath = DestinationPath.Remove(DestinationPath.LastIndexOf('\\') + 1);

                        btnCopy.Content = "Cancel";
                        cfewEngine.EventCopyHandler += new CopyFileExWrapper.CopyEventHandler(cfewEngine_EventCopyHandler);
                        cfewEngine.UpdateTextBlock += new CopyFileExWrapper.UpdateTextBlockDelegate(cfewEngine_UpdateTextBlock);
                        cfewEngine.CopyFiles(lstSource, SourcePath, pullpath);
                        btnCopy.Content = "Copy";
                    }
                    else
                    {
                        btnCopy.Content = "Cancel";
                        cfewEngine.EventCopyHandler += new CopyFileExWrapper.CopyEventHandler(cfewEngine_EventCopyHandler);
                        cfewEngine.UpdateTextBlock += new CopyFileExWrapper.UpdateTextBlockDelegate(cfewEngine_UpdateTextBlock);
                        cfewEngine.CopyFiles(lstSource, SourcePath, DestinationPath);
                        btnCopy.Content = "Copy";
                    }
                }
                else
                {
                    btnCopy.Content = "Copy";
                    cfewEngine.Cancel = true;
                }
            }));
        }

        private void cfewEngine_UpdateTextBlock(string strMessage)
        {
            txtFile.Text = strMessage;
            txtFile.Refresh();
            if (txtFile.Text == "Copying successfully")
            {
                t.Abort();
                this.Close();

                if (deletefile != null)
                {
                    deletefile();
                }
            }
        }

        private void UpdateProgress(int nPercent)
        {
            pbBar.Value = nPercent;
            pbBar.Refresh();
            btnCopy.RefreshInput();
        }

        private void cfewEngine_EventCopyHandler(CopyFileExWrapper sender, CopyEventArgs e)
        {
            this.Dispatcher.Invoke(new UpdateProgressDelegate(UpdateProgress), new object[] { Convert.ToInt32(e.Percent) });
        }
    }

    public static class ExtensionMethods
    {
        private static Action EmptyDelegate = delegate() { };

        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }

        public static void RefreshInput(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Input, EmptyDelegate);
        }
    }
}

