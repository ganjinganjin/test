using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CCWin.SkinControl;
using BAR.Commonlib;
using BAR.CommonLib_v1._0;

namespace BAR.Windows
{
    public partial class StaticalWnd : Form
    {
        NumericUpDown[]     ArrNumUpDown;
        Config g_config = Config.GetInstance();
        Act g_act = Act.GetInstance();
        public StaticalWnd()
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();
            UserEvent.staticalWndUI += new UserEvent.StaticalWndUI(__InitWndUI);
            ArrNumUpDown = new NumericUpDown[23];
            ArrNumUpDown[0] = this.numericUpDown1;
            ArrNumUpDown[1] = this.numericUpDown2;
            ArrNumUpDown[2] = this.numericUpDown3;
            ArrNumUpDown[3] = this.numericUpDown4;
            ArrNumUpDown[4] = this.numericUpDown5;
            ArrNumUpDown[5] = this.numericUpDown6;
            ArrNumUpDown[6] = this.numericUpDown7;
            ArrNumUpDown[7] = this.numericUpDown8;
            ArrNumUpDown[8] = this.numericUpDown9;
            ArrNumUpDown[9] = this.numericUpDown10;
            ArrNumUpDown[10] = this.numericUpDown11;
            ArrNumUpDown[11] = this.numericUpDown12;
            ArrNumUpDown[12] = this.numericUpDown13;
            ArrNumUpDown[13] = this.numericUpDown14;
            ArrNumUpDown[14] = this.numericUpDown15;
            ArrNumUpDown[15] = this.numericUpDown16;
            ArrNumUpDown[16] = this.numericUpDown17;
            ArrNumUpDown[17] = this.numericUpDown18;
            ArrNumUpDown[18] = this.numericUpDown19;
            ArrNumUpDown[19] = this.numericUpDown20;
            ArrNumUpDown[20] = this.numericUpDown21;
            ArrNumUpDown[21] = this.numericUpDown22;
            ArrNumUpDown[22] = this.numericUpDown23;
        }
        

        public void __InitWndUI()
        {
            float temp;

            ArrNumUpDown[0].Value = new decimal(TrayD.TIC_TrayN);
            ArrNumUpDown[1].Value = new decimal(TrayD.LIC_TrayN);
            ArrNumUpDown[2].Value = new decimal(TrayD.TIC_RowN[0]);
            ArrNumUpDown[3].Value = new decimal(TrayD.TIC_ColN[0]);
            ArrNumUpDown[4].Value = new decimal(TrayD.LIC_RowN[0]);
            ArrNumUpDown[5].Value = new decimal(TrayD.LIC_ColN[0]);
            ArrNumUpDown[6].Value = new decimal(TrayD.TIC_EndRowN[0]);
            ArrNumUpDown[7].Value = new decimal(TrayD.TIC_EndColN[0]);

            ArrNumUpDown[8].Value = new decimal(TrayD.TIC_RowN[1]);
            ArrNumUpDown[9].Value = new decimal(TrayD.TIC_ColN[1]);
            ArrNumUpDown[10].Value = new decimal(TrayD.LIC_RowN[1]);
            ArrNumUpDown[11].Value = new decimal(TrayD.LIC_ColN[1]);
            ArrNumUpDown[12].Value = new decimal(TrayD.TIC_EndRowN[1]);
            ArrNumUpDown[13].Value = new decimal(TrayD.TIC_EndColN[1]);

            ArrNumUpDown[14].Value = new decimal(TrayD.LIC_RowN[2]);
            ArrNumUpDown[15].Value = new decimal(TrayD.LIC_ColN[2]);

            ArrNumUpDown[16].Value = new decimal(UserTask.TargetC);
            ArrNumUpDown[17].Value = new decimal(UserTask.OKDoneC);
            ArrNumUpDown[18].Value = new decimal(UserTask.TIC_DoneC);

            
            if (UserTask.OKAllC + UserTask.NGAllC > 0)
            {
                temp = (float)UserTask.OKAllC / (UserTask.OKAllC + UserTask.NGAllC);
            }
            else
            {
                temp = 0;
            }      
            ArrNumUpDown[19].Value = new decimal(temp * 100);
            ArrNumUpDown[20].Value = new decimal(Efficiency.value);
            ArrNumUpDown[21].Value = new decimal(UserTask.OKAllC);
            ArrNumUpDown[22].Value = new decimal(UserTask.NGAllC);
            
            if (Auto_Flag.Pause)
            {
                if (timer1.Enabled)
                {
                    timer1.Stop();
                }
                for (int i = 0; i < 17; i++)
                {
                    if (!ArrNumUpDown[i].Enabled)
                    {
                        ArrNumUpDown[i].Enabled = true;
                    }
                }
                if (!cbWrite.Checked)
                {
                    cbWrite.Checked = true;
                }
            }
            else
            {
                if (!timer1.Enabled)
                {
                    timer1.Start();
                }
                for (int i = 0; i < 17; i++)
                {
                    if (ArrNumUpDown[i].Enabled)
                    {
                        ArrNumUpDown[i].Enabled = false;
                    }
                }
                if (cbWrite.Checked)
                {
                    cbWrite.Checked = false;
                }
            }

        }

