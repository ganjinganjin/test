using BAR.Commonlib;
using PLC;
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
    public partial class AxisCtlWnd : Form
    {
        BAR man = null;
        PLC1 plc = new PLC1();
        Axis axis = new Axis();

        public AxisCtlWnd()
        {
            InitializeComponent();
        }

        private void AxisCtlWnd_Load(object sender, EventArgs e)
        {
            man = (BAR)this.Owner;//实例化父窗体
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
                Axis_Z1_JOG负_Up.Location = new Point(430, Axis_Z1_JOG负_Up.Location.Y);
                Axis_Z1_JOG正_Up.Location = new Point(430, Axis_Z1_JOG正_Up.Location.Y);
                labelX.Location = new Point(labelX.Location.X, labelC1.Location.Y);
                labelY.Location = new Point(labelX.Location.X, labelC2.Location.Y);
                labelZ1.Location = new Point(labelX.Location.X, labelC3.Location.Y);
                xLab.Location = new Point(xLab.Location.X, c1Lab.Location.Y);
                yLab.Location = new Point(xLab.Location.X, c2Lab.Location.Y);
                z1Lab.Location = new Point(xLab.Location.X, c3Lab.Location.Y);
                labelZ1.Text = "Z轴当前值:";
            }
        }

        private void AxisCtlWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void AxisCtlWnd_Activated(object sender, EventArgs e)
        {
            speedLowRBtn.Checked = true;
            proccedRBtn.Checked = true;
            angle2RBtn.Checked = true;

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
                if(!axisSts.isHomeBusy)
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
                else if (offset3RBtn.Checked)
                {
                    offset = 1d * dir;
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
            if (Auto_Flag.SystemBusy)
            {
                return;
            }
            if (proccedRBtn.Checked)
            {
                Gts.GT_Stop(jogPrm.cardPrm.cardNum, 1 << (jogPrm.index - 1), 0);
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
            if(Axis_XY_Moveable_Check())
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


        private void BtnHome_Click(object sender, EventArgs e)
        {
            if (!Auto_Flag.SystemBusy)
            {
                Axis.Home_Start();
            }
        }

        public void Manual_Rotate(int penNum, bool flag = true)
        {
            if (Auto_Flag.SystemBusy)
            {
                return;
            }
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
    }
}
