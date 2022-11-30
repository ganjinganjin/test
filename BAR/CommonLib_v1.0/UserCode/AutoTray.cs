using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BAR.Commonlib;
using BAR.Commonlib.Connectors;
using BAR.Commonlib.Connectors.Protocols;
using BAR.Communicators;

namespace BAR
{
    public class AutoTray
    {
        public static UInt16 StatusAlarmWord = 0;
        public static bool SystemInitDone;    //初始化完成标志
        public static float[] setConfig = new float[8];
        public static float[] getConfig = new float[8];
        /// <summary>
        /// 原位
        /// </summary>
        public static string strHomeDone;
        /// <summary>
        /// 非原位
        /// </summary>
        public static string strHomeUnDone;

        public Config g_config = Config.GetInstance();
        public Act g_act = Act.GetInstance();
        private static AutoTray _instance = null;
        private static readonly object padlock = new object();
        private SerialConector SerialCtrl;
        private static AutoTrayProxy proxy = AutoTrayProxy.GetInstance();

        #region----------------声明命令常量-------------------
        public const UInt16 Cmd_SystemInit     = 0x0001;
        public const UInt16 Cmd_TakeTray       = 0x0002;
        public const UInt16 Cmd_ReceiveTray    = 0x0004;
        public const UInt16 Cmd_EndReceiveTray = 0x000C;

        public const UInt16 Cmd_Suspend        = 0x0010;
        public const UInt16 Cmd_Continue       = 0x0020;
        public const UInt16 Cmd_EmergencyStop  = 0x0040;
        public const UInt16 Cmd_AutoRevise     = 0x0080;

        
        public const UInt16 Cmd_ResetInk       = 0x0200;

        public const UInt16 Cmd_ClearAlarm     = 0x8000;
        #endregion

        #region----------------声明状态及报警常量-------------------
        public const UInt16 Alarm_SystemAbnormity   = 0x0001;
        public const UInt16 Alarm_TrayFull          = 0x0002;
        public const UInt16 Alarm_TrayLack          = 0x0004;
        public const UInt16 Alm_InkLack             = 0x0008;
        /// <summary>
        /// 料盘丢失
        /// </summary>
        public const UInt16 Alarm_TrayLose          = 0x0010;
        public const UInt16 Status_SystemBusy       = 0x0020;
        public const UInt16 Status_SystemInitDone   = 0x0040;
        public const UInt16 Status_OverplusTrayInit = 0x0080;

        public const UInt16 Status_TakeTrayDone     = 0x0100;
        public const UInt16 Status_TrayLack         = 0x0200;
        public const UInt16 Status_Status_DotBusy   = 0x0400;
        /// <summary>
        /// 料盘对标完成
        /// </summary>
        public const UInt16 Status_AutoReviseDone   = 0x0800;

        public const UInt16 Status_TrayFull         = 0x1000;
        public const UInt16 Status_Reserved_13      = 0x2000;
        public const UInt16 Status_Reserved_14      = 0x4000;
        public const UInt16 Status_SystemRnunning   = 0x8000;
        #endregion

        #region----------------声明时间常量-------------------
        private const UInt16 Timer_ReadStateVal = 100;
        private const UInt16 Timer_CheckBusyVal = 100;
        #endregion

        #region----------------声明步序变量-------------------
        private static int AT_I_Step = 0;
        private static int AT_TT_Step = 0;
        private static int AT_RT_Step = 0;
        private static int Alarm_Step = 0;
        #endregion

        public AutoTray()
        {
            SerialCtrl = SerialConector.GetInstance("AutoTraySerialCtl");
            if (SerialCtrl.CreateSerialPort() && !SerialCtrl.IsOpen)
            {
                SerialCtrl.OpenConnection(g_config.ArrStrutCom[2].ICom, g_config.ArrStrutCom[2].IBaud, g_config.ArrStrutCom[2].IDataBits, g_config.ArrStrutCom[2].IParity, g_config.ArrStrutCom[2].IStopBits, "AutoTray Ports");
            }
            SerialCtrl.AutoResetEventSet();
        }

        public static AutoTray GetInstance()
        {
            if (_instance == null)
            {
                lock (padlock)
                {
                    _instance = new AutoTray();
                }
            }
            return _instance;
        }

