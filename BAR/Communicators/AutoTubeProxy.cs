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
    public class AutoTubeProxy : BaseProxy
    {
        public delegate void ProxyEventNotifyHander(MsgEvent evt);
        public event ProxyEventNotifyHander ProxyNotifyEvent;
        // 定义一个静态变量来保存类的实例
        private static AutoTubeProxy _instance = null; 
        // 定义一个标识确保线程同步
        private static readonly object _objPadLock = new object();
        private AutoTubeProxy()
        {          
            this.CmdManger = CommandManager.GetInstance("AutoTube");
            CmdManger.CmdNotifyEvent += this.OnCommandBackHandle;
            CmdManger.CommandDequeue();
        }
        public static AutoTubeProxy GetInstance()
        {
            lock (_objPadLock)
            {
                if (_instance == null)
                {
                    _instance = new AutoTubeProxy();
                }
            }
            return (AutoTubeProxy)_instance;
        }
        
        public void SendCmd(String cmdName, byte[] sendData, int revBytesNum=0, bool isNoRepeatCmd = true, int outTime = 50)
        {
            AutoTubeCommand cmd = new AutoTubeCommand(cmdName, revBytesNum, outTime)
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
            AutoTubeCommand autoTubeCmd = (AutoTubeCommand)cmd;

            byte[] buf = (byte[])autoTubeCmd.RevData;
            byte[] singleF = new byte[4];
            //如果回来的数据为空,直接丢弃
            if (buf == null)
                return;
            object retB;
            switch (cmdName)
            {
                case "ReadTubeStateAlarm":
                    if (Modbus.ReceiveDataProcess(buf, out retB))
                    {
                        AutoTube.StatusAlarmWord = (ushort)(buf[3] * 256 + buf[4]);
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
