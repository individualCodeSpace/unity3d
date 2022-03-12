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
using UnityEngine;
using UnityEngine.UI;

public class VideoContral : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] MediaPlayer;
    public GameObject[] HugePlayer;
    public Button[] Switch;

    public void CloseAllSmall()
    {
        for (int i = 0; i < MediaPlayer.Length; i++)
        {
            MediaPlayer[i].GetComponent<RawImage>().enabled = false;
        }
    }
    public void OpenAllSmall()
    {
        for (int i = 0; i < MediaPlayer.Length; i++)
        {
            MediaPlayer[i].GetComponent<RawImage>().enabled = true;
        }
    }
    public void BtnPlayer1()
    {
        CloseAllSmall();
        HugePlayer[0].SetActive(true);
        
    }
    public void closePlayer1()
    {
        HugePlayer[0].SetActive(true);
    }
} 
