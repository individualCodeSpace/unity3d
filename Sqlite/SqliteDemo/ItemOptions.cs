using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.IO;
using System.Data;
using SqliteMgrOld;
using Mono.Data.Sqlite;
using UnityEngine.UI;

public class ItemOptions : MonoBehaviour
{
    public Text[] List = new Text[3];
    private string[] SelectStr = new string[7];
    private string Line;


    public class DataStr
    {
        public Int64 ID { set; get; }
        public string TeamName { set; get; }
        public string Subject { set; get; }
        public string Round { set; get; }
        public string Task { set; get; }
        public int grade { set; get; }

        //public override string ToString()
        //
           // return ID.ToString() + "-" + TeamName.ToString() + "-" + Subject.ToString() + "-" + Round.ToString() + "-" + Session.ToString() + "-" + OnTask.ToString() + "-" + NextTask.ToString();
        //}

    }
    public class DataStr1
    {
        public Int64 ID { set; get; }
        public string SubjectMess { set; get; }
        public string TeamName { set; get; }
        public string EquipmentMess { set; get; }
        public string GradeDepend { set; get; }
    }
    public class DataStr2
    {
        public Int64 ID { set; get; }
        public string TeamName { set; get; }
        public string SubjectGrade { set; get; }
        public int Composite { set; get; }
    }
    private void Awake()
    {
       
    //    //打开数据库
       SqliteMgrOld.SqliteMgr.SqlInit("TeamSource");
        //    //初始化数据表
        string[] head = { "ID", "TeamName", "Subject", "Round", "Task", "grade"};
        string[] item = new string[6];
        string[,] AllItem = new string[7, 6]
        {
                { "1", "测试验证队伍1", "科目一", "第一轮" , "音频传输", "10" },
                { "2", "测试验证队伍2", "科目一", "第一轮", "音频传输", "30" },
                { "3", "测试验证队伍3", "科目一", "第一轮","音频传输", "0" },
                { "4", "测试验证队伍4", "科目一", "第一轮", "音频传输", "0" },
                { "5", "披头士乐队", "科目五", "第五轮", "爵士乐", "0" },
                { "6", "甲壳虫乐队", "科目六", "第六轮", "古典乐", "0" },
                { "7", "classic_rock", "科目七", "第七轮", "music", "0"}
        };
        InsertWholeTable("TeamSource","item", head, item, AllItem);
        SqliteMgrOld.SqliteMgr.SQLiteClose();
    }
    private  void Start()
    {
        this.GetItem();
    }
    public void GetItem()
    {

        //打开数据库
        SqliteMgrOld.SqliteMgr.SqlInit("TeamSource");

        //读取全部数据，对某个表而言
        List<object[]> listData = new List<object[]>();
        SqliteMgrOld.SqliteMgr.SelectAll("item", (IDataReader x) =>
        {
            var elements = new object[6];
            elements[0] = x["ID"].ToString();
            elements[1] = x["TeamName"].ToString();
            elements[2] = x["Subject"].ToString();
            elements[3] = x["Round"].ToString();
            //elements[4] = x["Session"].ToString();
            //elements[5] = x["OnTask"].ToString();
            elements[4] = x["Task"].ToString();
            elements[5] = x["grade"].ToString();
            listData.Add(elements);
        });
        var ele = listData[gameObject.GetComponent<Dropdown>().value];//Dropdown.value也是从0开始。
        for (int j = 0; j < List.Length; j++)
        {
            List[j].text = ele[j + 2].ToString();
        }
        //方法二
        //List<DataStr> listData = new List<DataStr>();
        //SqliteMgrOld.SqliteMgr.SelectAll("item", (IDataReader x) =>
        //{
        //    DataStr data = new DataStr();
        //    data.TeamName = x["TeamName"].ToString();
        //    data.Subject = x["Subject"].ToString();
        //    data.Round = x["Round"].ToString();
        //    //data.Session = x["Session"].ToString();
        //    data.Task = x["Task"].ToString();
        //    data.grade =x["grade"].ToString();
        //    listData.Add(data);
        //});
        //DataStr dataTem = new DataStr();
        //dataTem = listData[gameObject.GetComponent<Dropdown>().value];
        //Debug.Log(gameObject.GetComponent<Dropdown>().value + "-" + dataTem);//能不能Object[i].[TeamName,Subject]...
        //List<string> daStr = new List<string>();
        //daStr.Add(dataTem.TeamName);
        //daStr.Add(dataTem.Subject);
        //daStr.Add(dataTem.Round);
        //daStr.Add(dataTem.Task);
        //daStr.Add(dataTem.grade);
        //int k = 0;
        //foreach(string daTem in daStr)
        //{
        //    List[k].text = daTem;
        //    k++;
        //}

        //Line = dataTem.ID.ToString() + " " + dataTem.TeamName.ToString() + " " + dataTem.Subject.ToString() + " " + dataTem.Round.ToString() + " " + dataTem.Session.ToString() + " " + dataTem.OnTask.ToString() + " " + dataTem.NextTask.ToString();
        //Debug.Log(Line);
        //SelectStr = Line.Split(' ');
        //for (int j = 0; j < List.Length; j++)
        //{
        //    List[j].text = SelectStr[j + 2];
        //}

        SqliteMgrOld.SqliteMgr.SQLiteClose();
    }

