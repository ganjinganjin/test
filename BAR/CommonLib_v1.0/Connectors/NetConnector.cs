using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace BAR.Commonlib.Connectors
{
    public class Conn
    {
        public const int data = 1024;
        //Socket
        public Socket socket;
        //是否使用
        public bool isUse = false;
        //Buff
        public byte[] readBuff = new byte[data];
        public int buffCount = 0;
        //构造函数
        public Conn()
        {
            readBuff = new byte[data];
        }
        //初始化
        public void Init(Socket socket)
        {
            this.socket = socket;
            isUse = true;
            buffCount = 0;
        }
        //缓冲区剩余的字节数
        public int BuffRemain()
        {
            return data - buffCount;
        }

        //获取客户端地址
        public string GetAdress()
        {
            if (!isUse)
                return "无法获取地址";
            return socket.RemoteEndPoint.ToString();
        }
        //关闭
        public void Close()
        {
            if (!isUse)
                return;
            Console.WriteLine("[断开链接]" + GetAdress());
            socket.Close();
            isUse = false;
        }
    }
    public class NetConnector
    {
        public  bool            IsConnected;
        
        private const String    BEGINCHAR = "#";
        private const short     BYTESIZE = 512;
        private Socket          socket;
        private Byte[]          bytesBuffer = null;

        private static NetConnector instance = null;
        private static Dictionary<String, NetConnector> instanceDir = new Dictionary<string, NetConnector>();
        public AutoResetEvent autoEvent;

        public String StrUDPTarIP;
        public int IUDPTarPort;

        NetConnector()
        {
            bytesBuffer = new Byte[BYTESIZE];
            autoEvent = new AutoResetEvent(false);
        }
        public static NetConnector GetInstance(String connectID)
        {
            if (instanceDir.ContainsKey(connectID))
            {
                instance = instanceDir[connectID];
            }else{
                instance = new NetConnector();
                instanceDir[connectID] = instance;
            }
            return instance;
        }
        public bool CreateSocket(int blockType,string IP, int port, SocketType socketType)
        {
            try{
                if (socketType == SocketType.Stream)
                {
                    this.socket = new Socket(AddressFamily.InterNetwork, socketType, ProtocolType.Tcp);
                }
                else
                {
                    this.socket = new Socket(AddressFamily.InterNetwork, socketType, ProtocolType.Udp);
                    IPEndPoint sin = new IPEndPoint(IPAddress.Parse("192.168.0.10"), port);
                    socket.Bind(sin);
                }
                socket.ReceiveBufferSize = 50 * 1024;
                socket.SendTimeout = 3000;
                int type = blockType;
                if (type == 1){
                    socket.ReceiveTimeout = 10000;
                }
                else if (type == 2)
                {
                    socket.ReceiveTimeout = 5;
                }
                else if (type == 3)
                {
                    socket.Blocking = false;
                }

                //socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                //socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, false);
            }catch (SocketException sEx)
            {
                return false;
            }
            return true;
    
        }
        public void SetUDPInfo(String ip, int port)
        {
            StrUDPTarIP = ip;
            IUDPTarPort = port;

        }
        public bool Connect(String host, int port)
        {
            if (!(host == "" || port == 0))
            {
                try
                {
                    socket.Connect(host, port);
                    IsConnected = true;
                }
                catch (SocketException sEx)
                {
                    socket.Close();
                    socket = null;
                }
            }
            return socket.Connected;
        }
        public bool Send(byte[] buffer, int len)
        {
            if(!IsConnected)
            {
                return false;
            }
            bool retBool;
            int hasSendLen = 0;
            hasSendLen = socket.Send(buffer, len, SocketFlags.None);
            if ( hasSendLen != len)
            {
                socket.Close();
                IsConnected = false;
                retBool = false;
            }
            retBool = true;
            return retBool;
        }
        public bool SendUDP(byte[] buffer, int len)
        {
            bool retBool;
            try
            {
                int sendNum = 0;
                IPEndPoint sender = new IPEndPoint(IPAddress.Parse(StrUDPTarIP), IUDPTarPort);
                EndPoint Remote = (EndPoint)(sender);
                sendNum = socket.SendTo(buffer, buffer.Length, SocketFlags.None, Remote);
                return sendNum == len;
            }
            catch(Exception ex)
            {
                retBool = false;
            }
            return retBool;
        }
        public int Receive(out byte[] recv,ref int len)
        {
            int retNun = 0;
            byte[] tem_recv = new byte[255];
            len = socket.Receive(tem_recv, SocketFlags.None);
            recv = tem_recv;
            if (len > 0)
            {
                retNun = 1;
            }
            else if (len == 0)
            {
                IsConnected = false;
                socket.Close();
                socket = null;
                retNun = -1;
            }
            return retNun;
        }
        

        /// <summary>
        /// 创建多个Conn管理客户端的连接
        /// </summary>
        public Conn[] conns;

        /// <summary>
        /// 最大连接数
        /// </summary>
        public int maxConn = 10;

        /// <summary>
        /// 将Socket定义为全局变量
        /// </summary>
        private Socket serverSocket;

        //public Act g_act = Act.GetInstance();

        /// <summary>
        /// 将远程连接的客户端的IP地址和Socket存入集合中
        /// </summary>
        Dictionary<string, Socket> disSocket = new Dictionary<string, Socket>();

        public string[] strSubAry = new string[20];
        /// <summary>
        /// 接收数据缓冲区
        /// </summary>
        public string strReceiveBuff;

        /// <summary>
        /// 接收数据缓冲区
        /// </summary>
        public byte[] bytReceiveBuff;
        /// <summary>
        /// 解析后的接收数据
        /// </summary>
        public string[] ReceiveDate = new string[40];

        /// <summary>
        /// 获取链接池索引，返回负数表示获取失败
        /// </summary>
        /// <returns></returns>
        public int NewIndex()
        {
            if (conns == null)
                return -1;
            for (int i = 0; i < conns.Length; i++)
            {
                if (conns[i] == null)

                {
                    conns[i] = new Conn();
                    return i;
                }
                else if (conns[i].isUse == false)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 创建服务器
        /// </summary>
        /// <param name="HostName">IP</param>
        /// <param name="Port">端口号</param>
        public void Create(string HostName, int Port)
        {
            try
            {
                //创建多个链接池，表示创建maxConn最大客户端
                conns = new Conn[maxConn];
                for (int i = 0; i < maxConn; i++)
                {
                    conns[i] = new Conn();
                }
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(HostName), Port);
                serverSocket.Bind(ipEndPoint);//绑定IP和端口号
                serverSocket.Listen(maxConn);//开始监听端口,0为监听无限个客户端
                Console.WriteLine("[服务器]启动成功");
                ShowMsg("[服务器]启动成功");
                //开始调用异步连接
                serverSocket.BeginAccept(AcceptCb, null);
            }
            catch (Exception e)
            {
                MessageBox.Show("Create失败:" + e.Message);
                ShowErrorMsg("Create失败:" + e.Message);
                Console.WriteLine("Create失败:" + e.Message);
            }

        }
        /// <summary>
        /// Accept回调
        /// </summary>
        /// <param name="ar"></param>
        private void AcceptCb(IAsyncResult ar)
        {
            try
            {
                Socket socket = serverSocket.EndAccept(ar);//尝试进行异步连接
                int index = NewIndex();
                if (index < 0)
                {
                    socket.Close();
                    Console.Write("[警告]链接已满");
                    ShowErrorMsg("[警告]链接已满");
                }
                else
                {
                    Conn conn = conns[index];
                    conn.Init(socket);
                    string adr = conn.GetAdress();
                    Console.WriteLine("客户端连接 [" + adr + "] conn池ID：" + index);
                    ShowMsg("客户端连接 [" + adr + "] conn池ID：" + index);
                    conn.socket.BeginReceive(conn.readBuff, conn.buffCount, conn.BuffRemain(), SocketFlags.None, ReceiveCb, conn);
                    disSocket.Add(adr, socket);//IP绑定socket
                }
                serverSocket.BeginAccept(AcceptCb, null);
            }
            catch (Exception e)
            {
                ShowErrorMsg("AcceptCb失败:" + e.Message);
                Console.WriteLine("AcceptCb失败:" + e.Message);
            }
        }
        /// <summary>
        /// 接收回调
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveCb(IAsyncResult ar)
        {
            Conn conn = (Conn)ar.AsyncState;
            try
            {
                strReceiveBuff = "";
                int count = conn.socket.EndReceive(ar);
                //关闭信号
                if (count <= 0)
                {
                    autoEvent.Set();
                    Console.WriteLine("收到 [" + conn.GetAdress() + "] 断开链接");
                    ShowMsg("收到 [" + conn.GetAdress() + "] 断开链接");
                    disSocket.Remove(conn.GetAdress());//将带有指定键的Socket从 Dictionary<TKey, TValue> 中移除
                    conn.Close();
                    return;
                }
                //数据处理
                strReceiveBuff = Encoding.UTF8.GetString(conn.readBuff, 0, count);
                autoEvent.Set();
                Console.WriteLine("收到 [" + conn.GetAdress() + "] 数据：" + strReceiveBuff);
                ShowMsg("收到 [" + conn.GetAdress() + "] 数据：" + strReceiveBuff);

                //继续接收
                conn.socket.BeginReceive(conn.readBuff, conn.buffCount, conn.BuffRemain(), SocketFlags.None, ReceiveCb, conn);
            }
            catch (Exception e)
            {
                Console.WriteLine("收到 [" + conn.GetAdress() + "] 断开链接");
                ShowMsg("收到 [" + conn.GetAdress() + "] 断开链接");
                disSocket.Remove(conn.GetAdress());//将带有指定键的Socket从 Dictionary<TKey, TValue> 中移除
                conn.Close();
            }
        }

        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="strMsg"></param>
        public bool Broadcast(byte[] buffer)
        {
            //string strMsg = string.Format("Site{0:d},{1:d},END", Site, Msg);
            //byte[] bytes = Encoding.UTF8.GetBytes(strMsg);
            try
            {
                for (int i = 0; i < conns.Length; i++)
                {
                    if (conns[i] == null)
                        continue;
                    if (!conns[i].isUse)
                        continue;
                    ShowMsg("将消息转播给 " + conns[i].GetAdress());
                    Console.WriteLine("将消息转播给 " + conns[i].GetAdress());
                    conns[i].socket.BeginSend(buffer, 0, buffer.Length, 0,
                    new AsyncCallback(SendCallback), conns[i].socket);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public bool Send(String data)
        {
            try
            {
                byte[] byteData = Encoding.ASCII.GetBytes(data);
                socket.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), socket);
                ShowMsg("发送成功：" + data);
                return true;
            }
            catch (Exception e)
            {
                ShowErrorMsg("Send失败:" + e.Message);
                return false;
            }

        }
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// 从网口读取指定长度的字节数据
        /// </summary>
        /// <param name="frame">接受到的字节数组</param>
        public bool ReadNet(out string frame)
        {
            try
            {
                frame = strReceiveBuff;
                return true;
            }
            catch (Exception Ex)
            {
                frame = null;
                return false;
            }

        }

        /// <summary>
        /// 解析数据
        /// </summary>
        public void AnalysisDate_RD()
        {
            strSubAry = strReceiveBuff.Split(',');
            if (strSubAry[0] == "Site1")
            {
                for (int i = 0; i < 8; i++)
                {
                    Download_RD.programer_Unit[i].Status = Convert.ToByte(strSubAry[i + 1]);
                }
            }
            else if (strSubAry[0] == "Site2")
            {
                for (int i = 0; i < 8; i++)
                {
                    Download_RD.programer_Unit[i + 8].Status = Convert.ToByte(strSubAry[i + 1]);
                }
            }
            else if (strSubAry[0] == "Site3")
            {
                for (int i = 0; i < 8; i++)
                {
                    Download_RD.programer_Unit[i + 16].Status = Convert.ToByte(strSubAry[i + 1]);
                }
            }
            else if (strSubAry[0] == "Site4")
            {
                for (int i = 0; i < 8; i++)
                {
                    Download_RD.programer_Unit[i + 24].Status = Convert.ToByte(strSubAry[i + 1]);
                }
            }
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="str">显示的信息</param>
        void ShowMsg(string str)
        {
            //g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str);
        }

        /// <summary>
        /// 显示警告信息Error
        /// </summary>
        /// <param name="str">显示的信息</param>
        void ShowErrorMsg(string str)
        {
            //g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "error");
        }
    }
}
