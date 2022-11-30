namespace BAR.Windows
{
    partial class TicketsWnd
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
            this.txtCustomer = new CCWin.SkinControl.SkinTextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.nudLotNum = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.txtWorkNo = new CCWin.SkinControl.SkinTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new CCWin.SkinControl.SkinButton();
            this.btnOK = new CCWin.SkinControl.SkinButton();
            this.txtICModel = new CCWin.SkinControl.SkinTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtICBrand = new CCWin.SkinControl.SkinTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtChecksum = new CCWin.SkinControl.SkinTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.nudExpectInput = new System.Windows.Forms.NumericUpDown();
            this.nudExpectOuput = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.nudNGInput = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.txtOperator = new CCWin.SkinControl.SkinTextBox();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudLotNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudExpectInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudExpectOuput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNGInput)).BeginInit();
            this.SuspendLayout();
            // 
            // txtCustomer
            // 
            this.txtCustomer.BackColor = System.Drawing.Color.Transparent;
            this.txtCustomer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtCustomer.DownBack = null;
            this.txtCustomer.Icon = null;
            this.txtCustomer.IconIsButton = false;
            this.txtCustomer.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtCustomer.IsPasswordChat = '\0';
            this.txtCustomer.IsSystemPasswordChar = false;
            this.txtCustomer.Lines = new string[0];
            this.txtCustomer.Location = new System.Drawing.Point(101, 15);
            this.txtCustomer.Margin = new System.Windows.Forms.Padding(0);
            this.txtCustomer.MaxLength = 32767;
            this.txtCustomer.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtCustomer.MouseBack = null;
            this.txtCustomer.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtCustomer.Multiline = true;
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.NormlBack = null;
            this.txtCustomer.Padding = new System.Windows.Forms.Padding(5);
            this.txtCustomer.ReadOnly = false;
            this.txtCustomer.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCustomer.Size = new System.Drawing.Size(263, 34);
            // 
            // 
            // 
            this.txtCustomer.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtCustomer.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCustomer.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCustomer.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtCustomer.SkinTxt.Multiline = true;
            this.txtCustomer.SkinTxt.Name = "BaseText";
            this.txtCustomer.SkinTxt.Size = new System.Drawing.Size(249, 20);
            this.txtCustomer.SkinTxt.TabIndex = 0;
            this.txtCustomer.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtCustomer.SkinTxt.WaterText = "";
            this.txtCustomer.TabIndex = 158;
            this.txtCustomer.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtCustomer.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtCustomer.WaterText = "";
            this.txtCustomer.WordWrap = true;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.Control;
            this.label9.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(15, 15);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(86, 34);
            this.label9.TabIndex = 157;
            this.label9.Text = "Customer:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudLotNum
            // 
            this.nudLotNum.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nudLotNum.Location = new System.Drawing.Point(101, 104);
            this.nudLotNum.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudLotNum.Name = "nudLotNum";
            this.nudLotNum.Size = new System.Drawing.Size(130, 29);
            this.nudLotNum.TabIndex = 294;
            this.nudLotNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label11.Location = new System.Drawing.Point(19, 104);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(82, 29);
            this.label11.TabIndex = 293;
            this.label11.Text = "Lot Num:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtWorkNo
            // 
            this.txtWorkNo.BackColor = System.Drawing.Color.Transparent;
            this.txtWorkNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtWorkNo.DownBack = null;
            this.txtWorkNo.Icon = null;
            this.txtWorkNo.IconIsButton = false;
            this.txtWorkNo.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtWorkNo.IsPasswordChat = '\0';
            this.txtWorkNo.IsSystemPasswordChar = false;
            this.txtWorkNo.Lines = new string[0];
            this.txtWorkNo.Location = new System.Drawing.Point(101, 60);
            this.txtWorkNo.Margin = new System.Windows.Forms.Padding(0);
            this.txtWorkNo.MaxLength = 32767;
            this.txtWorkNo.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtWorkNo.MouseBack = null;
            this.txtWorkNo.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtWorkNo.Multiline = true;
            this.txtWorkNo.Name = "txtWorkNo";
            this.txtWorkNo.NormlBack = null;
            this.txtWorkNo.Padding = new System.Windows.Forms.Padding(5);
            this.txtWorkNo.ReadOnly = false;
            this.txtWorkNo.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtWorkNo.Size = new System.Drawing.Size(263, 34);
            // 
            // 
            // 
            this.txtWorkNo.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtWorkNo.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtWorkNo.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtWorkNo.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtWorkNo.SkinTxt.Multiline = true;
            this.txtWorkNo.SkinTxt.Name = "BaseText";
            this.txtWorkNo.SkinTxt.Size = new System.Drawing.Size(249, 20);
            this.txtWorkNo.SkinTxt.TabIndex = 0;
            this.txtWorkNo.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtWorkNo.SkinTxt.WaterText = "";
            this.txtWorkNo.TabIndex = 160;
            this.txtWorkNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtWorkNo.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtWorkNo.WaterText = "";
            this.txtWorkNo.WordWrap = true;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(15, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 34);
            this.label1.TabIndex = 159;
            this.label1.Text = "WorkNo:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BaseColor = System.Drawing.Color.Red;
            this.btnCancel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0)))));
            this.btnCancel.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnCancel.DownBack = null;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.IsDrawGlass = false;
            this.btnCancel.Location = new System.Drawing.Point(238, 472);
            this.btnCancel.MouseBack = null;
            this.btnCancel.MouseBaseColor = System.Drawing.Color.DarkGoldenrod;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.NormlBack = null;
            this.btnCancel.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnCancel.Size = new System.Drawing.Size(126, 43);
            this.btnCancel.TabIndex = 296;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.BaseColor = System.Drawing.Color.Goldenrod;
            this.btnOK.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0)))));
            this.btnOK.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnOK.DownBack = null;
            this.btnOK.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK.IsDrawGlass = false;
            this.btnOK.Location = new System.Drawing.Point(17, 472);
            this.btnOK.MouseBack = null;
            this.btnOK.MouseBaseColor = System.Drawing.Color.DarkGoldenrod;
            this.btnOK.Name = "btnOK";
            this.btnOK.NormlBack = null;
            this.btnOK.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnOK.Size = new System.Drawing.Size(126, 43);
            this.btnOK.TabIndex = 295;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtICModel
            // 
            this.txtICModel.BackColor = System.Drawing.Color.Transparent;
            this.txtICModel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtICModel.DownBack = null;
            this.txtICModel.Icon = null;
            this.txtICModel.IconIsButton = false;
            this.txtICModel.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtICModel.IsPasswordChat = '\0';
            this.txtICModel.IsSystemPasswordChar = false;
            this.txtICModel.Lines = new string[0];
            this.txtICModel.Location = new System.Drawing.Point(101, 190);
            this.txtICModel.Margin = new System.Windows.Forms.Padding(0);
            this.txtICModel.MaxLength = 32767;
            this.txtICModel.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtICModel.MouseBack = null;
            this.txtICModel.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtICModel.Multiline = true;
            this.txtICModel.Name = "txtICModel";
            this.txtICModel.NormlBack = null;
            this.txtICModel.Padding = new System.Windows.Forms.Padding(5);
            this.txtICModel.ReadOnly = true;
            this.txtICModel.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtICModel.Size = new System.Drawing.Size(263, 34);
            // 
            // 
            // 
            this.txtICModel.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtICModel.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtICModel.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtICModel.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtICModel.SkinTxt.Multiline = true;
            this.txtICModel.SkinTxt.Name = "BaseText";
            this.txtICModel.SkinTxt.ReadOnly = true;
            this.txtICModel.SkinTxt.Size = new System.Drawing.Size(249, 20);
            this.txtICModel.SkinTxt.TabIndex = 0;
            this.txtICModel.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtICModel.SkinTxt.WaterText = "";
            this.txtICModel.TabIndex = 164;
            this.txtICModel.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtICModel.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtICModel.WaterText = "";
            this.txtICModel.WordWrap = true;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(15, 190);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 34);
            this.label2.TabIndex = 163;
            this.label2.Text = "IC Model:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtICBrand
            // 
            this.txtICBrand.BackColor = System.Drawing.Color.Transparent;
            this.txtICBrand.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtICBrand.DownBack = null;
            this.txtICBrand.Icon = null;
            this.txtICBrand.IconIsButton = false;
            this.txtICBrand.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtICBrand.IsPasswordChat = '\0';
            this.txtICBrand.IsSystemPasswordChar = false;
            this.txtICBrand.Lines = new string[0];
            this.txtICBrand.Location = new System.Drawing.Point(101, 145);
            this.txtICBrand.Margin = new System.Windows.Forms.Padding(0);
            this.txtICBrand.MaxLength = 32767;
            this.txtICBrand.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtICBrand.MouseBack = null;
            this.txtICBrand.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtICBrand.Multiline = true;
            this.txtICBrand.Name = "txtICBrand";
            this.txtICBrand.NormlBack = null;
            this.txtICBrand.Padding = new System.Windows.Forms.Padding(5);
            this.txtICBrand.ReadOnly = true;
            this.txtICBrand.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtICBrand.Size = new System.Drawing.Size(263, 34);
            // 
            // 
            // 
            this.txtICBrand.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtICBrand.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtICBrand.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtICBrand.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtICBrand.SkinTxt.Multiline = true;
            this.txtICBrand.SkinTxt.Name = "BaseText";
            this.txtICBrand.SkinTxt.ReadOnly = true;
            this.txtICBrand.SkinTxt.Size = new System.Drawing.Size(249, 20);
            this.txtICBrand.SkinTxt.TabIndex = 0;
            this.txtICBrand.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtICBrand.SkinTxt.WaterText = "";
            this.txtICBrand.TabIndex = 162;
            this.txtICBrand.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtICBrand.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtICBrand.WaterText = "";
            this.txtICBrand.WordWrap = true;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(15, 145);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 34);
            this.label3.TabIndex = 161;
            this.label3.Text = "IC Brand:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.Control;
            this.label4.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(15, 279);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 29);
            this.label4.TabIndex = 299;
            this.label4.Text = "Lot Input:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtChecksum
            // 
            this.txtChecksum.BackColor = System.Drawing.Color.Transparent;
            this.txtChecksum.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtChecksum.DownBack = null;
            this.txtChecksum.Icon = null;
            this.txtChecksum.IconIsButton = false;
            this.txtChecksum.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtChecksum.IsPasswordChat = '\0';
            this.txtChecksum.IsSystemPasswordChar = false;
            this.txtChecksum.Lines = new string[0];
            this.txtChecksum.Location = new System.Drawing.Point(101, 235);
            this.txtChecksum.Margin = new System.Windows.Forms.Padding(0);
            this.txtChecksum.MaxLength = 32767;
            this.txtChecksum.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtChecksum.MouseBack = null;
            this.txtChecksum.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtChecksum.Multiline = true;
            this.txtChecksum.Name = "txtChecksum";
            this.txtChecksum.NormlBack = null;
            this.txtChecksum.Padding = new System.Windows.Forms.Padding(5);
            this.txtChecksum.ReadOnly = true;
            this.txtChecksum.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtChecksum.Size = new System.Drawing.Size(263, 34);
            // 
            // 
            // 
            this.txtChecksum.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtChecksum.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtChecksum.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtChecksum.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtChecksum.SkinTxt.Multiline = true;
            this.txtChecksum.SkinTxt.Name = "BaseText";
            this.txtChecksum.SkinTxt.ReadOnly = true;
            this.txtChecksum.SkinTxt.Size = new System.Drawing.Size(249, 20);
            this.txtChecksum.SkinTxt.TabIndex = 0;
            this.txtChecksum.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtChecksum.SkinTxt.WaterText = "";
            this.txtChecksum.TabIndex = 298;
            this.txtChecksum.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtChecksum.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtChecksum.WaterText = "";
            this.txtChecksum.WordWrap = true;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.Control;
            this.label5.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(15, 235);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 34);
            this.label5.TabIndex = 297;
            this.label5.Text = "Checksum:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudExpectInput
            // 
            this.nudExpectInput.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nudExpectInput.Location = new System.Drawing.Point(101, 280);
            this.nudExpectInput.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudExpectInput.Name = "nudExpectInput";
            this.nudExpectInput.ReadOnly = true;
            this.nudExpectInput.Size = new System.Drawing.Size(130, 29);
            this.nudExpectInput.TabIndex = 300;
            // 
            // nudExpectOuput
            // 
            this.nudExpectOuput.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nudExpectOuput.Location = new System.Drawing.Point(101, 320);
            this.nudExpectOuput.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudExpectOuput.Name = "nudExpectOuput";
            this.nudExpectOuput.ReadOnly = true;
            this.nudExpectOuput.Size = new System.Drawing.Size(130, 29);
            this.nudExpectOuput.TabIndex = 302;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.Control;
            this.label6.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(15, 319);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 29);
            this.label6.TabIndex = 301;
            this.label6.Text = "Lot Ouput:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudNGInput
            // 
            this.nudNGInput.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nudNGInput.Location = new System.Drawing.Point(101, 360);
            this.nudNGInput.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudNGInput.Name = "nudNGInput";
            this.nudNGInput.ReadOnly = true;
            this.nudNGInput.Size = new System.Drawing.Size(130, 29);
            this.nudNGInput.TabIndex = 304;
            this.nudNGInput.Visible = false;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.SystemColors.Control;
            this.label7.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(15, 359);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 29);
            this.label7.TabIndex = 303;
            this.label7.Text = "NG Ouput:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label7.Visible = false;
            // 
            // txtOperator
            // 
            this.txtOperator.BackColor = System.Drawing.Color.Transparent;
            this.txtOperator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtOperator.DownBack = null;
            this.txtOperator.Icon = null;
            this.txtOperator.IconIsButton = false;
            this.txtOperator.IconMouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtOperator.IsPasswordChat = '\0';
            this.txtOperator.IsSystemPasswordChar = false;
            this.txtOperator.Lines = new string[0];
            this.txtOperator.Location = new System.Drawing.Point(101, 400);
            this.txtOperator.Margin = new System.Windows.Forms.Padding(0);
            this.txtOperator.MaxLength = 32767;
            this.txtOperator.MinimumSize = new System.Drawing.Size(28, 28);
            this.txtOperator.MouseBack = null;
            this.txtOperator.MouseState = CCWin.SkinClass.ControlState.Normal;
            this.txtOperator.Multiline = true;
            this.txtOperator.Name = "txtOperator";
            this.txtOperator.NormlBack = null;
            this.txtOperator.Padding = new System.Windows.Forms.Padding(5);
            this.txtOperator.ReadOnly = false;
            this.txtOperator.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtOperator.Size = new System.Drawing.Size(263, 34);
            // 
            // 
            // 
            this.txtOperator.SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOperator.SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOperator.SkinTxt.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtOperator.SkinTxt.Location = new System.Drawing.Point(5, 5);
            this.txtOperator.SkinTxt.Multiline = true;
            this.txtOperator.SkinTxt.Name = "BaseText";
            this.txtOperator.SkinTxt.Size = new System.Drawing.Size(249, 20);
            this.txtOperator.SkinTxt.TabIndex = 0;
            this.txtOperator.SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtOperator.SkinTxt.WaterText = "";
            this.txtOperator.TabIndex = 300;
            this.txtOperator.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtOperator.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.txtOperator.WaterText = "";
            this.txtOperator.WordWrap = true;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.Control;
            this.label8.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(15, 400);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(86, 34);
            this.label8.TabIndex = 299;
            this.label8.Text = "Operator:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TicketsWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(384, 532);
            this.Controls.Add(this.txtOperator);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.nudNGInput);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.nudExpectOuput);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.nudExpectInput);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtChecksum);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtICModel);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtICBrand);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtWorkNo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudLotNum);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtCustomer);
            this.Controls.Add(this.label9);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TicketsWnd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Work information";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TicketsWnd_FormClosing);
            this.Load += new System.EventHandler(this.TicketsWnd_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudLotNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudExpectInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudExpectOuput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNGInput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinTextBox txtCustomer;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nudLotNum;
        private System.Windows.Forms.Label label11;
        private CCWin.SkinControl.SkinTextBox txtWorkNo;
        private System.Windows.Forms.Label label1;
        private CCWin.SkinControl.SkinButton btnCancel;
        private CCWin.SkinControl.SkinButton btnOK;
        private CCWin.SkinControl.SkinTextBox txtICModel;
        private System.Windows.Forms.Label label2;
        private CCWin.SkinControl.SkinTextBox txtICBrand;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private CCWin.SkinControl.SkinTextBox txtChecksum;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudExpectInput;
        private System.Windows.Forms.NumericUpDown nudExpectOuput;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudNGInput;
        private System.Windows.Forms.Label label7;
        private CCWin.SkinControl.SkinTextBox txtOperator;
        private System.Windows.Forms.Label label8;
    }
}