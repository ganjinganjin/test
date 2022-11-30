using BAR.Commonlib;
using BAR.CommonLib_v1._0;
using CCWin.SkinControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BAR.Windows
{
    public partial class CylinderCtlWnd : Form
    {
        public Act g_act = Act.GetInstance();
        private SkinButton[] btnBurn;
        private SkinButton[] btnCylinder;
        private SkinButton[] btnVacuum;
        private SkinButton[] btnBlow;
        private SkinButton[] btnPen;
        private Label [] ledBurn;
        private Label[] ledCylinder;
        private Label[] ledVacuum;
        private Label[] ledBlow;
        private Label[] ledPen;
        public CylinderCtlWnd()
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();
            if (Config.BredeDotCount == 1)//编带双打点
            {
                skinButton2.Visible = true;
                skinButton60.Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English? "Dot_A" : "编带手动打点A";
            }
            _InitializeComponent();
        }

        private void _InitializeComponent()
        {
            int nW = 100, nH = 40;
            int left = 0, top = 0;
            string font = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Arial" : "楷体";
            //烧录手动触发控件
            btnBurn = new SkinButton[UserConfig.ScketGroupC];
            ledBurn = new Label[UserConfig.ScketGroupC];
            for (int i = 0; i < UserConfig.ScketGroupC; i++)
            {
                left = (i % 4) * 150;
                top = (i / 4) * (6 + nH);
                btnBurn[i] = new SkinButton
                {
                    BackColor = Color.Transparent,
                    BaseColor = Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224))))),
                    BorderColor = Color.Silver,
                    ControlState = CCWin.SkinClass.ControlState.Normal,
                    DownBack = null,
                    Font = new Font(font, 12F, FontStyle.Bold),
                    ForeColor = SystemColors.ActiveCaptionText,
                    ImeMode = ImeMode.NoControl,
                    Location = new Point(20 + left,top),
                    MouseBack = null,
                    Name = "btnManualDown" + i,
                    NormlBack = null,
                    Size = new Size(nW, nH),
                    TabIndex = i,
                    Tag = i,
                    Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Group " + (i + 1) : "第" + (i + 1) + "组",
                    UseVisualStyleBackColor = false
                };
                btnBurn[i].Click += new EventHandler(ManualDownBtn_Click);

                ledBurn[i] = new Label
                {
                    BackColor = Color.Maroon,
                    BorderStyle = BorderStyle.FixedSingle,
                    FlatStyle = FlatStyle.Flat,
                    Location = new Point(125 + left, top),
                    Name = "ledManualDown" + i,
                    Size = new Size(25, nH),
                    TabIndex = i,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                skinPanel1.Controls.Add(btnBurn[i]);
                skinPanel1.Controls.Add(ledBurn[i]);
            }
            //气缸手动操作控件
            int ratio1 = UserTask.ProgrammerType == GlobConstData.Programmer_RD || UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY ? 2 : 1;
            int ratio2 = !Auto_Flag.Flip && UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY ? 1 : ratio1;
            btnCylinder = new SkinButton[UserConfig.AllMotionC * ratio1];
            ledCylinder = new Label[UserConfig.AllMotionC * ratio1];
            for (int i = 0; i < UserConfig.AllMotionC * ratio1; i++)
            {
                string str = Auto_Flag.Flip && (UserTask.ProgrammerType == GlobConstData.Programmer_WG || UserTask.ProgrammerType == GlobConstData.Programmer_YED) ? "翻盖" : "压座";
                if (UserTask.ProgrammerType == GlobConstData.Programmer_RD || (Auto_Flag.Flip && UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY))
                {
                    if ((i % (UserConfig.AllMotionC * ratio1) % ratio1) == 0)
                    {
                        str = "推座";
                    }
                }
                if (MultiLanguage.DefaultLanguage == GlobConstData.ST_English)
                {
                    switch (str)
                    {
                        case "翻盖":
                            str = "Flip";
                            break;
                        case "压座":
                            str = "Wedge";
                            break;
                        case "推座":
                            str = "Push";
                            break;
                        default:
                            break;
                    }
                }

                left = (i % 4) * 150;
                top = (i / 4) * (6 + nH);
                btnCylinder[i] = new SkinButton
                {
                    BackColor = Color.Transparent,
                    BaseColor = Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224))))),
                    BorderColor = Color.Silver,
                    ControlState = CCWin.SkinClass.ControlState.Normal,
                    DownBack = null,
                    Font = new Font(font, 12F, FontStyle.Bold),
                    ForeColor = SystemColors.ActiveCaptionText,
                    ImeMode = ImeMode.NoControl,
                    Location = new Point(20 + left, top),
                    MouseBack = null,
                    Name = "btnCylinder" + i,
                    NormlBack = null,
                    Size = new Size(nW, nH),
                    TabIndex = i,
                    Tag = i,
                    Text = (i / (UserConfig.MotionGroupC * ratio2) + 1) + "_" + (i % (UserConfig.MotionGroupC * ratio2) / ratio2 + 1) + " " + str,
                    UseVisualStyleBackColor = false
                };
                btnCylinder[i].Click += new EventHandler(CylinderBtn_Click);

                ledCylinder[i] = new Label
                {
                    BackColor = Color.Maroon,
                    BorderStyle = BorderStyle.FixedSingle,
                    FlatStyle = FlatStyle.Flat,
                    Location = new Point(125 + left, top),
                    Name = "ledCylinder" + i,
                    Size = new Size(25, nH),
                    TabIndex = i,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                skinPanel2.Controls.Add(btnCylinder[i]);
                skinPanel2.Controls.Add(ledCylinder[i]);
                if (!Auto_Flag.Flip && UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY && i >= UserConfig.AllMotionC)
                {
                    btnCylinder[i].Visible = false;
                    ledCylinder[i].Visible = false;
                }
            }

            nW = 108;
            nH = 50;
            //真空、吹气手动操作控件
            btnVacuum = new SkinButton[UserConfig.VacuumPenC];
            btnBlow = new SkinButton[UserConfig.VacuumPenC];
            btnPen = new SkinButton[UserConfig.VacuumPenC];
            ledVacuum = new Label[UserConfig.VacuumPenC];
            ledBlow = new Label[UserConfig.VacuumPenC];
            ledPen = new Label[UserConfig.VacuumPenC];

            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                left = (i % 4) * 150;
                btnVacuum[i] = new SkinButton
                {
                    BackColor = Color.Transparent,
                    BaseColor = Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224))))),
                    BorderColor = Color.Silver,
                    ControlState = CCWin.SkinClass.ControlState.Normal,
                    DownBack = null,
                    Font = new Font(font, 12F, FontStyle.Bold),
                    ForeColor = SystemColors.ActiveCaptionText,
                    ImeMode = ImeMode.NoControl,
                    Location = new Point(20 + left, 0),
                    MouseBack = null,
                    Name = "btnVacuum" + i,
                    NormlBack = null,
                    Size = new Size(nW, nH),
                    TabIndex = i,
                    Tag = i,
                    Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Z" + (i + 1) + " Vacuo" : "吸嘴Z" + (i + 1) + "吸气",
                    UseVisualStyleBackColor = false
                };
                btnVacuum[i].Click += new EventHandler(VacuumBtn_Click);

                ledVacuum[i] = new Label
                {
                    BackColor = Color.Maroon,
                    BorderStyle = BorderStyle.FixedSingle,
                    FlatStyle = FlatStyle.Flat,
                    Location = new Point(130 + left, 0),
                    Name = "ledVacuum" + i,
                    Size = new Size(25, nH),
                    TabIndex = i,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                btnBlow[i] = new SkinButton
                {
                    BackColor = Color.Transparent,
                    BaseColor = Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224))))),
                    BorderColor = Color.Silver,
                    ControlState = CCWin.SkinClass.ControlState.Normal,
                    DownBack = null,
                    Font = new Font(font, 12F, FontStyle.Bold),
                    ForeColor = SystemColors.ActiveCaptionText,
                    ImeMode = ImeMode.NoControl,
                    Location = new Point(20 + left, 60),
                    MouseBack = null,
                    Name = "btnBlow" + i,
                    NormlBack = null,
                    Size = new Size(nW, nH),
                    TabIndex = i,
                    Tag = i,
                    Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Z" + (i + 1) + " Blow" : "吸嘴Z" + (i + 1) + "吹气",
                    UseVisualStyleBackColor = false
                };
                btnBlow[i].Click += new EventHandler(BlowBtn_Click);

                ledBlow[i] = new Label
                {
                    BackColor = Color.Maroon,
                    BorderStyle = BorderStyle.FixedSingle,
                    FlatStyle = FlatStyle.Flat,
                    Location = new Point(130 + left, 60),
                    Name = "ledBlow" + i,
                    Size = new Size(25, nH),
                    TabIndex = i,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                btnPen[i] = new SkinButton
                {
                    BackColor = Color.Transparent,
                    BaseColor = Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224))))),
                    BorderColor = Color.Silver,
                    ControlState = CCWin.SkinClass.ControlState.Normal,
                    DownBack = null,
                    Font = new Font(font, 12F, FontStyle.Bold),
                    ForeColor = SystemColors.ActiveCaptionText,
                    ImeMode = ImeMode.NoControl,
                    Location = new Point(20 + left, 120),
                    MouseBack = null,
                    Name = "btnPen" + i,
                    NormlBack = null,
                    Size = new Size(nW, nH),
                    TabIndex = i,
                    Tag = i,
                    Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Z" + (i + 1) + " Action" : "吸嘴Z" + (i + 1) + "动作",
                    UseVisualStyleBackColor = false
                };
                btnPen[i].Click += new EventHandler(PenBtn_Click);

                ledPen[i] = new Label
                {
                    BackColor = Color.Maroon,
                    BorderStyle = BorderStyle.FixedSingle,
                    FlatStyle = FlatStyle.Flat,
                    Location = new Point(130 + left, 120),
                    Name = "ledPen" + i,
                    Size = new Size(25, nH),
                    TabIndex = i,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                skinPanel3.Controls.Add(btnVacuum[i]);
                skinPanel3.Controls.Add(ledVacuum[i]);
                skinPanel3.Controls.Add(btnBlow[i]);
                skinPanel3.Controls.Add(ledBlow[i]);
                if (UserTask.PenType == 1)
                {
                    skinPanel3.Controls.Add(btnPen[i]);
                    skinPanel3.Controls.Add(ledPen[i]);
                }
            }
            UserEvent.renovateBtn_Click += new UserEvent.Button_Click(RenovateBtn_Click);
        }

        private void ManualDownBtn_Click(object sender, EventArgs e)
        {
            SkinButton btn = sender as SkinButton;
            int ind = Convert.ToInt32(btn.Tag);
            if (ind >= UserConfig.ScketGroupC)
            {
                return;
            }
            if (Axis.Group[ind].Down.Busy || Auto_Flag.AutoRunBusy)
            {
                return;
            }
            if (UserTask.ProgrammerType == GlobConstData.Programmer_YED)
            {
                Axis.Group[ind].Scan.Trigger = true;//组启动烧录
                Axis.Group[ind].Scan.Busy = true;
                Axis.Group[ind].Down.Busy = true;
            }
            else
            {
                Axis.Group[ind].Down.Trigger = true;//组启动烧录
                Axis.Group[ind].Down.Busy = true;
            }
        }

        private void CylinderBtn_Click(object sender, EventArgs e)
        {
            SkinButton btn = sender as SkinButton;
            int ind = Convert.ToInt32(btn.Tag);
            if (!Auto_Flag.AutoRunBusy)
            {
                if (Auto_Flag.Flip && (UserTask.ProgrammerType == GlobConstData.Programmer_WG || UserTask.ProgrammerType == GlobConstData.Programmer_YED))
                {
                    In_Output.flipO[ind].M = !In_Output.flipO[ind].M;
                }
                else
                {
                    if (UserTask.ProgrammerType == GlobConstData.Programmer_RD || (Auto_Flag.Flip && UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY))
                    {
                        if (!Axis.Group[ind / (UserConfig.MotionGroupC * 2)].Down.Busy)
                        {
                            if ((ind % (UserConfig.AllMotionC * 2) % 2) == 0)
                            {
                                In_Output.flipO[ind / 2].M = !In_Output.flipO[ind / 2].M;
                            }
                            else
                            {
                                In_Output.pushSeatO[ind / 2].M = !In_Output.pushSeatO[ind / 2].M;
                            }
                        }
                    }
                    else
                    {
                        In_Output.pushSeatO[ind].M = !In_Output.pushSeatO[ind].M;
                    }  
                }
            }
        }

        private void RenovateBtn_Click(object sender, EventArgs e)
        {
            int ratio1 = UserTask.ProgrammerType == GlobConstData.Programmer_RD || UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY ? 2 : 1;
            int ratio2 = !Auto_Flag.Flip && UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY ? 1 : ratio1;
            for (int i = 0; i < UserConfig.AllMotionC * ratio1; i++)
            {
                string str = Auto_Flag.Flip && (UserTask.ProgrammerType == GlobConstData.Programmer_WG || UserTask.ProgrammerType == GlobConstData.Programmer_YED) ? "翻盖" : "压座";
                if (UserTask.ProgrammerType == GlobConstData.Programmer_RD || (Auto_Flag.Flip && UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY))
                {
                    if ((i % (UserConfig.AllMotionC * ratio1) % ratio1) == 0)
                    {
                        str = "推座";
                    }
                }
                if (MultiLanguage.DefaultLanguage == GlobConstData.ST_English)
                {
                    switch (str)
                    {
                        case "翻盖":
                            str = "Flip";
                            break;
                        case "压座":
                            str = "Wedge";
                            break;
                        case "推座":
                            str = "Push";
                            break;
                        default:
                            break;
                    }
                }
                btnCylinder[i].Text = (i / (UserConfig.MotionGroupC * ratio2) + 1) + "_" + (i % (UserConfig.MotionGroupC * ratio2) / ratio2 + 1) + " " + str;
                if (UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY && i >= UserConfig.AllMotionC)
                {
                    btnCylinder[i].Visible = Auto_Flag.Flip;
                    ledCylinder[i].Visible = Auto_Flag.Flip;
                }
            }
            for (int i = 0; i < UserConfig.AllMotionC; i++)
            {
                if (UserTask.ProgrammerType == GlobConstData.Programmer_WG || UserTask.ProgrammerType == GlobConstData.Programmer_YED || UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY)
                {
                    In_Output.flipO[i].M = false;
                }
                if (UserTask.ProgrammerType != GlobConstData.Programmer_RD)
                {
                    In_Output.pushSeatO[i].M = false;
                }
            }
        }

        private void VacuumBtn_Click(object sender, EventArgs e)
        {
            SkinButton btn = sender as SkinButton;
            int ind = Convert.ToInt32(btn.Tag);
            if (!Auto_Flag.AutoRunBusy)
            {
                In_Output.vacuumO[ind].M = !In_Output.vacuumO[ind].M;
            }
        }

        private void BlowBtn_Click(object sender, EventArgs e)
        {
            SkinButton btn = sender as SkinButton;
            int ind = Convert.ToInt32(btn.Tag);
            if (!Auto_Flag.AutoRunBusy)
            {
                In_Output.blowO[ind].M = !In_Output.blowO[ind].M;
            }
        }

        private void PenBtn_Click(object sender, EventArgs e)
        {
            SkinButton btn = sender as SkinButton;
            int ind = Convert.ToInt32(btn.Tag);
            if (!Auto_Flag.AutoRunBusy)
            {
                In_Output.penO[ind].M = !In_Output.penO[ind].M;
            }
        }

        private void CylinderCtlWnd_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        public void __InitWnd()
        {
            if (Auto_Flag.Brede_LayIC && !Auto_Flag.AutoRunBusy)
            {
                Brede.MODBUS_WriteBredeParameter();
            }
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Auto_Flag.AutoRunBusy)
            {
                return;
            }

            if (Brede_Flag.Manual_Brede)
            {
                LedManual_Brede.BackColor = Color.Lime;
            }
            else
            {
                LedManual_Brede.BackColor = Color.Maroon;
            }

            if (AutoTray_Process.ManualReceiveTray)
            {
                LedManual_ReceiveTray.BackColor = Color.Lime;
            }
            else
            {
                LedManual_ReceiveTray.BackColor = Color.Maroon;
            }

            if (AutoTray_Process.ManualTakeTray)
            {
                LedManual_TakeTray.BackColor = Color.Lime;
            }
            else
            {
                LedManual_TakeTray.BackColor = Color.Maroon;
            }

            for (int i = 0; i < UserConfig.ScketGroupC; i++)
            {
                SetLed(ledBurn[i], Axis.Group[i].Down.Busy && !Auto_Flag.AutoRunBusy);
            }

            int ratio = UserTask.ProgrammerType == GlobConstData.Programmer_RD || UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY ? 2 : 1;
            for (int i = 0; i < UserConfig.AllMotionC * ratio; i++)
            {
                if (Auto_Flag.Flip && (UserTask.ProgrammerType == GlobConstData.Programmer_WG || UserTask.ProgrammerType == GlobConstData.Programmer_YED))
                {
                    SetLed(ledCylinder[i], In_Output.flipO[i].M && !Auto_Flag.AutoRunBusy);
                }
                else
                {
                    bool output = false;
                    if (UserTask.ProgrammerType == GlobConstData.Programmer_RD || (Auto_Flag.Flip && UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY))
                    {
                        if ((i % (UserConfig.AllMotionC * 2) % 2) == 0)
                        {
                            output = In_Output.flipO[i / 2].M;
                        }
                        else
                        {
                            output = In_Output.pushSeatO[i / 2].M;
                        }
                    }
                    else
                    {
                        if (!Auto_Flag.Flip && UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY && i == UserConfig.AllMotionC)
                        {
                            break;
                        }
                        output = In_Output.pushSeatO[i].M;
                    }
                    SetLed(ledCylinder[i], output && !Auto_Flag.AutoRunBusy);
                }
            }

            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                SetLed(ledVacuum[i], In_Output.vacuumO[i].M && !Auto_Flag.AutoRunBusy);
                SetLed(ledBlow[i], In_Output.blowO[i].M && !Auto_Flag.AutoRunBusy);
                SetLed(ledPen[i], In_Output.penO[i].M && !Auto_Flag.AutoRunBusy);
            }
        }
        public void SetLed(Label lBLed, bool IsBright)
        {
            if (IsBright)
            {
                lBLed.BackColor = Color.Lime;
            }
            else
            {
                lBLed.BackColor = Color.Maroon;
            }
        }

        private void CylinderCtlWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
            e.Cancel = true;
            Hide();
        }

        private void skinButton61_Click(object sender, EventArgs e)
        {
            Brede.Send_Cmd(Brede.Cmd_RMInchFwd);
        }

        private void skinButton59_Click(object sender, EventArgs e)
        {
            Brede.Send_Cmd(Brede.Cmd_RMInchRev);
        }

        private void skinButton60_Click(object sender, EventArgs e)
        {
            Brede.Send_Cmd(Brede.Cmd_MarkA);
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            Brede.Send_Cmd(Brede.Cmd_MarkB);
        }

        private void skinButton4_Click(object sender, EventArgs e)
        {
            Brede.Send_Cmd(Brede.Cmd_HotMelt);
        }

        private void skinButton6_Click(object sender, EventArgs e)
        {
            Brede.Send_Cmd(Brede.Cmd_ReceiveMaterial);
        }

        private void skinButton7_Click(object sender, EventArgs e)
        {
            Brede.Send_Cmd(Brede.Cmd_SendMaterial);
            if (UserTask.FeederIO)
            {
                UserTask.FeederTrigge(0);
            }
        }

        private void skinButton5_Click(object sender, EventArgs e)
        {
            if (!Auto_Flag.AutoRunBusy && Auto_Flag.Brede_LayIC)
            {
                if (!Brede_Flag.Manual_Brede)
                {
                    Brede_Trigger.Manual_Brede = true;
                }
                else
                {
                    Brede.CntVal_Manual = 0;
                }
            } 
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (Auto_Flag.AutoRunBusy)
            {
                BAR._ToolTipDlg.WriteToolTipStr("正在自动运行");
                BAR.ShowToolTipWnd(false);
                return;
            }
            else 
            {
                if ((AutoTray.StatusAlarmWord & AutoTray.Status_SystemInitDone) == 0)
                {
                    if ((AutoTray.StatusAlarmWord & AutoTray.Status_SystemRnunning) == 0)
                    {
                        BAR._ToolTipDlg.WriteToolTipStr("自动盘电机未在原位");
                        BAR.ShowToolTipWnd(false);
                        return;
                    }
                }
                if ((AutoTray.StatusAlarmWord & AutoTray.Status_SystemBusy) != 0)
                {
                    BAR._ToolTipDlg.WriteToolTipStr("自动盘正在忙");
                    BAR.ShowToolTipWnd(false);
                    return;
                }
                if ((AutoTray.StatusAlarmWord & AutoTray.Status_OverplusTrayInit) == 0)
                {
                    BAR._ToolTipDlg.WriteToolTipStr("自动盘前端缺少料盘");
                    BAR.ShowToolTipWnd(false);
                    return;
                }
            }
            AutoTray_Trigger.ReceiveTray = true;
            AutoTray_Process.ManualReceiveTray = true;
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            if (Auto_Flag.AutoRunBusy)
            {
                BAR._ToolTipDlg.WriteToolTipStr("正在自动运行");
                BAR.ShowToolTipWnd(false);
                return;
            }
            else
            {
                if ((AutoTray.StatusAlarmWord & AutoTray.Status_SystemInitDone) == 0)
                {
                    if ((AutoTray.StatusAlarmWord & AutoTray.Status_SystemRnunning) == 0)
                    {
                        BAR._ToolTipDlg.WriteToolTipStr("自动盘电机未在原位");
                        BAR.ShowToolTipWnd(false);
                        return;
                    }
                }
                if ((AutoTray.StatusAlarmWord & AutoTray.Status_SystemBusy) != 0)
                {
                    BAR._ToolTipDlg.WriteToolTipStr("自动盘正在忙");
                    BAR.ShowToolTipWnd(false);
                    return;
                }
                if ((AutoTray.StatusAlarmWord & AutoTray.Status_OverplusTrayInit) != 0)
                {
                    BAR._ToolTipDlg.WriteToolTipStr("自动盘前端存在料盘");
                    BAR.ShowToolTipWnd(false);
                    return;
                }
            }
            AutoTray_Trigger.TakeTray = true;
            AutoTray_Process.ManualTakeTray = true;
        }

        private void skinButton8_Click(object sender, EventArgs e)
        {
            if (!Auto_Flag.AutoRunBusy)
            {
                In_Output.BredeStart_CCD.M = false;
                In_Output.BredeLight_CCD.M = !In_Output.BredeLight_CCD.M;
                if (In_Output.BredeLight_CCD.M)
                {
                    //g_act.WaitDoEvent(200);
                    Brede.StartCCD();
                }
            }
        }
    }
}
