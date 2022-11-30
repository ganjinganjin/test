using CCWin.SkinClass;
using CCWin.SkinControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BAR.Commonlib.Connectors
{
    public class SerialConector
    {
        public  bool        IsOpen;
        public const string NewLine = "\r";

        private SerialPort  seriaPort;
        private static      SerialConector instance = null;
        private static      Dictionary<String, SerialConector> instanceDir = new Dictionary<String, SerialConector>();
        public AutoResetEvent autoEvent;
        // 定义一个标识确保线程同步
        private static readonly object _objPadLock = new object();
        private readonly object _syncLock = new object();

        int                  waitTime;
        byte[]               revBuffer;
        int              	 readTotalBytes;
        public delegate void ModbusSerialSlave(byte[] frame);
        public ModbusSerialSlave ModbusSlaveHandler = null;

        SerialConector()
        {
            IsOpen = false;
            waitTime = 5;
            autoEvent = new AutoResetEvent(false);
        }
        ~SerialConector()
        {
            if(IsOpen)
            {
                this.CloseConnection();
            }
        }
        public static SerialConector GetInstance(String connectID)
        {
            lock (_objPadLock)
            {
                if (instanceDir.ContainsKey(connectID))
                {
                    instance = instanceDir[connectID];
                }
                else
                {
                    instance = new SerialConector();
                    instanceDir[connectID] = instance;
                }
            }
            return instance;
        }
        public bool CreateSerialPort()
        {
            if (IsOpen)
                return true;

            try
            {
                string[] str = SerialPort.GetPortNames();
                if(str == null){
                    MessageBox.Show("本机没有串口！", "Error");
                    return false;
                }

                this.seriaPort = new SerialPort();
                
            
            }
            catch(Exception seriaEx)
            {
                MessageBox.Show(seriaEx.Message, "CreateSerialPort");
                return false;
            }

            return true;

            //打开串口设备
            
        }
        public void AddSerialListenor()
        {
            seriaPort.NewLine = NewLine;
            this.seriaPort.DataReceived += new SerialDataReceivedEventHandler(ReceivedData);
        }
        public void AutoResetEventSet()
        {
            seriaPort.DataReceived += (object sender, SerialDataReceivedEventArgs e) =>
            {
                autoEvent.Set();
            };
        }
        public bool OpenConnection(int comNum, int baud, int dataBits, int parity, int stopBits, String info)
        {
            try
            {
                if (IsOpen)
                {
                    return true;
                }
                IsOpen = false;

                if (comNum <= 0)
                {
                    comNum = 1;
                }

                seriaPort.BaudRate = baud;
                seriaPort.DataBits = dataBits;
                seriaPort.PortName = "COM" + comNum;
                //seriaPort.NewLine = "";
                switch(parity)  //校验设置
                {
                    case 0: seriaPort.Parity = Parity.None;
                        break;
                    case 1: seriaPort.Parity = Parity.Odd;
                        break;
                    case 2: seriaPort.Parity = Parity.Even;
                        break;
                }
                switch(stopBits)
                {
                    case 1: seriaPort.StopBits = StopBits.One;
                        break;
                    case 2: seriaPort.StopBits = StopBits.Two;
                        break;
                    default:
                        seriaPort.StopBits = StopBits.One;
                        break;
                }
                
                seriaPort.ReadTimeout =  500;
                seriaPort.WriteTimeout = 500;

                seriaPort.Open();
                IsOpen = true;
            }
            catch ( Exception ex)
            {
                MessageBox.Show(ex.Message, "OpenConnection");
                return false;
            }
            return true;
        }
        public void SetWaitTime(int time)
        {
            waitTime = time;
        }
        public int GetWaitTime()
        {
            return waitTime;
        }
        public bool PurgeWriteBuf()
        {
            if( !IsOpen)
            {
                return false;
            }
            seriaPort.DiscardOutBuffer();
            return true;
        }
        public bool PurgeReadBuf()
        {
            if (!IsOpen)
            {
                return false;
            }
            seriaPort.DiscardInBuffer();
            return true;
        }
        /// <summary>
        /// 向端口发送字符串数据
        /// </summary>
        /// <param name="buffer">要传输的字符串</param>
        public bool SendData(String sendData)
        {
            try
            {
                PurgeReadBuf();
                PurgeWriteBuf();

                char[] buffer = sendData.ToCharArray(); 
                seriaPort.Write(buffer, 0, sendData.Length);
                return true;
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SendData");
                return false;
            }
  
        }
        /// <summary>
        /// 向端口发送字节数组数据
        /// </summary>
        /// <param name="buffer">要传输的字节数组</param> 
        public bool SendData(byte[] buffer)
        {
            try
            {
                PurgeReadBuf();
                PurgeWriteBuf();
                 
                seriaPort.Write(buffer, 0, buffer.Length);
                revBuffer = null;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SendData");
                return false;
            }
        }

        public void ReceivedData(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                lock (_syncLock)
                {
                    //读取前先让缓存区写一段时间数据
                    autoEvent.WaitOne(waitTime);
                    byte[] frame;
                    if (ReadComByNum(out frame, 50))
                    {
                        ModbusSlaveHandler?.Invoke(frame);
                    }
                } 
            }
            catch(Exception ex)
            {
                seriaPort.DiscardInBuffer();
                revBuffer = null;
            }
            
        }

        public static byte[] HexToBytes(string hex)
        {
            if (hex == null)
            {
                throw new ArgumentNullException(nameof(hex));
            }

            if (hex.Length % 2 != 0)
            {
                throw new FormatException("Hex string must have even number of characters.");
            }

            byte[] bytes = new byte[hex.Length / 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }

            return bytes;
        }
        /// <summary>
        /// 从端口读取指定长度的字节数据
        /// </summary>
        /// <param name="buffer">接受到的字节数组</param>
        /// <param name="offset">数组开始点</param>
        /// <param name="len">长度</param>
        /// <param name="TimeOut">超时未返回时间</param>
        public bool ReadComByNum(out byte[] buf, int offset, int len, int TimeOut)
        {
            try
            {
                DateTime dt1 = DateTime.Now;
                byte[] ret_bytes = new byte[len];
                int retLen = 0,starInd = 0;
                do
                {
                    byte[] temBytes = new byte[seriaPort.BytesToRead];
                    retLen = seriaPort.Read(temBytes, 0, temBytes.Length);
                    
                    if (!temBytes.IsNull() && ret_bytes.Length >= temBytes.Length)
                    {
                        Array.Copy(temBytes, 0, ret_bytes, starInd, temBytes.Length);
                        starInd = starInd + temBytes.Length;
                    }
                    if (starInd == len)
                    {
                        buf = ret_bytes;
                        return true;
                    }
                }
                while ((DateTime.Now - dt1).TotalMilliseconds < TimeOut);
                throw new TimeoutException();
            }
            catch (Exception Ex)
            {
                //MessageBox.Show("端口连接异常！！！", "错误：", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                buf = null;
                return false;
            }
        }

        /// <summary>
        /// 从端口读取指定长度的字节数据
        /// </summary>
        /// <param name="frame">接受到的字节数组</param>
        /// <param name="TimeOut">超时未返回时间</param>
        public bool ReadComByNum(out byte[] frame, int TimeOut)
        {
            try
            {
                DateTime dt1 = DateTime.Now;
                var result = new StringBuilder();
                var singleByteBuffer = new byte[1];
                bool state = false;
                do
                {
                    if (seriaPort.Read(singleByteBuffer, 0, 1) == 0)
                    {
                        continue;
                    }
                    result.Append(Encoding.UTF8.GetChars(singleByteBuffer).First());
                    if (result.ToString().EndsWith(NewLine))
                    {
                        string frameHex = result.ToString().Substring(0, result.Length - NewLine.Length);
                        frame = HexToBytes(frameHex);
                        state = true;
                        return true;
                    }
                }
                while (!state && (DateTime.Now - dt1).TotalMilliseconds < TimeOut);
                throw new TimeoutException();
            }
            catch (Exception Ex)
            {
                frame = null;
                return false;
            }

        }

        /// <summary>
        /// 从端口读取指定长度的字节数据
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="TimeOut"></param>
        /// <returns></returns>
        public bool ReadComByNum(out string frame, int TimeOut)
        {
            try
            {
                DateTime dt1 = DateTime.Now;
                var result = new StringBuilder();
                var singleByteBuffer = new byte[1];
                bool state = false;
                do
                {
                    if (seriaPort.Read(singleByteBuffer, 0, 1) == 0)
                    {
                        continue;
                    }
                    result.Append(Encoding.UTF8.GetChars(singleByteBuffer).First());
                    if (result.ToString().EndsWith(NewLine))
                    {
                        frame = result.ToString().Substring(0, result.Length - NewLine.Length);
                        state = true;
                        return true;
                    }
                }
                while (!state && (DateTime.Now - dt1).TotalMilliseconds < TimeOut);
                throw new TimeoutException();
            }
            catch (Exception Ex)
            {
                frame = null;
                return false;
            }

        }

        public void CloseConnection()
        {
            try
            {
                seriaPort.Close();
                IsOpen = false;
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "CloseConnection");
            }
        }
    }
}
