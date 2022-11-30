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
using BAR.Communicates;
using System.Threading;
using BAR.Commonlib.Events;
using System.Threading.Tasks;
using BAR.CommonLib_v1._0;

namespace BAR.Windows
{
    public partial class BraidParametersWnd : Form
    {
        Config g_config = Config.GetInstance();
        BraidProxy proxy = BraidProxy.GetInstance();
        bool readBusy = false;

        NumericUpDown[] ArrNumTxt;

        public BraidParametersWnd()
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();
            ArrNumTxt = new NumericUpDown[32];
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
            ArrNumTxt[12] = this.numericUpDown13;
            ArrNumTxt[13] = this.numericUpDown14;
            ArrNumTxt[14] = this.numericUpDown15;
            ArrNumTxt[15] = this.numericUpDown16;
            ArrNumTxt[16] = this.numericUpDown17;
            ArrNumTxt[17] = this.numericUpDown18;
            ArrNumTxt[18] = this.numericUpDown19;
            ArrNumTxt[19] = this.numericUpDown20;
            ArrNumTxt[20] = this.numericUpDown21;
            ArrNumTxt[21] = this.numericUpDown22;
            ArrNumTxt[22] = this.numericUpDown23;
            ArrNumTxt[23] = this.numericUpDown24;
            ArrNumTxt[24] = this.numericUpDown25;
            ArrNumTxt[25] = this.numericUpDown26;
            ArrNumTxt[26] = this.numericUpDown27;
            ArrNumTxt[27] = this.numericUpDown28;
            ArrNumTxt[28] = this.numericUpDown29;
            ArrNumTxt[29] = this.numericUpDown30;
            ArrNumTxt[30] = this.numericUpDown31;
            ArrNumTxt[31] = this.numericUpDown32;
            __InitWndUI2();
            if (Config.BredeDotCount == 0)//编带单打点
            {
                label12.Text = MultiLanguage.IsEnglish() == true ? "DotA_Empty:" : "打点空数:";
                label10.Text = MultiLanguage.IsEnglish() == true ? "RES" : "备用:";
            }
            proxy.ProxyNotifyEvent += this.OnMsgEvenHandle;
        }

        private void StartReadParameter()
        {
            if (readBusy)
            {
                return;
            }
            readBusy = true;
            Brede.MODBUS_ReadBredeParameter();
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

            MessageBox.Show("编带参数读取失败");
            flashBtn.Text = MultiLanguage.GetString("刷新");
            readBusy = false;
        }

        public void __InitWnd()
        {
            if (Auto_Flag.Brede_LayIC)
            {
                StartReadParameter();
            }
        }
       
        private void OnMsgEvenHandle(MsgEvent e)
        {
            switch(e.MsgType)
            {
                case MsgEvent.MSG_BRAIDFLASH:
                    __InitWndUI();
                    flashBtn.Text = MultiLanguage.GetString("刷新");
                    readBusy = false;
                    break;
            }
           
        }
        private void __InitWndUI()
        {
            this.ArrNumTxt[7].Value = new decimal(Brede.getConfig.tHotMelt);
            this.ArrNumTxt[8].Value = new decimal(Brede.getConfig.tDot);
            this.ArrNumTxt[9].Value = new decimal(Brede.getConfig.tEmptyBredeFrap);
            this.ArrNumTxt[10].Value = new decimal(Brede.getConfig.tEmptyBredeSend);
            this.ArrNumTxt[11].Value = new decimal(Brede.getConfig.tBraidFrap);
            this.ArrNumTxt[12].Value = new decimal(Brede.getConfig.acc);
            this.ArrNumTxt[13].Value = new decimal(Brede.getConfig.dec);
        }

