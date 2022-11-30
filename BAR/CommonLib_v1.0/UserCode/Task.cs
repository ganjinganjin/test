using BAR.Commonlib;
using System;
using System.Diagnostics;
using PLC;
using BAR.CommonLib_v1._0;

namespace BAR
{
	public class UserTask : Axis
	{
		#region----------------声明-------------------
		/// <summary>
		/// IO触发飞达
		/// </summary>
		public static bool FeederIO = false;    //IO触发飞达
		/// <summary>
		/// 转移方式
		/// </summary>
		public static ushort ShiftWay;          //转移方式
		/// <summary>
		/// 烧录器类型
		/// </summary>
		public static int ProgrammerType;       //烧录器类型
        /// <summary>
		/// 外挂烧录器通讯协议
		/// </summary>
		public static int Protocol_WG;
        /// <summary>
        /// 吸笔类型
        /// </summary>
        public static int PenType;              //吸笔类型
		/// <summary>
		/// 烧录座放IC单元号
		/// </summary>
		public static int LIC_UnitN = 0;        //烧录座放IC单元号
		/// <summary>
		/// 烧录座放IC组号
		/// </summary>
		public static int LIC_GroupN = 0;       //烧录座放IC组号
        /// <summary>
		/// 烧录座同步吸笔号
		/// </summary>
		public static int SyncPenN = 0;
		/// <summary>
		/// 烧录座取IC单元号
		/// </summary>
		public static int TIC_UnitN = 0;        //烧录座取IC单元号
		/// <summary>
		/// 烧录座取IC组号
		/// </summary>
		public static int TIC_GroupN = 0;       //烧录座取IC组号
		/// <summary>
		/// 飞达取IC号
		/// </summary>
		public static int TIC_FeederN = 0;      //飞达取IC号
		/// <summary>
		/// 料管取IC管号
		/// </summary>
		public static int TIC_TubeN = 0;        //料管取IC管号
		/// <summary>
		/// 放IC吸笔号
		/// </summary>
		public static int LIC_PenN = 0;         //放IC吸笔号
		/// <summary>
		/// 取IC吸笔号
		/// </summary>
		public static int TIC_PenN = 0;         //取IC吸笔号
        /// <summary>
		/// 吸笔交替模式烧录座第一个取IC吸笔号
		/// </summary>
		public static int Unit_FirstPenN = 0;
		/// <summary>
		/// 工作吸笔号
		/// </summary>
		public static int WorkPenN = 0;         //工作吸笔号
		/// <summary>
		/// 定位IC吸笔号
		/// </summary>
		public static int PIC_PenN = 0;         //定位IC吸笔号
		/// <summary>
		/// 检测IC吸笔号
		/// </summary>
		public static int DIC_PenN = 0;         //检测IC吸笔号
		/// <summary>
		/// 烧录座扫码单元号
		/// </summary>
		public static int Scan_UnitN = 0;       //烧录座扫码单元号
		/// <summary>
		/// 烧录座扫码组号
		/// </summary>
		public static int Scan_GroupN = 0;      //烧录座扫码组号
		/// <summary>
		/// 烧录座放IC单元个数
		/// </summary>
		public static int LIC_UnitC = 0;        //烧录座放IC单元个数
		/// <summary>
		/// 烧录座取IC单元个数
		/// </summary>
		public static int TIC_UnitC = 0;        //烧录座取IC单元个数
		/// <summary>
		/// 单次烧录座取空IC单元个数
		/// </summary>
		public static int SingleTIC_UnitC = 0;  //单次烧录座取空IC单元个数
        /// <summary>
		/// 烧录座同步取放单元个数
		/// </summary>
		public static int Sync_UnitC = 0;
        /// <summary>
        /// NG重烧次数
        /// </summary>
        public static int NGReBurnC = 0;        //NG重烧次数
		/// <summary>
		/// NG连续次数
		/// </summary>
		public static int NGContinueC = 0;      //NG连续次数
        /// <summary>
		/// 烧录座连续NG关闭次数设置
		/// </summary>
		public static int NGContinueC_Shut = 0;
        /// <summary>
		/// 烧录座NG关闭次数设置
		/// </summary>
		public static int NGScketC_Shut = 0;
        /// <summary>
		/// 全部失败n次停止作业设置
		/// </summary>
		public static int NGAllC_Shut = 0;
        /// <summary>
		/// 全部失败计数器
		/// </summary>
		public static int NGAllDoneC_Shut = 0;
        /// <summary>
		/// 总气压值
		/// </summary>
		public static double MPa = 0;

        /// <summary>
        /// 取IC容量数
        /// </summary>
        public static int TIC_Capacity = 0;     //取IC容量数
		/// <summary>
		/// 有效取IC个数
		/// </summary>
		public static int TIC_ValidC = 0;       //有效取IC个数
		/// <summary>
		/// 实际取IC个数
		/// </summary>
		public static int TIC_DoneC = 0;        //实际取IC个数
		/// <summary>
		/// 目标IC个数
		/// </summary>
		public static int TargetC = 0;          //目标IC个数
		/// <summary>
		/// OK完成个数
		/// </summary>
		public static int OKDoneC = 0;          //OK完成个数
		/// <summary>
		/// NG完成个数
		/// </summary>
		public static int NGDoneC = 0;          //NG完成个数
		/// <summary>
		/// NG吸笔个数
		/// </summary>
		public static int NGPenC = 0;           //NG吸笔个数
		/// <summary>
		/// OK吸笔个数
		/// </summary>
		public static int OKPenC = 0;           //烧录前检测OK吸笔个数
        /// <summary>
		/// 烧录前检测OK吸笔个数
		/// </summary>
		public static int DIC_OKPenC = 0;       //烧录前检测OK吸笔个数
		/// <summary>
		/// 扫码NG吸笔个数
		/// </summary>
		public static int SNGPenC = 0;          //扫码NG吸笔个数
        /// <summary>
		/// 有原始料吸笔个数
		/// </summary>
		public static int ExistRawICPenC = 0;   //有原始料吸笔个数
		/// <summary>
		/// 有料吸笔个数
		/// </summary>
		public static int ExistICPenC = 0;      //有料吸笔个数
		/// <summary>
		/// 使能吸笔个数
		/// </summary>
		public static int EnalePenC = 0;        //使能吸笔个数
        /// <summary>
		/// 使能原始料吸笔个数
		/// </summary>
		public static int EnaleRawICPenC = 0; 
        /// <summary>
        /// OK总数
        /// </summary>
        public static int OKAllC = 0;          //OK总数
		/// <summary>
		/// NG总数
		/// </summary>
		public static int NGAllC = 0;          //NG总数

		/// <summary>
		/// 补IC个数
		/// </summary>
		public static int IC_SupplyC = 0;       //补IC个数

		/// <summary>
		/// 取IC步骤
		/// </summary>
		public static int TIC_Step = 0;       //取IC步骤
		/// <summary>
		/// 放IC步骤
		/// </summary>
		public static int LIC_Step = 0;       //放IC步骤
		/// <summary>
		/// 自动运行回原点步骤
		/// </summary>
		public static int ARHR_Step = 0;      //自动运行回原点步骤
		/// <summary>
		/// 自动运行结束步骤
		/// </summary>
		public static int ARE_Step = 0;       //自动运行结束步骤

		/// <summary>
		/// 排空结果
		/// </summary>
		public int EmptyingResult;                   //排空结果
		/// <summary>
		/// 烧录结果
		/// </summary>
		public int ScannerResult;                    //烧录结果
		/// <summary>
		/// 检测结果
		/// </summary>
		public static int DetectionResult = 0;       //检测结果
		/// <summary>
		/// 检测类型
		/// </summary>
		public static int DetectionType = 0;         //检测类型
        /// <summary>
		/// 偏差值
		/// </summary>
		public static double Deviate = 0;

        public static RUNSTATE RunState;

		/// <summary>
		/// 料管有料
		/// </summary>
		public static bool[] ExistIC_Tube = new bool[UserConfig.TubeC];         //料管有料

        /// <summary>
        /// 批量开始时间
        /// </summary>
        public static DateTime StartTime;

        /// <summary>
        /// 批量结束时间
        /// </summary>
        public static DateTime EndTime;

        /// <summary>
        /// 单次批量报警次数
        /// </summary>
        public static int AlarmCount;
        #endregion
        public static Stopwatch stopWatch = new Stopwatch();
        UserEvent evt = new UserEvent();

        public static void FeederTrigge(int feederN)
        {
            In_Output.FeederO[feederN].M = true;
            Feeder[feederN].FeederTakeDelay = GetSysTime() + AutoTiming.BredeTakeDelay;
            Feeder[feederN].FeederOffDelay = GetSysTime() + 100;
        }
        /// <summary>
        /// 旋转触发
        /// </summary>
        /// <param name="mode">真：绝对控制，假：相对控制</param>
        /// <param name="penN"></param>
        /// <param name="velAngle"></param>
        public static void RotateTrigge(bool mode, int penN, double velAngle)
		{
			Pen[penN].Rotate.Busy = true;
			Pen[penN].Rotate.mode = mode;
			Pen[penN].Rotate.AngleVal = velAngle;
			Pen[penN].Rotate.TimeOut = GetSysTime() + 5000;
		}

        /// <summary>
        /// 全部吸笔旋转触发
        /// </summary>
        /// <param name="velAngle"></param>
        public static void AllPenRotateTrigge(double velAngle, int firstPen = 0)
        {
            for (int i = firstPen; i < UserConfig.VacuumPenC; i++)
            {
                if (Pen[i].Enable)
                {
                    RotateTrigge(true, i, velAngle);
                }
            }
        }

        /// <summary>
        /// 获取取料旋转角度
        /// </summary>
        /// <returns></returns>
        public double Get_TIC_RotateAngle()
        {
            double angle = 0;
            if (Auto_Flag.Brede_TakeIC)
            {
                angle = RotateAngle.TIC_Brede;
            }
            else if (Auto_Flag.FixedTray_TakeIC || Auto_Flag.AutoTray_TakeIC)
            {
                if (Auto_Flag.LearnBusy)
                {
                    angle = RotateAngle.TIC_Tray[0];
                }
                else
                {
                    angle = RotateAngle.TIC_Tray[TrayD.TIC_TrayN - 1];
                }
            }
            else if (Auto_Flag.FixedTube_TakeIC)
            {
                angle = RotateAngle.TIC_Tube;
            }
            return angle;
        }

        /// <summary>
        /// 获取放料旋转角度
        /// </summary>
        /// <returns></returns>
        public double Get_LIC_RotateAngle()
        {
            double angle = 0;
            if (Auto_Flag.Brede_LayIC)
            {
                angle = RotateAngle.LIC_Brede;
            }
            else if (Auto_Flag.FixedTray_LayIC || Auto_Flag.AutoTray_LayIC)
            {
                angle = RotateAngle.LIC_Tray[TrayD.LIC_TrayN - 1];
            }
            return angle;
        }

        /// <summary>
        /// 是否允许下一个动作
        /// </summary>
        /// <returns></returns>
        public static bool NextAction_Check()
		{
            bool flag = Auto_Flag.AutoRunBusy && !In_Output.Gate_Sig.M;//安全门打开
            if ((!Auto_Flag.DebugMode && !Auto_Flag.Pause && !Auto_Flag.ALarmPause && !flag) || (Auto_Flag.DebugMode && Auto_Flag.Next))
			{
				Auto_Flag.RunPause = false;
				Auto_Flag.Next = false;
				return true;
			}
			else
			{
				Auto_Flag.RunPause = true;
				return false;
			}
		}

		/// <summary>
		/// 报警后继续
		/// </summary>
		/// <returns></returns>
		public static bool Continue_AfterAlarm()
		{
			if (!Auto_Flag.ALarm && (Auto_Flag.Next || !Auto_Flag.ALarmPause))
			{
				Auto_Flag.ALarmPause = false;
				Auto_Flag.Next = false;
				return true;
			}
			else
			{
				return false;
			}
		}

