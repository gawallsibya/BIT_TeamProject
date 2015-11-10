using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using PdfSharp.Pdf;
using PdfSharp;
using PdfSharp.Drawing;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Windows.Ink;
using UserWallPaper.CustomControl;
using UserWallPaper.Xml;
using com.gmail.nishantsinhaindia.DocumentConverter;

namespace UserWallPaper
{
    internal struct Matrix
    {
        public float A, B, C, D, E, F;
    }

    internal struct Rectangle
    {
        public float Left, Top, Right, Bottom;

        public float Width { get { return this.Right - this.Left; } }
        public float Height { get { return this.Bottom - this.Top; } }

    }
    public enum Shape { None, Erase, Pen, Rectangle, Brush };
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 변수
        //pdf 파일
        string file = string.Empty;

        static InkCanvas mycan;
        //pdf 문서 변수
        PDFLibNet.PDFWrapper _pdfDoc;
        //기본 사이즈
        static float size = 2.0f;

        //서비스 클래스
        Service service;

        //모양 열거자
        public Shape shape;
        #endregion

        public Service Service
        {
            get { return this.service; }
        }

        public string FileName
        {
            get { return this.file; }
            set { this.file = value; }
        }

        public string Page
        {
            get { return this.first.Text; }
            set { this.first.Text = value; }
        }

        public int Flag
        {
            get;
            set;
        }

        #region 초기화
        public MainWindow(Service s)
        {
            InitializeComponent();

            this.SourceInitialized += new EventHandler(win_SourceInitialized);

            service = s;
            service.Parent = this;
            service.Pdf = this.pdf;

            //user control 등록/////////
            picker.ButtonClick += User_ButtonClick;
            picker.PopupChecked += User_PopupCheck;
            menuthick.ButtonClick += User_ButtonClick;
            menuthick.PopupChecked += User_PopupCheck;
            brushpicker.ButtonClick += User_ButtonClick;
            brushpicker.PopupChecked += User_PopupCheck;
            bookmark.PopupChecked += User_PopupCheck;
            ////////////////////////////

            menu.Height = 0;

            mycan = pdf;

            this.first.Text = "1";

            this.pdf.IsEnabled = false;
            this.pdf.DefaultDrawingAttributes.Width = fontsize;
            this.pdf.DefaultDrawingAttributes.Height = fontsize;
            this.pdf.Parent = this;
            this.pdf.Service = s;

            //시간표에 따른 교재 출력
            string day = DateToDay(DateTime.Now);

            TimeBookLoad(TimeXml.LoadDay(day));
        }

        void win_SourceInitialized(object sender, EventArgs e)
        {
            WindowMaximize.Create(this);

            if (service.Type == "Teacher")
            {
                menu_mode.IsEnabled = false;
            }
        }
        #endregion

        #region pdf 파일 오픈
        public void FileOpen(string file)
        {
            this.file = file;
            Shape_Change(Shape.None);
            pdf.Children.Clear();
            pdf.Strokes.Clear();

            //기존 pdf Dispose
            if (_pdfDoc != null)
            {
                _pdfDoc.Dispose();
            }

            FileInfo finfo = new FileInfo(file);

            if (finfo.Extension != ".pdf") //PDF가 아닌 파일
            {
                //변환하는 소스코드
                string filePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "TestOutput");


                string fileName = finfo.FullName;

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                string outFileName = Util.getTargetFileName(filePath, fileName, ContentType.PDF);

                if (!File.Exists(outFileName))
                {
                    ConverterServices.convert(fileName, outFileName);
                }

                this.file = outFileName;
            }

            //pdf 파일 로드
            _pdfDoc = new PDFLibNet.PDFWrapper();
            _pdfDoc.LoadPDF(this.file);

            last.Text = _pdfDoc.PageCount.ToString();

            //pdf 페이지 로드
            GetPDFView(this.file, first.Text);

            //test();
            //노트 xml 이름
            NoteXml.BookName(System.IO.Path.GetFileName(this.file));
            DrawXml.BookName(System.IO.Path.GetFileName(this.file));

            ///
            try
            {
                Read();
            }
            catch
            {
            }
            ///

            //노트 xml 로드
            notepad.Text = NoteXml.Load(first.Text);

