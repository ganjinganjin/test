using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BAR.Commonlib;
using PLC;

namespace BAR
{
    public class In_Output
    {
        public const int GLinkC = 5;
        static bool Electricity_Flag = false;                                   //上电标志
        static UInt64[] TubeDelay = new UInt64[UserConfig.TubeC];               //料管延迟计时器
        PLC1 plc = new PLC1();
        public static Act g_act = Act.GetInstance();
        #region----------------硬件IO-------------------
        public static GPIO[][] posLimit = new GPIO[2][];
        public static GPIO[][] negLimit = new GPIO[2][];
        public static GPIO[][] alarm = new GPIO[2][];
        public static GPIO[][] home = new GPIO[2][];
        public static GPIO[][] stop = new GPIO[2][];
        public static GPIO[][] SVON = new GPIO[2][];
        public static GPIO[][] EXI = new GPIO[2][];
        public static GPIO[][] EXO = new GPIO[2][];
        public static GPIO[][] EXGLI = new GPIO[GLinkC][];    //gLink扩展IO卡输入
        public static GPIO[][] EXGLO = new GPIO[GLinkC][];    //gLink扩展IO卡输出
        #endregion

        #region----------------软件IO-------------------
        public static GPIO[] penO = new GPIO[UserConfig.VacuumPenC];		//吸笔电磁阀
        public static GPIO[] penHomeI = new GPIO[UserConfig.VacuumPenC];    //吸笔气缸原位
        public static GPIO[] penLimitI = new GPIO[UserConfig.VacuumPenC];   //吸笔气缸限位
        public static GPIO[] vacuumO = new GPIO[UserConfig.VacuumPenC];		//真空电磁阀
        public static GPIO[] vacuumI = new GPIO[UserConfig.VacuumPenC];		//真空检测
        public static GPIO[] blowO = new GPIO[UserConfig.VacuumPenC];		//吹气电磁阀
        public static GPIO[] originZI = new GPIO[UserConfig.VacuumPenC];	//Z轴原点感应

        public static GPIO[] pushSeatO = new GPIO[UserConfig.AllMotionC];	//压座电磁阀
        public static GPIO[] pushSeatI = new GPIO[UserConfig.AllMotionC];	//压座原位检测
        public static GPIO[] flipO = new GPIO[UserConfig.AllMotionC];	    //翻盖电磁阀
        public static GPIO[] flipI = new GPIO[UserConfig.AllMotionC];       //翻盖原位检测
        public static GPIO[] flipLimitI = new GPIO[UserConfig.AllMotionC];  //翻盖限位检测
        public static GPIO[] resetScketO = new GPIO[UserConfig.AllScketC];  //烧录座复位

        public static GPIO[] TubeI = new GPIO[UserConfig.TubeC];            //料管检测
        public static GPIO[] FeederO = new GPIO[UserConfig.FeederC];        //飞达
        public static GPIO[] _3DLightO = new GPIO[2];                       //引脚检测相机光源

        public static GPIO[] tray_Sig = new GPIO[3];    //料盘信号
        public static GPIO[] tray_LED = new GPIO[3];    //料盘指示灯

        public static GPIO redLight = new GPIO();	    //红灯
        public static GPIO yellowLight = new GPIO();	//黄灯
        public static GPIO greenLight = new GPIO();	    //绿灯
        public static GPIO buzzer = new GPIO();	        //蜂鸣器

        public static GPIO EMG_Sig = new GPIO();	    //急停信号总
        public static GPIO EMG_Sig1 = new GPIO();	    //急停信号1
        public static GPIO EMG_Sig2 = new GPIO();	    //急停信号2
        public static GPIO Gate_Sig = new GPIO();	    //门感应信号

        public static GPIO pumpSwitch = new GPIO();		//真空泵开关
        public static GPIO pumpPower = new GPIO();		//真空泵电源

        /// <summary>
        /// 启动按钮
        /// </summary>
        public static GPIO BtnStartI = new GPIO();
        /// <summary>
        /// 暂停按钮
        /// </summary>
        public static GPIO BtnPauseI = new GPIO();
        /// <summary>
        /// 复位按钮
        /// </summary>
        public static GPIO BtnResetI = new GPIO();
        /// <summary>
        /// 暂停按钮指示灯
        /// </summary>
        public static GPIO BtnPauseO = new GPIO();
        /// <summary>
        /// 复位按钮指示灯
        /// </summary>
        public static GPIO BtnResetO = new GPIO();

        /// <summary>
        /// 编带CCD OK
        /// </summary>
        public static GPIO BredeOK_CCD = new GPIO();
        /// <summary>
        /// 编带CCD NG
        /// </summary>
        public static GPIO BredeNG_CCD = new GPIO();
        /// <summary>
        /// 编带CCD触发
        /// </summary>
        public static GPIO BredeStart_CCD = new GPIO();
        /// <summary>
        /// 编带CCD光源气缸
        /// </summary>
        public static GPIO BredeLight_CCD = new GPIO();

