using PLC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HalconDotNet;
using BAR.Commonlib.Utils;
using BAR.Commonlib;
using BAR.Commonlib.Components;
using BAR.ControlPanels;
using BAR.Commonlib.Events;
using System.Security.AccessControl;
using CCWin.SkinControl;
using BAR.CommonLib_v1._0;

namespace BAR.Windows
{
    public partial class ConfigInitWnd : Form
    {
        #region
        public int IIndexModel;
        public int ISelBoxInd;
        public HObject SourceImage;

        public BAR main = null;
        PLC1 plc = new PLC1();
        Axis axis = new Axis();

        Act g_act = Act.GetInstance();
        Config g_config = Config.GetInstance();
        HalconImgUtil _HalconUtil = HalconImgUtil.GetInstance();

        HTuple _wndID, tempWndID;
        ImgOperater _ImgOperater;
        bool _IsHaveImg;

        int ISelLenModUp;

        bool isDrawLeanBox;         ///<画框方式
        bool isOperateImg;

        bool isLoaded = false;
        bool isCapModeShow = false;
        bool isChooseCamShow = false;

        bool isModelLearn = false;

        CamAccAndPenDisPnl pnlCamAccAndPenDis;
        GBPPosPnl pnlGBPPos;
        ICPosPnl pnlICPos;
        ICStatePosPnl pnlICState;
        ICShapePnl pnlICShape;
        AutoHeightPnl pnlAutoHeight;
        ICDirAndFlawPnl pnlICDirAndFlaw;
        public CMKPnl pnlCMKPnl;
        public AutoRevisePosPnl pnlAutoRevisePos;
        public MarkManageWnd _MarkManageWnd = new MarkManageWnd();

        #endregion

        public ConfigInitWnd()
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();
            ISelBoxInd = 0;
            pnlCamAccAndPenDis = new CamAccAndPenDisPnl(this);     
            pnlGBPPos = new GBPPosPnl(this);
            pnlICState = new ICStatePosPnl(this);
            pnlICPos = new ICPosPnl(this);
            pnlICShape = new ICShapePnl(this);
            pnlAutoHeight = new AutoHeightPnl(this);
            pnlICDirAndFlaw = new ICDirAndFlawPnl(this);
            pnlAutoRevisePos = new AutoRevisePosPnl(this);
            pnlCMKPnl = new CMKPnl(this);
            //_MarkManageWnd.Owner = this;
            this.__CreateHalconWnd();
            this.__InitOperateWnd();
        }

        private void ConfigInitWnd_Load(object sender, EventArgs e)
        {
            //timer1.Enabled = true;
            
            ISelBoxInd = 0;
            if (!isLoaded)
            {
                main = (BAR)this.Owner;
                //this.__InitOperateWnd();
                __InitUI();
                this.isLoaded = true;
                for (int i = 0; i < 4; i++)
                {
                    string ctlName;
                    Control[] tagAry;
                    ctlName = "z" + (i + 1) + "Lab";
                    tagAry = Controls.Find(ctlName, true);
                    if (tagAry.Length != 0)
                    {
                        (tagAry.First() as Label).Visible = i < UserConfig.AxisZC ? true : false;
                    }

                    ctlName = "labelZ" + (i + 1);
                    tagAry = Controls.Find(ctlName, true);
                    if (tagAry.Length != 0)
                    {
                        (tagAry.First() as Label).Visible = i < UserConfig.AxisZC ? true : false;
                    }

                    ctlName = "c" + (i + 1) + "Lab";
                    tagAry = Controls.Find(ctlName, true);
                    if (tagAry.Length != 0)
                    {
                        (tagAry.First() as Label).Visible = i < UserConfig.VacuumPenC ? true : false;
                    }

                    ctlName = "labelC" + (i + 1);
                    tagAry = Controls.Find(ctlName, true);
                    if (tagAry.Length != 0)
                    {
                        (tagAry.First() as Label).Visible = i < UserConfig.VacuumPenC ? true : false;
                    }

                    ctlName = "Axis_Z" + (i + 1) + "_JOG负_Up";
                    tagAry = Controls.Find(ctlName, true);
                    if (tagAry.Length != 0)
                    {
                        (tagAry.First() as ButtonBase).Visible = i < UserConfig.AxisZC ? true : false;
                    }

                    ctlName = "Axis_Z" + (i + 1) + "_JOG正_Up";
                    tagAry = Controls.Find(ctlName, true);
                    if (tagAry.Length != 0)
                    {
                        (tagAry.First() as ButtonBase).Visible = i < UserConfig.AxisZC ? true : false;
                    }

                    ctlName = "Axis_C" + (i + 1) + "_JOG负_Up";
                    tagAry = Controls.Find(ctlName, true);
                    if (tagAry.Length != 0)
                    {
                        (tagAry.First() as ButtonBase).Visible = i < UserConfig.VacuumPenC ? true : false;
                    }

                    ctlName = "Axis_C" + (i + 1) + "_JOG正_Up";
                    tagAry = Controls.Find(ctlName, true);
                    if (tagAry.Length != 0)
                    {
                        (tagAry.First() as ButtonBase).Visible = i < UserConfig.VacuumPenC ? true : false;
                    }
                }

                if (UserTask.PenType == 1)//吸笔类型
                {
                    Axis_Z1_JOG负_Up.Location = new Point(220, Axis_Z1_JOG负_Up.Location.Y);
                    Axis_Z1_JOG正_Up.Location = new Point(220, Axis_Z1_JOG正_Up.Location.Y);
                    labelX.Location = new Point(labelX.Location.X, labelC1.Location.Y);
                    labelY.Location = new Point(labelX.Location.X, labelC2.Location.Y);
                    labelZ1.Location = new Point(labelX.Location.X, labelC3.Location.Y);
                    xLab.Location = new Point(xLab.Location.X, c1Lab.Location.Y);
                    yLab.Location = new Point(xLab.Location.X, c2Lab.Location.Y);
                    z1Lab.Location = new Point(xLab.Location.X, c3Lab.Location.Y);
                    labelZ1.Text = "Z轴当前值:";
                }
            }
            //this.__CreateHalconWnd();
            this.__InitCameraParam();
            this.__RegistCameraHandle();

            __SetBox(GlobConstData.ST_MODELICSTATACC);
            timer1.Start();
            speedLowRBtn.Checked = true;
            proccedRBtn.Checked = true;
            tabConfigInitWnd_SelectedIndexChanged(null, null);
            VisibleControl();
        }

        public void PermissionControl(bool flag)
        {
            if (flag)
            {
                pnlCamAccAndPenDis.Enabled = true;
                pnlAutoHeight.panel1.Enabled = true;
            }
            else
            {
                pnlCamAccAndPenDis.Enabled = false;
                pnlAutoHeight.panel1.Enabled = false;
            }
        }

        /// <summary>
        /// 配置控件是否可见
        /// </summary>
        public void VisibleControl()
        {
            if (!Vision_3D.Function)
            {
                tabConfigInitWnd.Controls.Remove(pagICShape);
            }
            if (Config.Altimeter == GlobConstData.CLOSE)
            {
                tabConfigInitWnd.Controls.Remove(pagAutoHeight);
            }
            if (Config.ICDirAndFlaw == GlobConstData.CLOSE)
            {
                tabConfigInitWnd.Controls.Remove(pagICDirAndFlaw);
            }
            if (Config.ZoomLens == 0)
            {
                tabConfigInitWnd.Controls.Remove(pagAutoRevisePos);
            }
        }

        public void InitWnd()
        {
            ISelBoxInd = 0;
            if (!isLoaded)
            {
                main = (BAR)this.Owner;
                this.__InitOperateWnd();
                __InitUI();
                this.isLoaded = true;
            }
            this.__CreateHalconWnd();
            this.__InitCameraParam();
            this.__RegistCameraHandle();

            __SetBox(GlobConstData.ST_MODELICSTATACC);
            timer1.Start();
            speedLowRBtn.Checked = true;
            proccedRBtn.Checked = true;
            angle2RBtn.Checked = true;
            tabConfigInitWnd_SelectedIndexChanged(null, null);
        }

