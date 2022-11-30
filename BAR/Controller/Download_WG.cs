using BAR.Commonlib;
using BAR.Commonlib.Connectors;
using BAR.Commonlib.Connectors.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR
{
    public class Download_WG : UserTimer
    {
        public Config g_config = Config.GetInstance();
        public RPCServer rpcServer = RPCServer.GetInstance();

        public static UInt16 programerID = 0;
        public static Down_Parameter setParameter = new Down_Parameter();
        public static Down_Parameter getParameter = new Down_Parameter();
        public static Down_Parameter custom_Down_Param = new Down_Parameter();
        public static Down_Parameter[] const_Down_Param = new Down_Parameter[24];

        #region----------------声明步序常量-------------------
        public const int PROG_WAIT_CMD = 0;
        public const int PROG_START_DELAY = 1;
        public const int PROG_START_TRIGER = 2;
        public const int PROG_CHECK_TRIGER = 3;
        public const int PROG_CHECK_RESULT = 4;
        public const int PROG_WAIT_PUSH_O = 5;
        public const int PROG_WAIT_FLIP_O1 = 6;
        public const int PROG_WAIT_FLIP_O2 = 7;
        public const int PROG_WAIT_ALARM = 8;
        public const int PROG_CHECK_WORK = 9;
        public const int PROG_PUSH_RESET = 10;
        #endregion
        private static UInt64 Timer_ReadStateCnt;
        private const UInt16 Timer_ReadStateVal = 200;
        public static int PORT_NUM = 1;
        public const int UNIT_AMOUNT = 16;
        public static Programer_Unit[] programer_Unit = new Programer_Unit[UNIT_AMOUNT];
        public static Programer_Group[] programer_Group = new Programer_Group[UNIT_AMOUNT];

        public Download_WG()
        {
            #region----------------硕飞 烧录器参数 ID:11-------------------
            const_Down_Param[0].PulseWidth_Start = 100;
            const_Down_Param[0].Time_Busy = 100;
            const_Down_Param[0].Time_EOT = 100;
            const_Down_Param[0].Time_OKNG = 20;
            const_Down_Param[0].Level_Start = 0;
            const_Down_Param[0].Level_Busy = 1;
            const_Down_Param[0].Level_OK = 1;
            const_Down_Param[0].Level_NG = 1;
            const_Down_Param[0].RepeatNumber = 0;
            const_Down_Param[0].Time_Down = 20;
            #endregion
            #region----------------义隆DWTK8K 烧录器参数ID:12-------------------
            const_Down_Param[1].PulseWidth_Start = 100;
            const_Down_Param[1].Time_Busy = 50;
            const_Down_Param[1].Time_EOT = 100;
            const_Down_Param[1].Time_OKNG = 150;
            const_Down_Param[1].Level_Start = 0;
            const_Down_Param[1].Level_Busy = 0;
            const_Down_Param[1].Level_OK = 2;
            const_Down_Param[1].Level_NG = 2;
            const_Down_Param[1].RepeatNumber = 0;
            const_Down_Param[1].Time_Down = 10;
            #endregion
            #region----------------义隆U-WTR 烧录器参数ID:13-------------------
            const_Down_Param[2].PulseWidth_Start = 100;
            const_Down_Param[2].Time_Busy = 200;
            const_Down_Param[2].Time_EOT = 20;
            const_Down_Param[2].Time_OKNG = 100;
            const_Down_Param[2].Level_Start = 0;
            const_Down_Param[2].Level_Busy = 2;
            const_Down_Param[2].Level_OK = 0;
            const_Down_Param[2].Level_NG = 0;
            const_Down_Param[2].RepeatNumber = 0;
            const_Down_Param[2].Time_Down = 10;
            #endregion
            #region----------------松翰MP-3 烧录器参数ID:14-------------------
            const_Down_Param[3].PulseWidth_Start = 100;
            const_Down_Param[3].Time_Busy = 200;
            const_Down_Param[3].Time_EOT = 100;
            const_Down_Param[3].Time_OKNG = 100;
            const_Down_Param[3].Level_Start = 0;
            const_Down_Param[3].Level_Busy = 1;
            const_Down_Param[3].Level_OK = 1;
            const_Down_Param[3].Level_NG = 2;
            const_Down_Param[3].RepeatNumber = 0;
            const_Down_Param[3].Time_Down = 10;
            #endregion
            #region----------------松翰MP PRO 烧录器参数ID:15-------------------
            const_Down_Param[4].PulseWidth_Start = 100;
            const_Down_Param[4].Time_Busy = 200;
            const_Down_Param[4].Time_EOT = 10;
            const_Down_Param[4].Time_OKNG = 10;
            const_Down_Param[4].Level_Start = 0;
            const_Down_Param[4].Level_Busy = 0;
            const_Down_Param[4].Level_OK = 0;
            const_Down_Param[4].Level_NG = 0;
            const_Down_Param[4].RepeatNumber = 0;
            const_Down_Param[4].Time_Down = 10;
            #endregion
            #region----------------芯圣HC-PM 烧录器参数ID:16-------------------
            const_Down_Param[5].PulseWidth_Start = 100;
            const_Down_Param[5].Time_Busy = 100;
            const_Down_Param[5].Time_EOT = 10;
            const_Down_Param[5].Time_OKNG = 10;
            const_Down_Param[5].Level_Start = 0;
            const_Down_Param[5].Level_Busy = 1;
            const_Down_Param[5].Level_OK = 1;
            const_Down_Param[5].Level_NG = 1;
            const_Down_Param[5].RepeatNumber = 0;
            const_Down_Param[5].Time_Down = 10;
            #endregion
            #region----------------合泰e-w 烧录器参数ID:17-------------------
            const_Down_Param[6].PulseWidth_Start = 50;
            const_Down_Param[6].Time_Busy = 100;
            const_Down_Param[6].Time_EOT = 4;
            const_Down_Param[6].Time_OKNG = 40;
            const_Down_Param[6].Level_Start = 0;
            const_Down_Param[6].Level_Busy = 1;
            const_Down_Param[6].Level_OK = 0;
            const_Down_Param[6].Level_NG = 2;
            const_Down_Param[6].RepeatNumber = 0;
            const_Down_Param[6].Time_Down = 10;
            #endregion
            #region----------------PIC 烧录器参数ID:18-------------------
            const_Down_Param[7].PulseWidth_Start = 100;
            const_Down_Param[7].Time_Busy = 100;
            const_Down_Param[7].Time_EOT = 50;
            const_Down_Param[7].Time_OKNG = 50;
            const_Down_Param[7].Level_Start = 2;
            const_Down_Param[7].Level_Busy = 2;
            const_Down_Param[7].Level_OK = 2;
            const_Down_Param[7].Level_NG = 2;
            const_Down_Param[7].RepeatNumber = 0;
            const_Down_Param[7].Time_Down = 10;
            #endregion
            #region----------------三星GW-PRO2 烧录器参数ID:19-------------------
            const_Down_Param[8].PulseWidth_Start = 100;
            const_Down_Param[8].Time_Busy = 200;
            const_Down_Param[8].Time_EOT = 20;
            const_Down_Param[8].Time_OKNG = 210;
            const_Down_Param[8].Level_Start = 0;
            const_Down_Param[8].Level_Busy = 2;
            const_Down_Param[8].Level_OK = 1;
            const_Down_Param[8].Level_NG = 2;
            const_Down_Param[8].RepeatNumber = 0;
            const_Down_Param[8].Time_Down = 10;
            #endregion
            #region----------------三星G-PROG 烧录器参数ID:20-------------------
            const_Down_Param[9].PulseWidth_Start = 100;
            const_Down_Param[9].Time_Busy = 1;
            const_Down_Param[9].Time_EOT = 1;
            const_Down_Param[9].Time_OKNG = 50;
            const_Down_Param[9].Level_Start = 0;
            const_Down_Param[9].Level_Busy = 2;
            const_Down_Param[9].Level_OK = 1;
            const_Down_Param[9].Level_NG = 2;
            const_Down_Param[9].RepeatNumber = 0;
            const_Down_Param[9].Time_Down = 10;
            #endregion
            #region----------------中颍  烧录器参数ID:21-------------------
            const_Down_Param[10].PulseWidth_Start = 100;
            const_Down_Param[10].Time_Busy = 500;
            const_Down_Param[10].Time_EOT = 800;
            const_Down_Param[10].Time_OKNG = 200;
            const_Down_Param[10].Level_Start = 0;
            const_Down_Param[10].Level_Busy = 1;
            const_Down_Param[10].Level_OK = 1;
            const_Down_Param[10].Level_NG = 2;
            const_Down_Param[10].RepeatNumber = 0;
            const_Down_Param[10].Time_Down = 10;
            #endregion
            #region----------------建荣  烧录器参数ID:22-------------------
            const_Down_Param[11].PulseWidth_Start = 200;
            const_Down_Param[11].Time_Busy = 200;
            const_Down_Param[11].Time_EOT = 10;
            const_Down_Param[11].Time_OKNG = 1500;
            const_Down_Param[11].Level_Start = 1;
            const_Down_Param[11].Level_Busy = 2;
            const_Down_Param[11].Level_OK = 0;
            const_Down_Param[11].Level_NG = 1;
            const_Down_Param[11].RepeatNumber = 0;
            const_Down_Param[11].Time_Down = 5;
            #endregion
            #region----------------飞林  烧录器参数ID:23-------------------
            const_Down_Param[12].PulseWidth_Start = 100;
            const_Down_Param[12].Time_Busy = 100;
            const_Down_Param[12].Time_EOT = 10;
            const_Down_Param[12].Time_OKNG = 10;
            const_Down_Param[12].Level_Start = 0;
            const_Down_Param[12].Level_Busy = 0;
            const_Down_Param[12].Level_OK = 0;
            const_Down_Param[12].Level_NG = 2;
            const_Down_Param[12].RepeatNumber = 0;
            const_Down_Param[12].Time_Down = 10;
            #endregion
            #region----------------博巨兴  烧录器参数ID:24-------------------
            const_Down_Param[13].PulseWidth_Start = 100;
            const_Down_Param[13].Time_Busy = 100;
            const_Down_Param[13].Time_EOT = 50;
            const_Down_Param[13].Time_OKNG = 100;
            const_Down_Param[13].Level_Start = 0;
            const_Down_Param[13].Level_Busy = 0;
            const_Down_Param[13].Level_OK = 1;
            const_Down_Param[13].Level_NG = 2;
            const_Down_Param[13].RepeatNumber = 0;
            const_Down_Param[13].Time_Down = 10;
            #endregion
            #region----------------芯睿  烧录器参数ID:25-------------------
            const_Down_Param[14].PulseWidth_Start = 100;
            const_Down_Param[14].Time_Busy = 200;
            const_Down_Param[14].Time_EOT = 10;
            const_Down_Param[14].Time_OKNG = 10;
            const_Down_Param[14].Level_Start = 0;
            const_Down_Param[14].Level_Busy = 0;
            const_Down_Param[14].Level_OK = 1;
            const_Down_Param[14].Level_NG = 2;
            const_Down_Param[14].RepeatNumber = 0;
            const_Down_Param[14].Time_Down = 10;
            #endregion
            #region----------------十速TWR99  烧录器参数ID:26-------------------
            const_Down_Param[15].PulseWidth_Start = 100;
            const_Down_Param[15].Time_Busy = 100;
            const_Down_Param[15].Time_EOT = 10;
            const_Down_Param[15].Time_OKNG = 10;
            const_Down_Param[15].Level_Start = 1;
            const_Down_Param[15].Level_Busy = 2;
            const_Down_Param[15].Level_OK = 1;
            const_Down_Param[15].Level_NG = 1;
            const_Down_Param[15].RepeatNumber = 0;
            const_Down_Param[15].Time_Down = 10;
            #endregion
            #region----------------麦肯一拖二  烧录器参数ID:27-------------------
            const_Down_Param[16].PulseWidth_Start = 990;
            const_Down_Param[16].Time_Busy = 400;
            const_Down_Param[16].Time_EOT = 400;
            const_Down_Param[16].Time_OKNG = 150;
            const_Down_Param[16].Level_Start = 0;
            const_Down_Param[16].Level_Busy = 1;
            const_Down_Param[16].Level_OK = 0;
            const_Down_Param[16].Level_NG = 2;
            const_Down_Param[16].RepeatNumber = 0;
            const_Down_Param[16].Time_Down = 10;
            #endregion
            #region----------------应广  烧录器参数ID:28-------------------
            const_Down_Param[17].PulseWidth_Start = 100;
            const_Down_Param[17].Time_Busy = 100;
            const_Down_Param[17].Time_EOT = 1;
            const_Down_Param[17].Time_OKNG = 1;
            const_Down_Param[17].Level_Start = 0;
            const_Down_Param[17].Level_Busy = 1;
            const_Down_Param[17].Level_OK = 1;
            const_Down_Param[17].Level_NG = 2;
            const_Down_Param[17].RepeatNumber = 0;
            const_Down_Param[17].Time_Down = 10;
            #endregion
            #region----------------Nyquest九奇  烧录器参数ID:29-------------------
            const_Down_Param[18].PulseWidth_Start = 100;
            const_Down_Param[18].Time_Busy = 100;
            const_Down_Param[18].Time_EOT = 50;
            const_Down_Param[18].Time_OKNG = 50;
            const_Down_Param[18].Level_Start = 0;
            const_Down_Param[18].Level_Busy = 0;
            const_Down_Param[18].Level_OK = 0;
            const_Down_Param[18].Level_NG = 0;
            const_Down_Param[18].RepeatNumber = 0;
            const_Down_Param[18].Time_Down = 10;
            #endregion
            #region----------------易码  烧录器参数ID:30-------------------
            const_Down_Param[19].PulseWidth_Start = 100;
            const_Down_Param[19].Time_Busy = 100;
            const_Down_Param[19].Time_EOT = 5;
            const_Down_Param[19].Time_OKNG = 5;
            const_Down_Param[19].Level_Start = 0;
            const_Down_Param[19].Level_Busy = 2;
            const_Down_Param[19].Level_OK = 0;
            const_Down_Param[19].Level_NG = 0;
            const_Down_Param[19].RepeatNumber = 0;
            const_Down_Param[19].Time_Down = 10;
            #endregion
            #region----------------现代  烧录器参数ID:31-------------------
            const_Down_Param[20].PulseWidth_Start = 100;
            const_Down_Param[20].Time_Busy = 100;
            const_Down_Param[20].Time_EOT = 50;
            const_Down_Param[20].Time_OKNG = 50;
            const_Down_Param[20].Level_Start = 0;
            const_Down_Param[20].Level_Busy = 2;
            const_Down_Param[20].Level_OK = 0;
            const_Down_Param[20].Level_NG = 0;
            const_Down_Param[20].RepeatNumber = 0;
            const_Down_Param[20].Time_Down = 10;
            #endregion
            #region----------------中微CMS-WRITER 烧录器参数ID:32-------------------
            const_Down_Param[21].PulseWidth_Start = 200;
            const_Down_Param[21].Time_Busy = 2;
            const_Down_Param[21].Time_EOT = 2;
            const_Down_Param[21].Time_OKNG = 20;
            const_Down_Param[21].Level_Start = 0;
            const_Down_Param[21].Level_Busy = 2;
            const_Down_Param[21].Level_OK = 1;
            const_Down_Param[21].Level_NG = 1;
            const_Down_Param[21].RepeatNumber = 0;
            const_Down_Param[21].Time_Down = 10;
            #endregion
            #region----------------晟矽 烧录器参数ID:33-------------------
            const_Down_Param[22].PulseWidth_Start = 80;
            const_Down_Param[22].Time_Busy = 50;
            const_Down_Param[22].Time_EOT = 50;
            const_Down_Param[22].Time_OKNG = 50;
            const_Down_Param[22].Level_Start = 0;
            const_Down_Param[22].Level_Busy = 2;
            const_Down_Param[22].Level_OK = 0;
            const_Down_Param[22].Level_NG = 0;
            const_Down_Param[22].RepeatNumber = 0;
            const_Down_Param[22].Time_Down = 10;
            #endregion
            #region----------------众成  烧录器参数ID:34-------------------
            const_Down_Param[23].PulseWidth_Start = 100;
            const_Down_Param[23].Time_Busy = 100;
            const_Down_Param[23].Time_EOT = 20;
            const_Down_Param[23].Time_OKNG = 20;
            const_Down_Param[23].Level_Start = 1;
            const_Down_Param[23].Level_Busy = 1;
            const_Down_Param[23].Level_OK = 0;
            const_Down_Param[23].Level_NG = 0;
            const_Down_Param[23].RepeatNumber = 0;
            const_Down_Param[23].Time_Down = 10;
            #endregion
            if (programerID < 10)
            {
                g_config.ReadProgramerInfo();
            }
            else
            {
                setParameter = const_Down_Param[programerID - 10];
            }
            PORT_NUM = UserConfig.AllScketC % 8 == 0 ? 0 : 1;
            PORT_NUM += UserConfig.AllScketC / 8;
        }
        public static void Init()
        {
            for (int i = 0; i < UserConfig.ScketGroupC; i++)
            {
                programer_Group[i].Enable = 0;
                programer_Group[i].Step = 0;
                Axis.Group[i].Down.Busy = false;
                Axis.Group[i].Down.Trigger = false;
                for (int j = 0; j < UserConfig.ScketUnitC; j++)
                {
                    Axis.Group[i].Unit[j].DownResult = 0;
                }
            }

            for (int i = 0; i < UNIT_AMOUNT; i++)
            {
                programer_Unit[i].Step = 0;
                programer_Unit[i].Enable = false;
                programer_Unit[i].Trigger = false;
            }
        }


        public static void MODBUS_WriteDownloadParameter(byte add)
        {
            int CRCtemp;
            byte[] TxBuffer = new byte[31];

            TxBuffer[0] = add;
            TxBuffer[1] = 0x10;
            TxBuffer[2] = 0x00;
            TxBuffer[3] = 0x08;
            TxBuffer[4] = 0x00;
            TxBuffer[5] = 0x0B;
            TxBuffer[6] = 0x16;

            TxBuffer[7] = (byte)((setParameter.PulseWidth_Start & 0xFF00) >> 8);
            TxBuffer[8] = (byte)(setParameter.PulseWidth_Start & 0x00FF);

            TxBuffer[9] = (byte)((setParameter.Time_Busy & 0xFF00) >> 8);
            TxBuffer[10] = (byte)(setParameter.Time_Busy & 0x00FF);

            TxBuffer[11] = (byte)((setParameter.Time_EOT & 0xFF00) >> 8);
            TxBuffer[12] = (byte)(setParameter.Time_EOT & 0x00FF);

            TxBuffer[13] = (byte)((setParameter.Time_OKNG & 0xFF00) >> 8);
            TxBuffer[14] = (byte)(setParameter.Time_OKNG & 0x00FF);

            TxBuffer[15] = (byte)((setParameter.Level_Start & 0xFF00) >> 8);
            TxBuffer[16] = (byte)(setParameter.Level_Start & 0x00FF);

            TxBuffer[17] = (byte)((setParameter.Level_Busy & 0xFF00) >> 8);
            TxBuffer[18] = (byte)(setParameter.Level_Busy & 0x00FF);

            TxBuffer[19] = (byte)((setParameter.Level_OK & 0xFF00) >> 8);
            TxBuffer[20] = (byte)(setParameter.Level_OK & 0x00FF);

            TxBuffer[21] = (byte)((setParameter.Level_NG & 0xFF00) >> 8);
            TxBuffer[22] = (byte)(setParameter.Level_NG & 0x00FF);

            TxBuffer[23] = (byte)((setParameter.RepeatNumber & 0xFF00) >> 8);
            TxBuffer[24] = (byte)(setParameter.RepeatNumber & 0x00FF);

            TxBuffer[25] = (byte)((setParameter.Time_Down & 0xFF00) >> 8);
            TxBuffer[26] = (byte)(setParameter.Time_Down & 0x00FF);

            TxBuffer[27] = (byte)((setParameter.ID & 0xFF00) >> 8);
            TxBuffer[28] = (byte)(setParameter.ID & 0x00FF);

            CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
            TxBuffer[29] = (byte)(CRCtemp & 0x00FF);
            TxBuffer[30] = (byte)((CRCtemp & 0xFF00) >> 8);
            Download.proxy.SendCmd("WriteDownloadParameter", TxBuffer, 8);
        }

        public static void MODBUS_ReadDownloadParameter(byte add)//读烧录参数程序
        {
            int CRCtemp;
            byte[] TxBuffer = new byte[8];
            TxBuffer[0] = add;
            TxBuffer[1] = 0x03;
            TxBuffer[2] = 0x00;
            TxBuffer[3] = 0x08;
            TxBuffer[4] = 0x00;
            TxBuffer[5] = 0x0B;
            CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
            TxBuffer[6] = (byte)(CRCtemp & 0x00FF);
            TxBuffer[7] = (byte)((CRCtemp & 0xFF00) >> 8);
            Download.proxy.SendCmd("ReadDownloadParameter", TxBuffer, 27, true, 100);
        }

        private void MODBUS_DownloadStart(byte add, byte enabled)//烧录启动程序
        {
            int CRCtemp;
            byte[] TxBuffer = new byte[8];
            TxBuffer[0] = add;
            TxBuffer[1] = 0x06;
            TxBuffer[2] = 0x00;
            TxBuffer[3] = 0x14;
            TxBuffer[4] = 0x00;
            TxBuffer[5] = enabled;

            CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
            TxBuffer[6] = (byte)(CRCtemp & 0x00FF);
            TxBuffer[7] = (byte)((CRCtemp & 0xFF00) >> 8);
            Download.proxy.SendCmd("DownloadStart", TxBuffer, 8);
        }

        public void MODBUS_ReadDownloadStatus_Result(byte add)
        {
            int CRCtemp;
            byte[] TxBuffer = new byte[8];

            TxBuffer[0] = add;
            TxBuffer[1] = 0x03;
            TxBuffer[2] = 0x00;
            TxBuffer[3] = 0x15;
            TxBuffer[4] = 0x00;
            TxBuffer[5] = 0x04;
            CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
            TxBuffer[6] = (byte)(CRCtemp & 0x00FF);
            TxBuffer[7] = (byte)((CRCtemp & 0xFF00) >> 8);
            Download.proxy.SendCmd("ReadDownloadStatus_Result" + add, TxBuffer, 13, false);
        }

        /// <summary>
        /// 烧录器烧录处理
        /// </summary>
        public void Program()
        {
            if (GetSysTime() >= Timer_ReadStateCnt)
            {
                Timer_ReadStateCnt = GetSysTime() + Timer_ReadStateVal;
                if (Download.proxy.SerialCtrl.IsOpen)
                {
                    MODBUS_ReadDownloadStatus_Result(1);
                    if (PORT_NUM > 1)
                    {
                        int temp = 8 / UserConfig.ScketUnitC;//每块信号板组数
                        for (byte i = 1; i < PORT_NUM; i++)
                        {
                            for (byte j = 0; j < temp; j++)
                            {
                                int index = i * temp + j;
                                if (index == UserConfig.ScketGroupC)
                                {
                                    break;
                                }
                                if (Axis.Group[index].Down.Busy)
                                {
                                    MODBUS_ReadDownloadStatus_Result((byte)(i + 1));
                                    break;
                                }
                            }

                        }
                    }
                }
            }
            for (int i = 0; i < UserConfig.ScketGroupC; i++)
            {
                Program_Group_Handle(ref programer_Group[i], Axis.Group[i], i);
            }

            for (int i = 0; i < UNIT_AMOUNT; i++)
            {
                Programer_Unit_handle(ref programer_Unit[i], i);
            }
        }

        /// <summary>
        /// 组烧录处理
        /// </summary>
        /// <param name="programer"></param>
        /// <param name="group"></param>
        private void Program_Group_Handle(ref Programer_Group programer, SocketGroup group, int groupNum)
        {
            int temp;
            switch (programer.Step)
            {
                case PROG_WAIT_CMD: //等待触发
                    if (group.Down.Trigger)
                    {
                        group.Down.Trigger = false;
                        programer.Step = PROG_START_DELAY;
                    }
                    break;

                case PROG_START_DELAY: //触发延时处理
                    if (Auto_Flag.Flip && UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY)
                    {
                        programer.Enable = GetEnable(group, groupNum);
                        for (int i = 0; i < UserConfig.ScketUnitC; i++)
                        {
                            temp = groupNum * UserConfig.ScketUnitC + i;
                            programer_Unit[temp].Work = programer_Unit[temp].Trigger = programer_Unit[temp].Enable;
                        }
                    }
                    else
                    {
                        if (Auto_Flag.AutoRunBusy)//自动模式
                        {
                            programer.Timer_DownDelay = GetSysTime() + AutoTiming.DownDelay + 5;
                        }
                        else
                        {
                            programer.Timer_DownDelay = GetSysTime() + 5;
                        }
                    }
                    programer.Step = PROG_START_TRIGER;
                    break;

                case PROG_START_TRIGER: //触发烧录
                    if (Auto_Flag.Flip && UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY)
                    {
                        if (Channel_Work_Check(groupNum))
                        {
                            int add = groupNum * UserConfig.ScketUnitC / 8 + 1;
                            MODBUS_DownloadStart((byte)add, programer.Enable);
                            for (int i = 0; i < UserConfig.ScketUnitC; i++)
                            {
                                temp = groupNum * UserConfig.ScketUnitC + i;
                                programer_Unit[temp].Work = programer_Unit[temp].Enable;
                            }
                            programer.Step = PROG_CHECK_RESULT;
                        }
                    }
                    else
                    {
                        if (GetSysTime() > programer.Timer_DownDelay)//烧录延时时间到
                        {
                            programer.Enable = GetEnable(group, groupNum);
                            int add = groupNum * UserConfig.ScketUnitC / 8 + 1;
                            MODBUS_DownloadStart((byte)add, programer.Enable);
                            programer.Step = PROG_CHECK_TRIGER;
                        }
                    }   
                    break;

                case PROG_CHECK_TRIGER: //检测触发
                    for (int i = 0; i < UserConfig.ScketUnitC; i++)
                    {
                        temp = groupNum * UserConfig.ScketUnitC + i;
                        programer_Unit[temp].Work = programer_Unit[temp].Trigger = programer_Unit[temp].Enable;
                    }
                    programer.Step = PROG_CHECK_RESULT;
                    break;

                case PROG_CHECK_RESULT: //检测烧录结果
                    if (Channel_Work_Check(groupNum))
                    {
                        group.Down.Busy = false;
                        programer.Step = PROG_WAIT_CMD;
                    }
                    break;

                default:
                    programer.Step = PROG_WAIT_CMD;
                    break;
            }
        }

        /// <summary>
        /// 单元烧录处理
        /// </summary>
        /// <param name="programer"></param>
        /// <param name="triggerIndex"></param>
        private void Programer_Unit_handle(ref Programer_Unit programer, int triggerIndex)
        {
            switch (programer.Step)
            {
                case PROG_WAIT_CMD: //等待触发
                    if (programer.Trigger)
                    {
                        programer.Trigger = false;
                        if (Auto_Flag.Flip && UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY)
                        {
                            In_Output.flipO[triggerIndex].M = true;
                            programer.Timer_DownDelay = GetSysTime() + 3000;
                            programer.Step = PROG_WAIT_FLIP_O1;
                        }
                        else
                        {
                            programer.Timer_Down_TimeOut = GetSysTime() + AutoTiming.DownTimeOut + 3000;//设置烧录超时
                            programer.Timer_DownDelay = GetSysTime() + 3000;
                            programer.Step = PROG_CHECK_TRIGER;
                        }
                    }
                    break;

                case PROG_WAIT_FLIP_O1: //等待推座到位
                    if (In_Output.flipLimitI[triggerIndex].M == true)//感应到位
                    {
                        programer.Timer_DownDelay = GetSysTime();
                        programer.Step = PROG_START_DELAY;
                    }
                    else if (GetSysTime() > programer.Timer_DownDelay)//感应到位超时
                    {
                        BAR._ToolTipDlg.WriteToolTipStr("推座[" + (triggerIndex + 1).ToString() + "]气缸工位感应超时;[处理异常->确定->继续]");
                        BAR.ShowToolTipWnd(true);
                        programer.BackStep = programer.Step;
                        programer.Step = PROG_WAIT_ALARM;
                    }
                    break;

                case PROG_START_DELAY: //等待延时
                    if (GetSysTime() >= programer.Timer_DownDelay)
                    {
                        In_Output.pushSeatO[triggerIndex].M = true;
                        programer.Timer_DownDelay = GetSysTime() + AutoTiming.DownDelay + 5;
                        programer.Step = PROG_START_TRIGER;
                    }
                    break;

                case PROG_START_TRIGER: //触发烧录	
                    if (GetSysTime() > programer.Timer_DownDelay)//烧录延时时间到
                    {
                        programer.Work = false;
                        programer.Step = PROG_CHECK_WORK;
                    }
                    break;

                case PROG_CHECK_WORK: //检查烧录触发
                    if (programer.Work)
                    {
                        if (Auto_Flag.TestMode && Auto_Flag.AutoRunBusy)
                        {
                            programer.Timer_DownDelay = GetSysTime() + 1000;
                        }
                        else
                        {
                            programer.Timer_DownDelay = GetSysTime() + 3000;
                            programer.Timer_Down_TimeOut = GetSysTime() + AutoTiming.DownTimeOut + 3000;//设置烧录超时
                        }
                        programer.Step = PROG_CHECK_TRIGER;
                    }
                    break;

                case PROG_CHECK_TRIGER://检测烧录是否开始
                    if (Auto_Flag.TestMode && Auto_Flag.AutoRunBusy)
                    {
                        if (GetSysTime() > programer.Timer_DownDelay)
                        {
                            Result_Handle(triggerIndex, 1);
                            programer.Step = PROG_PUSH_RESET;
                        }
                    }
                    else if (programer.Status == 1)//烧录中
                    {
                        programer.Step = PROG_CHECK_RESULT;
                    }
                    else if (GetSysTime() > programer.Timer_DownDelay)//烧录触发异常
                    {
                        programer.Step = PROG_PUSH_RESET;
                        if (UserConfig.IsProgrammer)
                        {
                            Result_Handle(triggerIndex, 1);
                        }
                        else
                        {
                            Result_Handle(triggerIndex, 2);
                        }
                        
                    }
                    break;

                case PROG_CHECK_RESULT: //检测烧录结果		
                    if (programer.Status == 2)//烧录完成
                    {
                        if (programer.Result == 5)//OK
                        {
                            Result_Handle(triggerIndex, 1);
                        }
                        else//NG
                        {
                            Result_Handle(triggerIndex, 2);
                        }
                        programer.Step = PROG_PUSH_RESET;
                    }
                    else if (GetSysTime() > programer.Timer_Down_TimeOut)
                    {
                        Result_Handle(triggerIndex, 2);
                        programer.Step = PROG_PUSH_RESET;
                    }
                    break;

                case PROG_PUSH_RESET: //复位压座
                    if (Auto_Flag.Flip && UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY)
                    {
                        In_Output.pushSeatO[triggerIndex].M = false;
                        programer.Timer_DownDelay = GetSysTime() + 3000;
                        programer.Step = PROG_WAIT_PUSH_O;
                    }
                    else
                    {
                        programer.Work = false;
                        programer.Step = PROG_WAIT_CMD;
                    }
                    break;

                case PROG_WAIT_PUSH_O: //等待压座复位
                    if (In_Output.pushSeatI[triggerIndex].M == true)
                    {
                        In_Output.flipO[triggerIndex].M = false;
                        programer.Timer_DownDelay = GetSysTime() + 3000;
                        programer.Step = PROG_WAIT_FLIP_O2;
                    }
                    else if (GetSysTime() > programer.Timer_DownDelay)//感应到位超时
                    {
                        BAR._ToolTipDlg.WriteToolTipStr("压座[" + (triggerIndex + 1).ToString() + "]气缸原位感应超时;[处理异常->确定->继续]");
                        BAR.ShowToolTipWnd(true);
                        programer.BackStep = programer.Step;
                        programer.Step = PROG_WAIT_ALARM;
                    }
                    break;

                case PROG_WAIT_FLIP_O2: //等待推座复位
                    if (In_Output.flipI[triggerIndex].M == true)
                    {
                        programer.Step = PROG_WAIT_CMD;
                        programer.Work = false;
                    }
                    else if (GetSysTime() > programer.Timer_DownDelay)//感应到位超时
                    {
                        BAR._ToolTipDlg.WriteToolTipStr("推座[" + (triggerIndex + 1).ToString() + "]气缸原位感应超时;[处理异常->确定->继续]");
                        BAR.ShowToolTipWnd(true);
                        programer.BackStep = programer.Step;
                        programer.Step = PROG_WAIT_ALARM;
                    }
                    break;

                case PROG_WAIT_ALARM: //报警等待
                    if (UserTask.Continue_AfterSyncAlarm())
                    {
                        programer.Timer_DownDelay = GetSysTime() + 3000;
                        programer.Step = programer.BackStep;
                    }
                    break;

                default:
                    programer.Step = PROG_WAIT_CMD;
                    programer.Work = false;
                    break;
            }
        }

        /// <summary>
        /// 获取使能
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        private byte GetEnable(SocketGroup group, int groupNum)
        {
            int temp, index;
            int retval = 0;
            for (int i = 0; i < UserConfig.ScketUnitC; i++)
            {
                temp = groupNum * UserConfig.ScketUnitC + i;
                index = temp % 8;
                programer_Unit[temp].Step = 0;
                programer_Unit[temp].Enable = false;
                programer_Unit[temp].Work = false;
                programer_Unit[temp].Trigger = false;
                if (ChannelEnableCheck(group, i))
                {
                    group.Unit[i].Flag_NewIC = false;
                    group.Unit[i].DownResult = 4;
                    programer_Unit[temp].Enable = true;
                    retval = retval | (0x1 << index);
                    rpcServer.YieldNotify();
                }
            }
            return (byte)retval;
        }

        bool Channel_Work_Check(int groupNum)
        {
            int temp;
            for (int i = 0; i < UserConfig.ScketUnitC; i++)
            {
                temp = groupNum * UserConfig.ScketUnitC + i;
                if (programer_Unit[temp].Work)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 烧录结果检查
        /// </summary>
        /// <param name="unitNum"></param>
        /// <param name="result"></param>
        private void Result_Handle(int triggerIndex, byte result)
        {
            int groupNum, unitcNum;
            groupNum = triggerIndex / UserConfig.ScketUnitC;
            unitcNum = triggerIndex % UserConfig.ScketUnitC;
            Axis.Group[groupNum].Unit[unitcNum].DownResult = result;
            g_config.WriteSocketCounter(triggerIndex, result);
            if (Auto_Flag.AutoRunBusy)
            {
                ContinueNGCheck(Axis.Group[groupNum], unitcNum);
                rpcServer.YieldNotify();
            }
            rpcServer.YieldNotify();
        }

        /// <summary>
        /// 烧录通道使能检查
        /// </summary>
        /// <param name="group"></param>
        /// <param name="unitN"></param>
        /// <returns></returns>
        private bool ChannelEnableCheck(SocketGroup group, int unitN)
        {
            bool retval = false;
            if (Auto_Flag.AutoRunBusy && group.Unit[unitN].Flag_TakeIC)
            {
                retval = true;
            }
            if (!Auto_Flag.AutoRunBusy && group.Unit[unitN].Flag_Open)
            {
                retval = true;
            }
            return retval;
        }

        /// <summary>
        /// 连续NG检查
        /// </summary>
        /// <param name="group"></param>
        /// <param name="unitN"></param>
        private void ContinueNGCheck(SocketGroup group, int unitN)
        {
            if (group.Unit[unitN].DownResult == 2)//NG
            {
                if (UserTask.NGContinueC > 0)
                {
                    group.Unit[unitN].Counter_NG++;
                    if (group.Unit[unitN].Counter_NG >= UserTask.NGContinueC)
                    {
                        group.Continue_NG = true;
                    }
                }
            }
            else if (group.Unit[unitN].DownResult == 1)//OK
            {
                group.Unit[unitN].Counter_NG = 0;
            }
        }


        public struct Programer_Unit
        {
            public int Step;                                    //座子烧录步骤
            public int BackStep;                                //座子烧录备份步骤
            public bool Enable;                                 //烧录使能
            public bool Trigger;                                //烧录触发
            public bool Work;                                   //烧录工作
            public byte Status;                                 //烧录状态
            public byte Result;                                 //烧录结果
            public UInt64 Timer_DownDelay;                      //烧录延时时间
            public UInt64 Timer_Down_TimeOut;                   //烧录超时时间
        }

        public struct Programer_Group
        {
            public int Step;            //烧录步骤 
            public byte Enable;        //烧录器使能

            public UInt64 Timer_DownDelay;
            public UInt64 Timer_Down_TimeOut;
        }

        public struct Down_Parameter
        {
            public UInt16 PulseWidth_Start;  //启动脉宽
            public UInt16 Time_Busy;         //Busy时间
            public UInt16 Time_EOT;          //EOT时间
            public UInt16 Time_OKNG;         //OKNG时间
            public UInt16 Level_Start;       //启动电平
            public UInt16 Level_Busy;        //Busy电平
            public UInt16 Level_OK;          //OK电平
            public UInt16 Level_NG;          //NG电平
            public UInt16 RepeatNumber;      //重烧次数
            public UInt16 Time_Down;         //烧录时间
            public UInt16 ID;                //烧录时间
        }
    }
}
