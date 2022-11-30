using BAR.Commonlib;
using BAR.Commonlib.Utils;
using BAR.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAR
{
    //图像示教自动步骤
    public class TeachAction : UserTask
    {
        public delegate void UpdateHeight(int ind, double value);
        public static event UpdateHeight updateHeight;

        public delegate void UpdateSeatPos(int ind);
        public static event UpdateSeatPos updateSeatPos;
        int indPos = 0, indModel = 0, indTray = 0;
        double PosPul = 0;
        /// <summary>
        /// GO位置步骤
        /// </summary>
        private static int GOPos_Step = 0;    //GO位置步骤
        /// <summary>
        /// 反学步骤
        /// </summary>
        private static int Learn_Step = 0;    //反学步骤
        /// <summary>
        /// 检测步骤
        /// </summary>
        private static int Detection_Step = 0;//检测步骤
        /// <summary>
        /// 高度检测步骤
        /// </summary>
        private static int Height_Step = 0;   //高度检测步骤
        /// <summary>
        /// 校正座子坐标步骤
        /// </summary>
        private static int ReviseSeat_Step = 0;
        /// <summary>
        /// 校正所有坐标步骤
        /// </summary>
        private static int RevisePos_Step = 0;
        /// <summary>
        /// 备份步骤
        /// </summary>
        private static int BackStep = 0;      //备份步骤
        /// <summary>
        /// CMK测量步骤
        /// </summary>
        private static int CMK_Step;
        /// <summary>
        /// 旋转次数
        /// </summary>
        private static int RotateNum = 0;     //旋转次数
        /// <summary>
        /// 反学组
        /// </summary>
        private static int Learn_Group = 0;   //反学组
        /// <summary>
        /// 反学单元
        /// </summary>
        private static int Learn_Unit = 0;    //反学单元
        /// <summary>
        /// 反学吸笔
        /// </summary>
        private static int Learn_Pen = 0;     //反学吸笔
        /// <summary>
        /// 预备步骤
        /// </summary>
        private static int Prepare_Step = 0;  //预备步骤
        /// <summary>
        /// 计时器
        /// </summary>
        private static UInt64 timer = 0;      //计时器
        /// <summary>
        /// CMK测量轴号
        /// </summary>
        private static int CMK_Axis = 0;
        /// <summary>
        /// CMK采样次数
        /// </summary>
        private static int CMK_Count = 10;

        public static double dy, dx, dr;
        public delegate bool ReviseSeatDelegate(ref double dy, ref double dx);
        /// <summary>
        /// 自动获取烧录座坐标
        /// </summary>
        public static ReviseSeatDelegate ReviseSeat;
        public delegate bool RevisePOSDelegate(int Model, ref double dy, ref double dx);
        /// <summary>
        /// 自动校准所有坐标
        /// </summary>
        public static RevisePOSDelegate RevisePOS;
        /// <summary>
        /// 匹配完成标志
        /// </summary>
        private bool tempFlag;
        /// <summary>
        /// 单个烧录座校准次数
        /// </summary>
        private int ReviseConut;

        

        private bool ReviseNGTray;

        private void Prepare()
        {
            int temp;
            switch (Prepare_Step)
            {
                case 1:
                    if (Axis_Z_IsHome_Check())
                    {
                        Prepare_Step = 3;
                    }
                    else
                    {
                        Prepare_Step = 2;
                    }
                    if (PenType == 1)
                    {
                        for (int i = 0; i < UserConfig.VacuumPenC; i++)
                        {
                            In_Output.penO[i].M = false;
                        }
                    }
                    break;

                case 2:
                    temp = Axis_Z_Home_Init();
                    if (temp == 2)
                    {
                        Prepare_Step = 3;
                    }
                    else if (temp == 1 || temp == 3)
                    {
                        Auto_Flag.PrepareBusy = false;
                        Prepare_Step = 100;
                    }
                    break;

                case 3: //Z轴至安全高度				
                    if (HeightSafe_Handle())
                    {
                        AutoTimer.OriginZ_Check = GetSysTime() + 3000;
                        Prepare_Step = 5;                      
                    }
                    break;

                case 5:
                    if (OriginZ_Check() && PenHome_Check())
                    {
                        if (axisSts_X.isHome && axisSts_Y.isHome)
                        {
                            Prepare_Step = 7;
                        }
                        else
                        {
                            ORG_Function(homePrm_X, axisSts_X);
                            ORG_Function(homePrm_Y, axisSts_Y);
                            Prepare_Step = 6;
                        }
                    }
                    else if (GetSysTime() > AutoTimer.OriginZ_Check)
                    {
                        BAR._ToolTipDlg.WriteToolTipStr("Z轴未在安全检测高度");
                        BAR.ShowToolTipWnd(false);
                        Auto_Flag.PrepareBusy = false;
                        Prepare_Step = 100;
                    }
                    break;

                case 6:
                    if (axisSts_X.isHome && axisSts_Y.isHome)
                    {
                        Prepare_Step = 7;
                        if (Config.CCDModel == 1)//飞拍
                        {
                            Init_2DCompareMode();
                        }
                    }
                    break;

                case 7:
                    Auto_Flag.PrepareBusy = false;
                    Prepare_Step = 0;
                    break;

                default:
                    break;
            }
        }

        public static void GO_Start(Double setPos_X, Double setPos_Y)
        {
            if (!Auto_Flag.SystemBusy)
            {
                Position_X.Buffer = setPos_X;
                Position_Y.Buffer = setPos_Y;
                Learn_Pen = -1;
                Auto_Flag.GOBusy = true;
                GOPos_Step = 1;
            }
        }

        public static void GO_Start(Double setPos_X, Double setPos_Y, Double setPos_Z, int penN)
        {
            if (!Auto_Flag.SystemBusy)
            {
                Position_X.Buffer = setPos_X;
                Position_Y.Buffer = setPos_Y;
                HeightVal.Buffer = setPos_Z;
                Learn_Pen = penN;
                Auto_Flag.GOBusy = true;
                GOPos_Step = 1;
            }
        }

        public void GOPosition_Handle()
        {
            if(!Auto_Flag.GOBusy)
            {
                return;
            }
            switch (GOPos_Step)
            {         
                case 1:
                    Prepare_Step = 1;
                    Auto_Flag.PrepareBusy = true;
                    GOPos_Step = 2;
                    break;

                case 2:
                    Prepare();
                    if (!Auto_Flag.PrepareBusy)
                    {
                        if (Prepare_Step == 100)
                        {
                            GOPos_Step = 0;
                            Auto_Flag.GOBusy = false;
                        }
                        else if (Prepare_Step == 0)
                        {
                            GOPos_Step = 3;
                            AutoTimer.OriginZ_Check = GetSysTime() + 3000;
                        }
                    }
                    break;

                case 3: //Z轴安全检测
                    if (OriginZ_Check() && PenHome_Check())
                    {
                        GOPos_Step = 5;
                    }
                    else if (GetSysTime() > AutoTimer.OriginZ_Check)
                    {
                        BAR._ToolTipDlg.WriteToolTipStr("Z轴未在安全检测高度");
                        BAR.ShowToolTipWnd(true);
                        GOPos_Step = 4;
                    }
                    break;

                case 4:
                    if (Continue_AfterAlarm())
                    {
                        GOPos_Step = 3;
                    }
                    break;

                case 5:
                    Horizontal_Position_Control(Position_X.Buffer, Position_Y.Buffer, 500);
                    if (Auto_Flag.Run_InPlace)
                    {
                        Auto_Flag.Run_InPlace = false;
                        if (Learn_Pen == -1)
                        {
                            GOPos_Step = 9;
                        }
                        else
                        {
                            GOPos_Step = 6;
                        }
                    }
                    break;

                case 6:
                    Vertical_Position_Control(Learn_Pen, HeightVal.Buffer, homePrm_Z[Learn_Pen].velHigh, true);
                    if (Auto_Flag.Run_InPlace)
                    {
                        Auto_Flag.Run_InPlace = false;
                        GOPos_Step = PenType == 1 ? 7 : 9;
                    }
                    break;

                case 7: //检测吸笔下降到位			
                    if (In_Output.penLimitI[Learn_Pen].M)
                    {
                        GOPos_Step = 9;
                    }
                    else if (GetSysTime() > AutoTimer.OriginZ_Check)
                    {
                        BAR._ToolTipDlg.WriteToolTipStr("吸笔[" + Learn_Pen + "]下降到位检测超时");
                        BAR.ShowToolTipWnd(true);
                        GOPos_Step = 8;
                    }
                    break;

                case 8:
                    if (Continue_AfterAlarm())
                    {
                        GOPos_Step = 7;
                        AutoTimer.OriginZ_Check = GetSysTime() + 3000;
                    }
                    break;

                case 9:
                    GOPos_Step = 0;
                    Auto_Flag.GOBusy = false;
                    break;

                default:
                    break;
            }
        }


        public static void Learn_Start(int groupN, int unitN, int penN)
        {
            if (!Auto_Flag.SystemBusy)
            {
                RunState = RUNSTATE.Carrier_TakeIC;
                TIC_GroupN = LIC_GroupN = Learn_Group = groupN;
                TIC_UnitN = LIC_UnitN = Learn_Unit = unitN;
                Learn_Pen = penN;
                for (int i = 0; i < UserConfig.AllMotionC; i++)
                {
                    In_Output.pushSeatO[i].M = false;

                    if (ProgrammerType == GlobConstData.Programmer_WG || ProgrammerType == GlobConstData.Programmer_YED ||
                        ProgrammerType == GlobConstData.Programmer_RD || ProgrammerType == GlobConstData.Programmer_WG_TY)
                    {
                        In_Output.flipO[i].M = false;
                    }    
                }
                for (int i = 0; i < UserConfig.VacuumPenC; i++)
                {
                    In_Output.blowO[i].M = false;
                    In_Output.vacuumO[i].M = false;
                }
                Auto_Flag.LearnBusy = true;
                Button.replaceIC = false;
                Learn_Step = 1;
                Detection_Step = 0;
                Height_Step = 0;
            }
        }
        /// <summary>
        /// 定位反学
        /// </summary>
        public void Learn_Handle()
        {
            int num;
            if (!Auto_Flag.LearnBusy)
            {
                return;
            }
            if (Learn_Step == 0)
            {
                return;
            }
            switch (Learn_Step)
            {
                case 1:
                    Prepare_Step = 1;
                    Auto_Flag.PrepareBusy = true;
                    Learn_Step = 2;
                    break;

                case 2:
                    Prepare();
                    if (!Auto_Flag.PrepareBusy)
                    {
                        if (Prepare_Step == 100)
                        {
                            Learn_Step = 0;
                            Auto_Flag.LearnBusy = false;
                        }
                        else if (Prepare_Step == 0)
                        {
                            Learn_Step = 3;
                        }
                    }      
                    break;

                case 3:
                    if (Auto_Flag.AutoTray_TakeIC || Auto_Flag.FixedTray_TakeIC)
                    {
                        Position_X.Buffer = Position_X.trayFirst[0];
                        Position_Y.Buffer = Position_Y.trayFirst[0];
                    }
                    else if (Auto_Flag.Brede_TakeIC)
                    {
                        int temp = 0;
                        if (Config.FeederCount == 1)//双飞达
                        {
                            if (!Run.FeederEnable[0].Checked)
                            {
                                temp = 1;
                            }
                        }
                        Position_X.Buffer = Position_X.Feeder[temp];
                        Position_Y.Buffer = Position_Y.Feeder[temp];
                    }
                    else if (Auto_Flag.FixedTube_TakeIC)
                    {
                        Position_X.Buffer = Position_X.tubeIn;
                        Position_Y.Buffer = Position_Y.tubeIn;
                    }

                    Position_X.Buffer = Position_X.Buffer - Pen[Learn_Pen].Offset_TopCamera_X;
                    Position_Y.Buffer = Position_Y.Buffer - Pen[Learn_Pen].Offset_TopCamera_Y;

                    RotateTrigge(true, Learn_Pen, Get_TIC_RotateAngle());
                    BackStep = 4;
                    Learn_Step = 100;
                    break;

                case 4:                 
                    Horizontal_Position_Control(Position_X.Buffer, Position_Y.Buffer, homePrm_X.velHigh);
                    if (Auto_Flag.Run_InPlace)
                    {
                        Auto_Flag.Run_InPlace = false;
                        Learn_Step = 10;
                    }
                    break;

                case 10:
                    if (!Pen[Learn_Pen].Rotate.Busy && !Auto_Flag.ALarmPause)//轴c定位完成
                    {
                        Get_Vertical_Position();
                        if (Config.Altimeter != 0)
                        {
                            HeightVal.Buffer -= Pen[Learn_Pen].Altimeter_Z;
                        }
                        Learn_Step = 11;
                    }
                    else if (GetSysTime() > Pen[Learn_Pen].Rotate.TimeOut)
                    {
                        if (!Auto_Flag.ALarmPause)
                        {
                            BAR._ToolTipDlg.WriteToolTipStr("吸笔[" + (Learn_Pen + 1).ToString() + "]旋转超时;[处理异常->确定->继续]");
                            BAR.ShowToolTipWnd(true);
                        }
                        else
                        {
                            Continue_AfterAlarm();
                        }
                    }
                    break;

                case 11:
                    Vertical_Position_Control(Learn_Pen, HeightVal.Buffer, homePrm_Z[Learn_Pen].velHigh,true);
                    if (Auto_Flag.Run_InPlace)
                    {
                        Auto_Flag.Run_InPlace = false;
                        Learn_Step = PenType == 1 ? 70 : 12;
                    }
                    break;

                case 70: //检测吸笔下降到位			
                    if (In_Output.penLimitI[Learn_Pen].M)
                    {
                        Learn_Step = 12;
                    }
                    else if (GetSysTime() > AutoTimer.OriginZ_Check)
                    {
                        BAR._ToolTipDlg.WriteToolTipStr("吸笔[" + Learn_Pen + "]下降到位检测超时");
                        BAR.ShowToolTipWnd(true);
                        Learn_Step = 71;
                    }
                    break;

                case 71:
                    if (Continue_AfterAlarm())
                    {
                        Learn_Step = 70;
                        AutoTimer.OriginZ_Check = GetSysTime() + 3000;
                    }
                    break;

                case 12:
                    if (RunState == RUNSTATE.Carrier_TakeIC || RunState == RUNSTATE.BurnSeat_TakeIC)
                    {
                        Learn_Step = 13;
                        TIC_Step = 1;
                    }
                    else if (RunState == RUNSTATE.BurnSeat_LayIC || RunState == RUNSTATE.Carrier_LayOKIC)
                    {
                        Learn_Step = 14;
                        LIC_Step = 1;
                    }
                    break;

                case 13: //取料			
                    TakeIC_Handle(Learn_Pen, AutoTiming.VacuumDuration);
                    if (TIC_Step == 0)
                    {
                        Learn_Step = 15;
                    }
                    break;

                case 14: //放料				
                    LayIC_Handle(Learn_Pen);
                    if (LIC_Step == 0)
                    {
                        Learn_Step = 15;
                    }
                    break;

                case 15: //运行至安全高度
                    Vertical_Position_Control(Learn_Pen, HeightVal.Safe, homePrm_Z[Learn_Pen].velHigh);
                    if (Auto_Flag.Run_InPlace)
                    {
                        Auto_Flag.Run_InPlace = false;
                        if (RunState == RUNSTATE.Carrier_TakeIC)
                        {
                            if (In_Output.vacuumI[Learn_Pen].M)//取料成功
                            {
                                Learn_Step = 20;
                                RunState = RUNSTATE.BurnSeat_LayIC;
                            }
                            else
                            {
                                In_Output.vacuumO[Learn_Pen].M = false;
                                //M00911 = 1;
                                BAR._ToolTipDlg.WriteToolTipStr("真空取料失败;[检查真空是否异常或卡料->确定->继续]");
                                BAR.ShowToolTipWnd(true);
                                Learn_Step = 50;
                            }
                        }
                        else if (RunState == RUNSTATE.BurnSeat_TakeIC)
                        {
                            if (In_Output.vacuumI[Learn_Pen].M)//取料成功
                            {
                                Learn_Step = 40;
                                RunState = RUNSTATE.Carrier_LayOKIC;
                            }
                            else
                            {
                                In_Output.vacuumO[Learn_Pen].M = false;
                                //M00911 = 1;
                                BAR._ToolTipDlg.WriteToolTipStr("真空取料失败;[检查真空是否异常或卡料->确定->继续]");
                                BAR.ShowToolTipWnd(true);
                                Learn_Step = 50;
                            }
                        }
                        else if (RunState == RUNSTATE.BurnSeat_LayIC)
                        {
                            RunState = RUNSTATE.BurnSeat_TakeIC;
                            Learn_Step = 30;
                        }
                        else if (RunState == RUNSTATE.Carrier_LayOKIC)
                        {
                            Learn_Step = 60;
                        }
                    }
                    break;

                case 20: //至当前反学烧录座位置
                    RotateTrigge(true, Learn_Pen, 0d);
                    if (!(Auto_Flag.Flip && (ProgrammerType == GlobConstData.Programmer_WG || ProgrammerType == GlobConstData.Programmer_YED || ProgrammerType == GlobConstData.Programmer_WG_TY)))//压座
                    {
                        if (ProgrammerType != GlobConstData.Programmer_RD)
                        {
                            num = (Learn_Unit + Learn_Group * UserConfig.ScketUnitC) / UserConfig.ScketMotionC;
                            In_Output.pushSeatO[num].M = true;
                        }                    
                    }
                    AutoTimer.OpenSocket = GetSysTime() + 3000;
                    Position_X.Buffer = Group[Learn_Group].Unit[Learn_Unit].TopCamera_X - Pen[Learn_Pen].Offset_TopCamera_X;
                    Position_Y.Buffer = Group[Learn_Group].Unit[Learn_Unit].TopCamera_Y - Pen[Learn_Pen].Offset_TopCamera_Y;
                    BackStep = 21;
                    Learn_Step = 100;
                    break;

                case 21: //至当前反学烧录座位置      
                    Horizontal_Position_Control(Position_X.Buffer, Position_Y.Buffer, homePrm_X.velHigh);
                    if (Auto_Flag.Run_InPlace)
                    {
                        Auto_Flag.Run_InPlace = false;
                        Learn_Step = 22;
                    }               
                    break;

                case 22: //反学烧录座放料
                    num = (Learn_Unit + Learn_Group * UserConfig.ScketUnitC) / UserConfig.ScketMotionC;
                    if (ProgrammerType == GlobConstData.Programmer_WG || ProgrammerType == GlobConstData.Programmer_YED)
                    {
                        if ((!In_Output.pushSeatI[num].M && !Auto_Flag.Flip) || (In_Output.flipI[num].M && Auto_Flag.Flip))
                        {
                            Learn_Step = 10;
                        }
                        else if (GetSysTime() > AutoTimer.OpenSocket)
                        {
                            BAR._ToolTipDlg.WriteToolTipStr("打开座子,感应异常;[请检查气压气阀或气缸感应->确定->继续]");
                            BAR.ShowToolTipWnd(true);
                            Learn_Step = 23;
                        }
                    }
                    else
                    {
                        if (!In_Output.pushSeatI[num].M || ProgrammerType == GlobConstData.Programmer_RD || (Auto_Flag.Flip && ProgrammerType == GlobConstData.Programmer_WG_TY))
                        {
                            Learn_Step = 10;
                        }
                        else if (GetSysTime() > AutoTimer.OpenSocket)
                        {
                            BAR._ToolTipDlg.WriteToolTipStr("打开座子,感应异常;[请检查气压气阀或气缸感应->确定->继续]");
                            BAR.ShowToolTipWnd(true);
                            Learn_Step = 23;
                        }
                    }
                    break;

                case 23:
                    if (Continue_AfterAlarm())
                    {
                        Learn_Step = 22;
                    }
                    break;

                case 30:
                    timer = GetSysTime() + 1000;
                    Learn_Step = 31;
                    break;

                case 31:
                    if (GetSysTime() > timer)
                    {
                        Learn_Step = 10;
                    }
                    break;

                case 40:
                    if (!(Auto_Flag.Flip && (ProgrammerType == GlobConstData.Programmer_WG || ProgrammerType == GlobConstData.Programmer_YED || ProgrammerType == GlobConstData.Programmer_WG_TY)))//压座
                    {
                        if (ProgrammerType != GlobConstData.Programmer_RD)
                        {
                            num = (Learn_Unit + Learn_Group * UserConfig.ScketUnitC) / UserConfig.ScketMotionC;
                            In_Output.pushSeatO[num].M = false;
                        }  
                    }
                    BackStep = 41;
                    Learn_Step = 100;
                    break;

                case 41: //Z1至下相机拍照位置     	                 
                    Horizontal_Position_Control(Pen[Learn_Pen].LowCamera_X, Pen[Learn_Pen].LowCamera_Y, homePrm_X.velHigh);
                    if (Auto_Flag.Run_InPlace)
                    {
                        Auto_Flag.Run_InPlace = false;
                        Learn_Step = 42;
                    }
                    break;

                case 42:
                    if (!Pen[Learn_Pen].Rotate.Busy && !Auto_Flag.ALarmPause)//轴c定位完成
                    {
                        Auto_Flag.LearnReady = true;
                        Learn_Step = 43;
                    }
                    else if (GetSysTime() > Pen[Learn_Pen].Rotate.TimeOut)
                    {
                        if (!Auto_Flag.ALarmPause)
                        {
                            BAR._ToolTipDlg.WriteToolTipStr("吸笔[" + (Learn_Pen + 1).ToString() + "]旋转超时;[处理异常->确定->继续]");
                            BAR.ShowToolTipWnd(true);
                        }
                        else
                        {
                            Continue_AfterAlarm();
                        }
                    }
                    break;

                case 43:
                    if (Button.replaceIC)
                    {
                        Auto_Flag.LearnReady = false;
                        Button.replaceIC = false;
                        Learn_Step = 44;
                    }
                    break;

                case 44:
                    if (Auto_Flag.AutoTray_TakeIC || Auto_Flag.FixedTray_TakeIC)
                    {
                        Position_X.Buffer = Position_X.trayFirst[0];
                        Position_Y.Buffer = Position_Y.trayFirst[0];
                    }
                    else if (Auto_Flag.Brede_TakeIC)
                    {
                        int temp = 0;
                        if (Config.FeederCount == 1)//双飞达
                        {
                            if (!Run.FeederEnable[0].Checked)
                            {
                                temp = 1;
                            }
                        }
                        Position_X.Buffer = Position_X.Feeder[temp];
                        Position_Y.Buffer = Position_Y.Feeder[temp];
                    }
                    else if (Auto_Flag.FixedTube_TakeIC)
                    {
                        Position_X.Buffer = Position_X.NGCup;
                        Position_Y.Buffer = Position_Y.NGCup;
                    }
                    Position_X.Buffer = Position_X.Buffer - Pen[Learn_Pen].Offset_TopCamera_X;
                    Position_Y.Buffer = Position_Y.Buffer - Pen[Learn_Pen].Offset_TopCamera_Y;
                    RotateTrigge(true, Learn_Pen, Get_TIC_RotateAngle());
                    BackStep = 45;
                    Learn_Step = 100;
                    break;

                case 45:                 
                    Horizontal_Position_Control(Position_X.Buffer, Position_Y.Buffer, homePrm_X.velHigh);
                    if (Auto_Flag.Run_InPlace)
                    {
                        Auto_Flag.Run_InPlace = false;
                        Learn_Step = 10;
                    }
                    break;

                case 50:
                    if (Continue_AfterAlarm()) //取料失败报警清除后
                    {
                        Learn_Step = 11;
                    }
                    break;

                case 60:
                    Home_Start();
                    Learn_Step = 61;
                    break;

                case 61:        
                    if (!Auto_Flag.HomeBusy)
                    {
                        Auto_Flag.LearnBusy = false;
                        Learn_Step = 0;
                    }
                    break;

                case 100: //Z轴安全检测
                    AutoTimer.OriginZ_Check = GetSysTime() + 3000;
                    Learn_Step = 101;
                    break;

                case 101: //Z轴安全检测
                    if (OriginZ_Check() && PenHome_Check())
                    {
                        Learn_Step = BackStep;
                    }
                    else if (GetSysTime() > AutoTimer.OriginZ_Check)
                    {
                        BAR._ToolTipDlg.WriteToolTipStr("Z轴未在安全检测高度");
                        BAR.ShowToolTipWnd(true);
                        Learn_Step = 102;
                    }
                    break;

                case 102:
                    if (Continue_AfterAlarm())
                    {
                        Learn_Step = 101;
                    }
                    break;

                default:
                    break;
            }
        }

        public static void Detection_Start(int penN)
        {
            if (!Auto_Flag.SystemBusy)
            {
                RunState = RUNSTATE.Carrier_TakeIC;
                Learn_Pen = penN;
                RotateNum = 0;
                Auto_Flag.LearnBusy = true;
                Button.replaceIC = false;
                Button.rotateIC = false;
                Learn_Step = 0;
                Detection_Step = 1;
                Height_Step = 0;
            }
        }

        /// <summary>
        /// 引脚检测
        /// </summary>
        public void Detection_Handle()
        {
            if (!Auto_Flag.LearnBusy)
            {
                return;
            }
            if (Detection_Step == 0)
            {
                return;
            }
            switch (Detection_Step)
            {
                case 1:
                    Prepare_Step = 1;
                    Auto_Flag.PrepareBusy = true;
                    Detection_Step = 2;
                    break;

                case 2:
                    Prepare();
                    if (!Auto_Flag.PrepareBusy)
                    {
                        if (Prepare_Step == 100)
                        {
                            Detection_Step = 0;
                            Auto_Flag.LearnBusy = false;
                        }
                        else if (Prepare_Step == 0)
                        {
                            Detection_Step = 3;
                        }
                    }
                    break;

                case 3:
                    if (Auto_Flag.AutoTray_TakeIC || Auto_Flag.FixedTray_TakeIC)
                    {
                        Position_X.Buffer = Position_X.trayFirst[0];
                        Position_Y.Buffer = Position_Y.trayFirst[0];
                    }
                    else if (Auto_Flag.Brede_TakeIC)
                    {
                        Position_X.Buffer = Position_X.Feeder[0];
                        Position_Y.Buffer = Position_Y.Feeder[0];
                    }
                    else if (Auto_Flag.FixedTube_TakeIC)
                    {
                        Position_X.Buffer = Position_X.tubeIn;
                        Position_Y.Buffer = Position_Y.tubeIn;
                    }

                    Position_X.Buffer = Position_X.Buffer - Pen[Learn_Pen].Offset_TopCamera_X;
                    Position_Y.Buffer = Position_Y.Buffer - Pen[Learn_Pen].Offset_TopCamera_Y;

                    RotateTrigge(true, Learn_Pen, Get_TIC_RotateAngle());
                    BackStep = 4;
                    Detection_Step = 100;
                    break;

                case 4:
                    Horizontal_Position_Control(Position_X.Buffer, Position_Y.Buffer, homePrm_X.velHigh);
                    if (Auto_Flag.Run_InPlace)
                    {
                        Auto_Flag.Run_InPlace = false;
                        Detection_Step = 10;
                    }
                    break;

                case 10:
                    if (!Pen[Learn_Pen].Rotate.Busy && !Auto_Flag.ALarmPause)//轴c定位完成
                    {
                        Get_Vertical_Position();
                        if (Config.Altimeter != 0 && RunState != RUNSTATE.Detection)
                        {
                            HeightVal.Buffer -= Pen[Learn_Pen].Altimeter_Z;
                        }
                        Detection_Step = 11;
                    }
                    else if (GetSysTime() > Pen[Learn_Pen].Rotate.TimeOut)
                    {
                        if (!Auto_Flag.ALarmPause)
                        {
                            BAR._ToolTipDlg.WriteToolTipStr("吸笔[" + (Learn_Pen + 1).ToString() + "]旋转超时;[处理异常->确定->继续]");
                            BAR.ShowToolTipWnd(true);
                        }
                        else
                        {
                            Continue_AfterAlarm();
                        }
                    }
                    break;

                case 11:
                    Vertical_Position_Control(Learn_Pen, HeightVal.Buffer, homePrm_Z[Learn_Pen].velHigh, true);
                    if (Auto_Flag.Run_InPlace)
                    {
                        Auto_Flag.Run_InPlace = false;
                        Detection_Step = PenType == 1 ? 70 : 12;
                    }
                    break;

                case 70: //检测吸笔下降到位			
                    if (In_Output.penLimitI[Learn_Pen].M)
                    {
                        Detection_Step = 12;
                    }
                    else if (GetSysTime() > AutoTimer.OriginZ_Check)
                    {
                        BAR._ToolTipDlg.WriteToolTipStr("吸笔[" + Learn_Pen + "]下降到位检测超时");
                        BAR.ShowToolTipWnd(true);
                        Detection_Step = 71;
                    }
                    break;

                case 71:
                    if (Continue_AfterAlarm())
                    {
                        Detection_Step = 70;
                        AutoTimer.OriginZ_Check = GetSysTime() + 3000;
                    }
                    break;

                case 12:
                    if (RunState == RUNSTATE.Carrier_TakeIC)
                    {
                        Detection_Step = 13;
                        TIC_Step = 1;
                    }
                    else if (RunState == RUNSTATE.Carrier_LayOKIC)
                    {
                        Detection_Step = 14;
                        LIC_Step = 1;
                    }
                    else if (RunState == RUNSTATE.Detection)
                    {
                        Detection_Step = 30;
                    }
                    break;

                case 13: //取料			
                    TakeIC_Handle(Learn_Pen, AutoTiming.VacuumDuration);
                    if (TIC_Step == 0)
                    {
                        Detection_Step = 15;
                    }
                    break;

                case 14: //放料				
                    LayIC_Handle(Learn_Pen);
                    if (LIC_Step == 0)
                    {
                        Detection_Step = 15;
                    }
                    break;

                case 15: //运行至安全高度
                    Vertical_Position_Control(Learn_Pen, HeightVal.Safe, homePrm_Z[Learn_Pen].velHigh);
                    if (Auto_Flag.Run_InPlace)
                    {
                        Auto_Flag.Run_InPlace = false;
                        if (RunState == RUNSTATE.Carrier_TakeIC)
                        {
                            if (In_Output.vacuumI[Learn_Pen].M)//取料成功
                            {
                                Detection_Step = 20;
                                RunState = RUNSTATE.Detection;
                            }
                            else
                            {
                                In_Output.vacuumO[Learn_Pen].M = false;
                                //M00911 = 1;
                                BAR._ToolTipDlg.WriteToolTipStr("真空取料失败;[检查真空是否异常或卡料->确定->继续]");
                                BAR.ShowToolTipWnd(true);
                                Detection_Step = 50;
                            }
                        }
                        else if (RunState == RUNSTATE.Detection)
                        {
                            Detection_Step = 40;
                            RunState = RUNSTATE.Carrier_LayOKIC;
                        }
                        else if (RunState == RUNSTATE.Carrier_LayOKIC)
                        {
                            Detection_Step = 60;
                        }
                    }
                    break;

                case 20: //至当前反学检测位置
                    int num;
                    RotateTrigge(true, Learn_Pen, 0d);
                    if (Vision_3D.ICType == 1)//BGA
                    {
                        In_Output._3DLightO[0].M = true;
                    }
                    else
                    {
                        In_Output._3DLightO[1].M = true;
                    }
                    Position_X.Buffer = Vision_3D.X - Pen[Learn_Pen].Offset_TopCamera_X;
                    Position_Y.Buffer = Vision_3D.Y - Pen[Learn_Pen].Offset_TopCamera_Y;
                    BackStep = 4;
                    Detection_Step = 100;
                    break;             

                case 30:
                    Auto_Flag.LearnReady = true;
                    Detection_Step = 31;
                    break;
                
                case 31:
                    if (Button.replaceIC)
                    {
                        Auto_Flag.LearnReady = false;
                        Button.replaceIC = false;
                        In_Output._3DLightO[0].M = false;
                        In_Output._3DLightO[1].M = false;
                        Detection_Step = 15;
                    }
                    if (Button.rotateIC)
                    {
                        Auto_Flag.LearnReady = false;
                        Button.rotateIC = false;
                        Detection_Step = 32;
                    }
                    break;

                case 32: //运行至安全高度
                    Vertical_Position_Control(Learn_Pen, HeightVal.Safe, homePrm_Z[Learn_Pen].velHigh);
                    if (Auto_Flag.Run_InPlace)
                    {
                        Auto_Flag.Run_InPlace = false;
                        if (RotateNum % 2 == 0)
                        {
                            RotateTrigge(false, Learn_Pen, 180d);
                        }
                        else
                        {
                            RotateTrigge(false, Learn_Pen, -180d);
                        }
                        RotateNum++;
                        if (RotateNum == 10)
                        {
                            RotateNum = 0;
                        }
                        Detection_Step = 10;
                    }
                    break;

                case 40:
                    if (Auto_Flag.AutoTray_TakeIC || Auto_Flag.FixedTray_TakeIC)
                    {
                        Position_X.Buffer = Position_X.trayFirst[0];
                        Position_Y.Buffer = Position_Y.trayFirst[0];
                    }
                    else if (Auto_Flag.Brede_TakeIC)
                    {
                        Position_X.Buffer = Position_X.Feeder[0];
                        Position_Y.Buffer = Position_Y.Feeder[0];
                    }
                    else if (Auto_Flag.FixedTube_TakeIC)
                    {
                        Position_X.Buffer = Position_X.NGCup;
                        Position_Y.Buffer = Position_Y.NGCup;
                    }
                    Position_X.Buffer = Position_X.Buffer - Pen[Learn_Pen].Offset_TopCamera_X;
                    Position_Y.Buffer = Position_Y.Buffer - Pen[Learn_Pen].Offset_TopCamera_Y;
                    RotateTrigge(true, Learn_Pen, Get_TIC_RotateAngle());
                    BackStep = 4;
                    Detection_Step = 100;
                    break;

                case 50:
                    if (Continue_AfterAlarm()) //取料失败报警清除后
                    {
                        Detection_Step = 11;
                    }
                    break;

                case 60:
                    Home_Start();
                    Detection_Step = 61;
                    break;

                case 61:
                    if (!Auto_Flag.HomeBusy)
                    {
                        Auto_Flag.LearnBusy = false;
                        Detection_Step = 0;
                    }
                    break;

                case 100: //Z轴安全检测
                    AutoTimer.OriginZ_Check = GetSysTime() + 3000;
                    Detection_Step = 101;
                    break;

                case 101: //Z轴安全检测
                    if (OriginZ_Check() && PenHome_Check())
                    {
                        Detection_Step = BackStep;
                    }
                    else if (GetSysTime() > AutoTimer.OriginZ_Check)
                    {
                        BAR._ToolTipDlg.WriteToolTipStr("Z轴未在安全检测高度");
                        BAR.ShowToolTipWnd(true);
                        Detection_Step = 102;
                    }
                    break;

                case 102:
                    if (Continue_AfterAlarm())
                    {
                        Detection_Step = 101;
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 测量高度开始
        /// </summary>
        public static void MeasuringHeight_Start()
        {
            if (!Auto_Flag.SystemBusy)
            {
                RunState = RUNSTATE.Carrier_TakeIC;
                TIC_UnitN = 0;
                TIC_GroupN = 0;
                RotateNum = 0;
                Auto_Flag.LearnBusy = true;
                Button.replaceIC = false;
                Button.rotateIC = false;
                Learn_Step = 0;
                Detection_Step = 0;
                Height_Step = 1;
                for (int i = 0; i < UserConfig.AllMotionC; i++)
                {
                    In_Output.pushSeatO[i].M = false;

                    if (ProgrammerType == GlobConstData.Programmer_WG || ProgrammerType == GlobConstData.Programmer_YED || 
                        ProgrammerType == GlobConstData.Programmer_RD || ProgrammerType == GlobConstData.Programmer_WG_TY)
                    {
                        In_Output.flipO[i].M = false;
                    }
                }
            }
        }

        /// <summary>
        /// 测量高度
        /// </summary>
        public void MeasuringHeight_Handle()
        {
            if (!Auto_Flag.LearnBusy)
            {
                return;
            }
            if (Height_Step == 0)
            {
                return;
            }
            switch (Height_Step)
            {
                case 1:
                    Prepare_Step = 1;
                    Auto_Flag.PrepareBusy = true;
                    Height_Step = 2;
                    break;

                case 2:
                    Prepare();
                    if (!Auto_Flag.PrepareBusy)
                    {
                        if (Prepare_Step == 100)
                        {
                            Height_Step = 0;
                            Auto_Flag.LearnBusy = false;
                        }
                        else if (Prepare_Step == 0)
                        {
                            Height_Step = 3;
                        }
                    }
                    break;

                case 3:
                    if (RunState == RUNSTATE.Carrier_TakeIC)
                    {
                        if (Auto_Flag.AutoTray_TakeIC || Auto_Flag.FixedTray_TakeIC)
                        {
                            Position_X.Buffer = Position_X.trayFirst[0];
                            Position_Y.Buffer = Position_Y.trayFirst[0];
                        }
                        else if (Auto_Flag.Brede_TakeIC)
                        {
                            Position_X.Buffer = Position_X.Feeder[0];
                            Position_Y.Buffer = Position_Y.Feeder[0];
                        }
                        else if (Auto_Flag.FixedTube_TakeIC)
                        {
                            Position_X.Buffer = Position_X.tubeIn;
                            Position_Y.Buffer = Position_Y.tubeIn;
                        }
                    }
                    else if (RunState == RUNSTATE.Carrier_LayIC)
                    {
                        if (Auto_Flag.AutoTray_LayIC || Auto_Flag.FixedTray_LayIC)
                        {
                            Position_X.Buffer = Position_X.trayFirst[0];
                            Position_Y.Buffer = Position_Y.trayFirst[0];
                        }
                        else if (Auto_Flag.Brede_LayIC)
                        {
                            Position_X.Buffer = Position_X.bredeOut;
                            Position_Y.Buffer = Position_Y.bredeOut;
                        }
                    }
                    else if (RunState == RUNSTATE.BurnSeat_TakeIC)
                    {
                        while (true)
                        {
                            if (Group[TIC_GroupN].Unit[TIC_UnitN].Flag_Open)
                            {
                                OpenSocket();
                                Position_X.Buffer = Group[TIC_GroupN].Unit[TIC_UnitN].TopCamera_X - Altimeter.Offset_Socket_X;
                                Position_Y.Buffer = Group[TIC_GroupN].Unit[TIC_UnitN].TopCamera_Y - Altimeter.Offset_Socket_Y;
                                break;
                            }
                            TIC_UnitN++;
                            if (TIC_UnitN == UserConfig.ScketUnitC)//查询到每组最后一个座子
                            {
                                int buff = (TIC_GroupN * UserConfig.MotionGroupC);
                                for (int i = 0; i < UserConfig.MotionGroupC; i++)
                                {
                                    In_Output.pushSeatO[buff + i].M = false;
                                }
                                TIC_UnitN = 0;
                                TIC_GroupN++;
                                if (TIC_GroupN == UserConfig.ScketGroupC)//查询到最后一组
                                {
                                    Height_Step = 60;
                                    break;
                                }
                            }
                        }
                    }

                    if (Height_Step != 60)
                    {
                        Position_X.Buffer -= Altimeter.Offset_TopCamera_X;
                        Position_Y.Buffer -= Altimeter.Offset_TopCamera_Y;
                        BackStep = 4;
                        Height_Step = 100;
                    }
                    break;

                case 4:
                    Horizontal_Position_Control(Position_X.Buffer, Position_Y.Buffer, homePrm_X.velHigh);
                    if (Auto_Flag.Run_InPlace)
                    {
                        Auto_Flag.Run_InPlace = false;
                        timer = GetSysTime() + 1000;
                        Height_Step = 5;
                    }
                    break;                

                case 5:
                    if (RunState == RUNSTATE.BurnSeat_TakeIC)
                    {
                        OpenSocket();
                        int Ret = OpenSocket_InPlace();
                        if (Ret == 40)
                        {
                            BAR._ToolTipDlg.WriteToolTipStr("打开座子,感应异常;[请检查气压气阀或气缸感应->确定->继续]");
                            BAR.ShowToolTipWnd(true);
                            BackStep = 5;
                            Height_Step = 8;
                        }
                        else if (Ret == 6)
                        {

                            Height_Step = 6;
                        }
                    }
                    else
                    {
                        Height_Step = 6;
                    }
                    break;

                case 6:
                    if (GetSysTime() > timer)
                    {
                        Cathetometer.MODBUS_ReadAI();//测高触发处理
                        Height_Step = 7;
                    }
                    break;

                case 7:
                    if (!Altimeter.ReadAI_Busy)
                    {
                        if (Altimeter.ReadAI_Online)
                        {
                            if (Cathetometer.Value <= 0)
                            {
                                string strAlarm = "高度值不合法;[检查测高位置->确定->继续]";
                                BAR._ToolTipDlg.WriteToolTipStr(strAlarm);
                                BAR.ShowToolTipWnd(true);
                                BackStep = 6;
                                Height_Step = 8;
                                break;
                            }
                            int ind = 0;
                            if (RunState == RUNSTATE.Carrier_TakeIC)
                            {
                                if (Auto_Flag.AutoTray_TakeIC || Auto_Flag.FixedTray_TakeIC)
                                {
                                    HeightVal_Altimeter.Tray = Cathetometer.Value;
                                    ind = UserConfig.AllScketC;
                                }
                                else if (Auto_Flag.Brede_TakeIC)
                                {
                                    HeightVal_Altimeter.BredeIn = Cathetometer.Value;
                                    ind = UserConfig.AllScketC + 1;
                                }
                                else if (Auto_Flag.FixedTube_TakeIC)
                                {
                                    HeightVal_Altimeter.TubeIn = Cathetometer.Value;
                                    ind = UserConfig.AllScketC + 3;
                                }
                                g_config.WriteAltimeterHeightValue();
                                //判断是否为盘转盘
                                RunState = ShiftWay == 0 ? RUNSTATE.BurnSeat_TakeIC : RUNSTATE.Carrier_LayIC;
                            }
                            else if (RunState == RUNSTATE.Carrier_LayIC)
                            {
                                if (Auto_Flag.AutoTray_LayIC || Auto_Flag.FixedTray_LayIC)
                                {
                                    HeightVal_Altimeter.Tray = Cathetometer.Value;
                                    ind = UserConfig.AllScketC;
                                }
                                else if (Auto_Flag.Brede_LayIC)
                                {
                                    HeightVal_Altimeter.BredeOut = Cathetometer.Value;
                                    ind = UserConfig.AllScketC + 2;
                                }
                                g_config.WriteAltimeterHeightValue();
                                RunState = RUNSTATE.BurnSeat_TakeIC;
                            }
                            else if (RunState == RUNSTATE.BurnSeat_TakeIC)
                            {
                                Group[TIC_GroupN].Unit[TIC_UnitN].HeightVal = Cathetometer.Value;
                                ind = TIC_GroupN * UserConfig.ScketUnitC + TIC_UnitN;
                                g_config.WriteSocketAltimeterHeightValue(ind);
                                TIC_UnitN++;
                                if (TIC_UnitN == UserConfig.ScketUnitC)//查询到每组最后一个座子
                                {
                                    int buff = (TIC_GroupN * UserConfig.MotionGroupC);
                                    for (int i = 0; i < UserConfig.MotionGroupC; i++)
                                    {
                                        In_Output.pushSeatO[buff + i].M = false;
                                    }
                                    TIC_UnitN = 0;
                                    TIC_GroupN++;
                                    if (TIC_GroupN == UserConfig.ScketGroupC)//查询到最后一组
                                    {
                                        Height_Step = 60;
                                        updateHeight?.Invoke(ind, Cathetometer.Value);
                                        break;
                                    }
                                }
                            }
                            updateHeight?.Invoke(ind, Cathetometer.Value);
                            Height_Step = 3;
                        }
                        else
                        {
                            string strAlarm = "读取测高仪模拟量失败;[检查通讯->确定->继续]";
                            BAR._ToolTipDlg.WriteToolTipStr(strAlarm);
                            BAR.ShowToolTipWnd(true);
                            BackStep = 6;
                            Height_Step = 8;
                        }
                    }
                    break;

                case 8:
                    if (Continue_AfterAlarm())
                    {
                        Height_Step = BackStep;
                    }
                    break;

                case 60:
                    Home_Start();
                    Height_Step = 61;
                    break;

                case 61:
                    if (!Auto_Flag.HomeBusy)
                    {
                        Auto_Flag.LearnBusy = false;
                        Height_Step = 0;
                    }
                    break;

                case 100: //Z轴安全检测
                    AutoTimer.OriginZ_Check = GetSysTime() + 3000;
                    Height_Step = 101;
                    break;

                case 101: //Z轴安全检测
                    if (OriginZ_Check() && PenHome_Check())
                    {
                        Height_Step = BackStep;
                    }
                    else if (GetSysTime() > AutoTimer.OriginZ_Check)
                    {
                        BAR._ToolTipDlg.WriteToolTipStr("Z轴未在安全检测高度");
                        BAR.ShowToolTipWnd(true);
                        Height_Step = 102;
                    }
                    break;

                case 102:
                    if (Continue_AfterAlarm())
                    {
                        Height_Step = 101;
                    }
                    break;

                default:
                    break;
            }
        }


        /// <summary>
        /// 校正烧录座坐标开始
        /// </summary>
        public static void ReviseSeatPos_Start(bool flag = false)
        {
            if (!Auto_Flag.SystemBusy || flag)
            {
                RunState = RUNSTATE.BurnSeat_TakeIC;
                TIC_UnitN = 0;
                TIC_GroupN = 0;
                RotateNum = 0;
                Auto_Flag.LearnBusy = true;
                Button.replaceIC = false;
                Button.rotateIC = false;
                Learn_Step = 0;
                Detection_Step = 0;
                Height_Step = 0;
                ReviseSeat_Step = 1;
                RevisePos_Step = 0;
                for (int i = 0; i < UserConfig.AllMotionC; i++)
                {
                    In_Output.pushSeatO[i].M = false;

                    if (ProgrammerType == GlobConstData.Programmer_WG || ProgrammerType == GlobConstData.Programmer_YED || 
                        ProgrammerType == GlobConstData.Programmer_RD || ProgrammerType == GlobConstData.Programmer_WG_TY)
                    {
                        In_Output.flipO[i].M = false;
                    }
                }
            }
        }

        /// <summary>
        /// 校正烧录座坐标
        /// </summary>
        public void ReviseSeatPos_Handle()
        {
            if (!Auto_Flag.LearnBusy)
            {
                return;
            }
            if (ReviseSeat_Step == 0)
            {
                return;
            }
            if (Config.ZoomLens == 1)
            {
                ZoomLens.MODBUS_ReadStatus();
            }
            switch (ReviseSeat_Step)
            {
                case 1:
                    Prepare_Step = 1;
                    Auto_Flag.PrepareBusy = true;
                    ReviseSeat_Step = 2;
                    break;
                    
                case 2:
                    Prepare();
                    if (!Auto_Flag.PrepareBusy)
                    {
                        if (Prepare_Step == 100)
                        {
                            ReviseSeat_Step = 0;
                            Auto_Flag.LearnBusy = false;
                        }
                        else if (Prepare_Step == 0)
                        {
                            ReviseSeat_Step = 3;
                            ReviseConut = 0;
                        }
                    }
                    break;

                case 3:
                    if (RunState == RUNSTATE.BurnSeat_TakeIC)
                    {
                        if (Config.ZoomLens == 1)
                        {
                            ZoomLens.SetPos = ZoomLens.OriginPos - ZoomLens_S.Socket - ZoomLens.NowPos;
                        }
                        while (true)
                        {
                            if (Group[TIC_GroupN].Unit[TIC_UnitN].Flag_Open)
                            {
                                OpenSocket();
                                Position_X.Buffer = Group[TIC_GroupN].Unit[TIC_UnitN].TopCamera_X;
                                Position_Y.Buffer = Group[TIC_GroupN].Unit[TIC_UnitN].TopCamera_Y;
                                break;
                            }
                            TIC_UnitN++;
                            if (TIC_UnitN == UserConfig.ScketUnitC)//查询到每组最后一个座子
                            {
                                int buff = (TIC_GroupN * UserConfig.MotionGroupC);
                                for (int i = 0; i < UserConfig.MotionGroupC; i++)
                                {
                                    In_Output.pushSeatO[buff + i].M = false;
                                }
                                TIC_UnitN = 0;
                                TIC_GroupN++;
                                if (TIC_GroupN == UserConfig.ScketGroupC)//查询到最后一组
                                {
                                    g_act.CCDCap(GlobConstData.ST_CCDUP, true);
                                    ReviseSeat_Step = 60;
                                    TIC_GroupN = 0;
                                    break;
                                }
                            }
                        }
                    }

                    if (ReviseSeat_Step != 60)
                    {
                        ZoomLens_S.GODelay = GetSysTime() + 6000;
                        BackStep = 4;
                        ReviseSeat_Step = 100;
                    }
                    break;

                case 4:
                    Horizontal_Position_Control(Position_X.Buffer, Position_Y.Buffer, 500);
                    if (Config.ZoomLens == 1)
                    {
                        ZoomLens.MODBUS_ReadStatus();
                        DST_Function_ZL(ZoomLens.OriginPos - ZoomLens_S.Socket);
                    }
                    else if (Config.ZoomLens == 2)
                    {
                        ZoomLens.SetPos = ZoomLens_S.Socket;
                        Camera_Position_Control(ZoomLens.SetPos);
                    }

                    if (Auto_Flag.Run_InPlace && (Config.ZoomLens == 0 || ZoomLens_S.IsDone || Auto_Flag.Camera_InPlace))
                    {
                        Auto_Flag.Run_InPlace = false;
                        Auto_Flag.Camera_InPlace = false;
                        ZoomLens_S.IsDone = false;
                        timer = GetSysTime() + 100;
                        ReviseSeat_Step = 5;
                    }
                    else if (Config.ZoomLens == 1 && (GetSysTime() > ZoomLens_S.GODelay))
                    {
                        ZoomLens_S.IsBusy = false;
                        ZoomLens_S.IsDone = false;
                        string strAlarm = "变焦镜头未到位,检查镜头旋转是否正常;[确定->继续]";
                        BAR._ToolTipDlg.WriteToolTipStr(strAlarm);
                        BAR.ShowToolTipWnd(true);
                        BackStep = 4;
                        ReviseSeat_Step = 10;
                    }
                    break;

                case 5:
                    if (RunState == RUNSTATE.BurnSeat_TakeIC)
                    {
                        OpenSocket();
                        int Ret = OpenSocket_InPlace();
                        if (Ret == 40)
                        {
                            BAR._ToolTipDlg.WriteToolTipStr("打开座子,感应异常;[请检查气压气阀或气缸感应->确定->继续]");
                            BAR.ShowToolTipWnd(true);
                            BackStep = 5;
                            ReviseSeat_Step = 10;
                        }
                        else if (Ret == 6)
                        {

                            ReviseSeat_Step = 6;
                        }
                    }
                    else
                    {
                        ReviseSeat_Step = 6;
                    }
                    break;

                case 6:
                    if (GetSysTime() > timer)
                    {
                        //校正烧录座坐标触发处理
                        tempFlag = ReviseSeat(ref dy, ref dx);
                        Position_X.Buffer = dx;
                        Position_Y.Buffer = dy;
                        ReviseSeat_Step = 7;
                    }
                    break;

                case 7:
                    if (tempFlag)
                    {
                        Horizontal_Position_Control(Position_X.Buffer, Position_Y.Buffer, homePrm_X.velHigh);
                        if (Auto_Flag.Run_InPlace)
                        {
                            Group[TIC_GroupN].Unit[TIC_UnitN].TopCamera_X = Axis.trapPrm_X.getPos;
                            Group[TIC_GroupN].Unit[TIC_UnitN].TopCamera_Y = Axis.trapPrm_Y.getPos;
                            tempFlag = false;
                            Auto_Flag.Run_InPlace = false;
                            timer = GetSysTime() + 1000;
                            ReviseConut++;
                            ReviseSeat_Step = 8;
                        }
                    }
                    else
                    {
                        string strAlarm = "自动校正烧录座失败;[确定->继续]";
                        BAR._ToolTipDlg.WriteToolTipStr(strAlarm);
                        BAR.ShowToolTipWnd(true);
                        BackStep = 6;
                        ReviseSeat_Step = 10;
                    }
                    break;
                case 8:
                    if (ReviseConut > 2)
                    {
                        ReviseSeat_Step = 9;
                    }
                    else
                    {
                        ReviseSeat_Step = 3;
                    }
                    break;
                case 9:
                    if (GetSysTime() > timer)
                    {
                        if (RunState == RUNSTATE.BurnSeat_TakeIC)
                        {
                            int ind = TIC_GroupN * UserConfig.ScketUnitC + TIC_UnitN;
                            Group[TIC_GroupN].Unit[TIC_UnitN].TopCamera_X = g_config.ArrFixOut[ind].P.X = Axis.trapPrm_X.getPos;
                            Group[TIC_GroupN].Unit[TIC_UnitN].TopCamera_Y = g_config.ArrFixOut[ind].P.Y = Axis.trapPrm_Y.getPos;
                            
                            g_config.WriteFixOutPos(ind);
                            TIC_UnitN++;
                            if (TIC_UnitN == UserConfig.ScketUnitC)//查询到每组最后一个座子
                            {
                                int buff = (TIC_GroupN * UserConfig.MotionGroupC);
                                for (int i = 0; i < UserConfig.MotionGroupC; i++)
                                {
                                    In_Output.pushSeatO[buff + i].M = false;
                                }
                                TIC_UnitN = 0;
                                TIC_GroupN++;
                                if (TIC_GroupN == UserConfig.ScketGroupC)//查询到最后一组
                                {
                                    ReviseSeat_Step = 60;
                                    updateSeatPos?.Invoke(ind);
                                    TIC_GroupN = 0;
                                    break;
                                }
                            }
                            updateSeatPos?.Invoke(ind);
                            ReviseConut = 0;//单个烧录座校准次数清零
                            ReviseSeat_Step = 3;
                        }
                    }
                    break;
                case 10:
                    if (Continue_AfterAlarm())
                    {
                        ZoomLens_S.GODelay = GetSysTime() + 6000;
                        ReviseSeat_Step = BackStep;
                    }
                    break;

                case 60:
                    Home_Start();
                    ReviseSeat_Step = 61;
                    break;

                case 61:
                    if (!Auto_Flag.HomeBusy)
                    {
                        Auto_Flag.LearnBusy = false;
                        ReviseSeat_Step = 0;
                    }
                    break;

                case 100: //Z轴安全检测
                    AutoTimer.OriginZ_Check = GetSysTime() + 3000;
                    ReviseSeat_Step = 101;
                    break;

                case 101: //Z轴安全检测
                    if (OriginZ_Check() && PenHome_Check())
                    {
                        ReviseSeat_Step = BackStep;
                    }
                    else if (GetSysTime() > AutoTimer.OriginZ_Check)
                    {
                        BAR._ToolTipDlg.WriteToolTipStr("Z轴未在安全检测高度");
                        BAR.ShowToolTipWnd(true);
                        ReviseSeat_Step = 102;
                    }
                    break;

                case 102:
                    if (Continue_AfterAlarm())
                    {
                        ReviseSeat_Step = 101;
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 校准所有坐标开始
        /// </summary>
        public static void RevisePos_Start()
        {
            if (!Auto_Flag.SystemBusy || Auto_Flag.AutoRevisePos)
            {
                RunState = RUNSTATE.Carrier_TakeIC;
                TIC_UnitN = 0;
                TIC_GroupN = 0;
                RotateNum = 0;
                Auto_Flag.LearnBusy = true;
                Button.replaceIC = false;
                Button.rotateIC = false;
                Learn_Step = 0;
                Detection_Step = 0;
                Height_Step = 0;
                ReviseSeat_Step = 0;
                RevisePos_Step = 1;
				Auto_Flag.AutoRevisePos = false;
				for (int i = 0; i < UserConfig.AllMotionC; i++)
                {
                    In_Output.pushSeatO[i].M = false;

                    if (ProgrammerType == GlobConstData.Programmer_WG || ProgrammerType == GlobConstData.Programmer_YED || 
                        ProgrammerType == GlobConstData.Programmer_RD || ProgrammerType == GlobConstData.Programmer_WG_TY)
                    {
                        In_Output.flipO[i].M = false;
                    }
                }
			}
		}

        /// <summary>
        /// 校准所有坐标
        /// </summary>
        public void RevisePos_Handle()
        {
            
            if (!Auto_Flag.LearnBusy)
            {
                return;
            }
            if (RevisePos_Step == 0)
            {
                return;
            }
            if (Config.ZoomLens == 1)
            {
                ZoomLens.MODBUS_ReadStatus();
            }
            
            switch (RevisePos_Step)
            {
                case 1:
                    Prepare_Step = 1;
                    Auto_Flag.PrepareBusy = true;
                    RevisePos_Step = 2;
                    break;

                case 2:
                    Prepare();
                    if (!Auto_Flag.PrepareBusy)
                    {
                        if (Prepare_Step == 100)
                        {
                            RevisePos_Step = 0;
                            Auto_Flag.LearnBusy = false;
                        }
                        else if (Prepare_Step == 0)
                        {
                            RevisePos_Step = 3;
                            ReviseConut = 0;
                            indTray = 0;
                        }
                    }
                    break;

                case 3:
                    if (RunState == RUNSTATE.Carrier_TakeIC)
                    {
                        if (Auto_Flag.AutoTray_TakeIC || Auto_Flag.FixedTray_TakeIC)
                        {
                            if (indTray < 3)
                            {
                                Position_X.Buffer = Position_X.trayFirst[indTray];
                                Position_Y.Buffer = Position_Y.trayFirst[indTray];
                            }
                            else if (indTray == 3)
                            {
                                Position_X.Buffer = Position_X.trayEnd[0];
                                Position_Y.Buffer = Position_Y.trayEnd[0];
                            }
                            //Position_X.Buffer = g_config.MarkPos[g_config.TrayModel].Tray[indTray].X;
                            //Position_Y.Buffer = g_config.MarkPos[g_config.TrayModel].Tray[indTray].Y;
                            indPos = indTray;
                            indModel = GlobConstData.ST_ModelTrayPOS;
                            ZoomLens.SetPos = Config.ZoomLens == 1 ? ZoomLens.OriginPos - ZoomLens_S.Tray - ZoomLens.NowPos : ZoomLens_S.Tray;
                            PosPul = ZoomLens.OriginPos - ZoomLens_S.Tray;
                        }
                        else if (Auto_Flag.Brede_TakeIC)
                        {
                            if (Auto_Flag.NGTray)
                            {
                                ReviseNGTray = true;
                            }
                            Position_X.Buffer = Position_X.Feeder[0];
                            Position_Y.Buffer = Position_Y.Feeder[0];
                            indPos = 5;
                            indModel = GlobConstData.ST_ModelBredeInPOS;
                            ZoomLens.SetPos = Config.ZoomLens == 1 ? ZoomLens.OriginPos - ZoomLens_S.BredeIn - ZoomLens.NowPos : ZoomLens_S.BredeIn;
                            PosPul = ZoomLens.OriginPos - ZoomLens_S.BredeIn;
                        }
                        else if (Auto_Flag.FixedTube_TakeIC)
                        {
                            if (Auto_Flag.NGTray)
                            {
                                ReviseNGTray = true;
                            }
                            Position_X.Buffer = Position_X.tubeIn;
                            Position_Y.Buffer = Position_Y.tubeIn;
                            indPos = 7;
                            indModel = GlobConstData.ST_ModelTubeInPOS;
                            ZoomLens.SetPos = Config.ZoomLens == 1 ? ZoomLens.OriginPos - ZoomLens_S.TubeIn - ZoomLens.NowPos : ZoomLens_S.TubeIn;
                            PosPul = ZoomLens.OriginPos - ZoomLens_S.TubeIn;
                        }
                    }
                    else if (RunState == RUNSTATE.Carrier_LayIC)
                    {
                        if (Auto_Flag.AutoTray_LayIC || Auto_Flag.FixedTray_LayIC)
                        {
                            if (Auto_Flag.NGTray)
                            {
                                ReviseNGTray = false;
                            }
                            if (indTray < 3)
                            {
                                Position_X.Buffer = Position_X.trayFirst[indTray];
                                Position_Y.Buffer = Position_Y.trayFirst[indTray];
                            }
                            else if (indTray == 3)
                            {
                                Position_X.Buffer = Position_X.trayEnd[0];
                                Position_Y.Buffer = Position_Y.trayEnd[0];
                            }
                            //Position_X.Buffer = g_config.MarkPos[g_config.TrayModel].Tray[indTray].X;
                            //Position_Y.Buffer = g_config.MarkPos[g_config.TrayModel].Tray[indTray].Y;
                            indPos = indTray;
                            indModel = GlobConstData.ST_ModelTrayPOS;
                            ZoomLens.SetPos = Config.ZoomLens == 1 ? ZoomLens.OriginPos - ZoomLens_S.Tray - ZoomLens.NowPos : ZoomLens_S.Tray;
                            PosPul = ZoomLens.OriginPos - ZoomLens_S.Tray;
                        }
                        else if (Auto_Flag.Brede_LayIC)
                        {
                            Position_X.Buffer = Position_X.bredeOut;
                            Position_Y.Buffer = Position_Y.bredeOut;
                            indPos = 4;
                            indModel = GlobConstData.ST_ModelBredeOutPOS;
                            ZoomLens.SetPos = Config.ZoomLens == 1 ? ZoomLens.OriginPos - ZoomLens_S.BredeOut - ZoomLens.NowPos : ZoomLens_S.BredeOut;
                            PosPul = ZoomLens.OriginPos - ZoomLens_S.BredeOut;
                        }
                    }
                    else if (RunState == RUNSTATE.End)
                    {
                        Position_X.Buffer = Position_X.trayFirst[2];
                        Position_Y.Buffer = Position_Y.trayFirst[2];
                        indPos = indTray;
                        indModel = GlobConstData.ST_ModelTrayPOS;
                        ZoomLens.SetPos = Config.ZoomLens == 1 ? ZoomLens.OriginPos - ZoomLens_S.Tray - ZoomLens.NowPos : ZoomLens_S.Tray;
                        PosPul = ZoomLens.OriginPos - ZoomLens_S.Tray;
                    }
                    ZoomLens_S.GODelay = GetSysTime() + 6000;
                    BackStep = 4;
                    RevisePos_Step = 100;
                    break;

                case 4:
                    Horizontal_Position_Control(Position_X.Buffer, Position_Y.Buffer, 500);
                    if (Config.ZoomLens == 1)
                    {
                        ZoomLens.MODBUS_ReadStatus();
                        DST_Function_ZL(PosPul);
                    }
                    else if (Config.ZoomLens == 2)
                    {
                        Camera_Position_Control(ZoomLens.SetPos);
                    }
                    if (Auto_Flag.Run_InPlace && (ZoomLens_S.IsDone || Auto_Flag.Camera_InPlace))
                    {
                        Auto_Flag.Run_InPlace = false;
                        Auto_Flag.Camera_InPlace = false;
                        ZoomLens_S.IsDone = false;
                        timer = GetSysTime() + 100;
                        RevisePos_Step = 5;
                    }
                    else if(GetSysTime() > ZoomLens_S.GODelay)
                    {
                        ZoomLens_S.IsBusy = false;
                        ZoomLens_S.IsDone = false;
                        string strAlarm = "变焦镜头未到位,检查镜头旋转是否正常;[确定->继续]";
                        BAR._ToolTipDlg.WriteToolTipStr(strAlarm);
                        BAR.ShowToolTipWnd(true);
                        BackStep = 4;
                        RevisePos_Step = 10;
                    }
                    break;

                case 5:
                    if (GetSysTime() > timer)
                    {
                        //校正所有坐标触发处理
                        tempFlag = RevisePOS(indModel, ref dy, ref dx);
                        Position_X.Buffer = dx;
                        Position_Y.Buffer = dy;
                        RevisePos_Step = 6;
                    }
                    break;

                case 6:
                    if (tempFlag)
                    {
                        Horizontal_Position_Control(Position_X.Buffer, Position_Y.Buffer, homePrm_X.velHigh);
                        if (Auto_Flag.Run_InPlace)
                        {
                            if (RunState == RUNSTATE.Carrier_TakeIC)
                            {
                                if (Auto_Flag.AutoTray_TakeIC || Auto_Flag.FixedTray_TakeIC)
                                {
                                    //g_config.MarkPos[g_config.TrayModel].Tray[indTray].X = Axis.trapPrm_X.getPos;
                                    //g_config.MarkPos[g_config.TrayModel].Tray[indTray].Y = Axis.trapPrm_Y.getPos;
                                    if (indTray < 3)
                                    {
                                        Position_X.trayFirst[indTray] = g_config.ArrFixIn[indPos].P.X = Axis.trapPrm_X.getPos;
                                        Position_Y.trayFirst[indTray] = g_config.ArrFixIn[indPos].P.Y = Axis.trapPrm_Y.getPos;
                                    }
                                    else if (indTray == 3)
                                    {
                                        Position_X.trayEnd[0] = g_config.ArrFixIn[indPos].P.X = Axis.trapPrm_X.getPos;
                                        Position_Y.trayEnd[0] = g_config.ArrFixIn[indPos].P.Y = Axis.trapPrm_Y.getPos;
                                    }
                                }
                                else if (Auto_Flag.Brede_TakeIC)
                                {
                                    Position_X.Feeder[0] = g_config.ArrFixIn[indPos].P.X = Axis.trapPrm_X.getPos;
                                    Position_Y.Feeder[0] = g_config.ArrFixIn[indPos].P.Y = Axis.trapPrm_Y.getPos;
                                }
                                else if (Auto_Flag.FixedTube_TakeIC)
                                {
                                    Position_X.tubeIn = g_config.ArrFixIn[indPos].P.X = Axis.trapPrm_X.getPos;
                                    Position_Y.tubeIn = g_config.ArrFixIn[indPos].P.Y = Axis.trapPrm_Y.getPos;
                                }
                            }
                            else if (RunState == RUNSTATE.Carrier_LayIC)
                            {
                                if (Auto_Flag.AutoTray_LayIC || Auto_Flag.FixedTray_LayIC)
                                {
                                    //g_config.MarkPos[g_config.TrayModel].Tray[indTray].X = Axis.trapPrm_X.getPos;
                                    //g_config.MarkPos[g_config.TrayModel].Tray[indTray].Y = Axis.trapPrm_Y.getPos;
                                    if (indTray < 3)
                                    {
                                        Position_X.trayFirst[indTray] = g_config.ArrFixIn[indPos].P.X = Axis.trapPrm_X.getPos;
                                        Position_Y.trayFirst[indTray] = g_config.ArrFixIn[indPos].P.Y = Axis.trapPrm_Y.getPos;
                                    }
                                    else if (indTray == 3)
                                    {
                                        Position_X.trayEnd[0] = g_config.ArrFixIn[indPos].P.X = Axis.trapPrm_X.getPos;
                                        Position_Y.trayEnd[0] = g_config.ArrFixIn[indPos].P.Y = Axis.trapPrm_Y.getPos;
                                    }
                                }
                                else if (Auto_Flag.Brede_LayIC)
                                {
                                    Position_X.bredeOut = g_config.ArrFixIn[indPos].P.X = Axis.trapPrm_X.getPos;
                                    Position_Y.bredeOut = g_config.ArrFixIn[indPos].P.Y = Axis.trapPrm_Y.getPos;
                                }
                            }
                            else if (RunState == RUNSTATE.End)
                            {
                                Position_X.trayFirst[2] = g_config.ArrFixIn[2].P.X = Axis.trapPrm_X.getPos;
                                Position_Y.trayFirst[2] = g_config.ArrFixIn[2].P.Y = Axis.trapPrm_Y.getPos;
                            }
                            tempFlag = false;
                            Auto_Flag.Run_InPlace = false;
                            timer = GetSysTime() + 1000;
                            ReviseConut++;
                            RevisePos_Step = 7;
                        }
                    }
                    else
                    {
                        string strAlarm = "自动校正坐标失败;[确定->继续]";
                        BAR._ToolTipDlg.WriteToolTipStr(strAlarm);
                        BAR.ShowToolTipWnd(true);
                        BackStep = 5;
                        RevisePos_Step = 10;
                    }
                    break;
                case 7:
                    if (ReviseConut > 2)
                    {
                        RevisePos_Step = 8;
                    }
                    else
                    {
                        RevisePos_Step = 3;
                    }
                    break;

                case 8://校准位置3次结束
                    if (GetSysTime() > timer)
                    {
                        ReviseConut = 0;
                        
                        if (RunState == RUNSTATE.Carrier_TakeIC)
                        {
                            if (Auto_Flag.AutoTray_TakeIC || Auto_Flag.FixedTray_TakeIC)
                            {
                                //CheckOffset();
                                g_config.WriteFixInPos(indPos);
                                indTray++;//下一个料盘位置
                                if (indTray == 1 && Auto_Flag.FixedTray_TakeIC)//固定盘模式跳过料盘2对标
                                {
                                    indTray = 2;
                                }
                                //判断所有料盘是否校准完成
                                if (indTray < 4)//未完成
                                {
                                    RevisePos_Step = 3;
                                    break;
                                }
                                if (indTray > 3)
                                {
                                    ComputeTraySpace();
                                }
                            }
                            
                            g_config.WriteFixInPos(indPos);
                            
                            //判断是否为盘转盘
                            RunState = ShiftWay == 0 ? RUNSTATE.BurnSeat_TakeIC : RUNSTATE.Carrier_LayIC;
                            if (RunState == RUNSTATE.BurnSeat_TakeIC)
                            {
                                RevisePos_Step = 60;
                                break;
                            }
                        }
                        else if (RunState == RUNSTATE.Carrier_LayIC)
                        {
                            if (Auto_Flag.AutoTray_LayIC || Auto_Flag.FixedTray_LayIC)
                            {
                                //CheckOffset();
                                g_config.WriteFixInPos(indPos);
                                indTray++;//下一个料盘位置
                                if (indTray == 1 && Auto_Flag.FixedTray_TakeIC)//固定盘模式跳过料盘2对标
                                {
                                    indTray = 2;
                                }
                                //判断所有料盘是否校准完成
                                if (indTray < 4)//未完成
                                {
                                    RevisePos_Step = 3;
                                    break;
                                }
                                if (indTray > 3)
                                {
                                    ComputeTraySpace();
                                }
                            }
                            else if (Auto_Flag.NGTray && ReviseNGTray)
                            {
                                RunState = RUNSTATE.End;
                                RevisePos_Step = 3;
                                break;
                            }
                            g_config.WriteFixInPos(indPos);
                            RevisePos_Step = 60;//烧录座校准开始
                            break;
                        }
                        else if (RunState == RUNSTATE.End)//NG盘
                        {
                            g_config.WriteFixInPos(2);
                            RevisePos_Step = 60;//烧录座校准开始
                            break;
                        }

                        RevisePos_Step = 3;
                    }
                    break;

                case 10:
                    if (Continue_AfterAlarm())
                    {
                        RevisePos_Step = BackStep;
                    }
                    break;

                case 60:
                    //Auto_Flag.LearnBusy = false;
                    RevisePos_Step = 0;
                    ReviseSeatPos_Start(true);
                    break;

                case 100: //Z轴安全检测
                    AutoTimer.OriginZ_Check = GetSysTime() + 3000;
                    RevisePos_Step = 101;
                    break;

                case 101: //Z轴安全检测
                    if (OriginZ_Check() && PenHome_Check())
                    {
                        RevisePos_Step = BackStep;
                    }
                    else if (GetSysTime() > AutoTimer.OriginZ_Check)
                    {
                        BAR._ToolTipDlg.WriteToolTipStr("Z轴未在安全检测高度");
                        BAR.ShowToolTipWnd(true);
                        RevisePos_Step = 102;
                    }
                    break;

                case 102:
                    if (Continue_AfterAlarm())
                    {
                        RevisePos_Step = 101;
                    }
                    break;

                default:
                    break;
            }
        }

        private void CheckOffset()
        {
            double temp_X = 0, temp_Y = 0, temp_CorrectionX = 0, temp_CorrectionY = 0;
            short dir_X = 1, dir_Y = 1;
            temp_X = MarkOffset.Tray_X;
            temp_Y = MarkOffset.Tray_Y;
            temp_CorrectionX = g_config.MarkPos[g_config.TrayModel].Correction[indTray].X;
            temp_CorrectionY = g_config.MarkPos[g_config.TrayModel].Correction[indTray].Y;
            if (g_config.TrayModel == 0)//三横盘
            {

            }
            else if (g_config.TrayModel == 1)//两竖一横盘
            {
                
            }
            else if (g_config.TrayModel == 2)//两横一竖盘
            {
                if (indTray == 1)
                {
                    dir_X = 1;
                    dir_Y = -1;
                    temp_X = MarkOffset.Tray_Y;
                    temp_Y = MarkOffset.Tray_X;
                }
            }
            Position_X.trayFirst[indTray] = g_config.ArrFixIn[indPos].P.X = Axis.trapPrm_X.getPos - temp_X * dir_X + temp_CorrectionX;
            Position_Y.trayFirst[indTray] = g_config.ArrFixIn[indPos].P.Y = Axis.trapPrm_Y.getPos - temp_Y * dir_Y + temp_CorrectionY;
        }

        /// <summary>
        /// 计算料盘间距
        /// </summary>
        public void ComputeTraySpace()
        {
            PointD offSet = new PointD();

            offSet.X = Math.Abs(g_config.ArrFixIn[0].P.X - g_config.ArrFixIn[3].P.X);   //行相减
            offSet.Y = Math.Abs(g_config.ArrFixIn[0].P.Y - g_config.ArrFixIn[3].P.Y);   //列相减
            TrayD.Col_Space = offSet.X / (TrayD.ColC - 1);
            TrayD.Row_Space = offSet.Y / (TrayD.RowC - 1);

            g_config.SavePanVal(); //保存行列间距
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "保存行列间距成功", "Modify");
        }

        /// <summary>
        /// CMK测量开始
        /// </summary>
        /// <param name="setPos_X"></param>
        /// <param name="setPos_Y"></param>
        /// <param name="setPos_Z"></param>
        /// <param name="penN"></param>
        public static void CMK_Start(Double setPos_X, Double setPos_Y, Double setPos_Z, int axisN, int count)
        {
            if (!Auto_Flag.SystemBusy)
            {
                Position_X.Buffer = setPos_X;
                Position_Y.Buffer = setPos_Y;
                HeightVal.Buffer = setPos_Z;
                CMK_Axis = axisN;
                CMK_Count = count;
                Auto_Flag.LearnBusy = true;
                CMK_Step = 1;
            }
        }

        /// <summary>
        /// CMK测量停止
        /// </summary>
        public static void CMK_Stop()
        {
            Auto_Flag.LearnBusy = false;
            CMK_Step = 0;
        }

        public void CMK_Handle()
        {
            if (!Auto_Flag.LearnBusy)
            {
                return;
            }
            if (CMK_Step == 0)
            {
                return;
            }
            
            switch (CMK_Step)
            {
                case 1:
                    Prepare_Step = 1;
                    Auto_Flag.PrepareBusy = true;
                    CMK_Step = 2;
                    break;

                case 2:
                    Prepare();
                    if (!Auto_Flag.PrepareBusy)
                    {
                        if (Prepare_Step == 100)
                        {
                            CMK_Step = 0;
                            Auto_Flag.LearnBusy = false;
                        }
                        else if (Prepare_Step == 0)
                        {
                            CMK_Step = 3;
                        }
                    }
                    break;

                case 3:
                    if (CMK_Axis == 0)//X轴
                    {
                        Horizontal_Y_Position_Control(Position_Y.Buffer);
                    }
                    else if (CMK_Axis == 1)//Y轴
                    {
                        Horizontal_X_Position_Control(Position_X.Buffer);
                    }
                    else//Z轴
                    {
                        Horizontal_Position_Control(Position_X.Buffer, Position_Y.Buffer);
                    }
                    if (Auto_Flag.Run_InPlace)
                    {
                        Auto_Flag.Run_InPlace = false;
                        CMK_Step = 4;
                    }
                    break;

                case 4://零位
                    if (CMK_Axis == 0)//X轴
                    {
                        Horizontal_X_Position_Control(0);
                    }
                    else if (CMK_Axis == 1)//Y轴
                    {
                        Horizontal_Y_Position_Control(0);
                    }
                    else//Z轴
                    {
                        Vertical_Position_Control(0, HeightVal.Safe);
                    }
                    if (Auto_Flag.Run_InPlace)
                    {
                        Auto_Flag.Run_InPlace = false;
                        CMK_Step = 5;
                    }
                    break;

                case 5://测量位置
                    if (CMK_Axis == 0)//X轴
                    {
                        Horizontal_X_Position_Control(Position_X.Buffer);
                    }
                    else if (CMK_Axis == 1)//Y轴
                    {
                        Horizontal_Y_Position_Control(Position_Y.Buffer);
                    }
                    else//Z轴
                    {
                        Vertical_Position_Control(0, HeightVal.Buffer);
                    }
                    if (Auto_Flag.Run_InPlace)
                    {
                        Auto_Flag.Run_InPlace = false;
                        timer = GetSysTime() + 10000;
                        CMK_Count--;
                        CMK_Step = 6;
                    }
                    break;

                case 6:
                    if (GetSysTime() > timer)
                    {
                        if (CMK_Count < 0) 
                        {
                            
                            CMK_Step = 7;
                        }
                        else
                        {
                            CMK_Step = 4;
                        }
                    }
                    break;

                case 7:
                    Vertical_Position_Control(0, HeightVal.Safe);
                    if (Auto_Flag.Run_InPlace)
                    {
                        Auto_Flag.Run_InPlace = false;
                        CMK_Step = 8;
                    }
                    break;
                case 8:
                    Horizontal_Position_Control(0, 0);
                    if (Auto_Flag.Run_InPlace)
                    {
                        Auto_Flag.Run_InPlace = false;
                        Auto_Flag.LearnBusy = false;
                        CMK_Step = 0;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
