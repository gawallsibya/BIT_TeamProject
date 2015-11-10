using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using UserWallPaper.CustomControl;

namespace UserWallPaper.Xml
{
    class DrawXml
    {
        static private XmlDocument xmlDoc;
        static private XmlElement DrawElement;
        static private string DrawSave = null;
        static private string usbpath;

        static public MainWindow PdfWin
        {
            get;
            set;
        }

        static public void BookName(string book)
        {
            usbpath = (string)Application.Current.Properties["USBDrivePath"];

            string name = System.IO.Path.GetFileNameWithoutExtension(book);
            string ext = System.IO.Path.GetExtension(book);
            DrawSave = usbpath + @"SchoolInteligentUSB\UserWindowXml\" + name + "(" + ext + ").xml";
        }

        static public void SavePath(ref string path)
        {
            xmlDoc = null;
            path += DrawSave;
        }

        static private void Init()
        {
            xmlDoc = new XmlDocument();
            
            try
            {
                xmlDoc.Load(DrawSave);

                XmlNodeList asd = xmlDoc.GetElementsByTagName("Draw");
                DrawElement = (XmlElement)asd[0];
            }
            catch
            {
                DrawElement = xmlDoc.CreateElement("Draw");
            }
        }

        static private XmlNode FindTypeNode(string page, string type, int strokeidx)
        {
            XmlNodeList idxlist = xmlDoc.SelectNodes("//Page[@Value='" + page + "']/Type[@Value='" + type + "']/Index[@Value='" + strokeidx.ToString() + "']");
            if (idxlist[0] != null)
            {
                XmlNode pointnode = idxlist[0].LastChild;
                if (pointnode != null)
                    return pointnode;
            }

            return null;
        }
        static public void Xml_Save(string type,int strokeidx, System.Windows.Point pt,string page,Color color,int fontsize)
        {
            if (strokeidx < 0)
                return;

            Init();

            XmlNode pointnode = FindTypeNode(page, type, strokeidx);

            if (pointnode != null)
            {
                XmlElement New_X = xmlDoc.CreateElement("Pt_X");
                New_X.InnerText = pt.X.ToString();
                pointnode.AppendChild(New_X);

                XmlElement New_Y = xmlDoc.CreateElement("Pt_Y");
                New_Y.InnerText = pt.Y.ToString();
                pointnode.AppendChild(New_Y);

                xmlDoc.AppendChild(DrawElement);
                xmlDoc.Save(DrawSave);
                return;
            }

            XmlElement DlistElement = null;

            XmlNodeList pagelist = xmlDoc.SelectNodes("//Page[@Value='" + page + "']");
            XmlElement Page = null;
            if (pagelist[0] != null)
            {
                Page = (XmlElement)pagelist[0];
            }
            else
            {
                DlistElement = xmlDoc.CreateElement("DrawList");
                Page = xmlDoc.CreateElement("Page");
            }

            XmlAttribute PageAttribute = xmlDoc.CreateAttribute("Value");
            Page.SetAttributeNode(PageAttribute);
            PageAttribute.Value = page;

            XmlElement TypeElement = xmlDoc.CreateElement("Type");
            XmlAttribute TypeAttribute = xmlDoc.CreateAttribute("Value");
            TypeElement.SetAttributeNode(TypeAttribute);
            TypeAttribute.Value = type;      

            XmlElement IdxElement = xmlDoc.CreateElement("Index");
            XmlAttribute IdxAttribute = xmlDoc.CreateAttribute("Value");
            IdxElement.SetAttributeNode(IdxAttribute);
            IdxAttribute.Value = strokeidx.ToString();

            XmlElement ColorElement = xmlDoc.CreateElement("Color");
            ColorElement.InnerText = color.ToString();
            IdxElement.AppendChild(ColorElement);

            XmlElement FontSizeElement = xmlDoc.CreateElement("FontSize");
            FontSizeElement.InnerText = fontsize.ToString();
            IdxElement.AppendChild(FontSizeElement);

            XmlElement PtElement = xmlDoc.CreateElement("Point");
            IdxElement.AppendChild(PtElement);

            XmlElement Pt_X = xmlDoc.CreateElement("Pt_X");
            Pt_X.InnerText = pt.X.ToString();
            PtElement.AppendChild(Pt_X);

            XmlElement Pt_Y = xmlDoc.CreateElement("Pt_Y");
            Pt_Y.InnerText = pt.Y.ToString();
            PtElement.AppendChild(Pt_Y);


            TypeElement.AppendChild(IdxElement);
            Page.AppendChild(TypeElement);

            if (DlistElement != null)
            {
                DlistElement.AppendChild(Page);
                DrawElement.AppendChild(DlistElement);
            }
              
            xmlDoc.AppendChild(DrawElement);
            xmlDoc.Save(DrawSave);
        }
        static public void Xml_Save(string type, string id, double left, double top, double width, double height,string page)
        {
            Init();

            XmlElement DlistElement = null;

            XmlNodeList pagelist = xmlDoc.SelectNodes("//Page[@Value='" + page + "']");
            XmlElement Page = null;
            if (pagelist[0] != null)
            {
                Page = (XmlElement)pagelist[0];
            }
            else
            {
                DlistElement = xmlDoc.CreateElement("DrawList");
                Page = xmlDoc.CreateElement("Page");
            }
            XmlAttribute PageAttribute = xmlDoc.CreateAttribute("Value");
            Page.SetAttributeNode(PageAttribute);
            PageAttribute.Value = page;

            XmlElement TypeElement = xmlDoc.CreateElement("Type");
            XmlAttribute TypeAttribute = xmlDoc.CreateAttribute("Value");
            TypeElement.SetAttributeNode(TypeAttribute);
            TypeAttribute.Value = type;

            XmlElement IdElement = xmlDoc.CreateElement("Id");
            XmlAttribute IdAttribute = xmlDoc.CreateAttribute("Value");
            IdElement.SetAttributeNode(IdAttribute);
            IdAttribute.Value = id;

            XmlElement LeftElement = xmlDoc.CreateElement("Left");
            LeftElement.InnerText = left.ToString();
            IdElement.AppendChild(LeftElement);

            XmlElement TopElement = xmlDoc.CreateElement("Top");
            TopElement.InnerText = top.ToString();
            IdElement.AppendChild(TopElement);

            XmlElement WidthElement = xmlDoc.CreateElement("Width");
            WidthElement.InnerText = width.ToString();
            IdElement.AppendChild(WidthElement);

            XmlElement HeightElement = xmlDoc.CreateElement("Height");
            HeightElement.InnerText = height.ToString();
            IdElement.AppendChild(HeightElement);

            TypeElement.AppendChild(IdElement);
            Page.AppendChild(TypeElement);

            if (DlistElement != null)
            {
                DlistElement.AppendChild(Page);
                DrawElement.AppendChild(DlistElement);
            }

            xmlDoc.AppendChild(DrawElement);
            xmlDoc.Save(DrawSave);
        }

