using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BAR.Commonlib;
using BAR.Commonlib.Utils;
using BAR.Windows;
using System.Threading;
using System.IO;
using HalconDotNet;
using BAR.CommonLib_v1._0;

namespace BAR.ControlPanels
{
    public partial class ICPosPnl : UserControl
    {
        #region
        Config g_config = Config.GetInstance();
        Act g_act = Act.GetInstance();
        HalconImgUtil _HalconUtil = HalconImgUtil.GetInstance();

        bool isLoaded = false;

        ConfigInitWnd _ConInitWnd;
        HTuple WindowHandle2;
        HTuple WindowHandle1;

        #endregion

        public ICPosPnl()
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();
            UserEvent.productChange += new UserEvent.ProductChange(UpdateModelImage);
        }
        public ICPosPnl(ConfigInitWnd configWnd):this()
        {
            this._ConInitWnd  = configWnd;
        }
        private void ICPosPnl_Load(object sender, EventArgs e)
        {
            if(!isLoaded)
            {
                __InitUI();
                isLoaded = true;
            }
            Model_Check();
            //_ConInitWnd.IIndexModel = _ConInitWnd.ISelBoxInd = GlobConstData.ST_MODELICPOS;
            g_act.SendCmd(GlobConstData.ST_LIGHTDOWN, g_config.ArrLampBox[GlobConstData.ST_MODELICPOS].ILightDown);
            UpdateModelImage();
        }

        public void UpdateModelImage()
        {
            DispModelImage1(GlobConstData.ST_MODELICPOS);
            DispModelImage2(GlobConstData.ST_MODELICPOS_NCC);
        }

        private void __InitUI()
        {
            snapDelayTxt.Text = g_config.ISnapDelay.ToString();
            exposureTxt.Text = g_config.IShutter.ToString();
            geinTxt.Text = g_config.IGain.ToString();
            autoShowCBox.Checked = g_config.IsDispMatch;
            __InitComboBtn();
        }
        private void __InitComboBtn()
        {
            //吸笔
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                String str = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Pen" + (i + 1) : "吸笔" + (i + 1);
                this.penCBox.Items.Add(str);
            }
            penCBox.SelectedIndex = 0;

            //烧写座
            for (int i = 0; i < UserConfig.AllScketC; i++)
            {
                String str = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Seat" + (i / UserConfig.ScketUnitC + 1) + "_" + (i % UserConfig.ScketUnitC + 1) : "烧写座" + (i / UserConfig.ScketUnitC + 1) + "_" + (i % UserConfig.ScketUnitC + 1);
                this.seatCBox.Items.Add(str);
            }
            seatCBox.SelectedIndex = 0;

            //角度
            this.angCbo.Items.Add("40");
            this.angCbo.Items.Add("30");
            this.angCbo.Items.Add("20");
            this.angCbo.Items.Add("10");
            this.angCbo.Items.Add("5");
            if (g_config.DAngLimit == 40) angCbo.SelectedIndex = 0;
            else if (g_config.DAngLimit == 30) angCbo.SelectedIndex = 1;
            else if (g_config.DAngLimit == 20) angCbo.SelectedIndex = 2;
            else if (g_config.DAngLimit == 10) angCbo.SelectedIndex = 3;
            else if (g_config.DAngLimit == 5) angCbo.SelectedIndex = 4;

            //贪婪度
            this.greedCbo.Items.Add("0.9");
            this.greedCbo.Items.Add("0.8");
            this.greedCbo.Items.Add("0.7");
            this.greedCbo.Items.Add("0.6");
            this.greedCbo.Items.Add("0.5");
            this.greedCbo.Items.Add("0.4");
            this.greedCbo.Items.Add("0.3");
            this.greedCbo.Items.Add("0.2");
            this.greedCbo.Items.Add("0.1");
            if (g_config.DGreediness == 0.9) greedCbo.SelectedIndex = 0;
            else if (g_config.DGreediness == 0.8) greedCbo.SelectedIndex = 1;
            else if (g_config.DGreediness == 0.7) greedCbo.SelectedIndex = 2;
            else if (g_config.DGreediness == 0.6) greedCbo.SelectedIndex = 3;
            else if (g_config.DGreediness == 0.5) greedCbo.SelectedIndex = 4;
            else if (g_config.DGreediness == 0.4) greedCbo.SelectedIndex = 5;
            else if (g_config.DGreediness == 0.3) greedCbo.SelectedIndex = 6;
            else if (g_config.DGreediness == 0.2) greedCbo.SelectedIndex = 7;
            else if (g_config.DGreediness == 0.1) greedCbo.SelectedIndex = 8;

