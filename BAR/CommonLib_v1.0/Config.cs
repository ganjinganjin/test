using BAR.Commonlib.Utils;
using HalconDotNet;
using System;
using System.Runtime.InteropServices;
using System.Text;
using PLC;
using System.IO;
using System.Windows.Forms;
using System.Web.UI.WebControls;

namespace BAR.Commonlib
{
    public sealed class Config
    {
        #region MyRegion

        
        const int _MAX_FNAME = 512;

        [DllImport("kernel32")]//返回0表示失败，非0为成功
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        ///<当前产品名
        public String StrCurProduct;
        //当前Mark名
        public String StrCurMark;

        ///<配置文件路径
        public String StrAppDir;
        public String StrConfigDir;
        public String StrConfigIni;
        public String StrPLCIni;
        public String StrStaticIni;
        public String ProgramerIni;
        public String StrRunModelIni;

        ///<Product文件夹
        public String StrProductDir;
        public String StrProductIni;

        ///<Mark文件夹
        public String StrMarkDir;
        public String StrMarkIni;

        ///<工单信息文件夹
        public String StrTicketsInfoDir;
        public String StrTicketsInfoIni;

        ///<模板匹配属性值
        public double DAngLimit;
        public double DGreediness;
        public double DMaxOverlap;
        public double DScore;

        public double DModelCx, DModelCy, DModelCr;
        public double DModelCx2, DModelCy2, DModelCr2;

        //相机旋转方向
        public int[] ArrCamDir;
        public int IContrast;
        public int IMinContrast;
        public int IShutter;
        public int IGain;
        public int ISnapDelay;
        public int ICanDelay;


        public String StrLightIP;
        public int ILightPort;
        public int ILightType;
        public int ILightCType;

        //通用设置
        public int ILogo;
        public int ProgrammerType;
        public int PenType;
        public static int Altimeter;
        public static int FeederCount;
        public static int BredeDotCount;
        public static int CardType;
        public static int CameraType;
        public static int CameraIP;
        public static int CCDModel;
        public static int ICDirAndFlaw;
        public static int ZoomLens;
        public static bool SyncTakeLay;
        public static int Efficiency;
        /// <summary>
        /// 凤凰平台
        /// </summary>
        public static bool PhenixIOS;
        /// <summary>
        /// 相机曝光时间
        /// </summary>
        public static int Shutter;
        public static int Index_Z;
        /// <summary>
        /// 外挂通讯协议
        /// </summary>
        public static int Protocol_WG;



        public bool IsDispMatch;
        public bool IsSwWorkFlowInfo;
        public static bool IsLoaded;


        //料盘配置参数
        public int TrayModel;
        public int[] TrayRotateDir;
        public int Tray1Start;
        public int Tray1MoveDir;
        public int NGTrayStart;
        public int AutoTrayStart;
        public static int AutoTrayDir;
        public static int AutoTrayDot;
        public struct TrayStartDir
        {
            public bool dx;
            public bool dy;
        }

        private HalconImgUtil _HalconImgUtil = HalconImgUtil.GetInstance();

        public struct StuPara
        {
            public String StrParaName;
            public int IDataType;
            public double DData;
            public uint IData;
        }
        public StuPara[] ArrDevParas;
        public StuPara[] ArrBraidParas;
        

        public struct LampBox
        {
            public int ILightUp;
            public int ILightDown;
            public int IMW;
            public int IMH;
            public int ISW;
            public int ISH;
            public int IC;
        }
        public int IBoxCount;
        public LampBox[] ArrLampBox;

        public struct TagLamp
        {
            public int ICom;
            public int IBaud;
            public int IDataBits;
            public int IParity;
            public int IStopBits;
        }
        public TagLamp[] ArrStrutCom;
        public struct SearchBox
        {
            public HTuple Cy;
            public HTuple Cx;
            public HTuple len1;
            public HTuple len2;
            public HTuple Phi;
        }
        public struct Model
        {
            public string Name;                 ///<模板名词
            public SearchBox SeachBox;             ///<查找框
            public SearchBox ModelBox;             ///<模板框
            public double AngleStart;           ///<开始角度
            public double AngleExtent;          ///<结束角度
            public float MinScore;             ///<分数值
            public int NumMatches;           ///<匹配数量
            public float MaxOverLap;           ///<最大覆盖
            public String SubPixs;              ///<亚像素
            public int NumLevels;            ///<金字塔层数
            public double GreedIness;           ///<贪婪度
        }
        //取料盘及固定位置定义
        public struct FixPosIN
        {
            public PointD P;
            public int IRow;
            public int ICol;
            public double DRolDis;
            public double DColDis;
        };

        //烧录器固定位置定义
        public struct FixPosOut
        {
            public int INo;
            public int IGrop;
            public PointD P;
            public bool IsEn;
            public bool IsState;
        };

        public PointD[] ArrCCDPos;
        public PointD[] ArrCCDPrec;

        public PointD[] ArrPickPos;           ///<吸头在相机上方位置
        public PointD[] ArrPickPosBack;       ///<吸头在相机上方位置备用

        public Model[] ArrMod;
        public HTuple[] ArrModID;

        public int IFixInCount;
        public FixPosIN[] ArrFixIn;
        public FixPosOut[] ArrFixOut;
        public FixPosOut[] ArrFixOutBack;

        public bool[] Bye;
        public SearchBox[] By;
        public HTuple ModelIDBy;                 ///<模板ID          
        public int SnapDelay;
        public int IModelTotal;                  ///<模型总数

        public MarkPos_class[] MarkPos;
        public RunModel_class[] runModel_s;
        private static Config _instance = null;

        private static readonly object padlock = new object();
        #endregion

        Config()
        {
            ArrLampBox = new LampBox[20];
            ArrStrutCom = new TagLamp[6];
            Bye = new bool[10];
            By = new SearchBox[20];
            ArrCCDPos = new PointD[6];
            ArrCCDPrec = new PointD[6];

            ArrPickPos = new PointD[6];
            ArrPickPosBack = new PointD[6];
            ArrFixOut = new FixPosOut[6];
            ArrFixOutBack = new FixPosOut[6];
            ArrCamDir = new int[3];
            TrayRotateDir = new int[3];
            MarkPos = new MarkPos_class[3];
            runModel_s = new RunModel_class[2];
        }

