using BAR.Commonlib;
using BAR.Commonlib.Connectors.Protocols;
using CCWin.Win32.Const;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BAR
{
    public class Download_DP : UserTimer
    {
        public Config g_config = Config.GetInstance();
        public Act g_act = Act.GetInstance();
        private static readonly object _objInit_DedinetlinkLock = new object();
        private static readonly object _objGetProjectInfoLock = new object();
        #region----------------声明步序常量-------------------
        public const int PROG_WAIT_CMD = 0;
        public const int PROG_START_DELAY = 1;
        public const int PROG_START_TRIGER = 2;
        public const int PROG_CHECK_TRIGER = 3;
        public const int PROG_CHECK_RESULT = 4;
        #endregion
        private static UInt64 Timer_ReadStateCnt;
        private const UInt16 Timer_ReadStateVal = 600;
        public static Programer_Group[] programer = new Programer_Group[UserConfig.ScketGroupC];
        private static IntPtr m_linker, value;
        private static int P_Index, S_Index = 0;
        private byte S_Result;
        private static DP_API.ConnCallback connCallback;
        private static DP_API.ProgressCallback progressCallback;
        private string[] NG_Info = new string[] { "Detect Chip Error", "Error", "error", "fail" , "config file not founded", "select no chip", "no select file", "out of the counter",
            "ignore the start signal", "the project has be terminated", "over current", "to many bad block", "invalid", "project is running", "no batch setting", "can't create project",
            "have not read memory","no select project","no run project","no set the register value"," prohibitted","can't find operation dll","no enough storage space",
            "sdcard not plug in","no socket","out of user counter","invalid","Serial number conflict","no operation paramters","no image parameters","Operation Canceled",
            "Run Out of unique key","Not Enough Valid Block"};
        private int[] Index_Result_16 = new int[] { 0, 2, 4, 6, 8, 10, 12, 14, 1, 3, 5, 7, 9, 11, 13, 15 };
        private int[] Index_Enable_16 = new int[] { 0, 8, 1, 9, 2, 10, 3, 11, 4, 12, 5, 13, 6, 14, 7, 15 };
        private int[,] Index_Result_4 = new int[4, 4] { { 0, 2, 1, 3 }, { 4, 6, 5, 7 }, { 8, 10, 9, 11 }, { 12, 14, 13, 15 } };
        private int[] Index_Enable_4 = new int[] {0, 2, 1, 3};

        public Download_DP()
        {
            for (int i = 0; i < UserConfig.ScketGroupC; i++)
            {
                programer[i] = new Programer_Group();
            }
        }

        public static void Init()
        {
            for (int i = 0; i < UserConfig.ScketGroupC; i++)
            {
                programer[i].Enable = 0;
                programer[i].Step = 0;
                Axis.Group[i].Down.Busy = false;
                Axis.Group[i].Down.Trigger = false;
                for (int j = 0; j < UserConfig.ScketUnitC; j++)
                {
                    Axis.Group[i].Unit[j].DownResult = 0;
                    programer[i].Unit[j].Step = 0;
                    programer[i].Unit[j].Enable = false;
                    programer[i].Unit[j].Trigger = false;
                }
            }
        }

        public void Init_Dedinetlink()
        {
            Task t = Task.Run(() =>
            {
                lock (_objInit_DedinetlinkLock)
                {
                    try
                    {
                        m_linker = DP_API.CreateInstance();

                        var ret = DP_API.ExLogin("127.0.0.1", "8989", m_linker, 0);
                        if (ret != 0)
                        {
                            return;
                        }
                        connCallback = CCB;
                        progressCallback = PCB;
                        DP_API.EnableWaitStartMessage(true);
                        DP_API.SetConnCallBack(connCallback);
                        DP_API.SetCallBack(progressCallback);
                        Auto_Flag.BurnOnline = true;
                    }
                    catch (Exception)
                    {

                    }

                }

            });

        }
        
        private void CCB(IntPtr value)
        {
            /// ret is connect event message
            string ret = Marshal.PtrToStringAnsi(value);
            Auto_Flag.BurnOnline = false;
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, ret, "DP_Link");
        }
        
        private void PCB(IntPtr value)
        {
            /// _ret is programmer message
            string ret = string.Copy(Marshal.PtrToStringAnsi(value));
            g_act.RecordProgrammerInfo(ret);
            if (ret.Contains("PGMR"))
            {
                string[] strArray = ret.Split(new char[2] { ',', '-' });
                var temp = UserConfig.ScketUnitC;
                var tempP_Index = Convert.ToInt32(strArray[0].Substring(strArray[0].Length - 1, 1)) - 1;
                var tmepS_Index = Convert.ToInt32(strArray[1]) - 1;
                if (temp == 4)
                {
                    P_Index = (tmepS_Index / temp) + UserConfig.ScketUnitC * tempP_Index;
                    S_Index = Index_Enable_4[tmepS_Index % UserConfig.ScketUnitC];//置换烧录座烧录数据
                }
                else
                {
                    P_Index = tempP_Index;
                    S_Index = UserConfig.ScketUnitC == 16 ? Index_Enable_16[tmepS_Index] : Convert.ToInt32(strArray[1]) - 1;//置换烧录座烧录数据
                }

                if (strArray[3].Contains("Contact testing") || strArray[3].Contains("Program Chip"))//烧录中
                {
                    S_Result = 1;
                    programer[P_Index].Unit[S_Index].Status = S_Result;
                }
                else if(strArray[3].Contains("Operation Complete"))//OK
                {
                    S_Result = 2;
                    programer[P_Index].Unit[S_Index].Status = S_Result;
                }
                else//NG
                {
                    foreach (string str in NG_Info)//遍历NG字符串
                    {
                        if (strArray[3].Contains(str))
                        {
                            S_Result = 3;
                            programer[P_Index].Unit[S_Index].Status = S_Result;
                        }
                    }
                }
            }
        }

        private void GetProjectInfo()
        {
            try
            {
                Task t = Task.Run(() => {
                    lock (_objGetProjectInfoLock)
                    {
                        int _cLength = 0;
                        IntPtr value = DP_API.ExGetProjectInfo(m_linker, ref _cLength);
                        string ret = Marshal.PtrToStringAnsi(value, _cLength);
                        //g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, ret, "DP_Link");
                        string str = DP_API.ExGetChipChecksum().ToString("X");
                        Mes.Checksum_Chip = "0x" + str.Substring(str.Length - 8);

                        string[] strArray = ret.Split('{', ';', '}');
                        foreach (var item in strArray)
                        {
                            if (item.Contains("Ready"))
                            {
                                string[] temp = item.Split(':');
                                Auto_Flag.BurnReady = temp[1].Contains("Yes") ? true : false;
                            }
                            if (item.Contains("Chip"))
                            {
                                string[] temp = item.Split(':');
                                Mes.Device = temp[1];
                            }
                            if (item.Contains("Checksum"))
                            {
                                string[] temp = item.Split(':');
                                Mes.Checksum_File = "0x" + temp[1];
                            }
                            if (item.Contains("Brand"))
                            {
                                string[] temp = item.Split(':');
                                Mes.Brand = temp[1];
                            }
                        }
                        
                    }
                });
            }
            catch (Exception ex)
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, ex.ToString(), "DP_Link");
            }
            
        }

        private void MODBUS_DownloadStart(int groupNum, ushort enabled)//烧录启动程序
        {
            groupNum = UserConfig.ScketUnitC == 4 ? groupNum / UserConfig.ScketUnitC : groupNum;
            DP_API.ExDediNet_MaskStart(groupNum, enabled);
            g_act.RecordProgrammerInfo(string.Format("P{0:d} Mask:{1:d} Start", groupNum + 1, enabled), "Send");
        }

        /// <summary>
        /// 烧录器烧录处理
        /// </summary>
        public void Program()
        {
            if (UserTimer.GetSysTime() >= Timer_ReadStateCnt)
            {
                Timer_ReadStateCnt = UserTimer.GetSysTime() + Timer_ReadStateVal;
                if (!Auto_Flag.BurnOnline)
                {
                    Init_Dedinetlink();
                }
                else if (!Auto_Flag.BurnReady)
                {
                    GetProjectInfo();
                }

            }
            for (int i = 0; i < UserConfig.ScketGroupC; i++)
            {
                Program_Group_Handle(ref programer[i], Axis.Group[i], i);
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
                    if (Auto_Flag.AutoRunBusy)//自动模式
                    {
                        programer.Timer_DownDelay = GetSysTime() + AutoTiming.DownDelay + 5;
                        programer.Step = PROG_START_TRIGER;
                    }
                    else
                    {
                        programer.Timer_DownDelay = GetSysTime() + 5;
                        programer.Step = PROG_START_TRIGER;
                    }
                    break;

                case PROG_START_TRIGER: //触发烧录	
                    if (GetSysTime() > programer.Timer_DownDelay)//烧录延时时间到
                    {
                        programer.Enable = GetEnable(group, groupNum);
                        if (UserConfig.ScketUnitC == 8 && UserTask.ProgrammerType == GlobConstData.Programmer_DP)
                        {
                            MODBUS_DownloadStart(groupNum, programer.Enable);
                        }
                        else
                        {
                            MODBUS_DownloadStart(groupNum, programer.Enable);
                        }
                        programer.Step = PROG_CHECK_TRIGER;
                    }
                    break;

                case PROG_CHECK_TRIGER: //检测触发
                    for (int i = 0; i < UserConfig.ScketUnitC; i++)
                    {
                        programer.Unit[i].Work = programer.Unit[i].Trigger = programer.Unit[i].Enable;
                    }
                    programer.Step = PROG_CHECK_RESULT;
                    break;

                case PROG_CHECK_RESULT: //检测烧录结果
                    for (int i = 0; i < UserConfig.ScketUnitC; i++)
                    {
                        Programer_Unit_handle(group, ref programer.Unit[i], i);
                    }
                    if (Channel_Work_Check(groupNum))
                    {
                        In_Output.pushSeatO[groupNum].M = true;
                        In_Output.pushSeatO[groupNum].RM = true; 
                        AutoTimer.OpenSocket = GetSysTime() + 3000;
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
        /// <param name="unit"></param>
        /// <param name="triggerIndex"></param>
        private void Programer_Unit_handle(SocketGroup group, ref Programer_Unit unit, int triggerIndex)
        {
            switch (unit.Step)
            {
                case PROG_WAIT_CMD: //等待触发
                    if (unit.Trigger)
                    {
                        unit.Trigger = false;
                        unit.Timer_Down_TimeOut = GetSysTime() + AutoTiming.DownTimeOut + 3000;//设置烧录超时
                        unit.Timer_DownDelay = GetSysTime() + 20000;
                        unit.Step = PROG_CHECK_TRIGER;
                    }
                    break;

                case PROG_CHECK_TRIGER://检测烧录是否开始
                    if (unit.Status == 1)//烧录中
                    {
                        unit.Step = PROG_CHECK_RESULT;
                    }
                    else if (GetSysTime() > unit.Timer_DownDelay)//烧录触发异常
                    {
                        unit.Step = PROG_WAIT_CMD;
                        if (UserConfig.IsProgrammer)
                        {
                            Result_Handle(group, triggerIndex, 1);
                        }
                        else
                        {
                            Result_Handle(group, triggerIndex, 2);
                        }
                        unit.Work = false;
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "烧录触发异常", "DP_Link");
                    }
                    break;

                case PROG_CHECK_RESULT: //检测烧录结果		
                    if (unit.Status == 2)//烧录OK
                    {
                        Result_Handle(group, triggerIndex, 1);
                        unit.Step = PROG_WAIT_CMD;
                        unit.Work = false;
                    }
                    else if (unit.Status == 3)//烧录NG
                    {
                        Result_Handle(group, triggerIndex, 2);
                        unit.Step = PROG_WAIT_CMD;
                        unit.Work = false;
                    }
                    else if (GetSysTime() > unit.Timer_Down_TimeOut)
                    {
                        Result_Handle(group, triggerIndex, 2);
                        unit.Step = PROG_WAIT_CMD;
                        unit.Work = false;
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "烧录超时", "DP_Link");
                    }
                    break;

                default:
                    unit.Step = PROG_WAIT_CMD;
                    unit.Work = false;
                    break;
            }
        }

        /// <summary>
        /// 获取使能
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        private ushort GetEnable(SocketGroup group, int groupNum)
        {
            int retval = 0;
            int temp = UserConfig.ScketUnitC;
            for (int i = 0; i < UserConfig.ScketUnitC; i++)
            {
                programer[groupNum].Unit[i].Step = 0;
                programer[groupNum].Unit[i].Enable = false;
                programer[groupNum].Unit[i].Work = false;
                programer[groupNum].Unit[i].Trigger = false;
                if (ChannelEnableCheck(group, i))
                {
                    group.Unit[i].Flag_NewIC = false;
                    group.Unit[i].DownResult = 4;
                    programer[groupNum].Unit[i].Enable = true;
                    if (temp == 4)
                    {
                        retval = retval | (0x1 << Index_Result_4[groupNum % temp, i]);//置换烧录座触发位
                    }
                    else
                    {
                        retval = retval | (0x1 << (UserConfig.ScketUnitC == 16 ? Index_Result_16[i] : i));//置换烧录座触发位
                    }
                }
            }
            return (ushort)retval;
        }

        bool Channel_Work_Check(int groupNum)
        {
            for (int i = 0; i < UserConfig.ScketUnitC; i++)
            {
                if (programer[groupNum].Unit[i].Work)
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
        private void Result_Handle(SocketGroup group, int triggerIndex, byte result)
        {
            int ind = group.GroupNum * UserConfig.ScketUnitC + triggerIndex;
            group.Unit[triggerIndex].DownResult = result;
            g_config.WriteSocketCounter(ind, result);
            if (Auto_Flag.AutoRunBusy)
            {
                ContinueNGCheck(group, triggerIndex);
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

        private string LongToHexString(long Num)//数字转换16进制
        {
            string str = Convert.ToString(Num, 16);
            string Msg = "0x" + (str.Length == 1 ? "0" + str : str);//转换成指定格式
            return Msg;
        }

        public struct Programer_Unit
        {
            public int Step;                                    //座子烧录步骤
            public bool Enable;                                 //烧录使能
            public bool Trigger;                                //烧录触发
            public bool Work;                                   //烧录工作
            public byte Status;                                 //烧录状态
            public UInt64 Timer_DownDelay;                      //烧录延时时间
            public UInt64 Timer_Down_TimeOut;                   //烧录超时时间
        }

        public class Programer_Group
        {
            public int Step;            //烧录步骤 
            public ushort Enable;        //烧录器使能

            public UInt64 Timer_DownDelay;
            public UInt64 Timer_Down_TimeOut;

            public Programer_Unit[] Unit = new Programer_Unit[UserConfig.ScketUnitC];
        }
    }
}
