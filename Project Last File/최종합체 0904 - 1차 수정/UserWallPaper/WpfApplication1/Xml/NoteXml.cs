using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Xml;

namespace UserWallPaper.Xml
{
    public class NoteXml
    {
        #region xml 변수들
        static private XmlDocument xmlDoc;
        static private XmlElement NoteElement;
        static private string NoteSave;
        static private string usbpath;
        #endregion

        #region xml 파일 로드
        static public void BookName(string book)
        {
            usbpath = (string)Application.Current.Properties["USBDrivePath"];
            NoteSave = usbpath + @"SchoolInteligentUSB\UserWindowXml\" + book + "-note.xml";
        }

        static public void SavePath(ref string path)
        {
            path += NoteSave;
        }

        static private void Load()
        {
            xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(NoteSave);

                XmlNodeList asd = xmlDoc.GetElementsByTagName("Book");
                NoteElement = (XmlElement)asd[0];
            }
            catch
            {
                NoteElement = xmlDoc.CreateElement("Book");
            }
        }
        #endregion

        #region xml 파일 저장
        static public void Save(string page, string text)
        {
            Load();

            XmlNodeList note = xmlDoc.GetElementsByTagName("Note");
            if (note != null)
            {
                foreach (XmlNode node in note)
                {
                    XmlElement Element = (XmlElement)node;

                    if (Element.HasAttributes && page == Element.Attributes["Page"].InnerText && Element.GetElementsByTagName("Id")[0] == null)
                    {
                        Element.GetElementsByTagName("Text")[0].InnerText = text;
                        xmlDoc.AppendChild(NoteElement);
                        xmlDoc.Save(NoteSave);
                        return;
                    }
                }
            }

            XmlElement noteElement = xmlDoc.CreateElement("Note");

            XmlAttribute PageAttribute = xmlDoc.CreateAttribute("Page");
            noteElement.SetAttributeNode(PageAttribute);
            PageAttribute.Value = page;

            XmlElement TextElement = xmlDoc.CreateElement("Text");
            TextElement.InnerText = text;
            noteElement.AppendChild(TextElement);

            NoteElement.AppendChild(noteElement);

            xmlDoc.AppendChild(NoteElement);
            xmlDoc.Save(NoteSave);
        }

        static public void Save(string id,string page,string text)
        {
            Load();

            XmlNodeList note = xmlDoc.GetElementsByTagName("Note");
           
            if (note != null)
            {
                foreach (XmlNode node in note)
                {
                    XmlElement Element = (XmlElement)node;

                    try
                    {
                        if (Element.HasAttributes && page == Element.Attributes["Page"].InnerText && id == Element.GetElementsByTagName("Id")[0].InnerText)
                        {
                            Element.GetElementsByTagName("Text")[0].InnerText = text;
                            xmlDoc.AppendChild(NoteElement);
                            xmlDoc.Save(NoteSave);
                            return;
                        }
                    }
                    catch { continue; }
                }
            }

            XmlElement noteElement = xmlDoc.CreateElement("Note");

            XmlAttribute PageAttribute = xmlDoc.CreateAttribute("Page");
            noteElement.SetAttributeNode(PageAttribute);
            PageAttribute.Value = page;

            XmlElement IdElement = xmlDoc.CreateElement("Id");
            IdElement.InnerText = id;
            noteElement.AppendChild(IdElement);

            XmlElement TextElement = xmlDoc.CreateElement("Text");
            TextElement.InnerText = text;
            noteElement.AppendChild(TextElement);

            NoteElement.AppendChild(noteElement);

            xmlDoc.AppendChild(NoteElement);
            xmlDoc.Save(NoteSave);
        }
        #endregion

        #region xml 검색
        static public string Load(string page)
        {
            Load();

            XmlNodeList note = xmlDoc.GetElementsByTagName("Note");
            if (note != null)
            {
                foreach (XmlNode node in note)
                {
                    XmlElement nElement = (XmlElement)node;

                    if (page == nElement.Attributes["Page"].InnerText && nElement.GetElementsByTagName("Id")[0] == null)
                    {
                        return nElement.GetElementsByTagName("Text")[0].InnerText;
                    }
                }
            }

            return null;
        }

        static public string Load(string id, string page)
        {
            Load();

            XmlNodeList note = xmlDoc.GetElementsByTagName("Note");
            if (note != null)
            {
                foreach (XmlNode node in note)
                {
                    XmlElement nElement = (XmlElement)node;

                    try
                    {
                        if (page == nElement.Attributes["Page"].InnerText && id == nElement.GetElementsByTagName("Id")[0].InnerText)
                        {
                            return nElement.GetElementsByTagName("Text")[0].InnerText;
                        }
                    }
                    catch { continue; }
                }
            }

            return null;
        }

        static public void Remove(string id)
        {
            Load();

            XmlNode deletenode = xmlDoc.SelectSingleNode("//Note/Id[text()='" + id + "']");

            try
            {
                deletenode.ParentNode.ParentNode.RemoveChild(deletenode.ParentNode);
            }
            catch { }

            if (deletenode != null)
            {
                xmlDoc.AppendChild(NoteElement);
                xmlDoc.Save(NoteSave);
            }
            
        }
        #endregion
    }
}
