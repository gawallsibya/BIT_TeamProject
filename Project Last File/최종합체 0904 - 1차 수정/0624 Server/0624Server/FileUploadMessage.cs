using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace _0624Server
{
    [MessageContract]
    public class FileUploadMessage
    {
        [MessageBodyMember(Order = 1)]
        public Stream DataStream
        {
            get;
            set;
        }

        [MessageHeader(MustUnderstand = true)]
        public string VirtualPath
        {
            get;
            set;
        }

        [MessageHeader]
        public int pathflag
        {
            get;
            set;
        }

        [MessageHeader]
        public long Length
        {
            get;
            set;
        }

        public FileUploadMessage()
        {
        }
    }

    [MessageContract]
    public class ResponseFile : IDisposable
    {
        [MessageHeader]
        public string FileName;

        [MessageHeader]
        public long Length;

        [MessageBodyMember]
        public System.IO.Stream FileByteStream;

        [MessageHeader]
        public long byteStart = 0;

        [MessageHeader]
        public int pathflag;

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
