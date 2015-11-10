using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ConsoleApplication2
{
    class Xml
    {
        private XmlDocument xmlDoc;
        private XmlElement MembersElement;
        private string UsbSave = "c:\\AAA\\usb.xml";

        public Xml()
        {
            xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(UsbSave);

                XmlNodeList asd = xmlDoc.GetElementsByTagName("Member");
                MembersElement = (XmlElement)asd[0];
            }
            catch
            {
                MembersElement = xmlDoc.CreateElement("Member");
            }
        }

        public bool Xml_Save(string serial,string id, string pw,string name,string sort,string number)
        {
            XmlNodeList List = xmlDoc.GetElementsByTagName("Id");

            foreach (XmlNode node in List)
            {
                XmlElement memberElement = (XmlElement)node;

                if (memberElement.HasAttributes)
                {
                    if (id == memberElement.Attributes["Key"].InnerText)
                    {
                        return false;
                    }
                    else if (number == memberElement.GetElementsByTagName("Number")[0].InnerText)
                    {
                    }
                }
            }

            XmlElement IdElement = xmlDoc.CreateElement("Id");

            XmlAttribute idAttribute = xmlDoc.CreateAttribute("Key");
            IdElement.SetAttributeNode(idAttribute);
            idAttribute.Value = id;

            XmlElement NameElement = xmlDoc.CreateElement("Name");
            NameElement.InnerText = name;
            IdElement.AppendChild(NameElement);

            XmlElement PwElement = xmlDoc.CreateElement("Pw");
            PwElement.InnerText = pw;
            IdElement.AppendChild(PwElement);

            XmlElement SerialElement = xmlDoc.CreateElement("Serial");
            SerialElement.InnerText = serial;
            IdElement.AppendChild(SerialElement);

            XmlElement TypeElement = xmlDoc.CreateElement("Type");
            TypeElement.InnerText = sort;
            IdElement.AppendChild(TypeElement);

            XmlElement NumberElement = xmlDoc.CreateElement("Number");
            NumberElement.InnerText = sort;
            IdElement.AppendChild(NumberElement);

            MembersElement.AppendChild(IdElement);

            xmlDoc.AppendChild(MembersElement);
            xmlDoc.Save(UsbSave);

            return true;
        }

        public void Xml_Find(string serial)
        {
            XmlNodeList bookList = xmlDoc.GetElementsByTagName("ID");

            foreach (XmlNode node in bookList)
            {
                XmlElement IdElement = (XmlElement)node;

                if (serial == IdElement.GetElementsByTagName("Serial")[0].InnerText)
                {
                    System.Windows.Forms.MessageBox.Show("usb연결");
                    return;
                }
            }
            System.Windows.Forms.MessageBox.Show("미등록 usb");
        }
    }
}
