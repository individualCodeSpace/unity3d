using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class StopGame : MonoBehaviour
{
   
    private GameObject m_objTimeCount;
    private GameObject m_objStop;
    private GameObject m_objPause;
    private GameObject m_objNextRound;
    private GameObject m_objContinue;
    private GameObject m_objStart;
    public GameObject Continue;
    // Start is called before the first frame update
    [DllImport("User32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern int MessageBox(IntPtr handle, string message, string title, int type);

    private void Start()
    {
        m_objTimeCount = GameObject.Find("TimeCount");
        m_objStop = GameObject.Find("Stop");
        m_objPause = GameObject.Find("Pause");
        m_objNextRound = GameObject.Find("NextRound");
        m_objStart = GameObject.Find("Start");
    }
    private void SetStop()
    {
        m_objStart.GetComponent<Button>().interactable = true;
        m_objNextRound.GetComponent<Button>().interactable = true;
        m_objPause.GetComponent<Button>().interactable = false;
        gameObject.GetComponent<Button>().interactable = false;
        this.Setac1();
        m_objStop.GetComponent<Display>().Play0();
    }
    public void BtnStopGame()
    {
        if (MessageBox(IntPtr.Zero, "是否结束这一轮？", "温馨提示", 1) == 1)
        {
            if (Continue.activeSelf == true)
            {
                GameObject.Find("Continue").GetComponent<ContinueGame>().OnClick1();
                this.SetStop();
            }
            else if(Continue.activeSelf == false)
            {
                this.SetStop();
            }
            
        }
    }
    public void Setac1()
    {

        StartCoroutine("DeActivate");
       
    }
    IEnumerator DeActivate()
    {
        yield return new WaitForSeconds(3);
      
        m_objTimeCount.GetComponent<Clock>().Clc();
        m_objTimeCount.GetComponent<DeClock>().Clc1();
        m_objTimeCount.GetComponent<Clock>().enabled = false;
        m_objTimeCount.GetComponent<DeClock>().enabled = false;
       
        }
    }
