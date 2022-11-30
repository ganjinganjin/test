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
    public class ZoomLensProxy : BaseProxy
    {
        public delegate void ProxyEventNotifyHander(MsgEvent evt);
        public event ProxyEventNotifyHander ProxyNotifyEvent;
        public Config g_config = Config.GetInstance();
        // 定义一个静态变量来保存类的实例
        private static ZoomLensProxy _instance = null;
        // 定义一个标识确保线程同步
        private static readonly object _objPadLock = new object();
        /// <summary>
        /// 采集模板类型 ture：40；false：200
        /// </summary>
        public bool flag = false;
        private ZoomLensProxy()
        {          
            this.CmdManger = CommandManager.GetInstance("ZoomLens");
            CmdManger.CmdNotifyEvent += this.OnCommandBackHandle;
            CmdManger.CommandDequeue();
        }
        public static ZoomLensProxy GetInstance()
        {
            lock (_objPadLock)
            {
                if (_instance == null)
                {
                    _instance = new ZoomLensProxy();
                }
            }
            return (ZoomLensProxy)_instance;
        }

        public void SendCmd(String cmdName, byte[] sendData, int revByteNum, bool isNoRepeatCmd = true, int outTime = 50)
        {
            ZoomLensCommand cmd = new ZoomLensCommand(cmdName, revByteNum, outTime)
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
            ZoomLensCommand zoomLensCommand = (ZoomLensCommand)cmd;

            byte[] buf = (byte[])zoomLensCommand.RevData;
            byte[] singleF = new byte[4];
            //如果回来的数据为空,直接丢弃
            if (buf == null && zoomLensCommand.IRevByteNum != 0)
                return;
            object retB;
            switch (cmdName)
            {
                case "ReadZoomLensStatus":
                    if (Modbus.ReceiveDataProcess(buf, out retB))
                    {
                        ZoomLens.MotorStatus = buf[3];
                        singleF[0] = buf[11];
                        singleF[1] = buf[10];
                        singleF[2] = buf[9];
                        singleF[3] = buf[8];
                        ZoomLens.NowPos = BitConverter.ToInt32(singleF, 0);
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
