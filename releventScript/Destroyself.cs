using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class Destroyself : MonoBehaviour
{
    
    
   
    public ParticleSystem Explore;
    public ParticleSystem Burn;
    public ParticleSystem Burnload;
    public ParticleSystem Smoke;
    private GameObject m_objCurrent;
    private Vector3 m_vPoint;
    private Vector3 m_vPoint1;



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
    private void OnCollisionEnter(Collision collision)
    {
        //如果碰撞到的物体的标签为Bullet， 就销毁它
        if (collision.collider.gameObject.tag == "building" || collision.collider.gameObject.tag == "terrain" || collision.collider.gameObject.tag == "plane")
        {
            //在碰撞点销毁炮弹，并生成爆炸效果，并点燃
           
            StartCoroutine("Extinguish");
            //击中建筑物则摧毁目标
            if (collision.collider.gameObject.tag == "building" )
            {
                Destroy(collision.collider.gameObject);
            }
            if(collision.collider.gameObject.tag == "plane")
            {
                for (int i = 0; i < 2; i++)
                {
                    Destroy(collision.collider.gameObject.transform.GetChild(i).gameObject);
                }
               

            }

        }
        if(collision.collider.gameObject.tag == "enemy")
        {
            m_vPoint = this.gameObject.transform.position;
            //炸飞敌人
            collision.collider.gameObject.GetComponent<Rigidbody>().AddExplosionForce(300, m_vPoint, 10f);
            m_objCurrent = collision.collider.gameObject;
            StartCoroutine("Delay1");
            
            

        }
    }
    private void AddParticle(ParticleSystem PSOriginal, Vector3 point)
    {
        ParticleSystem PS = ParticleSystem.Instantiate(PSOriginal, point, Quaternion.identity);
        PS.Play();
    }

    IEnumerator Extinguish()
    {
        m_vPoint = this.gameObject.transform.position;
        AddParticle(Explore, m_vPoint);
        AddParticle(Burnload, m_vPoint);
        AddParticle(Smoke, m_vPoint);
        for (int i = 0; i < 2; i++)
        {
            gameObject.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        yield return new WaitForSeconds(3);
        //Destroy(ExploreCurrent);
        Destroy(this.gameObject);
    }
    IEnumerator Delay1()
    {
        
        m_vPoint = this.gameObject.transform.position;
        AddParticle(Explore, m_vPoint);
        AddParticle(Smoke, m_vPoint);
        for (int i = 0; i < 2; i++)
        {
            gameObject.transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        yield return new WaitForSeconds(2);
        m_vPoint1 = m_objCurrent.transform.position;
        AddParticle(Explore, m_vPoint1);
        AddParticle(Burn, m_vPoint1);
        Destroy(m_objCurrent.gameObject);
        Destroy(this.gameObject);


    }
}