        #endregion

        public In_Output()
        {
            if (posLimit[0] == null)
            {
                posLimit[0] = new GPIO[8];
                negLimit[0] = new GPIO[8];
                alarm[0] = new GPIO[8];
                home[0] = new GPIO[8];
                stop[0] = new GPIO[8];
                SVON[0] = new GPIO[8];
                EXI[0] = new GPIO[16];
                EXO[0] = new GPIO[16];

                posLimit[1] = new GPIO[4];
                negLimit[1] = new GPIO[4];
                alarm[1] = new GPIO[4];
                home[1] = new GPIO[4];
                stop[1] = new GPIO[4];
                SVON[1] = new GPIO[4];
                EXI[1] = new GPIO[16];
                EXO[1] = new GPIO[16];

                for (int i = 0; i < GLinkC; i++)
                {
                    EXGLI[i] = new GPIO[16];
                    EXGLO[i] = new GPIO[16];
                    for (int j = 0; j < 16; j++)
                    {
                        EXGLI[i][j] = new GPIO();
                        EXGLO[i][j] = new GPIO();
                    }
                }

                for (int i = 0; i < 8; i++)
                {
                    posLimit[0][i] = new GPIO();
                    negLimit[0][i] = new GPIO();
                    alarm[0][i] = new GPIO();
                    home[0][i] = new GPIO();
                    stop[0][i] = new GPIO();
                    SVON[0][i] = new GPIO();
                }
                for (int i = 0; i < 4; i++)
                {
                    posLimit[1][i] = new GPIO();
                    negLimit[1][i] = new GPIO();
                    alarm[1][i] = new GPIO();
                    home[1][i] = new GPIO();
                    stop[1][i] = new GPIO();
                    SVON[1][i] = new GPIO();
                }
                for (int i = 0; i < 16; i++)
                {
                    EXI[0][i] = new GPIO();
                    EXO[0][i] = new GPIO();
                    EXI[1][i] = new GPIO();
                    EXO[1][i] = new GPIO();
                }
                Config_IO_();
            }
        }

        private void Config_IO_()
        {
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                vacuumO[i] = EXO[0][2 * i];
                vacuumI[i] = EXI[0][i + 8];
                blowO[i] = EXO[0][2 * i + 1];
                originZI[i] = negLimit[0][2 + i];
                penO[i] = EXGLO[0][ 8 + i];
                penHomeI[i] = negLimit[0][3 + i];
                penLimitI[i] = posLimit[0][3 + i];
            }
            if (UserConfig.CardC == 2)
            {
                FeederO[0] = EXO[1][10];
                FeederO[1] = EXO[1][11];
            }
            else
            {
                FeederO[0] = EXGLO[0][12];
                FeederO[1] = EXGLO[0][13];
            }
            
            _3DLightO[0] = EXO[1][12];
            _3DLightO[1] = EXO[1][13];

            tray_LED[0] = EXO[0][8];
            tray_LED[1] = EXO[0][9];
            tray_LED[2] = EXO[0][10];

            pumpPower = EXO[0][11];

            redLight = EXO[0][12];
            yellowLight = EXO[0][13];
            greenLight = EXO[0][14];
            buzzer = EXO[0][15];

            if (UserConfig.CardC == 2)
            {
                EMG_Sig1 = EXI[0][0];
                EMG_Sig2 = home[1][0];
            }
            else
            {
                EMG_Sig1 = EXI[0][0];
                EMG_Sig2 = EXI[0][0];
            }
            Gate_Sig = EXI[0][1];

            pumpSwitch = EXI[0][4];
            tray_Sig[0] = EXI[0][5];
            tray_Sig[1] = EXI[0][6];
            tray_Sig[2] = EXI[0][7];
        }

        /// <summary>
        /// 扫描卡输入口状态
        /// </summary>
        public void ScanCardInput(CardPrm cardPrm)
        {
            plc.GetDInput(Config.CardType, cardPrm.cardNum, 0, 0, posLimit[cardPrm.index], cardPrm.axisCount);
            plc.GetDInput(Config.CardType, cardPrm.cardNum, 1, 0, negLimit[cardPrm.index], cardPrm.axisCount);
            plc.GetDInput(Config.CardType, cardPrm.cardNum, 2, 0, alarm[cardPrm.index], cardPrm.axisCount);
            plc.GetDInput(Config.CardType, cardPrm.cardNum, 3, 0, home[cardPrm.index], cardPrm.axisCount);
            plc.GetDInput(Config.CardType, cardPrm.cardNum, 5, 0, stop[cardPrm.index], cardPrm.axisCount);
            plc.GetDInput(Config.CardType, cardPrm.cardNum, 4, 0, EXI[cardPrm.index], cardPrm.GPIOCount);
        }

