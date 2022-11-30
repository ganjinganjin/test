﻿using BAR.Commonlib;
using BAR.Commonlib.Connectors;
using BAR.Commonlib.Connectors.Protocols;
using BAR.Communicates.Commands;
using CCWin.SkinControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAR.Communicators.Commands
{
    public class CathetometerCommand : Command
    {
        public Config g_config = Config.GetInstance();
        public Act g_act = Act.GetInstance();
        public CathetometerCommand(String name, int revByteNum, int outTime = 50) : base(name)
        {
            IOutTime = outTime;
            IRevByteNum = revByteNum;
            SerialCtrl = SerialConector.GetInstance("CathetometerSerialCtl");
        }
        public override bool Execute()
        {
            bool ret = false;
            byte[] rData = (byte[])RawData;
            string strBuff = null;

            byte[] buf = null;
            if (IRevByteNum == 0)
            {
                if (!SerialCtrl.ReadComByNum(out strBuff, IOutTime))
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
            if (Config.Altimeter == GlobConstData.ST_Altimeter_SX)
            {
                if (strBuff.Substring(4, 3) != StrSendData.ToString().Substring(4, 3))
                {
                    SerialCtrl.PurgeReadBuf();
                    return false;
                }
            }
            else
            {
                if (rData[0] != buf[0] || rData[1] != buf[1])
                {
                    SerialCtrl.PurgeReadBuf();
                    return false;
                }
            }
            
            //响应帧是否完整,不完整丟棄
            int framLen = 0;
            if (IRevByteNum != 0 && Modbus.GetRTUFrameLength(buf, ref framLen) && framLen != buf.Length)
            {
                return false;
            }

            int CRCtemp, CRCtemp2;
            if (Config.Altimeter == GlobConstData.ST_Altimeter_SX)
            {
                var DateBuffer = Encoding.UTF8.GetBytes(strBuff.ToCharArray());
                if (Modbus.BCC(DateBuffer, DateBuffer.Length - 2) == strBuff.Substring(strBuff.Length - 2, 2))
                {
                    this.StrRevData = strBuff;
                    SerialCtrl.PurgeReadBuf();
                    return true;
                }
                else
                {
                    Console.WriteLine("BurnBCC校验出错！");
                }
            }
            else
            {
                
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
            }
            
            SerialCtrl.PurgeReadBuf();
            return false;
        }
    }
}
