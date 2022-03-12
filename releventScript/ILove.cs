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

public class ILove : MonoBehaviour
{
    private GameObject[] m_objI;
    private GameObject[] m_objLove;
    private GameObject[] m_objU;
    private GameObject[] m_objArrow;
    //private GameObject m_mainCam;
    private Vector3[] m_vIPositions = new Vector3[44];
    private Vector3[] m_vLovePositions = new Vector3[44];
    private Vector3[] m_vUPositions = new Vector3[44];
    private Vector3[] m_vArrowPositions = new Vector3[44];
    private Vector3[] m_vCurrentIPos = new Vector3[44];
    private float m_fSpeed = 0.05f;
    private float m_fLast;
    public ParticleSystem Explore;
    public ParticleSystem Burn;
    public ParticleSystem Smoke;
    private bool m_bI_LoveisOK = false;
    private bool m_bLove_UisOK = false;
    private bool m_bU_ArrowisOK = false;
    private bool m_bBangIisOK = false;
    private bool m_bBangIIisOK = false;

    // Start is called before the first frame update
    private void Start()
    {
        
        //m_mainCam.transform.Translate(3138.739f, 367.5544f, 549.7721f);
        //m_mainCam.transform.Rotate(11.055f, 15.518f, 0f);
    }
    void Awake()
    {
        m_objI = GameObject.FindGameObjectsWithTag("I");
        m_objLove = GameObject.FindGameObjectsWithTag("love");
        m_objU = GameObject.FindGameObjectsWithTag("U");
        m_objArrow = GameObject.FindGameObjectsWithTag("Arrow");
        GetPosition(m_vIPositions, m_objI);
        GetPosition(m_vLovePositions, m_objLove);
        GetPosition(m_vUPositions, m_objU);
        GetPosition(m_vArrowPositions, m_objArrow);
        GameObject.Find("Love").SetActive(false);
        GameObject.Find("U").SetActive(false);
        GameObject.Find("ArrowImage").SetActive(false);
        // m_mainCam = GameObject.FindGameObjectWithTag("MainCamera");
       // StartCoroutine("BangI");
    }
    public void BtnSet()
    {
        m_fLast = Time.time;
    }
    // Update is called once per frame
    void Update()
    {
        GetPosition(m_vCurrentIPos, m_objI);
        //飞机螺旋桨自转
        for (int i = 0; i < m_objI.Length; i++)
        {

            m_objI[i].transform.GetChild(1).gameObject.transform.Rotate(0, 800 * Time.deltaTime, 0);

        }
       
        if (m_bI_LoveisOK == false)
        {
            SetPosition(m_vIPositions, m_vLovePositions);
        }
        if (m_bI_LoveisOK == true && m_bLove_UisOK == false && m_bBangIisOK == true)
        {
           
            SetPosition(m_vLovePositions, m_vUPositions);
           
        }
        if (m_bLove_UisOK == true && m_bU_ArrowisOK == false && m_bBangIIisOK == true)
        {
           
            SetPosition(m_vUPositions, m_vArrowPositions);
            
        }
        if (m_vCurrentIPos[43] == m_vLovePositions[43] && m_bI_LoveisOK == false)
        {
            StartCoroutine("BangI");
            m_bI_LoveisOK = true;
        }
        if (m_vCurrentIPos[43] == m_vUPositions[43] && m_bLove_UisOK == false)
        {
            StartCoroutine("BangII");
            m_bLove_UisOK = true;
            m_bBangIisOK = false;
        }
        if (m_vCurrentIPos[43] == m_vArrowPositions[43] && m_bU_ArrowisOK == false)
        {
            StartCoroutine("BangArrow");
            m_bU_ArrowisOK = true;
            m_bBangIIisOK = false;
        }

    }
    private void AddParticle(ParticleSystem PSOriginal, Vector3 point)
    {
        ParticleSystem PS = ParticleSystem.Instantiate(PSOriginal, point, Quaternion.identity);
        PS.Play();
    }
    private void GetPosition(Vector3[] pos,GameObject[] obj)
    {
        for (int i = 0; i < m_objI.Length; i++)
        {
           pos[i] = obj[i].transform.position;
        }
    }
    private void SetPosition(Vector3[] beginposition,Vector3[] endposition)
    {
        for (int i = 0; i < m_objI.Length; i++)
        {
            
            StartCoroutine(Change(i, beginposition, endposition));
           
        }
    }

    IEnumerator Change(int i, Vector3[] beginPosition, Vector3[] endPosition)
    {
        yield return new WaitForSeconds(0f);
        m_objI[i].transform.position = Vector3.Lerp(beginPosition[i], endPosition[i], (Time.time - m_fLast) * m_fSpeed);
    }
    IEnumerator BangI()
    {
        yield return new WaitForSeconds(3);
        for (int i = 0; i < m_vIPositions.Length; i++)
        {
            AddParticle(Burn, m_vCurrentIPos[i]);
        }
        m_bBangIisOK = true;
        m_fLast = Time.time;
    }
    IEnumerator BangII()
    {
        yield return new WaitForSeconds(3);
        for (int i = 0; i < m_vIPositions.Length; i++)
        {
            AddParticle(Burn, m_vCurrentIPos[i]);
        }
        m_bBangIIisOK = true;
        m_fLast = Time.time;
    }

    IEnumerator BangArrow()
    {
        yield return new WaitForSeconds(3);
        for (int i = 0; i < m_vIPositions.Length; i++)
        {
            AddParticle(Explore, m_vCurrentIPos[i]);
            AddParticle(Burn, m_vCurrentIPos[i]);
            AddParticle(Smoke, m_vCurrentIPos[i]);
            Destroy(m_objI[i].gameObject);
        }
    }
   
}