        /// <summary>
        /// 扫描gLink扩展卡输入口状态
        /// </summary>
        public void ScanGLinkCardInput(CardPrm cardPrm, short mdl)
        {
            plc.GetExtInput(Config.CardType, cardPrm.cardNum, mdl, 0, EXGLI[mdl], 16);
        }

        /// <summary>
        /// 写卡输出端口的状态
        /// </summary>
        public void WriteCardOutput(CardPrm cardPrm)
        {
            int value = 0;
            for (int i = 0; i < cardPrm.GPIOCount; i++)
            {
                if (!EXO[cardPrm.index][i].M)
                {
                    value = value | (0x1 << i);
                }
            }
            if (Config.CardType == 0)
            {
                Gts.GT_SetDo(cardPrm.cardNum, 12, value);
            }
            else
            {
                lhmtc.LH_SetDo(12, value, cardPrm.cardNum);
            }
            
        }

        /// <summary>
        /// 写gLink卡输出端口的状态
        /// </summary>
        public void WriteGLinkCardOutput(CardPrm cardPrm, short mdl)
        {
            int value = 0;
            for (int i = 0; i < 16; i++)
            {
                if (!EXGLO[mdl][i].M)
                {
                    value = value | (0x1 << i);
                }
            }
            if (Config.CardType == 0)
            {
                Gts.GT_SetExtIoValue(cardPrm.cardNum, mdl, (ushort)value);
            }
            else
            {
                lhmtc.LH_SetExtendDo(mdl, (ushort)value, cardPrm.cardNum);
            }
            
        }

        public void ScanCardOutput(CardPrm cardPrm)
        {
            plc.GetOutput(Config.CardType, cardPrm.cardNum, 10, 1, SVON[cardPrm.index], cardPrm.axisCount);
        }

        /// <summary>
        /// 打开烧录座到位延时
        /// </summary>
        public void OpenSocket_InPlaceDelay()
        {
            for (int i = 0; i < UserConfig.AllMotionC; i++)
            {
                if (pushSeatO[i].RM)
                {
                    if(Auto_Flag.AutoRunBusy || Auto_Flag.LearnBusy || Auto_Flag.GOBusy)
                    {
                        if (!pushSeatI[i].M)
                        {
                            pushSeatO[i].RM = false;
                            AutoTimer.SeatTakeDelay = UserTimer.GetSysTime() + AutoTiming.SeatTakeDelay;
                            pushSeatI[i].FM = true;
                        }
                    }
                    else
                    {
                        pushSeatO[i].RM = false;
                    } 
                }
                if (!pushSeatO[i].M && pushSeatI[i].FM)
                {
                    pushSeatI[i].FM = false;
                }
            }
        }

        /// <summary>
        /// 料管到位延时
        /// </summary>
        public void TubeExistIC_InPlaceDelay()
        {
            if(!Electricity_Flag)
            {
                Electricity_Flag = true;
                for (int i = 0; i < UserConfig.TubeC; i++)
                {
                    UserTask.ExistIC_Tube[i] = TubeI[i].M;
                }
            }
            else
            {
                for (int i = 0; i < UserConfig.TubeC; i++)
                {
                    if (TubeI[i].M)
                    {
                        if (!TubeI[i].RM)
                        {
                            TubeI[i].RM = true;
                            TubeDelay[i] = UserTimer.GetSysTime() + AutoTiming.TubeTakeDelay;
                        }
                        if (UserTimer.GetSysTime() > TubeDelay[i])
                        {
                            UserTask.ExistIC_Tube[i] = true;
                        }
                    }
                    else
                    {
                        TubeI[i].RM = false;
                        UserTask.ExistIC_Tube[i] = false;
                    }
                }
            }
        }

        /// <summary>
        /// 飞达延时关闭
        /// </summary>
        public void FeederDelayOff()
        {
            if (Config.FeederCount != 1)
            {
                if (UserTask.FeederIO)
                {
                    if(FeederO[0].M && UserTimer.GetSysTime() > Axis.Feeder[0].FeederOffDelay)
                    {
                        FeederO[0].M = false;
                    }
                }
                return;
            }
            for (int i = 0; i < UserConfig.FeederC; i++)
            {
                if (FeederO[i].M && UserTimer.GetSysTime() > Axis.Feeder[i].FeederOffDelay)
                {
                    FeederO[i].M = false;
                }
            }
        }

        /// <summary>
        /// 编带CCD延时关闭
        /// </summary>
        public void BredeDelayOff_CCD()
        {
            //if (Auto_Flag.BredeCCD_Check && BredeStart_CCD.M)
            //{
            //    if (UserTimer.GetSysTime() > AutoTimer.BredeCCDOffDelay)
            //    {
            //        BredeStart_CCD.M = false;
            //        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "编带CCD复位", "Flow");
            //    }
            //}
        }
    }
}
