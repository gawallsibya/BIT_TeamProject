using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace UserWallPaper
{
    public static class EventWait
    {
        static private EventWaitHandle ewh = new EventWaitHandle(false, EventResetMode.AutoReset);

        static public void Set()
        {
            ewh.Set();
        }

        static public void WaitOne()
        {
            ewh.WaitOne();
        }
    }
}
