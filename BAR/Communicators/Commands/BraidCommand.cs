using BAR.Commonlib;
using BAR.Commonlib.Connectors;
using BAR.Commonlib.Connectors.Protocols;
using BAR.Communicates.Commands;
using CCWin.SkinControl;
using CCWin.Win32.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAR.Communicators.Commands
{
    public class BraidCommand:Command
    {
        public Config           g_config = Config.GetInstance();
        public Act              g_act = Act.GetInstance();
        public BraidCommand(String name, int revByteNum, int outTime = 50): base(name)
        {
            SerialCtrl = SerialConector.GetInstance("BraidSerialCtl");
            IRevByteNum = revByteNum;
            IOutTime = outTime;
        }
        public override bool Execute()
        {
            bool ret = false;
            byte[] sData = (byte[])SendData;
           
            byte[] buf;
            
            ret = SerialCtrl.ReadComByNum(out buf, 0, IRevByteNum, IOutTime);

            if (!ret || buf.IsNull() || buf.Length < 5)
            {
                SerialCtrl.PurgeReadBuf();
                return false;
            }
            //如果响应帧没有跟发送帧对应则丢弃
            if(sData[0] != buf[0] || sData[1] != buf[1])
            {
                SerialCtrl.PurgeReadBuf();
                return false;
            }
            //响应帧是否完整,不完整丟棄
            int framLen = 0;
            if (Modbus.GetRTUFrameLength(buf, ref framLen) && framLen == buf.Length)
            {
                int CRCtemp, CRCtemp2;
               
                CRCtemp = Modbus.Crc16(buf, buf.Length - 2);
                CRCtemp2 = (int)(buf[buf.Length - 1] * 256 + buf[buf.Length - 2]);
                if (CRCtemp == CRCtemp2)
                {
                    this.RevData = buf;
                    SerialCtrl.PurgeReadBuf();
                    return true;
                }
                else
                {
                    Console.WriteLine("BraidCRC校验出错！");
                }
            }
            SerialCtrl.PurgeReadBuf();
            return false;
        }
    }
}
