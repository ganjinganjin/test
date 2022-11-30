using BAR.Commonlib;
using BAR.Commonlib.Events;
using BAR.CommonLib_v1._0;
using BAR.Communicators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BAR.Windows
{
    public partial class AutoTrayParameters : Form
    {
        Config g_config = Config.GetInstance();
        AutoTrayProxy proxy = AutoTrayProxy.GetInstance();
        bool readBusy = false;
        NumericUpDown[] ArrNumTxt;

        public AutoTrayParameters()
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();
            if (MultiLanguage.DefaultLanguage == GlobConstData.ST_English)
            {
                labNum1.Text = Config.AutoTrayDir == 0 ? "Columns:" : "Rows:";
                labNum2.Text = Config.AutoTrayDir == 1 ? "Columns:" : "Rows:";
                labSpace1.Text = Config.AutoTrayDir == 0 ? "Col pitch:" : "Rowledge:";
                labSpace2.Text = Config.AutoTrayDir == 1 ? "Col pitch:" : "Rowledge:";
            }
            else
            {
                labNum1.Text = Config.AutoTrayDir == 0 ? "列竖数:" : "行横数:";
                labNum2.Text = Config.AutoTrayDir == 1 ? "列竖数:" : "行横数:";
                labSpace1.Text = Config.AutoTrayDir == 0 ? "列间距:" : "行间距:";
                labSpace2.Text = Config.AutoTrayDir == 1 ? "列间距:" : "行间距:";
            }
            ArrNumTxt = new NumericUpDown[16];

            for (int i = 0; i < 8; i++)
            {
                string ctlName;
                Control[] tagAry;
                ctlName = "InputSetConfig" + i;
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    ArrNumTxt[i] = tagAry.First() as NumericUpDown;
                }
                ctlName = "SetConfig" + i;
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    ArrNumTxt[8 + i] = tagAry.First() as NumericUpDown;
                }
            }
            proxy.ProxyNotifyEvent += this.OnMsgEvenHandle;
            UserEvent.productChange += new UserEvent.ProductChange(UpdateUI);
        }

        /// <summary>
        /// 更新UI
        /// </summary>
        private void UpdateUI()
        {
            this.__InitWndUI();
            if (Auto_Flag.AutoTray_TakeIC || Auto_Flag.AutoTray_LayIC)
            {
                if (Config.AutoTrayDot == 0)
                {
                    AutoTray.MODBUS_WriteDotParameter();
                }
                StartReadDotParameter();
            }
            else
            {
                returnTxtBox1.Text = null;
                returnTxtBox2.Text = null;
            }
        }

        private void StartReadDotParameter()
        {
            if (readBusy)
            {
                return;
            }
            readBusy = true;
            AutoTray.MODBUS_ReadDotParameter();
            Task task = new Task(ReadTimeout);
            task.Start();
        }
        private void ReadTimeout()
        {
            DateTime dt1 = DateTime.Now;
            do
            {
                if (!readBusy)
                {
                    return;
                }
                Thread.Sleep(5);
            }
            while ((DateTime.Now - dt1).TotalMilliseconds < 3000);

            MessageBox.Show("自动盘参数读取失败");
            StringBuilder msg = new StringBuilder();
            msg.Append("自动盘回传数据失败" + Environment.NewLine);
            returnTxtBox1.Text = msg.ToString();
            returnTxtBox2.Text = null;
            readBusy = false;
        }

        private void OnMsgEvenHandle(MsgEvent e)
        {
            StringBuilder msg = new StringBuilder();
            msg.Append("Receive" + Environment.NewLine);
            msg.AppendLine();
            msg.Append("X1:" + AutoTray.getConfig[0] + Environment.NewLine);
            msg.Append("Y1:" + AutoTray.getConfig[1] + Environment.NewLine);
            msg.Append("X2:" + AutoTray.getConfig[2] + Environment.NewLine);
            msg.Append("Y2:" + AutoTray.getConfig[3] + Environment.NewLine);
            this.returnTxtBox1.Text = msg.ToString();

            msg.Clear();
            msg.AppendLine();
            msg.AppendLine();
            msg.Append(labNum1.Text + AutoTray.getConfig[4] + Environment.NewLine);
            msg.Append(labSpace1.Text + AutoTray.getConfig[5] + Environment.NewLine);
            msg.Append(labNum2.Text + AutoTray.getConfig[6] + Environment.NewLine);
            msg.Append(labSpace2.Text + AutoTray.getConfig[7] + Environment.NewLine);
            this.returnTxtBox2.Text = msg.ToString();
            msg.Clear();
            readBusy = false;
        }

        public void __InitWnd()
        {
            returnTxtBox1.Text = null;
            returnTxtBox2.Text = null;
            if (Auto_Flag.AutoTray_TakeIC || Auto_Flag.AutoTray_LayIC)
            {
                StartReadDotParameter();
            }
            this.__InitWndUI();   
        }

        private void __InitWndUI()
        {
            try
            {
                for (int i = 0; i < 8; i++)
                {
                    ArrNumTxt[i].Value = new decimal(AutoTray.setConfig[i]);
                    ArrNumTxt[8 + i].Value = new decimal(AutoTray.setConfig[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void BurnParaWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void saveButton1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 8; i++)
            {
                AutoTray.setConfig[i] = Convert.ToSingle(ArrNumTxt[i].Value);
            }
            g_config.WriteAutoTrayParaVal();
            if (Config.AutoTrayDot == 1 && (Auto_Flag.AutoTray_TakeIC || Auto_Flag.AutoTray_LayIC))
            {
                if ((AutoTray.StatusAlarmWord & AutoTray.Status_Status_DotBusy) == 0)
                {
                    AutoTray.MODBUS_WriteDotParameter();
                }  
            }
            UpdateUI();
        }

        private void readBtn_Click(object sender, EventArgs e)
        {
            if (!readBusy)
            {
                StartReadDotParameter();
            }
        }

        private void saveButton2_Click(object sender, EventArgs e)
        {
            if (returnTxtBox2.Text == null || readBusy)
            {
                return;
            }
            for (int i = 0; i < 8; i++)
            {
                AutoTray.setConfig[i] = AutoTray.getConfig[i];
            }
            __InitWndUI();
            g_config.WriteAutoTrayParaVal();
        }

    }
}
