/******************************************************************************************************************************************************
 江西联创精密机电有限公司对本代码拥有全部权限，未经许可不得引用本代码任何内容，也不得用于非江西联创精密机电有限公司之外的任何商业和非商业项目。

 项目编号：
 软件名称：

 开发环境：
 运行环境：
 功能描述：

 原作者：邓通福
 完成日期：
 特别说明：

 修改者：
 完成日期：
 修改记录：
 如经过多次修改，则继续往下添加。
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



