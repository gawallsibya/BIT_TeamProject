using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace UserWallPaper.Xml
{
    class DownloadedXml
    {
        #region xml 변수들
        static private XmlDocument xmlDoc;
        static private XmlElement MembersElement;
        #endregion

        #region DownloadedFile 관련 변수들
        static string usbpath = (string)Application.Current.Properties["USBDrivePath"];
        static private string downloadedfile = usbpath + @"\SchoolInteligentUSB\DownloadedFileDirectory\DownloadedFileList\DownloadedFile.xml";
        static private string downloadedbook = usbpath + @"\SchoolInteligentUSB\DownloadedFileDirectory\DownloadedFileList\DownloadedBook.xml";
        #endregion

        static public void Load(int flag)
        {
            if (flag == 1)
            {
                DirectoryInfo di = new DirectoryInfo(usbpath + @"\SchoolInteligentUSB\DownloadedFileDirectory\DownloadedFileList");
                if (di.Exists == false)
                {
                    di.Create();
                }

                xmlDoc = new XmlDocument();

                try
                {
                    xmlDoc.Load(downloadedbook);

                    XmlNodeList UploadedFileList = xmlDoc.GetElementsByTagName("DownloadedBook");
                    MembersElement = (XmlElement)UploadedFileList[0];
                }
                catch
                {
                    MembersElement = xmlDoc.CreateElement("DownloadedBook");
                }
            }
            else if (flag == 2)
            {
                DirectoryInfo di = new DirectoryInfo(usbpath + @"\SchoolInteligentUSB\DownloadedFileDirectory\DownloadedFileList");
                if (di.Exists == false)
                {
                    di.Create();
                }

                xmlDoc = new XmlDocument();

                try
                {
                    xmlDoc.Load(downloadedfile);

                    XmlNodeList UploadedFileList = xmlDoc.GetElementsByTagName("DownloadedFile");
                    MembersElement = (XmlElement)UploadedFileList[0];
                }
                catch
                {
                    MembersElement = xmlDoc.CreateElement("DownloadedFile");
                }
            }
        }

        static public bool DownloadedBookWrite(string filename, string lastdate)
        {
            Load(1);

            XmlNodeList List = xmlDoc.GetElementsByTagName("File");

            foreach (XmlNode node in List)
            {
                XmlElement fileElement = (XmlElement)node;

                if (fileElement.HasChildNodes)
                {
                    foreach (XmlElement nfileElement in fileElement)
                    {
                        if (filename == nfileElement.Attributes["Name"].InnerText)
                        {
                            nfileElement.GetElementsByTagName("Date")[0].InnerText = lastdate;
                            xmlDoc.AppendChild(MembersElement);
                            xmlDoc.Save(downloadedbook);
                            return true;
                        }
                    }
                }
            }

            XmlElement FileElement;
            if ((XmlElement)xmlDoc.GetElementsByTagName("File")[0] == null)
            {
                FileElement = xmlDoc.CreateElement("File");
            }
            else
                FileElement = (XmlElement)xmlDoc.GetElementsByTagName("File")[0];

            XmlElement dataElement = xmlDoc.CreateElement("Data");
            FileElement.AppendChild(dataElement);

            XmlAttribute nameAttribute = xmlDoc.CreateAttribute("Name");
            dataElement.SetAttributeNode(nameAttribute);
            nameAttribute.Value = filename;

            XmlElement dateElement = xmlDoc.CreateElement("Date");
            dataElement.AppendChild(dateElement);
            dateElement.InnerText = lastdate;

            MembersElement.AppendChild(FileElement);

            xmlDoc.AppendChild(MembersElement);
            xmlDoc.Save(downloadedbook);

            return true;
        }

        static public string DownloadedBookRead()
        {
            Load(1);

            XmlNodeList List = xmlDoc.GetElementsByTagName("File");

            foreach (XmlNode node in List)
            {
                XmlElement fileElement = (XmlElement)node;

                if (fileElement.HasChildNodes)
                {
                    foreach (XmlElement nfileElement in fileElement)
                    {
                        return nfileElement.InnerText;                        
                    }
                }
            }
            return null;
        }

        static public string DownloadedBookCheck(string name, string date)
        {
            Load(1);

            XmlNodeList List = xmlDoc.GetElementsByTagName("File");

            if (name != null && List.Count != 0)
            {
                foreach (XmlNode node in List)
                {
                    XmlElement fileElement = (XmlElement)node;

                    foreach (XmlElement nfileElement in fileElement.ChildNodes)
                    {
                        if (name == nfileElement.Attributes["Name"].InnerText && date == nfileElement.GetElementsByTagName("Date")[0].InnerText)
                        {
                            return null;
                        }
                    }
                }
            }
            return name;
        }

        static public bool DownloadedFileWrite(string filename, string lastdate)
        {
            Load(2);

            XmlNodeList List = xmlDoc.GetElementsByTagName("File");

            foreach (XmlNode node in List)
            {
                XmlElement fileElement = (XmlElement)node;

                if (fileElement.HasChildNodes)
                {
                    foreach (XmlElement nfileElement in fileElement)
                    {
                        if (filename == nfileElement.Attributes["Name"].InnerText)
                        {
                            nfileElement.GetElementsByTagName("Date")[0].InnerText = lastdate;
                            xmlDoc.AppendChild(MembersElement);
                            xmlDoc.Save(downloadedfile);
                            return true;
                        }
                    }
                }
            }

            XmlElement FileElement;
            if ((XmlElement)xmlDoc.GetElementsByTagName("File")[0] == null)
            {
                FileElement = xmlDoc.CreateElement("File");
            }
            else
                FileElement = (XmlElement)xmlDoc.GetElementsByTagName("File")[0];

            XmlElement dataElement = xmlDoc.CreateElement("Data");
            FileElement.AppendChild(dataElement);

            XmlAttribute nameAttribute = xmlDoc.CreateAttribute("Name");
            dataElement.SetAttributeNode(nameAttribute);
            nameAttribute.Value = filename;

            XmlElement dateElement = xmlDoc.CreateElement("Date");
            dataElement.AppendChild(dateElement);
            dateElement.InnerText = lastdate;

            MembersElement.AppendChild(FileElement);

            xmlDoc.AppendChild(MembersElement);
            xmlDoc.Save(downloadedfile);

            return true;
        }

        static public string DownloadedFileRead()
        {
            Load(2);

            XmlNodeList List = xmlDoc.GetElementsByTagName("File");

            foreach (XmlNode node in List)
            {
                XmlElement fileElement = (XmlElement)node;

                if (fileElement.HasChildNodes)
                {
                    foreach (XmlElement nfileElement in fileElement)
                    {
                        return nfileElement.InnerText;
                    }
                }
            }
            return null;
        }

        static public string DownloadedFileCheck(string name, string date)
        {
            Load(2);

            XmlNodeList List = xmlDoc.GetElementsByTagName("File");

            if (name != null && List.Count != 0)
            {
                foreach (XmlNode node in List)
                {
                    XmlElement fileElement = (XmlElement)node;

                    foreach (XmlElement nfileElement in fileElement.ChildNodes)
                    {
                        if (name == nfileElement.Attributes["Name"].InnerText && date == nfileElement.GetElementsByTagName("Date")[0].InnerText)
                        {
                            return null;
                        }
                    }
                }
            }
            return name;
        }
    }
}
