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
    public partial class AxisParametersWnd : Form
    {
        Config g_config = Config.GetInstance();
        Act g_act = Act.GetInstance();

        NumericUpDown[] ArrNumTxt;

        string[] strRunModel;
        public AxisParametersWnd()
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();
            ArrNumTxt = new NumericUpDown[40];
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
            ArrNumTxt[32] = this.numericUpDown33;
            ArrNumTxt[33] = this.numericUpDown34;
            ArrNumTxt[34] = this.numericUpDown35;
            ArrNumTxt[35] = this.numericUpDown36;
            ArrNumTxt[36] = this.numericUpDown37;
            ArrNumTxt[37] = this.numericUpDown38;
            ArrNumTxt[38] = this.numericUpDown39;
            ArrNumTxt[39] = this.numericUpDown40;

            strRunModel = new string[]
            {
                "标准模式","精准模式"
            };
        }
        private void AxisParametersWnd_Load(object sender, EventArgs e)
        {
            InitCbo();
            __InitWndUI();
            
            for (int i = 0; i < 4; i++)
            {
                ArrNumTxt[i + 26].Visible = i < UserConfig.AxisZC ? true : false;
                string ctlName;
                Control[] tagAry;
                ctlName = "saveBtn" + (i + 27);
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    (tagAry.First() as ButtonBase).Visible = i < UserConfig.AxisZC ? true : false;
                }

                ctlName = "labelZ" + (i + 1);
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    (tagAry.First() as Label).Visible = i < UserConfig.AxisZC ? true : false;
                }
            }

            if (UserTask.PenType == 1)
            {
                labelZ1.Text = "原点修正Z:";
            }
        }
        public void __InitWndUI()
        {
            CboRunModel.SelectedIndex = Auto_Flag.RunModel;

            this.ArrNumTxt[0].Value = new decimal(Axis.runPrm_X.speed);

            this.ArrNumTxt[1].Value = new decimal(Axis.jogPrm_X.velHigh);

            this.ArrNumTxt[2].Value = new decimal(Axis.jogPrm_X.velLow);

            this.ArrNumTxt[3].Value = new decimal(Axis.runPrm_X.longAcc);

            this.ArrNumTxt[4].Value = new decimal(Axis.runPrm_X.longDec);

            this.ArrNumTxt[5].Value = new decimal(Axis.runPrm_X.shortAcc);

            this.ArrNumTxt[6].Value = new decimal(Axis.runPrm_X.shortDec);

            this.ArrNumTxt[7].Value = new decimal(Axis.runPrm_X.longMin);

            this.ArrNumTxt[8].Value = new decimal(Axis.homePrm_X.homeOffset);

            this.ArrNumTxt[9].Value = new decimal(Axis.Position_X.MaxVal);


            this.ArrNumTxt[10].Value = new decimal(Axis.runPrm_Y.speed);

            this.ArrNumTxt[11].Value = new decimal(Axis.jogPrm_Y.velHigh);

            this.ArrNumTxt[12].Value = new decimal(Axis.jogPrm_Y.velLow);

            this.ArrNumTxt[13].Value = new decimal(Axis.runPrm_Y.longAcc);

            this.ArrNumTxt[14].Value = new decimal(Axis.runPrm_Y.longDec);

            this.ArrNumTxt[15].Value = new decimal(Axis.runPrm_Y.shortAcc);

            this.ArrNumTxt[16].Value = new decimal(Axis.runPrm_Y.shortDec);

            this.ArrNumTxt[17].Value = new decimal(Axis.runPrm_Y.longMin);

            this.ArrNumTxt[18].Value = new decimal(Axis.homePrm_Y.homeOffset);

            this.ArrNumTxt[19].Value = new decimal(Axis.Position_Y.MaxVal);


            this.ArrNumTxt[20].Value = new decimal(Axis.runPrm_Z.speed);

            this.ArrNumTxt[21].Value = new decimal(Axis.trapPrm_C[0].speed);

            this.ArrNumTxt[22].Value = new decimal(Axis.jogPrm_Z[0].velHigh);

            this.ArrNumTxt[23].Value = new decimal(Axis.jogPrm_Z[0].velLow);

            this.ArrNumTxt[24].Value = new decimal(Axis.runPrm_Z.longAcc);

            this.ArrNumTxt[25].Value = new decimal(Axis.runPrm_Z.longDec);

            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                this.ArrNumTxt[26 + i].Value = new decimal(Axis.homePrm_Z[i].homeOffset);

                this.ArrNumTxt[30 + i].Value = new decimal(Axis.Pen[i].Offset_Carrier_X);
                this.ArrNumTxt[34 + i].Value = new decimal(Axis.Pen[i].Offset_Carrier_Y);
            }
            this.ArrNumTxt[38].Value = new decimal(Axis.Pen[0].Offset_Base_X);
            this.ArrNumTxt[39].Value = new decimal(Axis.Pen[0].Offset_Base_Y);
        }

        /// <summary>
        /// 初始化下拉框
        /// </summary>
        private void InitCbo()
        {
            AddCboItems(CboRunModel, strRunModel, Auto_Flag.RunModel);
        }

        /// <summary>
        /// 添加下拉框选项
        /// </summary>
        /// <param name="comboBox">下拉框</param>
        /// <param name="str">选项组</param>
        /// <param name="select">选择项</param>
        private void AddCboItems(ComboBox comboBox, string[] str, int select)
        {
            comboBox.Items.Clear();
            foreach (string sName in str)
            {
                string strGet = MultiLanguage.GetString(sName);
                comboBox.Items.Add(strGet);
            }
            comboBox.SelectedIndex = select;
        }

        private void SaveAllBtn_Click(object sender, EventArgs e)
        {
            Axis.runPrm_X.speed = g_config.runModel_s[Auto_Flag.RunModel].SpeedX = Convert.ToDouble(ArrNumTxt[0].Value);
            Axis.jogPrm_X.velHigh = Convert.ToDouble(ArrNumTxt[1].Value);
            Axis.jogPrm_X.velLow = Convert.ToDouble(ArrNumTxt[2].Value);
            Axis.runPrm_X.longAcc = g_config.runModel_s[Auto_Flag.RunModel].LongAcceSpeedX = Convert.ToDouble(ArrNumTxt[3].Value);
            Axis.runPrm_X.longDec = g_config.runModel_s[Auto_Flag.RunModel].LongDecSpeedX = Convert.ToDouble(ArrNumTxt[4].Value);
            Axis.runPrm_X.shortAcc = g_config.runModel_s[Auto_Flag.RunModel].ShortAcceSpeedX = Convert.ToDouble(ArrNumTxt[5].Value);
            Axis.runPrm_X.shortDec = g_config.runModel_s[Auto_Flag.RunModel].ShortDecSpeedX = Convert.ToDouble(ArrNumTxt[6].Value);
            Axis.runPrm_X.longMin = Convert.ToDouble(ArrNumTxt[7].Value);
            Axis.homePrm_X.homeOffset = Convert.ToDouble(ArrNumTxt[8].Value);
            Axis.Position_X.MinVal = -Axis.homePrm_X.homeOffset;
            Axis.Position_X.MaxVal = Convert.ToDouble(ArrNumTxt[9].Value);

            Axis.runPrm_Y.speed = g_config.runModel_s[Auto_Flag.RunModel].SpeedY = Convert.ToDouble(ArrNumTxt[10].Value);
            Axis.jogPrm_Y.velHigh = Convert.ToDouble(ArrNumTxt[11].Value);
            Axis.jogPrm_Y.velLow = Convert.ToDouble(ArrNumTxt[12].Value);
            Axis.runPrm_Y.longAcc = g_config.runModel_s[Auto_Flag.RunModel].LongAcceSpeedY = Convert.ToDouble(ArrNumTxt[13].Value);
            Axis.runPrm_Y.longDec = g_config.runModel_s[Auto_Flag.RunModel].LongDecSpeedY = Convert.ToDouble(ArrNumTxt[14].Value);
            Axis.runPrm_Y.shortAcc = g_config.runModel_s[Auto_Flag.RunModel].ShortAcceSpeedY = Convert.ToDouble(ArrNumTxt[15].Value);
            Axis.runPrm_Y.shortDec = g_config.runModel_s[Auto_Flag.RunModel].ShortDecSpeedY = Convert.ToDouble(ArrNumTxt[16].Value);
            Axis.runPrm_Y.longMin = Convert.ToDouble(ArrNumTxt[17].Value);
            Axis.homePrm_Y.homeOffset = Convert.ToDouble(ArrNumTxt[18].Value);
            Axis.Position_Y.MinVal = -Axis.homePrm_Y.homeOffset;
            Axis.Position_Y.MaxVal = Convert.ToDouble(ArrNumTxt[19].Value);

            Axis.runPrm_Z.speed = g_config.runModel_s[Auto_Flag.RunModel].SpeedZ = Convert.ToDouble(ArrNumTxt[20].Value);
            Axis.trapPrm_C[0].speed = g_config.runModel_s[Auto_Flag.RunModel].SpeedC = Convert.ToDouble(ArrNumTxt[21].Value);
            Axis.jogPrm_Z[0].velHigh = Convert.ToDouble(ArrNumTxt[22].Value);
            Axis.jogPrm_Z[0].velLow = Convert.ToDouble(ArrNumTxt[23].Value);
            Axis.runPrm_Z.longAcc = g_config.runModel_s[Auto_Flag.RunModel].AcceSpeedZ = Convert.ToDouble(ArrNumTxt[24].Value);
            Axis.runPrm_Z.longDec = g_config.runModel_s[Auto_Flag.RunModel].DecSpeedZ = Convert.ToDouble(ArrNumTxt[25].Value);

            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                Axis.homePrm_Z[i].homeOffset = Convert.ToDouble(ArrNumTxt[26 + i].Value);
                Axis.jogPrm_Z[i].velHigh = Axis.jogPrm_Z[0].velHigh;
                Axis.jogPrm_Z[i].velLow = Axis.jogPrm_Z[0].velLow;
                Axis.trapPrm_C[i].speed = Axis.trapPrm_C[0].speed;

                Axis.Pen[i].Offset_Carrier_X = Convert.ToDouble(ArrNumTxt[30 + i].Value);
                Axis.Pen[i].Offset_Carrier_Y = Convert.ToDouble(ArrNumTxt[34 + i].Value);
            }
            Axis.Pen[0].Offset_Base_X = Convert.ToDouble(ArrNumTxt[38].Value);
            Axis.Pen[0].Offset_Base_Y = Convert.ToDouble(ArrNumTxt[39].Value);
            g_config.GetOffset_TopCameraToPen();

            g_config.WriteAxisParaVal();
            g_config.WritePenOffset();
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "修改运行参数成功", "Flow");
        }

        private void AxisParametersWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void CboRunModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            Auto_Flag.RunModel = CboRunModel.SelectedIndex;
            g_config.WriteAxisParaVal();
            g_config.ReadAxisParaVal();
            __InitWndUI();
        }
    }
}
