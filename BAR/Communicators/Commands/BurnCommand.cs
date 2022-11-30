using BAR.Commonlib;
using BAR.Commonlib.Connectors;
using BAR.Commonlib.Connectors.Protocols;
using BAR.Communicates.Commands;
using CCWin.SkinControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BAR.Communicators.Commands
{
    public class BurnCommand:Command
    {
        public BurnCommand(bool isNetCtrl, String name, int revByteNum, int outTime = 100) : base(name)
        {
            if (isNetCtrl)
            {
                NetCtrl = NetConnector.GetInstance("BurnNetCtl");
            }
            else
            {
                SerialCtrl = SerialConector.GetInstance("BurnSerialCtl");
            }
            IRevByteNum = revByteNum;
            IOutTime = outTime;
        }

        public override bool Execute()
        {
            bool ret = false;
            byte[] rData = (byte[])RawData;

            byte[] buf;
            if (IRevByteNum == 0)
            {
                if (!SerialCtrl.ReadComByNum(out buf, IOutTime))
                {
                    SerialCtrl.PurgeReadBuf();
                    return false;
                }
            }
            else
            {
                ret = SerialCtrl.ReadComByNum(out buf, 0, IRevByteNum, IOutTime);
                if (!ret || buf.IsNull() || buf.Length < 5)
                {
                    SerialCtrl.PurgeReadBuf();
                    return false;
                }
            }
            
            //如果响应帧没有跟发送帧对应则丢弃
            if (rData[0] != buf[0] || rData[1] != buf[1])
            {
                SerialCtrl.PurgeReadBuf();
                return false;
            }
            //响应帧是否完整,不完整丟棄
            int framLen = 0;
            if (IRevByteNum != 0 && Modbus.GetRTUFrameLength(buf, ref framLen) && framLen != buf.Length)
            {
                return false;
            }

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
                Console.WriteLine("BurnCRC校验出错！");
            }
            SerialCtrl.PurgeReadBuf();
            return false;
        }

        public override bool Execute_Net()
        {
            bool ret = false;
            byte[] rData = (byte[])RawData;
            string[] strData, rBuff;
            string strBuf;

            strData = Encoding.ASCII.GetString(rData).Split(',');
            ret = NetCtrl.ReadNet(out strBuf);
            if (ret)
            {
                rBuff = strBuf.Split(',');
                //如果响应帧没有跟发送帧对应则丢弃
                if (strData[0] != rBuff[0] || strData[1] != rBuff[1])
                {
                    return false;
                }
                if (strBuf.Contains("END"))//检查回传数据结束符
                {
                    RevData = strBuf;
                    return true;
                }
            }
            return false;
        }
    }
}
