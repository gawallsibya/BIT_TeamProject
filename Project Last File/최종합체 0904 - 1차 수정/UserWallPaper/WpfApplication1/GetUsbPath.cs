using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace UserWallPaper
{
    class GetUsbPath
    {
        public string GetUsbDriverName()
        {
            string usbpath = null;

            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady == true)
                {
                    if (d.DriveType.ToString() == "Removable")
                    {
                        usbpath = d.Name;
                    }
                }
            }

            return usbpath;
        }
    }
}
