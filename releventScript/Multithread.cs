/******************************************************************************************************************************************************
 江西联创精密机电有限公司对本代码拥有全部权限，未经许可不得引用本代码任何内容，也不得用于非江西联创精密机电有限公司之外的任何商业和非商业项目。

 项目编号：
 软件名称：

 开发环境：
 运行环境：
 功能描述：多线程代码框架

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
using UnityEngine;
using System.Threading;
using System.Linq;
using System.Text;
namespace ShareDate
{
    class Program
    {
        class ShareData
        {
            static int ShareDateInt = 0;
            public void ThreadFunc()
            {
                {
                    ++ShareDateInt;
                }
                System.Console.WriteLine(ShareDateInt);
            }
        }
        static void Main(string[] args)
        {
            ShareData shareDdataC = new ShareData();
            Thread[] ThreadArray = new Thread [5];
          for (int i = 0; i < 5; ++i)
            {
                ThreadArray[i] = new Thread(new ThreadStart(shareDdataC.ThreadFunc));
                ThreadArray[i].Start();
                Thread.Sleep(500);
            }
        }
    }
}
public class Multithread : MonoBehaviour
{
    
    private void Start()
    {
        

    

    }
  
}

