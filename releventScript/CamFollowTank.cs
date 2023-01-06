/******************************************************************************************************************************************************
author:laibaolin
 **********************************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowTank : MonoBehaviour
{
    private Transform m_tPlayer;
    private Vector3 m_vCameraPos;
    private float m_fGunAngle;
    private float m_fAngle;
    private void Start()
    {
        m_tPlayer = GameObject.FindGameObjectWithTag("tank").GetComponent<Transform>();
        m_vCameraPos = transform.position - m_tPlayer.position;

    }
    private void LateUpdate()
    {
        //摄像机平滑跟随坦克移动
        Vector3 Pos = m_vCameraPos + m_tPlayer.position;
        transform.position = Vector3.Lerp(transform.position, Pos, -0.01f);
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
        transform.Rotate(m_fAngle, m_fGunAngle, 0);
    }
}

