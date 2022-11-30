using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BAR.Commonlib;
using PLC;

namespace BAR
{ 
    public class Axis : UserTimer
    {
        #region----------------声明-------------------
        // 定义一个静态变量来保存类的实例
        private static Axis _instance = null;
        // 定义一个标识确保线程同步
        private static readonly object _objPadLock = new object();

        public static Act g_act = Act.GetInstance();
        public Config g_config = Config.GetInstance();
        PLC1 plc = new PLC1();
        In_Output in_Output = new In_Output();
        public static int AHZI_Step;
        public static int AHAI_Step;

        public static Position Position_X = new Position();
        public static Position Position_Y = new Position();
        public static RunPrm runPrm_X = new RunPrm();
        public static RunPrm runPrm_Y = new RunPrm();
        public static RunPrm runPrm_Z = new RunPrm();

        public static LimitHomePrm homePrm_X = new LimitHomePrm();
        public static LimitHomePrm homePrm_Y = new LimitHomePrm();
        public static LimitHomePrm homePrm_U = new LimitHomePrm();
        public static LimitHomePrm[] homePrm_Z = new LimitHomePrm[UserConfig.VacuumPenC];
        
        public static TrapPrm trapPrm_X = new TrapPrm();
        public static TrapPrm trapPrm_Y = new TrapPrm();
        public static TrapPrm trapPrm_U = new TrapPrm();
        public static TrapPrm[] trapPrm_Z = new TrapPrm[UserConfig.VacuumPenC];
        public static TrapPrm[] trapPrm_C = new TrapPrm[UserConfig.VacuumPenC];
        
        public static JogPrm jogPrm_X = new JogPrm();
        public static JogPrm jogPrm_Y = new JogPrm();
        public static JogPrm jogPrm_U = new JogPrm();
        public static JogPrm[] jogPrm_Z = new JogPrm[UserConfig.VacuumPenC];

        public static AxisSts axisSts_X = new AxisSts();
        public static AxisSts axisSts_Y = new AxisSts();
        public static AxisSts axisSts_U = new AxisSts();
        public static AxisSts[] axisSts_Z = new AxisSts[UserConfig.VacuumPenC];
        public static AxisSts[] axisSts_C = new AxisSts[UserConfig.VacuumPenC];
        public static VacuumPen_Struct[] Pen = new VacuumPen_Struct[UserConfig.VacuumPenC];
        public static SyncPen_Struct[] SyncPen = new SyncPen_Struct[UserConfig.VacuumPenC];
        public static Feeder_Struct[] Feeder = new Feeder_Struct[UserConfig.FeederC];
        public static Scanner_Struct Scanner = new Scanner_Struct();
        public static Altimeter_Struct Altimeter = new Altimeter_Struct();
        public static ZoomLens_Struct ZoomLens_S = new ZoomLens_Struct();
        public static SocketGroup[] Group = new SocketGroup[UserConfig.ScketGroupC];
        public static PenRectify[] rectify = new PenRectify[UserConfig.VacuumPenC];
        #endregion

        public Axis()
        {
            if (homePrm_Z[0] == null)
            {
                for (int i = 0; i < UserConfig.ScketGroupC; i++)
                {
                    Group[i] = new SocketGroup
                    {
                        GroupNum = i
                    };
                }

                for (int i = 0; i < UserConfig.VacuumPenC; i++)
                {
                    homePrm_Z[i] = new LimitHomePrm();
                    trapPrm_Z[i] = new TrapPrm();
                    trapPrm_C[i] = new TrapPrm();
                    jogPrm_Z[i] = new JogPrm();
                    axisSts_Z[i] = new AxisSts();
                    axisSts_C[i] = new AxisSts();
                }
            }
        }

        public static Axis GetInstance()
        {
            if (_instance == null)
            {
                lock (_objPadLock)
                {
                    if (_instance == null)
                    {
                        _instance = new Axis();
                    }
                }
            }
            return _instance;
        }

        public static void Home_Start()
        {
            Position_X.Buffer = 0;
            Position_Y.Buffer = 0;
            if (!Auto_Flag.HomeBusy)
            {
                if (Config.ZoomLens!=0)
                {
                    ZoomLens.Init_Program_Start();
                }
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "回原点启动", "Flow");
                Auto_Flag.HomeBusy = true;
                AHAI_Step = 1;
                AHZI_Step = 0;
            }
        }