        public static void MODBUS_WriteDotParameter()
        {
            int CRCtemp;
            byte[] singleF = new byte[4];
            byte[] TxBuffer = new byte[41];

            TxBuffer[0] = 0x02;
            TxBuffer[1] = 0x10;
            TxBuffer[2] = 0x11;
            TxBuffer[3] = 0xF8;
            TxBuffer[4] = 0x00;
            TxBuffer[5] = 0x10;
            TxBuffer[6] = 0x20;

            for (int i = 0; i < 8; i++)
            {
                singleF = BitConverter.GetBytes(setConfig[i]);
                TxBuffer[4 * i + 7] = singleF[1];
                TxBuffer[4 * i + 8] = singleF[0];
                TxBuffer[4 * i + 9] = singleF[3];
                TxBuffer[4 * i + 10] = singleF[2];
            }

            CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
            TxBuffer[39] = (byte)(CRCtemp & 0x00FF);
            TxBuffer[40] = (byte)((CRCtemp & 0xFF00) >> 8);
            proxy.SendCmd("WriteDotParameter", TxBuffer, 8);
        }

        public static void MODBUS_ReadDotParameter()
        {
            int CRCtemp;
            byte[] TxBuffer = new byte[8];

            TxBuffer[0] = 0x02;
            TxBuffer[1] = 0x03;
            TxBuffer[2] = 0x11;
            TxBuffer[3] = 0xF8;
            TxBuffer[4] = 0x00;
            TxBuffer[5] = 0x10;
            CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
            TxBuffer[6] = (byte)(CRCtemp & 0x00FF);
            TxBuffer[7] = (byte)((CRCtemp & 0xFF00) >> 8);
            proxy.SendCmd("ReadDotParameter", TxBuffer, 37, true, 100);
        }

        public static void AutoTray_Send_Cmd(UInt16 Cmd)
        {
            AutoTray_Timer.ReadState_Cnt = UserTimer.GetSysTime() + Timer_ReadStateVal;
            AutoTray_Timer.CheckBusy_Cnt = UserTimer.GetSysTime() + Timer_CheckBusyVal;
            MODBUS_TrayCmdSend(Cmd);
        }

        private void MODBUS_WriteTrayData(UInt16 Column, UInt16 Row)
        {
            int CRCtemp;
            byte[] TxBuffer = new byte[13];

            TxBuffer[0] = 0x02;
            TxBuffer[1] = 0x10;
            TxBuffer[2] = 0x12;
            TxBuffer[3] = 0xCE;
            TxBuffer[4] = 0x00;
            TxBuffer[5] = 0x02;
            TxBuffer[6] = 0x04;
            TxBuffer[7] = (byte)((Column & 0xFF00) >> 8);
            TxBuffer[8] = (byte)(Column & 0x00FF);
            TxBuffer[9] = (byte)((Row & 0xFF00) >> 8);
            TxBuffer[10] = (byte)(Row & 0x00FF);
            CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
            TxBuffer[11] = (byte)(CRCtemp & 0x00FF);
            TxBuffer[12] = (byte)((CRCtemp & 0xFF00) >> 8);
            proxy.SendCmd("WriteTrayData", TxBuffer, 8);
        }

        private void MODBUS_WriteInkData()
        {
            int CRCtemp;
            byte[] TxBuffer = new byte[13];
            TxBuffer[0] = 0x02;
            TxBuffer[1] = 0x10;
            TxBuffer[2] = 0x10;
            TxBuffer[3] = 0xCC;
            TxBuffer[4] = 0x00;
            TxBuffer[5] = 0x02;
            TxBuffer[6] = 0x04;
            //TxBuffer[7] = (byte)((DK04300 & 0xFF00) >> 8);
            //TxBuffer[8] = (byte)(DK04300 & 0x00FF);
            //TxBuffer[9] = (byte)((DK04301 & 0xFF00) >> 8);
            //TxBuffer[10] = (byte)(DK04301 & 0x00FF);
            CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
            TxBuffer[11] = (byte)(CRCtemp & 0x00FF);
            TxBuffer[12] = (byte)((CRCtemp & 0xFF00) >> 8);
            proxy.SendCmd("WriteInkData", TxBuffer, 8);
        }

