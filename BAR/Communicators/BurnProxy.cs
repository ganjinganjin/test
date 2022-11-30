using BAR.Commonlib;
using BAR.Commonlib.Connectors;
using BAR.Commonlib.Connectors.Protocols;
using BAR.Communicates;
using BAR.Communicators.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BAR.Commonlib.FrameWork.Patterns.Command;
using BAR.Commonlib.Events;
using System.Threading.Tasks;
using System.Threading;

namespace BAR.Communicators
{
    public class BurnProxy : BaseProxy
    {
        public delegate void ProxyEventNotifyHander(MsgEvent evt);
        public event ProxyEventNotifyHander ProxyNotifyEvent;
        private static BurnProxy _instance = null;
        // 定义一个标识确保线程同步
        private static readonly object _objPadLock = new object();
        private static readonly object AKLock = new object();
        public      Config              g_config = Config.GetInstance();
        public      SerialConector      SerialCtrl;
        public      NetConnector        NetCtrl;
        protected   byte[]              ArrRevBytes;
        protected   byte[]              ArrFrameData;
        public bool IsNetCtrl;
        private BurnProxy()
        {
            if (UserTask.ProgrammerType == GlobConstData.Programmer_RD)
            {
                IsNetCtrl = true;
                NetCtrl = NetConnector.GetInstance("BurnNetCtl");
                NetCtrl.Create(GlobalParam.ServerIP, GlobalParam.ServerPort);
                CmdManger = CommandManager.GetInstance("Burn");
                CmdManger.CmdNotifyEvent += RD_CommandBackHandle;
                CmdManger.CommandDequeue_Net();
                return;
            }
            IsNetCtrl = false;
            SerialCtrl = SerialConector.GetInstance("BurnSerialCtl");
            if (SerialCtrl.CreateSerialPort() && !SerialCtrl.IsOpen)
            {
                SerialCtrl.OpenConnection(g_config.ArrStrutCom[3].ICom, g_config.ArrStrutCom[3].IBaud, g_config.ArrStrutCom[3].IDataBits, g_config.ArrStrutCom[3].IParity, g_config.ArrStrutCom[3].IStopBits, "Burn Ports");
            }
            if (UserTask.ProgrammerType == GlobConstData.Programmer_AK)
            {
                SerialCtrl.ModbusSlaveHandler += Modbus_AK_Handler;
                SerialCtrl.AddSerialListenor();
                ArrRevBytes = new byte[0];
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_DP)
            {
                //SerialCtrl.AutoResetEventSet();
                //CmdManger = CommandManager.GetInstance("Burn");
                //CmdManger.CmdNotifyEvent += DP_CommandBackHandle;
                //CmdManger.CommandDequeue();
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_WG || UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY)
            {
                SerialCtrl.AutoResetEventSet();
                CmdManger = CommandManager.GetInstance("Burn");
                CmdManger.CmdNotifyEvent += WG_CommandBackHandle;
                CmdManger.CommandDequeue();
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_YED)
            {
                SerialCtrl.AutoResetEventSet();
                CmdManger = CommandManager.GetInstance("Burn");
                CmdManger.CmdNotifyEvent += YED_CommandBackHandle;
                CmdManger.CommandDequeue();
            }
        }
        public static BurnProxy GetInstance()
        {
            if (_instance == null)
            {
                lock (_objPadLock)
                {
                    if (_instance == null)
                    {
                        _instance = new BurnProxy();
                    }
                }
            }
            return (BurnProxy)_instance;
        }
        public void SendCmd(String cmdName, byte[] sendData, int revByteNum, bool isNoRepeatCmd = true, int outTime = 50)
        {
            BurnCommand cmd = new BurnCommand(IsNetCtrl, cmdName, revByteNum, outTime)
            {
                IsCanRedo = true,
                RawData = sendData,
                SendData = revByteNum == 0 ? HEX_to_ASCII(sendData) : sendData
            };
            this.SendCmd(cmd, isNoRepeatCmd);
        }
        
