﻿/******************************************************************************************************************************************************

 author:laibaolin
***********************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlane : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject BulletGo;
    public Transform firePosition;
    public float RotateSpeed = 10;
    private float shootTime = 0.2f;
    //射击间隔时间的计时器
    private float shootTimer = 0;
    private int Force = 10000;
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
            if (Input.GetMouseButtonDown(0))
            {
                //直升机炸弹投放
                GameObject bulletCurrent = GameObject.Instantiate(BulletGo, firePosition.position, Quaternion.identity);
                bulletCurrent.transform.Rotate(90, 0, 0);
                //通过刚体组件给子弹添加一个正前方向上的力，以达到让子弹向前运动的效果
                bulletCurrent.GetComponent<Rigidbody>().AddForce(0, 0, Force);
                shootTimer = 0;
            }
        }
    }
}


