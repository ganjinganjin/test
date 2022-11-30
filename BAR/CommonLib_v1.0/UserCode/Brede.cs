using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BAR.Commonlib;
using BAR.Commonlib.Connectors;
using BAR.Commonlib.Connectors.Protocols;
using BAR.Communicates;
using BAR.Communicators.Commands;

namespace BAR
{
    public class Brede
    {
        public Config g_config = Config.GetInstance();
        public static Act g_act = Act.GetInstance();
        public static BraidProxy proxy = BraidProxy.GetInstance();

        private static Brede _instance = null;

        private static readonly object padlock = new object();

        public static ushort StatusAlarmWord = 0;
        private SerialConector SerialCtrl;
        
        public static Brede_Config setConfig = new Brede_Config();
        public static Brede_Config getConfig = new Brede_Config();
        public static bool TCPowerOn;

        #region----------------声明命令常量-------------------
        public const UInt16 Cmd_SendMaterial    = 0x0001;
        public const UInt16 Cmd_ReceiveMaterial = 0x0002;
        public const UInt16 Cmd_RMInchFwd       = 0x0004;
        public const UInt16 Cmd_RMInchRev       = 0x0008;

        public const UInt16 Cmd_TCPowerOpen     = 0x0010;
        public const UInt16 Cmd_TCPowerClose    = 0x0020;
        public const UInt16 Cmd_HotMelt         = 0x0040;
        public const UInt16 Cmd_MarkA           = 0x0080;

        public const UInt16 Cmd_HotMelt_MarkA   = 0x0100;
        public const UInt16 Cmd_MarkB           = 0x0200;
        public const UInt16 Cmd_HotMelt_MarkB   = 0x0400;
        public const UInt16 Cmd_HotMelt_MarkAB  = 0x0800;
        

        public const UInt16 Cmd_BredeReset      = 0x4000;
        public const UInt16 Cmd_ClearAlarm      = 0x8000;
        #endregion

        #region----------------声明状态及报警常量-------------------
        public const ushort Alm_Material_Lack        = 0x0001;
        public const ushort Alm_Material_MalPosition = 0x0002;
        /// <summary>
        /// 走带故障
        /// </summary>
        public const ushort Alm_ReceiveBrede         = 0x0004;
        public const ushort Alm_Membrane_Lack        = 0x0008;

        public const ushort Status_Initialized       = 0x0010;
        public const ushort Status_Tmp_OK            = 0x0020;
        public const ushort Status_Tmp_HeatingUp     = 0x0040;
        public const ushort Status_Tmp_Close         = 0x0080;

        public const ushort Status_Busy              = 0x0100;
        /// <summary>
        /// 缺载带故障
        /// </summary>
        public const ushort Alm_Brede_Lack           = 0x0200;
        public const ushort Status_MarkAHome         = 0x0400;
        public const ushort Status_MarkBHome         = 0x0800;
        #endregion

        #region----------------声明时间常量-------------------
        private const ushort Timer_ReadStateVal = 300;
        private const ushort Timer_CheckBusyVal = 300;
        #endregion

        #region----------------声明步序变量-------------------
        private static int BM_P_Step = 0;
        private static int BA_P_Step = 0;
        private static int BE_P_Step = 0;
        private static int Back_Step = 0;
        #endregion

        #region----------------声明计数变量-------------------
        /// <summary>
        /// 编带前空料计数值
        /// </summary>
        public static short CntVal_FrontEmptyMaterial;   //编带前空料计数值
        /// <summary>
        /// CCD前空料计数值
        /// </summary>
        public static short CntVal_CCDEmptyMaterial;   //CCD前空料计数值
        /// <summary>
        /// 编带打点A空料计数值
        /// </summary>
        public static short CntVal_MarkAEmptyMaterial;   //编带打点A空料计数值
        /// <summary>
        /// 编带打点A空料计数值
        /// </summary>
        public static short CntVal_MarkBEmptyMaterial;   //编带打点B空料计数值
        /// <summary>
        /// 手动编带计数值
        /// </summary>
        public static short CntVal_Manual;               //手动编带计数值
        /// <summary>
        /// 自动编带计数值
        /// </summary>
        private static short CntVal_Auto;                 //自动编带计数值
        /// <summary>
        /// 结束编带计数值
        /// </summary>
        private static short CntVal_End;                  //结束编带计数值
        /// <summary>
        /// 热熔间隔数值
        /// </summary>
        private static short CntVal_HotMelt;              //热熔间隔数值
        #endregion