        private void Modbus_AK_Error(byte[] RxBuffer, byte Err_Code)  //报告通讯出错
        {
            int CRCtemp = 0x0000;
            byte[] TxBuffer;
            switch (RxBuffer[1])
            {
                case 0x65://告知指定模组（A-D）IC是否放好
                case 0x68://告知指定模组（A-D）IC是否放好
                case 0x66://告知模组（A-F）烧录情况
                    TxBuffer = new byte[6];
                    TxBuffer[0] = RxBuffer[0];
                    TxBuffer[1] = (byte)(RxBuffer[1] | 0x80);
                    TxBuffer[2] = RxBuffer[2];
                    TxBuffer[3] = Err_Code;
                    CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
                    TxBuffer[4] = (byte)(CRCtemp & 0x00FF);
                    TxBuffer[5] = (byte)((CRCtemp & 0xFF00) >> 8);
                    break;             

                default:
                    TxBuffer = new byte[5];
                    TxBuffer[0] = RxBuffer[0];
                    TxBuffer[1] = (byte)(RxBuffer[1] | 0x80);
                    TxBuffer[2] = Err_Code;
                    CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
                    TxBuffer[3] = (byte)(CRCtemp & 0x00FF);
                    TxBuffer[4] = (byte)((CRCtemp & 0xFF00) >> 8);
                    break;
            }
            SerialCtrl.SendData(HEX_to_ASCII(TxBuffer));
        }
        private bool ChecksumsMatch(byte[] RxBuffer, int DataLen)
        {
            ushort CRC_Value = 0x0000;
            CRC_Value = (ushort)(RxBuffer[DataLen - 1] * 256 + RxBuffer[DataLen - 2]);
            if (CRC_Value == Modbus.Crc16(RxBuffer, DataLen - 2))
            {
                return true;
            }
            return false;
        }

        private byte[] HEX_to_ASCII(byte[] TxBuffer)
        {
            var TxAciiBuffer = Encoding.UTF8.GetBytes(TxBuffer.SelectMany(n => n.ToString("X2")).ToArray());
            var nlAscii = Encoding.UTF8.GetBytes(SerialConector.NewLine.ToCharArray());
            var frame = new MemoryStream(TxAciiBuffer.Length + nlAscii.Length);
            frame.Write(TxAciiBuffer, 0, TxAciiBuffer.Length);
            frame.Write(nlAscii, 0, nlAscii.Length);
            return frame.ToArray();
        }

