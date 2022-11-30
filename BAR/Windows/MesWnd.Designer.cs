namespace BAR.Windows
{
    partial class MesWnd
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nudTargetNum = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAppPath = new CCWin.SkinControl.SkinTextBox();
            this.cobMesExit = new CCWin.SkinControl.SkinComboBox();
            this.txtFilePath = new CCWin.SkinControl.SkinTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cobFuncMode = new CCWin.SkinControl.SkinComboBox();
            this.btnBrowse2 = new CCWin.SkinControl.SkinButton();
            this.btnBrowse1 = new CCWin.SkinControl.SkinButton();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtCheckSum_File = new CCWin.SkinControl.SkinTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPositionNum = new CCWin.SkinControl.SkinTextBox();
            this.txtCheckSum_Mes = new CCWin.SkinControl.SkinTextBox();
            this.txtJobNum = new CCWin.SkinControl.SkinTextBox();
            this.txtPnId = new CCWin.SkinControl.SkinTextBox();
            this.label131 = new System.Windows.Forms.Label();
            this.txtUserID = new CCWin.SkinControl.SkinTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnGet = new CCWin.SkinControl.SkinButton();
            this.btnStart = new CCWin.SkinControl.SkinButton();
            this.btnStop = new CCWin.SkinControl.SkinButton();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMesLog = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtFileName = new CCWin.SkinControl.SkinTextBox();
            this.btnBrowse3 = new CCWin.SkinControl.SkinButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetNum)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nudTargetNum);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtAppPath);
            this.groupBox1.Controls.Add(this.cobMesExit);
            this.groupBox1.Controls.Add(this.txtFilePath);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cobFuncMode);
            this.groupBox1.Controls.Add(this.btnBrowse2);
            this.groupBox1.Controls.Add(this.btnBrowse1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(872, 171);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mes烧录信息";
            // 
            // nudTargetNum
            // 
            this.nudTargetNum.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nudTargetNum.Location = new System.Drawing.Point(531, 135);
            this.nudTargetNum.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudTargetNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTargetNum.Name = "nudTargetNum";
            this.nudTargetNum.Size = new System.Drawing.Size(130, 26);
            this.nudTargetNum.TabIndex = 292;
            this.nudTargetNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label11.Location = new System.Drawing.Point(443, 136);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(85, 24);
            this.label11.TabIndex = 291;
            this.label11.Text = "批次数量:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(52, 104);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(162, 24);
            this.label8.TabIndex = 290;
            this.label8.Text = "MES结束判定条件:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(55, 70);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(159, 24);
            this.label5.TabIndex = 160;
            this.label5.Text = "烧录文档所在文件夹:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtAppPath
            // 
            this.txtAppPath.BackColor = System.Drawing.Color.Transparent;
            this.txtAppPath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtAppPath.DownBack = null;
            this.txtAppPath.Icon = null;
            this.txtAppPath.IconIsButton = false;
            this.txtAppPath.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtAppPath.IsPasswordChat = '\0';
            this.txtAppPath.IsSystemPasswordChar = false;
            this.txtAppPath.Lines = new string[0];
            this.txtAppPath.Location = new System.Drawing.Point(217, 29);
            this.txtAppPath.Margin = new System.Windows.Forms.Padding(0);
            this.txtAppPath.MaxLength = 32767;
            this.txtAppPath.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtAppPath.MouseBack = null;
            this.txtAppPath.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtAppPath.Multiline = true;
            this.txtAppPath.Name = "txtAppPath";
            this.txtAppPath.NormlBack = null;
            this.txtAppPath.Padding = new System.Windows.Forms.Padding(5);
            this.txtAppPath.ReadOnly = true;
            this.txtAppPath.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtAppPath.Size = new System.Drawing.Size(494, 34);
            // 
            // 
            // 
            this.txtAppPath.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtAppPath.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAppPath.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtAppPath.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtAppPath.SkinTxt.Multiline = true;
            this.txtAppPath.SkinTxt.Name = "BaseText";
            this.txtAppPath.SkinTxt.ReadOnly = true;
            this.txtAppPath.SkinTxt.Size = new System.Drawing.Size(480, 20);
            this.txtAppPath.SkinTxt.TabIndex = 0;
            this.txtAppPath.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtAppPath.SkinTxt.WaterText = "";
            this.txtAppPath.TabIndex = 145;
            this.txtAppPath.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtAppPath.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtAppPath.WaterText = "";
            this.txtAppPath.WordWrap = true;
            // 
            // cobMesExit
            // 
            this.cobMesExit.BorderColor = System.Drawing.Color.Gray;
            this.cobMesExit.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cobMesExit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cobMesExit.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cobMesExit.FormattingEnabled = true;
            this.cobMesExit.Location = new System.Drawing.Point(217, 104);
            this.cobMesExit.Name = "cobMesExit";
            this.cobMesExit.Size = new System.Drawing.Size(494, 27);
            this.cobMesExit.TabIndex = 289;
            this.cobMesExit.WaterText = "";
            // 
            // txtFilePath
            // 
            this.txtFilePath.BackColor = System.Drawing.Color.Transparent;
            this.txtFilePath.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtFilePath.DownBack = null;
            this.txtFilePath.Icon = null;
            this.txtFilePath.IconIsButton = false;
            this.txtFilePath.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtFilePath.IsPasswordChat = '\0';
            this.txtFilePath.IsSystemPasswordChar = false;
            this.txtFilePath.Lines = new string[0];
            this.txtFilePath.Location = new System.Drawing.Point(217, 67);
            this.txtFilePath.Margin = new System.Windows.Forms.Padding(0);
            this.txtFilePath.MaxLength = 32767;
            this.txtFilePath.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtFilePath.MouseBack = null;
            this.txtFilePath.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtFilePath.Multiline = true;
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.NormlBack = null;
            this.txtFilePath.Padding = new System.Windows.Forms.Padding(5);
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtFilePath.Size = new System.Drawing.Size(494, 34);
            // 
            // 
            // 
            this.txtFilePath.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFilePath.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFilePath.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFilePath.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtFilePath.SkinTxt.Multiline = true;
            this.txtFilePath.SkinTxt.Name = "BaseText";
            this.txtFilePath.SkinTxt.ReadOnly = true;
            this.txtFilePath.SkinTxt.Size = new System.Drawing.Size(480, 20);
            this.txtFilePath.SkinTxt.TabIndex = 0;
            this.txtFilePath.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtFilePath.SkinTxt.WaterText = "";
            this.txtFilePath.TabIndex = 161;
            this.txtFilePath.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtFilePath.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtFilePath.WaterText = "";
            this.txtFilePath.WordWrap = true;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(94, 136);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 24);
            this.label3.TabIndex = 153;
            this.label3.Text = "默认执行命令:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cobFuncMode
            // 
            this.cobFuncMode.BorderColor = System.Drawing.Color.Gray;
            this.cobFuncMode.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cobFuncMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cobFuncMode.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cobFuncMode.FormattingEnabled = true;
            this.cobFuncMode.Location = new System.Drawing.Point(217, 136);
            this.cobFuncMode.Name = "cobFuncMode";
            this.cobFuncMode.Size = new System.Drawing.Size(121, 27);
            this.cobFuncMode.TabIndex = 150;
            this.cobFuncMode.WaterText = "";
            // 
            // btnBrowse2
            // 
            this.btnBrowse2.BackColor = System.Drawing.Color.Transparent;
            this.btnBrowse2.BaseColor = System.Drawing.Color.Gray;
            this.btnBrowse2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0)))));
            this.btnBrowse2.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnBrowse2.DownBack = null;
            this.btnBrowse2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBrowse2.ForeColor = System.Drawing.Color.White;
            this.btnBrowse2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBrowse2.IsDrawGlass = false;
            this.btnBrowse2.Location = new System.Drawing.Point(726, 66);
            this.btnBrowse2.MouseBack = null;
            this.btnBrowse2.Name = "btnBrowse2";
            this.btnBrowse2.NormlBack = null;
            this.btnBrowse2.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnBrowse2.Size = new System.Drawing.Size(63, 34);
            this.btnBrowse2.TabIndex = 149;
            this.btnBrowse2.Text = "浏览";
            this.btnBrowse2.UseVisualStyleBackColor = false;
            this.btnBrowse2.Click += new System.EventHandler(this.btnBrowse2_Click);
            // 
            // btnBrowse1
            // 
            this.btnBrowse1.BackColor = System.Drawing.Color.Transparent;
            this.btnBrowse1.BaseColor = System.Drawing.Color.Gray;
            this.btnBrowse1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0)))));
            this.btnBrowse1.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnBrowse1.DownBack = null;
            this.btnBrowse1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBrowse1.ForeColor = System.Drawing.Color.White;
            this.btnBrowse1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBrowse1.IsDrawGlass = false;
            this.btnBrowse1.Location = new System.Drawing.Point(726, 29);
            this.btnBrowse1.MouseBack = null;
            this.btnBrowse1.Name = "btnBrowse1";
            this.btnBrowse1.NormlBack = null;
            this.btnBrowse1.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnBrowse1.Size = new System.Drawing.Size(63, 34);
            this.btnBrowse1.TabIndex = 148;
            this.btnBrowse1.Text = "浏览";
            this.btnBrowse1.UseVisualStyleBackColor = false;
            this.btnBrowse1.Visible = false;
            this.btnBrowse1.Click += new System.EventHandler(this.btnBrowse1_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(19, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(195, 24);
            this.label2.TabIndex = 142;
            this.label2.Text = "MultiAprog.exe所在文件夹:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtCheckSum_File);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtPositionNum);
            this.groupBox2.Controls.Add(this.txtCheckSum_Mes);
            this.groupBox2.Controls.Add(this.txtJobNum);
            this.groupBox2.Controls.Add(this.txtPnId);
            this.groupBox2.Controls.Add(this.label131);
            this.groupBox2.Controls.Add(this.txtUserID);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(12, 189);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(872, 209);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mes设备信息";
            // 
            // txtCheckSum_File
            // 
            this.txtCheckSum_File.BackColor = System.Drawing.Color.Transparent;
            this.txtCheckSum_File.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtCheckSum_File.DownBack = null;
            this.txtCheckSum_File.Icon = null;
            this.txtCheckSum_File.IconIsButton = false;
            this.txtCheckSum_File.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtCheckSum_File.IsPasswordChat = '\0';
            this.txtCheckSum_File.IsSystemPasswordChar = false;
            this.txtCheckSum_File.Lines = new string[0];
            this.txtCheckSum_File.Location = new System.Drawing.Point(542, 101);
            this.txtCheckSum_File.Margin = new System.Windows.Forms.Padding(0);
            this.txtCheckSum_File.MaxLength = 32767;
            this.txtCheckSum_File.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtCheckSum_File.MouseBack = null;
            this.txtCheckSum_File.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtCheckSum_File.Multiline = true;
            this.txtCheckSum_File.Name = "txtCheckSum_File";
            this.txtCheckSum_File.NormlBack = null;
            this.txtCheckSum_File.Padding = new System.Windows.Forms.Padding(5);
            this.txtCheckSum_File.ReadOnly = true;
            this.txtCheckSum_File.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCheckSum_File.Size = new System.Drawing.Size(190, 34);
            // 
            // 
            // 
            this.txtCheckSum_File.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCheckSum_File.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCheckSum_File.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCheckSum_File.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtCheckSum_File.SkinTxt.Multiline = true;
            this.txtCheckSum_File.SkinTxt.Name = "BaseText";
            this.txtCheckSum_File.SkinTxt.ReadOnly = true;
            this.txtCheckSum_File.SkinTxt.Size = new System.Drawing.Size(176, 20);
            this.txtCheckSum_File.SkinTxt.TabIndex = 0;
            this.txtCheckSum_File.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtCheckSum_File.SkinTxt.WaterText = "";
            this.txtCheckSum_File.TabIndex = 164;
            this.txtCheckSum_File.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtCheckSum_File.Visible = false;
            this.txtCheckSum_File.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtCheckSum_File.WaterText = "";
            this.txtCheckSum_File.WordWrap = true;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(422, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 34);
            this.label1.TabIndex = 163;
            this.label1.Text = "烧录数据校验值:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.Visible = false;
            // 
            // txtPositionNum
            // 
            this.txtPositionNum.BackColor = System.Drawing.Color.Transparent;
            this.txtPositionNum.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtPositionNum.DownBack = null;
            this.txtPositionNum.Icon = null;
            this.txtPositionNum.IconIsButton = false;
            this.txtPositionNum.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtPositionNum.IsPasswordChat = '\0';
            this.txtPositionNum.IsSystemPasswordChar = false;
            this.txtPositionNum.Lines = new string[0];
            this.txtPositionNum.Location = new System.Drawing.Point(199, 65);
            this.txtPositionNum.Margin = new System.Windows.Forms.Padding(0);
            this.txtPositionNum.MaxLength = 32767;
            this.txtPositionNum.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtPositionNum.MouseBack = null;
            this.txtPositionNum.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtPositionNum.Multiline = true;
            this.txtPositionNum.Name = "txtPositionNum";
            this.txtPositionNum.NormlBack = null;
            this.txtPositionNum.Padding = new System.Windows.Forms.Padding(5);
            this.txtPositionNum.ReadOnly = false;
            this.txtPositionNum.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtPositionNum.Size = new System.Drawing.Size(190, 34);
            // 
            // 
            // 
            this.txtPositionNum.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPositionNum.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPositionNum.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPositionNum.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtPositionNum.SkinTxt.Multiline = true;
            this.txtPositionNum.SkinTxt.Name = "BaseText";
            this.txtPositionNum.SkinTxt.Size = new System.Drawing.Size(176, 20);
            this.txtPositionNum.SkinTxt.TabIndex = 0;
            this.txtPositionNum.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtPositionNum.SkinTxt.WaterText = "";
            this.txtPositionNum.TabIndex = 162;
            this.txtPositionNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtPositionNum.Visible = false;
            this.txtPositionNum.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtPositionNum.WaterText = "";
            this.txtPositionNum.WordWrap = true;
            // 
            // txtCheckSum_Mes
            // 
            this.txtCheckSum_Mes.BackColor = System.Drawing.Color.Transparent;
            this.txtCheckSum_Mes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtCheckSum_Mes.DownBack = null;
            this.txtCheckSum_Mes.Icon = null;
            this.txtCheckSum_Mes.IconIsButton = false;
            this.txtCheckSum_Mes.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtCheckSum_Mes.IsPasswordChat = '\0';
            this.txtCheckSum_Mes.IsSystemPasswordChar = false;
            this.txtCheckSum_Mes.Lines = new string[] {
        "F8AFFDD3"};
            this.txtCheckSum_Mes.Location = new System.Drawing.Point(542, 65);
            this.txtCheckSum_Mes.Margin = new System.Windows.Forms.Padding(0);
            this.txtCheckSum_Mes.MaxLength = 32767;
            this.txtCheckSum_Mes.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtCheckSum_Mes.MouseBack = null;
            this.txtCheckSum_Mes.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtCheckSum_Mes.Multiline = true;
            this.txtCheckSum_Mes.Name = "txtCheckSum_Mes";
            this.txtCheckSum_Mes.NormlBack = null;
            this.txtCheckSum_Mes.Padding = new System.Windows.Forms.Padding(5);
            this.txtCheckSum_Mes.ReadOnly = true;
            this.txtCheckSum_Mes.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCheckSum_Mes.Size = new System.Drawing.Size(190, 34);
            // 
            // 
            // 
            this.txtCheckSum_Mes.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCheckSum_Mes.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCheckSum_Mes.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCheckSum_Mes.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtCheckSum_Mes.SkinTxt.Multiline = true;
            this.txtCheckSum_Mes.SkinTxt.Name = "BaseText";
            this.txtCheckSum_Mes.SkinTxt.ReadOnly = true;
            this.txtCheckSum_Mes.SkinTxt.Size = new System.Drawing.Size(176, 20);
            this.txtCheckSum_Mes.SkinTxt.TabIndex = 0;
            this.txtCheckSum_Mes.SkinTxt.Text = "F8AFFDD3";
            this.txtCheckSum_Mes.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtCheckSum_Mes.SkinTxt.WaterText = "";
            this.txtCheckSum_Mes.TabIndex = 159;
            this.txtCheckSum_Mes.Text = "F8AFFDD3";
            this.txtCheckSum_Mes.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtCheckSum_Mes.Visible = false;
            this.txtCheckSum_Mes.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtCheckSum_Mes.WaterText = "";
            this.txtCheckSum_Mes.WordWrap = true;
            // 
            // txtJobNum
            // 
            this.txtJobNum.BackColor = System.Drawing.Color.Transparent;
            this.txtJobNum.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtJobNum.DownBack = null;
            this.txtJobNum.Icon = null;
            this.txtJobNum.IconIsButton = false;
            this.txtJobNum.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtJobNum.IsPasswordChat = '\0';
            this.txtJobNum.IsSystemPasswordChar = false;
            this.txtJobNum.Lines = new string[] {
        "01234567890123456789"};
            this.txtJobNum.Location = new System.Drawing.Point(199, 101);
            this.txtJobNum.Margin = new System.Windows.Forms.Padding(0);
            this.txtJobNum.MaxLength = 32767;
            this.txtJobNum.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtJobNum.MouseBack = null;
            this.txtJobNum.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtJobNum.Multiline = true;
            this.txtJobNum.Name = "txtJobNum";
            this.txtJobNum.NormlBack = null;
            this.txtJobNum.Padding = new System.Windows.Forms.Padding(5);
            this.txtJobNum.ReadOnly = false;
            this.txtJobNum.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtJobNum.Size = new System.Drawing.Size(533, 34);
            // 
            // 
            // 
            this.txtJobNum.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtJobNum.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtJobNum.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtJobNum.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtJobNum.SkinTxt.Multiline = true;
            this.txtJobNum.SkinTxt.Name = "BaseText";
            this.txtJobNum.SkinTxt.Size = new System.Drawing.Size(519, 20);
            this.txtJobNum.SkinTxt.TabIndex = 0;
            this.txtJobNum.SkinTxt.Text = "01234567890123456789";
            this.txtJobNum.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtJobNum.SkinTxt.WaterText = "";
            this.txtJobNum.TabIndex = 158;
            this.txtJobNum.Text = "01234567890123456789";
            this.txtJobNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtJobNum.Visible = false;
            this.txtJobNum.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtJobNum.WaterText = "";
            this.txtJobNum.WordWrap = true;
            // 
            // txtPnId
            // 
            this.txtPnId.BackColor = System.Drawing.Color.Transparent;
            this.txtPnId.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtPnId.DownBack = null;
            this.txtPnId.Icon = null;
            this.txtPnId.IconIsButton = false;
            this.txtPnId.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtPnId.IsPasswordChat = '\0';
            this.txtPnId.IsSystemPasswordChar = false;
            this.txtPnId.Lines = new string[0];
            this.txtPnId.Location = new System.Drawing.Point(542, 29);
            this.txtPnId.Margin = new System.Windows.Forms.Padding(0);
            this.txtPnId.MaxLength = 32767;
            this.txtPnId.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtPnId.MouseBack = null;
            this.txtPnId.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtPnId.Multiline = true;
            this.txtPnId.Name = "txtPnId";
            this.txtPnId.NormlBack = null;
            this.txtPnId.Padding = new System.Windows.Forms.Padding(5);
            this.txtPnId.ReadOnly = true;
            this.txtPnId.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtPnId.Size = new System.Drawing.Size(190, 34);
            // 
            // 
            // 
            this.txtPnId.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPnId.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPnId.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPnId.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtPnId.SkinTxt.Multiline = true;
            this.txtPnId.SkinTxt.Name = "BaseText";
            this.txtPnId.SkinTxt.ReadOnly = true;
            this.txtPnId.SkinTxt.Size = new System.Drawing.Size(176, 20);
            this.txtPnId.SkinTxt.TabIndex = 0;
            this.txtPnId.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtPnId.SkinTxt.WaterText = "";
            this.txtPnId.TabIndex = 157;
            this.txtPnId.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtPnId.Visible = false;
            this.txtPnId.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtPnId.WaterText = "";
            this.txtPnId.WordWrap = true;
            // 
            // label131
            // 
            this.label131.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label131.ForeColor = System.Drawing.Color.Black;
            this.label131.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label131.Location = new System.Drawing.Point(73, 65);
            this.label131.Name = "label131";
            this.label131.Size = new System.Drawing.Size(120, 34);
            this.label131.TabIndex = 140;
            this.label131.Text = "站位名称:";
            this.label131.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label131.Visible = false;
            // 
            // txtUserID
            // 
            this.txtUserID.BackColor = System.Drawing.Color.Transparent;
            this.txtUserID.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtUserID.DownBack = null;
            this.txtUserID.Icon = null;
            this.txtUserID.IconIsButton = false;
            this.txtUserID.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtUserID.IsPasswordChat = '\0';
            this.txtUserID.IsSystemPasswordChar = false;
            this.txtUserID.Lines = new string[0];
            this.txtUserID.Location = new System.Drawing.Point(200, 29);
            this.txtUserID.Margin = new System.Windows.Forms.Padding(0);
            this.txtUserID.MaxLength = 32767;
            this.txtUserID.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtUserID.MouseBack = null;
            this.txtUserID.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtUserID.Multiline = true;
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.NormlBack = null;
            this.txtUserID.Padding = new System.Windows.Forms.Padding(5);
            this.txtUserID.ReadOnly = true;
            this.txtUserID.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtUserID.Size = new System.Drawing.Size(190, 34);
            // 
            // 
            // 
            this.txtUserID.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUserID.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUserID.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUserID.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtUserID.SkinTxt.Multiline = true;
            this.txtUserID.SkinTxt.Name = "BaseText";
            this.txtUserID.SkinTxt.ReadOnly = true;
            this.txtUserID.SkinTxt.Size = new System.Drawing.Size(176, 20);
            this.txtUserID.SkinTxt.TabIndex = 0;
            this.txtUserID.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtUserID.SkinTxt.WaterText = "";
            this.txtUserID.TabIndex = 156;
            this.txtUserID.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtUserID.Visible = false;
            this.txtUserID.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtUserID.WaterText = "";
            this.txtUserID.WordWrap = true;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.Control;
            this.label9.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(60, 29);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(140, 34);
            this.label9.TabIndex = 155;
            this.label9.Text = "操作人员ID号:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label9.Visible = false;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(420, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(119, 24);
            this.label7.TabIndex = 153;
            this.label7.Text = "料号:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label7.Visible = false;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(422, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 34);
            this.label6.TabIndex = 152;
            this.label6.Text = "数据库校验值:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label6.Visible = false;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(76, 106);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 24);
            this.label4.TabIndex = 150;
            this.label4.Text = "工单号:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label4.Visible = false;
            // 
            // btnGet
            // 
            this.btnGet.BackColor = System.Drawing.Color.Transparent;
            this.btnGet.BaseColor = System.Drawing.Color.DodgerBlue;
            this.btnGet.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0)))));
            this.btnGet.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnGet.DownBack = null;
            this.btnGet.Font = new System.Drawing.Font("楷体", 12F, System.Drawing.FontStyle.Bold);
            this.btnGet.ForeColor = System.Drawing.Color.White;
            this.btnGet.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnGet.IsDrawGlass = false;
            this.btnGet.Location = new System.Drawing.Point(129, 440);
            this.btnGet.MouseBack = null;
            this.btnGet.MouseBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(200)))));
            this.btnGet.Name = "btnGet";
            this.btnGet.NormlBack = null;
            this.btnGet.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnGet.Size = new System.Drawing.Size(179, 43);
            this.btnGet.TabIndex = 125;
            this.btnGet.Text = "获取料号信息";
            this.btnGet.UseVisualStyleBackColor = false;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.Transparent;
            this.btnStart.BaseColor = System.Drawing.Color.Goldenrod;
            this.btnStart.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0)))));
            this.btnStart.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnStart.DownBack = null;
            this.btnStart.Font = new System.Drawing.Font("楷体", 12F, System.Drawing.FontStyle.Bold);
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnStart.IsDrawGlass = false;
            this.btnStart.Location = new System.Drawing.Point(350, 440);
            this.btnStart.MouseBack = null;
            this.btnStart.MouseBaseColor = System.Drawing.Color.DarkGoldenrod;
            this.btnStart.Name = "btnStart";
            this.btnStart.NormlBack = null;
            this.btnStart.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnStart.Size = new System.Drawing.Size(179, 43);
            this.btnStart.TabIndex = 124;
            this.btnStart.Text = "批量生产开始";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.Transparent;
            this.btnStop.BaseColor = System.Drawing.Color.Red;
            this.btnStop.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0)))));
            this.btnStop.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnStop.DownBack = null;
            this.btnStop.Font = new System.Drawing.Font("楷体", 12F, System.Drawing.FontStyle.Bold);
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnStop.IsDrawGlass = false;
            this.btnStop.Location = new System.Drawing.Point(571, 440);
            this.btnStop.MouseBack = null;
            this.btnStop.MouseBaseColor = System.Drawing.Color.DarkGoldenrod;
            this.btnStop.Name = "btnStop";
            this.btnStop.NormlBack = null;
            this.btnStop.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnStop.Size = new System.Drawing.Size(179, 43);
            this.btnStop.TabIndex = 126;
            this.btnStop.Text = "取消批量生产";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(8, 479);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(71, 24);
            this.label10.TabIndex = 287;
            this.label10.Text = "概要信息:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMesLog
            // 
            this.txtMesLog.BackColor = System.Drawing.Color.White;
            this.txtMesLog.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMesLog.Location = new System.Drawing.Point(0, 502);
            this.txtMesLog.Multiline = true;
            this.txtMesLog.Name = "txtMesLog";
            this.txtMesLog.ReadOnly = true;
            this.txtMesLog.Size = new System.Drawing.Size(896, 182);
            this.txtMesLog.TabIndex = 288;
            this.txtMesLog.Text = "<==欢迎使用MES一键烧录系统, 操作之前请阅读相关操作手册==>";
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(12, 405);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(131, 24);
            this.label12.TabIndex = 289;
            this.label12.Text = "当前执行工程位置:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFileName
            // 
            this.txtFileName.BackColor = System.Drawing.Color.Transparent;
            this.txtFileName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtFileName.DownBack = null;
            this.txtFileName.Icon = null;
            this.txtFileName.IconIsButton = false;
            this.txtFileName.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtFileName.IsPasswordChat = '\0';
            this.txtFileName.IsSystemPasswordChar = false;
            this.txtFileName.Lines = new string[0];
            this.txtFileName.Location = new System.Drawing.Point(146, 402);
            this.txtFileName.Margin = new System.Windows.Forms.Padding(0);
            this.txtFileName.MaxLength = 32767;
            this.txtFileName.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtFileName.MouseBack = null;
            this.txtFileName.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtFileName.Multiline = true;
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.NormlBack = null;
            this.txtFileName.Padding = new System.Windows.Forms.Padding(5);
            this.txtFileName.ReadOnly = true;
            this.txtFileName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtFileName.Size = new System.Drawing.Size(672, 34);
            // 
            // 
            // 
            this.txtFileName.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFileName.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFileName.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFileName.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtFileName.SkinTxt.Multiline = true;
            this.txtFileName.SkinTxt.Name = "BaseText";
            this.txtFileName.SkinTxt.ReadOnly = true;
            this.txtFileName.SkinTxt.Size = new System.Drawing.Size(658, 20);
            this.txtFileName.SkinTxt.TabIndex = 0;
            this.txtFileName.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtFileName.SkinTxt.WaterText = "";
            this.txtFileName.TabIndex = 290;
            this.txtFileName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtFileName.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtFileName.WaterText = "";
            this.txtFileName.WordWrap = true;
            // 
            // btnBrowse3
            // 
            this.btnBrowse3.BackColor = System.Drawing.Color.Transparent;
            this.btnBrowse3.BaseColor = System.Drawing.Color.Gray;
            this.btnBrowse3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0)))));
            this.btnBrowse3.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnBrowse3.DownBack = null;
            this.btnBrowse3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBrowse3.ForeColor = System.Drawing.Color.White;
            this.btnBrowse3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBrowse3.IsDrawGlass = false;
            this.btnBrowse3.Location = new System.Drawing.Point(821, 402);
            this.btnBrowse3.MouseBack = null;
            this.btnBrowse3.Name = "btnBrowse3";
            this.btnBrowse3.NormlBack = null;
            this.btnBrowse3.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnBrowse3.Size = new System.Drawing.Size(63, 34);
            this.btnBrowse3.TabIndex = 291;
            this.btnBrowse3.Text = "浏览";
            this.btnBrowse3.UseVisualStyleBackColor = false;
            this.btnBrowse3.Visible = false;
            this.btnBrowse3.Click += new System.EventHandler(this.btnBrowse3_Click);
            // 
            // MesWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(896, 685);
            this.Controls.Add(this.btnBrowse3);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.txtMesLog);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnGet);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MesWnd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " MES对接";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MesWnd_FormClosing);
            this.Load += new System.EventHandler(this.MesWnd_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetNum)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private CCWin.SkinControl.SkinTextBox txtAppPath;
        private CCWin.SkinControl.SkinButton btnBrowse2;
        private CCWin.SkinControl.SkinButton btnBrowse1;
        private CCWin.SkinControl.SkinButton btnGet;
        private CCWin.SkinControl.SkinButton btnStart;
        private CCWin.SkinControl.SkinButton btnStop;
        private System.Windows.Forms.Label label10;
        private CCWin.SkinControl.SkinComboBox cobFuncMode;
        private System.Windows.Forms.Label label5;
        private CCWin.SkinControl.SkinTextBox txtFilePath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMesLog;
        private System.Windows.Forms.Label label8;
        private CCWin.SkinControl.SkinComboBox cobMesExit;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown nudTargetNum;
        private CCWin.SkinControl.SkinTextBox txtCheckSum_File;
        private System.Windows.Forms.Label label1;
        private CCWin.SkinControl.SkinTextBox txtPositionNum;
        private CCWin.SkinControl.SkinTextBox txtCheckSum_Mes;
        private CCWin.SkinControl.SkinTextBox txtJobNum;
        private CCWin.SkinControl.SkinTextBox txtPnId;
        private System.Windows.Forms.Label label131;
        private CCWin.SkinControl.SkinTextBox txtUserID;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label12;
        private CCWin.SkinControl.SkinTextBox txtFileName;
        private CCWin.SkinControl.SkinButton btnBrowse3;
    }
}