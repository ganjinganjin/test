using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BAR.Windows;
using BAR.Commonlib;
using BAR.ControlPanels;
using System.Threading;

namespace BAR.Windows
{
    public partial class MarkManageWnd : Form
    {
        Config g_config = Config.GetInstance();
        Act g_act = Act.GetInstance();
        //ConfigInitWnd _ConInitWnd;
        bool addBtnClick;
        public MarkManageWnd()
        {
            InitializeComponent();
        }

        private void ProManageWnd_Load(object sender, EventArgs e)
        {
            __InitWnd();
        }

        public void __InitWnd()
        {
            //_ConInitWnd = (ConfigInitWnd)this.Owner;
            this.InitList();
            this.LoadProductFile();
        }

        private void InitList()
        {
            productListView.Clear();

            ColumnHeader ch = new ColumnHeader();
            ch.Text = "料盘型号";
            ch.Width = 365;
            ch.TextAlign = HorizontalAlignment.Left;
            this.productListView.Columns.Add(ch);

        }

        private void LoadProductFile()
        {
            productListView.Items.Clear();

            DirectoryInfo TheFolder = new DirectoryInfo(g_config.StrConfigDir + "\\MarkDate\\");

            this.productListView.BeginUpdate();
            //遍历文件夹
            foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
            {
                productListView.Items.Add(NextFolder.Name);
            }
            this.productListView.EndUpdate();
        }