            //重叠率
            this.duplicCbo.Items.Add("0.7");
            this.duplicCbo.Items.Add("0.6");
            this.duplicCbo.Items.Add("0.5");
            this.duplicCbo.Items.Add("0.4");
            this.duplicCbo.Items.Add("0.3"); 
            if( g_config.DMaxOverlap == 0.7) duplicCbo.SelectedIndex = 0;
            else if (g_config.DMaxOverlap == 0.6) duplicCbo.SelectedIndex = 1;
            else if (g_config.DMaxOverlap == 0.5) duplicCbo.SelectedIndex = 2;
            else if (g_config.DMaxOverlap == 0.4) duplicCbo.SelectedIndex = 3;
            else if (g_config.DMaxOverlap == 0.3) duplicCbo.SelectedIndex = 4;

            //相似率
            this.soccerCbo.Items.Add("0.9");
            this.soccerCbo.Items.Add("0.8");
            this.soccerCbo.Items.Add("0.7");
            this.soccerCbo.Items.Add("0.6");
            this.soccerCbo.Items.Add("0.5");
            this.soccerCbo.Items.Add("0.4");
            this.soccerCbo.Items.Add("0.3");
            this.soccerCbo.Items.Add("0.2");
            this.soccerCbo.Items.Add("0.1");
            if (g_config.DScore == 0.9) soccerCbo.SelectedIndex = 0;
            else if (g_config.DScore == 0.8) soccerCbo.SelectedIndex = 1;
            else if (g_config.DScore == 0.7) soccerCbo.SelectedIndex = 2;
            else if (g_config.DScore == 0.6) soccerCbo.SelectedIndex = 3;
            else if (g_config.DScore == 0.5) soccerCbo.SelectedIndex = 4;
            else if (g_config.DScore == 0.4) soccerCbo.SelectedIndex = 5;
            else if (g_config.DScore == 0.3) soccerCbo.SelectedIndex = 6;
            else if (g_config.DScore == 0.2) soccerCbo.SelectedIndex = 7;
            else if (g_config.DScore == 0.1) soccerCbo.SelectedIndex = 8;

            //对比度
            this.contrastCbo.Items.Add("auto");
            for (int n = 10; n < 250; n = n + 10)
            {
                this.contrastCbo.Items.Add(n);
            }
            contrastCbo.SelectedIndex = (g_config.IContrast / 10);

