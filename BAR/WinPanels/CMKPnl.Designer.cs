namespace BAR.ControlPanels
{
    partial class CMKPnl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CMKPnl));
            this.btnStop = new CCWin.SkinControl.SkinButton();
            this.btnStart = new CCWin.SkinControl.SkinButton();
            this.skinGroupBox6 = new CCWin.SkinControl.SkinGroupBox();
            this.NudPos_Z = new System.Windows.Forms.NumericUpDown();
            this.skinLabel3 = new CCWin.SkinControl.SkinLabel();
            this.NudPos_Y = new System.Windows.Forms.NumericUpDown();
            this.BtnSave = new CCWin.SkinControl.SkinButton();
            this.skinLabel2 = new CCWin.SkinControl.SkinLabel();
            this.NudCount = new System.Windows.Forms.NumericUpDown();
            this.skinLabel1 = new CCWin.SkinControl.SkinLabel();
            this.NudPos_X = new System.Windows.Forms.NumericUpDown();
            this.skinLabel33 = new CCWin.SkinControl.SkinLabel();
            this.CboAxisType = new CCWin.SkinControl.SkinComboBox();
            this.skinLabel19 = new CCWin.SkinControl.SkinLabel();
            this.skinGroupBox1 = new CCWin.SkinControl.SkinGroupBox();
            this.LabPos_Z = new CCWin.SkinControl.SkinLabel();
            this.LabPos_Y = new CCWin.SkinControl.SkinLabel();
            this.LabPos_X = new CCWin.SkinControl.SkinLabel();
            this.skinLabel4 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel5 = new CCWin.SkinControl.SkinLabel();
            this.skinLabel7 = new CCWin.SkinControl.SkinLabel();
            this.CboDisplayType = new CCWin.SkinControl.SkinComboBox();
            this.skinLabel8 = new CCWin.SkinControl.SkinLabel();
            this.skinGroupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NudPos_Z)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudPos_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudPos_X)).BeginInit();
            this.skinGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStop
            // 
            resources.ApplyResources(this.btnStop, "btnStop");
            this.btnStop.BackColor = System.Drawing.Color.Transparent;
            this.btnStop.BaseColor = System.Drawing.Color.Red;
            this.btnStop.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0)))));
            this.btnStop.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnStop.DownBack = null;
            this.btnStop.DownBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.IsDrawGlass = false;
            this.btnStop.MouseBack = null;
            this.btnStop.MouseBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnStop.Name = "btnStop";
            this.btnStop.NormlBack = null;
            this.btnStop.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            resources.ApplyResources(this.btnStart, "btnStart");
            this.btnStart.BackColor = System.Drawing.Color.Transparent;
            this.btnStart.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnStart.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnStart.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnStart.DownBack = null;
            this.btnStart.DownBaseColor = System.Drawing.Color.Green;
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.IsDrawGlass = false;
            this.btnStart.MouseBack = null;
            this.btnStart.MouseBaseColor = System.Drawing.Color.Lime;
            this.btnStart.Name = "btnStart";
            this.btnStart.NormlBack = null;
            this.btnStart.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // skinGroupBox6
            // 
            resources.ApplyResources(this.skinGroupBox6, "skinGroupBox6");
            this.skinGroupBox6.BackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox6.BorderColor = System.Drawing.Color.Black;
            this.skinGroupBox6.Controls.Add(this.NudPos_Z);
            this.skinGroupBox6.Controls.Add(this.skinLabel3);
            this.skinGroupBox6.Controls.Add(this.NudPos_Y);
            this.skinGroupBox6.Controls.Add(this.BtnSave);
            this.skinGroupBox6.Controls.Add(this.skinLabel2);
            this.skinGroupBox6.Controls.Add(this.NudCount);
            this.skinGroupBox6.Controls.Add(this.skinLabel1);
            this.skinGroupBox6.Controls.Add(this.NudPos_X);
            this.skinGroupBox6.Controls.Add(this.skinLabel33);
            this.skinGroupBox6.Controls.Add(this.CboAxisType);
            this.skinGroupBox6.Controls.Add(this.skinLabel19);
            this.skinGroupBox6.ForeColor = System.Drawing.Color.Black;
            this.skinGroupBox6.Name = "skinGroupBox6";
            this.skinGroupBox6.RectBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.skinGroupBox6.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinGroupBox6.TabStop = false;
            this.skinGroupBox6.TitleBorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.skinGroupBox6.TitleRectBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.skinGroupBox6.TitleRoundStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // NudPos_Z
            // 
            resources.ApplyResources(this.NudPos_Z, "NudPos_Z");
            this.NudPos_Z.DecimalPlaces = 3;
            this.NudPos_Z.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.NudPos_Z.Maximum = new decimal(new int[] {
            90000,
            0,
            0,
            0});
            this.NudPos_Z.Minimum = new decimal(new int[] {
            90000,
            0,
            0,
            -2147483648});
            this.NudPos_Z.Name = "NudPos_Z";
            this.NudPos_Z.ReadOnly = true;
            // 
            // skinLabel3
            // 
            resources.ApplyResources(this.skinLabel3, "skinLabel3");
            this.skinLabel3.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel3.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.skinLabel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.skinLabel3.Name = "skinLabel3";
            // 
            // NudPos_Y
            // 
            resources.ApplyResources(this.NudPos_Y, "NudPos_Y");
            this.NudPos_Y.DecimalPlaces = 3;
            this.NudPos_Y.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.NudPos_Y.Maximum = new decimal(new int[] {
            90000,
            0,
            0,
            0});
            this.NudPos_Y.Minimum = new decimal(new int[] {
            90000,
            0,
            0,
            -2147483648});
            this.NudPos_Y.Name = "NudPos_Y";
            this.NudPos_Y.ReadOnly = true;
            // 
            // BtnSave
            // 
            resources.ApplyResources(this.BtnSave, "BtnSave");
            this.BtnSave.BackColor = System.Drawing.Color.Transparent;
            this.BtnSave.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.BtnSave.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.BtnSave.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.BtnSave.DownBack = null;
            this.BtnSave.DownBaseColor = System.Drawing.Color.Green;
            this.BtnSave.ForeColor = System.Drawing.Color.White;
            this.BtnSave.IsDrawGlass = false;
            this.BtnSave.MouseBack = null;
            this.BtnSave.MouseBaseColor = System.Drawing.Color.Lime;
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.NormlBack = null;
            this.BtnSave.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.BtnSave.UseVisualStyleBackColor = false;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // skinLabel2
            // 
            resources.ApplyResources(this.skinLabel2, "skinLabel2");
            this.skinLabel2.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel2.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.skinLabel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.skinLabel2.Name = "skinLabel2";
            // 
            // NudCount
            // 
            resources.ApplyResources(this.NudCount, "NudCount");
            this.NudCount.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.NudCount.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.NudCount.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NudCount.Name = "NudCount";
            this.NudCount.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // skinLabel1
            // 
            resources.ApplyResources(this.skinLabel1, "skinLabel1");
            this.skinLabel1.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel1.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.skinLabel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.skinLabel1.Name = "skinLabel1";
            // 
            // NudPos_X
            // 
            resources.ApplyResources(this.NudPos_X, "NudPos_X");
            this.NudPos_X.DecimalPlaces = 3;
            this.NudPos_X.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.NudPos_X.Maximum = new decimal(new int[] {
            90000,
            0,
            0,
            0});
            this.NudPos_X.Minimum = new decimal(new int[] {
            90000,
            0,
            0,
            -2147483648});
            this.NudPos_X.Name = "NudPos_X";
            this.NudPos_X.ReadOnly = true;
            // 
            // skinLabel33
            // 
            resources.ApplyResources(this.skinLabel33, "skinLabel33");
            this.skinLabel33.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel33.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.skinLabel33.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.skinLabel33.Name = "skinLabel33";
            // 
            // CboAxisType
            // 
            resources.ApplyResources(this.CboAxisType, "CboAxisType");
            this.CboAxisType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.CboAxisType.FormattingEnabled = true;
            this.CboAxisType.Name = "CboAxisType";
            this.CboAxisType.Tag = "0";
            this.CboAxisType.WaterText = "";
            this.CboAxisType.SelectedValueChanged += new System.EventHandler(this.CboAxisType_SelectedValueChanged);
            // 
            // skinLabel19
            // 
            resources.ApplyResources(this.skinLabel19, "skinLabel19");
            this.skinLabel19.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel19.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.skinLabel19.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.skinLabel19.Name = "skinLabel19";
            // 
            // skinGroupBox1
            // 
            resources.ApplyResources(this.skinGroupBox1, "skinGroupBox1");
            this.skinGroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox1.BorderColor = System.Drawing.Color.Black;
            this.skinGroupBox1.Controls.Add(this.LabPos_Z);
            this.skinGroupBox1.Controls.Add(this.LabPos_Y);
            this.skinGroupBox1.Controls.Add(this.LabPos_X);
            this.skinGroupBox1.Controls.Add(this.skinLabel4);
            this.skinGroupBox1.Controls.Add(this.skinLabel5);
            this.skinGroupBox1.Controls.Add(this.skinLabel7);
            this.skinGroupBox1.Controls.Add(this.CboDisplayType);
            this.skinGroupBox1.Controls.Add(this.skinLabel8);
            this.skinGroupBox1.ForeColor = System.Drawing.Color.Black;
            this.skinGroupBox1.Name = "skinGroupBox1";
            this.skinGroupBox1.RectBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.skinGroupBox1.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinGroupBox1.TabStop = false;
            this.skinGroupBox1.TitleBorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.skinGroupBox1.TitleRectBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.skinGroupBox1.TitleRoundStyle = CCWin.SkinClass.RoundStyle.All;
            // 
            // LabPos_Z
            // 
            resources.ApplyResources(this.LabPos_Z, "LabPos_Z");
            this.LabPos_Z.BackColor = System.Drawing.Color.Gray;
            this.LabPos_Z.BorderColor = System.Drawing.Color.Transparent;
            this.LabPos_Z.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LabPos_Z.ForeColor = System.Drawing.Color.Blue;
            this.LabPos_Z.Name = "LabPos_Z";
            // 
            // LabPos_Y
            // 
            resources.ApplyResources(this.LabPos_Y, "LabPos_Y");
            this.LabPos_Y.BackColor = System.Drawing.Color.Gray;
            this.LabPos_Y.BorderColor = System.Drawing.Color.Transparent;
            this.LabPos_Y.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LabPos_Y.ForeColor = System.Drawing.Color.Blue;
            this.LabPos_Y.Name = "LabPos_Y";
            // 
            // LabPos_X
            // 
            resources.ApplyResources(this.LabPos_X, "LabPos_X");
            this.LabPos_X.BackColor = System.Drawing.Color.Gray;
            this.LabPos_X.BorderColor = System.Drawing.Color.Transparent;
            this.LabPos_X.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LabPos_X.ForeColor = System.Drawing.Color.Blue;
            this.LabPos_X.Name = "LabPos_X";
            // 
            // skinLabel4
            // 
            resources.ApplyResources(this.skinLabel4, "skinLabel4");
            this.skinLabel4.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel4.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.skinLabel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.skinLabel4.Name = "skinLabel4";
            // 
            // skinLabel5
            // 
            resources.ApplyResources(this.skinLabel5, "skinLabel5");
            this.skinLabel5.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel5.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.skinLabel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.skinLabel5.Name = "skinLabel5";
            // 
            // skinLabel7
            // 
            resources.ApplyResources(this.skinLabel7, "skinLabel7");
            this.skinLabel7.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel7.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.skinLabel7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.skinLabel7.Name = "skinLabel7";
            // 
            // CboDisplayType
            // 
            resources.ApplyResources(this.CboDisplayType, "CboDisplayType");
            this.CboDisplayType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.CboDisplayType.FormattingEnabled = true;
            this.CboDisplayType.Name = "CboDisplayType";
            this.CboDisplayType.Tag = "0";
            this.CboDisplayType.WaterText = "";
            this.CboDisplayType.SelectedIndexChanged += new System.EventHandler(this.CboDisplayType_SelectedIndexChanged);
            // 
            // skinLabel8
            // 
            resources.ApplyResources(this.skinLabel8, "skinLabel8");
            this.skinLabel8.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel8.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.skinLabel8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.skinLabel8.Name = "skinLabel8";
            // 
            // CMKPnl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this.skinGroupBox1);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.skinGroupBox6);
            this.DoubleBuffered = true;
            this.Name = "CMKPnl";
            this.Load += new System.EventHandler(this.CMKPnl_Load);
            this.skinGroupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NudPos_Z)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudPos_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudPos_X)).EndInit();
            this.skinGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinButton btnStop;
        private CCWin.SkinControl.SkinButton btnStart;
        private CCWin.SkinControl.SkinGroupBox skinGroupBox6;
        private CCWin.SkinControl.SkinComboBox CboAxisType;
        private CCWin.SkinControl.SkinLabel skinLabel19;
        private CCWin.SkinControl.SkinLabel skinLabel33;
        private CCWin.SkinControl.SkinButton BtnSave;
        private System.Windows.Forms.NumericUpDown NudPos_X;
        private System.Windows.Forms.NumericUpDown NudCount;
        private CCWin.SkinControl.SkinLabel skinLabel1;
        private System.Windows.Forms.NumericUpDown NudPos_Z;
        private CCWin.SkinControl.SkinLabel skinLabel3;
        private System.Windows.Forms.NumericUpDown NudPos_Y;
        private CCWin.SkinControl.SkinLabel skinLabel2;
        private CCWin.SkinControl.SkinGroupBox skinGroupBox1;
        private CCWin.SkinControl.SkinLabel skinLabel4;
        private CCWin.SkinControl.SkinLabel skinLabel5;
        private CCWin.SkinControl.SkinLabel skinLabel7;
        private CCWin.SkinControl.SkinComboBox CboDisplayType;
        private CCWin.SkinControl.SkinLabel skinLabel8;
        private CCWin.SkinControl.SkinLabel LabPos_Z;
        private CCWin.SkinControl.SkinLabel LabPos_Y;
        private CCWin.SkinControl.SkinLabel LabPos_X;
    }
}
