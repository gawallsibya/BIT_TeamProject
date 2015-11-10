using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;
using System.Windows;
using System.IO;

namespace UserWallPaper
{
    class WindowsXml
    {
        private XmlDocument USER = new XmlDocument();
        private XmlElement USERElement = null;
        private string[] userlist;
        private string usbpath = (string)System.Windows.Application.Current.Properties["USBDrivePath"];

        public void WindowsWriteXml(ArrayList IconOder)
        {
            CreateFloder(@"SchoolInteligentUSB\UserWindowXml");
            string SaveFilePath = usbpath + @"SchoolInteligentUSB\UserWindowXml\WindowStyle.xml";

            try
            {
                USER.Load(SaveFilePath);
                USER.RemoveAll();
                USERElement = USER.CreateElement("WindowsStyle");
            }
            catch
            {
                USERElement = USER.CreateElement("WindowsStyle");
            }

            XmlElement UserIconElement = USER.CreateElement("Order");

            XmlAttribute StockDummyAttribute = USER.CreateAttribute("OderUser");
            UserIconElement.SetAttributeNode(StockDummyAttribute);
            StockDummyAttribute.Value = "User";

            XmlElement[] usericonoder = new XmlElement[IconOder.Count];

            for (int i = 0; i < IconOder.Count; i++)
            {
                usericonoder[i] = USER.CreateElement("test" + i);
                usericonoder[i].InnerText = IconOder[i].ToString();
                UserIconElement.AppendChild(usericonoder[i]);
            }

            USERElement.AppendChild(UserIconElement);

            USER.AppendChild(USERElement);
            USER.Save(SaveFilePath);
        }

        public string[] WindowsLoadXml()
        {
            CreateFloder(@"SchoolInteligentUSB\UserWindowXml");
            string SaveFilePath = usbpath + @"SchoolInteligentUSB\UserWindowXml\WindowStyle.xml";

            try
            {
                USER.Load(SaveFilePath);

                XmlNodeList UserOderList = USER.GetElementsByTagName("Order");

                foreach (XmlNode node in UserOderList)
                {
                    XmlElement UserIconElement = (XmlElement)node;

                    int countnode = node.ChildNodes.Count;

                    userlist = new string[countnode];

                    if (UserIconElement.HasAttributes)
                    {
                        for (int i = 0; i < node.ChildNodes.Count; i++)
                        {
                            userlist[i] = UserIconElement.GetElementsByTagName("test" + i)[0].InnerText;
                        }
                    }
                }

                return userlist;
            }
            catch
            {
                return null;
            }
        }
        
        #region NoUsbDownLoadXML
        private XmlDocument xmlDoc;
        private XmlElement MembersElement = null;
        static ArrayList noUSB = new ArrayList();
        public void NoUsbDownLoadXML_Write(string noUSBinfo)
        {
            string noUSBlist = usbpath + @"SchoolInteligentUSB\UserWindowXml\NoUsbDownLoadXML.xml";
            xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(noUSBlist);

                XmlNodeList SubmissionList = xmlDoc.GetElementsByTagName("NoUSBlist");
                MembersElement = (XmlElement)SubmissionList[0];
            }
            catch
            {
                MembersElement = xmlDoc.CreateElement("NoUSBlist");
            }

            XmlElement MainElement = xmlDoc.CreateElement("NoUSBinfo");

            XmlAttribute NoUSBAttribute = xmlDoc.CreateAttribute("DownFile");
            MainElement.SetAttributeNode(NoUSBAttribute);
            NoUSBAttribute.Value = noUSBinfo;

            MembersElement.AppendChild(MainElement);

            xmlDoc.AppendChild(MembersElement);
            xmlDoc.Save(noUSBlist);
        }

