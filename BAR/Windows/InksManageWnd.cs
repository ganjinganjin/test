using BAR.Commonlib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BAR.Windows
{
    public partial class InksManageWnd : Form
    {
        public Config g_config = Config.GetInstance();
        BAR main = null;

        public InksManageWnd()
        {
            InitializeComponent();
        }

        private void InksManageWnd_Load(object sender, EventArgs e)
        {
            main = (BAR)this.Owner;
        }

        public void InitUI()
        {
            NudDotAutoTray.Value = Convert.ToDecimal(Inks.DotCount_AutoTray);
            NudDotBraid.Value = Convert.ToDecimal(Inks.DotCount_Braid);
            NudTimeAutoTray.Value = Convert.ToDecimal(Inks.TimeCount_AutoTray);
            NudTimeBraid.Value = Convert.ToDecimal(Inks.TimeCount_Braid);
        }

        private void ChkAutoTrayEnabled_Click(object sender, EventArgs e)
        {
            Inks.Enabled_AutoTray = !Inks.Enabled_AutoTray;
            g_config.WriteInksDate();
        }

        private void ChkBraidEnabled_Click(object sender, EventArgs e)
        {
            Inks.Enabled_Braid = !Inks.Enabled_Braid;
            g_config.WriteInksDate();
        }

        private void BtnResetAutoTray_Click(object sender, EventArgs e)
        {
            
        }

        private void BtnResetBraid_Click(object sender, EventArgs e)
        {
            Inks.DateTime_Braid = DateTime.Now;
            g_config.WriteInksDate();
        }

        private void InksManageWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
            main.Activate();
        }

        private void BtnSaveAutoTray_Click(object sender, EventArgs e)
        {
            Inks.DotCount_AutoTray = Convert.ToDouble(NudDotAutoTray.Value);
            Inks.TimeCount_AutoTray = Convert.ToDouble(NudTimeAutoTray.Value);
            g_config.WriteInksDate();
            MessageBox.Show("保存自动盘油墨数据成功！", "温馨提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnSaveBraid_Click(object sender, EventArgs e)
        {
            Inks.DotCount_Braid = Convert.ToDouble(NudDotBraid.Value);
            Inks.TimeCount_Braid = Convert.ToDouble(NudTimeBraid.Value);
            g_config.WriteInksDate();
            MessageBox.Show("保存编带油墨数据成功！", "温馨提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