        //创建Halcon窗口
        private void __CreateHalconWnd()
        {
            _wndID = hWindowControl2.HalconWindow;
            HOperatorSet.SetSystem("width", hWindowControl2.Width);
            HOperatorSet.SetSystem("height", hWindowControl2.Height);
            HDevWindowStack.Push(_wndID);
            if (_wndID == null)
            {
                return;
            }
            HOperatorSet.SetColor(_wndID, "red");
            g_act.ArrWndID[GlobConstData.SELECT_CONFIGINIT_WND] = _wndID;
        }
        private void __InitOperateWnd()
        {
            _ImgOperater = new ImgOperater();
            _ImgOperater.Initial(this, _wndID);
            _ImgOperater.IsZoomImg = true;
        }
        private void __InitCameraParam()
        {
            g_act.ISelectWnd = GlobConstData.SELECT_CONFIGINIT_WND;

            g_act.CCDCap(GlobConstData.ST_CCDUP);
            this.upCamBtn.Checked = true;
        }
        private void __RegistCameraHandle()
        {
            if (Config.CameraType == GlobConstData.Camera_DH)
            {
                g_act.ArrDHCameraUtils[GlobConstData.ST_CCDUP].MsgEventHandler += OnUpCamMsgEventHandler;
                g_act.ArrDHCameraUtils[GlobConstData.ST_CCDDOWN].MsgEventHandler += OnDownCamMsgEventHandler;
            }
            else
            {
                g_act.ArrHRCameraUtils[GlobConstData.ST_CCDUP].MsgEventHandler += OnUpCamMsgEventHandler_HR;
                g_act.ArrHRCameraUtils[GlobConstData.ST_CCDDOWN].MsgEventHandler += OnDownCamMsgEventHandler_HR;
            }
        }
        private void __UnRegistCameraHandle()
        {
            if (Config.CameraType == GlobConstData.Camera_DH)
            {
                g_act.ArrDHCameraUtils[GlobConstData.ST_CCDUP].MsgEventHandler -= OnUpCamMsgEventHandler;
                g_act.ArrDHCameraUtils[GlobConstData.ST_CCDDOWN].MsgEventHandler -= OnDownCamMsgEventHandler;
            }
            else
            {
                g_act.ArrHRCameraUtils[GlobConstData.ST_CCDUP].MsgEventHandler -= OnUpCamMsgEventHandler_HR;
                g_act.ArrHRCameraUtils[GlobConstData.ST_CCDDOWN].MsgEventHandler -= OnDownCamMsgEventHandler_HR;
            }
        }
        private void __InitUI()
        {
            __InitPanel();

        }
        private void __InitPanel()
        {
            this.pagCamAccAndPenDis.Controls.Add(pnlCamAccAndPenDis);
            this.pagGBPPos.Controls.Add(pnlGBPPos);
            this.pagICPos.Controls.Add(pnlICPos);
            this.pagICState.Controls.Add(pnlICState);
            this.pagICShape.Controls.Add(pnlICShape);
            this.pagAutoHeight.Controls.Add(pnlAutoHeight);
            this.pagICDirAndFlaw.Controls.Add(pnlICDirAndFlaw);
            this.pagAutoRevisePos.Controls.Add(pnlAutoRevisePos);
            this.pagCMK.Controls.Add(pnlCMKPnl);
        }
        public void __SetBox(int selBox)
        {
            ISelBoxInd = selBox;

            _HalconUtil.ArrBoxW[0] = g_config.ArrLampBox[selBox].IMW;
            _HalconUtil.ArrBoxH[0] = g_config.ArrLampBox[selBox].IMH;
            _HalconUtil.ArrBoxW[1] = g_config.ArrLampBox[selBox].ISW;
            _HalconUtil.ArrBoxH[1] = g_config.ArrLampBox[selBox].ISH;
            _HalconUtil.IBoxC =      g_config.ArrLampBox[selBox].IC;
            Disp_Cross();
        }
        private void OnUpCamMsgEventHandler(object sender, MsgEvent e)
        {
            try
            {
                if (e.MsgType == MsgEvent.MSG_IMGPAINT)
                {
                    DHCameraUtil cam = (DHCameraUtil)sender;
                    if (cam.SelectedWnd == GlobConstData.SELECT_CONFIGINIT_WND)
                    {
                        if (cam.ICameraID == g_act.ISelectCam)
                        {
                            HObject tmp_img1, tmp_img2; //先缓存图像数据到两个临时变量，最后在放到全局数据
                            HOperatorSet.GenImage1(out tmp_img1, "byte", GlobConstData.IMG500_WIDTH, GlobConstData.IMG500_HEIGHT, (HTuple)g_act.ArrDHCameraUtils[g_act.ISelectCam].PtrBufferRaw);
                            HOperatorSet.MirrorImage(tmp_img1, out tmp_img2, "row");
                            tmp_img1.Dispose();
                            HOperatorSet.MirrorImage(tmp_img2, out SourceImage, "column");
                            tmp_img2.Dispose();

                            _HalconUtil.DispImage(_ImgOperater, _wndID, SourceImage, isOperateImg, ref _IsHaveImg);
                            if (g_act.IsCamSnapMode != true) SourceImage.Dispose();
                            Disp_Cross();
                        }
                    } 
                }
            }
            catch (HalconException hEx)
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "TeachUpCamera" + hEx.Message, "Halcon");
            }
        }
        private void OnDownCamMsgEventHandler(object sender, MsgEvent e)
        {
            try
            {
                if (e.MsgType == MsgEvent.MSG_IMGPAINT)
                {
                    DHCameraUtil cam = (DHCameraUtil)sender;
                    if (cam.SelectedWnd == GlobConstData.SELECT_CONFIGINIT_WND)
                    {
                        if (cam.ICameraID == g_act.ISelectCam)
                        {
                            HObject tmp_img1;

                            HOperatorSet.GenImage1(out tmp_img1, "byte", GlobConstData.IMG130_WIDTH, GlobConstData.IMG130_HEIGHT, (HTuple)g_act.ArrDHCameraUtils[g_act.ISelectCam].PtrBufferRaw);
                            HOperatorSet.MirrorImage(tmp_img1, out SourceImage, "row");
                            tmp_img1.Dispose();
                            _HalconUtil.DispImage(_ImgOperater, _wndID, SourceImage, isOperateImg, ref _IsHaveImg);
                            if (g_act.IsCamSnapMode != true) SourceImage.Dispose();
                            Disp_Cross();
                        }
                    }
                }
            }
            catch (HalconException hEx)
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "TeachDownCamera" + hEx.Message, "Halcon");
            }
        }

        private void OnUpCamMsgEventHandler_HR(object sender, MsgEvent e)
        {
            try
            {
                if (e.MsgType == MsgEvent.MSG_IMGPAINT)
                {
                    HRCameraUtil cam = (HRCameraUtil)sender;
                    if (cam.SelectedWnd == GlobConstData.SELECT_CONFIGINIT_WND)
                    {
                        if (cam.ICameraID == g_act.ISelectCam)
                        {
                            HObject tmp_img1, tmp_img2; //先缓存图像数据到两个临时变量，最后在放到全局数据
                            HOperatorSet.GenImage1(out tmp_img1, "byte", GlobConstData.IMG500_WIDTH, GlobConstData.IMG500_HEIGHT, (HTuple)g_act.ArrHRCameraUtils[g_act.ISelectCam].PtrBufferRaw);
                            HOperatorSet.MirrorImage(tmp_img1, out tmp_img2, "row");
                            tmp_img1.Dispose();
                            HOperatorSet.MirrorImage(tmp_img2, out SourceImage, "column");
                            tmp_img2.Dispose();

                            _HalconUtil.DispImage(_ImgOperater, _wndID, SourceImage, isOperateImg, ref _IsHaveImg);
                            if (g_act.IsCamSnapMode != true) SourceImage.Dispose();
                            Disp_Cross();
                        }
                    }
                }
            }
            catch (HalconException hEx)
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "TeachUpCamera" + hEx.Message, "Halcon");
            }
        }
        private void OnDownCamMsgEventHandler_HR(object sender, MsgEvent e)
        {
            try
            {
                if (e.MsgType == MsgEvent.MSG_IMGPAINT)
                {
                    HRCameraUtil cam = (HRCameraUtil)sender;
                    if (cam.SelectedWnd == GlobConstData.SELECT_CONFIGINIT_WND)
                    {
                        if (cam.ICameraID == g_act.ISelectCam)
                        {
                            HObject tmp_img1;

                            HOperatorSet.GenImage1(out tmp_img1, "byte", GlobConstData.IMG130_WIDTH, GlobConstData.IMG130_HEIGHT, (HTuple)g_act.ArrHRCameraUtils[g_act.ISelectCam].PtrBufferRaw);
                            HOperatorSet.MirrorImage(tmp_img1, out SourceImage, "row");
                            tmp_img1.Dispose();
                            _HalconUtil.DispImage(_ImgOperater, _wndID, SourceImage, isOperateImg, ref _IsHaveImg);
                            if (g_act.IsCamSnapMode != true) SourceImage.Dispose();
                            Disp_Cross();
                        }
                    }
                }
            }
            catch (HalconException hEx)
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "TeachDownCamera" + hEx.Message, "Halcon");
            }
        }
        private void Disp_Cross()
        {
            if (g_act.DispLineType == GlobConstData.DISPLAYLINETYPE_CROSS)
            {
                _HalconUtil.DisplayCross(_wndID, _ImgOperater.DZoomWndFactor, false);

            }
            else if (g_act.DispLineType == GlobConstData.DISPLAYLINETYPE_SCALE)
            {
                _HalconUtil.DisplayCross(_wndID, 0.2, true);

            }
            else if (g_act.DispLineType == GlobConstData.DISPLAYLINETYPE_BACKRECT)
            {
                _HalconUtil.DisplayCross(_wndID, _ImgOperater.DZoomWndFactor, false);
                _HalconUtil.DarwICBox(_wndID);
            }

        }
        public void DispImage_Redraw()
        {
            _HalconUtil.DispImage(_ImgOperater, _wndID, SourceImage, isOperateImg, ref _IsHaveImg);
            Disp_Cross();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (this.btnW2Add.ControlState.Equals(CCWin.SkinClass.ControlState.Pressed)) { _HalconUtil.ArrBoxW[0]++; }
            else if (this.btnW2Cut.ControlState.Equals(CCWin.SkinClass.ControlState.Pressed)) { _HalconUtil.ArrBoxW[0]--; }
            else if (this.btnH2Add.ControlState.Equals(CCWin.SkinClass.ControlState.Pressed)) { _HalconUtil.ArrBoxH[0]++; }
            else if (this.btnH2Cut.ControlState.Equals(CCWin.SkinClass.ControlState.Pressed)) { _HalconUtil.ArrBoxH[0]--; }
            else if (this.btnWAdd.ControlState.Equals(CCWin.SkinClass.ControlState.Pressed)) { _HalconUtil.ArrBoxW[1]++; }
            else if (this.btnWCut.ControlState.Equals(CCWin.SkinClass.ControlState.Pressed)) { _HalconUtil.ArrBoxW[1]--; }
            else if (this.btnHAdd.ControlState.Equals(CCWin.SkinClass.ControlState.Pressed)) { _HalconUtil.ArrBoxH[1]++; }
            else if (this.btnHCut.ControlState.Equals(CCWin.SkinClass.ControlState.Pressed)) { _HalconUtil.ArrBoxH[1]--; }
            else if (this.btnW3Add.ControlState.Equals(CCWin.SkinClass.ControlState.Pressed)) { _HalconUtil.IBoxC++; }
            else if (this.btnW3Cut.ControlState.Equals(CCWin.SkinClass.ControlState.Pressed) && _HalconUtil.IBoxC > 0) { _HalconUtil.IBoxC--; };

            if (Vision_3D.Function)
            {
                pnlICShape.pnlICType.Visible = true;
                switch (Vision_3D.ICType)
                {
                    case 0:
                        pnlICShape.RbtnQFP.Checked = true;
                        break;
                    case 1:
                        pnlICShape.RbtnBGA.Checked = true;
                        break;
                    case 2:
                        pnlICShape.RbtnSOP.Checked = true;
                        break;
                    default:
                        break;
                }
            }
            pnlICShape.txtReceive.Text = BAR.tcpClient_3D.ReciveDate;

            if (Auto_Flag.Enabled_NCCModel)
            {
                pnlICPos.enableMod2CBox.Checked = true;
            }
            else
            {
                pnlICPos.enableMod2CBox.Checked = false;
            }

        }
        public void InitImgModel()
        {
            this.EnableBtnCam1(true);
            this.ISelLenModUp = IIndexModel;

            LeanModelImage(0, ISelLenModUp, true);
            if (ISelLenModUp == GlobConstData.ST_MODELICSTATPOS)
            {
                this.pnlICState.DispMode1Pic();
            }
            else if (ISelLenModUp == GlobConstData.ST_MODELICPOS)
            {
                this.pnlICPos.DispModelImage1(GlobConstData.ST_MODELICPOS);
            }
            else if (ISelLenModUp == GlobConstData.ST_MODELICPOS_NCC)
            {
                this.pnlICPos.DispModelImage2(GlobConstData.ST_MODELICPOS_NCC);
            }
            else if (ISelLenModUp == GlobConstData.ST_ModelTrayPOS || ISelLenModUp == GlobConstData.ST_ModelBredeInPOS
                    || ISelLenModUp == GlobConstData.ST_ModelBredeOutPOS || ISelLenModUp == GlobConstData.ST_ModelTubeInPOS)
            {
                this.pnlAutoRevisePos.DispModelImage1(ISelLenModUp);
            }
            this.EnableBtnCam1(false);

        }
        /// <summary>
        /// 图像模板匹配
        /// </summary>
        /// <param name="ind">匹配的类型</param>
        /// <param name="dy">Y偏移</param>
        /// <param name="dx">X偏移</param>
        public bool FindModel(int ind, ref double dy, ref double dx)
        {
            HTuple RltYc = new HTuple(), RltXc = new HTuple(), Rltang = new HTuple(), RltScore = new HTuple();
            bool nRet;
            if (ind == GlobConstData.ST_MODELICSTATACC || ind == GlobConstData.ST_MODELICSTATPOS)
            {
                nRet = _HalconUtil.NCCModelPar(true, _wndID, SourceImage, g_config.ArrModID[ind],
                    g_config.ArrMod[ind].SeachBox.Cy, g_config.ArrMod[ind].SeachBox.Cx, g_config.ArrMod[ind].SeachBox.Phi, g_config.ArrMod[ind].SeachBox.len1, g_config.ArrMod[ind].SeachBox.len2,
                    0.6, 30, 0.6, 1, out RltYc, out RltXc, out Rltang, out RltScore);
            }
            else
            {
                nRet = _HalconUtil.FindModel(true, _wndID, SourceImage, g_config.ArrModID[ind],
                        g_config.ArrMod[ind].SeachBox.Cy, g_config.ArrMod[ind].SeachBox.Cx, g_config.ArrMod[ind].SeachBox.Phi, g_config.ArrMod[ind].SeachBox.len1, g_config.ArrMod[ind].SeachBox.len2,
                        0.5, 1, out RltYc, out RltXc, out Rltang, out RltScore);
            }

            if (nRet)
            {
                dy = RltYc.D;
                dx = RltXc.D;
                RltYc?.Dispose();
                RltXc?.Dispose();
                Rltang?.Dispose();
                RltScore?.Dispose();
                return true;
            }
            else
            {
                RltYc?.Dispose();
                RltXc?.Dispose();
                Rltang?.Dispose();
                RltScore?.Dispose();
                return false;
            }
            
        }

        /// <summary>
        /// 相机精度与吸笔间距界面查找模板
        /// </summary>
        public void FindImgModel()
        {
            DispImage_Redraw();
            if (IIndexModel < 0) return;
            bool retBool;
            int ind = IIndexModel;
            HTuple resYC = new HTuple(), resXC = new HTuple(), resAng = new HTuple(), resScore = new HTuple();
            if (ind == GlobConstData.ST_MODELICSTATPOS || ind == GlobConstData.ST_ModelTrayPOS || ind == GlobConstData.ST_ModelBredeInPOS
                || ind == GlobConstData.ST_ModelBredeOutPOS || ind == GlobConstData.ST_ModelTubeInPOS || ind == GlobConstData.ST_MODELICSTATACC 
                || ind == GlobConstData.ST_ModelTrayACC || ind == GlobConstData.ST_ModelBredeInACC || ind == GlobConstData.ST_ModelBredeOutACC 
                || ind == GlobConstData.ST_ModelTubeInACC)
            {
                g_act.CCDSnap(GlobConstData.ST_CCDUP);
            }
            else if (ind == GlobConstData.ST_MODELPENACC)
            {
                g_act.CCDSnap(GlobConstData.ST_CCDDOWN);
            }
            g_act.WaitDoEvent(200);
            if (ind == GlobConstData.ST_MODELICPOS)
            {
                retBool = _HalconUtil.FindModelPar(true, _wndID, SourceImage, g_config.ArrModID[ind],
                    g_config.ArrMod[ind].SeachBox.Cy, g_config.ArrMod[ind].SeachBox.Cx, g_config.ArrMod[ind].SeachBox.Phi, g_config.ArrMod[ind].SeachBox.len1, g_config.ArrMod[ind].SeachBox.len2,
                    g_config.DScore, g_config.DAngLimit, g_config.DMaxOverlap, g_config.DGreediness, 1, out resYC, out resXC, out resAng, out resScore);
            }
            else if(ind == GlobConstData.ST_MODELICSTATACC || ind == GlobConstData.ST_MODELICSTATPOS || ind == GlobConstData.ST_ModelBredeInPOS || ind == GlobConstData.ST_ModelBredeOutPOS)
            {
                retBool = _HalconUtil.NCCModelPar(true, _wndID, SourceImage, g_config.ArrModID[ind],
                    g_config.ArrMod[ind].SeachBox.Cy, g_config.ArrMod[ind].SeachBox.Cx, g_config.ArrMod[ind].SeachBox.Phi, g_config.ArrMod[ind].SeachBox.len1, g_config.ArrMod[ind].SeachBox.len2,
                    0.4, 30, 0.2, 1, out resYC, out resXC, out resAng, out resScore);
            }
            else if (ind == GlobConstData.ST_ModelTrayPOS)
            {
                retBool = _HalconUtil.NCCModelPar(true, _wndID, SourceImage, g_config.ArrModID[ind],
                    g_config.ArrMod[ind].SeachBox.Cy, g_config.ArrMod[ind].SeachBox.Cx, g_config.ArrMod[ind].SeachBox.Phi, g_config.ArrMod[ind].SeachBox.len1, g_config.ArrMod[ind].SeachBox.len2,
                    0.4, 90, 0.2, 1, out resYC, out resXC, out resAng, out resScore);
            }
            else
            {
                retBool = _HalconUtil.FindModel(true, _wndID, SourceImage, g_config.ArrModID[ind],
                    g_config.ArrMod[ind].SeachBox.Cy, g_config.ArrMod[ind].SeachBox.Cx, g_config.ArrMod[ind].SeachBox.Phi, g_config.ArrMod[ind].SeachBox.len1, g_config.ArrMod[ind].SeachBox.len2,
                    0.3, 1, out resYC, out resXC, out resAng, out resScore);
            }
            resYC?.Dispose();
            resXC?.Dispose();
            resAng?.Dispose();
            resScore?.Dispose();
        }

        public bool FindIndexImage(int ind, ref double dy, ref double dx, ref double dr)
        {
            if (ind < 0) return false;
            if (g_act.ISelectWnd == GlobConstData.SELECT_MAIN_WND)
            {
                SourceImage = g_act.ArrSourceImage[g_act.ISelectCam];
            }
            tempWndID = g_act.ArrWndID[g_act.ISelectWnd];
            DispImage_Redraw();
            HTuple resYC = new HTuple(), resXC = new HTuple(), resAng = new HTuple(), resScore = new HTuple();
            //HTuple resYC, resXC, resAng, resScore;
            bool retBool = false;
            if (ind == GlobConstData.ST_MODELICPOS)
            {
                retBool = _HalconUtil.FindModelPar(true, tempWndID, SourceImage, g_config.ArrModID[ind],
                     g_config.ArrMod[ind].SeachBox.Cy, g_config.ArrMod[ind].SeachBox.Cx, g_config.ArrMod[ind].SeachBox.Phi, g_config.ArrMod[ind].SeachBox.len1, g_config.ArrMod[ind].SeachBox.len2,
                     g_config.DScore, g_config.DAngLimit, g_config.DMaxOverlap, g_config.DGreediness, 1, out resYC, out resXC, out resAng, out resScore);
            }
            else if (ind == GlobConstData.ST_MODELICPOS_NCC)
            {
                retBool = _HalconUtil.NCCModelPar(true, tempWndID, SourceImage, g_config.ArrModID[ind],
                     g_config.ArrMod[ind].SeachBox.Cy, g_config.ArrMod[ind].SeachBox.Cx, g_config.ArrMod[ind].SeachBox.Phi, g_config.ArrMod[ind].SeachBox.len1, g_config.ArrMod[ind].SeachBox.len2,
                     g_config.DScore, g_config.DAngLimit, g_config.DMaxOverlap, 1, out resYC, out resXC, out resAng, out resScore);
            }
            else if (ind == GlobConstData.ST_MODELICSTATPOS || ind == GlobConstData.ST_MODELICSTATACC || ind == GlobConstData.ST_ModelBredeInPOS || ind == GlobConstData.ST_ModelBredeOutPOS)
            {
                retBool = _HalconUtil.NCCModelPar(true, tempWndID, SourceImage, g_config.ArrModID[ind],
                    g_config.ArrMod[ind].SeachBox.Cy, g_config.ArrMod[ind].SeachBox.Cx, g_config.ArrMod[ind].SeachBox.Phi, g_config.ArrMod[ind].SeachBox.len1, g_config.ArrMod[ind].SeachBox.len2,
                    0.4, 30, 0.6, 1, out resYC, out resXC, out resAng, out resScore);
            }
            else if (ind == GlobConstData.ST_ModelTrayPOS)
            {
                retBool = _HalconUtil.NCCModelPar(true, tempWndID, SourceImage, g_config.ArrModID[ind],
                    g_config.ArrMod[ind].SeachBox.Cy, g_config.ArrMod[ind].SeachBox.Cx, g_config.ArrMod[ind].SeachBox.Phi, g_config.ArrMod[ind].SeachBox.len1, g_config.ArrMod[ind].SeachBox.len2,
                    0.4, 90, 0.6, 1, out resYC, out resXC, out resAng, out resScore);
            }
            else
            {
                retBool = _HalconUtil.FindModel(true, tempWndID, SourceImage, g_config.ArrModID[ind],
                    g_config.ArrMod[ind].SeachBox.Cy, g_config.ArrMod[ind].SeachBox.Cx, g_config.ArrMod[ind].SeachBox.Phi, g_config.ArrMod[ind].SeachBox.len1, g_config.ArrMod[ind].SeachBox.len2,
                    0.4, 1,out resYC, out resXC, out resAng, out resScore);
            }
            if(retBool)
            {
                dy = resYC.D;
                dx = resXC.D;
                dr = resAng.D;
            }
            else
            {
                dy = 0;
                dx = 0;
                dr = 0;
            }
            resYC?.Dispose();
            resXC?.Dispose();
            resAng?.Dispose();
            resScore?.Dispose();
            return retBool;
        }
        private void EnableBtnCam1(bool flag)
        {
            isModelLearn = flag;
        }
        private void DispModelImage(int sel, int ind)
        {
            String path = g_config.StrProductDir + "\\" + g_config.StrCurProduct + "_" + ind + ".jpg";
            if (System.IO.Directory.Exists(path) == false)
            {
                return;
            }
            _HalconUtil.LoadImgAndDisp(_wndID, SourceImage, path);
        }

        /// <summary>
        /// 学习模板
        /// </summary>
        /// <param name="sel"></param>
        /// <param name="indexModel"></param>
        /// <param name="flag"></param>
        public void LeanModelImage(int sel, int indexModel, bool flag)
        {
            int MaskInd = -1;
            int Angle = 30;
            bool IsOffset = false;
            isModelLearn = true;
            string msgStr = "";
            IIndexModel = indexModel;
            if (IIndexModel == GlobConstData.ST_MODELICSTATACC)
                msgStr = "是否示教[IC座精度]图象模板[上相机] \n YES[重新学习] NO[取消操作]";
            else if (IIndexModel == GlobConstData.ST_ModelTrayACC)
                msgStr = "是否示教[料盘精度]图象模板[上相机] \n YES[重新学习] NO[取消操作]";
            else if (IIndexModel == GlobConstData.ST_MODELPENACC)
                msgStr = "是否示教[吸头精度]图象模板[下相机] \n YES[重新学习] NO[取消操作]";
            else if (IIndexModel == GlobConstData.ST_MODELICSTATPOS){
                MaskInd = 0;
                IsOffset = true;
                msgStr = "是否示教[IC座定位]图象模板[上相机] \n YES[重新学习] NO[取消操作]";}
            else if (IIndexModel == GlobConstData.ST_MODELICPOS)
                msgStr = "是否示教[IC芯片定位]图象模板[下相机] \n YES[重新学习] NO[取消操作]";
            else if (IIndexModel == GlobConstData.ST_MODELICPOS_NCC){
                MaskInd = 1;
                msgStr = "是否示教[IC芯片定位]图象NCC模板[下相机] \n YES[重新学习] NO[取消操作]";}
            else if (IIndexModel == GlobConstData.ST_ModelTrayPOS){
                MaskInd = 2;
                Angle = 90;
                IsOffset = true;
                msgStr = "是否示教[料盘定位]图象模板[上相机] \n YES[重新学习] NO[取消操作]";}
            else if (IIndexModel == GlobConstData.ST_ModelBredeInPOS){
                MaskInd = 3;
                IsOffset = true;
                msgStr = "是否示教[飞达定位]图象模板[上相机] \n YES[重新学习] NO[取消操作]";}
            else if (IIndexModel == GlobConstData.ST_ModelBredeOutPOS){
                MaskInd = 4;
                IsOffset = true;
                msgStr = "是否示教[编带定位]图象模板[上相机] \n YES[重新学习] NO[取消操作]";}
            else if (IIndexModel == GlobConstData.ST_ModelTubeInPOS){
                MaskInd = 5;
                IsOffset = true;
                msgStr = "是否示教[料管定位]图象模板[上相机] \n YES[重新学习] NO[取消操作]";}
            else if (IIndexModel == GlobConstData.ST_ModelBredeInACC)
            {
                MaskInd = 6;
                msgStr = "是否示教[飞达精度]图象模板[上相机] \n YES[重新学习] NO[取消操作]";
            }
            else if (IIndexModel == GlobConstData.ST_ModelBredeOutACC)
            {
                MaskInd = 7;
                msgStr = "是否示教[编带精度]图象模板[上相机] \n YES[重新学习] NO[取消操作]";
            }
            else if (IIndexModel == GlobConstData.ST_ModelTubeInACC)
            {
                MaskInd = 8;
                msgStr = "是否示教[料管精度]图象模板[上相机] \n YES[重新学习] NO[取消操作]";
            }

            if (MessageBox.Show(MultiLanguage.GetString(msgStr), "Information:", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) return;
            drawCrossBtn_Click(null, null);
            int ind = IIndexModel;
            if (flag)
            {
                DispImage_Redraw();
                _HalconUtil.RepDarwBoxNoPhi(_wndID, ref g_config.ArrMod[ind].ModelBox.Cy, ref g_config.ArrMod[ind].ModelBox.Cx,
                  ref g_config.ArrMod[ind].ModelBox.len1, ref g_config.ArrMod[ind].ModelBox.len2, "blue", MultiLanguage.GetString("调整模板框大小与区域"), IsOffset);
                
                if (ind == GlobConstData.ST_MODELICSTATPOS || ind == GlobConstData.ST_ModelTrayPOS || ind == GlobConstData.ST_ModelBredeInPOS
                     || ind == GlobConstData.ST_ModelBredeOutPOS || ind == GlobConstData.ST_ModelTubeInPOS)
                {
                    DispImage_Redraw();
                    _HalconUtil.DispRectangle1(_wndID, g_config.ArrMod[ind].ModelBox.Cy, g_config.ArrMod[ind].ModelBox.Cx,
                  g_config.ArrMod[ind].ModelBox.len1, g_config.ArrMod[ind].ModelBox.len2);
                    _HalconUtil.RepDarwBoxNoPhi(_wndID, ref g_config.By[ind].Cy, ref g_config.By[ind].Cx,
                  ref g_config.By[ind].len1, ref g_config.By[ind].len2, "yellow", MultiLanguage.GetString("调整掩膜框大小与区域"), IsOffset);
                }
                else if (ind == GlobConstData.ST_MODELICPOS_NCC || ind == GlobConstData.ST_MODELICSTATACC || ind == GlobConstData.ST_ModelTrayACC 
                    || ind == GlobConstData.ST_ModelBredeInACC || ind == GlobConstData.ST_ModelBredeOutACC || ind == GlobConstData.ST_ModelTubeInACC)
                {
                    DispImage_Redraw();
                    _HalconUtil.DispRectangle1(_wndID, g_config.ArrMod[ind].ModelBox.Cy, g_config.ArrMod[ind].ModelBox.Cx,
                  g_config.ArrMod[ind].ModelBox.len1, g_config.ArrMod[ind].ModelBox.len2);
                    _HalconUtil.RepDarwBoxNoPhi(_wndID, ref g_config.By[ind].Cy, ref g_config.By[ind].Cx,
                  ref g_config.By[ind].len1, ref g_config.By[ind].len2, "yellow", MultiLanguage.GetString("调整掩膜框大小与区域"));
                }
                DispImage_Redraw();
                _HalconUtil.RepDarwBoxNoPhi(_wndID, ref g_config.ArrMod[ind].SeachBox.Cy, ref g_config.ArrMod[ind].SeachBox.Cx,
                  ref g_config.ArrMod[ind].SeachBox.len1, ref g_config.ArrMod[ind].SeachBox.len2, "red", MultiLanguage.GetString("调整查找框大小与区域"));
                DispImage_Redraw();
            }else
            {
                g_config.ArrMod[ind].ModelBox.Cy = _HalconUtil.IImgCenterY - _HalconUtil.ArrBoxH[0];
                g_config.ArrMod[ind].ModelBox.Cx = _HalconUtil.IImgCenterX - _HalconUtil.ArrBoxW[0];
                g_config.ArrMod[ind].ModelBox.len1 = _HalconUtil.IImgCenterY + _HalconUtil.ArrBoxH[0];
                g_config.ArrMod[ind].ModelBox.len2 = _HalconUtil.IImgCenterX + _HalconUtil.ArrBoxW[0];

                g_config.ArrMod[ind].SeachBox.Cy = _HalconUtil.IImgCenterY - _HalconUtil.ArrBoxH[1];
                g_config.ArrMod[ind].SeachBox.Cx = _HalconUtil.IImgCenterX - _HalconUtil.ArrBoxH[1];
                g_config.ArrMod[ind].SeachBox.len1 = _HalconUtil.IImgCenterY + _HalconUtil.ArrBoxH[1];
                g_config.ArrMod[ind].SeachBox.len2 = _HalconUtil.IImgCenterX + _HalconUtil.ArrBoxH[1];
            }
            if (ISelLenModUp == GlobConstData.ST_MODELICPOS)
            {
                if (g_config.IContrast == 0 && g_config.IMinContrast == 0)
                {
                    _HalconUtil.ModelSaveToFilePar(SourceImage, g_config.ArrMod[ind].ModelBox.Cy, g_config.ArrMod[ind].ModelBox.Cx, g_config.ArrMod[ind].ModelBox.len1, g_config.ArrMod[ind].ModelBox.len2,
                        g_config.DAngLimit, "auto", "auto", g_config.StrProductDir, ind, ref g_config.ArrModID[ind], ref g_config.ArrModID[ind + 1], ref g_config.ArrModID[ind + 2], ref g_config.ArrModID[ind + 3]);
                }
                else if (g_config.IContrast == 0 && g_config.IMinContrast != 0)
                {
                    _HalconUtil.ModelSaveToFilePar(SourceImage, g_config.ArrMod[ind].ModelBox.Cy, g_config.ArrMod[ind].ModelBox.Cx, g_config.ArrMod[ind].ModelBox.len1, g_config.ArrMod[ind].ModelBox.len2,
                        g_config.DAngLimit, "auto", g_config.IMinContrast, g_config.StrProductDir, ind, ref g_config.ArrModID[ind], ref g_config.ArrModID[ind + 1], ref g_config.ArrModID[ind + 2], ref g_config.ArrModID[ind + 3]);
                }
                else if (g_config.IContrast != 0 && g_config.IMinContrast == 0)
                {
                    _HalconUtil.ModelSaveToFilePar(SourceImage, g_config.ArrMod[ind].ModelBox.Cy, g_config.ArrMod[ind].ModelBox.Cx, g_config.ArrMod[ind].ModelBox.len1, g_config.ArrMod[ind].ModelBox.len2,
                        g_config.DAngLimit, g_config.IContrast,"auto", g_config.StrProductDir, ind, ref g_config.ArrModID[ind], ref g_config.ArrModID[ind + 1], ref g_config.ArrModID[ind + 2], ref g_config.ArrModID[ind + 3]);
                }
                else if (g_config.IContrast != 0 && g_config.IMinContrast != 0)
                {
                    _HalconUtil.ModelSaveToFilePar(SourceImage, g_config.ArrMod[ind].ModelBox.Cy, g_config.ArrMod[ind].ModelBox.Cx, g_config.ArrMod[ind].ModelBox.len1, g_config.ArrMod[ind].ModelBox.len2,
                        g_config.DAngLimit, g_config.IContrast, g_config.IMinContrast, g_config.StrProductDir, ind, ref g_config.ArrModID[ind], ref g_config.ArrModID[ind + 1], ref g_config.ArrModID[ind + 2], ref g_config.ArrModID[ind + 3]);
                }
            }
            else if (ISelLenModUp == GlobConstData.ST_MODELICPOS_NCC)
            {
                _HalconUtil.NCCModelSaveToFile(SourceImage, g_config.ArrMod[ind].ModelBox.Cy, g_config.ArrMod[ind].ModelBox.Cx, g_config.ArrMod[ind].ModelBox.len1, g_config.ArrMod[ind].ModelBox.len2,
                        g_config.By[ind].Cy, g_config.By[ind].Cx, g_config.By[ind].len1, g_config.By[ind].len2, g_config.DAngLimit, g_config.StrProductDir, ind, ref g_config.ArrModID[ind]);
            }
            else if (ISelLenModUp == GlobConstData.ST_MODELICSTATPOS || ind == GlobConstData.ST_MODELICSTATACC || ISelLenModUp == GlobConstData.ST_ModelBredeInPOS || ISelLenModUp == GlobConstData.ST_ModelBredeOutPOS || ISelLenModUp == GlobConstData.ST_ModelTrayPOS
                  /* || ISelLenModUp == GlobConstData.ST_ModelBredeInPOS || ISelLenModUp == GlobConstData.ST_ModelBredeOutPOS || ISelLenModUp == GlobConstData.ST_ModelTubeInPOS 
                     || ISelLenModUp == GlobConstData.ST_ModelTrayACC || ISelLenModUp == GlobConstData.ST_MODELPENACC || ISelLenModUp == GlobConstData.ST_ModelBredeInACC
                     || ISelLenModUp == GlobConstData.ST_ModelBredeOutACC || ISelLenModUp == GlobConstData.ST_ModelTubeInACC*/)
            {
                _HalconUtil.NCCModelSaveToFile(SourceImage, g_config.ArrMod[ind].ModelBox.Cy, g_config.ArrMod[ind].ModelBox.Cx, g_config.ArrMod[ind].ModelBox.len1, g_config.ArrMod[ind].ModelBox.len2,
                        g_config.By[ind].Cy, g_config.By[ind].Cx, g_config.By[ind].len1, g_config.By[ind].len2, Angle, g_config.StrProductDir, ind, ref g_config.ArrModID[ind]);
            }
            else
            {
                _HalconUtil.ModelSaveToFile(SourceImage, g_config.ArrMod[ind].ModelBox.Cy, g_config.ArrMod[ind].ModelBox.Cx,
                    g_config.ArrMod[ind].ModelBox.len1, g_config.ArrMod[ind].ModelBox.len2, g_config.StrProductDir, ind, out g_config.ArrModID[ind]);
            }
            //如果模板创建失败直接返回
            if (g_config.ArrModID[ind].IsNull()) return;

            g_config.SaveModelBox(ind, g_config.StrProductIni);
            HTuple resYC, resXC, resAng, resScore;

            bool retBool;
            if (ISelLenModUp == GlobConstData.ST_MODELICPOS)
            {
                retBool = _HalconUtil.FindModelPar(true, _wndID, SourceImage, g_config.ArrModID[ind],
                    g_config.ArrMod[ind].SeachBox.Cy, g_config.ArrMod[ind].SeachBox.Cx, g_config.ArrMod[ind].SeachBox.Phi, g_config.ArrMod[ind].SeachBox.len1, g_config.ArrMod[ind].SeachBox.len2,
                    g_config.DScore, g_config.DAngLimit, g_config.DMaxOverlap, g_config.DGreediness, 1, out resYC, out resXC, out resAng, out resScore);
            }
            else if (ISelLenModUp == GlobConstData.ST_MODELICPOS_NCC)
            {
                retBool = _HalconUtil.NCCModelPar(true, _wndID, SourceImage, g_config.ArrModID[ind],
                    g_config.ArrMod[ind].SeachBox.Cy, g_config.ArrMod[ind].SeachBox.Cx, g_config.ArrMod[ind].SeachBox.Phi, g_config.ArrMod[ind].SeachBox.len1, g_config.ArrMod[ind].SeachBox.len2,
                    g_config.DScore, g_config.DAngLimit, g_config.DMaxOverlap, 1, out resYC, out resXC, out resAng, out resScore);
            }
            else if (ISelLenModUp == GlobConstData.ST_MODELICSTATPOS || ind == GlobConstData.ST_MODELICSTATACC || ISelLenModUp == GlobConstData.ST_ModelBredeInPOS || ISelLenModUp == GlobConstData.ST_ModelBredeOutPOS
                     || ISelLenModUp == GlobConstData.ST_ModelTrayPOS
                     /*   || ISelLenModUp == GlobConstData.ST_ModelBredeInPOS || ISelLenModUp == GlobConstData.ST_ModelBredeOutPOS || ISelLenModUp == GlobConstData.ST_ModelTubeInPOS 
                          || ind == GlobConstData.ST_ModelTrayACC || ISelLenModUp == GlobConstData.ST_MODELPENACC || ind == GlobConstData.ST_ModelBredeInACC 
                          || ind == GlobConstData.ST_ModelBredeOutACC || ind == GlobConstData.ST_ModelTubeInACC*/)
            {
                retBool = _HalconUtil.NCCModelPar(true, _wndID, SourceImage, g_config.ArrModID[ind],
                    g_config.ArrMod[ind].SeachBox.Cy, g_config.ArrMod[ind].SeachBox.Cx, g_config.ArrMod[ind].SeachBox.Phi, g_config.ArrMod[ind].SeachBox.len1, g_config.ArrMod[ind].SeachBox.len2,
                    0.4, Angle, 0.2, 1, out resYC, out resXC, out resAng, out resScore);
            }
            else
            {
                retBool = _HalconUtil.FindModel(true, _wndID, SourceImage, g_config.ArrModID[ind],
                    g_config.ArrMod[ind].SeachBox.Cy, g_config.ArrMod[ind].SeachBox.Cx, g_config.ArrMod[ind].SeachBox.Phi, g_config.ArrMod[ind].SeachBox.len1, g_config.ArrMod[ind].SeachBox.len2,
                    0.5, 1, out resYC, out resXC,out resAng, out resScore);
            }
            if (retBool) DispModelImage(sel, ind);

            if(retBool)
            {
                if (ISelLenModUp == GlobConstData.ST_MODELICPOS)
                {
                    g_config.DModelCy = resYC.D;
                    g_config.DModelCx = resXC.D;
                    g_config.DModelCr = resAng.D;
                }
                else if (ISelLenModUp == GlobConstData.ST_MODELICPOS_NCC)
                {
                    g_config.DModelCy2 = resYC.D;
                    g_config.DModelCx2 = resXC.D;
                    g_config.DModelCr2 = resAng.D;
                }
                
                g_config.SaveModelCxy(); 
            }
        }
        private void ImgProcess_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender == this.btnW2Add)
            {
                if (_HalconUtil.ArrBoxW[0] >= 20 && _HalconUtil.ArrBoxW[0] <= 1000)
                {
                    _HalconUtil.ArrBoxW[0]++;
                    if (imgProsRadio1.Checked) timer1.Interval = 5;
                    else if (imgProsRadio2.Checked) timer1.Interval = 20;
                    else if (imgProsRadio3.Checked) timer1.Interval = 40;
                }
                else
                {
                    _HalconUtil.ArrBoxW[0] = 20;
                }
            }
            else if (sender == this.btnW2Cut)
            {
                if (_HalconUtil.ArrBoxW[0] >= 20 && _HalconUtil.ArrBoxW[0] <= 1000)
                {
                    _HalconUtil.ArrBoxW[0]--;
                    if (imgProsRadio1.Checked) timer1.Interval = 5;
                    else if (imgProsRadio2.Checked) timer1.Interval = 20;
                    else if (imgProsRadio3.Checked) timer1.Interval = 40;
                }
                else
                {
                    _HalconUtil.ArrBoxW[0] = 20;
                }
            }
            else if (sender == this.btnH2Add)
            {
                if (_HalconUtil.ArrBoxH[0] >= 20 && _HalconUtil.ArrBoxH[0] <= 1000)
                {
                    _HalconUtil.ArrBoxH[0]++;
                    if (imgProsRadio1.Checked) timer1.Interval = 5;
                    else if (imgProsRadio2.Checked) timer1.Interval = 20;
                    else if (imgProsRadio3.Checked) timer1.Interval = 40;

                }
                else
                {
                    _HalconUtil.ArrBoxH[0] = 20;
                }
            }
            else if (sender == this.btnH2Cut)
            {
                if (_HalconUtil.ArrBoxH[0] >= 20 && _HalconUtil.ArrBoxH[0] <= 1000)
                {
                    _HalconUtil.ArrBoxH[0]--;
                    if (imgProsRadio1.Checked) timer1.Interval = 5;
                    else if (imgProsRadio2.Checked) timer1.Interval = 20;
                    else if (imgProsRadio3.Checked) timer1.Interval = 40;

                }
                else
                {
                    _HalconUtil.ArrBoxH[0] = 20;
                }
            }
            else if (sender == this.btnWAdd)
            {
                if (_HalconUtil.ArrBoxW[1] >= 20 && _HalconUtil.ArrBoxW[0] <= 1000)
                {
                    _HalconUtil.ArrBoxW[1]++;
                    if (imgProsRadio1.Checked) timer1.Interval = 5;
                    else if (imgProsRadio2.Checked) timer1.Interval = 20;
                    else if (imgProsRadio3.Checked) timer1.Interval = 40;

                }
                else
                {
                    _HalconUtil.ArrBoxW[1] = 20;
                }
            }
            else if (sender == this.btnWCut)
            {
                if (_HalconUtil.ArrBoxW[1] >= 20 && _HalconUtil.ArrBoxW[1] <= 1000)
                {
                    _HalconUtil.ArrBoxW[1]--;
                    if (imgProsRadio1.Checked) timer1.Interval = 5;
                    else if (imgProsRadio2.Checked) timer1.Interval = 20;
                    else if (imgProsRadio3.Checked) timer1.Interval = 40;

                }
                else
                {
                    _HalconUtil.ArrBoxW[1] = 20;
                }
            }
            else if (sender == this.btnHAdd)
            {
                if (_HalconUtil.ArrBoxH[1] >= 20 && _HalconUtil.ArrBoxH[1] <= 1000)
                {
                    _HalconUtil.ArrBoxH[1]++;
                    if (imgProsRadio1.Checked) timer1.Interval = 5;
                    else if (imgProsRadio2.Checked) timer1.Interval = 20;
                    else if (imgProsRadio3.Checked) timer1.Interval = 40;
                }
                else
                {
                    _HalconUtil.ArrBoxH[1] = 20;
                }
            }
            else if (sender == this.btnHCut)
            {
                if (_HalconUtil.ArrBoxH[1] >= 20 && _HalconUtil.ArrBoxH[1] <= 1000)
                {
                    _HalconUtil.ArrBoxH[1]--;
                    if (imgProsRadio1.Checked) timer1.Interval = 5;
                    else if (imgProsRadio2.Checked) timer1.Interval = 20;
                    else if (imgProsRadio3.Checked) timer1.Interval = 40;

                }
                else
                {
                    _HalconUtil.ArrBoxH[1] = 20;
                }
            }
            else if (sender == this.btnW3Add)
            {
                if (_HalconUtil.IBoxC >= 20 && _HalconUtil.ArrBoxW[0] <= 1000)
                {
                    _HalconUtil.IBoxC++;
                    if (imgProsRadio1.Checked) timer1.Interval = 5;
                    else if (imgProsRadio2.Checked) timer1.Interval = 20;
                    else if (imgProsRadio3.Checked) timer1.Interval = 40;
                }
                else
                {
                    _HalconUtil.IBoxC = 20;
                }
            }
            else if (sender == this.btnW3Cut)
            {
                if (_HalconUtil.IBoxC >= 20 && _HalconUtil.ArrBoxW[0] <= 1000)
                {
                    _HalconUtil.IBoxC--;
                    if (imgProsRadio1.Checked) timer1.Interval = 5;
                    else if (imgProsRadio2.Checked) timer1.Interval = 20;
                    else if (imgProsRadio3.Checked) timer1.Interval = 40;
                }
                else
                {
                    _HalconUtil.IBoxC = 20;
                }
            }
            timer1.Start();
            
        }

        private void ImgProcess_MouseUp(object sender, MouseEventArgs e)
        {
            g_config.ArrLampBox[ISelBoxInd].IMW = _HalconUtil.ArrBoxW[0];
            g_config.ArrLampBox[ISelBoxInd].IMH = _HalconUtil.ArrBoxH[0];
            g_config.ArrLampBox[ISelBoxInd].ISW = _HalconUtil.ArrBoxW[1];
            g_config.ArrLampBox[ISelBoxInd].ISH = _HalconUtil.ArrBoxH[1];
            g_config.ArrLampBox[ISelBoxInd].IC = _HalconUtil.IBoxC;
            g_config.SaveLightVal(ISelBoxInd);
        }

        public void MDown(JogPrm jogPrm, LimitHomePrm homePrm, AxisSts axisSts, int dir)
        {
            if (Auto_Flag.SystemBusy)
            {
                return;
            }
            if (proccedRBtn.Checked)
            {
                Gts.GT_ClrSts(jogPrm.cardPrm.cardNum, jogPrm.index, 1);//清除轴报警
                if (speedHigthRBtn.Checked)
                {
                    plc.JOG(Config.CardType, jogPrm, jogPrm.velHigh * dir);
                }
                else
                {
                    plc.JOG(Config.CardType, jogPrm, jogPrm.velLow * dir);
                }
            }
            else if (homeRBtn.Checked)
            {
                if (!axisSts.isHomeBusy)
                {
                    axis.ORG_Function(homePrm, axisSts);
                }
            }
            else
            {
                double offset = 0.02d;
                Gts.GT_GetPrfVel(jogPrm.cardPrm.cardNum, jogPrm.index, out double vel, 1, out uint plcok);
                if (vel != 0)
                {
                    return;
                }
                Gts.GT_GetPrfPos(jogPrm.cardPrm.cardNum, jogPrm.index, out double prfpos, 1, out plcok);

                if (offset1RBtn.Checked)
                {
                    offset = 0.02d * dir;
                }
                else if (offset2RBtn.Checked)
                {
                    offset = 0.1d * dir;
                }
                TrapPrm trapPrm = new TrapPrm
                {
                    cardPrm = jogPrm.cardPrm,
                    index = jogPrm.index,
                    pulFactor = jogPrm.pulFactor,
                    speed = jogPrm.velHigh,
                    setPosPul = (int)(prfpos + offset / jogPrm.pulFactor),
                    acc = jogPrm.acc,
                    dec = jogPrm.dec,
                    smoothTime = homePrm.smoothTime,
                    velStart = 0 
                };
                plc.UpdateTrap(Config.CardType, trapPrm);
            }
        }

        public void MUp(JogPrm jogPrm)
        {
            if (proccedRBtn.Checked)
            {
                if (Config.CardType == 0)
                {
                    Gts.GT_Stop(jogPrm.cardPrm.cardNum, 1 << (jogPrm.index - 1), 0);
                }
                else
                {
                    lhmtc.LH_Stop((short)(1 << (jogPrm.index - 1)), 0, jogPrm.cardPrm.cardNum);
                }
            }
        }

        private bool Axis_XY_Moveable_Check()
        {
            if (!Axis.Axis_Z_IsHome_Check())
            {
                BAR._ToolTipDlg.WriteToolTipStr("Z轴未回原点");
                BAR.ShowToolTipWnd(false);
                return false;
            }
            if (((speedHigthRBtn.Checked && proccedRBtn.Checked) || homeRBtn.Checked) && !Axis.Axis_Z_IsSafe_Check())
            {
                BAR._ToolTipDlg.WriteToolTipStr("Z轴未在安全检测高度");
                BAR.ShowToolTipWnd(false);
                return false;
            }
            if (((speedHigthRBtn.Checked && proccedRBtn.Checked) || homeRBtn.Checked) && !Axis.PenHome_Check())
            {
                BAR._ToolTipDlg.WriteToolTipStr("吸笔未在安全检测原位");
                BAR.ShowToolTipWnd(false);
                return false;
            }
            return true;
        }

        private void Axis_X_JOG正_Up_MouseDown(object sender, MouseEventArgs e)
        {
            if (Axis_XY_Moveable_Check())
            {
                MDown(Axis.jogPrm_X, Axis.homePrm_X, Axis.axisSts_X, 1);
            }
        }

        private void Axis_X_JOG正_Up_MouseUp(object sender, MouseEventArgs e)
        {
            MUp(Axis.jogPrm_X);
        }

        private void Axis_X_JOG负_Up_MouseDown(object sender, MouseEventArgs e)
        {
            if (Axis_XY_Moveable_Check())
            {
                MDown(Axis.jogPrm_X, Axis.homePrm_X, Axis.axisSts_X, -1);
            }
        }

        private void Axis_X_JOG负_Up_MouseUp(object sender, MouseEventArgs e)
        {
            MUp(Axis.jogPrm_X);
        }

        private void Axis_Y_JOG正_Up_MouseDown(object sender, MouseEventArgs e)
        {
            if (Axis_XY_Moveable_Check())
            {
                MDown(Axis.jogPrm_Y, Axis.homePrm_Y, Axis.axisSts_Y, 1);
            }
        }

        private void Axis_Y_JOG正_Up_MouseUp(object sender, MouseEventArgs e)
        {
            MUp(Axis.jogPrm_Y);
        }

        private void Axis_Y_JOG负_Up_MouseDown(object sender, MouseEventArgs e)
        {
            if (Axis_XY_Moveable_Check())
            {
                MDown(Axis.jogPrm_Y, Axis.homePrm_Y, Axis.axisSts_Y, -1);
            }
        }

        private void Axis_Y_JOG负_Up_MouseUp(object sender, MouseEventArgs e)
        {
            MUp(Axis.jogPrm_Y);
        }

        private void Axis_Z1_JOG正_Up_MouseDown(object sender, MouseEventArgs e)
        {
            MDown(Axis.jogPrm_Z[0], Axis.homePrm_Z[0], Axis.axisSts_Z[0], 1);
        }

        private void Axis_Z1_JOG正_Up_MouseUp(object sender, MouseEventArgs e)
        {
            MUp(Axis.jogPrm_Z[0]);
        }

        private void Axis_Z1_JOG负_Up_MouseDown(object sender, MouseEventArgs e)
        {
            MDown(Axis.jogPrm_Z[0], Axis.homePrm_Z[0], Axis.axisSts_Z[0], -1);
        }

        private void Axis_Z1_JOG负_Up_MouseUp(object sender, MouseEventArgs e)
        {
            MUp(Axis.jogPrm_Z[0]);
        }

        private void Axis_Z2_JOG正_Up_MouseDown(object sender, MouseEventArgs e)
        {
            MDown(Axis.jogPrm_Z[1], Axis.homePrm_Z[1], Axis.axisSts_Z[1], 1);
        }

        private void Axis_Z2_JOG正_Up_MouseUp(object sender, MouseEventArgs e)
        {
            MUp(Axis.jogPrm_Z[1]);
        }

        private void Axis_Z2_JOG负_Up_MouseDown(object sender, MouseEventArgs e)
        {
            MDown(Axis.jogPrm_Z[1], Axis.homePrm_Z[1], Axis.axisSts_Z[1], -1);
        }

        private void Axis_Z2_JOG负_Up_MouseUp(object sender, MouseEventArgs e)
        {
            MUp(Axis.jogPrm_Z[1]);
        }

        private void Axis_Z3_JOG正_Up_MouseDown(object sender, MouseEventArgs e)
        {
            MDown(Axis.jogPrm_Z[2], Axis.homePrm_Z[2], Axis.axisSts_Z[2], 1);
        }

        private void Axis_Z3_JOG正_Up_MouseUp(object sender, MouseEventArgs e)
        {
            MUp(Axis.jogPrm_Z[2]);
        }

        private void Axis_Z3_JOG负_Up_MouseDown(object sender, MouseEventArgs e)
        {
            MDown(Axis.jogPrm_Z[2], Axis.homePrm_Z[2], Axis.axisSts_Z[2], -1);
        }

        private void Axis_Z3_JOG负_Up_MouseUp(object sender, MouseEventArgs e)
        {
            MUp(Axis.jogPrm_Z[2]);
        }

        private void Axis_Z4_JOG正_Up_MouseDown(object sender, MouseEventArgs e)
        {
            MDown(Axis.jogPrm_Z[3], Axis.homePrm_Z[3], Axis.axisSts_Z[3], 1);
        }

        private void Axis_Z4_JOG正_Up_MouseUp(object sender, MouseEventArgs e)
        {
            MUp(Axis.jogPrm_Z[3]);
        }

        private void Axis_Z4_JOG负_Up_MouseDown(object sender, MouseEventArgs e)
        {
            MDown(Axis.jogPrm_Z[3], Axis.homePrm_Z[3], Axis.axisSts_Z[3], -1);
        }

        private void Axis_Z4_JOG负_Up_MouseUp(object sender, MouseEventArgs e)
        {
            MUp(Axis.jogPrm_Z[3]);
        }

        public void Manual_Rotate(int penNum, bool flag = true)
        {
            double angleVal = 0d;
            if (flag)
            {
                if (angle1RBtn.Checked)
                {
                    angleVal = -90d;
                }
                else if (angle2RBtn.Checked)
                {
                    angleVal = 90;
                }
                else if (angle3RBtn.Checked)
                {
                    angleVal = 180;
                }
            }

            if (!Axis.Pen[penNum].Rotate.Busy)
            {
                if (angleVal != Axis.trapPrm_C[penNum].getPos)
                {
                    UserTask.RotateTrigge(true, penNum, angleVal);
                }
            }
        }

        private void Axis_C1_JOG正_Up_Click(object sender, EventArgs e)
        {
            Manual_Rotate(0);
        }

        private void Axis_C1_JOG负_Up_Click(object sender, EventArgs e)
        {
            Manual_Rotate(0, false);
        }

        private void Axis_C2_JOG正_Up_Click(object sender, EventArgs e)
        {
            Manual_Rotate(1);
        }

        private void Axis_C2_JOG负_Up_Click(object sender, EventArgs e)
        {
            Manual_Rotate(1, false);
        }

        private void Axis_C3_JOG正_Up_Click(object sender, EventArgs e)
        {
            Manual_Rotate(2);
        }

        private void Axis_C3_JOG负_Up_Click(object sender, EventArgs e)
        {
            Manual_Rotate(2, false);
        }

        private void Axis_C4_JOG正_Up_Click(object sender, EventArgs e)
        {
            Manual_Rotate(3);
        }

        private void Axis_C4_JOG负_Up_Click(object sender, EventArgs e)
        {
            Manual_Rotate(3, false);
        }

        private void ConfigInitWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isModelLearn)
            {
                MessageBox.Show("图像调整框未取消掉，请取消后再操作");
                e.Cancel = true;
                return;
            }
            
            drawCrossBtn_Click(null, null);
            timer1.Stop();
            g_act.ISelectWnd = GlobConstData.SELECT_MAIN_WND;

            //相机回调取消侦听
            __UnRegistCameraHandle();
            //关闭上下光源
            g_act.SendCmd(GlobConstData.ST_LIGHTUP, 0);
            g_act.SendCmd(GlobConstData.ST_LIGHTDOWN, 0);
            HDevWindowStack.Pop();
            e.Cancel = true;
            this.Hide();
            main.Activate();
            g_act.CCDSnap(GlobConstData.ST_CCDDOWN);
        }

        private void BtnHome_Click(object sender, EventArgs e)
        {
            if (!Auto_Flag.SystemBusy)
            {
                Axis.Home_Start();
            }
        }
        private void CapModeBtn_Click(object sender, EventArgs e)
        {
            if (!isCapModeShow)
            {
                this.capModPanel.Visible = true;
                isCapModeShow = true;
            }
            else
            {
                this.capModPanel.Visible = false;
                isCapModeShow = false;
            }
        }

        private void ChangeCamBtn_Click(object sender, EventArgs e)
        {
            if (!isChooseCamShow)
            {
                this.camPanel.Visible = true;
                isChooseCamShow = true;
            }
            else
            {
                this.camPanel.Visible = false;
                isChooseCamShow = false;
            }
        }

        private void UpCamBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (upCamBtn.Checked && g_act.ISelectCam != GlobConstData.ST_CCDUP)
            {
                g_act.CCDCap(GlobConstData.ST_CCDUP);
            }
        }

        private void DownCamBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (downCamBtn.Checked && g_act.ISelectCam != GlobConstData.ST_CCDDOWN)
            {
                g_act.CCDCap(GlobConstData.ST_CCDDOWN);
            }
        }
        //图像还原
        private void ImgOriginBtn_Click(object sender, EventArgs e)
        {
            _ImgOperater.IsImgMove = false;
            _ImgOperater.IsZoomImg = false;
            isOperateImg = false;
            _HalconUtil.DispImage(_ImgOperater, _wndID, SourceImage, isOperateImg, ref _IsHaveImg);
            Disp_Cross();
        }
        //图像放大
        private void ImgLargerBtn_Click(object sender, EventArgs e)
        {
            if ( _ImgOperater.DZoomWndFactor < 0.02 || _ImgOperater.DZoomWndFactor > 11 )return;
            isOperateImg = true;    //图像移动缩放标志
            _ImgOperater.ZoomImage(_HalconUtil.IImgCenterX, _HalconUtil.IImgCenterY, 0.5);//图像缩放函数m_Image.m_ZoomImage_X m_Image.m_ZoomImage_Y m_Image.m_ZoomImage_Scale
            _HalconUtil.DispImage(_ImgOperater, _wndID, SourceImage, isOperateImg, ref _IsHaveImg);
            Disp_Cross();
        }
        //图像缩小
        private void ImgReduceBtn_Click(object sender, EventArgs e)
        {
            if (_ImgOperater.DZoomWndFactor < 0.02 || _ImgOperater.DZoomWndFactor > 11) return;
            isOperateImg = true;    //图像移动缩放标志
            _ImgOperater.ZoomImage(_HalconUtil.IImgCenterX, _HalconUtil.IImgCenterY, 2);//图像缩放函数m_Image.m_ZoomImage_X m_Image.m_ZoomImage_Y m_Image.m_ZoomImage_Scale
            _HalconUtil.DispImage(_ImgOperater, _wndID, SourceImage, isOperateImg, ref _IsHaveImg);
            Disp_Cross();
        }

        private void SnapBtn_Click(object sender, EventArgs e)
        {
            if (this.upCamBtn.Checked) g_act.CCDSnap(GlobConstData.ST_CCDUP);
            else g_act.CCDSnap(GlobConstData.ST_CCDDOWN);
        }

        private void CapBtn_Click(object sender, EventArgs e)
        {
            if (this.upCamBtn.Checked) g_act.CCDCap(GlobConstData.ST_CCDUP, true);
            else g_act.CCDCap(GlobConstData.ST_CCDDOWN, true);
        }
        private void AdjustLightBtn_Click(object sender, EventArgs e)
        {
            LightCtlWnd wnd = new LightCtlWnd(this);

            wnd.ShowDialog();
        }
        private void openImgBtn_Click(object sender, EventArgs e)
        {
            string fileName;
            byte[] arr = new byte[1024];
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog.FileName;
                HObject sImg;
                _HalconUtil.LoadImgAndDisp(_wndID, out sImg, fileName);
                sImg?.Dispose();
            }
        }

        /// <summary>
        /// 显示十字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void drawCrossBtn_Click(object sender, EventArgs e)
        {
            g_act.DispLineType = GlobConstData.DISPLAYLINETYPE_CROSS;
        }

        /// <summary>
        /// 显示刻度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void drawScaleBtn_Click(object sender, EventArgs e)
        {
             g_act.DispLineType = GlobConstData.DISPLAYLINETYPE_SCALE;
        }

        /// <summary>
        /// 显示回字框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void drawBackRectBtn_Click(object sender, EventArgs e)
        {
            g_act.DispLineType = GlobConstData.DISPLAYLINETYPE_BACKRECT;
        }

        private void tabConfigInitWnd_SelectedIndexChanged(object sender, EventArgs e)
        {
            g_act.TriggerMode();
            int ind = tabConfigInitWnd.SelectedIndex;
            switch(ind)
            {
                case 0:
                    pnlCamAccAndPenDis.CheckRbtn();
                    break;
                case 1:
                    upCamBtn.Checked = true;
                    g_act.CCDCap(GlobConstData.ST_CCDUP, true);
                    IIndexModel = ISelBoxInd = GlobConstData.ST_MODELICSTATPOS;
                    __SetBox(ISelBoxInd);
                    g_act.SendCmd(GlobConstData.ST_LIGHTUP, g_config.ArrLampBox[ISelBoxInd].ILightUp);
                    break;
                case 2:
                    upCamBtn.Checked = true;
                    g_act.CCDCap(GlobConstData.ST_CCDUP, true);
                    IIndexModel = ISelBoxInd = GlobConstData.ST_ModelTrayACC;
                    __SetBox(ISelBoxInd);
                    g_act.SendCmd(GlobConstData.ST_LIGHTUP, g_config.ArrLampBox[ISelBoxInd].ILightUp);
                    break;
                case 3:
                    downCamBtn.Checked = true;
                    g_act.CCDCap(GlobConstData.ST_CCDDOWN, true);
                    pnlICPos.Model_Check();
                    __SetBox(ISelBoxInd);
                    g_act.SendCmd(GlobConstData.ST_LIGHTDOWN, g_config.ArrLampBox[ISelBoxInd].ILightDown);
                    break;
                case 4:
                    upCamBtn.Checked = true;
                    pnlAutoRevisePos.CheckRbtn();
                    break;
            }
        }
    }
}
