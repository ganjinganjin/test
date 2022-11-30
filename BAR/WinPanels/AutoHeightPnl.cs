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
    public partial class AutoHeightPnl : UserControl
    {
        private SkinRadioButton[] rBtnSelectPen;
        private SkinGroupBox[] scketGroupBox;
        private Label[] labSelectPen;
        private SkinRadioButton[] rBtnScketSelect;
        //private Label[] labelScketSelect;
        public SkinRadioButton SelectedRbtn;
        public NumericUpDown[] NudTakeHeight;

        ConfigInitWnd   _ConInitWnd;
        Config          g_config = Config.GetInstance();
        Act             g_act = Act.GetInstance();

        public AutoHeightPnl()
        {
            MultiLanguage.SetDefaultLanguage();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
            InitializeComponent();
            _InitializeComponent();
        }
        public AutoHeightPnl(ConfigInitWnd conInitWnd): this()
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
                    Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Pen" + (i + 1) : "吸笔" + (i + 1),
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
            groupH = 35 + 2 * (nH + 1) * layoutMode;
            

            Point[] groupPoint = new Point[colC * rowC];//分组位置
            for (int n = 0; n < rowC; n++)
            {
                for (int m = 0; m < colC; m++)
                {
                    int nIndex = n * colC + m;
                    left = Math.Abs((colC - 1) * 0 - m) * groupW;
                    top = Math.Abs((rowC - 1) * 1 - n) * (groupH + 1);
                    groupPoint[nIndex] = new Point(left, top);
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
                    Font = (UserTask.ProgrammerType == GlobConstData.Programmer_DP && UserConfig.ScketUnitC == 4) ? new Font("楷体", 12F, FontStyle.Bold) : new Font("楷体", 18F, FontStyle.Bold),
                    ForeColor = Color.Black,
                    Location = groupPoint[nIndex],
                    Name = "scketGroupBox" + i,
                    RectBackColor = SystemColors.ActiveCaption,
                    RoundStyle = CCWin.SkinClass.RoundStyle.All,
                    Size = new Size(groupW, groupH),
                    TabIndex = i,
                    TabStop = false,
                    Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "G_" + (i + 1) + "Height" : "组" + (i + 1) + "烧录座高度",
                    TitleBorderColor = SystemColors.ActiveCaption,
                    TitleRectBackColor = SystemColors.ActiveCaption,
                    TitleRoundStyle = CCWin.SkinClass.RoundStyle.All
                };

                scketPosPnl.Controls.Add(scketGroupBox[i]);
                scketGroupBox[i].SuspendLayout();
            }

            //烧录座子坐标相关控件
            rBtnScketSelect = new SkinRadioButton[UserConfig.AllScketC + 4];
            //labelScketSelect = new Label[UserConfig.AllScketC];
            NudTakeHeight = new NumericUpDown[UserConfig.AllScketC + 4];
            colC = UserConfig.ScketUnitC / layoutMode;
            if (colC == 0)
            {
                colC = 1;
            }
            nW = (groupW - 6) / colC;
            leftStart = (groupW - nW * colC + 1) / 2 + 1;

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
                    if (UserTask.ProgrammerType == GlobConstData.Programmer_DP && (UserConfig.ScketUnitC == 16 || UserConfig.ScketUnitC == 4))
                    {
                        top += (layoutMode - 1 - ((i % UserConfig.ScketUnitC) / colC)) * (2 * (nH + 1) + topStart);
                    }
                    else
                    {
                        top += ((i % UserConfig.ScketUnitC) / colC) * (2 * (nH + 1) + topStart);
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
                    Location = new Point(left, groupH - top + 1),
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


                NudTakeHeight[i] = new NumericUpDown
                {
                    DecimalPlaces = 1,
                    Font = new System.Drawing.Font("楷体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134))),
                    Location = new Point(left, groupH - top - nH - 1),

                    Name = "NudTakeHeight" + i,
                    Size = new Size(nW - 2, nH),
                    TabIndex = i,
                    TextAlign = System.Windows.Forms.HorizontalAlignment.Center,
                };
                NudTakeHeight[i].Increment = 0.1M;
                NudTakeHeight[i].Maximum = new decimal(new int[] {
                100,
                0,
                0,
                0});
                NudTakeHeight[i].Minimum = new decimal(new int[] {
                100,
                0,
                0,
                -2147483648});
                
                int group = i / UserConfig.ScketUnitC;
                scketGroupBox[group].Controls.Add(rBtnScketSelect[i]);
                //scketGroupBox[group].Controls.Add(labelScketSelect[i]);
                scketGroupBox[group].Controls.Add(NudTakeHeight[i]);
            }

            String ctlName;
            Control[] tagAry;
            for (int i = 0; i < 4; i++)
            {
                ctlName = "numericUpDown" + (i + 1);
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    NudTakeHeight[UserConfig.AllScketC + i] = (tagAry.First() as NumericUpDown);
                }

                ctlName = "skinRadioButton" + (i + 1);
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    rBtnScketSelect[UserConfig.AllScketC + i] = (tagAry.First() as SkinRadioButton);
                    rBtnScketSelect[UserConfig.AllScketC + i].MouseUp += new MouseEventHandler(InitRbtn_MouseUp);
                    rBtnScketSelect[UserConfig.AllScketC + i].Tag = UserConfig.AllScketC + i;
                }
            }
            ResumeLayout(false);
            PerformLayout();


            UpdateUI();
            UserEvent.productChange += new UserEvent.ProductChange(UpdateUI);
            TeachAction.updateHeight += UpdateHeight;

            if (UserConfig.VacuumPenC == 2)
            {
                BtnSaveZ3Height.Visible = false;
                BtnGOZ3Height.Visible = false;
                BtnSaveZ4Height.Visible = false;
                BtnGOZ4Height.Visible = false;
            }
            
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
                NudTakeHeight[i].Value = new decimal(Axis.Group[i / UserConfig.ScketUnitC].Unit[i % UserConfig.ScketUnitC].HeightVal);
            }
            NudTakeHeight[UserConfig.AllScketC].Value = new decimal(HeightVal_Altimeter.Tray);
            NudTakeHeight[UserConfig.AllScketC + 1].Value = new decimal(HeightVal_Altimeter.BredeIn);
            NudTakeHeight[UserConfig.AllScketC + 2].Value = new decimal(HeightVal_Altimeter.BredeOut);
            NudTakeHeight[UserConfig.AllScketC + 3].Value = new decimal(HeightVal_Altimeter.TubeIn);

            ValueEmptyICLab.Text = Axis.Altimeter.ValueEmptyIC.ToString();
            ValueExistICLab.Text = Axis.Altimeter.ValueExistIC.ToString();
            ICHeightLab.Text = Axis.Altimeter.Thickness.ToString();
            HeightDifference.Value = Convert.ToDecimal(Axis.Altimeter.HeightDifference);
            NudCalibrating_Z.Value = Convert.ToDecimal(Axis.Altimeter.Calibrating_Z);
        }

        private void UpdateHeight(int ind, double  value)
        {
            NudTakeHeight[ind].Value = new decimal(value);
        }

        
        private void InitRbtn_MouseUp(object sender, MouseEventArgs e)
        {
            SkinRadioButton clickRbtn = sender as SkinRadioButton;
            if (clickRbtn != SelectedRbtn && clickRbtn.Checked)
            {
                SelectedRbtn.Checked = false;
                SelectedRbtn = clickRbtn;
            }
        }
        

        private void SaveValueEmptyICBtn_Click(object sender, EventArgs e)
        {
            if (Axis.Altimeter.ReadAI_Busy)
            {
                return;
            }
            Cathetometer.MODBUS_ReadAI();
            Task.Run(() =>
            {
                do
                {
                    Thread.Sleep(5);
                } while (Axis.Altimeter.ReadAI_Busy);
                string strShow;
                if (Axis.Altimeter.ReadAI_Online)
                {
                    if (Cathetometer.Value <= 0)
                    {
                        strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Failed to save. The height value is invalid" : "保存失败，高度值不合法";
                        MessageBox.Show(strShow);
                        return;
                    }
                    Axis.Altimeter.ValueEmptyIC = Cathetometer.Value;
                    ValueEmptyICLab.Text = Cathetometer.Value.ToString();
                    g_config.WriteAltimeterHeightValue();
                    g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "保存烧录座无IC测高值成功", "Modify");
                }
                else
                {
                    strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Failed to save. The communication is abnormal" : "保存失败，通讯异常";
                    MessageBox.Show(strShow);
                }
            });
        }

        private void BtnSaveHeight_Click(object sender, EventArgs e)
        {
            string strShow;
            if (Axis.Altimeter.ReadAI_Busy)
            {
                return;
            }
            if (!AltimeterBtn.Checked)//测高仪
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "No altimeter is selected as a reference, confirm the correct position after selection and save!" : "未选择测高仪作为参照，选择后确认位置正确再保存!";
                MessageBox.Show(strShow);
                return;
            }
            int ind = Convert.ToInt32(SelectedRbtn.Tag);
            Cathetometer.MODBUS_ReadAI();
            Task.Run(() =>
            {
                do
                {
                    Thread.Sleep(5);
                } while (Axis.Altimeter.ReadAI_Busy);

                if (Axis.Altimeter.ReadAI_Online)
                {
                    if (Cathetometer.Value <= 0)
                    {
                        strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Failed to save. The height value is invalid" : "保存失败，高度值不合法";
                        MessageBox.Show(strShow);
                        return;
                    }
                    if (ind < UserConfig.AllScketC)//烧录座
                    {
                        Axis.Group[ind / UserConfig.ScketUnitC].Unit[ind % UserConfig.ScketUnitC].HeightVal = Cathetometer.Value;
                        g_config.WriteSocketAltimeterHeightValue(ind);
                    }
                    else
                    {
                        if (ind == UserConfig.AllScketC)//料盘
                        {
                            HeightVal_Altimeter.Tray = Cathetometer.Value;
                        }
                        else if (ind == UserConfig.AllScketC + 1)//飞达
                        {
                            HeightVal_Altimeter.BredeIn = Cathetometer.Value;
                        }
                        else if (ind == UserConfig.AllScketC + 2)//编带
                        {
                            HeightVal_Altimeter.BredeOut = Cathetometer.Value;
                        }
                        else if (ind == UserConfig.AllScketC + 3)//料管
                        {
                            HeightVal_Altimeter.TubeIn = Cathetometer.Value;
                        }
                        g_config.WriteAltimeterHeightValue();
                    }
                    NudTakeHeight[ind].Value = new decimal(Cathetometer.Value);
                    g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "保存高度成功", "Modify");
                }
                else
                {
                    strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Failed to save. The communication is abnormal" : "保存失败，通讯异常";
                    MessageBox.Show(strShow);
                }
            }); 
        }

        private void SaveValueExistICBtn_Click(object sender, EventArgs e)
        {
            if (Axis.Altimeter.ReadAI_Busy)
            {
                return;
            }
            Cathetometer.MODBUS_ReadAI();
            Task.Run(() =>
            {
                do
                {
                    Thread.Sleep(5);
                } while (Axis.Altimeter.ReadAI_Busy);
                string strShow;
                if (Axis.Altimeter.ReadAI_Online)
                {
                    if (Cathetometer.Value <= 0)
                    {
                        strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Failed to save. The height value is invalid" : "保存失败，高度值不合法";
                        MessageBox.Show(strShow);
                        return;
                    }
                    Axis.Altimeter.ValueExistIC = Cathetometer.Value;
                    ValueExistICLab.Text = Cathetometer.Value.ToString();
                    g_config.WriteAltimeterHeightValue();
                    g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "保存烧录座有IC测高值成功", "Modify");
                }
                else
                {
                    strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Failed to save. The communication is abnormal" : "保存失败，通讯异常";
                    MessageBox.Show(strShow);
                }
            });
        }

        private void BtnSavaICHeight_Click(object sender, EventArgs e)
        {
            Axis.Altimeter.Thickness = Axis.Altimeter.ValueEmptyIC - Axis.Altimeter.ValueExistIC;
            ICHeightLab.Text = Axis.Altimeter.Thickness.ToString();
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "计算高度差成功", "Modify");
        }

        private void BtnAutoGetHeight_Click(object sender, EventArgs e)
        {
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "自动获取高度开始", "Modify");
            TeachAction.MeasuringHeight_Start();
        }

        private void BtnGOHeight_Click(object sender, EventArgs e)
        {
            int ind = Convert.ToInt32(SelectedRbtn.Tag);
            double setx = 0, sety = 0, setz = 0;
            if (!Auto_Flag.SystemBusy)
            {
                if (ind < UserConfig.AllScketC)//烧录座
                {
                    setx = Axis.Group[ind / UserConfig.ScketUnitC].Unit[ind % UserConfig.ScketUnitC].TopCamera_X;
                    sety = Axis.Group[ind / UserConfig.ScketUnitC].Unit[ind % UserConfig.ScketUnitC].TopCamera_Y;
                    setz = Axis.Group[ind / UserConfig.ScketUnitC].Unit[ind % UserConfig.ScketUnitC].HeightVal - Axis.Altimeter.Thickness;
                    if (!(Auto_Flag.Flip && (UserTask.ProgrammerType == GlobConstData.Programmer_WG || UserTask.ProgrammerType == GlobConstData.Programmer_YED || UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY)))//压座
                    {
                        if (UserTask.ProgrammerType != GlobConstData.Programmer_RD && !AltimeterBtn.Checked)
                        {
                            In_Output.pushSeatO[ind / UserConfig.ScketMotionC].M = true;
                        }
                    }
                }
                else if (ind == UserConfig.AllScketC)//料盘
                {
                    setx = Axis.Position_X.trayFirst[0];
                    sety = Axis.Position_Y.trayFirst[0];
                    setz = HeightVal_Altimeter.Tray;
                }
                else if (ind == UserConfig.AllScketC + 1)//飞达
                {
                    setx = Axis.Position_X.Feeder[0];
                    sety = Axis.Position_Y.Feeder[0];
                    setz = HeightVal_Altimeter.BredeIn;
                }
                else if (ind == UserConfig.AllScketC + 2)//编带
                {
                    setx = Axis.Position_X.bredeOut;
                    sety = Axis.Position_Y.bredeOut;
                    setz = HeightVal_Altimeter.BredeOut;
                }
                else if (ind == UserConfig.AllScketC + 3)//料管
                {
                    setx = Axis.Position_X.tubeIn;
                    sety = Axis.Position_Y.tubeIn;
                    setz = HeightVal_Altimeter.TubeIn;
                }

                if (AltimeterBtn.Checked)//测高仪
                {
                    setx -= Axis.Altimeter.Offset_TopCamera_X;
                    sety -= Axis.Altimeter.Offset_TopCamera_Y;
                    if (ind < UserConfig.AllScketC)//烧录座
                    {
                        setx -= Axis.Altimeter.Offset_Socket_X;
                        sety -= Axis.Altimeter.Offset_Socket_Y;
                    }
                    TeachAction.GO_Start(setx, sety);
                }
                else//吸笔
                {
                    for (int i = 0; i < UserConfig.VacuumPenC; i++)
                    {
                        if (rBtnSelectPen[i].Checked)
                        {
                            setx -= Axis.Pen[i].Offset_TopCamera_X;
                            sety -= Axis.Pen[i].Offset_TopCamera_Y;
                            setz -= Axis.Pen[i].Altimeter_Z;
                            TeachAction.GO_Start(setx, sety, setz, i);
                            break;
                        }
                    }
                }
            }
        }
        private void BtnSavaAltimeterHeight_Click(object sender, EventArgs e)
        {
            if (Axis.Altimeter.ReadAI_Busy)
            {
                return;
            }
            Cathetometer.MODBUS_ReadAI();
            Task.Run(() =>
            {
                do
                {
                    Thread.Sleep(5);
                } while (Axis.Altimeter.ReadAI_Busy);
                string strShow;
                if (Axis.Altimeter.ReadAI_Online)
                {
                    if (Cathetometer.Value <= 0)
                    {
                        strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Failed to save. The height value is invalid" : "保存失败，高度值不合法";
                        MessageBox.Show(strShow);
                        return;
                    }
                    Axis.Altimeter.Calibrating_Z = Cathetometer.Value;
                    NudCalibrating_Z.Value = Convert.ToDecimal(Axis.Altimeter.Calibrating_Z);
                    g_config.WriteMachineInfo();
                    g_config.GetHeight_AltimeterToPen();
                    g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "保存测高仪到定位孔高度成功", "Modify");
                }
                else
                {
                    strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Failed to save. The communication is abnormal" : "保存失败，通讯异常";
                    MessageBox.Show(strShow);
                }
            });
        }

        private void BtnGOAltimeterHeight_Click(object sender, EventArgs e)
        {
            TeachAction.GO_Start(Axis.Altimeter.Calibrating_X, Axis.Altimeter.Calibrating_Y);
        }

        private void BtnSaveZHeight_Click(object sender, EventArgs e)
        {
            SkinButton btn = sender as SkinButton;
            int ind = Convert.ToInt32(btn.Tag);
            int num = UserTask.PenType == 1 ? 0 : ind;
            Axis.Pen[ind].Calibrating_Z = Axis.trapPrm_Z[num].getPos;
            g_config.WriteMachineInfo();
            g_config.GetHeight_AltimeterToPen();
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "保存吸笔[" + (ind + 1) + "]到定位孔高度成功", "Modify");
        }

        private void BtnGOZHeight_Click(object sender, EventArgs e)
        {
            SkinButton btn = sender as SkinButton;
            int ind = Convert.ToInt32(btn.Tag);
            double setx, sety, setz;
            setx = g_config.ArrPickPos[0].X - Axis.Pen[ind].Offset_TopCamera_X;
            sety = g_config.ArrPickPos[0].Y - Axis.Pen[ind].Offset_TopCamera_Y;
            setz = Axis.Pen[ind].Calibrating_Z;
            TeachAction.GO_Start(setx, sety, setz, ind);
        }

        private void BtnModify_Click(object sender, EventArgs e)
        {
            string strShow;
            strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Whether to correct the height from the altimeter to the positioning hole" : "是否修正测高仪到定位孔高度";
            if (MessageBox.Show(strShow, "Information：", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Axis.Altimeter.Calibrating_Z = Convert.ToDouble(NudCalibrating_Z.Value);
                g_config.WriteMachineInfo();
                g_config.GetHeight_AltimeterToPen();
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "修正测高仪到定位孔高度成功", "Modify");
            }
        }

        private void BtnSaveHeightDifference_Click(object sender, EventArgs e)
        {
            string strShow;
            strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Whether to save the height is poor" : "是否保存高度差";
            if (MessageBox.Show(strShow, "Information：", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Axis.Altimeter.HeightDifference = Convert.ToDouble(HeightDifference.Value);
                g_config.WriteAltimeterHeightValue();
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "保存高度差", "Modify");
            }
        }
    }
}
