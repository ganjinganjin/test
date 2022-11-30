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
using CCWin.SkinControl;
using System.Threading.Tasks;
using System.Threading;
using BAR.CommonLib_v1._0;

namespace BAR.ControlPanels
{
    public partial class GBPPosPnl : UserControl
    {
        private SkinRadioButton[] rBtnSelectPen;
        private Label[] labSelectPen;
        public RadioButton SelectedRbtn;
        Config          g_config = Config.GetInstance();
        Act             g_act = Act.GetInstance();
        public Axis axis = Axis.GetInstance();
        ConfigInitWnd   _ConInitWnd;

        Label[]         ArrLabs;
        public GBPPosPnl()
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();
            _InitializeComponent();
        }
        public GBPPosPnl(ConfigInitWnd configWnd):this()
        {
            this._ConInitWnd = configWnd;
        }

        private void _InitializeComponent()
        {
            string strx, stry;
            Config.TrayStartDir dir;
            int left = 0;

            ArrLabs = new Label[18];
            ArrLabs[0] = this.pan11XTxt;
            ArrLabs[1] = this.pan2XTxt;
            ArrLabs[2] = this.pan3XTxt;
            ArrLabs[3] = this.pan12XTxt;
            ArrLabs[4] = this.bredeOutX;
            ArrLabs[5] = this.bredeInX;
            ArrLabs[6] = this.nGCupX;
            ArrLabs[7] = this.tubeInX;
            ArrLabs[8] = this.brede2InX;

            ArrLabs[9] = this.pan11YTxt;
            ArrLabs[10] = this.pan2YTxt;
            ArrLabs[11] = this.pan3YTxt;
            ArrLabs[12] = this.pan12YTxt;
            ArrLabs[13] = this.bredeOutY;
            ArrLabs[14] = this.bredeInY;
            ArrLabs[15] = this.nGCupY;
            ArrLabs[16] = this.tubeInY;
            ArrLabs[17] = this.brede2InY;

            this.RBtn2.Text = MultiLanguage.GetString(g_config.Tray1Start == 0 ? "料盘1#坐标(左上角)" : "料盘1#坐标(左下角)");
            this.RBtn3.Text = MultiLanguage.GetString(g_config.Tray1Start == 0 ? "料盘1#坐标(右下角)" : "料盘1#坐标(右上角)");

            dir = g_config.Tray_start(1);
            strx = dir.dx ? "右" : "左";
            stry = dir.dy ? "下" : "上";
            if(!dir.dx && g_config.AutoTrayStart == 1)
            {
                strx = "右";
            }
            this.RBtn6.Text = MultiLanguage.GetString("料盘2#坐标(" + strx + stry + "角)");

            dir = g_config.Tray_start(2);
            strx = dir.dx ? "右" : "左";
            stry = dir.dy ? "下" : "上";
            if (!dir.dx && g_config.NGTrayStart == 1)
            {
                strx = "右";
            }
            this.RBtn1.Text = MultiLanguage.GetString("料盘3#坐标(" + strx + stry + "角)");

            rBtnSelectPen = new SkinRadioButton[UserConfig.VacuumPenC];
            labSelectPen = new Label[UserConfig.VacuumPenC];
            //GO吸笔控件
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
                    Font = new Font("楷体", 8F, System.Drawing.FontStyle.Bold),
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

            UpdateUI();
            UserEvent.productChange += new UserEvent.ProductChange(UpdateUI);
        }
        private void GBPPosPnl_Load(object sender, EventArgs e)
        {
            SelectedRbtn = RBtn1;
            if (Config.FeederCount == 1)
            {
                PnlFeeder2.Visible = true;
            }
        }
        private void UpdateUI()
        {
            for(int i = 0; i< 9; i++)
            {
                Double dx, dy;
                dx = g_config.ArrFixIn[i].P.X;
                dy = g_config.ArrFixIn[i].P.Y;

                if(ArrLabs[i]!= null) ArrLabs[i].Text = String.Format("{0:f2}", dx);
                if(ArrLabs[i]!= null) ArrLabs[9 + i].Text = String.Format("{0:f2}", dy);
            }

            panColTxtBox.Value = new decimal(TrayD.ColC);
            panRowTxtBox.Value = new decimal(TrayD.RowC);
            colDisTxtBox.Value = new decimal(TrayD.Col_Space);
            rowDisTxtBox.Value = new decimal(TrayD.Row_Space);
        }
        private void GoPosBtn_Click(object sender, EventArgs e)
        {
            int ind = Convert.ToInt32(SelectedRbtn.Tag) - 1;
            double setx, sety;
            if (!Auto_Flag.SystemBusy)
            {
                setx = g_config.ArrFixIn[ind].P.X;
                sety = g_config.ArrFixIn[ind].P.Y;

                for (int i = 0; i < UserConfig.VacuumPenC; i++)
                {
                    if (rBtnSelectPen[i].Checked)
                    {
                        setx -= Axis.Pen[i].Offset_TopCamera_X;
                        sety -= Axis.Pen[i].Offset_TopCamera_Y;
                        break;
                    }
                }
                TeachAction.GO_Start(setx, sety);

                if (Config.ZoomLens == 1)
                {
                    ind++;
                    ZoomLens.MODBUS_ReadStatus();
                    Task.Run(() =>
                    {
                        do
                        {
                            Thread.Sleep(5);
                        } while (Axis.ZoomLens_S.ReadStatus_Busy);

                        if (Axis.ZoomLens_S.ReadStatus_Online)
                        {
                            if (ind == 1 || ind == 2 || ind == 3 || ind == 4)
                            {
                                ZoomLens.SetPos = ZoomLens.OriginPos - Axis.ZoomLens_S.Tray - ZoomLens.NowPos;
                            }
                            else if (ind == 6)
                            {
                                ZoomLens.SetPos = ZoomLens.OriginPos - Axis.ZoomLens_S.BredeIn - ZoomLens.NowPos;
                            }
                            else if (ind == 5)
                            {
                                ZoomLens.SetPos = ZoomLens.OriginPos - Axis.ZoomLens_S.BredeOut - ZoomLens.NowPos;
                            }
                            else if (ind == 8)
                            {
                                ZoomLens.SetPos = ZoomLens.OriginPos - Axis.ZoomLens_S.TubeIn - ZoomLens.NowPos;
                            }
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
                    if (ind == 1 || ind == 2 || ind == 3 || ind == 4)
                    {
                        ZoomLens.SetPos = Axis.ZoomLens_S.Tray;
                    }
                    else if (ind == 6)
                    {
                        ZoomLens.SetPos = Axis.ZoomLens_S.BredeIn;
                    }
                    else if (ind == 5)
                    {
                        ZoomLens.SetPos = Axis.ZoomLens_S.BredeOut;
                    }
                    else if (ind == 8)
                    {
                        ZoomLens.SetPos = Axis.ZoomLens_S.TubeIn;
                    }
                    axis.Camera_Position_Control(ZoomLens.SetPos, true);
                }
            }
        }

        private void InitRbtn_MouseUp(object sender, MouseEventArgs e)
        {
            RadioButton clickRbtn = sender as RadioButton;
            if (clickRbtn != SelectedRbtn && clickRbtn.Checked)
            {
                SelectedRbtn.Checked = false;
                SelectedRbtn = clickRbtn;
            }
        }

        private void SavePosBtn_Click(object sender, EventArgs e)
        {
            int ind = Convert.ToInt32(SelectedRbtn.Tag) - 1;     
            g_config.ArrFixIn[ind].P.X = Axis.trapPrm_X.getPos;
            g_config.ArrFixIn[ind].P.Y = Axis.trapPrm_Y.getPos;
            if (ind < 3)
            {
                Axis.Position_X.trayFirst[ind] = g_config.ArrFixIn[ind].P.X;
                Axis.Position_Y.trayFirst[ind] = g_config.ArrFixIn[ind].P.Y;
            }
            else if (ind == 3)
            {
                Axis.Position_X.trayEnd[0] = g_config.ArrFixIn[ind].P.X;
                Axis.Position_Y.trayEnd[0] = g_config.ArrFixIn[ind].P.Y;
            }
            else if (ind == 4)
            {
                Axis.Position_X.bredeOut = g_config.ArrFixIn[ind].P.X;
                Axis.Position_Y.bredeOut = g_config.ArrFixIn[ind].P.Y;
            }
            else if (ind == 5)
            {
                Axis.Position_X.Feeder[0] = g_config.ArrFixIn[ind].P.X;
                Axis.Position_Y.Feeder[0] = g_config.ArrFixIn[ind].P.Y;
            }
            else if (ind == 6)
            {
                Axis.Position_X.NGCup = g_config.ArrFixIn[ind].P.X;
                Axis.Position_Y.NGCup = g_config.ArrFixIn[ind].P.Y;
            }
            else if (ind == 7)
            {
                Axis.Position_X.tubeIn = g_config.ArrFixIn[ind].P.X;
                Axis.Position_Y.tubeIn = g_config.ArrFixIn[ind].P.Y;
            }
            else if (ind == 8)
            {
                Axis.Position_X.Feeder[1] = g_config.ArrFixIn[ind].P.X;
                Axis.Position_Y.Feeder[1] = g_config.ArrFixIn[ind].P.Y;
            }
            ArrLabs[ind].Text = String.Format("{0:f2}", Axis.trapPrm_X.getPos);
            ArrLabs[9 + ind].Text = String.Format("{0:f2}", Axis.trapPrm_Y.getPos);
            g_config.WriteFixInPos(ind);
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "保存位置成功,序号[" + ind + "]", "Modify");
        }

        private void SavePanParaBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(MultiLanguage.GetString("是否修改行列间距"), "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                PointD offSet = new PointD();

                TrayD.ColC = Convert.ToInt32(panColTxtBox.Value);
                TrayD.RowC = Convert.ToInt32(panRowTxtBox.Value);

                offSet.X = Math.Abs(g_config.ArrFixIn[0].P.X - g_config.ArrFixIn[3].P.X);   //行相减
                offSet.Y = Math.Abs(g_config.ArrFixIn[0].P.Y - g_config.ArrFixIn[3].P.Y);   //列相减
                TrayD.Col_Space = offSet.X / (TrayD.ColC - 1);
                TrayD.Row_Space = offSet.Y / (TrayD.RowC - 1);


                colDisTxtBox.Value = new decimal(TrayD.Col_Space);
                rowDisTxtBox.Value = new decimal(TrayD.Row_Space);
                g_config.SavePanVal(); //保存行列间距
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "保存行列间距成功", "Modify");
            }
        }
    }
}
