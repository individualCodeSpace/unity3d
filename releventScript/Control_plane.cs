/******************************************************************************************************************************************************
author:laibaolin
 **********************************************************************************************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Control_plane : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    private float m_fH;
    private float m_fV;
    private float m_fHigh = 0;
    private Vector3 dir1;
    private Vector3 dir;
    private GameObject[] m_objEnemy;
    public Text InfoMessage;
    private int m_EnemyCount;
    private string [] m_sInfoMess = { "干得漂亮！", "章雪松附体！", "帅！", "牛逼！", "皇牌飞行员！", "秀得飞起！" };
    private void Start()
    {
        m_objEnemy = GameObject.FindGameObjectsWithTag("enemy");
        m_EnemyCount = m_objEnemy.Length;
       
    }
    private void Update()
    {
        PlayerMove();
        m_objEnemy = GameObject.FindGameObjectsWithTag("enemy");
       
        if (m_objEnemy.Length == 0)
        {
            //InfoMessage.SetActive(true);
            InfoMessage.text = "感谢您拯救了理工大学！";
        }
        if (m_objEnemy.Length != m_EnemyCount)
        {
            m_EnemyCount = m_objEnemy.Length;
            InfoMessage.text = m_sInfoMess[Random.Range(0,6)];
        }
    }
    private void PlayerMove()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_fHigh = -1;
        }
        if(Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E))
        {
            m_fHigh = 0;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            m_fHigh = 1;
        }
        m_fH = Input.GetAxis("Horizontal") * System.Math.Abs(Input.GetAxis("Horizontal"));
        m_fV = Input.GetAxis("Vertical") * System.Math.Abs( Input.GetAxis("Vertical"));
        dir = new Vector3(m_fH, m_fHigh, m_fV);
        dir1 = new Vector3(m_fH, 0, m_fV);
        // transform.LookAt(transform.position - dir1);
        transform.LookAt(Vector3.Lerp(transform.position,transform.position - dir1, -0.01f));
        transform.Translate(dir * moveSpeed, Space.World);
    }
}

