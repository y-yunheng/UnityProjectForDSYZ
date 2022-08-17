using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;


public class UDPclient
{
    private string remote_ip;
    private int port;
    private int size;
    public Queue<string> RecviceQueue = new Queue<string>();
    public Queue<string> SendQueue = new Queue<string>();
    public string str;
    public bool isaccept;
    private Socket udpClient;
    private IPEndPoint serverPoint;
    private IPEndPoint endPoint;
    private Thread ReceiveListern;
    private Thread SendListern;
    private int maxlen = 5;
    public UDPclient( int size)
    {
        this.size = size;

    }




    public void ConnectStart(string remote_ip, int port)
    {
        this.remote_ip = remote_ip;
        this.port = port;
        udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        udpClient.Bind(new IPEndPoint(IPAddress.Any, 0));
        //sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);//开启群发功能
        //ie = new IPEndPoint(IPAddress.Broadcast,5555);//广播端口
        serverPoint = new IPEndPoint(IPAddress.Any, port);
        IPEndPoint end = new IPEndPoint(IPAddress.Any, 0);


        ReceiveListern = new Thread(Receive);
        ReceiveListern.Start();
        SendListern = new Thread(SendMessage);
        SendListern.Start();

    }


    //收消息
    public void Receive()
    {
        while (true)
        {
            EndPoint point = new IPEndPoint(IPAddress.Any, 0);//用来保存发送方的ip和端口号
            byte[] buffer = new byte[size];
            int length = udpClient.ReceiveFrom(buffer, ref point);//接收数据报
            string msg = Encoding.ASCII.GetString(buffer, 0, length);
            if (msg.Contains("}@"))
            {
                int index = msg.IndexOf("}@");
                msg = msg.Substring(0, index + 1);
            }
            if(RecviceQueue.Count>=maxlen)
            {
                RecviceQueue.Dequeue();
            }
            RecviceQueue.Enqueue(msg);
            
        }
    }
    //发消息 单发
    public void SendMessage()
    {
        while (true)
        {
            try
            {
                EndPoint point = new IPEndPoint(IPAddress.Parse(remote_ip), port);
                byte[] buffer = new byte[size];
                buffer = Encoding.ASCII.GetBytes(SendQueue.Dequeue()+"@");

                udpClient.SendTo(buffer, point);

            }
            catch(Exception e)
            {
               
            }






        }


    }



}


