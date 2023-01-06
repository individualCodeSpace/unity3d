/******************************************************************************************************************************************************
author:laibaolin
 **********************************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Display : MonoBehaviour
{
   
    public GameObject m_objInfo;
    // Start is called before the first frame update
   
    public void Play()
    {
        StartCoroutine("SubSec");
        StopCoroutine("SubSec0");
    }
    public void Play0()
    {
        StartCoroutine("SubSec0");
        StopCoroutine("SubSec");
    }


    IEnumerator SubSec ()
    {
        m_objInfo.SetActive(true);
        m_objInfo.GetComponent<Text>().text = "预 备";
        yield return new WaitForSeconds(1);
        for (int i = 3; i > 0; i--)
        {
            m_objInfo.GetComponent<Text>().text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        m_objInfo.GetComponent<Text>().text = "开始！";
        yield return new WaitForSeconds(1);
        m_objInfo.SetActive(false);

        
    }
    IEnumerator SubSec0()
    {
       
        m_objInfo.SetActive(true);
        for (int j = 3; j > 0; j--)
        {
            m_objInfo.GetComponent<Text>().text = j.ToString();
            yield return new WaitForSeconds(1);
        }
        m_objInfo.GetComponent<Text>().text = "时间到！";
        yield return new WaitForSeconds(1);
        m_objInfo.SetActive(false);
    }
}
