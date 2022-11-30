using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BAR.Commonlib.Connectors
{
    public class MyTcpClient
    {
        public Act g_act = Act.GetInstance();
        Socket socketSend;

        public bool IsRecive;
        public string ReciveDate;

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="HostName">IP地址</param>
        /// <param name="port">端口号</param>
        public bool Connect(string HostName, int port)
        {
            try
            {
                //创建负责通信的Socket
                socketSend = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(HostName);
                IPEndPoint point = new IPEndPoint(ip, port);
                //获得要连接的远程服务器应用程序的IP地址和端口号
                socketSend.Connect(point);
                ShowMsg("网络连接成功");

                //开启一个新的线程不停的接收服务端发来的消息
                IsRecive = true;
                Thread th = new Thread(Recive);
                th.IsBackground = true;
                th.Start();
                return true;
            }
            catch (SocketException ex)
            {
                ShowErrorMsg("网络连接失败");
                ShowErrorMsg("错误信息:" + ex.Message);
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 不停的接受服务器发来的消息
        /// </summary>
        void Recive()
        {
            while (IsRecive)
            {
                try
                {
                    byte[] buffer = new byte[1024 * 1024 * 3];
                    int r = socketSend.Receive(buffer);
                    //实际接收到的有效字节数
                    if (r == 0)
                    {
                        break;
                    }
                    //表示发送的文字消息
                    ReciveDate = Encoding.UTF8.GetString(buffer, 0, r);
                    ShowMsg(socketSend.RemoteEndPoint + "：" + ReciveDate);
                }
                catch(Exception ex)
                {
                    ShowErrorMsg("错误信息:" + ex.Message);
                }
            }
        }
        
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="str">显示的信息</param>
        void ShowMsg(string str)
        {
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str);
        }

        /// <summary>
        /// 显示警告信息Error
        /// </summary>
        /// <param name="str">显示的信息</param>
        void ShowErrorMsg(string str)
        {
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "error");
        }

        /// <summary>
        /// 客户端给服务器发送消息
        /// </summary>
        /// <param name="Msg">发送的信息</param>
        public void Send(string Msg)
        {
            try
            {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(Msg);
                socketSend.Send(buffer);
                ShowMsg("发送信息:" + Msg);
                ReciveDate = "";
            }
            catch (Exception ex)
            {
                ShowErrorMsg("错误信息:" + ex.Message);
            }
            
        }

        /// <summary>
        /// 断开客户端连接
        /// </summary>
        public void DisConnect()
        {
            try
            {
                IsRecive = false;
                socketSend.Close();
            }
            catch (Exception ex)
            {
                ShowErrorMsg("错误信息:" + ex.Message);
            }
            
        }
    }
}
