using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BAR.Commonlib;
using PLC;

namespace BAR
{
    public class Reset
    {
        static Act g_act = Act.GetInstance();
        Config g_config = Config.GetInstance();
        public static void ResetState(bool mode)
        {
            Auto_Flag.AutoRunBusy = false;
            Auto_Flag.GOBusy = false;
            Auto_Flag.HomeBusy = false;          
            Auto_Flag.LearnBusy = false;
            Auto_Flag.AutoRevisePos = false;

            Auto_Flag.ALarm = false;
            Auto_Flag.ALarmPause = false;
            Auto_Flag.Pause = false;
            Auto_Flag.ManualEnd = false;
            Auto_Flag.Run_InPlace = false;
            In_Output.buzzer.M = false;
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                In_Output.blowO[i].M = false;
            }
            if (UserTask.ProgrammerType == GlobConstData.Programmer_RD)
            {
                for (int i = 0; i < UserConfig.AllScketC; i++)
                {
                    In_Output.resetScketO[i].M = false;
                }
            }

            if (mode)
            {
                for (int i = 0; i < UserConfig.AllMotionC; i++)
                {
                    In_Output.pushSeatO[i].M = false;

                    if (UserTask.ProgrammerType == GlobConstData.Programmer_WG || UserTask.ProgrammerType == GlobConstData.Programmer_YED || 
                        UserTask.ProgrammerType == GlobConstData.Programmer_RD || UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY)
                    {
                        In_Output.flipO[i].M = false;
                    }
                }

                for (int i = 0; i < UserConfig.VacuumPenC; i++)
                {
                    In_Output.vacuumO[i].M = false;
                    if (UserTask.PenType == 1)
                    {
                        In_Output.penO[i].M = false;
                    }
                }

                for (int i = 0; i < UserConfig.CardC; i++)
                {
                    Gts.GT_ClrSts(PLC1.card[i].cardNum, 1, PLC1.card[i].axisCount);    //清除各轴报警和限位
                }
                for (int i = 0; i < UserConfig.VacuumPenC; i++)
                {
                    Axis.axisSts_Z[i] = new AxisSts();
                    Axis.axisSts_C[i] = new AxisSts();
                    Axis.Pen[i].Rotate.Busy = false;
                }
                Axis.axisSts_X = new AxisSts();
                Axis.axisSts_Y = new AxisSts();
            }
            Download.Init();
            AutoTray.Init();//自动送盘初始化
            Brede.Init();//编带初始化      
            ZoomLens.Init();//变焦镜头初始化
            g_act.AutoRunShutoff();
        }

        public static void EmergencyStop()
        {          
            if (!In_Output.EMG_Sig.M)//紧急停止
            {
                if (!In_Output.EMG_Sig.FM)
                {
                    In_Output.EMG_Sig.FM = true;
                    ResetState(false);
                    g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "急停按下", "Flow");
                }
                In_Output.EMG_Sig.RM = false;
            }
            else
            {
                if (!In_Output.EMG_Sig.RM)
                {
                    In_Output.EMG_Sig.RM = true;
                    for (int i = 0; i < UserConfig.CardC; i++)
                    {
                        Gts.GT_ClrSts(PLC1.card[i].cardNum, 1, PLC1.card[i].axisCount);    //清除各轴报警和限位
                    }
                    for (int i = 0; i < UserConfig.VacuumPenC; i++)
                    {
                        Axis.axisSts_Z[i] = new AxisSts();
                        Axis.axisSts_C[i] = new AxisSts();
                        Axis.Pen[i].Rotate.Busy = false;
                    }
                    Axis.axisSts_X = new AxisSts();
                    Axis.axisSts_Y = new AxisSts();
                }
                In_Output.EMG_Sig.FM = false;
            }
        }
    }
}
