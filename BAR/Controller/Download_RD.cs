using BAR.Commonlib;
using BAR.Commonlib.Connectors.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR
{
    public class Download_RD : UserTimer
    {
        public Config g_config = Config.GetInstance();

        #region----------------声明步序常量-------------------
        public const int PROG_WAIT_CMD = 0;
        public const int PROG_WAIT_FLIP_O1 = 1;
        public const int PROG_START_DELAY = 2;
        public const int PROG_CHECK_WORK = 3;
        public const int PROG_START_TRIGER = 4;
        public const int PROG_CHECK_TRIGER = 5;
        public const int PROG_CHECK_RESULT = 6;
        public const int PROG_WAIT_PUSH_O = 7;
        public const int PROG_WAIT_FLIP_O2 = 8;
        public const int PROG_WAIT_ALARM = 9;
        public const int PROG_WAIT_RESET = 10;
        #endregion
        private const UInt16 Timer_ReadStateVal = 200;
        public const int UNIT_AMOUNT = 32;
        public const int PORT_NUM = 4;
        public static Programer_Unit[] programer_Unit = new Programer_Unit[UNIT_AMOUNT];
        public static Programer_Group[] programer_Group = new Programer_Group[UNIT_AMOUNT];
        public static int[] SendBuff;
        private static UInt64[] Timer_ReadState = new UInt64[PORT_NUM];

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

            SendBuff = new int[UserConfig.ScketGroupC * UserConfig.ScketUnitC];
        }

        /// <summary>
        /// 网口读烧录座座状态
        /// </summary>
        public void NETWORK_ReadDownloadStatus_Result()
        {
            string strBuffer;
            byte[] TxBuffer;
            int temp = UserConfig.ScketGroupC / PORT_NUM;
            for (int i = 0; i < PORT_NUM; i++)
            {
                for (int j = 0; j < temp; j++) 
                {
                    int index = i * temp + j;
                    if (Axis.Group[index].Down.Busy)
                    {
                        if (UserTimer.GetSysTime() >= Timer_ReadState[i])
                        {
                            Timer_ReadState[i] = UserTimer.GetSysTime() + Timer_ReadStateVal;
                            strBuffer = string.Format("Site{0:d},Read,END", i + 1);
                            TxBuffer = Encoding.ASCII.GetBytes(strBuffer);
                            Download.proxy.SendCmd($"ReadDownloadStatus_Result_{i + 1}", TxBuffer, 19, false, 400);
                        }
                        break;
                    }
                }
                
            }
        }

        public static void NETWORK_DownloadStart(int Site, int Status)//烧录启动程序
        {
            string strBuffer;
            byte[] TxBuffer;

            strBuffer = string.Format("Site{0:d},Start,{1:d},END", Site, Status);
            TxBuffer = Encoding.ASCII.GetBytes(strBuffer);
            Download.proxy.SendCmd("DownloadStart_" + Site, TxBuffer, 5, true, 100);
        }

        /// <summary>
        /// 烧录器烧录处理
        /// </summary>
        public void Program()
        {
            for (int i = 0; i < UserConfig.ScketGroupC; i++)
            {
                Program_Group_Handle(ref programer_Group[i], Axis.Group[i], i);
            }

            for (int i = 0; i < UNIT_AMOUNT; i++)
            {
                Programer_Unit_handle(ref programer_Unit[i], i);
            }

            NETWORK_ReadDownloadStatus_Result();
        }

        /// <summary>
        /// 组烧录处理
        /// </summary>
        /// <param name="programer"></param>
        /// <param name="group"></param>
        private void Program_Group_Handle(ref Programer_Group programer, SocketGroup group, int groupNum)
        {
            int temp, add;
            switch (programer.Step)
            {
                case PROG_WAIT_CMD: //等待触发
                    if (group.Down.Trigger)
                    {
                        group.Down.Trigger = false;
                        programer.Step = PROG_CHECK_TRIGER;
                    }
                    break;

                case PROG_CHECK_TRIGER: //检测触发
                    programer.Enable = GetEnable(group, groupNum);
                    for (int i = 0; i < UserConfig.ScketUnitC; i++)
                    {
                        temp = groupNum * UserConfig.ScketUnitC + i;
                        programer_Unit[temp].Work = programer_Unit[temp].Trigger = programer_Unit[temp].Enable;
                    }
                    programer.Step = PROG_START_TRIGER;
                    break;

                case PROG_START_TRIGER: //触发烧录
                    if (Channel_Work_Check(groupNum))
                    {
                        add = groupNum * UserConfig.ScketUnitC / 8 + 1;
                        NETWORK_DownloadStart(add, programer.Enable);
                        for (int i = 0; i < UserConfig.ScketUnitC; i++)
                        {
                            temp = groupNum * UserConfig.ScketUnitC + i;
                            programer_Unit[temp].Work = programer_Unit[temp].Enable;
                        }
                        programer.Step = PROG_CHECK_RESULT;
                    }
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
                        In_Output.flipO[triggerIndex].M = true;
                        programer.Timer_DownDelay = GetSysTime() + 4000;
                        programer.Step = PROG_WAIT_FLIP_O1;
                    }
                    break;

                case PROG_WAIT_FLIP_O1: //等待推座到位
                    if (In_Output.flipLimitI[triggerIndex].M == true)//感应到位
                    {
                        In_Output.pushSeatO[triggerIndex].M = true;
                        programer.Timer_DownDelay = GetSysTime() + AutoTiming.DownDelay + 5;
                        programer.Step = PROG_WAIT_RESET;
                    }
                    else if (GetSysTime() > programer.Timer_DownDelay)//感应到位超时
                    {
                        BAR._ToolTipDlg.WriteToolTipStr("推座[" + (triggerIndex + 1).ToString() + "]气缸工位感应超时;[处理异常->确定->继续]");
                        BAR.ShowToolTipWnd(true);
                        programer.BackStep = programer.Step;
                        programer.Step = PROG_WAIT_ALARM;
                    }
                    break;

                case PROG_WAIT_RESET: //烧录复位
                    if (GetSysTime() > programer.Timer_DownDelay)//烧录延时时间到
                    {
                        In_Output.resetScketO[triggerIndex].M = true;
                        programer.Timer_DownDelay = GetSysTime() + 1000;
                        programer.Step = PROG_START_TRIGER;
                    }
                    break;

                case PROG_START_TRIGER: //触发烧录	
                    if (GetSysTime() > programer.Timer_DownDelay && In_Output.resetScketO[triggerIndex].M)//烧录延时时间到
                    {
                        In_Output.resetScketO[triggerIndex].M = false;
                    }
                    if (GetSysTime() > programer.Timer_DownDelay + 2000)
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
                            programer.Timer_DownDelay = GetSysTime() + 20000;
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
                            programer.Step = PROG_WAIT_PUSH_O;
                            In_Output.pushSeatO[triggerIndex].M = false;
                            programer.Timer_DownDelay = GetSysTime() + 3000;
                        }
                    }
                    else if (programer.Status == 4)//烧录中
                    {
                        programer.Timer_Down_TimeOut = GetSysTime() + AutoTiming.DownTimeOut;//设置烧录超时
                        programer.Step = PROG_CHECK_RESULT;
                    }
                    else if (GetSysTime() > programer.Timer_DownDelay)//烧录触发异常
                    {
                        if (UserConfig.IsProgrammer)
                        {
                            Result_Handle(triggerIndex, 1);
                        }
                        else
                        {
                            Result_Handle(triggerIndex, 2);
                        }
                        programer.Step = PROG_WAIT_PUSH_O;
                        In_Output.pushSeatO[triggerIndex].M = false;
                        programer.Timer_DownDelay = GetSysTime() + 3000;
                    }
                    break;

                case PROG_CHECK_RESULT: //检测烧录结果		
                    if (programer.Status == 1)//OK
                    {
                        Result_Handle(triggerIndex, 1);
                        programer.Step = PROG_WAIT_PUSH_O;
                        In_Output.pushSeatO[triggerIndex].M = false;
                        programer.Timer_DownDelay = GetSysTime() + 3000;
                    }
                    else if (programer.Status == 2)//NG
                    {
                        Result_Handle(triggerIndex, 2);
                        programer.Step = PROG_WAIT_PUSH_O;
                        In_Output.pushSeatO[triggerIndex].M = false;
                        programer.Timer_DownDelay = GetSysTime() + 3000;
                    }
                    else if (GetSysTime() > programer.Timer_Down_TimeOut)
                    {
                        Result_Handle(triggerIndex, 2);
                        programer.Step = PROG_WAIT_PUSH_O;
                        In_Output.pushSeatO[triggerIndex].M = false;
                        programer.Timer_DownDelay = GetSysTime() + 3000;
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
