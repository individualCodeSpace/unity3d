using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeClock : MonoBehaviour
{
    //变量声明
    public Text DeHour0;
    public Text DeHour1;
    public Text DeMinute0;
    public Text DeMinute1;
    public Text DeSecond0;
    public Text DeSecond1;
    public Text EndInfo;
    public string TimerStr;
    private int m_iSecond = 0;
    private int m_iMinute = 1;
    private int m_iHour = 0;
    private GameObject m_objStop;
    private GameObject m_objNextRound;
    private bool m_bEn = false;
   // private float timer = 0;
   // private int i = 0;
    void OnEnable()
    {
        StartCoroutine("CountSecond");
    }
    private void OnDisable()
    {
        StopCoroutine("CountSecond");
    }
    private void Start()
    {
        m_objNextRound = GameObject.Find("NextRound");
        m_objStop = GameObject.Find("Stop");
    }
    private void Update()
    {
        //获取并累积游戏运行时间
        //
        //timer -= Time.deltaTime;
        //second = (int)timer;
        if (m_iSecond <= -1 && (m_iMinute>0 || m_iHour>0)  )
        {
            //i++;
            //if(i==1)
            //{
            //    minute--;
            //   // second = 59;
            //    timer += 61;
            //}
            //else
            
                m_iMinute--;
                //second = 59;
                m_iSecond += 60;
            
        }
        if(m_iMinute<=-1 && m_iHour>0)
        {
            m_iHour--;
            m_iMinute = 59;
        }
        if(m_iHour==0 && m_iMinute==0 && m_iSecond==0)
        {
            EndInfo.text = "比赛时间到！";
            m_bEn = true;
            m_objStop.GetComponent<Button>().interactable = false;
            m_objNextRound.GetComponent<Button>().interactable = true;
          
         }
        TimerStr = string.Format("{0:D2}:{1:D2}:{2:D2}", m_iHour, m_iMinute, m_iSecond);
        DeHour1.text = TimerStr.Substring(0, 1);
        DeHour0.text = TimerStr.Substring(1, 1);
        DeMinute1.text = TimerStr.Substring(3, 1);
        DeMinute0.text = TimerStr.Substring(4, 1);
        DeSecond1.text = TimerStr.Substring(6, 1);
        DeSecond0.text = TimerStr.Substring(7, 1);
    }
    public void Clc1()
    {
        DeHour1.text = "0";
        DeHour0.text = "0";
        DeMinute1.text = "0";
        DeMinute0.text = "0";
        DeSecond1.text = "0";
        DeSecond0.text = "0";
        m_iHour = 0;
        m_iMinute = 1;
        m_iSecond = 0;
    }
    IEnumerator CountSecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            m_iSecond--;
            if(m_bEn)
            {
                m_bEn = false;
                this.enabled = false;//使能必须放在此处不能放在update的判断里，否则update一帧过去没等m_bEn复位就结束了协程，导致下一次激活直接跳出循环。
                break;
            }
          
        }
    }
}