        private void Modbus_AK_Handler(byte[] RxBuffer)
        {
            Monitor.Enter(AKLock);
            int CRCtemp;
            byte[] TxBuffer;

            if (RxBuffer[0] != 0x01)
            {
                Monitor.Exit(AKLock);
                return;
            }
            
            if (!(ChecksumsMatch(RxBuffer, RxBuffer.Length)))
            {
                Modbus_AK_Error(RxBuffer,0xFF); //CRC校验错误
                Monitor.Exit(AKLock);
                return;
            }
            switch (RxBuffer[1])
            {
                case 0x63://告知当前协议版本
                    TxBuffer = new byte[6];
                    TxBuffer[0] = RxBuffer[0];
                    TxBuffer[1] = RxBuffer[1];
                    TxBuffer[2] = 0x01;
                    TxBuffer[3] = 0x01;
                    CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
                    TxBuffer[4] = (byte)(CRCtemp & 0x00FF);
                    TxBuffer[5] = (byte)((CRCtemp & 0xFF00) >> 8);
                    SerialCtrl.SendData(HEX_to_ASCII(TxBuffer));
                    break;

                case 0x64://模组使能
                    TxBuffer = new byte[5 + UserConfig.ScketGroupC];
                    TxBuffer[0] = RxBuffer[0];
                    TxBuffer[1] = RxBuffer[1];
                    TxBuffer[2] = (byte)UserConfig.ScketGroupC;
                    for (int i = 0; i < UserConfig.ScketGroupC; i++)
                    {
                        TxBuffer[3 + i] = 0xFF;
                    }
                    CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
                    TxBuffer[TxBuffer.Length - 2] = (byte)(CRCtemp & 0x00FF);
                    TxBuffer[TxBuffer.Length - 1] = (byte)((CRCtemp & 0xFF00) >> 8);
                    SerialCtrl.SendData(HEX_to_ASCII(TxBuffer));
                    break;

                case 0x65://告知指定模组（A-D）IC是否放好
                    TxBuffer = new byte[6];
                    TxBuffer[0] = RxBuffer[0];
                    TxBuffer[1] = RxBuffer[1];
                    TxBuffer[2] = RxBuffer[2];
                    if (UserTimer.GetSysTime() > Download_AK.Timer_StartDelay[RxBuffer[2] - 0x0A] + 6000)
                    {
                        TxBuffer[3] = Download_AK.programer[RxBuffer[2] - 0x0A].Start;//01准备好，02未准备好
                    }
                    else
                    {
                        TxBuffer[3] = 0x02;
                    }
                    CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
                    TxBuffer[4] = (byte)(CRCtemp & 0x00FF);
                    TxBuffer[5] = (byte)((CRCtemp & 0xFF00) >> 8);
                    SerialCtrl.SendData(HEX_to_ASCII(TxBuffer));
                    break;

                case 0x66://告知模组（A-F）烧录情况
                    TxBuffer = new byte[5];
                    TxBuffer[0] = RxBuffer[0];
                    TxBuffer[1] = RxBuffer[1];
                    TxBuffer[2] = RxBuffer[2];
                    Download_AK.programer[RxBuffer[2] - 0x0A].Start = 0x02;
                    Download_AK.programer[RxBuffer[2] - 0x0A].Result = RxBuffer[3];
                    Download_AK.programer[RxBuffer[2] - 0x0A].Finish = true;
                    Download_AK.Timer_StartDelay[RxBuffer[2] - 0x0A] = UserTimer.GetSysTime();
                    CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
                    TxBuffer[3] = (byte)(CRCtemp & 0x00FF);
                    TxBuffer[4] = (byte)((CRCtemp & 0xFF00) >> 8);
                    SerialCtrl.SendData(HEX_to_ASCII(TxBuffer));
                    break;

                case 0x67://告知模组初始化已完成
                    SerialCtrl.SendData(HEX_to_ASCII(RxBuffer));
                    break;

                case 0x68://告知指定模组（A-D）IC是否放好
                    TxBuffer = new byte[7];
                    TxBuffer[0] = RxBuffer[0];
                    TxBuffer[1] = RxBuffer[1];
                    TxBuffer[2] = RxBuffer[2];
                    if (UserTimer.GetSysTime() > Download_AK.Timer_StartDelay[RxBuffer[2] - 0x0A] + 6000)
                    {
                        TxBuffer[3] = Download_AK.programer[RxBuffer[2] - 0x0A].Start;//01准备好，02未准备好
                    }
                    else
                    {
                        TxBuffer[3] = 0x02;
                    }
                    //TxBuffer[3] = Download_AK.programer[RxBuffer[2] - 0x0A].Start;//01准备好，02未准备好
                    TxBuffer[4] = Download_AK.programer[RxBuffer[2] - 0x0A].Enable;
                    CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
                    TxBuffer[5] = (byte)(CRCtemp & 0x00FF);
                    TxBuffer[6] = (byte)((CRCtemp & 0xFF00) >> 8);
                    SerialCtrl.SendData(HEX_to_ASCII(TxBuffer));
                    break;

                default:
                    Modbus_AK_Error(RxBuffer,0x01);
                    break;
            }
            Monitor.Exit(AKLock);
        }

        private void DP_CommandBackHandle(BaseCmd cmd)
        {
            String cmdName = cmd.CommandName;
            BurnCommand burnCmd = (BurnCommand)cmd;

            byte[] buf = (byte[])burnCmd.RevData;
            byte[] singleF = new byte[4];
            //如果回来的数据为空,直接丢弃
            if (buf == null)
                return;
            object retB;
            switch (cmdName)
            {
                case "ReadDownloadStatus_Result":
                    for (int i = 0; i < UserConfig.AllScketC; i++)
                    {
                        Download_DP.programer[i / UserConfig.ScketUnitC].Unit[i % UserConfig.ScketUnitC].Status = buf[4 + 2 * i];
                    }
                    break;

                case "GetReadyState":
                    Auto_Flag.BurnOnline = buf[2] == 0x01;
                    break;

                default:
                    break;
            }
        }

