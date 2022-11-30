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
    public partial class BurnSeatStaticWnd : Form
    {
        Config g_config = Config.GetInstance();
        private SkinGroupBox[] scketGroupBox = new SkinGroupBox[UserConfig.ScketGroupC];
        private SkinButton[] rBtnClearGroup = new SkinButton[UserConfig.ScketGroupC];
        private Label[] txtOK;
        private Label[] txtNG;
        private Label[] txtAll;
        private Label[] txtYield;
        private SkinButton[] rBtnClearUnit = new SkinButton[UserConfig.AllScketC];
        private TextBox[] txtScketOK = new TextBox[UserConfig.AllScketC];
        private TextBox[] txtScketNG = new TextBox[UserConfig.AllScketC];
        private TextBox[] txtScketAll = new TextBox[UserConfig.AllScketC];
        private TextBox[] txtScketYield = new TextBox[UserConfig.AllScketC];
        public BurnSeatStaticWnd()
        {
            MultiLanguage.SetDefaultLanguage();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
            InitializeComponent();
            _InitializeComponent();
        }

        private void _InitializeComponent()
        {
            //烧录座子分组控件
            for (int i = 0; i < UserConfig.ScketGroupC; i++)
            {
                scketGroupBox[i] = new SkinGroupBox();
                rBtnClearGroup[i] = new SkinButton();
            }
            //烧录座子计数器相关控件
            for (int i = 0; i < UserConfig.AllScketC; i++)
            {
                rBtnClearUnit[i] = new SkinButton();
                txtScketOK[i] = new TextBox();
                txtScketNG[i] = new TextBox();
                txtScketAll[i] = new TextBox();
                txtScketYield[i] = new TextBox();
            }

            string font = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Arial" : "楷体";
            int left = 0, top = 0, nW = 68, nH = 20, offsetW = 40, leftStart = 2, topStart = 4;

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
                colMaxC = 2;
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_YED)
            {
                colMaxC = 8;
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
            groupH = 35 + 5 * (nH + 1) * layoutMode;


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

            for (int i = 0; i < UserConfig.ScketGroupC; i++)
            {
                scketGroupBox[i].SuspendLayout();
            }
            SuspendLayout();
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

                scketGroupBox[i].BackColor = Color.Transparent;
                scketGroupBox[i].BorderColor = Color.Black;
                scketGroupBox[i].Font = new Font(font, 18F, FontStyle.Bold);
                scketGroupBox[i].ForeColor = Color.Black;
                scketGroupBox[i].Location = groupPoint[nIndex];
                scketGroupBox[i].Name = "scketGroupBox" + i;
                scketGroupBox[i].RectBackColor = SystemColors.ActiveCaption;
                scketGroupBox[i].RoundStyle = CCWin.SkinClass.RoundStyle.All;
                scketGroupBox[i].Size = new Size(groupW, groupH);
                scketGroupBox[i].TabIndex = i;
                scketGroupBox[i].TabStop = false;
                scketGroupBox[i].TitleBorderColor = SystemColors.ActiveCaption;
                scketGroupBox[i].TitleRectBackColor = SystemColors.ActiveCaption;
                scketGroupBox[i].TitleRoundStyle = CCWin.SkinClass.RoundStyle.All;

                rBtnClearGroup[i].BaseColor = Color.Transparent;
                rBtnClearGroup[i].BackColor = SystemColors.ActiveCaption;
                rBtnClearGroup[i].BorderColor = Color.Empty;
                rBtnClearGroup[i].BorderInflate = new Size(0, 0);
                rBtnClearGroup[i].FlatStyle = FlatStyle.Popup;
                rBtnClearGroup[i].Font = new Font(font, 15F, FontStyle.Bold);
                rBtnClearGroup[i].ForeColor = Color.Black;
                rBtnClearGroup[i].Location = new Point(groupPoint[nIndex].X + 5, groupPoint[nIndex].Y);
                rBtnClearGroup[i].Name = "rBtnClearGroup" + i;
                rBtnClearGroup[i].RoundStyle = CCWin.SkinClass.RoundStyle.All;
                rBtnClearGroup[i].AutoSize = true;
                rBtnClearGroup[i].TabIndex = i;
                rBtnClearGroup[i].TabStop = false;
                rBtnClearGroup[i].Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "G" + (i + 1) + "_Clear" : "组" + (i + 1) + "清除";
                rBtnClearGroup[i].Tag = i;
                rBtnClearGroup[i].MouseUp += new MouseEventHandler(clearGroupBtn_MouseUp); 
            }

            //烧录座子计数器相关控件
            colC = UserConfig.ScketUnitC / layoutMode;
            if (colC == 0)
            {
                colC = 1;
            }
            nW = (groupW - offsetW - 6) / colC;
            leftStart = (groupW - offsetW - nW * colC + 1) / 2;

            txtOK = new Label[UserConfig.ScketGroupC * layoutMode];
            txtNG = new Label[UserConfig.ScketGroupC * layoutMode];
            txtAll = new Label[UserConfig.ScketGroupC * layoutMode];
            txtYield = new Label[UserConfig.ScketGroupC * layoutMode];
            for (int i = 0; i < UserConfig.ScketGroupC * layoutMode; i++)
            {
                txtOK[i] = new Label();
                txtNG[i] = new Label();
                txtAll[i] = new Label();
                txtYield[i] = new Label();

                top = nH + topStart;
                if (layoutMode != 1)
                {
                   top += (i % layoutMode) * (5 * (nH + 1) + topStart);
                }

                txtOK[i].Location = new Point(leftStart, groupH - top - 4 * nH);
                txtOK[i].Margin = new Padding(0);
                txtOK[i].MinimumSize = new Size(20, 20);
                txtOK[i].Name = "txtScketOK" + i;
                txtOK[i].Size = new Size(offsetW, nH);
                txtOK[i].Font = new Font(font, 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                txtOK[i].TabIndex = i;
                txtOK[i].Text = "OK:";
                txtOK[i].TextAlign = ContentAlignment.MiddleLeft;
                txtOK[i].BorderStyle = BorderStyle.FixedSingle;
                txtOK[i].ForeColor = Color.Green;
                txtOK[i].BackColor = Color.WhiteSmoke;

                txtNG[i].Location = new Point(leftStart, groupH - top - 3 * nH);
                txtNG[i].Margin = new Padding(0);
                txtNG[i].MinimumSize = new Size(20, 20);
                txtNG[i].Name = "txtNG" + i;
                txtNG[i].Size = new Size(offsetW, nH);
                txtNG[i].Font = new Font(font, 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                txtNG[i].TabIndex = i;
                txtNG[i].Text = "NG:";
                txtNG[i].TextAlign = ContentAlignment.MiddleLeft;
                txtNG[i].BorderStyle = BorderStyle.FixedSingle;
                txtNG[i].ForeColor = Color.Red;
                txtNG[i].BackColor = Color.WhiteSmoke;

                txtAll[i].Location = new Point(leftStart, groupH - top - 2 * nH);
                txtAll[i].Margin = new Padding(0);
                txtAll[i].MinimumSize = new Size(20, 20);
                txtAll[i].Name = "txtAll" + i;
                txtAll[i].Size = new Size(offsetW, nH);
                txtAll[i].Font = new Font(font, 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                txtAll[i].TabIndex = i;
                txtAll[i].Text = "All:";
                txtAll[i].TextAlign = ContentAlignment.MiddleLeft;
                txtAll[i].BorderStyle = BorderStyle.FixedSingle;
                txtAll[i].ForeColor = Color.Blue;
                txtAll[i].BackColor = Color.WhiteSmoke;

                txtYield[i].Location = new Point(leftStart, groupH - top - 1 * nH);
                txtYield[i].Margin = new Padding(1);
                txtYield[i].MinimumSize = new Size(20, 20);
                txtYield[i].Name = "txtYield" + i;
                txtYield[i].Size = new Size(offsetW, nH);
                txtYield[i].Font = new Font(font, 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                txtYield[i].TabIndex = i;
                txtYield[i].Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Yield" : "良率:";
                txtYield[i].TextAlign = ContentAlignment.MiddleLeft;
                txtYield[i].BorderStyle = BorderStyle.FixedSingle;
                txtYield[i].ForeColor = Color.Blue;
                txtYield[i].BackColor = Color.WhiteSmoke;

                int group = i / layoutMode;
                scketGroupBox[group].Controls.Add(txtOK[i]);
                scketGroupBox[group].Controls.Add(txtNG[i]);
                scketGroupBox[group].Controls.Add(txtAll[i]);
                scketGroupBox[group].Controls.Add(txtYield[i]);
            }
            for (int i = 0; i < UserConfig.AllScketC; i++)
            {
                left = (i % colC) * nW + offsetW + leftStart;
                top = nH + topStart;
                if (layoutMode != 1)
                {
                    if (UserTask.ProgrammerType == GlobConstData.Programmer_DP && UserConfig.ScketUnitC == 16)
                    {
                        top += (layoutMode - 1 - ((i % UserConfig.ScketUnitC) / colC)) * (5 * (nH + 1) + topStart);
                    }
                    else
                    {
                        top += ((i % UserConfig.ScketUnitC) / colC) * (5 * (nH + 1) + topStart);
                    }
                }

                rBtnClearUnit[i].AutoSize = false;
                rBtnClearUnit[i].BackColor = Color.Transparent;
                rBtnClearUnit[i].BaseColor = Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(235)))));
                rBtnClearUnit[i].BorderColor = Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                rBtnClearUnit[i].ControlState = CCWin.SkinClass.ControlState.Normal;
                rBtnClearUnit[i].DownBack = null;
                rBtnClearUnit[i].DownBaseColor = Color.Gray;
                rBtnClearUnit[i].Font = new Font(font, 9F, FontStyle.Bold);
                rBtnClearUnit[i].ForeColor = Color.Black;
                rBtnClearUnit[i].ImeMode = ImeMode.NoControl;
                rBtnClearUnit[i].IsDrawGlass = false;
                rBtnClearUnit[i].Location = new Point(left, groupH - top);
                rBtnClearUnit[i].MouseBack = null;
                rBtnClearUnit[i].MouseBaseColor = Color.Gray;
                rBtnClearUnit[i].Name = "rBtnClearUnit" + i;
                rBtnClearUnit[i].NormlBack = null;
                rBtnClearUnit[i].Size = new Size(nW, nH);
                rBtnClearUnit[i].TabIndex = i;
                rBtnClearUnit[i].Text = MultiLanguage.IsEnglish() == true ? "Clear " + (i / UserConfig.ScketUnitC + 1) + "-" + (i % UserConfig.ScketUnitC + 1) 
                    : "清零" + (i / UserConfig.ScketUnitC + 1) + "_" + (i % UserConfig.ScketUnitC + 1) + "座";
                rBtnClearUnit[i].UseVisualStyleBackColor = false;
                rBtnClearUnit[i].TabStop = true;
                rBtnClearUnit[i].Tag = i;
                rBtnClearUnit[i].MouseUp += new MouseEventHandler(clearUnitBtn_MouseUp);

                txtScketOK[i].Lines = new string[] { "0" };
                txtScketOK[i].Location = new Point(left, groupH - top - 4 * nH);
                txtScketOK[i].Margin = new Padding(0);
                txtScketOK[i].MaxLength = 32767;
                txtScketOK[i].MinimumSize = new Size(20, 20);
                txtScketOK[i].Multiline = false;
                txtScketOK[i].Name = "txtScketOK" + i;
                txtScketOK[i].Padding = new Padding(5);
                txtScketOK[i].ReadOnly = false;
                txtScketOK[i].ScrollBars = ScrollBars.None;
                txtScketOK[i].Size = new Size(nW, nH);
                txtScketOK[i].Font = new Font(font, 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                txtScketOK[i].TabIndex = i;
                txtScketOK[i].Text = "0";
                txtScketOK[i].ForeColor = Color.Green;
                txtScketOK[i].TextAlign = HorizontalAlignment.Center;

                txtScketNG[i].Lines = new string[] { "0" };
                txtScketNG[i].Location = new Point(left, groupH - top - 3 * nH);
                txtScketNG[i].Margin = new Padding(0);
                txtScketNG[i].MaxLength = 32767;
                txtScketNG[i].MinimumSize = new Size(20, 20);
                txtScketNG[i].Multiline = false;
                txtScketNG[i].Name = "txtScketNG" + i;
                txtScketNG[i].Padding = new Padding(5);
                txtScketNG[i].ReadOnly = false;
                txtScketNG[i].ScrollBars = ScrollBars.None;
                txtScketNG[i].Size = new Size(nW, nH);
                txtScketNG[i].Font = new Font(font, 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                txtScketNG[i].TabIndex = i;
                txtScketNG[i].Text = "0";
                txtScketNG[i].ForeColor = Color.Red;
                txtScketNG[i].TextAlign = HorizontalAlignment.Center;

                txtScketAll[i].Lines = new string[] { "0" };
                txtScketAll[i].Location = new Point(left, groupH - top - 2 * nH);
                txtScketAll[i].Margin = new Padding(0);
                txtScketAll[i].MaxLength = 32767;
                txtScketAll[i].MinimumSize = new Size(20, 20);
                txtScketAll[i].Multiline = false;
                txtScketAll[i].Name = "txtScketAll" + i;
                txtScketAll[i].Padding = new Padding(5);
                txtScketAll[i].ReadOnly = false;
                txtScketAll[i].ScrollBars = ScrollBars.None;
                txtScketAll[i].Size = new Size(nW, nH);
                txtScketAll[i].Font = new Font(font, 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                txtScketAll[i].TabIndex = i;
                txtScketAll[i].Text = "0";
                txtScketAll[i].ForeColor = Color.Blue;
                txtScketAll[i].TextAlign = HorizontalAlignment.Center;
               


                txtScketYield[i].Lines = new string[] { "0" };
                txtScketYield[i].Location = new Point(left, groupH - top - 1 * nH);
                txtScketYield[i].Margin = new Padding(0);
                txtScketYield[i].MaxLength = 32767;
                txtScketYield[i].MinimumSize = new Size(20, 20);
                txtScketYield[i].Multiline = false;
                txtScketYield[i].Name = "txtScketYield" + i;
                txtScketYield[i].Padding = new Padding(5);
                txtScketYield[i].ReadOnly = false;
                txtScketYield[i].ScrollBars = ScrollBars.None;
                txtScketYield[i].Size = new Size(nW, nH);
                txtScketYield[i].Font = new Font(font, 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                txtScketYield[i].TabIndex = i;
                txtScketYield[i].Text = "0";
                txtScketYield[i].ForeColor = Color.Blue;
                txtScketYield[i].TextAlign = HorizontalAlignment.Center;

                int group = i / UserConfig.ScketUnitC;
                scketGroupBox[group].Controls.Add(rBtnClearUnit[i]);
                scketGroupBox[group].Controls.Add(txtScketOK[i]);
                scketGroupBox[group].Controls.Add(txtScketNG[i]);
                scketGroupBox[group].Controls.Add(txtScketAll[i]);
                scketGroupBox[group].Controls.Add(txtScketYield[i]);
            }
            for (int i = 0; i < UserConfig.ScketGroupC; i++)
            {
                if (UserConfig.ScketUnitC > 1)
                {
                    scketPosPnl.Controls.Add(rBtnClearGroup[i]);
                }
                scketPosPnl.Controls.Add(scketGroupBox[i]);
            }
            for (int i = 0; i < UserConfig.ScketGroupC; i++)
            {
                scketGroupBox[i].ResumeLayout(false);
            }
            ResumeLayout(false);
            PerformLayout();

            SocketCounterDisplay();
        }

        /// <summary>
        /// 烧录座计数器显示
        /// </summary>
        private void SocketCounterDisplay()
        {
            int total;
            float yield;
            int k = UserConfig.ScketUnitC;
            for (int i = 0; i < UserConfig.AllScketC; i++)
            {
                total = Axis.Group[i / k].Unit[i % k].OKAllC + Axis.Group[i / k].Unit[i % k].NGAllC;
                if (total > 0)
                {
                    yield = (float)Axis.Group[i / k].Unit[i % k].OKAllC / total;
                }
                else
                {
                    yield = 0;
                }
                txtScketOK[i].Text = Convert.ToString(Axis.Group[i / k].Unit[i % k].OKAllC);
                txtScketNG[i].Text = Convert.ToString(Axis.Group[i / k].Unit[i % k].NGAllC);
                txtScketAll[i].Text = Convert.ToString(total);
                txtScketYield[i].Text = String.Format("{0:f2}", yield * 100) + "%";
            }
        }

        private void clearUnitBtn_MouseUp(object sender, MouseEventArgs e)
        {
            int ind = Convert.ToInt32((sender as SkinButton).Tag);
            int group = ind / UserConfig.ScketUnitC;
            int unit = ind % UserConfig.ScketUnitC;
            String str = "是否将" + (group + 1) + "_" + (unit + 1) + "座计数清零？\r\n\r\n【是】“清零”\r\n【否】“取消”";
            if (MultiLanguage.IsEnglish())
            {
                str = "Do you want to clear the " + (group + 1) + "_" + (unit + 1) + "seat count to zero?\r\n\r\n【Yes】“Clear”\r\n【NO】“Cancel”";
            }
            if (DialogResult.Yes == MessageBox.Show(str, "Information：", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
            {
                g_config.WriteSocketCounter(ind, 10);
            }
        }

        private void clearGroupBtn_MouseUp(object sender, MouseEventArgs e)
        {
            int ind = Convert.ToInt32((sender as SkinButton).Tag);
            String str = "是否将组" + (ind + 1) + "计数清零？\r\n\r\n【是】“清零”\r\n【否】“取消”";
            if (MultiLanguage.IsEnglish())
            {
                str = "Do you want to clear the group " + (ind + 1) + " count to zero?\r\n\r\n【Yes】“Clear”\r\n【NO】“Cancel”";
            }
            if (DialogResult.Yes == MessageBox.Show(str, "Information：", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
            {
                int temp = ind * UserConfig.ScketUnitC;
                for (int i = 0; i < UserConfig.ScketUnitC; i++)
                {
                    g_config.WriteSocketCounter(temp + i, 10);
                }
            }
        }

        private void clearBtn1_Click(object sender, EventArgs e)
        {
            String str = "是否将烧录座计数全部清零？\r\n\r\n【是】“清零”\r\n【否】“取消”";
            if (MultiLanguage.IsEnglish())
            {
                str = "Do you want to clear the burning seat count to zero?\r\n\r\n【Yes】“Clear”\r\n【NO】“Cancel”";
            }
            if (DialogResult.Yes == MessageBox.Show(str, "Information：", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
            {
                for (int i = 0; i < UserConfig.AllScketC; i++)
                {
                    g_config.WriteSocketCounter(i, 10);
                }
            }
        }

        public void InitWndUI()
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SocketCounterDisplay();
        }

        private void BurnSeatStaticWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
            e.Cancel = true;
            Hide();
        }
    }
}
