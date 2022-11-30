
namespace BAR.Windows
{
    partial class SelectProgFile
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnConfirm = new CCWin.SkinControl.SkinButton();
            this.lvProgFile = new CCWin.SkinControl.SkinListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnConfirm.BorderColor = System.Drawing.Color.Silver;
            this.btnConfirm.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnConfirm.DownBack = null;
            this.btnConfirm.Font = new System.Drawing.Font("楷体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnConfirm.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnConfirm.Location = new System.Drawing.Point(243, 372);
            this.btnConfirm.MouseBack = null;
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.NormlBack = null;
            this.btnConfirm.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnConfirm.Size = new System.Drawing.Size(127, 43);
            this.btnConfirm.TabIndex = 0;
            this.btnConfirm.Text = "确 定";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // lvProgFile
            // 
            this.lvProgFile.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lvProgFile.AutoArrange = false;
            this.lvProgFile.CheckBoxes = true;
            this.lvProgFile.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader1,
            this.columnHeader2});
            this.lvProgFile.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lvProgFile.FullRowSelect = true;
            this.lvProgFile.GridLines = true;
            this.lvProgFile.Location = new System.Drawing.Point(12, 12);
            this.lvProgFile.Name = "lvProgFile";
            this.lvProgFile.SelectedColor = System.Drawing.Color.Lime;
            this.lvProgFile.Size = new System.Drawing.Size(591, 354);
            this.lvProgFile.TabIndex = 200;
            this.lvProgFile.TabStop = false;
            this.lvProgFile.UseCompatibleStateImageBehavior = false;
            this.lvProgFile.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "";
            this.columnHeader3.Width = 0;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "序号";
            this.columnHeader1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader1.Width = 88;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "烧录文档";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 499;
            // 
            // SelectProgFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(612, 422);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.lvProgFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SelectProgFile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择烧录文档";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SelectProgFile_FormClosing);
            this.Load += new System.EventHandler(this.SelectProgFile_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinButton btnConfirm;
        public CCWin.SkinControl.SkinListView lvProgFile;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}