        private void MODBUS_ReadInkData()
        {
            int CRCtemp;
            byte[] TxBuffer = new byte[8];

            TxBuffer[0] = 0x02;
            TxBuffer[1] = 0x03;
            TxBuffer[2] = 0x10;
            TxBuffer[3] = 0xCC;
            TxBuffer[4] = 0x00;
            TxBuffer[5] = 0x06;
            CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
            TxBuffer[6] = (byte)(CRCtemp & 0x00FF);
            TxBuffer[7] = (byte)((CRCtemp & 0xFF00) >> 8);
            proxy.SendCmd("ReadInkData", TxBuffer, 17);
        }

        private void MODBUS_ReadTrayStateAlarm()
        {
            int CRCtemp;
            byte[] TxBuffer = new byte[8];

            TxBuffer[0] = 0x02;
            TxBuffer[1] = 0x03;
            TxBuffer[2] = 0x00;
            TxBuffer[3] = 0x5B;
            TxBuffer[4] = 0x00;
            TxBuffer[5] = 0x01;
            CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
            TxBuffer[6] = (byte)(CRCtemp & 0x00FF);
            TxBuffer[7] = (byte)((CRCtemp & 0xFF00) >> 8);
            proxy.SendCmd("ReadTrayStateAlarm", TxBuffer, 7, false);
        }

        private static void MODBUS_TrayCmdSend(UInt16 CmdWord)
        {
            int CRCtemp;
            byte[] TxBuffer = new byte[11];

            TxBuffer[0] = 0x02;
            TxBuffer[1] = 0x10;
            TxBuffer[2] = 0x00;
            TxBuffer[3] = 0x5A;
            TxBuffer[4] = 0x00;
            TxBuffer[5] = 0x01;
            TxBuffer[6] = 0x02;
            TxBuffer[7] = (byte)((CmdWord & 0xFF00) >> 8);
            TxBuffer[8] = (byte)(CmdWord & 0x00FF);
            CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
            TxBuffer[9] = (byte)(CRCtemp & 0x00FF);
            TxBuffer[10] = (byte)((CRCtemp & 0xFF00) >> 8);
            proxy.SendCmd("TrayCmdSend", TxBuffer, 8);
        }

        public static void Init()
        {
            AutoTray_Trigger.ReceiveTray = false;
            AutoTray_Trigger.TakeTray = false;
            AutoTray_Process.ReceiveTray = false;
            AutoTray_Process.ManualReceiveTray = false;
            AutoTray_Process.TakeTray = false;
            AutoTray_Process.ManualTakeTray = false;
            AT_I_Step = 0;
            AT_TT_Step = 0;
            AT_RT_Step = 0;
            Alarm_Step = 0;
        }

        public void Handle()
        { 
            if (Auto_Flag.AutoTray_LayIC || Auto_Flag.AutoTray_TakeIC)
            {
                bool temp = true;
                if ((StatusAlarmWord & Status_SystemRnunning) == 0)
                {
                    if ((StatusAlarmWord & Status_TrayFull) != 0 || (StatusAlarmWord & Status_SystemInitDone) == 0)
                    {
                        temp = false;
                    }
                    if ((StatusAlarmWord & Status_OverplusTrayInit) == 0 && (StatusAlarmWord & Status_TrayLack) != 0)
                    {
                        temp = false;
                    }
                }
                else
                {
                    if ((StatusAlarmWord & Status_TakeTrayDone) == 0 || (StatusAlarmWord & Status_SystemBusy) != 0)
                    {
                        temp = false;
                    }
                }
                Auto_Flag.AutoTrayReady = temp;
               
                if (UserTimer.GetSysTime() >= AutoTray_Timer.ReadState_Cnt)
                {
                    AutoTray_Timer.ReadState_Cnt = UserTimer.GetSysTime() + Timer_ReadStateVal;
                    if (SerialCtrl.IsOpen)
                    {
                        MODBUS_ReadTrayStateAlarm();//自动送盘状态警报读取
                    }                   	           
                }
                Init_Program();//自动托盘初始化程序
                TakeTray_Program();//自动托盘取盘程序
                ReceiveTray_Program();//自动托盘收盘程序		
                Alarm_Program();
            }
        }

