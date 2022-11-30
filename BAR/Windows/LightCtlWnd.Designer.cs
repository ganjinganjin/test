namespace BAR.Windows
{
    partial class LightCtlWnd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LightCtlWnd));
            this.upLightBar = new CCWin.SkinControl.SkinTrackBar();
            this.label137 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.downLightBar = new CCWin.SkinControl.SkinTrackBar();
            this.saveBtn = new CCWin.SkinControl.SkinButton();
            this.upTxtBox = new System.Windows.Forms.NumericUpDown();
            this.downTxtBox = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.upLightBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.downLightBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upTxtBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.downTxtBox)).BeginInit();
            this.SuspendLayout();
            // 
            // upLightBar
            // 
            resources.ApplyResources(this.upLightBar, "upLightBar");
            this.upLightBar.BackColor = System.Drawing.Color.Transparent;
            this.upLightBar.Bar = null;
            this.upLightBar.BarStyle = CCWin.SkinControl.HSLTrackBarStyle.Saturation;
            this.upLightBar.BaseColor = System.Drawing.Color.Brown;
            this.upLightBar.CausesValidation = false;
            this.upLightBar.Name = "upLightBar";
            this.upLightBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.upLightBar.Track = null;
            this.upLightBar.Value = 100;
            this.upLightBar.Scroll += new System.EventHandler(this.upLightBar_Scroll);
            // 
            // label137
            // 
            resources.ApplyResources(this.label137, "label137");
            this.label137.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label137.ForeColor = System.Drawing.Color.Black;
            this.label137.Name = "label137";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Name = "label1";
            // 
            // downLightBar
            // 
            resources.ApplyResources(this.downLightBar, "downLightBar");
            this.downLightBar.BackColor = System.Drawing.Color.Transparent;
            this.downLightBar.Bar = null;
            this.downLightBar.BarStyle = CCWin.SkinControl.HSLTrackBarStyle.Saturation;
            this.downLightBar.BaseColor = System.Drawing.Color.Brown;
            this.downLightBar.CausesValidation = false;
            this.downLightBar.Name = "downLightBar";
            this.downLightBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.downLightBar.Track = null;
            this.downLightBar.Value = 100;
            this.downLightBar.Scroll += new System.EventHandler(this.downLightBar_Scroll);
            // 
            // saveBtn
            // 
            resources.ApplyResources(this.saveBtn, "saveBtn");
            this.saveBtn.BackColor = System.Drawing.Color.Transparent;
            this.saveBtn.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0)))));
            this.saveBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0)))));
            this.saveBtn.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.saveBtn.DownBack = null;
            this.saveBtn.ForeColor = System.Drawing.Color.White;
            this.saveBtn.IsDrawGlass = false;
            this.saveBtn.MouseBack = null;
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.NormlBack = null;
            this.saveBtn.UseVisualStyleBackColor = false;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // upTxtBox
            // 
            resources.ApplyResources(this.upTxtBox, "upTxtBox");
            this.upTxtBox.ForeColor = System.Drawing.Color.Black;
            this.upTxtBox.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.upTxtBox.Name = "upTxtBox";
            this.upTxtBox.ValueChanged += new System.EventHandler(this.upTxtBox_ValueChanged);
            // 
            // downTxtBox
            // 
            resources.ApplyResources(this.downTxtBox, "downTxtBox");
            this.downTxtBox.ForeColor = System.Drawing.Color.Black;
            this.downTxtBox.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.downTxtBox.Name = "downTxtBox";
            this.downTxtBox.ValueChanged += new System.EventHandler(this.downTxtBox_ValueChanged);
            // 
            // LightCtlWnd
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this.downTxtBox);
            this.Controls.Add(this.upTxtBox);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.downLightBar);
            this.Controls.Add(this.label137);
            this.Controls.Add(this.upLightBar);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LightCtlWnd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.LightCtlWnd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.upLightBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.downLightBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upTxtBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.downTxtBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinTrackBar upLightBar;
        private System.Windows.Forms.Label label137;
        private System.Windows.Forms.Label label1;
        private CCWin.SkinControl.SkinTrackBar downLightBar;
        private CCWin.SkinControl.SkinButton saveBtn;
        public System.Windows.Forms.NumericUpDown upTxtBox;
        public System.Windows.Forms.NumericUpDown downTxtBox;
    }
}