            if (service != null && !service.Mode)
            {
                this.pdf.IsEnabled = true;
                menu_mode.IsEnabled = false;
                return;
            }
            this.pdf.IsEnabled = true;
        }
        #endregion

        #region 필기내용 Read

        private void Read()
        {
            DrawXml.PdfWin = this;
            ReadData[] readdata = DrawXml.XML_Read(first.Text);

            if (readdata != null)
            {
                foreach (ReadData data in readdata)
                {
                    if (data.Type == "Pen")
                    {
                        StylusPointCollection spc = new StylusPointCollection(data.Pts);
                        Stroke stroke = new Stroke(spc);
                        stroke.DrawingAttributes.Color = data.Color;
                        stroke.DrawingAttributes.Width = data.FontSize;
                        stroke.DrawingAttributes.Height = data.FontSize;
                        pdf.Strokes.Add(stroke);
                    }

                    else if (data.Type == "Brush")
                    {
                        StylusPointCollection spc = new StylusPointCollection(data.Pts);
                        Stroke stroke = new Stroke(spc);
                        stroke.DrawingAttributes.Color = data.Color;
                        stroke.DrawingAttributes.Width = data.FontSize / 2;
                        stroke.DrawingAttributes.Height = data.FontSize;
                        stroke.DrawingAttributes.IsHighlighter = true;
                        stroke.DrawingAttributes.StylusTip = StylusTip.Rectangle;
                        pdf.Strokes.Add(stroke);
                    }
                    else
                    {
                        pdf.Children.Add(data.Text);
                    }
                }
            }
        }

        private string[] Splitline(string str)
        {
            string[] temp = str.Split('\t');

            return temp;
        }
        #endregion

        #region 시간표 설정에 따른 교재 출력
        private void TimeBookLoad(ArrayList date)
        {
            if (date != null)
            {
                foreach (string[] time in date)
                {
                    string[] start = time[1].Split('~');
                    string[] end = time[2].Split('~');
                    DateTime dt = Convert.ToDateTime(start[0]);
                    if (Convert.ToDateTime(start[0]) < DateTime.Now && DateTime.Now < Convert.ToDateTime(end[end.Count<string>()-1]))
                    {
                        string book = time[0];

                        FileOpen(book);
                    }
                }
            }
        }
        #endregion

        #region 현재 시간에 따른 요일 반환
        private string DateToDay(DateTime now)
        {
            string day = now.DayOfWeek.ToString();

            switch (day)
            {
                case "Monsday": day = "Mon"; break;
                case "Tuesday": day = "Tue"; break;
                case "Wednesday": day = "Wed"; break;
                case "Thursday": day = "Thu"; break;
                case "Friday": day = "Fri"; break;
                case "Saturday": day = "Sat"; break;
            }

            return day;
        }
        #endregion

        #region pdf 파일 로드

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //pdf파일 오픈 다이얼로그
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "PDF파일(*.pdf)|*.pdf|Word Files (*.doc,*.docx,*.txt,*.rtf,*.xml)|*.doc;*.docx;*.txt;*.rtf;*.xml|Excel Files (*.xls,*.xlsx,*.csv)|*.xls;*.xlsx;*.csv|PowerPoint Files (*.ppt,*.pptx)|*.ppt;*.pptx|Visio Files (*.vsd;*.vdx)|*.vsd;*.vdx";

                dlg.ShowDialog();

                FileOpen(dlg.FileName);
            }

            catch { }
        }

        #region pdf to imagesource
        private void GetPDFView(string path, string page)
        {
            pdf.Children.Clear();

            System.Windows.Controls.Image image = new System.Windows.Controls.Image
            {
                Source = Imaging.CreateBitmapSourceFromHBitmap(ExtractPage(path, int.Parse(first.Text)).GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())
            };

            pdf.Children.Add(image);
        }
        #endregion

        public static Bitmap ExtractPage(string pdfFilename, int pageNumber)
        {
            var pageNumberIndex = Math.Max(0, pageNumber - 1); // pages start at index 0

            using (var stream = new PdfFileStream(pdfFilename))
            {
                IntPtr p = NativeMethods.LoadPage(stream.Document, pageNumberIndex); // loads the page
                var bmp = RenderPage(stream.Context, stream.Document, p);
                NativeMethods.FreePage(stream.Document, p); // releases the resources consumed by the page

                return bmp;
            }
        }

        static Bitmap RenderPage(IntPtr context, IntPtr document, IntPtr page)
        {
            Rectangle pageBound = NativeMethods.BoundPage(document, page);
            float zoomFactor = size;

            Matrix ctm = new Matrix();
            IntPtr pix = IntPtr.Zero;
            IntPtr dev = IntPtr.Zero;

            var currentDpi = DpiHelper.GetCurrentDpi();
            var zoomX = zoomFactor * (currentDpi.HorizontalDpi / DpiHelper.DEFAULT_DPI);
            var zoomY = zoomFactor * (currentDpi.VerticalDpi / DpiHelper.DEFAULT_DPI);

            // gets the size of the page and multiplies it with zoom factors
            int width = (int)(pageBound.Width * zoomX);
            int height = (int)(pageBound.Height * zoomY);
            mycan.Width = width;
            mycan.Height = height;

            // sets the matrix as a scaling matrix (zoomX,0,0,zoomY,0,0)
            ctm.A = zoomX;
            ctm.D = zoomY;

            // creates a pixmap the same size as the width and height of the page
            pix = NativeMethods.NewPixmap(context, NativeMethods.FindDeviceColorSpace(context, "DeviceRGB"), width, height);
            // sets white color as the background color of the pixmap
            NativeMethods.ClearPixmap(context, pix, 0xFF);

            // creates a drawing device
            dev = NativeMethods.NewDrawDevice(context, pix);
            // draws the page on the device created from the pixmap
            NativeMethods.RunPage(document, page, dev, ctm, IntPtr.Zero);

            NativeMethods.FreeDevice(dev); // frees the resources consumed by the device
            dev = IntPtr.Zero;

            // creates a colorful bitmap of the same size of the pixmap
            Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            var imageData = bmp.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            unsafe
            { // converts the pixmap data to Bitmap data
                byte* ptrSrc = (byte*)NativeMethods.GetSamples(context, pix); // gets the rendered data from the pixmap
                byte* ptrDest = (byte*)imageData.Scan0;
                for (int y = 0; y < height; y++)
                {
                    byte* pl = ptrDest;
                    byte* sl = ptrSrc;
                    for (int x = 0; x < width; x++)
                    {
                        //Swap these here instead of in MuPDF because most pdf images will be rgb or cmyk.
                        //Since we are going through the pixels one by one anyway swap here to save a conversion from rgb to bgr.
                        pl[2] = sl[0]; //b-r
                        pl[1] = sl[1]; //g-g
                        pl[0] = sl[2]; //r-b
                        //sl[3] is the alpha channel, we will skip it here
                        pl += 3;
                        sl += 4;
                    }
                    ptrDest += imageData.Stride;
                    ptrSrc += width * 4;
                }
            }
            bmp.UnlockBits(imageData);

            NativeMethods.DropPixmap(context, pix);
            bmp.SetResolution(currentDpi.HorizontalDpi, currentDpi.VerticalDpi);

            return bmp;
        }

        private sealed class PdfFileStream : IDisposable
        {
            const uint FZ_STORE_DEFAULT = 256 << 20;

            public IntPtr Context { get; private set; }
            public IntPtr Stream { get; private set; }
            public IntPtr Document { get; private set; }

            public PdfFileStream(string pdfFilename)
            {
                Context = NativeMethods.NewContext(IntPtr.Zero, IntPtr.Zero, FZ_STORE_DEFAULT); // Creates the context
                Stream = NativeMethods.OpenFile(Context, pdfFilename); // opens file as a stream
                Document = NativeMethods.OpenDocumentStream(Context, ".pdf", Stream); // opens the document
            }

            public void Dispose()
            {
                NativeMethods.CloseDocument(Document); // releases the resources
                NativeMethods.CloseStream(Stream);
                NativeMethods.FreeContext(Context);
            }
        }

        private static class NativeMethods
        {
            const string DLL = "libmupdf.dll";

            [DllImport(DLL, EntryPoint = "fz_new_context", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NewContext(IntPtr alloc, IntPtr locks, uint max_store);

            [DllImport(DLL, EntryPoint = "fz_free_context", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr FreeContext(IntPtr ctx);

            [DllImport(DLL, EntryPoint = "fz_open_file_w", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr OpenFile(IntPtr ctx, string fileName);

            [DllImport(DLL, EntryPoint = "fz_open_document_with_stream", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr OpenDocumentStream(IntPtr ctx, string magic, IntPtr stm);

            [DllImport(DLL, EntryPoint = "fz_close", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CloseStream(IntPtr stm);

            [DllImport(DLL, EntryPoint = "fz_close_document", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr CloseDocument(IntPtr doc);

            [DllImport(DLL, EntryPoint = "fz_count_pages", CallingConvention = CallingConvention.Cdecl)]
            public static extern int CountPages(IntPtr doc);

            [DllImport(DLL, EntryPoint = "fz_bound_page", CallingConvention = CallingConvention.Cdecl)]
            public static extern Rectangle BoundPage(IntPtr doc, IntPtr page);

            [DllImport(DLL, EntryPoint = "fz_clear_pixmap_with_value", CallingConvention = CallingConvention.Cdecl)]
            public static extern void ClearPixmap(IntPtr ctx, IntPtr pix, int byteValue);

            [DllImport(DLL, EntryPoint = "fz_find_device_colorspace", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr FindDeviceColorSpace(IntPtr ctx, string colorspace);

            [DllImport(DLL, EntryPoint = "fz_free_device", CallingConvention = CallingConvention.Cdecl)]
            public static extern void FreeDevice(IntPtr dev);

            [DllImport(DLL, EntryPoint = "fz_free_page", CallingConvention = CallingConvention.Cdecl)]
            public static extern void FreePage(IntPtr doc, IntPtr page);

            [DllImport(DLL, EntryPoint = "fz_load_page", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr LoadPage(IntPtr doc, int pageNumber);

            [DllImport(DLL, EntryPoint = "fz_new_draw_device", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NewDrawDevice(IntPtr ctx, IntPtr pix);

            [DllImport(DLL, EntryPoint = "fz_new_pixmap", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr NewPixmap(IntPtr ctx, IntPtr colorspace, int width, int height);

            [DllImport(DLL, EntryPoint = "fz_run_page", CallingConvention = CallingConvention.Cdecl)]
            public static extern void RunPage(IntPtr doc, IntPtr page, IntPtr dev, Matrix transform, IntPtr cookie);

            [DllImport(DLL, EntryPoint = "fz_drop_pixmap", CallingConvention = CallingConvention.Cdecl)]
            public static extern void DropPixmap(IntPtr ctx, IntPtr pix);

            [DllImport(DLL, EntryPoint = "fz_pixmap_samples", CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr GetSamples(IntPtr ctx, IntPtr pix);
        }
        #endregion

        #region 다음 페이지
        private void next_Click(object sender, RoutedEventArgs e)
        {
            if (file == string.Empty || int.Parse(first.Text) >= _pdfDoc.PageCount) return;

            string notepage = first.Text;

            first.Text = (int.Parse(first.Text) + 1).ToString();
            GetPDFView(file, first.Text);

            pdf.Strokes.Clear();

            if (service != null)
                service.MovePage(first.Text);

            ///저장 포인트  
            ///저장해야 할 것
            ///노트,필기내용,이미지

            #region 노트
            if (notepad.Text != "")
            {
                NoteXml.Save(notepage, notepad.Text);
                notepad.Text = "";
            }

            string detail = NoteXml.Load(first.Text);
            if (detail != null)
            {
                notepad.Text = detail;
            }
            #endregion

            try
            {
                Read();
            }
            catch
            {
            }

        }
        #endregion

        #region 이전 페이지
        private void previous_Click(object sender, RoutedEventArgs e)
        {
            if (file == string.Empty) return;

            string notepage = first.Text;

            int x = int.Parse(first.Text) - 1;
            if (x <= 0)
            {
                x = 1;
                notepage = "1";
            }

            first.Text = x.ToString();
            GetPDFView(file, first.Text);

            pdf.Strokes.Clear();

            if (service != null)
                service.MovePage(first.Text);

            ///저장 포인트
            ///저장해야 할 것
            ///노트,필기내용,이미지

            #region 노트
            if (notepad.Text != "")
            {
                NoteXml.Save(notepage, notepad.Text);
                notepad.Text = "";
            }

            string detail = NoteXml.Load(first.Text);
            if (detail != null)
            {
                notepad.Text = detail;
            }
            #endregion

            try
            {
                Read();
            }
            catch
            {

            }
        }
        #endregion

        #region 이벤트에 따른 페이지 이동
        public void Move(string page)
        {
            #region 노트
            if (notepad.Text != "")
            {
                NoteXml.Save(first.Text, notepad.Text);
                notepad.Text = "";
            }

            string detail = NoteXml.Load(first.Text);
            if (detail != null)
            {
                notepad.Text = detail;
            }
            #endregion

            first.Text = page;
            GetPDFView(file, page);
            pdf.Strokes.Clear();

            try
            {
                Read();
            }
            catch
            {

            }
        }
        #endregion

        #region 확대 및 축소
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Image btn = (System.Windows.Controls.Image)sender;
            if (btn.Name == "plus")
            {
                if (file == string.Empty) return;

                //size += 0.5f;

                st.ScaleX += 0.1;
                st.ScaleY += 0.1;
            }
            else
            {
                if (file == string.Empty || size <= 0) return;

                //bool flag = (size -= 0.5f) == 0 ? true : false;

                //if (flag)
                //{
                //    size = 0.5f;
                //    return;
                //}

                st.ScaleX -= 0.1;
                st.ScaleY -= 0.1;
            }
        }
        #endregion

        #region 상단메뉴 애니메이션
        private void Menu_MouseEnter_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DoubleAnimation MoveAnimation = Resources["MenuAnimation"] as DoubleAnimation;

            MyAnimation.MenuAnimation(MoveAnimation, menu, System.Windows.Controls.ListBox.HeightProperty, 45);
        }

        private void Menu_MouseLeave_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!menupin && pen == 0)
            {
                DoubleAnimation MoveAnimation = Resources["MenuAnimation"] as DoubleAnimation;

                MyAnimation.MenuAnimation(MoveAnimation, menu, System.Windows.Controls.ListBox.HeightProperty, 0);
            }
        }
        #endregion

        #region 우측 노트 애니메이션
        static bool notepin = false;
        private void NotePin_Click(object sender, MouseButtonEventArgs e)
        {
            if (!notepin)
            {
                notepin = true;

                DoubleAnimation NoteAnimation = Resources["NoteAnimation"] as DoubleAnimation;
                NoteAnimation.To = 600;

                grid.Width = grid.Width - 530;


                notepad.Visibility = System.Windows.Visibility.Visible;

                Note.BeginAnimation(Grid.WidthProperty, NoteAnimation, HandoffBehavior.Compose);
            }
            else
            {
                notepin = false;

                DoubleAnimation NoteAnimation = Resources["NoteAnimation"] as DoubleAnimation;
                NoteAnimation.To = 70;
                grid.Width = grid.Width + 530;

                notepad.Visibility = System.Windows.Visibility.Hidden;

                Note.BeginAnimation(Grid.WidthProperty, NoteAnimation, HandoffBehavior.Compose);
            }
        }
        #endregion

        #region 펜관련 이벤트 및 설정
        private void User_PopupCheck(object sender, RoutedEventArgs e)
        {
            if ((bool)((ToggleButton)e.Source).IsChecked)
            {
                pen = 1;
                ((Popup)sender).IsOpen = true;
            }
            else
            {
                pen = 0;
                ((Popup)sender).IsOpen = false;
                if (!menugrid.IsMouseOver)
                    Menu_MouseLeave_1(null, null);
            }
        }
        static int fontsize = 1;
        private void User_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is PickerControl)
            {
                string color = ((System.Windows.Controls.Button)e.Source).CommandParameter.ToString();
                Set_Pen((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(color));
                Shape_Change(Shape.Pen);
                ((PickerControl)sender).Popup.IsOpen = false;
                pen = 0;
            }
            else if (sender is ThickControl)
            {
                int idx = (int)((System.Windows.Controls.Button)e.Source).GetValue(Grid.RowProperty);

                switch (idx)
                {
                    case 0: fontsize = 1; break;
                    case 1: fontsize = 3; break;
                    case 2: fontsize = 5; break;
                    case 3: fontsize = 7; break;
                }

                pdf.DefaultDrawingAttributes.Width = fontsize;
                pdf.DefaultDrawingAttributes.Height = fontsize;

                ((ThickControl)sender).Popup.IsOpen = false;
                pen = 0;
            }
            else if (sender is BrushControl)
            {
                string col = (string)((System.Windows.Controls.Button)e.Source).GetValue(System.Windows.Controls.Button.NameProperty);
                Shape_Change(Shape.Brush);

                switch (col)
                {
                    case "Yellow_1":
                    case "Pink_1":
                    case "SkyBlue_1":
                    case "Green_1":
                    case "Red_1":
                        fontsize = 10; break;

                    case "Yellow_3":
                    case "Pink_3":
                    case "SkyBlue_3":
                    case "Green_3":
                    case "Red_3":
                        fontsize = 20; break;

                    case "Yellow_5":
                    case "Pink_5":
                    case "SkyBlue_5":
                    case "Green_5":
                    case "Red_5":
                        fontsize = 30; break;

                    case "Yellow_7":
                    case "Pink_7":
                    case "SkyBlue_7":
                    case "Green_7":
                    case "Red_7":
                        fontsize = 40; break;
                }

                pdf.DefaultDrawingAttributes.Height = fontsize;
                pdf.DefaultDrawingAttributes.Width = fontsize / 2;

                string color = ((System.Windows.Controls.Button)e.Source).CommandParameter.ToString();
                Set_Pen((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(color));

                ((BrushControl)sender).Popup.IsOpen = false;
                pen = 0;
            }
        }

        public void Set_Pen(System.Windows.Media.Color color)
        {
            pdf.DefaultDrawingAttributes.Color = color;
        }
        #endregion

        #region 페이지 바로가기
        private void first_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (int.Parse(first.Text) > 0 && int.Parse(first.Text) <= int.Parse(last.Text))
                {
                    GetPDFView(file, first.Text);
                    pdf.Strokes.Clear();

                    if (service != null)
                        service.MovePage(first.Text);
                }
            }
        }
        #endregion

        #region 메뉴 및 노트 고정 이벤트
        static bool menupin = false;
        private void MenuPin_Click(object sender, MouseButtonEventArgs e)
        {
            if (!menupin)
                menupin = true;
            else
                menupin = false;
        }
        #endregion

        #region 마우스 휠 페이지 이동
        private void ScrollViewer_MouseWheel_1(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer sv = sender as ScrollViewer;

            var v = pdf.Children.OfType<TextControl>();

            foreach (TextControl t in v)
            {
                t.Popup.IsOpen = false;
            }

            if (sv.VerticalOffset == 0 && e.Delta > 0)
            {
                if (first.Text != "1")
                {
                    sv.ScrollToBottom();

                    previous_Click(null, null);
                }
                else return;
            }
            else if (sv.VerticalOffset == sv.ScrollableHeight && e.Delta < 0)
            {
                sv.ScrollToTop();

                next_Click(null, null);
            }
        }

        private void ScrollViewer_ScrollChanged_1(object sender, ScrollChangedEventArgs e)
        {
            if (service != null)
                service.SendScroll(e.VerticalOffset);
        }
        #endregion

        #region InkCanvas Save
        private void Save_Click(object sender, MouseButtonEventArgs e)
        {
            FileInfo f = new FileInfo(".\\book\\" + System.IO.Path.GetFileName(file) + "&" + first.Text + ".png");
            if (f.Exists)
                f.Delete();
            SaveImageFromElement(pdf, ".\\book\\" + System.IO.Path.GetFileName(file) + "&" + first.Text + ".png");
        }

        public static void SaveImageFromElement(FrameworkElement obj, string filename)
        {
            System.Windows.Shapes.Rectangle rect = new System.Windows.Shapes.Rectangle();
            //Upload Image Size            
            VisualBrush vb = new VisualBrush();// System.Media
            vb.Visual = obj;
            rect.Width = obj.ActualWidth;
            rect.Height = obj.ActualHeight;
            rect.Fill = vb;
            ImageSource img = ToImageSource(rect); //첫번째 호출 함수

            SaveImage(img as BitmapSource, filename); //두번째 호출함수
        }
        public static ImageSource ToImageSource(FrameworkElement obj)
        {
            // Save current canvas transform
            Transform transform = obj.LayoutTransform;
            obj.LayoutTransform = null;
            // fix margin offset as well
            Thickness margin = obj.Margin;
            obj.Margin = new Thickness(0, 0,
                 margin.Right - margin.Left, margin.Bottom - margin.Top);

            // Get the size of canvas
            System.Windows.Size size = new System.Windows.Size(obj.Width, obj.Height);
            // force control to Update
            obj.Measure(size);
            obj.Arrange(new Rect(size));
            RenderTargetBitmap bmp = new RenderTargetBitmap(
                (int)obj.Width, (int)obj.Height, 0, 0, PixelFormats.Default);
            bmp.Render(obj);
            // return values as they were before
            obj.LayoutTransform = transform;
            obj.Margin = margin;
            return bmp;
        }
        public static void SaveImage(BitmapSource img, string fileName)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = File.Create(fileName); //System.IO
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.QualityLevel = 80;
            encoder.Frames.Add(BitmapFrame.Create(img));
            encoder.Save(stream);
            stream.Dispose();
            //stream.Flush(); //Dispose나 Flush 둘중에 아무거나 사용해도 됨.
            stream.Close();
        }

        #endregion InkCanvas Save

        #region 마우스 이벤트
        TextControl text;
        private System.Windows.Point startPoint;
        static private bool isdown = false;
        private void pdf_MouseDown(object sender, MouseButtonEventArgs e)
        {
            /*
            System.Windows.Point pt = e.GetPosition(pdf);
            DrawData.Oldpt = pt;
            */
            if (shape == Shape.Pen || shape == Shape.Brush)
            {
                isdown = true;
                if (service != null)
                    service.DrawPrepare(shape.ToString(), pdf.DefaultDrawingAttributes.Color, e.GetPosition(pdf), fontsize);
            }
            else if (shape == Shape.Rectangle)
            {
                startPoint = e.GetPosition(pdf);

                text = new TextControl
                {
                    BorderBrush = System.Windows.Media.Brushes.LightBlue,
                    BorderThickness = new Thickness(2),
                    Background = System.Windows.Media.Brushes.LightGreen,
                    Opacity = 0.5
                };
                text.Text_Changed += Text_Changed;

                InkCanvas.SetLeft(text, startPoint.X);
                InkCanvas.SetTop(text, startPoint.Y);
                pdf.Children.Add(text);

                if (service != null)
                    service.DrawPrepare(shape.ToString(), Colors.Transparent, startPoint, fontsize);
            }

        }

        private void pdf_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && shape == Shape.Erase)
            {
                InkPresenter inkPresenter = GetVisualChild<InkPresenter>(pdf);

                HitTestResult hitTestResult = VisualTreeHelper.HitTest(inkPresenter, e.GetPosition(pdf));

                if (hitTestResult == null)
                    return;

                var v = hitTestResult.VisualHit;

                while (!(v is TextControl))
                {
                    v = VisualTreeHelper.GetParent(v);
                    if (v == null)
                        break;
                }

                if (hitTestResult.VisualHit != null && v is TextControl)
                {
                    Flag = 1;
                    pdf.Children.Remove((UIElement)v);
                    Flag = 0;
                }
            }

            if (e.LeftButton == MouseButtonState.Pressed && (shape == Shape.Pen || shape == Shape.Brush) && isdown)
            {
                System.Windows.Point pt = e.GetPosition(pdf);
                if (pt == startPoint)
                    return;
                startPoint = pt;


                if (service != null)
                    service.Draw(shape.ToString(), pdf.DefaultDrawingAttributes.Color, pt, fontsize);

                DrawXml.Xml_Save(shape.ToString(), pdf.Strokes.Count, pt, first.Text, pdf.DefaultDrawingAttributes.Color, fontsize);
            }
            else if (e.LeftButton == MouseButtonState.Pressed && shape == Shape.Rectangle)
            {
                var pos = e.GetPosition(pdf);

                var x = Math.Min(pos.X, startPoint.X);
                var y = Math.Min(pos.Y, startPoint.Y);

                var w = Math.Max(pos.X, startPoint.X) - x;
                var h = Math.Max(pos.Y, startPoint.Y) - y;

                text.Width = w;
                text.Height = h;

                InkCanvas.SetLeft(text, x);
                InkCanvas.SetTop(text, y);

                if (service != null)
                    service.Draw(shape.ToString(), pdf.DefaultDrawingAttributes.Color, pos, fontsize);
            }
        }

        private void pdf_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (shape == Shape.Rectangle && text != null)
            {
                double left = (double)text.GetValue(InkCanvas.LeftProperty);
                double top = (double)text.GetValue(InkCanvas.TopProperty);
                double width = text.Width;
                double height = text.Height;

                text.ID = first.Text + "-" + top.ToString() + "/" + left.ToString();
                DrawXml.Xml_Save(shape.ToString(), text.ID, left, top, width, height, first.Text);

                if (service != null)
                    service.DrawEnd(shape.ToString(), text.ID);

                Shape_Change(Shape.None);
                text = null;
            }

            else if (shape == Shape.Pen || shape == Shape.Brush)
            {
                if (service != null)
                    service.DrawEnd(shape.ToString(), null);
                isdown = false;
            }
        }

        public T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                    break;
            }
            return child;
        }
        #endregion


        public void Text_Changed(object sender, TextChangedEventArgs e)
        {
            string id = ((TextControl)sender).ID;
            string text = ((System.Windows.Controls.TextBox)e.Source).Text;

            NoteXml.Save(id, first.Text, text);

            if (service != null)
                service.MemoText(id, text);
        }

        #region 윈도우 종료
        private void Window_Closed_1(object sender, EventArgs e)
        {
            try
            {
                if (service != null)
                {
                    _pdfDoc.Dispose();

                    string path = null;

                    DrawXml.SavePath(ref path);

                    path += "\t";

                    NoteXml.SavePath(ref path);

                    string[] files = path.Split('\t');

                    service.PutFile(files);
                }
            }
            catch
            {
                //System.Windows.MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 서비스 모드 설정(자동,수동)
        private void Mode_Click(object sender, MouseButtonEventArgs e)
        {
            if (service != null)
            {
                if (!service.Mode)
                {
                    service.Mode = true;
                    this.pdf.IsEnabled = false;
                    ((System.Windows.Controls.Image)sender).Source = new BitmapImage(new Uri("../Image/Mode_Auto.png", UriKind.Relative));
                }
                else
                {
                    service.Mode = false;
                    this.pdf.IsEnabled = true;
                    ((System.Windows.Controls.Image)sender).Source = new BitmapImage(new Uri("../Image/Mode_Manual.png", UriKind.Relative));
                }
            }
        }
        #endregion

        #region 메모장
        private void Text_click(object sender, MouseButtonEventArgs e)
        {
            Shape_Change(Shape.Rectangle);
        }
        #endregion

        #region 메뉴 아이콘 핀
        static int pen = 0;
        public int Pen
        {
            get { return pen; }
            set { pen = value; }
        }
        #endregion

        #region 마우스 모드 설정
        public void Shape_Change(Shape s)
        {
            pdf.DefaultDrawingAttributes.IsHighlighter = false;
            pdf.DefaultDrawingAttributes.StylusTip = System.Windows.Ink.StylusTip.Ellipse;
            switch (s)
            {
                case Shape.None:
                    {
                        shape = Shape.None;
                        pdf.EditingMode = InkCanvasEditingMode.None;
                    } break;
                case Shape.Erase:
                    {
                        shape = Shape.Erase;
                        pdf.EditingMode = InkCanvasEditingMode.EraseByStroke;
                    } break;
                case Shape.Pen:
                    {
                        shape = Shape.Pen;
                        pdf.EditingMode = InkCanvasEditingMode.Ink;
                        //this.pdf.Cursor = System.Windows.Input.Cursors.Pen;
                    } break;
                case Shape.Rectangle:
                    {
                        shape = Shape.Rectangle;
                        pdf.EditingMode = InkCanvasEditingMode.None;
                    } break;
                case Shape.Brush:
                    {
                        shape = Shape.Brush;
                        pdf.EditingMode = InkCanvasEditingMode.Ink;
                        pdf.DefaultDrawingAttributes.IsHighlighter = true;
                        pdf.DefaultDrawingAttributes.StylusTip = System.Windows.Ink.StylusTip.Rectangle;
                    } break;
            }
        }
        #endregion

        #region 시간표 실행
        private void TimeList_Click(object sender, MouseButtonEventArgs e)
        {
            TimeWindow twindow = new TimeWindow();
            twindow.ShowDialog();
        }
        #endregion

        private void Erase_Click(object sender, MouseButtonEventArgs e)
        {
            Shape_Change(Shape.Erase);
        }

        private void DefaultCursor(object sender, MouseButtonEventArgs e)
        {
            Shape_Change(Shape.None);
        }
    }
}
