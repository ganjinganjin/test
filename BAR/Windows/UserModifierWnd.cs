using BAR.CommonLib_v1._0;
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
    public partial class UserModifierWnd : Form
    {
        BAR main = null;
        public UserModifierWnd()
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            //不要光依赖 IsNullOrEmpty
            //Trim只能移除两边空白字符,不能移除中间
            //连中间一起移除
            //TDUserName = txtUserName.Text.Replace(" ", ""); //替换掉所有空白符(空格)
            string strShow;
            string TDUserPass = textUserPass.Text.Trim();
            string TDUserCoPass = this.textUserCoPass.Text.Trim();
            //先判断用户是不是都输入了数据
            if (string.IsNullOrEmpty(TDUserPass) || string.IsNullOrEmpty(TDUserCoPass))
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Please fill in all information correctly" : "请正确填写所有内容";
                MessageBox.Show(strShow);
                return; //不允许用户继续向下执行
            }
            //判断两次密码输入是否一致
            if (TDUserPass != TDUserCoPass)
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "The two passwords are different. Please enter them again" : "两次密码不一致,请重新输入";
                MessageBox.Show(strShow);
                textUserPass.Text = "";
                textUserCoPass.Text = "";
                return;
            }

            //判断当前用户是否有权限
            else if (StaticInfo.TDLevel != 2)
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Permission denied" : "权限不足";
                MessageBox.Show(this, strShow, "Information：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int res = TDMethod.Modify(StaticInfo.TDUser, TDUserPass);
            if (res > 0)
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "[ " + MultiLanguage.GetString(StaticInfo.TDUser) + " ] Changing the password succeeded!" : "[ " + StaticInfo.TDUser + " ] 修改密码成功！";
                MessageBox.Show(strShow);
                this.Close();
            }
            else
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "[ " + MultiLanguage.GetString(StaticInfo.TDUser) + " ] Failed to change the password!" : "[ " + StaticInfo.TDUser + " ] 修改密码失败！";
                MessageBox.Show(strShow);
            }
        }

        private void UserModifierWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            main.Activate();
        }

        private void UserModifierWnd_Activated(object sender, EventArgs e)
        {
            textUserName.Text = MultiLanguage.GetString(StaticInfo.TDUser);
        }

        private void UserModifierWnd_Load(object sender, EventArgs e)
        {
            main = (BAR)this.Owner;
        }
    }
}
