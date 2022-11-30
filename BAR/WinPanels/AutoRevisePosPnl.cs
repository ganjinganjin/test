using BAR.Commonlib;
using BAR.Commonlib.Utils;
using BAR.CommonLib_v1._0;
using BAR.Windows;
using CCWin.SkinControl;
using CCWin.Win32.Const;
using HalconDotNet;
using PLC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PLC.Gts;

namespace BAR.ControlPanels
{
    public partial class AutoRevisePosPnl : UserControl
    {
        Config g_config = Config.GetInstance();
        Act g_act = Act.GetInstance();
        HalconImgUtil _HalconUtil = HalconImgUtil.GetInstance();
        public Axis axis = Axis.GetInstance();
        PLC1 plc = new PLC1();

        private SkinRadioButton[] rBtnSelect = new SkinRadioButton[5];
        private Label[] labSelect = new Label[5];
        private NumericUpDown[] NudX, NudY;
        private SkinRadioButton[] RbtnTray;
        SkinRadioButton SelectedRbtn, SelRbtnTray;
        HTuple WindowHandle1;
        ConfigInitWnd _ConInitWnd;
        

        private int SelectedRbtn_Ind, SelRbtnTray_Ind;

        public AutoRevisePosPnl()
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();
            _InitializeComponent();
            SelectedRbtn = this.Rbtn0;
            SelRbtnTray = RbtnTray0;
            UserEvent.productChange += new UserEvent.ProductChange(UpdateMarkUI);
            TeachAction.RevisePOS = new TeachAction.RevisePOSDelegate(MovToPOS);
        }

        public AutoRevisePosPnl(ConfigInitWnd conInitWnd) : this()
        {
            this._ConInitWnd = conInitWnd;
        }