        #region---------XYZ轴回原点操作-----------
        /// <summary>
        /// XYZ回原点
        /// </summary>
        public void AxisHomeAll_Init()
        {
            int temp;
            if (!Auto_Flag.HomeBusy)
            {
                return;
            }
            switch (AHAI_Step)
            {
                case 1:
                    if (UserTask.PenType != 1)
                    {
                        AHAI_Step = 3;
                    }
                    else
                    {
                        for (int i = 0; i < UserConfig.VacuumPenC; i++)
                        {
                            In_Output.penO[i].M = false;
                        }
                        AutoTimer.OriginZ_Check = GetSysTime() + 3000;
                        AHAI_Step = 2;
                    }
                    break;

                case 2:
                    if (PenHome_Check())
                    {
                        AHAI_Step = 3;
                    }
                    else if (GetSysTime() > AutoTimer.OriginZ_Check)
                    {
                        Auto_Flag.HomeBusy = false;
                        //g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "吸笔原位检测超时，回原点失败", "Flow");
                        BAR._ToolTipDlg.WriteToolTipStr("吸笔原位检测超时,回原点失败");
                        BAR.ShowToolTipWnd(false);
                        AHAI_Step = 0;
                    }
                    break;

                case 3:
                    temp = Axis_Z_Home_Init();
                    if (temp == 2)
                    {
                        AHAI_Step = 4;
                        AutoTray.Init_Program_Start();
                    }
                    else if (temp == 1 || temp == 3)
                    {
                        Auto_Flag.HomeBusy = false;
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "回原点失败", "Flow");
                        AHAI_Step = 0;
                    }
                    break;

                case 4:
                    ORG_Function(homePrm_X, axisSts_X);
                    ORG_Function(homePrm_Y, axisSts_Y);
                    AHAI_Step = 5;
                    break;

                case 5:
                    if (axisSts_X.isHome && axisSts_Y.isHome)
                    {
                        AHAI_Step = 6;
                        if (Config.CCDModel == 1)//飞拍
                        {
                            Init_2DCompareMode();
                        }
                    }
                    break;

                case 6:
                    g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "回原点成功", "Flow");
                    Auto_Flag.HomeBusy = false;
                    AHAI_Step = 0;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Z轴回原点
        /// </summary>
        /// <returns></returns>
        public int Axis_Z_Home_Init()
        {
            int ret = 0;
            switch (AHZI_Step)
            {
                case 0:
                    for (int i = 0; i < UserConfig.AxisZC; i ++)
                    {
                        if (UserTask.PenType == 1 || Run.PenEnable[i].Checked || (!Run.PenEnable[i].Checked && !Auto_Flag.PenAbnormal))
                        {
                            ORG_Function(homePrm_Z[i], axisSts_Z[i]);
                        } 
                    }
                    AHZI_Step = 1;
                    break;

                case 1:
                    if (Axis_Z_IsFree_Check())
                    {
                        if(Axis_Z_IsHome_Check())
                        {
                            AHZI_Step = 2;
                            AutoTimer.OriginZ_Check = GetSysTime() + 3000;
                        }
                        else
                        {
                            AHZI_Step = 0;
                            ret = 1;//回原点失败
                        }
                    }
                    break;

                case 2:
                    if (OriginZ_Check())
                    {
                        for (int i = 0; i < UserConfig.VacuumPenC; i++)
                        {
                            Gts.GT_ClrSts(trapPrm_C[i].cardPrm.cardNum, trapPrm_C[i].index, 1);
                            Gts.GT_ZeroPos(trapPrm_C[i].cardPrm.cardNum, trapPrm_C[i].index, 1);
                        }
                        ret = 2;//回原点成功
                        AHZI_Step = 0;
                    }
                    else if (GetSysTime() > AutoTimer.OriginZ_Check)
                    {
                        BAR._ToolTipDlg.WriteToolTipStr("Z轴未在安全检测高度");
                        BAR.ShowToolTipWnd(false);
                        ret = 3;
                        AHZI_Step = 0;
                    }                  
                    break;

                default:
                    break;
            }
            return ret;          
        }

