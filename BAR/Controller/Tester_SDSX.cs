using BAR.Commonlib;
using BAR.Commonlib.Connectors;
using BAR.Commonlib.Connectors.Protocols;
using BAR.Communicators;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BAR
{
    public class Tester_SDSX : UserTimer
    {
        public Config g_config = Config.GetInstance();
        public static Act g_act = Act.GetInstance();
        public RPCServer rpcServer = RPCServer.GetInstance();


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
        public const int PROG_POWER_DELAY = 11;
        #endregion
        public static Programer_Unit[] programer_Unit;
        public static Programer_Group[] programer_Group;
        public static TesterProxy proxy;

        public Tester_SDSX()
        {
            programer_Group = new Programer_Group[UserConfig.ScketGroupC];
            programer_Unit = new Programer_Unit[UserConfig.AllScketC];
            ConnectionSeriaPort();
        }

        /// <summary>
        /// 连接串口
        /// </summary>
        public void ConnectionSeriaPort()
        {
            try
            {
                Auto_Flag.BurnOnline = false;
                proxy = new TesterProxy(0);
                proxy.OpenConnection(g_config.ArrStrutCom[3]);
                proxy.ProxyNotifyEvent += OnMsgEvenHandle;
                Auto_Flag.BurnOnline = true;
            }
            catch (Exception)
            {

            }
            
        }

        /// <summary>
        /// 断开串口
        /// </summary>
        public static void DisconnectionSeriaPort()
        {
            Auto_Flag.BurnOnline = false;
            proxy.CloseConnection();
            proxy.ProxyNotifyEvent -= OnMsgEvenHandle;
            proxy = null;
            GC.Collect();
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

            for (int i = 0; i < UserConfig.AllScketC; i++)
            {
                programer_Unit[i].Step = 0;
                programer_Unit[i].Enable = false;
                programer_Unit[i].Trigger = false;
            }
        }

        private void TesterStart(int add)//启动
        {
            string str = "@SOT";
            if (proxy != null)
            {
                proxy.CommandEnQueue(str);
                g_act.RecordProgrammerInfo("Site: "+ add + str,"Send");
            }
        }

        private static void TesterTimeOut(int add)//超时
        {
            string str = "@TIME_OUT +";
            if (proxy != null)
            {
                proxy.CommandEnQueue(str);
                g_act.RecordProgrammerInfo("Site: " + add + str, "TimeOut");
            }
        }

        private static void OnMsgEvenHandle(int index, string frame)
        {
            try
            {
                string[] Buffer = new string[UserConfig.AllScketC];
                for (int i = 0; i < UserConfig.AllScketC; i++)
                {
                    Buffer[i] = frame.Substring(2 + i, 1);
                }
                g_act.RecordProgrammerInfo(frame);

                for (int i = 0; i < UserConfig.AllScketC; i++)
                {
                    if (programer_Unit[i].Status == 1)
                    {
                        if (Buffer[i] == "1")//OK
                        {
                            programer_Unit[i].Status = 2;
                            programer_Unit[i].Result = 1;
                        }
                        else if (Buffer[i] == "2")//NG
                        {
                            programer_Unit[i].Status = 2;
                            programer_Unit[i].Result = 2;
                        }
                        else if (Buffer[i] == "3")//Error
                        {
                            programer_Unit[i].Status = 2;
                            programer_Unit[i].Result = 64;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, ex.Message, "Error");
                MessageBox.Show(ex.Message, "TesterOnMsgEvenHandle");
            }

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

            for (int i = 0; i < UserConfig.AllScketC; i++)
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
                            int add = groupNum + 1;
                            TesterStart((byte)add);
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
                            int add = groupNum + 1;
                            TesterStart((byte)add);
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
                    programer.Status = 1;
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
                        Result_Handle(triggerIndex, programer.Result);
                        programer.Step = PROG_PUSH_RESET;
                    }
                    else if (GetSysTime() > programer.Timer_Down_TimeOut)
                    {
                        Result_Handle(triggerIndex, 2);
                        programer.Status = 2;
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
                ErrorCheck(Axis.Group[groupNum], unitcNum);
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

        /// <summary>
        /// 烧录错误检查
        /// </summary>
        /// <param name="group"></param>
        /// <param name="unitN"></param>
        private void ErrorCheck(SocketGroup group, int unitN)
        {
            if (group.Unit[unitN].DownResult == 64)//Error
            {
                group.Flag_Error = true;
                group.Unit[unitN].Flag_Error = true;
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
    }
}