        private void selectProBtn_Click(object sender, EventArgs e)
        {
            if(this.productListView.SelectedItems.Count <= 0)
            {
                MessageBox.Show("请选择需要使用的料盘型号！！！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            String prodName = this.productListView.SelectedItems[0].Text;
            g_config.LoadMarkProd(prodName);
            TrayD.TIC_EndColN[0] = TrayD.ColC;
            TrayD.TIC_EndRowN[0] = TrayD.RowC;
            TrayD.TIC_EndColN[1] = g_config.TrayRotateDir[1] != 0 ? TrayD.RowC : TrayD.ColC;
            TrayD.TIC_EndRowN[1] = g_config.TrayRotateDir[1] != 0 ? TrayD.ColC : TrayD.RowC;
            UserEvent evt = new UserEvent();
            evt.ProductChange_Click();
            TrayState.TrayStateUpdate(true);
            MessageBox.Show("料盘数据加载成功！！！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void addProBtn_Click(object sender, EventArgs e)
        {
            label1.Text = "请输入新建料盘名称,请勿与已有产品重名";
            addBtnClick = true;
            this.inputPnl.Visible = true;
        }

        private void delProBtn_Click(object sender, EventArgs e)
        {
            if (this.productListView.SelectedItems.Count <= 0)
            {
                MessageBox.Show("请先选择需要删除的料盘型号！！！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (g_config.StrCurProduct == productListView.SelectedItems[0].Text)
            {
                MessageBox.Show("当前料盘型号正在使用，无法删除！！！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("将要删除这个料盘型号", "是否继续？", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                String delProdPath = g_config.StrConfigDir + "\\MarkDate\\" + this.productListView.SelectedItems[0].Text;
                Act.DelectDir(delProdPath);
            }
            this.LoadProductFile();
        }

        private void inputOKBtn_Click(object sender, EventArgs e)
        {
            String newProdName = prodNameTxt.Text.Trim();
            double[] temp1 = new double[4];
            int[] temp2 = new int[2];
            bool IsUse = false;
            String oldProdPath, newProdPath, newProdIni;

            temp1[0] = Convert.ToDouble(NudOffset_X.Value);
            temp1[1] = Convert.ToDouble(NudOffset_Y.Value);
            temp2[0] = Convert.ToInt32(NudCol.Value);
            temp2[1] = Convert.ToInt32(NudRol.Value);
            temp1[2] = Convert.ToDouble(NudColdis.Value);
            temp1[3] = Convert.ToDouble(NudRoldis.Value);
            

            newProdPath = g_config.StrConfigDir + "\\MarkDate\\" + newProdName;
            newProdIni = newProdPath + "\\set.prc";
            if (newProdName == "")
            {
                MessageBox.Show("料盘型号不能为空！！！", "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Directory.Exists(newProdPath) && addBtnClick)
            {
                MessageBox.Show("该料盘型号已存在！！！", "错误：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (addBtnClick)//判断是点击的添加按钮还是修改按钮
            {
                if (!Directory.Exists(newProdPath))//若文件夹不存在则新建文件夹  
                {
                    Directory.CreateDirectory(newProdPath); //新建文件夹  
                }

                if (!File.Exists(newProdIni))
                {
                    FileStream file = File.Create(newProdIni);
                    file.Close();
                }

                g_config.WriteMarkValue(temp1, temp2, newProdIni);
                MessageBox.Show("创建料盘参数成功！！！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                if (this.productListView.SelectedItems.Count <= 0)
                {
                    MessageBox.Show("请先选择需要修改的料盘型号！！！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else if (g_config.StrCurMark == productListView.SelectedItems[0].Text)
                {
                    IsUse = true;
                }
                oldProdPath = g_config.StrConfigDir + "\\MarkDate\\" + productListView.SelectedItems[0].Text;
                if (oldProdPath != newProdPath)
                {
                    Act.ModifyDir(oldProdPath, newProdPath);
                }
                g_config.WriteMarkValue(temp1, temp2, newProdIni);
                if (IsUse)
                {
                    g_config.LoadMarkProd(newProdName);
                    UserEvent evt = new UserEvent();
                    evt.ProductChange_Click();
                    TrayState.TrayStateUpdate(true);
                }
                
            }
            this.LoadProductFile();
            this.inputPnl.Visible = false;
        }

        private bool CopyOldLabFilesToNewLab(string sourcePath, string savePath)
        {
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            #region //拷贝文件夹到savePath下
            try
            {
                string[] labDirs = Directory.GetDirectories(sourcePath);//目录
                string[] labFiles = Directory.GetFiles(sourcePath);//文件
                if (labFiles.Length > 0)
                {
                    for (int i = 0; i < labFiles.Length; i++)
                    {   
                        File.Copy(sourcePath + "\\" + Path.GetFileName(labFiles[i]), savePath + "\\" + Path.GetFileName(labFiles[i]), true);
                    }
                }
                if (labDirs.Length > 0)
                {
                    for (int j = 0; j < labDirs.Length; j++)
                    {
                        Directory.GetDirectories(sourcePath + "\\" + Path.GetFileName(labDirs[j]));

                        //递归调用
                        CopyOldLabFilesToNewLab(sourcePath + "\\" + Path.GetFileName(labDirs[j]), savePath + "\\" + Path.GetFileName(labDirs[j]));
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            #endregion
            return true;
        }
        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.prodNameTxt.Text = "";
            this.inputPnl.Visible = false;
        }

        private void ProManageWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void modifyProBtn_Click(object sender, EventArgs e)
        {
            if (this.productListView.SelectedItems.Count <= 0)
            {
                MessageBox.Show("请先选择需要更改的料盘型号！！！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            label1.Text = "请更改料盘Mark参数";
            prodNameTxt.Text = productListView.SelectedItems[0].Text;
            string SelProdPath = g_config.StrConfigDir + "\\MarkDate\\" + prodNameTxt.Text;
            string SelProdIni = SelProdPath + "\\set.prc";
            double[] temp1 = new double[4];
            int[] temp2 = new int[2];
            g_config.ReadMarkValue(ref temp1,ref temp2, SelProdIni);
            NudOffset_X.Value = Convert.ToDecimal(temp1[0]);
            NudOffset_Y.Value = Convert.ToDecimal(temp1[1]);
            NudCol.Value = Convert.ToDecimal(temp2[0]);
            NudRol.Value = Convert.ToDecimal(temp2[1]);
            NudColdis.Value = Convert.ToDecimal(temp1[2]);
            NudRoldis.Value = Convert.ToDecimal(temp1[3]);
            addBtnClick = false;
            inputPnl.Visible = true;
        }
    }
}
