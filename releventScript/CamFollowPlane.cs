/******************************************************************************************************************************************************
 author:laibaolin
 **********************************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowPlane : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform m_tPlayer;
    private Vector3 m_vCameraPos;
    private void Start()
    {
        //获取摄像机与飞机的初始距离
        m_tPlayer = GameObject.FindGameObjectWithTag("plane").GetComponent<Transform>();
        m_vCameraPos = transform.position - m_tPlayer.position;

    }
    private void LateUpdate()
    {
        //摄像机平滑跟随飞机移动
        Vector3 Pos = m_vCameraPos + m_tPlayer.position;
        transform.position = Vector3.Lerp(transform.position, Pos, -0.01f);
    }
}



