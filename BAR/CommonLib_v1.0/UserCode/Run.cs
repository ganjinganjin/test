using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BAR.Windows;
using System.Windows.Forms;
using System.Threading;
using CCWin.SkinControl;
using BAR.Commonlib;
using PLC;
using System.Diagnostics;
using BAR.CommonLib_v1._0;

namespace BAR
{
    public class Run : UserTask
    {
        public static SkinCheckBox[] PenEnable;
        public static SkinCheckBox[] FeederEnable;
        public static RUNSTEP RunStep;
        public static int ARP_Step;
        public static Stopwatch RunStopWatch = new Stopwatch();
        public static Stopwatch PauseStopWatch = new Stopwatch();

        private void DownSeat_Init()
        {
            for (int i = 0; i < UserConfig.ScketGroupC; i++)
            {
                for (int j = 0; j < UserConfig.ScketUnitC; j++)
                {
                    Group[i].Unit[j].Flag_First = true;
                    Group[i].Unit[j].Flag_LayIC = false;
                    Group[i].Unit[j].Flag_TakeIC = false;
                    Group[i].Unit[j].Flag_NewIC = false;
                    Group[i].Unit[j].Flag_Error = false;
                    Group[i].Unit[j].Counter_Burn = 0;
                    Group[i].Unit[j].Counter_NG = 0;
                    Group[i].Unit[j].NGCounter_Shut = 0;
                    Group[i].Unit[j].NGAllC_Shut = 0;
                    NGAllDoneC_Shut = 0;
                }
                Group[i].Continue_NG = false;
                Group[i].Waiting_To_Burn = false;
                Group[i].Waiting_To_Take = false;
                Group[i].Waiting_To_Lay = false;
                Group[i].Flag_Error = false;
                Group[i].LIC_UnitC = 0;
            }           
            TIC_UnitN = 0;
            LIC_UnitN = 0;
            TIC_GroupN = 0;
            LIC_GroupN = 0;
            TIC_UnitC = 0;
            LIC_UnitC = 0;
            Auto_Flag.Seat_EndLayIC = false;
            Auto_Flag.Seat_EndTakeIC = false;
            Auto_Flag.Seat_TIC_Cycle = false;
        }

        private void VacuumPen_Init()
        {
            EnalePenC = 0;
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                Pen[i].ExistIC = false;
                Pen[i].ExistRawIC = false;
                Pen[i].DownTrigger = false;
                Pen[i].Rotate.Busy = false;
                Pen[i].DownResult = 0;
                Pen[i].DetectionResult = 0;
                Pen[i].Detection_Num = 0;
                Pen[i].Counter_Burn = 0;
                if (PenEnable[i].Checked)
                {
                    Pen[i].Enable = true;
                    EnalePenC++;
                }
                else
                {
                    Pen[i].Enable = false;
                }
            }
            bool flag = false;
            if (Config.SyncTakeLay && Auto_Flag.Enabled_Sync && ProgrammerType != GlobConstData.Programmer_YED)
            {
                int temp = 0;
                int firstPen = 0;
                for (int i = 0; i < UserConfig.VacuumPenC; i++)
                {
                    if (Pen[i].Enable)
                    {
                        if (firstPen == 0)
                        {
                            firstPen = i + 1;
                        }
                        temp++;
                    }
                }
                if (temp > 1)
                {
                    Sync_UnitC = temp;
                    for (int i = 0; i < Sync_UnitC; i++)
                    {
                        SyncPen[i].PenN = i + firstPen - 1;
                    }
                    flag = true;
                }
            }

            EnaleRawICPenC = EnalePenC / 2;
            Auto_Flag.PenAlt_Flag = EnaleRawICPenC > 0 && Auto_Flag.PenAltMode;
            if (Auto_Flag.PenAlt_Flag)
            {
                int temp = 0;
                flag = false;
                for (int i = 0; i < UserConfig.VacuumPenC; i++)
                {
                    if (Pen[i].Enable)
                    {
                        if (temp == EnaleRawICPenC)
                        {
                            Unit_FirstPenN = i;
                            break;
                        }
                        temp++;
                    }
                }
            }
            
            SyncPen_Struct.State_TakeIC = false;
            SyncPen_Struct.Seat_TIC_Cycle = false;
            Auto_Flag.OverDeviation = false;
            Auto_Flag.BurnSeat_Sync = flag;
            TIC_PenN = 0;
            LIC_PenN = 0;
            WorkPenN = 0;
            NGPenC = 0;
            OKPenC = 0;
            DIC_OKPenC = 0;
            ExistRawICPenC = 0;
            ExistICPenC = 0;
            SNGPenC = 0;
        }

        public Run()
        {
            RunStopWatch.Restart();
            PauseStopWatch.Reset();
            VacuumPen_Init();
            DownSeat_Init();
            Download.Init();
            if (Auto_Flag.Brede_LayIC || Auto_Flag.Brede_TakeIC)
            {
                Brede.Init();//编带初始化
                Brede.MODBUS_WriteBredeParameter();
            }
            TIC_TubeN = 0;
            DetectionType = Vision_3D.ICType;

            RunStep = RUNSTEP.AFTER_HP_GET;
            if (TrayEndFlag.tray2Burn || !Auto_Flag.BurnMode || Auto_Flag.PenAlt_Flag)
            {
                RunState = RUNSTATE.Carrier_TakeIC;
            }
            else
            {
                RunState = RUNSTATE.BurnSeat_TakeIC;
                Auto_Flag.BurnSeat_TakeIC = true;
            }
            
            Auto_Flag.Emptying = true;
            Auto_Flag.Ending = false;
            Auto_Flag.ForceEnd = false;
            Auto_Flag.ProductionFinish = false;
            Auto_Flag.Run_InPlace = false;
            Auto_Flag.BurnSeat_TakeNullIC = false;
            Auto_Flag.Burn_Again = false;
            Auto_Flag.Cam_2D_Mode_II = false;
            Auto_Flag.Cam_3D_Mode_II = false;

            Auto_Flag.Update_Tray = false;
            Auto_Flag.Tube_AllEmptying = false;
            Auto_Flag.RotateChange = false;
            AutoTimer.BredeTakeDelay = GetSysTime();
            Get_TakeIC_Capacity(out TIC_Capacity);
        }

