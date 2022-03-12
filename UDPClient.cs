using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UnityEngine;



//  开发环境：Visul Studio 2017
//  运行环境：Windows7
//  功能描述：实现了建立在UDP协议上的网络通信功能
 


public class UDPClient : MonoBehaviour
{


    public static UDPClient Inst;
    UdpClient m_udp;
    /// <summary>
    /// 给伺服控制软件发
    /// </summary>
    IPEndPoint m_sendRemote;
    private void SendInit(int localPort, string remoteIp, int remotePort)
    {
        m_sendRemote = new IPEndPoint(IPAddress.Parse(remoteIp), remotePort);
        m_udp = new UdpClient(localPort);
    }
    //向已初始化的目标端口发送字符串
    public void SendRemote(string str)
    {
        try
        {
            Send(str, m_sendRemote);
        }
        catch(Exception ex)
        {
            Debug.Log(ex.ToString());
        }
        
    }
    //利用多态，将字符串UTF8编码成字节流
    //注：UTF-8编码是一种变长度的编码方式，根据字符串类型来自动选择分配的字节数，例如英文字母用单字节的ASCLL码，
    //简体中文用更长字节的扩展unicode编码，利于计算机的数据存储。
    private void Send(string str, IPEndPoint endpoint)
    {
        var data = Encoding.UTF8.GetBytes(str);
        Send(data, endpoint);
    }
    //对字节流数据的发送
    public void SendRemote(byte[] bytes)
    {
        Send(bytes, m_sendRemote);
    }
    private void Send(byte[] bytes, IPEndPoint endpoint)
    {
        Last = bytes;
        print("send : " + BitConverter.ToString(bytes));
        //实际发送的数据字节数据
        m_udp.Send(bytes, bytes.Length, endpoint);
    }
    public byte[] Last;
    public event Action<byte[]> OnRecieveEvent;
    private void Recieve()
    {
        var ipe = new IPEndPoint(IPAddress.Any, 0);
        while (!m_bIsClosed)
        {
            Thread.Sleep(1);
            try
            {
                var bytes = m_udp.Receive(ref ipe);
                print("recieve : " + BitConverter.ToString(bytes));
                m_mutex.WaitOne();
                m_MsgQueue.Enqueue(bytes);
                m_mutex.ReleaseMutex();
            }
            catch (Exception e)
            {
                print(e);
                //break;
            }
        }
    }
    void Awake()
    {
        Inst = this;
    }
    Mutex m_mutex = new Mutex();
    Queue<Byte[]> m_MsgQueue = new Queue<byte[]>();
    // Use this for initialization 利用Socket建立连接；
    public void Init(int localPort, string remoteIp, int remotePort)
    {

        SendInit(localPort, remoteIp, remotePort);
        new Thread(Recieve).Start();
    }

    private void Update()
    {
        while (m_MsgQueue.Count > 0)
        {
            m_mutex.WaitOne();
            var bytes = m_MsgQueue.Dequeue();
            m_mutex.ReleaseMutex();
            if (OnRecieveEvent != null)
                OnRecieveEvent(bytes);
        }
    }

    bool m_bIsClosed;
    private void OnDestroy()
    {
        m_udp.Close();
        m_bIsClosed = true;
    }

}
