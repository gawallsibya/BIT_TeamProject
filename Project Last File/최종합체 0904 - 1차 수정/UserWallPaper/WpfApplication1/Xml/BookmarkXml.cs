using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows;

namespace UserWallPaper.Xml
{
   static public class BookmarkXml
    {
        static private XmlDocument Member = new XmlDocument();
        static private XmlElement MembersElement = null;
        static private string usbpath = (string)Application.Current.Properties["USBDrivePath"];
        static private string SaveFilePath = usbpath + @"SchoolInteligentUSB\UserWindowXml\test1.xml";
        static ArrayList Book_ArrayList = new ArrayList();

        static public void LOAD()
        {
            try
            {
                Member.Load(SaveFilePath);

                XmlNodeList asd = Member.GetElementsByTagName("BookMark");
                MembersElement = (XmlElement)asd[0];
            }
            catch
            {
                MembersElement = Member.CreateElement("BookMark");
            }
        }

        static public void WriteXml(string[] memberinfo)
        {
            LOAD();

            XmlNodeList notelist = Member.GetElementsByTagName("name");
            // 같은 page에서 저장시 내용 수정 기능
            foreach (XmlNode node in notelist)
            {
                XmlElement modifyElement = (XmlElement)node;

                if (modifyElement.HasAttributes && memberinfo[0].ToString() == modifyElement.Attributes["name"].InnerText)
                {
                    foreach (XmlElement ndeleteElement in modifyElement.GetElementsByTagName("Page"))
                    {
                        if (ndeleteElement.InnerText == memberinfo[2])
                        {
                            modifyElement.GetElementsByTagName("Page")[0].InnerText = memberinfo[2];
                            modifyElement.GetElementsByTagName("Memo")[0].InnerText = memberinfo[3];
                            Member.AppendChild(MembersElement);
                            Member.Save(SaveFilePath);
                            return;
                        }
                    }
                }
            }

            XmlElement MemElement = Member.CreateElement("name");

            XmlAttribute nameAttribute = Member.CreateAttribute("name");
            MemElement.SetAttributeNode(nameAttribute);
            nameAttribute.Value = memberinfo[0];

            XmlElement PathElement = Member.CreateElement("PDFPath");
            PathElement.InnerText = memberinfo[0];
            MemElement.AppendChild(PathElement);

            XmlElement PDFNameElement = Member.CreateElement("PDFName");
            PDFNameElement.InnerText = memberinfo[1];
            MemElement.AppendChild(PDFNameElement);

            XmlElement PageElement = Member.CreateElement("Page");
            PageElement.InnerText = memberinfo[2];
            MemElement.AppendChild(PageElement);

            XmlElement MemoElement = Member.CreateElement("Memo");
            MemoElement.InnerText = memberinfo[3];
            MemElement.AppendChild(MemoElement);

            MembersElement.AppendChild(MemElement);
            Member.AppendChild(MembersElement);
            Member.Save(SaveFilePath);
        }

        static public bool modifyXml(string[] memberinfo)
        {
            LOAD();

            XmlNodeList notelist = Member.GetElementsByTagName("name");
            // 같은 page에서 저장시 내용 수정 기능
            foreach (XmlNode node in notelist)
            {
                XmlElement modifyElement = (XmlElement)node;

                if (modifyElement.HasAttributes && memberinfo[0].ToString() == modifyElement.Attributes["name"].InnerText)
                {
                    foreach (XmlElement ndeleteElement in modifyElement.GetElementsByTagName("Page"))
                    {
                        if (ndeleteElement.InnerText == memberinfo[2])
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        static public void DeleteXml(string path, string page)
        {
            LOAD();

            XmlNodeList notelist = Member.GetElementsByTagName("name");

            foreach (XmlNode node in notelist)
            {
                XmlElement DeleteElement = (XmlElement)node;

                if (DeleteElement.HasAttributes && path.ToString() == DeleteElement.Attributes["name"].InnerText)
                {
                    foreach (XmlElement ndeleteElement in DeleteElement.GetElementsByTagName("Page"))
                    {
                        if (ndeleteElement.InnerText == page)
                        {
                            MembersElement.RemoveChild(DeleteElement);
                            Member.AppendChild(MembersElement);
                            Member.Save(SaveFilePath);
                            return;
                        }
                    }
                }
            }

        }

        static public void DeleteAllXml(string path, string page)
        {
            LOAD();

            XmlNodeList notelist = Member.GetElementsByTagName("name");

            foreach (XmlNode node in notelist)
            {
                XmlElement DeleteElement = (XmlElement)node;

                if (DeleteElement.HasAttributes && path.ToString() == DeleteElement.Attributes["name"].InnerText)
                {
                    MembersElement.RemoveAll();
                    Member.AppendChild(MembersElement);
                    Member.Save(SaveFilePath);
                    return;
                }
            }

        }

        static public ArrayList ReadXml()
        {
            Book_ArrayList.Clear();
            LOAD();

            string MemberName;

            XmlNodeList MemberList = Member.GetElementsByTagName("name");

            foreach (XmlNode node in MemberList)
            {
                XmlElement memberElement = (XmlElement)node;

                if (memberElement.HasAttributes)
                {
                    MemberName = memberElement.GetElementsByTagName("PDFPath")[0].InnerText + "\a"
                    + memberElement.GetElementsByTagName("PDFName")[0].InnerText + "\a"
                    + memberElement.GetElementsByTagName("Page")[0].InnerText + "\a"
                    + memberElement.GetElementsByTagName("Memo")[0].InnerText;

                    BOOK_AllData(MemberName);
                }
            }
            return Book_ArrayList;
        }

        static public void BOOK_AllData(string _info)
        {
            Book_ArrayList.Add(_info);
        }
    }
}
