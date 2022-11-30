using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BAR.CommonLib_v1._0;
using LBSoft.IndustrialCtrls.Leds;
using PLC;

namespace BAR.Windows
{
    public partial class AxisIOWnd : Form
    {
        BAR man = null;  //声明父窗体
        public AxisIOWnd()
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();
            SetLanguage();
        }

        private void AxisIO_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
            e.Cancel = true;
            this.Hide();
        }

        private void AxisIO_Load(object sender, EventArgs e)
        {
            man = (BAR)this.Owner;//实例化父窗体
            timer1.Enabled = true;
        }

        public void InitUI()
        {
            timer1.Enabled = true;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            String ctlName;
            Control[] tagAry;

            for (int i = 0; i < 8; i++)
            {
                int n = i + 1;
                ctlName = "Card1__Axis" + n + "_负限";
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    IO_Monitor((tagAry.First() as LBLed), In_Output.negLimit[0][i].M);
                }

                ctlName = "Card1__Axis" + n + "_正限";
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    IO_Monitor((tagAry.First() as LBLed), In_Output.posLimit[0][i].M);
                }

                ctlName = "Card1__Axis" + n + "_零点";
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    IO_Monitor((tagAry.First() as LBLed), In_Output.home[0][i].M);
                }


                ctlName = "Card1__Axis" + n + "_报警";
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    IO_Monitor((tagAry.First() as LBLed), In_Output.alarm[0][i].M);
                }
            }
        }

        private void SetLanguage()
        {
            if (!MultiLanguage.IsEnglish())
                return;

            String ctlName;
            Control[] tagAry;

            for (int i = 0; i < 8; i++)
            {
                int n = i + 1;
                ctlName = "Card1__Axis" + n + "_负限";
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    (tagAry.First() as LBLed).Label = "Axis" + n + "_Negative";
                    (tagAry.First() as LBLed).Font = new Font("Calibri", tagAry.First().Font.Size, tagAry.First().Font.Style);
                }

                ctlName = "Card1__Axis" + n + "_正限";
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    (tagAry.First() as LBLed).Label = "Axis" + n + "_Positive";
                    (tagAry.First() as LBLed).Font = new Font("Calibri", tagAry.First().Font.Size, tagAry.First().Font.Style);
                }

                ctlName = "Card1__Axis" + n + "_零点";
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    (tagAry.First() as LBLed).Label = "Axis" + n + "_Home";
                    (tagAry.First() as LBLed).Font = new Font("Calibri", tagAry.First().Font.Size, tagAry.First().Font.Style);
                }


                ctlName = "Card1__Axis" + n + "_报警";
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    (tagAry.First() as LBLed).Label = "Axis" + n + "_Alarm";
                    (tagAry.First() as LBLed).Font = new Font("Calibri", tagAry.First().Font.Size, tagAry.First().Font.Style);
                }
            }
        }

        /// <summary>
        /// IO监控
        /// </summary>
        /// <param name="lBLed">控件</param>
        /// <param name="IsBright">监控值</param>
        private void IO_Monitor(LBLed lBLed, bool IsBright)
        {
            if (IsBright)
            {
                lBLed.State = LBLed.LedState.On;
            }
            else
            {
                lBLed.State = LBLed.LedState.Off;
            }
        }
    }
}
