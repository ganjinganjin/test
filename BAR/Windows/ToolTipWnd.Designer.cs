namespace BAR.Windows
{
    partial class ToolTipWnd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ToolTipWnd));
            this.btnConfirm = new CCWin.SkinControl.SkinButton();
            this.tabBtn2 = new CCWin.SkinControl.SkinButton();
            this.tabBtn1 = new CCWin.SkinControl.SkinButton();
            this.btnJump = new CCWin.SkinControl.SkinButton();
            this.alarmList = new System.Windows.Forms.ListView();
            this.lvToolTip = new CCWin.SkinControl.SkinListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnConfirm.BorderColor = System.Drawing.Color.Silver;
            this.btnConfirm.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnConfirm.DownBack = null;
            resources.ApplyResources(this.btnConfirm, "btnConfirm");
            this.btnConfirm.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnConfirm.MouseBack = null;
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.NormlBack = null;
            this.btnConfirm.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // tabBtn2
            // 
            this.tabBtn2.BackColor = System.Drawing.Color.Transparent;
            this.tabBtn2.BaseColor = System.Drawing.Color.Gray;
            this.tabBtn2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tabBtn2.BorderInflate = new System.Drawing.Size(0, 0);
            this.tabBtn2.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.tabBtn2.DownBack = null;
            this.tabBtn2.DownBaseColor = System.Drawing.Color.DimGray;
            this.tabBtn2.FadeGlow = false;
            resources.ApplyResources(this.tabBtn2, "tabBtn2");
            this.tabBtn2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.tabBtn2.IsDrawGlass = false;
            this.tabBtn2.MouseBack = null;
            this.tabBtn2.MouseBaseColor = System.Drawing.Color.DimGray;
            this.tabBtn2.Name = "tabBtn2";
            this.tabBtn2.NormlBack = null;
            this.tabBtn2.UseVisualStyleBackColor = false;
            this.tabBtn2.Click += new System.EventHandler(this.tabBtn_MouseUp);
            // 
            // tabBtn1
            // 
            this.tabBtn1.BackColor = System.Drawing.Color.Transparent;
            this.tabBtn1.BaseColor = System.Drawing.Color.Gray;
            this.tabBtn1.BorderColor = System.Drawing.Color.Black;
            this.tabBtn1.BorderInflate = new System.Drawing.Size(0, 0);
            this.tabBtn1.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.tabBtn1.DownBack = null;
            this.tabBtn1.DownBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.tabBtn1.FadeGlow = false;
            resources.ApplyResources(this.tabBtn1, "tabBtn1");
            this.tabBtn1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.tabBtn1.IsDrawGlass = false;
            this.tabBtn1.MouseBack = null;
            this.tabBtn1.MouseBaseColor = System.Drawing.Color.DimGray;
            this.tabBtn1.Name = "tabBtn1";
            this.tabBtn1.NormlBack = null;
            this.tabBtn1.UseVisualStyleBackColor = false;
            this.tabBtn1.Click += new System.EventHandler(this.tabBtn_MouseUp);
            // 
            // btnJump
            // 
            this.btnJump.BackColor = System.Drawing.Color.Transparent;
            this.btnJump.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnJump.BorderColor = System.Drawing.Color.Silver;
            this.btnJump.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnJump.DownBack = null;
            resources.ApplyResources(this.btnJump, "btnJump");
            this.btnJump.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnJump.MouseBack = null;
            this.btnJump.Name = "btnJump";
            this.btnJump.NormlBack = null;
            this.btnJump.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnJump.UseVisualStyleBackColor = false;
            this.btnJump.Click += new System.EventHandler(this.btnJump_Click);
            // 
            // alarmList
            // 
            resources.ApplyResources(this.alarmList, "alarmList");
            this.alarmList.FullRowSelect = true;
            this.alarmList.GridLines = true;
            this.alarmList.HideSelection = false;
            this.alarmList.Name = "alarmList";
            this.alarmList.UseCompatibleStateImageBehavior = false;
            this.alarmList.View = System.Windows.Forms.View.Details;
            // 
            // lvToolTip
            // 
            this.lvToolTip.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader1,
            this.columnHeader2});
            resources.ApplyResources(this.lvToolTip, "lvToolTip");
            this.lvToolTip.GridLines = true;
            this.lvToolTip.HideSelection = false;
            this.lvToolTip.Name = "lvToolTip";
            this.lvToolTip.OwnerDraw = true;
            this.lvToolTip.UseCompatibleStateImageBehavior = false;
            this.lvToolTip.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            resources.ApplyResources(this.columnHeader3, "columnHeader3");
            // 
            // columnHeader1
            // 
            resources.ApplyResources(this.columnHeader1, "columnHeader1");
            // 
            // columnHeader2
            // 
            resources.ApplyResources(this.columnHeader2, "columnHeader2");
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lvToolTip);
            this.panel1.Controls.Add(this.alarmList);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // ToolTipWnd
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnJump);
            this.Controls.Add(this.tabBtn1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabBtn2);
            this.Controls.Add(this.btnConfirm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ToolTipWnd";
            this.ShowIcon = false;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ToolTipWnd_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ToolTipWnd_FormClosed);
            this.Load += new System.EventHandler(this.ToolTipWnd_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private CCWin.SkinControl.SkinButton btnConfirm;
        private CCWin.SkinControl.SkinButton tabBtn2;
        private CCWin.SkinControl.SkinButton tabBtn1;
        private CCWin.SkinControl.SkinButton btnJump;
        private System.Windows.Forms.ListView alarmList;
        public CCWin.SkinControl.SkinListView lvToolTip;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Panel panel1;
    }
}