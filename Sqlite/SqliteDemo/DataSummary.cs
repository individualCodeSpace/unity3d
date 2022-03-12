/******************************************************************************************************************************************************
 江西联创精密机电有限公司对本代码拥有全部权限，未经许可不得引用本代码任何内容，也不得用于非江西联创精密机电有限公司之外的任何商业和非商业项目。

 项目编号：
 软件名称：

 开发环境：
 运行环境：
 功能描述：

 原作者：邓通福
 完成日期：
 特别说明：

 修改者：
 完成日期：
 修改记录：
 如经过多次修改，则继续往下添加。
 **********************************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class DataSummary : MonoBehaviour
{
    public Text[] L1 = new Text[3];
    public Text[] L2 = new Text[3];
    public Text[] L3 = new Text[3];
    public Text[] L4 = new Text[3];
    public Text[] L5 = new Text[3];
    public Text[] L6 = new Text[3];
    public Text[] L7 = new Text[3];
    private string[,] m_tList = new string[7, 3];
    // Start is called before the first frame update
    private void Start()
    {
        SqliteMgrOld.SqliteMgr.SqlInit("TeamSource");
        //    //初始化数据表
        string[] head = { "ID", "TeamName", "SubjectGrade", "Composite" };
        string[] item = new string[4];
        string[,] AllItem = new string[7, 4]
        {
                { "1", "测试验证队伍1" , "科目一：90，科目二：75", "85" },
                 { "2","测试验证队伍2" , "科目一：60，科目二：70", "65" },
                  { "3","测试验证队伍3" , "科目一：70，科目二：70", "70" },
                   { "4","测试验证队伍4" , "科目一：90，科目二：90", "90" },
                    { "5","测试验证队伍5" , "科目一：95，科目二：70", "85" },
                     { "6","测试验证队伍6" , "科目一：100，科目二：95", "98" },
                      { "7","测试验证队伍7" , "科目一：100，科目二：100", "100" }
        };


        for (int i = 0; i < AllItem.GetLength(0); i++)
        {
            for (int j = 0; j < AllItem.GetLength(1); j++)
            {
                item[j] = AllItem[i, j];
                SqliteMgrOld.SqliteMgr.InsertValues("GradeData", head, item);

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
        Combine(0, L1);
        Combine(1, L2);
        Combine(2, L3);
        Combine(3, L4);
        Combine(4, L5);
        Combine(5, L6);
        Combine(6, L7);
        return m_tList;
    }
    public void SelectSummaryData()
    {
        //打开数据库
        SqliteMgrOld.SqliteMgr.SqlInit("TeamSource");
        //先对评分结果进行排序
        //读取全部数据，对某个表而言
        List<object[]> listData = new List<object[]>();
        SqliteMgrOld.SqliteMgr.SelectAndSort("GradeData","Composite","DESC", (IDataReader x) =>
        {
            var elements = new object[4];
            elements[0] = x["ID"].ToString();
            elements[1] = x["TeamName"].ToString();
            elements[2] = x["SubjectGrade"].ToString();
            elements[3] = x["Composite"].ToString();
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
