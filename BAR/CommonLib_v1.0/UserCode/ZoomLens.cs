using BAR.Commonlib;
using BAR.Commonlib.Connectors;
using BAR.Commonlib.Connectors.Protocols;
using BAR.Communicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR
{
    public class ZoomLens
    {
        public Config g_config = Config.GetInstance();
        public Act g_act = Act.GetInstance();
        public Axis axis = Axis.GetInstance();
        private static ZoomLens _instance = null;
        private static readonly object padlock = new object();
        private SerialConector SerialCtrl;
        private static ZoomLensProxy proxy = ZoomLensProxy.GetInstance();

        #region----------------声明状态及报警常量-------------------
        public const UInt16 Status_SystemStop = 0x00;
        #endregion

        #region----------------声明步序变量-------------------
        private static int ZL_I_Step = 0;
        #endregion

        #region----------------声明时间常量-------------------
        private const UInt16 Timer_ReadStateVal = 200;
        #endregion

        /// <summary>
        /// 当前位置值
        /// </summary>
        public static double NowPos;
        /// <summary>
        /// 原点位置
        /// </summary>
        public static double OriginPos;
        /// <summary>
        /// 电机状态
        /// </summary>
        public static int MotorStatus = 2;
        /// <summary>
        /// 设置位置值
        /// </summary>
        public static double SetPos;
        public ZoomLens()
        {
            if (Config.ZoomLens == 1)
            {
                SerialCtrl = SerialConector.GetInstance("ZoomLensSerialCtl");
                if (SerialCtrl.CreateSerialPort() && !SerialCtrl.IsOpen)
                {
                    SerialCtrl.OpenConnection(g_config.ArrStrutCom[5].ICom, g_config.ArrStrutCom[5].IBaud, g_config.ArrStrutCom[5].IDataBits, g_config.ArrStrutCom[5].IParity, g_config.ArrStrutCom[5].IStopBits, "ZoomLens Ports");
                }
                SerialCtrl.AutoResetEventSet();
            }
        }

        public static ZoomLens GetInstance()
        {
            if (_instance == null)
            {
                lock (padlock)
                {
                    _instance = new ZoomLens();
                }
            }
            return _instance;
        }

        /// <summary>
        /// 读取电机状态指令
        /// </summary>
        public static void MODBUS_ReadStatus()
        {
            //Axis.ZoomLens_S.ReadStatus_Busy = true;
            int CRCtemp;
            byte[] TxBuffer = new byte[16];
            TxBuffer[0] = 0x01;
            TxBuffer[1] = 0x65;
            TxBuffer[2] = 0x00;
            TxBuffer[3] = 0x00;
            TxBuffer[4] = 0x00;
            TxBuffer[5] = 0x00;
            TxBuffer[6] = 0x00;
            TxBuffer[7] = 0x00;
            TxBuffer[8] = 0x00;
            TxBuffer[9] = 0x00;
            TxBuffer[10] = 0x00;
            TxBuffer[11] = 0x00;
            TxBuffer[12] = 0x00;
            TxBuffer[13] = 0x00;
            CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
            TxBuffer[14] = (byte)(CRCtemp & 0x00FF);
            TxBuffer[15] = (byte)((CRCtemp & 0xFF00) >> 8);
            proxy.SendCmd("ReadZoomLensStatus", TxBuffer, 16, false, 50);
        }

        /// <summary>
        /// 电机控制指令
        /// </summary>
        public static byte[] MODBUS_DST(bool IsStop = false)
        {
            int CRCtemp;
            byte[] setPos;
            byte[] TxBuffer = new byte[16];
            TxBuffer[0] = 0x01;
            TxBuffer[1] = 0x64;
            TxBuffer[2] = 0x01;
            TxBuffer[3] = 0x00;
            TxBuffer[4] = 0x00;
            if (IsStop == false)
            {
                TxBuffer[5] = 0x33;
            }
            else
            {
                TxBuffer[5] = 0x00;
            }
            TxBuffer[6] = 0x00;
            setPos = BitConverter.GetBytes(IsStop == false ? (int)SetPos : 0);
            TxBuffer[7] = setPos[3];
            TxBuffer[8] = setPos[2];
            TxBuffer[9] = setPos[1];
            TxBuffer[10] = setPos[0];
            TxBuffer[11] = 0x00;
            TxBuffer[12] = 0x00;
            TxBuffer[13] = 0x00;
            CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
            TxBuffer[14] = (byte)(CRCtemp & 0x00FF);
            TxBuffer[15] = (byte)((CRCtemp & 0xFF00) >> 8);
            proxy.SendCmd("DST", TxBuffer, 16, true, 50);
            return TxBuffer;
        }


        public static void Init()
        {
            ZL_I_Step = 0;
            Axis.ZoomLens_S.Home = false;
        }

        public void Handle()
        {
            if (Config.ZoomLens == 1)
            {
                if (UserTimer.GetSysTime() >= ZoomLens_Timer.ReadState_Cnt)
                {
                    ZoomLens_Timer.ReadState_Cnt = UserTimer.GetSysTime() + Timer_ReadStateVal;
                    if (SerialCtrl.IsOpen)
                    {
                        MODBUS_ReadStatus();
                    }
                }
            }
            
            Init_Program();

        }

        /// <summary>
        /// 变焦镜头回原点
        /// </summary>
        public static void Init_Program_Start()
        {
            if (ZL_I_Step == 0 && !Auto_Flag.AutoRunBusy && !Axis.ZoomLens_S.Home)
            {
                ZL_I_Step = 1;
                OriginPos = 0;
            }
        }

        public void Init_Program()
        {
            if (Config.ZoomLens == 1)
            {
                switch (ZL_I_Step)
                {
                    case 1:
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "镜头回零中......", "Flow");
                        ZoomLens.SetPos = 500000;
                        ZoomLens.MODBUS_DST();
                        ZL_I_Step = 2;
                        break;

                    case 2:
                        ZoomLens.MODBUS_ReadStatus();
                        ZoomLens_Timer.Delay = UserTimer.GetSysTime() + 1000;
                        ZL_I_Step = 3;

                        break;

                    case 3:
                        if (UserTimer.GetSysTime() >= ZoomLens_Timer.Delay)
                        {
                            if (Math.Abs(ZoomLens.NowPos - OriginPos) > 5)
                            {
                                OriginPos = ZoomLens.NowPos;
                                ZL_I_Step = 2;
                            }
                            else
                            {
                                ZoomLens.SetPos = -2;
                                ZoomLens.MODBUS_DST(true);//停止
                                //g_act.WaitDoEvent(500);
                                //ZoomLens.MODBUS_DST();
                                ZoomLens.MODBUS_ReadStatus();
                                g_act.WaitDoEvent(500);
                                OriginPos = ZoomLens.NowPos;
                                Axis.ZoomLens_S.Home = true;
                                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "镜头回零成功[OriginPos：" + OriginPos.ToString() + "]", "Flow");
                                ZL_I_Step = 0;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            else if(Config.ZoomLens == 2)
            {
                switch (ZL_I_Step)
                {
                    case 1:
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "镜头回零中......", "Flow");
                        axis.ORG_Function(Axis.homePrm_U, Axis.axisSts_U);
                        ZL_I_Step = 2;
                        break;

                    case 2:
                        if (Axis.axisSts_U.isHome)
                        {
                            ZL_I_Step = 3;
                        }
                        break;

                    case 3:
                        Axis.ZoomLens_S.Home = true;
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "镜头回零成功", "Flow");
                        ZL_I_Step = 0;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public class ZoomLens_Timer
    {
        public static UInt64 ReadState_Cnt;  //变焦读状态间隔计数
        public static UInt64 Delay;
    }
}
