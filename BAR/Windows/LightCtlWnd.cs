using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BAR.Commonlib;
using BAR.CommonLib_v1._0;

namespace BAR.Windows
{
    public partial class LightCtlWnd : Form
    {
        public Act      g_act = Act.GetInstance();
        public Config   g_config = Config.GetInstance();

        public ConfigInitWnd _ConInitWnd;
        public short    ILimit;
        public LightCtlWnd()
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();
            if (g_config.ILightType == 1 || g_config.ILightType == 2)
            {
                ILimit = 255;
            }
            else
            {
                ILimit = 999;
            }
        }
        public LightCtlWnd(ConfigInitWnd configWnd):this()
        {
            this._ConInitWnd = configWnd;
        }
        private void LightCtlWnd_Load(object sender, EventArgs e)
        {
            SetValue();
        }

        private void upLightBar_Scroll(object sender, EventArgs e)
        {
            double percent = (double)(100 - this.upLightBar.Value) / 100;
            int lightNum = (int)(ILimit * percent);
            this.upTxtBox.Text = Convert.ToString(lightNum);
        }

        private void downLightBar_Scroll(object sender, EventArgs e)
        {
            double percent = (double)(100 - this.downLightBar.Value) / 100;
            int lightNum = (int)(ILimit * percent);
            this.downTxtBox.Text = Convert.ToString(lightNum);
        }
        private void SetValue()
        {
            int configNum, lightNum;
            double percent;
            //判断光源值是否在允许期间内
            if (g_config.ArrLampBox[_ConInitWnd.ISelBoxInd].ILightUp > ILimit || g_config.ArrLampBox[_ConInitWnd.ISelBoxInd].ILightUp < 0)
            {
                configNum = 0;
            }
            else
            {
                configNum = g_config.ArrLampBox[_ConInitWnd.ISelBoxInd].ILightUp;
            }
            percent = 1 - (double)configNum / ILimit;
            lightNum = (int)(100 * percent);
            this.upLightBar.Value = lightNum;
            this.upTxtBox.Text = Convert.ToString(configNum);

            //判断光源值是否在允许期间内
            if (g_config.ArrLampBox[_ConInitWnd.ISelBoxInd].ILightDown > ILimit || g_config.ArrLampBox[_ConInitWnd.ISelBoxInd].ILightDown < 0)
            {
                configNum = 0;
            }
            else
            {
                configNum = g_config.ArrLampBox[_ConInitWnd.ISelBoxInd].ILightDown;
            }
            percent = 1 - (double)configNum / ILimit;
            lightNum = (int)(100 * percent);
            this.downLightBar.Value = lightNum;
            this.downTxtBox.Text = Convert.ToString(configNum);

        }
        private void saveBtn_Click(object sender, EventArgs e)
        {
            g_act.WaitDoEvent(50);
            g_config.ArrLampBox[_ConInitWnd.ISelBoxInd].ILightUp = Convert.ToInt32(this.upTxtBox.Text);
            g_config.ArrLampBox[_ConInitWnd.ISelBoxInd].ILightDown = Convert.ToInt32(this.downTxtBox.Text);
            g_config.SaveLightVal(_ConInitWnd.ISelBoxInd);
        }

        private void upTxtBox_ValueChanged(object sender, EventArgs e)
        {
            int lightNum = 0;
            lightNum = Convert.ToInt32(this.upTxtBox.Text);
            g_act.SendCmd(0, lightNum);
        }

        private void downTxtBox_ValueChanged(object sender, EventArgs e)
        {
            int lightNum = 0;
            lightNum = Convert.ToInt32(this.downTxtBox.Text);
            g_act.SendCmd(1, lightNum);
        }
    }
}
