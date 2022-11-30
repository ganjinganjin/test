using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using BAR.Commonlib;
using BAR.CommonLib_v1._0;

namespace BAR
{
    public class Startup
    {
        public Config g_config = Config.GetInstance();
        public Act g_act = Act.GetInstance();
        public Thread RunSystem = null;
        public static bool[] Condition = new bool[11];
        Axis axis = new Axis();


        /// <summary>
        /// 气阀复位查询
        /// </summary>
        /// <returns></returns>
        public static bool AirValve_Reset_Check()
        {
            int i;

            if (Auto_Flag.Flip && (UserTask.ProgrammerType == GlobConstData.Programmer_WG || UserTask.ProgrammerType == GlobConstData.Programmer_YED))//翻盖
            {
                for (i = 0; i < UserConfig.AllMotionC; i++)
                {
                    if (In_Output.flipO[i].M)
                    {
                        return false;                       
                    }
                }
            }
            else//压座
            {
                for (i = 0; i < UserConfig.AllMotionC; i++)
                {
                    if (In_Output.pushSeatO[i].M)
                    {
                        return false;
                    }
                    if (UserTask.ProgrammerType == GlobConstData.Programmer_RD || (Auto_Flag.Flip && UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY))
                    {
                        if (In_Output.flipO[i].M)
                        {
                            return false;
                        }
                    }
                }
            }

            for (i = 0; i < UserConfig.VacuumPenC; i++)
            {
                if (In_Output.blowO[i].M || In_Output.vacuumO[i].M)
                {

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 气缸状态查询
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static bool AllCylinder_State_Check(bool state)
        {
            int i;

            if (Auto_Flag.Flip && (UserTask.ProgrammerType == GlobConstData.Programmer_WG || UserTask.ProgrammerType == GlobConstData.Programmer_YED))//翻盖
            {
                for (i = 0; i < UserConfig.AllMotionC; i++)
                {
                    if ((In_Output.flipI[i].M && !state) || (!In_Output.flipI[i].M && state))
                    {                      
                        return false;
                    }
                }
            }
            else//压座
            {
                for (i = 0; i < UserConfig.AllMotionC; i++)
                {
                    if ((In_Output.pushSeatI[i].M && !state) || (!In_Output.pushSeatI[i].M && state))
                    {
                        return false;
                    }
                    if (UserTask.ProgrammerType == GlobConstData.Programmer_RD || (Auto_Flag.Flip && UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY))
                    {
                        if ((In_Output.flipI[i].M && !state) || (!In_Output.flipI[i].M && state))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 座子使能查询
        /// </summary>
        /// <returns></returns>
        public static bool DownSeatSelect_Check()
        {
            if (!Auto_Flag.BurnMode)
            {
                return true;
            }
            for (int i = 0; i < UserConfig.ScketGroupC; i++)
            {
                for (int j = 0; j < UserConfig.ScketUnitC; j++)
                {   
                    if (Axis.Group[i].Unit[j].Flag_Open)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 吸笔使能查询
        /// </summary>
        /// <returns></returns>
        public static bool PenEnable_Check()
        {
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                if (Run.PenEnable[i].Checked)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 同步取放查询
        /// </summary>
        /// <returns></returns>
        public static bool SyncTake_Put_Check()
        {
            if (!Auto_Flag.BurnMode)
            {
                return true;
            }
            if (!Config.SyncTakeLay)
            {
                return true;
            }
            if (!Auto_Flag.Enabled_Sync)
            {
                return true;
            }
            int temp = 0;
            int firstPen = 0, endPen = 0;
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                if (Run.PenEnable[i].Checked)
                {
                    if (firstPen == 0)
                    {
                        firstPen = i + 1;
                    }
                    endPen = i + 1;
                    temp++; 
                }
            }
            if (temp < 2 || temp == 4)
            {
                return true;
            }
            else if (temp == 2)
            {
                if (endPen - firstPen == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public void StartupConditionCheck()
        {
            bool temp;

            TrayEndFlag.takeIC[0] = TrayD.TIC_ColN[0] == 0 || TrayD.TIC_RowN[0] == 0;
            TrayEndFlag.takeIC[1] = TrayD.TIC_ColN[1] == 0 || TrayD.TIC_RowN[1] == 0;
            TrayEndFlag.layIC[0] = TrayD.LIC_ColN[0] == 0 || TrayD.LIC_RowN[0] == 0;
            TrayEndFlag.layIC[1] = TrayD.LIC_ColN[1] == 0 || TrayD.LIC_RowN[1] == 0;
            TrayEndFlag.layIC[2] = TrayD.LIC_ColN[2] == 0 || TrayD.LIC_RowN[2] == 0;

            Condition[0] = axis.Axis_XYZ_IsZeroPos_Check();         //检测电机是否在原位
            Condition[1] = AirValve_Reset_Check();                  //检测气阀是否复位
            Condition[2] = AllCylinder_State_Check(true);           //检测压座气缸是否初始位
            Condition[3] = DownSeatSelect_Check();                  //检测烧写座是否有选择
            Condition[4] = !(Auto_Flag.Brede_LayIC && !Brede_Status.Tmp_OK && !Auto_Flag.TestMode); //检测热熔温度
            Condition[5] = !((Auto_Flag.AutoTray_TakeIC || Auto_Flag.AutoTray_LayIC) && !Auto_Flag.AutoTrayReady);//自动盘就绪状态

            if (((!In_Output.tray_Sig[0].M && !In_Output.tray_Sig[1].M) && (Auto_Flag.FixedTray_LayIC || Auto_Flag.FixedTray_TakeIC)) ||
                (!In_Output.tray_Sig[0].M && Auto_Flag.AutoTray_TakeIC && Auto_Flag.AutoTray_LayIC) || (!In_Output.tray_Sig[2].M && Auto_Flag.NGTray))
            {
                Condition[6] = false;   //检测料盘是否放好
            }
            else
            {
                Condition[6] = true;
            }

            if ((TrayEndFlag.layIC[2] && Auto_Flag.NGTray) || (TrayEndFlag.takeLayIC[0] && Auto_Flag.AutoTray_TakeIC && Auto_Flag.AutoTray_LayIC) ||
                (((In_Output.tray_Sig[0].M && TrayEndFlag.takeLayIC[0]) || (In_Output.tray_Sig[1].M && TrayEndFlag.takeLayIC[1])) && (Auto_Flag.FixedTray_TakeIC || Auto_Flag.FixedTray_LayIC)))
            {
                Condition[7] = false;  //检测料盘是是否完成未移出
            }
            else
            {
                Condition[7] = true;
            }

            if (!Auto_Flag.AutoRunBusy)
            {
                Condition[8] = PenEnable_Check();                  //检测吸笔是否有选择
                Condition[9] = SyncTake_Put_Check();               //检测吸笔选择是否满足同取同放
            }

            Condition[10] = !((Config.ZoomLens != 0 && !Axis.ZoomLens_S.Home));//检测变焦镜头是否已回零

            temp = true;
            for (int i = 0; i < 10; i++)
            {
                if(!Condition[i])
                {
                    temp = false;
                    break;
                }
            }
            Auto_Flag.CheckInitOK = temp;
        }

        /// <summary>
        /// 启动信息打印
        /// </summary>
        /// <returns></returns>
        private void StartupMessage()
        {
            string str = null;
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "自动运行开始", "Flow");
            str = MultiLanguage.GetString("使能吸笔号:");
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                if (Run.PenEnable[i].Checked)
                {
                    if (str == MultiLanguage.GetString("使能吸笔号:"))
                    {
                        str += (i + 1);
                    }
                    else
                    {
                        str += "," + (i + 1);
                    }
                }
            }
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, Auto_Flag.TestMode ? "测试模式" : "工作模式", "Flow");
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, Auto_Flag.BurnMode ? "烧录模式" : "转移模式", "Flow");
            if (UserTask.ShiftWay == 5)
            {
                str = MultiLanguage.GetString("管转盘,") + MultiLanguage.GetString(Auto_Flag.AutoTray ? "自动送盘" : "固定盘");
            }
            else if (UserTask.ShiftWay == 4)
            {
                str = MultiLanguage.GetString("管转编");
            }
            else if (UserTask.ShiftWay == 3)
            {
                str = MultiLanguage.GetString("编转盘,") + MultiLanguage.GetString(Auto_Flag.AutoTray ? "自动送盘" : "固定盘");
            }
            else if (UserTask.ShiftWay == 2)
            {
                str = MultiLanguage.GetString("编转编");
            }
            else if (UserTask.ShiftWay == 1)
            {
                str = MultiLanguage.GetString("盘转编,") + MultiLanguage.GetString(Auto_Flag.AutoTray ? "自动送盘" : "固定盘");
            }
            else
            {
                str = MultiLanguage.GetString("盘转盘,") + MultiLanguage.GetString(Auto_Flag.AutoTray ? "自动送盘" : "固定盘");
            }
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, Auto_Flag.NGTray ? "NG盘放NG料" : "NG杯放NG料", "Flow");

            str = MultiLanguage.GetString("取料旋转角度:");
            if (Auto_Flag.AutoTray_TakeIC || Auto_Flag.FixedTray_TakeIC)
            {
                str += MultiLanguage.GetString("料盘1:") + RotateAngle.TIC_Tray[0] + "°    " + MultiLanguage.GetString("料盘2:") + RotateAngle.TIC_Tray[1] + "°";
            }
            else if (Auto_Flag.Brede_TakeIC)
            {
                str += MultiLanguage.GetString("编带:") + RotateAngle.TIC_Brede + "°";
            }
            else if (Auto_Flag.FixedTube_TakeIC)
            {
                str += MultiLanguage.GetString("料管:") + RotateAngle.TIC_Tube + "°";
            }
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");

            str = MultiLanguage.GetString("放料旋转角度:");
            if (Auto_Flag.AutoTray_LayIC || Auto_Flag.FixedTray_LayIC)
            {
                str += MultiLanguage.GetString("料盘1:") + RotateAngle.LIC_Tray[0] + "°    " + MultiLanguage.GetString("料盘2:") + RotateAngle.LIC_Tray[1] + "°    ";
            }
            else if (Auto_Flag.Brede_LayIC)
            {
                str += MultiLanguage.GetString("编带:") + RotateAngle.LIC_Brede + "°    ";
            }
            else if (Auto_Flag.FixedTube_LayIC)
            {
                str += MultiLanguage.GetString("料管:") + RotateAngle.LIC_Tube + "°    ";
            }
            str += MultiLanguage.GetString("NG盘:") + RotateAngle.LIC_Tray[2] + "°" ;
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");

            if (Auto_Flag.Brede_LayIC)
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, Auto_Flag.Brede_Check ? "编带走带检测开" : "编带走带检测关", "Flow");
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, Auto_Flag.Velum_Check ? "编带盖膜检测开" : "编带盖膜检测关", "Flow");
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, Auto_Flag.DotA ? "编带打点开" : "编带打点关", "Flow");
            }

            if (Auto_Flag.Production)
            {
                str = MultiLanguage.GetString("定量生产,") + MultiLanguage.GetString(Auto_Flag.ProductionOK ? "OK数" : "取料数") + MultiLanguage.GetString(",目标数:") + UserTask.TargetC;
            }
            else
            {
                str = MultiLanguage.GetString("非定量生产");
            }
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
            str = MultiLanguage.GetString("OK完成数:") + UserTask.OKDoneC + MultiLanguage.GetString(",取料完成数:") + UserTask.TIC_DoneC;
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
            if (Config.CCDModel == 0)
            {
                str = MultiLanguage.GetString("拍照模式:定拍");
            }
            else
            {
                str = MultiLanguage.GetString("拍照模式:飞拍");
            }
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
        }

        public void StartupInit(bool restart)
        {
            if (Config.CCDModel == 1)//飞拍
            {
                axis.Init_2DCompareMode();
            }
            Auto_Flag.DownLightOn = false;
            Auto_Flag.AutoRunBusy = true;
            Run.ARP_Step = 0;
            if (!Auto_Flag.FixedTray_TakeIC && Auto_Flag.FixedTray_LayIC)
            {
                TrayD.TIC_ColN[0] = 0;
                TrayD.TIC_RowN[0] = 0;
                TrayEndFlag.takeIC[0] = true;

                TrayD.TIC_ColN[1] = 0;
                TrayD.TIC_RowN[1] = 0;
                TrayEndFlag.takeIC[1] = true;
            }
            if (Auto_Flag.FixedTray_TakeIC || Auto_Flag.FixedTray_LayIC)
            {
                if (!In_Output.tray_Sig[0].M)
                {
                    if (Auto_Flag.FixedTray_TakeIC)
                    {
                        TrayD.TIC_TrayN = 2;
                    }
                    if (Auto_Flag.FixedTray_LayIC)
                    {
                        TrayD.LIC_TrayN = 2;
                    }
                    TrayEndFlag.takeIC[0] = true;
                    TrayEndFlag.layIC[0] = true;
                    TrayEndFlag.takeLayIC[0] = true;

                    TrayD.TIC_ColN[0] = 0;
                    TrayD.TIC_RowN[0] = 0;

                    TrayD.LIC_ColN[0] = 1;
                    TrayD.LIC_RowN[0] = 1;
                }
                else if (!In_Output.tray_Sig[1].M)
                {
                    if (Auto_Flag.FixedTray_TakeIC)
                    {
                        TrayD.TIC_TrayN = 1;
                    }
                    if (Auto_Flag.FixedTray_LayIC)
                    {
                        TrayD.LIC_TrayN = 1;
                    }

                    TrayEndFlag.takeIC[1] = true;
                    TrayEndFlag.layIC[1] = true;
                    TrayEndFlag.takeLayIC[1] = true;

                    TrayD.TIC_ColN[1] = 0;
                    TrayD.TIC_RowN[1] = 0;

                    TrayD.LIC_ColN[1] = 1;
                    TrayD.LIC_RowN[1] = 1;
                }
            }

            if (restart)//重新启动
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "重新启动", "Flow");
                if (Auto_Flag.Production)
                {
                    UserTask.TIC_ValidC = 0;
                } 
                UserTask.OKDoneC = 0;    //OK完成数		
                UserTask.TIC_DoneC = 0;  //取料完成数
                UserTask.NGDoneC = 0;    //NG完成数	
                if (Auto_Flag.Brede_LayIC)
                {
                    Brede.CntVal_FrontEmptyMaterial = (short)Brede_Number.CntVal_FrontEmptyMaterial;      //编带前空料设置值
                    Brede.CntVal_CCDEmptyMaterial = (short)Brede_Number.CntVal_CCDEmptyMaterial;      //CCD前空料设置值
                    Brede.CntVal_MarkAEmptyMaterial = (short)Brede_Number.CntVal_MarkAEmptyMaterial;  //编带打点A空料设置值
                    Brede.CntVal_MarkBEmptyMaterial = (short)Brede_Number.CntVal_MarkBEmptyMaterial;  //编带打点B空料设置值
                }
            }
            else
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "记忆启动", "Flow");
                if (Auto_Flag.Production)
                {
                    if(Auto_Flag.ProductionOK)
                    {
                        UserTask.TIC_ValidC = UserTask.OKDoneC;
                    }
                    else
                    {
                        UserTask.TIC_ValidC = UserTask.TIC_DoneC;
                    }
                }
                if (Auto_Flag.Brede_LayIC)
                {
                    if (UserTask.OKDoneC >= Brede_Number.CntVal_FrontEmptyMaterial)
                    {
                        Brede.CntVal_FrontEmptyMaterial = 0;
                    }
                    else
                    {
                        Brede.CntVal_FrontEmptyMaterial = (short)(Brede_Number.CntVal_FrontEmptyMaterial - UserTask.OKDoneC);
                    }

                    if (UserTask.OKDoneC >= Brede_Number.CntVal_CCDEmptyMaterial)
                    {
                        Brede.CntVal_CCDEmptyMaterial = 0;
                    }
                    else
                    {
                        Brede.CntVal_CCDEmptyMaterial = (short)(Brede_Number.CntVal_CCDEmptyMaterial - UserTask.OKDoneC);
                    }

                    if (UserTask.OKDoneC >= Brede_Number.CntVal_MarkAEmptyMaterial)
                    {
                        Brede.CntVal_MarkAEmptyMaterial = 0;
                    }
                    else
                    {
                        Brede.CntVal_MarkAEmptyMaterial = (short)(Brede_Number.CntVal_MarkAEmptyMaterial - UserTask.OKDoneC);
                    }

                    if (UserTask.OKDoneC >= Brede_Number.CntVal_MarkBEmptyMaterial)
                    {
                        Brede.CntVal_MarkBEmptyMaterial = 0;
                    }
                    else
                    {
                        Brede.CntVal_MarkBEmptyMaterial = (short)(Brede_Number.CntVal_MarkBEmptyMaterial - UserTask.OKDoneC);
                    }
                }
            }

            StartupMessage();

            if (Auto_Flag.AutoTray_TakeIC || Auto_Flag.AutoTray_LayIC)
            {
                if (Config.AutoTrayDot == 0)
                {
                    AutoTray.MODBUS_WriteDotParameter();
                }
                Auto_Flag.AutoTray_End = false;
                AutoTray_Process.ReceiveTray = false;
                AutoTray.Init();//自动送盘初始化
                if ((AutoTray.StatusAlarmWord & AutoTray.Status_OverplusTrayInit) != 0 || UserConfig.IsProgrammer)//初始有剩余盘标志
                {
                    if (TrayEndFlag.takeLayIC[1])
                    {
                        AutoTray_Trigger.ReceiveTray = true;   //自动送盘收盘触发标志
                    }
                    else
                    {
                        if ((AutoTray.StatusAlarmWord & AutoTray.Status_TakeTrayDone) != 0 && (AutoTray.StatusAlarmWord & AutoTray.Status_SystemRnunning) != 0)
                        {
                            Run.ARP_Step = 1;
                        }
                        else
                        {
                            AutoTray.TakeTray_Program_Start();
                        }
                    }
                }
                else//初始没有剩余盘标志
                {
                    AutoTray.TakeTray_Program_Start();
                    TrayD.TIC_TrayN = 2;   //取料盘号
                    TrayD.LIC_TrayN = 2;   //放料盘号
                    if (!Auto_Flag.AutoTray_TakeIC && Auto_Flag.AutoTray_LayIC)
                    {
                        TrayD.TIC_ColN[1] = 0;
                        TrayD.TIC_RowN[1] = 0;
                    }
                    else
                    {
                        TrayD.TIC_ColN[1] = 1;
                        TrayD.TIC_RowN[1] = 1;
                    }
                    TrayD.LIC_ColN[1] = 1;
                    TrayD.LIC_RowN[1] = 1;
                    TrayD.TIC_EndColN[1] = g_config.TrayRotateDir[1] != 0 ? TrayD.RowC : TrayD.ColC;
                    TrayD.TIC_EndRowN[1] = g_config.TrayRotateDir[1] != 0 ? TrayD.ColC : TrayD.RowC;
                    TrayEndFlag.takeIC[1] = false;
                    TrayEndFlag.layIC[1] = false;
                    TrayEndFlag.takeLayIC[1] = false;
                    TrayEndFlag.tray2Burn = false;
                }
            }
            else
            {
                if (TrayEndFlag.tray2Burn)
                {
                    TrayEndFlag.tray2Burn = false;
                }
                Run.ARP_Step = 1;
            }
            TrayState.TrayStateUpdate();

            Run run = new Run();
            RunSystem = new Thread(run.Auto_Run_Program);
            RunSystem.IsBackground = true;
            RunSystem.Start();
            Efficiency.Algorithm_Start();
            UserTask.AlarmCount = 0;
            UserTask.StartTime = DateTime.Now;
        }
    }
}
