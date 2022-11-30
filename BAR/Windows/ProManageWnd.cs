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
using static System.Data.Entity.Infrastructure.Design.Executor;
using BAR.CommonLib_v1._0;

namespace BAR.Windows
{
    public partial class ProManageWnd : Form
    {
        Config g_config = Config.GetInstance();
        Act g_act = Act.GetInstance();
        BAR main = null;
        bool addBtnClick;
        public ProManageWnd()
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();
            UserEvent.productChange += new UserEvent.ProductChange(selectPro);
        }

        private void ProManageWnd_Load(object sender, EventArgs e)
        {
            __InitWnd();
        }

        public void __InitWnd()
        {
            main = (BAR)this.Owner;
            this.InitList();
            this.LoadProductFile();
        }

        private void InitList()
        {
            productListView.Clear();

            ColumnHeader ch = new ColumnHeader();
            ch.Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English? "Name" : "产品名称";
            ch.Width = 365;
            ch.TextAlign = HorizontalAlignment.Left;
            this.productListView.Columns.Add(ch);

        }

        private void LoadProductFile()
        {
            productListView.Items.Clear();

            DirectoryInfo TheFolder = new DirectoryInfo(g_config.StrAppDir + "Product\\");

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
            string strShow;
            if(this.productListView.SelectedItems.Count <= 0)
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Select the product name you want to use!" : "请选择需要使用的产品名！！！";
                MessageBox.Show(strShow, "Information：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            String prodName = this.productListView.SelectedItems[0].Text;
            g_config.LoadConfigProd(prodName);
            selectPro();
            UserEvent evt = new UserEvent();
            evt.ProductChange_Click();
            evt.RenovateBtn_Click();
            TrayState.TrayStateUpdate(true);

            strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "The product is loaded successfully." : "产品加载成功！！！";
            MessageBox.Show(strShow, "Information：", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void selectPro()
        {
            TrayD.TIC_EndColN[0] = TrayD.ColC;
            TrayD.TIC_EndRowN[0] = TrayD.RowC;
            TrayD.TIC_EndColN[1] = g_config.TrayRotateDir[1] != 0 ? TrayD.RowC : TrayD.ColC;
            TrayD.TIC_EndRowN[1] = g_config.TrayRotateDir[1] != 0 ? TrayD.ColC : TrayD.RowC;
            main.productLab.Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Product:" + g_config.StrCurProduct : "当前产品:" + g_config.StrCurProduct;
        }

        private void addProBtn_Click(object sender, EventArgs e)
        {
            string strShow;
            if (this.productListView.SelectedItems.Count <= 0)
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Select the name of the product you want to copy!" : "请先选择需要复制的产品名！！！";
                MessageBox.Show(strShow, "Information：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            label1.Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Enter the name of the new product. Do not use the same name as the existing product" : "请输入新建产品名称,请勿与已有产品重名";
            addBtnClick = true;
            this.inputPnl.Visible = true;
        }

        private void delProBtn_Click(object sender, EventArgs e)
        {
            string strShow;
            if (this.productListView.SelectedItems.Count <= 0)
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Select the name of the product you want to delete!" : "请先选择需要删除的产品名！！！";
                MessageBox.Show(strShow, "Information：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (g_config.StrCurProduct == productListView.SelectedItems[0].Text)
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "The current product is in use and cannot be deleted!" : "当前产品正在使用，无法删除！！！";
                MessageBox.Show(strShow, "Information：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "This product file will be deleted!" : "将要删除这个产品文件";
            if (MessageBox.Show(strShow, "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                String delProdPath = g_config.StrAppDir + "Product\\" + this.productListView.SelectedItems[0].Text;
                Act.DelectDir(delProdPath);
            }
            this.LoadProductFile();
        }

        private void inputOKBtn_Click(object sender, EventArgs e)
        {
            String newProdName = prodNameTxt.Text.Trim();
            String oldProdPath, newProdPath;
            string strShow;
            oldProdPath = g_config.StrAppDir + "Product\\" + productListView.SelectedItems[0].Text;
            newProdPath = g_config.StrAppDir + "Product\\" + newProdName;
            if(newProdName == "")
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "The product name cannot be empty!" : "产品名称不能为空！！！";
                MessageBox.Show(strShow, "Information：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Directory.Exists(newProdPath))
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "The product name already exists!" : "该产品名称已存在！！！";
                MessageBox.Show(strShow, "Information：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (addBtnClick)//判断是点击的添加按钮还是修改按钮
            {
                Directory.CreateDirectory(newProdPath);
                if (CopyOldLabFilesToNewLab(oldProdPath, newProdPath))
                {
                    strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "The new product data file is created successfully!" : "创建新产品数据文件成功！！！";
                    MessageBox.Show(strShow, "Information：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Failed to create a new product data file!" : "创建新产品数据文件失败！！！";
                    MessageBox.Show(strShow, "Information：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                Act.ModifyDir(oldProdPath, newProdPath);
                if (g_config.StrCurProduct == productListView.SelectedItems[0].Text)
                {
                    g_config.LoadConfigProd(newProdName);
                    main.productLab.Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Product:" + g_config.StrCurProduct : "当前产品:" + g_config.StrCurProduct;
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
            main.Activate();
        }

        private void modifyProBtn_Click(object sender, EventArgs e)
        {
            string strShow;
            if (this.productListView.SelectedItems.Count <= 0)
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Select the product name you want to change first!" : "请先选择需要更改的产品名！！！";
                MessageBox.Show("请先选择需要更改的产品名！！！", "Information：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            label1.Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Please change the product name, do not have the same name as the existing product" : "请更改产品名称,请勿与已有产品重名";
            prodNameTxt.Text = productListView.SelectedItems[0].Text;
            addBtnClick = false;
            inputPnl.Visible = true;
        }
    }
}