        private Brede()
        {
            SerialCtrl = SerialConector.GetInstance("BraidSerialCtl");
            if (SerialCtrl.CreateSerialPort() && !SerialCtrl.IsOpen)
            {
                SerialCtrl.OpenConnection(g_config.ArrStrutCom[1].ICom, g_config.ArrStrutCom[1].IBaud, g_config.ArrStrutCom[1].IDataBits, g_config.ArrStrutCom[1].IParity, g_config.ArrStrutCom[1].IStopBits, "Braid Ports");
            }
            SerialCtrl.AutoResetEventSet();
        }
        public static Brede GetInstance()
        {
            if (_instance == null)
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new Brede();
                    }
                }
            }
            return _instance;
        }

        public static void MODBUS_WriteBredeParameter()
        {
            int CRCtemp;
            byte[] singleF = new byte[4];
            byte[] TxBuffer = new byte[37];

            TxBuffer[0] = 0x03;
            TxBuffer[1] = 0x10;
            TxBuffer[2] = 0x00;
            TxBuffer[3] = 0x00;
            TxBuffer[4] = 0x00;
            TxBuffer[5] = 0x0E;
            TxBuffer[6] = 0x1C;


            TxBuffer[7] = (byte)((setConfig.tEmptyBredeFrap & 0xFF00) >> 8);
            TxBuffer[8] = (byte)(setConfig.tEmptyBredeFrap & 0x00FF);

            TxBuffer[9] = (byte)((setConfig.tEmptyBredeSend & 0xFF00) >> 8);
            TxBuffer[10] = (byte)(setConfig.tEmptyBredeSend & 0x00FF);

            TxBuffer[11] = (byte)((setConfig.tBraidFrap & 0xFF00) >> 8);
            TxBuffer[12] = (byte)(setConfig.tBraidFrap & 0x00FF);

            TxBuffer[13] = (byte)((setConfig.tHotMelt & 0xFF00) >> 8);
            TxBuffer[14] = (byte)(setConfig.tHotMelt & 0x00FF);

            TxBuffer[15] = (byte)((setConfig.tUV_Lamp & 0xFF00) >> 8);
            TxBuffer[16] = (byte)(setConfig.tUV_Lamp & 0x00FF);

            TxBuffer[17] = (byte)((setConfig.tDot & 0xFF00) >> 8);
            TxBuffer[18] = (byte)(setConfig.tDot & 0x00FF);

            TxBuffer[19] = (byte)((setConfig.reserved_1 & 0xFF00) >> 8);
            TxBuffer[20] = (byte)(setConfig.reserved_1 & 0x00FF);

            TxBuffer[21] = (byte)((setConfig.reserved_2 & 0xFF00) >> 8);
            TxBuffer[22] = (byte)(setConfig.reserved_2 & 0x00FF);

            TxBuffer[23] = (byte)((setConfig.acc & 0xFF00) >> 8);
            TxBuffer[24] = (byte)(setConfig.acc & 0x00FF);

            TxBuffer[25] = (byte)((setConfig.dec & 0xFF00) >> 8);
            TxBuffer[26] = (byte)(setConfig.dec & 0x00FF);

            singleF = BitConverter.GetBytes(setConfig.space);
            TxBuffer[27] = singleF[1];
            TxBuffer[28] = singleF[0];
            TxBuffer[29] = singleF[3];
            TxBuffer[30] = singleF[2];

            singleF = BitConverter.GetBytes(setConfig.speed);
            TxBuffer[31] = singleF[1];
            TxBuffer[32] = singleF[0];
            TxBuffer[33] = singleF[3];
            TxBuffer[34] = singleF[2];

            CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
            TxBuffer[35] = (byte)(CRCtemp & 0x00FF);
            TxBuffer[36] = (byte)((CRCtemp & 0xFF00) >> 8);
            proxy.SendCmd("WriteBredeParameter", TxBuffer, 8);
        }

        public static void MODBUS_ReadBredeParameter()
        {
            int CRCtemp;
            byte[] TxBuffer = new byte[8];

            TxBuffer[0] = 0x03;
            TxBuffer[1] = 0x03;
            TxBuffer[2] = 0x00;
            TxBuffer[3] = 0x00;
            TxBuffer[4] = 0x00;
            TxBuffer[5] = 0x0E;
            CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
            TxBuffer[6] = (byte)(CRCtemp & 0x00FF);
            TxBuffer[7] = (byte)((CRCtemp & 0xFF00) >> 8);
            proxy.SendCmd("ReadBredeParameter", TxBuffer, 33, true, 100);
        }

        public void MODBUS_ReadBredeStateAlarm()
        {
            int CRCtemp;
            byte[] TxBuffer = new byte[8];

            TxBuffer[0] = 0x03;
            TxBuffer[1] = 0x03;
            TxBuffer[2] = 0x00;
            TxBuffer[3] = 0x5B;
            TxBuffer[4] = 0x00;
            TxBuffer[5] = 0x01;
            CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
            TxBuffer[6] = (byte)(CRCtemp & 0x00FF);
            TxBuffer[7] = (byte)((CRCtemp & 0xFF00) >> 8);
            proxy.SendCmd("ReadBredeStateAlarm", TxBuffer, 7, false);
        }

        private static void MODBUS_BredeCmdSend(ushort CmdWord)
        {
            int CRCtemp;
            byte[] TxBuffer = new byte[11];

            TxBuffer[0] = 0x03;
            TxBuffer[1] = 0x10;
            TxBuffer[2] = 0x00;
            TxBuffer[3] = 0x5A;
            TxBuffer[4] = 0x00;
            TxBuffer[5] = 0x01;
            TxBuffer[6] = 0x02;
            TxBuffer[7] = (byte)((CmdWord & 0xFF00) >> 8);
            TxBuffer[8] = (byte)(CmdWord & 0x00FF);
            CRCtemp = Modbus.Crc16(TxBuffer, 9);
            TxBuffer[9] = (byte)(CRCtemp & 0x00FF);
            TxBuffer[10] = (byte)((CRCtemp & 0xFF00) >> 8);
            proxy.SendCmd("BredeCmdSend", TxBuffer, 8);
        }

        public static void Init()
        {
            BM_P_Step = 0;
            BA_P_Step = 0;
            BE_P_Step = 0;

            Brede_Trigger.End_Brede = false;
            Brede_Trigger.ALarmCheck = false;
            Brede_Trigger.Manual_Brede = false;
            Brede_Trigger.Auto_Brede = false;

            Brede_Flag.Manual_Brede = false;
            Brede_Flag.Auto_Brede = false;
            Brede_Flag.End_Brede = false;
            Brede_Flag.Move = false;
            if (Auto_Flag.Brede_LayIC)
            {
                Send_Cmd(Cmd_ClearAlarm); //清除报警命令
            }
            Brede_Number.CntVal_Auto = 0;//编带单次放料计数值
            CntVal_HotMelt = 0;
            CntVal_Manual = CntVal_HotMelt;
            CntVal_Auto = 0;
            CntVal_End = 0;
        }

        public void Handle()
        {
            if (Auto_Flag.Brede_LayIC)
            {
                if (UserTimer.GetSysTime() >= Brede_Timer.ReadState_Cnt)
                {
                    Brede_Timer.ReadState_Cnt = UserTimer.GetSysTime() + Timer_ReadStateVal;
                    if (SerialCtrl.IsOpen)
                    {
                        MODBUS_ReadBredeStateAlarm();//编带状态警报读取
                    }    
                }
                Status_Check();
                Manual_Program(); //手动编带
                Auto_Program();   //自动编带
                End_Program();    //结束编带
            }
        }

        public static void Send_Cmd(UInt16 Cmd)
        {
            Brede_Timer.CheckDot_Cnt = UserTimer.GetSysTime() + 1000;
            Brede_Timer.ReadState_Cnt = UserTimer.GetSysTime() + Timer_ReadStateVal;
            Brede_Timer.CheckBusy_Cnt = UserTimer.GetSysTime() + Timer_CheckBusyVal;
            MODBUS_BredeCmdSend(Cmd);
        }

        public static void Alarm_Check(bool IsDot = false)
        {
            if (UserConfig.IsProgrammer)
            {
                return;
            }
            Auto_Flag.BredeALarm = false;
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "编带报警查询", "Flow");
            if ((StatusAlarmWord & Alm_Brede_Lack) != 0 && !Auto_Flag.ManualEnd && !Brede_Flag.End_Brede && !Brede_Flag.Manual_Brede && Brede_Flag.Move)//空载带检测
            {
                Auto_Flag.BredeALarm = true;
                BAR._ToolTipDlg.WriteToolTipStr("编带缺载带故障;[处理异常->确定->继续]");
            }

            if ((StatusAlarmWord & Alm_Material_Lack) != 0 && CntVal_FrontEmptyMaterial == 0 && Auto_Flag.Empty_Check)//空料检测
            {
                Auto_Flag.BredeALarm = true;
                BAR._ToolTipDlg.WriteToolTipStr("编带空料故障;[处理异常->确定->继续]");
            }

            if ((StatusAlarmWord & Alm_Material_MalPosition) != 0)//跳料检测
            {
                Auto_Flag.BredeALarm = true;
                BAR._ToolTipDlg.WriteToolTipStr("编带跳料故障;[处理异常->确定->继续]");
            }

            if ((StatusAlarmWord & Alm_ReceiveBrede) != 0 && Auto_Flag.Brede_Check && !Brede_Flag.End_Brede && Brede_Flag.Move)//走料检测 && 检测使能 && 编带收尾 && 移动过
            {
                Auto_Flag.BredeALarm = true;
                BAR._ToolTipDlg.WriteToolTipStr("编带走带故障;[处理异常->确定->继续]");
            }

            if ((StatusAlarmWord & Alm_Membrane_Lack) != 0 && Auto_Flag.Velum_Check)//缺膜检测 && 检测使能
            {
                Auto_Flag.BredeALarm = true;
                BAR._ToolTipDlg.WriteToolTipStr("编带缺膜故障;[处理异常->确定->继续]");
            }

            if ((StatusAlarmWord & Status_Tmp_OK) == 0)//温控异常检测
            {
                Auto_Flag.BredeALarm = true;
                BAR._ToolTipDlg.WriteToolTipStr("编带温控异常;[处理异常->确定->继续]");
            }

            if (IsDot && Auto_Flag.DotA && CntVal_MarkAEmptyMaterial == 0 && (StatusAlarmWord & Status_MarkAHome) == 0 && BA_P_Step < 6)//打点A原位异常检测
            {
                Auto_Flag.BredeALarm = true;
                BAR._ToolTipDlg.WriteToolTipStr("打点A原位异常;[处理异常->确定->继续]");
            }

            if (IsDot && Auto_Flag.DotB && CntVal_MarkBEmptyMaterial == 0 && (StatusAlarmWord & Status_MarkBHome) == 0 && BA_P_Step < 6)//打点B原位异常检测
            {
                Auto_Flag.BredeALarm = true;
                BAR._ToolTipDlg.WriteToolTipStr("打点B原位异常;[处理异常->确定->继续]");
            }

            Brede_Flag.Move = false;
            if (Auto_Flag.BredeALarm)
            {
                BAR.ShowToolTipWnd(true);
            }                
        }

        public static void CCDAlarm_Check()
        {
            if (UserConfig.IsProgrammer)
            {
                return;
            }
            Auto_Flag.BredeALarm = false;
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "编带CCD报警查询", "Flow");
            
            if (Auto_Flag.BredeCCD_Check && CntVal_CCDEmptyMaterial == 0)//CCD异常检测
            {
                Auto_Flag.BredeALarm = true;
                BAR._ToolTipDlg.WriteToolTipStr("编带影像检测异常;[处理异常->确定->继续]");
            }
            if (Auto_Flag.BredeALarm)
            {
                BAR.ShowToolTipWnd(true);
            }
        }

        private void Status_Check()
        {
            Brede_Status.Initialized = (StatusAlarmWord & Status_Initialized) != 0;
            Brede_Status.Busy = (StatusAlarmWord & Status_Busy) != 0;
            Brede_Status.Tmp_Close = (StatusAlarmWord & Status_Tmp_Close) != 0;
            Brede_Status.Tmp_HeatingUp = (StatusAlarmWord & Status_Tmp_HeatingUp) != 0;
            Brede_Status.Tmp_OK = (StatusAlarmWord & Status_Tmp_OK) != 0;
        }

        /// <summary>
        /// CCD触发
        /// </summary>
        public static void StartCCD()
        {
            if (Auto_Flag.BredeCCD_Check)//CCD
            {
                AutoTimer.BredeCCDOffDelay = UserTimer.GetSysTime() + 500;
                In_Output.BredeStart_CCD.M = true;
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "编带CCD触发", "Flow");
            }
        }

        /// <summary>
        /// 光源
        /// </summary>
        /// <param name="flag"></param>
        public void LigthCCD(string flag)
        {
            if (Auto_Flag.BredeCCD_Check)//CCD
            {
                In_Output.BredeLight_CCD.M = flag == "off" ? true : false;
            }
        }

        private void Manual_Program()
        {
            switch (BM_P_Step)
            {
                case 0:
                    if (Brede_Trigger.Manual_Brede)
                    {
                        Brede_Trigger.Manual_Brede = false;
                        CntVal_Manual = (short)Brede_Number.CntVal_Manual;//手动编带个数设置值D04997		
                        if (CntVal_Manual > 0)
                        {
                            Brede_Flag.Manual_Brede = true;
                            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "手动编带开始,编带个数[" + CntVal_Manual.ToString() + "]", "Flow");
                            BM_P_Step = 1;
                        }
                    }
                    break;

                case 1:
                    Send_Cmd(Cmd_ReceiveMaterial);
                    g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送编带走带命令", "Flow");
                    BM_P_Step = 2;
                    break;

                case 2:
                    if (UserTimer.GetSysTime() >= Brede_Timer.CheckBusy_Cnt)
                    {
                        if (!Brede_Status.Busy)
                        {
                            BM_P_Step = 3;
                        }
                    }
                    break;

                case 3:
                    if (Auto_Flag.DotA || Auto_Flag.DotB)//打点使能
                    {
                        if (Auto_Flag.DotA && Auto_Flag.DotB)
                        {
                            Send_Cmd(Cmd_HotMelt_MarkAB);//热熔打点AB
                        }
                        else if (Auto_Flag.DotA)
                        {
                            Send_Cmd(Cmd_HotMelt_MarkA);//热熔打点A
                        }
                        else if (Auto_Flag.DotB)
                        {
                            Send_Cmd(Cmd_HotMelt_MarkB);//热熔打点B
                        }
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送编带热熔打点命令", "Flow");
                    }
                    else
                    {
                        Send_Cmd(Cmd_HotMelt);//热熔
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送编带热熔命令", "Flow");
                    }
                    BM_P_Step = 4;
                    break;

                case 4:
                    if (UserTimer.GetSysTime() >= Brede_Timer.CheckBusy_Cnt)
                    {
                        if (!Brede_Status.Busy)
                        {
                            BM_P_Step = 5;
                        }
                    }
                    break;

                case 5:
                    Brede_Timer.CheckBusy_Cnt = UserTimer.GetSysTime() + Timer_CheckBusyVal;
                    BM_P_Step = 6;
                    break;

                case 6:
                    if (UserTimer.GetSysTime() >= Brede_Timer.CheckBusy_Cnt)
                    {
                        CntVal_Manual--;
                        if (CntVal_Manual <= 0)
                        {
                            Brede_Flag.Manual_Brede = false;
                            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "手动编带结束", "Flow");
                        }
                        if (Brede_Flag.Manual_Brede)
                        {
                            BM_P_Step = 1;
                        }
                        else
                        {
                            BM_P_Step = 0;
                        }
                    }
                    break;

                default:
                    break;
            }
        }
        private int tempTimes;
        private void Auto_Program()
        {
            switch (BA_P_Step)
            {
                case 0:
                    if (Brede_Trigger.Auto_Brede)
                    {
                        Brede_Trigger.Auto_Brede = false;
                        CntVal_Auto = (short)Brede_Number.CntVal_Auto;
                        Brede_Number.CntVal_Auto = 0;
                        tempTimes = 0;
                        if (CntVal_Auto > 0)
                        {
                            //Brede_Flag.Move = true;
                            Brede_Flag.Auto_Brede = true;  //置编带自动过程标志
                            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "自动编带开始,编带个数[" + CntVal_Auto.ToString() + "]", "Flow");
                            BA_P_Step = 1;
                        }
                    }
                    break;

                case 1:
                    if (!Brede_Status.Busy)
                    {
                        Alarm_Check(true);//编带警报查询
                        if (Auto_Flag.ALarm)
                        {
                            Back_Step = 1;
                            BA_P_Step = 40;
                        }
                        else
                        {
                            Brede_Flag.Move = true;
                            Send_Cmd(Cmd_ReceiveMaterial);
                            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送编带走带命令", "Flow");
                            if (Auto_Flag.BredeCCD_Check)//CCD
                            {
                                Auto_Flag.JumpBredeStep_Flag = false;
                                LigthCCD("on");//打开光源
                            }
                            BA_P_Step = 2;
                        }
                    }
                    break;

                case 2:
                    if (UserTimer.GetSysTime() >= Brede_Timer.CheckBusy_Cnt)
                    {
                        if (!Brede_Status.Busy)
                        {
                            if (CntVal_FrontEmptyMaterial > 0)//编带前空料计数值
                            {
                                CntVal_FrontEmptyMaterial--;
                            }
                            else
                            {
                                CntVal_FrontEmptyMaterial = 0;
                            }
                            tempTimes++;

                            BA_P_Step = 3;
                        }
                    }
                    break;

                case 3:
                    if (UserTimer.GetSysTime() >= Brede_Timer.CheckBusy_Cnt)
                    {
                        if (!Brede_Status.Busy)
                        {
                            Alarm_Check(true);//编带警报查询
                            if (Auto_Flag.ALarm)
                            {
                                Back_Step = 3;
                                BA_P_Step = 40;
                            }
                            else
                            {
                                BA_P_Step = 4;
                            }
                        }
                    }
                    break;

                case 4:
                    if (tempTimes >= Brede_Number.CntVal_HotMelt || CntVal_Auto == 1)
                    {
                        tempTimes = 0;
                        if ((Auto_Flag.DotA && CntVal_MarkAEmptyMaterial == 0) || (Auto_Flag.DotB && CntVal_MarkBEmptyMaterial == 0))//打点使能 
                        {
                            if ((Auto_Flag.DotA && CntVal_MarkAEmptyMaterial == 0) && (Auto_Flag.DotB && CntVal_MarkBEmptyMaterial == 0))
                            {
                                Send_Cmd(Cmd_HotMelt_MarkAB);//热熔打点AB
                            }
                            else if (Auto_Flag.DotA && CntVal_MarkAEmptyMaterial == 0)
                            {
                                Send_Cmd(Cmd_HotMelt_MarkA);//热熔打点A
                            }
                            else if (Auto_Flag.DotB && CntVal_MarkBEmptyMaterial == 0)
                            {
                                Send_Cmd(Cmd_HotMelt_MarkB);//热熔打点B
                            }
                            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送编带热熔打点命令", "Flow");
                        }
                        else
                        {
                            Send_Cmd(Cmd_HotMelt);//热熔
                            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送编带热熔命令", "Flow");
                        }
                    }
                    else
                    {
                        if (Auto_Flag.DotA && CntVal_MarkAEmptyMaterial == 0)
                        {
                            Send_Cmd(Cmd_MarkA);//打点A
                            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送编带打点A命令", "Flow");
                        }
                        if (Auto_Flag.DotB && CntVal_MarkBEmptyMaterial == 0)
                        {
                            Send_Cmd(Cmd_MarkB);//打点B
                            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送编带打点B命令", "Flow");
                        }
                    }

                    BA_P_Step = 5;
                    break;
                    
                case 5:
                    if (UserTimer.GetSysTime() >= Brede_Timer.CheckBusy_Cnt)
                    {
                        if (!Brede_Status.Busy)
                        {
                            if (CntVal_MarkAEmptyMaterial > 0)//编带打点A空料计数值
                            {
                                CntVal_MarkAEmptyMaterial--;
                            }
                            else
                            {
                                CntVal_MarkAEmptyMaterial = 0;
                            }

                            if (CntVal_MarkBEmptyMaterial > 0)//编带打点B空料计数值
                            {
                                CntVal_MarkBEmptyMaterial--;
                            }
                            else
                            {
                                CntVal_MarkBEmptyMaterial = 0;
                            }
                            BA_P_Step = 7;
                            //Brede_Timer.CheckBusy_Cnt = UserTimer.GetSysTime() + Timer_CheckBusyVal;
                        }
                    }
                    break;
                case 7:
                    if (UserTimer.GetSysTime() >= Brede_Timer.CheckBusy_Cnt)
                    {
                        if (Auto_Flag.BredeCCD_Check && !Auto_Flag.JumpBredeStep_Flag && CntVal_CCDEmptyMaterial == 0)
                        {
                            StartCCD();
                            Brede_Timer.CheckCCD_Cnt = UserTimer.GetSysTime() + 10000;
                            BA_P_Step = 8;
                        }
                        else
                        {
                            BA_P_Step = 9;
                        }
                    }
                    break;

                case 8:
                    if (In_Output.BredeNG_CCD.M || UserTimer.GetSysTime() >= Brede_Timer.CheckCCD_Cnt)
                    {
                        CCDAlarm_Check();
                        if (Auto_Flag.ALarm)
                        {
                            LigthCCD("off");//关闭光源
                            Back_Step = 7;
                            BA_P_Step = 40;
                        }
                        else
                        {
                            Back_Step = 9;
                        }
                    }
                    else if (In_Output.BredeOK_CCD.M)
                    {
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "编带影像检测成功", "Flow");
                        BA_P_Step = 9;
                    }
                    break;

                case 9:
                    if (CntVal_CCDEmptyMaterial > 0)//CCD前空料计数值
                    {
                        CntVal_CCDEmptyMaterial--;
                    }
                    else
                    {
                        CntVal_CCDEmptyMaterial = 0;
                    }
                    BA_P_Step = 10;
                    break;

                case 10://检查打点是否在原位
                    if (Auto_Flag.DotA || Auto_Flag.DotB)
                    {
                        
                        if (Auto_Flag.DotA && Auto_Flag.DotB)
                        {
                            if ((StatusAlarmWord & Status_MarkAHome) != 0 && (StatusAlarmWord & Status_MarkBHome) != 0)
                            {
                                BA_P_Step = 11;
                            }
                        }
                        else if (Auto_Flag.DotA)
                        {
                            if ((StatusAlarmWord & Status_MarkAHome) != 0)
                            {
                                BA_P_Step = 11;
                            }
                        }
                        else if (Auto_Flag.DotB)
                        {
                            if ((StatusAlarmWord & Status_MarkBHome) != 0)
                            {
                                BA_P_Step = 11;
                            }
                        }

                        if (UserTimer.GetSysTime() >= Brede_Timer.CheckDot_Cnt)
                        {
                            BA_P_Step = 11;
                            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "打点回原位超时", "Flow");
                        }
                    }
                    else
                    {
                        BA_P_Step = 11;
                    }
                    break;

                case 11:
                    if (UserTimer.GetSysTime() >= Brede_Timer.CheckBusy_Cnt)
                    {
                        CntVal_Auto--;
                        if (CntVal_Auto <= 0)
                        {
                            Brede_Flag.Auto_Brede = false;  //清除编带自动过程标志
                            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "自动编带结束", "Flow");
                        }        
                        if (Brede_Flag.Auto_Brede)
                        {
                            BA_P_Step = 1;
                        }
                        else
                        {
                            BA_P_Step = 0;
                        }
                    }
                    break;

                case 40:
                    if (UserTask.Continue_AfterAlarm())
                    {
                        if (Auto_Flag.BredeCCD_Check && Back_Step == 7)
                        {
                            LigthCCD("on");//打开光源
                            Brede_Timer.CheckBusy_Cnt = UserTimer.GetSysTime() + Timer_CheckBusyVal;
                        }
                        BA_P_Step = Back_Step;
                    }
                    break;
                default:
                    break;
            }
        }

        private void End_Program()
        {
            switch (BE_P_Step)
            {
                case 0:
                    if (Brede_Trigger.End_Brede)
                    {
                        Brede_Trigger.End_Brede = false;
                        CntVal_End = (short)Brede_Number.CntVal_End;//编带后空料设置值Brede_Number.CntVal_End		        	
                        if (CntVal_End > 0)
                        {  
                            Brede_Flag.End_Brede = true;  //编带收尾过程标志
                            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "收尾编带开始,编带个数[" + CntVal_End.ToString() + "]", "Flow");
                            BE_P_Step = 1;
                        }
                    }
                    break;

                case 1:
                    if (!Brede_Status.Busy)
                    {
                        Send_Cmd(Cmd_ClearAlarm); //清除报警命令
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送编带清除报警命令", "Flow");
                        BE_P_Step = 2;
                    }
                    break;

                case 2:
                    if (UserTimer.GetSysTime() >= Brede_Timer.CheckBusy_Cnt)
                    {
                        if (!Brede_Status.Busy)
                        {
                            BE_P_Step = 3;
                        }
                    }
                    break;

                case 3:
                    Send_Cmd(Cmd_ReceiveMaterial);
                    g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送编带走带命令", "Flow");
                    BE_P_Step = 4;
                    break;

                case 4:
                    if (UserTimer.GetSysTime() >= Brede_Timer.CheckBusy_Cnt)
                    {
                        if (!Brede_Status.Busy)
                        {
                            if (CntVal_FrontEmptyMaterial > 0)//编带前空料计数值
                            {
                               CntVal_FrontEmptyMaterial--;
                            }
                            else
                            {
                               CntVal_FrontEmptyMaterial = 0;
                            }
                            BE_P_Step = 5;
                        }
                    }
                    break;

                case 5:
                    if ((Auto_Flag.DotA && CntVal_MarkAEmptyMaterial == 0) || (Auto_Flag.DotB && CntVal_MarkBEmptyMaterial == 0))//打点使能 
                    {
                        if ((Auto_Flag.DotA && CntVal_MarkAEmptyMaterial == 0) && (Auto_Flag.DotB && CntVal_MarkBEmptyMaterial == 0))
                        {
                            Send_Cmd(Cmd_HotMelt_MarkAB);//热熔打点AB
                        }
                        else if (Auto_Flag.DotA && CntVal_MarkAEmptyMaterial == 0)
                        {
                            Send_Cmd(Cmd_HotMelt_MarkA);//热熔打点A
                        }
                        else if (Auto_Flag.DotB && CntVal_MarkBEmptyMaterial == 0)
                        {
                            Send_Cmd(Cmd_HotMelt_MarkB);//热熔打点B
                        }
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送编带热熔打点命令", "Flow");
                    }
                    else
                    {
                        Send_Cmd(Cmd_HotMelt);//热熔
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送编带热熔命令", "Flow");
                    }
                    BE_P_Step = 6;

                    break;

                case 6:
                    if (UserTimer.GetSysTime() >= Brede_Timer.CheckBusy_Cnt)
                    {
                        if (!Brede_Status.Busy)
                        {
                            if (CntVal_MarkAEmptyMaterial > 0)//编带打点空料计数值
                            {
                                CntVal_MarkAEmptyMaterial--;
                            }
                            else
                            {
                                CntVal_MarkAEmptyMaterial = 0;
                            }

                            if (CntVal_MarkBEmptyMaterial > 0)//编带打点B空料计数值
                            {
                                CntVal_MarkBEmptyMaterial--;
                            }
                            else
                            {
                                CntVal_MarkBEmptyMaterial = 0;
                            }
                            BE_P_Step = 7;
                        }
                    }
                    break;

                case 7:
                    Alarm_Check();//编带警报查询
                    if (Auto_Flag.ALarm)
                    {
                        BE_P_Step = 8;
                    }
                    else
                    {
                        BE_P_Step = 9;
                    }
                    break;

                case 8:
                    if (UserTask.Continue_AfterAlarm())
                    {
                        BE_P_Step = 7;
                    }
                    break;

                case 9:
                    if (UserTask.NextAction_Check())
                    {
                        BE_P_Step = 10;
                    }
                    break;

                case 10:
                    CntVal_End--;
                    /* 编带收尾时，前空数这段之后不检测空料 */
                    if ((Brede_Number.CntVal_End - CntVal_End + 1) >= Brede_Number.CntVal_FrontEmptyMaterial)
                    {
                        CntVal_FrontEmptyMaterial = (short)Brede_Number.CntVal_FrontEmptyMaterial;//编带前空料设置值DK04994			
                    }
                    else
                    {
                        CntVal_FrontEmptyMaterial = 0;
                    }

                    if ((Brede_Number.CntVal_End - CntVal_End) >= Brede_Number.CntVal_MarkAEmptyMaterial)
                    {
                        CntVal_MarkAEmptyMaterial = (short)Brede_Number.CntVal_MarkAEmptyMaterial;//编带打点A空料设置值DK04995			
                    }
                    else
                    {
                        CntVal_MarkAEmptyMaterial = 0;
                    }

                    if ((Brede_Number.CntVal_End - CntVal_End) >= Brede_Number.CntVal_MarkBEmptyMaterial)
                    {
                        CntVal_MarkBEmptyMaterial = (short)Brede_Number.CntVal_MarkBEmptyMaterial;//编带打点B空料设置值DK04995			
                    }
                    else
                    {
                        CntVal_MarkBEmptyMaterial = 0;
                    }
                    BE_P_Step = 11;
                    break;

                case 11:
                    Brede_Timer.CheckBusy_Cnt = UserTimer.GetSysTime() + Timer_CheckBusyVal;
                    BE_P_Step = 12;
                    break;

                case 12:
                    if (UserTimer.GetSysTime() >= Brede_Timer.CheckBusy_Cnt)
                    {
                        if (CntVal_End <= 0)
                        {
                            Brede_Flag.End_Brede = false;  //编带收尾过程标志
                            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "收尾编带结束", "Flow");
                        }
                        if(Brede_Flag.End_Brede)
                        {
                            BE_P_Step = 1;
                        }
                        else
                        {
                            BE_P_Step = 0;
                        }
                    }
                    break;

                default:
                    break;
            }
        }     
    }

    public class Brede_Trigger
    {
        public static bool ALarmCheck;        //编带查询标志
        public static bool Auto_Brede;        //编带自动过程中标志	
        public static bool Manual_Brede;      //编带手动过程中标志	
        public static bool End_Brede;         //编带收尾过程中标志	 	
    }

    public class Brede_Flag
    {
        public static bool ALarmCheck; 		  //编带查询标志
        public static bool Move;              //编带移动标志
        public static bool Auto_Brede;        //编带自动过程中标志	
        public static bool Manual_Brede;      //编带手动过程中标志	
        public static bool End_Brede;         //编带收尾过程中标志	 	
    }

    public class Brede_Number
    {
        public static UInt16 CntVal_FrontEmptyMaterial;   //编带前空料数值
        public static UInt16 CntVal_CCDEmptyMaterial;     //CCD前空料数值
        public static UInt16 CntVal_MarkAEmptyMaterial;   //编带打点A空料数值
        public static UInt16 CntVal_MarkBEmptyMaterial;   //编带打点B空料数值
        public static UInt16 CntVal_Manual;               //手动编带数值
        public static UInt16 CntVal_Auto;                 //自动编带数值
        public static UInt16 CntVal_End;                  //结束编带数值
        public static UInt16 CntVal_HotMelt;              //热熔间隔数值
    }

    public class Brede_Config
    {
        public UInt16 tEmptyBredeFrap;	  //空带收时间
        public UInt16 tEmptyBredeSend;	  //空带送时间
        public UInt16 tBraidFrap;	      //料带收时间
        public UInt16 tHotMelt;	          //热熔时间
        public UInt16 tUV_Lamp;	          //UV灯控制时间
        public UInt16 tDot;               //打点时间

        public UInt16 reserved_1;	      //保留1
        public UInt16 reserved_2;         //保留2

        public UInt16 acc;	              //编带加速时间		
        public UInt16 dec;	              //编带减速时间
        public float space;	              //编带间距
        public float speed;	              //编带速度
    }

    public class Brede_Timer
    {
        public static UInt64 ReadState_Cnt;  //编带读状态间隔计数
        public static UInt64 CheckBusy_Cnt;  //编带延迟检测BUSY计数
        public static UInt64 CheckCCD_Cnt;  //CCD延迟检测BUSY计数
        /// <summary>
        /// 编带打点读状态间隔计数
        /// </summary>
        public static UInt64 CheckDot_Cnt;
    }

    public class Brede_Status
    {
        public static bool Initialized;         //系统初始化状态（主机用来确认配置参数已发送成功）    
        public static bool Tmp_OK;              //热熔温度OK
        public static bool Tmp_HeatingUp;       //热熔温度升温
        public static bool Tmp_Close;           //热熔温度关闭
        public static bool Busy;                //编带系统忙状态，为1说明正在执行命令，为0表明命令执行完毕或系统空闲
        /// <summary>
        /// 热熔温度OK
        /// </summary>
        public static string strTmp_OK;              //热熔温度OK
        /// <summary>
        /// 热熔温度升温
        /// </summary>
        public static string strTmp_HeatingUp;       //热熔温度升温
        /// <summary>
        /// 热熔温度关闭
        /// </summary>
        public static string strTmp_Close;           //热熔温度关闭
    }
}
