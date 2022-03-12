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

public class TankManager : MonoBehaviour
{
    public GameObject BulletGo;
    private GameObject m_objTank;
    private float m_fGunAngle;
    private float m_fAngle;
    public Transform firePosition;
    public float shootTime = 0.2f;
    //射击间隔时间的计时器
    private float shootTimer = 0;
    public int Force = 3000;
    public ParticleSystem Fire;
    // Start is called before the first frame update
    void Start()
    {
        //m_objTank = GameObject.FindGameObjectWithTag("tank");
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    m_fGunAngle = -5;
        //}
        //if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E))
        //{
        //    m_fGunAngle = 0;
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    m_fGunAngle = 5;
        //}
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    m_fAngle = -5;
        //}
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    m_fAngle = 5;
        //}
        //if (Input.GetKeyUp(KeyCode.O) || Input.GetKeyUp(KeyCode.P))
        //{
        //    m_fAngle = 0;
        //}
        //BulletGo.transform.Rotate(80 + m_fAngle, m_fGunAngle, 0);

        shootTimer += Time.deltaTime;
        if (shootTimer >= shootTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //坦克炮弹发射，并产生一个反向的后坐力
                ParticleSystem FireCurrent = ParticleSystem.Instantiate(Fire, firePosition.position, Quaternion.identity);
                FireCurrent.Play();
                GameObject bulletCurrent = GameObject.Instantiate(BulletGo, firePosition.position, Quaternion.identity);
                bulletCurrent.transform.Rotate(80, 0, 0);
                //bulletCurrent.transform.rotation = BulletGo.transform.rotation;
                //通过刚体组件给子弹添加一个正前方向上的力，以达到让子弹向前运动的效果
                bulletCurrent.GetComponent<Rigidbody>().AddForce(transform.forward * Force);
                //m_objTank.GetComponent<Rigidbody>().AddForce(-transform.forward * Force);
                shootTimer = 0;
            }
        }
    }
}


