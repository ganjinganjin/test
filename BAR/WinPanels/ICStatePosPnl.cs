using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CCWin.SkinControl;
using BAR.Windows;
using BAR.Commonlib;
using HalconDotNet;
using System.IO;
using BAR.Commonlib.Utils;
using BAR.WinPanels;
using System.Threading;
using System.Threading.Tasks;
using BAR.CommonLib_v1._0;

namespace BAR.ControlPanels
{
    public partial class ICStatePosPnl : UserControl
    {
        private SkinRadioButton[] rBtnSelectPen;
        private Label[] labSelectPen;
        private SkinGroupBox[] scketGroupBox;
        private SkinRadioButton[] rBtnScketSelect;
        //private Label[] labelScketSelect;
        private SkinTextBox[] txtScketX, txtScketY;
        public SkinRadioButton SelectedRbtn;

        ConfigInitWnd   _ConInitWnd;
        Config          g_config = Config.GetInstance();
        Act             g_act = Act.GetInstance();
        public Axis axis = Axis.GetInstance();
        HalconImgUtil   _HalconUtil = HalconImgUtil.GetInstance();
        HTuple          WindowHandle1;

        public ICStatePosPnl()
        {
            MultiLanguage.SetDefaultLanguage();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
            InitializeComponent();
            _InitializeComponent();
            TeachAction.ReviseSeat = new TeachAction.ReviseSeatDelegate(MovToICSeat);

            if (Config.Altimeter == GlobConstData.CLOSE)
            { }
        }
        public ICStatePosPnl(ConfigInitWnd conInitWnd): this()
        {
            this._ConInitWnd = conInitWnd;
        }

        private void _InitializeComponent()
        {
            int left = 0, top = 0, nW = 68, nH = 20, leftStart = 2, topStart = 4;

            //GO吸笔控件
            rBtnSelectPen = new SkinRadioButton[UserConfig.VacuumPenC];
            labSelectPen = new Label[UserConfig.VacuumPenC];
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                left = i * 118;
                rBtnSelectPen[i] = new SkinRadioButton
                {
                    AutoSize = true,
                    BackColor = Color.DimGray,
                    BaseColor = Color.Red,
                    ControlState = CCWin.SkinClass.ControlState.Normal,
                    DownBack = null,
                    Font = new Font("微软雅黑", 15F, FontStyle.Bold),
                    ForeColor = Color.White,
                    ImeMode = ImeMode.NoControl,
                    LightEffect = false,
                    Location = new Point(137 + left, 3),
                    MouseBack = null,
                    Name = "rBtnSelectPen" + 1,
                    NormlBack = null,
                    SelectedDownBack = null,
                    SelectedMouseBack = null,
                    SelectedNormlBack = null,
                    Size = new Size(90, 31),
                    TabIndex = i,
                    Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English? "Pen" + (i + 1) : "吸笔" + (i + 1),
                    UseVisualStyleBackColor = false
                };

                labSelectPen[i] = new Label
                {
                    BackColor = Color.DimGray,
                    BorderStyle = BorderStyle.Fixed3D,
                    Font = new Font("楷体", 8F, FontStyle.Bold),
                    ImeMode = ImeMode.NoControl,
                    Location = new Point(134 + left, 1),
                    Name = "labSelectPen" + i,
                    Size = new Size(115, 35),
                    TabIndex = i,
                    TextAlign = ContentAlignment.MiddleLeft
                };
                Controls.Add(rBtnSelectPen[i]);
                Controls.Add(labSelectPen[i]);
            }
            TopCameralab.Location = new Point(252 + left, 1);
            TopCameraBtn.Location = new Point(255 + left, 3);

            Scannerlab.Location = new Point(252 + 118 + left, 1);
            ScannerBtn.Location = new Point(255 + 118 + left, 3);

            Altimeterlab.Location = new Point(252 + 118 * 2 + left, 1);
            AltimeterBtn.Location = new Point(255 + 118 * 2 + left, 3);

