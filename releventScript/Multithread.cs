/******************************************************************************************************************************************************
 author:laibaolin
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

