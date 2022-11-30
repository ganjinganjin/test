using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HalconDotNet;
using BAR.Commonlib;
using BAR.Commonlib.Components;
using BAR.Commonlib.Utils;
using BAR.Commonlib.Events;
using BAR.Windows;
using PLC;
using System.Threading;
using QXZused;
using CCWin.SkinControl;
using System.Threading.Tasks;
using TensionDetectionDLL;
using SuperDog;
using System.Xml;
using System.Drawing.Drawing2D;
using LBSoft.IndustrialCtrls.Leds;
using BAR.Communicators;
using System.Net.Sockets;
using System.IO;
using System.Runtime.InteropServices;
using BAR.ControlPanels;
using BAR.WinPanels;
using BAR.Commonlib.Connectors;
using System.Runtime.CompilerServices;
using BAR.CommonLib_v1._0;

namespace BAR
{
    public partial class BAR : Form
    {
        #region
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);
        const int BM_CLICK = 0xF5;

        public Config g_config = Config.GetInstance();
        public Act g_act = Act.GetInstance();
        public Axis axis = Axis.GetInstance();
        public static ToolTipWnd _ToolTipDlg;
        public static ModifyICPosWnd _ModifyICPosWnd;
        private Btn btn = new Btn();

        PLC1 plc = new PLC1();
        In_Output in_Output = new In_Output();
        Download download;
        Brede brede;
        AutoTray autoTray;
        //AutoTube autoTube;
        Cathetometer cathetometer;
        ZoomLens zoomLens;
        Startup startup = new Startup();
        public TrayState trayState = new TrayState();
        TeachAction teachAction = new TeachAction();
        AutoSizeForm autoSize = new AutoSizeForm();
        public static MyTcpClient tcpClient_3D = new MyTcpClient();
        public RPCServer rpcServer = RPCServer.GetInstance();
        public PhenixIOS phenixIOS = PhenixIOS.GetInstance();
        private HalconImgUtil _HalconUtil = HalconImgUtil.GetInstance();

        public Dog dog;//超级狗

        DateTime StTime1, StTime2, StTime3, StTime4;

        private HalconImgUtil _HalconImg = new HalconImgUtil();
        private ImgOperater[] _ImgOperaters = new ImgOperater[2];
        private HTuple _HalconWndID;

        private bool[] _IsHaveImg = new bool[2];

        public bool m0 = false;  //检测加载卡是否成功
        public bool m1 = false;  //检测IO扫描是否成功
        public bool IsShowWarningDoor = false;
        public bool IsShowWarning = false;
        public bool IsShowWarningLimit = false;
        public bool[] IsWriteLimitStr = new bool[10];
        
        LoadingBar          _LoadBar;
        public StaticalWnd  _StaticalWnd;
        BurnSeatStaticWnd   _BurnSeatStaticWnd;
        AxisParametersWnd   _AxisParWnd;
        AxisCtlWnd          _AxisCtlWnd;
        ConfigInitWnd       _ConfigInitWnd;
        UserChangeWnd       _UserChangeWnd;
        UserInfoWnd         _UserInfoWnd;
        UserLoginWnd        _UserLoginWnd;
        UserModifierWnd     _UserModifierWnd ;
        UserRegisterWnd     _UserRegisterWnd;
        AxisIOWnd           _AxisIOWnd;
        GPIOWnd             _GPIOWnd;
        BurnParaWnd         _BurnParaWnd;
        DevPara             _DevParaWnd;
        BraidParametersWnd  _BraidParaWnd;
        AutoTrayParameters  _AutoTrayParametersWnd;
        CylinderCtlWnd      _CylinderCtlWnd;
        FuncSwitWnd         _FuncSwitWnd;
        ProManageWnd        _ProManageWnd;
        WarningWnd          _WarningWnd;
        WarningDoorWnd      _WarningDoorWnd;
        WarningLimitWnd     _warningLimitWnd;
        MesWnd              _MesWnd;
        EngineerSettingWnd  _EngineerSettingWnd;
        ClosureWnd          _ClosureWnd;
        InksManageWnd       _InksManageWnd;
        TicketsWnd          _TicketsWnd;


        Thread loadThd = null;
        Thread MainLogic = null;

        /// <summary>
        /// SuperDog DEMOMA vendor code
        /// </summary>
        private const string vendorCodeString = "XMfclTd+0uJ9hZ37/P13VoXIkMGWNeOsh9LqxfND/kX78WsvOjUzEOd+lYRHUa+ClV0RRXMyhedgtI4y" +
                                                "Ib4/Y5WlWdsfkdtokpCr1/dS5VjmEPfq//a3a7oYDoKPBYl0tk+ERJzH+UpwFOepA+P95q3pzHTG5Bvt" +
                                                "pxZP4lJkeSyOiG85pV7UL/VWlWACkU8II592jRTg2RnjYElftQSfbX2EHgW5fxF49IeucBWvGoXsvAc2" +
                                                "4aBCi0jSZhrCDNlw4meGx4fADIpd6HCOY2QftXgx6SrDn0tKKcW1z3d16iekMSCPhFN6kpBqn5cQ4DZe" +
                                                "cVNEibaRH5xoFNGl1/XaHSQ3LHj/yvp4FhLLzwOi+f5uLHR7QgEo/jnj+engsTzz/StZpPePx0XLtQuD" +
                                                "75JJsm4pulGACFn6lwlShRyopl8/BZ91glUJtIe6DAL3TPinu4QUubqcfSuuu5rzxlpHFQ6AGZsezItw" +
                                                "0bGRITH5waecY8MK7Zj2FJEvjm5lrjI3uYTImWnD/mFl5LCoH+Wa/zB43PnwJWWpSEfikNk6YVLj0RgY" +
                                                "BmfjmjK+n1/VOzmM0Vr4vR6ExDAw6vCKrwnrW5r6lxSUAN+KratwrtDdHtTTqmx6BPA0OZnV8wcu1ytA" +
                                                "87JT/NibkUaWzBiV3SQr3KN4ITTKOFEgGpwR8DsI0HYahGUAlNQYByDtTnaVfoa1N/W30YGHkQ5nVO+m" +
                                                "xXyFTObUbr9WiLo//NBpoRZyVLkg4jo0dRj8KwusyBsT+oMMWsCXw4m/pyj6pYlrqyR672cORGJu8jTp" +
                                                "1nvbghBzICnEPwA/OqQN9KsKd07V2qrlzfxmbf7jqLIL9a2lyXIbfqtVNfUwCL1gqFU5v6Vbr/YcNj8V" +
                                                "XKAJliS3Ynbl8YKEmpQzzsSlEufLsvDO70zsdgIUBtS9BHLFFCM1mKoPy/NF5Zfs6Tqp58tFCfw+aP4z" +
                                                "WCB7FoNPpe5FJqU4VFYgxA==";


        public const string defaultScope = "<dogscope />";
        #endregion
        public BAR()
        {
            MultiLanguage.SetDefaultLanguage();
            //设置窗体的双缓冲
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            InitializeComponent();
            g_act.MainWnd = this;

            //吸笔使能控件
            Run.PenEnable = new SkinCheckBox[4];
            Run.PenEnable[0] = this.PenEnable1;
            Run.PenEnable[1] = this.PenEnable2;
            Run.PenEnable[2] = this.PenEnable3;
            Run.PenEnable[3] = this.PenEnable4;

            TrayState._PlatePanel = PlatePanel;
            TrayState._RectPanels = new SkinPanel[3];
            TrayState._RectPanels[0] = this.Pan1Pnl;
            TrayState._RectPanels[1] = this.Pan2Pnl;
            TrayState._RectPanels[2] = this.Pan3Pnl;
            MultiLanguage.LoadLanguage(this, typeof(BAR));
            if (MultiLanguage.DefaultLanguage == GlobConstData.ST_English)
            {
                Brede_Status.strTmp_Close = "Close";
                Brede_Status.strTmp_HeatingUp = "Heating";
                Brede_Status.strTmp_OK = "OK";
                AutoTray.strHomeDone = "Reday";
                AutoTray.strHomeUnDone= "Uninit";
            }
            else
            {
                Brede_Status.strTmp_Close = "关闭";
                Brede_Status.strTmp_HeatingUp = "加热";
                Brede_Status.strTmp_OK = "OK";
                AutoTray.strHomeDone = "在原位";
                AutoTray.strHomeUnDone = "非原位";
            }
        }

        private void BAR_Load(object sender, EventArgs e)
        {
            UserEvent.btnAutoRun += new UserEvent.BtnAutoRun(SkinButton17_Click);
            g_act.ISelectWnd = GlobConstData.SELECT_MAIN_WND;
            _LoadBar = new LoadingBar();  //显示进度条窗体
            _LoadBar.Show(this);

            this._LoadBar.ShowProgressPos("加载配置文件", 20);
            g_config.LoadConfig();
            ControlVisible();
            __InitProgrammerUI();
            __InitPan();
            __InitLogo();
            autoSize.controllInitializeSize(this);
            autoSize.controlAutoSize(this);
            TrayState.TrayStateUpdate(true);

            Communicate();//通讯连接

            this._LoadBar.ShowProgressPos("初始化软件界面", 40);
            SetLogo();
            this.__InitWnds();
            download = Download.GetInstance();
            brede = Brede.GetInstance();
            autoTray = AutoTray.GetInstance();
            //autoTube = AutoTube.GetInstance();
            cathetometer = Cathetometer.GetInstance();
            zoomLens = ZoomLens.GetInstance();
            loadThd = new Thread(OnLoadHandler);
            loadThd.IsBackground = true;
            loadThd.Start();
            this.productLab.Text = "Product: " + g_config.StrCurProduct;
            __DelOutTimeLogFile();
            //检查超级狗
            if (!Login())
            {
                LockAll();
                MessageBox.Show(MultiLanguage.GetString("使用权限已到期,请联系供应商!"), "Information:", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                OpenAll();
                timerDog.Start();
            }
            //OpenAll();
            timerShowWnd.Start();

            PermissionControl();
        }

        /// <summary>
        /// 通讯连接
        /// </summary>
        private void Communicate()
        {
            this._LoadBar.ShowProgressPos("开始连接通信", 30);
            if (g_config.ILightCType == GlobConstData.LightCtl_WAN)
            {
                if (g_act.NetLightCtrl.CreateSocket(3, g_config.StrLightIP, g_config.ILightPort, SocketType.Dgram))
                {
                    g_act.NetLightCtrl.SetUDPInfo(g_config.StrLightIP, g_config.ILightPort);
                }
                else
                {
                    g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "连接光源控制器失败 IP:192.168.0.10", "");
                }
            }
            else
            {
                if (g_act.SerialLightCtrl.CreateSerialPort())
                {
                    g_act.SerialLightCtrl.OpenConnection(g_config.ArrStrutCom[0].ICom, g_config.ArrStrutCom[0].IBaud, g_config.ArrStrutCom[0].IDataBits, g_config.ArrStrutCom[0].IParity, g_config.ArrStrutCom[0].IStopBits, "Light Ports");
                }
            }
            //创建RPC服务器
            if (Mes.Type == GlobConstData.ST_MESTYPE_RPC)
            {
                rpcServer.Create();
            }
            //连接至凤凰平台服务器
            if (Config.PhenixIOS)
            {
                phenixIOS.Register();
                phenixIOS.Connect();
            }
        }