        private void saveAllBtn_Click(object sender, EventArgs e)
        {
            TrayD.TIC_TrayN = Convert.ToInt32(this.ArrNumUpDown[0].Value);
            TrayD.LIC_TrayN = Convert.ToInt32(this.ArrNumUpDown[1].Value);

            TrayD.TIC_RowN[0] = Convert.ToInt32(this.ArrNumUpDown[2].Value);
            TrayD.TIC_ColN[0] = Convert.ToInt32(this.ArrNumUpDown[3].Value);
            TrayD.LIC_RowN[0] = Convert.ToInt32(this.ArrNumUpDown[4].Value);
            TrayD.LIC_ColN[0] = Convert.ToInt32(this.ArrNumUpDown[5].Value);
            TrayD.TIC_EndRowN[0] = Convert.ToInt32(this.ArrNumUpDown[6].Value);
            TrayD.TIC_EndColN[0] = Convert.ToInt32(this.ArrNumUpDown[7].Value);

            TrayD.TIC_RowN[1] = Convert.ToInt32(this.ArrNumUpDown[8].Value);
            TrayD.TIC_ColN[1] = Convert.ToInt32(this.ArrNumUpDown[9].Value);
            TrayD.LIC_RowN[1] = Convert.ToInt32(this.ArrNumUpDown[10].Value);
            TrayD.LIC_ColN[1] = Convert.ToInt32(this.ArrNumUpDown[11].Value);
            TrayD.TIC_EndRowN[1] = Convert.ToInt32(this.ArrNumUpDown[12].Value);
            TrayD.TIC_EndColN[1] = Convert.ToInt32(this.ArrNumUpDown[13].Value);

            TrayD.LIC_RowN[2] = Convert.ToInt32(this.ArrNumUpDown[14].Value);
            TrayD.LIC_ColN[2] = Convert.ToInt32(this.ArrNumUpDown[15].Value);

            UserTask.TargetC = Convert.ToInt32(this.ArrNumUpDown[16].Value);
            g_config.WriteStaticVal();
            TrayState.TrayStateUpdate();
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "统计计数修改成功", "Modify");
        }


        private void clearBtn_MouseUp(object sender, MouseEventArgs e)
        {
            SkinButton btn = sender as SkinButton;
            if (btn == clearBtn1)
            {
                if (DialogResult.Yes == MessageBox.Show(MultiLanguage.GetString("是否将OK数清零?"), "Information：", MessageBoxButtons.YesNo, MessageBoxIcon.Information)) 
                {
                    UserTask.OKAllC = 0;
                }
            }
            else if (btn == clearBtn2)
            {
                if (DialogResult.Yes == MessageBox.Show(MultiLanguage.GetString("是否将NG数清零?"), "Information：", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                {
                    UserTask.NGAllC = 0;
                }
            }
            __InitWndUI();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.__InitWndUI();
        }

        private void StaticalWnd_Enter(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void StaticalWnd_MouseDown(object sender, MouseEventArgs e)
        {
            timer1.Start();
        }

        private void StaticalWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
            cbWrite.Checked = false;
            //cbWrite_Click(null,null);
            e.Cancel = true;
            this.Hide();

        }

        private void cbWrite_Click(object sender, EventArgs e)
        {
            if (cbWrite.Checked)
            {
                timer1.Stop();
                for (int i = 0; i < 17; i++)
                {
                    ArrNumUpDown[i].Enabled  = true;
                }
                Auto_Flag.Pause = true;
            }
            else
            {
                if (!timer1.Enabled)
                {
                    timer1.Start();
                }

                for (int i = 0; i < 17; i++)
                {
                    ArrNumUpDown[i].Enabled  = false;
                }
                Auto_Flag.Pause = false;
            }
        }

    }
}
