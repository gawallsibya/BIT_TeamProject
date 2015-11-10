using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace _0624Server
{
    public class Xml
    {
        #region xml 변수들
        static private XmlDocument xmlDoc;
        static private XmlElement MembersElement;
        static private string UsbSave = @"C:\SchoolInteligentUSB\UserInfoDirectory\usb.xml";
        #endregion

        #region Submission 관련 변수들
        static private string SugesstSubmissionList = @"C:\SchoolInteligentUSB\SubmissionDirectory\SugesstSubmissionList\Submission.xml";
        static private string SubmitSubmission;
        static private string StudentPersonalSumitinfo;
        static private string WriteDate = System.DateTime.Now.ToString("yy년 MM월 dd일 hh시 mm분");
        static ArrayList submissionarraylist = new ArrayList();
        #endregion

        #region StudentBoard 관련 변수들
        static private string studentboardlist = @"C:\SchoolInteligentUSB\StudentBoardDirectory\StudentBoardList.xml";
        static ArrayList studentboard = new ArrayList();
        #endregion

        #region ReadSubmissionBoard 관련 변수들
        static private string readsubmissionboardlist = @"C:\SchoolInteligentUSB\ReadSubmissionDirectory\ReadSubmissionBoard.xml";
        #endregion

        #region ReadStudentBoard 관련 변수들
        static private string readstudentboardlist = @"C:\SchoolInteligentUSB\ReadStudentBoardDirectory\ReadStudentBoard.xml";
        #endregion

        #region UploadedFile 관련 변수들
        static private string uploadedfilelist = @"C:\SchoolInteligentUSB\UploadedFileDirectory\UploadedFile.xml";
        static private string uploadedbooklist = @"C:\SchoolInteligentUSB\UploadedFileDirectory\UploadedBook.xml";
        #endregion

        #region NoUSBWindow 관련 변수들
        static private string noUSBlist = @"C:\SchoolInteligentUSB\NoUSBDirectory\NoUSBList.xml";
        static ArrayList noUSB = new ArrayList();
        #endregion

        #region xml 파일 로드
        static private void Load(int flag)
        {
            if (flag == 0)
            {
                DirectoryInfo di = new DirectoryInfo(@"C:\SchoolInteligentUSB\UserInfoDirectory");
                if (di.Exists == false)
                {
                    di.Create();
                }

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
            else if (flag == 1)
            {
                DirectoryInfo di = new DirectoryInfo(@"C:\SchoolInteligentUSB\SubmissionDirectory\SugesstSubmissionList");
                if (di.Exists == false)
                {
                    di.Create();
                }

                submissionarraylist.Clear();

                try
                {
                    xmlDoc.Load(SugesstSubmissionList);

                    XmlNodeList SubmissionList = xmlDoc.GetElementsByTagName("SubMission");
                    MembersElement = (XmlElement)SubmissionList[0];
                }
                catch
                {
                    MembersElement = xmlDoc.CreateElement("SubMission");
                }
            }

            else if (flag == 2)
            {
                DirectoryInfo di = new DirectoryInfo(@"C:\SchoolInteligentUSB\SubmissionDirectory\SubmitSubmission");
                if (di.Exists == false)
                {
                    di.Create();
                }

                submissionarraylist.Clear();

                try
                {
                    xmlDoc.Load(SubmitSubmission);

                    XmlNodeList SubmissionList = xmlDoc.GetElementsByTagName("SubMission");
                    MembersElement = (XmlElement)SubmissionList[0];
                }
                catch
                {
                    MembersElement = xmlDoc.CreateElement("SubMission");
                }
            }

            else if (flag == 3)
            {
                DirectoryInfo di = new DirectoryInfo(@"C:\SchoolInteligentUSB\SubmissionDirectory\StudentPersonalSumitinfo");
                if (di.Exists == false)
                {
                    di.Create();
                }

                submissionarraylist.Clear();

                try
                {
                    xmlDoc.Load(StudentPersonalSumitinfo);

                    XmlNodeList SubmissionList = xmlDoc.GetElementsByTagName("SubMission");
                    MembersElement = (XmlElement)SubmissionList[0];
                }
                catch
                {
                    MembersElement = xmlDoc.CreateElement("SubMission");
                }
            }

            else if (flag == 4)
            {
                DirectoryInfo di = new DirectoryInfo(@"C:\SchoolInteligentUSB\StudentBoardDirectory");
                if (di.Exists == false)
                {
                    di.Create();
                }

                studentboard.Clear();

                try
                {
                    xmlDoc.Load(studentboardlist);

                    XmlNodeList SubmissionList = xmlDoc.GetElementsByTagName("StudentBoard");
                    MembersElement = (XmlElement)SubmissionList[0];
                }
                catch
                {
                    MembersElement = xmlDoc.CreateElement("StudentBoard");
                }
            }

            else if (flag == 5)
            {
                DirectoryInfo di = new DirectoryInfo(@"C:\SchoolInteligentUSB\ReadSubmissionDirectory");
                if (di.Exists == false)
                {
                    di.Create();
                }

                try
                {
                    xmlDoc.Load(readsubmissionboardlist);

                    XmlNodeList ReadSubmissionBoard = xmlDoc.GetElementsByTagName("ReadSubmissionBoard");
                    MembersElement = (XmlElement)ReadSubmissionBoard[0];
                }
                catch
                {
                    MembersElement = xmlDoc.CreateElement("ReadSubmissionBoard");
                }
            }

            else if (flag == 6)
            {
                DirectoryInfo di = new DirectoryInfo(@"C:\SchoolInteligentUSB\ReadStudentBoardDirectory");
                if (di.Exists == false)
                {
                    di.Create();
                }
                try
                {
                    xmlDoc.Load(readstudentboardlist);

                    XmlNodeList ReadStudentBoardList = xmlDoc.GetElementsByTagName("ReadStudentBoard");
                    MembersElement = (XmlElement)ReadStudentBoardList[0];
                }
                catch
                {
                    MembersElement = xmlDoc.CreateElement("ReadStudentBoard");
                }
            }

            else if (flag == 7)
            {
                DirectoryInfo di = new DirectoryInfo(@"C:\SchoolInteligentUSB\UploadedFileDirectory");
                if (di.Exists == false)
                {
                    di.Create();
                }
                try
                {
                    xmlDoc.Load(uploadedfilelist);

                    XmlNodeList ReadStudentBoardList = xmlDoc.GetElementsByTagName("UploadedFile");
                    MembersElement = (XmlElement)ReadStudentBoardList[0];
                }
                catch
                {
                    MembersElement = xmlDoc.CreateElement("UploadedFile");
                }
            }

            else if (flag == 8)
            {
                noUSB.Clear();

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
            }
            else if (flag == 9)
            {
                DirectoryInfo di = new DirectoryInfo(@"C:\SchoolInteligentUSB\UploadedFileDirectory");
                if (di.Exists == false)
                {
                    di.Create();
                }

                try
                {
                    xmlDoc.Load(uploadedbooklist);

                    XmlNodeList SubmissionList = xmlDoc.GetElementsByTagName("UploadedBook");
                    MembersElement = (XmlElement)SubmissionList[0];
                }
                catch
                {
                    MembersElement = xmlDoc.CreateElement("UploadedBook");
                }
            }
        }
        #endregion

        #region xml 파일 저장
        static public bool Xml_Save(string serial, string id, string pw, string name, string sort, string number)
        {
            Load(0);

            XmlNodeList List = xmlDoc.GetElementsByTagName("User");

            foreach (XmlNode node in List)
            {
                XmlElement memberElement = (XmlElement)node;

                if (memberElement.HasAttributes)
                {
                    if (id == memberElement.Attributes["Key"].InnerText || number == memberElement.GetElementsByTagName("Number")[0].InnerText)
                    {
                        return false;
                    }
                }
            }

            XmlElement userElement = xmlDoc.CreateElement("User");

            XmlAttribute userAttribute = xmlDoc.CreateAttribute("Key");
            userElement.SetAttributeNode(userAttribute);
            userAttribute.Value = id;

            XmlElement NameElement = xmlDoc.CreateElement("Name");
            NameElement.InnerText = name;
            userElement.AppendChild(NameElement);

            XmlElement IdElement = xmlDoc.CreateElement("Id");
            IdElement.InnerText = id;
            userElement.AppendChild(IdElement);

            XmlElement PwElement = xmlDoc.CreateElement("Pw");
            PwElement.InnerText = pw;
            userElement.AppendChild(PwElement);

            XmlElement SerialElement = xmlDoc.CreateElement("Serial");
            SerialElement.InnerText = serial;
            userElement.AppendChild(SerialElement);

            XmlElement TypeElement = xmlDoc.CreateElement("Type");
            TypeElement.InnerText = sort;
            userElement.AppendChild(TypeElement);

            XmlElement NumberElement = xmlDoc.CreateElement("Number");
            NumberElement.InnerText = number;
            userElement.AppendChild(NumberElement);

            MembersElement.AppendChild(userElement);

            xmlDoc.AppendChild(MembersElement);
            xmlDoc.Save(UsbSave);

            return true;
        }
        #endregion

        #region xml 검색
        static public bool Xml_Find(string serial)
        {
            Load(0);

            XmlNodeList bookList = xmlDoc.GetElementsByTagName("User");

            foreach (XmlNode node in bookList)
            {
                XmlElement IdElement = (XmlElement)node;

                if (serial == IdElement.GetElementsByTagName("Serial")[0].InnerText)
                {
                    return true;
                }
            }
            return false;
        }

        static public string Xml_FindName(string serial)
        {
            Load(0);

            XmlNodeList bookList = xmlDoc.GetElementsByTagName("User");

            foreach (XmlNode node in bookList)
            {
                XmlElement IdElement = (XmlElement)node;

                if (serial == IdElement.GetElementsByTagName("Serial")[0].InnerText)
                {
                    return IdElement.GetElementsByTagName("Name")[0].InnerText;
                }
            }
            return null;
        }

        static public string Xml_FindtoID(string id)
        {
            Load(0);

            XmlNodeList bookList = xmlDoc.GetElementsByTagName("User");

            foreach (XmlNode node in bookList)
            {
                XmlElement IdElement = (XmlElement)node;

                if (id == IdElement.Attributes["Key"].InnerText)
                {
                    string userinfo = IdElement.GetElementsByTagName("Name")[0].InnerText + "\a"
                                    + IdElement.GetElementsByTagName("Serial")[0].InnerText + "\a"
                                    + IdElement.GetElementsByTagName("Type")[0].InnerText;

                    return userinfo;

                }
            }
            return null;
        }

        static public string Xml_FindType(string serial)
        {
            Load(0);

            XmlNodeList bookList = xmlDoc.GetElementsByTagName("User");

            foreach (XmlNode node in bookList)
            {
                XmlElement IdElement = (XmlElement)node;

                if (serial == IdElement.GetElementsByTagName("Serial")[0].InnerText)
                {
                    return IdElement.GetElementsByTagName("Type")[0].InnerText;
                }
            }
            return null;
        }

        static public string Xml_FindUserInfo(string serial)
        {
            Load(0);

            string userinfo;

            XmlNodeList List = xmlDoc.GetElementsByTagName("User");

            foreach (XmlNode node in List)
            {
                XmlElement IdElement = (XmlElement)node;

                if (serial == IdElement.GetElementsByTagName("Serial")[0].InnerText)
                {
                    return userinfo = IdElement.GetElementsByTagName("Name")[0].InnerText + "\a"
                                    + IdElement.GetElementsByTagName("Type")[0].InnerText + "\a"
                                    + IdElement.GetElementsByTagName("Id")[0].InnerText;
                }
            }
            return null;
        }
        #endregion

        #region xml 수정
        static public bool Xml_ReJoin(string serial, string id, string pw)
        {
            Load(0);

            XmlNodeList bookList = xmlDoc.GetElementsByTagName("User");

            foreach (XmlNode node in bookList)
            {
                XmlElement IdElement = (XmlElement)node;

                if (id == IdElement.Attributes["Key"].InnerText && pw == IdElement.GetElementsByTagName("Pw")[0].InnerText)
                {
                    IdElement.GetElementsByTagName("Serial")[0].InnerText = serial;

                    xmlDoc.AppendChild(MembersElement);
                    xmlDoc.Save(UsbSave);

                    return true;
                }
            }
            return false;
        }
        #endregion

        #region ID 로그인
        static public bool Login(string id, string pw)
        {
            Load(0);

            XmlNodeList List = xmlDoc.GetElementsByTagName("User");

            foreach (XmlNode node in List)
            {
                XmlElement memberElement = (XmlElement)node;

                if (memberElement.HasAttributes)
                {
                    if (id == memberElement.Attributes["Key"].InnerText && pw == memberElement.GetElementsByTagName("Pw")[0].InnerText)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        #endregion

        #region 과제 관련 함수
        //강사 write
        static public bool WriteSubmissionSugesstXml(string[] SubmissionInfo)
        {
            Load(1);

            XmlElement MainElement = xmlDoc.CreateElement("Suggest");

            XmlAttribute SubjectAttribute = xmlDoc.CreateAttribute("Subject");
            MainElement.SetAttributeNode(SubjectAttribute);
            SubjectAttribute.Value = SubmissionInfo[1];

            XmlElement NameElement = xmlDoc.CreateElement("Name");
            NameElement.InnerText = SubmissionInfo[0];
            MainElement.AppendChild(NameElement);

            XmlElement SubjectElement = xmlDoc.CreateElement("Subject");
            SubjectElement.InnerText = SubmissionInfo[1];
            MainElement.AppendChild(SubjectElement);

            XmlElement TitleElement = xmlDoc.CreateElement("Title");
            TitleElement.InnerText = SubmissionInfo[2];
            MainElement.AppendChild(TitleElement);

            XmlElement EndDateElement = xmlDoc.CreateElement("EndDate");
            EndDateElement.InnerText = SubmissionInfo[3];
            MainElement.AppendChild(EndDateElement);

            XmlElement FileNameElement = xmlDoc.CreateElement("FileName");
            FileNameElement.InnerText = SubmissionInfo[4];
            MainElement.AppendChild(FileNameElement);

            XmlElement TextMemoElement = xmlDoc.CreateElement("TextMemo");
            TextMemoElement.InnerText = SubmissionInfo[5];
            MainElement.AppendChild(TextMemoElement);

            XmlElement WriteIDElement = xmlDoc.CreateElement("ID");
            WriteIDElement.InnerText = SubmissionInfo[6];
            MainElement.AppendChild(WriteIDElement);

            XmlElement WriteDateElement = xmlDoc.CreateElement("WriteDate");
            WriteDateElement.InnerText = SubmissionInfo[7];
            MainElement.AppendChild(WriteDateElement);

            MembersElement.AppendChild(MainElement);

            xmlDoc.AppendChild(MembersElement);
            xmlDoc.Save(SugesstSubmissionList);

            return true;
        }

        static public ArrayList ReadSubmissionSugesstXml()
        {
            Load(1);

            string SubmissionInfo;

            XmlNodeList Submission = xmlDoc.GetElementsByTagName("Suggest");

            foreach (XmlNode node in Submission)
            {
                XmlElement SubmissionInfoElement = (XmlElement)node;

                if (SubmissionInfoElement.HasAttributes)
                {
                    SubmissionInfo = SubmissionInfoElement.GetElementsByTagName("Name")[0].InnerText + "\a"
                                   + SubmissionInfoElement.GetElementsByTagName("Subject")[0].InnerText + "\a"
                                   + SubmissionInfoElement.GetElementsByTagName("Title")[0].InnerText + "\a"
                                   + SubmissionInfoElement.GetElementsByTagName("EndDate")[0].InnerText + "\a"
                                   + SubmissionInfoElement.GetElementsByTagName("FileName")[0].InnerText + "\a"
                                   + SubmissionInfoElement.GetElementsByTagName("TextMemo")[0].InnerText + "\a"
                                   + SubmissionInfoElement.GetElementsByTagName("ID")[0].InnerText + "\a"
                                   + SubmissionInfoElement.GetElementsByTagName("WriteDate")[0].InnerText;

                    SubmissionAllData(SubmissionInfo);
                }
            }

            return submissionarraylist;
        }

        static public void SubmissionAllData(string info)
        {
            submissionarraylist.Add(info);
        }

        static public bool Xml_DeleteSubmission(string id, string title)
        {
            Load(1);

            XmlNodeList bookList = xmlDoc.GetElementsByTagName("Suggest");

            foreach (XmlNode node in bookList)
            {
                XmlElement IdElement = (XmlElement)node;

                if (id == IdElement.GetElementsByTagName("ID")[0].InnerText && title == IdElement.GetElementsByTagName("Title")[0].InnerText)
                {
                    IdElement.RemoveAll();
                    IdElement.ParentNode.RemoveChild(IdElement);
                    xmlDoc.Save(SugesstSubmissionList);

                    return true;
                }
            }
            return false;
        }

        static public bool WriteSubmitSubmissionXml(string teacherid, string[] SubmissionInfo)
        {
            SubmitSubmission = @"C:\SchoolInteligentUSB\SubmissionDirectory\SubmitSubmission\" + teacherid + ".xml";
            Load(2);

            XmlElement MainElement = xmlDoc.CreateElement("Submission");

            XmlAttribute SubjectAttribute = xmlDoc.CreateAttribute("Teacher");
            MainElement.SetAttributeNode(SubjectAttribute);
            SubjectAttribute.Value = teacherid;

            XmlElement NameElement = xmlDoc.CreateElement("Name");
            NameElement.InnerText = SubmissionInfo[0];
            MainElement.AppendChild(NameElement);

            XmlElement SubjectElement = xmlDoc.CreateElement("Subject");
            SubjectElement.InnerText = SubmissionInfo[1];
            MainElement.AppendChild(SubjectElement);

            XmlElement TitleElement = xmlDoc.CreateElement("Title");
            TitleElement.InnerText = SubmissionInfo[2];
            MainElement.AppendChild(TitleElement);

            XmlElement EndDateElement = xmlDoc.CreateElement("EndDate");
            EndDateElement.InnerText = SubmissionInfo[3];
            MainElement.AppendChild(EndDateElement);

            XmlElement FileNameElement = xmlDoc.CreateElement("FileName");
            FileNameElement.InnerText = SubmissionInfo[4];
            MainElement.AppendChild(FileNameElement);

            XmlElement TextMemoElement = xmlDoc.CreateElement("TextMemo");
            TextMemoElement.InnerText = SubmissionInfo[5];
            MainElement.AppendChild(TextMemoElement);

            XmlElement WriteIDElement = xmlDoc.CreateElement("ID");
            WriteIDElement.InnerText = SubmissionInfo[6];
            MainElement.AppendChild(WriteIDElement);

            XmlElement WriteDateElement = xmlDoc.CreateElement("WriteDate");
            WriteDateElement.InnerText = SubmissionInfo[7];
            MainElement.AppendChild(WriteDateElement);

            MembersElement.AppendChild(MainElement);

            xmlDoc.AppendChild(MembersElement);
            xmlDoc.Save(SubmitSubmission);

            return true;
        }

        static public ArrayList ReadSubmitSubmissionXml(string teacherid)
        {
            SubmitSubmission = @"C:\SchoolInteligentUSB\SubmissionDirectory\SubmitSubmission\" + teacherid + ".xml";
            Load(2);

            string SubmissionInfo;

            XmlNodeList Submission = xmlDoc.GetElementsByTagName("Submission");

            foreach (XmlNode node in Submission)
            {
                XmlElement SubmissionInfoElement = (XmlElement)node;

                if (SubmissionInfoElement.HasAttributes)
                {
                    SubmissionInfo = SubmissionInfoElement.GetElementsByTagName("Name")[0].InnerText + "\a"
                                   + SubmissionInfoElement.GetElementsByTagName("Subject")[0].InnerText + "\a"
                                   + SubmissionInfoElement.GetElementsByTagName("Title")[0].InnerText + "\a"
                                   + SubmissionInfoElement.GetElementsByTagName("EndDate")[0].InnerText + "\a"
                                   + SubmissionInfoElement.GetElementsByTagName("FileName")[0].InnerText + "\a"
                                   + SubmissionInfoElement.GetElementsByTagName("TextMemo")[0].InnerText + "\a"
                                   + SubmissionInfoElement.GetElementsByTagName("ID")[0].InnerText + "\a"
                                   + SubmissionInfoElement.GetElementsByTagName("WriteDate")[0].InnerText;

                    SubmitAllData(SubmissionInfo);
                }
            }

            return submissionarraylist;
        }

        static public void SubmitAllData(string info)
        {
            submissionarraylist.Add(info);
        }

        static public bool Xml_DeleteSumitSubmission(string id, string title)
        {
            Load(2);

            XmlNodeList bookList = xmlDoc.GetElementsByTagName("Submission");

            foreach (XmlNode node in bookList)
            {
                XmlElement IdElement = (XmlElement)node;

                if (id == IdElement.GetElementsByTagName("ID")[0].InnerText && title == IdElement.GetElementsByTagName("Title")[0].InnerText)
                {
                    IdElement.RemoveAll();
                    IdElement.ParentNode.RemoveChild(IdElement);
                    xmlDoc.Save(SubmitSubmission);

                    return true;
                }
            }
            return false;
        }

        static public bool WritePersonalStudentSubmissionXml(string studentid, string[] SubmissionInfo)
        {
            StudentPersonalSumitinfo = @"C:\SchoolInteligentUSB\SubmissionDirectory\StudentPersonalSumitinfo\" + studentid + ".xml";
            Load(3);

            XmlElement MainElement = xmlDoc.CreateElement("PersonalSumitInfo");

            XmlAttribute SubjectAttribute = xmlDoc.CreateAttribute("UserID");
            MainElement.SetAttributeNode(SubjectAttribute);
            SubjectAttribute.Value = studentid;

            XmlElement NameElement = xmlDoc.CreateElement("Name");
            NameElement.InnerText = SubmissionInfo[0];
            MainElement.AppendChild(NameElement);

            XmlElement SubjectElement = xmlDoc.CreateElement("Subject");
            SubjectElement.InnerText = SubmissionInfo[1];
            MainElement.AppendChild(SubjectElement);

            XmlElement TitleElement = xmlDoc.CreateElement("Title");
            TitleElement.InnerText = SubmissionInfo[2];
            MainElement.AppendChild(TitleElement);

            XmlElement EndDateElement = xmlDoc.CreateElement("EndDate");
            EndDateElement.InnerText = SubmissionInfo[3];
            MainElement.AppendChild(EndDateElement);

            XmlElement FileNameElement = xmlDoc.CreateElement("FileName");
            FileNameElement.InnerText = SubmissionInfo[4];
            MainElement.AppendChild(FileNameElement);

            XmlElement TextMemoElement = xmlDoc.CreateElement("TextMemo");
            TextMemoElement.InnerText = SubmissionInfo[5];
            MainElement.AppendChild(TextMemoElement);

            XmlElement WriteIDElement = xmlDoc.CreateElement("ID");
            WriteIDElement.InnerText = SubmissionInfo[6];
            MainElement.AppendChild(WriteIDElement);

            XmlElement WriteDateElement = xmlDoc.CreateElement("WriteDate");
            WriteDateElement.InnerText = SubmissionInfo[7];
            MainElement.AppendChild(WriteDateElement);

            MembersElement.AppendChild(MainElement);

            xmlDoc.AppendChild(MembersElement);
            xmlDoc.Save(StudentPersonalSumitinfo);

            return true;
        }

        static public ArrayList ReadPersonalStudentSubmissionXml(string studentid)
        {
            StudentPersonalSumitinfo = @"C:\SchoolInteligentUSB\SubmissionDirectory\StudentPersonalSumitinfo\" + studentid + ".xml";
            Load(3);

            string SubmissionInfo;

            XmlNodeList Submission = xmlDoc.GetElementsByTagName("PersonalSumitInfo");

            foreach (XmlNode node in Submission)
            {
                XmlElement SubmissionInfoElement = (XmlElement)node;

                if (SubmissionInfoElement.HasAttributes)
                {
                    SubmissionInfo = SubmissionInfoElement.GetElementsByTagName("Name")[0].InnerText + "\a"
                                   + SubmissionInfoElement.GetElementsByTagName("Subject")[0].InnerText + "\a"
                                   + SubmissionInfoElement.GetElementsByTagName("Title")[0].InnerText + "\a"
                                   + SubmissionInfoElement.GetElementsByTagName("EndDate")[0].InnerText + "\a"
                                   + SubmissionInfoElement.GetElementsByTagName("FileName")[0].InnerText + "\a"
                                   + SubmissionInfoElement.GetElementsByTagName("TextMemo")[0].InnerText + "\a"
                                   + SubmissionInfoElement.GetElementsByTagName("ID")[0].InnerText + "\a"
                                   + SubmissionInfoElement.GetElementsByTagName("WriteDate")[0].InnerText;

                    PersonalStudentSumitAllInfo(SubmissionInfo);
                }
            }

            return submissionarraylist;
        }

        static public void PersonalStudentSumitAllInfo(string info)
        {
            submissionarraylist.Add(info);
        }

        static public bool Xml_DeletePersonalStudentInfo(string id, string title)
        {
            Load(3);

            XmlNodeList bookList = xmlDoc.GetElementsByTagName("PersonalSumitInfo");

            foreach (XmlNode node in bookList)
            {
                XmlElement IdElement = (XmlElement)node;

                if (id == IdElement.GetElementsByTagName("ID")[0].InnerText && title == IdElement.GetElementsByTagName("Title")[0].InnerText)
                {
                    IdElement.RemoveAll();
                    IdElement.ParentNode.RemoveChild(IdElement);
                    xmlDoc.Save(StudentPersonalSumitinfo);

                    return true;
                }
            }
            return false;
        }

        static public bool ReadSubmissionBoardWrite(string id, string title, string date)
        {
            Load(5);

            XmlNodeList List = xmlDoc.GetElementsByTagName("User");

            foreach (XmlNode node in List)
            {
                XmlElement userElement = (XmlElement)node;

                if (userElement.HasAttributes)
                {
                    if (id == userElement.Attributes["Id"].InnerText)
                    {
                        foreach (XmlElement nuserElement in userElement)
                        {
                            if (title == nuserElement.InnerText && date == nuserElement.Attributes["Date"].InnerText)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            XmlElement UserElement = xmlDoc.CreateElement("User");

            XmlAttribute IdAttribute = xmlDoc.CreateAttribute("Id");
            UserElement.SetAttributeNode(IdAttribute);
            IdAttribute.Value = id;

            XmlElement TitleElement = xmlDoc.CreateElement("Title");
            UserElement.AppendChild(TitleElement);
            TitleElement.InnerText = title;

            XmlAttribute dateAttribute = xmlDoc.CreateAttribute("Date");
            TitleElement.SetAttributeNode(dateAttribute);
            dateAttribute.InnerText = date;

            MembersElement.AppendChild(UserElement);

            xmlDoc.AppendChild(MembersElement);
            xmlDoc.Save(readsubmissionboardlist);

            return true;
        }

        static public bool ReadSubmissionBoardCheck(string id, string title, string date)
        {
            Load(5);

            XmlNodeList List = xmlDoc.GetElementsByTagName("User");

            foreach (XmlNode node in List)
            {
                XmlElement userElement = (XmlElement)node;

                if (userElement.HasAttributes)
                {
                    if (id == userElement.Attributes["Id"].InnerText)
                    {
                        foreach (XmlElement nuserElement in userElement)
                        {
                            if (title == nuserElement.InnerText && date == nuserElement.Attributes["Date"].InnerText)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        #endregion

        #region 학생게시판 관련 함수
        static public bool StudentBoardWrite(string[] boardinfo)
        {
            Load(4);

            XmlElement MainElement = xmlDoc.CreateElement("Board");

            XmlAttribute SubjectAttribute = xmlDoc.CreateAttribute("ID");
            MainElement.SetAttributeNode(SubjectAttribute);
            SubjectAttribute.Value = boardinfo[0];

            XmlElement TitleElement = xmlDoc.CreateElement("Title");
            TitleElement.InnerText = boardinfo[1];
            MainElement.AppendChild(TitleElement);

            XmlElement IDElement = xmlDoc.CreateElement("ID");
            IDElement.InnerText = boardinfo[0];
            MainElement.AppendChild(IDElement);

            XmlElement NameElement = xmlDoc.CreateElement("Name");
            NameElement.InnerText = boardinfo[2];
            MainElement.AppendChild(NameElement);

            XmlElement FileNameElement = xmlDoc.CreateElement("FileName");
            FileNameElement.InnerText = boardinfo[3];
            MainElement.AppendChild(FileNameElement);

            XmlElement TextMemoElement = xmlDoc.CreateElement("TextMemo");
            TextMemoElement.InnerText = boardinfo[4];
            MainElement.AppendChild(TextMemoElement);

            XmlElement WriteDateElement = xmlDoc.CreateElement("WriteDate");
            WriteDateElement.InnerText = boardinfo[5];
            MainElement.AppendChild(WriteDateElement);

            MembersElement.AppendChild(MainElement);

            xmlDoc.AppendChild(MembersElement);
            xmlDoc.Save(studentboardlist);

            return true;
        }

        static public ArrayList StudentBoardRead()
        {
            Load(4);

            string boardinfo;

            XmlNodeList Submission = xmlDoc.GetElementsByTagName("Board");

            foreach (XmlNode node in Submission)
            {
                XmlElement BoardListInfoElement = (XmlElement)node;

                if (BoardListInfoElement.HasAttributes)
                {
                    boardinfo = BoardListInfoElement.GetElementsByTagName("Title")[0].InnerText + "\a"
                              + BoardListInfoElement.GetElementsByTagName("ID")[0].InnerText + "\a"
                              + BoardListInfoElement.GetElementsByTagName("Name")[0].InnerText + "\a"
                              + BoardListInfoElement.GetElementsByTagName("FileName")[0].InnerText + "\a"
                              + BoardListInfoElement.GetElementsByTagName("TextMemo")[0].InnerText + "\a"
                              + BoardListInfoElement.GetElementsByTagName("WriteDate")[0].InnerText;

                    BoardAllData(boardinfo);
                }
            }

            return studentboard;
        }

        static public void BoardAllData(string info)
        {
            studentboard.Add(info);
        }

        static public bool Xml_DeleteStudentBoard(string id, string title)
        {
            Load(4);

            XmlNodeList bookList = xmlDoc.GetElementsByTagName("Board");

            foreach (XmlNode node in bookList)
            {
                XmlElement IdElement = (XmlElement)node;

                if (id == IdElement.GetElementsByTagName("ID")[0].InnerText && title == IdElement.GetElementsByTagName("Title")[0].InnerText)
                {
                    IdElement.RemoveAll();
                    IdElement.ParentNode.RemoveChild(IdElement);
                    xmlDoc.Save(studentboardlist);

                    return true;
                }
            }
            return false;
        }

        static public bool ReadStudentBoardWrite(string id, string title, string date)
        {
            Load(6);

            XmlNodeList List = xmlDoc.GetElementsByTagName("User");

            foreach (XmlNode node in List)
            {
                XmlElement userElement = (XmlElement)node;

                if (userElement.HasAttributes)
                {
                    if (id == userElement.Attributes["Id"].InnerText)
                    {
                        foreach (XmlElement nuserElement in userElement)
                        {
                            if (title == nuserElement.InnerText && date == nuserElement.Attributes["Date"].InnerText)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            XmlElement UserElement = xmlDoc.CreateElement("User");

            XmlAttribute IdAttribute = xmlDoc.CreateAttribute("Id");
            UserElement.SetAttributeNode(IdAttribute);
            IdAttribute.Value = id;

            XmlElement TitleElement = xmlDoc.CreateElement("Title");
            UserElement.AppendChild(TitleElement);
            TitleElement.InnerText = title;

            XmlAttribute dateAttribute = xmlDoc.CreateAttribute("Date");
            TitleElement.SetAttributeNode(dateAttribute);
            dateAttribute.InnerText = date;

            MembersElement.AppendChild(UserElement);

            xmlDoc.AppendChild(MembersElement);
            xmlDoc.Save(readstudentboardlist);

            return true;
        }

        static public bool ReadStudentBoardCheck(string id, string title, string date)
        {
            Load(6);

            XmlNodeList List = xmlDoc.GetElementsByTagName("User");

            foreach (XmlNode node in List)
            {
                XmlElement userElement = (XmlElement)node;

                if (userElement.HasAttributes)
                {
                    if (id == userElement.Attributes["Id"].InnerText)
                    {
                        foreach (XmlElement nuserElement in userElement)
                        {
                            if (title == nuserElement.InnerText && date == nuserElement.Attributes["Date"].InnerText)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }
        #endregion

        #region 참고자료 관련 함수
        static public bool UploadedFileWrite(string filename, string lastdate)
        {
            Load(7);

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
                            xmlDoc.Save(uploadedfilelist);
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
            xmlDoc.Save(uploadedfilelist);

            return true;
        }

        static public ArrayList UploadedFileRead()
        {
            Load(7);

            XmlNodeList List = xmlDoc.GetElementsByTagName("File");
            string fileinfo;
            ArrayList arrlist = new ArrayList();
            foreach (XmlNode node in List)
            {
                XmlElement fileElement = (XmlElement)node;

                if (fileElement.HasChildNodes)
                {
                    foreach (XmlElement nfileElement in fileElement)
                    {
                        fileinfo = nfileElement.Attributes["Name"].InnerText + "\a"
                                 + nfileElement.InnerText;
                        arrlist.Add(fileinfo);
                    }
                }
            }
            return arrlist;
        }

        static public bool UploadedBookWrite(string filename, string lastdate)
        {
            Load(9);

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
                            xmlDoc.Save(uploadedbooklist);
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
            xmlDoc.Save(uploadedbooklist);

            return true;
        }

        static public ArrayList UploadedBookRead()
        {
            Load(9);

            XmlNodeList List = xmlDoc.GetElementsByTagName("File");
            string fileinfo;
            ArrayList arrlist = new ArrayList();
            foreach (XmlNode node in List)
            {
                XmlElement fileElement = (XmlElement)node;

                if (fileElement.HasChildNodes)
                {
                    foreach (XmlElement nfileElement in fileElement)
                    {
                        fileinfo = nfileElement.Attributes["Name"].InnerText + "\a"
                                 + nfileElement.InnerText;
                        arrlist.Add(fileinfo);
                    }
                }
            }
            return arrlist;
        }
        #endregion

        #region No USB 관련 함수

        static public void NoUSBAllData(string info)
        {
            noUSB.Add(info);
        }

        static public bool NoUSBWrite(string[] noUSBinfo)
        {
            Load(8);

            XmlElement MainElement = xmlDoc.CreateElement("NoUSBinfo");

            XmlAttribute NoUSBAttribute = xmlDoc.CreateAttribute("ID");
            MainElement.SetAttributeNode(NoUSBAttribute);
            NoUSBAttribute.Value = noUSBinfo[0];

            XmlElement IDElement = xmlDoc.CreateElement("ID");
            IDElement.InnerText = noUSBinfo[0];
            MainElement.AppendChild(IDElement);

            XmlElement CompactFileElement = xmlDoc.CreateElement("CompactFile");
            CompactFileElement.InnerText = noUSBinfo[1];
            MainElement.AppendChild(CompactFileElement);

            XmlElement WriteDateElement = xmlDoc.CreateElement("WriteDate");
            WriteDateElement.InnerText = WriteDate;
            MainElement.AppendChild(WriteDateElement);

            MembersElement.AppendChild(MainElement);

            xmlDoc.AppendChild(MembersElement);
            xmlDoc.Save(noUSBlist);

            return true;
        }

        static public ArrayList NoUSBRead()
        {
            Load(8);

            string noUSBinfo;

            XmlNodeList MainNode = xmlDoc.GetElementsByTagName("NoUSBinfo");

            foreach (XmlNode node in MainNode)
            {
                XmlElement NoUSBinfoListInfoElement = (XmlElement)node;

                if (NoUSBinfoListInfoElement.HasAttributes)
                {
                    noUSBinfo = NoUSBinfoListInfoElement.GetElementsByTagName("ID")[0].InnerText + "\a"
                              + NoUSBinfoListInfoElement.GetElementsByTagName("CompactFile")[0].InnerText + "\a"
                              + NoUSBinfoListInfoElement.GetElementsByTagName("WriteDate")[0].InnerText;

                    NoUSBAllData(noUSBinfo);
                }
            }
            return noUSB;
        }
        #endregion

    }
}
