using BAR.Commonlib;
using BAR.Commonlib.Connectors;
using BAR.Commonlib.Connectors.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace BAR
{
    public class Download_SFLY : UserTimer//硕飞烧录器
    {
        public const short RESULT_OK = 0;
        public const short RESULT_ERR_MCP = 1;
        public const short RESULT_ERR_MSG = 2;
        public const short RESULT_ERR_INIT = 3;
        public const short RESULT_ERR_PARAM = 4;
        public const short RESULT_ERR_BUSY = 5;

        public const byte FLAG_USBCON = 0x01;       // bit0, USB已连接
        public const byte FLAG_RUN    = 0x02;       // bit1, 已启动运行(加载&运行项目)
        public const byte FLAG_ERROR  = 0x08;       // bit3, 运行错误
        public const byte FLAG_FAIL   = 0x20;       // bit5, 烧录失败
        public const byte FLAG_PASS   = 0x40;       // bit6, 烧录成功
        public const byte FLAG_BUSY   = 0x80;       // bit7, 正在烧录

        public const byte STATUS_USB_DISCON	= 0x00;                                   // USB未连接
        public const byte STATUS_USB_CON    = (FLAG_USBCON)	;                         // USB已连接(未启动运行)
        public const byte STATUS_READY      = (FLAG_USBCON | FLAG_RUN);               // 等待开始烧录
        public const byte STATUS_BUSY       = (FLAG_USBCON | FLAG_RUN | FLAG_BUSY);   // 烧录中...
        public const byte STATUS_PASS       = (FLAG_USBCON | FLAG_RUN | FLAG_PASS);   // 烧录成功
        public const byte STATUS_FAIL       = (FLAG_USBCON | FLAG_RUN | FLAG_FAIL);	  // 烧录失败


        [DllImport("rmc.dll")]
        public static extern short RMC_Init();
        [DllImport("rmc.dll")]
        public static extern short RMC_Start(int iSocket);
        [DllImport("rmc.dll")]
        public static extern short RMC_GetSocketStatus(byte[] pStatus);
        public Config g_config = Config.GetInstance();
        public RPCServer rpcServer = RPCServer.GetInstance();

        #region----------------声明步序常量-------------------
        public const int PROG_WAIT_CMD = 0;
        public const int PROG_START_DELAY = 1;
        public const int PROG_START_TRIGER = 2;
        public const int PROG_CHECK_TRIGER = 3;
        public const int PROG_CHECK_RESULT = 4;
        #endregion
        private static UInt64 Timer_ReadStateCnt;
        private const UInt16 Timer_ReadStateVal = 200;
        public const int UNIT_AMOUNT = 32;
        private static byte[] Status = new byte[UNIT_AMOUNT];
        public static Programer_Unit[] programer_Unit = new Programer_Unit[UNIT_AMOUNT];
        public static Programer_Group[] programer_Group = new Programer_Group[UNIT_AMOUNT];

        public Download_SFLY()
        {
            RMC_Init();
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

        /// <summary>
        /// 烧录器烧录处理
        /// </summary>
        public void Program()
        {
            if (GetSysTime() >= Timer_ReadStateCnt)
            {
                Timer_ReadStateCnt = GetSysTime() + Timer_ReadStateVal;
                Auto_Flag.BurnOnline = RMC_GetSocketStatus(Status) == RESULT_OK;
                if (!Auto_Flag.BurnOnline)
                {
                    RMC_Init();
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
                        int add = groupNum * UserConfig.ScketUnitC / 8 + 1;
                        programer.Step = PROG_CHECK_TRIGER;
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
                        RMC_Start(triggerIndex);
                        programer.Trigger = false;
                        programer.Timer_Down_TimeOut = GetSysTime() + AutoTiming.DownTimeOut + 3000;//设置烧录超时
                        programer.Timer_DownDelay = GetSysTime() + 3000;
                        programer.Step = PROG_CHECK_TRIGER;
                    }
                    break;

                case PROG_CHECK_TRIGER://检测烧录是否开始
                    if (Status[triggerIndex] == STATUS_BUSY)//烧录中
                    {
                        programer.Step = PROG_CHECK_RESULT;
                    }
                    else if (GetSysTime() > programer.Timer_DownDelay)//烧录触发异常
                    {
                        programer.Step = PROG_WAIT_CMD;
                        if (UserConfig.IsProgrammer)
                        {
                            Result_Handle(triggerIndex, 1);
                        }
                        else
                        {
                            Result_Handle(triggerIndex, 2);
                        }
                        programer.Work = false;
                    }
                    break;

                case PROG_CHECK_RESULT: //检测烧录结果		
                    if (Status[triggerIndex] != STATUS_BUSY)//烧录完成
                    {
                        if (Status[triggerIndex] == STATUS_PASS)//OK
                        {
                            Result_Handle(triggerIndex, 1);
                        }
                        else//NG
                        {
                            Result_Handle(triggerIndex, 2);
                        }
                        programer.Step = PROG_WAIT_CMD;
                        programer.Work = false;
                    }
                    else if (GetSysTime() > programer.Timer_Down_TimeOut)
                    {
                        Result_Handle(triggerIndex, 2);
                        programer.Step = PROG_WAIT_CMD;
                        programer.Work = false;
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
