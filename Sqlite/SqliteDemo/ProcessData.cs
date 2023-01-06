/******************************************************************************************************************************************************
 author:laibaolin
 **********************************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class ProcessData : MonoBehaviour
{
    public Text[] Row1 = new Text[3];
    public Text[] Row2 = new Text[3];
    public Text[] Row3 = new Text[3];
    public Text[] Row4 = new Text[3];
    private string[,] m_tList = new string[4, 3];
    // Start is called before the first frame update
    private void Start()
    {
        SqliteMgrOld.SqliteMgr.SqlInit("TeamSource");
        //    //初始化数据表
        string[] head = { "ID", "RaceVideo", "RaceResult", "NavigateData" };
        string[] item = new string[4];
        string[,] AllItem = new string[4, 4]
        {
                { "1", "测试验证队伍1科目一第一场" , "科目一得分：90  ", "85" },
                 { "2","测试验证队伍2科目一第一场" , "科目一得分：60 ", "65" },
                  { "3","测试验证队伍3科目二第一场" , "科目二得分：70 ", "70" },
                   { "4","测试验证队伍4科目二第二场" , "科目二得分：90 ", "90" },
        };


        for (int i = 0; i < AllItem.GetLength(0); i++)
        {
            for (int j = 0; j < AllItem.GetLength(1); j++)
            {
                item[j] = AllItem[i, j];
                SqliteMgrOld.SqliteMgr.InsertValues("ProcessData", head, item);

            }

        }
        SqliteMgrOld.SqliteMgr.SQLiteClose();
    }
    private void Combine(int i, Text[] list)
    {
        for (int j = 0; j < list.Length; j++)
        {
            list[j].text = m_tList[i, j];
        }
    }
    private string[,] ListCombined()
    {
        Combine(0, Row1);
        Combine(1, Row2);
        Combine(2, Row3);
        Combine(3, Row4);
        return m_tList;
    }
    public void SelectProcessData()
    {
        SqliteMgrOld.SqliteMgr.SqlInit("TeamSource");

        //读取全部数据，对某个表而言
        List<object[]> listData = new List<object[]>();
        SqliteMgrOld.SqliteMgr.SelectAll("ProcessData", (IDataReader x) =>
        {
            var elements = new object[4];
            elements[0] = x["ID"].ToString();
            elements[1] = x["RaceVideo"].ToString();
            elements[2] = x["RaceResult"].ToString();
            elements[3] = x["NavigateData"].ToString();
            listData.Add(elements);
        });
        var ele = new object[4];
        for (int i = 0; i < m_tList.GetLength(0); i++)
        {
            for (int j = 0; j < m_tList.GetLength(1); j++)
            {
                ele = listData[i];
                m_tList[i, j] = ele[j + 1].ToString();//Text是UI类型，并不是数据类型，要存储数据必须要用数据类型；
            }
        }
        ListCombined();
        SqliteMgrOld.SqliteMgr.SQLiteClose();
    }
}
