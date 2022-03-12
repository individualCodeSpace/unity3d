using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Clock : MonoBehaviour
{
    //声明各种变量
    private int m_iHour = 0;
    private int m_iMinute = 0;
    private int m_iSecond = 0;
    //private float m_fTimer = 0;
   // private bool m_bEn = false;
    public Text Hour0;
    public Text Hour1;
    public Text Minute0;
    public Text Minute1;
    public Text Second0;
    public Text Second1;
    public string ClockTimeStr = string.Empty;

    private void OnEnable()
    {
        StartCoroutine("AddSecond");
    }
    private void OnDisable()
    {
        StopCoroutine("AddSecond");
    }
    private void Update()
    {
        //获取运行时间，时分秒定时器设计
        //m_fTimer += Time.deltaTime;
        // m_iSecond = (int)m_fTimer;
        if (m_iSecond >= 60)
        {
            //时间累计变量遇分进位需要减去60s
            // m_fTimer -= 60;
            m_iSecond = 0;
            m_iMinute++;
        }
        if (m_iMinute >= 60)
        {
            m_iMinute = 0;
            m_iHour++;

        }
        if (m_iHour >= 25)
        {
            m_iHour = 0;
        }
        //将时分秒变量按一定格式拼接成字符串
        ClockTimeStr = string.Format("{0:D2}:{1:D2}:{2:D2}", m_iHour, m_iMinute, m_iSecond);
        //拆分字符串以适应UI文本框
        Hour1.text = ClockTimeStr.Substring(0, 1);
        Hour0.text = ClockTimeStr.Substring(1, 1);
        Minute1.text = ClockTimeStr.Substring(3, 1);
        Minute0.text = ClockTimeStr.Substring(4, 1);
        Second1.text = ClockTimeStr.Substring(6, 1);
        Second0.text = ClockTimeStr.Substring(7, 1);
    }
    public void Clc()
    {
        Hour1.text = "0";
        Hour0.text = "0";
        Minute1.text = "0";
        Minute0.text = "0";
        Second1.text = "0";
        Second0.text = "0";
        m_iHour = 0;
        m_iMinute = 0;
        m_iSecond = 0;
        //m_fTimer = 0;
    }
    //public void IntTimer()
    //    {
    //        m_fTimer = (int)m_fTimer;
    //    }
    IEnumerator AddSecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            m_iSecond++;

        }
    }
}