            Scannerlab.Visible = ScannerBtn.Visible = UserTask.ProgrammerType == GlobConstData.Programmer_YED;
            Altimeterlab.Visible = AltimeterBtn.Visible = modifyBtn.Visible = Config.Altimeter != 0;
            if (UserTask.ProgrammerType != GlobConstData.Programmer_YED)
            {
                AltimeterBtn.Location = ScannerBtn.Location;
                Altimeterlab.Location = Scannerlab.Location;
            }

            //烧录座子分组控件
            int groupW = 0, groupH = 0, layoutMode = 1;
            int rowC = 1, colC = 2, rowMaxC = 3, colMaxC = 4;//行列数
            if (UserTask.ProgrammerType == GlobConstData.Programmer_AK)
            {
                layoutMode = 2;//1：单行排列，2：双行排列
                colMaxC = 3;
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_DP)
            {
                layoutMode = UserConfig.ScketUnitC == 8 ? 1 : 2;//1：单行排列，2：双行排列
                colMaxC = UserConfig.ScketUnitC == 4 ? 8 : 2;
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_YED)
            {
                colMaxC = 8;
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_WG || UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY)
            {
                colMaxC = UserConfig.ScketGroupC;
            }
            if (UserConfig.ScketGroupC % colMaxC == 0)
            {
                rowC = UserConfig.ScketGroupC / colMaxC;
                colC = colMaxC;
            }
            else
            {
                rowC = UserConfig.ScketGroupC / colMaxC + 1;
                colC = rowC == 1 ? UserConfig.ScketGroupC : colMaxC;
            }
            groupW = scketPosPnl.Width / colC;
            groupH = 35 + 3 * (nH + 1) * layoutMode;
            

            Point[] groupPoint = new Point[colC * rowC];//分组位置
            for (int n = 0; n < rowC; n++)
            {
                for (int m = 0; m < colC; m++)
                {
                    int nIndex = n * colC + m;
                    left = Math.Abs((colC - 1) * 0 - m) * groupW;
                    top = Math.Abs((rowC - 1) * 1 - n) * (groupH + 1);
                    groupPoint[nIndex] = new Point(left, 30 + top);
                }
            }

            scketGroupBox = new SkinGroupBox[UserConfig.ScketGroupC];
            for (int i = 0; i < UserConfig.ScketGroupC; i++)
            {
                int nIndex = i;
                if (UserTask.ProgrammerType == GlobConstData.Programmer_AK && UserConfig.ScketGroupC == 5)
                {
                    if (i > 0)
                    {
                        nIndex++;
                    }
                }
                scketGroupBox[i] = new SkinGroupBox
                {
                    BackColor = Color.Transparent,
                    BorderColor = Color.Black,
                    Font = (UserTask.ProgrammerType == GlobConstData.Programmer_DP && UserConfig.ScketUnitC == 4) ? new Font("楷体", 15F, FontStyle.Bold) : new Font("楷体", 18F, FontStyle.Bold),
                    ForeColor = Color.Black,
                    Location = groupPoint[nIndex],
                    Name = "scketGroupBox" + i,
                    RectBackColor = SystemColors.ActiveCaption,
                    RoundStyle = CCWin.SkinClass.RoundStyle.All,
                    Size = new Size(groupW, groupH),
                    TabIndex = i,
                    TabStop = false,
                    Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Group" + (i + 1) : "组" + (i + 1) + "烧录座",
                    TitleBorderColor = SystemColors.ActiveCaption,
                    TitleRectBackColor = SystemColors.ActiveCaption,
                    TitleRoundStyle = CCWin.SkinClass.RoundStyle.All
                };
                scketGroupBox[i].SuspendLayout();
                scketPosPnl.Controls.Add(scketGroupBox[i]);
            }

            //烧录座子坐标相关控件
            rBtnScketSelect = new SkinRadioButton[UserConfig.AllScketC];
            //labelScketSelect = new Label[UserConfig.AllScketC];
            txtScketX = new SkinTextBox[UserConfig.AllScketC];
            txtScketY = new SkinTextBox[UserConfig.AllScketC];
            colC = UserConfig.ScketUnitC / layoutMode;
            if (colC == 0)
            {
                colC = 1;
            }
            nW = (groupW - 6) / colC;
            leftStart = (groupW - nW * colC + 1) / 2;