        public ArrayList NoUsbDownLoadXML_Read()
        {
            xmlDoc = new XmlDocument();
            string noUSBlist = usbpath + @"SchoolInteligentUSB\UserWindowXml\NoUsbDownLoadXML.xml";
            try
            {
                xmlDoc.Load(noUSBlist);

                XmlNodeList SubmissionList = xmlDoc.GetElementsByTagName("NoUSBlist");
                MembersElement = (XmlElement)SubmissionList[0];
            }
            catch
            {
                MembersElement = xmlDoc.CreateElement("NoUSBlist");
            }

            string noUSBinfo = null;

            XmlNodeList MainNode = xmlDoc.GetElementsByTagName("NoUSBinfo");

            foreach (XmlNode node in MainNode)
            {
                XmlElement NoUSBinfoListInfoElement = (XmlElement)node;

                if (NoUSBinfoListInfoElement.HasAttributes)
                {
                    noUSBinfo = NoUSBinfoListInfoElement.Attributes["DownFile"].InnerText;

                    NoUSBAllData(noUSBinfo);
                }
            }
            return noUSB;
        }


        static public void NoUSBAllData(string info)
        {
            noUSB.Add(info);
        }
        #endregion

        private XmlDocument wallpaper = new XmlDocument();
        private XmlElement wallpaperElement = null;

        public void WallpaperWrite(string userpath)
        {
            CreateFloder(@"SchoolInteligentUSB\UserWindowXml");
            string SaveFilePath = usbpath + @"SchoolInteligentUSB\UserWindowXml\WindowWallpaper.xml";

            try
            {
                wallpaper.Load(SaveFilePath);
                wallpaper.RemoveAll();
                wallpaperElement = wallpaper.CreateElement("WindowWallpaper");
            }
            catch
            {
                wallpaperElement = wallpaper.CreateElement("WindowWallpaper");
            }

            //XmlElement UserIconElement = wallpaper.CreateElement("Optioncheck");

            ////XmlAttribute StockDummyAttribute = wallpaper.CreateAttribute("Option");
            ////UserIconElement.SetAttributeNode(StockDummyAttribute);
            ////StockDummyAttribute.Value = "UserOption";

            //XmlAttribute ExeNameAttribute = wallpaper.CreateAttribute("Option");
            //UserIconElement.SetAttributeNode(ExeNameAttribute);
            //ExeNameAttribute.Value = "UserOption";

            //XmlElement PathElement = wallpaper.CreateElement("Title");
            //PathElement.InnerText = userpath;
            //wallpaperElement.AppendChild(PathElement);

            XmlElement MainElement = wallpaper.CreateElement("Optioncheck");

            XmlAttribute wallpaperAttribute = wallpaper.CreateAttribute("Option");
            MainElement.SetAttributeNode(wallpaperAttribute);
            wallpaperAttribute.Value = "UserOption";

            XmlElement PathElement = wallpaper.CreateElement("NamePath");
            PathElement.InnerText = userpath;
            MainElement.AppendChild(PathElement);

            wallpaperElement.AppendChild(MainElement);

            wallpaper.AppendChild(wallpaperElement);
            wallpaper.Save(SaveFilePath);
        }

        public string WallpaperLoad()
        {
            CreateFloder(@"SchoolInteligentUSB\UserWindowXml");
            string SaveFilePath = usbpath + @"SchoolInteligentUSB\UserWindowXml\WindowWallpaper.xml";

            try
            {
                wallpaper.Load(SaveFilePath);

                XmlNodeList UserOderList = wallpaper.GetElementsByTagName("Optioncheck");

                string optionselect = null;

                foreach (XmlNode node in UserOderList)
                {
                    XmlElement UserIconElement = (XmlElement)node;

                    int countnode = node.ChildNodes.Count;

                    if (UserIconElement.HasAttributes)
                    {
                        for (int i = 0; i < node.ChildNodes.Count; i++)
                        {
                            optionselect = UserIconElement.GetElementsByTagName("NamePath")[0].InnerText;
                        }
                    }
                }

                return optionselect;
            }
            catch
            {
                return null;
            }
        }

        public void CreateFloder(string path)
        {
            string directorypath = usbpath + path;

            if (Directory.Exists(directorypath) == false)
            {
                DirectoryInfo di = Directory.CreateDirectory(directorypath);
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }
        }
    }
}
