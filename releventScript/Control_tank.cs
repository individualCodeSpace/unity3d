/******************************************************************************************************************************************************
 author:laibaolin
 **********************************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_tank : MonoBehaviour
{

    public float moveSpeed = 0.5f;
    private float m_fH;
    private float m_fV;
    private float m_fGunAngle;
    private float m_fAngle;
    public GameObject Gun;
    private Vector3 dir;
    private void Update()
    {
        PlayerMove();

    }
    private void PlayerMove()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_fGunAngle = -5;
        }
        if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E))
        {
            m_fGunAngle = 0;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            m_fGunAngle = 5;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            m_fAngle = -5;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            m_fAngle = 5;
        }
        if (Input.GetKeyUp(KeyCode.O) || Input.GetKeyUp(KeyCode.P))
        {
            m_fAngle = 0;
        }
        Gun.transform.Rotate(m_fAngle, m_fGunAngle,0);
        m_fH = Input.GetAxis("Horizontal");
        m_fV = Input.GetAxis("Vertical");
        dir = new Vector3(m_fH, 0, m_fV);
        //transform.Rotate(transform.position - dir);
        transform.LookAt ( Vector3.Lerp(transform.position, transform.position - dir, -0.01f));
        transform.Translate(dir * moveSpeed, Space.World);
        
    }
}


