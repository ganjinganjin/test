using BAR.Commonlib;
using BAR.Commonlib.Connectors.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR
{
    public class Download_YED : UserTimer
    {
        public Config g_config = Config.GetInstance();

        public static UInt16 programerID = 0;

        #region----------------声明步序常量-------------------
        public const int SCAN_WAIT_CMD = 0;
        public const int SCAN_CHECK_TRIGER = 1;
        public const int SCAN_CHECK_RESULT = 2;
        public const int PROG_WAIT_CMD = 3;
        public const int PROG_START_DELAY = 4;
        public const int PROG_START_TRIGER = 5;
        public const int PROG_CHECK_TRIGER = 6;
        public const int PROG_CHECK_RESULT = 7;
        #endregion
        private static UInt64 Timer_ReadStateCnt;
        private const UInt16 Timer_ReadStateVal = 50;
        public const int UNIT_AMOUNT = 8;
        public static Programer_Group[] programer_Group = new Programer_Group[UNIT_AMOUNT];

        public static void Init()
        {
            for (int i = 0; i < UserConfig.ScketGroupC; i++)
            {
                programer_Group[i].Enable = 0;
                programer_Group[i].Step = 0;
                Axis.Group[i].Down.Busy = false;
                Axis.Group[i].Down.Trigger = false;
                Axis.Group[i].Scan.Busy = false;
                Axis.Group[i].Scan.Trigger = false;
                for (int j = 0; j < UserConfig.ScketUnitC; j++)
                {
                    Axis.Group[i].Unit[j].DownResult = 0;
                }
            }
        }

        private void MODBUS_DownloadStart(byte add, byte enabled, bool mode = true)//烧录启动程序
        {
            int CRCtemp;
            byte[] TxBuffer = new byte[11];
            TxBuffer[0] = add;
            if (mode)
            {
                TxBuffer[1] = 0x66;
                TxBuffer[2] = 0x00;
                TxBuffer[3] = 0xC8;
            }
            else
            {
                TxBuffer[1] = 0x68;
                TxBuffer[2] = 0x00;
                TxBuffer[3] = 0xC9;
            }
            TxBuffer[4] = 0x00;
            TxBuffer[5] = 0x01;
            TxBuffer[6] = 0x02;
            TxBuffer[7] = 0x00;
            TxBuffer[8] = enabled;

            CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
            TxBuffer[9] = (byte)(CRCtemp & 0x00FF);
            TxBuffer[10] = (byte)((CRCtemp & 0xFF00) >> 8);
            Download.proxy.SendCmd("DownloadStart", TxBuffer, 0);
        }

        public void MODBUS_ReadDownloadStatus_Result(byte add)
        {
            int CRCtemp;
            byte[] TxBuffer = new byte[8];

            TxBuffer[0] = add;
            TxBuffer[1] = 0x67;
            TxBuffer[2] = 0x00;
            TxBuffer[3] = 0x00;
            TxBuffer[4] = 0x00;
            TxBuffer[5] = 0x08;
            CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
            TxBuffer[6] = (byte)(CRCtemp & 0x00FF);
            TxBuffer[7] = (byte)((CRCtemp & 0xFF00) >> 8);
            Download.proxy.SendCmd("ReadDownloadStatus_Result", TxBuffer, 0, false);
        }

        /// <summary>
        /// 烧录器烧录处理
        /// </summary>
        public void Program()
        {
            if (UserTimer.GetSysTime() >= Timer_ReadStateCnt)
            {
                Timer_ReadStateCnt = UserTimer.GetSysTime() + Timer_ReadStateVal;
                if (Download.proxy.SerialCtrl.IsOpen)
                {
                    MODBUS_ReadDownloadStatus_Result(1);
                }
            }
            for (int i = 0; i < UserConfig.ScketGroupC; i++)
            {
                Program_Group_Handle(ref programer_Group[i], Axis.Group[i], i);
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
                case SCAN_WAIT_CMD: //等待扫码触发
                    if (group.Scan.Trigger || group.Down.Trigger)
                    {
                        if (ChannelEnableCheck(group,0))
                        {
                            programer.Enable = GetEnable(group, groupNum);   
                            if (group.Down.Trigger)
                            {
                                programer.Step = PROG_START_DELAY;
                            }
                            else
                            {
                                programer.Timer_Down_TimeOut = GetSysTime() + 3000;//设置扫码超时
                                programer.Timer_DownDelay = GetSysTime() + 3000;
                                MODBUS_DownloadStart(1, programer.Enable, false);
                                programer.Step = SCAN_CHECK_TRIGER;
                            } 
                        }
                        else
                        {
                            group.Scan.Busy = false;
                            group.Down.Busy = false;
                        }
                        group.Down.Trigger = false;
                        group.Scan.Trigger = false;
                    }
                    break;

                case SCAN_CHECK_TRIGER: //检测扫码触发
                    if (programer.Status == 8)
                    {
                        programer.Step = SCAN_CHECK_RESULT;
                    }
                    else if (GetSysTime() > programer.Timer_DownDelay)//扫码触发异常
                    {
                        programer.Step = SCAN_WAIT_CMD;
                        if (UserConfig.IsProgrammer)
                        {
                            Result_Handle(groupNum, 1);
                        }
                        else
                        {
                            Result_Handle(groupNum, 2);
                        }
                        group.Scan.Busy = false;
                        group.Down.Busy = false;
                    }
                    break;

                case SCAN_CHECK_RESULT: //检测扫码结果
                    if (programer.Status == 1)//扫码OK
                    {
                        Result_Handle(groupNum, 1);
                        if (!Auto_Flag.AutoRunBusy)
                        {
                            group.Down.Trigger = true;//该组启动烧录;
                            if (Auto_Flag.Flip)
                            {
                                In_Output.flipO[groupNum].M = true;
                            }
                            else
                            {
                                In_Output.pushSeatO[groupNum].M = false;
                            }
                        }
                        programer.Step = PROG_WAIT_CMD;
                        group.Scan.Busy = false;
                    }
                    else if (programer.Status == 2)//扫码NG
                    {
                        Result_Handle(groupNum, 2);
                        programer.Step = SCAN_WAIT_CMD;
                        group.Scan.Busy = false;
                        group.Down.Busy = false;
                    }
                    else if (GetSysTime() > programer.Timer_Down_TimeOut)
                    {
                        Result_Handle(groupNum, 2);
                        programer.Step = SCAN_WAIT_CMD;
                        group.Scan.Busy = false;
                        group.Down.Busy = false;
                    }
                    break;

                case PROG_WAIT_CMD: //等待烧录触发
                    if (group.Down.Trigger)
                    {
                        group.Down.Trigger = false;
                        group.Unit[0].DownResult = 4;
                        programer.Step = PROG_START_DELAY;
                    }
                    break;

                case PROG_START_DELAY: //触发延时处理		
                    programer.Timer_DownDelay = GetSysTime() + AutoTiming.DownDelay + 5;
                    programer.Step = PROG_START_TRIGER;
                    break;

                case PROG_START_TRIGER: //触发烧录	
                    if (GetSysTime() > programer.Timer_DownDelay)//烧录延时时间到
                    {
                        programer.Timer_Down_TimeOut = GetSysTime() + AutoTiming.DownTimeOut + 3000;//设置烧录超时
                        programer.Timer_DownDelay = GetSysTime() + 3000;
                        MODBUS_DownloadStart(1, programer.Enable);
                        programer.Step = PROG_CHECK_TRIGER;
                    }
                    break;

                case PROG_CHECK_TRIGER://检测烧录是否开始
                    if (programer.Status == 4)//烧录中
                    {
                        programer.Step = PROG_CHECK_RESULT;
                    }
                    else if (GetSysTime() > programer.Timer_DownDelay)//烧录触发异常
                    {
                        programer.Step = SCAN_WAIT_CMD;
                        if (UserConfig.IsProgrammer)
                        {
                            Result_Handle(groupNum, 1);
                        }
                        else
                        {
                            Result_Handle(groupNum, 2);
                        }
                        group.Down.Busy = false;
                    }
                    break;

                case PROG_CHECK_RESULT: //检测烧录结果		
                    if (programer.Status == 1)//烧录OK
                    {
                        Result_Handle(groupNum, 1);
                        programer.Step = SCAN_WAIT_CMD;
                        group.Down.Busy = false;
                    }
                    else if (programer.Status == 2)//烧录NG
                    {
                        Result_Handle(groupNum, 2);
                        programer.Step = SCAN_WAIT_CMD;
                        group.Down.Busy = false;
                    }
                    else if (GetSysTime() > programer.Timer_Down_TimeOut)
                    {
                        Result_Handle(groupNum, 2);
                        programer.Step = SCAN_WAIT_CMD;
                        group.Down.Busy = false;
                    }
                    if (programer.Step == SCAN_WAIT_CMD)
                    {
                        if (Auto_Flag.Flip)
                        {
                            In_Output.flipO[groupNum].M = false;
                        }
                        else
                        {
                            In_Output.pushSeatO[groupNum].M = true;
                        }
                        AutoTimer.OpenSocket = GetSysTime() + 3000;
                    }
                    break;

                default:
                    programer.Step = SCAN_WAIT_CMD;
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
            int temp;
            int retval = 0;
            retval = 0x1 << groupNum;
            group.Unit[0].DownResult = 4;
            return (byte)retval;
        }

        /// <summary>
        /// 烧录结果检查
        /// </summary>
        /// <param name="unitNum"></param>
        /// <param name="result"></param>
        private void Result_Handle(int triggerIndex, byte result)
        {
            Axis.Group[triggerIndex].Unit[0].DownResult = result;
            if (result == 1 && !Axis.Group[triggerIndex].Scan.Busy)
            {
                g_config.WriteSocketCounter(triggerIndex, result);
            }
            else if (result == 2)
            {
                g_config.WriteSocketCounter(triggerIndex, result);
            }
            if (Auto_Flag.AutoRunBusy)
            {
                ContinueNGCheck(Axis.Group[triggerIndex], 0);
            }
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
            if (Auto_Flag.AutoRunBusy)
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
                if (!group.Scan.Busy)
                {
                    group.Unit[unitN].Counter_NG = 0;
                }
            }
        }

        public struct Programer_Group
        {
            public int Step;                           //烧录步骤 
            public byte Enable;                        //烧录器使能
            public byte Status;                        //烧录状态
            public UInt64 Timer_DownDelay;
            public UInt64 Timer_Down_TimeOut;
        }
    }
}