        static public void Remove(string page,int idx,int length)
        {
            Init();

            XmlNodeList idxlist = xmlDoc.SelectNodes("//Page[@Value='" + page + "']/Type/Index");

            //첫번째 인덱스
            if (idx == 0)
            {
                XmlNode deletenode = idxlist[idx];
                
                foreach (XmlNode idxnode in idxlist)
                {
                    if (idxnode == idxlist[idx])
                        continue;
                    idxnode.Attributes["Value"].InnerText = (idx++).ToString();
                }

                deletenode.ParentNode.ParentNode.RemoveChild(deletenode.ParentNode);
            }

            //중간 인덱스
            else if (idx > 0 && idx < length - 1)
            {
                XmlNode deletenode = idxlist[idx];

                foreach (XmlNode idxnode in idxlist)
                {
                    if (idxnode.Attributes["Value"].InnerText == "0")
                        continue;
                    if (idxnode == idxlist[idx])
                        continue;
                    idxnode.Attributes["Value"].InnerText = (idx++).ToString();
                }

                deletenode.ParentNode.ParentNode.RemoveChild(deletenode.ParentNode);
            }

            //마지막 인덱스
            else if (idx == length - 1)
            {
                idxlist[idx].ParentNode.ParentNode.RemoveChild(idxlist[idx].ParentNode);
            }

            xmlDoc.AppendChild(DrawElement);
            xmlDoc.Save(DrawSave);
        }

