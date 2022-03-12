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

public class GetBeforeData : MonoBehaviour
{
    public Text[] List1 = new Text[4];
    public Text[] List2 = new Text[4];
    public Text[] List3 = new Text[4];
    public Text[] List4 = new Text[4];
    public Text[] List5 = new Text[4];
    public Text[] List6 = new Text[4];
    public Text[] List7 = new Text[4];
    private string[,] m_tList = new string[7,4];
    
   
    private void Start()
    {
        // Start is called before the first frame update

        //打开数据库
        SqliteMgrOld.SqliteMgr.SqlInit("TeamSource");
        //    //初始化数据表
        string[] head = { "ID", "SubjectMess", "TeamName", "EquipmentMess", "GradeDepend" };
        string[] item = new string[5];
        string[,] AllItem = new string[7, 5]
        {
                { "1","科目一：音频传输，科目二：时延统计", "测试验证队伍1" , "发射机，接收机，传输带宽", "数字传输误码率，模拟信号信噪比" },
                 { "2","科目一：音频传输，科目二：时延统计", "测试验证队伍2" , "发射机，接收机，传输带宽", "数字传输误码率，模拟信号信噪比" },
                  { "3","科目一：音频传输，科目二：时延统计", "测试验证队伍3" , "发射机，接收机，传输带宽", "数字传输误码率，模拟信号信噪比" },
                   { "4","科目一：音频传输，科目二：时延统计", "测试验证队伍4" , "发射机，接收机，传输带宽", "数字传输误码率，模拟信号信噪比" },
                    { "5","科目一：音频传输，科目二：时延统计", "测试验证队伍5" , "发射机，接收机，传输带宽", "数字传输误码率，模拟信号信噪比" },
                     { "6","科目一：音频传输，科目二：时延统计", "测试验证队伍6" , "发射机，接收机，传输带宽", "数字传输误码率，模拟信号信噪比" },
                      { "7","科目一：音频传输，科目二：时延统计", "测试验证队伍7" , "发射机，接收机，传输带宽", "数字传输误码率，模拟信号信噪比" }
        };

        
        for (int i = 0; i < AllItem.GetLength(0); i++)
        {
            for (int j = 0; j < AllItem.GetLength(1); j++)
            {
                item[j] = AllItem[i, j];
                SqliteMgrOld.SqliteMgr.InsertValues("BeforeData", head, item);

            }

        }
        SqliteMgrOld.SqliteMgr.SQLiteClose();
    }
    private void Combine(int i,Text[] list)
    {
        for (int j = 0; j < list.Length; j++)
        {
            list[j].text = m_tList[i, j];
        }
    }
    private void ListCombined()
    {
        Combine(0, List1);
        Combine(1, List2);
        Combine(2, List3);
        Combine(3, List4);
        Combine(4, List5);
        Combine(5, List6);
        Combine(6, List7);
    }
    public void SelectBeforeData()
    {
        //打开数据库
        SqliteMgrOld.SqliteMgr.SqlInit("TeamSource");

        //读取全部数据，对某个表而言
        List<object[]> listData = new List<object[]>();
        SqliteMgrOld.SqliteMgr.SelectAll("BeforeData", (IDataReader x) =>
        {
            var elements = new object[5];
            elements[0] = x["ID"].ToString();
            elements[1] = x["SubjectMess"].ToString();
            elements[2] = x["TeamName"].ToString();
            elements[3] = x["EquipmentMess"].ToString();
            elements[4] = x["GradeDepend"].ToString();
            listData.Add(elements);
        });
        var ele = new object[5];
        for(int i = 0; i < m_tList.GetLength(0); i++)
        {
            for(int j = 0; j < m_tList.GetLength(1); j++)
            {
               ele = listData[i];
               m_tList[i, j] = ele[j+1].ToString();//Text是UI类型，并不是数据类型，要存储数据必须要用数据类型；
            }
        }
        ListCombined();
        SqliteMgrOld.SqliteMgr.SQLiteClose();
    }
}
