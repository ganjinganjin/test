using BAR.CommonLib_v1._0;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BAR
{
    public partial class LoadingBar : Form
    {
        int loadTotalW;

        public LoadingBar()
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();
           
        }
        public void ShowProgressPos(String info, int percent)
        {
            this.infoLab.Text = MultiLanguage.GetString(info);
            float dPercent = (float)percent / 100;
            this.loadBar.Width = (int)(dPercent * loadTotalW);
        }
        //取消
        private void btnCancel_Click(object sender, EventArgs e)
        {
        }

        private void LoadingBar_Load(object sender, EventArgs e)
        {
            loadTotalW = this.loadBar.Width;
            CheckForIllegalCrossThreadCalls = false;
        }
        private void LoadingBar_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if(!(this.Owner as BAR).LoadDone)
            //{
            //    e.Cancel = true;
            //}
            
            //this.Visible = false;
        }


    }
}