        static public void Remove(string page, string id)
        {
            Init();

            XmlNodeList typelist = xmlDoc.SelectNodes("//Page[@Value='" + page + "']/Type[@Value='Rectangle']/Id[@Value='"+id+"']");

            typelist[0].ParentNode.ParentNode.RemoveChild(typelist[0].ParentNode);

            xmlDoc.AppendChild(DrawElement);
            xmlDoc.Save(DrawSave);
        }

        static public ReadData[] XML_Read(string page)
        {
            Init();

            ArrayList data = new ArrayList();

            XmlNodeList idxlist = xmlDoc.SelectNodes("//Page[@Value='" + page + "']/Type/Index");
            XmlNodeList typelist = xmlDoc.SelectNodes("//Page[@Value='" + page + "']/Type[@Value='Rectangle']/Id");
            if (idxlist[0] != null)
            {
                for (int i = 0; i < idxlist.Count; i++)
                {
                    XmlNode pointnode = idxlist[i].LastChild;

                    ArrayList ptlist = new ArrayList();
                    Point pt = new Point();
                    int j = 0;
                    foreach (XmlNode node in pointnode.ChildNodes)
                    {
                        j++;
                        if (j % 2 == 0)
                        {
                            pt.Y = double.Parse(node.InnerText);
                            ptlist.Add(pt);
                        }
                        else
                        {
                            pt.X = double.Parse(node.InnerText);
                        }
                    }

                    ReadData readdata = new ReadData();
                    readdata.Pts = (Point[])ptlist.ToArray(typeof(Point));

                    Color color = (Color)ColorConverter.ConvertFromString(idxlist[i]["Color"].InnerText);
                    readdata.Color = color;
                    readdata.FontSize = int.Parse(idxlist[i]["FontSize"].InnerText);
                    readdata.Type = idxlist[i].ParentNode.Attributes["Value"].InnerText;

                    data.Add(readdata);
                }

                if (typelist[0] != null)
                {
                    foreach (XmlNode node in typelist)
                    {
                        double left = double.Parse(node["Left"].InnerText);
                        double top = double.Parse(node["Top"].InnerText);
                        double width = double.Parse(node["Width"].InnerText);
                        double height = double.Parse(node["Height"].InnerText);
                        string id = node.Attributes["Value"].InnerText;

                        ReadData readdata = new ReadData();
                        readdata.Text = new TextControl
                        {
                            BorderBrush = System.Windows.Media.Brushes.LightBlue,
                            BorderThickness = new Thickness(2),
                            Background = System.Windows.Media.Brushes.LightGreen,
                            Opacity = 0.5,
                            Width = width,
                            Height = height,
                            ID = id
                        };
                        readdata.Text.SetValue(MyInkCanvas.LeftProperty, left);
                        readdata.Text.SetValue(MyInkCanvas.TopProperty, top);
                        readdata.Type = "Rectangle";
                        readdata.Text.Text_Changed += PdfWin.Text_Changed;

                        data.Add(readdata);
                    }
                }

                return (ReadData[])data.ToArray(typeof(ReadData));
            }

            else
            {
                if (typelist[0] != null)
                {
                    foreach (XmlNode node in typelist)
                    {
                        double left = double.Parse(node["Left"].InnerText);
                        double top = double.Parse(node["Top"].InnerText);
                        double width = double.Parse(node["Width"].InnerText);
                        double height = double.Parse(node["Height"].InnerText);
                        string id = node.Attributes["Value"].InnerText;

                        ReadData readdata = new ReadData();
                        readdata.Text = new TextControl
                        {
                            BorderBrush = System.Windows.Media.Brushes.LightBlue,
                            BorderThickness = new Thickness(2),
                            Background = System.Windows.Media.Brushes.LightGreen,
                            Opacity = 0.5,
                            Width = width,
                            Height = height,
                            ID = id
                        };
                        readdata.Text.SetValue(MyInkCanvas.LeftProperty, left);
                        readdata.Text.SetValue(MyInkCanvas.TopProperty, top);
                        readdata.Type = "Rectangle";
                        readdata.Text.Text_Changed += PdfWin.Text_Changed;

                        data.Add(readdata);
                    }
                }

                return (ReadData[])data.ToArray(typeof(ReadData));
            }
        }
    }

    public class ReadData
    {
        public Color Color
        {
            get;
            set;
        }

        public int FontSize
        {
            get;
            set;
        }

        public Point[] Pts
        {
            get;
            set;
        }

        public TextControl Text
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }
    }
}
