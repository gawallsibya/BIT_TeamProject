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
    class UserLinkListXML
    {
        private XmlDocument linklist = new XmlDocument();
        private XmlElement linklistElement = null;
        private string usbpath = (string)System.Windows.Application.Current.Properties["USBDrivePath"];
        private static ArrayList UserLink = new ArrayList();

        public void Load()
        {
            CreateFloder(@"SchoolInteligentUSB\UserWindowXml");
            string SaveFilePath = usbpath + @"SchoolInteligentUSB\UserWindowXml\UserLinkList.xml";

            try
            {
                linklist.Load(SaveFilePath);

                XmlNodeList UserLinkList = linklist.GetElementsByTagName("LinkList");
                linklistElement = (XmlElement)UserLinkList[0];
            }
            catch
            {
                linklistElement = linklist.CreateElement("LinkList");

                XmlElement ListElement = linklist.CreateElement("List");

                XmlAttribute ExeNameAttribute = linklist.CreateAttribute("EXE_Name");
                ListElement.SetAttributeNode(ExeNameAttribute);
                ExeNameAttribute.Value = "Internet Explorer";

                XmlElement TitleElement = linklist.CreateElement("Title");
                TitleElement.InnerText = "Internet Explorer";
                ListElement.AppendChild(TitleElement);

                XmlElement PathElement = linklist.CreateElement("PULLPATH");
                PathElement.InnerText = "C:\\Program Files\\Internet Explorer\\iexplore.exe";
                ListElement.AppendChild(PathElement);

                linklistElement.AppendChild(ListElement);

                linklist.AppendChild(linklistElement);
                linklist.Save(SaveFilePath);
            }
        }

        public bool SaveUserLink(string name, string path)
        {
            CreateFloder(@"SchoolInteligentUSB\UserWindowXml");
            string SaveFilePath = usbpath + @"SchoolInteligentUSB\UserWindowXml\UserLinkList.xml";

            if (UserLinkOverlap(name, path))
            {
                Load();

                XmlElement ListElement = linklist.CreateElement("List");

                XmlAttribute ExeNameAttribute = linklist.CreateAttribute("EXE_Name");
                ListElement.SetAttributeNode(ExeNameAttribute);
                ExeNameAttribute.Value = name;

                XmlElement TitleElement = linklist.CreateElement("Title");
                TitleElement.InnerText = name;
                ListElement.AppendChild(TitleElement);

                XmlElement PathElement = linklist.CreateElement("PULLPATH");
                PathElement.InnerText = path;
                ListElement.AppendChild(PathElement);

                linklistElement.AppendChild(ListElement);

                linklist.AppendChild(linklistElement);
                linklist.Save(SaveFilePath);

                return true;
            }

            return false;
        }

        public bool UserLinkOverlap(string name, string path)
        {
            Load();

            XmlNodeList userlinklist = linklist.GetElementsByTagName("List");

            foreach (XmlNode node in userlinklist)
            {
                XmlElement LinkListInfoElement = (XmlElement)node;

                if (name == LinkListInfoElement.GetElementsByTagName("Title")[0].InnerText && path == LinkListInfoElement.GetElementsByTagName("PULLPATH")[0].InnerText)
                {
                    return false;
                }
            }
            return true;
        }

        public ArrayList LoadUserLink()
        {
            CreateFloder(@"SchoolInteligentUSB\UserWindowXml");
            UserLink.Clear();

            try
            {
                Load();

                string linklistinfo;

                XmlNodeList userlinklist = linklist.GetElementsByTagName("List");

                foreach (XmlNode node in userlinklist)
                {
                    XmlElement LinkListInfoElement = (XmlElement)node;

                    if (LinkListInfoElement.HasAttributes)
                    {
                        linklistinfo = LinkListInfoElement.GetElementsByTagName("Title")[0].InnerText + "\a"
                                  + LinkListInfoElement.GetElementsByTagName("PULLPATH")[0].InnerText;

                        LinkListAllData(linklistinfo);
                    }
                }

                return UserLink;
            }
            catch
            {
                return null;
            }
        }

        private void LinkListAllData(string info)
        {
            UserLink.Add(info);
        }

        public bool DeleteUserLink(string name, string path)
        {
            string SaveFilePath = usbpath + @"SchoolInteligentUSB\UserWindowXml\UserLinkList.xml";

            Load();

            XmlNodeList userlinklist = linklist.GetElementsByTagName("List");

            foreach (XmlNode node in userlinklist)
            {
                XmlElement LinkListInfoElement = (XmlElement)node;

                if (name == LinkListInfoElement.GetElementsByTagName("Title")[0].InnerText && path == LinkListInfoElement.GetElementsByTagName("PULLPATH")[0].InnerText)
                {
                    LinkListInfoElement.RemoveAll();
                    LinkListInfoElement.ParentNode.RemoveChild(LinkListInfoElement);
                    linklist.Save(SaveFilePath);

                    return true;
                }
            }
            return false;
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