        public static void Init_Program_Start()
        {
            if (AT_I_Step == 0 && (Auto_Flag.AutoTray_TakeIC || Auto_Flag.AutoTray_LayIC))
            {
                Button.AutoTray_Init = true;
            }
        }

        public void Init_Program()
        {
            switch (AT_I_Step)
            {
                case 0:
                    if ((StatusAlarmWord & Status_SystemInitDone) != 0)//初始化完成
                    {
                        SystemInitDone = true;
                    }
                    else
                    {
                        SystemInitDone = false;
                    }
                    if (Button.AutoTray_Init)//自动送盘初始化按钮
                    {
                        SystemInitDone = false;
                        Button.AutoTray_Init = false;
                        if ((StatusAlarmWord & Status_SystemRnunning) == 0 && (StatusAlarmWord & Status_SystemBusy) == 0)
                        {
                            AT_I_Step = 1;
                        }                          
                    }
                    break;

                case 1:
                    AutoTray_Send_Cmd(Cmd_SystemInit);//初始化命令		 	
                    AT_I_Step = 2;
                    break;

                case 2:
                    if (UserTimer.GetSysTime() >= AutoTray_Timer.CheckBusy_Cnt)
                    {
                        if ((StatusAlarmWord & Status_SystemBusy) == 0)
                        {
                            AT_I_Step = 3;
                        }
                    }
                    break;

                case 3:
                    if ((StatusAlarmWord & Status_SystemInitDone) != 0)//等待初始化完成
                    {
                        SystemInitDone = true; 	
                    }
                    else
                    {
                        SystemInitDone = false;	
                    }
                    AT_I_Step = 0;
                    break;

                default:
                    break;
            }
        }

        public static void TakeTray_Program_Start()
        {
            AT_TT_Step = 1;
        }