        public void Auto_Run_Program()
        {
            int backStep = 0;
            int temp = 0;
            string str = null;
            Efficiency.freshCount = 0;
            while (Auto_Flag.AutoRunBusy)
            {
                //Thread.Sleep(1);
                Efficiency.Algorithm();
                GetCardMessage();
                #region----------------预改-------------------
                //switch (RunStep)
                //{
                //    case RUNSTEP.START: //

                //        break;

                //    case RUNSTEP.BEFORE_HP_GET: //

                //        break;

                //    case RUNSTEP.NOW_HP_GET: //

                //        break;

                //    case RUNSTEP.TUBE_WAIT: //		

                //        break;

                //    case RUNSTEP.AFTER_HP_GET: //

                //        break;

                //    case RUNSTEP.BEFORE_VP_GET: //

                //        break;

                //    case RUNSTEP.NOW_VP_GET: //

                //        break;

                //    case RUNSTEP.AFTER_VP_GET: //		

                //        break;

                //    case RUNSTEP.MOVE_VP: //

                //        break;

                //    case RUNSTEP.BEFORE_TLIC://

                //        break;

                //    case RUNSTEP.AFTER_TLIC: //

                //        break;

                //    case RUNSTEP.HEIGHT_SAFE: //

                //        break;

                //    case RUNSTEP.DATA_WORK://

                //        break;

                //    case RUNSTEP.ALARM_TIC_FAILURE://

                //        break;

                //    case RUNSTEP.DATA_DEBUG://

                //        break;

                //    case RUNSTEP.BEFORE_SEAT_TLIC://

                //        break;

                //    case RUNSTEP.ALARM_ACTION:

                //        break;

                //    case RUNSTEP.ALARM_WAIT:

                //        break;

                //    case RUNSTEP.MASTERLOGIC://

                //        break;

                //    case RUNSTEP.BREDE_WAIT:

                //        break;

                //    case RUNSTEP.BREDE_CHECK:

                //        break;

                //    case RUNSTEP.ALARM_BREDE:

                //        break;

                //    case RUNSTEP.END:

                //        break;

                //    default:
                //        break;
                //}
                #endregion
                switch (ARP_Step)
                {
                    case 1: //自动运行初始处理
                        if(HeightSafe_Handle())
                        {
                            if (Config.ZoomLens != 0 && Auto_Flag.AutoRevisePos) 
                            {
                                g_act.CCDCap(GlobConstData.ST_CCDUP, true);
                                g_act.CCDSnap(GlobConstData.ST_CCDUP);
                                TeachAction.RevisePos_Start();
                                ARP_Step = 300;
                            }
                            else
                            {
                                ARP_Step = 2;
                            }
                        }
                        break;

                    case 300:
                        if (!Auto_Flag.LearnBusy)
                        {
                            g_act.CCDCap(GlobConstData.ST_CCDDOWN, true);
                            g_act.CCDSnap(GlobConstData.ST_CCDDOWN);
                            if (Config.CCDModel == 1)
                            {
                                if (Config.CameraType == GlobConstData.Camera_DH)
                                {
                                    g_act.ArrDHCameraUtils[GlobConstData.ST_CCDDOWN].SetEnumValue("TriggerSource", "Line2");
                                }
                                else if(Config.CameraType == GlobConstData.Camera_HR)
                                {
                                    g_act.ArrHRCameraUtils[GlobConstData.ST_CCDDOWN].SetLineMode();
                                }
                                else
                                {
                                    g_act.ArrMVCameraUtils[GlobConstData.ST_CCDDOWN].SetLineMode();
                                }
                            }

                            ARP_Step = 2;
                        }
                        break;
			 
                    case 2: //允许下一步动作
                        if (NextAction_Check())
                        {
                            if (RunState == RUNSTATE.Carrier_TakeIC && Auto_Flag.ManualEnd)
                            {
                                //取消
                                ARP_Step = 15;
                            }
                            else if ((RunState == RUNSTATE.Emptying && !Auto_Flag.BurnSeat_Sync) || 
                                    (RunState == RUNSTATE.BurnSeat_TakeIC && Auto_Flag.BurnSeat_TakeNullIC))
                            {
                                //直接执行
                                ARP_Step = 4;
                            }
                            else 
                            {
                                //获取坐标后执行
                                ARP_Step = 3;
                            }    
                        }
                        break;
				
                    case 3: //获取水平方向位置
                        if (Auto_Flag.BurnMode)
                        {
                            if (Auto_Flag.PenAlt_Flag)
                            {
                                Get_Horizontal_Position_II();
                            }
                            else
                            {
                                Get_Horizontal_Position_I();
                            }
                        }
                        else
                        {
                            Get_Horizontal_Position_III();//转移模式
                        }

                        if (Auto_Flag.Get_Horizontal)
                        {
                            ARP_Step = Get_Horizontal_Position_successful();
                            if (ARP_Step == 4 || ARP_Step == 90)
                            {
                                backStep = ARP_Step;
                                ARP_Step = 200;
                            }
                        }
                        else
                        {
                            if (Alarm_Check())
                            {
                                ARP_Step = 60;
                            }
                        }
                        break;

                    case 4: //运行至水平方向位置
                        Horizontal_Position_Control(Position_X.Buffer, Position_Y.Buffer);
                        if (Auto_Flag.Run_InPlace)
                        {
                            Auto_Flag.Run_InPlace = false;
                            ARP_Step = 5;
                        }
                        break;

                    case 5: //XY到位到位后
                        temp = Horizontal_InPlace_Handle();
                        if (temp != 0)
                        {
                            if (ErrorCheck())
                            {
                                temp = 63;//Error停机
                            }
                            ARP_Step = temp;
                        }                        			
                        break;

                    case 6: //允许下一步动作
                        if (NextAction_Check())
                        {
                            if (Auto_Flag.BurnSeat_Sync && (RunState == RUNSTATE.BurnSeat_TakeIC || (RunState == RUNSTATE.BurnSeat_LayIC && !Auto_Flag.OverDeviation)))
                            {
                                for (int i = 0; i < Sync_UnitC; i++)
                                {
                                    if (((SyncPen[i].Flag_TakeIC || (SyncPen[i].Flag_First && Config.Altimeter == 0)) && RunState == RUNSTATE.BurnSeat_TakeIC) || 
                                        (Pen[SyncPen[i].PenN].ExistIC && RunState == RUNSTATE.BurnSeat_LayIC))
                                    {
                                        if (Config.Altimeter != 0)
                                        {
                                            SyncPen[i].HeightVal = Group[SyncPen[i].GroupN].Unit[SyncPen[i].UnitN].HeightVal - Altimeter.Thickness;
                                            SyncPen[i].HeightVal -= Pen[SyncPen[i].PenN].Altimeter_Z;
                                        }
                                        else
                                        {
                                            SyncPen[i].HeightVal = RunState == RUNSTATE.BurnSeat_TakeIC ? HeightVal.DownSeat_TakeIC : HeightVal.DownSeat_LayIC;//计算高度脉冲
                                        }
                                        SyncPen[i].Step = 1;
                                    }
                                    else
                                    {
                                        SyncPen[i].Step = 0;
                                    }
                                }
                                ARP_Step = 42;
                            }
                            else
                            {
                                Get_Vertical_Position();//获取垂直方向位置
                                ARP_Step = 41;
                            } 
                        }
                        break;

                    case 40:
                        if (Continue_AfterAlarm())
                        {
                            ARP_Step = 5;
                        }
                        break;

                    case 41:
                        if (!Pen[WorkPenN].Rotate.Busy && !Auto_Flag.ALarmPause)//轴c定位完成
                        {
                            if (RunState == RUNSTATE.Carrier_TakeIC && Auto_Flag.ManualEnd) //终止取料
                            {
                                ARP_Step = 15;
                            }
                            else
                            {
                                AutoTimer.LostIC = GetSysTime() + 1000;
                                ARP_Step = 7;
                            }
                        }
                        else if (GetSysTime() > Pen[WorkPenN].Rotate.TimeOut)
                        {
                            if (!Auto_Flag.ALarmPause)
                            {
                                BAR._ToolTipDlg.WriteToolTipStr("吸笔[" + (WorkPenN + 1).ToString() + "]旋转超时[处理异常->确定->继续]");
                                BAR.ShowToolTipWnd(true);
                            }
                            else
                            {
                                Continue_AfterAlarm();
                            }
                        }
                        break;

                    case 42:
                        int pen = 0;
                        for (int i = 0; i < Sync_UnitC; i++)
                        {
                            Sync_Take_Lay(ref SyncPen[i]);
                            if (SyncPen[i].Step == 0)
                            {
                                pen++;
                            }
                        }
                        if (pen == Sync_UnitC)
                        {
                            ARP_Step = 44;
                            if (PenType == 1)
                            {
                                axisSts_Z[0].isDone = false;
                                ARP_Step = 43;
                            }   
                        }
                        break;

                    case 43:
                        Vertical_Position_Control(0, HeightVal.Safe);
                        if (Auto_Flag.Run_InPlace)
                        {
                            Auto_Flag.Run_InPlace = false;
                            ARP_Step = 44;
                        }
                        break;

                    case 44:
                        if (RunState == RUNSTATE.BurnSeat_TakeIC)
                        {
                            for (int i = 0; i < Sync_UnitC; i++)
                            {
                                if (SyncPen[i].Flag_LayIC)
                                {
                                    if (SyncPen[i].Flag_First)//第一次排空
                                    {
                                        SyncPen[i].Flag_First = false;
                                    }
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
                        else if (RunState == RUNSTATE.BurnSeat_LayIC)
                        {
                            if (Auto_Flag.OverDeviation)
                            {
                                SyncPenN = 0;
                            }
                        }
                        ARP_Step = 15;
                        break;

                    case 7: //获取垂直位置后			
                        if(RunState == RUNSTATE.Carrier_TakeIC || RunState == RUNSTATE.BurnSeat_TakeIC ||  RunState == RUNSTATE.Detection || RunState == RUNSTATE.TakeSNGIC)
                        {
                            ARP_Step = 70; 
                        }
                        else
                        {
                            if (RunState == RUNSTATE.BurnSeat_LayIC && Auto_Flag.OverDeviation)
                            {
                                ARP_Step = 70;
                            }
                            else if((In_Output.vacuumI[WorkPenN].M && !Auto_Flag.TestMode) || Auto_Flag.TestMode)//取料成功
                            {
                                ARP_Step = 70;
                            }
                            else
                            {
                                if (GetSysTime() > AutoTimer.LostIC)
                                {
                                    In_Output.vacuumO[WorkPenN].M = false;
                                    BAR._ToolTipDlg.WriteToolTipStr("吸笔["+(WorkPenN + 1).ToString() + "]物料丢失;[请人工寻找物料->确定->继续]");
                                    BAR.ShowToolTipWnd(true);
                                    ARP_Step = 50;
                                }
                            }
                        }
                        break;	
				
                    case 70: //运行至垂直方向位置			
                        Vertical_Position_Control(WorkPenN, HeightVal.Buffer, true);		
                        if(Auto_Flag.Run_InPlace)
                        {
                            Auto_Flag.Run_InPlace = false;
                            ARP_Step = PenType == 1 ? 71 : 8;
                        }
                        break;

                    case 71: //检测吸笔下降到位			
                        if (In_Output.penLimitI[WorkPenN].M)
                        {
                            ARP_Step = 8;
                        }
                        else if (GetSysTime() > AutoTimer.OriginZ_Check)
                        {
                            BAR._ToolTipDlg.WriteToolTipStr("吸笔[" + WorkPenN + "]下降到位检测超时");
                            BAR.ShowToolTipWnd(true);
                            ARP_Step = 72;
                        }
                        break;

                    case 72:
                        if (Continue_AfterAlarm())
                        {
                            ARP_Step = 71;
                            AutoTimer.OriginZ_Check = GetSysTime() + 3000;
                        }
                        break;

                    case 8:
                        if(NextAction_Check())
                        {
                            if(RunState == RUNSTATE.Detection)
                            {		
                                if(!Auto_Flag.TestMode)
                                {
                                    //D00847 = 1;//触发检测
                                }						
                                ARP_Step = 80; 						
                            }
                            else if(RunState == RUNSTATE.Carrier_TakeIC && Auto_Flag.ManualEnd)//终止取料
                            {
                                ARP_Step = 11; 
                            }
                            else if(RunState == RUNSTATE.Carrier_TakeIC || RunState == RUNSTATE.BurnSeat_TakeIC || RunState == RUNSTATE.TakeSNGIC)
                            {
                                if (!Auto_Flag.TestMode)
                                {
                                    ARP_Step = 9;
                                    TIC_Step = 1;
                                }
                                else
                                {
                                    Pen[WorkPenN].ExistIC = true;
                                    ARP_Step = 11;
                                }                              
                            }
                            else
                            {
                                if (!Auto_Flag.TestMode)
                                {
                                    ARP_Step = 10;
                                    LIC_Step = 1;
                                }
                                else
                                {
                                    ARP_Step = 11;
                                }                             
                            }
                        }
                        break;
			
                    case 9: //取料
                        if (RunState == RUNSTATE.BurnSeat_TakeIC && Group[TIC_GroupN].Unit[TIC_UnitN].Flag_First && !Auto_Flag.BurnSeat_TakeNullIC)
                        {
                            TakeIC_Handle(WorkPenN, 300);
                        }
                        else
                        {
                            TakeIC_Handle(WorkPenN, AutoTiming.VacuumDuration);
                        }               
                        if(TIC_Step == 0)
                        {
                            if (RunState == RUNSTATE.BurnSeat_TakeIC && Group[TIC_GroupN].Unit[TIC_UnitN].Flag_First && !Auto_Flag.BurnSeat_TakeNullIC)
                            {
                                if(In_Output.vacuumI[WorkPenN].M)
                                {
                                    Pen[WorkPenN].ExistIC = true;
                                }
                                else
                                {
                                    In_Output.vacuumO[WorkPenN].M = false;
                                }
                            }                        
                            ARP_Step = 11; 
                        }
                        break;
				
                    case 10: //放料				
                        LayIC_Handle(WorkPenN);
                        if(LIC_Step == 0)
                        {
                            ARP_Step = 11;
                        }				
                        break;
				
                    case 11: //允许下一步动作
                        if(NextAction_Check())
                        {
                            ARP_Step = 12;
                        }
                        break;
				
                    case 12: //运行至安全高度
                        Vertical_Position_Control(WorkPenN, HeightVal.Safe);
                        if(Auto_Flag.Run_InPlace)
                        {
                            Auto_Flag.Run_InPlace = false;
                            if(RunState == RUNSTATE.Detection)
                            {
                                ARP_Step = 81;
                            }
                            else
                            {
                                if(!Auto_Flag.TestMode)
                                {
                                    ARP_Step = 13;
                                }
                                else
                                {
                                    ARP_Step = 14; 
                                }
                            }									
                        }
                        break;
				
                    case 13://烧录模式
                        if(RunState == RUNSTATE.BurnSeat_TakeIC || RunState == RUNSTATE.Carrier_TakeIC || RunState == RUNSTATE.TakeSNGIC)//取料
                        {
                            if(In_Output.vacuumI[WorkPenN].M)//取料成功
                            {	
                                Pen[WorkPenN].ExistIC = true;
                                if(RunState == RUNSTATE.BurnSeat_TakeIC)
                                {
                                    if (Config.Altimeter != 0 && Auto_Flag.Enabled_Overlay)
                                    {
                                        GetSocketXY(TIC_GroupN, TIC_UnitN);
                                        RunState = RUNSTATE.Emptying;
                                        str = "吸头[" + (WorkPenN + 1) + "]到烧写座[" + (TIC_GroupN + 1) + "_" + (TIC_UnitN + 1) + "]取料成功";
                                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
                                        ARP_Step = 2;
                                        break;
                                    }
                                    else
                                    {
                                        BurnSeat_TakeIC_Data_Operation();
                                    }
                                }
                                else if (RunState == RUNSTATE.Carrier_TakeIC)
                                {     
                                    Carrier_TakeIC_Data_Operation();
                                }
                                else if (RunState == RUNSTATE.TakeSNGIC)
                                {
                                    TakeSNGIC_Data_Operation();
                                }
                                ARP_Step = 15;
                            }
                            else
                            {
                                if(Group[TIC_GroupN].Unit[TIC_UnitN].Flag_First && RunState == RUNSTATE.BurnSeat_TakeIC && !Pen[WorkPenN].ExistIC && !Auto_Flag.BurnSeat_TakeNullIC)//第一次取料失败
                                {                          
                                    BurnSeat_TakeIC_Data_Operation();
                                    ARP_Step = 15;
                                }
                                else//取料失败
                                {
                                    if(RunState == RUNSTATE.Carrier_TakeIC && Auto_Flag.ManualEnd)//终止取料
                                    {
                                        ARP_Step = 15; 
                                    }
                                    else
                                    {
                                        if(RunState == RUNSTATE.Carrier_TakeIC && TrayEndFlag.tray2Burn)
                                        {
                                            //M00905 = 1;
                                            Auto_Flag.ALarm = true;
                                            Auto_Flag.ALarmPause = true;
                                            In_Output.buzzer.M = true;
                                            string strShow;
                                            if (MultiLanguage.IsEnglish())
                                            {
                                                strShow = "Failure to fetch! Do you continue to transfer materials?\r\n\r\n[Yes]There are materials, continue to transfer materials\r\n[No]No material, stop transferring material";
                                            }
                                            else
                                            {
                                                strShow = "取料失败！是否继续转移物料？\r\n\r\n【是】：有物料，继续转移物料\r\n【否】：无物料，停止转移物料";
                                            }
                                            if (DialogResult.Yes == MessageBox.Show(strShow, "Information：", MessageBoxButtons.YesNo, MessageBoxIcon.Information)) 
                                            {
                                                Auto_Flag.ALarm = false;
                                            }
                                            else
                                            {
                                                Auto_Flag.ALarm = false;
                                                Button.endTakeIC = true;
                                            }
                                        }
                                        else
                                        {
                                            //M00911 = 1;
                                            BAR._ToolTipDlg.WriteToolTipStr("真空取料失败;[检查真空是否异常或卡料->确定->继续]");
                                            BAR.ShowToolTipWnd(true);
                                        }
                                        In_Output.vacuumO[WorkPenN].M = false;								
                                        Pen[WorkPenN].ExistIC = false;
                                        ARP_Step = 50;
                                    }
                                }						
                            }
                        }
                        else//放料
                        {
                            if(RunState == RUNSTATE.BurnSeat_LayIC || RunState == RUNSTATE.Burn_Again)
                            {
                                BurnSeat_LayIC_Data_Operation();		
                            }
                            else
                            {
                                Carrier_LayIC_Data_Operation();
                            }
                            ARP_Step = 15;
                        }
                        break;
				
                    case 14://测试模式
                        if(RunState == RUNSTATE.BurnSeat_TakeIC || RunState == RUNSTATE.Carrier_TakeIC || RunState == RUNSTATE.TakeSNGIC)//取料
                        {
                            if(Pen[WorkPenN].ExistIC)//取料成功
                            {	
                                if(RunState == RUNSTATE.BurnSeat_TakeIC)
                                {
                                    if (Config.Altimeter != 0 && Auto_Flag.Enabled_Overlay)
                                    {
                                        GetSocketXY(TIC_GroupN, TIC_UnitN);
                                        RunState = RUNSTATE.Emptying;
                                        str = "吸头[" + (WorkPenN + 1) + "]到烧写座[" + (TIC_GroupN + 1) + "_" + (TIC_UnitN + 1) + "]取料成功";
                                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
                                        ARP_Step = 2;
                                        break;
                                    }
                                    else
                                    {
                                        BurnSeat_TakeIC_Data_Operation();
                                    }
                                }
                                else if (RunState == RUNSTATE.Carrier_TakeIC)
                                {
                                    Carrier_TakeIC_Data_Operation();
                                }
                                else if (RunState == RUNSTATE.TakeSNGIC)
                                {
                                    TakeSNGIC_Data_Operation();
                                }
                            }
                            else 
                            {
                                if(Group[TIC_GroupN].Unit[TIC_UnitN].Flag_First && RunState == RUNSTATE.BurnSeat_TakeIC && !Auto_Flag.BurnSeat_TakeNullIC)//第一次取料失败
                                {                                    
                                    BurnSeat_TakeIC_Data_Operation();
                                }					
                            }
                            ARP_Step = 15;
                        }
                        else//放料
                        {
                            if (RunState == RUNSTATE.BurnSeat_LayIC || RunState == RUNSTATE.Burn_Again)
                            {
                                BurnSeat_LayIC_Data_Operation();	
                            }
                            else
                            {
                                Carrier_LayIC_Data_Operation();
                            }
                            ARP_Step = 15;
                        }
                        break;
				
                    case 15://主控逻辑
                        if (Auto_Flag.BurnMode)//烧录模式
                        {
                            if (Auto_Flag.PenAlt_Flag)
                            {
                                MasterLogic_Program_II();
                            }
                            else
                            {
                                MasterLogic_Program_I();
                            }
                        }
                        else
                        {
                            MasterLogic_Program_III();//转移模式
                        } 
                        if(RunState == RUNSTATE.End)
                        {
                            bool flag = Auto_Flag.AutoTray_TakeIC && !Auto_Flag.AutoTray_LayIC;//自动盘取料，非自动盘放
                            if ((Auto_Flag.AutoTray_LayIC || Auto_Flag.AutoTray_TakeIC) && 
                               (TrayEndFlag.takeLayIC[1] || (Auto_Flag.ProductionFinish && !TrayEndFlag.tray2Burn && !flag)))
                            {
                                AutoTray_Trigger.ReceiveTray = true;
                                AutoTray_Process.ReceiveTray = true;
                            }
					
                            if((TrayD.TIC_TrayN == 0 && Auto_Flag.FixedTray_TakeIC) || 
                               (TrayD.LIC_TrayN == 0 && Auto_Flag.FixedTray_LayIC) ||
                                Auto_Flag.ProductionFinish)
                            {
                                In_Output.buzzer.M = true;
                            }
                            if(Auto_Flag.FixedTray_TakeIC && Auto_Flag.FixedTray_LayIC)
                            {
                                //料盘1为空盘
                                if(TrayD.TIC_TrayN == 0 && ((TrayD.LIC_TrayN == 2 && !TrayEndFlag.layIC[0]) || 
                                    (TrayD.LIC_TrayN == 1 && TrayD.LIC_ColN[0] == 1 && TrayD.LIC_RowN[0] == 1)))
                                {
                                    TrayEndFlag.takeLayIC[0] = true;
                                }
                                //料盘2为空盘
                                if(TrayD.TIC_TrayN == 0 && ((TrayD.LIC_TrayN == 1 && !TrayEndFlag.layIC[1]) || 
                                    (TrayD.LIC_TrayN == 2 && TrayD.LIC_ColN[1] == 1 && TrayD.LIC_RowN[1] == 1)))
                                {
                                    TrayEndFlag.takeLayIC[1] = true;
                                }
                                //料盘1和料盘2都为空盘
                                if(TrayD.TIC_TrayN == 0 && !TrayEndFlag.layIC[0] && !TrayEndFlag.layIC[1] && 
                                    ((TrayD.LIC_TrayN == 1 && TrayD.LIC_ColN[0] == 1 && TrayD.LIC_RowN[0] == 1) || 
                                    (TrayD.LIC_TrayN  == 2 && TrayD.LIC_ColN[1] == 1 && TrayD.LIC_RowN[1] == 1)))
                                {
                                    TrayEndFlag.takeLayIC[0] = true;
                                    TrayEndFlag.takeLayIC[1] = true;
                                }
                            }
                            ARE_Step = 1;
                            ARHR_Step = 1;
                            Auto_Flag.RunHome_End = false;
                            ARP_Step  = 100;
                        }
                        else
                        {
                            ARP_Step = 2;
                        }
                        break;

                    case 16: //飞拍
                        for (int i = 0; i < UserConfig.VacuumPenC; i++)
                        {
                            if (Pen[i].Rotate.Busy)
                            {
                                if (GetSysTime() > Pen[i].Rotate.TimeOut)
                                {
                                    BAR._ToolTipDlg.WriteToolTipStr("吸笔[" + (i + 1).ToString() + "]旋转超时;[处理异常->确定->继续]");
                                    BAR.ShowToolTipWnd(true);
                                    ARP_Step = 17;
                                }
                                break;
                            }
                            else if ((i + 1) == UserConfig.VacuumPenC)
                            {
                                if (!Auto_Flag.TestMode)
                                {
                                    FlyPhotographTrigger_Handle();
                                }
                                ARP_Step = 18;
                            }
                        }
                        break;

                    case 17:
                        if (Continue_AfterAlarm())
                        {
                            ARP_Step = 16;
                        }
                        break;

                    case 18:
                        if (NextAction_Check())
                        {
                            temp = Auto_Flag.Ascending_ICPos ? 1 : -1;
                            Position_X.Buffer = Pen[WorkPenN].LowCamera_X + temp * 10.0d;
                            backStep = 19;
                            ARP_Step = 200;
                        }
                        break;

                    case 19:
                        Horizontal_X_Position_Control(Position_X.Buffer, ExistICPenC);
                        if (Auto_Flag.Run_InPlace)
                        {
                            Auto_Flag.Run_InPlace = false;
                            if (!Auto_Flag.TestMode)
                            {
                                Gts.GT_2DCompareStatus(PLC1.card[0].cardNum, 0, out short pStatus, out int pCount, out short pFifo, out short pFifoCount, out short pBufCount);
                                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "飞拍位置次数[" + pCount + "]", "Flow");
                                Gts.GT_2DCompareClearData(PLC1.card[0].cardNum, 0); //清除位置比较输出数据
                            }
                            ARP_Step = 15;
                        }
                        break;

                    case 20: //获取纠偏修正值
                        if (!Pen[PIC_PenN].Rotate.Busy && !Auto_Flag.ALarmPause)//轴c定位完成
                        {
                            if (!Auto_Flag.TestMode)
                            {
                                double retX = 0, retY = 0, retR = 0;
                                bool ret;
                                ret = UserConfig.IsProgrammer ? true : g_act.CCDProccess(ref retX, ref retY, ref retR, trapPrm_C[PIC_PenN].getPos);
                                if (ret)
                                {
                                    rectify[PIC_PenN].AxisX = -retX;
                                    rectify[PIC_PenN].AxisY = retY;
                                    RotateTrigge(false, PIC_PenN, retR);
                                    str = "吸头[" + (PIC_PenN + 1) + "]纠偏成功[X：" + (-retX).ToString("f2") + "mm，Y：" + retY.ToString("f2") + "mm，C：" + retR.ToString("f2") + "°]";
                                    g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
                                    PIC_PenN += Auto_Flag.Ascending_ICPos ? 1 : -1;
                                    ARP_Step = 15;
                                }
                                else
                                {
                                    //第一次拍照不成功，旋转一个小角度
                                    RotateTrigge(false, PIC_PenN, -10d);
                                    str = "吸头[" + (PIC_PenN + 1) + "]第一次拍照不成功,旋转一个小角度[C：" + 5 + "°]";
                                    g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
                                    ARP_Step = 21;
                                }                               
                                stopWatch.Stop();
                                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINT, "动作处理时长[" + stopWatch.ElapsedMilliseconds.ToString() + "ms]", "Flow");
                            }
                            else
                            {
                                PIC_PenN += Auto_Flag.Ascending_ICPos ? 1 : -1;
                                ARP_Step = 15;
                            }
                        }
                        else if (GetSysTime() > Pen[PIC_PenN].Rotate.TimeOut)
                        {
                            if (!Auto_Flag.ALarmPause)
                            {
                                BAR._ToolTipDlg.WriteToolTipStr("吸笔[" + (PIC_PenN + 1).ToString() + "]旋转超时;[处理异常->确定->继续]");
                                BAR.ShowToolTipWnd(true);
                            }
                            else
                            {
                                Continue_AfterAlarm();
                            }
                        }
                        break;

                    case 21: //获取纠偏修正值
                        if (!Pen[PIC_PenN].Rotate.Busy && !Auto_Flag.ALarmPause)//轴c定位完成
                        {
                            double retX = 0, retY = 0, retR = 0;
                            bool ret;
                            DateTime dt1 = DateTime.Now;
                            ret = g_act.CCDProccess(ref retX, ref retY, ref retR, trapPrm_C[PIC_PenN].getPos + 10d);
                            double usetime = (DateTime.Now - dt1).TotalMilliseconds;
                            //g_act.GenLogMessage(GlobConstData.ST_LOG_PRINT, "CCD处理时长：" + usetime.ToString(), "Flow");
                            if (ret)
                            {
                                rectify[PIC_PenN].AxisX = -retX;
                                rectify[PIC_PenN].AxisY = retY;
                                RotateTrigge(false, PIC_PenN, retR);
                                str = "吸头[" + (PIC_PenN + 1) + "]纠偏成功[X：" + (-retX).ToString("f2") + "mm，Y：" + retY.ToString("f2") + "mm，C：" + retR.ToString("f2") + "°]";
                                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
                                PIC_PenN += Auto_Flag.Ascending_ICPos ? 1 : -1;
                                ARP_Step = 15;
                            }
                            else
                            {
                                BAR._ToolTipDlg.WriteToolTipStr("吸笔[" + (PIC_PenN + 1).ToString() + "]纠偏失败;[处理异常->确定->继续]");
                                BAR.ShowToolTipWnd(true);
                                ARP_Step = 22;
                            }
                        }
                        else if (GetSysTime() > Pen[PIC_PenN].Rotate.TimeOut)
                        {
                            if (!Auto_Flag.ALarmPause)
                            {
                                BAR._ToolTipDlg.WriteToolTipStr("吸笔[" + (PIC_PenN + 1).ToString() + "]旋转超时;[处理异常->确定->继续]");
                                BAR.ShowToolTipWnd(true);
                            }
                            else
                            {
                                Continue_AfterAlarm();
                            }
                        }
                        break;

                    case 22: //获取纠偏修正值
                        if (Continue_AfterAlarm())
                        {
                            ARP_Step = 21;
                        }
                        break;

                    case 23: //允许下一步动作
                        if (NextAction_Check())
                        {
                            ScanTrigger_Handle(Scan_GroupN);//扫码触发处理
                            ARP_Step = 24;
                        }
                        break;

                    case 24: //获取扫码结果
                        if (!Group[Scan_GroupN].Scan.Busy)
                        {
                            if (Auto_Flag.TestMode)
                            {
                                Group[Scan_GroupN].Unit[Scan_UnitN].DownResult = 1;
                            }
                            ScanCode_Data_Operation();
                            ARP_Step = 15;
                        }
                        break;

                    case 25: //允许下一步动作
                        if (NextAction_Check())
                        {
                            if (!Auto_Flag.TestMode)
                            {
                                ARP_Step = 26;
                            }
                            else
                            {
                                ARP_Step = 27;
                            }
                        }
                        break;

                    case 26:
                        g_act.WaitDoEvent(100);
                        Cathetometer.MODBUS_ReadAI();//测高触发处理
                        ARP_Step = 27;
                        break;

                    case 27: //获取测高结果
                        if (!Altimeter.ReadAI_Busy)
                        {
                            int group = Auto_Flag.BurnSeat_Sync ? SyncPen[SyncPenN].GroupN : TIC_GroupN;
                            int unit = Auto_Flag.BurnSeat_Sync ? SyncPen[SyncPenN].UnitN : TIC_UnitN;
                            if (Auto_Flag.TestMode)
                            {
                                if (Auto_Flag.BurnSeat_Sync)
                                {
                                    EmptyingResult = SyncPen[SyncPenN].Flag_First ? 1 : 0;
                                }
                                else
                                {
                                    EmptyingResult = Pen[TIC_PenN].ExistIC ? 0 : 1;
                                }
                            }
                            else
                            {
                                if (Altimeter.ReadAI_Online)
                                {
                                    if (Cathetometer.Value >= Group[group].Unit[unit].HeightVal)//比无IC的高度值还大，为空料
                                    {
                                        EmptyingResult = 0;
                                    }
                                    else if (Cathetometer.Value <= Group[group].Unit[unit].HeightVal - Altimeter.Thickness)//比有IC的高度值还小，为有料
                                    {
                                        EmptyingResult = 1;
                                    }
                                    else//介于两个阀值之间，判断更接近哪个
                                    {
                                        if (Group[group].Unit[unit].HeightVal - Cathetometer.Value >= Altimeter.Thickness / 2.0d)
                                        {
                                            EmptyingResult = 1;
                                        }
                                        else
                                        {
                                            EmptyingResult = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    string strAlarm = "读取测高仪模拟量失败;[检查通讯->确定->继续]";
                                    BAR._ToolTipDlg.WriteToolTipStr(strAlarm);
                                    BAR.ShowToolTipWnd(true);
                                    ARP_Step = 28;
                                    break;
                                }
                            }
                            if (!Auto_Flag.BurnSeat_Sync)
                            {
                                RunState = RUNSTATE.BurnSeat_TakeIC;
                            }
                            str = "测高仪到烧写座[" + (group + 1) + "_" + (unit + 1) + "]排空";
                            if (EmptyingResult == 1)//排空有料
                            {
                                str += "有料";
                                if (Auto_Flag.BurnSeat_Sync)
                                {
                                    if (SyncPen[SyncPenN].Flag_First)
                                    {
                                        SyncPen[SyncPenN].Flag_TakeIC = true;
                                        SyncPen_Struct.State_TakeIC = true;
                                        ARP_Step = 15;
                                    }
                                    else
                                    {
                                        //把已取到IC的结果OK改为NG
                                        if (Pen[SyncPen[SyncPenN].PenN].DownResult == 1)
                                        {
                                            Pen[SyncPen[SyncPenN].PenN].DownResult = 2;
                                            OKPenC--;
                                            NGPenC++;
                                            if (Auto_Flag.Ending && !Auto_Flag.ForceEnd)
                                            {
                                                Auto_Flag.Ending = false;
                                            }
                                            if (Auto_Flag.Production && Auto_Flag.ProductionOK)
                                            {
                                                TIC_ValidC--;
                                            }
                                            RotateTrigge(true, SyncPen[SyncPenN].PenN, RotateAngle.LIC_Tray[2]);
                                        }
                                        str += "请人工拿走物料,测量值[" + String.Format("{0:f4}", Cathetometer.Value) + "]";
                                        BAR._ToolTipDlg.WriteToolTipStr(str);
                                        BAR.ShowToolTipWnd(true);
                                        ARP_Step = 28;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (Pen[TIC_PenN].ExistIC)
                                    {
                                        ExistICPenC++;
                                        NGPenC++;
                                        Pen[TIC_PenN].DownResult = 3;
                                        RotateTrigge(true, TIC_PenN, RotateAngle.LIC_Tray[2]);
                                        TIC_PenN++;
                                        if (ExistICPenC < EnalePenC)
                                        {
                                            PenEnable_Check();
                                        }
                                    }
                                    if (ExistICPenC == EnalePenC)
                                    {
                                        Auto_Flag.BurnSeat_TakeNullIC = false;
                                        ARP_Step = 15;
                                    }
                                    else
                                    {
                                        Auto_Flag.BurnSeat_TakeNullIC = true;
                                        GetSocketXY(TIC_GroupN, TIC_UnitN, TIC_PenN);
                                        ARP_Step = 2;
                                    }
                                }
                            }
                            else
                            {
                                str += "无料";
                                if (!Auto_Flag.BurnSeat_Sync)
                                {
                                    BurnSeat_TakeIC_Data_Operation(false);
                                }
                                ARP_Step = 15;
                            }
                            str += ",测量值[" + String.Format("{0:f4}", Cathetometer.Value) + "]";
                            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
                        }
                        break;

                    case 28:
                        if (Continue_AfterAlarm())
                        {
                            Cathetometer.MODBUS_ReadAI();//测高触发处理
                            ARP_Step = 27;
                        }
                        break;

                    case 30:                                       
                        if(!Brede_Flag.Auto_Brede && !Brede_Trigger.Auto_Brede)//等待自动编带停止
                        {
                            ARP_Step = 31;						
                        }                      
                        break;

                    case 31:
                        Brede.Alarm_Check();//编带警报查询
                        if (Auto_Flag.ALarm)
                        {
                            ARP_Step = 32;
                        }
                        else
                        {
                            Brede.Send_Cmd(Brede.Cmd_ClearAlarm);	//清除报警命令
                            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送编带清除报警命令", "Flow");
                            ARP_Step = 4;
                        }
                        break;

                    case 32:
                        if (Continue_AfterAlarm())
                        {
                            ARP_Step = 31;
                        }
                        break;

                    case 50:
                        if(Button.endTakeIC)
                        {
                            Button.endTakeIC = false;
                            TrayD.TIC_ColN[1] = 0;
                            TrayD.TIC_RowN[1] = 0;				
                            TrayEndFlag.takeIC[1] = true;
                            TrayEndFlag.takeLayIC[1] = true;
                            Auto_Flag.ALarmPause = false;
                            ARP_Step = 15;
                        }
                        else
                        {
                            if (Continue_AfterAlarm())
                            {
                                if (RunState == RUNSTATE.Carrier_TakeIC && Auto_Flag.ManualEnd) //终止取料
                                {
                                    ARP_Step = 15;
                                }
                                else if (RunState == RUNSTATE.Carrier_TakeIC || RunState == RUNSTATE.BurnSeat_TakeIC)
                                {
                                    if (Auto_Flag.JumpMainStep_Flag)//跳过取料
                                    {
                                        Auto_Flag.JumpMainStep_Flag = false;
                                        TakeIC_Failure_Data_Operation();
                                        ARP_Step = 15;
                                    }
                                    else
                                    {
                                        ARP_Step = 70;
                                    }
                                }
                                else
                                {
                                    LayIC_Failure_Data_Operation();
                                    ARP_Step = 15;
                                }
                            }
                        }
                        break;	
				
                    case 60:
                        if (RunState == RUNSTATE.BurnSeat_TakeIC)//连续NG报警处理
                        {
                            if (Auto_Flag.BurnSeat_Sync)
                            {
                                for (int i = 0; i < Sync_UnitC; i++)
                                {
                                    if (Group[SyncPen[i].GroupN].Continue_NG)
                                    {
                                        Group[SyncPen[i].GroupN].Continue_NG = false;
                                        for (int j = 0; j < UserConfig.ScketUnitC; j++)
                                        {
                                            if (Group[SyncPen[i].GroupN].Unit[j].Counter_NG >= NGContinueC)
                                            {
                                                Group[SyncPen[i].GroupN].Unit[j].Counter_NG = 0;
                                                string strAlarm = "烧写座[" + (SyncPen[i].GroupN + 1) + "_" + (j + 1) + "]异常;[检测烧写座->确定->继续]";
                                                BAR._ToolTipDlg.WriteToolTipStr(strAlarm);
                                            }
                                        }
                                    }
                                }
                                //M00909 = 1; 
                                BAR.ShowToolTipWnd(true);
                            }
                            else
                            {
                                if (Group[TIC_GroupN].Continue_NG)
                                {
                                    Group[TIC_GroupN].Continue_NG = false;
                                    for (int i = 0; i < UserConfig.ScketUnitC; i++)
                                    {
                                        if (Group[TIC_GroupN].Unit[i].Counter_NG >= NGContinueC)
                                        {
                                            Group[TIC_GroupN].Unit[i].Counter_NG = 0;
                                            string strAlarm = "烧写座[" + (TIC_GroupN + 1) + "_" + (i + 1) + "]异常;[检测烧写座->确定->继续]";
                                            BAR._ToolTipDlg.WriteToolTipStr(strAlarm);
                                        }
                                    }
                                    //M00909 = 1; 
                                    BAR.ShowToolTipWnd(true);
                                }
                            }
                        }
                        else if (Group[Scan_GroupN].Continue_NG && RunState == RUNSTATE.TakeSNGIC)//连续NG报警处理
                        {
                            Group[Scan_GroupN].Continue_NG = false;
                            for (int i = 0; i < UserConfig.ScketUnitC; i++)
                            {
                                if (Group[Scan_GroupN].Unit[i].Counter_NG >= NGContinueC)
                                {
                                    Group[Scan_GroupN].Unit[i].Counter_NG = 0;
                                    string strAlarm = "烧写座[" + (Scan_GroupN + 1) + "_" + (i + 1) + "]异常;[检测烧写座->确定->继续]";
                                    BAR._ToolTipDlg.WriteToolTipStr(strAlarm);
                                }
                            }
                            //M00909 = 1; 
                            BAR.ShowToolTipWnd(true);
                        }
                        ARP_Step = 200;
                        backStep = 61;
                        break;

                    case 61:
                        Horizontal_Position_Control(0, 0);
                        if (Auto_Flag.Run_InPlace)//XY轴回原位
                        {
                            Auto_Flag.Run_InPlace = false;
                            ARP_Step = 62;
                        }
                        break;

                    case 62:
                        if (Continue_AfterAlarm())
                        {
                            if(RunState == RUNSTATE.BurnSeat_TakeIC || RunState == RUNSTATE.TakeSNGIC)
                            {     
                                ARP_Step = 4;//运行至水平方向位置
                            }
                            else if(RunState == RUNSTATE.Carrier_LayIC)
                            {		
                                if(TrayD.TIC_ColN[0] == 1 && TrayD.TIC_RowN[0] == 1)
                                {
                                    //M00904 = 1;
                                    BAR._ToolTipDlg.WriteToolTipStr("补料盘满料;[请更换一个空料的盘->确定->继续]");
                                    BAR.ShowToolTipWnd(true);
                                }
                                else
                                {
                                    ARP_Step = 3;
                                }
                            }
                            else if(RunState == RUNSTATE.Carrier_LayNGIC || RunState == RUNSTATE.Carrier_LayDNGIC)
                            {
                                if(TrayEndFlag.layIC[2])
                                {
                                    //M00912 = 1;
                                    BAR._ToolTipDlg.WriteToolTipStr("NG盘满料;[更换新的NG盘->确定->继续]");
                                    BAR.ShowToolTipWnd(true);
                                }
                                else
                                {
                                    ARP_Step = 3;
                                }
                            }
                            else if(RunState == RUNSTATE.Carrier_TakeIC)
                            {
                                if(TrayEndFlag.takeLayIC[0] && Auto_Flag.AutoTray_LayIC && Auto_Flag.AutoTray_TakeIC)
                                {
                                    //M00913 = 1;
                                    BAR._ToolTipDlg.WriteToolTipStr("补料盘缺料;[请更换一个有料的盘->确定->继续]");
                                    BAR.ShowToolTipWnd(true);
                                }
                                else
                                {
                                    if (Auto_Flag.TubeTimeOut)
                                    {
                                        Auto_Flag.TubeTimeOut = false;
                                        Auto_Flag.Tube_AllEmptying = false;
                                    }
                                    ARP_Step = 3;
                                }
                            }					
                        }	
                        break;

                    case 63:
                        if (RunState == RUNSTATE.BurnSeat_TakeIC)//Error报警处理
                        {
                            if (Auto_Flag.BurnSeat_Sync)
                            {
                                for (int i = 0; i < Sync_UnitC; i++)
                                {
                                    if (Group[SyncPen[i].GroupN].Flag_Error)
                                    {
                                        Group[SyncPen[i].GroupN].Flag_Error = false;
                                        for (int j = 0; j < UserConfig.ScketUnitC; j++)
                                        {
                                            if (Group[SyncPen[i].GroupN].Unit[j].Flag_Error)
                                            {
                                                Group[SyncPen[i].GroupN].Unit[j].Flag_Error = false;
                                                string strAlarm = "烧写座[" + (SyncPen[i].GroupN + 1) + "_" + (j + 1) + "]Error;[检测烧写座->确定->继续]";
                                                BAR._ToolTipDlg.WriteToolTipStr(strAlarm);
                                            }
                                        }
                                    }
                                }
                                BAR.ShowToolTipWnd(false);
                            }
                            else
                            {
                                if (Group[TIC_GroupN].Flag_Error)
                                {
                                    Group[TIC_GroupN].Flag_Error = false;
                                    for (int i = 0; i < UserConfig.ScketUnitC; i++)
                                    {
                                        if (Group[TIC_GroupN].Unit[i].Flag_Error)
                                        {
                                            Group[TIC_GroupN].Unit[i].Flag_Error = false;
                                            string strAlarm = "烧写座[" + (TIC_GroupN + 1) + "_" + (i + 1) + "]Error;[检测烧写座->确定->继续]";
                                            BAR._ToolTipDlg.WriteToolTipStr(strAlarm);
                                        }
                                    }
                                    BAR.ShowToolTipWnd(false);
                                }
                            }
                        }
                        ARE_Step = 1;
                        ARHR_Step = 1;
                        Auto_Flag.RunHome_End = false;
                        ARP_Step = 200;
                        backStep = 100;
                        break;

                    case 80: //获取3D检测结果
                        if (!Auto_Flag.TestMode)
                        {
                            BAR.tcpClient_3D.Send("A");
                            DateTime StTime = DateTime.Now;
                            do
                            {
                                Thread.Sleep(1);
                            } while (BAR.tcpClient_3D.ReciveDate == null && (DateTime.Now - StTime).TotalMilliseconds > 300);

                            if (BAR.tcpClient_3D.ReciveDate != null)//3D检测完成
                            {
                                if (BAR.tcpClient_3D.ReciveDate == "O")//检测结果
                                {
                                    Pen[DIC_PenN].DetectionResult = 1;//OK
                                }
                                else
                                {
                                    if (Auto_Flag.Cam_3D_Mode_II)//烧录后料3D检测
                                    {
                                        Pen[DIC_PenN].DownResult = 2;
                                    }
                                    Pen[DIC_PenN].DetectionResult = 2;//NG
                                }
                                Pen[DIC_PenN].Detection_Num++;
                                ARP_Step = 11;
                            }
                        }
                        else
                        {
                            Pen[DIC_PenN].DetectionResult = 1;
                            Pen[DIC_PenN].Detection_Num++;
                            ARP_Step = 11;
                        }
                        break;

                    case 81: //获取3D检测结果回到安全高度后下一步动作判断	
                        if (Pen[DIC_PenN].DetectionResult == 2)//3D检测NG
                        {
                            if (Auto_Flag.Cam_3D_Mode_II)//烧录后料3D检测
                            {
                                OKPenC--;
                            }
                            else
                            {
                                IC_SupplyC++;
                            }
                            if (Pen[DIC_PenN].ExistRawIC)
                            {
                                Pen[DIC_PenN].ExistRawIC = false;
                                ExistRawICPenC--;
                            }
                            NGPenC++;
                            RotateTrigge(true, DIC_PenN, RotateAngle.LIC_Tray[2]);
                            if (Auto_Flag.Ending && !Auto_Flag.ForceEnd)
                            {
                                Auto_Flag.Ending = false;
                            }
                            if (Auto_Flag.Production && Auto_Flag.ProductionOK)
                            {
                                TIC_ValidC--;
                            }

                            Pen[DIC_PenN].Detection_Num = 0;
                            DIC_PenN++;
                            ARP_Step = 15;//主控逻辑
                        }
                        else if (Pen[DIC_PenN].DetectionResult == 1)//3D检测OK
                        {
                            if (DetectionType == 1)//BGA
                            {
                                if (Auto_Flag.Cam_3D_Mode_II)//烧录后料3D检测
                                {
                                    RotateTrigge(true, DIC_PenN, Get_LIC_RotateAngle());
                                }
                                else
                                {
                                    DIC_OKPenC++;
                                }
                                Pen[DIC_PenN].Detection_Num = 0;
                                DIC_PenN++;
                                ARP_Step = 15;
                            }
                            else if (DetectionType == 0 || DetectionType == 2)//QFP,SOP
                            {
                                if (Pen[DIC_PenN].Detection_Num == 2)//3D检测第二次OK
                                {
                                    if (Auto_Flag.Cam_3D_Mode_II)//烧录后料3D检测
                                    {
                                        RotateTrigge(true, DIC_PenN, Get_LIC_RotateAngle());
                                    }
                                    else
                                    {
                                        DIC_OKPenC++;
                                        RotateTrigge(false, DIC_PenN, -180d);
                                    }
                                    Pen[DIC_PenN].Detection_Num = 0;
                                    DIC_PenN++;
                                    ARP_Step = 15;
                                }
                                else if (Pen[DIC_PenN].Detection_Num == 1)//3D检测第一次OK
                                {
                                    RotateTrigge(false, DIC_PenN, 180d);
                                    ARP_Step = 6;
                                }
                            }
                        }
                        break;

                    case 90:
                        Horizontal_Position_Control(Position_X.Buffer, Position_Y.Buffer);		
                        if(Auto_Flag.Run_InPlace)
                        {
                            Auto_Flag.Run_InPlace = false;
                            Auto_Flag.AutoRunBusy = false;
                            ARP_Step = 0;
                            //M00903 = 1;
                            g_act.AutoRunShutoff();
                            BAR._ToolTipDlg.WriteToolTipStr("运动超出有效行程,已退运行;请检查坐标或X,Y轴最大值设置");
                            BAR.ShowToolTipWnd(true);
                        }
                        break;	
				
                    case 100:
                        Auto_Run_Home_Return();
                        Auto_Run_End();
                        if (Auto_Flag.PenAlt_Flag)
                        {
                            DownSeat_Init();
                        }
                        break;

                    case 200: //Z轴安全检测                       
                        AutoTimer.OriginZ_Check = GetSysTime() + 3000;
                        ARP_Step = 201;
                        break;

                    case 201: //Z轴安全检测
                        if (OriginZ_Check() && PenHome_Check())
                        {
                            ARP_Step = backStep;
                        }
                        else if (GetSysTime() > AutoTimer.OriginZ_Check)
                        {
                            BAR._ToolTipDlg.WriteToolTipStr("Z轴未在安全检测高度");
                            BAR.ShowToolTipWnd(true);
                            ARP_Step = 202;
                        }
                        break;

                    case 202:
                        if (Continue_AfterAlarm())
                        {
                            ARP_Step = 201;
                        }
                        break;

                    default:
                        break;
                }	
            }
            RunStopWatch.Stop();
            PauseStopWatch.Stop();
            Efficiency.stopWatch.Reset();
            UserTask.EndTime= DateTime.Now;
            TimeSpan ts = RunStopWatch.Elapsed;
            str = "自动运行时间[" + String.Format("{0:0}day {1:00}:{2:00}:{3:00}", ts.Days, ts.Hours, ts.Minutes, ts.Seconds) + "]";
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
            ts = PauseStopWatch.Elapsed;
            str = "暂停时间[" + String.Format("{0:0}day {1:00}:{2:00}:{3:00}", ts.Days, ts.Hours, ts.Minutes, ts.Seconds) + "]";
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
        }
    }
}
