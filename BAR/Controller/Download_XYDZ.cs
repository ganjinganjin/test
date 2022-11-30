using BAR.Commonlib;
using BAR.Commonlib.Connectors;
using BAR.Commonlib.Connectors.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using FlashLib4Net;

namespace BAR
{
    public class Download_XYDZ : UserTimer//欣扬电子烧录器
    {
        FlashLib flib = new FlashLib();
        BoardInfo bdinfo;
        List<BoardInfo> bdlist;
        List<Slot> slots;

        public Config g_config = Config.GetInstance();
        public RPCServer rpcServer = RPCServer.GetInstance();

        #region----------------声明步序常量-------------------
        public const int PROG_WAIT_CMD = 0;
        public const int PROG_START_DELAY = 1;
        public const int PROG_START_TRIGER = 2;
        public const int PROG_CHECK_TRIGER = 3;
        public const int PROG_CHECK_RESULT = 4;
        #endregion
        /// <summary>
        /// 同步烧录座数
        /// </summary>
        private const int SyncBurnCnt = 4;
        private bool IsInit;
        private static UInt64 Timer_ReadStateCnt;
        private const UInt16 Timer_ReadStateVal = 200;
        public static Programer_Unit[] programer_Unit = new Programer_Unit[UserConfig.AllScketC];
        public static volatile Programer[] programer = new Programer[UserConfig.ScketGroupC];
        public static UInt64[] Timer_StartDelay = new UInt64[UserConfig.ScketGroupC];

        public Download_XYDZ()
        {
            FlashLib_Init();
        }

        /// <summary>
        /// 初始化欣扬电子烧录器
        /// </summary>
        /// <returns></returns>
        public bool FlashLib_Init()
        {
            int count = flib.QueryBoardCount();
            if (0 == count)//没有找到烧录模组
            {
                return false;
            }
            
            bdlist = flib.QueryBoardInfo();
            if (null == bdlist)//没有找到烧录模组
            {
                return false;
            }
            
            foreach (BoardInfo bd in bdlist)
            {
                bd.isused = true;
                bd.SetAllUsedSlotStatus(SlotStatus.EMPTY);
            }
            
            ProjectInfo prjinfo = flib.QueryPrjInfo();
            if (null == prjinfo)//未加载烧录工程并下载
            {
                return false;
            }
            
            if (ErrorCode.OK != flib.SendStart())//未下载烧录工程
            {
                return false;
            }
            IsInit = true;
            Auto_Flag.BurnOnline = true;
            return IsInit;
        }

        /// <summary>
        /// 设置单个烧录通道 Slot 的启用状态
        /// </summary>
        public void FlashLib_SetSlotStatus(byte Status, int triggerIndex)
        {
            if (bdlist == null)
            {
                return;
            }
            foreach (BoardInfo bd in bdlist)
            {
                if (bd.index == triggerIndex)
                {
                    for (int i = 0; i < SyncBurnCnt; i++)
                    {
                        Slot slot = bd.getSlotById(i + 1);
                        slot.isused = (Status >> i & 0x1) == 1 ? true : false;
                        flib.SetSlotStatus(bd.uuid, slot);
                    }
                }
            }
        }

        public static void Init()
        {
            for (int i = 0; i < UserConfig.ScketGroupC; i++)
            {
                programer[i].Result = 0;
                programer[i].Start = 0;
                programer[i].Enable = 0;
                programer[i].Step = 0;
                programer[i].Finish = false;
                Axis.Group[i].Down.Busy = false;
                Axis.Group[i].Down.Trigger = false;
                for (int j = 0; j < UserConfig.ScketUnitC; j++)
                {
                    Axis.Group[i].Unit[j].DownResult = 0;
                }
                Timer_StartDelay[i] = GetSysTime();
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
                if (!IsInit)
                {
                    FlashLib_Init();
                }
                if (!FlashLib_GetSocketStatus())
                {
                    Auto_Flag.BurnOnline = false;
                    IsInit = false;
                    bdlist = null;
                }
            }
            for (int i = 0; i < UserConfig.ScketGroupC; i++)
            {
                Program_handle(ref programer[i], Axis.Group[i], i);
            }
        }

        /// <summary>
        /// 获取烧录座状态
        /// </summary>
        public bool FlashLib_GetSocketStatus()
        {
            bool falg = false;
            if (bdlist == null)
            {
                return falg;
            }
            foreach (BoardInfo bd in bdlist)
            {
                bdinfo = bd;
                if (ErrorCode.OK == flib.QueryBoardSlotStatus(ref bdinfo))
                {
                    falg = true;
                    slots = bdinfo.slots;

                    if (bdinfo.IsBurnFinished())//模组烧录完成
                    {
                        programer[bd.index].Finish = true;
                    }
                    int retval = 0;
                    foreach (Slot st in slots)//模组烧录座状态
                    {
                        if (st.isused)
                        {
                            if (SlotStatus.BURNING == st.status)
                            {
                                programer[bd.index].Finish = false;
                            }
                            else if (SlotStatus.OK == st.status)
                            {
                                retval = retval | (0x1 << (st.index - bd.index * SyncBurnCnt));
                                programer_Unit[st.index].Result = 1;
                            }
                            else if (SlotStatus.NG == st.status)
                            {
                                programer_Unit[st.index].Result = 2;
                            }
                        }
                    }
                    programer[bd.index].Result = (byte)retval;
                }
            }
            return falg;
        }

