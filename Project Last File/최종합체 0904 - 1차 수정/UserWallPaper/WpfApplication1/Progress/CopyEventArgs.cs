using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserWallPaper
{
    class CopyEventArgs : EventArgs
    {
        public CopyEventArgs(double fPercent, double dTimeToTransfer)
        {
            Percent = fPercent;
            TimeToTransfer = dTimeToTransfer;
        }

        public double Percent { get; private set; }
        public double TimeToTransfer { get; private set; }
    }
}
