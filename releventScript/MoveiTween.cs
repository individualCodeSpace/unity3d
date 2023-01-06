/******************************************************************************************************************************************************
author:laibaolin
 **********************************************************************************************************************************************************/

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveiTween : MonoBehaviour
{
    // Start is called before the first frame updat
    private Vector3 pos;
    private Vector3 dir = new Vector3(0, 0, 1200);
    private float m_fSpeed = 0.01f;
    private float m_fLast;
    private bool m_bIsOnCollision = false;
    private void Start()
    {
         pos = gameObject.transform.position;
        m_fLast = Time.time;
    }
    private void Update()
    {
        transform.GetChild(1).gameObject.transform.Rotate(0, 800 * Time.deltaTime, 0);
        if (m_bIsOnCollision == false)
        {
            gameObject.transform.position = Vector3.Lerp(pos, pos - dir, (Time.time - m_fLast) * m_fSpeed);
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        m_bIsOnCollision = true;
    }
}