        /// <summary>
        /// 单轴回原点
        /// </summary>
        public void ORG_Function(LimitHomePrm homePrm, AxisSts axisSts)
        {
            axisSts.isHome = false;
            axisSts.isBusy = false;
            axisSts.isDone = false;
            axisSts.isHomeBusy = true;
            //Task t1 = Task.Factory.StartNew(() =>
            //          HomeReturn(homePrm, axisSts));//开启检测线程
            Thread homeTd = new Thread(() => HomeReturn(homePrm, axisSts));
            homeTd.IsBackground = true;
            homeTd.Start();
        }
        public void HomeReturn(LimitHomePrm homePrm, AxisSts axisSts)
        {
            try
            {
                bool result = plc.SmartHome(Config.CardType, homePrm);
                g_act.WaitDoEvent(10);
                if (result)
                {
                    axisSts.isHome = true;
                }
                else
                {
                    MessageBox.Show(homePrm.strMessage + "轴回零失败", "温馨提示：",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                axisSts.isHomeBusy = false;
            }
            catch (System.IO.IOException ex)
            {
                axisSts.isHomeBusy = false;
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 所有轴停止
        /// </summary>
        public void AxisStopAll()
        {
            if (Run.ARP_Step == 100) 
            {
                return;
            }
            AxisStop(trapPrm_X, axisSts_X);
            AxisStop(trapPrm_Y, axisSts_Y);
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                AxisStop(trapPrm_Z[i], axisSts_Z[i]);
            }
        }

        /// <summary>
        /// 吸笔轴停止
        /// </summary>
        public void AxisZStopAll()
        {
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                AxisStop(trapPrm_Z[i], axisSts_Z[i]);
            }
        }

        /// <summary>
        /// 轴停止
        /// </summary>
        public void AxisStop(TrapPrm trapPrm, AxisSts axisSts)
        {
            double temp;
            if (trapPrm.index == 1)
            {
                temp = 30;
            }
            else
            {
                temp = 10;
            }
            Gts.GT_SetStopDec(trapPrm.cardPrm.cardNum, trapPrm.index, temp, temp);
            Gts.GT_Stop(trapPrm.cardPrm.cardNum, 1 << (trapPrm.index - 1), 0);
            axisSts.isBusy = false;
            axisSts.isDone = false;
        }

        /// <summary>
        /// 初始化2维比较模式
        /// </summary>
        public void Init_2DCompareMode()
        {
            g_act.CCDSnap(GlobConstData.ST_CCDDOWN);
            Gts.GT_2DCompareMode(PLC1.card[0].cardNum, 0, Gts.COMPARE2D_MODE_2D); //启动二维比较输出模块，为二维位置比较方式
            Gts.T2DComparePrm Prm; //二维位置比较输出参数
            Prm.encx = 1; // X 轴为轴
            Prm.ency = 2; // Y 轴为轴
            Prm.maxerr = 140; // 最大误差Pulse
            Prm.outputType = 0; // 输出类型脉冲
            Prm.source = 1; // 比较源规划器
            Prm.startLevel = 0; // 无效参数
            Prm.threshold = 0; //该参数设置为0
            Prm.time = 500; // 脉冲宽度 us
            Gts.GT_2DCompareSetPrm(PLC1.card[0].cardNum, 0, ref Prm);
            Gts.GT_2DCompareClear(PLC1.card[0].cardNum, 0); //清除位置比较输出数据
            Gts.GT_2DCompareStart(PLC1.card[0].cardNum, 0); //启动位置比较输出

        }

        /// <summary>
        /// 定位控制
        /// </summary>
        /// <param name="mode">true:绝对控制，false:相对控制</param>
        /// <param name="trapPrm"></param>
        /// <param name="axisSts"></param>
        public void DST_Function(bool mode, TrapPrm trapPrm, AxisSts axisSts)
        {
            if (Auto_Flag.Pause)
            {
                return;
            }
            if (UserConfig.IsProgrammer)
            {
                axisSts.isBusy = false;
                axisSts.isDone = true;
                return;
            }
            if (axisSts.isDone)
            {
                return;
            }

            if (!axisSts.isBusy)
            {
                if (!mode)
                {
                    if(trapPrm.setPos == 0)
                    {
                        axisSts.isDone = true;
                        return;
                    }
                }
                else
                {
                    if (trapPrm.setPos == trapPrm.getPos)
                    {
                        axisSts.isDone = true;
                        return;
                    }
                }
                trapPrm.setPosPul = (int)(trapPrm.setPos / trapPrm.pulFactor);
                if (!mode)
                {
                    trapPrm.setPosPul = trapPrm.getPosPul + trapPrm.setPosPul;
                }
                plc.UpdateTrap(Config.CardType, trapPrm);
                axisSts.isBusy = true;
            }
            if ((trapPrm.index == 1 || trapPrm.index == 2) && (PLC1.card[0].cardNum == 0 && trapPrm.cardPrm.cardNum == 0 || PLC1.card[0].cardNum == 1 && trapPrm.cardPrm.cardNum == 1))
            {
                if (trapPrm.setPosPul >= trapPrm.getEncPosPul - 10 && trapPrm.setPosPul <= trapPrm.getEncPosPul + 10)
                {
                    if (axisSts.stop)
                    {
                        axisSts.isBusy = false;
                        axisSts.isDone = true;
                    }
                    return;
                }
            }
            else if (trapPrm.setPosPul == trapPrm.getPosPul)
            {
                if (axisSts.stop)
                {
                    axisSts.isBusy = false;
                    axisSts.isDone = true;
                }        
                return;
            }
        }

        public void DST_Function_ZL(double PosPul)
        {
            if (ZoomLens_S.IsDone)
            {
                return;
            }
            //ZoomLens.SetPos = pos;
            if (!ZoomLens_S.IsBusy)
            {
                if (PosPul == ZoomLens.NowPos)
                {
                    ZoomLens_S.IsDone = true;
                    return;
                }
                ZoomLens.MODBUS_DST();
                ZoomLens_S.IsBusy = true;
            }
            if (PosPul > ZoomLens.NowPos - 25 && PosPul < ZoomLens.NowPos + 25)
            {
                ZoomLens_S.IsBusy = false;
                ZoomLens_S.IsDone = true;
                return;
            }
        }

        /// <summary>
        /// Z轴原点信号
        /// </summary>
        /// <returns></returns>
        public bool OriginZ_Check()
        {
	        for(int i = 0; i < UserConfig.AxisZC; i ++)
	        {		
		        if(!In_Output.originZI[i].M)
		        {
                    if (UserTask.PenType == 1 || Run.PenEnable[i].Checked || (!Run.PenEnable[i].Checked && !Auto_Flag.PenAbnormal))
                    {
                        return false;
                    }
                }
	        }
            return true;
        }

        /// <summary>
        /// 吸笔原位信号
        /// </summary>
        /// <returns></returns>
        public static bool PenHome_Check()
        {
            if (UserTask.PenType != 1)
            {
                return true;
            }
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                if (!In_Output.penHomeI[i].M)
                {
                    if (Run.PenEnable[i].Checked || (!Run.PenEnable[i].Checked && !Auto_Flag.PenAbnormal))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Z轴在安全位置
        /// </summary>
        /// <returns></returns>
        public static bool Axis_Z_IsSafe_Check()
        {
	        for(int i = 0; i < UserConfig.AxisZC; i ++)
	        {
		        if(!axisSts_Z[i].isSafe)
		        {
                    if (UserTask.PenType == 1 || Run.PenEnable[i].Checked || (!Run.PenEnable[i].Checked && !Auto_Flag.PenAbnormal))
                    {
                        return false;
                    }
                }
	        }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Axis_Z_IsZeroPos_Check()
        {
            for (int i = 0; i < UserConfig.AxisZC; i ++)
            {
                if (!axisSts_Z[i].isZeroPos)
                {
                    if (UserTask.PenType == 1 || Run.PenEnable[i].Checked || (!Run.PenEnable[i].Checked && !Auto_Flag.PenAbnormal))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Axis_XYZ_IsZeroPos_Check()
        {
            if (Axis_Z_IsZeroPos_Check() && axisSts_X.isZeroPos && axisSts_Y.isZeroPos)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Axis_Z_IsHome_Check()
        {
            for (int i = 0; i < UserConfig.AxisZC; i++)
            {
                if (!axisSts_Z[i].isHome)
                {
                    if (UserTask.PenType == 1 || Run.PenEnable[i].Checked || (!Run.PenEnable[i].Checked && !Auto_Flag.PenAbnormal))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool Axis_Z_IsFree_Check()
        {
            for (int i = 0; i < UserConfig.AxisZC; i++)
            {
                if (axisSts_Z[i].isHomeBusy)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 水平位置控制
        /// </summary>
        public void Horizontal_Position_Control(Double setPos_X, Double setPos_Y)
        {
            if (UserConfig.IsProgrammer)
            {
                Auto_Flag.Run_InPlace = true;
                return;
            }
            if (!Pen_RTCheck())
            {
                return;
            }
            if (!axisSts_X.isBusy)
            {
                trapPrm_X.setPos = setPos_X;
                trapPrm_X.speed = runPrm_X.speed;
                if (Math.Abs(setPos_X - trapPrm_X.getPos) > runPrm_X.longMin)
                {
                    trapPrm_X.acc = runPrm_X.longAcc;
                    trapPrm_X.dec = runPrm_X.longDec;
                    trapPrm_X.velStart = 0;
                }
                else
                {
                    if (runPrm_X.speed > 1200)
                    {
                        trapPrm_X.speed = 1200;
                    }
                    trapPrm_X.acc = runPrm_X.shortAcc;
                    trapPrm_X.dec = runPrm_X.shortDec;
                    trapPrm_X.velStart = 0;
                }

                trapPrm_Y.setPos = setPos_Y;
                trapPrm_Y.speed = runPrm_Y.speed;
                if (Math.Abs(setPos_Y - trapPrm_Y.getPos) > runPrm_Y.longMin)
                {
                    trapPrm_Y.acc = runPrm_Y.longAcc;
                    trapPrm_Y.dec = runPrm_Y.longDec;
                    trapPrm_Y.velStart = 0;
                }
                else
                {
                    if (runPrm_Y.speed > 1200)
                    {
                        trapPrm_Y.speed = 1200;
                    }
                    trapPrm_Y.acc = runPrm_Y.shortAcc;
                    trapPrm_Y.dec = runPrm_Y.shortDec;
                    trapPrm_Y.velStart = 0;
                }
            }             
            DST_Function(true, trapPrm_X, axisSts_X);
            DST_Function(true, trapPrm_Y, axisSts_Y);
            if (axisSts_X.isDone && axisSts_Y.isDone)//轴XY定位控制
            {
                axisSts_X.isDone = false;
                axisSts_Y.isDone = false;
                Auto_Flag.Run_InPlace = true;
            }
        }

        /// <summary>
        /// 水平X位置控制
        /// </summary>
        public void Horizontal_X_Position_Control(Double setPos_X, int penNum)
        {
            if (UserConfig.IsProgrammer)
            {
                Auto_Flag.Run_InPlace = true;
                return;
            }
            if (!Pen_RTCheck())
            {
                return;
            }
            if (!axisSts_X.isBusy)
            {
                trapPrm_X.setPos = setPos_X;
                trapPrm_X.speed = runPrm_X.speed;
                double temp = 500;
                if (penNum > 1)
                {
                    if (Config.CameraType == GlobConstData.Camera_DH)
                    {
                        temp = 600;
                    }
                    else
                    {
                        if (GlobConstData.IMG130_WIDTH > 720)
                        {
                            temp = 300;
                        }
                        else
                        {
                            temp = 500;
                        }
                    }
                }
                if (runPrm_X.speed > temp)
                {
                    trapPrm_X.speed = temp;
                }
                if (Math.Abs(setPos_X - trapPrm_X.getPos) > runPrm_X.longMin)
                {
                    trapPrm_X.acc = runPrm_X.longAcc;
                    trapPrm_X.dec = runPrm_X.longDec;
                    trapPrm_X.velStart = 0;
                }
                else
                {
                    trapPrm_X.acc = runPrm_X.shortAcc;
                    trapPrm_X.dec = runPrm_X.shortDec;
                    trapPrm_X.velStart = 0;
                }
            }
            DST_Function(true, trapPrm_X, axisSts_X);
            if (axisSts_X.isDone)//轴X定位控制
            {
                axisSts_X.isDone = false;
                Auto_Flag.Run_InPlace = true;
            }
        }

        public void Horizontal_Position_Control(Double setPos_X, Double setPos_Y, double speed)
        {
            if (UserConfig.IsProgrammer)
            {
                Auto_Flag.Run_InPlace = true;
                return;
            }
            if (!Pen_RTCheck())
            {
                return;
            }
            if (!axisSts_X.isBusy)
            {
                trapPrm_X.setPos = setPos_X;
                trapPrm_X.speed = speed;
                trapPrm_X.acc = homePrm_X.acc;
                trapPrm_X.dec = homePrm_X.dec;

                trapPrm_Y.setPos = setPos_Y;
                trapPrm_Y.speed = speed;
                trapPrm_Y.acc = homePrm_Y.acc;
                trapPrm_Y.dec = homePrm_Y.dec;
            }
            DST_Function(true, trapPrm_X, axisSts_X);
            DST_Function(true, trapPrm_Y, axisSts_Y);
            if (axisSts_X.isDone && axisSts_Y.isDone)//轴XY定位控制
            {
                axisSts_X.isDone = false;
                axisSts_Y.isDone = false;
                Auto_Flag.Run_InPlace = true;
            }
        }

        /// <summary>
        /// 轴X定位控制
        /// </summary>
        /// <param name="setPos_X"></param>
        /// <param name="setPos_Y"></param>
        /// <param name="speed"></param>
        public void Horizontal_X_Position_Control(Double setPos_X)
        {
            if (UserConfig.IsProgrammer)
            {
                Auto_Flag.Run_InPlace = true;
                return;
            }
            if (!Pen_RTCheck())
            {
                return;
            }
            if (!axisSts_X.isBusy)
            {
                trapPrm_X.setPos = setPos_X;
                trapPrm_X.speed = runPrm_X.speed;
                if (Math.Abs(setPos_X - trapPrm_X.getPos) > runPrm_X.longMin)
                {
                    trapPrm_X.acc = runPrm_X.longAcc;
                    trapPrm_X.dec = runPrm_X.longDec;
                    trapPrm_X.velStart = 0;
                }
                else
                {
                    if (runPrm_X.speed > 1200)
                    {
                        trapPrm_X.speed = 1200;
                    }
                    trapPrm_X.acc = runPrm_X.shortAcc;
                    trapPrm_X.dec = runPrm_X.shortDec;
                    trapPrm_X.velStart = 0;
                }
            }
            DST_Function(true, trapPrm_X, axisSts_X);
            if (axisSts_X.isDone)//轴X定位控制
            {
                axisSts_X.isDone = false;
                Auto_Flag.Run_InPlace = true;
            }
        }

        /// <summary>
        /// 轴Y定位控制
        /// </summary>
        /// <param name="setPos_X"></param>
        /// <param name="setPos_Y"></param>
        /// <param name="speed"></param>
        public void Horizontal_Y_Position_Control(Double setPos_Y)
        {
            if (UserConfig.IsProgrammer)
            {
                Auto_Flag.Run_InPlace = true;
                return;
            }
            if (!Pen_RTCheck())
            {
                return;
            }
            if (!axisSts_Y.isBusy)
            {
                trapPrm_Y.setPos = setPos_Y;
                trapPrm_Y.speed = runPrm_Y.speed;
                if (Math.Abs(setPos_Y - trapPrm_Y.getPos) > runPrm_Y.longMin)
                {
                    trapPrm_Y.acc = runPrm_Y.longAcc;
                    trapPrm_Y.dec = runPrm_Y.longDec;
                    trapPrm_Y.velStart = 0;
                }
                else
                {
                    if (runPrm_Y.speed > 1200)
                    {
                        trapPrm_Y.speed = 1200;
                    }
                    trapPrm_Y.acc = runPrm_Y.shortAcc;
                    trapPrm_Y.dec = runPrm_Y.shortDec;
                    trapPrm_Y.velStart = 0;
                }
            }
            DST_Function(true, trapPrm_Y, axisSts_Y);
            if (axisSts_Y.isDone)//轴Y定位控制
            {
                axisSts_Y.isDone = false;
                Auto_Flag.Run_InPlace = true;
            }
        }

        public void Camera_Position_Control(Double setPos_U, bool IsManual = false, double speed = 50)
        {
            if (UserConfig.IsProgrammer)
            {
                Auto_Flag.Camera_InPlace = true;
                return;
            }
            Gts.GT_ClrSts(Axis.jogPrm_U.cardPrm.cardNum, Axis.jogPrm_U.index, 1);//清除轴报警
            if (IsManual)
            {
                axisSts_U.isBusy = false;
                axisSts_U.isDone = false;
                Auto_Flag.Camera_InPlace = false;
            }
            if (!axisSts_U.isBusy)
            {
                trapPrm_U.setPos = setPos_U;
                trapPrm_U.speed = speed;
                trapPrm_U.acc = homePrm_U.acc;
                trapPrm_U.dec = homePrm_U.dec;

            }
            DST_Function(true, trapPrm_U, axisSts_U);
            if (axisSts_U.isDone)//轴U定位控制
            {
                axisSts_U.isDone = false;
                Auto_Flag.Camera_InPlace = true;
            }
        }

        /// <summary>
        /// 实时检测吸笔安全位
        /// </summary>
        /// <returns></returns>
        public bool Pen_RTCheck()
        {
            AxisZStopAll();
            if (UserTask.PenType == 1)
            {
                return true;
            }
            if (!(OriginZ_Check() && PenHome_Check()))
            {
                plc.StopAxis(Config.CardType);
                Auto_Flag.AutoRunBusy = false;
                Auto_Flag.LearnBusy = false;
                g_act.AutoRunShutoff();
                BAR._ToolTipDlg.WriteToolTipStr("吸笔异常动作脱离安全位置,已退运行");
                BAR.ShowToolTipWnd(true);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 垂直位置控制
        /// </summary>
        /// <param name="AxisNum"></param>
        /// <param name="height"></param>
        public void Vertical_Position_Control(int penN, double height, double speed, bool on = false)
        {
            if (UserConfig.IsProgrammer)
            {
                Auto_Flag.Run_InPlace = true;
                return;
            }
            if (UserTask.PenType == 1)
            {
                In_Output.penO[penN].M = on;
                penN = 0;
            }
            if (!axisSts_Z[penN].isBusy)
            {
                AutoTimer.OriginZ_Check = GetSysTime() + 3000;
                trapPrm_Z[penN].setPos = height;
                trapPrm_Z[penN].speed = speed;
                trapPrm_Z[penN].acc = homePrm_Z[0].acc;
                trapPrm_Z[penN].dec = homePrm_Z[0].dec;
            }          
            DST_Function(true, trapPrm_Z[penN], axisSts_Z[penN]);
            if (axisSts_Z[penN].isDone)
            {
                axisSts_Z[penN].isDone = false;
                Auto_Flag.Run_InPlace = true;
            }
        }

        public void Vertical_Position_Control(int penN, double height, bool on = false)
        {
            if (UserConfig.IsProgrammer)
            {
                Auto_Flag.Run_InPlace = true;
                return;
            }
            if (UserTask.PenType == 1)
            {
                In_Output.penO[penN].M = on;
                penN = 0;
            }
            if (!axisSts_Z[penN].isBusy)
            {
                AutoTimer.OriginZ_Check = GetSysTime() + 3000;
                trapPrm_Z[penN].setPos = height;
                trapPrm_Z[penN].speed = runPrm_Z.speed;
                trapPrm_Z[penN].acc = runPrm_Z.longAcc;
                trapPrm_Z[penN].dec = runPrm_Z.longDec;
            }
            DST_Function(true, trapPrm_Z[penN], axisSts_Z[penN]);
            if (axisSts_Z[penN].isDone)
            {
                axisSts_Z[penN].isDone = false;
                Auto_Flag.Run_InPlace = true;
            }
        }

        /// <summary>
        /// 全部吸笔回安全高度
        /// </summary>
        /// <returns></returns>
        public bool HeightSafe_Handle()
        {
            if (UserConfig.IsProgrammer)
            {
                return true;
            }
            short i, inPlaceN = 0, workAxisC = 0;
            for (i = 0; i < UserConfig.AxisZC; i ++)
            {
                if (!axisSts_Z[i].isBusy)
                {
                    trapPrm_Z[i].setPos = HeightVal.Safe;
                    trapPrm_Z[i].speed = runPrm_Z.speed;
                    trapPrm_Z[i].acc = runPrm_Z.longAcc;
                    trapPrm_Z[i].dec = runPrm_Z.longDec;
                }
                if (UserTask.PenType == 1 || Run.PenEnable[i].Checked || (!Run.PenEnable[i].Checked && !Auto_Flag.PenAbnormal))
                {
                    DST_Function(true, trapPrm_Z[i], axisSts_Z[i]);
                    workAxisC++;
                }
                if (axisSts_Z[i].isDone)
                {
                    inPlaceN ++;
                }
            }
            if (inPlaceN == workAxisC)
            {
                for (i = 0; i < UserConfig.AxisZC; i ++)
                {
                    axisSts_Z[i].isDone = false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 吸笔旋转处理
        /// </summary>
        public void VacuumPenRotate_Handle()
        {
            short i;
            for (i = 0; i < UserConfig.VacuumPenC; i ++)
            {
                if (Pen[i].Rotate.Busy)
                {
                    trapPrm_C[i].setPos = Pen[i].Rotate.AngleVal;
                    if (UserConfig.IsProgrammer)
                    {
                        Pen[i].Rotate.Busy = false;
                    }
                    else
                    {
                        DST_Function(Pen[i].Rotate.mode, trapPrm_C[i], axisSts_C[i]);
                        if (axisSts_C[i].isDone)
                        {
                            axisSts_C[i].isDone = false;
                            Pen[i].Rotate.Busy = false;
                        }
                    }
                    
                }
            }
        }

        /// <summary>
        /// 获取板卡信息
        /// </summary>
        public void GetCardMessage()
        {
            GetAllPrfPos();
            GetAllSts();
            for (int i = 0; i < UserConfig.CardC; i++)
            {
                in_Output.ScanCardInput(PLC1.card[i]);
                in_Output.ScanCardOutput(PLC1.card[i]);
                in_Output.WriteCardOutput(PLC1.card[i]);
                if (i == 0 && UserConfig.GLinkC > 0)
                {
                    for (short j = 0; j < UserConfig.GLinkC; j++)
                    {
                        in_Output.ScanGLinkCardInput(PLC1.card[i], j);
                        in_Output.WriteGLinkCardOutput(PLC1.card[i], j);
                    }
                }
            }
        }

        /// <summary>
        /// 获取全部使用轴位置
        /// </summary>
        public void GetAllPrfPos()
        {
            GetSinglePrfPos(trapPrm_X);
            GetSinglePrfPos(trapPrm_Y);
            GetSinglePrfPos(trapPrm_U);
            GetAxisEncPos(trapPrm_X);
            GetAxisEncPos(trapPrm_Y);
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                GetSinglePrfPos(trapPrm_Z[i]);
                GetSinglePrfPos(trapPrm_C[i]);
            }
        }

        /// <summary>
        /// 获取全部使用轴状态
        /// </summary>
        public void GetAllSts()
        {
            GetSingleSts(trapPrm_X, axisSts_X);
            GetSingleSts(trapPrm_Y, axisSts_Y);
            GetSingleSts(trapPrm_U, axisSts_U);
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                GetSingleSts(trapPrm_Z[i], axisSts_Z[i]);
                GetSingleSts(trapPrm_C[i], axisSts_C[i]);
            }
            AxisState_Handle();
        }

        /// <summary>
        /// 获取单轴位置
        /// </summary>
        private void GetSinglePrfPos(TrapPrm trapPrm)
        {
            Gts.GT_GetPrfPos(trapPrm.cardPrm.cardNum, trapPrm.index, out double prfpos, 1, out uint clock);
            trapPrm.getPos = (float)prfpos * trapPrm.pulFactor;
            trapPrm.getPosPul = (int)prfpos;
        }

        /// <summary>
        /// 获取单轴编码器位置
        /// </summary>
        private void GetAxisEncPos(TrapPrm trapPrm)
        {
            Gts.GT_GetAxisEncPos(trapPrm.cardPrm.cardNum, trapPrm.index, out double prfpos, 1, out uint clock);
            trapPrm.getEncPos = (float)prfpos * trapPrm.pulFactor;
            trapPrm.getEncPosPul = (int)prfpos;
        }

        /// <summary>
        /// 获取单轴状态
        /// </summary>
        /// <param name="trapPrm"></param>
        /// <param name="axisSts"></param>
        private void GetSingleSts(TrapPrm trapPrm, AxisSts axisSts)
        {
            Gts.GT_GetSts(trapPrm.cardPrm.cardNum, trapPrm.index, out int pSts, 1, out uint clock);

            axisSts.alarm = ((pSts >> 1 & 0x1) == 1);
            axisSts.mError = ((pSts >> 4 & 0x1) == 1);
            axisSts.posLimit = ((pSts >> 5 & 0x1) == 1);
            axisSts.negLimit = ((pSts >> 6 & 0x1) == 1);
            axisSts.svOn = ((pSts >> 9 & 0x1) == 1);
            axisSts.stop = ((pSts >> 10 & 0x1) == 0);
            axisSts.arrive = ((pSts >> 11 & 0x1) == 1);
        }

        private void AxisState_Handle()
        {
            axisSts_X.isZeroPos = (axisSts_X.isHome && trapPrm_X.getPos == 0);       
            axisSts_Y.isZeroPos = (axisSts_Y.isHome && trapPrm_Y.getPos == 0);
            axisSts_U.isZeroPos = (axisSts_U.isHome && trapPrm_U.getPos == 0);

            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                if(axisSts_Z[i].isHome)
                {
                    axisSts_Z[i].isZeroPos = (trapPrm_Z[i].getPos >= -7.1 && trapPrm_Z[i].getPos <= -6.9);
                    axisSts_Z[i].isSafe = (In_Output.originZI[i].M);
                }            
            }
        }
    }

    //轴状态
    public class AxisSts
    {
        public bool alarm;
        public bool mError;
        public bool posLimit;
        public bool negLimit;      
        public bool svOn;
        public bool stop;
        public bool arrive;

        public bool isZeroPos;
        public bool isSafe;
        public bool isHome;
        public bool isHomeBusy;
        public bool isBusy;
        public bool isDone;
    }

    public class RunPrm
    {    
        public double speed;
        public double longAcc;
        public double longDec;
        public double shortAcc;
        public double shortDec;
        public double longMin;
    }
}
