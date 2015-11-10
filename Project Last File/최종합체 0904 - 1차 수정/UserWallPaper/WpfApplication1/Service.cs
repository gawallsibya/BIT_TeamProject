using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserWallPaper.ServiceReference1;
using UserWallPaper.Xml;
using System.ServiceModel;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace UserWallPaper
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Service : ISIUCallback, IDisposable
    {
        private SIUClient client;

        static private bool mode = true;

        private MainWindow parent;
        private InkCanvas pdf;
        private TextControl text;
        private Point startpoint;

        private string type;
        private string bookname;

        public Service()
        {
            client = new SIUClient(new InstanceContext(this));
        }

        public MainWindow Parent
        {
            get { return this.parent; }
            set { this.parent = value; }
        }

        public InkCanvas Pdf
        {
            get { return this.pdf; }
            set { this.pdf = value; }
        }

        public WindowsStyle Ws
        {
            get;
            set;
        }
        public string Type
        {
            get { return this.type; }
        }

        public bool Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        public void Dispose()
        {
            try
            {
                client.Leave();
            }
            catch { }
            finally
            {
                AbortClient();
            }
        }

        private void AbortClient()
        {
            if (client != null)
            {
                client.Abort();
                client.Close();
                client = null;
            }
        }

        public string[] Join(string serial)
        {
            type = client.MyType(serial);

            return client.Join(serial);

            //if (type == "Teacher")
            //    mode = false;
        }

        public void Bookname(string bookname)
        {
            this.bookname = bookname;
            client.MyBook(bookname);
        }

        public string MyName(string serial)
        {
            return client.MyName(serial);
        }

        public bool Save(string serial, string id, string pw, string name, string sort, string number)
        {

            return client.Save(serial, id, pw, name, sort, number);
        }

        public bool Find(string serial)
        {
            return client.Find(serial);
        }

        public string FindName(string serial)
        {
            return client.FindName(serial);
        }

        public string FindTpye(string serial)
        {
            return client.FindType(serial);
        }

        public string FindUserInfo(string serial)
        {
            return client.FindUserInfo(serial);
        }

        public bool Rejoin(string serial, string id, string pw)
        {
            return client.Rejoin(serial, id, pw);
        }

        public bool Login(string id, string pw)
        {
            return client.Login(id, pw);
        }

        public string FindtoID(string id)
        {
            return client.FindtoID(id);
        }

        public void DrawPrepare(string type, Color color, Point pt, int fontsize)
        {
            if (this.type != "Student")
                client.DrawPrepare(type, color, pt, fontsize);
        }

        public void Draw(string type, Color color, Point pt, int fontsize)
        {
            if (this.type != "Student")
                client.Draw(type, color, pt, fontsize);
        }

        public void DrawEnd(string type, string id)
        {
            if (this.type != "Student")
                client.DrawEnd(type, id);
        }

        public void MovePage(string page)
        {
            if (this.type != "Student")
                client.MovePage(page);
        }

        public void PutFile(string[] files)
        {
            foreach (string file in files)
            {
                string virtualPath = System.IO.Path.GetFileName(file);

                using (Stream uploadStream = new FileStream(file, FileMode.Open))
                {
                    client.PutFile(0, virtualPath, 0, uploadStream);
                }
            }
        }

        public void UploadFile(string file, long length, long byteStart, Stream FileByteStream, int flag)
        {
            TransferClient client = new TransferClient();

            client.UploadFile(file, length, byteStart, flag, FileByteStream);

            client.Close();
        }

        public void DeleteFile(string virtualPath, int flag)
        {
            client.DeleteFile(virtualPath, flag);
        }

        public StorageFileInfo[] List(string virtualPath, int flag)
        {
            return client.List(virtualPath, flag);
        }

        public void Memo(string id)
        {
            client.Memo(id);
        }

        public void MemoText(string id, string text)
        {
            client.MemoText(id, text);
        }

        public void Erase(string type, string page, double left, double top, Point[] pts)
        {
            client.Erase(type, page, left, top, pts);
        }

        public void SendScroll(double offset)
        {
            client.SendScroll(offset);
        }

        public bool writesugesstsubmission(string[] str)
        {
            return client.WriteSugesstSubmissionList(str);
        }

        public ArrayList readsugesstsubmissionList()
        {
            ArrayList sugesstlist = new ArrayList(client.ReadSugesstSubmissionList());
            return sugesstlist;
        }

        public bool DeleteSubmission(string id, string title)
        {
            return client.DeleteSubmission(id, title);
        }

        public bool writesubmissionlist(string teacher, string[] str)
        {
            return client.WriteSubmissionList(teacher, str);
        }

        public ArrayList readsubmissionlist(string teacher)
        {
            ArrayList sugesstlist = new ArrayList(client.ReadSubmissionList(teacher));
            return sugesstlist;
        }

        public bool DeleteSumitSubmission(string id, string title)
        {
            return client.DeleteSumitSubmission(id, title);
        }

        public bool WriteStudentSumitList(string studentid, string[] str)
        {
            return client.WritePersonalStudentSubmissionXml(studentid, str);
        }

        public ArrayList ReadStudentSumitList(string studentid)
        {
            ArrayList sugesstlist = new ArrayList(client.ReadPersonalStudentSubmissionXml(studentid));
            return sugesstlist;
        }

        public bool DeleteStudentSumit(string id, string title)
        {
            return client.DeletePersonalStudentInfo(id, title);
        }

        public bool ReadSubmissionBoardWrite(string id, string title, string date)
        {
            return client.ReadSubmissionBoardWrite(id, title, date);
        }

        public bool ReadSubmissionBoardCheck(string id, string title, string date)
        {
            return client.ReadSubmissionBoardCheck(id, title, date);
        }

        public bool StudentBoardWrite(string[] SubmissionInfo)
        {
            return client.StudentBoardWrite(SubmissionInfo);
        }

        public ArrayList StudentBoardRead()
        {
            ArrayList boardlist = new ArrayList(client.StudentBoardRead());
            return boardlist;
        }

        public bool DeleteStudentBoard(string id, string title)
        {
            return client.DeleteStudentBoard(id, title);
        }

        public bool ReadStudentBoardWrite(string id, string title, string date)
        {
            return client.ReadStudentBoardWrite(id, title, date);
        }

        public bool ReadStudentBoardCheck(string id, string title, string date)
        {
            return client.ReadStudentBoardCheck(id, title, date);
        }

        public bool BoardDeleteFile(string filename, int flag)
        {
            return client.BoardDeleteFile(filename, flag);
        }

        public System.IO.Stream GetFile(string virtualPath, int flag)
        {
            TransferClient client2 = new TransferClient();

            return client2.GetFile(virtualPath, flag);
        }

        public void Uploaded()
        {
            WindowsStyle wnd = (WindowsStyle)Application.Current.MainWindow;

            wnd.FileRoomTextChange();
        }

        public bool NoUSBWrite(string[] str)
        {
            return client.NoUSBWrite(str);
        }

        public ArrayList NoUSBRead()
        {
            ArrayList NoUSBlist = new ArrayList(client.NoUSBRead());
            return NoUSBlist;
        }

        public void SendRequest(string to, string file)
        {
            this.client.SendRequest(to, file);
        }

        public long GetTotalSize(int flag)
        {
            TransferClient client2 = new TransferClient();
            return client2.GetTotalSize(flag);
        }

        public long GetTotalFileSize(int flag, string[] filepath)
        {
            TransferClient client2 = new TransferClient();
            return client2.GetTotalFileSize(flag, filepath);
        }

        public long DownloadFile(int idx, ref string filename, ref long startlength, out Stream inputstream, int flag)
        {
            TransferClient client2 = new TransferClient();

            return client2.DownloadFile(idx, flag, ref filename, ref startlength, out inputstream);
        }

        public long DownloadFiles(string[] filepath, int idx, int flag, ref string filename, ref long startlength, out Stream inputstream)
        {
            TransferClient client2 = new TransferClient();

            return client2.DownloadFiles(filepath, idx, flag, ref filename, ref startlength, out inputstream);
        }

        public void First_Files()
        {
            MultiProgressBar multiprogress = new MultiProgressBar(this, 1);
            multiprogress.ShowDialog();

            string usbpath = (string)System.Windows.Application.Current.Properties["USBDrivePath"];
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + "Temp";
            string realpath = usbpath + @"SchoolInteligentUSB\BookRoom";

            DirectoyProgress directorycopy = new DirectoyProgress(path, realpath, 1);
            directorycopy.ShowDialog();

            //----
            string[] files = client.Files(1);
            foreach (string str in files)
            {
                string[] f = new string[2];
                f = str.Split('\a');
                Xml.DownloadedXml.DownloadedFileWrite(f[0].ToString(), f[1].ToString());
            }
            //---
            Directory.Delete(path, true);
        }

        public void First_FileRoomFiles()
        {
            MultiProgressBar multiprogress = new MultiProgressBar(this, 2);
            multiprogress.ShowDialog();

            string usbpath = (string)System.Windows.Application.Current.Properties["USBDrivePath"];
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + "Temp";
            string realpath = usbpath + @"SchoolInteligentUSB\FileRoom";

            DirectoyProgress directorycopy = new DirectoyProgress(path, realpath, 1);
            directorycopy.ShowDialog();

            //---           
            string[] files = client.Files(2);
            foreach (string str in files)
            {
                string[] f = new string[2];
                f = str.Split('\a');
                Xml.DownloadedXml.DownloadedFileWrite(f[0].ToString(), f[1].ToString());
            }
            //---
            Directory.Delete(path, true);
        }

        public ArrayList GetUploadedXmlInfo(int flag)
        {
            ArrayList arrlist = new ArrayList(client.GetUploadedXmlInfo(flag));
            return arrlist;
        }

        #region 콜백
        public void Receive(string sendername, string msg)
        {
        }

        public void UserEnter(string serial, string name)
        {
            MenuItem item = new MenuItem();
            item.Click += Ws.item_Click;
            item.Header = name;
            item.Name = "s_" + serial;
            ((MenuItem)Ws.WallPaper1.ContextMenu.Items[8]).Items.Add(item);
        }

        public void UserLeave(string name)
        {
            MenuItem dleleteitem = null;
            foreach (MenuItem item in ((MenuItem)Ws.WallPaper1.ContextMenu.Items[8]).Items)
            {
                if (item.Name.ToString() == "s_" + name)
                    dleleteitem = item;
            }
            ((MenuItem)Ws.WallPaper1.ContextMenu.Items[8]).Items.Remove(dleleteitem);
        }
        int fontsize;
        static bool isdataexist = false;

        int idx;

        public void PrepareReceive(string type1, Color color, Point pt, int fontsize, string bookname)
        {
            idx = -1;
            if (this.bookname != bookname)
                return;
            if (this.type != "Teacher" && mode)
            {
                switch (type1)
                {
                    case "Pen":
                    case "Brush":
                        {
                            if (isdataexist)
                            {
                                pdf.Strokes[idx].StylusPoints.Add(new StylusPoint(pt.X, pt.Y));
                                isdataexist = false;
                                return;
                            }

                            StylusPointCollection spc = new StylusPointCollection();
                            spc.Add(new StylusPoint(pt.X, pt.Y));
                            Stroke stroke = new Stroke(spc);
                            stroke.DrawingAttributes.Color = color;
                            if (type1 == "Pen")
                            {
                                stroke.DrawingAttributes.Width = fontsize;
                                stroke.DrawingAttributes.Height = fontsize;
                            }
                            else
                            {
                                stroke.DrawingAttributes.StylusTip = StylusTip.Rectangle;
                                stroke.DrawingAttributes.IsHighlighter = true;
                                stroke.DrawingAttributes.Width = fontsize / 2;
                                stroke.DrawingAttributes.Height = fontsize;
                            }
                            pdf.Strokes.Add(stroke);

                            idx = pdf.Strokes.IndexOf(stroke);

                            this.fontsize = fontsize;
                        } break;
                    case "Rectangle":
                        {
                            startpoint = pt;
                            text = new TextControl
                            {
                                BorderBrush = Brushes.LightBlue,
                                BorderThickness = new Thickness(2),
                                Background = Brushes.LightGreen,
                                Opacity = 0.5
                            };
                            text.Text_Changed += parent.Text_Changed;

                            InkCanvas.SetLeft(text, startpoint.X);
                            InkCanvas.SetTop(text, startpoint.Y);
                            pdf.Children.Add(text);

                        } break;
                }
            }
        }

        public void DrawReceive(string type1, Color color, Point pt, int fontsize, string bookname)
        {
            if (this.bookname != bookname)
                return;
            if (this.type != "Teacher" && mode)
            {
                try
                {
                    switch (type1)
                    {
                        case "Pen":
                        case "Brush":
                            {
                                lock (this)
                                {
                                    pdf.Strokes[idx].StylusPoints.Add(new StylusPoint(pt.X, pt.Y));
                                    DrawXml.Xml_Save(type1, idx, pt, parent.Page, pdf.Strokes[idx].DrawingAttributes.Color, fontsize);
                                }
                            } break;
                        case "Rectangle":
                            {
                                var pos = pt;

                                var x = Math.Min(pos.X, startpoint.X);
                                var y = Math.Min(pos.Y, startpoint.Y);

                                var w = Math.Max(pos.X, startpoint.X) - x;
                                var h = Math.Max(pos.Y, startpoint.Y) - y;

                                text.Width = w;
                                text.Height = h;

                                InkCanvas.SetLeft(text, x);
                                InkCanvas.SetTop(text, y);
                            } break;
                    }
                }
                catch
                {
                    startpoint = pt;

                    StylusPointCollection spc = new StylusPointCollection();
                    spc.Add(new StylusPoint(startpoint.X, startpoint.Y));
                    Stroke stroke = new Stroke(spc);
                    stroke.DrawingAttributes.Color = color;
                    if (type1 == "Pen")
                    {
                        stroke.DrawingAttributes.Width = fontsize;
                        stroke.DrawingAttributes.Height = fontsize;
                    }
                    else
                    {
                        stroke.DrawingAttributes.StylusTip = StylusTip.Rectangle;
                        stroke.DrawingAttributes.IsHighlighter = true;
                        stroke.DrawingAttributes.Width = fontsize / 2;
                        stroke.DrawingAttributes.Height = fontsize;
                    }
                    pdf.Strokes.Add(stroke);

                    idx = pdf.Strokes.IndexOf(stroke);

                    isdataexist = true;
                }
            }
        }

        public void DrawEndReceive(string type, string id, string bookname)
        {
            if (this.bookname != bookname)
                return;
            if (this.type != "Teacher" && mode)
            {
                if (type == "Rectangle")
                {
                    text.ID = id;
                    DrawXml.Xml_Save("Rectangle", text.ID, InkCanvas.GetLeft(text), InkCanvas.GetTop(text), text.Width, text.Height, parent.Page);
                    text = null;
                }
                if (type == "Pen" || type == "Brush")
                {
                    try
                    {
                        pdf.Strokes[idx].StylusPoints.RemoveAt(0);
                    }
                    catch
                    {
                        pdf.Strokes.Clear();
                    }
                }
            }
        }

        public void MemoReceive(string id, string bookname)
        {
            if (this.bookname != bookname)
                return;
            if (this.type != "Teacher" && mode)
            {
                foreach (UIElement u in pdf.Children)
                {
                    if (u is TextControl)
                    {
                        if (((TextControl)u).ID == id)
                        {
                            if (((TextControl)u).Popup.IsOpen)
                            {
                                ((TextControl)u).Popup.IsOpen = false;
                            }
                            else ((TextControl)u).Popup.IsOpen = true;
                        }
                    }
                }
            }
        }

        public void MemoTextReceive(string id, string text, string bookname)
        {
            if (this.bookname != bookname)
                return;
            if (this.type != "Teacher" && mode)
            {
                foreach (UIElement u in pdf.Children)
                {
                    if (u is TextControl)
                    {
                        if (((TextControl)u).ID == id)
                        {
                            ((TextControl)u).Text = text;

                            NoteXml.Save(id, parent.Page, text);
                        }
                    }
                }
            }
        }

        public static bool ListEquals<T>(IList<T> list1, IList<T> list2)
        {
            if (list1.Count != list2.Count)
                return false;
            for (int i = 0; i < list1.Count; i++)
                if (!list1[i].Equals(list2[i]))
                    return false;
            return true;
        }

        public void EraseReceive(string type, string page, double left, double top, Point[] pts, string bookname)
        {
            if (this.bookname != bookname)
                return;
            if (this.type != "Teacher" && mode)
            {
                switch (type)
                {
                    case "Pen":
                        {
                            StylusPointCollection spc = new StylusPointCollection(pts);
                            Stroke stroke = new Stroke(spc);
                            Stroke delete = null;

                            foreach (Stroke st in pdf.Strokes)
                            {
                                if (ListEquals(st.StylusPoints, stroke.StylusPoints))
                                {
                                    delete = st;
                                    break;
                                }
                            }


                            if (delete != null)
                                //DrawXml.RemoveDraw.BeginInvoke(parent.Page, pdf.Strokes.IndexOf(delete).ToString(), parent.Count--, new AsyncCallback(EndAsync), null);
                                //DrawXml.Remove(parent.Page, pdf.Strokes.IndexOf(delete).ToString(), parent.Count--);

                                DrawXml.Remove(parent.Page, pdf.Strokes.IndexOf(delete), pdf.Strokes.Count);

                            pdf.Strokes.Remove(delete);
                        } break;
                    case "Rectangle":
                        {
                            foreach (UIElement u in pdf.Children)
                            {
                                if (u is TextControl)
                                {
                                    if (((double)u.GetValue(InkCanvas.LeftProperty)).ToString("N2") == left.ToString("N2") &&
                                        ((double)u.GetValue(InkCanvas.TopProperty)).ToString("N2") == top.ToString("N2"))
                                    {
                                        parent.Flag = 1;
                                        pdf.Children.Remove(u);
                                        parent.Flag = 0;
                                        return;
                                    }
                                }
                            }
                        } break;
                }
            }
        }

        public void ReceivePage(string page, string bookname)
        {
            if (this.bookname != bookname)
                return;
            if (this.type != "Teacher" && mode)
                parent.Move(page);
        }

        public void ReceiveScroll(double offset, string bookname)
        {
            try
            {
                if (this.bookname != bookname)
                    return;
                if (this.type != "Teacher" && mode)
                    ((ScrollViewer)pdf.Parent).ScrollToVerticalOffset(offset);
            }
            catch { }
        }

        public bool ExistFile()
        {
            if (file != "" && filepath != "")
            {
                file = "";
                filepath = "";
                return true;
            }
            else return false;
        }

        string file = "";
        string filepath = "";

        public string File
        {
            get { return file; }
        }

        public string FilePath
        {
            get { return filepath; }
        }

        public void SendResponse(string sender, string file)
        {
            if (System.Windows.MessageBox.Show(sender + "님이 파일 전송을 요청합니다.", "File Transfer", MessageBoxButton.YesNo)
               == MessageBoxResult.Yes)
            {
                System.Windows.Forms.SaveFileDialog dlg = new System.Windows.Forms.SaveFileDialog()
                {
                    Title = "Select a file to download",
                    RestoreDirectory = true,
                    FileName = file
                };

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.filepath = dlg.FileName;
                    this.file = file;
                }
            }
            else
                this.client.DeleteFile(file, -1);
        }
        #endregion
    }
}