        /// <summary>
        /// 控件可视化
        /// </summary>
        private void ControlVisible()
        {
            bool flag;
            toolStripBtn12.Visible = (UserTask.ProgrammerType == GlobConstData.Programmer_WG || UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY) && UserTask.Protocol_WG == GlobConstData.Protocol_WG;
            BurnOnlineLed.Visible = flag = UserTask.ProgrammerType != GlobConstData.Programmer_AK && UserTask.ProgrammerType != GlobConstData.Programmer_RD;
            ZLOnlineLed.Visible = Config.ZoomLens == 1 ? true : false;
            if (!flag)
            {
                ZLOnlineLed.Location = BurnOnlineLed.Location;
            }
            for (int i = 0; i < 4; i++)
            {
                 Run.PenEnable[i].Visible = i < UserConfig.VacuumPenC;
                string ctlName;
                Control[] tagAry;
                ctlName = "z" + (i + 1) + "Lab";
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    (tagAry.First() as Label).Visible = i < UserConfig.AxisZC;
                }

                ctlName = "labelZ" + (i + 1);
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    (tagAry.First() as Label).Visible = i < UserConfig.AxisZC;
                }
            }
        }

        /// <summary>
        /// 初始化料盘
        /// </summary>
        private void __InitPan()
        {
            if (g_config.TrayModel == 0) 
            {
                //料盘1、2、3横
                SetPanLocation(532, 2, 530, 268, 532, 270, 530, 268, 2, 2, 530, 268, 150, 270, 381, 268, 2, 271, false);
            }
            else if(g_config.TrayModel == 1)
            {
                //料盘1、2竖（自动盘右），料盘3横
                SetPanLocation(264, 2, 262, 536, 2, 2, 262, 536, 526, 2, 536, 262, 825, 263, 237, 282, 527, 270, true);
            }
            else if (g_config.TrayModel == 2)
            {
                //料盘2竖（自动盘右），料盘1、3横
                SetPanLocation(2, 270, 500, 268, 502, 2, 268, 536, 2, 2, 500, 268, 770, 270, 292, 268, 769, 0, true);
            }
            else if (g_config.TrayModel == 3)
            {
                //料盘2竖（自动盘左），料盘1、3横
                SetPanLocation(269, 270, 500, 268, 2, 2, 268, 536, 269, 2, 500, 268, 770, 270, 292, 268, 769, 0, true);
            }
            else if (g_config.TrayModel == 4)
            {
                //料盘1、2竖（自动盘左），料盘3横
                SetPanLocation(2, 2, 262, 536, 264, 2, 262, 536, 526, 2, 536, 262, 825, 263, 237, 282, 527, 270, true);
            }
            else if (g_config.TrayModel == 5)
            {
                g_config.TrayRotateDir[2] = 0;
                //料盘1、2、3竖
                SetPanLocation(264, 2, 262, 536, 2, 2, 262, 536, 526, 2, 262, 536, 788, 268, 274, 270, 786, 2, true);
            }
        }

        /// <summary>
        /// 初始化Logo
        /// </summary>
        private void __InitLogo()
        {
            if (g_config.ILogo == GlobConstData.Logo_DP || g_config.ILogo == GlobConstData.Logo_ZHH || g_config.ILogo == GlobConstData.Logo_LYDZ)
            {
                pictureBox1.BackColor = Color.White;
            }
        }

        /// <summary>
        /// 配置料盘摆向
        /// </summary>
        private void SetPanLocation(int Pan1X, int Pan1Y, int Pan1W, int Pan1H, int Pan2X, int Pan2Y, int Pan2W, int Pan2H, 
            int Pan3X, int Pan3Y, int Pan3W, int Pan3H, int LogX, int LogY, int LogW, int LogH, int StatusBarX, int StatusBarY, bool flag)
        {
            Pan1Pnl.Location = new Point(Pan1X, Pan1Y);
            Pan1Pnl.Size = new Size(Pan1W, Pan1H);

            Pan2Pnl.Location = new Point(Pan2X, Pan2Y);
            Pan2Pnl.Size = new Size(Pan2W, Pan2H);

            Pan3Pnl.Location = new Point(Pan3X, Pan3Y);
            Pan3Pnl.Size = new Size(Pan3W, Pan3H);

            textBox1.Location = new Point(LogX, LogY);
            textBox1.Size = new Size(LogW, LogH);

            panel1.Location = new Point(StatusBarX, StatusBarY);
            if (flag)
            {
                panel2.Visible = true;
                panel2.Location = new Point(StatusBarX + panel1.Width - 5, StatusBarY);
            }
        }

        /// <summary>
        /// 设置Logo
        /// </summary>
        private void SetLogo()
        {
            try
            {
                pictureBox1.BackgroundImage = Image.FromFile(Path.Combine(Application.StartupPath, "Logo", "Logo_" + g_config.ILogo + ".bmp"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        /// <summary>
        /// 初始化编程器显示
        /// </summary>
        private void __InitProgrammerUI()
        {
            UserControl userControl = new ProgrammerPnl();
            pnlProgrammer.Controls.Add(userControl);
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        public void PermissionControl()
        {
            if (StaticInfo.TDLevel == 1)
            {
                _ConfigInitWnd.PermissionControl(true);
                btnEngineerSetting.Visible = true;
                toolStripBtn9.Visible = true;
                SetControl(true);
            }
            else if (StaticInfo.TDLevel == 2)
            {
                _ConfigInitWnd.PermissionControl(false);
                btnEngineerSetting.Visible = false;
                toolStripBtn9.Visible = false;
                SetControl(true);
            }
            else
            {
                _ConfigInitWnd.PermissionControl(false);
                btnEngineerSetting.Visible = false;
                toolStripBtn9.Visible = false;
                SetControl(false);
            }
                

            if (Mes.Type != 0 && Mes.Type != GlobConstData.ST_MESTYPE_RPC)
            {
                toolStripMenuItem1.Visible = true;
            }

            if (Inks.Function)
            {
                toolStripMenuItem6.Visible = true;
            }

            if (g_config.ProgrammerType == GlobConstData.Programmer_DP)
            {
                toolStripMenuItem7.Visible = true;
            }
        }

        /// <summary>
        /// 设置控件属性
        /// </summary>
        /// <param name="flag"></param>
        public void SetControl(bool flag)
        {
            _ProManageWnd.addProBtn.Enabled = flag;
            _ProManageWnd.modifyProBtn.Enabled = flag;
            _ProManageWnd.delProBtn.Enabled = flag;
            toolStripBtn4.Enabled = flag;
            toolStripBtn10.Enabled = flag;
            toolStripBtn11.Enabled = flag;
            toolStripMenuItem3.Enabled = flag;
        }

        /// <summary>
        /// 初始化子窗口
        /// </summary>
        private void __InitWnds()
        {
            _StaticalWnd = new StaticalWnd();
            _BurnSeatStaticWnd = new BurnSeatStaticWnd();
            _AxisCtlWnd = new AxisCtlWnd();
            _AxisParWnd = new AxisParametersWnd();
            _ConfigInitWnd = new ConfigInitWnd();
            _UserChangeWnd = new UserChangeWnd();
            _UserInfoWnd = new UserInfoWnd();
            _UserLoginWnd = new UserLoginWnd();
            _UserModifierWnd = new UserModifierWnd();
            _UserRegisterWnd = new UserRegisterWnd();
            _ProManageWnd = new ProManageWnd();
            _FuncSwitWnd = new FuncSwitWnd();
            _AxisIOWnd = new AxisIOWnd();
            _GPIOWnd = new GPIOWnd();
            _DevParaWnd = new DevPara();
            _BraidParaWnd = new BraidParametersWnd();
            _AutoTrayParametersWnd = new AutoTrayParameters();
            _CylinderCtlWnd = new CylinderCtlWnd();
            _WarningWnd = new WarningWnd();
            _WarningDoorWnd = new WarningDoorWnd();
            _warningLimitWnd = new WarningLimitWnd();
            _MesWnd = new MesWnd();
            _ToolTipDlg = new ToolTipWnd(this);
            _EngineerSettingWnd = new EngineerSettingWnd();
            _ClosureWnd = new ClosureWnd();
            _InksManageWnd = new InksManageWnd();
            _ModifyICPosWnd = new ModifyICPosWnd();
            _TicketsWnd=new TicketsWnd();

            _StaticalWnd.Owner = this;
            _BurnSeatStaticWnd.Owner = this;
            _AxisCtlWnd.Owner = this;
            _AxisParWnd.Owner = this;
            _ConfigInitWnd.Owner = this;
            _UserChangeWnd.Owner = this;
            _UserInfoWnd.Owner = this;
            _UserLoginWnd.Owner = this;
            _UserModifierWnd.Owner = this;
            _UserRegisterWnd.Owner = this;
            _ProManageWnd.Owner = this;
            _FuncSwitWnd.Owner = this;
            _AxisIOWnd.Owner = this;
            _GPIOWnd.Owner = this;
            _BraidParaWnd.Owner = this;
            _AutoTrayParametersWnd.Owner = this;
            _CylinderCtlWnd.Owner = this;
            _DevParaWnd.Owner = this;
            _WarningWnd.Owner = this;
            _WarningDoorWnd.Owner = this;
            _warningLimitWnd.Owner = this;
            _MesWnd.Owner = this;
            _ToolTipDlg.Owner = this;
            _EngineerSettingWnd.Owner = this;
            _ClosureWnd.Owner = this;
            _InksManageWnd.Owner = this;
            _ModifyICPosWnd.Owner = this;
            _TicketsWnd.Owner = this;
            if ((UserTask.ProgrammerType == GlobConstData.Programmer_WG || UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY) && UserTask.Protocol_WG == GlobConstData.Protocol_WG)
            {
                _BurnParaWnd = new BurnParaWnd();
            }
        }

        /// <summary>
        /// 删除过期的日志文件
        /// </summary>
        private void __DelOutTimeLogFile()
        {
            string logPath = AppDomain.CurrentDomain.BaseDirectory + @"\Log\";
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
            DirectoryInfo dirInfo = new DirectoryInfo(logPath);
            DirectoryInfo[] dirs = dirInfo.GetDirectories();
            DateTime date;

            for (int i = 0; i < dirs.Length; i++)
            {
                String fileName = dirs[i].Name;
                bool isDel = true;
                for(int j = 0; j <= 30; j++)
                {
                    int day = j - 30;
                    date = DateTime.Now.AddDays(day);
                    string strDate = date.ToString("yyyyMMdd");
                    if (fileName == strDate)
                    {
                        isDel = false;
                        continue;
                    }  
                }
                if(isDel)
                {
                    Act.DelectDir(logPath + fileName);
                }
            }
           
        }
        private void OnLoadHandler()
        {
            try//TCP
            {
                if (Vision_3D.Function)//3D网口
                {
                    this._LoadBar.ShowProgressPos("正在连接2.5D相机", 50);
                    tcpClient_3D.Connect(Vision_3D.IP, Vision_3D.Port);
                }
            }
            catch (Exception ex)
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, ex.Message, "Error");
            }

            InitMotionCard();//初始化运动控制卡

            try//相机
            {
                this._LoadBar.ShowProgressPos("初始化图像操作运行库", 80);
                __CreateHalconWnd();
                __InitOperateWnd(GlobConstData.ST_CCDUP);
                __InitOperateWnd(GlobConstData.ST_CCDDOWN);
                this._LoadBar.ShowProgressPos("正在打开相机", 95);
                __InitCamera(); //初始化相机    
                this._LoadBar.ShowProgressPos("相机打开成功", 100);
            }
            catch (Exception ex)
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, ex.Message, "Error");
                this._LoadBar.ShowProgressPos("相机打开失败", 95);
            }
            this._LoadBar.Close();
           
            //g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, StaticInfo.TDUser + " 已登录", "");
        }
        /// <summary>
        /// 登录验证
        /// </summary>
        protected bool Login()
        {
            bool p0 = false;
            DogFeature feature = DogFeature.FromFeature(13032);
            dog = new Dog(feature);
            //调用登录函数
            DogStatus status = dog.Login(vendorCodeString, defaultScope);

            //判断超级狗执行登录是否成功
            if (DogStatus.StatusOk == status)
            {
                p0 = true;
            }
            else
            {
                p0 = false;
            }
            //dog.Dispose();
            return p0;
        }

        bool dialog = false;
        private void OnUpCamMsgEventHandler(object sender, MsgEvent e)
        {
            try
            {
                if (e.MsgType == MsgEvent.MSG_IMGPAINT)
                {
                    DHCameraUtil cam = (DHCameraUtil)sender;
                    if (cam.SelectedWnd == GlobConstData.SELECT_MAIN_WND)
                    {
                        if (cam.ICameraID == g_act.ISelectCam)
                        {
                            HObject tmp_img1;
                            if (cam.ICameraID == GlobConstData.ST_CCDUP)
                            {
                                HObject tmp_img2; //先缓存图像数据到两个临时变量，最后在放到全局数据
                                HOperatorSet.GenImage1(out tmp_img1, "byte", GlobConstData.IMG500_WIDTH, GlobConstData.IMG500_HEIGHT, (HTuple)g_act.ArrDHCameraUtils[g_act.ISelectCam].PtrBufferRaw);
                                HOperatorSet.MirrorImage(tmp_img1, out tmp_img2, "row");
                                tmp_img1.Dispose();
                                HOperatorSet.MirrorImage(tmp_img2, out g_act.ArrSourceImage[g_act.ISelectCam], "column");
                                tmp_img2.Dispose();
                            }

                            _HalconUtil.DispImage(_ImgOperaters[g_act.ISelectCam], g_act.ArrWndID[g_act.ISelectWnd], g_act.ArrSourceImage[g_act.ISelectCam], false, ref _IsHaveImg[g_act.ISelectCam]);
                            if (g_act.IsCamSnapMode != true)
                            {
                                g_act.ArrSourceImage[g_act.ISelectCam]?.Dispose();
                            }
                            else
                            {
                                g_act.IsSnapOver = true;
                            }
                            if(!Auto_Flag.SystemBusy)_HalconUtil.DisplayCross(_HalconWndID, _ImgOperaters[g_act.ISelectCam].DZoomWndFactor, false);
                        }
                    }
                }
            }catch(Exception ex)
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_RECORD, "MainUpCamera" + ex.Message, "Halcon");
            }
            
        }
        private void OnDownCamMsgEventHandler(object sender, MsgEvent e)
        {   
            try 
            {
                if (e.MsgType == MsgEvent.MSG_IMGPAINT)
                {
                    DHCameraUtil cam = (DHCameraUtil)sender;
                    if (cam.SelectedWnd == GlobConstData.SELECT_MAIN_WND)
                    {
                        if (cam.ICameraID == g_act.ISelectCam)
                        {
                            HObject tmp_img1;
                            if (cam.ICameraID == GlobConstData.ST_CCDDOWN)
                            {
                                HOperatorSet.GenImage1(out tmp_img1, "byte", GlobConstData.IMG130_WIDTH, GlobConstData.IMG130_HEIGHT, (HTuple)g_act.ArrDHCameraUtils[g_act.ISelectCam].PtrBufferRaw);
                                HOperatorSet.MirrorImage(tmp_img1, out g_act.ArrSourceImage[g_act.ISelectCam], "row");
                                tmp_img1.Dispose();
                                if (Auto_Flag.AutoRunBusy && Config.CCDModel == 1 && Run.ARP_Step >= 2) 
                                {
                                    while (true)
                                    {
                                        if (Axis.Pen[UserTask.PIC_PenN].ExistIC)
                                        {
                                            break;
                                        }
                                        UserTask.PIC_PenN += Auto_Flag.Ascending_ICPos ? 1 : -1;
                                    }
                                    int buffer = UserTask.PIC_PenN;
                                    g_act.ArrSourceImageBuffer[buffer] = g_act.ArrSourceImage[g_act.ISelectCam];
                                    Thread CCDTd = new Thread(() => g_act.CCDProccess_Fast(buffer, Axis.trapPrm_C[buffer].getPos));
                                    CCDTd.IsBackground = true;
                                    CCDTd.Start();
                                    UserTask.PIC_PenN += Auto_Flag.Ascending_ICPos ? 1 : -1;
                                }
                                
                            }   
                            _HalconUtil.DispImage(_ImgOperaters[g_act.ISelectCam], g_act.ArrWndID[g_act.ISelectWnd], g_act.ArrSourceImage[g_act.ISelectCam], false, ref _IsHaveImg[g_act.ISelectCam]);
                            if (g_act.IsCamSnapMode != true)
                            {
                                g_act.ArrSourceImage[g_act.ISelectCam]?.Dispose();
                            }
                            else
                            {
                                g_act.IsSnapOver = true;
                            }
                            if (!Auto_Flag.SystemBusy) _HalconUtil.DisplayCross(_HalconWndID, _ImgOperaters[g_act.ISelectCam].DZoomWndFactor, false);
                            
                        }
                    }
                }
            }catch(Exception ex)
            {
                //g_act.GenLogMessage(GlobConstData.ST_LOG_RECORD, "主界面下相机采图" + ex.Message, "Halcon");
            }
        }
        private void OnUpCamMsgEventHandler_HR(object sender, MsgEvent e)
        {
            try
            {
                if (e.MsgType == MsgEvent.MSG_IMGPAINT)
                {
                    HRCameraUtil cam = (HRCameraUtil)sender;
                    if (cam.SelectedWnd == GlobConstData.SELECT_MAIN_WND)
                    {
                        if (cam.ICameraID == g_act.ISelectCam)
                        {
                            HObject tmp_img1;
                            if (cam.ICameraID == GlobConstData.ST_CCDUP)
                            {
                                HObject tmp_img2; //先缓存图像数据到两个临时变量，最后在放到全局数据
                                HOperatorSet.GenImage1(out tmp_img1, "byte", GlobConstData.IMG500_WIDTH, GlobConstData.IMG500_HEIGHT, (HTuple)g_act.ArrHRCameraUtils[g_act.ISelectCam].PtrBufferRaw);
                                HOperatorSet.MirrorImage(tmp_img1, out tmp_img2, "row");
                                tmp_img1.Dispose();
                                HOperatorSet.MirrorImage(tmp_img2, out g_act.ArrSourceImage[g_act.ISelectCam], "column");
                                tmp_img2.Dispose();
                            }

                            _HalconUtil.DispImage(_ImgOperaters[g_act.ISelectCam], _HalconWndID, g_act.ArrSourceImage[g_act.ISelectCam], false, ref _IsHaveImg[g_act.ISelectCam]);
                            if (g_act.IsCamSnapMode != true)
                            {
                                g_act.ArrSourceImage[g_act.ISelectCam]?.Dispose();
                            }
                            else
                            {
                                g_act.IsSnapOver = true;
                            }
                            //if (!Auto_Flag.SystemBusy) _HalconUtil.DisplayCross(_HalconWndID, _ImgOperaters[g_act.ISelectCam].DZoomWndFactor, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_RECORD, "MainUpCamera" + ex.Message, "Halcon");
            }

        }
        private void OnDownCamMsgEventHandler_HR(object sender, MsgEvent e)
        {
            try
            {
                if (e.MsgType == MsgEvent.MSG_IMGPAINT)
                {
                    HRCameraUtil cam = (HRCameraUtil)sender;
                    if (cam.SelectedWnd == GlobConstData.SELECT_MAIN_WND)
                    {
                        if (cam.ICameraID == g_act.ISelectCam)
                        {
                            if (cam.ICameraID == g_act.ISelectCam)
                            {
                                HObject tmp_img1;
                                if (cam.ICameraID == GlobConstData.ST_CCDDOWN)
                                {
                                    HOperatorSet.GenImage1(out tmp_img1, "byte", GlobConstData.IMG130_WIDTH, GlobConstData.IMG130_HEIGHT, (HTuple)g_act.ArrHRCameraUtils[g_act.ISelectCam].PtrBufferRaw);
                                    HOperatorSet.MirrorImage(tmp_img1, out g_act.ArrSourceImage[g_act.ISelectCam], "row");
                                    tmp_img1.Dispose();
                                    if (Auto_Flag.AutoRunBusy && Config.CCDModel == 1 && Run.ARP_Step >= 2)
                                    {
                                        while (true)
                                        {
                                            if (Axis.Pen[UserTask.PIC_PenN].ExistIC)
                                            {
                                                break;
                                            }
                                            UserTask.PIC_PenN += Auto_Flag.Ascending_ICPos ? 1 : -1;
                                        }
                                        int buffer = UserTask.PIC_PenN;
                                        g_act.ArrSourceImageBuffer[buffer] = g_act.ArrSourceImage[g_act.ISelectCam];
                                        Thread CCDTd = new Thread(() => g_act.CCDProccess_Fast(buffer, Axis.trapPrm_C[buffer].getPos));
                                        CCDTd.IsBackground = true;
                                        CCDTd.Start();
                                        UserTask.PIC_PenN += Auto_Flag.Ascending_ICPos ? 1 : -1;
                                    }

                                }
                                _HalconUtil.DispImage(_ImgOperaters[g_act.ISelectCam], g_act.ArrWndID[g_act.ISelectWnd], g_act.ArrSourceImage[g_act.ISelectCam], false, ref _IsHaveImg[g_act.ISelectCam]);
                                if (g_act.IsCamSnapMode != true)
                                {
                                    g_act.ArrSourceImage[g_act.ISelectCam]?.Dispose();
                                }
                                else
                                {
                                    g_act.IsSnapOver = true;
                                }
                                if (!Auto_Flag.SystemBusy) _HalconUtil.DisplayCross(_HalconWndID, _ImgOperaters[g_act.ISelectCam].DZoomWndFactor, false);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //g_act.GenLogMessage(GlobConstData.ST_LOG_RECORD, "主界面下相机采图" + ex.Message, "Halcon");
            }
        }
        //创建Halcon窗口
        private void __CreateHalconWnd()
        {
            HWindow wnd;
            wnd = hWindowControl1.HalconWindow;
            _HalconUtil.CreateImgDispRect(wnd, hWindowControl1.Width, hWindowControl1.Height, out _HalconWndID);
            g_act.ArrWndID[GlobConstData.SELECT_MAIN_WND] = _HalconWndID;
        }
        /// <summary>
        ///  初始化图像操作窗口
        /// </summary>
        /// <param name="openCamera">图像操作类</param>
        private void __InitOperateWnd(int openCamera)
        {
            if (_ImgOperaters[openCamera] == null) _ImgOperaters[openCamera] = new ImgOperater();
            this._ImgOperaters[openCamera].Initial(this, _HalconWndID);
            this._ImgOperaters[openCamera].IsZoomImg = true;
        }
        /// <summary>
        ///  初始化相机
        /// </summary>
        private void __InitCamera()
        {
            if (Config.CameraType == GlobConstData.Camera_DH)
            {
                g_act.InitCCD();
                g_act.ArrDHCameraUtils[GlobConstData.ST_CCDUP].MsgEventHandler += OnUpCamMsgEventHandler;
                g_act.ArrDHCameraUtils[GlobConstData.ST_CCDDOWN].MsgEventHandler += OnDownCamMsgEventHandler;

                this.__DownCameraStartSnap();
            }
            else
            {
                g_act.InitCCD();
                g_act.ArrHRCameraUtils[GlobConstData.ST_CCDUP].MsgEventHandler += OnUpCamMsgEventHandler_HR;
                g_act.ArrHRCameraUtils[GlobConstData.ST_CCDDOWN].MsgEventHandler += OnDownCamMsgEventHandler_HR;
                this.__DownCameraStartSnap();
            }
            
        } 
        public void __DownCameraStartSnap()
        {
            g_act.WaitDoEvent(100);
            if (Config.CameraType == GlobConstData.Camera_DH)
            {
                if (g_act.ArrDHCameraUtils[GlobConstData.ST_CCDDOWN].IsInit)
                {
                    this.upCamBtn.BaseColor = Color.Silver;
                    this.downCamBtn.BaseColor = Color.DimGray;
                    g_act.CCDCap(GlobConstData.ST_CCDDOWN);
                }
            }
            else
            {
                if (g_act.ArrHRCameraUtils[GlobConstData.ST_CCDDOWN].IsInit)
                {
                    this.upCamBtn.BaseColor = Color.Silver;
                    this.downCamBtn.BaseColor = Color.DimGray;
                    g_act.CCDCap(GlobConstData.ST_CCDDOWN);
                }
            }
            
        }
        private void __UpCameraStarSnap()
        {
            g_act.WaitDoEvent(100);
            if (Config.CameraType == GlobConstData.Camera_DH)
            {
                if (g_act.ArrDHCameraUtils[GlobConstData.ST_CCDUP].IsInit)
                {
                    this.downCamBtn.BaseColor = Color.Silver;
                    this.upCamBtn.BaseColor = Color.DimGray;
                    g_act.CCDCap(GlobConstData.ST_CCDUP);
                }
            }
            else
            {
                if (g_act.ArrHRCameraUtils[GlobConstData.ST_CCDUP].IsInit)
                {
                    this.downCamBtn.BaseColor = Color.Silver;
                    this.upCamBtn.BaseColor = Color.DimGray;
                    g_act.CCDCap(GlobConstData.ST_CCDUP);
                }
            }
            
        }

        /// <summary>
        /// 初始化运动控制卡
        /// </summary>
        public void InitMotionCard()
        {
            if (Config.CardType == GlobConstData.MotionCard_GTS)
            {
                InitCard_GTS();
            }
            else
            {
                InitCard_LH();
            }

            MainLogic = new Thread(__MainLogic);
            MainLogic.IsBackground = true;
            MainLogic.Start();
        }

        /// <summary>
        /// 初始化固高运动卡
        /// </summary>
        public void InitCard_GTS()
        {
            try
            {
                short[] rtn = new short[UserConfig.CardC];
                for (int i = 0; i < UserConfig.CardC; i++)
                {
                    rtn[i] = Gts.GT_Open(PLC1.card[i].cardNum, 0, 1); //打开卡
                    if (rtn[i] == 0)
                    {
                        ParameSetting_GTS(i); //板卡初始化设定
                        if (i == 0 && UserConfig.GLinkC > 0)
                        {
                            Gts.GT_OpenExtMdl(PLC1.card[i].cardNum, "gts.dll");//打开IO板
                            Gts.GT_LoadExtConfig(PLC1.card[i].cardNum, "ExtMdl.cfg");//下载扩展IO配置文件
                            for (short j = 0; j < UserConfig.GLinkC; j++)
                            {
                                Gts.GT_SetExtIoValue(PLC1.card[i].cardNum, j, 0xFFFF);//复位扩展输出IO
                            }
                        }
                        this._LoadBar.ShowProgressPos("运动控制卡[" + (i + 1) + "]成功打开", 70);
                    }
                    else
                    {
                        this._LoadBar.ShowProgressPos("运动控制卡[" + (i + 1) + "]打开失败", 70);
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "运动控制卡[" + (i + 1) + "]打开失败", "Error");
                    }
                }
                
                m0 = true; //板卡加载成功标志
                for (int i = 0; i < UserConfig.CardC; i++)
                {
                    if (rtn[i] != 0)
                    {
                        m0 = false;
                        break;
                    }
                }
                g_act.SetDac(12, UserTask.MPa);
            }
            catch (Exception ex)
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, ex.Message, "Error");
            }
        }

        /// <summary>
        /// 初始化唯精运动卡
        /// </summary>
        public void InitCard_LH()
        {
            try
            {
                short[] rtn = new short[UserConfig.CardC];
                for (int i = 0; i < UserConfig.CardC; i++)
                {
                    rtn[i] = lhmtc.LH_Open(PLC1.card[i].cardNum, 0, 1); //打开卡
                    if (rtn[i] == 0)
                    {
                        ParameSetting_LH(i); //板卡初始化设定
                        if (i == 0 && UserConfig.GLinkC > 0)
                        {
                            lhmtc.LH_SetExtendCardCount((short)UserConfig.GLinkC, PLC1.card[i].cardNum);//设置扩展IO数量
                            for (short j = 0; j < UserConfig.GLinkC; j++)
                            {
                                lhmtc.LH_SetExtendDo(j, 0xFFFF, PLC1.card[i].cardNum);//复位扩展输出IO
                            }
                        }
                        this._LoadBar.ShowProgressPos("运动控制卡[" + (i + 1) + "]成功打开", 70);
                    }
                    else
                    {
                        this._LoadBar.ShowProgressPos("运动控制卡[" + (i + 1) + "]打开失败", 70);
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "运动控制卡[" + (i + 1) + "]打开失败", "Error");
                    }
                }

                m0 = true; //板卡加载成功标志
                for (int i = 0; i < UserConfig.CardC; i++)
                {
                    if (rtn[i] != 0)
                    {
                        m0 = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, ex.Message, "Error");
            }
        }

        /// <summary>
        /// 固高运动卡参数设置
        /// </summary>
        private void ParameSetting_GTS(int index)
        {
            string str = "GTS" + PLC1.card[index].axisCount +  "00-KINCOTO.cfg";
            if (PLC1.card[index].axisCount == 8 && UserTask.PenType == 1)
            {
                str = "GTS" + PLC1.card[index].axisCount + "00-KINCOTO2.cfg";
            }
            Gts.GT_Reset(PLC1.card[index].cardNum);//复位卡  
            Gts.GT_LoadConfig(PLC1.card[index].cardNum, str);     //下载配置文件
            Gts.GT_ClrSts(PLC1.card[index].cardNum, 1, PLC1.card[index].axisCount);    //清除各轴报警和限位
            Gts.GT_ZeroPos(PLC1.card[index].cardNum, 1, PLC1.card[index].axisCount);   //位置清零
            for (short i = 1; i < PLC1.card[index].axisCount + 1; i++)
            {
                Gts.GT_LmtsOff(PLC1.card[index].cardNum, i, 1);//负限位无效
                Gts.GT_AxisOn(PLC1.card[index].cardNum, i);//使能伺服器
            }
        }

        /// <summary>
        /// 唯精运动卡参数设置
        /// </summary>
        private void ParameSetting_LH(int index)
        {
            string str = "LH" + PLC1.card[index].axisCount + "00-KINCOTO.cfg";
            if (PLC1.card[index].axisCount == 8 && UserTask.PenType == 1)
            {
                str = "LH" + PLC1.card[index].axisCount + "00-KINCOTO2.cfg";
            }
            lhmtc.LH_Reset(PLC1.card[index].cardNum);//复位卡  
            lhmtc.LH_LoadConfig(str, PLC1.card[index].cardNum);     //下载配置文件
            lhmtc.LH_ClrSts(1, PLC1.card[index].axisCount, PLC1.card[index].cardNum);    //清除各轴报警和限位
            lhmtc.LH_ZeroPos(1, PLC1.card[index].axisCount, PLC1.card[index].cardNum);   //位置清零
            for (short i = 1; i < PLC1.card[index].axisCount + 1; i++)
            {
                lhmtc.LH_LmtsOff(i, 1, PLC1.card[index].cardNum);//负限位无效
                lhmtc.LH_AxisOn(i, PLC1.card[index].cardNum);//使能伺服器
            }
        }

        private void 修改密码ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            _UserModifierWnd.Show();
        }
        private void 删除用户ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _UserInfoWnd.Show();
        }
        private void 注册用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _UserRegisterWnd.Show();
        }
        private void 切换用户ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _UserChangeWnd.Show();
            _UserChangeWnd.__InitUI();
        }
        private void 普通IOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _GPIOWnd.Show();
            _GPIOWnd.InitUI();
        }
        private void 专用IOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _AxisIOWnd.Show();
            _AxisIOWnd.InitUI();
        }
        private void BAR_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Mes.IsRun)
            {
                MessageBox.Show("MES系统批量生产中，请先取消批量生产", "温馨提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
                return;
            }
            if (MessageBox.Show(MultiLanguage.GetString("是否关闭软件"), "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                try
                {
                    g_act.IsSoftExit = true;
                    timerShowWnd.Stop();
                    timerDog.Stop();
                    //退出超级狗
                    dog?.Logout();
                    dog?.Dispose();

                    if (startup.RunSystem != null)
                    {
                        startup.RunSystem.Abort();
                        startup.RunSystem.Join();
                    }
                    MainLogic.Abort();
                    MainLogic.Join();
                    e.Cancel = false;
                    g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "关闭软件", "Flow");
                }
                catch (Exception)
                {

                }
            }
            else
            {
                e.Cancel = true;
            }
        }
        private void TimerDog_Tick(object sender, EventArgs e)
        {
            CheckDogTime();
        }

        private void ToolStripMenuAxisCtl_Click(object sender, EventArgs e)
        {
            _AxisParWnd.Show();
            _AxisParWnd.__InitWndUI();
        }
        private void __StatRealHandleFlow()
        {

            //while (!g_act.IsSoftExit)
            {
                if ((DateTime.Now - StTime2).TotalMilliseconds > 50)
                {
                    g_config.WriteStaticVal();
                    Button_Handle();

                    //线程里访问UI控件
                    this.BeginInvoke(new MethodInvoker(delegate
                    {
                        
                        float temp;
                        //把轴当前置更新到UI上                       
                        _AxisCtlWnd.xLab.Text = String.Format("{0:f2}", Axis.trapPrm_X.getPos);
                        _AxisCtlWnd.yLab.Text = String.Format("{0:f2}", Axis.trapPrm_Y.getPos);

                        _ConfigInitWnd.xLab.Text = String.Format("{0:f2}", Axis.trapPrm_X.getPos);
                        _ConfigInitWnd.yLab.Text = String.Format("{0:f2}", Axis.trapPrm_Y.getPos);

                        _ConfigInitWnd.pnlAutoRevisePos.uLab.Text = String.Format("{0:f2}", Config.ZoomLens == 1 ? ZoomLens.NowPos : Axis.trapPrm_U.getPos);
                        _ConfigInitWnd.pnlCMKPnl.Display();

                        xBLab.Text = String.Format("{0:f2}", Axis.trapPrm_X.getEncPos);
                        yBLab.Text = String.Format("{0:f2}", Axis.trapPrm_Y.getEncPos);

                        xLab.Text = String.Format("{0:f2}", Axis.trapPrm_X.getPos);
                        yLab.Text = String.Format("{0:f2}", Axis.trapPrm_Y.getPos);

                        for (int i = 0; i < UserConfig.VacuumPenC; i++)
                        {
                            string ctlName;
                            Control[] tagAry;
                            ctlName = "z" + (i + 1) + "Lab";
                            tagAry = _AxisCtlWnd.Controls.Find(ctlName, true);
                            if (tagAry.Length != 0)
                            {
                                (tagAry.First() as Label).Text = String.Format("{0:f2}", Axis.trapPrm_Z[i].getPos);
                            }
                            tagAry = _ConfigInitWnd.Controls.Find(ctlName, true);
                            if (tagAry.Length != 0)
                            {
                                (tagAry.First() as Label).Text = String.Format("{0:f2}", Axis.trapPrm_Z[i].getPos);
                            }
                            tagAry = Controls.Find(ctlName, true);
                            if (tagAry.Length != 0)
                            {
                                (tagAry.First() as Label).Text = String.Format("{0:f2}", Axis.trapPrm_Z[i].getPos);
                            }

                            ctlName = "C" + (i + 1) + "Lab";
                            tagAry = _AxisCtlWnd.Controls.Find(ctlName, true);
                            if (tagAry.Length != 0)
                            {
                                (tagAry.First() as Label).Text = String.Format("{0:f2}", Axis.trapPrm_C[i].getPos);
                            }
                            tagAry = _ConfigInitWnd.Controls.Find(ctlName, true);
                            if (tagAry.Length != 0)
                            {
                                (tagAry.First() as Label).Text = String.Format("{0:f2}", Axis.trapPrm_C[i].getPos);
                            }
                        }
                        if (UserTask.OKAllC + UserTask.NGAllC > 0)
                        {
                            temp = (float)UserTask.OKAllC / (UserTask.OKAllC + UserTask.NGAllC);
                        }
                        else
                        {
                            temp = 0;
                        }
                        LabYield.Text = String.Format("{0:f2}",temp * 100)+" %";
                        LabUPH.Text = Convert.ToString(Efficiency.value);
                        LabBrand.Text = Mes.Brand.ToString();
                        LabDevice.Text = Mes.Device.ToString();
                        LabChecksum.Text = Mes.Checksum_File.ToString();
                        LabChecksum_Chip.Text = Mes.Checksum_Chip.ToString();
                        LabTargetC.Text = UserTask.TargetC.ToString() + " pcs";
                        LabStartTime.Text = UserTask.StartTime.Year == 1 ?"---" :UserTask.StartTime.ToString("MM/dd HH:mm");
                        LabEndTime.Text = UserTask.EndTime.Year == 1 ? "---" : UserTask.EndTime.ToString("MM/dd HH:mm");
                        LabAlarmCount.Text = UserTask.AlarmCount.ToString() + " times";
                        LabPauseTime.Text = String.Format("{0:00}:{1:00}:{2:00}", Run.PauseStopWatch.Elapsed.Hours, 
                            Run.PauseStopWatch.Elapsed.Minutes, Run.PauseStopWatch.Elapsed.Seconds);
                        LabTIC_DoneC.Text = UserTask.TIC_DoneC.ToString() + " pcs";
                        LabOKDoneC.Text = UserTask.OKDoneC.ToString() + " pcs";
                        LabRejectRatio.Text = String.Format("{0:f2}", 100 - temp * 100) + " %";
                        if ((DateTime.Now - StTime4).Minutes > 1)
                        {
                            if (Run.RunStopWatch.ElapsedMilliseconds + Run.PauseStopWatch.ElapsedMilliseconds > 0)
                            {
                                temp = (float)(Run.RunStopWatch.ElapsedMilliseconds - Run.PauseStopWatch.ElapsedMilliseconds) / Run.RunStopWatch.ElapsedMilliseconds;
                            }
                            else
                            {
                                temp = 0;
                            }
                            LabRunRatio.Text = String.Format("{0:f2}", temp * 100) + " %";
                            StTime4 = DateTime.Now;
                        }
                        
                    }));

                    StTime2 = DateTime.Now;
                }
                if ((DateTime.Now - StTime3).TotalMilliseconds > 75)
                {
                    if (Auto_Flag.SystemBusy)
                    {
                        if (Auto_Flag.AutoRunBusy)
                        {
                            this.toolStripBtn4.Enabled = false;
                            this.toolStripBtn13.Enabled = false;
                            this.toolStripBtn14.Enabled = false;
                            toolStripMenuItem7.Enabled = false;
                        }
                    }
                    else
                    {
                        if (!Auto_Flag.AutoRunBusy)
                        {
                            this.toolStripBtn4.Enabled = StaticInfo.TDLevel == 3 ? false : true;
                            this.toolStripBtn13.Enabled = true;
                            this.toolStripBtn14.Enabled = true;
                            toolStripMenuItem7.Enabled = true;
                        }
                    }
                    StTime3 = DateTime.Now;
                }
            }
        }
        #region----------------系统状态显示-------------------
        private void SystemStateDisplay()
        {
            //油墨管理状态
             InksDisplay();

            //温度状态
            if (Brede_Status.Tmp_Close || !Auto_Flag.Brede_LayIC)
            {
                if (label3.Text != Brede_Status.strTmp_Close)
                {
                    label3.BackColor = Color.White;
                    label3.ForeColor = Color.Goldenrod;
                    label3.Text = Brede_Status.strTmp_Close;
                }
            }
            else if (Brede_Status.Tmp_HeatingUp)
            {
                if (label3.Text != Brede_Status.strTmp_HeatingUp)
                {
                    label3.BackColor = Color.Red;
                    label3.ForeColor = Color.DarkRed;
                    label3.Text = Brede_Status.strTmp_HeatingUp;
                }
            }
            else if (Brede_Status.Tmp_OK)
            {
                if (label3.Text != Brede_Status.strTmp_OK)
                {
                    label3.BackColor = Color.Lime;
                    label3.ForeColor = Color.Green;
                    label3.Text = Brede_Status.strTmp_OK;
                }
            }

            if (AutoTray.SystemInitDone)
            {
                if (label2.Text != AutoTray.strHomeDone)
                {
                    label2.BackColor = Color.Lime;
                    label2.ForeColor = Color.Green;
                    label2.Text = AutoTray.strHomeDone;
                }
            }
            else
            {
                if (label2.Text != AutoTray.strHomeUnDone)
                {
                    label2.BackColor = Color.Red;
                    label2.ForeColor = Color.DarkRed;
                    label2.Text = AutoTray.strHomeUnDone;
                }
            }

            //暂停状态
            if (Auto_Flag.Pause)
            {
                if (this.skinButton2.BaseColor != Color.Lime)
                {
                    this.skinButton2.BaseColor = Color.Lime;
                }
            }
            else
            {
                if (this.skinButton2.BaseColor != SystemColors.ControlLight)
                {
                    this.skinButton2.BaseColor = SystemColors.ControlLight;
                }
            }

            //手动收尾状态
            if (Auto_Flag.ManualEnd)
            {
                if (this.skinButton4.BaseColor != Color.Lime)
                {
                    this.skinButton4.BaseColor = Color.Lime;
                }
            }
            else
            {
                if (this.skinButton4.BaseColor != SystemColors.ControlLight)
                {
                    this.skinButton4.BaseColor = SystemColors.ControlLight;
                }
            }

            if (Auto_Flag.AutoRunBusy || Auto_Flag.HomeBusy || Auto_Flag.GOBusy || Auto_Flag.LearnBusy)
            {
                Auto_Flag.SystemBusy = true;
            }
            else
            {
                Auto_Flag.SystemBusy = false;
            }

            //编带通讯状态
            if (!Auto_Flag.Brede_LayIC)
            {
                Auto_Flag.BredeOnline = false;
            }

            //自动盘通讯状态
            if (!(Auto_Flag.AutoTray_LayIC || Auto_Flag.AutoTray_TakeIC))
            {
                Auto_Flag.AutoTrayOnline = false;
            }

            //变焦镜头
            if (Config.ZoomLens != 1) 
            {
                Axis.ZoomLens_S.ReadStatus_Online = false;
            }

            //警报暂停状态
            State_Monitor(aLarmPauseLed, Auto_Flag.ALarmPause);
            //系统忙状态
            State_Monitor(systemBusyLed, Auto_Flag.SystemBusy);
            //运行忙状态
            State_Monitor(runBusyLed, Auto_Flag.AutoRunBusy);
            //归零忙状态
            State_Monitor(homeBusyLed, Auto_Flag.HomeBusy);
            //启动确认状态
            State_Monitor(checkInitOKLed, Auto_Flag.CheckInitOK);
            //原点确认状态
            State_Monitor(homeOKLed, Startup.Condition[0]);
            //编带通讯状态
            State_Monitor(BredeOnlineLed, Auto_Flag.BredeOnline);
            //自动盘通讯状态
            State_Monitor(AutoTrayOnlineLed, Auto_Flag.AutoTrayOnline);
            //烧录通讯状态
            State_Monitor(BurnOnlineLed, Auto_Flag.BurnOnline);
            //烧录通讯状态
            State_Monitor(ZLOnlineLed, Axis.ZoomLens_S.ReadStatus_Online);

            In_Output.pumpPower.M = In_Output.pumpSwitch.M;

            if (In_Output.buzzer.M)//蜂鸣器
            {
                if (!In_Output.buzzer.RM)
                {
                    In_Output.buzzer.RM = true;
                    AutoTimer.BuzzerDuration = UserTimer.GetSysTime() + AutoTiming.BuzzerDuration;
                }
            }
            else
            {
                In_Output.buzzer.RM = false;
            }

            if (In_Output.buzzer.M && UserTimer.GetSysTime() > AutoTimer.BuzzerDuration)
            {
                In_Output.buzzer.M = false;
            }

            UserTimer.Blink();
            if (Auto_Flag.AutoRunBusy && !In_Output.Gate_Sig.M && !Auto_Flag.DebugMode)
            {
                if (!Auto_Flag.Safety_Gate)
                {
                    Auto_Flag.Safety_Gate = true;
                    g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "安全门未关闭", "Flow");
                }
                Auto_Flag.Pause = true;
                axis.AxisStopAll();
            }
            else
            {
                if (Auto_Flag.Safety_Gate)
                {
                    //Auto_Flag.Pause = false;
                    if (Auto_Flag.AutoRunBusy)
                    {
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "安全门已关闭", "Flow");
                    }
                }
                Auto_Flag.Safety_Gate = false;
            }


            if ((Auto_Flag.CheckInitOK && !Auto_Flag.SystemBusy && UserTimer.blink) ||
               (!Auto_Flag.ALarm && !Auto_Flag.Safety_Gate && Auto_Flag.AutoRunBusy))
            {
                In_Output.greenLight.M = true;
            }
            else
            {
                In_Output.greenLight.M = false;
            }

            if (!Auto_Flag.AutoRunBusy && In_Output.EMG_Sig.M && !Auto_Flag.CheckInitOK && !Auto_Flag.Exception)
            {
                In_Output.yellowLight.M = true;
            }
            else
            {
                In_Output.yellowLight.M = false;
            }

            if (((Auto_Flag.ALarm || Auto_Flag.Safety_Gate) && Auto_Flag.AutoRunBusy && UserTimer.blink) ||
                 !In_Output.EMG_Sig.M || (Auto_Flag.Exception && UserTimer.blink))
            {
                In_Output.redLight.M = true;
            }
            else
            {
                In_Output.redLight.M = false;
            }
        }

        /// <summary>
        /// 油墨管理状态显示
        /// </summary>
        private void InksDisplay()
        {
            Inks.TimeSpan_AutoTray = DateTime.Now.Subtract(Inks.DateTime_AutoTray);
            Inks.TimeSpan_Braid = DateTime.Now.Subtract(Inks.DateTime_Braid);

            //自动盘
            if (Inks.Enabled_AutoTray)
            {
                if (!_InksManageWnd.ChkAutoTrayEnabled.Checked)
                {
                    _InksManageWnd.ChkAutoTrayEnabled.Checked = true;
                }
                
                if (!_InksManageWnd.PnlAutoTray.Visible)
                {
                    _InksManageWnd.PnlAutoTray.Visible = true;
                }
            }
            else
            {
                _InksManageWnd.ChkAutoTrayEnabled.Checked = false;
                _InksManageWnd.PnlAutoTray.Visible = false;
            }

            
            //编带
            if (Inks.Enabled_Braid)
            {
                if (!_InksManageWnd.ChkBraidEnabled.Checked)
                {
                    _InksManageWnd.ChkBraidEnabled.Checked = true;
                }
                if (!_InksManageWnd.PnlBraid.Visible)
                {
                    _InksManageWnd.PnlBraid.Visible = true;
                }
                
                if (Inks.TimeSpan_Braid.TotalMinutes >= Inks.TimeCount_Braid)//编带油墨告警
                {
                    _InksManageWnd.LblTest.Text = "油墨警告";
                }
                else
                {
                    _InksManageWnd.LblTest.Text = "油墨安全";
                }
                _InksManageWnd.TxtTimeBraid.Text = string.Format("{0:d} 时 {1:d} 分 {2:d} 秒", Inks.TimeSpan_Braid.Hours,
                                                        Inks.TimeSpan_Braid.Minutes, Inks.TimeSpan_Braid.Seconds);
                _InksManageWnd.TxtDotBraid.Text = Inks.DotCount_Braid.ToString();
            }
            else
            {
                _InksManageWnd.ChkBraidEnabled.Checked = false;
                _InksManageWnd.PnlBraid.Visible = false;
            }
        }

        /// <summary>
        /// 状态监控
        /// </summary>
        /// <param name="lBLed">控件</param>
        /// <param name="IsBright">监控值</param>
        public void State_Monitor(LBLed lBLed, bool IsBright)
        {
            if (IsBright)
            {
                if (lBLed.State != LBLed.LedState.On)
                {
                    lBLed.State = LBLed.LedState.On;
                }
            }
            else
            {
                if (lBLed.State != LBLed.LedState.Off)
                {
                    lBLed.State = LBLed.LedState.Off;
                }
            }
        }
        #endregion

        /// <summary>
        /// 主程序区域，写逻辑
        /// </summary>
        private void __MainLogic()
        {
            StTime2 = StTime3 = StTime4 = DateTime.Now;
            while (!g_act.IsSoftExit)
            {
                Thread.Sleep(5);
                if (m0 == true || UserConfig.IsProgrammer)
                {
                    if (!Auto_Flag.AutoRunBusy)
                    {
                        axis.GetCardMessage();
                    }
                    In_Output.EMG_Sig.M = In_Output.EMG_Sig1.M && In_Output.EMG_Sig2.M;

                    if (!m1)
                    {
                        In_Output.EMG_Sig.RM = In_Output.EMG_Sig.M;
                        In_Output.EMG_Sig.FM = !In_Output.EMG_Sig.M;
                        m1 = true;
                    }
                    axis.VacuumPenRotate_Handle();
                    axis.AxisHomeAll_Init();
                    teachAction.GOPosition_Handle();
                    teachAction.Learn_Handle();
                    teachAction.Detection_Handle();
                    teachAction.MeasuringHeight_Handle();
                    teachAction.ReviseSeatPos_Handle();
                    teachAction.RevisePos_Handle();
                    teachAction.CMK_Handle();
                }
                in_Output.OpenSocket_InPlaceDelay();
                in_Output.TubeExistIC_InPlaceDelay();
                in_Output.FeederDelayOff();
                in_Output.BredeDelayOff_CCD();
                brede.Handle();
                autoTray.Handle();
                zoomLens.Handle();
                //autoTube.Handle();
                download.Download_Program_Handle();
                startup.StartupConditionCheck();
                if (m1 == true)
                {
                    Reset.EmergencyStop();
                    trayState.TrayState_Handle();
                }
                __StatRealHandleFlow();
                
            }
        }

        private void ToolStripConfigInitBtn_Click(object sender, EventArgs e)
        {
            g_act.TriggerMode();
            _ConfigInitWnd.Show();
            _ConfigInitWnd.InitWnd();
            //this.__DownCameraStartSnap();
        }

        /// <summary>
        /// 设备参数窗口
        /// </summary>
        private void ToolStripDevParaBtn_Click(object sender, EventArgs e)
        {
            _DevParaWnd.Show();
            _DevParaWnd.__InitParaText();
        }

        private void SkinMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)   
        {

        }
        /// <summary>
        /// 功能开关窗口
        /// </summary>
        private void ToolStripMenuItem12_Click(object sender, EventArgs e)
        {
            _FuncSwitWnd.Show();
            _FuncSwitWnd.__InitWnd();
        }
        /// <summary>
        /// 图像示教窗口
        /// </summary>
        private void ToolStripMenuItem8_Click(object sender, EventArgs e)
        {
            _BurnParaWnd.Show();
            _BurnParaWnd.__InitWnd();
        }
        /// <summary>
        /// 产品管理窗口
        /// </summary>
        private void ToolStripMenuItem9_Click(object sender, EventArgs e)
        {
            _ProManageWnd.Show();
            _ProManageWnd.__InitWnd();
            
        }

        static void OpenToolTipWnd()
        {
            _ToolTipDlg.ShowDialog();
        }

        public static void ShowToolTipWnd(bool alarmflag, bool buzzerflag = true)
        {
            if(alarmflag)
            {
                Auto_Flag.ALarm = true;
                Auto_Flag.ALarmPause = true;
            }
            if (buzzerflag)
            {
                In_Output.buzzer.M = true;
            }
            if (!_ToolTipDlg.IsShow)
            {
                UserTask.AlarmCount++;
                _ToolTipDlg.IsShow = true;
                Auto_Flag.JumpBredeStep_Flag = false;
                Auto_Flag.JumpMainStep_Flag = false;
                //_ToolTipDlg.Show();
                Thread newThread = new Thread(OpenToolTipWnd);
                newThread.IsBackground = true;
                newThread.Start();
            }
            
        } 

        /// <summary>
        /// 显示急停、安全门弹窗
        /// </summary>
        public void __ShowWnd()
        {
            if (m0 && m1)
            {
                if ((DateTime.Now - StTime1).TotalMilliseconds > 200)
                {
                    //显示急停弹窗
                    if (!In_Output.EMG_Sig.M && !IsShowWarning)
                    {
                        IsShowWarning = true;
                        _WarningWnd.Show();
                    }
                    else if (In_Output.EMG_Sig.M && IsShowWarning)
                    {
                        IntPtr Hwnd = FindWindow(null, "急停");
                        if (Hwnd != IntPtr.Zero)
                        {
                            ShowWindow(Hwnd, 0);
                        }
                        IsShowWarning = false;
                    }

                    //显示安全门弹窗
                    if (Auto_Flag.Safety_Gate && !IsShowWarningDoor)
                    {
                        IsShowWarningDoor = true;
                        _WarningDoorWnd.Show();
                    }
                    else if (!Auto_Flag.Safety_Gate && IsShowWarningDoor)
                    {
                        //获取窗口句柄
                        IntPtr Hwnd = FindWindow(null, "安全门");
                        if (Hwnd != IntPtr.Zero)
                        {
                            ShowWindow(Hwnd, 0);//0为关闭窗口
                        }
                        IsShowWarningDoor = false;
                    }

                    if ((((Axis.axisSts_X.negLimit || Axis.axisSts_X.posLimit) && !Axis.axisSts_X.isHomeBusy) || Axis.axisSts_X.alarm ||
                    ((Axis.axisSts_Y.negLimit || Axis.axisSts_Y.posLimit) && !Axis.axisSts_Y.isHomeBusy) || Axis.axisSts_Y.alarm) && !Auto_Flag.HomeBusy)
                    {
                        if (Auto_Flag.AutoRunBusy)
                        {
                            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "X或Y轴异常,已退出自动运行", "Flow");
                            Auto_Flag.AutoRunBusy = false;
                            g_act.AutoRunShutoff();
                        }

                        if (Auto_Flag.GOBusy)
                        {
                            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "X或Y轴异常,已退出GO动作", "Flow");
                            Auto_Flag.GOBusy = false;
                        }

                        if (Auto_Flag.LearnBusy)
                        {
                            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "X或Y轴异常,已退出反学", "Flow");
                            Auto_Flag.LearnBusy = false;
                        }

                        if (Axis.axisSts_X.alarm && !IsWriteLimitStr[0])
                        {
                            IsWriteLimitStr[0] = true;
                            _warningLimitWnd.WriteStr("X轴驱动器异常");
                        }
                        if (Axis.axisSts_X.negLimit && !IsWriteLimitStr[1])
                        {
                            IsWriteLimitStr[1] = true;
                            _warningLimitWnd.WriteStr("X轴负极限异常");
                        }
                        if (Axis.axisSts_X.posLimit && !IsWriteLimitStr[2])
                        {
                            IsWriteLimitStr[2] = true;
                            _warningLimitWnd.WriteStr("X轴正极限异常");
                        }

                        if (Axis.axisSts_Y.alarm && !IsWriteLimitStr[3])
                        {
                            IsWriteLimitStr[3] = true;
                            _warningLimitWnd.WriteStr("Y轴驱动器异常");
                        }
                        if (Axis.axisSts_Y.negLimit && !IsWriteLimitStr[4])
                        {
                            IsWriteLimitStr[4] = true;
                            _warningLimitWnd.WriteStr("Y轴负极限异常");
                        }
                        if (Axis.axisSts_Y.posLimit && !IsWriteLimitStr[5])
                        {
                            IsWriteLimitStr[5] = true;
                            _warningLimitWnd.WriteStr("Y轴正极限异常");
                        }

                        if (!Auto_Flag.Exception)
                        {
                            Auto_Flag.Exception = true;
                            _warningLimitWnd.Show();
                        }
                    }
                    else
                    {
                        if (Auto_Flag.Exception)
                        {
                            IntPtr Hwnd = FindWindow(null, MultiLanguage.GetString("限位告警"));
                            if (Hwnd != IntPtr.Zero)
                            {
                                ShowWindow(Hwnd, 0);
                                _warningLimitWnd.InitWnd();
                            }
                            Auto_Flag.Exception = false;
                            for (int i = 0; i < 6; i++)
                            {
                                IsWriteLimitStr[i] = false;
                            }
                        }
                    }
                    StTime1 = DateTime.Now;
                }  
            }
        }

        /// <summary>
        /// 超级狗到期时间
        /// </summary>
        public void CheckDogTime()
        {
            DogStatus status;
            double now_date = 0;
            double exp_date = 0;
            string timeType = "yyyy-MM-dd HH:mm:ss";
            string dog_StartTime = "1970-01-01 00:00:00";//超级狗开始记秒的时间
            DateTime dog_NowTime = new DateTime();
            string info = null;
            string strLicense_type = null;


            status = dog.GetTime(ref dog_NowTime);//获取超级狗当前时间
            if (DogStatus.StatusOk != status)
            {
                LockAll();
                timerDog.Enabled = false;
                MessageBox.Show(MultiLanguage.GetString("检查使用权限失败"), "Warning：", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DateTime dogTime1 = DateTime.ParseExact(dog_NowTime.ToString(timeType), timeType, null);//根据时间字符串输出datetime对象
            DateTime dogTime2 = DateTime.ParseExact(dog_StartTime, timeType, null);//根据时间字符串输出datetime对象
            TimeSpan ts1 = new TimeSpan(dogTime1.Ticks);
            double c11 = ts1.TotalSeconds;//超级狗当前时间转换为总秒数
            TimeSpan ts2 = new TimeSpan(dogTime2.Ticks);
            double c12 = ts2.TotalSeconds;//超级狗开始记秒的时间转换为总秒数
            now_date = c11 - c12 + 8 * 60 * 60;//计算差值(国际标准时相差8小时)

            status = dog.GetSessionInfo(Dog.SessionInfo, ref info);//获取超级狗到期时间
            if (DogStatus.StatusOk != status)
            {
                LockAll();
                timerDog.Enabled = false;
                MessageBox.Show(MultiLanguage.GetString("检查使用权限失败"), "Warning：", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            XmlDocument xdoc = new XmlDocument();//实例化xml
            xdoc.LoadXml(info);//从指定的字符串加载 XML 文档
            XmlNodeList xDocList = xdoc.GetElementsByTagName("exp_date"); //取得节点名为exp_date的XmlNode集合
            XmlNodeList xlicense_type = xdoc.GetElementsByTagName("license"); //取得节点名为license_type的XmlNode集合
            foreach (XmlNode xDocNode in xlicense_type)
            {
                strLicense_type = xDocNode.InnerText;//返回的是col的文字内容
            }

            if (strLicense_type == "perpetual")//登录权限为永久
            {
                //timerDog.Stop();
                return;
            }

            foreach (XmlNode xDocNode in xDocList)
            {
                exp_date = Convert.ToDouble(xDocNode.InnerText) + 5 * 60;//返回的是col的文字内容
            }

            //将总秒数转换为datetime对象
            DateTime dt_max = DateTime.Parse(dog_StartTime).AddSeconds(exp_date);
            string str = dt_max.ToString();

            if (exp_date - now_date <= 0)
            {
                //权限不足，屏蔽按钮
                LockAll();
                timerDog.Enabled = false;
                MessageBox.Show(MultiLanguage.GetString("设备使用权限到期,请联系供应商!"), "Warning：", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (exp_date - now_date < 24 * 60 * 60 * 15 && !dialog)
            {
                //使用权限即将到期，提醒
                dialog = true;
                string strShow;
                if (MultiLanguage.IsEnglish())
                {
                    strShow = "The permission to use the device will expire in " + str + ",Please contact the supplier as soon as possible\r\nIs it no longer prompted?";
                }
                else
                {
                    strShow = "设备使用权限将于[" + str + "]到期，请尽快联系供应商！！！\r\n是否不再提示";
                }
                if (DialogResult.No == MessageBox.Show(strShow, "Information：",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                {
                    dialog = false;
                }
            }
        }

        /// <summary>
        /// 屏蔽所有的按钮
        /// </summary>
        public void LockAll()
        {
            skinMenuStrip1.Enabled = false;
            skinButton1.Enabled = false;
            skinButton2.Enabled = false;
            skinButton3.Enabled = false;
            skinButton4.Enabled = false;
            skinButton5.Enabled = false;
            skinButton6.Enabled = false;
        }
        /// <summary>
        /// 使能所有的按钮
        /// </summary>
        public void OpenAll()
        {
            skinMenuStrip1.Enabled = true;
            skinButton1.Enabled = true;
            skinButton2.Enabled = true;
            skinButton3.Enabled = true;
            skinButton4.Enabled = true;
            skinButton5.Enabled = true;
            skinButton6.Enabled = true;
        }
        /// <summary>
        /// 打印日志信息
        /// </summary>
        public void PrintLogInfo(String msg) 
        {
            if (this.textBox1.Lines.Length > 50)
                this.textBox1.Lines = this.textBox1.Lines.Skip(1).ToArray();
            this.textBox1.Text += Environment.NewLine;
            this.textBox1.AppendText(msg);
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            _ToolTipDlg.ShowDialog();
        }

        private void 运行计数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _StaticalWnd.Show();
            _StaticalWnd.__InitWndUI();
        }

        private void AutoTrayInitBtn_Click(object sender, EventArgs e)
        {
            AutoTray.Init_Program_Start();
        }
        private void BredeTCPower_Click(object sender, EventArgs e)
        {
            if(!Brede.TCPowerOn)
            {
                Brede.Send_Cmd(Brede.Cmd_TCPowerOpen);
                Brede.MODBUS_WriteBredeParameter();
            }
            else
            {
                Brede.Send_Cmd(Brede.Cmd_TCPowerClose);
            }
            Brede.TCPowerOn = !Brede.TCPowerOn;
        }
        
        private void PenEnable_Click(object sender, EventArgs e)
        {
            if (!Auto_Flag.AutoRunBusy)
            {
                int ind = Convert.ToInt32((sender as SkinCheckBox).Tag);

                Run.PenEnable[ind].Checked = !Run.PenEnable[ind].Checked;
                g_config.WritePenEnabled();
            }
        }
        private void 烧录座计数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _BurnSeatStaticWnd.Show();
            _BurnSeatStaticWnd.InitWndUI();
        }

        /// <summary>
        /// 上相机连续采集
        /// </summary>
        private void UpCamBtn_Click(object sender, EventArgs e)
        {
            __UpCameraStarSnap();
        }
        /// <summary>
        /// 下相机连续采集
        /// </summary>
        private void DownCamBtn_Click(object sender, EventArgs e)
        {
            __DownCameraStartSnap();
        }
        /// <summary>
        /// 相机切换触发采集模式
        /// </summary>
        private void SnapMenuBtn_Click(object sender, EventArgs e)
        {
            if (g_act.ISelectCam == GlobConstData.ST_CCDUP) g_act.CCDSnap(GlobConstData.ST_CCDUP);
            else g_act.CCDSnap(GlobConstData.ST_CCDDOWN);
        }
        /// <summary>
        /// 相机切换连续采集模式
        /// </summary>
        private void CapMenuBtn_Click(object sender, EventArgs e)
        {
            if (g_act.ISelectCam == GlobConstData.ST_CCDUP) g_act.CCDCap(GlobConstData.ST_CCDUP, true);
            else g_act.CCDCap(GlobConstData.ST_CCDDOWN, true);
        }
        /// <summary>
        /// 自动模式按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoModBtn_Click(object sender, EventArgs e)
        {
            singleModeLed.BackColor = Color.GhostWhite;
            autoModLed.BackColor = Color.Lime;
            Auto_Flag.DebugMode = false;
            Auto_Flag.Pause = false;
        }
        /// <summary>
        /// 单动动模式按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SingleModBtn_Click(object sender, EventArgs e)
        {
            autoModLed.BackColor = Color.GhostWhite;
            singleModeLed.BackColor = Color.Lime;
            Auto_Flag.DebugMode = true;
            Auto_Flag.Pause = false;
        }
        /// <summary>
        /// 原点按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SkinButton6_Click(object sender, EventArgs e)
        {
            if (!Auto_Flag.SystemBusy)
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "手动回原点", "Flow");
                Axis.Home_Start();
            }
        }

        /// <summary>
        /// 通讯检测
        /// </summary>
        /// <returns></returns>
        private bool OnlineCheck()
        {
            if (!Auto_Flag.BredeOnline && Auto_Flag.Brede_LayIC)
            {
                return false;
            }
            if (!Auto_Flag.AutoTrayOnline && (Auto_Flag.AutoTray_LayIC || Auto_Flag.AutoTray_TakeIC))
            {
                return false;
            }
            if (!Auto_Flag.TestMode && !Auto_Flag.BurnOnline && UserTask.ProgrammerType != GlobConstData.Programmer_AK && UserTask.ProgrammerType != GlobConstData.Programmer_RD)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 启动按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SkinButton17_Click(object sender, EventArgs e)
        {
            if (!UserConfig.IsProgrammer)
            {
                if (Mes.Type == GlobConstData.ST_MESTYPE_XC && !Mes.IsRun)
                {
                    MessageBox.Show("请先对接MES服务器再启动自动烧录", "温馨提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (Auto_Flag.SystemBusy)
                {
                    if (Auto_Flag.AutoRunBusy)
                    {
                        _ToolTipDlg.WriteToolTipStr("正在自动运行");
                    }
                    else if (Auto_Flag.HomeBusy)
                    {
                        _ToolTipDlg.WriteToolTipStr("正在执行回原点");
                    }
                    else if (Auto_Flag.GOBusy)
                    {
                        _ToolTipDlg.WriteToolTipStr("正在执行GO动作");
                    }
                    else if (Auto_Flag.LearnBusy)
                    {
                        _ToolTipDlg.WriteToolTipStr("正在执行模板示教动作");
                    }
                    ShowToolTipWnd(false);
                    return;
                }
                if (!OnlineCheck())
                {
                    if (!Auto_Flag.BredeOnline && Auto_Flag.Brede_LayIC)
                    {
                        _ToolTipDlg.WriteToolTipStr("编带通讯异常");
                    }
                    if (!Auto_Flag.AutoTrayOnline && Auto_Flag.AutoTray)
                    {
                        _ToolTipDlg.WriteToolTipStr("自动盘通讯异常");
                    }
                    if (!Auto_Flag.TestMode && !Auto_Flag.BurnOnline && UserTask.ProgrammerType != GlobConstData.Programmer_AK && UserTask.ProgrammerType != GlobConstData.Programmer_RD)
                    {
                        _ToolTipDlg.WriteToolTipStr("烧录器通讯异常");
                    }
                    ShowToolTipWnd(false);
                    return;
                }

                if (!Auto_Flag.CheckInitOK)
                {
                    if (!Startup.Condition[0])
                    {
                        _ToolTipDlg.WriteToolTipStr("未满足XYZ轴全部在原位");
                    }
                    if (!Startup.Condition[1])
                    {
                        _ToolTipDlg.WriteToolTipStr("未满足气阀全部复位");
                    }
                    if (!Startup.Condition[2])
                    {
                        _ToolTipDlg.WriteToolTipStr("未满足烧录座气缸检测全部在原位");
                    }
                    if (!Startup.Condition[3])
                    {
                        _ToolTipDlg.WriteToolTipStr("未选择烧写座");
                    }
                    if (!Startup.Condition[4])
                    {
                        _ToolTipDlg.WriteToolTipStr("未打开编带温控,或是未到达指定温度");
                    }
                    if (!Startup.Condition[5])
                    {
                        if ((AutoTray.StatusAlarmWord & AutoTray.Status_SystemRnunning) != 0)
                        {
                            _ToolTipDlg.WriteToolTipStr("自动盘正在忙");
                        }
                        else
                        {
                            if ((AutoTray.StatusAlarmWord & AutoTray.Status_SystemInitDone) == 0)
                            {
                                _ToolTipDlg.WriteToolTipStr("自动盘电机未在原位");
                            }
                            if ((AutoTray.StatusAlarmWord & AutoTray.Status_TrayFull) != 0)
                            {
                                _ToolTipDlg.WriteToolTipStr("自动盘已烧录区装满料盘");
                            }
                            if ((AutoTray.StatusAlarmWord & AutoTray.Status_OverplusTrayInit) == 0 && (AutoTray.StatusAlarmWord & AutoTray.Status_TrayLack) != 0)
                            {
                                _ToolTipDlg.WriteToolTipStr("自动盘未烧录区缺少料盘");
                            }
                        }
                    }
                    if (!Startup.Condition[6])
                    {
                        _ToolTipDlg.WriteToolTipStr("未放好料盘");
                    }
                    if (!Startup.Condition[7])
                    {
                        _ToolTipDlg.WriteToolTipStr("未更换指示灯亮的料盘");
                    }
                    if (!Startup.Condition[8])
                    {
                        _ToolTipDlg.WriteToolTipStr("未选择使用吸笔");
                    }
                    if (!Startup.Condition[9])
                    {
                        _ToolTipDlg.WriteToolTipStr("吸笔未满足同取同放,请选吸笔序号连续且吸笔个数为2或者4");
                    }
                    if (!Startup.Condition[10])
                    {
                        _ToolTipDlg.WriteToolTipStr("未满足变焦镜头在原位");
                    }

                    ShowToolTipWnd(false);
                    return;
                }
            }

            //检查工单是否已完成
            //if (g_act.ReadLotInfo())
            //{
            //    if ((Mes.Exit == 1 && Mes.OKDoneC >= Mes.Count) || (Mes.Exit == 2 && Mes.TIC_DoneC >= Mes.Count))
            //    {
            //        MessageBox.Show("工单：[" + Mes.LotSN + "]已完成，请添加新工单！！！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }
            //    UserTask.TargetC = Mes.Count;
            //    UserTask.TIC_DoneC = Mes.TIC_DoneC;
            //    UserTask.OKDoneC = Mes.OKDoneC;
            //    Auto_Flag.Production = true;
            //    Auto_Flag.ProductionOK = Mes.Exit == 1 ? true : false;
            //}
            DialogResult result;
            if (Auto_Flag.RemoteStart)//凤凰平台远程启动，记忆启动
            {
                Auto_Flag.RemoteStart = false;
                result = DialogResult.Yes;
            }
            else
            {
                if (MultiLanguage.IsEnglish())
                {
                    result = MessageBox.Show(" Memory priming or not\r\n\r\n【Yes】Priming of memory\r\n【No】Non memory priming", "Information：", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                }
                else
                {
                    result = MessageBox.Show(" 是否为记忆启动\r\n\r\n【是】：记忆启动\r\n【否】：非记忆启动", "Information：", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                }
                
            }
            
            //if (Config.ZoomLens && (DialogResult.Cancel != result))
            //{
            //    DialogResult result1 = MessageBox.Show(" 是否自动校准所有坐标\r\n\r\n【是】：校准\r\n【否】：跳过", "提示：", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            //    if (DialogResult.Yes == result1)
            //    {
            //        //this.upCamBtn.PerformClick();
            //        Auto_Flag.AutoRevisePos = true;
            //        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "开始自动校准所有坐标", "Flow");
            //    }
            //}
            Auto_Flag.BurnReady = false;
            g_act.TriggerMode();
            //自动运行开始，启动下相机采图
            this.downCamBtn.PerformClick();
            if (Config.CCDModel == 1)
            {
                Run.ARP_Step = 0;
                if (Config.CameraType == GlobConstData.Camera_DH)
                {
                    g_act.ArrDHCameraUtils[GlobConstData.ST_CCDDOWN].SetEnumValue("TriggerSource", "Line2");
                }
                else
                {
                    g_act.CCDCap(GlobConstData.ST_CCDDOWN, true);
                    g_act.WaitDoEvent(700);
                    g_act.CCDSnap(GlobConstData.ST_CCDDOWN);
                    g_act.ArrHRCameraUtils[GlobConstData.ST_CCDDOWN].SetLineMode();
                }
            }
            if (DialogResult.Yes == result)
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, this.Text, "Flow");
                if (Auto_Flag.Production)
                {
                    int temp;
                    temp = Auto_Flag.ProductionOK ? UserTask.OKDoneC : UserTask.TIC_DoneC;
                    startup.StartupInit(temp >= UserTask.TargetC);
                }
                else
                {
                    startup.StartupInit(false);
                }
            }
            else if (DialogResult.No == result)
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, this.Text, "Flow");
                startup.StartupInit(true);
            }

        }

        /// <summary>
        /// 编带参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripBtn11_Click(object sender, EventArgs e)
        {
            _BraidParaWnd.Show();
            _BraidParaWnd.__InitWnd();
        }

        private void timerShowWnd_Tick(object sender, EventArgs e)
        {
            trayState.LED();
            SystemStateDisplay();
            __ShowWnd();
            //Button_Handle();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _MesWnd.Show();
            _MesWnd.InitWndUI();
        }

        private void toolStripBtn3_Click(object sender, EventArgs e)
        {
            _StaticalWnd.Show();
            _StaticalWnd.__InitWndUI();
        }

        private void toolStripBtn14_Click(object sender, EventArgs e)
        {
            _AxisCtlWnd.Show();
        }

        private void toolStripBtn8_Click(object sender, EventArgs e)
        {
            _CylinderCtlWnd.Show();
            _CylinderCtlWnd.__InitWnd();
        }

        /// <summary>
        /// 继续按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SkinButton7_Click(object sender, EventArgs e)
        {
            if (Auto_Flag.ALarmPause || Auto_Flag.RunPause)
            {
                if (Auto_Flag.AutoRunBusy || Auto_Flag.LearnBusy)
                {
                    Auto_Flag.Next = true;
                }
                else if (UserTask.ProgrammerType == GlobConstData.Programmer_RD || UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY)
                {
                    for (int i = 0; i < UserConfig.ScketGroupC; i++)
                    {
                        if (Axis.Group[i].Down.Busy)
                        {
                            Auto_Flag.Next = true;
                            break;
                        }
                    }
                }
            }
            Auto_Flag.Pause = false;
            _StaticalWnd.__InitWndUI();
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "继续运行", "Flow");
        }
        /// <summary>
        /// 暂停按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SkinButton2_Click(object sender, EventArgs e)
        {
            if(!Auto_Flag.Safety_Gate)
            {
                Auto_Flag.Pause = !Auto_Flag.Pause;
                _StaticalWnd.__InitWndUI();
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "手动暂停运行", "Flow");
            }
        }

        /// <summary>
        /// 工程设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripBtn15_Click(object sender, EventArgs e)
        {
            _EngineerSettingWnd.Show();
        }

        /// <summary>
        /// 收尾按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SkinButton4_Click(object sender, EventArgs e)
        {
            if (!Auto_Flag.AutoRunBusy)
            {
                return;
            }
            _ClosureWnd.Show();
            _ClosureWnd.__InitUI();
        }
        /// <summary>
        /// 复位按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SkinButton3_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show(MultiLanguage.GetString("是否执行复位"), "Information：", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
            {
                Reset.ResetState(true);
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "复位成功", "Flow");
            }
        }

        /// <summary>
        /// 自动盘参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            _AutoTrayParametersWnd.Show();
            _AutoTrayParametersWnd.__InitWnd();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            _InksManageWnd.Show();
            _InksManageWnd.InitUI();
        }

        private void BAR_MouseMove(object sender, MouseEventArgs e)
        {
            if ((!Auto_Flag.AutoRunBusy || Auto_Flag.RunPause || Auto_Flag.ALarmPause) && trayState.DisplayRowAndCol(out string strMsg))
            {
                toolTip1.Show(strMsg, PlatePanel, PlatePanel.PointToClient(new Point(MousePosition.X + 20, MousePosition.Y + 20)), 2000);
            }
        }

        private void BAR_MouseDown(object sender, MouseEventArgs e)
        {
            trayState.RightMouseDown();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            _TicketsWnd.Show();
            _TicketsWnd._InitUI();
        }

        private void BAR_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ReleseDevices();
        }
        public void ReleseDevices()
        {
            if (Config.CameraType == GlobConstData.Camera_DH)
            {
                g_act.ArrDHCameraUtils[0]?.ReleaseDevices();
                g_act.ArrDHCameraUtils[1]?.ReleaseDevices();
            }
            else
            {
                g_act.ArrHRCameraUtils[0]?.ReleaseDevices();
                g_act.ArrHRCameraUtils[1]?.ReleaseDevices();
            }
            
        }

        /// <summary>
        /// 外部按钮处理函数
        /// </summary>
        private void Button_Handle()
        {
            if (UserConfig.IsProgrammer)
            {
                return;
            }
            //启动按钮报警复位
            if ((Auto_Flag.ALarmPause || Auto_Flag.RunPause) && In_Output.BtnStartI.M)
            {
                _ToolTipDlg.btnConfirm_Click(null,null);
                SkinButton7_Click(null, null);
            }
            //暂停按钮、启动按钮
            if ((!Auto_Flag.Pause && In_Output.BtnPauseI.M) || (Auto_Flag.Pause && In_Output.BtnStartI.M)) 
            {
                SkinButton2_Click(null, null);
            }
            In_Output.BtnPauseO.M = Auto_Flag.Pause == true ? true : false;

            btn.Button_Handle();
        }
        
    }
}
