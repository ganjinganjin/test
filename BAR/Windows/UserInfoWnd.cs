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
    public partial class UserInfoWnd : Form
    {
        string strSel;
        BAR main = null;
        public UserInfoWnd()
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string strShow;

            if (strSel == MultiLanguage.GetString("管理员") || strSel == MultiLanguage.GetString("供应商"))
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "The current user has the highest rights and cannot be deleted" : "当前用户拥有最高权限，无法删除";
                MessageBox.Show(this, strShow, "Information：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (listView1.SelectedItems.Count == 0)
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Select a user name and delete it" : "请选中一个用户名再进行删除";
                MessageBox.Show(this, strShow, "Information：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (StaticInfo.TDLevel != 2)
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Sermission denied" : "权限不足";
                MessageBox.Show(this, strShow, "Information：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Whether to delete a user: " + strSel : "是否删除用户：" + strSel;
            if (MessageBox.Show(strShow, "Information：", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }
            //数据库删除数据
            int res = TDMethod.TDDelete(MultiLanguage.GetString(strSel));
            if (res > 0)
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Deleted successfully" : "删除成功";
                MessageBox.Show(strShow);
                //删除列表中的选中项
                if(listView1.SelectedItems.Count>0)
                {
                    ListViewItem lvi = listView1.SelectedItems[0];
                    listView1.Items.RemoveAt(lvi.Index);
                }
            }
            else
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Delete Failed" : "删除失败";
                MessageBox.Show(strShow);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedIndices != null && listView1.SelectedIndices.Count > 0)
            {
                ListView.SelectedIndexCollection c = listView1.SelectedIndices;
                strSel = listView1.Items[c[0]].Text;
            }
        }

        private void UserInfoWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            main.Activate();
        }

        private void UserInfoWnd_Activated(object sender, EventArgs e)
        {
            int TDRead = 1;
            TDMethod.TDReadDate(TDRead);
            listView1.Items.Clear();
            //循环接收数据库中的用户名
            for (int i = 0; i < StaticInfo.nDate; i++)
            {
                ListViewItem lvi = new ListViewItem
                {
                    Text = MultiLanguage.GetString(StaticInfo.TDDate[i])
                };
                listView1.Items.Add(lvi);
            }
        }

        private void UserInfoWnd_Load(object sender, EventArgs e)
        {
            main = (BAR)this.Owner;
        }
    }
}
