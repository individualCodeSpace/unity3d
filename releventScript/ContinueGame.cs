/******************************************************************************************************************************************************
 author:laibaolinauthor:laibaolin
 **********************************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueGame : MonoBehaviour
{
    public GameObject Pause;
    private GameObject m_objTimeCount;
    private void Start()
    {
        m_objTimeCount = GameObject.Find("TimeCount");
    
    }
    public void Continued()
    {

        m_objTimeCount.GetComponent<Clock>().enabled = true;
        m_objTimeCount.GetComponent<DeClock>().enabled = true;
      
    }
    public void OnClick1()
    {
        Pause.SetActive(true);
        Continued();
        gameObject.SetActive(false);
    }
}
