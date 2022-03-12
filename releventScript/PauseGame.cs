using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject Continue;
    private GameObject m_objTimeCount;
    private void Start()
    {
        m_objTimeCount = GameObject.Find("TimeCount");
       
    }
    public void Pause()
    {
        m_objTimeCount.GetComponent<Clock>().enabled = false;
        m_objTimeCount.GetComponent<DeClock>().enabled = false;
    }
    public void OnClick()
    {
    
        Continue.SetActive(true);
        Pause();
        gameObject.SetActive(false);
    }
   
}
