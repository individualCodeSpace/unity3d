/******************************************************************************************************************************************************
 江西联创精密机电有限公司对本代码拥有全部权限，未经许可不得引用本代码任何内容，也不得用于非江西联创精密机电有限公司之外的任何商业和非商业项目。

 项目编号：
 软件名称：

 开发环境：
 运行环境：
 功能描述：

 原作者：邓通福
 完成日期：
 特别说明：委托就是方法类，顾名思义是把方法委托给类，在声明类的时候就能执行对应一些方法。一般的类里面只有一堆属性和方法，
 当某一类必须执行一些方法，但又不知道具体执行哪个方法时，就可以用委托，先把需要的方法全都放在委托类里
 然后实例化该类的时候指明要使用的方法名，然后在该实例中传入该方法需要的参数就可以实现委托类的方法调用了。

 修改者：
 完成日期：
 修改记录：
 如经过多次修改，则继续往下添加。
 **********************************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Test
{
    static int medium = 0;
    public static int[] SortArray(int[] array)
    {
        for (int i = array.GetUpperBound(0); i >= 0; i--)
        {
            for (int j = 0; j <= i; j++)
            {
                if (array[j] <= array[i])
                {
                    medium = array[j];
                    array[j] = array[i];
                    array[j] = medium;
                }
            }

        }
        return array;
    }
}
public class DelegateSort : MonoBehaviour
{
   
   
    int[] arr = { 1, 8, 9, 3, 4, 7, 0, 2, 8 };
     //定义委托
    public delegate int[] SortDelegate(int[] x);
     //声明委托变量并实例化委托
    SortDelegate myDelegate = new SortDelegate(Test.SortArray);
    delegate int[] mydele(int[] array);
   
    

        
        



    
       
    
}