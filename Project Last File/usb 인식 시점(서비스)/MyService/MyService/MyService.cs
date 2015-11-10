using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;

namespace MyService
{
    public partial class MyService : ServiceBase
    {
        public MyService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            DeviceChangeNotifier.Start();
        }

        protected override void OnStop()
        {
            DeviceChangeNotifier.Stop();
        }
    }
    public class DeviceChangeNotifier : Form
    {
        public delegate void DeviceNotifyDelegate(Message msg);
        private static DeviceChangeNotifier mInstance;
        public delegate void DelegateGetDrives(bool falg);
        public delegate void DelegateKillProcess();
        public delegate void DelegateRun(string serial);
        private static DelegateRun mRun;

        public static void Start()
        {
            Thread t = new Thread(runForm);
            t.SetApartmentState(ApartmentState.STA);
            t.IsBackground = true;
            t.Start();
        }
        public static void Stop()
        {
            if (mInstance == null) throw new InvalidOperationException("Notifier not started");
            mInstance.Invoke(new MethodInvoker(mInstance.endForm));
        }
        private static void runForm()
        {
            Application.Run(new DeviceChangeNotifier());
        }

        private void endForm()
        {
            this.Close();
        }
        protected override void SetVisibleCore(bool value)
        {
            // Prevent window getting visible
            if (mInstance == null) CreateHandle();
            mInstance = this;
            value = false;
            mRun = Run;
            base.SetVisibleCore(value);
        }

        static public void RefreshDevice(bool flag)
        {
            string path = @"c:\" + DateTime.Now.Date.ToString("yy/MM/dd") + ".txt";

            if (flag)
            {
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                foreach (DriveInfo d in allDrives)
                {
                    if (d.IsReady == true)
                    {
                        if (d.DriveType.ToString() == "Removable")
                        {
                            string[] number = d.Name.Split('\\');
                            USBSerialNumber usb = new USBSerialNumber();
                            string serial = usb.getSerialNumberFromDriveLetter(number[0]);
                            mRun(serial);
                        }
                    }
                }
            }
            else
            {

            }
        }

        public void ProcessKill()
        {
            Process[] processes = Process.GetProcessesByName("UserWallPaper");
            Process currentProcess = Process.GetCurrentProcess();
            foreach (Process proc in processes)
            {
                if (proc.Id != currentProcess.Id)
                    proc.Kill();
            }
        }

        protected override void WndProc(ref Message m)
        {
            UInt32 WM_DEVICECHANGE = 0x0219;
            UInt32 DBT_DEVTUP_VOLUME = 0x02;
            UInt32 DBT_DEVICEARRIVAL = 0x8000;
            UInt32 DBT_DEVICEREMOVECOMPLETE = 0x8004;

            if ((m.Msg == WM_DEVICECHANGE) && (m.WParam.ToInt32() == DBT_DEVICEARRIVAL)) //디바이스 연결
            {
                int devType = Marshal.ReadInt32(m.LParam, 4);

                if (devType == DBT_DEVTUP_VOLUME)
                {
                    
                    DelegateGetDrives gdi = new DelegateGetDrives(RefreshDevice);
                    IAsyncResult result = gdi.BeginInvoke(true, null, "");
                    gdi.EndInvoke(result);

                    //string path = @"C:\Users\m\Desktop\프로젝트\2차 시연\UserWallPaper\WpfApplication1\bin\Debug\UserWallPaper.exe";
                    //Process.Start(path);

                    //Run();
                }
            }

            if ((m.Msg == WM_DEVICECHANGE) && (m.WParam.ToInt32() == DBT_DEVICEREMOVECOMPLETE)) //디바이스 연결 해제
            {
                int devType = Marshal.ReadInt32(m.LParam, 4);

                if (devType == DBT_DEVTUP_VOLUME)
                {
                    //RefreshDevice(false);
                    DelegateKillProcess kp = new DelegateKillProcess(ProcessKill);
                    IAsyncResult result = kp.BeginInvoke(null, "");
                    kp.EndInvoke(result);
                }
            }
            base.WndProc(ref m);
        }

        #region 세션
        private static uint STANDARD_RIGHTS_REQUIRED = 0x000F0000;
        private static uint STANDARD_RIGHTS_READ = 0x00020000;
        private static uint TOKEN_ASSIGN_PRIMARY = 0x0001;
        private static uint TOKEN_DUPLICATE = 0x0002;
        private static uint TOKEN_IMPERSONATE = 0x0004;
        private static uint TOKEN_QUERY = 0x0008;
        private static uint TOKEN_QUERY_SOURCE = 0x0010;
        private static uint TOKEN_ADJUST_PRIVILEGES = 0x0020;
        private static uint TOKEN_ADJUST_GROUPS = 0x0040;
        private static uint TOKEN_ADJUST_DEFAULT = 0x0080;
        private static uint TOKEN_ADJUST_SESSIONID = 0x0100;
        private static uint TOKEN_READ = (STANDARD_RIGHTS_READ | TOKEN_QUERY);
        private static uint TOKEN_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | TOKEN_ASSIGN_PRIMARY | TOKEN_DUPLICATE | TOKEN_IMPERSONATE | TOKEN_QUERY | TOKEN_QUERY_SOURCE | TOKEN_ADJUST_PRIVILEGES | TOKEN_ADJUST_GROUPS | TOKEN_ADJUST_DEFAULT | TOKEN_ADJUST_SESSIONID);

        private const uint NORMAL_PRIORITY_CLASS = 0x0020;

        private const uint CREATE_UNICODE_ENVIRONMENT = 0x00000400;


        private const uint MAX_PATH = 260;

        private const uint CREATE_NO_WINDOW = 0x08000000;

        private const uint INFINITE = 0xFFFFFFFF;

        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            public int bInheritHandle;
        }

        public enum SECURITY_IMPERSONATION_LEVEL
        {
            SecurityAnonymous,
            SecurityIdentification,
            SecurityImpersonation,
            SecurityDelegation
        }

        public enum TOKEN_TYPE
        {
            TokenPrimary = 1,
            TokenImpersonation
        }

        public enum WTS_CONNECTSTATE_CLASS
        {
            WTSActive,
            WTSConnected,
            WTSConnectQuery,
            WTSShadow,
            WTSDisconnected,
            WTSIdle,
            WTSListen,
            WTSReset,
            WTSDown,
            WTSInit
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct STARTUPINFO
        {
            public Int32 cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwYSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public Int32 dwFlags;
            public Int16 wShowWindow;
            public Int16 cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct WTS_SESSION_INFO
        {
            public Int32 SessionID;

            [MarshalAs(UnmanagedType.LPStr)]
            public String pWinStationName;

            public WTS_CONNECTSTATE_CLASS State;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern uint WTSGetActiveConsoleSessionId();

        [DllImport("wtsapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool WTSQueryUserToken(int sessionId, out IntPtr tokenHandle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool DuplicateTokenEx(IntPtr existingToken, uint desiredAccess, IntPtr tokenAttributes, SECURITY_IMPERSONATION_LEVEL impersonationLevel, TOKEN_TYPE tokenType, out IntPtr newToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool CreateProcessAsUser(IntPtr token, string applicationName, string commandLine, ref SECURITY_ATTRIBUTES processAttributes, ref SECURITY_ATTRIBUTES threadAttributes, bool inheritHandles, uint creationFlags, IntPtr environment, string currentDirectory, ref STARTUPINFO startupInfo, out PROCESS_INFORMATION processInformation);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetLastError();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int WaitForSingleObject(IntPtr token, uint timeInterval);

        [DllImport("wtsapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int WTSEnumerateSessions(System.IntPtr hServer, int Reserved, int Version, ref System.IntPtr ppSessionInfo, ref int pCount);

        [DllImport("userenv.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool CreateEnvironmentBlock(out IntPtr lpEnvironment, IntPtr hToken, bool bInherit);

        [DllImport("wtsapi32.dll", ExactSpelling = true, SetLastError = false)]
        public static extern void WTSFreeMemory(IntPtr memory);

        [DllImport("userenv.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DestroyEnvironmentBlock(IntPtr lpEnvironment);


        private PROCESS_INFORMATION processInfo_;

        public static IntPtr GetCurrentUserToken()
        {
            IntPtr currentToken = IntPtr.Zero;
            IntPtr primaryToken = IntPtr.Zero;
            IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;

            int dwSessionId = 0;
            IntPtr hUserToken = IntPtr.Zero;
            IntPtr hTokenDup = IntPtr.Zero;

            IntPtr pSessionInfo = IntPtr.Zero;
            int dwCount = 0;

            WTSEnumerateSessions(WTS_CURRENT_SERVER_HANDLE, 0, 1, ref pSessionInfo, ref dwCount);

            Int32 dataSize = Marshal.SizeOf(typeof(WTS_SESSION_INFO));

            Int32 current = (int)pSessionInfo;
            for (int i = 0; i < dwCount; i++)
            {
                WTS_SESSION_INFO si = (WTS_SESSION_INFO)Marshal.PtrToStructure((System.IntPtr)current, typeof(WTS_SESSION_INFO));
                if (WTS_CONNECTSTATE_CLASS.WTSActive == si.State)
                {
                    dwSessionId = si.SessionID;
                    break;
                }

                current += dataSize;
            }

            WTSFreeMemory(pSessionInfo);

            bool bRet = WTSQueryUserToken(dwSessionId, out currentToken);
            if (bRet == false)
            {
                return IntPtr.Zero;
            }

            bRet = DuplicateTokenEx(currentToken, TOKEN_ASSIGN_PRIMARY | TOKEN_ALL_ACCESS, IntPtr.Zero, SECURITY_IMPERSONATION_LEVEL.SecurityImpersonation, TOKEN_TYPE.TokenPrimary, out primaryToken);
            if (bRet == false)
            {
                return IntPtr.Zero;
            }

            return primaryToken;
        }

        public void Run(string serial)
        {

            IntPtr primaryToken = GetCurrentUserToken();
            if (primaryToken == IntPtr.Zero)
            {
                return;
            }
            STARTUPINFO StartupInfo = new STARTUPINFO();
            processInfo_ = new PROCESS_INFORMATION();
            StartupInfo.cb = Marshal.SizeOf(StartupInfo);

            SECURITY_ATTRIBUTES Security1 = new SECURITY_ATTRIBUTES();
            SECURITY_ATTRIBUTES Security2 = new SECURITY_ATTRIBUTES();

            //string command = "\"" + processPath_ + "\"";
            //string command = Application.StartupPath + "\\EDMLogin.exe";
            string command = @"E:\5.MyFile\Project Last File\최종합체 0904 - 1차 수정\UserWallPaper\WpfApplication1\bin\Debug\UserWallPaper.exe";
            command += " " + serial;
            //string command = "C:\\EDMLoginTest\\EDMLogin.exe";
            //if ((arguments_ != null) && (arguments_.Length != 0))
            //{
            //    command += " " + arguments_;
            //}

            IntPtr lpEnvironment = IntPtr.Zero;
            bool resultEnv = CreateEnvironmentBlock(out lpEnvironment, primaryToken, false);
            if (resultEnv != true)
            {
                int nError = GetLastError();
            }

            CreateProcessAsUser(primaryToken, null, command, ref Security1, ref Security2, false, CREATE_NO_WINDOW | NORMAL_PRIORITY_CLASS | CREATE_UNICODE_ENVIRONMENT, lpEnvironment, null, ref StartupInfo, out processInfo_);

            DestroyEnvironmentBlock(lpEnvironment);
            CloseHandle(primaryToken);
        }
        #endregion
    }

    public class USBSerialNumber
    {

        string _serialNumber;
        string _driveLetter;

        public string getSerialNumberFromDriveLetter(string driveLetter)
        {
            this._driveLetter = driveLetter.ToUpper();

            if (!this._driveLetter.Contains(":"))
            {
                this._driveLetter += ":";
            }

            matchDriveLetterWithSerial();

            return this._serialNumber;
        }

        private void matchDriveLetterWithSerial()
        {

            string[] diskArray;
            string driveNumber;
            string driveLetter;

            ManagementObjectSearcher searcher1 = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDiskToPartition");

            foreach (ManagementObject dm in searcher1.Get())
            {
                diskArray = null;
                driveLetter = getValueInQuotes(dm["Dependent"].ToString());
                diskArray = getValueInQuotes(dm["Antecedent"].ToString()).Split(',');
                driveNumber = diskArray[0].Remove(0, 6).Trim();
                if (driveLetter == this._driveLetter)
                {
                    /* This is where we get the drive serial */
                    ManagementObjectSearcher disks = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                    foreach (ManagementObject disk in disks.Get())
                    {

                        if (disk["Name"].ToString() == ("\\\\.\\PHYSICALDRIVE" + driveNumber) & disk["InterfaceType"].ToString() == "USB")
                        {
                            this._serialNumber = parseSerialFromDeviceID(disk["PNPDeviceID"].ToString());
                        }
                    }
                }
            }



        }

        private string parseSerialFromDeviceID(string deviceId)
        {
            string[] splitDeviceId = deviceId.Split('\\');
            string[] serialArray;
            string serial;
            int arrayLen = splitDeviceId.Length - 1;

            serialArray = splitDeviceId[arrayLen].Split('&');
            serial = serialArray[0];

            return serial;
        }

        private string getValueInQuotes(string inValue)
        {
            string parsedValue = "";

            int posFoundStart = 0;
            int posFoundEnd = 0;

            posFoundStart = inValue.IndexOf("\"");
            posFoundEnd = inValue.IndexOf("\"", posFoundStart + 1);

            parsedValue = inValue.Substring(posFoundStart + 1, (posFoundEnd - posFoundStart) - 1);

            return parsedValue;
        }
    }
    
}