    public void InsertWholeTable(string dbName,string tabName, string[] head, string[] item, string[,] AllValues)
    {
        //打开数据库
        SqliteMgrOld.SqliteMgr.SqlInit(dbName);
        for (int i = 0; i < AllValues.GetLength(0); i++)
        {
            for (int j = 0; j < AllValues.GetLength(1); j++)
            {
                item[j] = AllValues[i,j];
                SqliteMgrOld.SqliteMgr.InsertValues(tabName, head, item);

            }
        }
        SqliteMgrOld.SqliteMgr.SQLiteClose();
    }
    public void GetOneItem()
    {
        SqliteMgrOld.SqliteMgr.SqlInit("TeamSource");
        DataStr data1 = new DataStr();
        SqliteMgrOld.SqliteMgr.SelectData("item", "ID", "==", (gameObject.GetComponent<Dropdown>().value+1).ToString(), ref data1);
        Line = data1.ID.ToString() +" "+ data1.TeamName.ToString() +" "+ data1.Subject.ToString() +" "+ data1.Round.ToString() +" "+ data1.Task.ToString();
        Debug.Log(Line);
        SelectStr = Line.Split(' ');//能不能foreach遍历对象的所有成员属性简化代码:C#高级编程反射内容
        for (int j = 0; j < List.Length; j++)
        {
            List[j].text = SelectStr[j + 2];
        }
        SqliteMgrOld.SqliteMgr.SQLiteClose();
        
    }
    public void SelectOneItem()
    {
        SqliteMgrOld.SqliteMgr.SqlInit("TeamSource");

        SqliteMgrOld.SqliteMgr.SQLiteClose();

    }
    public void DeleteItems()
    {
        SqliteMgrOld.SqliteMgr.SqlInit("TeamSource");
        bool isAnd = true;
        string[] columns = { "TeamName","ID" };
        string[] operations = {"==", "=="};
        string[] colValue = {"classic_rock","3" };
        SqliteMgrOld.SqliteMgr.DeleteValues("item", columns, operations, colValue,isAnd); 
    }
    public void  SelectFull()
    {
        SqliteMgrOld.SqliteMgr.SqlInit("TeamSource");
        SqliteMgrOld.SqliteMgr.ReadFullTable("item");
        SqliteMgrOld.SqliteMgr.SQLiteClose();
    }
    //用自定义的对象类型获取数据库中的数据，并将其压进Save格式中等待序列化。针对队伍信息数据表
    public Save_item CreateSaveGO(string dbName,string tableName)
    {
        Save_item save = new Save_item();
        SqliteMgrOld.SqliteMgr.SqlInit(dbName);
        List<DataStr> ListDataTem = SqliteMgrOld.SqliteMgr.GetAllValues<DataStr>(tableName);
        for (int i = 0; i < ListDataTem.Count; i++)
        { 
            save.ID.Add(ListDataTem[i].ID);
            save.TeamName.Add(ListDataTem[i].TeamName);
            save.Subject.Add(ListDataTem[i].Subject);
            save.Round.Add(ListDataTem[i].Round);
            save.Task.Add(ListDataTem[i].Task);
            save.Grade.Add(ListDataTem[i].grade);
        }
        SqliteMgrOld.SqliteMgr.SQLiteClose();
        return save;
    }
    //用自定义的对象类型获取数据库中的数据，并将其压进Save格式中等待序列化。针对赛前数据安排表
    public Save_BeforeData CreateSave1(string dbName, string tableName)
    {
        Save_BeforeData save = new Save_BeforeData();
        SqliteMgrOld.SqliteMgr.SqlInit(dbName);
        List<DataStr1> ListDataTem = SqliteMgrOld.SqliteMgr.GetAllValues<DataStr1>(tableName);
        for (int i = 0; i < ListDataTem.Count; i++)
        {
            save.ID.Add(ListDataTem[i].ID);
            save.SubjectMess.Add(ListDataTem[i].SubjectMess);
            save.TeamName.Add(ListDataTem[i].TeamName);
            save.EquipmentMess.Add(ListDataTem[i].EquipmentMess);
            save.GradeDepend.Add(ListDataTem[i].GradeDepend);
        }
        SqliteMgrOld.SqliteMgr.SQLiteClose();
        return save;
    }
    //用自定义的对象类型获取数据库中的数据，并将其压进Save格式中等待序列化。针对赛后数据统计表
    public Save_GradeData CreateSave2(string dbName, string tableName)
    {
        Save_GradeData save = new Save_GradeData();
        SqliteMgrOld.SqliteMgr.SqlInit(dbName);
        List<DataStr2> ListDataTem = SqliteMgrOld.SqliteMgr.GetAllValues<DataStr2>(tableName);
        for (int i = 0; i < ListDataTem.Count; i++)
        {
            save.ID.Add(ListDataTem[i].ID);
            save.TeamName.Add(ListDataTem[i].TeamName);
            save.SubjectGrade.Add(ListDataTem[i].SubjectGrade);
            save.Composite.Add(ListDataTem[i].Composite);
        }
        SqliteMgrOld.SqliteMgr.SQLiteClose();
        return save;
    }
    public void UpdateOneRecordByID( string dbName, string tableName, string colName, string colValue, int id)
    {    
        SqliteMgrOld.SqliteMgr.SqlInit(dbName);
        SqliteMgrOld.SqliteMgr.UpdateOneData(tableName, colName, colValue, "ID", "==",id.ToString());
        SqliteMgrOld.SqliteMgr.SQLiteClose();
    }
    public void ExportCsvFile(string dbName, string tableName, string path)
    {
        SqliteMgrOld.SqliteMgr.SqlInit(dbName);
        SqliteMgrOld.SqliteMgr.ExportFile(tableName, path);
        SqliteMgrOld.SqliteMgr.SQLiteClose();
    }
   
}





//问题1：相同名字的数据库辅助类为什么不会访问错误:命名空间不同
//问题2：数据库脚本放在哪个组件下都可以，不放行不行：有一个调用的脚本即可，在引用方法时会把方法所在脚本先初始化，执行awake()和start（）方法。
//问题3：字符串数组作参数问题
//问题4：条件查询语句使用报错问题，reader被占用问题