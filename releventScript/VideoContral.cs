/******************************************************************************************************************************************************
author:laibaolin
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
