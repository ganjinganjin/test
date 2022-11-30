using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BAR.Commonlib;
using BAR.CommonLib_v1._0;
using CCWin.SkinControl;
using CCWin.Win32.Const;
using LBSoft.IndustrialCtrls.Leds;
using PLC;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace BAR.Windows
{
    public partial class FuncSwitWnd : Form
    {
        Config g_config = Config.GetInstance();
        string[] strRunModel;
        public FuncSwitWnd()
        {
            strRunModel = new string[]
            {
                "标准模式","精准模式"
            };

            InitializeComponent();
            InitCbo();
            this.__InitUI();

            //飞达使能控件
            Run.FeederEnable = new SkinCheckBox[2];
            Run.FeederEnable[0] = ChkFeeder1;
            Run.FeederEnable[1] = ChkFeeder2;
            //读飞达使能
            g_config.ReadFeederEnabled();

        }

        private void __InitUI()
        {
            
            skinGroupBox6.Visible = UserTask.ProgrammerType == GlobConstData.Programmer_WG || UserTask.ProgrammerType == GlobConstData.Programmer_YED ||
                                    UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY;
            skinGroupBox9.Visible = Config.Altimeter != 0;
            skinGroupBox14.Visible = Config.SyncTakeLay && UserTask.ProgrammerType != GlobConstData.Programmer_YED;
            if (UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY)
            {
                renovateBtn.Text = MultiLanguage.GetString("推压");
            }
            if (Config.Altimeter == 0)
            {
                skinGroupBox14.Location = skinGroupBox9.Location;
            }
            GbxVision_3D.Visible = Vision_3D.Function;
            if (!Vision_3D.Function)
            {
                Vision_3D.Enabled_I = false;
                Vision_3D.Enabled_II = false;
            }
            //UpdateUI();
            UserEvent.productChange += new UserEvent.ProductChange(UpdateUI);

            if (Config.FeederCount == 1)
            {
                GbxFeeder.Visible = true;
            }
            if (Config.BredeDotCount == 0)//编带单打点
            {
                dotBBtn.Visible = false;
                dotBLed.Visible = false;
                dotABtn.Size = new Size(120,34);
                dotALed.Size = new Size(25, 34);
                dotALed.Location = new Point(134,171);
                dotABtn.Text = MultiLanguage.GetString("打点");
            }
        }

        private void FuncSwitWnd_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            
        }

        public void __InitWnd()
        {
            timer1.Enabled = true;
            UpdateUI();
        }

        /// <summary>
        /// 更新UI
        /// </summary>
        private void UpdateUI()
        {
            if (UserTask.ShiftWay == 5)
            {
                funcRBtn6.Checked = true;
            }
            else if (UserTask.ShiftWay == 4)
            {
                funcRBtn5.Checked = true;
            }
            else if (UserTask.ShiftWay == 3)
            {
                funcRBtn4.Checked = true;
            }
            else if (UserTask.ShiftWay == 2)
            {
                funcRBtn3.Checked = true;
            }
            else if (UserTask.ShiftWay == 1)
            {
                funcRBtn2.Checked = true;
            }
            else
            {
                funcRBtn1.Checked = true;
            }

            CboRunModel.SelectedIndex = Auto_Flag.RunModel;

            RbtnNonTestMode.Checked = !Auto_Flag.TestMode;
            RbtnNonBurnMode.Checked = !Auto_Flag.BurnMode;
            RbtnNonAutoTray.Checked = !Auto_Flag.AutoTray;
            RbtnNonNGTray.Checked = !Auto_Flag.NGTray;

            RbtnTestMode.Checked = Auto_Flag.TestMode;
            RbtnBurnMode.Checked = Auto_Flag.BurnMode;
            RbtnAutoTray.Checked = Auto_Flag.AutoTray;
            RbtnNGTray.Checked = Auto_Flag.NGTray;

            ChkICPosEnabled1.Checked = Auto_Flag.Enabled_TakeICPos;
            ChkICPosEnabled2.Checked = Auto_Flag.Enabled_LayICPos;
            ChkVision_3D1.Checked = Vision_3D.Enabled_I;
            ChkVision_3D2.Checked = Vision_3D.Enabled_II;

            nudTIC_BredeR.Value = Convert.ToDecimal(RotateAngle.TIC_Brede);
            nudLIC_BredeR.Value = Convert.ToDecimal(RotateAngle.LIC_Brede);
            nudTIC_TubeR.Value = Convert.ToDecimal(RotateAngle.TIC_Tube);
            nudTIC_Pan1R.Value = Convert.ToDecimal(RotateAngle.TIC_Tray[0]);
            nudTIC_Pan2R.Value = Convert.ToDecimal(RotateAngle.TIC_Tray[1]);
            nudLIC_Pan1R.Value = Convert.ToDecimal(RotateAngle.LIC_Tray[0]);
            nudLIC_Pan2R.Value = Convert.ToDecimal(RotateAngle.LIC_Tray[1]);
            nudLIC_Pan3R.Value = Convert.ToDecimal(RotateAngle.LIC_Tray[2]);
            InitPic();
        }

        /// <summary>
        /// 初始化下拉框
        /// </summary>
        private void InitCbo()
        {
            AddCboItems(CboRunModel, strRunModel, Auto_Flag.RunModel);
        }

        /// <summary>
        /// 添加下拉框选项
        /// </summary>
        /// <param name="comboBox">下拉框</param>
        /// <param name="str">选项组</param>
        /// <param name="select">选择项</param>
        private void AddCboItems(ComboBox comboBox, string[] str, int select)
        {
            comboBox.Items.Clear();
            foreach (string sName in str)
            {
                string strGet = MultiLanguage.GetString(sName);
                comboBox.Items.Add(strGet);
            }
            comboBox.SelectedIndex = select;
        }

        /// <summary>
        /// 初始化图像框
        /// </summary>
        private void InitPic()
        {
            String ctlName;
            Control[] tagAry;
            PictureBox pictureBox;
            RotateFlipType rotateFlipType;
            double Index;
            double[] tempRotate = { -RotateAngle.TIC_Tray[0], -RotateAngle.LIC_Tray[0], -RotateAngle.TIC_Tray[1], -RotateAngle.LIC_Tray[1],0, -RotateAngle.LIC_Tray[2],
                -RotateAngle.TIC_Brede, -RotateAngle.LIC_Brede, -RotateAngle.TIC_Tube, 0 , RotateAngle.Base_Socket};
            for (int i = 0; i < tempRotate.Length; i++)
            {
                ctlName = "pictureBox" + i;
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    pictureBox = (tagAry.First() as PictureBox);
                    Index = tempRotate[i] + RotateAngle.Base_Socket;
                    if (i == tempRotate.Length - 1) 
                    {
                        Index = tempRotate[i];
                    }
                    switch (Index)
                    {
                        case 0:
                        case 360:
                            rotateFlipType = RotateFlipType.RotateNoneFlipNone;
                            break;
                        case 90:
                            rotateFlipType = RotateFlipType.Rotate90FlipNone;
                            break;
                        case 180:
                        case -180:
                            rotateFlipType = RotateFlipType.Rotate180FlipNone;
                            break;
                        case 270:
                        case -90:
                            rotateFlipType = RotateFlipType.Rotate270FlipNone;
                            break;
                        default:
                            rotateFlipType = RotateFlipType.RotateNoneFlipNone;
                            break;
                    }
                    Bitmap b = new Bitmap(pictureBox_Base.Image);
                    b.RotateFlip(rotateFlipType);//不进行翻转的旋转
                    pictureBox.Image = b;
                }
            }
            
        }



        private void FuncSwitWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
            e.Cancel = true;
            Hide();
        }    

        private void funcRBtn_clicked(object sender, MouseEventArgs e)
        {
            if (Auto_Flag.LearnBusy || Auto_Flag.AutoRunBusy)
            {
                return;
            }
            UserTask.ShiftWay = Convert.ToUInt16((sender as SkinRadioButton).Tag);
            g_config.WriteFuncSwitVal();
        }

        private void SetLed(bool flag, Label label)
        {
            if (flag)
            {
                label.BackColor = Color.Lime;
            }
            else
            {
                label.BackColor = Color.Maroon;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SetLed(Auto_Flag.Brede_Check, braidCheckLed);
            SetLed(Auto_Flag.Velum_Check, GMCheckLed);
            SetLed(Auto_Flag.Empty_Check, ledEmptyCheck);
            SetLed(Auto_Flag.BredeCCD_Check, ledBredeCCD_Check);
            SetLed(Auto_Flag.DotA, dotALed);
            SetLed(Auto_Flag.DotB, dotBLed);
            SetLed(Auto_Flag.Flip, renovateLed);
            SetLed(In_Output.SVON[0][0].M, excitXLed);
            SetLed(In_Output.SVON[0][1].M, excitYLed);

            ChkOverlay.Checked = Auto_Flag.Enabled_Overlay;
            ChkPenAbnormal.Checked = Auto_Flag.PenAbnormal;
            ChkPenAltMode.Checked = Auto_Flag.PenAltMode;
            ChkSyncTakeLay.Checked = Auto_Flag.Enabled_Sync;

            //定量
            if (!Auto_Flag.Production)
            {
                normCBox.Checked = false;
                okRbtn.Visible = false;
                tarRBtn.Visible = false;
            }
            else
            {
                normCBox.Checked = true;
                okRbtn.Visible = true;
                tarRBtn.Visible = true;
                if (Auto_Flag.ProductionOK)
                {
                    okRbtn.Checked = true;
                }
                else
                {
                    tarRBtn.Checked = true;
                }
            }

            //油墨监控
            if (Inks.Function)
            {
                if (!GbxInks.Visible)
                {
                    GbxInks.Visible = true;
                }

                if (Inks.Enabled_AutoTray)
                {
                    ChkInksAutoTray.Checked = true;
                }
                else
                {
                    ChkInksAutoTray.Checked = false;
                }

                if (Inks.Enabled_Braid)
                {
                    ChkInksBraid.Checked = true;
                }
                else
                {
                    ChkInksBraid.Checked = false;
                }
            }
        }

        private void braidCheckBtn_Click(object sender, EventArgs e)
        {
            Auto_Flag.Brede_Check = !Auto_Flag.Brede_Check;
            g_config.WriteFuncSwitVal();
        }

        private void GMCheckBtn_Click(object sender, EventArgs e)
        {
            Auto_Flag.Velum_Check = !Auto_Flag.Velum_Check;
            g_config.WriteFuncSwitVal();
        }

        private void dotBtn_Click(object sender, EventArgs e)
        {
            Auto_Flag.DotA = !Auto_Flag.DotA;
            g_config.WriteFuncSwitVal();
        }

        private void dotBBtn_Click(object sender, EventArgs e)
        {
            Auto_Flag.DotB = !Auto_Flag.DotB;
            g_config.WriteFuncSwitVal();
        }

        private void renovateBtn_Click(object sender, EventArgs e)
        {
            if (Auto_Flag.AutoRunBusy)
            {
                return;
            }
            Auto_Flag.Flip = !Auto_Flag.Flip;
            g_config.WriteFuncSwitVal();
            UserEvent evt = new UserEvent();
            evt.RenovateBtn_Click();
        }

        private void excitXBtn_Click(object sender, EventArgs e)
        {
            if (In_Output.SVON[0][0].M)
            {
                Gts.GT_AxisOff(PLC1.card[0].cardNum, 1);
            }
            else
            {
                Gts.GT_AxisOn(PLC1.card[0].cardNum, 1);
            }
        }

        private void excitYBtn_Click(object sender, EventArgs e)
        {
            if (In_Output.SVON[0][1].M)
            {
                Gts.GT_AxisOff(PLC1.card[0].cardNum, 2);
            }
            else
            {
                Gts.GT_AxisOn(PLC1.card[0].cardNum, 2);
            }
        }

        private void okRbtn_CheckedChanged(object sender, EventArgs e)
        {
            Auto_Flag.ProductionOK = true;
            g_config.WriteFuncSwitVal();
        }

        private void tarRBtn_CheckedChanged(object sender, EventArgs e)
        {
            Auto_Flag.ProductionOK = false;
            g_config.WriteFuncSwitVal();
        }

        private void normCBox_Click(object sender, EventArgs e)
        {
            Auto_Flag.Production = !Auto_Flag.Production;
            g_config.WriteFuncSwitVal();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            RotateAngle.TIC_Brede = Convert.ToDouble(nudTIC_BredeR.Value);
            RotateAngle.LIC_Brede = Convert.ToDouble(nudLIC_BredeR.Value);
            RotateAngle.TIC_Tube = Convert.ToDouble(nudTIC_TubeR.Value);
            RotateAngle.TIC_Tray[0] = Convert.ToDouble(nudTIC_Pan1R.Value);
            RotateAngle.TIC_Tray[1] = Convert.ToDouble(nudTIC_Pan2R.Value);
            RotateAngle.LIC_Tray[0] = Convert.ToDouble(nudLIC_Pan1R.Value);
            RotateAngle.LIC_Tray[1] = Convert.ToDouble(nudLIC_Pan2R.Value);
            RotateAngle.LIC_Tray[2] = Convert.ToDouble(nudLIC_Pan3R.Value);

            g_config.SaveRotateAnglel();
        }

        private void ChkICPosEnabled1_Click(object sender, EventArgs e)
        {
            if (!Auto_Flag.AutoRunBusy)
            {
                Auto_Flag.Enabled_TakeICPos = !Auto_Flag.Enabled_TakeICPos;
                g_config.WriteFuncSwitVal();
            }
            ChkICPosEnabled1.Checked = Auto_Flag.Enabled_TakeICPos;
        }

        private void ChkICPosEnabled2_Click(object sender, EventArgs e)
        {
            Auto_Flag.Enabled_LayICPos = !Auto_Flag.Enabled_LayICPos;
            g_config.WriteFuncSwitVal();
        }

        private void ChkVision_3D1_Click(object sender, EventArgs e)
        {
            if (!Auto_Flag.AutoRunBusy)
            {
                Vision_3D.Enabled_I = !Vision_3D.Enabled_I;
                g_config.WriteFuncSwitVal();
            }
            ChkVision_3D1.Checked = Vision_3D.Enabled_I;
        }

        private void ChkVision_3D2_Click(object sender, EventArgs e)
        {
            if (!Auto_Flag.AutoRunBusy)
            {
                Vision_3D.Enabled_II = !Vision_3D.Enabled_II;
                g_config.WriteFuncSwitVal();
            }
            ChkVision_3D2.Checked = Vision_3D.Enabled_II;
        }

        private void ChkInksAutoTray_Click(object sender, EventArgs e)
        {
            Inks.Enabled_AutoTray = !Inks.Enabled_AutoTray;
            g_config.WriteFuncSwitVal();
        }

        private void ChkInksBraid_Click(object sender, EventArgs e)
        {
            Inks.Enabled_Braid = !Inks.Enabled_Braid;
            g_config.WriteFuncSwitVal();
        }

        private void ChkFeeder_Click(object sender, EventArgs e)
        {
            if (!Auto_Flag.AutoRunBusy)
            {
                int ind = Convert.ToInt32((sender as SkinCheckBox).Tag);

                Run.FeederEnable[ind].Checked = !Run.FeederEnable[ind].Checked;
                if (!Run.FeederEnable[0].Checked && !Run.FeederEnable[1].Checked)
                {
                    Run.FeederEnable[0].Checked = true;
                }
                g_config.WriteFeederEnabled();
            }
        }

        private void ChkOverlay_Click(object sender, EventArgs e)
        {
            if (!Auto_Flag.SystemBusy)
            {
                Auto_Flag.Enabled_Overlay = !Auto_Flag.Enabled_Overlay;
                g_config.WriteFuncSwitVal();
            }
            ChkOverlay.Checked = Auto_Flag.Enabled_Overlay;
        }

        private void ChkPenAbnormal_Click(object sender, EventArgs e)
        {
            if (!Auto_Flag.SystemBusy)
            {
                Auto_Flag.PenAbnormal = !Auto_Flag.PenAbnormal;
                g_config.WritePenEnabled();
            }
            ChkPenAbnormal.Checked = Auto_Flag.PenAbnormal;
        }
        private void ChkSyncTakeLay_Click(object sender, EventArgs e)
        {
            if (!Auto_Flag.AutoRunBusy)
            {
                Auto_Flag.Enabled_Sync = !Auto_Flag.Enabled_Sync;
                g_config.WriteFuncSwitVal();
            }
            ChkSyncTakeLay.Checked = Auto_Flag.Enabled_Sync;
        }

        private void RbtnTestMode_CheckedChanged(object sender, EventArgs e)
        {
            if (!Auto_Flag.AutoRunBusy)
            {
                Auto_Flag.TestMode = RbtnTestMode.Checked;
                g_config.WriteFuncSwitVal();
            }
        }

        private void RbtnAutoTray_CheckedChanged(object sender, EventArgs e)
        {
            if (!Auto_Flag.AutoRunBusy)
            {
                Auto_Flag.AutoTray = RbtnAutoTray.Checked;
                g_config.WriteFuncSwitVal();
            }
        }

        private void RbtnNGTray_CheckedChanged(object sender, EventArgs e)
        {
            Auto_Flag.NGTray = RbtnNGTray.Checked;
            g_config.WriteFuncSwitVal();
        }

        private void RbtnBurnMode_CheckedChanged(object sender, EventArgs e)
        {
            if (!Auto_Flag.AutoRunBusy)
            {
                Auto_Flag.BurnMode = RbtnBurnMode.Checked;
                g_config.WriteFuncSwitVal();
            }
        }

        private void CboRunModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            Auto_Flag.RunModel = CboRunModel.SelectedIndex;
            g_config.WriteFuncSwitVal();
            g_config.ReadAxisParaVal();
        }

        private void ChkPenAltMode_Click(object sender, EventArgs e)
        {
            if (!Auto_Flag.AutoRunBusy)
            {
                Auto_Flag.PenAltMode = !Auto_Flag.PenAltMode;
                g_config.WriteFuncSwitVal();
            }
            ChkPenAltMode.Checked = Auto_Flag.PenAltMode;
        }

        private void btnEmptyCheck_Click(object sender, EventArgs e)
        {
            Auto_Flag.Empty_Check = !Auto_Flag.Empty_Check;
            g_config.WriteFuncSwitVal();
        }

        private void btnBredeCCD_Check_Click(object sender, EventArgs e)
        {
            Auto_Flag.BredeCCD_Check = !Auto_Flag.BredeCCD_Check;
            g_config.WriteFuncSwitVal();
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            PictureBox clickPbox = sender as PictureBox;
            Bitmap b = new Bitmap(clickPbox.Image);
            int Index = Convert.ToInt16(clickPbox.Tag);
            b.RotateFlip(RotateFlipType.Rotate90FlipNone);//不进行翻转的旋转
            clickPbox.Image = b;
            int Angle_1 = -90, Angle_2 = 90;
            switch (Index)
            {
                case 0://料盘1取料角度变化
                    RotateAngle_Operation(ref RotateAngle.TIC_Tray[0], Angle_1);
                    break;
                case 1://料盘1放料角度变化
                    RotateAngle_Operation(ref RotateAngle.LIC_Tray[0], Angle_1);
                    break;
                case 2://料盘2取料角度变化
                    RotateAngle_Operation(ref RotateAngle.TIC_Tray[1], Angle_1);
                    break;
                case 3://料盘2放料角度变化
                    RotateAngle_Operation(ref RotateAngle.LIC_Tray[1], Angle_1);
                    break;
                case 5://料盘3放料角度变化
                    RotateAngle_Operation(ref RotateAngle.LIC_Tray[2], Angle_1);
                    break;
                case 6://编带取料角度变化
                    RotateAngle_Operation(ref RotateAngle.TIC_Brede, Angle_1);
                    break;
                case 7://编带放料角度变化
                    RotateAngle_Operation(ref RotateAngle.LIC_Brede, Angle_1);
                    break;
                case 8://料管取料角度变化
                    RotateAngle_Operation(ref RotateAngle.TIC_Tube, Angle_1);
                    break;

                case 10://烧录座基准角度变化
                    RotateAngle.Base_Socket += 90;
                    if (RotateAngle.Base_Socket > 270)
                    {
                        RotateAngle.Base_Socket = 0;
                    }
                    RotateAngle_Operation(ref RotateAngle.TIC_Tray[0], Angle_2);
                    RotateAngle_Operation(ref RotateAngle.LIC_Tray[0], Angle_2);
                    RotateAngle_Operation(ref RotateAngle.TIC_Tray[1], Angle_2);
                    RotateAngle_Operation(ref RotateAngle.LIC_Tray[1], Angle_2);
                    RotateAngle_Operation(ref RotateAngle.LIC_Tray[2], Angle_2);
                    RotateAngle_Operation(ref RotateAngle.TIC_Brede, Angle_2);
                    RotateAngle_Operation(ref RotateAngle.LIC_Brede, Angle_2);
                    RotateAngle_Operation(ref RotateAngle.TIC_Tube, Angle_2);
                    break;
                default:
                    break;
            }
            
            g_config.SaveRotateAnglel();
            UpdateUI();
        }

        private void RotateAngle_Operation(ref double Origina_Angle, int Sub)
        {
            Origina_Angle += Sub;
            if (Origina_Angle == -180)
            {
                Origina_Angle = 180;
            }
            if (Origina_Angle > 180)
            {
                Origina_Angle = -90;
            }
            if (Origina_Angle < -180)
            {
                Origina_Angle = 90;
            }
        }

    }
}
