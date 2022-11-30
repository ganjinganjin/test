using BAR.Commonlib;
using BAR.CommonLib_v1._0;
using RRQMCore.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TensionDetectionDLL;




namespace BAR.Windows
{
    public partial class UserChangeWnd : Form
    {
        BAR main = null;
        public Act g_act = Act.GetInstance();
        public UserChangeWnd()
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();
            
            //this.ControlBox = false; // 设置不出现关闭按钮
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string strShow;
            //当控件不再是活动控件时 进行用户名检测
            string TDUserName = MultiLanguage.GetString(cobLoginName.Text.Trim()); //Trim只能移除两边空白字符,不能移除中间
            string TDUserPass = textLoginPass.Text.Trim();
            if (string.IsNullOrEmpty(TDUserName))
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Please enter the user name" : "请输入用户名";
                MessageBox.Show(strShow);
                return;
            }
            StaticInfo.TDUser = "";
            TDMethod.TDLogin(TDUserName, TDUserPass);
            if (!StaticInfo.CheckUserName())
            {
                textLoginPass.Clear();
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "User switchover failed. Please check the user name and password" : "切换用户失败，请检查用户名和密码";
                MessageBox.Show(this, strShow, "Information", MessageBoxButtons.OK,MessageBoxIcon.Error);
                textLoginPass.Clear();
                return;
            }
            string[] r = main.Text.Split("     ");
            main.Text = r[0] + "     " + MultiLanguage.GetString(StaticInfo.TDUser);
            main.PermissionControl();
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "切换[" + StaticInfo.TDUser + "]", "Flow");
            textLoginPass.Clear();
            this.Close();
        }
        

        private void UserChangeWnd_Load(object sender, EventArgs e)
        {
            __InitUI();

        }

        public void __InitUI()
        {
            main = (BAR)this.Owner;

            int TDRead = 1;
            TDMethod.TDReadDate(TDRead);
            //循环接收数据库中的用户名
            string[] cobName = new string[StaticInfo.nDate];
            for (int i = 0; i < StaticInfo.nDate; i++)
            {
                cobName[i] = MultiLanguage.GetString(StaticInfo.TDDate[i]);
            }

            //添加下拉控件项，并设置默认选择项
            cobLoginName.Items.Clear();
            cobLoginName.Items.AddRange(cobName);
            cobLoginName.SelectedIndex = cobLoginName.Items.Count > 0 ? 1 : -1;
        }

        private void UserChangeWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
            main.Activate();
        }
    }
}
