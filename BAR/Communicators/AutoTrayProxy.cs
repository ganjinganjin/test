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

namespace BAR.Communicators
{
    public class AutoTrayProxy : BaseProxy
    {
        public delegate void ProxyEventNotifyHander(MsgEvent evt);
        public event ProxyEventNotifyHander ProxyNotifyEvent;
        public Config g_config = Config.GetInstance();
        // 定义一个静态变量来保存类的实例
        private static AutoTrayProxy _instance = null;
        // 定义一个标识确保线程同步
        private static readonly object _objPadLock = new object();
        private AutoTrayProxy()
        {          
            this.CmdManger = CommandManager.GetInstance("AutoTray");
            CmdManger.CmdNotifyEvent += this.OnCommandBackHandle;
            CmdManger.CommandDequeue();
        }
        public static AutoTrayProxy GetInstance()
        {
            lock (_objPadLock)
            {
                if (_instance == null)
                {
                    _instance = new AutoTrayProxy();
                }
            }
            return (AutoTrayProxy)_instance;
        }
        
        public void SendCmd(String cmdName, byte[] sendData, int revBytesNum=0, bool isNoRepeatCmd = true, int outTime = 50)
        {
            AutoTrayCommand cmd = new AutoTrayCommand(cmdName, revBytesNum, outTime)
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
            AutoTrayCommand autoTrayCmd = (AutoTrayCommand)cmd;

            byte[] buf = (byte[])autoTrayCmd.RevData;
            byte[] singleF = new byte[4];
            //如果回来的数据为空,直接丢弃
            if (buf == null)
                return;
            object retB;
            switch (cmdName)
            {
                case "ReadInkData":
                    if (Modbus.ReceiveDataProcess(buf, out retB))
                    {

                    }
                    break;

                case "ReadDotParameter":
                    if (Modbus.ReceiveDataProcess(buf, out retB))
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            singleF[3] = buf[4 * i + 5];
                            singleF[2] = buf[4 * i + 6];
                            singleF[1] = buf[4 * i + 3];
                            singleF[0] = buf[4 * i + 4];
                            AutoTray.getConfig[i] = BitConverter.ToSingle(singleF, 0);
                        }
                        if (ProxyNotifyEvent != null)
                        {
                            MsgEvent evt = new MsgEvent(MsgEvent.MSG_BURNFLASH, null);
                            Task t1 = Task.Factory.StartNew(() => ProxyNotifyEvent(evt));//开启检测线程
                        }
                    }
                    break;

                case "ReadTrayStateAlarm":
                    if (Modbus.ReceiveDataProcess(buf, out retB))
                    {
                        AutoTray.StatusAlarmWord = (ushort)(buf[3] * 256 + buf[4]);
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
