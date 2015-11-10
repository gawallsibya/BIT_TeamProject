using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace UserWallPaper
{
    internal class CopyFileExWrapper
    {
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CopyFileEx(string lpExistingFileName, string lpNewFileName,
           CopyProgressRoutine lpProgressRoutine, IntPtr lpData, ref bool pbCancel,
           CopyFileFlags dwCopyFlags);

        private delegate CopyProgressResult CopyProgressRoutine(long TotalFileSize, long TotalBytesTransferred,
            long StreamSize, long StreamBytesTransferred, uint dwStreamNumber, CopyProgressCallbackReason dwCallbackReason,
            IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData);

        [Flags]
        private enum CopyFileFlags : uint
        {
            COPY_FILE_FAIL_IF_EXISTS = 0x00000001,
            COPY_FILE_RESTARTABLE = 0x00000002,
            COPY_FILE_OPEN_SOURCE_FOR_WRITE = 0x00000004,
            COPY_FILE_ALLOW_DECRYPTED_DESTINATION = 0x00000008,
            COPY_FILE_COPY_SYMLINK = 0x00000800 //NT 6.0+
        }

        private enum CopyProgressResult : uint
        {
            PROGRESS_CONTINUE = 0,
            PROGRESS_CANCEL = 1,
            PROGRESS_STOP = 2,
            PROGRESS_QUIET = 3
        }

        private enum CopyProgressCallbackReason : uint
        {
            CALLBACK_CHUNK_FINISHED = 0x00000000,
            CALLBACK_STREAM_SWITCH = 0x00000001
        }

        public delegate void CopyEventHandler(CopyFileExWrapper sender, CopyEventArgs e);
        public event CopyEventHandler EventCopyHandler;
        public delegate void UpdateTextBlockDelegate(string strMessage);
        public event UpdateTextBlockDelegate UpdateTextBlock;

        
        private TimeSpan m_dtElapsedTime;
        private DateTime m_dtStartTime;
        private bool m_bCancel;

        public void CopyFiles(List<string> lstSource, string selectedpath, string strDest)
        {
            m_bCancel = false;

            for (int nIndex = 0; nIndex < lstSource.Count; nIndex++)
            {
                UpdateTextBlock("Copying " + (nIndex + 1).ToString() + " of " + lstSource.Count + " files");
                //m_strDestFile = strDest + Path.GetFileName(lstSource[nIndex]);
                m_dtStartTime = DateTime.Now;
                bool bSuccess = CopyFileEx(lstSource[nIndex], lstSource[nIndex].Replace(selectedpath, strDest), new CopyProgressRoutine(CopyProgressHandler), IntPtr.Zero, ref m_bCancel, CopyFileFlags.COPY_FILE_RESTARTABLE);

                if (!bSuccess)
                {
                    int error = Marshal.GetLastWin32Error();
                    throw new Win32Exception(error);
                }
            }

            UpdateTextBlock("Copying successfully");
        }

        private CopyProgressResult CopyProgressHandler(long total, long transferred, long streamSize, long StreamByteTrans, uint dwStreamNumber, CopyProgressCallbackReason reason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData)
        {
            switch (reason)
            {
                case CopyProgressCallbackReason.CALLBACK_CHUNK_FINISHED:
                    m_dtElapsedTime = DateTime.Now - m_dtStartTime;
                    EventCopyHandler(this, new CopyEventArgs(((double)transferred / total) * 100, (transferred / 1024) / m_dtElapsedTime.TotalSeconds));
                    return m_bCancel ? CopyProgressResult.PROGRESS_CANCEL : CopyProgressResult.PROGRESS_CONTINUE;
                default:
                    return CopyProgressResult.PROGRESS_CONTINUE;
            }
        }

        //public event EventHandler ChipChanged;

        public bool Cancel
        {
            get { return m_bCancel; }
            set { m_bCancel = value; }
        }
    }
}