        private void __InitWndUI2()
        {
            this.ArrNumTxt[0].Value = new decimal(Brede.setConfig.tHotMelt);
            this.ArrNumTxt[1].Value = new decimal(Brede.setConfig.tDot);
            this.ArrNumTxt[2].Value = new decimal(Brede.setConfig.tEmptyBredeFrap);
            this.ArrNumTxt[3].Value = new decimal(Brede.setConfig.tEmptyBredeSend);
            this.ArrNumTxt[4].Value = new decimal(Brede.setConfig.tBraidFrap);
            this.ArrNumTxt[5].Value = new decimal(Brede.setConfig.acc);
            this.ArrNumTxt[6].Value = new decimal(Brede.setConfig.dec);

            this.ArrNumTxt[14].Value = new decimal(Brede.setConfig.space);
            this.ArrNumTxt[15].Value = new decimal(Brede.setConfig.speed);
            this.ArrNumTxt[16].Value = new decimal(Brede_Number.CntVal_FrontEmptyMaterial);
            this.ArrNumTxt[17].Value = new decimal(Brede_Number.CntVal_End);
            this.ArrNumTxt[18].Value = new decimal(Brede_Number.CntVal_MarkAEmptyMaterial);
            this.ArrNumTxt[19].Value = new decimal(Brede_Number.CntVal_Manual);
            this.ArrNumTxt[20].Value = new decimal(Brede_Number.CntVal_MarkBEmptyMaterial);
            this.ArrNumTxt[28].Value = new decimal(Brede_Number.CntVal_HotMelt);
            this.ArrNumTxt[30].Value = new decimal(Brede_Number.CntVal_CCDEmptyMaterial);

            this.ArrNumTxt[21].Value = new decimal(Brede.getConfig.space);
            this.ArrNumTxt[22].Value = new decimal(Brede.getConfig.speed);
            this.ArrNumTxt[23].Value = new decimal(Brede_Number.CntVal_FrontEmptyMaterial);
            this.ArrNumTxt[24].Value = new decimal(Brede_Number.CntVal_End);
            this.ArrNumTxt[25].Value = new decimal(Brede_Number.CntVal_MarkAEmptyMaterial);
            this.ArrNumTxt[26].Value = new decimal(Brede_Number.CntVal_Manual);
            this.ArrNumTxt[27].Value = new decimal(Brede_Number.CntVal_MarkBEmptyMaterial);
            this.ArrNumTxt[29].Value = new decimal(Brede_Number.CntVal_HotMelt);
            this.ArrNumTxt[31].Value = new decimal(Brede_Number.CntVal_CCDEmptyMaterial);
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (readBusy)
            {
                return;
            }
            saveBtn.Text = MultiLanguage.GetString("保存中...");
            g_config.ArrBraidParas[0].IData = Convert.ToUInt16(this.ArrNumTxt[0].Value);

            g_config.ArrBraidParas[1].IData = Convert.ToUInt16(this.ArrNumTxt[1].Value);

            g_config.ArrBraidParas[2].IData = Convert.ToUInt16(this.ArrNumTxt[2].Value);

            g_config.ArrBraidParas[3].IData = Convert.ToUInt16(this.ArrNumTxt[3].Value);

            g_config.ArrBraidParas[4].IData = Convert.ToUInt16(this.ArrNumTxt[4].Value);

            g_config.ArrBraidParas[5].IData = Convert.ToUInt16(this.ArrNumTxt[5].Value);

            g_config.ArrBraidParas[6].IData = Convert.ToUInt16(this.ArrNumTxt[6].Value);


            g_config.ArrBraidParas[7].DData = Convert.ToSingle(this.ArrNumTxt[14].Value);

            g_config.ArrBraidParas[8].DData = Convert.ToSingle(this.ArrNumTxt[15].Value);

            g_config.ArrBraidParas[9].IData = Convert.ToUInt16(this.ArrNumTxt[16].Value);

            g_config.ArrBraidParas[10].IData = Convert.ToUInt16(this.ArrNumTxt[17].Value);

            g_config.ArrBraidParas[11].IData = Convert.ToUInt16(this.ArrNumTxt[18].Value);

            g_config.ArrBraidParas[12].IData = Convert.ToUInt16(this.ArrNumTxt[19].Value);

            g_config.ArrBraidParas[16].IData = Convert.ToUInt16(this.ArrNumTxt[20].Value);

            g_config.ArrBraidParas[13].IData = Convert.ToUInt16(this.ArrNumTxt[29].Value);

            g_config.ArrBraidParas[14].IData = Convert.ToUInt16(this.ArrNumTxt[31].Value);


            Brede.setConfig.tHotMelt = (ushort)g_config.ArrBraidParas[0].IData;

            Brede.setConfig.tDot = (ushort)g_config.ArrBraidParas[1].IData;

            Brede.setConfig.tEmptyBredeFrap = (ushort)g_config.ArrBraidParas[2].IData;

            Brede.setConfig.tEmptyBredeSend = (ushort)g_config.ArrBraidParas[3].IData;

            Brede.setConfig.tBraidFrap = (ushort)g_config.ArrBraidParas[4].IData;

            Brede.setConfig.acc = (ushort)g_config.ArrBraidParas[5].IData;

            Brede.setConfig.dec = (ushort)g_config.ArrBraidParas[6].IData;

            Brede.setConfig.space = (float)g_config.ArrBraidParas[7].DData;

            Brede.setConfig.speed = (float)g_config.ArrBraidParas[8].DData;

            Brede_Number.CntVal_FrontEmptyMaterial = (ushort)g_config.ArrBraidParas[9].IData;

            Brede_Number.CntVal_End = (ushort)g_config.ArrBraidParas[10].IData;

            Brede_Number.CntVal_MarkAEmptyMaterial = (ushort)g_config.ArrBraidParas[11].IData;
            Brede_Number.CntVal_MarkBEmptyMaterial = (ushort)g_config.ArrBraidParas[16].IData;

            Brede_Number.CntVal_Manual = (ushort)g_config.ArrBraidParas[12].IData;

            Brede_Number.CntVal_HotMelt = (ushort)g_config.ArrBraidParas[13].IData;

            Brede_Number.CntVal_CCDEmptyMaterial = (ushort)g_config.ArrBraidParas[14].IData;

            Brede.MODBUS_WriteBredeParameter();

            g_config.WriteBraidParaVal();

            Brede.MODBUS_ReadBredeParameter();

            saveBtn.Text = MultiLanguage.GetString("保存");
            __InitWndUI2();
        }

        private void flashBtn_Click(object sender, EventArgs e)
        {
            if (readBusy)
            {
                return;
            }
            flashBtn.Text = MultiLanguage.GetString("刷新中...");
            __InitWndUI2();
            StartReadParameter();
        }

        private void BraidParametersWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
