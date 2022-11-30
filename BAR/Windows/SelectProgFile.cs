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
    public partial class SelectProgFile : Form
    {
        public SelectProgFile()
        {
            InitializeComponent();
            this.ControlBox = false;
        }

        private void SelectProgFile_Load(object sender, EventArgs e)
        {
            InitUI();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (this.lvProgFile.SelectedItems.Count <= 0)
            {
                MessageBox.Show("请先选择需要烧录的文档！！！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int index= lvProgFile.SelectedItems[0].Index;
            Mes.ProgFileName = Mes.lstFiles[index].FullName;
            this.Close();
        }

        private void SelectProgFile_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        public void InitUI()
        {
            lvProgFile.Items.Clear();
            lvProgFile.BeginUpdate();
            for (int i = 0; i < Mes.lstFiles.Count; i++)
            {
                string ID = (lvProgFile.Items.Count + 1).ToString();
                ListViewItem lvItem = new ListViewItem();//新建行
                lvItem.SubItems.Add(ID);//写入序号
                lvItem.SubItems.Add(Mes.lstFiles[i].Name);//写入告警内容
                lvProgFile.Items.Add(lvItem);//更新控件显示
            }
            lvProgFile.EndUpdate();
        }
    }
}