        /// <summary>
		/// 同步报警后继续
		/// </summary>
		/// <returns></returns>
		public static bool Continue_AfterSyncAlarm()
        {
            if (!Auto_Flag.ALarm && (Auto_Flag.Next || !Auto_Flag.ALarmPause))
            {
                Auto_Flag.ALarmPause = false;
                Auto_Flag.Next = false;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取水平位置成功
        /// 返回值:步序
        /// </summary>
        /// <returns></returns>
        public int Get_Horizontal_Position_successful()
		{

			Auto_Flag.Get_Horizontal = false;
			if (OutOfRange())
			{
				//超行程
				return 90;
			}
			else if (Continue_NGCheck())
			{
				//连续NG报警
				return 60;
			}
			else if (Brede_Flag.ALarmCheck)
			{
				//编带报警查询
				Brede_Flag.ALarmCheck = false;
				return 30;
			}
			else
			{
				//执行
				if (RunState == RUNSTATE.BurnSeat_TakeIC || RunState == RUNSTATE.Emptying || RunState == RUNSTATE.BurnSeat_LayIC)
				{
                    if (!Auto_Flag.BurnSeat_Sync || RunState == RUNSTATE.Emptying || Auto_Flag.OverDeviation)
                    {
                        OpenSocket();
                    }
                    else
                    {
                        SyncOpenSocket();
                    }   
				}
				return 4;
			}
		}

		/// <summary>
		/// 判断是否超出范围
		/// </summary>
		/// <returns></returns>
		public bool OutOfRange()
		{
            if (UserConfig.IsProgrammer)
            {
                return false;
            }
			bool Ret = false;
			if (Position_X.Buffer < Position_X.MinVal)
			{
				Position_X.Buffer = Position_X.MinVal;
				Ret = true;
			}
			else if (Position_X.Buffer > Position_X.MaxVal)
			{
				Position_X.Buffer = Position_X.MaxVal;
				Ret = true;
			}

			if (Position_Y.Buffer < Position_Y.MinVal)
			{
				Position_Y.Buffer = Position_Y.MinVal;
				Ret = true;
			}
			else if (Position_Y.Buffer > Position_Y.MaxVal)
			{
				Position_Y.Buffer = Position_Y.MaxVal;
				Ret = true;
			}
			return Ret;
		}

        /// <summary>
		/// 判断烧录座是否连续NG
		/// </summary>
		/// <returns></returns>
		public bool Continue_NGCheck()
        {
            bool Ret = false;
            if (RunState == RUNSTATE.BurnSeat_TakeIC)
            {
                if (Auto_Flag.BurnSeat_Sync)
                {
                    for (int i = 0; i < Sync_UnitC; i++)
                    {
                        if (Group[SyncPen[i].GroupN].Continue_NG)
                        {
                            Ret = true;
                            break;
                        }
                    }
                }
                else
                {
                    if (Group[TIC_GroupN].Continue_NG)
                    {
                        Ret = true;
                    }
                }
            }
            else if (RunState == RUNSTATE.TakeSNGIC)
            {
                if (Group[Scan_GroupN].Continue_NG)
                {
                    Ret = true;
                }
            }    
            return Ret;
        }

        /// <summary>
		/// 判断烧录座是否Error
		/// </summary>
		/// <returns></returns>
		public bool ErrorCheck()
        {
            bool Ret = false;
            if (RunState == RUNSTATE.BurnSeat_TakeIC)
            {
                if (Auto_Flag.BurnSeat_Sync)
                {
                    for (int i = 0; i < Sync_UnitC; i++)
                    {
                        if (Group[SyncPen[i].GroupN].Flag_Error)
                        {
                            Ret = true;
                            break;
                        }
                    }
                }
                else
                {
                    if (Group[TIC_GroupN].Flag_Error)
                    {
                        Ret = true;
                    }
                }
            }
            return Ret;
        }

        /// <summary>
        /// 报警查询
        /// </summary>
        /// <returns></returns>
        public bool Alarm_Check()
		{
			bool Ret = false;
			if (Auto_Flag.Update_Tray)
			{
				Auto_Flag.Update_Tray = false;
				if (RunState == RUNSTATE.Carrier_LayIC)
				{
					/*M00904 = 1;*/
					BAR._ToolTipDlg.WriteToolTipStr("补料盘满料;[请更换一个空料的盘->确定->继续]");
					BAR.ShowToolTipWnd(true);
				}
				else if (RunState == RUNSTATE.Carrier_LayNGIC || RunState == RUNSTATE.Carrier_LayDNGIC)
				{
					/*M00912 = 1;*/
					BAR._ToolTipDlg.WriteToolTipStr("NG盘满料;[更换新的NG盘->确定->继续]");
					BAR.ShowToolTipWnd(true);
				}
				else if (RunState == RUNSTATE.Carrier_TakeIC)
				{
					/*M00913 = 1;*/
					BAR._ToolTipDlg.WriteToolTipStr("补料盘缺料;[请更换一个有料的盘->确定->继续]");
					BAR.ShowToolTipWnd(true);
				}
				Ret = true;
			}
			else if (Auto_Flag.TubeTimeOut && !Auto_Flag.Ending)
			{
				/*M00906   = 1;*/
				BAR._ToolTipDlg.WriteToolTipStr("料管缺料故障;[处理异常->确定->继续]");
				BAR.ShowToolTipWnd(true);
				Ret = true;
			}
			return Ret;
		}

		/// <summary>
		/// 打开烧录座
		/// </summary>
		public void OpenSocket()
		{
            if (ProgrammerType == GlobConstData.Programmer_RD || (Auto_Flag.Flip && ProgrammerType == GlobConstData.Programmer_WG_TY))
            {
                return;
            }
            int num, GroupN = 0, UnitN = 0;
			if (RunState == RUNSTATE.BurnSeat_TakeIC)
			{
				GroupN = TIC_GroupN;
				UnitN = TIC_UnitN;
			}
            else if (RunState == RUNSTATE.Emptying)
            {
				if (Auto_Flag.BurnSeat_Sync)
				{
					GroupN = SyncPen[SyncPenN].GroupN;
					UnitN = SyncPen[SyncPenN].UnitN;
				}
				else
				{
					GroupN = TIC_GroupN;
					UnitN = TIC_UnitN;
				}
			}
			else
			{
				if (Auto_Flag.OverDeviation)
				{
					GroupN = SyncPen[SyncPenN].GroupN;
					UnitN = SyncPen[SyncPenN].UnitN;
				}
				else
				{
					GroupN = LIC_GroupN;
					UnitN = LIC_UnitN;
				}
			}

			if (!Group[GroupN].Down.Busy)
			{
				num = (UnitN + GroupN * UserConfig.ScketUnitC) / UserConfig.ScketMotionC;
                if (Auto_Flag.Flip && (ProgrammerType == GlobConstData.Programmer_WG || ProgrammerType == GlobConstData.Programmer_YED))//翻盖
                {
                    if(In_Output.flipO[num].M)
                    {
                        In_Output.flipO[num].M = false;
                        AutoTimer.OpenSocket = GetSysTime() + 3000;
                    }                      
                }
                else//压座
				{
					if (!In_Output.pushSeatO[num].M)
					{
						In_Output.pushSeatO[num].M = true;
						In_Output.pushSeatO[num].RM = true;
						AutoTimer.OpenSocket = GetSysTime() + 3000;
					}
				}
			}
		}

        /// <summary>
        /// 打开烧录座到位
        /// </summary> 
        /// <returns></returns>
        public int OpenSocket_InPlace()
        {
            int num, GroupN = 0, UnitN = 0;
            int Ret = 0;
            
			if (RunState == RUNSTATE.BurnSeat_TakeIC)
			{
				GroupN = TIC_GroupN;
				UnitN = TIC_UnitN;
			}
			else if (RunState == RUNSTATE.Emptying)
			{
				if (Auto_Flag.BurnSeat_Sync)
				{
					GroupN = SyncPen[SyncPenN].GroupN;
					UnitN = SyncPen[SyncPenN].UnitN;
				}
				else
				{
					GroupN = TIC_GroupN;
					UnitN = TIC_UnitN;
				}
			}
			else
			{
				if (Auto_Flag.OverDeviation)
				{
					GroupN = SyncPen[SyncPenN].GroupN;
					UnitN = SyncPen[SyncPenN].UnitN;
				}
				else
				{
					GroupN = LIC_GroupN;
					UnitN = LIC_UnitN;
				}
			}

			if (ProgrammerType == GlobConstData.Programmer_RD || (Auto_Flag.Flip && ProgrammerType == GlobConstData.Programmer_WG_TY))
            {
                if (!Group[GroupN].Down.Busy)
                {
                    Ret = 6;
                }
                return Ret;
            }

            if (!Group[GroupN].Down.Busy)
            {
                if (UserConfig.IsProgrammer)
                {
                    return 6;
                }
                num = (UnitN + GroupN * UserConfig.ScketUnitC) / UserConfig.ScketMotionC;
                if (Auto_Flag.Flip && (ProgrammerType == GlobConstData.Programmer_WG || ProgrammerType == GlobConstData.Programmer_YED))//翻盖
                {
                    if (In_Output.flipI[num].M)
                    {
                        Ret = 6;
                    }
                    else if (GetSysTime() > AutoTimer.OpenSocket && !In_Output.flipO[num].M)
                    {
                        Ret = 40;
                    }
                }
                else//压座
                {
                    if (!In_Output.pushSeatI[num].M && In_Output.pushSeatI[num].FM && GetSysTime() > AutoTimer.SeatTakeDelay) 
                    {
                        Ret = 6;
                    }
                    else if (GetSysTime() > AutoTimer.OpenSocket && In_Output.pushSeatO[num].M)
                    {
                        Ret = 40;
                    }
                }
            }
            return Ret;
        }

        /// <summary>
		/// 同步打开烧录座
		/// </summary>
		public void SyncOpenSocket()
        {
            if (ProgrammerType == GlobConstData.Programmer_RD || (Auto_Flag.Flip && ProgrammerType == GlobConstData.Programmer_WG_TY))
            {
                return;
            }
            int num, GroupN = 0, UnitN = 0;
            for (int i = 0; i < Sync_UnitC; i++)
            {
                GroupN = SyncPen[i].GroupN;
                UnitN = SyncPen[i].UnitN;
                if ((RunState == RUNSTATE.BurnSeat_TakeIC && (SyncPen[i].Flag_TakeIC || (SyncPen[i].Flag_First && Config.Altimeter == 0))) || 
                    (RunState == RUNSTATE.BurnSeat_LayIC && SyncPen[i].Flag_LayIC))
                {
                    if (!Group[GroupN].Down.Busy)
                    {
                        num = (UnitN + GroupN * UserConfig.ScketUnitC) / UserConfig.ScketMotionC;
                        if (Auto_Flag.Flip && (ProgrammerType == GlobConstData.Programmer_WG || ProgrammerType == GlobConstData.Programmer_YED))//翻盖
                        {
                            if (In_Output.flipO[num].M)
                            {
                                In_Output.flipO[num].M = false;
                                AutoTimer.OpenSocket = GetSysTime() + 3000;
                            }
                        }
                        else//压座
                        {
                            if (!In_Output.pushSeatO[num].M)
                            {
                                In_Output.pushSeatO[num].M = true;
                                In_Output.pushSeatO[num].RM = true;
                                AutoTimer.OpenSocket = GetSysTime() + 3000;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 同步打开烧录座到位
        /// </summary> 
        /// <returns></returns>
        public int SyncOpenSocket_InPlace()
		{
			int num, GroupN = 0, UnitN = 0;
			int Ret = 0;
            int temp1 = 0, temp2 = 0;
            if (ProgrammerType == GlobConstData.Programmer_RD || (Auto_Flag.Flip && ProgrammerType == GlobConstData.Programmer_WG_TY))
            {
                for (int i = 0; i < Sync_UnitC; i++)
                {
                    if (Group[SyncPen[i].GroupN].Down.Busy)
                    {
                        return 0;
                    }
                }
                return 6;
            }
            for (int i = 0; i < Sync_UnitC; i++)
            {
                GroupN = SyncPen[i].GroupN;
                UnitN = SyncPen[i].UnitN;
                if ((RunState == RUNSTATE.BurnSeat_TakeIC && (SyncPen[i].Flag_TakeIC || (SyncPen[i].Flag_First && Config.Altimeter == 0))) || (RunState == RUNSTATE.BurnSeat_LayIC && SyncPen[i].Flag_LayIC))
                {
                    temp1++;
                    if (!Group[GroupN].Down.Busy)
                    {
                        num = (UnitN + GroupN * UserConfig.ScketUnitC) / UserConfig.ScketMotionC;
                        if (Auto_Flag.Flip && (ProgrammerType == GlobConstData.Programmer_WG || ProgrammerType == GlobConstData.Programmer_YED))//翻盖
                        {
                            if (In_Output.flipI[num].M || UserConfig.IsProgrammer)
                            {
                                temp2++;
                            }
                            else if (GetSysTime() > AutoTimer.OpenSocket && !In_Output.flipO[num].M)
                            {
                                return 40;
                            }
                        }
                        else//压座
                        {
                            if ((!In_Output.pushSeatI[num].M && In_Output.pushSeatI[num].FM && GetSysTime() > AutoTimer.SeatTakeDelay) || UserConfig.IsProgrammer)
                            {
                                temp2++;
                            }
                            else if (GetSysTime() > AutoTimer.OpenSocket && In_Output.pushSeatO[num].M)
                            {
                                return 40;
                            }
                        }
                    }
                }
            }
            if (temp1 == temp2)
            {
                Ret = 6;
            }
			return Ret;
		}

        /// <summary>
		/// 自动收尾后打开烧录座
		/// </summary>
		public void OpenSocket_Auto_End()
        {
            if (ProgrammerType == GlobConstData.Programmer_RD || (Auto_Flag.Flip && ProgrammerType == GlobConstData.Programmer_WG_TY))
            {
                return;
            }
            int num;

            for (int j = 0; j < UserConfig.ScketGroupC; j++)
            {
                for (int i = 0; i < UserConfig.ScketUnitC; i++)
                {
                    if (!Group[j].Down.Busy && Group[j].Unit[i].Flag_Open)
                    {
                        num = (i + j * UserConfig.ScketUnitC) / UserConfig.ScketMotionC;
                        if (Auto_Flag.Flip && (ProgrammerType == GlobConstData.Programmer_WG || ProgrammerType == GlobConstData.Programmer_YED))//翻盖
                        {
                            if (In_Output.flipO[num].M)
                            {
                                In_Output.flipO[num].M = false;
                                AutoTimer.OpenSocket = GetSysTime() + 3000;
                            }
                        }
                        else//压座
                        {
                            if (!In_Output.pushSeatO[num].M)
                            {
                                In_Output.pushSeatO[num].M = true;
                                In_Output.pushSeatO[num].RM = true;
                                AutoTimer.OpenSocket = GetSysTime() + 3000;
                            }
                        }
                    }
                }
            }

            
        }

        /// <summary>
        /// 水平位置到位处理
        /// 返回值:步序
        /// </summary>
        /// <returns></returns>
        public int Horizontal_InPlace_Handle()
		{
			int Ret = 0;
			if (RunState == RUNSTATE.Photograph)
			{
                if (Config.CCDModel == 0)//定拍
                {
                    stopWatch.Reset();
                    stopWatch.Start();
                    Ret = 20;
                }
                else//飞拍
                {
                    Ret = 16;
                }
			}
            else if(RunState == RUNSTATE.ScanCode)
            {
                Ret = 23;
            }
            else if (RunState == RUNSTATE.BurnSeat_TakeIC || RunState == RUNSTATE.BurnSeat_LayIC || RunState == RUNSTATE.Emptying)
			{
                if (!Auto_Flag.BurnSeat_Sync || RunState == RUNSTATE.Emptying || Auto_Flag.OverDeviation)
                {
                    OpenSocket();
                    Ret = OpenSocket_InPlace();
                    if (Ret == 40)
                    {
                        BAR._ToolTipDlg.WriteToolTipStr("打开座子,感应异常;[请检查气压气阀或气缸感应->确定->继续]");
                        BAR.ShowToolTipWnd(true);
                    }
                    else if (Ret == 6 && RunState == RUNSTATE.Emptying)
                    {
                        Ret = 25;
                    }
                }
                else
                {
                    SyncOpenSocket();
                    Ret = SyncOpenSocket_InPlace();
                    if (Ret == 40)
                    {
                        BAR._ToolTipDlg.WriteToolTipStr("打开座子,感应异常;[请检查气压气阀或气缸感应->确定->继续]");
                        BAR.ShowToolTipWnd(true);
                    }
                }
			}
			else if (RunState == RUNSTATE.Carrier_TakeIC && Auto_Flag.Brede_TakeIC)
			{
                if (Config.FeederCount == 0)//单飞达
                {
                    if (GetSysTime() > AutoTimer.BredeTakeDelay)
                    {
                        Ret = 6;
                    }
                }
                else if (Config.FeederCount == 1)//双飞达
                {
                    if (GetSysTime() > Feeder[TIC_FeederN].FeederTakeDelay)
                    {
                        Ret = 6;
                    }
                }
                
			}
			else
			{
				Ret = 6;
			}
			return Ret;
		}

		/// <summary>
		/// 烧录触发
		/// </summary>
		/// <param name="GroupN"></param>
		public void DownTrigger_Handle(int GroupN)
		{
			int i, j, temp;
            if (ProgrammerType == GlobConstData.Programmer_RD || (Auto_Flag.Flip && ProgrammerType == GlobConstData.Programmer_WG_TY))
            {
                Group[GroupN].Down.Trigger = true;//该组启动烧录;
                Group[GroupN].Down.Busy = true;
            }
            else
            {
                if (!Auto_Flag.TestMode)
                {
                    Group[GroupN].Down.Trigger = true;//该组启动烧录;
                    Group[GroupN].Down.Busy = true;
                }
            }
            for (i = 0; i < UserConfig.ScketUnitC; i++)
            {
                if (Group[GroupN].Unit[i].Flag_TakeIC)
                {
                    Group[GroupN].Unit[i].Flag_NewIC = false;
                }
            }
            Group[GroupN].Waiting_To_Burn = false;
            LIC_UnitN = 0;
            LIC_GroupN++;
            if (LIC_GroupN == UserConfig.ScketGroupC)//查询到最后一组
            {
                LIC_GroupN = 0;
            }
            string str = "第[" + (GroupN + 1) + "]组烧写座烧录触发";
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
            if (ProgrammerType == GlobConstData.Programmer_RD || (Auto_Flag.Flip && ProgrammerType == GlobConstData.Programmer_WG_TY))
            {
                return;
            }
            if (Auto_Flag.Flip && (ProgrammerType == GlobConstData.Programmer_WG || ProgrammerType == GlobConstData.Programmer_YED))//翻盖
	        {
                temp = (GroupN * UserConfig.MotionGroupC);
                for (i = 0; i < UserConfig.MotionGroupC; i++)
                {
                    for (j = 0; j < UserConfig.ScketMotionC; j++)
                    {
                        if (Group[GroupN].Unit[i * UserConfig.ScketMotionC + j].Flag_TakeIC)
                        {
                            In_Output.flipO[temp + i].M = true;
                            break;
                        }
                    }
                }
            }
	        else//压座
            {
                temp = (GroupN * UserConfig.MotionGroupC);
				for (i = 0; i < UserConfig.MotionGroupC; i++)
				{
					In_Output.pushSeatO[temp + i].M = false;
				}
			}
		}

        /// <summary>
		/// 同步吸笔烧录触发检查
		/// </summary>
		public void DownTrigger_SyncPenCheck()
        {
            for (int i = 0; i < Sync_UnitC; i++)
            {
                if (Group[SyncPen[i].GroupN].Waiting_To_Burn)
                {
                    if (Group[SyncPen[i].GroupN].LIC_UnitC == 0 || Auto_Flag.Ending)
                    {
                        if (Pen[SyncPen[i].PenN].DownTrigger)
                        {
                            Pen[SyncPen[i].PenN].DownTrigger = false;
                            DownTrigger_Handle(SyncPen[i].GroupN);//烧录触发处理
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 扫码触发
        /// </summary>
        /// <param name="GroupN"></param>
        public void ScanTrigger_Handle(int GroupN)
        {
            if (!Auto_Flag.TestMode)
            {
                Group[GroupN].Scan.Trigger = true;//该组启动扫码;
                Group[GroupN].Scan.Busy = true;
                Group[GroupN].Down.Busy = true;
            }
            else
            {
                Group[GroupN].Scan.Busy = false;
                Group[GroupN].Down.Busy = false;
            }
        }

        /// <summary>
        /// 拍照打开光源
        /// </summary>
        public void PhotographLightOn_Handle(bool flag = false)
        {
            RunState = RUNSTATE.Photograph;
            int firstPen = 0, endPen = UserConfig.VacuumPenC - 1;
            while (endPen >= 0)
            {
                if (Pen[endPen].ExistIC)
                {
                    break;
                }
                endPen--;
            }
            while (firstPen < UserConfig.VacuumPenC)
            {
                if (Pen[firstPen].ExistIC)
                {
                    break;
                }
                firstPen++;
            }

            if (flag)
            {
                Auto_Flag.Ascending_ICPos = true;
                Auto_Flag.Cam_2D_Mode_II = true;
            }
            else if (Pen[firstPen].LowCamera_X - trapPrm_X.getPos >= 0)//下相机拍照位置在第一支有料吸笔的左侧
            {
                Auto_Flag.Ascending_ICPos = true;
            }
            else if (Pen[endPen].LowCamera_X - trapPrm_X.getPos <= 0)//下相机拍照位置在最后一支有料吸笔的右侧
            {
                Auto_Flag.Ascending_ICPos = false;
            }
            else//下相机拍照位置在第一支有料吸笔和最后一支有料吸笔之间
            {
                Auto_Flag.Ascending_ICPos = true;
                if (Pen[endPen].LowCamera_X + Pen[firstPen].LowCamera_X < 2 * trapPrm_X.getPos)//最后一支有料吸笔拍照位置较近
                {
                    Auto_Flag.Ascending_ICPos = false;
                }
            }
            PIC_PenN = Auto_Flag.Ascending_ICPos ? firstPen : endPen;
            WorkPenN = Auto_Flag.Ascending_ICPos ? endPen : firstPen;
            //D00849 = 1; //打开下光源
            Auto_Flag.DownLightOn = true;
            g_act.SendCmd(GlobConstData.ST_LIGHTDOWN, g_config.ArrLampBox[Auto_Flag.Enabled_NCCModel ? GlobConstData.ST_MODELICPOS_NCC : GlobConstData.ST_MODELICPOS].ILightDown); 
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送打开下光源命令", "Flow");
        }

        /// <summary>
        /// 测高顺序
        /// </summary>
        public void Altimeter_Order()
        {
            Auto_Flag.Ascending_ICPos = Altimeter.Calibrating_X < g_config.ArrPickPos[1].X;
            int firstPen = 0, endPen = Sync_UnitC - 1;
            while (endPen >= 0)
            {
                if (SyncPen[endPen].Flag_TakeIC)
                {
                    break;
                }
                endPen--;
            }
            while (firstPen < Sync_UnitC)
            {
                if (SyncPen[firstPen].Flag_TakeIC)
                {
                    break;
                }
                firstPen++;
            }
            SyncPenN = Auto_Flag.Ascending_ICPos ? firstPen : endPen;
        }

        /// <summary>
        /// 飞拍触发
        /// </summary>
        public void FlyPhotographTrigger_Handle()
        {
            short temp = 0;
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                if (Pen[i].ExistIC)
                {
                    temp++;
                }
            }
            Gts.T2DCompareData[] buf = new Gts.T2DCompareData[temp]; //二维位置比较输出数据
            Gts.T2DCompareData[] databuf = new Gts.T2DCompareData[temp]; //二维位置比较输出数据
            //设置数据在XY轴位置达到目标位置时通道HSIO0输出脉冲。
            temp = 0;
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                
                if (Pen[i].ExistIC)
                {
                    rectify[i].AxisX = 0;
                    rectify[i].AxisY = 0;
                    Pen[i].Image_Busy = true;
					databuf[temp].px = buf[temp].px = (int)(Pen[i].LowCamera_X / trapPrm_X.pulFactor);
					databuf[temp].py = buf[temp].py = (int)(Pen[0].LowCamera_Y / trapPrm_Y.pulFactor);
                    temp++;
                }
                else
                {
                    Pen[i].Image_Busy = false;
                }
            }
            if (!Auto_Flag.Ascending_ICPos)
            {
				for (int i = 0; i < temp; i++)
				{
					databuf[i].px = buf[temp - i - 1].px;
					databuf[i].py = buf[temp - i - 1].py;
				}
			}
            if (UserConfig.IsProgrammer)
            {
                for (int i = 0; i < UserConfig.VacuumPenC; i++)
                {
                    Pen[i].Image_Busy = false;
                }
                return;
            }

            Gts.GT_2DCompareStatus(PLC1.card[0].cardNum, 0, out short pStatus, out int pCount, out short pFifo, out short pFifoCount, out short pBufCount);
            Gts.GT_2DCompareData(PLC1.card[0].cardNum, 0, temp, ref databuf[0], pFifo);
            if (Config.CameraType == GlobConstData.Camera_DH)
            {
                g_act.ArrDHCameraUtils[GlobConstData.ST_CCDDOWN].SetEnumValue("TriggerSource", "Line2");
            }
            else
            {
                g_act.ArrHRCameraUtils[GlobConstData.ST_CCDDOWN].SetLineMode();
            }
        }

        /// <summary>
        /// 获取烧录座XY坐标
        /// </summary>
        /// <param name="groupN"></param>
        /// <param name="unitN"></param>
        /// <param name="penN"></param>
        public void GetSocketXY(int groupN, int unitN, int penN)
		{
            Position_X.Buffer = Group[groupN].Unit[unitN].TopCamera_X -
								Pen[penN].Offset_TopCamera_X;
			Position_Y.Buffer = Group[groupN].Unit[unitN].TopCamera_Y -
								Pen[penN].Offset_TopCamera_Y;
		}

        public void GetSocketXY(int groupN, int unitN)
        {
            Position_X.Buffer = Group[groupN].Unit[unitN].TopCamera_X -
                                Altimeter.Offset_TopCamera_X - Altimeter.Offset_Socket_X;
            Position_Y.Buffer = Group[groupN].Unit[unitN].TopCamera_Y -
                                Altimeter.Offset_TopCamera_Y - Altimeter.Offset_Socket_Y;
        }

        public void GetSocketXY(int groupN, int unitN, int penN, PenRectify penRectify)
		{
			Position_X.Buffer = Group[groupN].Unit[unitN].TopCamera_X -
								Pen[penN].Offset_TopCamera_X + penRectify.AxisX;
			Position_Y.Buffer = Group[groupN].Unit[unitN].TopCamera_Y -
								Pen[penN].Offset_TopCamera_Y + penRectify.AxisY;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="penN"></param>
        public void SyncPen_LIC_UnitC(int n)
        {
            IC_SupplyC++;
            if (!Group[SyncPen[n].GroupN].Unit[SyncPen[n].UnitN].Flag_LayIC)
            {
                Group[SyncPen[n].GroupN].Unit[SyncPen[n].UnitN].Flag_LayIC = true;
                LIC_UnitC++;
                Group[SyncPen[n].GroupN].LIC_UnitC++;
            }
            if (LIC_UnitC >= TIC_Capacity)
            {
                Auto_Flag.Emptying = false;
            }
        }

        /// <summary>
		/// 烧录座同步取料查询
		/// </summary>
		public bool SyncTakeIC_Seat_Check()
        {
            bool rtn = false;
            if (SyncPen_Struct.State_TakeIC)
            {
				for (int i = 0; i < Sync_UnitC; i++)
				{
					if (SyncPen[i].Flag_First || SyncPen[i].Flag_TakeIC)
					{
						GetSocketXY(SyncPen[i].GroupN, SyncPen[i].UnitN, SyncPen[i].PenN);
						break;
					}
				}
                SyncPen_Struct.State_TakeIC = false;
                return true;
            }
            if (Auto_Flag.Ending)
            {
                Auto_Flag.Emptying = false;
            }

            SyncPenN = 0;
            IC_SupplyC = 0;
            SyncPen_Struct.State_Emptying = false;
            SyncPen_Struct.State_LayIC = false;
			int checkCount = 0;
            while (true)
            {
                if (TIC_GroupN == 0 && SyncPen_Struct.Seat_TIC_Cycle)//非第一次查询到第一组
				{
                    //作用于效率刷新
					SyncPen_Struct.Seat_TIC_Cycle = false;
					Auto_Flag.Seat_TIC_Cycle = true;
				}
				//获取取料座序号
				SyncPen[SyncPenN].GroupN = TIC_GroupN;
                SyncPen[SyncPenN].UnitN = TIC_UnitN;
                SyncPen[SyncPenN].Flag_First = false;
                SyncPen[SyncPenN].Flag_TakeIC = false;
                SyncPen[SyncPenN].Flag_LayIC = false;
                SyncPen[SyncPenN].OverDeviation = false;
                Pen[SyncPen[SyncPenN].PenN].DownTrigger = false;
				if (Group[TIC_GroupN].Unit[TIC_UnitN].Flag_TakeIC)
                {
                    if (Group[TIC_GroupN].Unit[TIC_UnitN].Flag_NewIC && !Auto_Flag.TestMode)//烧录模式该座有料且待烧录
                    {
                        DownTrigger_Handle(TIC_GroupN);//烧录触发处理
                        Auto_Flag.Seat_EndTakeIC = true;
                        break;
                    }
                    else //if(!Group[TIC_GroupN].Down.Busy)//烧录组优先取
                    {
                        //标记取料吸笔
                        SyncPen[SyncPenN].Flag_TakeIC = true;
                        SyncPen_Struct.State_TakeIC = true;
						Group[TIC_GroupN].Waiting_To_Take = true;
					}
                }
                else if (Group[TIC_GroupN].Unit[TIC_UnitN].Flag_Open)
                {
                    if (Group[TIC_GroupN].Unit[TIC_UnitN].Flag_First)
                    {
                        if (Auto_Flag.Emptying)
                        {
                            SyncPen[SyncPenN].Flag_First = true;
                            if (Config.Altimeter != 0)//使用测高仪
                            {
                                SyncPen_Struct.State_Emptying = true;
                            }
                            else//不使用测高仪
                            {
                                SyncPen_Struct.State_TakeIC = true;
                            }
							Group[TIC_GroupN].Waiting_To_Take = true;
						}
                    }
                    else
                    {
                        //重开座子是个空座
                        SyncPen[SyncPenN].Flag_LayIC = true;
                        SyncPen_Struct.State_LayIC = true;
						Group[TIC_GroupN].Waiting_To_Lay = true;
					}
                }
                TIC_UnitN++;
                if (TIC_UnitN == UserConfig.ScketUnitC)//查询到每组最后一个座子
                {
					Pen[SyncPen[SyncPenN].PenN].DownTrigger = true;
                    if (!Group[TIC_GroupN].Waiting_To_Take && (!Group[TIC_GroupN].Waiting_To_Lay || Auto_Flag.Ending))
                    {
                        if (!Group[TIC_GroupN].Waiting_To_Burn)
                        {
                            if (!(Auto_Flag.Flip && (ProgrammerType == GlobConstData.Programmer_WG || ProgrammerType == GlobConstData.Programmer_YED || ProgrammerType == GlobConstData.Programmer_WG_TY)))//压座
                            {
                                if (ProgrammerType != GlobConstData.Programmer_RD)
                                {
                                    int temp = TIC_GroupN * UserConfig.MotionGroupC;
                                    for (int i = 0; i < UserConfig.MotionGroupC; i++)
                                    {
                                        In_Output.pushSeatO[temp + i].M = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            DownTrigger_Handle(TIC_GroupN);//烧录触发处理
                        }
                    }
					
					TIC_UnitN = 0;
                    TIC_GroupN++;
                    if (TIC_GroupN == UserConfig.ScketGroupC)//查询到最后一组
                    {
						SyncPen_Struct.Seat_TIC_Cycle = true;
						TIC_GroupN = 0;
                    }
                }
                
                SyncPenN++;
                if (SyncPenN == Sync_UnitC)
                {
					for (int i = 0; i < Sync_UnitC; i++)
					{
						Group[SyncPen[i].GroupN].Waiting_To_Take = false;
						Group[SyncPen[i].GroupN].Waiting_To_Lay = false;
					}
					SyncPenN = 0;
                    if (SyncPen_Struct.State_Emptying)
                    {
                        for (int i = 0; i < Sync_UnitC; i++)
                        {
                            if (SyncPen[i].Flag_First)
                            {
                                SyncPenN = i;
                                break;
                            }
                        }
                        Auto_Flag.Ascending_ICPos = true;
                        RunState = RUNSTATE.Emptying;
                        break;
                    }
                    else if (SyncPen_Struct.State_TakeIC)
                    {
						//获取取料座坐标
						for (int i = 0; i < Sync_UnitC; i++)
						{
							if (SyncPen[i].Flag_First || SyncPen[i].Flag_TakeIC)
							{
								GetSocketXY(SyncPen[i].GroupN, SyncPen[i].UnitN, SyncPen[i].PenN);
								break;
							}
						}	
                        SyncPen_Struct.State_TakeIC = false;
                        rtn = true;
                        break;
                    }
                    else if (SyncPen_Struct.State_LayIC)
                    {
                        for (int i = 0; i < Sync_UnitC; i++)
                        {
                            if (SyncPen[i].Flag_LayIC)
                            {
                                SyncPen_LIC_UnitC(i);
                            }
                        }
                        if (!Auto_Flag.Ending)
                        {
                            RunState = RUNSTATE.Carrier_TakeIC;
                            break;
                        } 
                    }
                }

                checkCount++;
                if (checkCount == UserConfig.AllScketC)
                {
                    break;
                }
            }
            return rtn;
        }
        /// <summary>
        /// 烧录座查询
        /// </summary>
        public bool TakeIC_Seat_Check()
		{
            bool rtn = false;
            int i, temp;
			while (true)
			{
				if (Auto_Flag.Ending)
				{
					Auto_Flag.Emptying = false;
				}
                if (Group[TIC_GroupN].Unit[TIC_UnitN].Flag_TakeIC)
				{
                    if (Group[TIC_GroupN].Unit[TIC_UnitN].Flag_NewIC && !Auto_Flag.TestMode)//烧录模式该座有料且待烧录
					{
						if (Auto_Flag.Ending)
						{
							DownTrigger_Handle(TIC_GroupN);//烧录触发处理
						}
						Auto_Flag.Seat_EndTakeIC = true;
					}
					else
					{
                        //获取取料座坐标
                        GetSocketXY(TIC_GroupN, TIC_UnitN, TIC_PenN);
                        rtn = true;
                    }
					break;
				}
				else if (Group[TIC_GroupN].Unit[TIC_UnitN].Flag_Open)
				{
					if (Group[TIC_GroupN].Unit[TIC_UnitN].Flag_First)
					{
						if (Auto_Flag.Emptying)
						{
                            //获取排空座坐标
                            if (Config.Altimeter != 0)
                            {
                                GetSocketXY(TIC_GroupN, TIC_UnitN);
                                RunState = RUNSTATE.Emptying;
                            }
                            else
                            {
                                GetSocketXY(TIC_GroupN, TIC_UnitN, TIC_PenN);
                            }
                            rtn = true;
                            break;
						}
					}
					else
					{
                        //重开座子是个空座
                        if (!Group[TIC_GroupN].Unit[TIC_UnitN].Flag_LayIC)
                        {
                            Group[TIC_GroupN].Unit[TIC_UnitN].Flag_LayIC = true;
                            LIC_UnitC++;
                            Group[TIC_GroupN].LIC_UnitC++;
                        }
                        if (LIC_UnitC >= TIC_Capacity)
                        {
                            Auto_Flag.Emptying = false;
                        }
                    }
				}
                else
                {
                    if (Group[TIC_GroupN].Unit[TIC_UnitN].Flag_LayIC)
                    {
                        Group[TIC_GroupN].Unit[TIC_UnitN].Flag_LayIC = false;
                        LIC_UnitC--;
                        Group[TIC_GroupN].LIC_UnitC--;
                    }
                }
				TIC_UnitN++;
				if (TIC_UnitN == UserConfig.ScketUnitC)//查询到每组最后一个座子
				{
					if (!Group[TIC_GroupN].Waiting_To_Burn && Group[TIC_GroupN].LIC_UnitC == 0)
					{
                        if (!(Auto_Flag.Flip && (ProgrammerType == GlobConstData.Programmer_WG || ProgrammerType == GlobConstData.Programmer_YED || ProgrammerType == GlobConstData.Programmer_WG_TY)))//压座
                        {
                            if (ProgrammerType != GlobConstData.Programmer_RD)
                            {
                                temp = TIC_GroupN * UserConfig.MotionGroupC;
                                for (i = 0; i < UserConfig.MotionGroupC; i++)
                                {
                                    In_Output.pushSeatO[temp + i].M = false;
                                }
                            }
						}
					}
					if (Group[TIC_GroupN].Waiting_To_Burn &&
					(Group[TIC_GroupN].LIC_UnitC == 0 || Auto_Flag.ForceEnd ||
					(Auto_Flag.Ending && NGPenC == 0)))
					{
						DownTrigger_Handle(TIC_GroupN);//烧录触发处理				
					}
					TIC_UnitN = 0;
					TIC_GroupN++;
					if (TIC_GroupN == UserConfig.ScketGroupC)//查询到最后一组
					{
						TIC_GroupN = 0;
						if (Group[TIC_GroupN].Unit[TIC_UnitN].Flag_NewIC && !Auto_Flag.TestMode)//烧录模式该座有料且待烧录
						{
							if (Auto_Flag.Ending)
							{
								DownTrigger_Handle(TIC_GroupN);//烧录触发处理
							}
						}
						Auto_Flag.Seat_TIC_Cycle = true;
						Auto_Flag.Seat_EndTakeIC = true;
						break;
					}
				}
			}
            return rtn;
        }

        /// <summary>
        /// 吸笔交替模式烧录座取料查询
        /// </summary>
        public bool TakeIC_Seat_Check_II()
        {
            Random rd = new Random();
            int groupC = rd.Next(0, UserConfig.ScketGroupC);
            bool rtn = false;
            int i, temp;
            int groupN = TIC_GroupN;
            int unitN = TIC_UnitN;

            Auto_Flag.Emptying = !(Auto_Flag.Ending && ExistRawICPenC == 0);
            while (true)
            {
                if (!Group[groupN].Down.Busy)
                {
                    if (Group[groupN].Unit[unitN].Flag_TakeIC)
                    {
                        if (Group[groupN].Unit[unitN].Flag_NewIC && !Auto_Flag.TestMode)//烧录模式该座有料且待烧录
                        {
                            DownTrigger_Handle(groupN);//烧录触发处理
                            Auto_Flag.Seat_EndTakeIC = true;
                        }
                        else
                        {
                            //获取取料座坐标
                            GetSocketXY(groupN, unitN, TIC_PenN);
                            rtn = true;
                        }
                        break;
                    }
                    else if (Group[groupN].Unit[unitN].Flag_Open)
                    {
                        if (Group[groupN].Unit[unitN].Flag_First)
                        {
                            if (Auto_Flag.Emptying)
                            {
                                //获取排空座坐标
                                if (Config.Altimeter != 0)
                                {
                                    GetSocketXY(groupN, unitN);
                                    RunState = RUNSTATE.Emptying;
                                }
                                else
                                {
                                    GetSocketXY(groupN, unitN, TIC_PenN);
                                }
                                rtn = true;
                                break;
                            }
                        }
                        else
                        {
                            //重开座子是个空座
                            if (!Group[groupN].Unit[unitN].Flag_LayIC)
                            {
                                Group[groupN].Unit[unitN].Flag_LayIC = true;
                                LIC_UnitC++;
                                Group[groupN].LIC_UnitC++;
                                if (true)
                                {
                                    RunState = RUNSTATE.BurnSeat_LayIC;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Group[groupN].Unit[unitN].Flag_LayIC)
                        {
                            Group[groupN].Unit[unitN].Flag_LayIC = false;
                            LIC_UnitC--;
                            Group[groupN].LIC_UnitC--;
                        }
                    }
                    unitN++;
                    if (unitN == UserConfig.ScketUnitC)//查询到每组最后一个座子
                    {
                        if (!Group[groupN].Waiting_To_Burn && Group[groupN].LIC_UnitC == 0)
                        {
                            if (!(Auto_Flag.Flip && (ProgrammerType == GlobConstData.Programmer_WG || ProgrammerType == GlobConstData.Programmer_YED || ProgrammerType == GlobConstData.Programmer_WG_TY)))//压座
                            {
                                if (ProgrammerType != GlobConstData.Programmer_RD)
                                {
                                    temp = groupN * UserConfig.MotionGroupC;
                                    for (i = 0; i < UserConfig.MotionGroupC; i++)
                                    {
                                        In_Output.pushSeatO[temp + i].M = false;
                                    }
                                }
                            }
                        }
                        if (Group[groupN].Waiting_To_Burn &&
                        (Group[groupN].LIC_UnitC == 0 || Auto_Flag.ForceEnd ||
                        (Auto_Flag.Ending && NGPenC == 0)))
                        {
                            DownTrigger_Handle(groupN);//烧录触发处理				
                        }
                        unitN = 0;
                        groupN++;
                        groupC++;
                        Auto_Flag.Seat_TIC_Cycle = true;
                    }
                }
                else
                {
                    unitN = 0;
                    groupN++;
                    groupC++;
                }

                if (groupN == UserConfig.ScketGroupC)//查询到最后一组
                {
                    groupN = 0;
                }

                if (groupC == UserConfig.ScketGroupC)//查询了一轮
                {
                    break;
                }
            }
            TIC_GroupN = groupN;
            TIC_UnitN = unitN;
            return rtn;
        }

        /// <summary>
        /// 放料座查询
        /// </summary>
        public void LayIC_Seat_Check()
		{
			while (true)
			{
				if (Group[LIC_GroupN].Unit[LIC_UnitN].Flag_LayIC)
				{
					if (RunState == RUNSTATE.BurnSeat_LayIC)
					{
						//获取放料座位置
						GetSocketXY(LIC_GroupN, LIC_UnitN, LIC_PenN, rectify[LIC_PenN]);
					}
					else if (RunState == RUNSTATE.Burn_Again)//重烧
					{
						TIC_PenN--;
						//获取放料座位置
						GetSocketXY(LIC_GroupN, LIC_UnitN, TIC_PenN);
					}
					break;
				}
				LIC_UnitN++;
				if (LIC_UnitN == UserConfig.ScketUnitC)//查询到每组最后一个座子
				{
					if (Group[LIC_GroupN].Waiting_To_Burn)
					{
						DownTrigger_Handle(LIC_GroupN);//烧录触发处理
					}
					else
					{
						LIC_UnitN = 0;
						LIC_GroupN++;
						if (LIC_GroupN == UserConfig.ScketGroupC)//查询到最后一组
						{
							LIC_GroupN = 0;
						}
					}
				}
				if ((LIC_UnitN == TIC_UnitN) && (LIC_GroupN == TIC_GroupN))//查询到取料座位置
				{
					if (RunState == RUNSTATE.Burn_Again)
					{
						Auto_Flag.Seat_EndLayIC = true;
					}
					break;
				}
			}
		}

        /// <summary>
        /// 吸笔交替模式放料座查询
        /// </summary>
        public void LayIC_Seat_Check_II()
        {
            while (true)
            {
                if ((LIC_UnitN == TIC_UnitN) && (LIC_GroupN == TIC_GroupN) && !Group[LIC_GroupN].Unit[LIC_UnitN].Flag_LayIC && RunState != RUNSTATE.Burn_Again)//查询到取料座位置
                {
                    Auto_Flag.Seat_EndLayIC = true;
                    break;
                }
                if (Group[LIC_GroupN].Unit[LIC_UnitN].Flag_LayIC)
                {
                    if (RunState == RUNSTATE.BurnSeat_LayIC)
                    {
                        //获取放料座位置
                        GetSocketXY(LIC_GroupN, LIC_UnitN, LIC_PenN, rectify[LIC_PenN]);
                    }
                    else if (RunState == RUNSTATE.Burn_Again)//重烧
                    {
                        TIC_PenN--;
                        //获取放料座位置
                        GetSocketXY(LIC_GroupN, LIC_UnitN, TIC_PenN);
                    }
                    break;
                }
                LIC_UnitN++;
                if (LIC_UnitN == UserConfig.ScketUnitC)//查询到每组最后一个座子
                {
                    if (Group[LIC_GroupN].Waiting_To_Burn)
                    {
                        DownTrigger_Handle(LIC_GroupN);//烧录触发处理
                    }
                    else
                    {
                        LIC_UnitN = 0;
                        LIC_GroupN++;
                        if (LIC_GroupN == UserConfig.ScketGroupC)//查询到最后一组
                        {
                            LIC_GroupN = 0;
                        }
                    }
                }
                //if ((LIC_UnitN == TIC_UnitN) && (LIC_GroupN == TIC_GroupN))//查询到取料座位置
                //{
                //    Auto_Flag.Seat_EndLayIC = true;
                //    break;
                //}
            }
        }

        /// <summary>
        /// 烧录座取料优先级查询
        /// </summary>
        public void TakeIC_Seat_Priority_Check()
        {
            while (true)
            {
                if (Group[TIC_GroupN].Down.Busy)
                {
                    TIC_GroupN++;
                    if (TIC_GroupN == UserConfig.ScketGroupC)//查询到最后一组
                    {
                        TIC_GroupN = 0;
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        public void TakeIC_Seat_Priority_Check1()
        {
            while (true)
            {
                if (Group[TIC_GroupN].Down.Busy)
                {
                    TIC_UnitN = 0;
                    TIC_GroupN++;
                    if (TIC_GroupN == UserConfig.ScketGroupC)//查询到最后一组
                    {
                        TIC_GroupN = 0;
                        LIC_GroupN = TIC_GroupN;
                        LIC_UnitN = TIC_UnitN;
                        break;
                    }
                }
                else
                {
                    LIC_GroupN = TIC_GroupN;
                    LIC_UnitN = TIC_UnitN;
                    break;
                }
            }
        }

        /// <summary>
        /// 获取补料数
        /// </summary>
        public void Get_SupplyIC_Sum()
		{
			int GroupN;
			int SeatN;
			GroupN = LIC_GroupN;
			SeatN = LIC_UnitN;
			while (true)
			{
				if (SeatN != 0 && GroupN != 0)
				{
					if (SeatN == TIC_UnitN && GroupN == TIC_GroupN)//查询到取料座位置
					{
						break;
					} 
				}
				if (Group[GroupN].Unit[SeatN].Flag_LayIC)
				{
					IC_SupplyC++;
				}
				SeatN++;
				if (SeatN == UserConfig.ScketUnitC)//查询到每组最后一个座子
				{
					if (IC_SupplyC == 0 && Group[GroupN].Waiting_To_Burn)
					{
						LIC_GroupN = GroupN;
						DownTrigger_Handle(LIC_GroupN);//烧录触发处理
					}
					SeatN = 0;
					GroupN++;
					if (GroupN == UserConfig.ScketGroupC)//查询到最后一组
					{
						GroupN = 0;
                        if (IC_SupplyC == 0)
                        {
                            LIC_UnitN = 0;
                            LIC_GroupN = 0;
                        }
                    }
				}
				if ((SeatN == TIC_UnitN) && (GroupN == TIC_GroupN))//查询到取料座位置
				{
					break;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool Tube_Check()
		{
			int i, Num;
			Num = 0;
			for (i = 0; i < UserConfig.TubeC; i++)
			{
				if (ExistIC_Tube[i])
				{
					break;
				}
				Num++;
			}
			if (Num == UserConfig.TubeC)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// 获取料管位置
		/// </summary>
		public bool Get_Tube_Position()
		{
			if (Tube_Check())
			{
                Auto_Flag.Tube_AllEmptying = false;
                Auto_Flag.TubeTimeOut = false;
				while (true)
				{
					if (ExistIC_Tube[TIC_TubeN])//对应料管处有料
					{
						Position_X.Buffer = Position_X.tubeIn - TIC_TubeN * Position_X.tubespace;
						Position_Y.Buffer = Position_Y.tubeIn;
						break;
					}
					TIC_TubeN++;
					if (TIC_TubeN >= UserConfig.TubeC)
					{
						TIC_TubeN = 0;
					}
				}
				return true;
			}
			else// 所有料管位置都没有料
			{
                if (!Auto_Flag.Tube_AllEmptying)
				{
                    TIC_TubeN = 0;
                    Auto_Flag.Tube_AllEmptying = true;
                    Auto_Flag.TubeTimeOut = false;
                    AutoTimer.TubeTimeOut = GetSysTime() + AutoTiming.TubeTimeOut;
                }
				else
				{
                    if (GetSysTime() > AutoTimer.TubeTimeOut)
                    {
                        Auto_Flag.TubeTimeOut = true;
                    }
                }
                return false;
			}
		}

        /// <summary>
        /// 获取料盘位置
        /// </summary>
        /// <param name="trayNum"></param>
        /// <param name="mode"></param>
        public void Get_Tray_Position(int trayNum, bool mode)
        {
            double offsetX, offsetY, spaceX, spaceY;
            int dx, dy;
            
            trayNum--;
            Config.TrayStartDir dir = g_config.Tray_start(trayNum);
            dx = dir.dx ? 1 : -1;
            dy = dir.dy ? -1 : 1;
            //行列是否对调
            spaceX = g_config.TrayRotateDir[trayNum] == 0 ? TrayD.Col_Space : TrayD.Row_Space;
            spaceY = g_config.TrayRotateDir[trayNum] == 0 ? TrayD.Row_Space : TrayD.Col_Space;
            if (mode)//取料
            {
                offsetX = (TrayD.TIC_ColN[trayNum] - 1) * spaceX;
                offsetY = (TrayD.TIC_RowN[trayNum] - 1) * spaceY;
               
            }
            else//放料
            {
                offsetX = (TrayD.LIC_ColN[trayNum] - 1) * spaceX;
                offsetY = (TrayD.LIC_RowN[trayNum] - 1) * spaceY;
            }

            Position_X.Buffer = Position_X.trayFirst[trayNum] + offsetX * dx;
            Position_Y.Buffer = Position_Y.trayFirst[trayNum] + offsetY * dy;
        }

		/// <summary>
		/// 获取载体取料位置
		/// </summary>
		public void Get_TakeIC_Position()
		{
			if (Auto_Flag.Brede_TakeIC)
			{
                if (Config.FeederCount == 0)//单飞达
                {
                    TIC_FeederN = 0;
                }
                else if (Config.FeederCount == 1)//双飞达
                {
                    FeederEnable_Check();
                }
                Position_X.Buffer = Position_X.Feeder[TIC_FeederN];
				Position_Y.Buffer = Position_Y.Feeder[TIC_FeederN];
            }
			else if (Auto_Flag.FixedTray_TakeIC || Auto_Flag.AutoTray_TakeIC)
			{
				if (TrayD.TIC_TrayN == 1)
				{
					if (TrayEndFlag.takeLayIC[0] && Auto_Flag.AutoTray_TakeIC && Auto_Flag.AutoTray_LayIC)
					{
						Auto_Flag.Update_Tray = true;
					}
					else
					{
                        Get_Tray_Position(TrayD.TIC_TrayN, true);
					}
				}
				else if (TrayD.TIC_TrayN == 2)
				{
                    Get_Tray_Position(TrayD.TIC_TrayN, true);
                    if (g_config.AutoTrayStart == 1)
                    {
                        if (g_config.TrayRotateDir[1] == 0)
                        {
                            Position_X.Buffer += (TrayD.ColC - 1) * TrayD.Col_Space;
                        }
                        else
                        {
                            Position_X.Buffer += (TrayD.RowC - 1) * TrayD.Row_Space;
                        }
                    }
                }
                else
                {
                    Auto_Flag.Update_Tray = true;
                }
			}
			else if (Auto_Flag.FixedTube_TakeIC)//管装取料
			{
				if (ExistICPenC == 0)
				{
					TIC_TubeN = 0;
				}
				if (Get_Tube_Position())
				{
					TIC_TubeN++;
					if (TIC_TubeN >= UserConfig.TubeC)
					{
						TIC_TubeN = 0;
					}
				}
			}
			Position_X.Buffer = Position_X.Buffer - Pen[TIC_PenN].Offset_TopCamera_X;
			Position_Y.Buffer = Position_Y.Buffer - Pen[TIC_PenN].Offset_TopCamera_Y;
		}

        /// <summary>
        /// 放NG料坐标
        /// </summary>
        public void Get_LayNGIC_Position()
		{
			if (Auto_Flag.NGTray)//NG盘	
			{
				Auto_Flag.NGTrayKeep = true;
                Get_Tray_Position(3, false);
                if (g_config.NGTrayStart == 1)
                {
                    if (g_config.TrayRotateDir[2] == 0)
                    {
                        Position_X.Buffer += (TrayD.ColC - 1) * TrayD.Col_Space;
                    }
                    else
                    {
                        Position_X.Buffer += (TrayD.RowC - 1) * TrayD.Row_Space;
                    }
                }
            }
			else//NG杯
			{
				Auto_Flag.NGTrayKeep = false;
				Position_X.Buffer = Position_X.NGCup;
				Position_Y.Buffer = Position_Y.NGCup;
			}

			Position_X.Buffer = Position_X.Buffer - Pen[LIC_PenN].Offset_TopCamera_X;
			Position_Y.Buffer = Position_Y.Buffer - Pen[LIC_PenN].Offset_TopCamera_Y;
		}

		/// <summary>
		/// 放OK料坐标
		/// </summary>
		public void Get_LayOKIC_Position()
		{
			if (Auto_Flag.Brede_LayIC)
			{
				Position_X.Buffer = Position_X.bredeOut;
				Position_Y.Buffer = Position_Y.bredeOut;
				Position_Y.Buffer = Position_Y.Buffer - Brede_Number.CntVal_Auto * Brede.setConfig.space;
			}
			else if (Auto_Flag.FixedTray_LayIC || Auto_Flag.AutoTray_LayIC)
			{
				if (TrayD.LIC_TrayN == 1 || TrayD.LIC_TrayN == 2)
				{
                    Get_Tray_Position(TrayD.LIC_TrayN, false);
                    if (TrayD.LIC_TrayN == 2)
                    {
                        if (g_config.AutoTrayStart == 1)
                        {
                            if (g_config.TrayRotateDir[1] == 0)
                            {
                                Position_X.Buffer += (TrayD.ColC - 1) * TrayD.Col_Space;
                            }
                            else
                            {
                                Position_X.Buffer += (TrayD.RowC - 1) * TrayD.Row_Space;
                            }
                        }
                    }
                }
			}
			Position_X.Buffer = Position_X.Buffer - Pen[LIC_PenN].Offset_TopCamera_X + Pen[LIC_PenN].Offset_Carrier_X;
			Position_Y.Buffer = Position_Y.Buffer - Pen[LIC_PenN].Offset_TopCamera_Y + Pen[LIC_PenN].Offset_Carrier_Y;
        }

		/// <summary>
		/// 获取放料位置
		/// </summary>
		public void Get_LayIC_Position()
		{
            int dir;
            dir = g_config.Tray1Start == 0 ? 1 : -1;
            //补料盘是满盘
            if (TrayD.TIC_ColN[0] == 1 && TrayD.TIC_RowN[0] == 1)
			{
				Auto_Flag.Update_Tray = true;
			}
			//补料盘是空盘
			else if (TrayD.TIC_ColN[0] == 0 && TrayD.TIC_RowN[0] == 0)
			{
				Position_X.Buffer = Position_X.trayFirst[0] - (TrayD.ColC - 1) *
									TrayD.Col_Space;

				Position_Y.Buffer = Position_Y.trayFirst[0] + (TrayD.RowC - 1) *
									TrayD.Row_Space * dir;
			}
			else
			{
                if (g_config.Tray_Col_Add(0))
                {
                    if (TrayD.TIC_ColN[0] == 1)
                    {
                        Position_X.Buffer = Position_X.trayFirst[0] - (TrayD.ColC - 1) *
                                            TrayD.Col_Space;

                        Position_Y.Buffer = Position_Y.trayFirst[0] + (TrayD.TIC_RowN[0] - 2) *
                                            TrayD.Row_Space * dir;
                    }
                    else
                    {
                        Position_X.Buffer = Position_X.trayFirst[0] - (TrayD.TIC_ColN[0] - 2) *
                                            TrayD.Col_Space;

                        Position_Y.Buffer = Position_Y.trayFirst[0] + (TrayD.TIC_RowN[0] - 1) *
                                            TrayD.Row_Space * dir;
                    }
                }
                else
                {
                    if (TrayD.TIC_RowN[0] == 1)
                    {
                        Position_X.Buffer = Position_X.trayFirst[0] - (TrayD.TIC_ColN[0] - 2) *
                                            TrayD.Col_Space;

                        Position_Y.Buffer = Position_Y.trayFirst[0] + (TrayD.RowC - 1) *
                                            TrayD.Row_Space * dir;
                    }
                    else
                    {
                        Position_X.Buffer = Position_X.trayFirst[0] - (TrayD.TIC_ColN[0] - 1) *
                                            TrayD.Col_Space;

                        Position_Y.Buffer = Position_Y.trayFirst[0] + (TrayD.TIC_RowN[0] - 2) *
                                            TrayD.Row_Space * dir;
                    }
                }
                
			}
		}

		/// <summary>
		/// 获取拍照位置
		/// </summary>
		public void Get_Photograph_Position()
		{
			Position_X.Buffer = Pen[PIC_PenN].LowCamera_X;
			Position_Y.Buffer = Pen[PIC_PenN].LowCamera_Y;
		}
		/// <summary>
		/// 获取3D检测位置
		/// </summary>
		public void Get_Detection_Position()
		{
	        if(Auto_Flag.AutoRunBusy)//烧录座取料后3D检测
	        {
		        while(DIC_PenN < TIC_PenN)
		        {
			        if(Pen[DIC_PenN].DownResult == 1)
			        {
				        Position_X.Buffer = Position_X.detection + Pen[0].Offset_TopCamera_X;
				        Position_Y.Buffer = Position_Y.detection + Pen[0].Offset_TopCamera_Y;
				        break;
			        }
			        else
			        {
				        DIC_PenN ++;				
			        }
		        }
	        }
	        else
			{
				Position_X.Buffer = Position_X.detection + Pen[0].Offset_TopCamera_X;
				Position_Y.Buffer = Position_Y.detection + Pen[0].Offset_TopCamera_Y;
			}
			Position_X.Buffer = Position_X.Buffer - Pen[DIC_PenN].Offset_TopCamera_X;
			Position_Y.Buffer = Position_Y.Buffer - Pen[DIC_PenN].Offset_TopCamera_Y;
		}

        /// <summary>
        /// 飞达使能查询
        /// </summary>
        public void FeederEnable_Check()
        {
            int count = 0;
            while (true)
            {
                if (Run.FeederEnable[TIC_FeederN].Checked)
                {
                    break;
                }
                count++;
                TIC_FeederN++;
                if (TIC_FeederN >= UserConfig.FeederC)
                {
                    TIC_FeederN = 0;
                }
                if (count >= UserConfig.FeederC)
                {
                    TIC_FeederN = 0;
                    break;
                }
            }
        }

        /// <summary>
        /// 吸笔使能查询
        /// </summary>
        public void PenEnable_Check()
        {
            while (TIC_PenN < UserConfig.VacuumPenC)
            {
                if (Pen[TIC_PenN].Enable)
                {
                    break;
                }
                TIC_PenN++;
            }
        }

        /// <summary>
        /// 吸笔交替模式使能查询
        /// </summary>
        public void PenAltEnable_Check()
        {
            while (TIC_PenN < UserConfig.VacuumPenC)
            {
                if (Pen[TIC_PenN].Enable && !Pen[TIC_PenN].ExistIC)
                {
                    break;
                }
                TIC_PenN++;
            }
        }

        /// <summary>
        /// 吸笔拍照查询
        /// </summary>
        /// <returns></returns>
        public bool PhotographPen_Check()
        {
            if (PIC_PenN == UserConfig.VacuumPenC || PIC_PenN == -1)
            {
                return true;
            }
            while (true)
            {
                if (Pen[PIC_PenN].ExistIC)
                {
                    return false;
                }
                PIC_PenN += Auto_Flag.Ascending_ICPos ? 1 : -1;
                if (PIC_PenN == UserConfig.VacuumPenC || PIC_PenN == -1)
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 3D检测吸笔查询
        /// </summary>
        public void DetectionPen_Check()
        {
            while (DIC_PenN < UserConfig.VacuumPenC)
            {
                if (Pen[DIC_PenN].ExistIC)
                {
                    if ((Pen[DIC_PenN].DownResult == 1 && Auto_Flag.Cam_3D_Mode_II) || !Auto_Flag.Cam_3D_Mode_II)
                    {
                        break;
                    }
                }
                DIC_PenN++;
            }
        }

        /// <summary>
        /// 吸笔吸笔放料查询
        /// </summary>
        public void LIC_Pen_Check()
        {
            while (LIC_PenN < UserConfig.VacuumPenC)
            {
                if (!Auto_Flag.PenAlt_Flag && Pen[LIC_PenN].ExistIC)//吸笔非交替方式
                {
                    break;
                }
                if (Auto_Flag.PenAlt_Flag && Pen[LIC_PenN].ExistRawIC)//吸笔交替方式
                {
                    break;
                }
                LIC_PenN++;
            }
        }

        /// <summary>
        /// NG吸笔查询
        /// </summary>
        public void NGPen_Check()
		{
			if (TrayEndFlag.layIC[2])
			{
				Auto_Flag.Update_Tray = true;
			}
			else
			{
				while (true)
				{
					if (Pen[LIC_PenN].DownResult == 2 || Pen[LIC_PenN].DownResult == 3)//NG
					{
                        Get_LayNGIC_Position();
						break;
					}
					LIC_PenN++;
				}
			}
		}

		/// <summary>
		/// OK吸笔查询
		/// </summary>
		public void OKPen_Check()
		{
			while (true)
			{
                if (Pen[LIC_PenN].DownResult == 1)//OK
				{
					Get_LayOKIC_Position();
					break;
				}
				LIC_PenN++;
			}
            if (Auto_Flag.RotateChange)
            {
                double angle = Get_LIC_RotateAngle();
                AllPenRotateTrigge(angle, LIC_PenN);
            }
        }

		/// <summary>
		/// 3D检测NG吸笔查询
		/// </summary>
		public void DNGPen_Check()
		{
			if (TrayEndFlag.layIC[2])
			{
				Auto_Flag.Update_Tray = true;
			}
			else
			{
				while (true)
				{
					if (Pen[LIC_PenN].DetectionResult == 2)//3D检测NG
					{
						Get_LayNGIC_Position();
						break;
					}
					LIC_PenN++;
				}
			}
		}

		/// <summary>
		/// 3D检测OK吸笔查询
		/// </summary>
		public void DOKPen_Check()
		{
			while (LIC_PenN < UserConfig.VacuumPenC)
			{
				if (Pen[LIC_PenN].DetectionResult == 1)//3D检测OK
				{
					break;
				}
				LIC_PenN++;
			}
		}

        /// <summary>
        /// 烧录前2D相机逻辑判断
        /// </summary>
        private void LogicJudgment_Cam_2D_Mode_I()
        {
            if (ExistICPenC > 0)//吸笔有料
            {
                if (Auto_Flag.PenAlt_Flag)
                {
                    Auto_Flag.BurnSeat_TakeIC = true;
                }
                if (Auto_Flag.Enabled_TakeICPos)
                {
                    PhotographLightOn_Handle();
                }
                else if (Vision_3D.Enabled_I)//烧录前3D检测
                {
                    RunState = RUNSTATE.Detection;
                    DIC_PenN = 0;
                }
                else
                {
                    RunState = RUNSTATE.BurnSeat_LayIC;
                    LIC_PenN = 0;
                }
            }
            else//吸笔没有料
            {
                if ((Auto_Flag.Ending || LIC_UnitC == 0) && TIC_UnitC == 0)
                {
                    RunState = RUNSTATE.End;
                }
                else
                {
                    if (Auto_Flag.BurnSeat_Sync)
                    {
                        for (int i = 0; i < Sync_UnitC; i++)
                        {
                            if (Pen[SyncPen[i].PenN].DownTrigger)
                            {
                                Pen[SyncPen[i].PenN].DownTrigger = false;
                                if (Group[SyncPen[i].GroupN].Waiting_To_Burn)
                                {
                                    DownTrigger_Handle(SyncPen[i].GroupN);//烧录触发处理
                                }
                            }
                        }
                    }
                    RunState = RUNSTATE.BurnSeat_TakeIC;
                    Auto_Flag.BurnSeat_TakeIC = true;
                }
            }
        }

        /// <summary>
        /// 烧录后3D检测逻辑判断
        /// </summary>
        /// <param name="flag"></param>
        private void LogicJudgment_Cam_3D_Mode_II(bool flag = true)
        {
            if (ExistICPenC - ExistRawICPenC > 0)//吸笔烧录后的有料
            {
                if (Vision_3D.Enabled_II && OKPenC > 0)//烧录后3D检测
                {
                    RunState = RUNSTATE.Detection;
                    Auto_Flag.Cam_3D_Mode_II = true;
                    DIC_PenN = 0;
                }
                else
                {
                    if (Auto_Flag.Enabled_LayICPos)//烧录后定位拍照
                    {
                        PhotographLightOn_Handle(true);
                    }
                    else if (NGPenC > 0)//吸笔有NG料
                    {
                        RunState = RUNSTATE.Carrier_LayNGIC;
                        LIC_PenN = 0;
                    }
                    else//吸笔没有NG料
                    {
                        RunState = RUNSTATE.Carrier_LayOKIC;
                        if (Auto_Flag.Brede_LayIC)
                        {
                            Brede_Flag.ALarmCheck = true;
                        }
                        LIC_PenN = 0;
                    }
                }
            }
            else//吸笔没有烧录后的料
            {
                if (!Auto_Flag.Ending && flag)//补料到烧录座
                {
                    TIC_PenN = 0;
                    RunState = RUNSTATE.Carrier_TakeIC;
                }
                else if (Auto_Flag.PenAlt_Flag && ExistRawICPenC != 0)//交替方式吸笔有原始料
                {
                    RunState = RUNSTATE.BurnSeat_LayIC;
                }
                else if (TIC_UnitC == 0)//不需要补料到烧录座，烧录座没有料
                {
                    RunState = RUNSTATE.End;
                }
                else//如果不需要补料到烧录座，烧录座有料，继续到烧录座取料
                {
                    RunState = RUNSTATE.BurnSeat_TakeIC;
                } 
            }
        }

        /// <summary>
        /// 获取水平方向位置
        /// </summary>
        public void Get_Horizontal_Position_I()
		{
			double angleVal;

			switch (RunState)
			{
				case RUNSTATE.BurnSeat_TakeIC:
                    if (Auto_Flag.BurnSeat_TakeIC)
                    {
                        TIC_PenN = 0;
                    }
                    if (Auto_Flag.BurnSeat_Sync)
                    {
					    if (SyncTakeIC_Seat_Check())
                        {
                            Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                            if (Auto_Flag.BurnSeat_TakeIC)
                            {
                                Auto_Flag.BurnSeat_TakeIC = false;
                                AllPenRotateTrigge(0d);
                            }
                        } 
                    }
                    else
                    {
                        PenEnable_Check();
                        TakeIC_Seat_Check();
                        if (Auto_Flag.Seat_EndTakeIC)//获取取料座坐标失败
                        {
                            Auto_Flag.Seat_EndTakeIC = false;
                            LogicJudgment_Cam_3D_Mode_II();
                        }
                        else
                        {
                            Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                            if (Auto_Flag.BurnSeat_TakeIC)
                            {
                                Auto_Flag.BurnSeat_TakeIC = false;
                                SingleTIC_UnitC = 0;
                                AllPenRotateTrigge(0d);
                            }
                        }
                    }
					break;

				case RUNSTATE.Carrier_TakeIC:
                    if (Auto_Flag.BurnSeat_Sync && !TrayEndFlag.tray2Burn)
                    {
                        while (IC_SupplyC != 0)
                        {
                            if (SyncPen[TIC_PenN].Flag_LayIC)
                            {
                                break;
                            }
                            TIC_PenN++;
                        }
                    }
                    else
                    {
                        PenEnable_Check();
                    }
                    if (TrayEndFlag.tray2Burn)
					{
						Get_TakeIC_Position();//获取载体取料位置
						Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
						if (ExistICPenC == 0)
						{
                            double angle = Get_TIC_RotateAngle();
                            AllPenRotateTrigge(angle);
                        }
					}
					else
					{
						if (ExistICPenC == 0)
						{
                            TIC_FeederN = 0;
                            if (!Auto_Flag.BurnSeat_Sync)
                            {
                                IC_SupplyC = 0;
                                Get_SupplyIC_Sum();
                            }
						}
						if (IC_SupplyC == 0 || Auto_Flag.Ending)//没可以放料的座子或收尾
						{
                            LogicJudgment_Cam_2D_Mode_I();
                        }
						else//有可以放料的座子
						{
							Get_TakeIC_Position();//获取载体取料位置
							if (!Auto_Flag.Update_Tray && !Auto_Flag.Tube_AllEmptying)
							{
								Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
							}
							if (Auto_Flag.Get_Horizontal)
							{
								if (ExistICPenC == 0 || Auto_Flag.RotateChange)
								{
                                    double angle = Get_TIC_RotateAngle();
                                    AllPenRotateTrigge(angle, TIC_PenN);
                                }
							}
						}
					}
					break;

				case RUNSTATE.BurnSeat_LayIC:
                    Auto_Flag.Get_Horizontal = false;
                    if (Auto_Flag.BurnSeat_Sync)
                    {
						if (Auto_Flag.OverDeviation)
						{
							while (SyncPenN < Sync_UnitC)
							{
								if (SyncPen[SyncPenN].OverDeviation)
								{
									GetSocketXY(SyncPen[SyncPenN].GroupN, SyncPen[SyncPenN].UnitN, SyncPen[SyncPenN].PenN, rectify[SyncPen[SyncPenN].PenN]);
									Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
									break;
								}
								SyncPenN++;
							}
							if (!Auto_Flag.Get_Horizontal)
							{
								Auto_Flag.OverDeviation = false;
								if (NGPenC > 0)
								{
									RunState = RUNSTATE.Carrier_LayNGIC;
									LIC_PenN = 0;
								}
								else if (IC_SupplyC == 0 || Auto_Flag.Ending)
								{
									for (int i = 0; i < Sync_UnitC; i++)
									{
										if (Pen[SyncPen[i].PenN].DownTrigger)
										{
											Pen[SyncPen[i].PenN].DownTrigger = false;
											DownTrigger_Handle(SyncPen[i].GroupN);//烧录触发处理
										}
									}
									RunState = RUNSTATE.BurnSeat_TakeIC;
									Auto_Flag.BurnSeat_TakeIC = true;
								}
								else
								{
                                    RunState = RUNSTATE.Carrier_TakeIC;
								}
								TIC_PenN = 0;
							}
						}
                        else
                        {
							for (int i = 0; i < Sync_UnitC; i++)
							{
								if (Pen[SyncPen[i].PenN].ExistIC)
								{
									GetSocketXY(SyncPen[i].GroupN, SyncPen[i].UnitN, SyncPen[i].PenN);
									break;
								}
							}
							Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
						}
                    }
                    else
                    {
                        if (Vision_3D.Enabled_I)//烧录前3D检测
                        {
                            DOKPen_Check();
                        }
                        else
                        {
                            LIC_Pen_Check();
                        }
                        if (Config.CCDModel == 0)//定拍
                        {
                            LayIC_Seat_Check();
                            Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                        }
                        else//飞拍
                        {
                            if (LIC_PenN == UserConfig.VacuumPenC)//查询完最后的吸笔没有料
                            {
                                if (NGPenC > 0)//吸笔有料
                                {
                                    RunState = RUNSTATE.Carrier_LayNGIC;
                                    LIC_PenN = 0;
                                }
                                else
                                {
                                    RunState = RUNSTATE.BurnSeat_TakeIC;
                                    Auto_Flag.BurnSeat_TakeIC = true;
                                }
                            }
                            else
                            {
                                if (!Pen[LIC_PenN].Image_Busy)
                                {
                                    if (Pen[LIC_PenN].ImageResult)
                                    {
                                        LayIC_Seat_Check();
                                        Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                                    }
                                    else
                                    {
                                        Pen[LIC_PenN].DownResult = 2;
                                        RotateTrigge(true, LIC_PenN, RotateAngle.LIC_Tray[2]);
                                        if (Pen[LIC_PenN].ExistRawIC)
                                        {
                                            Pen[LIC_PenN].ExistRawIC = false;
                                            ExistRawICPenC--;
                                        }
                                        NGPenC++;
                                        LIC_PenN++;
                                        IC_SupplyC++;
                                        if (Auto_Flag.Ending && !Auto_Flag.ForceEnd)
                                        {
                                            Auto_Flag.Ending = false;
                                        }

                                        if (Auto_Flag.Production && Auto_Flag.ProductionOK)
                                        {
                                            TIC_ValidC--;
                                        }
                                    }
                                }
                            }
                        }
					}
					break;

				case RUNSTATE.Burn_Again:
					LayIC_Seat_Check();
					if (Auto_Flag.Seat_EndLayIC)//获取取放料座坐标失败
					{
						Auto_Flag.Seat_EndLayIC = false;
                        RotateTrigge(true, TIC_PenN, RotateAngle.LIC_Tray[2]);
						NGPenC++;
                        if (ExistICPenC == EnalePenC)//最后一个使能吸笔
                        {
							RunState = RUNSTATE.Carrier_LayNGIC;
							LIC_PenN = 0;
						}
						else
						{
							if (Auto_Flag.Seat_EndTakeIC)//获取取料座坐标失败
							{
								Auto_Flag.Seat_EndTakeIC = false;
								RunState = RUNSTATE.Carrier_LayNGIC;
								LIC_PenN = 0;
							}
							else
							{
								RunState = RUNSTATE.BurnSeat_TakeIC;
							}
						}
					}
					else
					{
						Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
					}
					break;

				case RUNSTATE.Carrier_LayNGIC:
					NGPen_Check();
                    if ((!Pen[LIC_PenN].Image_Busy && Config.CCDModel != 0) || Config.CCDModel == 0)
                    {
                        if (!Auto_Flag.Update_Tray)
                        {
                            Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                            if (Auto_Flag.Enabled_LayICPos)
                            {
                                Position_X.Buffer = Position_X.Buffer + rectify[LIC_PenN].AxisX;
                                Position_Y.Buffer = Position_Y.Buffer + rectify[LIC_PenN].AxisY;
                            }
                        }
                    }
					break;

				case RUNSTATE.Carrier_LayOKIC:
					OKPen_Check();
                    if ((!Pen[LIC_PenN].Image_Busy && Config.CCDModel != 0) || Config.CCDModel == 0)
                    {
                        Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                        if (Auto_Flag.Enabled_LayICPos)
                        {
                            Position_X.Buffer = Position_X.Buffer + rectify[LIC_PenN].AxisX;
                            Position_Y.Buffer = Position_Y.Buffer + rectify[LIC_PenN].AxisY;
                        }
                    }
					break;

				case RUNSTATE.Photograph:
                    if (Config.CCDModel == 0)//定拍
                    {
                        Get_Photograph_Position();
                    }
                    else//飞拍
                    {
                        int temp = Auto_Flag.Ascending_ICPos ? -1 : 1;
                        Position_X.Buffer = Pen[PIC_PenN].LowCamera_X + temp * 10.0d;
                        Position_Y.Buffer = Pen[0].LowCamera_Y;
                    }
					Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
					break;

				case RUNSTATE.Detection:
                    if (DIC_PenN == 0)
                    {
                        DetectionPen_Check();
                    }
                    Get_Detection_Position();
                    Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                    break;

                case RUNSTATE.Emptying:
                    if (Auto_Flag.BurnSeat_Sync)
                    {
                        GetSocketXY(SyncPen[SyncPenN].GroupN, SyncPen[SyncPenN].UnitN);
                        Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                    }
                    break;

                case RUNSTATE.Carrier_LayDNGIC:
					DNGPen_Check();
					if (!Auto_Flag.Update_Tray)
					{
						Auto_Flag.Get_Horizontal = true;//获取水平坐标成功			
					}
					break;

                case RUNSTATE.Carrier_LayIC:
                    LIC_Pen_Check();
                    Get_LayIC_Position();
					if (!Auto_Flag.Update_Tray)
					{
						Position_X.Buffer = Position_X.Buffer - Pen[LIC_PenN].Offset_TopCamera_X;
						Position_Y.Buffer = Position_Y.Buffer - Pen[LIC_PenN].Offset_TopCamera_Y;
						Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
					}
					break;

                case RUNSTATE.ScanCode:
                    Position_X.Buffer = Group[Scan_GroupN].Unit[Scan_UnitN].TopCamera_X - Scanner.Offset_TopCamera_X;
                    Position_Y.Buffer = Group[Scan_GroupN].Unit[Scan_UnitN].TopCamera_Y - Scanner.Offset_TopCamera_Y;
                    Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                    break;

                case RUNSTATE.TakeSNGIC:
                    PenEnable_Check();
                    GetSocketXY(Scan_GroupN, Scan_UnitN, TIC_PenN);
                    Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                    break;

                case RUNSTATE.End:
					Run.RunStep = RUNSTEP.MASTERLOGIC;
					break;

				default:
					break;
			}
		}

        /// <summary>
        /// 吸笔交替模式获取水平方向位置
        /// </summary>
        public void Get_Horizontal_Position_II()
        {
            double angleVal;

            switch (RunState)
            {
                case RUNSTATE.BurnSeat_TakeIC:
                    if (Auto_Flag.BurnSeat_TakeIC)
                    {
                        TIC_PenN = ExistRawICPenC == 0 ? 0 : Unit_FirstPenN;
                    }
                    //if (Auto_Flag.BurnSeat_Sync)
                    //{
                        
                    //}
                    //else
                    {
                        PenEnable_Check();
                        if (TIC_PenN < UserConfig.VacuumPenC && TakeIC_Seat_Check_II())
                        {
                            Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                            if (Auto_Flag.BurnSeat_TakeIC)
                            {
                                Auto_Flag.BurnSeat_TakeIC = false;
                                SingleTIC_UnitC = 0;
                                AllPenRotateTrigge(0d, TIC_PenN);
                            }
                        }
                        else if (TIC_PenN >= UserConfig.VacuumPenC || Auto_Flag.Seat_EndTakeIC || (Auto_Flag.Ending && TIC_UnitC == 0))//获取取料座坐标失败
                        {
                            Auto_Flag.Seat_EndTakeIC = false;
                            LogicJudgment_Cam_3D_Mode_II();
                        }
                    }
                    break;

                case RUNSTATE.Carrier_TakeIC:
                    if (TrayEndFlag.tray2Burn)
                    {
                        PenAltEnable_Check();
                        Get_TakeIC_Position();//获取载体取料位置
                        Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                        if (ExistICPenC == 0)
                        {
                            double angle = Get_TIC_RotateAngle();
                            AllPenRotateTrigge(angle);
                        }
                    }
                    else
                    {
                        if (ExistRawICPenC == 0)
                        {
                            TIC_FeederN = 0;
                        }
                        if (ExistRawICPenC == EnaleRawICPenC || Auto_Flag.Ending)
                        {
                            if (Auto_Flag.RotateChange)
                            {
                                Auto_Flag.RotateChange = false;
                            }
                            LogicJudgment_Cam_2D_Mode_I();
                        }
                        else
                        {
                            Get_TakeIC_Position();//获取载体取料位置
                            if (!Auto_Flag.Update_Tray && !Auto_Flag.Tube_AllEmptying)
                            {
                                Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                            }
                            if (Auto_Flag.Get_Horizontal)
                            {
                                if (ExistICPenC == 0 || Auto_Flag.RotateChange)
                                {
                                    double angle = Get_TIC_RotateAngle();
                                    AllPenRotateTrigge(angle, TIC_PenN);
                                }
                            }
                        }
                    }
                    break;

                case RUNSTATE.BurnSeat_LayIC:
                    Auto_Flag.Get_Horizontal = false;
                    if (Vision_3D.Enabled_I)//烧录前3D检测
                    {
                        DOKPen_Check();
                    }
                    else
                    {
                        LIC_Pen_Check();
                    }
                    if (Config.CCDModel == 0)//定拍
                    {
                        LayIC_Seat_Check_II();
                        if (Auto_Flag.Seat_EndLayIC)//获取取放料座坐标失败
                        {
                            RunState = RUNSTATE.BurnSeat_TakeIC;
                            Auto_Flag.Seat_EndLayIC = false;
                        }
                        else
                        {
                            Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                        }
                    }
                    else//飞拍
                    {
                        if (LIC_PenN == UserConfig.VacuumPenC)//查询完最后的吸笔没有料
                        {
                            if (NGPenC > 0)//吸笔有料
                            {
                                RunState = RUNSTATE.Carrier_LayNGIC;
                                LIC_PenN = 0;
                            }
                            else
                            {
                                RunState = RUNSTATE.BurnSeat_TakeIC;
                                Auto_Flag.BurnSeat_TakeIC = true;
                            }
                        }
                        else
                        {
                            if (!Pen[LIC_PenN].Image_Busy)
                            {
                                if (Pen[LIC_PenN].ImageResult)
                                {
                                    LayIC_Seat_Check_II();
                                    if (Auto_Flag.Seat_EndLayIC)//获取取放料座坐标失败
                                    {
                                        Auto_Flag.Seat_EndLayIC = false;
                                        RunState = RUNSTATE.BurnSeat_TakeIC;
                                    }
                                    else
                                    {
                                        Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                                    }
                                }
                                else
                                {
                                    Pen[LIC_PenN].DownResult = 2;
                                    RotateTrigge(true, LIC_PenN, RotateAngle.LIC_Tray[2]);
                                    if (Pen[LIC_PenN].ExistRawIC)
                                    {
                                        Pen[LIC_PenN].ExistRawIC = false;
                                        ExistRawICPenC--;
                                    }
                                    NGPenC++;
                                    LIC_PenN++;
                                    IC_SupplyC++;
                                    if (Auto_Flag.Ending && !Auto_Flag.ForceEnd)
                                    {
                                        Auto_Flag.Ending = false;
                                    }

                                    if (Auto_Flag.Production && Auto_Flag.ProductionOK)
                                    {
                                        TIC_ValidC--;
                                    }
                                }
                            }
                        }
                    }
                    break;

                case RUNSTATE.Burn_Again:
                    LayIC_Seat_Check_II();
                    if (Auto_Flag.Seat_EndLayIC)//获取取放料座坐标失败
                    {
                        RotateTrigge(true, TIC_PenN, RotateAngle.LIC_Tray[2]);
                        NGPenC++;
                        Auto_Flag.Seat_EndLayIC = false;
                    }
                    else
                    {
                        Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                    }
                    break;

                case RUNSTATE.Carrier_LayNGIC:
                    NGPen_Check();
                    if ((!Pen[LIC_PenN].Image_Busy && Config.CCDModel != 0) || Config.CCDModel == 0)
                    {
                        if (!Auto_Flag.Update_Tray)
                        {
                            Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                            if (Auto_Flag.Enabled_LayICPos)
                            {
                                Position_X.Buffer = Position_X.Buffer + rectify[LIC_PenN].AxisX;
                                Position_Y.Buffer = Position_Y.Buffer + rectify[LIC_PenN].AxisY;
                            }
                        }
                    }
                    break;

                case RUNSTATE.Carrier_LayOKIC:
                    OKPen_Check();
                    if ((!Pen[LIC_PenN].Image_Busy && Config.CCDModel != 0) || Config.CCDModel == 0)
                    {
                        Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                        if (Auto_Flag.Enabled_LayICPos)
                        {
                            Position_X.Buffer = Position_X.Buffer + rectify[LIC_PenN].AxisX;
                            Position_Y.Buffer = Position_Y.Buffer + rectify[LIC_PenN].AxisY;
                        }
                    }
                    break;

                case RUNSTATE.Photograph:
                    if (Config.CCDModel == 0)//定拍
                    {
                        Get_Photograph_Position();
                    }
                    else//飞拍
                    {
                        int temp = Auto_Flag.Ascending_ICPos ? -1 : 1;
                        Position_X.Buffer = Pen[PIC_PenN].LowCamera_X + temp * 10.0d;
                        Position_Y.Buffer = Pen[0].LowCamera_Y;
                    }
                    Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                    break;

                case RUNSTATE.Detection:
                    if (DIC_PenN == 0)
                    {
                        DetectionPen_Check();
                    }
                    Get_Detection_Position();
                    Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                    break;

                case RUNSTATE.Emptying:
                    if (Auto_Flag.BurnSeat_Sync)
                    {
                        GetSocketXY(SyncPen[SyncPenN].GroupN, SyncPen[SyncPenN].UnitN);
                        Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                    }
                    break;

                case RUNSTATE.Carrier_LayDNGIC:
                    DNGPen_Check();
                    if (!Auto_Flag.Update_Tray)
                    {
                        Auto_Flag.Get_Horizontal = true;//获取水平坐标成功			
                    }
                    break;

                case RUNSTATE.Carrier_LayIC:
                    LIC_Pen_Check();
                    Get_LayIC_Position();
                    if (!Auto_Flag.Update_Tray)
                    {
                        Position_X.Buffer = Position_X.Buffer - Pen[LIC_PenN].Offset_TopCamera_X + Pen[LIC_PenN].Offset_Carrier_X;
                        Position_Y.Buffer = Position_Y.Buffer - Pen[LIC_PenN].Offset_TopCamera_Y + Pen[LIC_PenN].Offset_Carrier_Y;
                        Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                    }
                    break;

                case RUNSTATE.ScanCode:
                    Position_X.Buffer = Group[Scan_GroupN].Unit[Scan_UnitN].TopCamera_X - Scanner.Offset_TopCamera_X;
                    Position_Y.Buffer = Group[Scan_GroupN].Unit[Scan_UnitN].TopCamera_Y - Scanner.Offset_TopCamera_Y;
                    Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                    break;

                case RUNSTATE.TakeSNGIC:
                    PenEnable_Check();
                    GetSocketXY(Scan_GroupN, Scan_UnitN, TIC_PenN);
                    Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                    break;

                case RUNSTATE.End:
                    Run.RunStep = RUNSTEP.MASTERLOGIC;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
		/// 转移模式获取水平方向位置
		/// </summary>
		public void Get_Horizontal_Position_III()
        {
            switch (RunState)
            {
                case RUNSTATE.Carrier_TakeIC:
                    PenEnable_Check();
                    Get_TakeIC_Position();//获取载体取料位置
                    if (!Auto_Flag.Update_Tray && !Auto_Flag.Tube_AllEmptying)
                    {
                        Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                    }
                    if (Auto_Flag.Get_Horizontal)
                    {
                        if (ExistICPenC == 0 || Auto_Flag.RotateChange)
                        {
                            double angle = Get_TIC_RotateAngle();
                            AllPenRotateTrigge(angle, TIC_PenN);
                        }
                    }
                    break;

                case RUNSTATE.Photograph:
                    if (Config.CCDModel == 0)//定拍
                    {
                        Get_Photograph_Position();
                    }
                    else//飞拍
                    {
                        int temp = Auto_Flag.Ascending_ICPos ? -1 : 1;
                        Position_X.Buffer = Pen[PIC_PenN].LowCamera_X + temp * 10.0d;
                        Position_Y.Buffer = Pen[0].LowCamera_Y;
                    }
                    Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                    break;

                case RUNSTATE.Carrier_LayOKIC:
                    OKPen_Check();
                    if ((!Pen[LIC_PenN].Image_Busy && Config.CCDModel != 0) || Config.CCDModel == 0)
                    {
                        Auto_Flag.Get_Horizontal = true;//获取水平坐标成功
                        if (Auto_Flag.Enabled_TakeICPos)
                        {
                            Position_X.Buffer = Position_X.Buffer + rectify[LIC_PenN].AxisX;
                            Position_Y.Buffer = Position_Y.Buffer + rectify[LIC_PenN].AxisY;
                        }
                    }
                    break;

                case RUNSTATE.End:
                    Run.RunStep = RUNSTEP.MASTERLOGIC;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 获取垂直方向位置
        /// </summary>
        public void Get_Vertical_Position()
		{
			switch (RunState)
			{
				case RUNSTATE.BurnSeat_TakeIC:
                case RUNSTATE.TakeSNGIC:
                    if (Config.Altimeter != 0)
                    {
                        if (RunState == RUNSTATE.BurnSeat_TakeIC)
                        {
                            HeightVal.Buffer = Group[TIC_GroupN].Unit[TIC_UnitN].HeightVal - Altimeter.Thickness;
                        }
                        else
                        {
                            HeightVal.Buffer = Group[Scan_GroupN].Unit[Scan_UnitN].HeightVal;
                        }

                    }
                    else
                    {
                        HeightVal.Buffer = HeightVal.DownSeat_TakeIC;//计算高度脉冲
                    }   
					WorkPenN = TIC_PenN;
					break;

				case RUNSTATE.Carrier_TakeIC:
					if (Auto_Flag.Brede_TakeIC)
					{
                        if (Config.Altimeter != 0)
                        {
                            HeightVal.Buffer = HeightVal_Altimeter.BredeIn;
                        }
                        else
                        {
                            HeightVal.Buffer = HeightVal.Brede_TakeIC;//计算高度脉冲
                        }  
					}
					else if (Auto_Flag.FixedTray_TakeIC || Auto_Flag.AutoTray_TakeIC)
					{
                        if (Config.Altimeter != 0)
                        {
                            HeightVal.Buffer = HeightVal_Altimeter.Tray;
                        }
                        else
                        {
                            HeightVal.Buffer = HeightVal.Tray_TakeIC;//计算高度脉冲
                        }     
					}
					else if (Auto_Flag.FixedTube_TakeIC)
					{
                        if (Config.Altimeter != 0)
                        {
                            HeightVal.Buffer = HeightVal_Altimeter.TubeIn;
                        }
                        else
                        {
                            HeightVal.Buffer = HeightVal.Tube_TakeIC;//计算高度脉冲
                        } 
					}
					WorkPenN = TIC_PenN;
					break;

				case RUNSTATE.BurnSeat_LayIC:
                case RUNSTATE.Burn_Again:
                    if (Config.Altimeter != 0)
                    {
                        if (Auto_Flag.OverDeviation)
                        {
							HeightVal.Buffer = Group[SyncPen[SyncPenN].GroupN].Unit[SyncPen[SyncPenN].UnitN].HeightVal - Altimeter.Thickness - Altimeter.HeightDifference;
						}
                        else
                        {
							HeightVal.Buffer = Group[LIC_GroupN].Unit[LIC_UnitN].HeightVal - Altimeter.Thickness - Altimeter.HeightDifference;
						}
                    }
                    else
                    {
                        HeightVal.Buffer = HeightVal.DownSeat_LayIC;//计算高度脉冲
                    }
                    int temp = RunState == RUNSTATE.Burn_Again ? TIC_PenN : LIC_PenN;
                    WorkPenN = Auto_Flag.OverDeviation ? SyncPen[SyncPenN].PenN : temp;
					break;

				case RUNSTATE.Carrier_LayNGIC:
                case RUNSTATE.Carrier_LayDNGIC:
                    if (Auto_Flag.NGTrayKeep)
					{
                        if (Config.Altimeter != 0)
                        {
                            HeightVal.Buffer = HeightVal_Altimeter.Tray - Altimeter.HeightDifference;
                        }
                        else
                        {
                            HeightVal.Buffer = HeightVal.Tray_LayIC;//计算高度脉冲
                        }
					}
					else
					{
                        if (Config.Altimeter != 0)
                        {
                            HeightVal.Buffer = HeightVal_Altimeter.BredeOut - Altimeter.HeightDifference;
                        }
                        else
                        {
                            HeightVal.Buffer = HeightVal.Brede_LayIC;//计算高度脉冲
                        }
					}
					WorkPenN = LIC_PenN;
					break;

				case RUNSTATE.Carrier_LayOKIC:
					if (Auto_Flag.LearnBusy)
					{
                        if (Config.Altimeter != 0)
                        {
                            if (Auto_Flag.Brede_TakeIC)
                            {
                                HeightVal.Buffer = HeightVal_Altimeter.BredeIn - Altimeter.HeightDifference;//计算高度脉冲
                            }
                            else if (Auto_Flag.FixedTube_TakeIC)
                            {
                                HeightVal.Buffer = HeightVal_Altimeter.BredeOut - Altimeter.HeightDifference;//计算高度脉冲
                            }
                            else if (Auto_Flag.FixedTray_TakeIC || Auto_Flag.AutoTray_TakeIC)
                            {
                                HeightVal.Buffer = HeightVal_Altimeter.Tray - Altimeter.HeightDifference;
                            }
                        }
                        else
                        {
                            if (Auto_Flag.Brede_TakeIC)
                            {
                                HeightVal.Buffer = HeightVal.Brede_TakeIC - Altimeter.HeightDifference;//计算高度脉冲
                            }
                            else if (Auto_Flag.FixedTube_TakeIC)
                            {
                                HeightVal.Buffer = HeightVal.Brede_LayIC;//计算高度脉冲
                            }
                            else if (Auto_Flag.FixedTray_TakeIC || Auto_Flag.AutoTray_TakeIC)
                            {
                                HeightVal.Buffer = HeightVal.Tray_LayIC;//计算高度脉冲
                            }
                        }
					}
					else
					{
                        if (Config.Altimeter != 0)
                        {
                            if (Auto_Flag.Brede_LayIC)
                            {
                                HeightVal.Buffer = HeightVal_Altimeter.BredeOut - Altimeter.HeightDifference;//计算高度脉冲
                            }
                            else if (Auto_Flag.FixedTray_LayIC || Auto_Flag.AutoTray_LayIC)
                            {
                                HeightVal.Buffer = HeightVal_Altimeter.Tray - Altimeter.HeightDifference;
                            }
                        }
                        else
                        {
                            if (Auto_Flag.Brede_LayIC)
                            {
                                HeightVal.Buffer = HeightVal.Brede_LayIC;//计算高度脉冲
                            }
                            else if (Auto_Flag.FixedTray_LayIC || Auto_Flag.AutoTray_LayIC)
                            {
                                HeightVal.Buffer = HeightVal.Tray_LayIC;//计算高度脉冲
                            }
                        }   
					}
					WorkPenN = LIC_PenN;
					break;

				case RUNSTATE.Detection:
					HeightVal.Buffer = Vision_3D.Z;//计算高度脉冲
					WorkPenN = DIC_PenN;
					break;

				case RUNSTATE.Carrier_LayIC:
                    if (Config.Altimeter != 0)
                    {
                        HeightVal.Buffer = HeightVal_Altimeter.Tray - Altimeter.HeightDifference;
                    }
                    else
                    {
                        HeightVal.Buffer = HeightVal.Tray_LayIC;//计算高度脉冲
                    }
					WorkPenN = LIC_PenN;
					break;

				default:
					break;
			}

            if (Config.Altimeter != 0)
            {
                if (RunState != RUNSTATE.Detection && !Auto_Flag.LearnBusy)
                {
                    HeightVal.Buffer -= Pen[WorkPenN].Altimeter_Z;
                }
            }
		}

		/// <summary>
		/// 取料处理
		/// </summary>
		/// <param name="time"></param>
		public void TakeIC_Handle(int pen, UInt32 time)
		{
			switch (TIC_Step)
			{
				case 1:
					In_Output.vacuumO[pen].M = true;
					AutoTimer.VacuumDuration = GetSysTime() + time;
					TIC_Step = 2;
					break;

				case 2:
					if (GetSysTime() > AutoTimer.VacuumDuration)
					{
						TIC_Step = 0;
					}
					break;

				default:
					break;
			}
		}

		/// <summary>
		/// 放料处理
		/// </summary>
		public void LayIC_Handle(int pen)
		{
			switch (LIC_Step)
			{
				case 1:
					In_Output.vacuumO[pen].M = false;
					AutoTimer.BlowDelay = GetSysTime() + AutoTiming.BlowDelay;
					LIC_Step = 2;
					break;

				case 2:
					if (GetSysTime() > AutoTimer.BlowDelay)
					{
						In_Output.blowO[pen].M = true;
						AutoTimer.BlowDuration = GetSysTime() + AutoTiming.BlowDuration;
						LIC_Step = 3;
					}
					break;

				case 3:
					if (GetSysTime() > AutoTimer.BlowDuration)
					{
						In_Output.blowO[pen].M = false;
						LIC_Step = 0;
					}
					break;

				default:
					break;
			}
		}

		/// <summary>
		/// 获取取料容量
		/// </summary>
		/// <param name="pValue"></param>
		/// <returns></returns>
		public short Get_TakeIC_Capacity(out int pValue)
		{
            TrayState trayState = new TrayState();
            int trayLIC_Capacity = 0, temp = 0;
			pValue = 0;
			if (Auto_Flag.FixedTray_LayIC)
			{
				if (TrayEndFlag.layIC[0] || TrayEndFlag.layIC[1])
				{
                    int k = TrayD.LIC_TrayN - 1;
                    trayLIC_Capacity = TrayD.ColC * TrayD.RowC - trayState.Get_RectangleF_ID(k, TrayD.LIC_RowN[k], TrayD.LIC_ColN[k]);
				}
				else
				{
					trayLIC_Capacity = 2 * TrayD.ColC * TrayD.RowC -
                    trayState.Get_RectangleF_ID(0, TrayD.LIC_RowN[0], TrayD.LIC_ColN[0]) -
                    trayState.Get_RectangleF_ID(1, TrayD.LIC_RowN[1], TrayD.LIC_ColN[1]);
				}
			}
			else if (Auto_Flag.AutoTray_LayIC)
			{
                //trayLIC_Capacity = TrayD.ColC * TrayD.RowC - trayState.Get_RectangleF_ID(1, TrayD.LIC_RowN[1], TrayD.LIC_ColN[1]);
                var a = trayState.Get_RectangleF_ID(1, TrayD.TIC_EndRowN[1], TrayD.TIC_EndColN[1]) + 1;
                trayLIC_Capacity = a - trayState.Get_RectangleF_ID(1, TrayD.LIC_RowN[1], TrayD.LIC_ColN[1]);
            }

			if (Auto_Flag.FixedTray_LayIC || Auto_Flag.AutoTray_LayIC)
			{
				pValue = trayLIC_Capacity - TIC_UnitC - ExistICPenC;
				if (pValue < 0)
				{
					pValue = 0;
					return 1;
				}
			}

			if (Auto_Flag.Production)
			{
				temp = TargetC - TIC_ValidC;
				if (temp < 0)
				{
					pValue = 0;
					return 1;
				}
				if (Auto_Flag.Brede_LayIC)
				{
					pValue = temp;
				}
				else if (Auto_Flag.FixedTray_LayIC || Auto_Flag.AutoTray_LayIC)
				{
					if (pValue > temp)
					{
						pValue = temp;
					}
				}
			}
			else
			{
				if (Auto_Flag.Brede_LayIC)
				{
					pValue = 5000;
					return 2;
				}
			}
			return 0;
		}

		/// <summary>
		/// 载体取料数据运算
		/// </summary>
		public void Carrier_TakeIC_Data_Operation()
		{
			string str = null;
            int rowC, colC, end_row, end_col;        
            if (Auto_Flag.BurnMode)
            {
                if (TrayEndFlag.tray2Burn)
                {
                    RotateTrigge(true, TIC_PenN, RotateAngle.TIC_Tray[0]);
                }
                else
                {
                    RotateTrigge(true, TIC_PenN, 0d);
                }
            }
            else
            {
                RotateTrigge(true, TIC_PenN, Get_LIC_RotateAngle());
                Pen[TIC_PenN].DownResult = 1;
                OKPenC++;
            }
            Pen[TIC_PenN].ExistRawIC = true;
            Pen[TIC_PenN].ImageResult = true;
            rectify[TIC_PenN].AxisX = 0;
            rectify[TIC_PenN].AxisY = 0;
            ExistRawICPenC++;
            ExistICPenC++;
            TIC_PenN++;
			if (Auto_Flag.Brede_TakeIC)
			{
				if (!Auto_Flag.TestMode)
				{
                    if (Config.FeederCount == 0)//单飞达
                    {
                        Brede.Send_Cmd(Brede.Cmd_SendMaterial);//编带模块飞达触发
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "编带模块飞达触发", "Flow");
                        AutoTimer.BredeTakeDelay = GetSysTime() + AutoTiming.BredeTakeDelay;
                        if (FeederIO)
                        {
                            FeederTrigge(0);
                        }
                    }
                    else if (Config.FeederCount == 1)//双飞达
                    {
                        FeederTrigge(TIC_FeederN);
                        TIC_FeederN++;
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "飞达[" + TIC_FeederN + "]触发", "Flow");
                        if (TIC_FeederN >= UserConfig.FeederC)
                        {
                            TIC_FeederN = 0;
                        }
                    }

                }
				str = "吸头[" + TIC_PenN + "]到飞达取料成功";
			}
			else if (Auto_Flag.FixedTray_TakeIC || Auto_Flag.AutoTray_TakeIC)
			{
                if (TrayD.TIC_TrayN == 2 && g_config.TrayRotateDir[TrayD.TIC_TrayN - 1] != 0)
                {
                    rowC = TrayD.ColC;
                    colC = TrayD.RowC;
                }
                else
                {
                    rowC = TrayD.RowC;
                    colC = TrayD.ColC;
                }
                if (MultiLanguage.IsEnglish())
                {
                    str = "Pen" + TIC_PenN + " successfully take IC from the " + TrayD.TIC_RowN[TrayD.TIC_TrayN - 1] + " row, " + TrayD.TIC_ColN[TrayD.TIC_TrayN - 1] + " column of the tray" + TrayD.TIC_TrayN;
                }
                else
                {
                    str = "吸头" + TIC_PenN + "到料盘" + TrayD.TIC_TrayN + "第" + TrayD.TIC_RowN[TrayD.TIC_TrayN - 1] + "行第" + TrayD.TIC_ColN[TrayD.TIC_TrayN - 1] + "列取料成功";
                }
				
				if ((TrayD.TIC_ColN[TrayD.TIC_TrayN - 1] == TrayD.TIC_EndColN[TrayD.TIC_TrayN - 1] &&
					 TrayD.TIC_RowN[TrayD.TIC_TrayN - 1] == TrayD.TIC_EndRowN[TrayD.TIC_TrayN - 1]) ||
					(TrayD.TIC_ColN[TrayD.TIC_TrayN - 1] == colC && TrayD.TIC_RowN[TrayD.TIC_TrayN - 1] == rowC))
				{
					TrayD.TIC_ColN[TrayD.TIC_TrayN - 1] = 0;
					TrayD.TIC_RowN[TrayD.TIC_TrayN - 1] = 0;
					if (TrayD.TIC_TrayN == 1)
					{
						TrayEndFlag.takeIC[0] = true;
						if (Auto_Flag.AutoTray_TakeIC && Auto_Flag.AutoTray_LayIC)
						{
							TrayEndFlag.takeLayIC[0] = true;
						}
						if (Auto_Flag.FixedTray_TakeIC)
						{
							if (!Auto_Flag.FixedTray_LayIC)
							{
								TrayEndFlag.takeLayIC[0] = true;
							}
							if (TrayEndFlag.takeLayIC[1] || TrayEndFlag.takeIC[1])
							{
								Auto_Flag.ForceEnd = true;//硬收尾
								Auto_Flag.Ending = true;
								TrayD.TIC_TrayN = 0;
							}
							else
							{
								TrayD.TIC_TrayN = 2;
                                if (g_config.TrayRotateDir[1] != 0)
                                {
                                    Auto_Flag.RotateChange = true;
                                }
							}
						}
					}
					else if (TrayD.TIC_TrayN == 2)
					{
						
						TrayEndFlag.takeIC[1] = true;
						if ((!Auto_Flag.FixedTray_LayIC && !Auto_Flag.AutoTray_LayIC) || TrayEndFlag.tray2Burn)
						{
							TrayEndFlag.takeLayIC[1] = true;
						}
						if (((TrayEndFlag.takeLayIC[0] || TrayEndFlag.takeIC[0]) && Auto_Flag.FixedTray_TakeIC) || (Auto_Flag.AutoTray_TakeIC && !Auto_Flag.AutoTray_LayIC))
						{
							Auto_Flag.ForceEnd = true;//硬收尾
							Auto_Flag.Ending = true;
							TrayD.TIC_TrayN = 0;
						}
						else
						{
							TrayD.TIC_TrayN = 1;
                            if (g_config.TrayRotateDir[1] != 0)
                            {
                                Auto_Flag.RotateChange = true;
                            }
                        }
					}
				}
                else
                {
                    if (g_config.Tray_Col_Add(TrayD.TIC_TrayN - 1))
                    {
                        if (TrayD.TIC_ColN[TrayD.TIC_TrayN - 1] >= colC)//最后一列
                        {
                            TrayD.TIC_ColN[TrayD.TIC_TrayN - 1] = 1;
                            TrayD.TIC_RowN[TrayD.TIC_TrayN - 1]++;
                        }
                        else
                        {
                            TrayD.TIC_ColN[TrayD.TIC_TrayN - 1]++;
                        }
                    }
                    else
                    {
                        if (TrayD.TIC_RowN[TrayD.TIC_TrayN - 1] >= rowC)//最后一行
                        {
                            TrayD.TIC_RowN[TrayD.TIC_TrayN - 1] = 1;
                            TrayD.TIC_ColN[TrayD.TIC_TrayN - 1]++;
                        }
                        else
                        {
                            TrayD.TIC_RowN[TrayD.TIC_TrayN - 1]++;
                        }
                    }
                }
                TrayState.TrayStateUpdate();
            }
			else if (Auto_Flag.FixedTube_TakeIC)
			{
				if (TIC_TubeN == 0)
				{
					str = MultiLanguage.IsEnglish()==true? "Pen " + TIC_PenN + " to " + UserConfig.TubeC + " material tube successfully collected material" : "吸头" + TIC_PenN + "到料管第" + UserConfig.TubeC + "号管取料成功";
				}
				else
				{
					str = MultiLanguage.IsEnglish() == true ? "Pen " + TIC_PenN + " to " + TIC_PenN + " material tube successfully collected material" :"吸头" + TIC_PenN + "到料管第" + TIC_TubeN + "号管取料成功";
				}
			}
			g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");

			if (!TrayEndFlag.tray2Burn)
			{
				if (Auto_Flag.Production)
				{
					TIC_ValidC++;
				}

				short ret = Get_TakeIC_Capacity(out TIC_Capacity);
				if (ret == 1 || (ret == 0 && TIC_Capacity == 0))
				{
					Auto_Flag.Ending = true;
					if (Auto_Flag.Production && !Auto_Flag.ProductionOK)
					{
						Auto_Flag.ForceEnd = true;//硬收尾   
					}
				}
                Efficiency.freshCount++;
				TIC_DoneC++;
                if (Auto_Flag.BurnMode)
                {
                    IC_SupplyC--;
                }    
			}
            g_act.WriteLotInfo();
        }

		/// <summary>
		/// 载体放料数据运算
		/// </summary>
		public void Carrier_LayIC_Data_Operation()
		{
			string str = null;
            int rowC, colC, EndRowC, EndColC;
            if (TrayEndFlag.tray2Burn)
			{
				if (TrayD.TIC_ColN[0] == 0 && TrayD.TIC_RowN[0] == 0)
				{
					TrayD.TIC_ColN[0] = TrayD.ColC;
					TrayD.TIC_RowN[0] = TrayD.RowC;
					TrayEndFlag.takeLayIC[0] = false;
					TrayEndFlag.takeIC[0] = false;
				}
				else
				{
                    if (g_config.Tray_Col_Add(0))
                    {
                        if (TrayD.TIC_ColN[0] == 1)//第一列
                        {
                            TrayD.TIC_ColN[0] = TrayD.ColC;
                            TrayD.TIC_RowN[0]--;
                        }
                        else
                        {
                            TrayD.TIC_ColN[0]--;
                        }
                    }
                    else
                    {
                        if (TrayD.TIC_RowN[0] == 1)//第一行
                        {
                            TrayD.TIC_RowN[0] = TrayD.RowC;
                            TrayD.TIC_ColN[0]--;
                        }
                        else
                        {
                            TrayD.TIC_RowN[0]--;
                        }
                    }
				}
                if (MultiLanguage.IsEnglish())
                {
                    str = "Pen" + (LIC_PenN + 1) + " successfully lay IC from the " + TrayD.TIC_RowN[0] + " row, " + TrayD.TIC_ColN[0] + " column of the tray1";
                }  
                else
                {
                    str = "吸头" + (LIC_PenN + 1) + "到料盘1第" + TrayD.TIC_RowN[0] + "行第" + TrayD.TIC_ColN[0] + "列放料成功";
                }
                TrayState.TrayStateUpdate();
            }
			else
			{
				if (Pen[LIC_PenN].DownResult == 1)//OK
				{
                    g_act.RecordDateInfo(LIC_PenN + 1, rectify[LIC_PenN].AxisX.ToString("F4") + "," + rectify[LIC_PenN].AxisY.ToString("F4"));
                    OKDoneC++;//OK完成计数
					if (!Auto_Flag.TestMode)
					{
						OKAllC++;
					}

					if (Auto_Flag.Brede_LayIC)
					{
						Brede_Number.CntVal_Auto++;
						str = "吸头[" + (LIC_PenN + 1) + "]到编带放料成功";
					}
					else if (Auto_Flag.FixedTray_LayIC || Auto_Flag.AutoTray_LayIC)
					{
                        if (TrayD.LIC_TrayN == 2 && g_config.TrayRotateDir[TrayD.LIC_TrayN - 1] != 0)
                        {
                            rowC = TrayD.ColC;
                            colC = TrayD.RowC;
                            EndRowC = TrayD.TIC_EndRowN[TrayD.LIC_TrayN - 1];
                            EndColC = TrayD.TIC_EndColN[TrayD.LIC_TrayN - 1];
                        }
                        else
                        {
                            rowC = TrayD.RowC;
                            colC = TrayD.ColC;
                            EndRowC = TrayD.TIC_EndRowN[TrayD.LIC_TrayN - 1];
                            EndColC = TrayD.TIC_EndColN[TrayD.LIC_TrayN - 1];
                        }
                        if (MultiLanguage.IsEnglish())
                        {
                            str = "Pen" + (LIC_PenN + 1) + " successfully lay IC from the " + TrayD.LIC_RowN[TrayD.TIC_TrayN - 1] + " row, " + TrayD.LIC_ColN[TrayD.TIC_TrayN - 1] + " column of the tray" + TrayD.LIC_TrayN;
                        }
                        else
                        {
                            str = "吸头" + (LIC_PenN + 1) + "到料盘" + TrayD.LIC_TrayN + "第" + TrayD.LIC_RowN[TrayD.LIC_TrayN - 1] + "行第" + TrayD.LIC_ColN[TrayD.LIC_TrayN - 1] + "列放料成功";
                        }
						if ((TrayD.LIC_ColN[TrayD.LIC_TrayN - 1] == EndColC && TrayD.LIC_RowN[TrayD.LIC_TrayN - 1] == EndRowC) 
                            || (TrayD.LIC_ColN[TrayD.LIC_TrayN - 1] >= colC && TrayD.LIC_RowN[TrayD.LIC_TrayN - 1] >= rowC))//最后一个料
						{
							TrayD.LIC_ColN[TrayD.LIC_TrayN - 1] = 0;
							TrayD.LIC_RowN[TrayD.LIC_TrayN - 1] = 0;
							if (TrayD.LIC_TrayN == 1)
							{
								TrayEndFlag.layIC[0] = true;
								TrayEndFlag.takeLayIC[0] = true;
								if (TrayEndFlag.layIC[1])
								{
									TrayD.LIC_TrayN = 0;
								}
								else
								{
									TrayD.LIC_TrayN = 2;
                                    if (g_config.TrayRotateDir[1] != 0)
                                    {
                                        Auto_Flag.RotateChange = true;
                                    }
                                }
							}
							else if (TrayD.LIC_TrayN == 2)
							{
								TrayEndFlag.layIC[1] = true;
								TrayEndFlag.takeLayIC[1] = true;
								if (Auto_Flag.FixedTray_LayIC)
								{
									if (TrayEndFlag.layIC[0])
									{
										TrayD.LIC_TrayN = 0;
									}
									else
									{
										TrayD.LIC_TrayN = 1;
                                        if (g_config.TrayRotateDir[1] != 0)
                                        {
                                            Auto_Flag.RotateChange = true;
                                        }
                                    }
								}
							}
						}
						else
						{
                            if (g_config.Tray_Col_Add(TrayD.LIC_TrayN - 1))
                            {
                                if (TrayD.LIC_ColN[TrayD.LIC_TrayN - 1] >= colC)
                                {
                                    TrayD.LIC_ColN[TrayD.LIC_TrayN - 1] = 1;
                                    TrayD.LIC_RowN[TrayD.LIC_TrayN - 1]++;
                                }
                                else
                                {
                                    TrayD.LIC_ColN[TrayD.LIC_TrayN - 1]++;
                                }
                            }
                            else
                            {
                                if (TrayD.LIC_RowN[TrayD.LIC_TrayN - 1] >= rowC)
                                {
                                    TrayD.LIC_RowN[TrayD.LIC_TrayN - 1] = 1;
                                    TrayD.LIC_ColN[TrayD.LIC_TrayN - 1]++;
                                }
                                else
                                {
                                    TrayD.LIC_RowN[TrayD.LIC_TrayN - 1]++;
                                }
                            }
                        }
                        TrayState.TrayStateUpdate();
                    }
					OKPenC--;
				}
				else//NG
				{
					if (Pen[LIC_PenN].DownResult == 2)
					{
						NGDoneC++;//NG完成计数
						if (!Auto_Flag.TestMode)
						{
							NGAllC++;
						}
					}
					if (Auto_Flag.NGTrayKeep)
					{
                        if (g_config.TrayRotateDir[2] != 0)
                        {
                            rowC = TrayD.ColC;
                            colC = TrayD.RowC;
                        }
                        else
                        {
                            rowC = TrayD.RowC;
                            colC = TrayD.ColC;
                        }
                        if (MultiLanguage.IsEnglish())
                        {
                            str = "Pen" + (LIC_PenN + 1) + " successfully lay IC from the " + TrayD.LIC_RowN[2] + " row, " + TrayD.LIC_ColN[2] + " column of the trayNG";
                        }
                        else
                        {
                            str = "吸头" + (LIC_PenN + 1) + "到NG盘第" + TrayD.LIC_RowN[2] + "行第" + TrayD.LIC_ColN[2] + "列放料成功";
                        }
                        
						if (TrayD.LIC_ColN[2] >= colC && TrayD.LIC_RowN[2] >= rowC)//最后一个料
						{
							TrayD.LIC_ColN[2] = 0;
							TrayD.LIC_RowN[2] = 0;
							TrayEndFlag.layIC[2] = true;
						}
						else
						{
                            if (g_config.Tray_Col_Add(2))
                            {
                                if (TrayD.LIC_ColN[2] >= colC)
                                {
                                    TrayD.LIC_ColN[2] = 1;
                                    TrayD.LIC_RowN[2]++;
                                }
                                else
                                {
                                    TrayD.LIC_ColN[2]++;
                                }
                            }
                            else
                            {
                                if (TrayD.LIC_RowN[2] >= rowC)
                                {
                                    TrayD.LIC_RowN[2] = 1;
                                    TrayD.LIC_ColN[2]++;
                                }
                                else
                                {
                                    TrayD.LIC_RowN[2]++;
                                }
                            }
						}
                        TrayState.TrayStateUpdate();
                    }
					else
					{
						str = "吸头[" + (LIC_PenN + 1) + "]到NG杯放料成功";
					}
					NGPenC--;
				}
				Pen[LIC_PenN].DownResult = 0;
				if (Auto_Flag.Detection)
				{
					Pen[LIC_PenN].DetectionResult = 0;
				}
			}
            if (Pen[LIC_PenN].ExistRawIC)
            {
                Pen[LIC_PenN].ExistRawIC = false;
                ExistRawICPenC--;
            }
			Pen[LIC_PenN].ExistIC = false;
            ExistICPenC--;
            LIC_PenN++;
			g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
            g_act.WriteLotInfo();
        }

		/// <summary>
		/// 烧录座取料数据运算
		/// </summary>
		public void BurnSeat_TakeIC_Data_Operation(bool mode = true)
		{
			int i, temp;
			string str = null;
            Auto_Flag.BurnSeat_TakeNullIC = false;
            if (Group[TIC_GroupN].Unit[TIC_UnitN].Flag_First)//第一次
			{
				if (Pen[TIC_PenN].ExistIC)
				{
                    ExistICPenC++;
                    Pen[TIC_PenN].DownResult = 3;
					NGPenC++;
                    RotateTrigge(true, TIC_PenN, RotateAngle.LIC_Tray[2]);
					TIC_PenN++;
					str = "吸头" + TIC_PenN + "到烧写座" + (TIC_GroupN + 1) + "_" + (TIC_UnitN + 1) + "首次排空，取料成功";
				}
				else
				{
                    str = "吸头" + (TIC_PenN + 1) + "到烧写座" + (TIC_GroupN + 1) + "_" + (TIC_UnitN + 1) + "首次排空，取料失败"; 
				}
				Group[TIC_GroupN].Unit[TIC_UnitN].Flag_LayIC = true;
				Group[TIC_GroupN].Unit[TIC_UnitN].Flag_First = false;
			}
			else//非第一次
			{
				Group[TIC_GroupN].Unit[TIC_UnitN].Flag_TakeIC = false;
                ExistICPenC++;
                TIC_UnitC--;
				if (Group[TIC_GroupN].Unit[TIC_UnitN].Flag_Open)
				{
					Group[TIC_GroupN].Unit[TIC_UnitN].Flag_LayIC = true;
				}
				if (((Group[TIC_GroupN].Unit[TIC_UnitN].DownResult == 1) && !Auto_Flag.TestMode) || Auto_Flag.TestMode)//OK
				{
					OKPenC++;
					Auto_Flag.Burn_Again = false;
					Pen[TIC_PenN].DownResult = 1;
					Group[TIC_GroupN].Unit[TIC_UnitN].Counter_Burn = 0;//NG重烧计数	
                    Group[TIC_GroupN].Unit[TIC_UnitN].NGCounter_Shut = 0;//连续NG关闭烧录座计数器
                }
				else//NG
				{
					Pen[TIC_PenN].DownResult = 2;
					if (Auto_Flag.ManualEnd)//手动收尾
					{
						NGPenC++;
						Auto_Flag.Burn_Again = false;
						Group[TIC_GroupN].Unit[TIC_UnitN].Counter_Burn = 0;//NG重烧计数
					}
					else if (Group[TIC_GroupN].Unit[TIC_UnitN].Counter_Burn >= NGReBurnC)//重烧次数已用完
					{
						NGPenC++;
						Auto_Flag.Burn_Again = false;
						Group[TIC_GroupN].Unit[TIC_UnitN].Counter_Burn = 0;//NG重烧计数 
					}
					else
					{
						Auto_Flag.Burn_Again = true;
						Group[TIC_GroupN].Unit[TIC_UnitN].Counter_Burn++;
						Pen[TIC_PenN].Counter_Burn = Group[TIC_GroupN].Unit[TIC_UnitN].Counter_Burn;
						Group[TIC_GroupN].Unit[TIC_UnitN].Counter_Burn = 0;//NG重烧计数 
					}

                    Group[TIC_GroupN].Unit[TIC_UnitN].NGAllC_Shut++;
                    Group[TIC_GroupN].Unit[TIC_UnitN].NGCounter_Shut++;
                    NGAllDoneC_Shut++;
                    if (NGScketC_Shut != 0 && Group[TIC_GroupN].Unit[TIC_UnitN].NGAllC_Shut >= NGScketC_Shut)
                    {
                        Group[TIC_GroupN].Unit[TIC_UnitN].Flag_Open = false;
                        evt.EnableSeat_Click(TIC_GroupN, TIC_UnitN);
                        Group[TIC_GroupN].Unit[TIC_UnitN].NGAllC_Shut = 0;
                    }
                    if (NGContinueC_Shut != 0 && Group[TIC_GroupN].Unit[TIC_UnitN].NGCounter_Shut >= NGContinueC_Shut)
                    {
                        Group[TIC_GroupN].Unit[TIC_UnitN].Flag_Open = false;
                        evt.EnableSeat_Click(TIC_GroupN, TIC_UnitN);
                        Group[TIC_GroupN].Unit[TIC_UnitN].NGCounter_Shut = 0;
                    }
                    if (NGAllC_Shut != 0 && NGAllDoneC_Shut >= NGAllC_Shut)
                    {
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "全部失败[ " + NGAllC_Shut + " ]次停止作业", "Flow");
                        Auto_Flag.Next = true;
                        Auto_Flag.ManualEnd = true;
                        Auto_Flag.Pause = false;
                        Auto_Flag.Ending = true;
                        Auto_Flag.ForceEnd = true;
                        NGAllDoneC_Shut = 0;
                    }
                }
				//旋转处理
				if (!Auto_Flag.Burn_Again)
				{
                    if (Pen[TIC_PenN].DownResult == 2)
                    {
                        RotateTrigge(true, TIC_PenN, RotateAngle.LIC_Tray[2]);
                    }
                    else
                    {
                        if (!Vision_3D.Enabled_II)
                        {
                            RotateTrigge(true, TIC_PenN, Get_LIC_RotateAngle());
                        }
                    } 
				}

				if (!Auto_Flag.TestMode)
				{
					//烧录NG处理
					if (Pen[TIC_PenN].DownResult == 2 && !Auto_Flag.Burn_Again)
					{
						if (Auto_Flag.Ending && !Auto_Flag.ForceEnd)
						{
							Auto_Flag.Ending = false;
						}

						if (Auto_Flag.Production && Auto_Flag.ProductionOK)
						{
							TIC_ValidC--;
						}
					}
				}
				TIC_PenN++;
               str = "吸头[" + TIC_PenN + "]到烧写座[" + (TIC_GroupN + 1) + "_" + (TIC_UnitN + 1) + "]取料成功";
			}
            if (mode)
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
            }
            
			if (Group[TIC_GroupN].Unit[TIC_UnitN].Flag_LayIC)
			{
				LIC_UnitC++;
				Group[TIC_GroupN].LIC_UnitC++;
			}
			if (LIC_UnitC >= TIC_Capacity || Auto_Flag.ForceEnd)
			{
				Auto_Flag.Emptying = false;
			}
			else
			{
				Auto_Flag.Emptying = true;
			}
			SingleTIC_UnitC++;
			TIC_UnitN++;
			if (TIC_UnitN == UserConfig.ScketUnitC)//取料到每组最后一个座子
			{
				if (!Group[TIC_GroupN].Waiting_To_Burn && Group[TIC_GroupN].LIC_UnitC == 0)
				{
                    if (!(Auto_Flag.Flip && (ProgrammerType == GlobConstData.Programmer_WG || ProgrammerType == GlobConstData.Programmer_YED || ProgrammerType == GlobConstData.Programmer_WG_TY)))//压座
                    {
                        if (ProgrammerType != GlobConstData.Programmer_RD)
                        {
                            temp = TIC_GroupN * (UserConfig.MotionGroupC);
                            for (i = 0; i < UserConfig.MotionGroupC; i++)
                            {
                                In_Output.pushSeatO[temp + i].M = false;
                            }
                        }
					}
				}
                if (Group[TIC_GroupN].Waiting_To_Burn)
                {
                    if (Auto_Flag.PenAlt_Flag)
                    {
                        if (Group[TIC_GroupN].LIC_UnitC == 0 || (Auto_Flag.Ending && ExistRawICPenC == 0 && !Auto_Flag.Burn_Again))
                        {
                            DownTrigger_Handle(TIC_GroupN);//烧录触发处理
                        }
                    }
                    else
                    {
                        if (Group[TIC_GroupN].LIC_UnitC == 0 || Auto_Flag.ManualEnd || (Auto_Flag.Ending && NGPenC == 0 && !Auto_Flag.Burn_Again))
                        {
                            DownTrigger_Handle(TIC_GroupN);//烧录触发处理
                        }
                    }
                }
				TIC_UnitN = 0;
				TIC_GroupN++;
				if (TIC_GroupN == UserConfig.ScketGroupC)//取料到最后一组
				{
					TIC_GroupN = 0;
					Auto_Flag.Seat_TIC_Cycle = true;
					Auto_Flag.Seat_EndTakeIC = true;
				}
			}
		}

		/// <summary>
		/// 烧录座放料数据运算
		/// </summary>
		public void BurnSeat_LayIC_Data_Operation()
		{
			string str = null;

            ExistICPenC--;
            if (Auto_Flag.OverDeviation)
            {
                if (Pen[SyncPen[SyncPenN].PenN].ExistRawIC)
                {
                    Pen[SyncPen[SyncPenN].PenN].ExistRawIC = false;
                    ExistRawICPenC--;
                }
                Pen[SyncPen[SyncPenN].PenN].ExistIC = false;
                str = "吸头[" + (SyncPen[SyncPenN].PenN + 1) + "]到烧写座[" + (SyncPen[SyncPenN].GroupN + 1) + "_" + (SyncPen[SyncPenN].UnitN + 1) + "]放料成功";
				TIC_UnitC++;
				LIC_UnitC--;
				SyncPen[SyncPen[SyncPenN].PenN].Flag_LayIC = false;
                SyncPen[SyncPen[SyncPenN].PenN].OverDeviation = false;
                Group[SyncPen[SyncPenN].GroupN].Unit[SyncPen[SyncPenN].UnitN].Flag_TakeIC = true;
				Group[SyncPen[SyncPenN].GroupN].Unit[SyncPen[SyncPenN].UnitN].Flag_LayIC = false;
				Group[SyncPen[SyncPenN].GroupN].Waiting_To_Burn = true;
				Group[SyncPen[SyncPenN].GroupN].Unit[SyncPen[SyncPenN].UnitN].Flag_NewIC = true;
				Group[SyncPen[SyncPenN].GroupN].LIC_UnitC--;
				SyncPenN++;
				g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
                return;
			}
            bool flag = RunState == RUNSTATE.BurnSeat_LayIC && ProgrammerType == GlobConstData.Programmer_YED;
            if (!flag)
            {
                TIC_UnitC++;
                LIC_UnitC--;
                Group[LIC_GroupN].Unit[LIC_UnitN].Flag_TakeIC = true;
                Group[LIC_GroupN].Unit[LIC_UnitN].Flag_LayIC = false;
                Group[LIC_GroupN].Waiting_To_Burn = true;
                Group[LIC_GroupN].Unit[LIC_UnitN].Flag_NewIC = true;
                Group[LIC_GroupN].LIC_UnitC--;
            }
            else
            {
                Scan_GroupN = LIC_GroupN;
                Scan_UnitN = LIC_UnitN;
            }
            
			if (RunState == RUNSTATE.Burn_Again)//重烧烧录座放料
			{
				SingleTIC_UnitC--;
				Group[LIC_GroupN].Unit[LIC_UnitN].Counter_Burn = Pen[TIC_PenN].Counter_Burn;
				Pen[TIC_PenN].ExistIC = false;
				str = "吸头[" + (TIC_PenN + 1) + "]到烧写座[" + (LIC_GroupN + 1) + "_" + (LIC_UnitN + 1) + "]重烧放料成功";
				g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
			}
			else//非重烧烧录座放料
			{
                if (Pen[LIC_PenN].ExistRawIC)
                {
                    Pen[LIC_PenN].ExistRawIC = false;
                    ExistRawICPenC--;
                }
                Pen[LIC_PenN].ExistIC = false;
				LIC_PenN++;
				str = "吸头[" + LIC_PenN + "]到烧写座[" + (LIC_GroupN + 1) + "_" + (LIC_UnitN + 1) + "]放料成功";
				g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
                if (!flag)
                {
                    if (ExistICPenC == 0 && (Auto_Flag.Ending || IC_SupplyC == 0) &&
                    (TIC_GroupN != LIC_GroupN || (TIC_GroupN == LIC_GroupN && LIC_UnitN >= TIC_UnitN)))
                    {
                        DownTrigger_Handle(LIC_GroupN);//烧录触发处理				
                        return;
                    }
                }
			}
            if (!flag)
            {
                LIC_UnitN++;
                if (LIC_UnitN == UserConfig.ScketUnitC) //查询到每组最后一个座子
                {
                    DownTrigger_Handle(LIC_GroupN);//烧录触发处理
                }
            } 
		}

        /// <summary>
		/// 烧录座取扫码NG料数据运算
		/// </summary>
		public void TakeSNGIC_Data_Operation()
        {
            string str = null;
            Pen[TIC_PenN].DownResult = 2;
            RotateTrigge(true, TIC_PenN, RotateAngle.LIC_Tray[2]);
            NGPenC++;
            ExistICPenC++;
            SNGPenC++;
            TIC_PenN++;
            if (Auto_Flag.Production && Auto_Flag.ProductionOK)
            {
                TIC_ValidC--;
            }
            str = "吸头" + TIC_PenN + "到烧写座" + (LIC_GroupN + 1) + "_" + (LIC_UnitN + 1) + "取扫码NG料成功";
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
        }

        /// <summary>
		/// 烧录座扫码数据运算
		/// </summary>
		public void ScanCode_Data_Operation()
        {
            string str = null;

            ScannerResult = Group[Scan_GroupN].Unit[Scan_UnitN].DownResult;
            if (Group[Scan_GroupN].Unit[Scan_UnitN].DownResult == 1)//扫码OK
            {
                TIC_UnitC++;
                LIC_UnitC--;
                Group[LIC_GroupN].Unit[LIC_UnitN].Flag_TakeIC = true;
                Group[LIC_GroupN].Unit[LIC_UnitN].Flag_LayIC = false;
                Group[LIC_GroupN].Waiting_To_Burn = true;
                Group[LIC_GroupN].Unit[LIC_UnitN].Flag_NewIC = true;
                Group[LIC_GroupN].LIC_UnitC--;
                str = "扫码枪" + "到烧写座" + (LIC_GroupN + 1) + "_" + (LIC_UnitN + 1) + "扫码成功";
                LIC_UnitN++;
                if (LIC_UnitN == UserConfig.ScketUnitC) //查询到每组最后一个座子
                {
                    DownTrigger_Handle(LIC_GroupN);//烧录触发处理
                }
            }
            else
            {
                str = "扫码枪" + "到烧写座" + (LIC_GroupN + 1) + "_" + (LIC_UnitN + 1) + "扫码失败";
            }
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
        }

        /// <summary>
        /// 取料失败数据运算
        /// </summary>
        public void TakeIC_Failure_Data_Operation()
        {
            int rowC, colC;
            if (RunState == RUNSTATE.Carrier_TakeIC)
            {
                if (Auto_Flag.Brede_TakeIC)
                {
                    if (!Auto_Flag.TestMode)
                    {
                        if (Config.FeederCount == 0)//单飞达
                        {
                            Brede.Send_Cmd(Brede.Cmd_SendMaterial);//编带模块飞达触发
                            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "编带模块飞达触发", "Flow");
                            AutoTimer.BredeTakeDelay = GetSysTime() + AutoTiming.BredeTakeDelay;
                            if (FeederIO)
                            {
                                FeederTrigge(0);
                            }
                        }
                        else if (Config.FeederCount == 1)//双飞达
                        {
                            FeederTrigge(TIC_FeederN);
                            TIC_FeederN++;
                            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "飞达[" + TIC_FeederN + "]触发", "Flow");
                            if (TIC_FeederN >= UserConfig.FeederC)
                            {
                                TIC_FeederN = 0;
                            }
                        }

                    }
                }
                else if (Auto_Flag.FixedTray_TakeIC || Auto_Flag.AutoTray_TakeIC)
                {
                    if (TrayD.TIC_TrayN == 2 && g_config.TrayRotateDir[TrayD.TIC_TrayN - 1] != 0)
                    {
                        rowC = TrayD.ColC;
                        colC = TrayD.RowC;
                    }
                    else
                    {
                        rowC = TrayD.RowC;
                        colC = TrayD.ColC;
                    }
                    
                    if ((TrayD.TIC_ColN[TrayD.TIC_TrayN - 1] == TrayD.TIC_EndColN[TrayD.TIC_TrayN - 1] &&
                         TrayD.TIC_RowN[TrayD.TIC_TrayN - 1] == TrayD.TIC_EndRowN[TrayD.TIC_TrayN - 1]) ||
                        (TrayD.TIC_ColN[TrayD.TIC_TrayN - 1] == colC && TrayD.TIC_RowN[TrayD.TIC_TrayN - 1] == rowC))
                    {
                        TrayD.TIC_ColN[TrayD.TIC_TrayN - 1] = 0;
                        TrayD.TIC_RowN[TrayD.TIC_TrayN - 1] = 0;
                        if (TrayD.TIC_TrayN == 1)
                        {
                            TrayEndFlag.takeIC[0] = true;
                            if (Auto_Flag.AutoTray_TakeIC && Auto_Flag.AutoTray_LayIC)
                            {
                                TrayEndFlag.takeLayIC[0] = true;
                            }
                            if (Auto_Flag.FixedTray_TakeIC)
                            {
                                if (!Auto_Flag.FixedTray_LayIC)
                                {
                                    TrayEndFlag.takeLayIC[0] = true;
                                }
                                if (TrayEndFlag.takeLayIC[1] || TrayEndFlag.takeIC[1])
                                {
                                    Auto_Flag.ForceEnd = true;//硬收尾
                                    Auto_Flag.Ending = true;
                                    TrayD.TIC_TrayN = 0;
                                }
                                else
                                {
                                    TrayD.TIC_TrayN = 2;
                                    if (g_config.TrayRotateDir[1] != 0)
                                    {
                                        Auto_Flag.RotateChange = true;
                                    }
                                }
                            }
                        }
                        else if (TrayD.TIC_TrayN == 2)
                        {

                            TrayEndFlag.takeIC[1] = true;
                            if ((!Auto_Flag.FixedTray_LayIC && !Auto_Flag.AutoTray_LayIC) || TrayEndFlag.tray2Burn)
                            {
                                TrayEndFlag.takeLayIC[1] = true;
                            }
                            if (((TrayEndFlag.takeLayIC[0] || TrayEndFlag.takeIC[0]) && Auto_Flag.FixedTray_TakeIC) || (Auto_Flag.AutoTray_TakeIC && !Auto_Flag.AutoTray_LayIC))
                            {
                                Auto_Flag.ForceEnd = true;//硬收尾
                                Auto_Flag.Ending = true;
                                TrayD.TIC_TrayN = 0;
                            }
                            else
                            {
                                TrayD.TIC_TrayN = 1;
                                if (g_config.TrayRotateDir[1] != 0)
                                {
                                    Auto_Flag.RotateChange = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (g_config.Tray_Col_Add(TrayD.TIC_TrayN - 1))
                        {
                            if (TrayD.TIC_ColN[TrayD.TIC_TrayN - 1] >= colC)//最后一列
                            {
                                TrayD.TIC_ColN[TrayD.TIC_TrayN - 1] = 1;
                                TrayD.TIC_RowN[TrayD.TIC_TrayN - 1]++;
                            }
                            else
                            {
                                TrayD.TIC_ColN[TrayD.TIC_TrayN - 1]++;
                            }
                        }
                        else
                        {
                            if (TrayD.TIC_RowN[TrayD.TIC_TrayN - 1] >= rowC)//最后一行
                            {
                                TrayD.TIC_RowN[TrayD.TIC_TrayN - 1] = 1;
                                TrayD.TIC_ColN[TrayD.TIC_TrayN - 1]++;
                            }
                            else
                            {
                                TrayD.TIC_RowN[TrayD.TIC_TrayN - 1]++;
                            }
                        }
                    }
                    TrayState.TrayStateUpdate();
                }
            }
            else
            {
                int i, temp;
                if (Group[TIC_GroupN].Unit[TIC_UnitN].Flag_First)//第一次
                {
                    Group[TIC_GroupN].Unit[TIC_UnitN].Flag_LayIC = true;
                    Group[TIC_GroupN].Unit[TIC_UnitN].Flag_First = false;
                }
                else//非第一次
                {
                    if (Auto_Flag.Production && Auto_Flag.ProductionOK)
                    {
                        TIC_ValidC--;
                    }
                    Group[TIC_GroupN].Unit[TIC_UnitN].Flag_TakeIC = false;
                    TIC_UnitC--;
                    if (Group[TIC_GroupN].Unit[TIC_UnitN].Flag_Open)
                    {
                        Group[TIC_GroupN].Unit[TIC_UnitN].Flag_LayIC = true;
                    }
                    Auto_Flag.Burn_Again = false;
                    Group[TIC_GroupN].Unit[TIC_UnitN].Counter_Burn = 0;//NG重烧计数	
                    Group[TIC_GroupN].Unit[TIC_UnitN].NGCounter_Shut = 0;//连续NG关闭烧录座计数器
                    
                    if (!Auto_Flag.TestMode)
                    {
                        //烧录NG处理
                        if (Pen[TIC_PenN].DownResult == 2 && !Auto_Flag.Burn_Again)
                        {
                            if (Auto_Flag.Ending && !Auto_Flag.ForceEnd)
                            {
                                Auto_Flag.Ending = false;
                            }

                            if (Auto_Flag.Production && Auto_Flag.ProductionOK)
                            {
                                TIC_ValidC--;
                            }
                        }
                    }
                }

                if (Group[TIC_GroupN].Unit[TIC_UnitN].Flag_LayIC)
                {
                    LIC_UnitC++;
                    Group[TIC_GroupN].LIC_UnitC++;
                }
                if (LIC_UnitC >= TIC_Capacity || Auto_Flag.ForceEnd)
                {
                    Auto_Flag.Emptying = false;
                }
                else
                {
                    Auto_Flag.Emptying = true;
                }
                SingleTIC_UnitC++;
                TIC_UnitN++;
                if (TIC_UnitN == UserConfig.ScketUnitC)//取料到每组最后一个座子
                {
                    if (!Group[TIC_GroupN].Waiting_To_Burn && Group[TIC_GroupN].LIC_UnitC == 0)
                    {
                        if (!(Auto_Flag.Flip && (ProgrammerType == GlobConstData.Programmer_WG || ProgrammerType == GlobConstData.Programmer_YED || ProgrammerType == GlobConstData.Programmer_WG_TY)))//压座
                        {
                            if (ProgrammerType != GlobConstData.Programmer_RD)
                            {
                                temp = TIC_GroupN * (UserConfig.MotionGroupC);
                                for (i = 0; i < UserConfig.MotionGroupC; i++)
                                {
                                    In_Output.pushSeatO[temp + i].M = false;
                                }
                            }
                        }
                    }
                    if (Group[TIC_GroupN].Waiting_To_Burn)
                    {
                        if (Auto_Flag.PenAlt_Flag)
                        {
                            if (Group[TIC_GroupN].LIC_UnitC == 0 || (Auto_Flag.Ending && ExistRawICPenC == 0 && !Auto_Flag.Burn_Again))
                            {
                                DownTrigger_Handle(TIC_GroupN);//烧录触发处理
                            }
                        }
                        else
                        {
                            if (Group[TIC_GroupN].LIC_UnitC == 0 || Auto_Flag.ManualEnd || (Auto_Flag.Ending && NGPenC == 0 && !Auto_Flag.Burn_Again))
                            {
                                DownTrigger_Handle(TIC_GroupN);//烧录触发处理
                            }
                        }
                    }
                    TIC_UnitN = 0;
                    TIC_GroupN++;
                    if (TIC_GroupN == UserConfig.ScketGroupC)//取料到最后一组
                    {
                        TIC_GroupN = 0;
                        Auto_Flag.Seat_TIC_Cycle = true;
                        Auto_Flag.Seat_EndTakeIC = true;
                    }
                }
            }
        }

        /// <summary>
        /// 放料失败数据运算
        /// </summary>
        public void LayIC_Failure_Data_Operation()
		{
			if (RunState == RUNSTATE.Carrier_LayNGIC || RunState == RUNSTATE.Carrier_LayDNGIC)
			{
				NGPenC--;
			}
			else if (RunState == RUNSTATE.Carrier_LayOKIC || RunState == RUNSTATE.BurnSeat_LayIC)
			{
				if (RunState == RUNSTATE.Carrier_LayOKIC)
				{
					OKPenC--;
				}
				else if (RunState == RUNSTATE.BurnSeat_LayIC)
				{
					IC_SupplyC++;
                    if (Vision_3D.Enabled_I)//烧录前3D检测
                    {
                        OKPenC--;
                    }
                }
				if (Auto_Flag.Ending && !Auto_Flag.ForceEnd)
				{
					Auto_Flag.Ending = false;
				}

				if (Auto_Flag.Production && Auto_Flag.ProductionOK)
				{
					TIC_ValidC--;
				}
			}
            if (Pen[LIC_PenN].ExistRawIC)
            {
                Pen[LIC_PenN].ExistRawIC = false;
                ExistRawICPenC--;
            }
            Pen[LIC_PenN].DownResult = 0;
			Pen[LIC_PenN].DetectionResult = 0;
			Pen[LIC_PenN].ExistIC = false;
            ExistICPenC--;
            LIC_PenN++;
		}

        /// <summary>
        /// 主控逻辑程序
        /// </summary>
        public void MasterLogic_Program_I()
		{
			switch (RunState)
			{
				case RUNSTATE.BurnSeat_TakeIC:
					if (Auto_Flag.Burn_Again)//重烧
					{
						RunState = RUNSTATE.Burn_Again;
                        Auto_Flag.Burn_Again = false;
                    }
					else
					{
                        if (Auto_Flag.Seat_EndTakeIC || SingleTIC_UnitC == EnalePenC || ExistICPenC == EnalePenC || Auto_Flag.BurnSeat_Sync)
                        {
                            if (Auto_Flag.Seat_EndTakeIC)
                            {
                                Auto_Flag.Seat_EndTakeIC = false;
                            }
                            if (Auto_Flag.BurnSeat_Sync)
                            {
                                if (Config.Altimeter != 0 && Auto_Flag.Enabled_Overlay)//使用测高仪且使能防叠料
                                {
                                    if (ExistICPenC > 0)
                                    {
                                        Altimeter_Order();
                                        RunState = RUNSTATE.Emptying;
                                        break;
                                    }
                                }
                                DownTrigger_SyncPenCheck();
                            }

                            LogicJudgment_Cam_3D_Mode_II();
                        }
					}
					break;

				case RUNSTATE.Carrier_TakeIC:
					if (TrayEndFlag.tray2Burn)
					{
						if (ExistICPenC == EnalePenC || Auto_Flag.ManualEnd || TrayEndFlag.takeLayIC[1])
						{
							if (ExistICPenC > 0)//吸笔有料
							{
								RunState = RUNSTATE.Carrier_LayIC;
								LIC_PenN = 0;
							}
							else//吸笔没有料
							{
								RunState = RUNSTATE.End;
                                if (TrayEndFlag.takeLayIC[1])
                                {
                                    TrayEndFlag.tray2Burn = false;
                                }
							}
						}
					}
					else
					{
						if (IC_SupplyC == 0 || ExistICPenC == EnalePenC || Auto_Flag.Ending)
						{
                            if (Auto_Flag.RotateChange)
                            {
                                Auto_Flag.RotateChange = false;
                            }
                            LogicJudgment_Cam_2D_Mode_I();
                        }
					}
					break;

				case RUNSTATE.BurnSeat_LayIC:
                    if (ProgrammerType == GlobConstData.Programmer_YED)
                    {
                        RunState = RUNSTATE.ScanCode;
                    }
                    else
                    {
                        if (Auto_Flag.BurnSeat_Sync)
                        {
							if (!Auto_Flag.OverDeviation)
							{
								if (NGPenC > 0)
								{
									RunState = RUNSTATE.Carrier_LayNGIC;
									LIC_PenN = 0;
								}
                                else
                                {
									if (IC_SupplyC == 0 || Auto_Flag.Ending)
									{
										for (int i = 0; i < Sync_UnitC; i++)
										{
											if (Pen[SyncPen[i].PenN].DownTrigger)
											{
												Pen[SyncPen[i].PenN].DownTrigger = false;
												DownTrigger_Handle(SyncPen[i].GroupN);//烧录触发处理
											}
										}
										RunState = RUNSTATE.BurnSeat_TakeIC;
										Auto_Flag.BurnSeat_TakeIC = true;
									}
									else
									{
										RunState = RUNSTATE.Carrier_TakeIC;
									}
									TIC_PenN = 0;
								}
							}
						}
                        else
                        {
							if (Config.CCDModel == 0)//定拍
							{
								if (ExistICPenC == 0)
								{
									if (IC_SupplyC == 0 || Auto_Flag.Ending)
									{
										if (IC_SupplyC == 0)
										{
											LIC_UnitN = TIC_UnitN;
											LIC_GroupN = TIC_GroupN;
										}
										RunState = RUNSTATE.BurnSeat_TakeIC;
										Auto_Flag.BurnSeat_TakeIC = true;
									}
									else
									{
										RunState = RUNSTATE.Carrier_TakeIC;
									}
									TIC_PenN = 0;
								}
							}
							else//飞拍
							{
								if (ExistICPenC - NGPenC == 0)
								{
									if (NGPenC > 0)//吸笔有NG料
									{
										RunState = RUNSTATE.Carrier_LayNGIC;
										LIC_PenN = 0;
									}
									else
									{
										if (IC_SupplyC == 0 || Auto_Flag.Ending)
										{
											if (IC_SupplyC == 0)
											{
												LIC_UnitN = TIC_UnitN;
												LIC_GroupN = TIC_GroupN;
											}
											RunState = RUNSTATE.BurnSeat_TakeIC;
											Auto_Flag.BurnSeat_TakeIC = true;
										}
										else
										{
											RunState = RUNSTATE.Carrier_TakeIC;
										}
										TIC_PenN = 0;
									}
								}
							}
						}
                    }
					break;

				case RUNSTATE.Burn_Again:
					if (Auto_Flag.Seat_EndTakeIC)
					{
						Auto_Flag.Seat_EndTakeIC = false;
						if (NGPenC > 0)//吸笔有NG料
						{
							RunState = RUNSTATE.Carrier_LayNGIC;
							LIC_PenN = 0;
						}
						else if (OKPenC > 0)//吸笔有OK料
						{
							RunState = RUNSTATE.Carrier_LayOKIC;
							if (Auto_Flag.Brede_LayIC)
							{
								Brede_Flag.ALarmCheck = true;
							}
							LIC_PenN = 0;
						}
						else//吸笔没有料
						{
							if (Auto_Flag.Seat_EndLayIC || Auto_Flag.ForceEnd)
							{
                                if (Auto_Flag.Seat_EndLayIC)
                                {
                                    Auto_Flag.Seat_EndLayIC = false;
                                }
								RunState = RUNSTATE.BurnSeat_TakeIC;
								Auto_Flag.BurnSeat_TakeIC = true;
							}
							else
							{
                                RunState = RUNSTATE.Carrier_TakeIC;
							}
							TIC_PenN = 0;
						}
					}
					else
					{
						RunState = RUNSTATE.BurnSeat_TakeIC;
					}
					break;

				case RUNSTATE.Carrier_LayNGIC:
					if (NGPenC == 0)//吸笔没有有NG料
					{
						if (OKPenC > 0)//吸笔有OK料
						{
							RunState = RUNSTATE.Carrier_LayOKIC;
							if (Auto_Flag.Brede_LayIC)
							{
								Brede_Flag.ALarmCheck = true;
							}
							LIC_PenN = 0;
						}
						else//吸笔没有料
						{
							if (Auto_Flag.Ending)
							{
								if (TIC_UnitC != 0)
								{
									RunState = RUNSTATE.BurnSeat_TakeIC;
									Auto_Flag.BurnSeat_TakeIC = true;
								}
								else
								{
									if (Auto_Flag.Production && TIC_ValidC >= TargetC)
									{
										Auto_Flag.ProductionFinish = true;
									}
									if (Auto_Flag.AutoTray_LayIC && Auto_Flag.AutoTray_TakeIC)
									{
										if (TrayEndFlag.takeLayIC[1])
										{
											RunState = RUNSTATE.End;
										}
										else
										{
											if (Auto_Flag.Production && TIC_ValidC >= TargetC)
											{
												if (TrayEndFlag.takeIC[1] || TrayD.TIC_TrayN != 2)
												{
													RunState = RUNSTATE.End;
													TrayEndFlag.takeLayIC[1] = true;
												}
												else
												{
													TrayEndFlag.tray2Burn = true;
													RunState = RUNSTATE.Carrier_TakeIC;
												}
											}
											else
											{
												RunState = RUNSTATE.End;
											}
										}
									}
									else
									{
										RunState = RUNSTATE.End;
									}
								}
							}
							else
							{
								RunState = RUNSTATE.Carrier_TakeIC;
							}
							TIC_PenN = 0;
						}
					}
					break;

				case RUNSTATE.Carrier_LayOKIC:
					if (OKPenC == 0)//吸笔没有料
					{
                        if (Auto_Flag.RotateChange)
                        {
                            Auto_Flag.RotateChange = false;
                        }
                        if (Auto_Flag.Production && TIC_ValidC >= TargetC)
						{
							Auto_Flag.ProductionFinish = true;
						}
						if (Auto_Flag.Brede_LayIC)
						{
							Brede_Trigger.Auto_Brede = true;
						}
						if (Auto_Flag.Ending)
						{
							if (TIC_UnitC != 0)
							{
								RunState = RUNSTATE.BurnSeat_TakeIC;
								Auto_Flag.BurnSeat_TakeIC = true;
							}
							else
							{
								if (Auto_Flag.AutoTray_LayIC && Auto_Flag.AutoTray_TakeIC)
								{
									if (TrayEndFlag.takeLayIC[1])
									{
										RunState = RUNSTATE.End;
									}
									else
									{
										if (Auto_Flag.Production && TIC_ValidC >= TargetC)
										{
											if (TrayEndFlag.takeIC[1] || TrayD.TIC_TrayN != 2)
											{
												RunState = RUNSTATE.End;
												TrayEndFlag.takeLayIC[1] = true;
											}
											else
											{
												TrayEndFlag.tray2Burn = true;
												RunState = RUNSTATE.Carrier_TakeIC;
											}
										}
										else
										{
											RunState = RUNSTATE.End;
										}
									}
								}
								else
								{
									RunState = RUNSTATE.End;
								}
							}
						}
						else
						{
							RunState = RUNSTATE.Carrier_TakeIC;
						}
						TIC_PenN = 0;
					}
					break;

				case RUNSTATE.Photograph:
                    if (Config.CCDModel == 0)//定拍
                    {
                        if (PhotographPen_Check())
                        {
                            if (Auto_Flag.Cam_2D_Mode_II)//烧录后定位
                            {
                                Auto_Flag.Cam_2D_Mode_II = false;
                                if (NGPenC > 0)//吸笔有NG料
                                {
                                    RunState = RUNSTATE.Carrier_LayNGIC;
                                }
                                else if (OKPenC > 0)//吸笔有OK料
                                {
                                    RunState = RUNSTATE.Carrier_LayOKIC;
                                    if (Auto_Flag.Brede_LayIC)
                                    {
                                        Brede_Flag.ALarmCheck = true;
                                    }
                                }
                                LIC_PenN = 0;
                            }
                            else
                            {
                                if (Vision_3D.Enabled_I)//烧录前3D检测
                                {
                                    RunState = RUNSTATE.Detection;
                                    DIC_PenN = 0;
                                }
                                else
                                {
                                    RunState = RUNSTATE.BurnSeat_LayIC;
                                    LIC_PenN = 0;
                                }
                            }
                            //D00849 = 0; //关闭下光源
                            Auto_Flag.DownLightOn = false;
                            g_act.SendCmd(GlobConstData.ST_LIGHTDOWN, 0);
                            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送关闭下光源命令", "Flow");
                        }
                    }
                    else//飞拍
                    {
                        if (Auto_Flag.Cam_2D_Mode_II)//烧录后定位
                        {
                            Auto_Flag.Cam_2D_Mode_II = false;
                            if (NGPenC > 0)//吸笔有NG料
                            {
                                RunState = RUNSTATE.Carrier_LayNGIC;
                            }
                            else if (OKPenC > 0)//吸笔有OK料
                            {
                                RunState = RUNSTATE.Carrier_LayOKIC;
                                if (Auto_Flag.Brede_LayIC)
                                {
                                    Brede_Flag.ALarmCheck = true;
                                }
                            }
                            LIC_PenN = 0;
                        }
                        else
                        {
                            RunState = RUNSTATE.BurnSeat_LayIC;
                            LIC_PenN = 0;
                        }
                        //D00849 = 0; //关闭下光源
                        Auto_Flag.DownLightOn = false;
                        g_act.SendCmd(GlobConstData.ST_LIGHTDOWN, 0);
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送关闭下光源命令", "Flow");
                    }
					break;

				case RUNSTATE.Detection://3D检测
                    DetectionPen_Check();
                    if (DIC_PenN == UserConfig.VacuumPenC)
                    {
                        if (Auto_Flag.Cam_3D_Mode_II)//烧录座取料后3D检测模式
                        {
                            Auto_Flag.Cam_3D_Mode_II = false;
                            if (Auto_Flag.Enabled_LayICPos)//烧录后定位拍照
                            {
                                PhotographLightOn_Handle(true);
                            }
                            else
                            {
                                LIC_PenN = 0;
                                if (NGPenC > 0)
                                {
                                    RunState = RUNSTATE.Carrier_LayNGIC;
                                }
                                else if (OKPenC > 0)
                                {
                                    RunState = RUNSTATE.Carrier_LayOKIC;
                                    if (Auto_Flag.Brede_LayIC)
                                    {
                                        Brede_Flag.ALarmCheck = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (NGPenC > 0)//吸笔有NG料（3D检测未通过的料）
                            {
                                RunState = RUNSTATE.Carrier_LayDNGIC;
                            }
                            else if (DIC_OKPenC > 0)//吸笔有OK料（3D检测通过的料）
                            {
                                RunState = RUNSTATE.BurnSeat_LayIC;
                            }
                        }
                        LIC_PenN = 0;
                    }
                    break;

                case RUNSTATE.Emptying://测高仪排空
                    bool flag = true;
                    if (SyncPen[SyncPenN].Flag_First)//第一次排空
                    {
                        flag = false;
                        if (!SyncPen[SyncPenN].Flag_TakeIC)//没有料
                        {
                            Group[SyncPen[SyncPenN].GroupN].Unit[SyncPen[SyncPenN].UnitN].Flag_First = false;
                            SyncPen[SyncPenN].Flag_LayIC = true;
                            SyncPen_Struct.State_LayIC = true;
                        }
                    }
                    SyncPenN += Auto_Flag.Ascending_ICPos ? 1 : -1;
                    while (true)
                    {
                        if (SyncPenN == Sync_UnitC || SyncPenN == -1)
                        {
                            if (SyncPen_Struct.State_TakeIC)
                            {
                                RunState = RUNSTATE.BurnSeat_TakeIC;
                            }
                            else
                            {
                                for (int i = 0; i < Sync_UnitC; i++)
                                {
                                    if (SyncPen[i].Flag_LayIC)
                                    {
                                        if (SyncPen[i].Flag_First)//第一次排空
                                        {
                                            SyncPen[i].Flag_First = false;
                                            if (Group[SyncPen[i].GroupN].Unit[SyncPen[i].UnitN].Flag_Open)
                                            {
                                                SyncPen_Struct.State_LayIC = true;
                                                SyncPen_LIC_UnitC(i);
                                            }
                                            else
                                            {
                                                SyncPen[i].Flag_LayIC = false;
                                            }
                                        }
                                    }
                                }
                                DownTrigger_SyncPenCheck();
                                LogicJudgment_Cam_3D_Mode_II(SyncPen_Struct.State_LayIC);
                            }
                            break;
                        }
                        if (SyncPen[SyncPenN].Flag_First && !flag)//第一次排空
                        {
                            break;
                        }
                        if (SyncPen[SyncPenN].Flag_TakeIC && flag)
                        {
                            break;
                        }
                        SyncPenN += Auto_Flag.Ascending_ICPos ? 1 : -1;
                    }
                    break;

                case RUNSTATE.Carrier_LayDNGIC:
					if (NGPenC == 0)//吸笔没有有NG料
					{
						if (OKPenC > 0)//吸笔有OK料
						{
							RunState = RUNSTATE.BurnSeat_LayIC;
							LIC_PenN = 0;
						}
						else//吸笔没有料
						{
							if (IC_SupplyC == 0 || Auto_Flag.Ending)
							{
								if (TIC_UnitC != 0)
								{
									RunState = RUNSTATE.BurnSeat_TakeIC;
									Auto_Flag.BurnSeat_TakeIC = true;
								}
								else
								{
									if (Auto_Flag.Production && TIC_ValidC >= TargetC)
									{
										Auto_Flag.ProductionFinish = true;
									}
									if (Auto_Flag.AutoTray_LayIC && Auto_Flag.AutoTray_TakeIC)
									{
										if (TrayEndFlag.takeLayIC[1])
										{
											RunState = RUNSTATE.End;
										}
										else
										{
											if (Auto_Flag.Production && TIC_ValidC >= TargetC)
											{
												if (TrayEndFlag.takeIC[1] || TrayD.TIC_TrayN != 2)
												{
													RunState = RUNSTATE.End;
													TrayEndFlag.takeLayIC[1] = true;
												}
												else
												{
													TrayEndFlag.tray2Burn = true;
													RunState = RUNSTATE.Carrier_TakeIC;
												}
											}
											else
											{
												RunState = RUNSTATE.End;
											}
										}
									}
									else
									{
										RunState = RUNSTATE.End;
									}
								}
							}
							else
							{
								RunState = RUNSTATE.Carrier_TakeIC;
							}
							TIC_PenN = 0;
						}
					}
					break;

				case RUNSTATE.Carrier_LayIC:
					if (ExistICPenC == 0)
					{
						if (!TrayEndFlag.takeLayIC[1])
						{
							RunState = RUNSTATE.Carrier_TakeIC;
							TIC_PenN = 0;
						}
						else
						{
							RunState = RUNSTATE.End;
							TrayEndFlag.tray2Burn = false;
						}
					}
					break;

                case RUNSTATE.ScanCode:
                    if (ScannerResult == 1)//扫码OK
                    {
                        if (ExistICPenC - SNGPenC > 0)
                        {
                            RunState = RUNSTATE.BurnSeat_LayIC;
                        }
                        else if(SNGPenC > 0)
                        {
                            RunState = RUNSTATE.Carrier_LayNGIC;
                            SNGPenC = 0;
                            LIC_PenN = 0;
                        }
                        else
                        {
                            if (IC_SupplyC == 0 || Auto_Flag.Ending)
                            {
                                if (IC_SupplyC == 0)
                                {
                                    LIC_UnitN = TIC_UnitN;
                                    LIC_GroupN = TIC_GroupN;
                                }
                                RunState = RUNSTATE.BurnSeat_TakeIC;
                                Auto_Flag.BurnSeat_TakeIC = true;
                            }
                            else
                            {
                                RunState = RUNSTATE.Carrier_TakeIC;
                            }
                            TIC_PenN = 0;
                        }
                    }
                    else//扫码NG
                    {
                        RunState = RUNSTATE.TakeSNGIC;
                        if (SNGPenC == 0)
                        {
                            TIC_PenN = 0;
                        }
                    }
                    break;

                case RUNSTATE.TakeSNGIC:
                    if (ExistICPenC - SNGPenC > 0)
                    {
                        RunState = RUNSTATE.BurnSeat_LayIC;
                    }
                    else
                    {
                        RunState = RUNSTATE.Carrier_LayNGIC;
                        SNGPenC = 0;
                        LIC_PenN = 0;
                    }
                    break;

                case RUNSTATE.End:

					break;

				default:
					break;
			}
		}

        /// <summary>
        /// 吸笔交替模式主控逻辑程序
        /// </summary>
        public void MasterLogic_Program_II()
        {
            switch (RunState)
            {
                case RUNSTATE.BurnSeat_TakeIC:
                    if (Auto_Flag.Burn_Again)//重烧
                    {
                        RunState = RUNSTATE.Burn_Again;
                        Auto_Flag.Burn_Again = false;
                    }
                    else
                    {
                        if (ExistRawICPenC > 0)
                        {
                            RunState = RUNSTATE.BurnSeat_LayIC;
                        }
                        else if (!Auto_Flag.Ending || (Auto_Flag.Ending && TIC_PenN >= UserConfig.VacuumPenC))//还没收尾或者收尾了没有吸笔取料
                        {
                            if (Auto_Flag.Seat_EndTakeIC)
                            {
                                Auto_Flag.Seat_EndTakeIC = false;
                            }
                            LogicJudgment_Cam_3D_Mode_II();
                        }
                    }
                    break;

                case RUNSTATE.Carrier_TakeIC:
                    if (TrayEndFlag.tray2Burn)
                    {
                        if (ExistICPenC == EnalePenC || Auto_Flag.ManualEnd || TrayEndFlag.takeLayIC[1])
                        {
                            if (ExistICPenC > 0)//吸笔有料
                            {
                                RunState = RUNSTATE.Carrier_LayIC;
                                LIC_PenN = 0;
                            }
                            else//吸笔没有料
                            {
                                RunState = RUNSTATE.End;
                                if (TrayEndFlag.takeLayIC[1])
                                {
                                    TrayEndFlag.tray2Burn = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (ExistRawICPenC == EnaleRawICPenC || Auto_Flag.Ending)
                        {
                            if (Auto_Flag.RotateChange)
                            {
                                Auto_Flag.RotateChange = false;
                            }
                            LogicJudgment_Cam_2D_Mode_I();
                        }
                    }
                    break;

                case RUNSTATE.BurnSeat_LayIC:
                    if (ProgrammerType == GlobConstData.Programmer_YED)
                    {
                        RunState = RUNSTATE.ScanCode;
                    }
                    else
                    {
                        if (ExistRawICPenC == 0)
                        {
                            LogicJudgment_Cam_3D_Mode_II();
                        }
                    }
                    break;

                case RUNSTATE.Burn_Again:
                    if (Auto_Flag.Seat_EndTakeIC)
                    {
                        Auto_Flag.Seat_EndTakeIC = false;
                        if (NGPenC > 0)//吸笔有NG料
                        {
                            RunState = RUNSTATE.Carrier_LayNGIC;
                            LIC_PenN = 0;
                        }
                        else if (OKPenC > 0)//吸笔有OK料
                        {
                            RunState = RUNSTATE.Carrier_LayOKIC;
                            if (Auto_Flag.Brede_LayIC)
                            {
                                Brede_Flag.ALarmCheck = true;
                            }
                            LIC_PenN = 0;
                        }
                        else//吸笔没有料
                        {
                            if (Auto_Flag.Seat_EndLayIC || Auto_Flag.ForceEnd)
                            {
                                if (Auto_Flag.Seat_EndLayIC)
                                {
                                    Auto_Flag.Seat_EndLayIC = false;
                                }
                                RunState = RUNSTATE.BurnSeat_TakeIC;
                                Auto_Flag.BurnSeat_TakeIC = true;
                            }
                            else
                            {
                                RunState = RUNSTATE.Carrier_TakeIC;
                            }
                            TIC_PenN = 0;
                        }
                    }
                    else
                    {
                        RunState = RUNSTATE.BurnSeat_TakeIC;
                    }
                    break;

                case RUNSTATE.Carrier_LayNGIC:
                    if (NGPenC == 0)//吸笔没有有NG料
                    {
                        if (OKPenC > 0)//吸笔有OK料
                        {
                            RunState = RUNSTATE.Carrier_LayOKIC;
                            if (Auto_Flag.Brede_LayIC)
                            {
                                Brede_Flag.ALarmCheck = true;
                            }
                            LIC_PenN = 0;
                        }
                        else//吸笔没有料
                        {
                            if (Auto_Flag.Ending)
                            {
                                if (TIC_UnitC != 0)
                                {
                                    RunState = RUNSTATE.BurnSeat_TakeIC;
                                    Auto_Flag.BurnSeat_TakeIC = true;
                                }
                                else
                                {
                                    if (Auto_Flag.Production && TIC_ValidC >= TargetC)
                                    {
                                        Auto_Flag.ProductionFinish = true;
                                    }
                                    if (Auto_Flag.AutoTray_LayIC && Auto_Flag.AutoTray_TakeIC)
                                    {
                                        if (TrayEndFlag.takeLayIC[1])
                                        {
                                            RunState = RUNSTATE.End;
                                        }
                                        else
                                        {
                                            if (Auto_Flag.Production && TIC_ValidC >= TargetC)
                                            {
                                                if (TrayEndFlag.takeIC[1] || TrayD.TIC_TrayN != 2)
                                                {
                                                    RunState = RUNSTATE.End;
                                                    TrayEndFlag.takeLayIC[1] = true;
                                                }
                                                else
                                                {
                                                    TrayEndFlag.tray2Burn = true;
                                                    RunState = RUNSTATE.Carrier_TakeIC;
                                                }
                                            }
                                            else
                                            {
                                                RunState = RUNSTATE.End;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        RunState = RUNSTATE.End;
                                    }
                                }
                            }
                            else
                            {
                                RunState = RUNSTATE.Carrier_TakeIC;
                            }
                            TIC_PenN = 0;
                        }
                    }
                    break;

                case RUNSTATE.Carrier_LayOKIC:
                    if (OKPenC == 0)//吸笔没有料
                    {
                        if (Auto_Flag.RotateChange)
                        {
                            Auto_Flag.RotateChange = false;
                        }
                        if (Auto_Flag.Production && TIC_ValidC >= TargetC)
                        {
                            Auto_Flag.ProductionFinish = true;
                        }
                        if (Auto_Flag.Brede_LayIC)
                        {
                            Brede_Trigger.Auto_Brede = true;
                        }
                        if (Auto_Flag.Ending)
                        {
                            if (TIC_UnitC != 0)
                            {
                                RunState = RUNSTATE.BurnSeat_TakeIC;
                                Auto_Flag.BurnSeat_TakeIC = true;
                            }
                            else
                            {
                                if (Auto_Flag.AutoTray_LayIC && Auto_Flag.AutoTray_TakeIC)
                                {
                                    if (TrayEndFlag.takeLayIC[1])
                                    {
                                        RunState = RUNSTATE.End;
                                    }
                                    else
                                    {
                                        if (Auto_Flag.Production && TIC_ValidC >= TargetC)
                                        {
                                            if (TrayEndFlag.takeIC[1] || TrayD.TIC_TrayN != 2)
                                            {
                                                RunState = RUNSTATE.End;
                                                TrayEndFlag.takeLayIC[1] = true;
                                            }
                                            else
                                            {
                                                TrayEndFlag.tray2Burn = true;
                                                RunState = RUNSTATE.Carrier_TakeIC;
                                            }
                                        }
                                        else
                                        {
                                            RunState = RUNSTATE.End;
                                        }
                                    }
                                }
                                else
                                {
                                    RunState = RUNSTATE.End;
                                }
                            }
                        }
                        else
                        {
                            RunState = RUNSTATE.Carrier_TakeIC;
                        }
                        TIC_PenN = 0;
                    }
                    break;

                case RUNSTATE.Photograph:
                    if (Config.CCDModel == 0)//定拍
                    {
                        if (PhotographPen_Check())
                        {
                            if (Auto_Flag.Cam_2D_Mode_II)//烧录后定位
                            {
                                Auto_Flag.Cam_2D_Mode_II = false;
                                if (NGPenC > 0)//吸笔有NG料
                                {
                                    RunState = RUNSTATE.Carrier_LayNGIC;
                                }
                                else if (OKPenC > 0)//吸笔有OK料
                                {
                                    RunState = RUNSTATE.Carrier_LayOKIC;
                                    if (Auto_Flag.Brede_LayIC)
                                    {
                                        Brede_Flag.ALarmCheck = true;
                                    }
                                }
                                LIC_PenN = 0;
                            }
                            else
                            {
                                if (Vision_3D.Enabled_I)//烧录前3D检测
                                {
                                    RunState = RUNSTATE.Detection;
                                    DIC_PenN = 0;
                                }
                                else
                                {
                                    RunState = RUNSTATE.BurnSeat_LayIC;
                                    LIC_PenN = 0;
                                }
                            }
                            //D00849 = 0; //关闭下光源
                            Auto_Flag.DownLightOn = false;
                            g_act.SendCmd(GlobConstData.ST_LIGHTDOWN, 0);
                            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送关闭下光源命令", "Flow");
                        }
                    }
                    else//飞拍
                    {
                        if (Auto_Flag.Cam_2D_Mode_II)//烧录后定位
                        {
                            Auto_Flag.Cam_2D_Mode_II = false;
                            if (NGPenC > 0)//吸笔有NG料
                            {
                                RunState = RUNSTATE.Carrier_LayNGIC;
                            }
                            else if (OKPenC > 0)//吸笔有OK料
                            {
                                RunState = RUNSTATE.Carrier_LayOKIC;
                                if (Auto_Flag.Brede_LayIC)
                                {
                                    Brede_Flag.ALarmCheck = true;
                                }
                            }
                            LIC_PenN = 0;
                        }
                        else
                        {
                            RunState = RUNSTATE.BurnSeat_LayIC;
                            LIC_PenN = 0;
                        }
                        //D00849 = 0; //关闭下光源
                        Auto_Flag.DownLightOn = false;
                        g_act.SendCmd(GlobConstData.ST_LIGHTDOWN, 0);
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送关闭下光源命令", "Flow");
                    }
                    break;

                case RUNSTATE.Detection://3D检测
                    DetectionPen_Check();
                    if (DIC_PenN == UserConfig.VacuumPenC)
                    {
                        if (Auto_Flag.Cam_3D_Mode_II)//烧录座取料后3D检测模式
                        {
                            Auto_Flag.Cam_3D_Mode_II = false;
                            if (Auto_Flag.Enabled_LayICPos)//烧录后定位拍照
                            {
                                PhotographLightOn_Handle(true);
                            }
                            else
                            {
                                LIC_PenN = 0;
                                if (NGPenC > 0)
                                {
                                    RunState = RUNSTATE.Carrier_LayNGIC;
                                }
                                else if (OKPenC > 0)
                                {
                                    RunState = RUNSTATE.Carrier_LayOKIC;
                                    if (Auto_Flag.Brede_LayIC)
                                    {
                                        Brede_Flag.ALarmCheck = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (NGPenC > 0)//吸笔有NG料（3D检测未通过的料）
                            {
                                RunState = RUNSTATE.Carrier_LayDNGIC;
                            }
                            else if (DIC_OKPenC > 0)//吸笔有OK料（3D检测通过的料）
                            {
                                RunState = RUNSTATE.BurnSeat_LayIC;
                            }
                        }
                        LIC_PenN = 0;
                    }
                    break;

                case RUNSTATE.Emptying://测高仪排空
                    RunState = RUNSTATE.BurnSeat_LayIC;
                    break;

                case RUNSTATE.Carrier_LayDNGIC:
                    if (NGPenC == 0)//吸笔没有有NG料
                    {
                        if (OKPenC > 0)//吸笔有OK料
                        {
                            RunState = RUNSTATE.BurnSeat_LayIC;
                            LIC_PenN = 0;
                        }
                        else//吸笔没有料
                        {
                            if (IC_SupplyC == 0 || Auto_Flag.Ending)
                            {
                                if (TIC_UnitC != 0)
                                {
                                    RunState = RUNSTATE.BurnSeat_TakeIC;
                                    Auto_Flag.BurnSeat_TakeIC = true;
                                }
                                else
                                {
                                    if (Auto_Flag.Production && TIC_ValidC >= TargetC)
                                    {
                                        Auto_Flag.ProductionFinish = true;
                                    }
                                    if (Auto_Flag.AutoTray_LayIC && Auto_Flag.AutoTray_TakeIC)
                                    {
                                        if (TrayEndFlag.takeLayIC[1])
                                        {
                                            RunState = RUNSTATE.End;
                                        }
                                        else
                                        {
                                            if (Auto_Flag.Production && TIC_ValidC >= TargetC)
                                            {
                                                if (TrayEndFlag.takeIC[1] || TrayD.TIC_TrayN != 2)
                                                {
                                                    RunState = RUNSTATE.End;
                                                    TrayEndFlag.takeLayIC[1] = true;
                                                }
                                                else
                                                {
                                                    TrayEndFlag.tray2Burn = true;
                                                    RunState = RUNSTATE.Carrier_TakeIC;
                                                }
                                            }
                                            else
                                            {
                                                RunState = RUNSTATE.End;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        RunState = RUNSTATE.End;
                                    }
                                }
                            }
                            else
                            {
                                RunState = RUNSTATE.Carrier_TakeIC;
                            }
                            TIC_PenN = 0;
                        }
                    }
                    break;

                case RUNSTATE.Carrier_LayIC:
                    if (ExistICPenC == 0)
                    {
                        if (!TrayEndFlag.takeLayIC[1])
                        {
                            RunState = RUNSTATE.Carrier_TakeIC;
                            TIC_PenN = 0;
                        }
                        else
                        {
                            RunState = RUNSTATE.End;
                            TrayEndFlag.tray2Burn = false;
                        }
                    }
                    break;

                case RUNSTATE.ScanCode:
                    if (ScannerResult == 1)//扫码OK
                    {
                        if (ExistICPenC - SNGPenC > 0)
                        {
                            RunState = RUNSTATE.BurnSeat_LayIC;
                        }
                        else if (SNGPenC > 0)
                        {
                            RunState = RUNSTATE.Carrier_LayNGIC;
                            SNGPenC = 0;
                            LIC_PenN = 0;
                        }
                        else
                        {
                            if (IC_SupplyC == 0 || Auto_Flag.Ending)
                            {
                                if (IC_SupplyC == 0)
                                {
                                    LIC_UnitN = TIC_UnitN;
                                    LIC_GroupN = TIC_GroupN;
                                }
                                RunState = RUNSTATE.BurnSeat_TakeIC;
                                Auto_Flag.BurnSeat_TakeIC = true;
                            }
                            else
                            {
                                RunState = RUNSTATE.Carrier_TakeIC;
                            }
                            TIC_PenN = 0;
                        }
                    }
                    else//扫码NG
                    {
                        RunState = RUNSTATE.TakeSNGIC;
                        if (SNGPenC == 0)
                        {
                            TIC_PenN = 0;
                        }
                    }
                    break;

                case RUNSTATE.TakeSNGIC:
                    if (ExistICPenC - SNGPenC > 0)
                    {
                        RunState = RUNSTATE.BurnSeat_LayIC;
                    }
                    else
                    {
                        RunState = RUNSTATE.Carrier_LayNGIC;
                        SNGPenC = 0;
                        LIC_PenN = 0;
                    }
                    break;

                case RUNSTATE.End:

                    break;

                default:
                    break;
            }
        }

        /// <summary>
		/// 转移模式主控逻辑程序
		/// </summary>
		public void MasterLogic_Program_III()
        {
            switch (RunState)
            {
                case RUNSTATE.Carrier_TakeIC:
                    if (ExistICPenC == EnalePenC || Auto_Flag.Ending)
                    {
                        if (Auto_Flag.RotateChange)
                        {
                            Auto_Flag.RotateChange = false;
                        }
                        if (ExistICPenC > 0)//吸笔有料
                        {
                            if (Auto_Flag.Enabled_TakeICPos)
                            {
                                PhotographLightOn_Handle();
                            }
                            else
                            {
                                RunState = RUNSTATE.Carrier_LayOKIC;
                                if (Auto_Flag.Brede_LayIC)
                                {
                                    Brede_Flag.ALarmCheck = true;
                                }
                                LIC_PenN = 0;
                            }
                        }
                        else//吸笔没有料
                        {
                            RunState = RUNSTATE.End;
                        }
                    }
                    break;

                case RUNSTATE.Photograph:
                    if (Config.CCDModel == 0)//定拍
                    {
                        if (PhotographPen_Check())
                        {
                            RunState = RUNSTATE.Carrier_LayOKIC;
                            if (Auto_Flag.Brede_LayIC)
                            {
                                Brede_Flag.ALarmCheck = true;
                            }
                            LIC_PenN = 0;
                            //D00849 = 0; //关闭下光源
                            Auto_Flag.DownLightOn = false;
                            g_act.SendCmd(GlobConstData.ST_LIGHTDOWN, 0);
                            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送关闭下光源命令", "Flow");
                        }
                    }
                    else//飞拍
                    {
                        RunState = RUNSTATE.Carrier_LayOKIC;
                        if (Auto_Flag.Brede_LayIC)
                        {
                            Brede_Flag.ALarmCheck = true;
                        }
                        LIC_PenN = 0;
                        //D00849 = 0; //关闭下光源
                        Auto_Flag.DownLightOn = false;
                        g_act.SendCmd(GlobConstData.ST_LIGHTDOWN, 0);
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送关闭下光源命令", "Flow");
                    }
                    break;

                case RUNSTATE.Carrier_LayOKIC:
                    if (OKPenC == 0)//吸笔没有料
                    {
                        if (Auto_Flag.RotateChange)
                        {
                            Auto_Flag.RotateChange = false;
                        }
                        if (Auto_Flag.Production && TIC_ValidC >= TargetC)
                        {
                            Auto_Flag.ProductionFinish = true;
                        }
                        if (Auto_Flag.Brede_LayIC)
                        {
                            Brede_Trigger.Auto_Brede = true;
                        }
                        if (Auto_Flag.Ending)
                        {
                            RunState = RUNSTATE.End;
                        }
                        else
                        {
                            RunState = RUNSTATE.Carrier_TakeIC;
                        }
                        TIC_PenN = 0;
                    }
                    break;

                case RUNSTATE.End:

                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 同步取放
        /// </summary>
        public void Sync_Take_Lay(ref SyncPen_Struct syncPen)
        {
            if (syncPen.Step == 0)
            {
                return;
            }
            int penN = 0;
            double height = 0;
			string str = null;
			switch (syncPen.Step)
            {
                case 1:
                    if (!Pen[syncPen.PenN].Rotate.Busy)//轴c定位完成
                    {
						syncPen.TimeLostIC = GetSysTime() + 1000;
                        syncPen.Burn_Again = false;
                        syncPen.Step = 3;
                    }
                    else if (GetSysTime() > Pen[syncPen.PenN].Rotate.TimeOut)
                    {
                        BAR._ToolTipDlg.WriteToolTipStr("吸笔[" + (syncPen.PenN + 1).ToString() + "]旋转超时;[处理异常->确定->继续]");
                        BAR.ShowToolTipWnd(true);
                        syncPen.Step = 2;
                    }
                    break;

                case 2:
                    if (Continue_AfterSyncAlarm())
                    {
                        syncPen.Step = 1;
                    }
                    break;

                case 3: //获取垂直位置后			
                    if (RunState == RUNSTATE.BurnSeat_TakeIC)
                    {
                        syncPen.Step = 4;
                    }
                    else
                    {
                        if ((In_Output.vacuumI[syncPen.PenN].M && !Auto_Flag.TestMode) || Auto_Flag.TestMode)//取料成功
                        {
                            syncPen.Step = 40;
                        }
                        else
                        {
                            if (GetSysTime() > syncPen.TimeLostIC)
                            {
                                In_Output.vacuumO[syncPen.PenN].M = false;
								Pen[syncPen.PenN].ExistIC = false;
								ExistICPenC--;
                                if (Pen[syncPen.PenN].ExistRawIC)
                                {
                                    Pen[syncPen.PenN].ExistRawIC = false;
                                    ExistRawICPenC--;
                                }
                                IC_SupplyC++;
								if (Auto_Flag.Ending && !Auto_Flag.ForceEnd)
								{
									Auto_Flag.Ending = false;
								}

								if (Auto_Flag.Production && Auto_Flag.ProductionOK)
								{
									TIC_ValidC--;
								}
								BAR._ToolTipDlg.WriteToolTipStr("吸笔[" + (syncPen.PenN + 1).ToString() + "]物料丢失;[请人工寻找物料->确定->继续]");
                                BAR.ShowToolTipWnd(true);
                                syncPen.Step = 50;
                            }
                        }
                    }
                    break;

                case 4: //运行至垂直方向位置
                    if (PenType == 1)
                    {
                        if (!In_Output.penO[syncPen.PenN].M)
                        {
                            In_Output.penO[syncPen.PenN].M = true;
                            syncPen.TimeOut = GetSysTime() + 3000;
                        }
                        penN = 0;
                    }
                    else
                    {
                        penN = syncPen.PenN;
                    }
                    if (!axisSts_Z[penN].isBusy && !axisSts_Z[penN].isDone)
                    {
                        if (Config.Altimeter != 0)
                        {
                            height = (RunState == RUNSTATE.BurnSeat_TakeIC && !syncPen.Burn_Again) ? 0 : -Altimeter.HeightDifference;
                        }
                        else
                        {
                            height = 0;
                        }  
                        trapPrm_Z[penN].setPos = SyncPen[penN].HeightVal + height;
                        trapPrm_Z[penN].speed = runPrm_Z.speed;
                        trapPrm_Z[penN].acc = runPrm_Z.longAcc;
                        trapPrm_Z[penN].dec = runPrm_Z.longDec;
                    }
                    DST_Function(true, trapPrm_Z[penN], axisSts_Z[penN]);
                    if (axisSts_Z[penN].isDone)
                    {
                        if (PenType == 0)
                        {
                            axisSts_Z[penN].isDone = false;
                            syncPen.Step = 7;
                        }
                        else
                        {
                            syncPen.Step = 5;
                        }
                    }
                    break;

                case 5: //检测吸笔下降到位			
                    if (In_Output.penLimitI[syncPen.PenN].M)
                    {
                        syncPen.Step = 7;
                    }
                    else if (GetSysTime() > syncPen.TimeOut)
                    {
                        BAR._ToolTipDlg.WriteToolTipStr("吸笔[" + (syncPen.PenN + 1) + "]下降到位检测超时");
                        BAR.ShowToolTipWnd(true);
                        syncPen.Step = 6;
                    }
                    break;

                case 6:
                    if (Continue_AfterSyncAlarm())
                    {
                        syncPen.Step = 5;
                        syncPen.TimeOut = GetSysTime() + 3000;
                    }
                    break;

                case 7:
                    if (NextAction_Check())
                    {
                        if (RunState == RUNSTATE.BurnSeat_TakeIC && !syncPen.Burn_Again)
                        {
                            if (!Auto_Flag.TestMode)
                            {
                                syncPen.Step = 8;
                            }
                            else
                            {
                                Pen[syncPen.PenN].ExistIC = true;
                                syncPen.Step = 13;
                            }
                        }
                        else
                        {
                            if (!Auto_Flag.TestMode)
                            {
                                syncPen.Step = 10;
                            }
                            else
                            {
                                syncPen.Step = 13;
                            }
                        }
                    }
                    break;

                case 8: //取料
                    if (RunState == RUNSTATE.BurnSeat_TakeIC && !syncPen.Flag_TakeIC)
                    {
                        syncPen.VacuumDuration = GetSysTime() + 300;
                    }
                    else
                    {
                        syncPen.VacuumDuration = GetSysTime() + AutoTiming.VacuumDuration;
                    }
                    In_Output.vacuumO[syncPen.PenN].M = true;
					syncPen.Step = 9;
					break;

                case 9:
                    if (GetSysTime() > syncPen.VacuumDuration)
                    {
                        if (RunState == RUNSTATE.BurnSeat_TakeIC && !syncPen.Flag_TakeIC)
                        {
                            if(In_Output.vacuumI[syncPen.PenN].M)
                            {
                                Pen[syncPen.PenN].ExistIC = true;
                            }
                        }
                        syncPen.Step = 13;
                    }
                    break;

                case 10: //放料	
                    In_Output.vacuumO[syncPen.PenN].M = false;
                    syncPen.BlowDelay = GetSysTime() + AutoTiming.BlowDelay;
                    syncPen.Step = 11;
                    break;

                case 11:
                    if (GetSysTime() > syncPen.BlowDelay)
                    {
                        In_Output.blowO[syncPen.PenN].M = true;
                        syncPen.BlowDuration = GetSysTime() + AutoTiming.BlowDuration;
                        syncPen.Step = 12;
                    }
                    break;

                case 12:
                    if (GetSysTime() > syncPen.BlowDuration)
                    {
                        In_Output.blowO[syncPen.PenN].M = false;
                        Pen[syncPen.PenN].ExistIC = false;
                        syncPen.Step = 13;
                    }
                    break;

                case 13: //允许下一步动作
                    if (NextAction_Check())
                    {
                        syncPen.Step = 14;
                    }
                    break;

                case 14: //运行至安全高度
                    if (PenType == 1)
                    {
                        if (In_Output.penO[syncPen.PenN].M)
                        {
                            In_Output.penO[syncPen.PenN].M = false;
                            syncPen.TimeOut = GetSysTime() + 3000;
                        }
                    }
                    else
                    {
                        if (!axisSts_Z[syncPen.PenN].isBusy)
                        {
                            trapPrm_Z[syncPen.PenN].setPos = HeightVal.Safe;
                            trapPrm_Z[syncPen.PenN].speed = runPrm_Z.speed;
                            trapPrm_Z[syncPen.PenN].acc = runPrm_Z.longAcc;
                            trapPrm_Z[syncPen.PenN].dec = runPrm_Z.longDec;
                        }
                        DST_Function(true, trapPrm_Z[syncPen.PenN], axisSts_Z[syncPen.PenN]);
                        if (axisSts_Z[syncPen.PenN].isDone)
                        {
                            axisSts_Z[syncPen.PenN].isDone = false;
                            syncPen.Step = 15;
                            if (!Auto_Flag.TestMode)
                            {
                                if (RunState == RUNSTATE.BurnSeat_TakeIC && !syncPen.Burn_Again)//取料
                                {
                                    if (In_Output.vacuumI[syncPen.PenN].M)//取料成功
                                    {
                                        Pen[syncPen.PenN].ExistIC = true;
                                    }
                                    else
                                    {
                                        if (syncPen.Flag_TakeIC  || (Pen[syncPen.PenN].ExistIC && !syncPen.Flag_TakeIC))
                                        {
                                            BAR._ToolTipDlg.WriteToolTipStr("吸笔[" + (syncPen.PenN + 1) + "]真空取料失败;[检查真空是否异常或卡料->确定->继续]");
                                            BAR.ShowToolTipWnd(true);
                                            syncPen.Step = 16;
                                        }
                                        In_Output.vacuumO[syncPen.PenN].M = false;
                                        Pen[syncPen.PenN].ExistIC = false;
                                    }
                                }
                            }
                        }
                    }
                    break;

                case 15:
					syncPen.Step = 0;
					str = "吸头[" + (syncPen.PenN + 1) + "]到烧写座[" + (syncPen.GroupN + 1) + "_" + (syncPen.UnitN + 1);
					if (RunState == RUNSTATE.BurnSeat_TakeIC)
                    {
                        if (syncPen.Burn_Again)
                        {
                            Pen[syncPen.PenN].DownResult = 0;
                            SyncPen[syncPen.PenN].Flag_TakeIC = false;
                            SyncPen[syncPen.PenN].Flag_LayIC = false;
							Group[syncPen.GroupN].Waiting_To_Burn = true;
							Group[syncPen.GroupN].Unit[syncPen.UnitN].Flag_NewIC = true;
							str += "]重烧放料成功";
						}
                        else
                        {
							if (Pen[syncPen.PenN].ExistIC)
							{
								if (syncPen.Flag_First)
								{
                                    Pen[syncPen.PenN].DownResult = 3;
									NGPenC++;
									str += "]首次排空,取料成功";
								}
								else if (((Group[syncPen.GroupN].Unit[syncPen.UnitN].DownResult == 1) && !Auto_Flag.TestMode) || Auto_Flag.TestMode)//OK
								{
									str += "]取料成功,状态:OK";
									OKPenC++;
									Pen[syncPen.PenN].DownResult = 1;
									Group[syncPen.GroupN].Unit[syncPen.UnitN].Counter_Burn = 0;//NG重烧计数	
                                    Group[syncPen.GroupN].Unit[syncPen.UnitN].NGCounter_Shut = 0;//连续NG关闭烧录座计数器
                                }
								else//NG
								{
									str += "]取料成功,状态:NG";
									Pen[syncPen.PenN].DownResult = 2;
									if (Auto_Flag.ManualEnd)//手动收尾
									{
										NGPenC++;
										Group[syncPen.GroupN].Unit[syncPen.UnitN].Counter_Burn = 0;//NG重烧计数
									}
									else if (Group[syncPen.GroupN].Unit[syncPen.UnitN].Counter_Burn >= NGReBurnC)//重烧次数已用完
									{
										NGPenC++;
										Group[syncPen.GroupN].Unit[syncPen.UnitN].Counter_Burn = 0;//NG重烧计数 
									}
									else
									{
										syncPen.Burn_Again = true;
										Group[syncPen.GroupN].Unit[syncPen.UnitN].Counter_Burn++;
										Pen[syncPen.PenN].Counter_Burn = Group[syncPen.GroupN].Unit[syncPen.UnitN].Counter_Burn;
										syncPen.Step = 4;
									}
                                    if (!syncPen.Burn_Again)
                                    {
                                        if (Auto_Flag.Ending && !Auto_Flag.ForceEnd)
                                        {
                                            Auto_Flag.Ending = false;
                                        }

                                        if (Auto_Flag.Production && Auto_Flag.ProductionOK)
                                        {
                                            TIC_ValidC--;
                                        }
                                    }

                                    Group[syncPen.GroupN].Unit[syncPen.UnitN].NGAllC_Shut++;
                                    Group[syncPen.GroupN].Unit[syncPen.UnitN].NGCounter_Shut++;
                                    NGAllDoneC_Shut++;
                                    if (NGScketC_Shut != 0 && Group[syncPen.GroupN].Unit[syncPen.UnitN].NGAllC_Shut >= NGScketC_Shut)
                                    {
                                        Group[syncPen.GroupN].Unit[syncPen.UnitN].Flag_Open = false;
                                        Group[syncPen.GroupN].Unit[syncPen.UnitN].NGAllC_Shut = 0;
                                        evt.EnableSeat_Click(syncPen.GroupN, syncPen.UnitN);
                                    }
                                    if (NGContinueC_Shut != 0 && Group[syncPen.GroupN].Unit[syncPen.UnitN].NGCounter_Shut >= NGContinueC_Shut)
                                    {
                                        Group[syncPen.GroupN].Unit[syncPen.UnitN].Flag_Open = false;
                                        Group[syncPen.GroupN].Unit[syncPen.UnitN].NGCounter_Shut = 0;
                                        evt.EnableSeat_Click(syncPen.GroupN, syncPen.UnitN);
                                    }
                                    if (NGAllC_Shut != 0 && NGAllDoneC_Shut >= NGAllC_Shut)
                                    {
                                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "全部失败[ " + NGAllC_Shut + " ]次停止作业", "Flow");
                                        Auto_Flag.Next = true;
                                        Auto_Flag.ManualEnd = true;
                                        Auto_Flag.Pause = false;
                                        Auto_Flag.Ending = true;
                                        Auto_Flag.ForceEnd = true;
                                        NGAllDoneC_Shut = 0;
                                    }
                                }
							}
							else
							{
								str += "]首次排空,取料失败";
							}
							if (!syncPen.Burn_Again)
							{
								Group[syncPen.GroupN].Unit[syncPen.UnitN].Flag_TakeIC = false;
								if (Pen[syncPen.PenN].ExistIC)
								{
									double angle = Pen[syncPen.PenN].DownResult == 1 ? Get_LIC_RotateAngle() : RotateAngle.LIC_Tray[2];
									RotateTrigge(true, syncPen.PenN, angle);
									ExistICPenC++;                                   
                                }
								if (syncPen.Flag_First)
								{
                                    syncPen.Flag_First = false;
                                    Group[syncPen.GroupN].Unit[syncPen.UnitN].Flag_First = false;
								}
								else
								{
									TIC_UnitC--;
								}
								SyncPen[syncPen.PenN].Flag_LayIC = true;
							}
						}
                    }
                    else if (RunState == RUNSTATE.BurnSeat_LayIC)
                    {
						str += "]放料成功";
						ExistICPenC--;
                        if (Pen[syncPen.PenN].ExistRawIC)
                        {
                            Pen[syncPen.PenN].ExistRawIC = false;
                            ExistRawICPenC--;
                        }
                        TIC_UnitC++;
						LIC_UnitC--;
						SyncPen[syncPen.PenN].Flag_LayIC = false;
						Group[syncPen.GroupN].Unit[syncPen.UnitN].Flag_TakeIC = true;
						Group[syncPen.GroupN].Unit[syncPen.UnitN].Flag_LayIC = false;
						Group[syncPen.GroupN].Waiting_To_Burn = true;
						Group[syncPen.GroupN].Unit[syncPen.UnitN].Flag_NewIC = true;
						Group[syncPen.GroupN].LIC_UnitC--;
					}
					g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
					break;

                case 16:
                    if (Continue_AfterSyncAlarm())
                    {
                        if (RunState == RUNSTATE.BurnSeat_TakeIC && Auto_Flag.JumpMainStep_Flag)//跳过取料
                        {
                            Group[syncPen.GroupN].Unit[syncPen.UnitN].Flag_TakeIC = false;
                            if (syncPen.Flag_First)
                            {
                                syncPen.Flag_First = false;
                                Group[syncPen.GroupN].Unit[syncPen.UnitN].Flag_First = false;
                            }
                            else
                            {
                                TIC_UnitC--;
                            }
                            //syncPen.Burn_Again = true;
                            SyncPen[syncPen.PenN].Flag_LayIC = true;
                            //Auto_Flag.JumpMainStep_Flag = false;
                            //IC_SupplyC++;
                            if (Auto_Flag.Production && Auto_Flag.ProductionOK)
                            {
                                TIC_ValidC--;
                            }
                            syncPen.Step = 0;
                        }
                        else
                        {
                            syncPen.Step = 4;
                        }
                            
                    }
                    break;

				case 40:
					bool flag = false;
					if (Config.CCDModel == 0)//定拍
					{
						flag = true;
					}
					else//飞拍
					{
						if (!Pen[syncPen.PenN].Image_Busy)
						{
							if (Pen[syncPen.PenN].ImageResult)
							{
								flag = true;
							}
							else
							{
								Pen[syncPen.PenN].DownResult = 2;
								RotateTrigge(true, syncPen.PenN, RotateAngle.LIC_Tray[2]);
								NGPenC++;
                                IC_SupplyC++;
                                if (Auto_Flag.Ending && !Auto_Flag.ForceEnd)
                                {
                                    Auto_Flag.Ending = false;
                                }

                                if (Auto_Flag.Production && Auto_Flag.ProductionOK)
                                {
                                    TIC_ValidC--;
                                }
                                syncPen.Step = 0;
							}
						}
					}
					if (flag)
					{
                        if (Math.Abs(rectify[syncPen.PenN].AxisX) <= Deviate && Math.Abs(rectify[syncPen.PenN].AxisY) <= Deviate)
                        {
							syncPen.Step = 4;
						}
                        else
                        {
							syncPen.Step = 0;
							Auto_Flag.OverDeviation = true;
							syncPen.OverDeviation = true;
						}
					}
					break;

				case 50:
					if (Continue_AfterSyncAlarm())
					{
						syncPen.Step = 0;
					}
					break;

				default:
                    break;
            }
        }


        public void Auto_Run_Home_Return()
		{
			switch (ARHR_Step)
			{
				case 1:
					for (int i = 0; i < UserConfig.AllMotionC; i++)
					{
						In_Output.pushSeatO[i].M = false;
						In_Output.flipO[i].M = false;
					}
					AutoTimer.OriginZ_Check = GetSysTime() + 3000;
					ARHR_Step = 2;
					break;

				case 2: //Z轴安全检测
					if (OriginZ_Check() && PenHome_Check())
					{
						ARHR_Step = 4;
					}
					else if (GetSysTime() > AutoTimer.OriginZ_Check)
					{
						BAR._ToolTipDlg.WriteToolTipStr("Z轴未在安全检测高度");
						BAR.ShowToolTipWnd(true);
						ARHR_Step = 3;
					}
					break;

				case 3:
					if (Continue_AfterAlarm())
					{
						ARHR_Step = 2;
					}
					break;

				case 4:
					Horizontal_Position_Control(0, 0);
					if (Auto_Flag.Run_InPlace)//XY轴回原位
					{
						Auto_Flag.Run_InPlace = false;
						ARHR_Step = 5;
					}
					break;

				case 5:
					Home_Start();
					ARHR_Step = 6;
					break;

				case 6:
					if (!Auto_Flag.HomeBusy)
					{
						Auto_Flag.RunHome_End = true;
						ARHR_Step = 0;
					}
					break;

				default:
					break;
			}
		}

		public void Auto_Run_End() 
		{
			switch (ARE_Step)
			{
				case 1:
					if (!Auto_Flag.TestMode)
					{
						if (Auto_Flag.Brede_LayIC)
						{
							ARE_Step = 2;
						}
						else
						{
							ARE_Step = 7;
						}
					}
					else
					{
						if (Auto_Flag.Brede_LayIC)
						{
							ARE_Step = 6;
						}
						else
						{
							ARE_Step = 7;
						}
					}
					break;

				case 2:
					if (!Brede_Flag.Auto_Brede && !Brede_Trigger.Auto_Brede)//等待自动编带停止
					{
						ARE_Step = 3;
					}
					break;

				case 3:
					Brede.Alarm_Check();//编带警报查询
					if (Auto_Flag.ALarm)
					{
						ARE_Step = 4;
					}
					else
					{
						if (Auto_Flag.Brede_LayIC && Auto_Flag.ProductionFinish)
						{
							ARE_Step = 5;
							Brede_Trigger.End_Brede = true; //编带收尾触发标志 
						}
						else
						{
							ARE_Step = 7;
						}
					}
					break;

				case 4:
					if (Continue_AfterAlarm())
					{
						ARE_Step = 3;
					}
					break;

				case 5:
					if (!Brede_Flag.End_Brede)
					{
						ARE_Step = 7;
					}
					break;

				case 6:
					if (!Brede_Flag.Auto_Brede)
					{
						ARE_Step = 7;
					}
					break;

				case 7:
					if (Auto_Flag.RunHome_End)
					{
						Auto_Flag.RunHome_End = false;
						if (Auto_Flag.AutoTray_TakeIC || Auto_Flag.AutoTray_LayIC)
						{
							ARE_Step = 8;
						}
						else
						{
							ARE_Step = 10;
						}
					}
					break;

				case 8:
					if (!AutoTray_Process.ReceiveTray)
					{
						if (Auto_Flag.AutoTray_End || Auto_Flag.ManualEnd || Auto_Flag.ProductionFinish)//自动盘结束标志
						{
							Auto_Flag.AutoTray_End = false;
							if ((AutoTray.StatusAlarmWord & AutoTray.Status_TrayLack) != 0)
							{
								In_Output.buzzer.M = true;
							}
							ARE_Step = 10;
						}
						else
						{
							ARE_Step = 9;
						}
					}
					break;

				case 9:
                    if (Auto_Flag.ManualEnd)
                    {
                        if ((AutoTray.StatusAlarmWord & AutoTray.Status_TrayLack) != 0)
                        {
                            In_Output.buzzer.M = true;
                        }
                        ARE_Step = 10;
                    }
					else if (!AutoTray_Process.TakeTray)
					{
						Auto_Flag.Emptying = true;
						Auto_Flag.Ending = false;
						Auto_Flag.ForceEnd = false;
                        if (!Auto_Flag.BurnMode || Auto_Flag.PenAlt_Flag)
                        {
                            RunState = RUNSTATE.Carrier_TakeIC;
                        }
                        else
                        {
                            RunState = RUNSTATE.BurnSeat_TakeIC;
                            Auto_Flag.BurnSeat_TakeIC = true;
                        }
                        
						TIC_PenN = 0;
						WorkPenN = 0;
						TIC_UnitN = 0;
						LIC_UnitN = 0;
						TIC_GroupN = 0;
						LIC_GroupN = 0;
						TIC_UnitC = 0;
                        Get_TakeIC_Capacity(out TIC_Capacity);
                        OpenSocket_Auto_End();
                        Run.ARP_Step = 1;
                        Efficiency.Algorithm_Start();
                        ARE_Step = 0;
					}
					break;

				case 10:
					Auto_Flag.ManualEnd = false;
					Auto_Flag.AutoRunBusy = false;
					g_act.AutoRunShutoff();
                    g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "正常退出自动运行", "Flow");
					ARE_Step = 0;
					break;

				default:
					break;
			}
		}

	}
}