            for (int i = 0; i < UserConfig.AllScketC; i++)
            {
                left = (i % colC) * nW + leftStart;
                top = nH + topStart;

                int tempW = 52;
                if ((i % UserConfig.ScketUnitC) >= 9)
                {
                    tempW = 56;
                }
                if (layoutMode != 1)
                {
                    if (UserTask.ProgrammerType == GlobConstData.Programmer_DP && (UserConfig.ScketUnitC == 16|| UserConfig.ScketUnitC == 4))
                    {
                        top += (layoutMode - 1 - ((i % UserConfig.ScketUnitC) / colC)) * (3 * (nH + 1) + topStart);
                    }
                    else
                    {
                        top += ((i % UserConfig.ScketUnitC) / colC) * (3 * (nH + 1) + topStart);
                    }
                }
                rBtnScketSelect[i] = new SkinRadioButton
                {
                    AutoSize = true,
                    BaseColor = Color.Red,
                    BackColor = Color.Transparent,
                    Checked = false,
                    ControlState = CCWin.SkinClass.ControlState.Normal,
                    DefaultRadioButtonWidth = 15,
                    DownBack = null,
                    Font = new Font("微软雅黑", 8F, FontStyle.Bold),
                    ForeColor = Color.Black,
                    ImeMode = ImeMode.NoControl,
                    Location = new Point(left, groupH - top),
                    MouseBack = null,
                    Name = "rBtnScketSelect" + i,
                    NormlBack = null,
                    SelectedDownBack = null,
                    SelectedMouseBack = null,
                    SelectedNormlBack = null,
                    Size = new Size(tempW, nH),
                    TabIndex = i,
                    TabStop = true,
                    Tag = i,
                    Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? (i / UserConfig.ScketUnitC + 1) + "_" + (i % UserConfig.ScketUnitC + 1) : (i / UserConfig.ScketUnitC + 1) + "_" + (i % UserConfig.ScketUnitC + 1) + "座",
                    TextAlign = ContentAlignment.MiddleLeft,
                    UseVisualStyleBackColor = false
                };
                rBtnScketSelect[i].MouseUp += new MouseEventHandler(InitRbtn_MouseUp);

                //labelScketSelect[i] = new Label
                //{
                //    BorderStyle = BorderStyle.Fixed3D,
                //    Font = new Font("楷体", 8F, FontStyle.Bold),
                //    ImeMode = ImeMode.NoControl,
                //    Location = new Point(4 + left, groupH - top),
                //    Name = "labelScketSelect" + i,
                //    Size = new Size(nW - 4, 22),
                //    TabIndex = i,
                //    TextAlign = ContentAlignment.MiddleLeft
                //};

                txtScketX[i] = new SkinTextBox
                {
                    BackColor = Color.Transparent,
                    DownBack = null,
                    Icon = null,
                    IconIsButton = false,
                    IconMouseState = CCWin.SkinClass.ControlState.Normal,
                    IsPasswordChat = '\0',
                    IsSystemPasswordChar = false,
                    Lines = new string[] { "0.00" },
                    Location = new Point(left, groupH - top - 2 * nH),
                    Margin = new Padding(0),
                    MaxLength = 32767,
                    MinimumSize = new Size(20, 20),
                    MouseBack = null,
                    MouseState = CCWin.SkinClass.ControlState.Normal,
                    Multiline = false,
                    Name = "txtScketX" + i,
                    NormlBack = null,
                    Padding = new Padding(5),
                    ReadOnly = false,
                    ScrollBars = ScrollBars.None,
                    Size = new Size(nW, nH)
                };
                txtScketX[i].SkinTxt.BorderStyle = BorderStyle.None;
                txtScketX[i].SkinTxt.Dock = DockStyle.Fill;
                txtScketX[i].SkinTxt.Font = new Font("楷体", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                txtScketX[i].SkinTxt.Location = new Point(5, 5);
                txtScketX[i].SkinTxt.Name = "BaseText";
                txtScketX[i].SkinTxt.Size = new Size(nW, nH);
                txtScketX[i].SkinTxt.TabIndex = 0;
                txtScketX[i].SkinTxt.Text = "0.00";
                txtScketX[i].SkinTxt.TextAlign = HorizontalAlignment.Center;
                txtScketX[i].SkinTxt.WaterColor = Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
                txtScketX[i].SkinTxt.WaterText = "";
                txtScketX[i].TabIndex = i;
                txtScketX[i].Text = "0.00";
                txtScketX[i].TextAlign = HorizontalAlignment.Center;
                txtScketX[i].WaterColor = Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
                txtScketX[i].WaterText = "";
                txtScketX[i].WordWrap = true;

                txtScketY[i] = new SkinTextBox
                {
                    BackColor = Color.Transparent,
                    DownBack = null,
                    Icon = null,
                    IconIsButton = false,
                    IconMouseState = CCWin.SkinClass.ControlState.Normal,
                    IsPasswordChat = '\0',
                    IsSystemPasswordChar = false,
                    Lines = new string[] { "0.00" },
                    Location = new Point(left, groupH - top - nH),
                    Margin = new Padding(0),
                    MaxLength = 32767,
                    MinimumSize = new Size(20, 20),
                    MouseBack = null,
                    MouseState = CCWin.SkinClass.ControlState.Normal,
                    Multiline = false,
                    Name = "txtScketY" + i,
                    NormlBack = null,
                    Padding = new Padding(5),
                    ReadOnly = false,
                    ScrollBars = ScrollBars.None,
                    Size = new Size(nW, nH),
                 };
                txtScketY[i].SkinTxt.BorderStyle = BorderStyle.None;
                txtScketY[i].SkinTxt.Dock = DockStyle.Fill;
                txtScketY[i].SkinTxt.Font = new Font("楷体", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                txtScketY[i].SkinTxt.Location = new Point(5, 5);
                txtScketY[i].SkinTxt.Name = "BaseText";
                txtScketY[i].SkinTxt.Size = new Size(nW, nH);
                txtScketY[i].SkinTxt.TabIndex = 0;
                txtScketY[i].SkinTxt.Text = "0.00";
                txtScketY[i].SkinTxt.TextAlign = HorizontalAlignment.Center;
                txtScketY[i].SkinTxt.WaterColor = Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
                txtScketY[i].SkinTxt.WaterText = "";
                txtScketY[i].TabIndex = i;
                txtScketY[i].Text = "0.00";
                txtScketY[i].TextAlign = HorizontalAlignment.Center;
                txtScketY[i].WaterColor = Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
                txtScketY[i].WaterText = "";
                txtScketY[i].WordWrap = true;

                int group = i / UserConfig.ScketUnitC;
                scketGroupBox[group].Controls.Add(rBtnScketSelect[i]);
                //scketGroupBox[group].Controls.Add(labelScketSelect[i]);
                scketGroupBox[group].Controls.Add(txtScketX[i]);
                scketGroupBox[group].Controls.Add(txtScketY[i]);
                if (i % UserConfig.ScketUnitC == UserConfig.ScketUnitC - 1)
                {
                    scketGroupBox[group].ResumeLayout(false);
                }
            }
            ResumeLayout(false);
            PerformLayout();


            UpdateUI();
            UserEvent.productChange += new UserEvent.ProductChange(UpdateUI);
            TeachAction.updateSeatPos += UpdateSeatPos;
        }

        private void ICStatePosPnl_Load(object sender, EventArgs e)
        {
            rBtnScketSelect[0].Checked = true;
            SelectedRbtn = rBtnScketSelect[0];
        }
        private void UpdateUI()
        {
            for(int i =0; i < UserConfig.AllScketC; i++)
            {
                txtScketX[i].Text = Convert.ToString(g_config.ArrFixOut[i].P.X);
                txtScketY[i].Text = Convert.ToString(g_config.ArrFixOut[i].P.Y);
            }
            DispMode1Pic();
        }

        private void UpdateSeatPos(int ind)
        {
            txtScketX[ind].Text = Convert.ToString(g_config.ArrFixOut[ind].P.X);
            txtScketY[ind].Text = Convert.ToString(g_config.ArrFixOut[ind].P.Y);
        }

        public void DispMode1Pic()
        {
            if(!WindowHandle1.IsNull()) HOperatorSet.CloseWindow(WindowHandle1);
            HOperatorSet.OpenWindow(0, 0, modelPicBox1.Width, modelPicBox1.Height, modelPicBox1.Handle, "visible", "", out WindowHandle1);
            String path = g_config.StrProductDir + "\\set_" + GlobConstData.ST_MODELICSTATPOS + ".jpg";
            if (File.Exists(path))//判断文件是否存在
            {
                HObject img;
                _HalconUtil.LoadImgAndDisp(WindowHandle1, out img, path);
                img?.Dispose();
            }
        }
        private void InitRbtn_MouseUp(object sender, MouseEventArgs e)
        {
            SkinRadioButton clickRbtn = sender as SkinRadioButton;
            if (clickRbtn != SelectedRbtn && clickRbtn.Checked)
            {
                SelectedRbtn.Checked = false;
                SelectedRbtn = clickRbtn;
            }
            g_act.CCDCap(GlobConstData.ST_CCDUP);
        }
        private void SavePosBtn_Click(object sender, EventArgs e)
        {
            int ind = Convert.ToInt32(SelectedRbtn.Tag);
            float dx = 0, dy = 0;//当前X，Y轴坐标

            g_config.ArrFixOut[ind].P.X = Axis.trapPrm_X.getPos;
            g_config.ArrFixOut[ind].P.Y = Axis.trapPrm_Y.getPos;
            Axis.Group[ind / UserConfig.ScketUnitC].Unit[ind % UserConfig.ScketUnitC].TopCamera_X = g_config.ArrFixOut[ind].P.X;
            Axis.Group[ind / UserConfig.ScketUnitC].Unit[ind % UserConfig.ScketUnitC].TopCamera_Y = g_config.ArrFixOut[ind].P.Y;
            
            txtScketX[ind].Text = Convert.ToString(g_config.ArrFixOut[ind].P.X);
            txtScketY[ind].Text = Convert.ToString(g_config.ArrFixOut[ind].P.Y);

            g_config.WriteFixOutPos(ind);
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "修改烧录座位置成功,序号[" + ind + "]", "Modify");
        }

