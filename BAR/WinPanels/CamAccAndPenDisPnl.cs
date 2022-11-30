using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BAR.Windows;
using BAR.Commonlib;
using BAR.Commonlib.Utils;
using HalconDotNet;
using CCWin.SkinControl;
using System.Threading.Tasks;
using System.Threading;
using BAR.CommonLib_v1._0;

namespace BAR.ControlPanels
{
    public partial class CamAccAndPenDisPnl : UserControl
    {
        private SkinButton[] btnSavePen;
        private SkinButton[] btnGOPen;
        public RadioButton  SelectedRbtn;
        public int          SelectedInd;
        ConfigInitWnd       _ConInitWnd;
        Act                 g_act = Act.GetInstance();
        Config              g_config = Config.GetInstance();
        public Axis axis = Axis.GetInstance();
        HalconImgUtil       _HalconUtil = HalconImgUtil.GetInstance();
        public CamAccAndPenDisPnl()
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();
            _InitializeComponent();
            this.initModBtn.Enabled = false;
            this.findModBtn.Enabled = false;
            this.__InitUI();
            saveScannerPosBtn.Visible = GoScannerPosBtn.Visible = UserTask.ProgrammerType == GlobConstData.Programmer_YED;
            saveAltimeterPosBtn.Visible = GoAltimeterPosBtn.Visible = Config.Altimeter != 0;
            if (UserTask.ProgrammerType != GlobConstData.Programmer_YED)
            {
                saveAltimeterPosBtn.Location = saveScannerPosBtn.Location;
                GoAltimeterPosBtn.Location = GoScannerPosBtn.Location;
            }
        }

