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

public class RotatePlane : MonoBehaviour
{
    public GameObject BulletGo;
    public Transform firePosition;
    public float RotateSpeed = 10;
    public float shootTime = 2f;
    //射击间隔时间的计时器
    private float shootTimer = 0;
    public int Force = 1000;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //模拟飞机螺旋桨自转，可随飞行姿态的变换改变转速
        transform.Rotate(0, RotateSpeed * Time.deltaTime, 0);
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootTime)
        {
            //if (Input.GetMouseButtonDown(0))
            //{
                //直升机炸弹投放
                GameObject bulletCurrent = GameObject.Instantiate(BulletGo, firePosition.position, Quaternion.identity);
                bulletCurrent.transform.Rotate(90, 0, 180);
                //通过刚体组件给子弹添加一个正前方向上的力，以达到让子弹向前运动的效果
                bulletCurrent.GetComponent<Rigidbody>().AddForce(0,0,-Force);
                shootTimer = 0;
            //}
        }
        }
}



