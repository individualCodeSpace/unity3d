using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class RoudUpdate : MonoBehaviour
{
    // Start is called before the first frame 
    private int i = 1;
    private int j = 0;
    private string[] m_sChineseNum = { "一", "二", "三" };
    private GameObject m_objDropdown;
    private GameObject m_objTimeCount;
    private GameObject m_objStart;
    private GameObject m_objStop;
    private GameObject m_objPause;
    public GameObject Continue;
    [DllImport("User32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern int MessageBox(IntPtr handle, string message, string title, int type);
    private void Start()
    {
       
        m_objDropdown = GameObject.Find("Dropdown");
        m_objTimeCount = GameObject.Find("TimeCount");
        m_objStart = GameObject.Find("Start");
        m_objStop = GameObject.Find("Stop");
        m_objPause = GameObject.Find("Pause");//只能找到ui中激活显示了的组件，未激活的找不到。

    }
    public void BtnNextRound()
    {
        if(MessageBox(IntPtr.Zero,"是否进入下一轮？","温馨提示",1)==1)
        {
            gameObject.GetComponent<Button>().interactable = false;
            if (Continue.activeSelf == true)
            {
                //初始状态Continue未激活，不能通过GameObject.Find函数找到
                GameObject.Find("Continue").GetComponent<ContinueGame>().OnClick1();
                NextRound();
            }
            else
            {
                NextRound();
            }
        }
    }
    private void TurnRound()
    {
        m_objTimeCount.GetComponent<Clock>().Clc();
        m_objTimeCount.GetComponent<DeClock>().Clc1();
        m_objTimeCount.GetComponent<Clock>().enabled = false;
        m_objTimeCount.GetComponent<DeClock>().enabled = false;
        m_objStart.GetComponent<Button>().interactable = true;
        m_objStop.GetComponent<Button>().interactable = false;
        m_objPause.GetComponent<Button>().interactable = false;
    }
    //更新场次信息并渲染
    private void UpdateRound(int i)
    {
       string colValue = "第" + m_sChineseNum[i] + "场";
       m_objDropdown.GetComponent<ItemOptions>().UpdateOneRecordByID("TeamSource","item", "Round", colValue, m_objDropdown.GetComponent<Dropdown>().value + 1);
       m_objDropdown.GetComponent<ItemOptions>().List[1].text = colValue;
    }
    //更新科目并渲染
    private void UpdateSubject(int i)
    {
        string colValue = ("科目" + m_sChineseNum[i]);
        m_objDropdown.GetComponent<ItemOptions>().UpdateOneRecordByID("TeamSource", "item", "Subject", colValue, m_objDropdown.GetComponent<Dropdown>().value + 1);
        m_objDropdown.GetComponent<ItemOptions>().List[0].text = colValue;
    }
    //根据比赛方案设计的流程树结构
    public void NextRound()
    {
        if (i <= 2 && j <= 2)
        {
            UpdateRound(i);
            TurnRound();
            i++;
        }
       else if (i > 2 && j < 2)
        {
            i = 0;
            j++;
            UpdateRound(i);
            UpdateSubject(j);
            TurnRound();
            i++;
           
        }
      else if (j >= 2)
        {
            //切换队伍并重置科目和轮次
            m_objDropdown.GetComponent<Dropdown>().value++;
            j = 0;
            i = 0;
            UpdateRound(i);
            UpdateSubject(j);
            TurnRound();
            i++;
        }
       
    }
    
}
