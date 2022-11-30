using BAR.CommonLib_v1._0;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TensionDetectionDLL;




namespace BAR.Windows
{
    public partial class UserLoginWnd : Form
    {
        public UserLoginWnd()
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();
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
           TDMethod.TDLogin(TDUserName, TDUserPass);
           if (!StaticInfo.CheckUserName())
           {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Login failed. Please check the user name and password" : "登录失败，请检查用户名和密码";
                textLoginPass.Text = "";
               MessageBox.Show(this, strShow, "Information", MessageBoxButtons.OK,MessageBoxIcon.Error);
               return;
           }
           else
           {
               this.Close();
           }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            int TDRead = 1;
            TDMethod.TDReadDate(TDRead);
            //循环接收数据库中的用户名
            string[] cobName = new string[StaticInfo.nDate];
            for (int i = 0; i < StaticInfo.nDate; i++)
            {
                cobName[i] = MultiLanguage.GetString(StaticInfo.TDDate[i]);
            }
            //添加下拉控件项，并设置默认选择项
            this.cobLoginName.Items.AddRange(cobName);
            this.cobLoginName.SelectedIndex = this.cobLoginName.Items.Count > 0 ? 2 : -1;

        }
        private void LoginForm_Paint(object sender, PaintEventArgs e)
        {
            Type(this, 20, 0.1);
        }
        private void Type(Control sender, int p_1, double p_2)
        {
            GraphicsPath oPath = new GraphicsPath();
            oPath.AddClosedCurve(new Point[] {
                new Point(0, sender.Height / p_1),
                new Point(sender.Width / p_1, 0),
                new Point(sender.Width - sender.Width / p_1, 0),
                new Point(sender.Width, sender.Height / p_1),
                new Point(sender.Width, sender.Height - sender.Height / p_1),
                new Point(sender.Width - sender.Width / p_1, sender.Height),
                new Point(sender.Width / p_1, sender.Height),
                new Point(0, sender.Height - sender.Height / p_1) }, (float)p_2);
            sender.Region = new Region(oPath);
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void UserLoginWnd_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }

        private void UserLoginWnd_KeyDown(object sender, KeyEventArgs e)
        {
            int i = 0;
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }

        private void cobLoginName_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //textLoginPass.Focus();
        }
    }
}
