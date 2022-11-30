using BAR.Commonlib;
using BAR.CommonLib_v1._0;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BAR.Windows
{
    public partial class ClosureWnd : Form
    {
        public BAR main = null;
        public Act g_act = Act.GetInstance();
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);
        public ClosureWnd()
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();
        }

        public void __InitUI()
        {
            if (Auto_Flag.AutoRunBusy && Auto_Flag.ManualEnd)
            {
                label1.Text = MultiLanguage.GetString("正在执行收尾动作");
                btnYes.Visible = false;
                btnNo.Visible = false;
                btnSure.Visible = true;
            }
            else if (Auto_Flag.AutoRunBusy && !Auto_Flag.ManualEnd)
            {
                label1.Text = MultiLanguage.GetString("是否执行收尾动作");
                btnYes.Visible = true;
                btnNo.Visible = true;
                btnSure.Visible = false;
            }
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "手动收尾", "Flow");
            Auto_Flag.Next = true;
            Auto_Flag.ManualEnd = true;
            Auto_Flag.Pause = false;
            Auto_Flag.Ending = true;
            Auto_Flag.ForceEnd = true;
            main._StaticalWnd.__InitWndUI();
            btnNo_Click(null, null);
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            IntPtr Hwnd = FindWindow(null, MultiLanguage.GetString("收尾提示"));
            if (Hwnd != IntPtr.Zero)
            {
                ShowWindow(Hwnd, 0);
            }
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            btnNo_Click(null, null);
        }

        private void ClosureWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void ClosureWnd_Load(object sender, EventArgs e)
        {
            main = (BAR)this.Owner;
        }
    }
}