        private void _InitializeComponent()
        {
            NudX = new NumericUpDown[3];
            NudY = new NumericUpDown[3];
            RbtnTray = new SkinRadioButton[3];

            String ctlName;
            Control[] tagAry;
            //焦距
            for (int i = 0; i < 5; i++)
            {
                ctlName = "Lab" + i;
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    labSelect[i] = (tagAry.First() as Label);
                }

                ctlName = "Rbtn" + i;
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    rBtnSelect[i] = (tagAry.First() as SkinRadioButton);
                    rBtnSelect[i].MouseUp += new MouseEventHandler(Rbtn_MouseUp);
                }
            }

            //Mark点参数
            for (int i = 0; i < NudX.Length; i++)
            {
                ctlName = "NudX_" + i;
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    NudX[i] = (tagAry.First() as NumericUpDown);
                }

                ctlName = "NudY_" + i;
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    NudY[i] = (tagAry.First() as NumericUpDown);
                }

                ctlName = "RbtnTray" + i;
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    RbtnTray[i] = (tagAry.First() as SkinRadioButton);
                    RbtnTray[i].MouseUp += new MouseEventHandler(RbtnTray_MouseUp);
                }
            }
        }

        private void AutoRevisePosPnl_Load(object sender, EventArgs e)
        {
            __InitUI();
        }

        private void __InitUI()
        {
            __InitComboBtn();
            UpdateUI();
            UpdateMarkUI();
        }

        private void UpdateUI()
        {
            labSelect[0].Text = Axis.ZoomLens_S.Socket.ToString();
            labSelect[1].Text = Axis.ZoomLens_S.Tray.ToString();
            labSelect[2].Text = Axis.ZoomLens_S.BredeIn.ToString();
            labSelect[3].Text = Axis.ZoomLens_S.BredeOut.ToString();
            labSelect[4].Text = Axis.ZoomLens_S.TubeIn.ToString();

            //for (int i = 0; i < NudX.Length; i++)
            //{
            //    NudX[i].Value = Convert.ToDecimal(g_config.MarkPos[g_config.TrayModel].Tray[i].X);
            //    NudY[i].Value = Convert.ToDecimal(g_config.MarkPos[g_config.TrayModel].Tray[i].Y);
            //}
        }

        public void UpdateMarkUI()
        {
            productLab.Text = "当前型号:" + g_config.StrCurMark;
            LabOffset_X.Text = MarkOffset.Tray_X.ToString();
            LabOffset_Y.Text = MarkOffset.Tray_Y.ToString();
            LabCol.Text = MarkOffset.Tray_Col.ToString();
            LabRol.Text = MarkOffset.Tray_Rol.ToString();
            LabColdis.Text = MarkOffset.Tray_Coldis.ToString();
            LabRoldis.Text = MarkOffset.Tray_Roldis.ToString();
        }

        private void __InitComboBtn()
        {
            //烧写座
            for (int i = 0; i < UserConfig.AllScketC; i++)
            {
                String str = MultiLanguage.DefaultLanguage == GlobConstData.ST_English? "Seat" + (i / UserConfig.ScketUnitC + 1) + "_" + (i % UserConfig.ScketUnitC + 1) : "烧写座" + (i / UserConfig.ScketUnitC + 1) + "_" + (i % UserConfig.ScketUnitC + 1);
                this.seatCBox.Items.Add(str);
            }
            seatCBox.SelectedIndex = 0;

            //Mark点参数
            CBoxMark.Items.Add("坐标");
            CBoxMark.Items.Add("修正量");
            CBoxMark.SelectedIndex = 0;
        }

        private void SetReadOnly(bool flag)
        {
            for (int i = 0; i < NudX.Length; i++)
            {
                NudX[i].ReadOnly = flag;
                NudY[i].ReadOnly = flag;
            }
        }

        public void DispModelImage1(int modelIndex)
        {
            if (WindowHandle1 != null) HOperatorSet.CloseWindow(WindowHandle1);
            HOperatorSet.OpenWindow(0, 0, modelPicBox1.Width, modelPicBox1.Height, modelPicBox1.Handle, "visible", "", out WindowHandle1);
            String path = g_config.StrProductDir + "\\set_" + modelIndex + ".jpg";
            if (File.Exists(path))//判断文件是否存在
            {
                HObject img;
                HOperatorSet.ClearWindow(WindowHandle1);
                _HalconUtil.LoadImgAndDisp(WindowHandle1, out img, path);
                img?.Dispose();
            }
        }

        private void Rbtn_MouseUp(object sender, MouseEventArgs e)
        {
            SelectedRbtn = sender as SkinRadioButton;
            SelectedRbtn_Ind = Convert.ToInt32(SelectedRbtn.Tag);
            CheckRbtn();
        }

        private void RbtnTray_MouseUp(object sender, MouseEventArgs e)
        {
            SelRbtnTray = sender as SkinRadioButton;
            SelRbtnTray_Ind = Convert.ToInt32(SelRbtnTray.Tag);
        }

        public void CheckRbtn()
        {
            if (SelectedRbtn == this.Rbtn0)
            {
                g_act.CCDCap(GlobConstData.ST_CCDUP);
                _ConInitWnd.IIndexModel = _ConInitWnd.ISelBoxInd = GlobConstData.ST_MODELICSTATPOS;
                g_act.SendCmd(GlobConstData.ST_LIGHTUP, g_config.ArrLampBox[_ConInitWnd.ISelBoxInd].ILightUp);
            }
            else if (SelectedRbtn == this.Rbtn1)
            {
                g_act.CCDCap(GlobConstData.ST_CCDUP);
                _ConInitWnd.IIndexModel = _ConInitWnd.ISelBoxInd = GlobConstData.ST_ModelTrayPOS;
                g_act.SendCmd(GlobConstData.ST_LIGHTUP, g_config.ArrLampBox[_ConInitWnd.ISelBoxInd].ILightUp);
            }
            else if (SelectedRbtn == this.Rbtn2)
            {
                g_act.CCDCap(GlobConstData.ST_CCDUP);
                _ConInitWnd.IIndexModel = _ConInitWnd.ISelBoxInd = GlobConstData.ST_ModelBredeInPOS;
                g_act.SendCmd(GlobConstData.ST_LIGHTUP, g_config.ArrLampBox[_ConInitWnd.ISelBoxInd].ILightUp);
            }
            else if (SelectedRbtn == this.Rbtn3)
            {
                g_act.CCDCap(GlobConstData.ST_CCDUP);
                _ConInitWnd.IIndexModel = _ConInitWnd.ISelBoxInd = GlobConstData.ST_ModelBredeOutPOS;
                g_act.SendCmd(GlobConstData.ST_LIGHTUP, g_config.ArrLampBox[_ConInitWnd.ISelBoxInd].ILightUp);
            }
            else if (SelectedRbtn == this.Rbtn4)
            {
                g_act.CCDCap(GlobConstData.ST_CCDUP);
                _ConInitWnd.IIndexModel = _ConInitWnd.ISelBoxInd = GlobConstData.ST_ModelTubeInPOS;
                g_act.SendCmd(GlobConstData.ST_LIGHTUP, g_config.ArrLampBox[_ConInitWnd.ISelBoxInd].ILightUp);
            }
            DispModelImage1(_ConInitWnd.IIndexModel);
            _ConInitWnd.__SetBox(_ConInitWnd.ISelBoxInd);
        }

        private void BtnRotate_Click(object sender, EventArgs e)
        {
            if (Config.ZoomLens != 1)
            {
                return;
            }
            if (sender == this.BtnRotateFWD)
            {
                ZoomLens.SetPos = -910;
            }
            else if (sender == this.BtnRotateREV)
            {
                ZoomLens.SetPos = 910;
            }
            ZoomLens.MODBUS_DST();
        }

        private void savePosBtn_Click(object sender, EventArgs e)
        {
            if (Config.ZoomLens == 1)
            {
                if (Axis.ZoomLens_S.ReadStatus_Busy)
                {
                    return;
                }
                ZoomLens.MODBUS_ReadStatus();
                Task.Run(() =>
                {
                    do
                    {
                        Thread.Sleep(5);
                    } while (Axis.ZoomLens_S.ReadStatus_Busy);

                    int ind = Convert.ToInt32(SelectedRbtn_Ind);
                    int temp = 0;
                    string strMsg = null;
                    if (Axis.ZoomLens_S.ReadStatus_Online)
                    {
                        if (ind == 0)
                        {
                            Axis.ZoomLens_S.Socket = ZoomLens.OriginPos - ZoomLens.NowPos;
                            strMsg = "保存烧录座镜头焦距成功";
                            temp = 0;
                        }
                        else if (ind == 1)
                        {
                            Axis.ZoomLens_S.Tray = ZoomLens.OriginPos - ZoomLens.NowPos;
                            strMsg = "保存料盘镜头焦距成功";
                            temp = 1;
                        }
                        else if (ind == 6)
                        {
                            Axis.ZoomLens_S.BredeIn = ZoomLens.OriginPos - ZoomLens.NowPos;
                            strMsg = "保存飞达镜头焦距成功";
                            temp = 2;
                        }
                        else if (ind == 5)
                        {
                            Axis.ZoomLens_S.BredeOut = ZoomLens.OriginPos - ZoomLens.NowPos;
                            strMsg = "保存编带镜头焦距成功";
                            temp = 3;
                        }
                        else if (ind == 8)
                        {
                            Axis.ZoomLens_S.TubeIn = ZoomLens.OriginPos - ZoomLens.NowPos;
                            strMsg = "保存料管镜头焦距成功";
                            temp = 4;
                        }
                        labSelect[temp].Text = (ZoomLens.OriginPos - ZoomLens.NowPos).ToString();
                        g_config.WriteZoomLensValue();
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, strMsg, "Modify");
                    }
                    else
                    {
                        MessageBox.Show(MultiLanguage.GetString("通讯异常"));
                    }
                });
            }
            else if (Config.ZoomLens == 2)
            {
                int ind = Convert.ToInt32(SelectedRbtn_Ind);
                int temp = 0;
                string strMsg = null;
                if (ind == 0)
                {
                    Axis.ZoomLens_S.Socket = Axis.trapPrm_U.getPos;
                    strMsg = "保存烧录座镜头焦距成功";
                    temp = 0;
                }
                else if (ind == 1)
                {
                    Axis.ZoomLens_S.Tray = Axis.trapPrm_U.getPos;
                    strMsg = "保存料盘镜头焦距成功";
                    temp = 1;
                }
                else if (ind == 6)
                {
                    Axis.ZoomLens_S.BredeIn = Axis.trapPrm_U.getPos;
                    strMsg = "保存飞达镜头焦距成功";
                    temp = 2;
                }
                else if (ind == 5)
                {
                    Axis.ZoomLens_S.BredeOut = Axis.trapPrm_U.getPos;
                    strMsg = "保存编带镜头焦距成功";
                    temp = 3;
                }
                else if (ind == 8)
                {
                    Axis.ZoomLens_S.TubeIn = Axis.trapPrm_U.getPos;
                    strMsg = "保存料管镜头焦距成功";
                    temp = 4;
                }
                labSelect[temp].Text = Axis.trapPrm_U.getPos.ToString();
                g_config.WriteZoomLensValue();
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, strMsg, "Modify");
            }
        }

        private void techICModBtn_Click(object sender, EventArgs e)
        {
            g_act.CCDCap(GlobConstData.ST_CCDUP);
            g_act.CCDSnap(GlobConstData.ST_CCDUP);
            this._ConInitWnd.InitImgModel();
        }

        private void findImgModBtn_Click(object sender, EventArgs e)
        {
            _ConInitWnd.FindImgModel();
        }

        private void GO_Socket_Click(object sender, EventArgs e)
        {
            int ind = SelectedRbtn_Ind - 1;
            double setx, sety;
            if (!Auto_Flag.SystemBusy)
            {
                if (SelectedRbtn_Ind == 0)//烧录座
                {
                    setx = Axis.Group[seatCBox.SelectedIndex / UserConfig.ScketUnitC].Unit[seatCBox.SelectedIndex % UserConfig.ScketUnitC].TopCamera_X;
                    sety = Axis.Group[seatCBox.SelectedIndex / UserConfig.ScketUnitC].Unit[seatCBox.SelectedIndex % UserConfig.ScketUnitC].TopCamera_Y;
                }
                else
                {
                    setx = g_config.ArrFixIn[ind].P.X;
                    sety = g_config.ArrFixIn[ind].P.Y;
                }
                TeachAction.GO_Start(setx, sety);

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
                            if (SelectedRbtn_Ind == 0)
                            {
                                ZoomLens.SetPos = ZoomLens.OriginPos - Axis.ZoomLens_S.Socket - ZoomLens.NowPos;
                            }
                            else if (SelectedRbtn_Ind == 1)
                            {
                                ZoomLens.SetPos = ZoomLens.OriginPos - Axis.ZoomLens_S.Tray - ZoomLens.NowPos;
                            }
                            else if (SelectedRbtn_Ind == 6)
                            {
                                ZoomLens.SetPos = ZoomLens.OriginPos - Axis.ZoomLens_S.BredeIn - ZoomLens.NowPos;
                            }
                            else if (SelectedRbtn_Ind == 5)
                            {
                                ZoomLens.SetPos = ZoomLens.OriginPos - Axis.ZoomLens_S.BredeOut - ZoomLens.NowPos;
                            }
                            else if (SelectedRbtn_Ind == 8)
                            {
                                ZoomLens.SetPos = ZoomLens.OriginPos - Axis.ZoomLens_S.TubeIn - ZoomLens.NowPos;
                            }
                            ZoomLens.MODBUS_DST();
                        }
                        else
                        {
                            MessageBox.Show("通讯异常");
                        }
                    });
                }
                else if (Config.ZoomLens == 2)
                {
                    if (SelectedRbtn_Ind == 0)
                    {
                        ZoomLens.SetPos = Axis.ZoomLens_S.Socket;
                    }
                    else if (SelectedRbtn_Ind == 1)
                    {
                        ZoomLens.SetPos = Axis.ZoomLens_S.Tray;
                    }
                    else if (SelectedRbtn_Ind == 6)
                    {
                        ZoomLens.SetPos = Axis.ZoomLens_S.BredeIn;
                    }
                    else if (SelectedRbtn_Ind == 5)
                    {
                        ZoomLens.SetPos = Axis.ZoomLens_S.BredeOut;
                    }
                    else if (SelectedRbtn_Ind == 8)
                    {
                        ZoomLens.SetPos = Axis.ZoomLens_S.TubeIn;
                    }
                    axis.Camera_Position_Control(ZoomLens.SetPos, true);
                }
                
            }
            
        }

        private void BtnMovToPOS_Click(object sender, EventArgs e)
        {
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "手动校准位置", "Modify");
            double curX = 0, curY = 0;
            MovToPOS(_ConInitWnd.IIndexModel, ref curY, ref curX);
            if (!Auto_Flag.SystemBusy)
            {
                TeachAction.GO_Start(curX, curY);
            }
            double dy = 0, dx = 0, dr = 0;
            g_act.WaitDoEvent(1000);
            g_act.CCDCap(GlobConstData.ST_CCDUP, true);
        }

        /// <summary>
        /// 自动校正坐标
        /// </summary>
        /// <returns></returns>
        public bool MovToPOS(int model, ref double curY, ref double curX)
        {
            double dXpix = 0, dYpix = 0, dRpix = 0, dx = 0, dy = 0, dr = 0;
            int ind = 0;
            g_act.SendCmd(GlobConstData.ST_LIGHTUP, g_config.ArrLampBox[model].ILightUp);
            g_act.CCDSnap(GlobConstData.ST_CCDUP);
            g_act.WaitDoEvent(300);
            if (!_ConInitWnd.FindIndexImage(model, ref dYpix, ref dXpix, ref dRpix))
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "自动校准坐标失败", "Waring");
                return false;
            }
            if (model == GlobConstData.ST_MODELICSTATPOS) 
            {
                ind = 0;
            }
            else if (model == GlobConstData.ST_ModelTrayPOS)
            {
                ind = 1;
            }
            else if (model == GlobConstData.ST_ModelBredeInPOS)
            {
                ind = 3;
            }
            else if (model == GlobConstData.ST_ModelBredeOutPOS)
            {
                ind = 4;
            }
            else if (model == GlobConstData.ST_ModelTubeInPOS)
            {
                ind = 5;
            }

            dXpix = (_HalconUtil.IImgCenterX - dXpix) * g_config.ArrCCDPrec[ind].X;
            dYpix = (_HalconUtil.IImgCenterY - dYpix) * g_config.ArrCCDPrec[ind].Y;
            dx = Axis.trapPrm_X.getPos;
            dy = Axis.trapPrm_Y.getPos;
            curX = Math.Round(dx + dXpix, 3);
            curY = Math.Round(dy - dYpix, 3);

            return true;
        }

        private void BtnRevisePos_Click(object sender, EventArgs e)
        {
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "开始自动校准所有坐标", "Modify");
            TeachAction.RevisePos_Start();
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            if (Auto_Flag.LearnBusy && (Auto_Flag.ALarmPause || Auto_Flag.RunPause))
            {
                Auto_Flag.Next = true;
            }
        }

        private void BtnRotate_MouseDown(object sender, MouseEventArgs e)
        {
            if (Config.ZoomLens != 2)
            {
                return;
            }
            Gts.GT_ClrSts(Axis.jogPrm_U.cardPrm.cardNum, Axis.jogPrm_U.index, 1);//清除轴报警
            if (sender == this.BtnRotateFWD)
            {
                plc.JOG(Config.CardType, Axis.jogPrm_U, -20);
            }
            else if (sender == this.BtnRotateREV)
            {
                plc.JOG(Config.CardType, Axis.jogPrm_U, 20);
            }
        }

        private void BtnRotate_MouseUp(object sender, MouseEventArgs e)
        {
            if (Config.ZoomLens != 2)
            {
                return;
            }
            if (Config.CardType == 0)
            {
                Gts.GT_Stop(Axis.jogPrm_U.cardPrm.cardNum, 1 << (Axis.jogPrm_U.index - 1), 0);
            }
            else
            {
                lhmtc.LH_Stop((short)(1 << (Axis.jogPrm_U.index - 1)), 0, Axis.jogPrm_U.cardPrm.cardNum);
            }
        }


        private void skinButton1_Click(object sender, EventArgs e)
        {
            ZoomLens.Init_Program_Start();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (CBoxMark.SelectedIndex == 0)//坐标
            {
                NudX[SelectedRbtn_Ind].Value = Convert.ToDecimal(Axis.trapPrm_X.getPos);
                NudY[SelectedRbtn_Ind].Value = Convert.ToDecimal(Axis.trapPrm_Y.getPos);
                g_config.MarkPos[g_config.TrayModel].Tray[SelectedRbtn_Ind].X = Axis.trapPrm_X.getPos;
                g_config.MarkPos[g_config.TrayModel].Tray[SelectedRbtn_Ind].Y = Axis.trapPrm_Y.getPos;
                g_config.WriteMarkPosValue();
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "保存Mark点坐标成功", "Modify");
            }
            else//偏移量
            {
                for (int i = 0; i < NudX.Length; i++)
                {
                    g_config.MarkPos[g_config.TrayModel].Correction[i].X = Convert.ToDouble(NudX[i].Value);
                    g_config.MarkPos[g_config.TrayModel].Correction[i].Y = Convert.ToDouble(NudY[i].Value);
                }
                g_config.WriteMarkCorrection();
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "保存Mark点修正量成功", "Modify");
            }
        }

        private void BtnTrayManager_Click(object sender, EventArgs e)
        {
            _ConInitWnd._MarkManageWnd.Show();
            _ConInitWnd._MarkManageWnd.__InitWnd();
        }

        //private void CBoxMark_SelectedValueChanged(object sender, EventArgs e)
        //{
        //    if (CBoxMark.SelectedIndex == 0)//坐标
        //    {
        //        SetReadOnly(true);
        //        for (int i = 0; i < NudX.Length; i++)
        //        {
        //            NudX[i].Value = Convert.ToDecimal(g_config.MarkPos[g_config.TrayModel].Tray[i].X);
        //            NudY[i].Value = Convert.ToDecimal(g_config.MarkPos[g_config.TrayModel].Tray[i].Y);
        //        }
        //    }
        //    else
        //    {
        //        SetReadOnly(false);
        //        for (int i = 0; i < NudX.Length; i++)
        //        {
        //            NudX[i].Value = Convert.ToDecimal(g_config.MarkPos[g_config.TrayModel].Correction[i].X);
        //            NudY[i].Value = Convert.ToDecimal(g_config.MarkPos[g_config.TrayModel].Correction[i].Y);
        //        }
        //    }
        //}
    }
}
