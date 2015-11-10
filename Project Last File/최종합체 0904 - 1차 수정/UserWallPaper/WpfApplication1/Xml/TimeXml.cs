using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;

namespace UserWallPaper.Xml
{
    public class TimeXml
    {
        #region xml 변수들
        static private XmlDocument xmlDoc;
        static private XmlElement TimeElement;
        static private string TimeSave = "c:\\AAA\\Time.xml";
        #endregion

        #region xml 파일 로드
        static private void Load()
        {
            xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.Load(TimeSave);

                XmlNodeList asd = xmlDoc.GetElementsByTagName("Time");
                TimeElement = (XmlElement)asd[0];
            }
            catch
            {
                TimeElement = xmlDoc.CreateElement("Time");
            }
        }
        #endregion

        #region xml 파일 저장
        static public bool Save(string day,string sub,string start, string end, string book)
        {
            Load();

            XmlElement DayElement = xmlDoc.CreateElement("Day");

            XmlAttribute DayAttribute = xmlDoc.CreateAttribute("Key");
            DayElement.SetAttributeNode(DayAttribute);
            DayAttribute.Value = day;

            XmlElement SubElement = xmlDoc.CreateElement("Subject");
            SubElement.InnerText = sub;
            DayElement.AppendChild(SubElement);

            XmlElement BookElement = xmlDoc.CreateElement("Book");
            BookElement.InnerText = book;
            DayElement.AppendChild(BookElement);

            XmlElement StartElement = xmlDoc.CreateElement("Start");
            StartElement.InnerText = start;
            DayElement.AppendChild(StartElement);

            XmlElement EndElement = xmlDoc.CreateElement("End");
            EndElement.InnerText = end;
            DayElement.AppendChild(EndElement);

            TimeElement.AppendChild(DayElement);

            xmlDoc.AppendChild(TimeElement);
            xmlDoc.Save(TimeSave);

            return true;
        }
        #endregion

        static public ArrayList LoadAll()
        {
            Load();

            XmlNodeList timeList = xmlDoc.GetElementsByTagName("Day");

            ArrayList ar = new ArrayList();

            foreach (XmlNode node in timeList)
            {
                XmlElement timeElement = (XmlElement)node;

                if (timeElement.HasAttributes)
                { 
                    string[] time = new string[4];

                    time[0] = timeElement.Attributes["Key"].InnerText;                      //요일
                    time[1] = timeElement.GetElementsByTagName("Subject")[0].InnerText;     //과목
                    time[2] = timeElement.GetElementsByTagName("Start")[0].InnerText;       //시작
                    time[3] = timeElement.GetElementsByTagName("End")[0].InnerText;         //끝

                    ar.Add(time);
                }
            }
            return ar;
        }

        static public ArrayList LoadDay(string day)
        {
            Load();

            XmlNodeList timeList = xmlDoc.GetElementsByTagName("Day");

            ArrayList ar = new ArrayList();

            foreach (XmlNode node in timeList)
            {
                XmlElement timeElement = (XmlElement)node;

                if (timeElement.HasAttributes)
                {
                    if (day == timeElement.Attributes["Key"].InnerText)
                    {
                        string[] time = new string[3];

                        time[0] = timeElement.GetElementsByTagName("Book")[0].InnerText;     //과목
                        time[1] = timeElement.GetElementsByTagName("Start")[0].InnerText;       //시작
                        time[2] = timeElement.GetElementsByTagName("End")[0].InnerText;         //끝

                        ar.Add(time);
                    }
                }
            }
            return ar;
        }

        static public void Remove(string sub)
        {
            Load();

            XmlNodeList timeList = xmlDoc.GetElementsByTagName("Day");

            XmlNode deletenode = null;

            foreach (XmlNode node in timeList)
            {
                XmlElement timeElement = (XmlElement)node;

                if (timeElement.HasAttributes)
                {
                    if (sub == timeElement.GetElementsByTagName("Subject")[0].InnerText)
                    {
                        deletenode = node;
                    }
                }
            }

            TimeElement.RemoveChild(deletenode);
            xmlDoc.AppendChild(TimeElement);
            xmlDoc.Save(TimeSave);
        }
    }
}
