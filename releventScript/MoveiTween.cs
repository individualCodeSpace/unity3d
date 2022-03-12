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




