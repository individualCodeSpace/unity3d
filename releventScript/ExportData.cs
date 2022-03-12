/******************************************************************************************************************************************************
 江西联创精密机电有限公司对本代码拥有全部权限，未经许可不得引用本代码任何内容，也不得用于非江西联创精密机电有限公司之外的任何商业和非商业项目。

 项目编号：
 软件名称：

 开发环境：
 运行环境：
 功能描述：实现竞赛数据的序列化和反序列化，并导入导出为想要的格式

 原作者：赖宝林
 完成日期：
 特别说明：

 修改者：
 完成日期：
 修改记录：
 如经过多次修改，则继续往下添加。
 **********************************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using System.Xml;
using UnityEngine.UI;
using System;
using System.Runtime.InteropServices;
//namespace FileLSaveApplication
//{
//    class Program
//    {
//    static void Main(string[] args)
//        {
//            string[] names = new string[] { "Zara Ali", "Nuha Ali" };
//            using (StreamWriter sw = new StreamWriter("name.txt"))
//            {
//                foreach(string s in names)
//                {
//                    sw.WriteLine(s);
//                }
//            }
//            string line = "";
//            using (StreamReader sr = new StreamReader("names.txt"))
//            {
//                while((line = sr.ReadLine()) != null)
//                {
//                    Console.WriteLine(line);
//                }
//            }
//            Console.ReadKey();
//        }
//    }

//}
//namespace FileLoadApplication
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            try
//            {
//                using (StreamReader sr = new StreamReader(Application.dataPath + " / StreamingFile" + " / befData.xml"))
//                {
//                    string line;
//                    while((line = sr.ReadLine()) != null)
//                    {
//                        Console.WriteLine(line);
//                    }
//                }
//            }
//            catch(Exception e)
//            {
//                Console.WriteLine("111111");
//                Console.WriteLine(e.Message);
//            }
//            Console.ReadKey();
//        }
//    }

//}

public class ExportData : MonoBehaviour
{
    public Text Info;
    private GameObject m_objDropDown;
    [DllImport("User32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern int MessageBox(IntPtr handle, string message, string title, int type);

    private void Start()
    {
   
        m_objDropDown = GameObject.Find("Dropdown");
    }
    private void SaveBefData()
    {
        Save_BeforeData m_save = m_objDropDown.GetComponent<ItemOptions>().CreateSave1("TeamSource", "BeforeData");
        //创建XML文件的存储路径
        string filePath = Application.dataPath + "/StreamingFile" + "/befData.xml";
        //创建XML文档
        XmlDocument xmlDoc = new XmlDocument();
        //创建根节点，即最上层节点
        XmlElement root = xmlDoc.CreateElement("save");
        //设置根节点中的值
        root.SetAttribute("name", "saveBefData");

        //创建XmlElement
        XmlElement TeamMessage;
        XmlElement ID;
        XmlElement SubjectMess;
        XmlElement TeamName;
        XmlElement EquipmentMess;
        XmlElement GradeDepend;
       
        //遍历save中存储的数据，将数据转换成XML格式
        for (int i = 0; i < m_save.ID.Count; i++)
        {
            TeamMessage = xmlDoc.CreateElement("TeamMessage");
            ID = xmlDoc.CreateElement("ID");
            SubjectMess = xmlDoc.CreateElement("SubjectMess");
            TeamName = xmlDoc.CreateElement("TeamName");
            EquipmentMess = xmlDoc.CreateElement("EquipmentMess");
            GradeDepend = xmlDoc.CreateElement("GradeDepend");
            //设置InnerText值
            ID.InnerText = m_save.ID[i].ToString();
            SubjectMess.InnerText = m_save.SubjectMess[i].ToString();
            TeamName.InnerText = m_save.TeamName[i].ToString();
            EquipmentMess.InnerText = m_save.EquipmentMess[i].ToString();
            GradeDepend.InnerText = m_save.GradeDepend[i].ToString();

            //设置节点间的层级关系 root -- TeamMessage -- (ID,TeamName,Subject,Round,Task,Grade)
            TeamMessage.AppendChild(ID);
            TeamMessage.AppendChild(SubjectMess);
            TeamMessage.AppendChild(TeamName);
            TeamMessage.AppendChild(EquipmentMess);
            TeamMessage.AppendChild(GradeDepend);
            root.AppendChild(TeamMessage);
        }

        xmlDoc.AppendChild(root);
        xmlDoc.Save(filePath);

        if (File.Exists(filePath))
        {
            Info.text = "保存成功";
        }
        if (MessageBox(IntPtr.Zero, "是否打开保存文档？", "温馨提示", 1) == 1)
        {
            gameObject.GetComponent<OpenFile>().OpenFileWin(Application.dataPath + "/StreamingFile");
        }
    }

    private void LoadBefData()
    {
        string filePath = Application.dataPath + "/StreamingFile" + "/befData.xml";
        if (File.Exists(filePath))
        {
            //加载XML文档
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            // 通过节点名称来获取元素，结果为XmlNodeList类型
            XmlNodeList TeamMessages = xmlDoc.GetElementsByTagName("TeamMessage");

            //遍历所有的TeamMessage节点，并获得子节点和子节点的InnerText
            if (TeamMessages.Count != 0)
            {
                int j = 0;
                string[] head = { "ID", "SubjectMess", "TeamName", "EquipmentMess", "GradeDepend" };
                string[] item = new string[head.Length];
                string[,] m_strAllValue = new string[TeamMessages.Count, head.Length];
                foreach (XmlNode TeamMessage in TeamMessages)
                {

                    //将每一个节点的所有子节点内容遍历存放至一个二维数组准备导入渲染
                    for (int i = 0; i < TeamMessage.ChildNodes.Count; i++)
                    {
                        m_strAllValue[j, i] = TeamMessage.ChildNodes[i].InnerText.ToString();
                    }
                    j++;
                }
                //初始化数据库表头和每一条记录的容器

                //开始导入
                m_objDropDown.GetComponent<ItemOptions>().InsertWholeTable("TeamSource", "BeforeData", head, item, m_strAllValue);
                Info.text = "导入成功";

            }
        }
        else
        {
            Info.text = "存档文件不存在";
        }
    }
    private void SaveSummaryData()
    {
        Save_GradeData m_save = m_objDropDown.GetComponent<ItemOptions>().CreateSave2("TeamSource", "GradeData");
        //创建XML文件的存储路径
        string filePath = Application.dataPath + "/StreamingFile" + "/GradeData.xml";
        //创建XML文档
        XmlDocument xmlDoc = new XmlDocument();
        //创建根节点，即最上层节点
        XmlElement root = xmlDoc.CreateElement("save");
        //设置根节点中的值
        root.SetAttribute("name", "saveGradeData");

        //创建XmlElement
        XmlElement TeamMessage;
        XmlElement ID;
        XmlElement TeamName;
        XmlElement SubjectGrade;
        XmlElement Composite;

        //遍历save中存储的数据，将数据转换成XML格式
        for (int i = 0; i < m_save.ID.Count; i++)
        {
            TeamMessage = xmlDoc.CreateElement("TeamMessage");
            ID = xmlDoc.CreateElement("ID");
            TeamName = xmlDoc.CreateElement("TeamName");
            SubjectGrade = xmlDoc.CreateElement("SubjectGrade");
            Composite = xmlDoc.CreateElement("Composite");
            //设置InnerText值
            ID.InnerText = m_save.ID[i].ToString();
            TeamName.InnerText = m_save.TeamName[i].ToString();
            SubjectGrade.InnerText = m_save.SubjectGrade[i].ToString();
            Composite.InnerText = m_save.Composite[i].ToString();

            //设置节点间的层级关系 root -- TeamMessage -- (ID,TeamName,Subject,Round,Task,Grade)
            TeamMessage.AppendChild(ID);
            TeamMessage.AppendChild(TeamName);
            TeamMessage.AppendChild(SubjectGrade);
            TeamMessage.AppendChild(Composite);
            root.AppendChild(TeamMessage);
        }

        xmlDoc.AppendChild(root);
        xmlDoc.Save(filePath);

        if (File.Exists(filePath))
        {
            Info.text = "保存成功";
        }
    }
    private void LoadSummaryData()
    {
        string filePath = Application.dataPath + "/StreamingFile" + "/GradeData.xml";
        if (File.Exists(filePath))
        {
            //加载XML文档
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            // 通过节点名称来获取元素，结果为XmlNodeList类型
            XmlNodeList TeamMessages = xmlDoc.GetElementsByTagName("TeamMessage");

            //遍历所有的TeamMessage节点，并获得子节点和子节点的InnerText
            if (TeamMessages.Count != 0)
            {
                int j = 0;
                string[] head = { "ID", "TeamName", "SubjectGrade", "Composite" };
                string[] item = new string[head.Length];
                string[,] m_strAllValue = new string[TeamMessages.Count, head.Length];
                foreach (XmlNode TeamMessage in TeamMessages)
                {

                    //将每一个节点的所有子节点内容遍历存放至一个二维数组准备导入渲染
                    for (int i = 0; i < TeamMessage.ChildNodes.Count; i++)
                    {
                        m_strAllValue[j, i] = TeamMessage.ChildNodes[i].InnerText.ToString();
                    }
                    j++;
                }
                //初始化数据库表头和每一条记录的容器

                //开始导入
                m_objDropDown.GetComponent<ItemOptions>().InsertWholeTable("TeamSource", "GradeData", head, item, m_strAllValue);
                Info.text = "导入成功";

            }
        }
        else
        {
            Info.text = "存档文件不存在";
        }
    }
    private void SaveByXml()
    {
        Save_item m_save = m_objDropDown.GetComponent<ItemOptions>().CreateSaveGO("TeamSource", "item");
        //创建XML文件的存储路径
        string filePath = Application.dataPath + "/StreamingFile" + "/byXML.xml";
        //创建XML文档
        XmlDocument xmlDoc = new XmlDocument();
        //创建根节点，即最上层节点
        XmlElement root = xmlDoc.CreateElement("save");
        //设置根节点中的值
        root.SetAttribute("name", "saveFile1");

        //创建XmlElement
        XmlElement TeamMessage;
        XmlElement ID;
        XmlElement TeamName;
        XmlElement Subject;
        XmlElement Round;
        XmlElement Task;
        XmlElement Grade;

        //遍历save中存储的数据，将数据转换成XML格式
        for (int i = 0; i < m_save.ID.Count; i++)
        {
            TeamMessage = xmlDoc.CreateElement("TeamMessage");
            ID = xmlDoc.CreateElement("ID");
            TeamName = xmlDoc.CreateElement("TeamName");
            Subject = xmlDoc.CreateElement("Subject");
            Round = xmlDoc.CreateElement("Round");
            Task = xmlDoc.CreateElement("Task");
            Grade = xmlDoc.CreateElement("Grade");
            //设置InnerText值
            ID.InnerText = m_save.ID[i].ToString();
            TeamName.InnerText = m_save.TeamName[i].ToString();
            Subject.InnerText = m_save.Subject[i].ToString();
            Round.InnerText = m_save.Round[i].ToString();
            Task.InnerText = m_save.Task[i].ToString();
            Grade.InnerText = m_save.Grade[i].ToString();

            //设置节点间的层级关系 root -- TeamMessage -- (ID,TeamName,Subject,Round,Task,Grade)
            TeamMessage.AppendChild(ID);
            TeamMessage.AppendChild(TeamName);
            TeamMessage.AppendChild(Subject);
            TeamMessage.AppendChild(Round);
            TeamMessage.AppendChild(Task);
            TeamMessage.AppendChild(Grade);
            root.AppendChild(TeamMessage);
        }

        xmlDoc.AppendChild(root);
        xmlDoc.Save(filePath);

        if (File.Exists(filePath))
        {
            Info.text = "保存成功";
        }
    }
    private void LoadByXml()
    {
        string filePath = Application.dataPath + "/StreamingFile" + "/byXML.xml";
        if (File.Exists(filePath))
        {
            //加载XML文档
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            // 通过节点名称来获取元素，结果为XmlNodeList类型
             XmlNodeList TeamMessages = xmlDoc.GetElementsByTagName("TeamMessage");

            //遍历所有的TeamMessage节点，并获得子节点和子节点的InnerText
            if (TeamMessages.Count != 0)
            {
                int j = 0;
                string[] head = { "ID", "TeamName", "Subject", "Round", "Task", "grade" };
                string[] item = new string[head.Length];
                string[,] m_strAllValue = new string[TeamMessages.Count,head.Length];
                foreach (XmlNode TeamMessage in TeamMessages)
                {
                    
                    //将每一个节点的所有子节点内容遍历存放至一个二维数组准备导入渲染
                    for (int i = 0; i < TeamMessage.ChildNodes.Count; i++)
                    {
                        m_strAllValue[j, i] = TeamMessage.ChildNodes[i].InnerText.ToString();
                    }
                    j++;
                }
                //初始化数据库表头和每一条记录的容器
               
                //开始导入
                m_objDropDown.GetComponent<ItemOptions>().InsertWholeTable("TeamSource","item", head, item, m_strAllValue);
                Info.text = "导入成功";

            }
        }
        else
        {
            Info.text = "存档文件不存在";
        }
    }

    private void SaveByJson()
    {
        Save_item save = m_objDropDown.GetComponent<ItemOptions>().CreateSaveGO("TeamSource", "item");
        string filePath = Application.dataPath + "/StreamingFile" + "/byJson.json";
        //利用JsonMapper将save对象转换为Json格式的字符串
        string saveJsonStr = JsonMapper.ToJson(save);
        //将这个字符串写入到文件中
        //创建一个StreamWriter，并将字符串写入文件中
        StreamWriter sw = new StreamWriter(filePath);
        sw.Write(saveJsonStr);
        //关闭StreamWriter
        sw.Close();

       Info.text = "保存成功";
    }
    private void LoadByJson()
    {
        string filePath = Application.dataPath + "/StreamingFile" + "/byJson.json";
        if (File.Exists(filePath))
        {
            //创建一个StreamReader，用来读取流
            StreamReader sr = new StreamReader(filePath);
            //将读取到的流赋值给jsonStr
            string jsonStr = sr.ReadToEnd();
            //关闭
            sr.Close();

            //将字符串jsonStr转换为Save对象
            Save_item save = JsonMapper.ToObject<Save_item>(jsonStr);
            string[] head = { "ID", "TeamName", "Subject", "Round", "Task", "grade" };
            string[] item = new string[head.Length];
            string[,] AllValues = new string[save.ID.Count, head.Length];
            for (int i = 0; i < save.ID.Count; i++)
            {
                AllValues[i, 0] = save.ID[i].ToString();
                AllValues[i, 1] = save.TeamName[i].ToString();
                AllValues[i, 2] = save.Subject[i].ToString();
                AllValues[i, 3] = save.Round[i].ToString();
                AllValues[i, 4] = save.Task[i].ToString();
                AllValues[i, 5] = save.Grade[i].ToString();
            }
            //开始导入
            m_objDropDown.GetComponent<ItemOptions>().InsertWholeTable("TeamSource", "item", head, item, AllValues);
            Info.text = "导入成功";
        }
        else
        {
            Info.text = "存档文件不存在";
        }
    }

    public void ExportFile()
    {
        //选择合适的文件格式对应方法
        SaveByXml();
        SaveBefData();
        SaveSummaryData();
        //SaveByJson();
    }
    public void LoadFile()
    {
        //选择合适的文件格式对应方法
        LoadByXml();
        LoadBefData();
        LoadSummaryData();

        //LoadByJson();
    }
}
