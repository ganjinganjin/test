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
    public class AutoTube
    {
        public static UInt16 StatusAlarmWord = 0;
        public static bool SystemInitDone;    //初始化完成标志

        public Config g_config = Config.GetInstance();
        private static AutoTube _instance = null;
        private static readonly object padlock = new object();
        private SerialConector SerialCtrl;
        private static AutoTubeProxy proxy = AutoTubeProxy.GetInstance();

        #region----------------声明命令常量-------------------
        public const UInt16 Cmd_SystemInit     = 0x0001;
        public const UInt16 Cmd_TakeTray       = 0x0002;
        public const UInt16 Cmd_ReceiveTray    = 0x0004;
        public const UInt16 Cmd_EndReceiveTray = 0x000C;

        public const UInt16 Cmd_Suspend        = 0x0010;
        public const UInt16 Cmd_Continue       = 0x0020;

        public const UInt16 Cmd_EmergencyStop  = 0x0100;
        public const UInt16 Cmd_ResetInk       = 0x0200;

        public const UInt16 Cmd_ClearAlarm     = 0x8000;
        #endregion

        #region----------------声明状态及报警常量-------------------
        public const UInt16 Alarm_SystemAbnormity   = 0x0001;
        public const UInt16 Alarm_TrayFull          = 0x0002;
        public const UInt16 Alarm_TrayLack          = 0x0004;
        public const UInt16 Alm_InkLack             = 0x0008;

        public const UInt16 Alarm_Reserved_4        = 0x0010;
        public const UInt16 Status_SystemBusy       = 0x0020;
        public const UInt16 Status_SystemInitDone   = 0x0040;
        public const UInt16 Status_OverplusTrayInit = 0x0080;

        public const UInt16 Status_TakeTrayDone     = 0x0100;
        public const UInt16 Status_TrayLack         = 0x0200;
        public const UInt16 Status_Status_DotBusy   = 0x0400;
        public const UInt16 Status_Update_Ink       = 0x0800;

        public const UInt16 Status_TrayFull         = 0x1000;
        public const UInt16 Status_Reserved_13      = 0x2000;
        public const UInt16 Status_Reserved_14      = 0x4000;
        public const UInt16 Status_SystemRnunning   = 0x8000;
        #endregion

        #region----------------声明时间常量-------------------
        private const UInt16 Timer_ReadStateVal = 100;
        private const UInt16 Timer_CheckBusyVal = 300;
        #endregion


        public AutoTube()
        {
            //SerialCtrl = SerialConector.GetInstance("AutoTubeSerialCtl");
            //if (SerialCtrl.CreateSerialPort() && !SerialCtrl.IsOpen)
            //{
            //    SerialCtrl.OpenConnection(g_config.ArrStrutCom[5].ICom, g_config.ArrStrutCom[5].IBaud, g_config.ArrStrutCom[5].IDataBits, g_config.ArrStrutCom[5].IParity, g_config.ArrStrutCom[5].IStopBits, "AutoTube Ports");
            //}
            //SerialCtrl.AutoResetEventSet();
        }

        public static AutoTube GetInstance()
        {
            if (_instance == null)
            {
                lock (padlock)
                {
                    _instance = new AutoTube();
                }
            }
            return _instance;
        }

        public static void AutoTube_Send_Cmd(UInt16 Cmd)
        {
            AutoTube_Timer.ReadState_Cnt = UserTimer.GetSysTime() + Timer_ReadStateVal;
            AutoTube_Timer.CheckBusy_Cnt = UserTimer.GetSysTime() + Timer_CheckBusyVal;
            MODBUS_TubeCmdSend(Cmd);
        }

        private void MODBUS_ReadTubeStateAlarm()
        {
            int CRCtemp;
            byte[] TxBuffer = new byte[8];

            TxBuffer[0] = 0x04;
            TxBuffer[1] = 0x03;
            TxBuffer[2] = 0x00;
            TxBuffer[3] = 0x5B;
            TxBuffer[4] = 0x00;
            TxBuffer[5] = 0x01;
            CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
            TxBuffer[6] = (byte)(CRCtemp & 0x00FF);
            TxBuffer[7] = (byte)((CRCtemp & 0xFF00) >> 8);
            proxy.SendCmd("ReadTubeStateAlarm", TxBuffer, 7, false);
        }

        private static void MODBUS_TubeCmdSend(UInt16 CmdWord)
        {
            int CRCtemp;
            byte[] TxBuffer = new byte[11];

            TxBuffer[0] = 0x04;
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
            proxy.SendCmd("TubeCmdSend", TxBuffer, 8);
        }

        public void Handle()
        { 
            if (Auto_Flag.AutoTube_TakeIC)
            {
                if (UserTimer.GetSysTime() >= AutoTube_Timer.ReadState_Cnt)
                {
                    AutoTube_Timer.ReadState_Cnt = UserTimer.GetSysTime() + Timer_ReadStateVal;
                    if (SerialCtrl.IsOpen)
                    {
                        MODBUS_ReadTubeStateAlarm();//自动送盘状态警报读取
                    }                   	           
                } 	
            }
        }
    }

    public class AutoTube_Timer
    {
        public static UInt64 ReadState_Cnt;  //编带读状态间隔计数
        public static UInt64 CheckBusy_Cnt;  //编带延迟检测BUSY计数
    }
}
