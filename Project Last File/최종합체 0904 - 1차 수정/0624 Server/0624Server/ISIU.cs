using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;

namespace _0624Server
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ISIUCallback))]
    public interface ISIU
    {
        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false)]
        string[] Join(string serial);

        [OperationContract(IsOneWay = false)]
        string MyType(string serial);

        [OperationContract(IsOneWay = false)]
        string MyName(string serial);

        [OperationContract(IsOneWay = true)]
        void MyBook(string bookname);

        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = false)]
        void Say(string msg);

        [OperationContract(IsOneWay = false)]
        bool Save(string serial, string id, string pw, string name, string sort, string number);

        [OperationContract(IsOneWay = false)]
        bool Find(string serial);

        [OperationContract(IsOneWay = false)]
        string FindName(string serial);

        [OperationContract(IsOneWay = false)]
        string FindType(string serial);

        [OperationContract(IsOneWay = false)]
        string FindUserInfo(string serial);

        [OperationContract(IsOneWay = false)]
        bool Rejoin(string serial, string id, string pw);

        [OperationContract(IsOneWay = false)]
        bool Login(string id, string pw);

        [OperationContract(IsOneWay = false)]
        string FindtoID(string id);

        [OperationContract(IsOneWay = true)]
        void DrawPrepare(string type, Color color, Point pt, int fontsize);

        [OperationContract(IsOneWay = true)]
        void Draw(string type, Color color, Point pt, int fontsize);

        [OperationContract(IsOneWay = true)]
        void DrawEnd(string type, string id);

        [OperationContract(IsOneWay = true)]
        void MovePage(string page);

        [OperationContract(IsOneWay = true)]
        void Leave();

        [OperationContract(IsOneWay = false)]
        string[] Files(int nflag);

        //-------------- 과제 관련 정의 ----------------
        [OperationContract(IsOneWay = false)]
        bool WriteSugesstSubmissionList(string[] str);

        [OperationContract(IsOneWay = false)]
        ArrayList ReadSugesstSubmissionList();

        [OperationContract(IsOneWay = false)]
        bool DeleteSubmission(string id, string title);

        [OperationContract(IsOneWay = false)]
        bool WriteSubmissionList(string teacher, string[] str);

        [OperationContract(IsOneWay = false)]
        ArrayList ReadSubmissionList(string teacher);

        [OperationContract(IsOneWay = false)]
        bool DeleteSumitSubmission(string id, string title);

        [OperationContract(IsOneWay = false)]
        bool WritePersonalStudentSubmissionXml(string teacher, string[] str);

        [OperationContract(IsOneWay = false)]
        ArrayList ReadPersonalStudentSubmissionXml(string teacher);

        [OperationContract(IsOneWay = false)]
        bool DeletePersonalStudentInfo(string id, string title);

        [OperationContract(IsOneWay = false)]
        bool BoardDeleteFile(string filename, int flag);

        [OperationContract(IsOneWay = false)]
        bool ReadSubmissionBoardWrite(string id, string title, string date);

        [OperationContract(IsOneWay = false)]
        bool ReadSubmissionBoardCheck(string id, string title, string date);

        //-------------- 파일 전송 관련 정의 ----------------
        [OperationContract]
        void DeleteFile(string virtualPath, int pathflag);

        [OperationContract]
        StorageFileInfo[] List(string virtualPath, int pathflag);

        //-------------------------//
        [OperationContract(IsOneWay = false)]
        ArrayList GetUploadedXmlInfo(int flag);

        //--------------- 학생 게시판 관련 정의 ------------------
        [OperationContract(IsOneWay = false)]
        bool StudentBoardWrite(string[] SubmissionInfo);

        [OperationContract(IsOneWay = false)]
        ArrayList StudentBoardRead();

        [OperationContract(IsOneWay = false)]
        bool DeleteStudentBoard(string id, string title);

        [OperationContract(IsOneWay = false)]
        bool ReadStudentBoardWrite(string id, string title, string date);

        [OperationContract(IsOneWay = false)]
        bool ReadStudentBoardCheck(string id, string title, string date);
        //------------------------//
        [OperationContract]
        void PutFile(FileUploadMessage msg);

        //------------------------//
        [OperationContract(IsOneWay = true)]
        void Memo(string id);

        [OperationContract(IsOneWay = true)]
        void MemoText(string id, string text);

        //--- 지우개 관련 메소드 ---//
        [OperationContract(IsOneWay = true)]
        void Erase(string type, string page, double left, double top, Point[] pts);

        //--- 스크롤 동기화 메소드 ---//
        [OperationContract(IsOneWay = true)]
        void SendScroll(double offset);

        //---------NoUSB------------//
        [OperationContract(IsOneWay = false)]
        bool NoUSBWrite(string[] noUSBinfo);

        [OperationContract(IsOneWay = false)]
        ArrayList NoUSBRead();

        //---파일전송---//
        [OperationContract(IsOneWay = true)]
        void SendRequest(string to, string file);
    }

    [ServiceContract]
    public interface ITransfer
    {
        [OperationContract()]
        void UploadFile(ResponseFile request);

        [OperationContract(IsOneWay = false)]
        Stream GetFile(string virtualPath, int pathflag);

        //다중 파일 다운로드(폴더)
        [OperationContract]
        ResponseMultiFile DownloadFile(RequestFile request);

        [OperationContract]
        ResponseMultiFile DownloadFiles(RequestFiles request);

        [OperationContract(IsOneWay = false)]
        long GetTotalSize(int pathflag);

        [OperationContract(IsOneWay = false)]
        long GetTotalFileSize(int pathflag, string[] filepath);
    }

    public interface ISIUCallback
    {
        [OperationContract(IsOneWay = true)]
        void Receive(string senderName, string message);

        [OperationContract(IsOneWay = true)]
        void UserEnter(string serial, string Name);

        [OperationContract]
        void Uploaded();

        [OperationContract(IsOneWay = true)]
        void PrepareReceive(string type, Color color, Point pt, int fontsize, string bookname);

        [OperationContract(IsOneWay = true)]
        void DrawReceive(string type, Color color, Point pt, int fontsize, string bookname);

        [OperationContract(IsOneWay = true)]
        void DrawEndReceive(string type, string id, string bookname);

        [OperationContract(IsOneWay = true)]
        void UserLeave(string name);

        [OperationContract(IsOneWay = true)]
        void ReceivePage(string page, string bookname);


        //--- 메모장 관련 콜백---//
        [OperationContract(IsOneWay = true)]
        void MemoReceive(string id, string bookname);

        [OperationContract(IsOneWay = true)]
        void MemoTextReceive(string id, string text, string bookname);

        //--- 지우개 관련 콜백---//
        [OperationContract(IsOneWay = true)]
        void EraseReceive(string type, string page, double left, double top, Point[] pts, string bookname);

        //--- 스크롤 동기화 메소드 ---//
        [OperationContract(IsOneWay = true)]
        void ReceiveScroll(double offset, string bookname);

        //--- 파일전송 ---//
        [OperationContract(IsOneWay = false)]
        void SendResponse(string sender, string file);
    }

    #region 3. 메세지 타입&이벤트Agrs정의

    //메세지 타입
    public enum MessageType
    {
        Receive, UserEnter, UserLeave, DrawPrepare, DrawReceive, DrawEnd, MemoReceive, MemoTextReceive,
        EraseReceive, ReceivePage, ReceiveScroll, Uploaded, SendResponse
    };

    //ChatEventArgs 이벤트 Arg에 포함될 항목
    public class ChatEventArgs : EventArgs
    {
        public MessageType msgType;
        public string serial;
        public string name;
        public string type;
        public string message;

        public double new_x;
        public double new_y;
        public double old_x;
        public double old_y;

        public double percent;

        public string page;
        public string bookname;
        public Stream data;

        /// <summary>
        /// 펜관련 이벤트 항목
        /// </summary>
        public Color color;
        public Point pt;
        public int fontsize;

        /// <summary>
        /// 메모 관련 항목
        /// </summary>
        public double rectleft;
        public double recttop;
        public double usertop;

        /// <summary>
        /// 지우개 관련 항목
        /// </summary>
        public Point[] pts;

        /// <summary>
        /// 스크롤 전송 오프셋
        /// </summary>
        public double offset;

        public string file;
    }

    #endregion

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    class SIUService : ISIU, ITransfer
    {
        //동기화 작업을 위해서 가상의 객체 생성
        private static Object syncObj = new Object();

        //콜백 델리게이트와 이벤트선언
        public delegate void ChatEventHandler(object sender, ChatEventArgs e);
        public static event ChatEventHandler ChatEvent;

        //키와 값을 가지고 있는 컬렉셔, Key=string 타입, 값은 ChatEventHandler를 가지고 있는다.
        static Dictionary<string, ChatEventHandler> chatters = new Dictionary<string, ChatEventHandler>();

        ISIUCallback callback = null;

        //키가 될 이름
        private string serial;
        private string type;
        private string bookname;
        //채팅 이벤트
        private ChatEventHandler myEventHandler = null;

        //파일 폴더 주소들..
        static string path = null;
        static string path0 = @"C:\SchoolInteligentUSB\UserPDFDirectory\";
        static string path1 = @"C:\SchoolInteligentUSB\BookRoomDirectory\";
        static string path2 = @"C:\SchoolInteligentUSB\FileRoomDirectory\";
        static string path3 = @"C:\SchoolInteligentUSB\StudentBoardDirectory\";
        static string path4 = @"C:\SchoolInteligentUSB\SubmissionDirectory\SugesstSubmissionList\";
        static string path5 = @"C:\SchoolInteligentUSB\SubmissionDirectory\SubmitSubmission\";
        static string path6 = @"C:\SchoolInteligentUSB\NoUSBDirectory\";
        static string path7 = @"C:\SchoolInteligentUSB\Transfer\";

        public string[] Join(string serial)
        {
            //자기 자신의 이벤트 핸들러 등록
            myEventHandler = new ChatEventHandler(MyEventHandler);

            lock (syncObj)
            {
                if (!chatters.ContainsKey(serial))//이름이 기존 채터에 있는지 검색한다.
                {
                    //이름과 이벤트를 추가한다.
                    this.serial = serial;
                    chatters.Add(serial, MyEventHandler);

                    //시리얼을 기반으로 타입 검색
                    this.type = Xml.Xml_FindType(serial);

                    //사용자에게 보내 줄 채널을 설정한다.
                    callback = OperationContext.Current.GetCallbackChannel<ISIUCallback>();

                    //UserEnter 라는 이벤트를 전달한다
                    ChatEventArgs e = new ChatEventArgs();
                    e.msgType = MessageType.UserEnter;
                    e.serial = serial;
                    e.name = Xml.Xml_FindName(serial);
                    BroadcastMessage(e);

                    //델리게이터 추가
                    ChatEvent += myEventHandler;

                    //사용자리스트를 보내준다.
                    string[] list = new string[chatters.Count];
                    lock (syncObj)
                    {
                        chatters.Keys.CopyTo(list, 0);
                    }
                    return list;
                }
                else //이미 사용자가 사용하고 있는 이름일 경우
                {
                    return null;
                }
            }
        }

        public void MyBook(string bookname)
        {
            this.bookname = bookname;
        }

        public string MyType(string serial)
        {
            //시리얼을 기반으로 타입 검색
            return this.type = Xml.Xml_FindType(serial);
        }

        public string MyName(string serial)
        {
            return Xml.Xml_FindName(serial);
        }

        private void BroadcastMessage(ChatEventArgs e)
        {
            //이벤트
            ChatEventHandler temp = ChatEvent;

            if (temp != null)
            {
                //현재 이벤트들을 전달한다.
                foreach (ChatEventHandler handler in temp.GetInvocationList())
                {
                    handler.BeginInvoke(this, e, new AsyncCallback(EndAsync), null);
                }
            }
        }

        private void EndAsync(IAsyncResult ar)
        {
            ChatEventHandler d = null;

            //비동기 메서드
            try
            {
                System.Runtime.Remoting.Messaging.AsyncResult asres = (System.Runtime.Remoting.Messaging.AsyncResult)ar;
                d = ((ChatEventHandler)asres.AsyncDelegate);
                d.EndInvoke(ar);
            }
            catch
            {
                ChatEvent -= d;
            }
        }

        private void MyEventHandler(object sender, ChatEventArgs e)
        {
            try
            {
                //클라이언트에게 보내기
                switch (e.msgType)
                {
                    case MessageType.Receive:
                        callback.Receive(e.serial, e.message);
                        break;
                    case MessageType.UserEnter:
                        //callback.UserEnter(Xml.Xml_FindName(e.serial));
                        callback.UserEnter(e.serial, e.name);
                        break;
                    case MessageType.DrawPrepare:
                        callback.PrepareReceive(e.type, e.color, e.pt, e.fontsize, e.bookname);
                        break;
                    case MessageType.DrawReceive:
                        callback.DrawReceive(e.type, e.color, e.pt, e.fontsize, e.bookname);
                        break;
                    case MessageType.DrawEnd:
                        callback.DrawEndReceive(e.type, e.serial, e.bookname);
                        break;
                    case MessageType.UserLeave:
                        callback.UserLeave(e.serial);
                        break;
                    case MessageType.MemoReceive:
                        callback.MemoReceive(e.serial, e.bookname);
                        break;
                    case MessageType.MemoTextReceive:
                        callback.MemoTextReceive(e.serial, e.message, e.bookname);
                        break;
                    case MessageType.EraseReceive:
                        callback.EraseReceive(e.type, e.page, e.rectleft, e.recttop, e.pts, e.bookname);
                        break;
                    case MessageType.SendResponse:
                        callback.SendResponse(e.serial, e.file);
                        break;
                    case MessageType.ReceivePage:
                        callback.ReceivePage(e.page, e.bookname);
                        break;
                    case MessageType.ReceiveScroll:
                        callback.ReceiveScroll(e.offset, e.bookname);
                        break;
                    case MessageType.Uploaded:
                        callback.Uploaded();
                        break;
                }
            }
            catch//에러가 발생했을 경우
            {
                Leave();
            }
        }

        #region 4.3. Say메서드
        public void Say(string msg)
        {
            ChatEventArgs e = new ChatEventArgs();
            e.msgType = MessageType.Receive;
            e.serial = this.serial;
            e.message = msg;
            BroadcastMessage(e);
        }
        #endregion

        #region 4.5. 방나가기
        public void Leave()
        {
            if (this.serial == null) return;

            lock (syncObj)
            {
                chatters.Remove(this.serial);
            }
            ChatEvent -= myEventHandler;

            //새로운 이벤트 발생
            ChatEventArgs e = new ChatEventArgs();
            e.msgType = MessageType.UserLeave;
            e.serial = this.serial;
            BroadcastMessage(e);
        }
        #endregion

        public bool Save(string serial, string id, string pw, string name, string sort, string number)
        {
            return Xml.Xml_Save(serial, id, pw, name, sort, number);
        }

        public bool Find(string serial)
        {
            return Xml.Xml_Find(serial);
        }

        public string FindName(string serial)
        {
            return Xml.Xml_FindName(serial);
        }

        public string FindType(string serial)
        {
            return Xml.Xml_FindType(serial);
        }

        public string FindUserInfo(string serial)
        {
            return Xml.Xml_FindUserInfo(serial);
        }

        public string FindtoID(string id)
        {
            return Xml.Xml_FindtoID(id);
        }

        public bool Rejoin(string serial, string id, string pw)
        {
            return Xml.Xml_ReJoin(serial, id, pw);
        }

        public bool Login(string id, string pw)
        {
            return Xml.Login(id, pw);
        }

        public void DrawPrepare(string type, Color color, Point pt, int fontsize)
        {
            if (this.type == "Teacher")
            {
                ChatEventArgs e = new ChatEventArgs();
                e.msgType = MessageType.DrawPrepare;
                e.pt = pt;
                e.color = color;
                e.type = type;
                e.fontsize = fontsize;
                e.bookname = this.bookname;

                BroadcastMessage(e);
            }
        }

        public void Draw(string type, Color color, Point pt, int fontsize)
        {
            if (this.type == "Teacher")
            {
                ChatEventArgs e = new ChatEventArgs();
                e.msgType = MessageType.DrawReceive;
                e.pt = pt;
                e.color = color;
                e.type = type;
                e.fontsize = fontsize;
                e.bookname = this.bookname;

                BroadcastMessage(e);
            }
        }

        public void DrawEnd(string type, string id)
        {
            if (this.type == "Teacher")
            {
                ChatEventArgs e = new ChatEventArgs();
                e.msgType = MessageType.DrawEnd;
                e.type = type;
                e.serial = id;
                e.bookname = this.bookname;

                BroadcastMessage(e);
            }
        }

        public void MovePage(string page)
        {
            if (this.type == "Teacher")
            {
                ChatEventArgs e = new ChatEventArgs();
                e.msgType = MessageType.ReceivePage;
                e.serial = this.serial;
                e.page = page;
                e.bookname = this.bookname;

                BroadcastMessage(e);
            }
        }

        public string[] Files(int nflag)
        {
            SetPath(nflag);

            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            FileInfo[] files = directoryInfo.GetFiles("*.*", SearchOption.TopDirectoryOnly);

            string[] file = files.Select<FileInfo, string>((FileInfo f) =>
            {
                string filedata = Path.GetFileName(f.FullName) + "\a"
                 + f.LastWriteTime.ToString();
                return filedata;
            }).ToArray<string>();

            return file;
        }

        public void Memo(string id)
        {
            if (this.type == "Teacher")
            {
                ChatEventArgs e = new ChatEventArgs();
                e.msgType = MessageType.MemoReceive;
                e.serial = id;
                e.bookname = this.bookname;

                BroadcastMessage(e);
            }
        }

        public void MemoText(string id, string text)
        {
            if (this.type == "Teacher")
            {
                ChatEventArgs e = new ChatEventArgs();
                e.msgType = MessageType.MemoTextReceive;
                e.serial = id;
                e.message = text;
                e.bookname = this.bookname;

                BroadcastMessage(e);
            }
        }

        public void Erase(string type, string page, double left, double top, Point[] pts)
        {
            if (this.type == "Teacher")
            {
                ChatEventArgs e = new ChatEventArgs();
                e.msgType = MessageType.EraseReceive;
                e.type = type;
                e.page = page;
                e.rectleft = left;
                e.recttop = top;
                e.pts = pts;
                e.bookname = this.bookname;

                BroadcastMessage(e);
            }
        }

        public void SendScroll(double offset)
        {
            if (this.type == "Teacher")
            {
                ChatEventArgs e = new ChatEventArgs();
                e.msgType = MessageType.ReceiveScroll;
                e.offset = offset;
                e.bookname = this.bookname;

                BroadcastMessage(e);
            }
        }

        #region 과제 관련 코드
        public bool WriteSugesstSubmissionList(string[] str)
        {
            return Xml.WriteSubmissionSugesstXml(str);
        }

        public ArrayList ReadSugesstSubmissionList()
        {
            ArrayList readlist = new ArrayList();
            readlist = Xml.ReadSubmissionSugesstXml();
            return readlist;
        }

        public bool DeleteSubmission(string id, string title)
        {
            return Xml.Xml_DeleteSubmission(id, title);
        }

        public bool WriteSubmissionList(string teacher, string[] str)
        {
            return Xml.WriteSubmitSubmissionXml(teacher, str);
        }

        public ArrayList ReadSubmissionList(string teacher)
        {
            ArrayList readlist = new ArrayList();
            readlist = Xml.ReadSubmitSubmissionXml(teacher);
            return readlist;
        }

        public bool DeleteSumitSubmission(string id, string title)
        {
            return Xml.Xml_DeleteSumitSubmission(id, title);
        }

        public bool WritePersonalStudentSubmissionXml(string teacher, string[] str)
        {
            return Xml.WritePersonalStudentSubmissionXml(teacher, str);
        }

        public ArrayList ReadPersonalStudentSubmissionXml(string teacher)
        {
            ArrayList readlist = new ArrayList();
            readlist = Xml.ReadPersonalStudentSubmissionXml(teacher);
            return readlist;
        }

        public bool DeletePersonalStudentInfo(string id, string title)
        {
            return Xml.Xml_DeletePersonalStudentInfo(id, title);
        }

        public bool ReadSubmissionBoardWrite(string id, string title, string date)
        {
            return Xml.ReadSubmissionBoardWrite(id, title, date);
        }

        public bool ReadSubmissionBoardCheck(string id, string title, string date)
        {
            return Xml.ReadSubmissionBoardCheck(id, title, date);
        }

        public bool BoardDeleteFile(string filename, int pathflag)
        {
            SetPath(pathflag);
            ArrayList filelist = new ArrayList();

            DirectoryInfo di = new DirectoryInfo(path);

            foreach (FileInfo file in di.GetFiles())
            {
                filelist.Add(file);
            }

            for (int i = 0; i < filelist.Count; i++)
            {
                string file = Convert.ToString(filelist[i]);
                string pullpath = path + file;
                if (filename == file)
                {
                    Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(pullpath,
                    Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs,
                    Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin);
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region 학생 게시판
        public bool StudentBoardWrite(string[] str)
        {
            return Xml.StudentBoardWrite(str);
        }

        public ArrayList StudentBoardRead()
        {
            ArrayList readlist = new ArrayList();
            readlist = Xml.StudentBoardRead();
            return readlist;
        }

        public bool ReadStudentBoardWrite(string id, string title, string date)
        {
            return Xml.ReadStudentBoardWrite(id, title, date);
        }

        public bool ReadStudentBoardCheck(string id, string title, string date)
        {
            return Xml.ReadStudentBoardCheck(id, title, date);
        }

        public bool DeleteStudentBoard(string id, string title)
        {
            return Xml.Xml_DeleteStudentBoard(id, title);
        }
        #endregion

        #region No USB Case
        public bool NoUSBWrite(string[] str)
        {
            return Xml.NoUSBWrite(str);
        }

        public ArrayList NoUSBRead()
        {
            ArrayList readlist = new ArrayList();
            readlist = Xml.NoUSBRead();
            return readlist;
        }

        #endregion

        public void FileRepositoryService()
        {
        }

        private void SetPath(int flag)
        {
            switch (flag)
            {
                case 0: path = path0; break;
                case 1: path = path1; break;
                case 2: path = path2; break;
                case 3: path = path3; break;
                case 4: path = path4; break;
                case 5: path = path5; break;
                case 6: path = path6; break;
                case 7: path = path7; break;
            }
        }

        public void DeleteFile(string virtualPath, int pathflag)
        {
            SetPath(pathflag);
            if (pathflag < 0)
            {
                if (totaldown > 0)
                    --totaldown;
            }

            if (totaldown == 0)
            {
                string str = Path.Combine(path, virtualPath);
                bool flag = !File.Exists(str);
                if (!flag)
                {
                    this.SendFileDeleted(virtualPath);
                    File.Delete(str);
                }
            }
        }

        public Stream GetFile(string virtualPath, int pathflag)
        {
            SetPath(pathflag);
            string str = Path.Combine(path, virtualPath);
            bool flag = File.Exists(str);
            if (!flag)
            {
                throw new FileNotFoundException("File was not found", Path.GetFileName(str));
            }
            this.SendFileRequested(virtualPath);
            Stream fileStream = new FileStream(str, FileMode.Open, FileAccess.Read);
            return fileStream;
        }

        public StorageFileInfo[] List(string virtualPath, int pathflag)
        {
            SetPath(pathflag);
            bool flag = string.IsNullOrEmpty(virtualPath);
            if (!flag)
            {
                path = Path.Combine(path, virtualPath);
            }
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
            StorageFileInfo[] array = files.Select<FileInfo, StorageFileInfo>((FileInfo f) =>
            {
                StorageFileInfo storageFileInfo = new StorageFileInfo();
                storageFileInfo.Size = f.Length;
                storageFileInfo.VirtualPath = f.FullName.Substring(f.FullName.IndexOf(path) + path.Length);
                storageFileInfo.Time = f.LastAccessTime;
                StorageFileInfo storageFileInfo1 = storageFileInfo;
                return storageFileInfo1;
            }).ToArray<StorageFileInfo>();
            return array;
        }

        public void PutFile(FileUploadMessage msg)
        {
            SetPath(msg.pathflag);

            string str = Path.Combine(path, msg.VirtualPath);
            string directoryName = Path.GetDirectoryName(str);
            bool flag = Directory.Exists(directoryName);
            if (!flag)
            {
                Directory.CreateDirectory(directoryName);
            }
            FileStream fileStream = new FileStream(str, FileMode.Create);
            try
            {
                msg.DataStream.CopyTo(fileStream);
            }
            finally
            {
                flag = fileStream == null;
                if (!flag)
                {
                    ((IDisposable)fileStream).Dispose();
                }
            }
        }

        public void UploadFile(ResponseFile request)
        {
            SetPath(request.pathflag);

            string filePath = path + request.FileName;

            int chunkSize = 2048;
            byte[] buffer = new byte[chunkSize];

            using (FileStream stream = new FileStream(filePath, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                do
                {
                    int readbyte = request.FileByteStream.Read(buffer, 0, chunkSize);

                    if (readbyte == 0) break;

                    stream.Write(buffer, 0, readbyte);
                } while (true);
                stream.Close();

                FileInfo file = new FileInfo(filePath);
                if (request.pathflag == 2)
                {
                    Xml.UploadedFileWrite(request.FileName, file.LastWriteTime.ToString());//업로드 파일 정보 기록
                    Uploaded();
                }
                else if (request.pathflag == 1)
                {
                    Xml.UploadedBookWrite(request.FileName, file.LastWriteTime.ToString());//업로드 파일 정보 기록
                    Uploaded();
                }
            }
        }

        public ArrayList GetUploadedXmlInfo(int flag)
        {
            if (flag == 2)
                return Xml.UploadedFileRead();
            else
                return Xml.UploadedBookRead();
        }

        public void Uploaded()
        {
            if (this.type == "Teacher")
            {
                //UserEnter 라는 이벤트를 전달한다
                ChatEventArgs e = new ChatEventArgs();
                e.msgType = MessageType.Uploaded;

                BroadcastMessage(e);
            }
        }
        static int totaldown = 0;

        public void SendRequest(string to, string file)
        {
            ChatEventArgs e = new ChatEventArgs();
            e.msgType = MessageType.SendResponse;
            e.serial = Xml.Xml_FindName(this.serial);
            e.file = file;

            try
            {
                ChatEventHandler chatterTo;
                lock (syncObj)
                {
                    chatterTo = chatters[to];
                }
                chatterTo.BeginInvoke(this, e, new AsyncCallback(EndAsync), null);

            }
            catch (KeyNotFoundException)
            {
                foreach (string serial in chatters.Keys)
                {
                    if (serial == this.serial)
                        continue;
                    ++totaldown;
                }

                foreach (ChatEventHandler ch in ChatEvent.GetInvocationList())
                {
                    if (chatters[this.serial] == ch)
                        continue;
                    ch.BeginInvoke(this, e, new AsyncCallback(EndAsync), null);
                }
            }
        }

        protected void SendFileDeleted(string vPath)
        {
            bool flag = this.FileDeleted == null;
            if (!flag)
            {
                this.FileDeleted(this, new FileEventArgs(vPath));
            }
        }

        protected void SendFileRequested(string vPath)
        {
            bool flag = this.FileRequested == null;
            if (!flag)
            {
                this.FileRequested(this, new FileEventArgs(vPath));
            }
        }

        protected void SendFileUploaded(string vPath)
        {
            bool flag = this.FileUploaded == null;
            if (!flag)
            {
                this.FileUploaded(this, new FileEventArgs(vPath));
            }
        }

        public event FileEventHandler FileDeleted;

        public event FileEventHandler FileRequested;

        public event FileEventHandler FileUploaded;

        #region 다중 파일 다운로드 프로그래스바
        public ResponseMultiFile DownloadFile(RequestFile request)
        {
            SetPath(request.pathflag);

            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);

            ResponseMultiFile[] result = new ResponseMultiFile[files.Count<FileInfo>()];

            FileStream stream = this.GetFileStream(Path.GetFullPath(files[request.idx].FullName));
            stream.Seek(request.byteStart, SeekOrigin.Begin);
            result[request.idx] = new ResponseMultiFile();
            result[request.idx].FileName = files[request.idx].FullName;
            result[request.idx].Length = stream.Length;
            result[request.idx].FileByteStream = stream;

            ResponseMultiFile retval = result[request.idx];

            return retval;
        }

        public ResponseMultiFile DownloadFiles(RequestFiles request)
        {
            // DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\Users\77\Desktop\PDF");
            //   FileInfo[] files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);

            SetPath(request.pathflag);

            FileInfo[] files = new FileInfo[request.FilePath.Count];

            for (int i = 0; i < request.FilePath.Count; i++)
            {
                files[i] = (new FileInfo(Path.Combine(path, request.FilePath[i].ToString())));
            }

            ResponseMultiFile[] result = new ResponseMultiFile[files.Count<FileInfo>()];

            FileStream stream = this.GetFileStream(Path.GetFullPath(files[request.idx].FullName));
            stream.Seek(request.byteStart, SeekOrigin.Begin);
            result[request.idx] = new ResponseMultiFile();
            result[request.idx].FileName = files[request.idx].FullName;
            result[request.idx].Length = stream.Length;
            result[request.idx].FileByteStream = stream;

            ResponseMultiFile retval = result[request.idx];

            return retval;
        }

        public long GetTotalSize(int pathflag)
        {
            SetPath(pathflag);
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            {
                if (directoryInfo.Exists != true)
                    directoryInfo.Create();
            }
            FileInfo[] files = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories);

            long size = 0;
            string file = "";
            foreach (FileInfo f in files)
            {
                size += f.Length;
                file += f.Name;
            }

            return size;
        }

        public long GetTotalFileSize(int pathflag, string[] filepath)
        {
            SetPath(pathflag);
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            {
                if (directoryInfo.Exists != true)
                    directoryInfo.Create();
            }
            FileInfo[] files = new FileInfo[filepath.Count()];

            for (int i = 0; i < filepath.Count(); i++)
            {
                files[i] = (new FileInfo(Path.Combine(path, filepath[i].ToString())));
            }

            long size = 0;
            string file = "";
            foreach (FileInfo f in files)
            {
                size += f.Length;
                file += f.Name;
            }

            return size;
        }

        private FileStream GetFileStream(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);

            if (!fileInfo.Exists)
                throw new FileNotFoundException("File not found");

            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }
        #endregion
    }
}
