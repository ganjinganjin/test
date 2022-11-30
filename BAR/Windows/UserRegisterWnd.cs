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
    public partial class UserRegisterWnd : Form
    {
        BAR main = null;
        public UserRegisterWnd()
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();
        }
        
        
        private void textUserName_Leave(object sender, EventArgs e)
        {
            //当控件不再是活动控件时 进行用户名检测
            string TDUserName = textUserName.Text.Trim(); //Trim只能移除两边空白字符,不能移除中间 
            string strShow;
            if (string.IsNullOrEmpty(TDUserName))
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Please enter the user name" : "请输入用户名";
                MessageBox.Show(strShow);
                return;
            }
            int res=TDMethod.CheckUserName(TDUserName);
            if (res != 0)
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "【 " + TDUserName + " 】 have been registered" : "【 " + TDUserName + " 】 已被注册";
                MessageBox.Show(this, strShow, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //切回焦点
                textUserName.Focus();
            }
        }

        private void textUserName_TextChanged(object sender, EventArgs e)
        {
            //在这里使用change事件来判断是否超长
            if (textUserName.Text.Length > 15)
            {
                string strShow;
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "The user name contains a maximum of 15 characters" : "用户名最大长度为15个字符";
                MessageBox.Show(strShow);
                //处理超长的数据
                //1,截断.直接截断数据.
                textUserName.Text = textUserName.Text.Substring(0, 15);
                //2.清空. textUserName.Text = "";
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            //不要光依赖 IsNullOrEmpty
            string TDUserName = textUserName.Text.Trim(); //Trim只能移除两边空白字符,不能移除中间
            //连中间一起移除
            //TDUserName = txtUserName.Text.Replace(" ", ""); //替换掉所有空白符(空格)
            string strShow;
            string TDUserPass = textUserPass.Text.Trim();
            string TDUserCoPass = this.textUserCoPass.Text.Trim();
            //先判断用户是不是都输入了数据
            if (string.IsNullOrEmpty(TDUserName) || string.IsNullOrEmpty(TDUserPass) || string.IsNullOrEmpty(TDUserCoPass))
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Please fill in all information correctly" : "请正确填写所有内容";
                MessageBox.Show(strShow, "Information：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return; //不允许用户继续向下执行
            }
            //判断两次密码输入是否一致
            if (TDUserPass != TDUserCoPass)
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "The two passwords are different. Please enter them again" : "两次密码不一致,请重新输入";
                MessageBox.Show(strShow, "Information：", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            int res = TDMethod.Reg(TDUserName, TDUserPass);
            if (res > 0)
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "【 " + TDUserName + " 】 registered successfully" : "【 " + TDUserName + " 】 注册成功";
                MessageBox.Show(strShow);
                textUserName.Clear();
                textUserCoPass.Clear();
                textUserPass.Clear();
                this.Close();
            }
            else
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "【 " + TDUserName + " 】 registration failed " : "【 " + TDUserName + " 】 注册失败";
                MessageBox.Show("【 " + TDUserName + " 】 注册失败");
            }
        }

        private void UserRegisterWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            main.Activate();
        }

        private void UserRegisterWnd_Load(object sender, EventArgs e)
        {
            main = (BAR)this.Owner;
        }
    }
}
