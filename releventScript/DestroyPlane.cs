﻿/******************************************************************************************************************************************************
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
using UnityEngine.UI;

public class DestroyPlane : MonoBehaviour
{
    public ParticleSystem Explore;
    public ParticleSystem Burn;
    public ParticleSystem Smoke;
    public Text InfoMessage;
    //private void Start()
    //{
    //    StartCoroutine("BlowUp");
    //}

    //IEnumerator BlowUp()
    //{       
    //    yield return new WaitForSeconds(3);
    //    var point = this.gameObject.transform.position;
    //    ParticleSystem ExploreCurrent = ParticleSystem.Instantiate(Explore, point, Quaternion.identity);
    //    ExploreCurrent.Play();
    //    ParticleSystem BurnCurrent = ParticleSystem.Instantiate(Burn, point, Quaternion.identity);
    //    BurnCurrent.Play();
    //    Destroy(this.gameObject);

    //}
    private void AddParticle(ParticleSystem PSOriginal,Vector3 point)
    {
        ParticleSystem PS = ParticleSystem.Instantiate(PSOriginal, point, Quaternion.identity);
        PS.Play();
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        //如果碰撞到的物体的标签为Bullet， 就销毁它
        if (collision.collider.gameObject.tag == "building" || collision.collider.gameObject.tag == "terrain" || collision.collider.gameObject.tag == "enemy")
        {
            //在碰撞点生成爆炸效果，并点燃
            var point = this.gameObject.transform.position;
            AddParticle(Explore, point);
            AddParticle(Burn, point);
            AddParticle(Smoke, point);
            //飞机撞上建筑或者地形爆炸
            InfoMessage.text = "很抱歉，作战失败了，没保住理工大学！";
            for (int i = 0; i < 2; i++)
            {
                Destroy(gameObject.transform.GetChild(i).gameObject);
            }
           
            //击中建筑物则摧毁目标
            if (collision.collider.gameObject.tag == "building")
            {
                Destroy(collision.collider.gameObject);
            }
            if (collision.collider.gameObject.tag == "enemy")
            {
                //炸飞敌人
                collision.collider.gameObject.GetComponent<Rigidbody>().AddExplosionForce(300, point, 10f);
               
                Destroy(collision.collider.gameObject);


            }
        }
    }
    

}

