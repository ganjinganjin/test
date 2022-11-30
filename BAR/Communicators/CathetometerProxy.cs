using BAR.Commonlib;
using BAR.Commonlib.Connectors;
using BAR.Commonlib.FrameWork.Patterns.Command;
using BAR.Communicates;
using BAR.Communicators.Commands;
using BAR.Commonlib.Connectors.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BAR.Commonlib.Events;
using System.Threading.Tasks;
using System.IO;

namespace BAR.Communicators
{
    public class CathetometerProxy : BaseProxy
    {
        public delegate void ProxyEventNotifyHander(MsgEvent evt);
        public event ProxyEventNotifyHander ProxyNotifyEvent;
        public Config g_config = Config.GetInstance();
        // 定义一个静态变量来保存类的实例
        private static CathetometerProxy _instance = null;
        // 定义一个标识确保线程同步
        private static readonly object _objPadLock = new object();
        /// <summary>
        /// 采集模板类型 ture：40；false：200
        /// </summary>
        public bool flag = false;
        private CathetometerProxy()
        {          
            this.CmdManger = CommandManager.GetInstance("Cathetometer");
            CmdManger.CmdNotifyEvent += this.OnCommandBackHandle;
            CmdManger.CommandDequeue();
        }
        public static CathetometerProxy GetInstance()
        {
            lock (_objPadLock)
            {
                if (_instance == null)
                {
                    _instance = new CathetometerProxy();
                }
            }
            return (CathetometerProxy)_instance;
        }

        public void SendCmd(String cmdName, byte[] sendData, int revByteNum, bool isNoRepeatCmd = true, int outTime = 500)
        {
            CathetometerCommand cmd = new CathetometerCommand(cmdName, revByteNum, outTime)
            {
                IsCanRedo = true,
                RawData = sendData,
                SendData = revByteNum == 0 ? HEX_to_ASCII(sendData) : sendData
            };
            this.SendCmd(cmd, isNoRepeatCmd);
        }

        public void SendCmd(String cmdName, string sendData, int revByteNum, bool isNoRepeatCmd = true, int outTime = 500)
        {
            CathetometerCommand cmd = new CathetometerCommand(cmdName, revByteNum, outTime)
            {
                IsCanRedo = true,
                StrSendData = sendData,
                SendData = Encoding.UTF8.GetBytes(sendData.ToCharArray())
            };
            this.SendCmd(cmd, isNoRepeatCmd);
        }
        private void OnCommandBackHandle(BaseCmd cmd)
        {
            String cmdName = cmd.CommandName;
            CathetometerCommand cathetometerCmd = (CathetometerCommand)cmd;

            byte[] buf = (byte[])cathetometerCmd.RevData;
            byte[] singleF = new byte[4];
            //如果回来的数据为空,直接丢弃
            if (buf == null && cathetometerCmd.IRevByteNum != 0)
                return;
            object retB;
            switch (cmdName)
            {
                case "ReadAI":
                    if (Modbus.ReceiveDataProcess(buf, out retB))
                    {
                        if (Config.Altimeter == GlobConstData.ST_Altimeter_YX)//远向采集模块
                        {
                            singleF[0] = buf[6];
                            singleF[1] = buf[5];
                            singleF[2] = buf[4];
                            singleF[3] = buf[3];
                            Cathetometer.Value = BitConverter.ToSingle(singleF, 0) * 1.875d - 7.5d;
                        }
                        else if(Config.Altimeter == GlobConstData.ST_Altimeter_ZZ_C1030)//舟正采集模块 + HG_C1030
                        {
                            Cathetometer.Value = (buf[3] * 256 + buf[4]) * (20d / 4095d) * 1.875d - 7.5d;
                        }
                        else//舟正采集模块 + HG_C1100
                        {
                            Cathetometer.Value = (buf[3] * 256 + buf[4]) * (20d / 4095d) * (70d / 16d) - ((70d / 16d) * 4);
                        }
                    }
                    break;

                case "ReadMD":
                    string str = cathetometerCmd.StrRevData.ToString().Substring(7, 8);
                    Cathetometer.Value = Convert.ToDouble(str.Insert(4, "."));
                    break;

                default:
                    break;
            }
        }
        private byte[] HEX_to_ASCII(byte[] TxBuffer)
        {
            var nlAscii = Encoding.UTF8.GetBytes(SerialConector.NewLine.ToCharArray());
            var frame = new MemoryStream(TxBuffer.Length + nlAscii.Length);
            frame.Write(TxBuffer, 0, TxBuffer.Length);
            frame.Write(nlAscii, 0, nlAscii.Length);
            return frame.ToArray();
        }
    }
}
