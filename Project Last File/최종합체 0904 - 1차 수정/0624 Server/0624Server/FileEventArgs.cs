using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _0624Server
{
    public class FileEventArgs : EventArgs
    {
        private string _VirtualPath = null;

        public string VirtualPath
        {
            get
            {
                string str = this._VirtualPath;
                return str;
            }
        }

        public FileEventArgs(string vPath)
        {
            this._VirtualPath = vPath;
        }
    }
}
