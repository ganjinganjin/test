using BAR.Commonlib;
using BAR.CommonLib_v1._0;
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
    public partial class WarningLimitWnd : Form
    {
        public Act g_act = Act.GetInstance();
        Config g_config = Config.GetInstance();

        public WarningLimitWnd()
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();
            this.ControlBox = false;
        }

        public void InitWnd()
        {
            lvToolTip.Items.Clear();
        }

        public void WriteStr(string str)
        {
            str = MultiLanguage.GetString(str);
            string ID = (lvToolTip.Items.Count + 1).ToString();
            ListViewItem lvItem = new ListViewItem();//新建行
            lvItem.SubItems.Add(ID);//写入序号
            lvItem.SubItems.Add(str);//写入告警内容
            lvToolTip.Items.Add(lvItem);//更新控件显示
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Alarm");
            g_act.RecordAlarmsInfo(str);
        }

        private void WarningLimitWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
