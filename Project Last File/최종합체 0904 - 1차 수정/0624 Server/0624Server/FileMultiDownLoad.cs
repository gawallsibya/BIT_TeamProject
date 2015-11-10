using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace _0624Server
{
    [MessageContract]
    public class RequestFile
    {
        [MessageBodyMember]
        public string FileName;

        [MessageBodyMember]
        public long byteStart = 0;

        [MessageHeader]
        public int idx = 0;

        [MessageHeader]
        public int pathflag
        {
            get;
            set;
        }
    }

    [MessageContract]
    public class RequestFiles
    {
        [MessageBodyMember]
        public string FileName;

        [MessageHeader]
        public ArrayList FilePath;
        
        [MessageHeader]
        public int pathflag;

        [MessageBodyMember]
        public long byteStart = 0;

        [MessageHeader]
        public int idx = 0;
    }


    [MessageContract]
    public class ResponseMultiFile : IDisposable
    {
        [MessageHeader]
        public string FileName;

        [MessageHeader]
        public long Length;

        [MessageBodyMember]
        public System.IO.Stream FileByteStream;

        [MessageHeader]
        public long byteStart = 0;

        public void Dispose()
        {
            if (FileByteStream != null)
            {
                FileByteStream.Close();
                FileByteStream = null;
            }
        }
    }
}