        private void _InitializeComponent()
        {
            int nW = 112, nH = 43;
            int left = 0;
            //吸笔位置保存、GO控件
            btnSavePen = new SkinButton[UserConfig.VacuumPenC];
            btnGOPen = new SkinButton[UserConfig.VacuumPenC];
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                left = i * 114;
                btnSavePen[i] = new SkinButton
                {
                    BackColor = Color.Transparent,
                    BaseColor = Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0))))),
                    BorderColor = Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0))))),
                    ControlState = CCWin.SkinClass.ControlState.Normal,
                    DownBack = null,
                    Font = new Font("楷体", 12F, FontStyle.Bold),
                    ForeColor = Color.White,
                    ImeMode = ImeMode.NoControl,
                    IsDrawGlass = false,
                    Location = new Point(164 + left, 28),
                    MouseBack = null,
                    Name = "btnSavePen" + i,
                    NormlBack = null,
                    Size = new Size(112, 43),
                    TabIndex = i,
                    Tag = i,
                    Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English? "【Save】Pen" + (i + 1) : "保存:吸笔" + (i + 1) + "拍照位置",
                    UseVisualStyleBackColor = false
                };
                btnSavePen[i].Click += new EventHandler(SavePenPhoLocBtn_Click);


                btnGOPen[i] = new SkinButton
                {
                    BackColor = Color.Transparent,
                    BaseColor = Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0))))),
                    BorderColor = Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0))))),
                    ControlState = CCWin.SkinClass.ControlState.Normal,
                    DownBack = null,
                    Font = new Font("楷体", 12F, FontStyle.Bold),
                    ForeColor = Color.White,
                    ImeMode = ImeMode.NoControl,
                    IsDrawGlass = false,
                    Location = new Point(164 + left, 77),
                    MouseBack = null,
                    Name = "btnGOPen" + i,
                    NormlBack = null,
                    Size = new Size(112, 43),
                    TabIndex = i,
                    Tag = i,
                    Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "【GO】Pen" + (i + 1) : "GO:吸笔" + (i + 1) + "拍照位置",
                    UseVisualStyleBackColor = false
                };
                btnGOPen[i].Click += new EventHandler(GoPenPhoLocBtn_Click);

                skinGroupBox4.Controls.Add(btnSavePen[i]);
                skinGroupBox4.Controls.Add(btnGOPen[i]);
            }
            calDisBtn.Location = new Point(278 + left, 28);
        }

        private void SavePenPhoLocBtn_Click(object sender, EventArgs e)
        {
            SkinButton btn = sender as SkinButton;
            int ind = Convert.ToInt32(btn.Tag);
            g_config.ArrPickPos[2 + ind].X = Axis.trapPrm_X.getPos;
            g_config.ArrPickPos[2 + ind].Y = Axis.trapPrm_Y.getPos;
            Axis.Pen[ind].LowCamera_X = Axis.trapPrm_X.getPos;
            Axis.Pen[ind].LowCamera_Y = Axis.trapPrm_Y.getPos;
            g_config.WriteMachineInfo();
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "保存吸笔[" + (ind + 1) + "]拍照位置成功", "Modify");
        }

        private void GoPenPhoLocBtn_Click(object sender, EventArgs e)
        {
            SkinButton btn = sender as SkinButton;
            int ind = Convert.ToInt32(btn.Tag);
            if (!Auto_Flag.SystemBusy)
            {
                TeachAction.GO_Start(Axis.Pen[ind].LowCamera_X, Axis.Pen[ind].LowCamera_Y);
            }
        }

        public CamAccAndPenDisPnl(ConfigInitWnd conInitWnd)
            : this()
        {
            this._ConInitWnd = conInitWnd;
        }
        private void __InitUI()
        {
            SelectedRbtn = this.initRbtn1;
        }
        private void CamAccAndPenDisPnl_Load(object sender, EventArgs e)
        {
            DisplayGetOffset_TopCameraToPen();
        }
        //示教精度模板
        private void InitICSeatBtn_Click(object sender, EventArgs e)
        {
            if (SelectedInd >= 6)
            {
                return;
            }
            EnableImageDlg(true);
            this.LenModelUp(_ConInitWnd.IIndexModel);
            if (SelectedInd == 5)
            {
                g_act.CCDSnap(GlobConstData.ST_CCDDOWN);
            }
            else
            {
                g_act.CCDSnap(GlobConstData.ST_CCDUP);
            }
            //================================
            g_config.ArrCCDPos[SelectedInd].X = Axis.trapPrm_X.getPos;
            g_config.ArrCCDPos[SelectedInd].Y = Axis.trapPrm_Y.getPos;
            g_config.WriteMachineInfo();
            //EnableImageDlg(false);
        }

        private void EnableImageDlg(bool flag)
        {
            initModBtn.Enabled = flag;
            findModBtn.Enabled = flag;
        }

        private void LenModelUp(int sel)
        {
            if (sel < 0)
            {
                _ConInitWnd.IIndexModel = GlobConstData.ST_MODELICSTATACC;
                initModBtn.Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Teach template" : "示教模板";
                findModBtn.Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Find" : "查找模板";
                initModBtn.Enabled = false;
                findModBtn.Enabled = false;
            }
            else if (sel == GlobConstData.ST_MODELICSTATACC)
            {
                _ConInitWnd.IIndexModel = GlobConstData.ST_MODELICSTATACC;
                initModBtn.Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Teach【Seat】" : "示教【IC座精度】";
                findModBtn.Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Find【Seat】" : "查找【IC座精度】";
                initModBtn.Enabled = true;
                findModBtn.Enabled = true;
            }
            else if (sel == GlobConstData.ST_ModelTrayACC)
            {
                _ConInitWnd.IIndexModel = GlobConstData.ST_ModelTrayACC;
                initModBtn.Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Teach【Tary】" : "示教【料盘精度】";
                findModBtn.Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Find【Tray】" : "查找【料盘精度】";
                initModBtn.Enabled = true;
                findModBtn.Enabled = true;
            }
            else if (sel == GlobConstData.ST_MODELPENACC)
            {
                _ConInitWnd.IIndexModel = GlobConstData.ST_MODELPENACC;
                initModBtn.Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Teach【Pen】" : "示教【吸头精度】";
                findModBtn.Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Find【Pen】" : "查找【吸头精度】";
                initModBtn.Enabled = true;
                findModBtn.Enabled = true;
            }
            else if (sel == GlobConstData.ST_ModelBredeInACC)
            {
                _ConInitWnd.IIndexModel = GlobConstData.ST_ModelBredeInACC;
                initModBtn.Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Teach【Feeder】" : "示教【飞达精度】";
                findModBtn.Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Find【Feeder】" : "查找【飞达精度】";
                initModBtn.Enabled = true;
                findModBtn.Enabled = true;
            }
            else if (sel == GlobConstData.ST_ModelBredeOutACC)
            {
                _ConInitWnd.IIndexModel = GlobConstData.ST_ModelBredeOutACC;
                initModBtn.Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Teach【Tape】" : "示教【编带精度】";
                findModBtn.Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Find【Tape】" : "查找【编带精度】";
                initModBtn.Enabled = true;
                findModBtn.Enabled = true;
            }
            else if (sel == GlobConstData.ST_ModelTubeInACC)
            {
                _ConInitWnd.IIndexModel = GlobConstData.ST_ModelTubeInACC;
                initModBtn.Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Teach【Tube】" : "示教【料管精度】";
                findModBtn.Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Find【Tube】" : "查找【料管精度】";
                initModBtn.Enabled = true;
                findModBtn.Enabled = true;
            }
        }
        private void InitRbtn_MouseUp(object sender, MouseEventArgs e)
        {
            RadioButton clickRbtn = sender as RadioButton;
            if (clickRbtn != SelectedRbtn && clickRbtn.Checked)
            {
                SelectedRbtn.Checked = false;
                SelectedRbtn = clickRbtn;
                SelectedInd = Convert.ToInt32(SelectedRbtn.Tag);
                CheckRbtn();

            }
        }

        /// <summary>
        /// 检查所选相机
        /// </summary>
        public void CheckRbtn()
        {
            if (initRbtn1.Checked)
            {
                EnableImageDlg(false);
                _ConInitWnd.upCamBtn.Checked = true;
                g_act.CCDCap(GlobConstData.ST_CCDUP);
                _ConInitWnd.IIndexModel = _ConInitWnd.ISelBoxInd = GlobConstData.ST_MODELICSTATACC;
                g_act.SendCmd(GlobConstData.ST_LIGHTUP, g_config.ArrLampBox[_ConInitWnd.ISelBoxInd].ILightUp);
            }
            else if (initRbtn2.Checked)
            {
                EnableImageDlg(false);
                _ConInitWnd.upCamBtn.Checked = true;
                g_act.CCDCap(GlobConstData.ST_CCDUP, true);
                _ConInitWnd.IIndexModel = _ConInitWnd.ISelBoxInd = GlobConstData.ST_ModelTrayACC;
                g_act.SendCmd(GlobConstData.ST_LIGHTUP, g_config.ArrLampBox[_ConInitWnd.ISelBoxInd].ILightUp);
            }
            else if (initRbtn3.Checked)
            {
                EnableImageDlg(false);
                _ConInitWnd.downCamBtn.Checked = true;
                g_act.CCDCap(GlobConstData.ST_CCDDOWN);
                _ConInitWnd.IIndexModel = _ConInitWnd.ISelBoxInd = GlobConstData.ST_MODELPENACC;
                g_act.SendCmd(GlobConstData.ST_LIGHTDOWN, g_config.ArrLampBox[_ConInitWnd.ISelBoxInd].ILightDown);
            }
            else if (initRbtn4.Checked)
            {
                EnableImageDlg(false);
                _ConInitWnd.upCamBtn.Checked = true;
                g_act.CCDCap(GlobConstData.ST_CCDUP, true);
                _ConInitWnd.IIndexModel = _ConInitWnd.ISelBoxInd = GlobConstData.ST_MODELICSTATACC;
                g_act.SendCmd(GlobConstData.ST_LIGHTUP, g_config.ArrLampBox[_ConInitWnd.ISelBoxInd].ILightUp);
            }
            else if (initRbtn5.Checked)
            {
                EnableImageDlg(false);
                _ConInitWnd.downCamBtn.Checked = true;
                g_act.CCDCap(GlobConstData.ST_CCDDOWN);
                _ConInitWnd.IIndexModel = _ConInitWnd.ISelBoxInd = GlobConstData.ST_MODELPENACC;
                g_act.SendCmd(GlobConstData.ST_LIGHTDOWN, g_config.ArrLampBox[_ConInitWnd.ISelBoxInd].ILightDown);
            }
            else if (SelectedRbtn == this.initRbtn6)
            {
                EnableImageDlg(false);
                _ConInitWnd.upCamBtn.Checked = true;
                g_act.CCDCap(GlobConstData.ST_CCDUP, true);
                _ConInitWnd.IIndexModel = _ConInitWnd.ISelBoxInd = GlobConstData.ST_ModelBredeInACC;
                g_act.SendCmd(GlobConstData.ST_LIGHTUP, g_config.ArrLampBox[_ConInitWnd.ISelBoxInd].ILightUp);
            }
            else if (SelectedRbtn == this.initRbtn7)
            {
                EnableImageDlg(false);
                _ConInitWnd.upCamBtn.Checked = true;
                g_act.CCDCap(GlobConstData.ST_CCDUP, true);
                _ConInitWnd.IIndexModel = _ConInitWnd.ISelBoxInd = GlobConstData.ST_ModelBredeOutACC;
                g_act.SendCmd(GlobConstData.ST_LIGHTUP, g_config.ArrLampBox[_ConInitWnd.ISelBoxInd].ILightUp);
            }
            else if (SelectedRbtn == this.initRbtn8)
            {
                EnableImageDlg(false);
                _ConInitWnd.upCamBtn.Checked = true;
                g_act.CCDCap(GlobConstData.ST_CCDUP, true);
                _ConInitWnd.IIndexModel = _ConInitWnd.ISelBoxInd = GlobConstData.ST_ModelTubeInACC;
                g_act.SendCmd(GlobConstData.ST_LIGHTUP, g_config.ArrLampBox[_ConInitWnd.ISelBoxInd].ILightUp);
            }
            _ConInitWnd.__SetBox(_ConInitWnd.ISelBoxInd);
            if (SelectedInd < 6)
            {
                if (MultiLanguage.DefaultLanguage == GlobConstData.ST_English)
                {
                    camFixTxt.Text = string.Format("AccuracyX：{0:f3}\r\nAccuracyY：{1:f3}", g_config.ArrCCDPrec[SelectedInd].X, g_config.ArrCCDPrec[SelectedInd].Y);
                }
                else
                {
                    camFixTxt.Text = string.Format("相机精度X：{0:f3}\r\n相机精度Y：{1:f3}", g_config.ArrCCDPrec[SelectedInd].X, g_config.ArrCCDPrec[SelectedInd].Y);
                }
                
            }
        }

        //示教模板
        private void InitModBtn_Click(object sender, EventArgs e)
        {
            EnableImageDlg(false);
            _ConInitWnd.InitImgModel();
            EnableImageDlg(true);
        }
        //查找模板
        private void FindModBtn_Click(object sender, EventArgs e)
        {
            _ConInitWnd.FindImgModel();
        }
        //计算间距
        private void CalDisBtn_Click(object sender, EventArgs e)
        {
            g_config.GetOffset_TopCameraToPen();
            DisplayGetOffset_TopCameraToPen();
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "计算间距成功", "Modify");
        }

        private void DisplayGetOffset_TopCameraToPen()
        {
            string str = null, strx, stry;
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                strx = string.Format("{0,8:f3}", Axis.Pen[i].Offset_TopCamera_X);
                stry = string.Format("{0,8:f3}", Axis.Pen[i].Offset_TopCamera_Y);
                str += "CCD - Z" + (i + 1) + ":  X =" + strx + "mm       Y =" + stry + "mm\r\n";
            }
            if (UserTask.ProgrammerType == GlobConstData.Programmer_YED)
            {
                strx = string.Format("{0,8:f3}", Axis.Scanner.Offset_TopCamera_X);
                stry = string.Format("{0,8:f3}", Axis.Scanner.Offset_TopCamera_Y);
                str += "CCD - S " + ":  X =" + strx + "mm       Y =" + stry + "mm\r\n";
            }
            if (Config.Altimeter != 0)
            {
                strx = string.Format("{0,8:f3}", Axis.Altimeter.Offset_TopCamera_X);
                stry = string.Format("{0,8:f3}", Axis.Altimeter.Offset_TopCamera_Y);
                str += "CCD - A " + ":  X =" + strx + "mm       Y =" + stry + "mm\r\n";
            }
            penPhoTxtBox.Text = str;
        }

        //保存上相机中心与定位孔中心重合位置
        private void SaveUpCamBtn_Click(object sender, EventArgs e)
        {
            g_config.ArrPickPos[0].X = Axis.trapPrm_X.getPos;
            g_config.ArrPickPos[0].Y = Axis.trapPrm_Y.getPos;
            g_config.WriteMachineInfo();
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "保存上相机中心与定位孔中心重合位置成功", "Modify");
        }
        //保存:吸头1对准定位孔中心位置
        private void SavePenZ1PosBtn_Click(object sender, EventArgs e)
        {
            g_config.ArrPickPos[1].X = Axis.trapPrm_X.getPos;
            g_config.ArrPickPos[1].Y = Axis.trapPrm_Y.getPos;
            g_config.WriteMachineInfo();
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "保存吸头1对准定位孔中心位置成功", "Modify");
        }

        private void saveScannerPosBtn_Click(object sender, EventArgs e)
        {
            Axis.Scanner.Calibrating_X = Axis.trapPrm_X.getPos;
            Axis.Scanner.Calibrating_Y = Axis.trapPrm_Y.getPos;
            g_config.WriteMachineInfo();
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "保存扫码枪对准定位孔中心重叠位置成功", "Modify");
        }

        private void saveAltimeterPosBtn_Click(object sender, EventArgs e)
        {
            Axis.Altimeter.Calibrating_X = Axis.trapPrm_X.getPos;
            Axis.Altimeter.Calibrating_Y = Axis.trapPrm_Y.getPos;
            g_config.WriteMachineInfo();
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "保存测高仪对准定位孔中心重叠位置成功", "Modify");
        }

        private void GoUpCamBtn_Click(object sender, EventArgs e)
        {
            if (!Auto_Flag.SystemBusy)
            {
                TeachAction.GO_Start(g_config.ArrPickPos[0].X, g_config.ArrPickPos[0].Y);
            }               
        }

        private void GoPenZ1PosBtn_Click(object sender, EventArgs e)
        {
            if (!Auto_Flag.SystemBusy)
            {
                TeachAction.GO_Start(g_config.ArrPickPos[1].X, g_config.ArrPickPos[1].Y);
            }
        }

        private void GoScannerPosBtn_Click(object sender, EventArgs e)
        {
            if (!Auto_Flag.SystemBusy)
            {
                TeachAction.GO_Start(Axis.Scanner.Calibrating_X, Axis.Scanner.Calibrating_Y);
            }
        }

        private void GoAltimeterPosBtn_Click(object sender, EventArgs e)
        {
            if (!Auto_Flag.SystemBusy)
            {
                TeachAction.GO_Start(Axis.Altimeter.Calibrating_X, Axis.Altimeter.Calibrating_Y);
            }
        }

        private void camFixICBtn_Click(object sender, EventArgs e)
        {
            CalCCDRate(_ConInitWnd.IIndexModel);
        }

        private void CalCCDRate(int model)
        {
            double dMoveDis = 5.0;
            double dx = 0, dy = 0, dxtemp = 0, dytemp = 0;
            double[] dXpix0 = new double[3], dYpix0 = new double[3], dXpix2 = new double[3], dYpix2 = new double[3], dDisx = new double[3], dDisy = new double[3];
            double dXrat = 0, dYrat = 0, dDisX2 = 0, dDisY2 = 0;
            int camID;
            string strShow;

            if (model == GlobConstData.ST_MODELICSTATACC || model == GlobConstData.ST_ModelTrayACC || model == GlobConstData.ST_ModelTubeInACC 
                || model == GlobConstData.ST_ModelBredeOutACC || model == GlobConstData.ST_ModelTubeInACC) camID = GlobConstData.ST_CCDUP;   //上相机切换为连续采集
            else camID = GlobConstData.ST_CCDDOWN; //上相机切换为连续采集
                                                          //来回移动几次确定上相机针对IC座的象素精度

            camFixTxt.Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "In the calculation..." : "计算中...";
            dx = Axis.trapPrm_X.getPos;
            dy = Axis.trapPrm_Y.getPos;
            if (model == GlobConstData.ST_MODELICSTATACC || model == GlobConstData.ST_ModelTrayACC || model == GlobConstData.ST_ModelTubeInACC
                || model == GlobConstData.ST_ModelBredeOutACC || model == GlobConstData.ST_ModelTubeInACC)
            {
                dxtemp = dx + dMoveDis;
                dytemp = dy + dMoveDis;
            }
            else
            {
                dxtemp = dx - dMoveDis;
                dytemp = dy - dMoveDis;
            }
            for (int n = 0; n < 3; n++)
            {
                g_act.CCDSnap(camID);
                g_act.WaitDoEvent(300);
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Image search failure" : "图象查找失败";
                if (!_ConInitWnd.FindModel(model,ref dXpix0[n],ref dYpix0[n])) { MessageBox.Show(strShow); return; }
                if (!Auto_Flag.SystemBusy)
                {
                    TeachAction.GO_Start(dxtemp, dytemp);
                }
                g_act.WaitDoEvent(1000);

                g_act.CCDSnap(camID);
                g_act.WaitDoEvent(300);
                if (!_ConInitWnd.FindModel(model,ref dXpix2[n],ref dYpix2[n])) { MessageBox.Show(strShow); return; }
                if (!Auto_Flag.SystemBusy)
                {
                    TeachAction.GO_Start(dx, dy);
                }
                g_act.WaitDoEvent(1000);

                dDisx[n] = Math.Abs(dXpix0[n] - dXpix2[n]);
                dDisy[n] = Math.Abs(dYpix0[n] - dYpix2[n]);
                dDisX2 += dDisx[n];
                dDisY2 += dDisy[n];
            }
            dXrat = dMoveDis / (dDisX2 / 3);
            dYrat = dMoveDis / (dDisY2 / 3);
            strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Whether to update accuracy?" : "是否更新精度?";
            if (MessageBox.Show(strShow, "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                g_config.ArrCCDPrec[SelectedInd].X = dXrat;
                g_config.ArrCCDPrec[SelectedInd].Y = dYrat;
                if (MultiLanguage.DefaultLanguage == GlobConstData.ST_English)
                {
                    camFixTxt.Text = string.Format("AccuracyX：{0:f3}\r\nAccuracyY：{1:f3}", g_config.ArrCCDPrec[SelectedInd].X, g_config.ArrCCDPrec[SelectedInd].Y);
                }
                else
                {
                    camFixTxt.Text = string.Format("相机精度X：{0:f3}\r\n相机精度Y：{1:f3}", g_config.ArrCCDPrec[SelectedInd].X, g_config.ArrCCDPrec[SelectedInd].Y);
                }
                g_config.WriteMachineInfo(); //写入参数
            }
        }
        private void goPosBtn1_Click(object sender, EventArgs e)
        {
            if (SelectedInd >= 6)
            {
                return;
            }
            if (!Auto_Flag.SystemBusy)
            {
                TeachAction.GO_Start(g_config.ArrCCDPos[SelectedInd].X, g_config.ArrCCDPos[SelectedInd].Y);

                if (Config.ZoomLens != 0 && SelectedInd != 2) 
                {
                    if (Config.ZoomLens == 1)
                    {
                        ZoomLens.MODBUS_ReadStatus();
                        Task.Run(() =>
                        {
                            do
                            {
                                Thread.Sleep(5);
                            } while (Axis.ZoomLens_S.ReadStatus_Busy);

                            if (Axis.ZoomLens_S.ReadStatus_Online)
                            {
                                if (SelectedInd == 0)
                                {
                                    ZoomLens.SetPos = ZoomLens.OriginPos - Axis.ZoomLens_S.Socket - ZoomLens.NowPos;
                                }
                                else if (SelectedInd == 1)
                                {
                                    ZoomLens.SetPos = ZoomLens.OriginPos - Axis.ZoomLens_S.Tray - ZoomLens.NowPos;
                                }
                                else if (SelectedInd == 3)
                                {
                                    ZoomLens.SetPos = ZoomLens.OriginPos - Axis.ZoomLens_S.BredeIn - ZoomLens.NowPos;
                                }
                                else if (SelectedInd == 4)
                                {
                                    ZoomLens.SetPos = ZoomLens.OriginPos - Axis.ZoomLens_S.BredeOut - ZoomLens.NowPos;
                                }
                                else if (SelectedInd == 5)
                                {
                                    ZoomLens.SetPos = ZoomLens.OriginPos - Axis.ZoomLens_S.TubeIn - ZoomLens.NowPos;
                                }
                                ZoomLens.MODBUS_DST();
                            }
                            else
                            {
                                MessageBox.Show(MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Abnormal communication" : "通讯异常");
                            }
                        });
                    }
                    else if (Config.ZoomLens == 2)
                    {
                        if (SelectedInd == 0)
                        {
                            ZoomLens.SetPos = Axis.ZoomLens_S.Socket;
                        }
                        else if (SelectedInd == 1)
                        {
                            ZoomLens.SetPos = Axis.ZoomLens_S.Tray;
                        }
                        else if (SelectedInd == 3)
                        {
                            ZoomLens.SetPos = Axis.ZoomLens_S.BredeIn;
                        }
                        else if (SelectedInd == 4)
                        {
                            ZoomLens.SetPos = Axis.ZoomLens_S.BredeOut;
                        }
                        else if (SelectedInd == 5)
                        {
                            ZoomLens.SetPos = Axis.ZoomLens_S.TubeIn;
                        }
                        axis.Camera_Position_Control(ZoomLens.SetPos, true);
                    }
                }
            }
        }

        private void findModBtn1_Click(object sender, EventArgs e)
        {
            _ConInitWnd.FindImgModel();
        }
    }
}