        private void modifyBtn_Click(object sender, EventArgs e)
        {
            if (!AltimeterBtn.Checked)//测高仪
            {
                MessageBox.Show(MultiLanguage.GetString("未选择测高仪作为参照,选择后确认位置正确再修改!"));
                return;
            }
            int ind = Convert.ToInt32(SelectedRbtn.Tag);
            double setx, sety;
            setx = Axis.Group[ind / UserConfig.ScketUnitC].Unit[ind % UserConfig.ScketUnitC].TopCamera_X;
            sety = Axis.Group[ind / UserConfig.ScketUnitC].Unit[ind % UserConfig.ScketUnitC].TopCamera_Y;
            setx -= Axis.Altimeter.Offset_TopCamera_X;
            sety -= Axis.Altimeter.Offset_TopCamera_Y;

            Axis.Altimeter.Offset_Socket_X = setx - Axis.trapPrm_X.getPos;
            Axis.Altimeter.Offset_Socket_Y = sety - Axis.trapPrm_Y.getPos;
            g_config.WriteAltimeterHeightValue();
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "修改测高仪位置成功", "Modify");
        }

        private void GO_Socket_Click(object sender, EventArgs e)
        {         
            int ind = Convert.ToInt32(SelectedRbtn.Tag);
            double setx, sety;
            if(!Auto_Flag.SystemBusy)
            {
                setx = Axis.Group[ind / UserConfig.ScketUnitC].Unit[ind % UserConfig.ScketUnitC].TopCamera_X;
                sety = Axis.Group[ind / UserConfig.ScketUnitC].Unit[ind % UserConfig.ScketUnitC].TopCamera_Y;
                
                for (int i = 0; i < UserConfig.VacuumPenC; i++)
                {
                    if (rBtnSelectPen[i].Checked)
                    {
                        setx -= Axis.Pen[i].Offset_TopCamera_X;
                        sety -= Axis.Pen[i].Offset_TopCamera_Y;
                        break;
                    }
                }
                if (ScannerBtn.Checked)
                {
                    setx -= Axis.Scanner.Offset_TopCamera_X;
                    sety -= Axis.Scanner.Offset_TopCamera_Y;
                }
                else if (AltimeterBtn.Checked)
                {
                    setx -= Axis.Altimeter.Offset_TopCamera_X;
                    sety -= Axis.Altimeter.Offset_TopCamera_Y;
                    setx -= Axis.Altimeter.Offset_Socket_X;
                    sety -= Axis.Altimeter.Offset_Socket_Y;
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
                            ZoomLens.SetPos = ZoomLens.OriginPos - Axis.ZoomLens_S.Socket - ZoomLens.NowPos;
                            ZoomLens.MODBUS_DST();
                        }
                        else
                        {
                            MessageBox.Show(MultiLanguage.GetString("通讯异常"));
                        }
                    });
                }
                else if (Config.ZoomLens == 2)
                {
                    ZoomLens.SetPos = Axis.ZoomLens_S.Socket;
                    axis.Camera_Position_Control(ZoomLens.SetPos, true);
                }
            }          
        }
        private void techICModBtn_Click(object sender, EventArgs e)
        {
            _ConInitWnd.IIndexModel = _ConInitWnd.ISelBoxInd = GlobConstData.ST_MODELICSTATPOS;

            g_act.CCDSnap(GlobConstData.ST_CCDUP);
            g_act.WaitDoEvent(300);
            this._ConInitWnd.InitImgModel();
        }

        private void movToICSeatBtn_Click(object sender, EventArgs e)
        {
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "手动校准烧录座位置", "Modify");
            double curX = 0, curY = 0;
            MovToICSeat(ref curY,ref curX);
            if (!Auto_Flag.SystemBusy)
            {
                TeachAction.GO_Start(curX, curY);
            }
            double dy = 0, dx = 0, dr = 0;
            g_act.WaitDoEvent(1000);
            g_act.CCDCap(GlobConstData.ST_CCDUP, true);
        }

        /// <summary>
        /// 自动校正烧录座坐标
        /// </summary>
        /// <returns></returns>
        public bool MovToICSeat(ref double curY,ref double curX)
        {
            double dXpix = 0, dYpix = 0, dRpix = 0, dx = 0, dy = 0, dr = 0;
            g_act.SendCmd(GlobConstData.ST_LIGHTUP, g_config.ArrLampBox[GlobConstData.ST_MODELICSTATPOS].ILightUp);
            g_act.CCDSnap(GlobConstData.ST_CCDUP);
            g_act.WaitDoEvent(300);
            if (!_ConInitWnd.FindIndexImage(GlobConstData.ST_MODELICSTATPOS, ref dYpix, ref dXpix, ref dRpix))
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "自动获取烧录座坐标失败", "Waring");
                return false;
                
            }
            dXpix = (_HalconUtil.IImgCenterX - dXpix) * g_config.ArrCCDPrec[GlobConstData.ST_MODELICSTATACC].X;
            dYpix = (_HalconUtil.IImgCenterY - dYpix) * g_config.ArrCCDPrec[GlobConstData.ST_MODELICSTATACC].Y;
            dx = Axis.trapPrm_X.getPos;
            dy = Axis.trapPrm_Y.getPos;
            curX = Math.Round(dx + dXpix, 3);
            curY = Math.Round(dy - dYpix, 3);

            return true;
        }

        private void BtnReviseSeatPos_Click(object sender, EventArgs e)
        {
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "开始自动校准烧录座位置", "Modify");
            TeachAction.ReviseSeatPos_Start();
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            if (Auto_Flag.LearnBusy && (Auto_Flag.ALarmPause || Auto_Flag.RunPause))
            {
                Auto_Flag.Next = true;
            }
        }

        private void findImgModBtn_Click(object sender, EventArgs e)
        {
            g_act.CCDSnap(GlobConstData.ST_CCDUP);
            g_act.WaitDoEvent(300);
            double dy = 0, dx = 0, dr = 0;
            if (_ConInitWnd.FindIndexImage(GlobConstData.ST_MODELICSTATPOS, ref dy, ref dx, ref dr))
            {
                //offsetXTxT1.Text = dx.ToString("f2");
                //offsetYTxT1.Text = dy.ToString("f2");
                //offsetRTxT1.Text = dr.ToString("f2");
            }
            else
            {
                MessageBox.Show(MultiLanguage.GetString("图像未找到"));
            }
        }
    }
}
