namespace BAR.Windows
{
    partial class ProManageWnd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProManageWnd));
            this.productListView = new System.Windows.Forms.ListView();
            this.selectProBtn = new CCWin.SkinControl.SkinButton();
            this.addProBtn = new CCWin.SkinControl.SkinButton();
            this.delProBtn = new CCWin.SkinControl.SkinButton();
            this.label16 = new System.Windows.Forms.Label();
            this.inputPnl = new System.Windows.Forms.Panel();
            this.cancelBtn = new CCWin.SkinControl.SkinButton();
            this.inputOKBtn = new CCWin.SkinControl.SkinButton();
            this.prodNameTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.modifyProBtn = new CCWin.SkinControl.SkinButton();
            this.inputPnl.SuspendLayout();
            this.SuspendLayout();
            // 
            // productListView
            // 
            resources.ApplyResources(this.productListView, "productListView");
            this.productListView.ForeColor = System.Drawing.SystemColors.WindowText;
            this.productListView.FullRowSelect = true;
            this.productListView.GridLines = true;
            this.productListView.HideSelection = false;
            this.productListView.Name = "productListView";
            this.productListView.UseCompatibleStateImageBehavior = false;
            this.productListView.View = System.Windows.Forms.View.Details;
            // 
            // selectProBtn
            // 
            resources.ApplyResources(this.selectProBtn, "selectProBtn");
            this.selectProBtn.BackColor = System.Drawing.Color.Transparent;
            this.selectProBtn.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(235)))));
            this.selectProBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0)))));
            this.selectProBtn.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.selectProBtn.DownBack = null;
            this.selectProBtn.DownBaseColor = System.Drawing.Color.Gray;
            this.selectProBtn.ForeColor = System.Drawing.Color.Black;
            this.selectProBtn.IsDrawGlass = false;
            this.selectProBtn.MouseBack = null;
            this.selectProBtn.Name = "selectProBtn";
            this.selectProBtn.NormlBack = null;
            this.selectProBtn.UseVisualStyleBackColor = false;
            this.selectProBtn.Click += new System.EventHandler(this.selectProBtn_Click);
            // 
            // addProBtn
            // 
            resources.ApplyResources(this.addProBtn, "addProBtn");
            this.addProBtn.BackColor = System.Drawing.Color.Transparent;
            this.addProBtn.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(235)))));
            this.addProBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0)))));
            this.addProBtn.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.addProBtn.DownBack = null;
            this.addProBtn.DownBaseColor = System.Drawing.Color.Gray;
            this.addProBtn.ForeColor = System.Drawing.Color.Black;
            this.addProBtn.IsDrawGlass = false;
            this.addProBtn.MouseBack = null;
            this.addProBtn.Name = "addProBtn";
            this.addProBtn.NormlBack = null;
            this.addProBtn.UseVisualStyleBackColor = false;
            this.addProBtn.Click += new System.EventHandler(this.addProBtn_Click);
            // 
            // delProBtn
            // 
            resources.ApplyResources(this.delProBtn, "delProBtn");
            this.delProBtn.BackColor = System.Drawing.Color.Transparent;
            this.delProBtn.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(235)))));
            this.delProBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0)))));
            this.delProBtn.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.delProBtn.DownBack = null;
            this.delProBtn.DownBaseColor = System.Drawing.Color.Gray;
            this.delProBtn.ForeColor = System.Drawing.Color.Black;
            this.delProBtn.IsDrawGlass = false;
            this.delProBtn.MouseBack = null;
            this.delProBtn.Name = "delProBtn";
            this.delProBtn.NormlBack = null;
            this.delProBtn.UseVisualStyleBackColor = false;
            this.delProBtn.Click += new System.EventHandler(this.delProBtn_Click);
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label16.ForeColor = System.Drawing.Color.Black;
            this.label16.Name = "label16";
            // 
            // inputPnl
            // 
            resources.ApplyResources(this.inputPnl, "inputPnl");
            this.inputPnl.Controls.Add(this.cancelBtn);
            this.inputPnl.Controls.Add(this.inputOKBtn);
            this.inputPnl.Controls.Add(this.prodNameTxt);
            this.inputPnl.Controls.Add(this.label1);
            this.inputPnl.Name = "inputPnl";
            // 
            // cancelBtn
            // 
            resources.ApplyResources(this.cancelBtn, "cancelBtn");
            this.cancelBtn.BackColor = System.Drawing.Color.Transparent;
            this.cancelBtn.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(235)))));
            this.cancelBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0)))));
            this.cancelBtn.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.cancelBtn.DownBack = null;
            this.cancelBtn.DownBaseColor = System.Drawing.Color.Gray;
            this.cancelBtn.ForeColor = System.Drawing.Color.Black;
            this.cancelBtn.IsDrawGlass = false;
            this.cancelBtn.MouseBack = null;
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.NormlBack = null;
            this.cancelBtn.UseVisualStyleBackColor = false;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // inputOKBtn
            // 
            resources.ApplyResources(this.inputOKBtn, "inputOKBtn");
            this.inputOKBtn.BackColor = System.Drawing.Color.Transparent;
            this.inputOKBtn.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(235)))));
            this.inputOKBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0)))));
            this.inputOKBtn.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.inputOKBtn.DownBack = null;
            this.inputOKBtn.DownBaseColor = System.Drawing.Color.Gray;
            this.inputOKBtn.ForeColor = System.Drawing.Color.Black;
            this.inputOKBtn.IsDrawGlass = false;
            this.inputOKBtn.MouseBack = null;
            this.inputOKBtn.Name = "inputOKBtn";
            this.inputOKBtn.NormlBack = null;
            this.inputOKBtn.UseVisualStyleBackColor = false;
            this.inputOKBtn.Click += new System.EventHandler(this.inputOKBtn_Click);
            // 
            // prodNameTxt
            // 
            resources.ApplyResources(this.prodNameTxt, "prodNameTxt");
            this.prodNameTxt.Name = "prodNameTxt";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // modifyProBtn
            // 
            resources.ApplyResources(this.modifyProBtn, "modifyProBtn");
            this.modifyProBtn.BackColor = System.Drawing.Color.Transparent;
            this.modifyProBtn.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(235)))));
            this.modifyProBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0)))));
            this.modifyProBtn.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.modifyProBtn.DownBack = null;
            this.modifyProBtn.DownBaseColor = System.Drawing.Color.Gray;
            this.modifyProBtn.ForeColor = System.Drawing.Color.Black;
            this.modifyProBtn.IsDrawGlass = false;
            this.modifyProBtn.MouseBack = null;
            this.modifyProBtn.Name = "modifyProBtn";
            this.modifyProBtn.NormlBack = null;
            this.modifyProBtn.UseVisualStyleBackColor = false;
            this.modifyProBtn.Click += new System.EventHandler(this.modifyProBtn_Click);
            // 
            // ProManageWnd
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.inputPnl);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.delProBtn);
            this.Controls.Add(this.addProBtn);
            this.Controls.Add(this.selectProBtn);
            this.Controls.Add(this.productListView);
            this.Controls.Add(this.modifyProBtn);
            this.Name = "ProManageWnd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProManageWnd_FormClosing);
            this.Load += new System.EventHandler(this.ProManageWnd_Load);
            this.inputPnl.ResumeLayout(false);
            this.inputPnl.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView productListView;
        private CCWin.SkinControl.SkinButton selectProBtn;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Panel inputPnl;
        private System.Windows.Forms.TextBox prodNameTxt;
        private System.Windows.Forms.Label label1;
        private CCWin.SkinControl.SkinButton inputOKBtn;
        private CCWin.SkinControl.SkinButton cancelBtn;
        public CCWin.SkinControl.SkinButton addProBtn;
        public CCWin.SkinControl.SkinButton delProBtn;
        public CCWin.SkinControl.SkinButton modifyProBtn;
    }
}