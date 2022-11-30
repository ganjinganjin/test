using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BAR.Commonlib;
using BAR.Commonlib.Connectors;
using BAR.Commonlib.Connectors.Protocols;
using BAR.Commonlib.Events;
using BAR.Commonlib.FrameWork.Patterns.Command;
using BAR.Communicators.Commands;

namespace BAR.Communicates
{
    public class BraidProxy : BaseProxy
    {
        private static BraidProxy _instance = null;

        public delegate void ProxyEventNotifyHander(MsgEvent evt);
        public event ProxyEventNotifyHander ProxyNotifyEvent;
        // 定义一个标识确保线程同步
        private static readonly object _objPadLock = new object();
        public Config g_config = Config.GetInstance();
        private BraidProxy()
        {
            this.CmdManger = CommandManager.GetInstance("Braid");
            CmdManger.CmdNotifyEvent += this.OnCommandBackHandle;
            CmdManger.CommandDequeue();
        }
        public static BraidProxy GetInstance()
        {
            lock (_objPadLock)
            {
                if (_instance == null)
                {
                    _instance = new BraidProxy();
                }
            }
            return (BraidProxy)_instance;
        }
        public void SendCmd(String cmdName, byte[] sendData ,int revByteNum, bool isNoRepeatCmd = true, int outTime = 50 )
        {
            BraidCommand cmd = new BraidCommand(cmdName, revByteNum, outTime)
            {
                IsCanRedo = true,
                RawData = sendData,
                SendData = sendData
            };
            this.SendCmd(cmd, isNoRepeatCmd);
        }
        private void OnCommandBackHandle(BaseCmd cmd)
        {
            String cmdName = cmd.CommandName;
            BraidCommand braidCmd = (BraidCommand)cmd;  

            byte[] buf = (byte[])braidCmd.RevData;
            byte[] singleF = new byte[4];
            //如果回来的数据为空,直接丢弃
            if (buf == null)
                return;
            object retB;
            switch (cmdName)
            {
                case "ReadBredeParameter":
                    if(Modbus.ReceiveDataProcess(buf, out retB))
                    {
                        Brede.getConfig.tEmptyBredeFrap = (ushort)(buf[3] * 256 + buf[4]);
                        Brede.getConfig.tEmptyBredeSend = (ushort)(buf[5] * 256 + buf[6]);
                        Brede.getConfig.tBraidFrap = (ushort)(buf[7] * 256 + buf[8]);
                        Brede.getConfig.tHotMelt = (ushort)(buf[9] * 256 + buf[10]);
                        Brede.getConfig.tUV_Lamp = (ushort)(buf[11] * 256 + buf[12]);
                        Brede.getConfig.tDot = (ushort)(buf[13] * 256 + buf[14]);

                        Brede.getConfig.reserved_1 = (ushort)(buf[15] * 256 + buf[16]);
                        Brede.getConfig.reserved_2 = (ushort)(buf[17] * 256 + buf[18]);

                        Brede.getConfig.acc = (ushort)(buf[19] * 256 + buf[20]);
                        Brede.getConfig.dec = (ushort)(buf[21] * 256 + buf[22]);

                        singleF[3] = buf[25];
                        singleF[2] = buf[26];
                        singleF[1] = buf[23];
                        singleF[0] = buf[24];
                        Brede.getConfig.space = BitConverter.ToSingle(singleF, 0);
                        singleF[3] = buf[29];
                        singleF[2] = buf[30];
                        singleF[1] = buf[27];
                        singleF[0] = buf[28];
                        Brede.getConfig.speed = BitConverter.ToSingle(singleF, 0);

                        if (ProxyNotifyEvent != null)
                        {
                            MsgEvent evt = new MsgEvent(MsgEvent.MSG_BRAIDFLASH, null);
                            Task t1 = Task.Factory.StartNew(() => ProxyNotifyEvent(evt));//开启检测线程
                        }
                    }
                    break;
         
                case "ReadBredeStateAlarm":
                    if (Modbus.ReceiveDataProcess(buf, out retB))
                    {
                        Brede.StatusAlarmWord = (ushort)(buf[3] * 256 + buf[4]);
                        //Console.WriteLine(Brede.StatusAlarmWord);
                    }
                    break;

                default:
                    break;
        }
    }
}
}
