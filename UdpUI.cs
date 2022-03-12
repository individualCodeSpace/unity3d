using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UdpUI : MonoBehaviour
{

    [SerializeField]
    Text m_recv;
    [SerializeField]
    InputField m_localPort;
    [SerializeField]
    InputField m_remotePort;
    [SerializeField]
    InputField m_remoteIp;
    public GameObject PaiLiuTank;
    private int PaiLiuTankID = 1003;
    private float m_fCurPositionX;
    private float m_fCurPositionY;
    private float m_fCurPositionZ;
    private float m_fCurTime;
    private float m_fPeriod = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
       
        UDPClient.Inst.OnRecieveEvent += Recv;

    }
    public void Init()
    {
        UDPClient.Inst.Init(int.Parse(m_localPort.text), m_remoteIp.text, int.Parse(m_remotePort.text));
        StartCoroutine("AutoSend");
    }
    private void Recv(byte[] bytes)
    {
        //UTF-8解码出字符串
        m_recv.text = Encoding.UTF8.GetString(bytes);
    }

    public void Send()
    {
       // string message_QZ = "STX,QZPOSITION," + m_vehicleId.text + "," + m_startPositionX.text + "," + m_startPositionY.text + "," + m_terminalPositionX.text + "," + m_terminalPositionY.text + ",ETX";
        //string message_REAL = "STX,REALPOSITION," + m_vehicleId.text + "," + m_RealTerPositionX.text + "," + m_RealTerPositionY.text + ",ETX";
        string message_CUR = "STX,CURPOSITION," + PaiLiuTankID + "," + m_fCurPositionX + "," + m_fCurPositionY + "," + m_fCurPositionZ + "," + m_fCurTime + ",ETX";
        //UDPClient.Inst.SendRemote(message_QZ);
        //UDPClient.Inst.SendRemote(message_REAL);
        UDPClient.Inst.SendRemote(message_CUR);
    }
    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator AutoSend()
    {
        while(true)
        {
            yield return new WaitForSeconds(m_fPeriod);
            m_fCurPositionX = PaiLiuTank.transform.position.x;
            m_fCurPositionY = PaiLiuTank.transform.position.y;
            m_fCurPositionZ = PaiLiuTank.transform.position.z;
            m_fCurTime = Time.time;
            Send();
        }
    }

}
