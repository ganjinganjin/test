using BAR.Commonlib;
using BAR.Commonlib.Events;
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
    public partial class BurnParaWnd : Form
    {
        Config g_config = Config.GetInstance();
        BurnProxy proxy = BurnProxy.GetInstance();
        bool readBusy = false;
        NumericUpDown[] ArrNumTxt;
        ComboBox[] ArrNumCBox;
        Label[] ArrNumLabel;

        public BurnParaWnd()
        {
            InitializeComponent();
            ArrNumTxt = new NumericUpDown[12];
            ArrNumCBox = new ComboBox[4];
            ArrNumLabel = new Label[4];

            ArrNumTxt[0] = this.numericUpDown1;
            ArrNumTxt[1] = this.numericUpDown2;
            ArrNumTxt[2] = this.numericUpDown3;
            ArrNumTxt[3] = this.numericUpDown4;
            ArrNumTxt[4] = this.numericUpDown5;
            ArrNumTxt[5] = this.numericUpDown6;
            ArrNumTxt[6] = this.numericUpDown7;
            ArrNumTxt[7] = this.numericUpDown8;
            ArrNumTxt[8] = this.numericUpDown9;
            ArrNumTxt[9] = this.numericUpDown10;
            ArrNumTxt[10] = this.numericUpDown11;
            ArrNumTxt[11] = this.numericUpDown12;

            ArrNumCBox[0] = this.Level_StartCBox;
            ArrNumCBox[1] = this.Level_BusyCBox;
            ArrNumCBox[2] = this.Level_OKCBox;
            ArrNumCBox[3] = this.Level_NGCBox;

            ArrNumLabel[0] = this.Level_StartLabel;
            ArrNumLabel[1] = this.Level_BusyLabel;
            ArrNumLabel[2] = this.Level_OKLabel;
            ArrNumLabel[3] = this.Level_NGLabel;
            proxy.ProxyNotifyEvent += this.OnMsgEvenHandle;
        }

        private void StartReadDownloadParameter()
        {
            if (readBusy)
            {
                return;
            }
            readBusy = true;
            Download_WG.MODBUS_ReadDownloadParameter(1);
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

            MessageBox.Show("烧录参数读取失败");
            readBusy = false;
        }

        private void OnMsgEvenHandle(MsgEvent e)
        {
            string[] strLevel = { "低电平L", "高电平H", "无电平X" };
            string[] strID = {"----",BurnPara.Name[0],BurnPara.Name[1],BurnPara.Name[2],BurnPara.Name[3],BurnPara.Name[4],
                BurnPara.Name[5],BurnPara.Name[6],BurnPara.Name[7],BurnPara.Name[8],BurnPara.Name[9],"硕飞_11","义隆DWTK8K_12",
                "义隆U-WTR_13","松翰MP-3_14","松翰MP PRO_15","芯圣HC-PM_16","合泰 e -W_17","PIC_18","三星GW-PRO2_19",
                "三星G-PROG_20","中颖Pro 06_21","建荣_22","飞林_23","博巨兴_24","芯睿_25","十速TWR99_26","麦肯一拖二_27",
                "应广_28","Nyquest九奇_29","易码_30","现代_31","中微CMS-WRITER_32","晟矽_33","众成_34"};
            StringBuilder msg = new StringBuilder();
            msg.Append("烧录器 型号：" + strID[Download_WG.getParameter.ID] + Environment.NewLine);
            msg.Append("启动   脉宽:" + Download_WG.getParameter.PulseWidth_Start + Environment.NewLine);
            msg.Append("Busy   时间:" + Download_WG.getParameter.Time_Busy + Environment.NewLine);
            msg.Append("EOT    时间:" + Download_WG.getParameter.Time_EOT + Environment.NewLine);
            msg.Append("OK/NG  时间:" + Download_WG.getParameter.Time_OKNG + Environment.NewLine);
            msg.Append("重复烧录次数:" + Download_WG.getParameter.RepeatNumber + Environment.NewLine);
            this.returnTxtBox1.Text = msg.ToString();

            msg.Clear();
            msg.AppendLine();
            msg.Append("启动电平:" + strLevel[Download_WG.getParameter.Level_Start] + Environment.NewLine);
            msg.Append("Busy电平:" + strLevel[Download_WG.getParameter.Level_Busy] + Environment.NewLine);
            msg.Append("OK  电平:" + strLevel[Download_WG.getParameter.Level_OK] + Environment.NewLine);
            msg.Append("NG  电平:" + strLevel[Download_WG.getParameter.Level_NG] + Environment.NewLine);
            msg.Append("烧录时间:" + Download_WG.getParameter.Time_Down + Environment.NewLine);
            this.returnTxtBox2.Text = msg.ToString();
            msg.Clear();
            readBusy = false;
        }

        public void __InitWnd()
        {
            StartReadDownloadParameter();
            this.__InitWndUI();   
        }

        private void __InitWndUI()
        {
            string[] strLevel = { "低电平L", "高电平H", "无电平X" };
            string[] strID = {BurnPara.Name[0],BurnPara.Name[1],BurnPara.Name[2],BurnPara.Name[3],BurnPara.Name[4],BurnPara.Name[5],
                BurnPara.Name[6],BurnPara.Name[7],BurnPara.Name[8],BurnPara.Name[9],
                "硕飞_11","义隆DWTK8K_12","义隆U-WTR_13","松翰MP-3_14","松翰MP PRO_15","芯圣HC-PM_16","合泰 e -W_17","PIC_18",
                "三星GW-PRO2_19","三星G-PROG_20","中颖Pro 06_21","建荣_22","飞林_23","博巨兴_24","芯睿_25","十速TWR99_26",
                "麦肯一拖二_27","应广_28","Nyquest九奇_29","易码_30","现代_31","中微CMS-WRITER_32","晟矽_33","众成_34"};

            IDCBox.Items.Clear();
            for (int i = 0; i < strID.Length; i++)
            {
                IDCBox.Items.Add(strID[i]);
            }

            if (Download_WG.programerID > strID.Length)
            {
                Download_WG.programerID = 0;
            }

            IDCBox.SelectedIndex = Download_WG.programerID;
            Download_WG.setParameter.ID = (UInt16)(Download_WG.programerID + 1);

            for (int i = 0; i < ArrNumCBox.Length; i++)
            {
                ArrNumCBox[i].Items.Clear();
                for (int j = 0; j < strLevel.Length; j++)
                {
                    ArrNumCBox[i].Items.Add(strLevel[j]);
                }
                GetSelectedIndex(i);
                ArrNumLabel[i].Text = this.ArrNumCBox[i].SelectedItem.ToString();
            }

            for (int i = 0; i < 6; i++)
            {
                ArrNumTxt[i].Enabled = IDCBox.SelectedIndex < 10;
            }
            for (int i = 0; i < 4; i++)
            {
                ArrNumCBox[i].Enabled = IDCBox.SelectedIndex < 10;
            }
            this.saveButton.Enabled = IDCBox.SelectedIndex < 10;
            this.BtnModifyName.Enabled = IDCBox.SelectedIndex < 10;

            this.ArrNumTxt[0].Value = new decimal(Download_WG.setParameter.PulseWidth_Start);
            this.ArrNumTxt[1].Value = new decimal(Download_WG.setParameter.Time_Busy);
            this.ArrNumTxt[2].Value = new decimal(Download_WG.setParameter.Time_EOT);
            this.ArrNumTxt[3].Value = new decimal(Download_WG.setParameter.Time_OKNG);
            this.ArrNumTxt[4].Value = new decimal(Download_WG.setParameter.RepeatNumber);
            this.ArrNumTxt[5].Value = new decimal(Download_WG.setParameter.Time_Down);

            this.ArrNumTxt[6].Value = new decimal(Download_WG.setParameter.PulseWidth_Start);
            this.ArrNumTxt[7].Value = new decimal(Download_WG.setParameter.Time_Busy);
            this.ArrNumTxt[8].Value = new decimal(Download_WG.setParameter.Time_EOT);
            this.ArrNumTxt[9].Value = new decimal(Download_WG.setParameter.Time_OKNG);
            this.ArrNumTxt[10].Value = new decimal(Download_WG.setParameter.RepeatNumber);
            this.ArrNumTxt[11].Value = new decimal(Download_WG.setParameter.Time_Down);
        }

        private void GetSelectedIndex(int index)
        {
            if(index == 0)
            {
                ArrNumCBox[index].SelectedIndex = Download_WG.setParameter.Level_Start;
            }
            else if (index == 1)
            {
                ArrNumCBox[index].SelectedIndex = Download_WG.setParameter.Level_Busy;
            }
            else if (index == 2)
            {
                ArrNumCBox[index].SelectedIndex = Download_WG.setParameter.Level_OK;
            }
            else if (index == 3)
            {
                ArrNumCBox[index].SelectedIndex = Download_WG.setParameter.Level_NG;
            }
        }

        private void BurnParaWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            IDCBox.Items.Clear();
            for (int i = 0; i < ArrNumCBox.Length; i++)
            {
                ArrNumCBox[i].Items.Clear();
            }
            Hide();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Download_WG.setParameter.PulseWidth_Start = Convert.ToUInt16(ArrNumTxt[0].Value);
            Download_WG.setParameter.Time_Busy = Convert.ToUInt16(ArrNumTxt[1].Value);
            Download_WG.setParameter.Time_EOT = Convert.ToUInt16(ArrNumTxt[2].Value);
            Download_WG.setParameter.Time_OKNG = Convert.ToUInt16(ArrNumTxt[3].Value);
            Download_WG.setParameter.RepeatNumber = Convert.ToUInt16(ArrNumTxt[4].Value);
            Download_WG.setParameter.Time_Down = Convert.ToUInt16(ArrNumTxt[5].Value);
            Download_WG.setParameter.Level_Start = (ushort)Level_StartCBox.SelectedIndex;
            Download_WG.setParameter.Level_Busy = (ushort)Level_BusyCBox.SelectedIndex;
            Download_WG.setParameter.Level_OK = (ushort)Level_OKCBox.SelectedIndex;
            Download_WG.setParameter.Level_NG = (ushort)Level_NGCBox.SelectedIndex;
            g_config.WriteProgramerInfo(IDCBox.SelectedIndex);
            this.__InitWndUI();
        }

        private void IDCBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Download_WG.programerID = (UInt16)IDCBox.SelectedIndex;
            if (Download_WG.programerID < 10)
            {
                g_config.ReadProgramerInfo();
            }
            else
            {
                Download_WG.setParameter = Download_WG.const_Down_Param[Download_WG.programerID - 10];
            }
            g_config.WriteProgramerID();
            this.__InitWndUI();
        }

        private void readBtn_Click(object sender, EventArgs e)
        {
            if (!readBusy)
            {
                StartReadDownloadParameter();
            }
        }

        private void writeBtn_Click(object sender, EventArgs e)
        {
            if (!readBusy)
            {
                for (byte i = 1; i <= Download_WG.PORT_NUM; i++)
                {
                    Download_WG.MODBUS_WriteDownloadParameter(i);
                }
                StartReadDownloadParameter();
            } 
        }

        private void BtnModifyName_Click(object sender, EventArgs e)
        {
            inputPnl.Visible = true;
            TxtName.Text = BurnPara.Name[IDCBox.SelectedIndex];
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            int ind = IDCBox.SelectedIndex;
            string strName = TxtName.Text.Trim();
            bool IsSame = false;
            for (int i = 0; i < BurnPara.Name.Length; i++)
            {
                if (strName == BurnPara.Name[i])
                {
                    MessageBox.Show("该型号名称已存在！！！", "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    IsSame = true;
                    break;
                }
            }

            if (!IsSame)
            {
                BurnPara.Name[ind] = strName;
                g_config.WriteBurnPara();
                __InitWndUI();
                MessageBox.Show("型号名称更改成功", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                inputPnl.Visible = false;
            }
            
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            inputPnl.Visible = false;
        }

        
    }
}