        public static Config GetInstance()
        {
            if (_instance == null)
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new Config();

                    }
                }
            }
            return _instance;
        }

        public void LoadConfig()
        {
            int ret = 1;
            //指定系统参数路径
            this.StrAppDir = System.AppDomain.CurrentDomain.BaseDirectory;
            this.StrConfigDir = StrAppDir + "Config";
            this.StrConfigIni = StrConfigDir + "\\" + "Config.sys";
            this.StrPLCIni = StrConfigDir + "\\" + "PLC.sys";
            this.ProgramerIni = StrConfigDir + "\\" + "Programer.sys";
            //指定保存统计计数文件路径
            this.StrStaticIni = StrConfigDir + "\\" + "Static.sys";
            this.StrRunModelIni = StrConfigDir + "\\" + "RunModel.sys";
            //获取当前产品
            StringBuilder tem_strb = new StringBuilder();
            GetPrivateProfileString("current_product", "current_product_name", "ProTest", tem_strb, _MAX_FNAME, StrConfigIni);
            this.StrCurProduct = tem_strb.ToString();
            //指定产品文件和路径
            this.StrProductDir = StrAppDir + "Product\\" + StrCurProduct;
            this.StrProductIni = StrProductDir + "\\set.prc";
            //指定工单信息文件和路径
            this.StrTicketsInfoDir = StrAppDir + "Product\\" + StrCurProduct;
            this.StrTicketsInfoIni = StrProductDir + "\\set.prc";
            if (!File.Exists(StrProductIni))
            {
                MessageBox.Show("未找到产品：" + StrCurProduct);
                return;
            }
            //获取当前料盘型号
            string strBuffer;
            GetPrivateProfileString("current_mark", "current_mark_name", "", tem_strb, _MAX_FNAME, StrProductIni);
            strBuffer = tem_strb.ToString();
            if (strBuffer == "")
            {
                strBuffer = "Default";
                WritePrivateProfileString("current_mark", "current_mark_name", strBuffer, StrProductIni);
            }
            this.StrCurMark = tem_strb.ToString();

            IsLoaded = true;
            //读取板卡配置参数
            ReadCardInfo();
            //读板卡资源配置
            ReadPLCInfo();
            //读取料盘工作模式配置参数
            ReadTrayWorkModeInfo();
            //读取通信串口
            ReadComInfo();
            ReadPenOffset();
            //读取参数信息
            ReadMachineInfo();
            //读取烧录器配置参数
            if (UserTask.ProgrammerType == GlobConstData.Programmer_WG || UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY)
            {
                ReadProgramerID();
            }
            //读取模板
            ReadModelBox(StrProductIni);
            //读取光源数值
            ReadLightVal();
            //读取产品
            ReadAllPos();
            //读取模板的中心参数
            ReadModelCxy();
            //盘参数;
            ReadPanVal();
            //旋转角度参数;
            ReadRotateAngle();
            //设备参数
            ReadDevParaVal();
            //功能设定
            ReadFuncSwitVal();
            //读取统计计数参数
            ReadStaticVal();
            //读取xyz轴参数
            ReadAxisParaVal();
            //读取编带参数
            ReadBraidParaVal();
            //读取自动盘参数
            ReadAutoTrayParaVal();
            //读座子使能
            ReadSocketEnabled();
            //读座子计数器
            ReadSocketCounter();
            //读吸笔使能
            ReadPenEnabled();
            //读MES数据
            ReadMesDate();
            //读油墨数据
            ReadInksDate();
            //读测高仪高度值
            ReadAltimeterHeightValue();
            //读镜头焦距
            ReadZoomLensValue();
            
            //读Mark位置
            ReadMarkPosValue();
            //读Mark修正量
            ReadMarkCorrection();
            // 加载Markd点数据
            //LoadMarkProd(StrCurMark);
            //读外挂烧录器参数名
            ReadBurnPara();
            
        }

        /// <summary>
        /// 加载配方
        /// </summary>
        /// <param name="prod"></param>
        /// <returns></returns>
        public bool LoadConfigProd(string prod)
        {
            StrCurProduct = prod;
            StrProductDir = StrAppDir + "\\Product\\" + StrCurProduct;
            this.StrProductIni = StrProductDir + "\\set.prc";
            if (!File.Exists(StrProductIni))//判断是否存在文件
            {
                return false;
            }
            //读取模板
            ReadModelBox(StrProductIni);
            //读取产品
            ReadAllPos();
            //读取模板的中心参数
            ReadModelCxy();
            //盘参数;
            ReadPanVal();
            //旋转角度参数;
            ReadRotateAngle();
            //设备参数
            ReadDevParaVal();
            //功能设定
            ReadFuncSwitVal();
            //读取自动盘参数
            ReadAutoTrayParaVal();
            //读座子使能
            ReadSocketEnabled();
            //读取编带参数
            ReadBraidParaVal();
            //获取当前产品
            
            WritePrivateProfileString("current_product", "current_product_name", StrCurProduct, StrConfigIni);

            //读MES数据
            ReadMesDate();
            //读油墨数据
            ReadInksDate();
            //读测高仪高度值
            ReadAltimeterHeightValue();
            //读镜头焦距
            ReadZoomLensValue();

            //获取当前料盘型号
            string strBuffer;
            StringBuilder tem_strb = new StringBuilder();
            GetPrivateProfileString("current_mark", "current_mark_name", "", tem_strb, _MAX_FNAME, StrProductIni);
            strBuffer = tem_strb.ToString();
            if (strBuffer == "")
            {
                strBuffer = "Default";
                WritePrivateProfileString("current_mark", "current_mark_name", strBuffer, StrProductIni);
            }
            this.StrCurMark = tem_strb.ToString();
            // 加载Markd点数据
            //LoadMarkProd(StrCurMark);
            ReadPenOffset();

            return true;
        }

        /// <summary>
        /// 加载Markd点数据
        /// </summary>
        /// <param name="prod"></param>
        public void LoadMarkProd(string prod)
        {
            //if (!ZoomLens)
            //{
            //    return;
            //}
            //StrCurMark = prod;
            //StrMarkDir = StrConfigDir + "\\MarkDate\\" + StrCurMark;
            //this.StrMarkIni = StrMarkDir + "\\set.prc";

            //if (!Directory.Exists(StrMarkDir))//若文件夹不存在则新建文件夹  
            //{
            //    Directory.CreateDirectory(StrMarkDir); //新建文件夹  
            //}

            //if (!File.Exists(StrMarkIni))
            //{
            //    FileStream file = File.Create(StrMarkIni);
            //    file.Close();
            //}
            //WritePrivateProfileString("current_mark", "current_mark_name", StrCurMark, StrProductIni);
            ////读Mark数据
            //ReadMarkValue();
            //SavePanVal(false); //保存行列间距
        }

        /// <summary>
        /// 读运动卡配置
        /// </summary>
        public bool ReadCardInfo()
        {
            StringBuilder str = new StringBuilder();

            for (int i = 0; i < PLC1.card.Length; i++)
            {
                int ind = i + 1;
                String appName = "cardInfo_" + ind;
                GetPrivateProfileString(appName, "cardNum", "0", str, _MAX_FNAME, StrConfigIni);
                PLC1.card[i].cardNum = Convert.ToInt16(str.ToString());
                str.Clear();
                GetPrivateProfileString(appName, "axisCount", "8", str, _MAX_FNAME, StrConfigIni);
                PLC1.card[i].axisCount = Convert.ToInt16(str.ToString());
                str.Clear();
                GetPrivateProfileString(appName, "GPIOCount", "16", str, _MAX_FNAME, StrConfigIni);
                PLC1.card[i].GPIOCount = Convert.ToInt16(str.ToString());
                str.Clear();
            }
            return true;
        }
        /// <summary>
        /// 写运动卡配置
        /// </summary>
        public bool WriteCardInfo()
        {
            for (int i = 0; i < PLC1.card.Length; i++)
            {
                int ind = i + 1;
                String appName = "cardInfo_" + ind;
                WritePrivateProfileString(appName, "cardNum", Convert.ToString(PLC1.card[i].cardNum), StrConfigIni);
                WritePrivateProfileString(appName, "axisCount", Convert.ToString(PLC1.card[i].axisCount), StrConfigIni);
                WritePrivateProfileString(appName, "GPIOCount", Convert.ToString(PLC1.card[i].GPIOCount), StrConfigIni);
            }
            return true;
        }

        //读板卡配置
        public bool ReadPLCInfo()
        {
            String appName;
            appName = "AxisX";
            ReadAxisInfo(appName, Axis.homePrm_X, Axis.trapPrm_X, Axis.jogPrm_X);
            appName = "AxisY";
            ReadAxisInfo(appName, Axis.homePrm_Y, Axis.trapPrm_Y, Axis.jogPrm_Y);
            appName = "AxisU";
            ReadAxisInfo(appName, Axis.homePrm_U, Axis.trapPrm_U, Axis.jogPrm_U);
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                appName = "AxisZ" + (i + 1);
                ReadAxisInfo(appName, Axis.homePrm_Z[i], Axis.trapPrm_Z[i], Axis.jogPrm_Z[i]);
                appName = "AxisC" + (i + 1);
                LimitHomePrm homePrm_C = new LimitHomePrm();
                JogPrm jogPrm_C = new JogPrm();
                ReadAxisInfo(appName, homePrm_C, Axis.trapPrm_C[i], jogPrm_C);
                Axis.trapPrm_C[i].acc = homePrm_C.acc;
                Axis.trapPrm_C[i].dec = homePrm_C.dec;
            }

            if (!File.Exists(StrPLCIni))
            {
                WritePLCInfo();
            }
            ReadInOutputInfo();
            return true;
        }

        //读轴配置参数（工程人员设置）
        public void ReadAxisInfo(string appName, LimitHomePrm homePrm, TrapPrm trapPrm, JogPrm jogPrm)
        {
            StringBuilder str = new StringBuilder();
            short ind;
            GetPrivateProfileString(appName, "CardIndex", "1", str, _MAX_FNAME, StrPLCIni);
            ind = Convert.ToInt16(str.ToString());
            homePrm.cardPrm = trapPrm.cardPrm = jogPrm.cardPrm = PLC1.card[ind - 1];
            str.Clear();

            GetPrivateProfileString(appName, "AxisIndex", "1", str, _MAX_FNAME, StrPLCIni);
            homePrm.index = trapPrm.index = jogPrm.index = Convert.ToInt16(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "PulFactor", "0.002", str, _MAX_FNAME, StrPLCIni);
            homePrm.pulFactor = trapPrm.pulFactor = jogPrm.pulFactor = Convert.ToDouble(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "MoveDir", "-1", str, _MAX_FNAME, StrPLCIni);
            homePrm.moveDir = Convert.ToInt16(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "IndexDir", "-1", str, _MAX_FNAME, StrPLCIni);
            homePrm.indexDir = Convert.ToInt16(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "Edge", "1", str, _MAX_FNAME, StrPLCIni);
            homePrm.edge = Convert.ToInt16(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "VelHigh", "80", str, _MAX_FNAME, StrPLCIni);
            homePrm.velHigh = Convert.ToDouble(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "VelLow", "10", str, _MAX_FNAME, StrPLCIni);
            homePrm.velLow = Convert.ToDouble(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "Acc", "0.4", str, _MAX_FNAME, StrPLCIni);
            homePrm.acc = jogPrm.acc = Convert.ToDouble(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "Dec", "0.4", str, _MAX_FNAME, StrPLCIni);
            homePrm.dec = jogPrm.dec = Convert.ToDouble(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "SmoothTime", "10", str, _MAX_FNAME, StrPLCIni);
            homePrm.smoothTime = trapPrm.smoothTime = Convert.ToInt16(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "EscapeStep", "5", str, _MAX_FNAME, StrPLCIni);
            homePrm.escapeStep = Convert.ToDouble(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "SearchHomeDistance", "600", str, _MAX_FNAME, StrPLCIni);
            homePrm.searchHomeDistance = Convert.ToDouble(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "SearchIndexDistance", "300", str, _MAX_FNAME, StrPLCIni);
            homePrm.searchIndexDistance = Convert.ToDouble(str.ToString());
            str.Clear(); 

            GetPrivateProfileString(appName, "VelStart", "5", str, _MAX_FNAME, StrPLCIni);
            trapPrm.velStart = Convert.ToDouble(str.ToString());
            str.Clear();

            homePrm.strMessage = appName.Remove(0, 4);
        }

        /// <summary>
        /// 读输入输出端口配置参数（工程人员设置）
        /// </summary>
        public void ReadInOutputInfo()
        {
            StringBuilder str = new StringBuilder();
            string appName = "PushSeat";
            for (int i = 0; i < UserConfig.AllMotionC; i++)
            {
                int[] ind = new int[3];
                GetPrivateProfileString(appName, "PushSeat" + (i + 1), null, str, _MAX_FNAME, StrPLCIni);
                string[] strSubAry = str.ToString().Split(',');
                if (strSubAry?.Length <= 1)
                {
                    string str_temp = i < 16 ? "1,0," + i : "1,0,0";
                    WritePrivateProfileString(appName, "PushSeat" + (i + 1), str_temp, StrPLCIni);
                    strSubAry = str_temp.Split(',');
                }
                
                ind[0] = Convert.ToInt32(strSubAry[0].ToString());//板卡号
                ind[1] = Convert.ToInt32(strSubAry[1].ToString());//等于零板卡IO，否则扩展模块IO
                ind[2] = Convert.ToInt32(strSubAry[2].ToString());//端口号
                if (ind[1] == 0)//板卡IO
                {
                    In_Output.pushSeatO[i] = In_Output.EXO[ind[0]][ind[2]];
                    In_Output.pushSeatI[i] = In_Output.EXI[ind[0]][ind[2]];
                    
                }
                else
                {
                    In_Output.pushSeatO[i] = In_Output.EXGLO[ind[1] - 1][ind[2]];
                    In_Output.pushSeatI[i] = In_Output.EXGLI[ind[1] - 1][ind[2]];
                }
                str.Clear();
            }

            appName = "Flip";
            for (int i = 0; i < UserConfig.AllMotionC; i++)
            {
                int[] ind = new int[3];
                GetPrivateProfileString(appName, "Flip" + (i + 1), null, str, _MAX_FNAME, StrPLCIni);
                string[] strSubAry = str.ToString().Split(',');
                if (strSubAry?.Length <= 1)
                {
                    string str_temp = i < 16 ? "1,0," + i : "1,0,0";
                    WritePrivateProfileString(appName, "Flip" + (i + 1), str_temp, StrPLCIni);
                    strSubAry = str_temp.Split(',');
                }

                ind[0] = Convert.ToInt32(strSubAry[0].ToString());//板卡号
                ind[1] = Convert.ToInt32(strSubAry[1].ToString());//等于零板卡IO，否则扩展模块IO
                ind[2] = Convert.ToInt32(strSubAry[2].ToString());//端口号
                if (ind[1] == 0)//板卡IO
                {
                    In_Output.flipO[i] = In_Output.EXO[ind[0]][ind[2]];
                    In_Output.flipI[i] = In_Output.EXI[ind[0]][ind[2]];
                }
                else
                {
                    In_Output.flipO[i] = In_Output.EXGLO[ind[1] - 1][ind[2]];
                    In_Output.flipI[i] = In_Output.EXGLI[ind[1] - 1][ind[2]];
                }
                str.Clear();

                GetPrivateProfileString("FlipLimit", "FlipLimit" + (i + 1), null, str, _MAX_FNAME, StrPLCIni);
                strSubAry = str.ToString().Split(',');
                if (strSubAry?.Length <= 1)
                {
                    string str_temp = i < 16 ? "1,0," + i : "1,0,0";
                    WritePrivateProfileString("FlipLimit", "FlipLimit" + (i + 1), str_temp, StrPLCIni);
                    strSubAry = str_temp.Split(',');
                }

                ind[0] = Convert.ToInt32(strSubAry[0].ToString());//板卡号
                ind[1] = Convert.ToInt32(strSubAry[1].ToString());//等于零板卡IO，否则扩展模块IO
                ind[2] = Convert.ToInt32(strSubAry[2].ToString());//端口号
                if (ind[1] == 0)//板卡IO
                {
                    In_Output.flipLimitI[i] = In_Output.EXI[ind[0]][ind[2]];
                }
                else
                {
                    In_Output.flipLimitI[i] = In_Output.EXGLI[ind[1] - 1][ind[2]];
                }
                str.Clear();
            }
            appName = "Tube";
            for (int i = 0; i < UserConfig.TubeC; i++)
            {
                int[] ind = new int[3];
                GetPrivateProfileString(appName, "Tube" + (i + 1), null, str, _MAX_FNAME, StrPLCIni);
                string[] strSubAry = str.ToString().Split(',');
                if (strSubAry?.Length <= 1)
                {
                    string str_temp = i < 16 ? "0,0," + i : "0,0,0";
                    WritePrivateProfileString(appName, "Tube" + (i + 1), str_temp, StrPLCIni);
                    strSubAry = str_temp.Split(',');
                }

                ind[0] = Convert.ToInt32(strSubAry[0].ToString());//板卡号
                ind[1] = Convert.ToInt32(strSubAry[1].ToString());//等于零板卡IO，否则扩展模块IO
                ind[2] = Convert.ToInt32(strSubAry[2].ToString());//端口号
                if (ind[1] == 0)//板卡IO
                {
                    In_Output.TubeI[i] = In_Output.EXI[ind[0]][ind[2]];
                }
                else
                {
                    In_Output.TubeI[i] = In_Output.EXGLI[ind[1] - 1][ind[2]];
                }
                str.Clear();
            }

            appName = "Feeder";
            for (int i = 0; i < UserConfig.FeederC; i++)
            {
                int[] ind = new int[3];
                GetPrivateProfileString(appName, "Feeder" + (i + 1), null, str, _MAX_FNAME, StrPLCIni);
                string[] strSubAry = str.ToString().Split(',');
                if (strSubAry?.Length == 3)
                {
                    if (i == 0)
                    {
                        UserTask.FeederIO = true;
                    }
                    ind[0] = Convert.ToInt32(strSubAry[0].ToString());//板卡号
                    ind[1] = Convert.ToInt32(strSubAry[1].ToString());//等于零板卡IO，否则扩展模块IO
                    ind[2] = Convert.ToInt32(strSubAry[2].ToString());//端口号
                    if (ind[1] == 0)//板卡IO
                    {
                        In_Output.FeederO[i] = In_Output.EXO[ind[0]][ind[2]];
                    }
                    else
                    {
                        In_Output.FeederO[i] = In_Output.EXGLO[ind[1] - 1][ind[2]];
                    }
                }
                str.Clear();
            }

            appName = "ResetScket";
            for (int i = 0; i < UserConfig.AllScketC; i++)
            {
                int[] ind = new int[3];
                GetPrivateProfileString(appName, "ResetScket" + (i + 1), null, str, _MAX_FNAME, StrPLCIni);
                string[] strSubAry = str.ToString().Split(',');
                if (strSubAry?.Length == 3)
                {
                    ind[0] = Convert.ToInt32(strSubAry[0].ToString());//板卡号
                    ind[1] = Convert.ToInt32(strSubAry[1].ToString());//等于零板卡IO，否则扩展模块IO
                    ind[2] = Convert.ToInt32(strSubAry[2].ToString());//端口号
                    if (ind[1] == 0)//板卡IO
                    {
                        In_Output.resetScketO[i] = In_Output.EXO[ind[0]][ind[2]];
                    }
                    else
                    {
                        In_Output.resetScketO[i] = In_Output.EXGLO[ind[1] - 1][ind[2]];
                    }
                }
                str.Clear();
            }

            //按钮输入
            ReadInputInfo("ButtonInput", "BtnStartI", ref In_Output.BtnStartI);
            ReadInputInfo("ButtonInput", "BtnPauseI", ref In_Output.BtnPauseI);
            ReadInputInfo("ButtonInput", "BtnResetI", ref In_Output.BtnResetI);
            //按钮输出
            ReadOutputInfo("ButtonOutput", "BtnPauseO", ref In_Output.BtnPauseO);
            ReadOutputInfo("ButtonOutput", "BtnResetO", ref In_Output.BtnResetO);

            //编带CCD输入
            ReadInputInfo("BredeCCDInput", "BredeOK_CCD", ref In_Output.BredeOK_CCD);
            ReadInputInfo("BredeCCDInput", "BredeNG_CCD", ref In_Output.BredeNG_CCD);
            //编带CCD输出
            ReadOutputInfo("BredeCCDOutput", "BredeStart_CCD", ref In_Output.BredeStart_CCD);
            ReadOutputInfo("BredeCCDOutput", "BredeLight_CCD", ref In_Output.BredeLight_CCD);
        }

        /// <summary>
        /// 读按钮输入配置参数（工程人员设置）
        /// </summary>
        /// <param name="btn">按钮名称</param>
        /// <param name="gPIO">按钮IO</param>
        private void ReadInputInfo(string appName, string btn, ref GPIO gPIO)
        {
            StringBuilder str = new StringBuilder();
            //string appName = "ButtonInput";
            int[] ind = new int[3];
            GetPrivateProfileString(appName, btn, null, str, _MAX_FNAME, StrPLCIni);
            string[] strSubAry = str.ToString().Split(',');
            if (strSubAry?.Length == 3)
            {
                ind[0] = Convert.ToInt32(strSubAry[0].ToString());//板卡号
                ind[1] = Convert.ToInt32(strSubAry[1].ToString());//等于零板卡IO，否则扩展模块IO
                ind[2] = Convert.ToInt32(strSubAry[2].ToString());//端口号
                if (ind[1] == 0)//板卡IO
                {
                    gPIO = In_Output.EXI[ind[0]][ind[2]];
                }
                else
                {
                    gPIO = In_Output.EXGLI[ind[1] - 1][ind[2]];
                }
            }
            str.Clear();
        }

        /// <summary>
        /// 读按钮指示灯输出配置参数（工程人员设置）
        /// </summary>
        /// <param name="btn">按钮名称</param>
        /// <param name="gPIO">按钮IO</param>
        private void ReadOutputInfo(string appName, string btn, ref GPIO gPIO)
        {
            StringBuilder str = new StringBuilder();
            //string appName = "ButtonOutput";
            int[] ind = new int[3];
            GetPrivateProfileString(appName, btn, null, str, _MAX_FNAME, StrPLCIni);
            string[] strSubAry = str.ToString().Split(',');
            if (strSubAry?.Length == 3)
            {
                ind[0] = Convert.ToInt32(strSubAry[0].ToString());//板卡号
                ind[1] = Convert.ToInt32(strSubAry[1].ToString());//等于零板卡IO，否则扩展模块IO
                ind[2] = Convert.ToInt32(strSubAry[2].ToString());//端口号
                if (ind[1] == 0)//板卡IO
                {
                    gPIO = In_Output.EXO[ind[0]][ind[2]];
                }
                else
                {
                    gPIO = In_Output.EXGLO[ind[1] - 1][ind[2]];
                }
            }
            str.Clear();
        }

        public bool WritePLCInfo()
        {
            String appName;
            appName = "AxisX";
            WriteAxisInfo(appName, Axis.homePrm_X, Axis.trapPrm_X);
            appName = "AxisY";
            WriteAxisInfo(appName, Axis.homePrm_Y, Axis.trapPrm_Y);
            appName = "AxisU";
            WriteAxisInfo(appName, Axis.homePrm_U, Axis.trapPrm_U);
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                appName = "AxisZ" + (i + 1);
                WriteAxisInfo(appName, Axis.homePrm_Z[i], Axis.trapPrm_Z[i]);
                appName = "AxisC" + (i + 1);
            }
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                appName = "AxisC" + (i + 1);
                LimitHomePrm homePrm_C = new LimitHomePrm
                {
                    acc = Axis.trapPrm_C[i].acc,
                    dec = Axis.trapPrm_C[i].dec
                };
                WriteAxisInfo(appName, homePrm_C, Axis.trapPrm_C[i]);
            }

            return true;
        }
        public void WriteAxisInfo(string appName, LimitHomePrm homePrm, TrapPrm trapPrm)
        {
            WritePrivateProfileString(appName, "CardIndex", Convert.ToString(trapPrm.cardPrm.index + 1), StrPLCIni);
            WritePrivateProfileString(appName, "AxisIndex", Convert.ToString(trapPrm.index), StrPLCIni);
            WritePrivateProfileString(appName, "PulFactor", Convert.ToString(trapPrm.pulFactor), StrPLCIni);
            WritePrivateProfileString(appName, "MoveDir", Convert.ToString(homePrm.moveDir), StrPLCIni);
            WritePrivateProfileString(appName, "IndexDir", Convert.ToString(homePrm.indexDir), StrPLCIni);
            WritePrivateProfileString(appName, "Edge", Convert.ToString(homePrm.edge), StrPLCIni);
            WritePrivateProfileString(appName, "VelHigh", Convert.ToString(homePrm.velHigh), StrPLCIni);
            WritePrivateProfileString(appName, "VelLow", Convert.ToString(homePrm.velLow), StrPLCIni);
            WritePrivateProfileString(appName, "Acc", Convert.ToString(homePrm.acc), StrPLCIni);
            WritePrivateProfileString(appName, "Dec", Convert.ToString(homePrm.dec), StrPLCIni);
            WritePrivateProfileString(appName, "SmoothTime", Convert.ToString(trapPrm.smoothTime), StrPLCIni);
            WritePrivateProfileString(appName, "EscapeStep", Convert.ToString(homePrm.escapeStep), StrPLCIni);
            WritePrivateProfileString(appName, "SearchHomeDistance", Convert.ToString(homePrm.searchHomeDistance), StrPLCIni);
            WritePrivateProfileString(appName, "SearchIndexDistance", Convert.ToString(homePrm.searchIndexDistance), StrPLCIni);
            WritePrivateProfileString(appName, "VelStart", Convert.ToString(trapPrm.velStart), StrPLCIni);
        }

        public bool ReadComInfo()
        {
            StringBuilder str = new StringBuilder();
            long ret;
            for (int i = 0; i < 6; i++)
            {
                String appName = "serial_port_com_control_" + i;
                ret = GetPrivateProfileString(appName, "com_no", "1", str, _MAX_FNAME, StrConfigIni);
                ArrStrutCom[i].ICom = Convert.ToInt32(str.ToString());
                str.Clear();
                ret = GetPrivateProfileString(appName, "baud", "115200", str, _MAX_FNAME, StrConfigIni);
                ArrStrutCom[i].IBaud = Convert.ToInt32(str.ToString());
                str.Clear();
                ret = GetPrivateProfileString(appName, "data_bits", "8", str, _MAX_FNAME, StrConfigIni);
                ArrStrutCom[i].IDataBits = Convert.ToInt32(str.ToString());
                str.Clear();
                ret = GetPrivateProfileString(appName, "parity", "0", str, _MAX_FNAME, StrConfigIni);
                ArrStrutCom[i].IParity = Convert.ToInt32(str.ToString());
                str.Clear();
                ret = GetPrivateProfileString(appName, "stop_bits", "1", str, _MAX_FNAME, StrConfigIni);
                ArrStrutCom[i].IStopBits = Convert.ToInt32(str.ToString());
                str.Clear();
            }
            return true;
        }
        public bool WriteComInfo()
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                String appName = "serial_port_com_control_" + i;
                WritePrivateProfileString(appName, "com_no", Convert.ToString(ArrStrutCom[i].ICom), StrConfigIni);
                WritePrivateProfileString(appName, "baud", Convert.ToString(ArrStrutCom[i].IBaud), StrConfigIni);
                WritePrivateProfileString(appName, "data_bits", Convert.ToString(ArrStrutCom[i].IDataBits), StrConfigIni);
                WritePrivateProfileString(appName, "parity", Convert.ToString(ArrStrutCom[i].IParity), StrConfigIni);
                WritePrivateProfileString(appName, "stop_bits", Convert.ToString(ArrStrutCom[i].IStopBits), StrConfigIni);
            }
            return true;
        }

        /// <summary>
        /// 保存工程设置
        /// </summary>
        /// <returns></returns>
        public bool SaveEngineerSetting()
        {
            string str = null;
            String appName;
            for (int i = 0; i < 6; i++)
            {
                appName = "serial_port_com_control_" + i;
                WritePrivateProfileString(appName, "com_no", Convert.ToString(ArrStrutCom[i].ICom), StrConfigIni);
                WritePrivateProfileString(appName, "baud", Convert.ToString(ArrStrutCom[i].IBaud), StrConfigIni);
                WritePrivateProfileString(appName, "data_bits", Convert.ToString(ArrStrutCom[i].IDataBits), StrConfigIni);
                WritePrivateProfileString(appName, "parity", Convert.ToString(ArrStrutCom[i].IParity), StrConfigIni);
                WritePrivateProfileString(appName, "stop_bits", Convert.ToString(ArrStrutCom[i].IStopBits), StrConfigIni);
                //if (i == 3)
                //{
                //    WritePrivateProfileString(appName, "baud", Convert.ToString(ArrStrutCom[i].IBaud), StrConfigIni);
                //    WritePrivateProfileString(appName, "parity", Convert.ToString(ArrStrutCom[i].IParity), StrConfigIni);
                //}
            }

            appName = "ParameterSet";
            WritePrivateProfileString(appName, "ProgrammerType", Convert.ToString(ProgrammerType), StrConfigIni);
            WritePrivateProfileString(appName, "PenType", Convert.ToString(PenType), StrConfigIni);
            WritePrivateProfileString(appName, "Logo", Convert.ToString(ILogo), StrConfigIni);
            WritePrivateProfileString(appName, "MesType", Convert.ToString(Mes.Type), StrConfigIni);
            WritePrivateProfileString(appName, "LightType", Convert.ToString(ILightType), StrConfigIni);
            WritePrivateProfileString(appName, "LitghtCType", Convert.ToString(ILightCType), StrConfigIni);
            str = String.Format("{0:d},{1:d},{2:d}", ArrCamDir[0], ArrCamDir[1], ArrCamDir[2]);
            WritePrivateProfileString(appName, "CamDir", str, StrConfigIni);
            WritePrivateProfileString(appName, "3DFun", Vision_3D.Function == true ? "1" : "0", StrConfigIni);
            WritePrivateProfileString(appName, "Altimeter", Convert.ToString(Altimeter), StrConfigIni);
            WritePrivateProfileString(appName, "InksFunction", Inks.Function == true ? "1" : "0", StrConfigIni);
            WritePrivateProfileString(appName, "FeederCount", Convert.ToString(FeederCount), StrConfigIni);
            WritePrivateProfileString(appName, "BredeDotCount", Convert.ToString(BredeDotCount), StrConfigIni);
            WritePrivateProfileString(appName, "CardType", Convert.ToString(CardType), StrConfigIni);
            WritePrivateProfileString(appName, "CameraType", Convert.ToString(CameraType), StrConfigIni);
            WritePrivateProfileString(appName, "CameraIP", Convert.ToString(CameraIP), StrConfigIni);
            WritePrivateProfileString(appName, "CCDModel", Convert.ToString(CCDModel), StrConfigIni);
            WritePrivateProfileString(appName, "ICDirAndFlaw", Convert.ToString(ICDirAndFlaw), StrConfigIni);
            WritePrivateProfileString(appName, "Shutter", Convert.ToString(Shutter), StrConfigIni);
            WritePrivateProfileString(appName, "ZoomLens", Convert.ToString(ZoomLens), StrConfigIni);
            WritePrivateProfileString(appName, "SyncTakeLay", SyncTakeLay == true ? "1" : "0", StrConfigIni);
            WritePrivateProfileString(appName, "Efficiency", Convert.ToString(Efficiency), StrConfigIni);
            WritePrivateProfileString(appName, "PhenixIOS", PhenixIOS == true ? "1" : "0", StrConfigIni);
            WritePrivateProfileString(appName, "Protocol_WG", Convert.ToString(Protocol_WG), StrConfigIni);

            appName = "TrayWorkModeInfo";
            WritePrivateProfileString(appName, "TrayModel", Convert.ToString(TrayModel), StrConfigIni);
            WritePrivateProfileString(appName, "Tray1Start", Convert.ToString(Tray1Start), StrConfigIni);
            WritePrivateProfileString(appName, "Tray1MoveDir", Convert.ToString(Tray1MoveDir), StrConfigIni);
            WritePrivateProfileString(appName, "NGTrayStart", Convert.ToString(NGTrayStart), StrConfigIni);
            WritePrivateProfileString(appName, "AutoTrayStart", Convert.ToString(AutoTrayStart), StrConfigIni);
            WritePrivateProfileString(appName, "TrayRotateDir2", Convert.ToString(TrayRotateDir[1]), StrConfigIni);
            WritePrivateProfileString(appName, "TrayRotateDir3", Convert.ToString(TrayRotateDir[2]), StrConfigIni);
            WritePrivateProfileString(appName, "AutoTrayDir", Convert.ToString(AutoTrayDir), StrConfigIni);
            WritePrivateProfileString(appName, "AutoTrayDot", Convert.ToString(AutoTrayDot), StrConfigIni);

            appName = "GlobalParam";
            WritePrivateProfileString(appName, "ServerIP", GlobalParam.ServerIP, StrConfigIni);
            WritePrivateProfileString(appName, "ServerPort", GlobalParam.ServerPort.ToString(), StrConfigIni);
            WritePrivateProfileString(appName, "RPCServerIP", GlobalParam.RPCServerIP, StrConfigIni);
            WritePrivateProfileString(appName, "RPCServerPort", GlobalParam.RPCServerPort.ToString(), StrConfigIni);


            for (int i = 0; i < PLC1.card.Length; i++)
            {
                int ind = i + 1;
                appName = "cardInfo_" + ind;
                WritePrivateProfileString(appName, "cardNum", Convert.ToString(PLC1.card[i].cardNum), StrConfigIni);
            }
            return true;
        }

        /// <summary>
        /// 读料盘工作模式配置
        /// </summary>
        public bool ReadTrayWorkModeInfo()
        {
            StringBuilder str = new StringBuilder();
            String appName = "TrayWorkModeInfo";

            GetPrivateProfileString(appName, "TrayModel", "0", str, _MAX_FNAME, StrConfigIni);
            TrayModel = Convert.ToInt16(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "Tray1Start", "0", str, _MAX_FNAME, StrConfigIni);
            Tray1Start = Convert.ToInt16(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "Tray1MoveDir", "0", str, _MAX_FNAME, StrConfigIni);
            Tray1MoveDir = Convert.ToInt16(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "AutoTrayDir", "0", str, _MAX_FNAME, StrConfigIni);
            AutoTrayDir = Convert.ToInt16(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "NGTrayStart", "0", str, _MAX_FNAME, StrConfigIni);
            NGTrayStart = Convert.ToInt16(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "AutoTrayStart", "0", str, _MAX_FNAME, StrConfigIni);
            AutoTrayStart = Convert.ToInt16(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "AutoTrayDot", "0", str, _MAX_FNAME, StrConfigIni);
            AutoTrayDot = Convert.ToInt16(str.ToString());
            str.Clear();

            for (int i = 1; i < TrayRotateDir.Length; i++)
            {
                GetPrivateProfileString(appName, "TrayRotateDir" + Convert.ToString(i + 1), "0", str, _MAX_FNAME, StrConfigIni);
                TrayRotateDir[i] = Convert.ToInt16(str.ToString());
                str.Clear();
            }
            return true;
        }

        /// <summary>
        /// 料盘起始点
        /// </summary>
        /// <param name="trayNum"></param>
        /// <returns 返回值代表起点:></returns>
        public TrayStartDir Tray_start(int trayNum)
        {
            TrayStartDir dir = new TrayStartDir();
            if (trayNum == 0)
            {
                dir.dx = false;
                dir.dy = Tray1Start == 0 ? false : true;
            }
            else
            {
                dir.dx = TrayRotateDir[trayNum] - Tray1Start == 1 ? true : false;
                dir.dy = TrayRotateDir[trayNum] == Tray1Start || (TrayRotateDir[trayNum] == 1 && Tray1Start == 0) ? false : true;
            }
            return dir;
        }

        /// <summary>
        /// 料盘列数++判断
        /// </summary>
        /// <param name="trayNum"></param>
        public bool Tray_Col_Add(int trayNum)
        {
            if (Tray1MoveDir == 1 && TrayRotateDir[trayNum] == 2)
            {
                return true;
            }
            else if (Tray1MoveDir == TrayRotateDir[trayNum])
            {
               return true;
            }
            return false;
        }

        /// <summary>
        /// 写料盘工作模式配置
        /// </summary>
        public bool WriteTrayWorkModeInfo()
        {
            StringBuilder str = new StringBuilder();
            String appName = "TrayWorkModeInfo";

            WritePrivateProfileString(appName, "Tray1Start", Convert.ToString(Tray1Start), StrConfigIni);
            WritePrivateProfileString(appName, "Tray1MoveDir", Convert.ToString(Tray1MoveDir), StrConfigIni);
            WritePrivateProfileString(appName, "NGTrayStart", Convert.ToString(NGTrayStart), StrConfigIni);
            for (int i = 1; i < TrayRotateDir.Length; i++)
            {
                WritePrivateProfileString(appName, "TrayRotateDir" + Convert.ToString(i + 1), Convert.ToString(TrayRotateDir[i]), StrConfigIni);
            }
            return true;
        }

        /// <summary>
        /// 读烧录ID
        /// </summary>
        public bool ReadProgramerID()
        {
            StringBuilder str = new StringBuilder();
            String appName = "ProgramerID";

            GetPrivateProfileString(appName, "IDNum", "0", str, _MAX_FNAME, ProgramerIni);
            Download_WG.programerID = Convert.ToUInt16(str.ToString());
            str.Clear();
            
            if (!File.Exists(ProgramerIni))
            {
                ReadProgramerInfo();
                WriteProgramerID();
                for (int i = 0; i < 10; i++)
                {
                    WriteProgramerInfo(i);
                }
            }
            return true;
        }
        /// <summary>
        /// 写烧录器ID
        /// </summary>
        /// <returns></returns>
        public bool WriteProgramerID()
        {
            String appName = "ProgramerID";
            WritePrivateProfileString(appName, "IDNum", Convert.ToString(Download_WG.programerID), ProgramerIni);
            return true;
        }

        /// <summary>
        /// 读烧录器配置
        /// </summary>
        public bool ReadProgramerInfo()
        {
            StringBuilder str = new StringBuilder();
            int ind = Download_WG.programerID + 1;
            String appName = "Programer_" + ind;

            GetPrivateProfileString(appName, "PulseWidth_Start", "100", str, _MAX_FNAME, ProgramerIni);
            Download_WG.setParameter.PulseWidth_Start = Convert.ToUInt16(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "Time_Busy", "100", str, _MAX_FNAME, ProgramerIni);
            Download_WG.setParameter.Time_Busy = Convert.ToUInt16(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "Time_EOT", "100", str, _MAX_FNAME, ProgramerIni);
            Download_WG.setParameter.Time_EOT = Convert.ToUInt16(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "Time_OKNG", "100", str, _MAX_FNAME, ProgramerIni);
            Download_WG.setParameter.Time_OKNG = Convert.ToUInt16(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "Level_Start", "0", str, _MAX_FNAME, ProgramerIni);
            Download_WG.setParameter.Level_Start = Convert.ToUInt16(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "Level_Busy", "0", str, _MAX_FNAME, ProgramerIni);
            Download_WG.setParameter.Level_Busy = Convert.ToUInt16(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "Level_OK", "0", str, _MAX_FNAME, ProgramerIni);
            Download_WG.setParameter.Level_OK = Convert.ToUInt16(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "Level_NG", "0", str, _MAX_FNAME, ProgramerIni);
            Download_WG.setParameter.Level_NG = Convert.ToUInt16(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "RepeatNumber", "0", str, _MAX_FNAME, ProgramerIni);
            Download_WG.setParameter.RepeatNumber = Convert.ToUInt16(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "Time_Down", "20", str, _MAX_FNAME, ProgramerIni);
            Download_WG.setParameter.Time_Down = Convert.ToUInt16(str.ToString());
            str.Clear();
            return true;
        }
        /// <summary>
        /// 写烧录器配置
        /// </summary>
        /// <returns></returns>
        public bool WriteProgramerInfo(int ind)
        {
            ind++;
            String appName = "Programer_"+ ind;
            WritePrivateProfileString(appName, "PulseWidth_Start", Convert.ToString(Download_WG.setParameter.PulseWidth_Start), ProgramerIni);
            WritePrivateProfileString(appName, "Time_Busy", Convert.ToString(Download_WG.setParameter.Time_Busy), ProgramerIni);
            WritePrivateProfileString(appName, "Time_EOT", Convert.ToString(Download_WG.setParameter.Time_EOT), ProgramerIni);
            WritePrivateProfileString(appName, "Time_OKNG", Convert.ToString(Download_WG.setParameter.Time_OKNG), ProgramerIni);
            WritePrivateProfileString(appName, "Level_Start", Convert.ToString(Download_WG.setParameter.Level_Start), ProgramerIni);
            WritePrivateProfileString(appName, "Level_Busy", Convert.ToString(Download_WG.setParameter.Level_Busy), ProgramerIni);
            WritePrivateProfileString(appName, "Level_OK", Convert.ToString(Download_WG.setParameter.Level_OK), ProgramerIni);
            WritePrivateProfileString(appName, "Level_NG", Convert.ToString(Download_WG.setParameter.Level_NG), ProgramerIni);
            WritePrivateProfileString(appName, "RepeatNumber", Convert.ToString(Download_WG.setParameter.RepeatNumber), ProgramerIni);
            WritePrivateProfileString(appName, "Time_Down", Convert.ToString(Download_WG.setParameter.Time_Down), ProgramerIni);
            return true;
        }

        public void ReadAllPos()
        {
            StringBuilder str_temp = new StringBuilder(), str_data = new StringBuilder();
            int n = 0;
            String appName = "FixInPos", keyName = "FixInCount";

            //GetPrivateProfileString(appName, keyName, "8", str_temp, _MAX_FNAME, StrProductIni);
            //IFixInCount = Convert.ToInt32(str_temp.ToString());
            IFixInCount = 9;
            ArrFixIn = new FixPosIN[IFixInCount];

            String str;
            String[] strSubAry;
            for (n = 0; n < IFixInCount; n++)
            {
                GetPrivateProfileString(appName, "FixIn_" + n, "0,0,0,0,0,0", str_temp, _MAX_FNAME, StrProductIni);
                str = str_temp.ToString();
                strSubAry = str.Split(',');
                ArrFixIn[n] = new FixPosIN();
                Double posX, posY;
                posX = Double.Parse(strSubAry[0]);//x
                posY = Double.Parse(strSubAry[1]);//y
                ArrFixIn[n].P = new PointD(posX, posY);
                ArrFixIn[n].ICol = Convert.ToInt32(strSubAry[2]);//行
                ArrFixIn[n].IRow = Convert.ToInt32(strSubAry[3]);//列
                ArrFixIn[n].DRolDis = Double.Parse(strSubAry[4]);//行间距
                ArrFixIn[n].DColDis = Double.Parse(strSubAry[5]);//列间距

                if (n < 3)
                {
                    Axis.Position_X.trayFirst[n] = ArrFixIn[n].P.X;
                    Axis.Position_Y.trayFirst[n] = ArrFixIn[n].P.Y;
                }
                else if (n == 3)
                {
                    Axis.Position_X.trayEnd[0] = ArrFixIn[n].P.X;
                    Axis.Position_Y.trayEnd[0] = ArrFixIn[n].P.Y;
                }
                else if (n == 4)
                {
                    Axis.Position_X.bredeOut = ArrFixIn[n].P.X;
                    Axis.Position_Y.bredeOut = ArrFixIn[n].P.Y;
                }
                else if (n == 5)
                {
                    Axis.Position_X.Feeder[0] = ArrFixIn[n].P.X;
                    Axis.Position_Y.Feeder[0] = ArrFixIn[n].P.Y;
                }
                else if (n == 6)
                {
                    Axis.Position_X.NGCup = ArrFixIn[n].P.X;
                    Axis.Position_Y.NGCup = ArrFixIn[n].P.Y;
                }
                else if (n == 7)
                {
                    Axis.Position_X.tubeIn = ArrFixIn[n].P.X;
                    Axis.Position_Y.tubeIn = ArrFixIn[n].P.Y;
                }
                else if (n == 8)
                {
                    Axis.Position_X.Feeder[1] = ArrFixIn[n].P.X;
                    Axis.Position_Y.Feeder[1] = ArrFixIn[n].P.Y;
                }
            }

            //位置2#
            appName = "FixOutPos";

            ArrFixOut = new FixPosOut[UserConfig.AllScketC];
            ArrFixOutBack = new FixPosOut[UserConfig.AllScketC];

            for (n = 0; n < UserConfig.AllScketC; n++)
            {
                GetPrivateProfileString(appName, "FixOut_" + n, "0,0,0,0,0,0", str_temp, _MAX_FNAME, StrProductIni);
                str = str_temp.ToString();
                strSubAry = str.Split(',');
                ArrFixOut[n] = new FixPosOut
                {
                    IGrop = Convert.ToInt32(strSubAry[0]),//组
                    INo = Convert.ToInt32(strSubAry[1]),//序号
                    IsEn = strSubAry[2] == "1" ? true : false//使能
                };
                Double posX, posY;
                posX = Double.Parse(strSubAry[3]);//x
                posY = Double.Parse(strSubAry[4]);//y
                ArrFixOut[n].P = new PointD(posX, posY);

                int k = UserConfig.ScketUnitC;
                Axis.Group[n / k].Unit[n % k].TopCamera_X = ArrFixOut[n].P.X;
                Axis.Group[n / k].Unit[n % k].TopCamera_Y = ArrFixOut[n].P.Y;
            }
        }
        public void ReadMachineInfo()
        {
            String appName = "ParameterSet";
            StringBuilder tem_str = new StringBuilder();
            string strBuffer;

            for (int n = 0; n < 6; n++)
            {
                ArrCCDPrec[n] = new PointD();
                GetPrivateProfileString(appName, "ccdx" + n, "0", tem_str, _MAX_FNAME, StrConfigIni);
                ArrCCDPrec[n].X = Double.Parse(tem_str.ToString());
                GetPrivateProfileString(appName, "ccdy" + n, "0", tem_str, _MAX_FNAME, StrConfigIni);
                ArrCCDPrec[n].Y = Double.Parse(tem_str.ToString());
               
                ArrCCDPos[n] = new PointD();
                GetPrivateProfileString(appName, "posx" + n, "0", tem_str, _MAX_FNAME, StrConfigIni);
                ArrCCDPos[n].X = Double.Parse(tem_str.ToString());
                GetPrivateProfileString(appName, "posy" + n, "0", tem_str, _MAX_FNAME, StrConfigIni);
                ArrCCDPos[n].Y = Double.Parse(tem_str.ToString());
            }

            for (int n = 0; n < 6; n++)
            {
                ArrPickPos[n] = new PointD();
                GetPrivateProfileString(appName, "Pickposx" + n, "0", tem_str, _MAX_FNAME, StrConfigIni);
                ArrPickPos[n].X = Double.Parse(tem_str.ToString());
                GetPrivateProfileString(appName, "Pickposy" + n, "0", tem_str, _MAX_FNAME, StrConfigIni);
                ArrPickPos[n].Y = Double.Parse(tem_str.ToString());
            }

            GetPrivateProfileString(appName, "ScannerX", "0", tem_str, _MAX_FNAME, StrConfigIni);
            Axis.Scanner.Calibrating_X = Double.Parse(tem_str.ToString());
            GetPrivateProfileString(appName, "ScannerY", "0", tem_str, _MAX_FNAME, StrConfigIni);
            Axis.Scanner.Calibrating_Y = Double.Parse(tem_str.ToString());

            GetPrivateProfileString(appName, "AltimeterX", "0", tem_str, _MAX_FNAME, StrConfigIni);
            Axis.Altimeter.Calibrating_X = Double.Parse(tem_str.ToString());
            GetPrivateProfileString(appName, "AltimeterY", "0", tem_str, _MAX_FNAME, StrConfigIni);
            Axis.Altimeter.Calibrating_Y = Double.Parse(tem_str.ToString());
            GetPrivateProfileString(appName, "AltimeterZ", "0", tem_str, _MAX_FNAME, StrConfigIni);
            Axis.Altimeter.Calibrating_Z = Double.Parse(tem_str.ToString());

            for (int n = 0; n < UserConfig.VacuumPenC; n++)
            {
                Axis.Pen[n].LowCamera_X = ArrPickPos[2 + n].X;
                Axis.Pen[n].LowCamera_Y = ArrPickPos[2 + n].Y;

                GetPrivateProfileString(appName, "Calibrating_Z" + (n + 1), "0", tem_str, _MAX_FNAME, StrConfigIni);
                Axis.Pen[n].Calibrating_Z = Double.Parse(tem_str.ToString());
            }
            GetOffset_TopCameraToPen();
            GetHeight_AltimeterToPen();

            GetPrivateProfileString(appName, "ProgrammerType", "0", tem_str, _MAX_FNAME, StrConfigIni);
            UserTask.ProgrammerType = ProgrammerType = Convert.ToInt32(tem_str.ToString());

            GetPrivateProfileString(appName, "Protocol_WG", "0", tem_str, _MAX_FNAME, StrConfigIni);
            UserTask.Protocol_WG = Protocol_WG = Convert.ToInt32(tem_str.ToString());

            GetPrivateProfileString(appName, "PenType", "0", tem_str, _MAX_FNAME, StrConfigIni);
            UserTask.PenType = PenType = Convert.ToInt32(tem_str.ToString());
            UserConfig.AxisZC = PenType == 1 ? 1 : UserConfig.VacuumPenC;

            GetPrivateProfileString(appName, "Logo", "0", tem_str, _MAX_FNAME, StrConfigIni);
            ILogo = Convert.ToInt32(tem_str.ToString());

            GetPrivateProfileString(appName, "MesType", "0", tem_str, _MAX_FNAME, StrConfigIni);
            Mes.Type = Convert.ToInt32(tem_str.ToString());

            GetPrivateProfileString(appName, "LightType", "0", tem_str, _MAX_FNAME, StrConfigIni);
            int temp = Convert.ToInt32(tem_str.ToString());
            if (temp == 0)
            {
                temp = 2;
                WritePrivateProfileString(appName, "LightType", Convert.ToString(temp), StrConfigIni);
            }
            ILightType = temp;

            GetPrivateProfileString(appName, "LitghtCType", "1", tem_str, _MAX_FNAME, StrConfigIni);
            if (Convert.ToInt32(tem_str.ToString()) == 1)
            {
                ILightCType = GlobConstData.LightCtl_WAN;
            }
            else
            {
                ILightCType = GlobConstData.LightCtl_COM; 
            }

            GetPrivateProfileString(appName, "LightIP", "", tem_str, _MAX_FNAME, StrConfigIni);
            if (tem_str.ToString() == "")
            {
                WritePrivateProfileString(appName, "LightIP", "192.168.0.110", StrConfigIni);
                StrLightIP = "192.168.0.110";
            }
            else
            {
                StrLightIP = tem_str.ToString();
            }
            tem_str.Clear();

            GetPrivateProfileString(appName, "LightPORT", "", tem_str, _MAX_FNAME, StrConfigIni);
            if (tem_str.ToString() == "")
            {
                ILightPort = 3205;
                WritePrivateProfileString(appName, "LightPORT", Convert.ToString(ILightPort), StrConfigIni);
            }
            else
            {
                ILightPort = Convert.ToInt32(tem_str.ToString());
            }
            tem_str.Clear();

            GetPrivateProfileString(appName, "IP3D", "", tem_str, _MAX_FNAME, StrConfigIni);
            strBuffer = tem_str.ToString();
            if (tem_str.ToString() == "")
            {
                strBuffer = "192.168.0.11";
                WritePrivateProfileString(appName, "IP3D", strBuffer, StrConfigIni);
            }
            Vision_3D.IP = strBuffer;
            tem_str.Clear();

            GetPrivateProfileString(appName, "PORT3D", "", tem_str, _MAX_FNAME, StrConfigIni);
            strBuffer = tem_str.ToString();
            if (strBuffer == "")
            {
                strBuffer = "5000";
                WritePrivateProfileString(appName, "PORT3D", strBuffer, StrConfigIni);
            }
            Vision_3D.Port = Convert.ToInt32(strBuffer);
            tem_str.Clear();

            GetPrivateProfileString(appName, "3DFun", "", tem_str, _MAX_FNAME, StrConfigIni);
            strBuffer = tem_str.ToString();
            if (strBuffer == "")
            {
                strBuffer = "0";
                WritePrivateProfileString(appName, "3DFun", strBuffer, StrConfigIni);
            }
            Vision_3D.Function = strBuffer == "1" ? true : false;
            tem_str.Clear();

            GetPrivateProfileString(appName, "Altimeter", "", tem_str, _MAX_FNAME, StrConfigIni);
            strBuffer = tem_str.ToString();
            if (strBuffer == "")
            {
                strBuffer = "0";
                WritePrivateProfileString(appName, "Altimeter", strBuffer, StrConfigIni);
            }
            Altimeter = Convert.ToInt32(strBuffer);
            tem_str.Clear();

            GetPrivateProfileString(appName, "Efficiency", "", tem_str, _MAX_FNAME, StrConfigIni);
            strBuffer = tem_str.ToString();
            if (strBuffer == "")
            {
                strBuffer = "0";
                WritePrivateProfileString(appName, "Efficiency", strBuffer, StrConfigIni);
            }
            Efficiency = Convert.ToInt32(strBuffer);
            tem_str.Clear();

            GetPrivateProfileString(appName, "InksFunction", "", tem_str, _MAX_FNAME, StrConfigIni);
            strBuffer = tem_str.ToString();
            if (strBuffer == "")
            {
                strBuffer = "0";
                WritePrivateProfileString(appName, "InksFunction", strBuffer, StrConfigIni);
            }
            Inks.Function = strBuffer == "1" ? true : false;
            tem_str.Clear();

            GetPrivateProfileString(appName, "ZoomLens", "", tem_str, _MAX_FNAME, StrConfigIni);
            strBuffer = tem_str.ToString();
            if (strBuffer == "")
            {
                strBuffer = "0";
                WritePrivateProfileString(appName, "ZoomLens", strBuffer, StrConfigIni);
            }
            ZoomLens = Convert.ToInt32(strBuffer);
            tem_str.Clear();

            GetPrivateProfileString(appName, "PhenixIOS", "", tem_str, _MAX_FNAME, StrConfigIni);
            strBuffer = tem_str.ToString();
            if (strBuffer == "")
            {
                strBuffer = "0";
                WritePrivateProfileString(appName, "PhenixIOS", strBuffer, StrConfigIni);
            }
            PhenixIOS = strBuffer == "1" ? true : false;
            tem_str.Clear();

            GetPrivateProfileString(appName, "SyncTakeLay", "", tem_str, _MAX_FNAME, StrConfigIni);
            strBuffer = tem_str.ToString();
            if (strBuffer == "")
            {
                strBuffer = "0";
                WritePrivateProfileString(appName, "SyncTakeLay", strBuffer, StrConfigIni);
            }
            SyncTakeLay = strBuffer == "1" ? true : false;
            tem_str.Clear();

            GetPrivateProfileString(appName, "FeederCount", "", tem_str, _MAX_FNAME, StrConfigIni);
            strBuffer = tem_str.ToString();
            if (strBuffer == "")
            {
                strBuffer = "0";
                WritePrivateProfileString(appName, "FeederCount", strBuffer, StrConfigIni);
            }
            FeederCount = Convert.ToInt32(strBuffer);
            tem_str.Clear();

            GetPrivateProfileString(appName, "BredeDotCount", "", tem_str, _MAX_FNAME, StrConfigIni);
            strBuffer = tem_str.ToString();
            if (strBuffer == "")
            {
                strBuffer = "0";
                WritePrivateProfileString(appName, "BredeDotCount", strBuffer, StrConfigIni);
            }
            BredeDotCount = Convert.ToInt32(strBuffer);
            tem_str.Clear();

            GetPrivateProfileString(appName, "CardType", "", tem_str, _MAX_FNAME, StrConfigIni);
            strBuffer = tem_str.ToString();
            if (strBuffer == "")
            {
                strBuffer = "0";
                WritePrivateProfileString(appName, "CardType", strBuffer, StrConfigIni);
            }
            CardType = Convert.ToInt32(strBuffer);
            tem_str.Clear();

            GetPrivateProfileString(appName, "CameraType", "", tem_str, _MAX_FNAME, StrConfigIni);
            strBuffer = tem_str.ToString();
            if (strBuffer == "")
            {
                strBuffer = "0";
                WritePrivateProfileString(appName, "CameraType", strBuffer, StrConfigIni);
            }
            CameraType = Convert.ToInt32(strBuffer);
            tem_str.Clear();

            GetPrivateProfileString(appName, "CameraIP", "", tem_str, _MAX_FNAME, StrConfigIni);
            strBuffer = tem_str.ToString();
            if (strBuffer == "")
            {
                strBuffer = "0";
                WritePrivateProfileString(appName, "CameraIP", strBuffer, StrConfigIni);
            }
            CameraIP = Convert.ToInt32(strBuffer);
            tem_str.Clear();

            GetPrivateProfileString(appName, "CCDModel", "", tem_str, _MAX_FNAME, StrConfigIni);
            strBuffer = tem_str.ToString();
            if (strBuffer == "")
            {
                strBuffer = "0";
                WritePrivateProfileString(appName, "CCDModel", strBuffer, StrConfigIni);
            }
            CCDModel = Convert.ToInt32(strBuffer);
            tem_str.Clear();

            GetPrivateProfileString(appName, "Shutter", "", tem_str, _MAX_FNAME, StrConfigIni);
            strBuffer = tem_str.ToString();
            if (strBuffer == "")
            {
                strBuffer = "50";
                WritePrivateProfileString(appName, "Shutter", strBuffer, StrConfigIni);
            }
            Shutter = Convert.ToInt32(strBuffer);
            tem_str.Clear();

            GetPrivateProfileString(appName, "ICDirAndFlaw", "", tem_str, _MAX_FNAME, StrConfigIni);
            strBuffer = tem_str.ToString();
            if (strBuffer == "")
            {
                strBuffer = "0";
                WritePrivateProfileString(appName, "ICDirAndFlaw", strBuffer, StrConfigIni);
            }
            ICDirAndFlaw = Convert.ToInt32(strBuffer);
            tem_str.Clear();

            GetPrivateProfileString(appName, "IsDispMatch", "", tem_str, _MAX_FNAME, StrConfigIni);
            if (tem_str.ToString().Length <= 0)
            {
                temp = 1;
                WritePrivateProfileString(appName, "IsDispMatch", Convert.ToString(temp), StrConfigIni);
            }
            IsDispMatch = tem_str.ToString() == "1";

            GetPrivateProfileString(appName, "CamDir", "0,0,0", tem_str, _MAX_FNAME, StrConfigIni);
            String str = tem_str.ToString();
            string[] strSubAry = str.Split(',');
            if (strSubAry?.Length <= 1)
            {
                str = "0,0,0";
                WritePrivateProfileString(appName, "CamDir", str, StrConfigIni);
                strSubAry = str.Split(',');
            }
            ArrCamDir[0] = Convert.ToInt32(strSubAry[0].ToString());
            ArrCamDir[1] = Convert.ToInt32(strSubAry[1].ToString());
            ArrCamDir[2] = Convert.ToInt32(strSubAry[2].ToString());

            appName = "GlobalParam";
            GetPrivateProfileString(appName, "ServerIP", "", tem_str, _MAX_FNAME, StrConfigIni);
            strBuffer = tem_str.ToString();
            if (tem_str.ToString() == "")
            {
                strBuffer = "192.168.1.1";
                WritePrivateProfileString(appName, "ServerIP", strBuffer, StrConfigIni);
            }
            GlobalParam.ServerIP = strBuffer;
            tem_str.Clear();

            GetPrivateProfileString(appName, "ServerPort", "", tem_str, _MAX_FNAME, StrConfigIni);
            strBuffer = tem_str.ToString();
            if (strBuffer == "")
            {
                strBuffer = "5100";
                WritePrivateProfileString(appName, "ServerPort", strBuffer, StrConfigIni);
            }
            GlobalParam.ServerPort = Convert.ToInt32(strBuffer);
            tem_str.Clear();

            GetPrivateProfileString(appName, "RPCServerIP", "", tem_str, _MAX_FNAME, StrConfigIni);
            strBuffer = tem_str.ToString();
            if (tem_str.ToString() == "")
            {
                strBuffer = "127.0.0.1";
                WritePrivateProfileString(appName, "RPCServerIP", strBuffer, StrConfigIni);
            }
            GlobalParam.RPCServerIP = strBuffer;
            tem_str.Clear();

            GetPrivateProfileString(appName, "RPCServerPort", "", tem_str, _MAX_FNAME, StrConfigIni);
            strBuffer = tem_str.ToString();
            if (strBuffer == "")
            {
                strBuffer = "20010";
                WritePrivateProfileString(appName, "RPCServerPort", strBuffer, StrConfigIni);
            }
            GlobalParam.RPCServerPort = Convert.ToInt32(strBuffer);
            tem_str.Clear();

        }

        public void GetOffset_TopCameraToPen()
        {
            //相机与吸头1的间距
            Axis.Pen[0].Offset_TopCamera_X = ArrPickPos[0].X - ArrPickPos[1].X + Axis.Pen[0].Offset_Base_X;
            Axis.Pen[0].Offset_TopCamera_Y = ArrPickPos[0].Y - ArrPickPos[1].Y + Axis.Pen[0].Offset_Base_Y;
            for (int i = 1; i < UserConfig.VacuumPenC; i++)
            {
                Axis.Pen[i].Offset_TopCamera_X = Axis.Pen[0].Offset_TopCamera_X - ArrPickPos[i + 2].X + ArrPickPos[2].X;
                Axis.Pen[i].Offset_TopCamera_Y = Axis.Pen[0].Offset_TopCamera_Y - ArrPickPos[i + 2].Y + ArrPickPos[2].Y;
            }
            Axis.Scanner.Offset_TopCamera_X = ArrPickPos[0].X - Axis.Scanner.Calibrating_X;
            Axis.Scanner.Offset_TopCamera_Y = ArrPickPos[0].Y - Axis.Scanner.Calibrating_Y;

            Axis.Altimeter.Offset_TopCamera_X = ArrPickPos[0].X - Axis.Altimeter.Calibrating_X;
            Axis.Altimeter.Offset_TopCamera_Y = ArrPickPos[0].Y - Axis.Altimeter.Calibrating_Y;
        }

        public void GetHeight_AltimeterToPen()
        {
            //测高仪与吸头的高度差
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                Axis.Pen[i].Altimeter_Z = Axis.Altimeter.Calibrating_Z - Axis.Pen[i].Calibrating_Z;
            } 
        }

        public bool WriteMachineInfo()
        {
            StringBuilder str = new StringBuilder();
            String appName = "ParameterSet";
            for (int n = 0; n < 6; n++)
            {
                WritePrivateProfileString(appName, "ccdx" + n, ArrCCDPrec[n].X.ToString("f3"), StrConfigIni);
                WritePrivateProfileString(appName, "ccdy" + n, ArrCCDPrec[n].Y.ToString("f3"), StrConfigIni);
                WritePrivateProfileString(appName, "posx" + n, ArrCCDPos[n].X.ToString("f3"), StrConfigIni);
                WritePrivateProfileString(appName, "posy" + n, ArrCCDPos[n].Y.ToString("f3"), StrConfigIni);
            }
            for (int n = 0; n < 6; n++)
            {
                WritePrivateProfileString(appName, "Pickposx" + n, Convert.ToString(ArrPickPos[n].X), StrConfigIni);
                WritePrivateProfileString(appName, "Pickposy" + n, Convert.ToString(ArrPickPos[n].Y), StrConfigIni);
            }
            WritePrivateProfileString(appName, "ScannerX", Convert.ToString(Axis.Scanner.Calibrating_X), StrConfigIni);
            WritePrivateProfileString(appName, "ScannerY", Convert.ToString(Axis.Scanner.Calibrating_Y), StrConfigIni);
            WritePrivateProfileString(appName, "AltimeterX", Convert.ToString(Axis.Altimeter.Calibrating_X), StrConfigIni);
            WritePrivateProfileString(appName, "AltimeterY", Convert.ToString(Axis.Altimeter.Calibrating_Y), StrConfigIni);
            WritePrivateProfileString(appName, "AltimeterZ", Convert.ToString(Axis.Altimeter.Calibrating_Z), StrConfigIni);
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                WritePrivateProfileString(appName, "Calibrating_Z" + (i + 1), Convert.ToString(Axis.Pen[i].Calibrating_Z), StrConfigIni);
            }
            return true;
        }
        public void ReadModelBox(String productPath)
        {
            String str, sub;
            String path2 = "";

            int pos = productPath.LastIndexOf('.');
            if (pos != -1)
            {
                path2 = productPath.Substring(0, pos);
            }

            String appName = "ModelBox";
            String keyName = "N";
            StringBuilder tem_str = new StringBuilder();
            GetPrivateProfileString(appName, keyName.ToString(), "17", tem_str, _MAX_FNAME, productPath);
            int.TryParse(tem_str.ToString(), out IModelTotal);

            if (IModelTotal != 17)
            {
                for (int i = IModelTotal; i < 17; i++)
                {
                    str = string.Format("192,308,828,976,0,模板{0:d},461,511,619,793,0,0.00,360.00,0.60,1,0.50,none,0,0.90", i);
                    WritePrivateProfileString("ModelBox", "M_" + i, str, productPath);
                }
                IModelTotal = 17;
                WritePrivateProfileString("ModelBox", "N", IModelTotal.ToString(), productPath);
            }

            ArrMod = new Model[IModelTotal];
            ArrModID = new HTuple[IModelTotal];
            for (int j = 0; j < IModelTotal; j++)
            {
                keyName = String.Format("M_{0:d}", j);
                GetPrivateProfileString(appName, keyName, "0,0,0,0", tem_str, _MAX_FNAME, productPath);

                ArrMod[j] = new Model();
                str = tem_str.ToString();
                string[] arrStr = str.Split(',');
                ArrMod[j].SeachBox = new SearchBox();
                int.TryParse(arrStr[0], out int tem_num);
                ArrMod[j].SeachBox.Cy = tem_num;
                int.TryParse(arrStr[1], out tem_num);
                ArrMod[j].SeachBox.Cx = tem_num;
                int.TryParse(arrStr[2], out tem_num);
                ArrMod[j].SeachBox.len1 = tem_num;
                int.TryParse(arrStr[3], out tem_num);
                ArrMod[j].SeachBox.len2 = tem_num;
                double.TryParse(arrStr[4], out double tem_double);
                ArrMod[j].SeachBox.Phi = tem_double;

                //框的名称
                ArrMod[j].Name = arrStr[5];

                //框1 模板框
                ArrMod[j].ModelBox = new SearchBox();
                int.TryParse(arrStr[6], out tem_num);
                ArrMod[j].ModelBox.Cy = tem_num;
                int.TryParse(arrStr[7], out tem_num);
                ArrMod[j].ModelBox.Cx = tem_num;
                int.TryParse(arrStr[8], out tem_num);
                ArrMod[j].ModelBox.len1 = tem_num;
                int.TryParse(arrStr[9], out tem_num);
                ArrMod[j].ModelBox.len2 = tem_num;
                double.TryParse(arrStr[10], out tem_double);
                ArrMod[j].ModelBox.Phi = tem_double;

                //框1模板参数
                double.TryParse(arrStr[11], out tem_double);
                ArrMod[j].AngleStart = tem_double;
                double.TryParse(arrStr[12], out tem_double);
                ArrMod[j].AngleExtent = tem_double;
                float.TryParse(arrStr[13], out float tem_float);
                ArrMod[j].MinScore = tem_float;
                int.TryParse(arrStr[14], out tem_num);
                ArrMod[j].NumMatches = tem_num;
                float.TryParse(arrStr[15], out tem_float);
                ArrMod[j].MaxOverLap = tem_float;
                ArrMod[j].SubPixs = arrStr[16];
                int.TryParse(arrStr[17], out tem_num);
                ArrMod[j].NumLevels = tem_num;
                double.TryParse(arrStr[18], out tem_double);
                ArrMod[j].GreedIness = tem_double;
                if (path2.Length > 0)
                {
                    ReadModeFile(j, path2 + "_" + j + ".shp");
                }
            }

            for (int k = 0; k < 10; k++)
            {
                keyName = String.Format("B_{0:d}", k);
                GetPrivateProfileString(appName, keyName, "", tem_str, _MAX_FNAME, productPath);
                str = tem_str.ToString();
                if (str.Length < 2)
                {
                    str = "0,50,50,100,100,150,150,250,250";
                    WritePrivateProfileString(appName, keyName, str, productPath);
                }

                string[] arrStr = str.Split(',');
                bool.TryParse(arrStr[0], out Bye[k]);
                By[k * 2] = new SearchBox();
                int.TryParse(arrStr[1], out int tem_num);
                By[k * 2].Cy = tem_num;
                int.TryParse(arrStr[2], out tem_num);
                By[k * 2].Cx = tem_num;
                int.TryParse(arrStr[3], out tem_num);
                By[k * 2].len1 = tem_num;
                int.TryParse(arrStr[4], out tem_num);
                By[k * 2].len2 = tem_num;
                By[k * 2 + 1] = new SearchBox();
                int.TryParse(arrStr[5], out tem_num);
                By[k * 2 + 1].Cy = tem_num;
                int.TryParse(arrStr[6], out tem_num);
                By[k * 2 + 1].Cx = tem_num;
                int.TryParse(arrStr[7], out tem_num);
                By[k * 2 + 1].len1 = tem_num;
                int.TryParse(arrStr[8], out tem_num);
                By[k * 2 + 1].len2 = tem_num;
            }
            //加载模板
            GetPrivateProfileString(appName, "Ben", "", tem_str, _MAX_FNAME, productPath);
            str = tem_str.ToString();
            if (str.Length < 1)
            {
                str = "0";
                WritePrivateProfileString(appName, "Ben", str, StrProductIni);
            }
            System.GC.Collect();
        }
        public void ReadModelCxyDefault()
        {
            StringBuilder tempStr = new StringBuilder();
            GetPrivateProfileString("ModelCxyr", "Cx", "0", tempStr, _MAX_FNAME, StrConfigIni);
            DModelCx = Convert.ToDouble(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("ModelCxyr", "Cy", "0", tempStr, _MAX_FNAME, StrConfigIni);
            DModelCy = Convert.ToDouble(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("ModelCxyr", "Cr", "0", tempStr, _MAX_FNAME, StrConfigIni);
            DModelCr = Convert.ToDouble(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("ModelCxyr", "Score", "0", tempStr, _MAX_FNAME, StrConfigIni);
            DScore = Convert.ToDouble(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("ModelCxyr", "MaxOverlap", "0", tempStr, _MAX_FNAME, StrConfigIni);
            DMaxOverlap = Convert.ToDouble(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("ModelCxyr", "Greediness", "0", tempStr, _MAX_FNAME, StrConfigIni);
            DGreediness = Convert.ToDouble(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("ModelCxyr", "AngLimit", "0", tempStr, _MAX_FNAME, StrConfigIni);
            DAngLimit = Convert.ToDouble(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("ModelCxyr", "Contrast", "auto", tempStr, _MAX_FNAME, StrConfigIni);
            IContrast = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("ModelCxyr", "minContrast", "atuo", tempStr, _MAX_FNAME, StrConfigIni);
            IMinContrast = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("ModelCxyr", "shutter", "10000", tempStr, _MAX_FNAME, StrConfigIni);
            IShutter = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("ModelCxyr", "gain", "2", tempStr, _MAX_FNAME, StrConfigIni);
            IGain = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("ModelCxyr", "SnapDelay", "0", tempStr, _MAX_FNAME, StrConfigIni);
            ISnapDelay = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();
        }
        public void SaveModelCxy()
        {
            WritePrivateProfileString("ModelCxyr", "Cx", DModelCx.ToString("f3"), StrProductIni);
            WritePrivateProfileString("ModelCxyr", "Cy", DModelCy.ToString("f3"), StrProductIni);
            WritePrivateProfileString("ModelCxyr", "Cr", DModelCr.ToString("f3"), StrProductIni);
            WritePrivateProfileString("ModelCxyr", "Cx2", DModelCx2.ToString("f3"), StrProductIni);
            WritePrivateProfileString("ModelCxyr", "Cy2", DModelCy2.ToString("f3"), StrProductIni);
            WritePrivateProfileString("ModelCxyr", "Cr2", DModelCr2.ToString("f3"), StrProductIni);
        }
        public void SaveModelCxy1()
        {
            WritePrivateProfileString("ModelCxyr", "Score", Convert.ToString(DScore), StrProductIni);
            WritePrivateProfileString("ModelCxyr", "MaxOverlap", Convert.ToString(DMaxOverlap), StrProductIni);
            WritePrivateProfileString("ModelCxyr", "Greediness", Convert.ToString(DGreediness), StrProductIni);
            WritePrivateProfileString("ModelCxyr", "AngLimit", Convert.ToString(DAngLimit), StrProductIni);
            WritePrivateProfileString("ModelCxyr", "Contrast", Convert.ToString(IContrast), StrProductIni);
            WritePrivateProfileString("ModelCxyr", "minContrast", Convert.ToString(IMinContrast), StrProductIni);
            WritePrivateProfileString("ModelCxyr", "shutter", Convert.ToString(IShutter), StrProductIni);
            WritePrivateProfileString("ModelCxyr", "gain", Convert.ToString(IGain), StrProductIni);
            WritePrivateProfileString("ModelCxyr", "SnapDelay", Convert.ToString(ISnapDelay), StrProductIni);
        }

        public void Save3DPar()
        {
            WritePrivateProfileString("ModelCxyr", "3DICType", Vision_3D.ICType.ToString(), StrProductIni);
            WritePrivateProfileString("ModelCxyr", "3D_X", Vision_3D.X.ToString("f3"), StrProductIni);
            WritePrivateProfileString("ModelCxyr", "3D_Y", Vision_3D.Y.ToString("f3"), StrProductIni);
            WritePrivateProfileString("ModelCxyr", "3D_Z", Vision_3D.Z.ToString("f3"), StrProductIni);
        }

        public void WriteIsDispMatch()
        {
            WritePrivateProfileString("ParameterSet", "IsDispMatch", IsDispMatch?"1":"0", StrConfigIni);
        }
        public void SaveModelBox(int ind, String path)
        {
            String str;
            str = String.Format("{0:d},{1:d},{2:d},{3:d},{4:d},{5},{6:d},{7:d},{8:d},{9:d},{10:d},{11:f2},{12:f2},{13:f2},{14:d},{15:f2},{16},{17:d},{18:f2}",
                Convert.ToInt32(ArrMod[ind].SeachBox.Cy.D), Convert.ToInt32(ArrMod[ind].SeachBox.Cx.D), Convert.ToInt32(ArrMod[ind].SeachBox.len1.D), Convert.ToInt32(ArrMod[ind].SeachBox.len2.D), 0,
                ArrMod[ind].Name,
                Convert.ToInt32(ArrMod[ind].ModelBox.Cy.D), Convert.ToInt32(ArrMod[ind].ModelBox.Cx.D), Convert.ToInt32(ArrMod[ind].ModelBox.len1.D), Convert.ToInt32(ArrMod[ind].ModelBox.len2.D), 0,
                ArrMod[ind].AngleStart, ArrMod[ind].AngleExtent, ArrMod[ind].MinScore, ArrMod[ind].NumMatches,
                ArrMod[ind].MaxOverLap, ArrMod[ind].SubPixs, ArrMod[ind].NumLevels, ArrMod[ind].GreedIness);

            WritePrivateProfileString("ModelBox", "M_" + ind, str, path);

            if (ind == GlobConstData.ST_MODELICPOS)
            {
                for (int i = 6; i < 9; i++)
                {
                    WritePrivateProfileString("ModelBox", "M_" + i, str, path);
                }
                //ReadModelBox(StrProductIni);
            }

            for (int i = 0; i < 10; i++)
            {
                str = String.Format("{0:d},{1:d},{2:d},{3:d},{4:d},{5},{6:d},{7:d},{8:d}", Bye[i] == true ? 1 : 0, Convert.ToInt32(By[i * 2].Cy.D), Convert.ToInt32(By[i * 2].Cx.D),
                Convert.ToInt32(By[i * 2].len1.D), Convert.ToInt32(By[i * 2].len2.D), Convert.ToInt32(By[i * 2 + 1].Cy.D), Convert.ToInt32(By[i * 2 + 1].Cx.D),
                Convert.ToInt32(By[i * 2 + 1].len1.D), Convert.ToInt32(By[i * 2 + 1].len2.D));
                WritePrivateProfileString("ModelBox", "B_" + i, str, path);
            }
        }
        //IC图象位置【产品】 
        public void ReadModelCxy()
        {
            StringBuilder tempStr = new StringBuilder();
            string strBuffer;

            GetPrivateProfileString("ModelCxyr", "Cx", "0", tempStr, _MAX_FNAME, StrProductIni);
            DModelCx = Convert.ToDouble(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("ModelCxyr", "Cy", "0", tempStr, _MAX_FNAME, StrProductIni);
            DModelCy = Convert.ToDouble(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("ModelCxyr", "Cr", "0", tempStr, _MAX_FNAME, StrProductIni);
            DModelCr = Convert.ToDouble(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("ModelCxyr", "Score", "0", tempStr, _MAX_FNAME, StrProductIni);
            DScore = Convert.ToDouble(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("ModelCxyr", "MaxOverlap", "0", tempStr, _MAX_FNAME, StrProductIni);
            DMaxOverlap = Convert.ToDouble(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("ModelCxyr", "Greediness", "0", tempStr, _MAX_FNAME, StrProductIni);
            DGreediness = Convert.ToDouble(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("ModelCxyr", "AngLimit", "0", tempStr, _MAX_FNAME, StrProductIni);
            DAngLimit = Convert.ToDouble(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("ModelCxyr", "Contrast", "auto", tempStr, _MAX_FNAME, StrProductIni);
            IContrast = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("ModelCxyr", "minContrast", "atuo", tempStr, _MAX_FNAME, StrProductIni);
            IMinContrast = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("ModelCxyr", "shutter", "10000", tempStr, _MAX_FNAME, StrProductIni);
            IShutter = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("ModelCxyr", "gain", "2", tempStr, _MAX_FNAME, StrProductIni);
            IGain = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("ModelCxyr", "SnapDelay", "0", tempStr, _MAX_FNAME, StrProductIni);
            ISnapDelay = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();
             
            GetPrivateProfileString("ModelCxyr", "Cx2", "0", tempStr, _MAX_FNAME, StrProductIni);
            DModelCx2 = Convert.ToDouble(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("ModelCxyr", "Cy2", "0", tempStr, _MAX_FNAME, StrProductIni);
            DModelCy2 = Convert.ToDouble(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("ModelCxyr", "Cr2", "0", tempStr, _MAX_FNAME, StrProductIni);
            DModelCr2 = Convert.ToDouble(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString("ModelCxyr", "3DICType", "", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            if (tempStr.ToString() == "")
            {
                strBuffer = "0";
                WritePrivateProfileString("ModelCxyr", "3DICType", strBuffer, StrProductIni);
            }
            Vision_3D.ICType = Convert.ToInt32(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString("ModelCxyr", "3D_X", "", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            if (tempStr.ToString() == "")
            {
                strBuffer = "0";
                WritePrivateProfileString("ModelCxyr", "3D_X", strBuffer, StrProductIni);
            }
            Vision_3D.X = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString("ModelCxyr", "3D_Y", "", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            if (tempStr.ToString() == "")
            {
                strBuffer = "0";
                WritePrivateProfileString("ModelCxyr", "3D_Y", strBuffer, StrProductIni);
            }
            Vision_3D.Y = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString("ModelCxyr", "3D_Z", "", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            if (tempStr.ToString() == "")
            {
                strBuffer = "0";
                WritePrivateProfileString("ModelCxyr", "3D_Z", strBuffer, StrProductIni);
            }
            Vision_3D.Z = Convert.ToDouble(strBuffer);
            tempStr.Clear();

        }
        public bool ReadModeFile(int ind, String path)
        {
            ModelIDBy = null;
            ArrModID[ind] = null;
            if (_HalconImgUtil.LoadModel(ind, path, ref ModelIDBy))
            {
                ArrModID[ind] = ModelIDBy;
                return true;
            }
            else
            {
                return false;
            }
        }
        //public bool ReadModeFile(int ind)
        //{
        //    String path2 = "";

        //    int pos = StrProductIni.LastIndexOf('.');
        //    if (pos != -1)
        //    {
        //        path2 = StrProductIni.Substring(0, pos);
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //    for (int i = 0; i < ArrModID.Length; i++)
        //    {
        //        ArrModID[i] = null;
        //    }
        //    if (_HalconImgUtil.LoadModel(ind, path2 + "_" + ind + ".shp", ref ArrModID[ind]))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //public void ReadModeBoxDefault()
        //{
        //    ReadModelBox(StrConfigIni);
        //}
        //public void SaveModelBoxDefault()
        //{
        //    for (int n = 0; n < IModelTotal; n++)
        //    {
        //        SaveModelBox(n, StrConfigIni);
        //    }
        //}
        public void SaveLightVal(int ind)
        {
            WritePrivateProfileString("DrawBox", "L0" + ind, Convert.ToString(ArrLampBox[ind].ILightUp), StrProductIni);
            WritePrivateProfileString("DrawBox", "L1" + ind, Convert.ToString(ArrLampBox[ind].ILightDown), StrProductIni);
            WritePrivateProfileString("DrawBox", "MW" + ind, Convert.ToString(ArrLampBox[ind].IMW), StrProductIni);
            WritePrivateProfileString("DrawBox", "MH" + ind, Convert.ToString(ArrLampBox[ind].IMH), StrProductIni);
            WritePrivateProfileString("DrawBox", "SW" + ind, Convert.ToString(ArrLampBox[ind].ISW), StrProductIni);
            WritePrivateProfileString("DrawBox", "SH" + ind, Convert.ToString(ArrLampBox[ind].ISH), StrProductIni);
            WritePrivateProfileString("DrawBox", "C" + ind, Convert.ToString(ArrLampBox[ind].IC), StrProductIni);
        }
        public void ReadLightVal()
        {
            StringBuilder tempStr = new StringBuilder();
            GetPrivateProfileString("DrawBox", "N", "0", tempStr, _MAX_FNAME, StrProductIni);
            IBoxCount = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();
            if (IBoxCount < 20)
            {
                for (int i = IBoxCount; i < 20; i++)
                {
                    SaveLightVal(i);
                }
                IBoxCount = 20;
                WritePrivateProfileString("DrawBox", "N", IBoxCount.ToString(), StrProductIni);
            }
            for (int n = 0; n < IBoxCount; n++)
            {
                ArrLampBox[n] = new LampBox();
                GetPrivateProfileString("DrawBox", "L0" + Convert.ToString(n), "0", tempStr, _MAX_FNAME, StrProductIni);
                ArrLampBox[n].ILightUp = Convert.ToInt32(tempStr.ToString());
                tempStr.Clear();
                GetPrivateProfileString("DrawBox", "L1" + Convert.ToString(n), "0", tempStr, _MAX_FNAME, StrProductIni);
                ArrLampBox[n].ILightDown = Convert.ToInt32(tempStr.ToString());
                tempStr.Clear();
                GetPrivateProfileString("DrawBox", "MW" + Convert.ToString(n), "0", tempStr, _MAX_FNAME, StrProductIni);
                ArrLampBox[n].IMW = Convert.ToInt32(tempStr.ToString());
                tempStr.Clear();
                GetPrivateProfileString("DrawBox", "MH" + Convert.ToString(n), "0", tempStr, _MAX_FNAME, StrProductIni);
                ArrLampBox[n].IMH = Convert.ToInt32(tempStr.ToString());
                tempStr.Clear();
                GetPrivateProfileString("DrawBox", "SW" + Convert.ToString(n), "0", tempStr, _MAX_FNAME, StrProductIni);
                ArrLampBox[n].ISW = Convert.ToInt32(tempStr.ToString());
                tempStr.Clear();
                GetPrivateProfileString("DrawBox", "SH" + Convert.ToString(n), "0", tempStr, _MAX_FNAME, StrProductIni);
                ArrLampBox[n].ISH = Convert.ToInt32(tempStr.ToString());
                tempStr.Clear();
                GetPrivateProfileString("DrawBox", "C" + Convert.ToString(n), "0", tempStr, _MAX_FNAME, StrProductIni);
                ArrLampBox[n].IC = Convert.ToInt32(tempStr.ToString());
                tempStr.Clear();
            }
        }
        //读取所有位置数据【默认】
        public void ReadAllPosDefault()
        {
            StringBuilder str_temp = new StringBuilder(), str_data = new StringBuilder();
            int n = 0;
            String appName = "FixInPos", keyName = "FixInCount";

            GetPrivateProfileString(appName, keyName, "8", str_temp, _MAX_FNAME, StrConfigIni);
            IFixInCount = Convert.ToInt32(str_temp.ToString());
            ArrFixIn = new FixPosIN[IFixInCount];

            String str;
            String[] strSubAry;
            for (n = 0; n < IFixInCount; n++)
            {
                GetPrivateProfileString(appName, "FixIn_" + n, "0,0,0,0,0,0", str_temp, _MAX_FNAME, StrConfigIni);
                str = str_temp.ToString();
                strSubAry = str.Split(',');
                ArrFixIn[n] = new FixPosIN();
                double posX, posY;
                posX = double.Parse(strSubAry[0]);//x
                posY = double.Parse(strSubAry[1]);//y
                ArrFixIn[n].P = new PointD(posX, posY);
                ArrFixIn[n].ICol = Convert.ToInt32(strSubAry[2]);//行
                ArrFixIn[n].IRow = Convert.ToInt32(strSubAry[3]);//列
                ArrFixIn[n].DRolDis = double.Parse(strSubAry[4]);//行间距
                ArrFixIn[n].DColDis = double.Parse(strSubAry[5]);//列间距

                if (n < 3)
                {
                    Axis.Position_X.trayFirst[n] = ArrFixIn[n].P.X;
                    Axis.Position_Y.trayFirst[n] = ArrFixIn[n].P.Y;
                }
                else if (n == 3)
                {
                    Axis.Position_X.trayEnd[0] = ArrFixIn[n].P.X;
                    Axis.Position_Y.trayEnd[0] = ArrFixIn[n].P.Y;
                }
            }

            //位置2#
            appName = "FixOutPos";

            ArrFixOut = new FixPosOut[UserConfig.AllScketC];
            ArrFixOutBack = new FixPosOut[UserConfig.AllScketC];

            for (n = 0; n < UserConfig.AllScketC; n++)
            {
                GetPrivateProfileString(appName, "FixOut_" + n, "0,0,0,0,0", str_temp, _MAX_FNAME, StrProductIni);
                str = str_temp.ToString();
                strSubAry = str.Split(',');
                ArrFixOut[n] = new FixPosOut
                {
                    IGrop = Convert.ToInt32(strSubAry[0]),//组
                    INo = Convert.ToInt32(strSubAry[1]),//序号
                    IsEn = bool.Parse(strSubAry[2])//使能
                };
                double posX, posY;
                posX = double.Parse(strSubAry[3]);//x
                posY = double.Parse(strSubAry[4]);//y
                ArrFixOut[n].P = new PointD(posX, posY);

                int k = UserConfig.ScketUnitC;
                Axis.Group[n / k].Unit[n % k].TopCamera_X = ArrFixOut[n].P.X;
                Axis.Group[n / k].Unit[n % k].TopCamera_Y = ArrFixOut[n].P.Y;
            }
        }
        public bool WriteFixInPos(int ind)
        {
            StringBuilder str = new StringBuilder();
            String appName = "FixInPos";
            str.Append(Convert.ToString(ArrFixIn[ind].P.X) + ",");
            str.Append(Convert.ToString(ArrFixIn[ind].P.Y) + ",");
            str.Append(Convert.ToString(ArrFixIn[ind].IRow) + ",");
            str.Append(Convert.ToString(ArrFixIn[ind].ICol) + ",");
            str.Append(Convert.ToString(ArrFixIn[ind].DRolDis) + ",");
            str.Append(Convert.ToString(ArrFixIn[ind].DColDis) + ",");

            WritePrivateProfileString(appName, "FixIn_" + ind, str.ToString(), StrProductIni);
            return true;
        }
        public bool WriteFixOutPos(int ind)
        {
            StringBuilder str = new StringBuilder();
            String appName = "FixOutPos";
            str.Append(Convert.ToString(ArrFixOut[ind].IGrop) + ",");
            str.Append(Convert.ToString(ArrFixOut[ind].INo) + ",");
            str.Append(Convert.ToString(ArrFixOut[ind].IsEn ? "1" : "0") + ",");
            str.Append(Convert.ToString(ArrFixOut[ind].P.X) + ",");
            str.Append(Convert.ToString(ArrFixOut[ind].P.Y));

            WritePrivateProfileString(appName, "FixOut_" + ind, str.ToString(), StrProductIni);
            return true;
        }
      
        /// <summary>
        /// 读盘参数配置 
        /// </summary>
        public void ReadPanVal()
        {
            StringBuilder tempStr = new StringBuilder();
            GetPrivateProfileString("PanPar", "nCol", "0", tempStr, _MAX_FNAME, StrProductIni);
            TrayD.ColC = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("PanPar", "nColdis", "0", tempStr, _MAX_FNAME, StrProductIni);
            TrayD.Col_Space = double.Parse(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("PanPar", "nRol", "0", tempStr, _MAX_FNAME, StrProductIni);
            TrayD.RowC = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();
            GetPrivateProfileString("PanPar", "nRoldis", "0", tempStr, _MAX_FNAME, StrProductIni);
            TrayD.Row_Space = double.Parse(tempStr.ToString());
            tempStr.Clear();
        }

        /// <summary>
        /// 写盘参数配置
        /// </summary>
        public bool SavePanVal(bool flag = true)
        {
            TrayD.TIC_EndColN[0] = TrayD.ColC;
            TrayD.TIC_EndRowN[0] = TrayD.RowC;
            TrayD.TIC_EndColN[1] = TrayRotateDir[1] != 0 ? TrayD.RowC : TrayD.ColC;
            TrayD.TIC_EndRowN[1] = TrayRotateDir[1] != 0 ? TrayD.ColC : TrayD.RowC;
            WritePrivateProfileString("Statics", "nPan1EndRow", Convert.ToString(TrayD.TIC_EndRowN[0]), StrStaticIni);
            WritePrivateProfileString("Statics", "nPan1EndCol", Convert.ToString(TrayD.TIC_EndColN[0]), StrStaticIni);
            WritePrivateProfileString("Statics", "nPan2EndRow", Convert.ToString(TrayD.TIC_EndRowN[1]), StrStaticIni);
            WritePrivateProfileString("Statics", "nPan2EndCol", Convert.ToString(TrayD.TIC_EndColN[1]), StrStaticIni);

            WritePrivateProfileString("PanPar", "nCol", Convert.ToString(TrayD.ColC), StrProductIni);
            WritePrivateProfileString("PanPar", "nColdis", Convert.ToString(TrayD.Col_Space), StrProductIni);
            WritePrivateProfileString("PanPar", "nRol", Convert.ToString(TrayD.RowC), StrProductIni);
            WritePrivateProfileString("PanPar", "nRoldis", Convert.ToString(TrayD.Row_Space), StrProductIni);
            if (flag)
            {
                TrayState.TrayStateUpdate(true);
            }
            return true;
        }

        /// <summary>
        /// 读旋转角度参数
        /// </summary>
        public void ReadRotateAngle()
        {
            String appName = "RotateAngle";
            StringBuilder str = new StringBuilder();

            GetPrivateProfileString(appName, "TIC_Brede", "0", str, _MAX_FNAME, StrProductIni);
            RotateAngle.TIC_Brede = Convert.ToDouble(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "LIC_Brede", "0", str, _MAX_FNAME, StrProductIni);
            RotateAngle.LIC_Brede = Convert.ToDouble(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "TIC_Tube", "0", str, _MAX_FNAME, StrProductIni);
            RotateAngle.TIC_Tube = Convert.ToDouble(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "LIC_Tube", "0", str, _MAX_FNAME, StrProductIni);
            RotateAngle.LIC_Tube = Convert.ToDouble(str.ToString());
            str.Clear();

            GetPrivateProfileString(appName, "Base_Socket", "0", str, _MAX_FNAME, StrProductIni);
            RotateAngle.Base_Socket = Convert.ToDouble(str.ToString());
            str.Clear();

            for (int i = 0; i < 2; i++)
            {
                GetPrivateProfileString(appName, "TIC_Tray" + (i + 1), "0", str, _MAX_FNAME, StrProductIni);
                RotateAngle.TIC_Tray[i] = Convert.ToDouble(str.ToString());
                str.Clear();
            }

            for (int i = 0; i < 3; i++)
            {
                GetPrivateProfileString(appName, "LIC_Tray" + (i + 1), "0", str, _MAX_FNAME, StrProductIni);
                RotateAngle.LIC_Tray[i] = Convert.ToDouble(str.ToString());
                str.Clear();
            }
        }
        /// <summary>
        /// 写旋转角度参数
        /// </summary>
        public bool SaveRotateAnglel()
        {
            String appName = "RotateAngle";

            WritePrivateProfileString(appName, "TIC_Brede", Convert.ToString(RotateAngle.TIC_Brede), StrProductIni);
            WritePrivateProfileString(appName, "LIC_Brede", Convert.ToString(RotateAngle.LIC_Brede), StrProductIni);
            WritePrivateProfileString(appName, "TIC_Tube", Convert.ToString(RotateAngle.TIC_Tube), StrProductIni);
            WritePrivateProfileString(appName, "LIC_Tube", Convert.ToString(RotateAngle.LIC_Tube), StrProductIni);
            WritePrivateProfileString(appName, "Base_Socket", Convert.ToString(RotateAngle.Base_Socket), StrProductIni);

            for (int i = 0; i < 2; i++)
            {
                WritePrivateProfileString(appName, "TIC_Tray" + (i + 1), Convert.ToString(RotateAngle.TIC_Tray[i]), StrProductIni);
            }

            for (int i = 0; i < 3; i++)
            {
                WritePrivateProfileString(appName, "LIC_Tray" + (i + 1), Convert.ToString(RotateAngle.LIC_Tray[i]), StrProductIni);
            }
            return true;
        }

        /// <summary>
        /// 读设备参数配置
        /// </summary>
        public void ReadDevParaVal()
        {
            String appName = "DevPara";
            StringBuilder tempStr = new StringBuilder();
            String[]      arrSubStr;
            GetPrivateProfileString(appName, "N", "0", tempStr, _MAX_FNAME, StrProductIni);
            int num = Convert.ToInt32(tempStr.ToString());
            ArrDevParas = new StuPara[num];

            GetPrivateProfileString(appName, "nPanLayIC", "0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrDevParas[0] = new StuPara();
            ArrDevParas[0].IDataType = 1;
            ArrDevParas[0].StrParaName = arrSubStr[0];
            ArrDevParas[0].DData = HeightVal.Tray_LayIC = double.Parse(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nPanTakeIC", "0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrDevParas[1] = new StuPara();
            ArrDevParas[1].IDataType = 1;
            ArrDevParas[1].StrParaName = arrSubStr[0];
            ArrDevParas[1].DData = HeightVal.Tray_TakeIC = double.Parse(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nSeatLayIC", "0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrDevParas[2] = new StuPara();
            ArrDevParas[2].IDataType = 1;
            ArrDevParas[2].StrParaName = arrSubStr[0];
            ArrDevParas[2].DData = HeightVal.DownSeat_LayIC = double.Parse(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nSeatTakeIC", "0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrDevParas[3] = new StuPara();
            ArrDevParas[3].IDataType = 1;
            ArrDevParas[3].StrParaName = arrSubStr[0];
            ArrDevParas[3].DData = HeightVal.DownSeat_TakeIC = double.Parse(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nTubeTakeIC", "0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrDevParas[4] = new StuPara();
            ArrDevParas[4].IDataType = 1;
            ArrDevParas[4].StrParaName = arrSubStr[0];
            ArrDevParas[4].DData = HeightVal.Tube_TakeIC = double.Parse(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nBredeTakeIC", "0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrDevParas[5] = new StuPara();
            ArrDevParas[5].IDataType = 1;
            ArrDevParas[5].StrParaName = arrSubStr[0];
            ArrDevParas[5].DData = HeightVal.Brede_TakeIC = double.Parse(arrSubStr[1].ToString());

            tempStr.Clear();
            GetPrivateProfileString(appName, "nBredeLayIC", "0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrDevParas[6] = new StuPara();
            ArrDevParas[6].IDataType = 1;
            ArrDevParas[6].StrParaName = arrSubStr[0];
            ArrDevParas[6].DData = HeightVal.Brede_LayIC = double.Parse(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nTubeSpace", "0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrDevParas[7] = new StuPara();
            ArrDevParas[7].IDataType = 1;
            ArrDevParas[7].StrParaName = arrSubStr[0];
            ArrDevParas[7].DData = Axis.Position_X.tubespace = double.Parse(arrSubStr[1].ToString()); ;   
            tempStr.Clear();

            GetPrivateProfileString(appName, "nTakeDelay", "0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrDevParas[8] = new StuPara();
            ArrDevParas[8].IDataType = 0;
            ArrDevParas[8].StrParaName = arrSubStr[0];
            ArrDevParas[8].DData = AutoTiming.SeatTakeDelay = Convert.ToUInt32(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nBuzzer", "0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrDevParas[9] = new StuPara();
            ArrDevParas[9].IDataType = 0;
            ArrDevParas[9].StrParaName = arrSubStr[0];
            ArrDevParas[9].DData = AutoTiming.BuzzerDuration = Convert.ToUInt32(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nTimeOut", "0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrDevParas[10] = new StuPara();
            ArrDevParas[10].IDataType = 0;
            ArrDevParas[10].StrParaName = arrSubStr[0];
            ArrDevParas[10].IData = AutoTiming.DownTimeOut = Convert.ToUInt32(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nBlowTime", "0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrDevParas[11] = new StuPara();
            ArrDevParas[11].IDataType = 0;
            ArrDevParas[11].StrParaName = arrSubStr[0];
            ArrDevParas[11].DData = AutoTiming.BlowDuration = Convert.ToUInt32(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nSafe", "0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrDevParas[12] = new StuPara();
            ArrDevParas[12].IDataType = 1;
            ArrDevParas[12].StrParaName = arrSubStr[0];
            ArrDevParas[12].DData = HeightVal.Safe = double.Parse(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nDeviate", "0.1", tempStr, _MAX_FNAME, StrProductIni);
            UserTask.Deviate = Convert.ToDouble(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nDownDelay", "0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrDevParas[14] = new StuPara();
            ArrDevParas[14].IDataType = 0;
            ArrDevParas[14].StrParaName = arrSubStr[0];
            ArrDevParas[14].IData = AutoTiming.DownDelay = Convert.ToUInt32(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nVacuumDuration", "0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrDevParas[15] = new StuPara();
            ArrDevParas[15].IDataType = 0;
            ArrDevParas[15].StrParaName = arrSubStr[0];
            ArrDevParas[15].IData = AutoTiming.VacuumDuration = Convert.ToUInt32(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nBlowDelay", "0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrDevParas[16] = new StuPara();
            ArrDevParas[16].IDataType = 0;
            ArrDevParas[16].StrParaName = arrSubStr[0];
            ArrDevParas[16].IData = AutoTiming.BlowDelay = Convert.ToUInt32(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nNGContinue", "0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrDevParas[17] = new StuPara();
            ArrDevParas[17].IDataType = 0;
            ArrDevParas[17].StrParaName = arrSubStr[0];
            UserTask.NGContinueC = Convert.ToInt32(arrSubStr[1].ToString());
            ArrDevParas[17].IData = (uint)UserTask.NGContinueC;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nNGReBurn", "0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrDevParas[18] = new StuPara();
            ArrDevParas[18].IDataType = 0;
            ArrDevParas[18].StrParaName = arrSubStr[0];
            UserTask.NGReBurnC = Convert.ToInt32(arrSubStr[1].ToString());
            ArrDevParas[18].IData = (uint)UserTask.NGReBurnC;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nBredeTakeDelay", "0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrDevParas[19] = new StuPara();
            ArrDevParas[19].IDataType = 0;
            ArrDevParas[19].StrParaName = arrSubStr[0];
            ArrDevParas[19].IData = AutoTiming.BredeTakeDelay = Convert.ToUInt32(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nNGScketC_Shut", "0", tempStr, _MAX_FNAME, StrProductIni);
            UserTask.NGScketC_Shut = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nNGContinueC_Shut", "0", tempStr, _MAX_FNAME, StrProductIni);
            UserTask.NGContinueC_Shut = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nNGAllC_Shut", "0", tempStr, _MAX_FNAME, StrProductIni);
            UserTask.NGAllC_Shut = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nTubeTakeDelay", "500", tempStr, _MAX_FNAME, StrProductIni);
            AutoTiming.TubeTakeDelay = Convert.ToUInt32(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nTubeTimeOut", "3000", tempStr, _MAX_FNAME, StrProductIni);
            AutoTiming.TubeTimeOut = Convert.ToUInt32(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nMPa", "0.1", tempStr, _MAX_FNAME, StrProductIni);
            UserTask.MPa = Convert.ToDouble(tempStr.ToString());
            tempStr.Clear();
        }
        /// <summary>
        /// 写设备参数配置
        /// </summary>
        public bool SaveDevParaVal()
        {
            String appName = "DevPara";

            WritePrivateProfileString(appName, "nPanLayIC", ArrDevParas[0].StrParaName + "," + Convert.ToString(HeightVal.Tray_LayIC), StrProductIni);
            WritePrivateProfileString(appName, "nPanTakeIC", ArrDevParas[1].StrParaName + "," + Convert.ToString(HeightVal.Tray_TakeIC), StrProductIni);
            WritePrivateProfileString(appName, "nSeatLayIC", ArrDevParas[2].StrParaName + "," + Convert.ToString(HeightVal.DownSeat_LayIC), StrProductIni);
            WritePrivateProfileString(appName, "nSeatTakeIC", ArrDevParas[3].StrParaName + "," + Convert.ToString(HeightVal.DownSeat_TakeIC), StrProductIni);
            WritePrivateProfileString(appName, "nTubeTakeIC", ArrDevParas[4].StrParaName + "," + Convert.ToString(HeightVal.Tube_TakeIC), StrProductIni);
            WritePrivateProfileString(appName, "nBredeTakeIC", ArrDevParas[5].StrParaName + "," + Convert.ToString(HeightVal.Brede_TakeIC), StrProductIni);
            WritePrivateProfileString(appName, "nBredeLayIC", ArrDevParas[6].StrParaName + "," + Convert.ToString(HeightVal.Brede_LayIC), StrProductIni);
            WritePrivateProfileString(appName, "nTubeSpace", ArrDevParas[7].StrParaName + "," + Convert.ToString(Axis.Position_X.tubespace), StrProductIni);
            WritePrivateProfileString(appName, "nTakeDelay", ArrDevParas[8].StrParaName + "," + Convert.ToString(AutoTiming.SeatTakeDelay), StrProductIni);
            WritePrivateProfileString(appName, "nBuzzer", ArrDevParas[9].StrParaName + "," + Convert.ToString(AutoTiming.BuzzerDuration), StrProductIni);
            WritePrivateProfileString(appName, "nTimeOut", ArrDevParas[10].StrParaName + "," + Convert.ToString(AutoTiming.DownTimeOut), StrProductIni);
            WritePrivateProfileString(appName, "nBlowTime", ArrDevParas[11].StrParaName + "," + Convert.ToString(AutoTiming.BlowDuration), StrProductIni);
            WritePrivateProfileString(appName, "nSafe", ArrDevParas[12].StrParaName + "," + Convert.ToString(HeightVal.Safe), StrProductIni);
            WritePrivateProfileString(appName, "nDeviate", Convert.ToString(UserTask.Deviate), StrProductIni);
            WritePrivateProfileString(appName, "nDownDelay", ArrDevParas[14].StrParaName + "," + Convert.ToString(AutoTiming.DownDelay), StrProductIni);
            WritePrivateProfileString(appName, "nVacuumDuration", ArrDevParas[15].StrParaName + "," + Convert.ToString(AutoTiming.VacuumDuration), StrProductIni);
            WritePrivateProfileString(appName, "nBlowDelay", ArrDevParas[16].StrParaName + "," + Convert.ToString(AutoTiming.BlowDelay), StrProductIni);
            WritePrivateProfileString(appName, "nNGContinue", ArrDevParas[17].StrParaName + "," + Convert.ToString(UserTask.NGContinueC), StrProductIni);
            WritePrivateProfileString(appName, "nNGReBurn", ArrDevParas[18].StrParaName + "," + Convert.ToString(UserTask.NGReBurnC), StrProductIni);
            WritePrivateProfileString(appName, "nBredeTakeDelay", ArrDevParas[19].StrParaName + "," + Convert.ToString(AutoTiming.BredeTakeDelay), StrProductIni);
            WritePrivateProfileString(appName, "nNGScketC_Shut", Convert.ToString(UserTask.NGScketC_Shut), StrProductIni);
            WritePrivateProfileString(appName, "nNGContinueC_Shut", Convert.ToString(UserTask.NGContinueC_Shut), StrProductIni);
            WritePrivateProfileString(appName, "nNGAllC_Shut", Convert.ToString(UserTask.NGAllC_Shut), StrProductIni);
            WritePrivateProfileString(appName, "nTubeTakeDelay", Convert.ToString(AutoTiming.TubeTakeDelay), StrProductIni);
            WritePrivateProfileString(appName, "nTubeTimeOut", Convert.ToString(AutoTiming.TubeTimeOut), StrProductIni);
            WritePrivateProfileString(appName, "nMPa", Convert.ToString(UserTask.MPa), StrProductIni);
            SavePanVal();
            return true;
        }

        /// <summary>
        /// 读轴参数配置
        /// </summary>
        public void ReadAxisParaVal()
        {
            if (!File.Exists(StrRunModelIni))
            {
                FileStream file = File.Create(StrRunModelIni);
                file.Close();
            }
            for (int i = 0; i < 2; i++)
            {
                runModel_s[i] = new RunModel_class();
            }
            String appName = "AxisPara";
            String appName1 = "RunModelPara";
            String appName2 = "FuncSwit";
            StringBuilder tempStr = new StringBuilder();
            String[] arrSubStr;

            GetPrivateProfileString(appName2, "nRunModel", "1", tempStr, _MAX_FNAME, StrProductIni);
            Auto_Flag.RunModel = tempStr.ToString() == "0" ? 0 : 1;
            tempStr.Clear();

            GetPrivateProfileString(appName1, "nSpeedX", "2500,1500", tempStr, _MAX_FNAME, StrRunModelIni);
            arrSubStr = tempStr.ToString().Split(',');
            runModel_s[0].SpeedX = double.Parse(arrSubStr[0]);
            runModel_s[1].SpeedX = double.Parse(arrSubStr[1].ToString());
            Axis.runPrm_X.speed = runModel_s[Auto_Flag.RunModel].SpeedX; 
            tempStr.Clear();

            GetPrivateProfileString(appName, "nFastSpeedX", "30", tempStr, _MAX_FNAME, StrConfigIni);
            Axis.jogPrm_X.velHigh = double.Parse(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nSlowSpeedX", "10", tempStr, _MAX_FNAME, StrConfigIni);
            Axis.jogPrm_X.velLow = double.Parse(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName1, "nLongAcceSpeedX", "35,30", tempStr, _MAX_FNAME, StrRunModelIni);
            arrSubStr = tempStr.ToString().Split(',');
            runModel_s[0].LongAcceSpeedX = double.Parse(arrSubStr[0].ToString());
            runModel_s[1].LongAcceSpeedX = double.Parse(arrSubStr[1].ToString());
            Axis.runPrm_X.longAcc = runModel_s[Auto_Flag.RunModel].LongAcceSpeedX;
            tempStr.Clear();

            GetPrivateProfileString(appName1, "nLongDecSpeedX", "35,30", tempStr, _MAX_FNAME, StrRunModelIni);
            arrSubStr = tempStr.ToString().Split(',');
            runModel_s[0].LongDecSpeedX = double.Parse(arrSubStr[0].ToString());
            runModel_s[1].LongDecSpeedX = double.Parse(arrSubStr[1].ToString());
            Axis.runPrm_X.longDec = runModel_s[Auto_Flag.RunModel].LongDecSpeedX;
            tempStr.Clear();

            GetPrivateProfileString(appName1, "nShortAcceSpeedX", "20,18", tempStr, _MAX_FNAME, StrRunModelIni);
            arrSubStr = tempStr.ToString().Split(',');
            runModel_s[0].ShortAcceSpeedX = double.Parse(arrSubStr[0].ToString());
            runModel_s[1].ShortAcceSpeedX = double.Parse(arrSubStr[1].ToString());
            Axis.runPrm_X.shortAcc = runModel_s[Auto_Flag.RunModel].ShortAcceSpeedX;
            tempStr.Clear();

            GetPrivateProfileString(appName1, "nShortDecSpeedX", "20,18", tempStr, _MAX_FNAME, StrRunModelIni);
            arrSubStr = tempStr.ToString().Split(',');
            runModel_s[0].ShortDecSpeedX = double.Parse(arrSubStr[0].ToString());
            runModel_s[1].ShortDecSpeedX = double.Parse(arrSubStr[1].ToString());
            Axis.runPrm_X.shortDec = runModel_s[Auto_Flag.RunModel].ShortDecSpeedX;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nLongMinX", "40", tempStr, _MAX_FNAME, StrConfigIni);
            Axis.runPrm_X.longMin = double.Parse(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nHomePotFixX", "3", tempStr, _MAX_FNAME, StrConfigIni);
            Axis.homePrm_X.homeOffset = double.Parse(tempStr.ToString());
            Axis.Position_X.MinVal = -Axis.homePrm_X.homeOffset;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nMaxLimitX", "810", tempStr, _MAX_FNAME, StrConfigIni);
            Axis.Position_X.MaxVal = double.Parse(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName1, "nSpeedY", "1700,1000", tempStr, _MAX_FNAME, StrRunModelIni);
            arrSubStr = tempStr.ToString().Split(',');
            runModel_s[0].SpeedY = double.Parse(arrSubStr[0].ToString());
            runModel_s[1].SpeedY = double.Parse(arrSubStr[1].ToString());
            Axis.runPrm_Y.speed = runModel_s[Auto_Flag.RunModel].SpeedY;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nFastSpeedY", "30", tempStr, _MAX_FNAME, StrConfigIni);
            Axis.jogPrm_Y.velHigh = double.Parse(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nSlowSpeedY", "10", tempStr, _MAX_FNAME, StrConfigIni);
            Axis.jogPrm_Y.velLow = double.Parse(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName1, "nLongAcceSpeedY", "15,6", tempStr, _MAX_FNAME, StrRunModelIni);
            arrSubStr = tempStr.ToString().Split(',');
            runModel_s[0].LongAcceSpeedY = double.Parse(arrSubStr[0].ToString());
            runModel_s[1].LongAcceSpeedY = double.Parse(arrSubStr[1].ToString());
            Axis.runPrm_Y.longAcc = runModel_s[Auto_Flag.RunModel].LongAcceSpeedY;
            tempStr.Clear();

            GetPrivateProfileString(appName1, "nLongDecSpeedY", "15,6", tempStr, _MAX_FNAME, StrRunModelIni);
            arrSubStr = tempStr.ToString().Split(',');
            runModel_s[0].LongDecSpeedY = double.Parse(arrSubStr[0].ToString());
            runModel_s[1].LongDecSpeedY = double.Parse(arrSubStr[1].ToString());
            Axis.runPrm_Y.longDec = runModel_s[Auto_Flag.RunModel].LongDecSpeedY;
            tempStr.Clear();

            GetPrivateProfileString(appName1, "nShortAcceSpeedY", "8,4", tempStr, _MAX_FNAME, StrRunModelIni);
            arrSubStr = tempStr.ToString().Split(',');
            runModel_s[0].ShortAcceSpeedY = double.Parse(arrSubStr[0].ToString());
            runModel_s[1].ShortAcceSpeedY = double.Parse(arrSubStr[1].ToString());
            Axis.runPrm_Y.shortAcc = runModel_s[Auto_Flag.RunModel].ShortAcceSpeedY;
            tempStr.Clear();

            GetPrivateProfileString(appName1, "nShortDecSpeedY", "8,4", tempStr, _MAX_FNAME, StrRunModelIni);
            arrSubStr = tempStr.ToString().Split(',');
            runModel_s[0].ShortDecSpeedY = double.Parse(arrSubStr[0].ToString());
            runModel_s[1].ShortDecSpeedY = double.Parse(arrSubStr[1].ToString());
            Axis.runPrm_Y.shortDec = runModel_s[Auto_Flag.RunModel].ShortDecSpeedY;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nLongMinY", "60", tempStr, _MAX_FNAME, StrConfigIni);
            Axis.runPrm_Y.longMin = double.Parse(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nHomePotFixY", "3", tempStr, _MAX_FNAME, StrConfigIni);
            Axis.homePrm_Y.homeOffset = double.Parse(tempStr.ToString());
            Axis.Position_Y.MinVal = -Axis.homePrm_Y.homeOffset;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nMaxLimitY", "680", tempStr, _MAX_FNAME, StrConfigIni);
            Axis.Position_Y.MaxVal = double.Parse(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName1, "nSpeedZ", "1500,1000", tempStr, _MAX_FNAME, StrRunModelIni);
            arrSubStr = tempStr.ToString().Split(',');
            runModel_s[0].SpeedZ = double.Parse(arrSubStr[0].ToString());
            runModel_s[1].SpeedZ = double.Parse(arrSubStr[1].ToString());
            Axis.runPrm_Z.speed = runModel_s[Auto_Flag.RunModel].SpeedZ;
            tempStr.Clear();

            GetPrivateProfileString(appName1, "nSpeedC", "1500,900", tempStr, _MAX_FNAME, StrRunModelIni);
            arrSubStr = tempStr.ToString().Split(',');
            runModel_s[0].SpeedC = double.Parse(arrSubStr[0].ToString());
            runModel_s[1].SpeedC = double.Parse(arrSubStr[1].ToString());
            Axis.trapPrm_C[0].speed = runModel_s[Auto_Flag.RunModel].SpeedC;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nFastSpeedZ", "20", tempStr, _MAX_FNAME, StrConfigIni);
            Axis.jogPrm_Z[0].velHigh = double.Parse(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nSlowSpeedZ", "5", tempStr, _MAX_FNAME, StrConfigIni);
            Axis.jogPrm_Z[0].velLow = double.Parse(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName1, "nAcceSpeedZ", "100,40", tempStr, _MAX_FNAME, StrRunModelIni);
            arrSubStr = tempStr.ToString().Split(',');
            runModel_s[0].AcceSpeedZ = double.Parse(arrSubStr[0].ToString());
            runModel_s[1].AcceSpeedZ = double.Parse(arrSubStr[1].ToString());
            Axis.runPrm_Z.longAcc = runModel_s[Auto_Flag.RunModel].AcceSpeedZ;
            tempStr.Clear();

            GetPrivateProfileString(appName1, "nDecSpeedZ", "100,40", tempStr, _MAX_FNAME, StrRunModelIni);
            arrSubStr = tempStr.ToString().Split(',');
            runModel_s[0].DecSpeedZ = double.Parse(arrSubStr[0].ToString());
            runModel_s[1].DecSpeedZ = double.Parse(arrSubStr[1].ToString());
            Axis.runPrm_Z.longDec = runModel_s[Auto_Flag.RunModel].DecSpeedZ;
            tempStr.Clear();

            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                GetPrivateProfileString(appName, "nHomePotFixZ" + Convert.ToString(i + 1), "3", tempStr, _MAX_FNAME, StrConfigIni);
                Axis.homePrm_Z[i].homeOffset = double.Parse(tempStr.ToString());
                tempStr.Clear();

                Axis.jogPrm_Z[i].velHigh = Axis.jogPrm_Z[0].velHigh;
                Axis.jogPrm_Z[i].velLow = Axis.jogPrm_Z[0].velLow;
                Axis.trapPrm_C[i].speed = Axis.trapPrm_C[0].speed;
            }
        }
        /// <summary>
        /// 写轴参数配置
        /// </summary>
        public void WriteAxisParaVal()
        {
            String appName = "AxisPara";
            String appName1 = "RunModelPara";
            String appName2 = "FuncSwit";

            WritePrivateProfileString(appName2, "nRunModel", Auto_Flag.RunModel.ToString(), StrProductIni);

            WritePrivateProfileString(appName1, "nSpeedX", runModel_s[0].SpeedX.ToString() + "," + runModel_s[1].SpeedX.ToString(), StrRunModelIni);
            WritePrivateProfileString(appName, "nFastSpeedX", Convert.ToString(Axis.jogPrm_X.velHigh), StrConfigIni);
            WritePrivateProfileString(appName, "nSlowSpeedX", Convert.ToString(Axis.jogPrm_X.velLow), StrConfigIni);
            WritePrivateProfileString(appName1, "nLongAcceSpeedX", runModel_s[0].LongAcceSpeedX.ToString() + "," + runModel_s[1].LongAcceSpeedX.ToString(), StrRunModelIni);
            WritePrivateProfileString(appName1, "nLongDecSpeedX", runModel_s[0].LongDecSpeedX.ToString() + "," + runModel_s[1].LongDecSpeedX.ToString(), StrRunModelIni);
            WritePrivateProfileString(appName1, "nShortAcceSpeedX", runModel_s[0].ShortAcceSpeedX.ToString() + "," + runModel_s[1].ShortAcceSpeedX.ToString(), StrRunModelIni);
            WritePrivateProfileString(appName1, "nShortDecSpeedX", runModel_s[0].ShortDecSpeedX.ToString() + "," + runModel_s[1].ShortDecSpeedX.ToString(), StrRunModelIni);
            WritePrivateProfileString(appName, "nLongMinX", Convert.ToString(Axis.runPrm_X.longMin), StrConfigIni);
            WritePrivateProfileString(appName, "nHomePotFixX",Convert.ToString(Axis.homePrm_X.homeOffset), StrConfigIni);
            WritePrivateProfileString(appName, "nMaxLimitX",Convert.ToString(Axis.Position_X.MaxVal), StrConfigIni);

            WritePrivateProfileString(appName1, "nSpeedY", runModel_s[0].SpeedY.ToString() + "," + runModel_s[1].SpeedY.ToString(), StrRunModelIni);
            WritePrivateProfileString(appName, "nFastSpeedY", Convert.ToString(Axis.jogPrm_Y.velHigh), StrConfigIni);
            WritePrivateProfileString(appName, "nSlowSpeedY", Convert.ToString(Axis.jogPrm_Y.velLow), StrConfigIni);
            WritePrivateProfileString(appName1, "nLongAcceSpeedY", runModel_s[0].LongAcceSpeedY.ToString() + "," + runModel_s[1].LongAcceSpeedY.ToString(), StrRunModelIni);
            WritePrivateProfileString(appName1, "nLongDecSpeedY", runModel_s[0].LongDecSpeedY.ToString() + "," + runModel_s[1].LongDecSpeedY.ToString(), StrRunModelIni);
            WritePrivateProfileString(appName1, "nShortAcceSpeedY", runModel_s[0].ShortAcceSpeedY.ToString() + "," + runModel_s[1].ShortAcceSpeedY.ToString(), StrRunModelIni);
            WritePrivateProfileString(appName1, "nShortDecSpeedY", runModel_s[0].ShortDecSpeedY.ToString() + "," + runModel_s[1].ShortDecSpeedY.ToString(), StrRunModelIni);
            WritePrivateProfileString(appName, "nLongMinY", Convert.ToString(Axis.runPrm_Y.longMin), StrConfigIni);
            WritePrivateProfileString(appName, "nHomePotFixY", Convert.ToString(Axis.homePrm_Y.homeOffset), StrConfigIni);
            WritePrivateProfileString(appName, "nMaxLimitY", Convert.ToString(Axis.Position_Y.MaxVal), StrConfigIni);

            WritePrivateProfileString(appName1, "nSpeedZ", runModel_s[0].SpeedZ.ToString() + "," + runModel_s[1].SpeedZ.ToString(), StrRunModelIni);
            WritePrivateProfileString(appName1, "nSpeedC", runModel_s[0].SpeedC.ToString() + "," + runModel_s[1].SpeedC.ToString(), StrRunModelIni);
            WritePrivateProfileString(appName, "nFastSpeedZ", Convert.ToString(Axis.jogPrm_Z[0].velHigh), StrConfigIni);
            WritePrivateProfileString(appName, "nSlowSpeedZ", Convert.ToString(Axis.jogPrm_Z[0].velLow), StrConfigIni);
            WritePrivateProfileString(appName1, "nAcceSpeedZ", runModel_s[0].AcceSpeedZ.ToString() + "," + runModel_s[1].AcceSpeedZ.ToString(), StrRunModelIni);
            WritePrivateProfileString(appName1, "nDecSpeedZ", runModel_s[0].DecSpeedZ.ToString() + "," + runModel_s[1].DecSpeedZ.ToString(), StrRunModelIni);

            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                WritePrivateProfileString(appName, "nHomePotFixZ" + Convert.ToString(i + 1), Convert.ToString(Axis.homePrm_Z[i].homeOffset), StrConfigIni);
            }

        }
        
        /// <summary>
        /// 读编带参数配置
        /// </summary>
        public void ReadBraidParaVal()
        {
            String appName = "BraidPara";
            StringBuilder tempStr = new StringBuilder();
            String[] arrSubStr;
            GetPrivateProfileString(appName, "N", "0", tempStr, _MAX_FNAME, StrProductIni);
            int num = Convert.ToInt32(tempStr.ToString());
            ArrBraidParas = new StuPara[17];

            GetPrivateProfileString(appName, "nHotTime", "NULL,0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrBraidParas[0] = new StuPara();
            ArrBraidParas[0].IDataType = 0;
            ArrBraidParas[0].StrParaName = arrSubStr[0];
            ArrBraidParas[0].IData = Brede.setConfig.tHotMelt =  Convert.ToUInt16(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nPointTime", "NULL,0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrBraidParas[1] = new StuPara();
            ArrBraidParas[1].IDataType = 0;
            ArrBraidParas[1].StrParaName = arrSubStr[0];
            ArrBraidParas[1].IData = Brede.setConfig.tDot =  Convert.ToUInt16(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nEmptyBredeFrapTime", "NULL,0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrBraidParas[2] = new StuPara();
            ArrBraidParas[2].IDataType = 0;
            ArrBraidParas[2].StrParaName = arrSubStr[0];
            ArrBraidParas[2].IData = Brede.setConfig.tEmptyBredeFrap =  Convert.ToUInt16(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nEmptyBredeSendTime", "NULL,0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrBraidParas[3] = new StuPara();
            ArrBraidParas[3].IDataType = 0;
            ArrBraidParas[3].StrParaName = arrSubStr[0];
            ArrBraidParas[3].IData = Brede.setConfig.tEmptyBredeSend =  Convert.ToUInt16(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nBraidFrapTime", "NULL,0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrBraidParas[4] = new StuPara();
            ArrBraidParas[4].IDataType = 0;
            ArrBraidParas[4].StrParaName = arrSubStr[0];
            ArrBraidParas[4].IData = Brede.setConfig.tBraidFrap =  Convert.ToUInt16(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nBraidAccTime", "NULL,0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrBraidParas[5] = new StuPara();
            ArrBraidParas[5].IDataType = 0;
            ArrBraidParas[5].StrParaName = arrSubStr[0];
            ArrBraidParas[5].IData = Brede.setConfig.acc =  Convert.ToUInt16(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nBraidDecTime", "NULL,0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrBraidParas[6] = new StuPara();
            ArrBraidParas[6].IDataType = 0;
            ArrBraidParas[6].StrParaName = arrSubStr[0];
            ArrBraidParas[6].IData = Brede.setConfig.dec =  Convert.ToUInt16(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nBraidGap", "NULL,0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrBraidParas[7] = new StuPara();
            ArrBraidParas[7].IDataType = 0;
            ArrBraidParas[7].StrParaName = arrSubStr[0];
            ArrBraidParas[7].DData = Brede.setConfig.space = float.Parse(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nBraidSpeed", "NULL,0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrBraidParas[8] = new StuPara();
            ArrBraidParas[8].IDataType = 0;
            ArrBraidParas[8].StrParaName = arrSubStr[0];
            ArrBraidParas[8].DData = Brede.setConfig.speed = float.Parse(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nFrontEmptyNum", "NULL,0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrBraidParas[9] = new StuPara();
            ArrBraidParas[9].IDataType = 0;
            ArrBraidParas[9].StrParaName = arrSubStr[0];
            ArrBraidParas[9].IData = Brede_Number.CntVal_FrontEmptyMaterial =  Convert.ToUInt16(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nBackEmptyNum", "NULL,0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrBraidParas[10] = new StuPara();
            ArrBraidParas[10].IDataType = 0;
            ArrBraidParas[10].StrParaName = arrSubStr[0];
            ArrBraidParas[10].IData = Brede_Number.CntVal_End =  Convert.ToUInt16(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nBraidPointNum", "NULL,0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrBraidParas[11] = new StuPara();
            ArrBraidParas[11].IDataType = 0;
            ArrBraidParas[11].StrParaName = arrSubStr[0];
            ArrBraidParas[11].IData = Brede_Number.CntVal_MarkAEmptyMaterial =  Convert.ToUInt16(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nBraidPointBNum", "编带打点B空个数,0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrBraidParas[16] = new StuPara();
            ArrBraidParas[16].IDataType = 0;
            ArrBraidParas[16].StrParaName = arrSubStr[0];
            ArrBraidParas[16].IData = Brede_Number.CntVal_MarkBEmptyMaterial = Convert.ToUInt16(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nManualBraidNum", "NULL,0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrBraidParas[12] = new StuPara();
            ArrBraidParas[12].IDataType = 0;
            ArrBraidParas[12].StrParaName = arrSubStr[0];
            ArrBraidParas[12].IData = Brede_Number.CntVal_Manual =  Convert.ToUInt16(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nHotGapNum", "NULL,0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrBraidParas[13] = new StuPara();
            ArrBraidParas[13].IDataType = 0;
            ArrBraidParas[13].StrParaName = arrSubStr[0];
            ArrBraidParas[13].IData = Brede_Number.CntVal_HotMelt =  Convert.ToUInt16(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nCCDEmptyMaterial", "CCD前空个数,0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrBraidParas[14] = new StuPara();
            ArrBraidParas[14].IDataType = 0;
            ArrBraidParas[14].StrParaName = arrSubStr[0];
            ArrBraidParas[14].IData = Brede_Number.CntVal_CCDEmptyMaterial = Convert.ToUInt16(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nWholeBraidNum", "NULL,0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrBraidParas[15] = new StuPara();
            ArrBraidParas[15].IDataType = 0;
            ArrBraidParas[15].StrParaName = arrSubStr[0];
            ArrBraidParas[15].IData = Convert.ToUInt16(arrSubStr[1].ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nBraidPointBNum", "NULL,0", tempStr, _MAX_FNAME, StrProductIni);
            arrSubStr = tempStr.ToString().Split(',');
            ArrBraidParas[16] = new StuPara();
            ArrBraidParas[16].IDataType = 0;
            ArrBraidParas[16].StrParaName = arrSubStr[0];
            ArrBraidParas[16].IData = Convert.ToUInt16(arrSubStr[1].ToString());
            tempStr.Clear();
        }
        /// <summary>
        /// 写编带参数配置
        /// </summary>
        public void WriteBraidParaVal()
        {
            String appName = "BraidPara";
            WritePrivateProfileString(appName, "nHotTime", ArrBraidParas[0].StrParaName + "," + Convert.ToString(Brede.setConfig.tHotMelt), StrProductIni);
            WritePrivateProfileString(appName, "nPointTime", ArrBraidParas[1].StrParaName + "," + Convert.ToString(Brede.setConfig.tDot), StrProductIni);
            WritePrivateProfileString(appName, "nEmptyBredeFrapTime", ArrBraidParas[2].StrParaName + "," + Convert.ToString(Brede.setConfig.tEmptyBredeFrap), StrProductIni);
            WritePrivateProfileString(appName, "nEmptyBredeSendTime", ArrBraidParas[3].StrParaName + "," + Convert.ToString(Brede.setConfig.tEmptyBredeSend), StrProductIni);
            WritePrivateProfileString(appName, "nBraidFrapTime", ArrBraidParas[4].StrParaName + "," + Convert.ToString(Brede.setConfig.tBraidFrap), StrProductIni);
            WritePrivateProfileString(appName, "nBraidAccTime", ArrBraidParas[5].StrParaName + "," + Convert.ToString(Brede.setConfig.acc), StrProductIni);
            WritePrivateProfileString(appName, "nBraidDecTime", ArrBraidParas[6].StrParaName + "," + Convert.ToString(Brede.setConfig.dec), StrProductIni);

            WritePrivateProfileString(appName, "nBraidGap", ArrBraidParas[7].StrParaName + "," + Convert.ToString(Brede.setConfig.space), StrProductIni);
            WritePrivateProfileString(appName, "nBraidSpeed", ArrBraidParas[8].StrParaName + "," + Convert.ToString(Brede.setConfig.speed), StrProductIni);
            WritePrivateProfileString(appName, "nFrontEmptyNum", ArrBraidParas[9].StrParaName + "," + Convert.ToString(Brede_Number.CntVal_FrontEmptyMaterial), StrProductIni);
            WritePrivateProfileString(appName, "nBackEmptyNum", ArrBraidParas[10].StrParaName + "," + Convert.ToString(Brede_Number.CntVal_End), StrProductIni);
            WritePrivateProfileString(appName, "nBraidPointNum", ArrBraidParas[11].StrParaName + "," + Convert.ToString(Brede_Number.CntVal_MarkAEmptyMaterial), StrProductIni);
            WritePrivateProfileString(appName, "nBraidPointBNum", ArrBraidParas[16].StrParaName + "," + Convert.ToString(Brede_Number.CntVal_MarkBEmptyMaterial), StrProductIni);
            WritePrivateProfileString(appName, "nManualBraidNum", ArrBraidParas[12].StrParaName + "," + Convert.ToString(Brede_Number.CntVal_Manual), StrProductIni);
            WritePrivateProfileString(appName, "nHotGapNum", ArrBraidParas[13].StrParaName + "," + Convert.ToString(Brede_Number.CntVal_HotMelt), StrProductIni);
            WritePrivateProfileString(appName, "nCCDEmptyMaterial", ArrBraidParas[14].StrParaName + "," + Convert.ToString(Brede_Number.CntVal_CCDEmptyMaterial), StrProductIni);
        }

        /// <summary>
        /// 读自动盘参数配置
        /// </summary>
        public void ReadAutoTrayParaVal()
        {
            String appName = "AutoTrayPara";
            StringBuilder tempStr = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                GetPrivateProfileString(appName, "setConfig" + i, "10", tempStr, _MAX_FNAME, StrProductIni);
                AutoTray.setConfig[i] = float.Parse(tempStr.ToString()); ;
                tempStr.Clear();
            }
        }

        /// <summary>
        /// 写自动盘参数配置
        /// </summary>
        public void WriteAutoTrayParaVal()
        {
            String appName = "AutoTrayPara";
            StringBuilder tempStr = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                WritePrivateProfileString(appName, "setConfig" + i, Convert.ToString(AutoTray.setConfig[i]), StrProductIni);
            }
        }

        /// <summary>
        /// 读功能开关参数配置
        /// </summary>
        public void ReadFuncSwitVal()
        {
            String appName = "FuncSwit";
            StringBuilder tempStr = new StringBuilder();

            GetPrivateProfileString(appName, "nShiftWay", "0", tempStr, _MAX_FNAME, StrProductIni);
            UserTask.ShiftWay = Convert.ToUInt16(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nBraidCheck", "0", tempStr, _MAX_FNAME, StrProductIni);
            Auto_Flag.Brede_Check = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nVelumCheck", "0", tempStr, _MAX_FNAME, StrProductIni);
            Auto_Flag.Velum_Check = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nEmptyCheck", "1", tempStr, _MAX_FNAME, StrProductIni);
            Auto_Flag.Empty_Check = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nBredeCCD_Check", "0", tempStr, _MAX_FNAME, StrProductIni);
            Auto_Flag.BredeCCD_Check = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nDot", "0", tempStr, _MAX_FNAME, StrProductIni);
            Auto_Flag.DotA = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nDotB", "0", tempStr, _MAX_FNAME, StrProductIni);
            Auto_Flag.DotB = tempStr.ToString() == "1" ? true : false;
            if (BredeDotCount == 0)//编带单打点
            {
                Auto_Flag.DotB = false;
            }
            tempStr.Clear();

            GetPrivateProfileString(appName, "nBurn", "0", tempStr, _MAX_FNAME, StrProductIni);
            Auto_Flag.TestMode = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nBurnMode", "1", tempStr, _MAX_FNAME, StrProductIni);
            Auto_Flag.BurnMode = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nSetPanWay", "0", tempStr, _MAX_FNAME, StrProductIni);
            Auto_Flag.AutoTray = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nSetNGWay", "0", tempStr, _MAX_FNAME, StrProductIni);
            Auto_Flag.NGTray = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nQuantitate_Enabled", "0", tempStr, _MAX_FNAME, StrProductIni);
            Auto_Flag.Production = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nQuantitate_OKMode", "0", tempStr, _MAX_FNAME, StrProductIni);
            Auto_Flag.ProductionOK = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nFlip", "0", tempStr, _MAX_FNAME, StrProductIni);
            Auto_Flag.Flip = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nTakeICPosEnabled", "0", tempStr, _MAX_FNAME, StrProductIni);
            Auto_Flag.Enabled_TakeICPos = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nLayICPosEnabled", "0", tempStr, _MAX_FNAME, StrProductIni);
            Auto_Flag.Enabled_LayICPos = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nVision_3D_Enabled_I", "0", tempStr, _MAX_FNAME, StrProductIni);
            Vision_3D.Enabled_I = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nVision_3D_Enabled_II", "0", tempStr, _MAX_FNAME, StrProductIni);
            Vision_3D.Enabled_II = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nOverlayEnabled", "0", tempStr, _MAX_FNAME, StrProductIni);
            Auto_Flag.Enabled_Overlay = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nNCCModelEnabled", "0", tempStr, _MAX_FNAME, StrProductIni);
            Auto_Flag.Enabled_NCCModel = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nSyncEnabled", "0", tempStr, _MAX_FNAME, StrProductIni);
            Auto_Flag.Enabled_Sync = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nRunModel", "0", tempStr, _MAX_FNAME, StrProductIni);
            Auto_Flag.RunModel = tempStr.ToString() == "0" ? 0 : 1;
            tempStr.Clear();
            GetPrivateProfileString(appName, "nPenAltMode", "0", tempStr, _MAX_FNAME, StrProductIni);
            Auto_Flag.PenAltMode = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();            ShiftWayParse();
        }
        /// <summary>
        /// 写功能开关参数配置
        /// </summary>
        public void WriteFuncSwitVal()
        {
            ShiftWayParse();
            String appName = "FuncSwit";

            WritePrivateProfileString(appName, "nShiftWay", Convert.ToString(UserTask.ShiftWay), StrProductIni);
            WritePrivateProfileString(appName, "nBraidCheck", Convert.ToString(Auto_Flag.Brede_Check == true ? "1" : "0"), StrProductIni);
            WritePrivateProfileString(appName, "nVelumCheck", Convert.ToString(Auto_Flag.Velum_Check == true ? "1" : "0"), StrProductIni);
            WritePrivateProfileString(appName, "nEmptyCheck", Convert.ToString(Auto_Flag.Empty_Check == true ? "1" : "0"), StrProductIni);
            WritePrivateProfileString(appName, "nBredeCCD_Check", Convert.ToString(Auto_Flag.BredeCCD_Check == true ? "1" : "0"), StrProductIni);
            WritePrivateProfileString(appName, "nDot", Convert.ToString(Auto_Flag.DotA == true ? "1" : "0"), StrProductIni);
            WritePrivateProfileString(appName, "nDotB", Convert.ToString(Auto_Flag.DotB == true ? "1" : "0"), StrProductIni);
            WritePrivateProfileString(appName, "nBurn", Convert.ToString(Auto_Flag.TestMode == true ? "1" : "0"), StrProductIni);
            WritePrivateProfileString(appName, "nBurnMode", Convert.ToString(Auto_Flag.BurnMode == true ? "1" : "0"), StrProductIni);
            WritePrivateProfileString(appName, "nSetPanWay", Convert.ToString(Auto_Flag.AutoTray == true ? "1" : "0"), StrProductIni);
            WritePrivateProfileString(appName, "nSetNGWay", Convert.ToString(Auto_Flag.NGTray == true ? "1" : "0"), StrProductIni);
            WritePrivateProfileString(appName, "nQuantitate_Enabled", Convert.ToString(Auto_Flag.Production == true ? "1" : "0"), StrProductIni);
            WritePrivateProfileString(appName, "nQuantitate_OKMode", Convert.ToString(Auto_Flag.ProductionOK == true ? "1" : "0"), StrProductIni);
            WritePrivateProfileString(appName, "nFlip", Convert.ToString(Auto_Flag.Flip == true ? "1" : "0"), StrProductIni);
            WritePrivateProfileString(appName, "nTakeICPosEnabled", Convert.ToString(Auto_Flag.Enabled_TakeICPos == true ? "1" : "0"), StrProductIni);
            WritePrivateProfileString(appName, "nLayICPosEnabled", Convert.ToString(Auto_Flag.Enabled_LayICPos == true ? "1" : "0"), StrProductIni);
            WritePrivateProfileString(appName, "nVision_3D_Enabled_I", Convert.ToString(Vision_3D.Enabled_I == true ? "1" : "0"), StrProductIni);
            WritePrivateProfileString(appName, "nVision_3D_Enabled_II", Convert.ToString(Vision_3D.Enabled_II == true ? "1" : "0"), StrProductIni);
            WritePrivateProfileString(appName, "nOverlayEnabled", Convert.ToString(Auto_Flag.Enabled_Overlay == true ? "1" : "0"), StrProductIni);
            WritePrivateProfileString(appName, "nNCCModelEnabled", Convert.ToString(Auto_Flag.Enabled_NCCModel == true ? "1" : "0"), StrProductIni);
            WritePrivateProfileString(appName, "nSyncEnabled", Convert.ToString(Auto_Flag.Enabled_Sync == true ? "1" : "0"), StrProductIni);
            WritePrivateProfileString(appName, "nRunModel", Auto_Flag.RunModel.ToString(), StrProductIni);
            WritePrivateProfileString(appName, "nPenAltMode", Convert.ToString(Auto_Flag.PenAltMode == true ? "1" : "0"), StrProductIni);

            appName = "ModelCxyr";
            WritePrivateProfileString(appName, "3DICType", Vision_3D.ICType.ToString(), StrProductIni);

            appName = "InksDate";
            WritePrivateProfileString(appName, "Enabled_AutoTray", Inks.Enabled_AutoTray == true ? "1" : "0", StrProductIni);
            WritePrivateProfileString(appName, "Enabled_Braid", Inks.Enabled_Braid == true ? "1" : "0", StrProductIni);

        }
        /// <summary>
        /// 放料方式解析
        /// </summary>
        private void ShiftWayParse()
        {
            if(UserTask.ShiftWay < 0 || UserTask.ShiftWay > 5)
            {
                UserTask.ShiftWay = 0;

            }
            //盘装取料
            if (UserTask.ShiftWay == 0 || UserTask.ShiftWay == 1)
            {
                if (!Auto_Flag.AutoTray)//固定盘
                {
                    Auto_Flag.FixedTray_TakeIC = true;
                    Auto_Flag.AutoTray_TakeIC = false;
                }
                else
                {
                    Auto_Flag.FixedTray_TakeIC = false;
                    Auto_Flag.AutoTray_TakeIC = true;
                }
                Auto_Flag.Brede_TakeIC = false;
                Auto_Flag.FixedTube_TakeIC = false;
            }

            //编带取料
            if (UserTask.ShiftWay == 2 || UserTask.ShiftWay == 3)
            {
                Auto_Flag.FixedTray_TakeIC = false;
                Auto_Flag.AutoTray_TakeIC = false;
                Auto_Flag.Brede_TakeIC = true;
                Auto_Flag.FixedTube_TakeIC = false;
            }

            //管装取料
            if (UserTask.ShiftWay == 4 || UserTask.ShiftWay == 5)
            {
                Auto_Flag.FixedTray_TakeIC = false;
                Auto_Flag.AutoTray_TakeIC = false;
                Auto_Flag.Brede_TakeIC = false;
                Auto_Flag.FixedTube_TakeIC = true;
            }

            //盘装放料
            if (UserTask.ShiftWay == 0 || UserTask.ShiftWay == 3 || UserTask.ShiftWay == 5)
            {
                if (!Auto_Flag.AutoTray)//固定盘
                {
                    Auto_Flag.FixedTray_LayIC = true;
                    Auto_Flag.AutoTray_LayIC = false;
                }
                else
                {
                    Auto_Flag.FixedTray_LayIC = false;
                    Auto_Flag.AutoTray_LayIC = true;
                }
                Auto_Flag.Brede_LayIC = false;
                Auto_Flag.FixedTube_LayIC = false;
            }

            //编带放料
            if (UserTask.ShiftWay == 1 || UserTask.ShiftWay == 2 || UserTask.ShiftWay == 4)
            {
                Auto_Flag.FixedTray_LayIC = false;
                Auto_Flag.AutoTray_LayIC = false;
                Auto_Flag.Brede_LayIC = true;
                Auto_Flag.FixedTube_LayIC = false;
            }
        }
        /// <summary>
        /// 读统计数据配置
        /// </summary>
        public void ReadStaticVal()
        {
            String appName = "Statics";
            StringBuilder tempStr = new StringBuilder();

            GetPrivateProfileString(appName, "nPanTake", "0", tempStr, _MAX_FNAME, StrStaticIni);
            TrayD.TIC_TrayN = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nPanLay", "0", tempStr, _MAX_FNAME, StrStaticIni);
            TrayD.LIC_TrayN = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();


            GetPrivateProfileString(appName, "nPan1TakeRow", "0", tempStr, _MAX_FNAME, StrStaticIni);
            TrayD.TIC_RowN[0] = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nPan1TakeCol", "0", tempStr, _MAX_FNAME, StrStaticIni);
            TrayD.TIC_ColN[0] = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nPan1LayRow", "0", tempStr, _MAX_FNAME, StrStaticIni);
            TrayD.LIC_RowN[0] = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nPan1LayCol", "0", tempStr, _MAX_FNAME, StrStaticIni);
            TrayD.LIC_ColN[0] = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nPan1EndRow", "0", tempStr, _MAX_FNAME, StrStaticIni);
            TrayD.TIC_EndRowN[0] = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nPan1EndCol", "0", tempStr, _MAX_FNAME, StrStaticIni);
            TrayD.TIC_EndColN[0] = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();


            GetPrivateProfileString(appName, "nPan2TakeRow", "0", tempStr, _MAX_FNAME, StrStaticIni);
            TrayD.TIC_RowN[1] = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nPan2TakeCol", "0", tempStr, _MAX_FNAME, StrStaticIni);
            TrayD.TIC_ColN[1] = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nPan2LayRow", "0", tempStr, _MAX_FNAME, StrStaticIni);
            TrayD.LIC_RowN[1] = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nPan2LayCol", "0", tempStr, _MAX_FNAME, StrStaticIni);
            TrayD.LIC_ColN[1] = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nPan2EndRow", "0", tempStr, _MAX_FNAME, StrStaticIni);
            TrayD.TIC_EndRowN[1] = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nPan2EndCol", "0", tempStr, _MAX_FNAME, StrStaticIni);
            TrayD.TIC_EndColN[1] = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nPan3LayRow", "0", tempStr, _MAX_FNAME, StrStaticIni);
            TrayD.LIC_RowN[2] = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nPan3LayCol", "0", tempStr, _MAX_FNAME, StrStaticIni);
            TrayD.LIC_ColN[2] = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nOKTarget", "0", tempStr, _MAX_FNAME, StrStaticIni);
            UserTask.TargetC = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nOKDone", "0", tempStr, _MAX_FNAME, StrStaticIni);
            UserTask.OKDoneC = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nTakeDone", "0", tempStr, _MAX_FNAME, StrStaticIni);
            UserTask.TIC_DoneC = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();

           
            GetPrivateProfileString(appName, "nOKAllNum", "0", tempStr, _MAX_FNAME, StrStaticIni);
            UserTask.OKAllC = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();

            GetPrivateProfileString(appName, "nNGAllNum", "0", tempStr, _MAX_FNAME, StrStaticIni);
            UserTask.NGAllC = Convert.ToInt32(tempStr.ToString());
            tempStr.Clear();



            GetPrivateProfileString(appName, "nPan1TakeEndFlag", "0", tempStr, _MAX_FNAME, StrStaticIni);
            TrayEndFlag.takeIC[0] = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();
            GetPrivateProfileString(appName, "nPan2TakeEndFlag", "0", tempStr, _MAX_FNAME, StrStaticIni);
            TrayEndFlag.takeIC[1] = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nPan1layEndFlag", "0", tempStr, _MAX_FNAME, StrStaticIni);
            TrayEndFlag.layIC[0] = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();
            GetPrivateProfileString(appName, "nPan2layEndFlag", "0", tempStr, _MAX_FNAME, StrStaticIni);
            TrayEndFlag.layIC[1] = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();
            GetPrivateProfileString(appName, "nPan3layEndFlag", "0", tempStr, _MAX_FNAME, StrStaticIni);
            TrayEndFlag.layIC[2] = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();

            GetPrivateProfileString(appName, "nPan1TakeLayEndFlag", "0", tempStr, _MAX_FNAME, StrStaticIni);
            TrayEndFlag.takeLayIC[0] = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();
            GetPrivateProfileString(appName, "nPan2TakeLayEndFlag", "0", tempStr, _MAX_FNAME, StrStaticIni);
            TrayEndFlag.takeLayIC[1] = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();
            GetPrivateProfileString(appName, "nPan2BurnEndFlag", "0", tempStr, _MAX_FNAME, StrStaticIni);
            TrayEndFlag.tray2Burn = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();
        }
        /// <summary>
        /// 写统计数据配置
        /// </summary>
        public void WriteStaticVal()
        {
            String appName = "Statics";
            WritePrivateProfileString(appName, "nPanTake", Convert.ToString(TrayD.TIC_TrayN), StrStaticIni);
            WritePrivateProfileString(appName, "nPanLay", Convert.ToString(TrayD.LIC_TrayN), StrStaticIni);

            WritePrivateProfileString(appName, "nPan1TakeRow", Convert.ToString(TrayD.TIC_RowN[0]), StrStaticIni);
            WritePrivateProfileString(appName, "nPan1TakeCol", Convert.ToString(TrayD.TIC_ColN[0]), StrStaticIni);
            WritePrivateProfileString(appName, "nPan1LayRow", Convert.ToString(TrayD.LIC_RowN[0]), StrStaticIni);
            WritePrivateProfileString(appName, "nPan1LayCol", Convert.ToString(TrayD.LIC_ColN[0]), StrStaticIni);
            WritePrivateProfileString(appName, "nPan1EndRow", Convert.ToString(TrayD.TIC_EndRowN[0]), StrStaticIni);
            WritePrivateProfileString(appName, "nPan1EndCol", Convert.ToString(TrayD.TIC_EndColN[0]), StrStaticIni);

            WritePrivateProfileString(appName, "nPan2TakeRow", Convert.ToString(TrayD.TIC_RowN[1]), StrStaticIni);
            WritePrivateProfileString(appName, "nPan2TakeCol", Convert.ToString(TrayD.TIC_ColN[1]), StrStaticIni);
            WritePrivateProfileString(appName, "nPan2LayRow", Convert.ToString(TrayD.LIC_RowN[1]), StrStaticIni);
            WritePrivateProfileString(appName, "nPan2LayCol", Convert.ToString(TrayD.LIC_ColN[1]), StrStaticIni);
            WritePrivateProfileString(appName, "nPan2EndRow", Convert.ToString(TrayD.TIC_EndRowN[1]), StrStaticIni);
            WritePrivateProfileString(appName, "nPan2EndCol", Convert.ToString(TrayD.TIC_EndColN[1]), StrStaticIni);

            WritePrivateProfileString(appName, "nPan3LayRow", Convert.ToString(TrayD.LIC_RowN[2]), StrStaticIni);
            WritePrivateProfileString(appName, "nPan3LayCol", Convert.ToString(TrayD.LIC_ColN[2]), StrStaticIni);

            WritePrivateProfileString(appName, "nOKTarget", Convert.ToString(UserTask.TargetC), StrStaticIni);
            WritePrivateProfileString(appName, "nOKDone", Convert.ToString(UserTask.OKDoneC), StrStaticIni);
            WritePrivateProfileString(appName, "nTakeDone", Convert.ToString(UserTask.TIC_DoneC), StrStaticIni);

            WritePrivateProfileString(appName, "nOKAllNum", Convert.ToString(UserTask.OKAllC), StrStaticIni);
            WritePrivateProfileString(appName, "nNGAllNum", Convert.ToString(UserTask.NGAllC), StrStaticIni);

        
            WritePrivateProfileString(appName, "nPan1TakeEndFlag", Convert.ToString(TrayEndFlag.takeIC[0] == true ? "1" : "0"), StrStaticIni);
            WritePrivateProfileString(appName, "nPan2TakeEndFlag", Convert.ToString(TrayEndFlag.takeIC[1] == true ? "1" : "0"), StrStaticIni);

            WritePrivateProfileString(appName, "nPan1layEndFlag", Convert.ToString(TrayEndFlag.layIC[0] == true ? "1" : "0"), StrStaticIni);
            WritePrivateProfileString(appName, "nPan2layEndFlag", Convert.ToString(TrayEndFlag.layIC[1] == true ? "1" : "0"), StrStaticIni);
            WritePrivateProfileString(appName, "nPan3layEndFlag", Convert.ToString(TrayEndFlag.layIC[2] == true ? "1" : "0"), StrStaticIni);

            WritePrivateProfileString(appName, "nPan1TakeLayEndFlag", Convert.ToString(TrayEndFlag.takeLayIC[0] == true ? "1" : "0"), StrStaticIni);
            WritePrivateProfileString(appName, "nPan2TakeLayEndFlag", Convert.ToString(TrayEndFlag.takeLayIC[1] == true ? "1" : "0"), StrStaticIni);
            WritePrivateProfileString(appName, "nPan2BurnEndFlag", Convert.ToString(TrayEndFlag.tray2Burn == true ? "1" : "0"), StrStaticIni);
        }

        /// <summary>
        /// 读座子使能
        /// </summary>
        public void ReadSocketEnabled()
        {
            String appName = "SocketEnabled";
            StringBuilder tempStr = new StringBuilder();
            for (int i = 0; i < UserConfig.AllScketC; i++)
            {
                GetPrivateProfileString(appName, "Socket_" + Convert.ToString(i), "0", tempStr, _MAX_FNAME, StrProductIni);
                int k = UserConfig.ScketUnitC;
                Axis.Group[i / k].Unit[i % k].Flag_Open = tempStr.ToString() == "1" ? true : false;
                tempStr.Clear();
            }
        }

        /// <summary>
        /// 写座子使能
        /// </summary>
        public void WriteSocketEnabled()
        {
            String appName = "SocketEnabled";
            for (int i = 0; i < UserConfig.AllScketC; i++)
            {
                int k = UserConfig.ScketUnitC;
                WritePrivateProfileString(appName, "Socket_" + Convert.ToString(i), Convert.ToString(Axis.Group[i / k].Unit[i % k].Flag_Open == true ? "1" : "0"), StrProductIni);
            }
        }
        /// <summary>
        /// 写座子使能
        /// </summary>
        public void WriteSocketEnabled(int ind)
        {
            String appName = "SocketEnabled";
            int k = UserConfig.ScketUnitC;
            WritePrivateProfileString(appName, "Socket_" + Convert.ToString(ind), Convert.ToString(Axis.Group[ind / k].Unit[ind % k].Flag_Open == true ? "1" : "0"), StrProductIni);
        }

        /// <summary>
        /// 读座子计数器
        /// </summary>
        public void ReadSocketCounter()
        {
            String appName = "SocketCounter";
            StringBuilder tempStr = new StringBuilder();
            GetPrivateProfileString(appName, "N", "", tempStr, _MAX_FNAME, StrStaticIni);
            if (tempStr.ToString() == "" || tempStr.ToString() != Convert.ToString(UserConfig.AllScketC))
            {
                WritePrivateProfileString(appName, "N", Convert.ToString(UserConfig.AllScketC), StrStaticIni);
                for (int i = 0; i < UserConfig.AllScketC; i++)
                {
                    WriteSocketCounter(i, 10);
                }
            }
            for (int i = 0; i < UserConfig.AllScketC; i++)
            {
                int k = UserConfig.ScketUnitC;
                GetPrivateProfileString(appName, "SocketOK_" + Convert.ToString(i), "0", tempStr, _MAX_FNAME, StrStaticIni);
                Axis.Group[i / k].Unit[i % k].OKAllC = Convert.ToInt32(tempStr.ToString());
                tempStr.Clear();

                GetPrivateProfileString(appName, "SocketNG_" + Convert.ToString(i), "0", tempStr, _MAX_FNAME, StrStaticIni);
                Axis.Group[i / k].Unit[i % k].NGAllC = Convert.ToInt32(tempStr.ToString());
                tempStr.Clear();
            }
        }

        /// <summary>
        /// 写座子计数器
        /// </summary>
        public void WriteSocketCounter(int ind, int result)
        {
            String appName = "SocketCounter";
            int k = UserConfig.ScketUnitC;
            if (result == 1)
            {
                Axis.Group[ind / k].Unit[ind % k].OKAllC++;
                WritePrivateProfileString(appName, "SocketOK_" + Convert.ToString(ind), Convert.ToString(Axis.Group[ind / k].Unit[ind % k].OKAllC), StrStaticIni);
            }
            else if (result == 2)
            {
                Axis.Group[ind / k].Unit[ind % k].NGAllC++;
                WritePrivateProfileString(appName, "SocketNG_" + Convert.ToString(ind), Convert.ToString(Axis.Group[ind / k].Unit[ind % k].NGAllC), StrStaticIni);
            }
            else if (result == 10)
            {
                Axis.Group[ind / k].Unit[ind % k].OKAllC = 0;
                Axis.Group[ind / k].Unit[ind % k].NGAllC = 0;
                WritePrivateProfileString(appName, "SocketOK_" + Convert.ToString(ind), Convert.ToString(Axis.Group[ind / k].Unit[ind % k].OKAllC), StrStaticIni);
                WritePrivateProfileString(appName, "SocketNG_" + Convert.ToString(ind), Convert.ToString(Axis.Group[ind / k].Unit[ind % k].NGAllC), StrStaticIni);
            }
        }

        /// <summary>
        /// 读吸笔使能
        /// </summary>
        public void ReadPenEnabled()
        {
            String appName = "PenEnabled";
            StringBuilder tempStr = new StringBuilder();

            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                GetPrivateProfileString(appName, "Pen_" + Convert.ToString(i + 1), "1", tempStr, _MAX_FNAME, StrConfigIni);
                Run.PenEnable[i].Checked = tempStr.ToString() == "1" ? true : false;
                tempStr.Clear();
            }
            GetPrivateProfileString(appName, "PenAbnormal", "0", tempStr, _MAX_FNAME, StrConfigIni);
            Auto_Flag.PenAbnormal = tempStr.ToString() == "1" ? true : false;
            tempStr.Clear();
        }

        /// <summary>
        /// 写吸笔使能
        /// </summary>
        public void WritePenEnabled()
        {
            String appName = "PenEnabled";

            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                WritePrivateProfileString(appName, "Pen_" + Convert.ToString(i + 1), Convert.ToString(Run.PenEnable[i].Checked == true ? "1" : "0"), StrConfigIni);
            }
            WritePrivateProfileString(appName, "PenAbnormal", Convert.ToString(Auto_Flag.PenAbnormal == true ? "1" : "0"), StrConfigIni);
        }

        /// <summary>
        /// 读飞达使能
        /// </summary>
        public void ReadFeederEnabled()
        {
            String appName = "FeederEnabled";
            StringBuilder tempStr = new StringBuilder();

            for (int i = 0; i < 2; i++)
            {
                GetPrivateProfileString(appName, "Feeder_" + Convert.ToString(i + 1), "1", tempStr, _MAX_FNAME, StrConfigIni);
                Run.FeederEnable[i].Checked = tempStr.ToString() == "1" ? true : false;
                tempStr.Clear();
            }
        }

        /// <summary>
        /// 写飞达使能
        /// </summary>
        public void WriteFeederEnabled()
        {
            String appName = "FeederEnabled";

            for (int i = 0; i < 2; i++)
            {
                WritePrivateProfileString(appName, "Feeder_" + Convert.ToString(i + 1), Convert.ToString(Run.FeederEnable[i].Checked == true ? "1" : "0"), StrConfigIni);
            }
        }

        /// <summary>
        /// 读MES参数配置
        /// </summary>
        public void ReadMesDate()
        {
            String appName = "MesDate";
            StringBuilder tempStr = new StringBuilder();
            string strBuffer;
            Mes.ComputerName = SystemInformation.ComputerName;

            GetPrivateProfileString(appName, "IP", "192.168.0.11", tempStr, _MAX_FNAME, StrConfigIni);
            Mes.IP = tempStr.ToString();
            tempStr.Clear();

            GetPrivateProfileString(appName, "IP_XC", "", tempStr, _MAX_FNAME, StrConfigIni);
            strBuffer = tempStr.ToString();
            if (tempStr.ToString() == "")
            {
                strBuffer = "192.168.100.18";
                WritePrivateProfileString(appName, "IP_XC", strBuffer, StrConfigIni);
            }
            Mes.IP_XC = strBuffer;
            tempStr.Clear();

            GetPrivateProfileString(appName, "Port", "5000", tempStr, _MAX_FNAME, StrConfigIni);
            Mes.Port = tempStr.ToString();
            tempStr.Clear();

            GetPrivateProfileString(appName, "FuncModeIndex", "0", tempStr, _MAX_FNAME, StrConfigIni);
            Mes.FuncModeIndex = tempStr.ToString();
            tempStr.Clear();

            GetPrivateProfileString(appName, "DeviceSN", "QCT01", tempStr, _MAX_FNAME, StrConfigIni);
            Mes.DeviceSN = tempStr.ToString();
            tempStr.Clear();

            GetPrivateProfileString(appName, "DeviceDes", "", tempStr, _MAX_FNAME, StrConfigIni);
            Mes.DeviceDes = tempStr.ToString();
            tempStr.Clear();

            GetPrivateProfileString(appName, "Version", "", tempStr, _MAX_FNAME, StrConfigIni);
            Mes.Version_File = tempStr.ToString();
            tempStr.Clear();

            GetPrivateProfileString(appName, "ProgFilePath", "", tempStr, _MAX_FNAME, StrConfigIni);
            Mes.ProgFilePath = tempStr.ToString();
            tempStr.Clear();

            GetPrivateProfileString(appName, "DatabasePath_XC", "", tempStr, _MAX_FNAME, StrConfigIni);
            strBuffer = tempStr.ToString();
            if (tempStr.ToString() == "")
            {
                strBuffer = @"\\192.168.20.22\i\db.json";
                WritePrivateProfileString(appName, "DatabasePath_XC", strBuffer, StrConfigIni);
            }
            Mes.DatabasePath_XC = strBuffer;
            tempStr.Clear();

            GetPrivateProfileString(appName, "LogPath_XC", "", tempStr, _MAX_FNAME, StrConfigIni);
            strBuffer = tempStr.ToString();
            if (tempStr.ToString() == "")
            {
                strBuffer = @"\\192.168.20.22\g\program";
                WritePrivateProfileString(appName, "LogPath_XC", strBuffer, StrConfigIni);
            }
            Mes.LogPath_XC = strBuffer;
            tempStr.Clear();

            GetPrivateProfileString(appName, "ProgFilePath_XC", "", tempStr, _MAX_FNAME, StrConfigIni);
            strBuffer = tempStr.ToString();
            if (tempStr.ToString() == "")
            {
                strBuffer = @"\\192.168.20.22\i\software";
                WritePrivateProfileString(appName, "ProgFilePath_XC", strBuffer, StrConfigIni);
            }
            Mes.ProgFilePath_XC = strBuffer;
            tempStr.Clear();

        }

        /// <summary>
        /// 写MES参数配置
        /// </summary>
        public void WriteMesDate()
        {
            String appName = "MesDate";

            WritePrivateProfileString(appName, "FuncModeIndex", Mes.FuncModeIndex, StrConfigIni);
            WritePrivateProfileString(appName, "DeviceSN", Mes.DeviceSN, StrConfigIni);
            WritePrivateProfileString(appName, "DeviceDes", Mes.DeviceDes, StrConfigIni);
            WritePrivateProfileString(appName, "Version", Mes.Version_File, StrConfigIni);
            WritePrivateProfileString(appName, "ProgFilePath", Mes.ProgFilePath, StrConfigIni);
        }
        /// <summary>
        /// 读油墨数据
        /// </summary>
        public void ReadInksDate()
        {
            String appName = "InksDate";
            StringBuilder tempStr = new StringBuilder();
            string strBuffer;

            

            GetPrivateProfileString(appName, "Enabled_Braid", "", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            if (strBuffer == "")
            {
                strBuffer = "0";
                WritePrivateProfileString(appName, "Enabled_Braid", strBuffer, StrProductIni);
            }
            Inks.Enabled_Braid = strBuffer == "1" ? true : false;
            tempStr.Clear();

            GetPrivateProfileString(appName, "Enabled_AutoTray", "", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            if (strBuffer == "")
            {
                strBuffer = "0";
                WritePrivateProfileString(appName, "Enabled_AutoTray", strBuffer, StrProductIni);
            }
            Inks.Enabled_AutoTray = strBuffer == "1" ? true : false;
            tempStr.Clear();

            GetPrivateProfileString(appName, "DotCount_AutoTray", "", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            if (strBuffer == "")
            {
                strBuffer = "5000";
                WritePrivateProfileString(appName, "DotCount_AutoTray", strBuffer, StrProductIni);
            }
            Inks.DotCount_AutoTray = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "DotCount_Braid", "", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            if (strBuffer == "")
            {
                strBuffer = "5000";
                WritePrivateProfileString(appName, "DotCount_Braid", strBuffer, StrProductIni);
            }
            Inks.DotCount_Braid = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "TimeCount_AutoTray", "", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            if (strBuffer == "")
            {
                strBuffer = "1000";
                WritePrivateProfileString(appName, "TimeCount_AutoTray", strBuffer, StrProductIni);
            }
            Inks.TimeCount_AutoTray = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "TimeCount_Braid", "", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            if (strBuffer == "")
            {
                strBuffer = "1000";
                WritePrivateProfileString(appName, "TimeCount_Braid", strBuffer, StrProductIni);
            }
            Inks.TimeCount_Braid = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "DateTime_Braid", "", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            if (strBuffer == "")
            {
                strBuffer = DateTime.Now.ToString();
                WritePrivateProfileString(appName, "DateTime_Braid", strBuffer, StrProductIni);
            }
            Inks.DateTime_Braid = Convert.ToDateTime(strBuffer);
            tempStr.Clear();
        }

        /// <summary>
        /// 写油墨数据
        /// </summary>
        public void WriteInksDate()
        {
            String appName = "InksDate";

            WritePrivateProfileString(appName, "Enabled_AutoTray", Inks.Enabled_AutoTray == true ? "1" : "0", StrProductIni);
            WritePrivateProfileString(appName, "Enabled_Braid", Inks.Enabled_Braid == true ? "1" : "0", StrProductIni);

            WritePrivateProfileString(appName, "DotCount_AutoTray", Inks.DotCount_AutoTray.ToString(), StrProductIni);
            WritePrivateProfileString(appName, "TimeCount_AutoTray", Inks.TimeCount_AutoTray.ToString(), StrProductIni);
            WritePrivateProfileString(appName, "DotCount_Braid", Inks.DotCount_Braid.ToString(), StrProductIni);
            WritePrivateProfileString(appName, "TimeCount_Braid", Inks.TimeCount_Braid.ToString(), StrProductIni);
            WritePrivateProfileString(appName, "DateTime_Braid", Inks.DateTime_Braid.ToString(), StrProductIni);
        }

        /// <summary>
        /// 读测高仪高度值
        /// </summary>
        public void ReadAltimeterHeightValue()
        {
            String appName = "AltimeterHeightValue";
            StringBuilder tempStr = new StringBuilder();
            string strBuffer;

            GetPrivateProfileString(appName, "Offset_Socket_X", "0", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            Axis.Altimeter.Offset_Socket_X = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "Offset_Socket_Y", "0", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            Axis.Altimeter.Offset_Socket_Y = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "ValueEmptyIC", "1", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            Axis.Altimeter.ValueEmptyIC = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "ValueExistIC", "0", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            Axis.Altimeter.ValueExistIC = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "HeightDifference", "1.5", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            Axis.Altimeter.HeightDifference = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            Axis.Altimeter.Thickness = Axis.Altimeter.ValueEmptyIC - Axis.Altimeter.ValueExistIC;

            for (int i = 0; i < UserConfig.AllScketC; i++)
            {
                GetPrivateProfileString(appName, "Socket_" + i, "0", tempStr, _MAX_FNAME, StrProductIni);
                strBuffer = tempStr.ToString();
                Axis.Group[i / UserConfig.ScketUnitC].Unit[i % UserConfig.ScketUnitC].HeightVal = Convert.ToDouble(strBuffer);
                tempStr.Clear();
            }

            GetPrivateProfileString(appName, "Tray", "0", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            HeightVal_Altimeter.Tray = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "BredeIn", "0", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            HeightVal_Altimeter.BredeIn = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "BredeOut", "0", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            HeightVal_Altimeter.BredeOut = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "TubeIn", "0", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            HeightVal_Altimeter.TubeIn = Convert.ToDouble(strBuffer);
            tempStr.Clear();
        }

        /// <summary>
        /// 写测高仪高度值
        /// </summary>
        public void WriteAltimeterHeightValue()
        {
            String appName = "AltimeterHeightValue";

            WritePrivateProfileString(appName, "Offset_Socket_X", Axis.Altimeter.Offset_Socket_X.ToString(), StrProductIni);
            WritePrivateProfileString(appName, "Offset_Socket_Y", Axis.Altimeter.Offset_Socket_Y.ToString(), StrProductIni);
            WritePrivateProfileString(appName, "ValueEmptyIC", Axis.Altimeter.ValueEmptyIC.ToString(), StrProductIni);
            WritePrivateProfileString(appName, "ValueExistIC", Axis.Altimeter.ValueExistIC.ToString(), StrProductIni);
            WritePrivateProfileString(appName, "HeightDifference", Axis.Altimeter.HeightDifference.ToString(), StrProductIni);

            WritePrivateProfileString(appName, "Tray", HeightVal_Altimeter.Tray.ToString(), StrProductIni);
            WritePrivateProfileString(appName, "BredeIn", HeightVal_Altimeter.BredeIn.ToString(), StrProductIni);
            WritePrivateProfileString(appName, "BredeOut", HeightVal_Altimeter.BredeOut.ToString(), StrProductIni);
            WritePrivateProfileString(appName, "TubeIn", HeightVal_Altimeter.TubeIn.ToString(), StrProductIni);
        }

        /// <summary>
        /// 写烧录座测高仪高度值
        /// </summary>
        public void WriteSocketAltimeterHeightValue(int ind)
        {
            String appName = "AltimeterHeightValue";
            double temp = Axis.Group[ind / UserConfig.ScketUnitC].Unit[ind % UserConfig.ScketUnitC].HeightVal;
            WritePrivateProfileString(appName, "Socket_" + ind, temp.ToString(), StrProductIni);
        }

        /// <summary>
        /// 读镜头焦距
        /// </summary>
        public void ReadZoomLensValue()
        {
            String appName = "ZoomLensValue";
            StringBuilder tempStr = new StringBuilder();
            string strBuffer;

            GetPrivateProfileString(appName, "Socket", "0", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            Axis.ZoomLens_S.Socket = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "Tray", "0", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            Axis.ZoomLens_S.Tray = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "BredeIn", "0", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            Axis.ZoomLens_S.BredeIn = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "BredeOut", "0", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            Axis.ZoomLens_S.BredeOut = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "TubeIn", "0", tempStr, _MAX_FNAME, StrProductIni);
            strBuffer = tempStr.ToString();
            Axis.ZoomLens_S.TubeIn = Convert.ToDouble(strBuffer);
            tempStr.Clear();
        }

        /// <summary>
        /// 写镜头焦距
        /// </summary>
        public void WriteZoomLensValue()
        {
            String appName = "ZoomLensValue";

            WritePrivateProfileString(appName, "Socket", Axis.ZoomLens_S.Socket.ToString(), StrProductIni);
            WritePrivateProfileString(appName, "Tray", Axis.ZoomLens_S.Tray.ToString(), StrProductIni);
            WritePrivateProfileString(appName, "BredeIn", Axis.ZoomLens_S.BredeIn.ToString(), StrProductIni);
            WritePrivateProfileString(appName, "BredeOut", Axis.ZoomLens_S.BredeOut.ToString(), StrProductIni);
            WritePrivateProfileString(appName, "TubeIn", Axis.ZoomLens_S.TubeIn.ToString(), StrProductIni);
        }

        /// <summary>
        /// 读Mark数据
        /// </summary>
        public void ReadMarkValue()
        {
            String appName = "Tray";
            StringBuilder tempStr = new StringBuilder();
            string strBuffer;
            GetPrivateProfileString(appName, "Offset_X", "", tempStr, _MAX_FNAME, StrMarkIni);
            strBuffer = tempStr.ToString();
            if (strBuffer == "")
            {
                strBuffer = "14";
                WritePrivateProfileString(appName, "Offset_X", strBuffer, StrMarkIni);
            }
            MarkOffset.Tray_X = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "Offset_Y", "", tempStr, _MAX_FNAME, StrMarkIni);
            strBuffer = tempStr.ToString();
            if (strBuffer == "")
            {
                strBuffer = "14";
                WritePrivateProfileString(appName, "Offset_Y", strBuffer, StrMarkIni);
            }
            MarkOffset.Tray_Y = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "Col", "", tempStr, _MAX_FNAME, StrMarkIni);
            strBuffer = tempStr.ToString();
            if (strBuffer == "")
            {
                strBuffer = "25";
                WritePrivateProfileString(appName, "Col", strBuffer, StrMarkIni);
            }
            TrayD.ColC = MarkOffset.Tray_Col = Convert.ToInt32(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "Rol", "", tempStr, _MAX_FNAME, StrMarkIni);
            strBuffer = tempStr.ToString();
            if (strBuffer == "")
            {
                strBuffer = "10";
                WritePrivateProfileString(appName, "Rol", strBuffer, StrMarkIni);
            }
            TrayD.RowC = MarkOffset.Tray_Rol = Convert.ToInt32(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "Coldis", "", tempStr, _MAX_FNAME, StrMarkIni);
            strBuffer = tempStr.ToString();
            if (strBuffer == "")
            {
                strBuffer = "12.2";
                WritePrivateProfileString(appName, "Coldis", strBuffer, StrMarkIni);
            }
            TrayD.Col_Space = MarkOffset.Tray_Coldis = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "Roldis", "", tempStr, _MAX_FNAME, StrMarkIni);
            strBuffer = tempStr.ToString();
            if (strBuffer == "")
            {
                strBuffer = "12.6";
                WritePrivateProfileString(appName, "Roldis", strBuffer, StrMarkIni);
            }
            TrayD.Row_Space = MarkOffset.Tray_Roldis = Convert.ToDouble(strBuffer);
            tempStr.Clear();
        }

        /// <summary>
        /// 写Mark数据
        /// </summary>
        public void WriteMarkValue(double[] date1, int[] date2, string ProdIni)
        {
            String appName = "Tray";

            WritePrivateProfileString(appName, "Offset_X", date1[0].ToString(), ProdIni);
            WritePrivateProfileString(appName, "Offset_Y", date1[1].ToString(), ProdIni);
            WritePrivateProfileString(appName, "Col", date2[0].ToString(), ProdIni);
            WritePrivateProfileString(appName, "Rol", date2[1].ToString(), ProdIni);
            WritePrivateProfileString(appName, "Coldis", date1[2].ToString(), ProdIni);
            WritePrivateProfileString(appName, "Roldis", date1[3].ToString(), ProdIni);
        }

        /// <summary>
        /// 读Mark数据
        /// </summary>
        public void ReadMarkValue(ref double[] date1, ref int[] date2, string ProdIni)
        {
            String appName = "Tray";
            StringBuilder tempStr = new StringBuilder();
            string strBuffer;
            GetPrivateProfileString(appName, "Offset_X", "", tempStr, _MAX_FNAME, ProdIni);
            strBuffer = tempStr.ToString();
            if (strBuffer == "")
            {
                strBuffer = "14";
                WritePrivateProfileString(appName, "Offset_X", strBuffer, ProdIni);
            }
            date1[0] = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "Offset_Y", "", tempStr, _MAX_FNAME, ProdIni);
            strBuffer = tempStr.ToString();
            if (strBuffer == "")
            {
                strBuffer = "14";
                WritePrivateProfileString(appName, "Offset_Y", strBuffer, ProdIni);
            }
            date1[1] = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "Col", "", tempStr, _MAX_FNAME, ProdIni);
            strBuffer = tempStr.ToString();
            if (strBuffer == "")
            {
                strBuffer = "25";
                WritePrivateProfileString(appName, "Col", strBuffer, ProdIni);
            }
            date2[0] = MarkOffset.Tray_Col = Convert.ToInt32(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "Rol", "", tempStr, _MAX_FNAME, ProdIni);
            strBuffer = tempStr.ToString();
            if (strBuffer == "")
            {
                strBuffer = "10";
                WritePrivateProfileString(appName, "Rol", strBuffer, ProdIni);
            }
            date2[1] = MarkOffset.Tray_Rol = Convert.ToInt32(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "Coldis", "", tempStr, _MAX_FNAME, ProdIni);
            strBuffer = tempStr.ToString();
            if (strBuffer == "")
            {
                strBuffer = "12.2";
                WritePrivateProfileString(appName, "Coldis", strBuffer, ProdIni);
            }
            date1[2] = MarkOffset.Tray_Coldis = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "Roldis", "", tempStr, _MAX_FNAME, ProdIni);
            strBuffer = tempStr.ToString();
            if (strBuffer == "")
            {
                strBuffer = "12.6";
                WritePrivateProfileString(appName, "Roldis", strBuffer, ProdIni);
            }
            date1[3] = MarkOffset.Tray_Roldis = Convert.ToDouble(strBuffer);
            tempStr.Clear();
        }


        /// <summary>
        /// 读取Mark坐标
        /// </summary>
        public void ReadMarkPosValue()
        {
            String appName = "ReadMarkPosValue";
            String keyName = "N", str;
            StringBuilder tem_str = new StringBuilder();
            for (int j = 0; j < 3; j++)
            {
                MarkPos[j] = new MarkPos_class();
                keyName = String.Format("M_{0:d}", j);
                GetPrivateProfileString(appName, keyName, "", tem_str, _MAX_FNAME, StrConfigIni);

                str = tem_str.ToString();
                if (tem_str.ToString() == "")
                {
                    str = "500,500,500,500,500,500,500,500";
                    WritePrivateProfileString(appName, keyName, str, StrConfigIni);
                }

                string[] arrStr = str.Split(',');

                double.TryParse(arrStr[0], out double tem_num);
                MarkPos[j].Tray[0].X = tem_num;
                double.TryParse(arrStr[1], out tem_num);
                MarkPos[j].Tray[0].Y = tem_num;
                double.TryParse(arrStr[2], out tem_num);
                MarkPos[j].Tray[1].X = tem_num;
                double.TryParse(arrStr[3], out tem_num);
                MarkPos[j].Tray[1].Y = tem_num;
                double.TryParse(arrStr[4], out tem_num);
                MarkPos[j].Tray[2].X = tem_num;
                double.TryParse(arrStr[5], out tem_num);
                MarkPos[j].Tray[2].Y = tem_num;
                double.TryParse(arrStr[6], out tem_num);
                MarkPos[j].Tray[3].X = tem_num;
                double.TryParse(arrStr[7], out tem_num);
                MarkPos[j].Tray[3].Y = tem_num;
            }
        }

        /// <summary>
        /// 写Mark坐标
        /// </summary>
        public void WriteMarkPosValue()
        {
            String appName = "ReadMarkPosValue";
            String keyName = String.Format("M_{0:d}", TrayModel);
            string str = String.Format("{0:f},{1:f},{2:f},{3:f},{4:f},{5:f},{6:f},{7:f}",
                MarkPos[TrayModel].Tray[0].X, MarkPos[TrayModel].Tray[0].Y, MarkPos[TrayModel].Tray[1].X, MarkPos[TrayModel].Tray[1].Y,
                MarkPos[TrayModel].Tray[2].X, MarkPos[TrayModel].Tray[2].Y, MarkPos[TrayModel].Tray[3].X, MarkPos[TrayModel].Tray[3].Y);

            WritePrivateProfileString(appName, keyName, str, StrConfigIni);
        }

        /// <summary>
        /// 读Mark点修正量
        /// </summary>
        public void ReadMarkCorrection()
        {
            String appName = "MarkCorrection";
            String keyName = "N", str;
            StringBuilder tem_str = new StringBuilder();
            for (int j = 0; j < 3; j++)
            {
                keyName = String.Format("M_{0:d}", j);
                GetPrivateProfileString(appName, keyName, "", tem_str, _MAX_FNAME, StrConfigIni);

                str = tem_str.ToString();
                if (tem_str.ToString() == "")
                {
                    str = "0,0,0,0,0,0,0,0";
                    WritePrivateProfileString(appName, keyName, str, StrConfigIni);
                }

                string[] arrStr = str.Split(',');

                double.TryParse(arrStr[0], out double tem_num);
                MarkPos[j].Correction[0].X = tem_num;
                double.TryParse(arrStr[1], out tem_num);
                MarkPos[j].Correction[0].Y = tem_num;
                double.TryParse(arrStr[2], out tem_num);
                MarkPos[j].Correction[1].X = tem_num;
                double.TryParse(arrStr[3], out tem_num);
                MarkPos[j].Correction[1].Y = tem_num;
                double.TryParse(arrStr[4], out tem_num);
                MarkPos[j].Correction[2].X = tem_num;
                double.TryParse(arrStr[5], out tem_num);
                MarkPos[j].Correction[2].Y = tem_num;
                double.TryParse(arrStr[6], out tem_num);
                MarkPos[j].Correction[3].X = tem_num;
                double.TryParse(arrStr[7], out tem_num);
                MarkPos[j].Correction[3].Y = tem_num;
            }
        }

        /// <summary>
        /// 写Mark修正量
        /// </summary>
        public void WriteMarkCorrection()
        {
            String appName = "MarkCorrection";
            String keyName = String.Format("M_{0:d}", TrayModel);
            string str = String.Format("{0:f},{1:f},{2:f},{3:f},{4:f},{5:f},{6:f},{7:f}",
                MarkPos[TrayModel].Correction[0].X, MarkPos[TrayModel].Correction[0].Y, MarkPos[TrayModel].Correction[1].X, MarkPos[TrayModel].Correction[1].Y,
                MarkPos[TrayModel].Correction[2].X, MarkPos[TrayModel].Correction[2].Y, MarkPos[TrayModel].Correction[3].X, MarkPos[TrayModel].Correction[3].Y);

            WritePrivateProfileString(appName, keyName, str, StrConfigIni);
        }

        /// <summary>
        /// 读外挂烧录器参数名
        /// </summary>
        public void ReadBurnPara()
        {
            String appName = "BurnPara";
            StringBuilder tempStr = new StringBuilder();
            string strBuffer, keyName;

            for (int i = 0; i < BurnPara.Name.Length; i++)
            {
                keyName = string.Format("Name_{0:d}", i);
                GetPrivateProfileString(appName, keyName, "", tempStr, _MAX_FNAME, StrConfigIni);
                strBuffer = tempStr.ToString();
                if (strBuffer == "")
                {
                    strBuffer = string.Format("自定义_{0:d}", i);
                    WritePrivateProfileString(appName, keyName, strBuffer, StrConfigIni);
                }
                BurnPara.Name[i] = strBuffer;
                tempStr.Clear();
            }
        }

        /// <summary>
        /// 写外挂烧录器参数名
        /// </summary>
        public void WriteBurnPara()
        {
            String appName = "BurnPara";
            StringBuilder tempStr = new StringBuilder();
            string keyName;

            for (int i = 0; i < BurnPara.Name.Length; i++)
            {
                keyName = string.Format("Name_{0:d}", i);
                WritePrivateProfileString(appName, keyName, BurnPara.Name[i], StrConfigIni);
            }
        }

        /// <summary>
        /// 读吸笔取放料位置偏移
        /// </summary>
        public void ReadPenOffset()
        {
            String appName = "PenOffset";
            StringBuilder tempStr = new StringBuilder();
            string strBuffer;
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                GetPrivateProfileString(appName, "Carrier_X" + i, "", tempStr, _MAX_FNAME, StrProductIni);
                strBuffer = tempStr.ToString();
                if (strBuffer == "")
                {
                    strBuffer = "0";
                    WritePrivateProfileString(appName, "Carrier_X" + i, strBuffer, StrProductIni);
                }
                Axis.Pen[i].Offset_Carrier_X = Convert.ToDouble(strBuffer);
                tempStr.Clear();

                GetPrivateProfileString(appName, "Carrier_Y" + i, "", tempStr, _MAX_FNAME, StrProductIni);
                strBuffer = tempStr.ToString();
                if (strBuffer == "")
                {
                    strBuffer = "0";
                    WritePrivateProfileString(appName, "Carrier_Y" + i, strBuffer, StrProductIni);
                }
                Axis.Pen[i].Offset_Carrier_Y = Convert.ToDouble(strBuffer);
                tempStr.Clear();
            }
            GetPrivateProfileString(appName, "Base_X", "", tempStr, _MAX_FNAME, StrConfigIni);
            strBuffer = tempStr.ToString();
            if (strBuffer == "")
            {
                strBuffer = "0";
                WritePrivateProfileString(appName, "Base_X", strBuffer, StrConfigIni);
            }
            Axis.Pen[0].Offset_Base_X = Convert.ToDouble(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "Base_Y", "", tempStr, _MAX_FNAME, StrConfigIni);
            strBuffer = tempStr.ToString();
            if (strBuffer == "")
            {
                strBuffer = "0";
                WritePrivateProfileString(appName, "Base_Y", strBuffer, StrConfigIni);
            }
            Axis.Pen[0].Offset_Base_Y = Convert.ToDouble(strBuffer);
            tempStr.Clear();
        }

        /// <summary>
        /// 写吸笔取放料位置偏移
        /// </summary>
        public void WritePenOffset()
        {
            String appName = "PenOffset";
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                WritePrivateProfileString(appName, "Carrier_X" + i, Axis.Pen[i].Offset_Carrier_X.ToString(), StrProductIni);
                WritePrivateProfileString(appName, "Carrier_Y" + i, Axis.Pen[i].Offset_Carrier_Y.ToString(), StrProductIni);
            }
            WritePrivateProfileString(appName, "Base_X", Axis.Pen[0].Offset_Base_X.ToString(), StrConfigIni);
            WritePrivateProfileString(appName, "Base_Y", Axis.Pen[0].Offset_Base_Y.ToString(), StrConfigIni);
        }
    }
}