        public void TakeTray_Program()
        {
            switch (AT_TT_Step)
            {
                case 0:
                    if (AutoTray_Trigger.TakeTray)//取盘触发标志
                    {
                        AutoTray_Trigger.TakeTray = false;
                        if ((StatusAlarmWord & Status_SystemRnunning) != 0 || UserConfig.IsProgrammer || AutoTray_Process.ManualTakeTray)//自动送盘运行标志
                        {
                            AT_TT_Step = 1;
                        }
                    }
                    break;

                case 1:
                    AutoTray_Send_Cmd(Cmd_TakeTray);//取盘命令
                    g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送取盘命令", "Flow");
                    AT_TT_Step = 2;
                    break;

                case 2:
                    if (UserTimer.GetSysTime() >= AutoTray_Timer.CheckBusy_Cnt)
                    {
                        if ((StatusAlarmWord & Status_SystemBusy) == 0)
                        {
                            if ((StatusAlarmWord & Status_TakeTrayDone) != 0 || UserConfig.IsProgrammer)//等待取盘完成
                            {
                                if(!AutoTray_Process.TakeTray)
                                {
                                    Efficiency.Algorithm_Start();
                                    Run.ARP_Step = 1;
                                }
                                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "取盘完成", "Flow");
                                if (AutoTray_Process.ManualTakeTray)
                                {
                                    TrayD.TIC_TrayN = 2;   //取料盘号
                                    TrayD.LIC_TrayN = 2;   //放料盘号
                                    if (!Auto_Flag.AutoTray_TakeIC && Auto_Flag.AutoTray_LayIC)
                                    {
                                        TrayD.TIC_ColN[1] = 0;
                                        TrayD.TIC_RowN[1] = 0;
                                    }
                                    else
                                    {
                                        TrayD.TIC_ColN[1] = 1;
                                        TrayD.TIC_RowN[1] = 1;
                                    }
                                    TrayD.LIC_ColN[1] = 1;
                                    TrayD.LIC_RowN[1] = 1;
                                    TrayD.TIC_EndColN[1] = g_config.TrayRotateDir[1] != 0 ? TrayD.RowC : TrayD.ColC;
                                    TrayD.TIC_EndRowN[1] = g_config.TrayRotateDir[1] != 0 ? TrayD.ColC : TrayD.RowC;
                                    TrayState.TrayStateUpdate();
                                    AutoTray_Process.ManualTakeTray = false;
                                }
                                AutoTray_Process.TakeTray = false;
                                AT_TT_Step = 0;
                            }
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        public void ReceiveTray_Program()
        {
            UInt16 Column, Row;
            switch (AT_RT_Step)
            {
                case 0:
                    if (AutoTray_Trigger.ReceiveTray)//收盘触发标志
                    {
                        AutoTray_Trigger.ReceiveTray = false;
                        AT_RT_Step = 1;
                    }
                    break;

                case 1:
                    if ((StatusAlarmWord & Alm_InkLack) != 0)
                    {
                        AT_RT_Step = 2;
                        //M00908 = 1;
                        BAR._ToolTipDlg.WriteToolTipStr("自动盘缺油墨;[更换油墨->确定->继续]");
                        BAR.ShowToolTipWnd(true);
                    }
                    else
                    {
                        if ((StatusAlarmWord & Status_Status_DotBusy) == 0)
                        {
                            if (Config.AutoTrayDot == 1)
                            {
                                MODBUS_WriteDotParameter();
                            }
                            AT_RT_Step = 3;
                        }
                    }
                    break;

                case 2:
                    if (UserTask.Continue_AfterAlarm())
                    {
                        AutoTray_Send_Cmd(Cmd_Continue);//继续命令
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送继续命令", "Flow");
                        Auto_Flag.ALarmPause = false;
                        AT_RT_Step = 1;
                    }
                    break;

                case 3:
                    if (TrayD.LIC_ColN[1] == 0 && TrayD.LIC_RowN[1] == 0)
                    {
                        Column = g_config.TrayRotateDir[1] != 0 ? (UInt16)TrayD.RowC : (UInt16)TrayD.ColC;
                        Row = g_config.TrayRotateDir[1] != 0 ? (UInt16)TrayD.ColC : (UInt16)TrayD.RowC;
                    }
                    else if (TrayD.LIC_ColN[1] == 1 && TrayD.LIC_RowN[1] == 1)
                    {
                        Column = 0;
                        Row = 0;
                    }
                    else
                    {
                        if (g_config.Tray_Col_Add(1))
                        {
                            if (TrayD.TIC_ColN[1] == 1)//第一列
                            {
                                Column = g_config.TrayRotateDir[1] != 0 ? (UInt16)TrayD.RowC : (UInt16)TrayD.ColC;
                                Row = (UInt16)(TrayD.LIC_RowN[1] - 1);
                            }
                            else
                            {
                                Column = (UInt16)(TrayD.LIC_ColN[1] - 1);
                                Row = (UInt16)TrayD.LIC_RowN[1];
                            }
                        }
                        else
                        {
                            if (TrayD.LIC_RowN[1] == 1)//第一行
                            {
                                Row = g_config.TrayRotateDir[1] != 0 ? (UInt16)TrayD.ColC : (UInt16)TrayD.RowC;
                                Column = (UInt16)(TrayD.LIC_ColN[1] - 1);
                            }
                            else
                            {
                                Column = (UInt16)TrayD.LIC_ColN[1];
                                Row = (UInt16)(TrayD.LIC_RowN[1] - 1);
                            }
                        }
                    }
                    if (Config.AutoTrayDir == 1)
                    {
                        MODBUS_WriteTrayData(Row, Column);
                    }
                    else
                    {
                        MODBUS_WriteTrayData(Column, Row);
                    }
                    g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送自动送盘放料行列数据,行[" + Row + "]列[" + Column, "Flow");
                    AT_RT_Step = 4;
                    break;

                case 4:
                    if (((StatusAlarmWord & Status_TrayLack) != 0 && !UserConfig.IsProgrammer) || Auto_Flag.ProductionFinish || Auto_Flag.ManualEnd || AutoTray_Process.ManualReceiveTray)//没有取料需求
                    {
                        AutoTray_Send_Cmd(Cmd_EndReceiveTray);//结束收盘命令
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送结束收盘命令", "Flow");
                    }
                    else
                    {
                        AutoTray_Send_Cmd(Cmd_ReceiveTray);//收盘命令
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送收盘命令", "Flow");
                    }
                    AT_RT_Step = 5;
                    break;

                case 5:
                    if (UserTimer.GetSysTime() >= AutoTray_Timer.CheckBusy_Cnt && (StatusAlarmWord & Status_SystemBusy) == 0)
                    {
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "收盘完成", "Flow");
                        //if ((StatusAlarmWord & Status_SystemRnunning) == 0)
                        {
                            TrayEndFlag.takeIC[1] = false;
                            TrayEndFlag.layIC[1] = false;
                            TrayEndFlag.takeLayIC[1] = false;
                            if (((StatusAlarmWord & Status_TrayLack) != 0 && !UserConfig.IsProgrammer) || Auto_Flag.ProductionFinish || Auto_Flag.ManualEnd || AutoTray_Process.ManualReceiveTray)//没有取料需求
                            {
                                if (AutoTray_Process.ManualReceiveTray)
                                {
                                    AutoTray_Process.ManualReceiveTray = false;
                                }
                                else
                                {
                                    Auto_Flag.AutoTray_End = true;
                                }
                            }
                            else
                            {
                                AutoTray_Trigger.TakeTray = true;
                                AutoTray_Process.TakeTray = true;
                                TrayD.TIC_TrayN = 2;   //取料盘号
                                TrayD.LIC_TrayN = 2;   //放料盘号
                                if (!Auto_Flag.AutoTray_TakeIC && Auto_Flag.AutoTray_LayIC)
                                {
                                    TrayD.TIC_ColN[1] = 0;
                                    TrayD.TIC_RowN[1] = 0;
                                }
                                else
                                {
                                    TrayD.TIC_ColN[1] = 1;
                                    TrayD.TIC_RowN[1] = 1;
                                }
                                TrayD.LIC_ColN[1] = 1;
                                TrayD.LIC_RowN[1] = 1;
                                TrayD.TIC_EndColN[1] = g_config.TrayRotateDir[1] != 0 ? TrayD.RowC : TrayD.ColC;
                                TrayD.TIC_EndRowN[1] = g_config.TrayRotateDir[1] != 0 ? TrayD.ColC : TrayD.RowC;
                                TrayState.TrayStateUpdate();
                            }
                            AT_RT_Step = 6;
                        }
                    }
                    break;

                case 6:
                    AutoTray_Process.ReceiveTray = false;
                    AT_RT_Step = 0;
                    break;

                default:
                    break;
            }
        }

        public void Alarm_Program()
        {
            switch (Alarm_Step)
            {
                case 0:
                    if ((StatusAlarmWord & Alarm_TrayLose) != 0)
                    {
                        AT_RT_Step = 0;
                        AT_TT_Step = 0;
                        Alarm_Step = 1;
                        BAR._ToolTipDlg.WriteToolTipStr("自动盘料盘丢失;[按下自动盘急停,处理异常->重新启动]");
                        BAR.ShowToolTipWnd(true);
                    }
                    break;

                case 1:
                    if (UserTask.Continue_AfterAlarm())
                    {
                        Auto_Flag.ALarmPause = false;
                        AT_RT_Step = 0;
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public class Dot_Config
    {
        public float FirstX;	          //起点X
        public float FirstY;	          //起点Y
        public float EndX;	              //终点X
        public float EndY;	              //终点Y
        public float ColC;	              //列数
        public float Col_Space;	          //列间距
        public float RowC;	              //行数
        public float Row_Space;	          //行间距
    }

    public class AutoTray_Timer
    {
        public static UInt64 ReadState_Cnt;  //编带读状态间隔计数
        public static UInt64 CheckBusy_Cnt;  //编带延迟检测BUSY计数
    }

    public class AutoTray_Trigger
    {
        /// <summary>
        /// 取盘触发标志
        /// </summary>
        public static bool TakeTray;    //取盘触发标志
        /// <summary>
        /// 收盘触发标志
        /// </summary>
        public static bool ReceiveTray; //收盘触发标志
    }

    public class AutoTray_Process
    {
        /// <summary>
        /// 取盘过程中标志
        /// </summary>
        public static bool TakeTray;          //取盘过程中标志
        /// <summary>
        /// 收盘过程中标志
        /// </summary>
        public static bool ReceiveTray;       //收盘过程中标志
        /// <summary>
        /// 收盘过程中标志
        /// </summary>
        public static bool ManualReceiveTray; //收盘过程中标志
        /// <summary>
        /// 手动取盘过程中标志
        /// </summary>
        public static bool ManualTakeTray;
    }
}
