using BAR.Commonlib;
using BAR.Commonlib.Utils;
using BAR.Windows;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BAR.ControlPanels
{
    public partial class ICDirAndFlawPnl : UserControl
    {
        ConfigInitWnd _ConInitWnd;
        Config g_config = Config.GetInstance();
        Act g_act = Act.GetInstance();
        HalconImgUtil _HalconUtil = HalconImgUtil.GetInstance();
        HTuple WindowHandle1;
        bool isLoaded = false;

        public ICDirAndFlawPnl()
        {
            InitializeComponent();
        }

        public ICDirAndFlawPnl(ConfigInitWnd configWnd) : this()
        {
            this._ConInitWnd = configWnd;
        }

        private void ICDirAndFlawPnl_Load(object sender, EventArgs e)
        {
            if (!isLoaded)
            {
                __InitUI();
                isLoaded = true;
            }
            //_ConInitWnd.IIndexModel = _ConInitWnd.ISelBoxInd = GlobConstData.ST_MODELICPOS;
            g_act.SendCmd(GlobConstData.ST_LIGHTDOWN, g_config.ArrLampBox[GlobConstData.ST_MODELICPOS].ILightDown);
            DispModelImage1(GlobConstData.ST_MODELICPOS);
        }
        private void __InitUI()
        {
            __InitComboBtn();
        }
        private void __InitComboBtn()
        {
            //吸笔
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                String str = "吸笔" + (i + 1);
                this.penCBox.Items.Add(str);
            }
            penCBox.SelectedIndex = 0;

            //烧写座
            for (int i = 0; i < UserConfig.AllScketC; i++)
            {
                String str = "烧写座" + (i / UserConfig.ScketUnitC + 1) + "_" + (i % UserConfig.ScketUnitC + 1);
                this.seatCBox.Items.Add(str);
            }
            seatCBox.SelectedIndex = 0;

            //角度
            this.angCbo.Items.Add("40");
            this.angCbo.Items.Add("30");
            this.angCbo.Items.Add("20");
            this.angCbo.Items.Add("10");
            this.angCbo.Items.Add("5");
            if (g_config.DAngLimit == 40) angCbo.SelectedIndex = 0;
            else if (g_config.DAngLimit == 30) angCbo.SelectedIndex = 1;
            else if (g_config.DAngLimit == 20) angCbo.SelectedIndex = 2;
            else if (g_config.DAngLimit == 10) angCbo.SelectedIndex = 3;
            else if (g_config.DAngLimit == 5) angCbo.SelectedIndex = 4;

            //贪婪度
            this.greedCbo.Items.Add("0.9");
            this.greedCbo.Items.Add("0.8");
            this.greedCbo.Items.Add("0.7");
            this.greedCbo.Items.Add("0.6");
            this.greedCbo.Items.Add("0.5");
            this.greedCbo.Items.Add("0.4");
            this.greedCbo.Items.Add("0.3");
            this.greedCbo.Items.Add("0.2");
            this.greedCbo.Items.Add("0.1");
            if (g_config.DGreediness == 0.9) greedCbo.SelectedIndex = 0;
            else if (g_config.DGreediness == 0.8) greedCbo.SelectedIndex = 1;
            else if (g_config.DGreediness == 0.7) greedCbo.SelectedIndex = 2;
            else if (g_config.DGreediness == 0.6) greedCbo.SelectedIndex = 3;
            else if (g_config.DGreediness == 0.5) greedCbo.SelectedIndex = 4;
            else if (g_config.DGreediness == 0.4) greedCbo.SelectedIndex = 5;
            else if (g_config.DGreediness == 0.3) greedCbo.SelectedIndex = 6;
            else if (g_config.DGreediness == 0.2) greedCbo.SelectedIndex = 7;
            else if (g_config.DGreediness == 0.1) greedCbo.SelectedIndex = 8;

            //重叠率
            this.duplicCbo.Items.Add("0.7");
            this.duplicCbo.Items.Add("0.6");
            this.duplicCbo.Items.Add("0.5");
            this.duplicCbo.Items.Add("0.4");
            this.duplicCbo.Items.Add("0.3");
            if (g_config.DMaxOverlap == 0.7) duplicCbo.SelectedIndex = 0;
            else if (g_config.DMaxOverlap == 0.6) duplicCbo.SelectedIndex = 1;
            else if (g_config.DMaxOverlap == 0.5) duplicCbo.SelectedIndex = 2;
            else if (g_config.DMaxOverlap == 0.4) duplicCbo.SelectedIndex = 3;
            else if (g_config.DMaxOverlap == 0.3) duplicCbo.SelectedIndex = 4;

            //相似率
            this.soccerCbo.Items.Add("0.9");
            this.soccerCbo.Items.Add("0.8");
            this.soccerCbo.Items.Add("0.7");
            this.soccerCbo.Items.Add("0.6");
            this.soccerCbo.Items.Add("0.5");
            this.soccerCbo.Items.Add("0.4");
            this.soccerCbo.Items.Add("0.3");
            this.soccerCbo.Items.Add("0.2");
            this.soccerCbo.Items.Add("0.1");
            if (g_config.DScore == 0.9) soccerCbo.SelectedIndex = 0;
            else if (g_config.DScore == 0.8) soccerCbo.SelectedIndex = 1;
            else if (g_config.DScore == 0.7) soccerCbo.SelectedIndex = 2;
            else if (g_config.DScore == 0.6) soccerCbo.SelectedIndex = 3;
            else if (g_config.DScore == 0.5) soccerCbo.SelectedIndex = 4;
            else if (g_config.DScore == 0.4) soccerCbo.SelectedIndex = 5;
            else if (g_config.DScore == 0.3) soccerCbo.SelectedIndex = 6;
            else if (g_config.DScore == 0.2) soccerCbo.SelectedIndex = 7;
            else if (g_config.DScore == 0.1) soccerCbo.SelectedIndex = 8;

            //对比度
            this.contrastCbo.Items.Add("auto");
            for (int n = 10; n < 250; n = n + 10)
            {
                this.contrastCbo.Items.Add(n);
            }
            contrastCbo.SelectedIndex = (g_config.IContrast / 10);

            //最小对比度
            this.minContrastCbo.Items.Add("auto");
            for (int n = 10; n < 250; n = n + 10)
            {
                this.minContrastCbo.Items.Add(n);
            }
            minContrastCbo.SelectedIndex = (g_config.IMinContrast / 10);
        }
        public void DispModelImage1(short modelIndex)
        {
            if (WindowHandle1 != null) HOperatorSet.CloseWindow(WindowHandle1);
            HOperatorSet.OpenWindow(0, 0, modelPicBox1.Width, modelPicBox1.Height, modelPicBox1.Handle, "visible", "", out WindowHandle1);
            String path = g_config.StrProductDir + "\\set_" + modelIndex + ".jpg";
            if (File.Exists(path))//判断文件是否存在
            {
                HObject img;
                HOperatorSet.ClearWindow(WindowHandle1);
                _HalconUtil.LoadImgAndDisp(WindowHandle1, out img, path);
                img?.Dispose();
            }
        }
    }
}
