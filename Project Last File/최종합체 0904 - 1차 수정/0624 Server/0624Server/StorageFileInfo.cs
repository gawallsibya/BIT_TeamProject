using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0624Server
{
    [Serializable]
    public class StorageFileInfo
    {
        public long Size
        {
            get;
            set;
        }

        public DateTime Time
        {
            get;
            set;
        }

        public string VirtualPath
        {
            get;
            set;
        }

        public StorageFileInfo()
        {
        }
    }
}