        /// <summary>
        /// 组烧录处理
        /// </summary>
        /// <param name="pro"></param>
        /// <param name="group"></param>
        public void Program_handle(ref Programer pro, SocketGroup group, int triggerIndex)
        {
            switch (pro.Step)
            {
                case PROG_WAIT_CMD: //等待触发
                    if (group.Down.Trigger)
                    {
                        pro.Enable = GetEnable(group);
                        FlashLib_SetSlotStatus(pro.Enable, triggerIndex);
                        group.Down.Trigger = false;
                        if (pro.Enable != 0)
                        {
                            pro.Step = PROG_START_DELAY;
                        }
                        else
                        {
                            group.Down.Busy = false;
                        }
                    }
                    break;

                case PROG_START_DELAY: //触发延时处理		
                    if (Auto_Flag.AutoRunBusy)//自动模式
                    {
                        pro.Timer_DownDelay = GetSysTime() + AutoTiming.DownDelay + 5;
                        pro.Step = PROG_START_TRIGER;
                    }
                    else
                    {
                        pro.Timer_DownDelay = GetSysTime() + 5;
                        pro.Step = PROG_START_TRIGER;
                    }
                    break;

                case PROG_START_TRIGER: //触发烧录	
                    if (GetSysTime() > pro.Timer_DownDelay)//烧录延时时间到
                    {
                        if (bdlist != null)
                        {
                            foreach (BoardInfo bd in bdlist)
                            {
                                if (bd.index == triggerIndex)
                                {
                                    var a = flib.SendBurning(bd);
                                }
                            }
                        }
                        pro.Finish = false;
                        pro.Timer_Down_TimeOut = GetSysTime() + AutoTiming.DownTimeOut;//设置烧录超时				
                        pro.Step = PROG_CHECK_TRIGER;
                    }
                    break;

                case PROG_CHECK_TRIGER: //检测烧录是否结束					
                    if (pro.Finish)
                    {
                        pro.Step = PROG_CHECK_RESULT;
                    }
                    else if (GetSysTime() > pro.Timer_Down_TimeOut)
                    {
                        if (UserConfig.IsProgrammer)
                        {
                            pro.Result = 0xFF;
                        }
                        else
                        {
                            pro.Result = 0x00;
                        }
                        pro.Step = PROG_CHECK_RESULT;
                    }
                    break;

                case PROG_CHECK_RESULT: //检测烧录结果					                  
                    ResultCheck(group, pro.Result);
                    group.Down.Busy = false;
                    pro.Step = PROG_WAIT_CMD;
                    break;

                default:
                    //pro.Step = PROG_WAIT_CMD;
                    break;
            }
        }

        /// <summary>
        /// 获取使能
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        private byte GetEnable(SocketGroup group)
        {
            int retval = 0;
            for (int i = 0; i < UserConfig.ScketUnitC; i++)
            {
                if (ChannelEnableCheck(group, i))
                {
                    group.Unit[i].Flag_NewIC = false;
                    group.Unit[i].DownResult = 4;
                    retval = retval | (0x1 << i);
                }
            }
            return (byte)retval;
        }

        /// <summary>
        /// 烧录结果检查
        /// </summary>
        /// <param name="group"></param>
        /// <param name="result"></param>
        private void ResultCheck(SocketGroup group, byte result)
        {
            for (int i = 0; i < UserConfig.ScketUnitC; i++)
            {
                if (ChannelEnableCheck(group, i))
                {
                    if ((result >> i & 0x1) == 1)
                    {
                        group.Unit[i].DownResult = 1;
                    }
                    else
                    {
                        group.Unit[i].DownResult = 2;
                    }
                    int ind = group.GroupNum * UserConfig.ScketUnitC + i;
                    g_config.WriteSocketCounter(ind, group.Unit[i].DownResult);
                    if (Auto_Flag.AutoRunBusy)
                    {
                        ContinueNGCheck(group, i);
                    }
                }
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

        public struct Programer
        {
            public int Step;            //烧录步骤    
            public bool Burning;         //烧录器烧录中信号
            public bool Finish;         //烧录器完成信号
            public byte Start;          //烧录器准备信号
            public byte Result;         //烧录器结果
            public byte Enable;        //烧录器使能

            public UInt64 Timer_DownDelay;
            public UInt64 Timer_Down_TimeOut;
        }

        public struct Programer_Unit
        {
            public byte Status;                                 //烧录状态
            public byte Result;                                 //烧录结果
        }
    }
}
