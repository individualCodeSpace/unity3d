using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    private GameObject m_objTimeCount;
    private GameObject m_objStop;
    private GameObject m_objPause;
    private GameObject m_objNextRound;


    // Start is called before the first frame update
    private void Start()
    {
        m_objTimeCount = GameObject.Find("TimeCount");
        m_objStop = GameObject.Find("Stop");
        m_objPause = GameObject.Find("Pause");
        m_objNextRound = GameObject.Find("NextRound");
    }
    public void Setac1()
    {
        m_objNextRound.GetComponent<Button>().interactable = false;
        gameObject.GetComponent<Button>().interactable = false;
        m_objStop.GetComponent<Display>().Play();
        StartCoroutine("DelayAct");
    }
    //public void Setac0()
    //{
    //    GameObject.Find("TimeCount").GetComponent<Clock>().enabled = true;
    //    GameObject.Find("TimeCount").GetComponent<DeClock>().enabled = true;
    //}
    IEnumerator DelayAct()
    {
        yield return new WaitForSeconds(4);
        m_objTimeCount.GetComponent<Clock>().enabled = true;
        m_objTimeCount.GetComponent<DeClock>().enabled = true;
        m_objStop.GetComponent<Button>().interactable = true;
        m_objPause.GetComponent<Button>().interactable = true;
    }
}