        private void WG_CommandBackHandle(BaseCmd cmd)
        {
            String cmdName = cmd.CommandName;
            BurnCommand burnCmd = (BurnCommand)cmd;

            byte[] buf = (byte[])burnCmd.RevData;
            byte[] singleF = new byte[4];
            //如果回来的数据为空,直接丢弃
            if (buf == null)
                return;
            if (cmd.CommandName.Contains("ReadDownloadStatus_Result"))
            {
                cmdName = "ReadDownloadStatus_Result";
            }
            object retB;
            switch (cmdName)
            {
                case "ReadDownloadParameter":
                    if (Modbus.ReceiveDataProcess(buf, out retB))
                    {
                        Download_WG.getParameter.PulseWidth_Start = (ushort)(buf[3] * 256 + buf[4]);
                        Download_WG.getParameter.Time_Busy = (ushort)(buf[5] * 256 + buf[6]);
                        Download_WG.getParameter.Time_EOT = (ushort)(buf[7] * 256 + buf[8]);
                        Download_WG.getParameter.Time_OKNG = (ushort)(buf[9] * 256 + buf[10]);
                        Download_WG.getParameter.Level_Start = (ushort)(buf[11] * 256 + buf[12]);
                        Download_WG.getParameter.Level_Busy = (ushort)(buf[13] * 256 + buf[14]);
                        Download_WG.getParameter.Level_OK = (ushort)(buf[15] * 256 + buf[16]);
                        Download_WG.getParameter.Level_NG = (ushort)(buf[17] * 256 + buf[18]);
                        Download_WG.getParameter.RepeatNumber = (ushort)(buf[19] * 256 + buf[20]);
                        Download_WG.getParameter.Time_Down = (ushort)(buf[21] * 256 + buf[22]);
                        Download_WG.getParameter.ID = (ushort)(buf[23] * 256 + buf[24]);

                        if (ProxyNotifyEvent != null)
                        {
                            MsgEvent evt = new MsgEvent(MsgEvent.MSG_BURNFLASH, null);
                            Task t1 = Task.Factory.StartNew(() => ProxyNotifyEvent(evt));//开启检测线程
                        }
                    }
                    break;

                case "ReadDownloadStatus_Result":
                    if (Modbus.ReceiveDataProcess(buf, out retB))
                    {
                        int temp = (buf[0] - 1) * 8;
                        for (int i = 0; i < 8; i++)
                        {
                            if (i % 2 == 0)
                            {
                                Download_WG.programer_Unit[i + temp].Status = (byte)(buf[3 + i / 2] & 0x0F);
                                Download_WG.programer_Unit[i + temp].Result = (byte)(buf[7 + i / 2] & 0x0F);
                            }
                            else
                            {
                                Download_WG.programer_Unit[i + temp].Status = (byte)((buf[3 + i / 2] & 0xF0) >> 4);
                                Download_WG.programer_Unit[i + temp].Result = (byte)((buf[7 + i / 2] & 0xF0) >> 4);
                            }
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        private void YED_CommandBackHandle(BaseCmd cmd)
        {
            String cmdName = cmd.CommandName;
            BurnCommand burnCmd = (BurnCommand)cmd;

            byte[] buf = (byte[])burnCmd.RevData;
            byte[] singleF = new byte[4];
            //如果回来的数据为空,直接丢弃
            if (buf == null)
                return;
            object retB;
            switch (cmdName)
            {
                case "ReadDownloadStatus_Result":
                    for (int i = 0; i < 8; i++)
                    {
                        if (i % 2 == 0)
                        {
                            Download_YED.programer_Group[i].Status = buf[4 + i];
                        }
                        else
                        {
                            Download_YED.programer_Group[i].Status = buf[2 + i];
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        private void RD_CommandBackHandle(BaseCmd cmd)
        {
            String cmdName = cmd.CommandName;
            BurnCommand burnCmd = (BurnCommand)cmd;

            string strbuf = (string)burnCmd.RevData;
            //如果回来的数据为空,直接丢弃
            if (strbuf == null)
                return;
            string[] strSubAry = strbuf.Split(',');
            switch (cmdName)
            {
                case "DownloadStart_1":
                    
                    break;

                case "ReadDownloadStatus_Result_1":
                    if (strSubAry[0] == "Site1")
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            Download_RD.programer_Unit[i].Status = Convert.ToByte(strSubAry[i + 2]);
                        }
                    }
                    break;

                case "ReadDownloadStatus_Result_2":
                    if (strSubAry[0] == "Site2")
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            Download_RD.programer_Unit[i + 8].Status = Convert.ToByte(strSubAry[i + 2]);
                        }
                    }
                    break;

                case "ReadDownloadStatus_Result_3":
                    if (strSubAry[0] == "Site3")
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            Download_RD.programer_Unit[i + 16].Status = Convert.ToByte(strSubAry[i + 2]);
                        }
                    }
                    break;

                case "ReadDownloadStatus_Result_4":
                    if (strSubAry[0] == "Site4")
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            Download_RD.programer_Unit[i + 24].Status = Convert.ToByte(strSubAry[i + 2]);
                        }
                    }
                    break;

                default:
                    break;
            }
        }
    } 
}