            //最小对比度
            this.minContrastCbo.Items.Add("auto");
            for (int n = 10; n < 250; n = n + 10)
            {
                this.minContrastCbo.Items.Add(n);
            }
            minContrastCbo.SelectedIndex = (g_config.IMinContrast / 10);
        }
        public void DispModelImage1(short modelIndex)
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
        public void DispModelImage2(short modelIndex)
        {
            if (WindowHandle2 != null) HOperatorSet.CloseWindow(WindowHandle2);
            HOperatorSet.OpenWindow(0, 0, modelPicBox2.Width, modelPicBox2.Height, modelPicBox2.Handle, "visible", "", out WindowHandle2);
            String path = g_config.StrProductDir + "\\set_" + modelIndex + ".jpg";
            if (File.Exists(path))//判断文件是否存在
            {
                HObject img;
                HOperatorSet.ClearWindow(WindowHandle2);
                _HalconUtil.LoadImgAndDisp(WindowHandle2, out img, path);
                img?.Dispose();
            }
        }
        private void saveBtn_Click(object sender, EventArgs e)
        {
            g_config.ISnapDelay = Convert.ToInt32(snapDelayTxt.Text);
            combox_SelectionChangeCommitted(sender, e);
        }
        private void setCamParaBtn_Click(object sender, EventArgs e)
        {
            g_config.IShutter = Convert.ToInt32(exposureTxt.Text);
            g_config.IGain = Convert.ToInt32(geinTxt.Text);
            if (Config.CameraType == GlobConstData.Camera_DH)
            {
                g_act.ArrDHCameraUtils[GlobConstData.ST_CCDDOWN].SetShutter(g_config.IShutter);
                g_act.ArrDHCameraUtils[GlobConstData.ST_CCDDOWN].SetGain(g_config.IGain);
            }
            else if(Config.CameraType == GlobConstData.Camera_HR)
            {
                g_act.ArrHRCameraUtils[GlobConstData.ST_CCDDOWN].SetShutter(g_config.IShutter);
                g_act.ArrHRCameraUtils[GlobConstData.ST_CCDDOWN].SetGain(g_config.IGain);
            }
            else
            {
                g_act.ArrMVCameraUtils[GlobConstData.ST_CCDDOWN].SetShutter(g_config.IShutter);
                g_act.ArrMVCameraUtils[GlobConstData.ST_CCDDOWN].SetGain(g_config.IGain);
            }
            g_config.SaveModelCxy1();
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "修改相机参数成功", "Modify");
        }
        private void autoShowCBox_CheckedChanged(object sender, EventArgs e)
        {
            g_config.IsDispMatch = autoShowCBox.Checked;
            g_config.WriteIsDispMatch();
        }
        private void toDownCamBtn_Click(object sender, EventArgs e)
        {
            int group,unit,pen;
            
            group = seatCBox.SelectedIndex / UserConfig.ScketUnitC;
            unit = seatCBox.SelectedIndex % UserConfig.ScketUnitC;
            pen = penCBox.SelectedIndex;
            TeachAction.Learn_Start(group, unit, pen);
        }
        private void techModBtn_Click(object sender, EventArgs e)
        {
            //_ConInitWnd.IIndexModel = GlobConstData.ST_MODELICPOS;
            Model_Check();
            g_act.CCDCap(GlobConstData.ST_CCDDOWN);
            g_act.CCDSnap(GlobConstData.ST_CCDDOWN);
            this._ConInitWnd.InitImgModel();
        }

        private void findModelBtn_Click(object sender, EventArgs e)
        {
            g_act.CCDSnap(GlobConstData.ST_CCDDOWN);
            g_act.WaitDoEvent(100);
            double dy = 0, dx = 0, dr = 0;
            if (_ConInitWnd.FindIndexImage(GlobConstData.ST_MODELICPOS, ref dy, ref dx, ref dr))
            {
                int dirX, dirY, dirR;
                dirX = g_config.ArrCamDir[0] == 0 ? 1 : -1;
                dirY = g_config.ArrCamDir[1] == 0 ? 1 : -1;
                dirR = g_config.ArrCamDir[2] == 0 ? 1 : -1;
                //相素偏差
                string str = string.Format("[Pix] Y:{0:f2},X:{1:f2},R:{2:f2}", dy, dx, dr);
                TextModelXYR1.Text = str;
                //距离偏差
                double offsetX, offsetY, offsetR;
                offsetX = (dx - g_config.DModelCx) * g_config.ArrCCDPrec[GlobConstData.ST_MODELPENACC].X * dirX;
                offsetY = (dy - g_config.DModelCy) * g_config.ArrCCDPrec[GlobConstData.ST_MODELPENACC].Y * dirY;
                offsetR = (dr - g_config.DModelCr) * dirR;
                offsetXTxT1.Text = offsetX.ToString("f2");
                offsetYTxT1.Text = offsetY.ToString("f2");
                offsetRTxT1.Text = offsetR.ToString("f2");
            }
            else
            {
                MessageBox.Show(MultiLanguage.GetString("图像未找到"));
            }
        }

        private void layICBtn_Click(object sender, EventArgs e)
        {
            if (Auto_Flag.LearnReady)
            {
                Button.replaceIC = true;
            }
        }

        private void combox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //角度
            g_config.DAngLimit = Convert.ToDouble(angCbo.SelectedItem);

            //贪婪度
            g_config.DGreediness = Convert.ToDouble(greedCbo.SelectedItem);

            //重叠率
            g_config.DMaxOverlap = Convert.ToDouble(duplicCbo.SelectedItem);

            //相似率
            g_config.DScore = Convert.ToDouble(soccerCbo.SelectedItem);

            //对比度
            String temStr = Convert.ToString(contrastCbo.SelectedItem);
            if (temStr.Equals("auto")) g_config.IContrast = 0;
            else g_config.IContrast = Convert.ToInt32(contrastCbo.SelectedItem);

            //最小对比度
            temStr = Convert.ToString(minContrastCbo.SelectedItem);
            if (temStr.Equals("auto")) g_config.IMinContrast = 0;
            else g_config.IMinContrast = Convert.ToInt32(minContrastCbo.SelectedItem);

            g_config.SaveModelCxy1();
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (Auto_Flag.LearnBusy && (Auto_Flag.ALarmPause || Auto_Flag.RunPause))
            {
                Auto_Flag.Next = true;
            }
        }

        private void Model_CheckedChanged(object sender, EventArgs e)
        {
            if (RbtnFrontModel.Checked)
            {
                _ConInitWnd.IIndexModel = _ConInitWnd.ISelBoxInd = GlobConstData.ST_MODELICPOS;
                DispModelImage1(GlobConstData.ST_MODELICPOS);
            }
            else if (RbtnBackModel.Checked)
            {
                _ConInitWnd.IIndexModel = _ConInitWnd.ISelBoxInd = GlobConstData.ST_MODELICPOS_BACK1;
                DispModelImage1(GlobConstData.ST_MODELICPOS_BACK1);
            }
        }

        /// <summary>
        /// 检查匹配的模式
        /// </summary>
        public void Model_Check()
        {
            if (Auto_Flag.Enabled_NCCModel)
            {
                _ConInitWnd.IIndexModel = _ConInitWnd.ISelBoxInd = GlobConstData.ST_MODELICPOS_NCC;
            }
            else
            {
                _ConInitWnd.IIndexModel = _ConInitWnd.ISelBoxInd = GlobConstData.ST_MODELICPOS;
            }
            g_act.SendCmd(GlobConstData.ST_LIGHTDOWN, g_config.ArrLampBox[_ConInitWnd.ISelBoxInd].ILightDown);
        }

        private void BtnRegion_Click(object sender, EventArgs e)
        {

        }

        private void BtnMask_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 查找NCC模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnFindNCCModel_Click(object sender, EventArgs e)
        {
            g_act.CCDSnap(GlobConstData.ST_CCDDOWN);
            g_act.WaitDoEvent(100);
            double dy = 0, dx = 0, dr = 0;
            if (_ConInitWnd.FindIndexImage(GlobConstData.ST_MODELICPOS_NCC, ref dy, ref dx, ref dr))
            {
                int dirX, dirY, dirR;
                dirX = g_config.ArrCamDir[0] == 0 ? 1 : -1;
                dirY = g_config.ArrCamDir[1] == 0 ? 1 : -1;
                dirR = g_config.ArrCamDir[2] == 0 ? 1 : -1;
                //相素偏差
                string str = string.Format("[Pix] Y:{0:f2},X:{1:f2},R:{2:f2}", dy, dx, dr);
                TextModelXYR2.Text = str;
                //距离偏差
                double offsetX, offsetY, offsetR;
                offsetX = (dx - g_config.DModelCx2) * g_config.ArrCCDPrec[GlobConstData.ST_MODELPENACC].X * dirX;
                offsetY = (dy - g_config.DModelCy2) * g_config.ArrCCDPrec[GlobConstData.ST_MODELPENACC].Y * dirY;
                offsetR = (dr - g_config.DModelCr2) * dirR;
                offsetXTxT2.Text = offsetX.ToString("f2");
                offsetYTxT2.Text = offsetY.ToString("f2");
                offsetRTxT2.Text = offsetR.ToString("f2");
            }
            else
            {
                MessageBox.Show(MultiLanguage.GetString("图像未找到"));
            }
        }

        private void enableMod2CBox_Click(object sender, EventArgs e)
        {
            Auto_Flag.Enabled_NCCModel = !Auto_Flag.Enabled_NCCModel;
            g_config.WriteFuncSwitVal();
            Model_Check();
        }
    }
}
