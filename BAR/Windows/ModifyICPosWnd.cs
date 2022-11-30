using BAR.Commonlib;
using BAR.CommonLib_v1._0;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BAR.Windows
{
    public partial class ModifyICPosWnd : Form
    {
        public Config g_config = Config.GetInstance();
        public Act g_act = Act.GetInstance();
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        public static bool IsShow;
        BAR main = null;

        public ModifyICPosWnd()
        {
            InitializeComponent();
        }

        public void __InitUI()
        {
            string temp;
            if (TrayState.selectPanIndex == 2)
            {
                temp = MultiLanguage.DefaultLanguage == GlobConstData.ST_English? "NG_Tray" : "NG盘";
                btnTakePos.Enabled = false;
                btnEndPos.Enabled = false;
            }
            else
            {
                temp = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? $"Tray{TrayState.selectPanIndex + 1}" : $"料盘{TrayState.selectPanIndex + 1}";
                btnTakePos.Enabled = true;
                btnEndPos.Enabled = true;
            }
            if (MultiLanguage.DefaultLanguage == GlobConstData.ST_English)
            {
                label2.Text = string.Format(temp + "：{0:d} Row {1:d} Column", TrayState.selectRow, TrayState.selectCol);
            }
            else
            {
                label2.Text = string.Format(temp + "：{0:d}行 {1:d}列", TrayState.selectRow, TrayState.selectCol);
            }
            
        }

        private void btnTakePos_Click(object sender, EventArgs e)
        {
            if (TrayState.selectPanIndex == 2)
            {
                btnCancel_Click(null, null);
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "NG盘不支持取料位置设置", "Warning");
                return;
            }
            main.trayState.Check_RectangleF_ID(0);
            btnCancel_Click(null, null);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLayPos_Click(object sender, EventArgs e)
        {
            main.trayState.Check_RectangleF_ID(1);
            btnCancel_Click(null, null);
        }

        private void ClosureWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
            IsShow = false;
            TrayState.IsSelect = false;
        }

        private void btnEndPos_Click(object sender, EventArgs e)
        {
            if (TrayState.selectPanIndex == 2)
            {
                btnCancel_Click(null, null);
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "NG盘不支持结束位置设置", "Warning");
                return;
            }
            main.trayState.Check_RectangleF_ID(2);
            btnCancel_Click(null, null);
        }

        public void ModifyICPosWnd_Load(object sender, EventArgs e)
        {
            main = (BAR)this.Owner;
        }
    }
}
