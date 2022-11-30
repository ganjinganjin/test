namespace BAR.Windows
{
    partial class MarkManageWnd
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
            this.productListView = new System.Windows.Forms.ListView();
            this.selectProBtn = new CCWin.SkinControl.SkinButton();
            this.addProBtn = new CCWin.SkinControl.SkinButton();
            this.delProBtn = new CCWin.SkinControl.SkinButton();
            this.label16 = new System.Windows.Forms.Label();
            this.inputPnl = new System.Windows.Forms.Panel();
            this.NudRoldis = new System.Windows.Forms.NumericUpDown();
            this.NudColdis = new System.Windows.Forms.NumericUpDown();
            this.NudRol = new System.Windows.Forms.NumericUpDown();
            this.NudCol = new System.Windows.Forms.NumericUpDown();
            this.NudOffset_Y = new System.Windows.Forms.NumericUpDown();
            this.NudOffset_X = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cancelBtn = new CCWin.SkinControl.SkinButton();
            this.inputOKBtn = new CCWin.SkinControl.SkinButton();
            this.prodNameTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.modifyProBtn = new CCWin.SkinControl.SkinButton();
            this.inputPnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NudRoldis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudColdis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudRol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudCol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudOffset_Y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudOffset_X)).BeginInit();
            this.SuspendLayout();
            // 
            // productListView
            // 
            this.productListView.Font = new System.Drawing.Font("宋体", 15F);
            this.productListView.ForeColor = System.Drawing.SystemColors.WindowText;
            this.productListView.FullRowSelect = true;
            this.productListView.GridLines = true;
            this.productListView.HideSelection = false;
            this.productListView.Location = new System.Drawing.Point(12, 50);
            this.productListView.Name = "productListView";
            this.productListView.Size = new System.Drawing.Size(370, 420);
            this.productListView.TabIndex = 0;
            this.productListView.UseCompatibleStateImageBehavior = false;
            this.productListView.View = System.Windows.Forms.View.Details;
            // 
            // selectProBtn
            // 
            this.selectProBtn.BackColor = System.Drawing.Color.Transparent;
            this.selectProBtn.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(235)))));
            this.selectProBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0)))));
            this.selectProBtn.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.selectProBtn.DownBack = null;
            this.selectProBtn.DownBaseColor = System.Drawing.Color.Gray;
            this.selectProBtn.Font = new System.Drawing.Font("楷体", 10F, System.Drawing.FontStyle.Bold);
            this.selectProBtn.ForeColor = System.Drawing.Color.Black;
            this.selectProBtn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.selectProBtn.IsDrawGlass = false;
            this.selectProBtn.Location = new System.Drawing.Point(391, 50);
            this.selectProBtn.MouseBack = null;
            this.selectProBtn.Name = "selectProBtn";
            this.selectProBtn.NormlBack = null;
            this.selectProBtn.Size = new System.Drawing.Size(135, 35);
            this.selectProBtn.TabIndex = 214;
            this.selectProBtn.Text = "选择型号";
            this.selectProBtn.UseVisualStyleBackColor = false;
            this.selectProBtn.Click += new System.EventHandler(this.selectProBtn_Click);
            // 
            // addProBtn
            // 
            this.addProBtn.BackColor = System.Drawing.Color.Transparent;
            this.addProBtn.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(235)))));
            this.addProBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0)))));
            this.addProBtn.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.addProBtn.DownBack = null;
            this.addProBtn.DownBaseColor = System.Drawing.Color.Gray;
            this.addProBtn.Font = new System.Drawing.Font("楷体", 10F, System.Drawing.FontStyle.Bold);
            this.addProBtn.ForeColor = System.Drawing.Color.Black;
            this.addProBtn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.addProBtn.IsDrawGlass = false;
            this.addProBtn.Location = new System.Drawing.Point(391, 101);
            this.addProBtn.MouseBack = null;
            this.addProBtn.Name = "addProBtn";
            this.addProBtn.NormlBack = null;
            this.addProBtn.Size = new System.Drawing.Size(135, 35);
            this.addProBtn.TabIndex = 215;
            this.addProBtn.Text = "添加型号";
            this.addProBtn.UseVisualStyleBackColor = false;
            this.addProBtn.Click += new System.EventHandler(this.addProBtn_Click);
            // 
            // delProBtn
            // 
            this.delProBtn.BackColor = System.Drawing.Color.Transparent;
            this.delProBtn.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(235)))));
            this.delProBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0)))));
            this.delProBtn.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.delProBtn.DownBack = null;
            this.delProBtn.DownBaseColor = System.Drawing.Color.Gray;
            this.delProBtn.Font = new System.Drawing.Font("楷体", 10F, System.Drawing.FontStyle.Bold);
            this.delProBtn.ForeColor = System.Drawing.Color.Black;
            this.delProBtn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.delProBtn.IsDrawGlass = false;
            this.delProBtn.Location = new System.Drawing.Point(391, 203);
            this.delProBtn.MouseBack = null;
            this.delProBtn.Name = "delProBtn";
            this.delProBtn.NormlBack = null;
            this.delProBtn.Size = new System.Drawing.Size(135, 35);
            this.delProBtn.TabIndex = 216;
            this.delProBtn.Text = "删除型号";
            this.delProBtn.UseVisualStyleBackColor = false;
            this.delProBtn.Click += new System.EventHandler(this.delProBtn_Click);
            // 
            // label16
            // 
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label16.Font = new System.Drawing.Font("楷体", 12F, System.Drawing.FontStyle.Bold);
            this.label16.ForeColor = System.Drawing.Color.Black;
            this.label16.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label16.Location = new System.Drawing.Point(12, 23);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(370, 24);
            this.label16.TabIndex = 271;
            this.label16.Text = "当前型号列表";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // inputPnl
            // 
            this.inputPnl.Controls.Add(this.NudRoldis);
            this.inputPnl.Controls.Add(this.NudColdis);
            this.inputPnl.Controls.Add(this.NudRol);
            this.inputPnl.Controls.Add(this.NudCol);
            this.inputPnl.Controls.Add(this.NudOffset_Y);
            this.inputPnl.Controls.Add(this.NudOffset_X);
            this.inputPnl.Controls.Add(this.label6);
            this.inputPnl.Controls.Add(this.label7);
            this.inputPnl.Controls.Add(this.label4);
            this.inputPnl.Controls.Add(this.label5);
            this.inputPnl.Controls.Add(this.label3);
            this.inputPnl.Controls.Add(this.label2);
            this.inputPnl.Controls.Add(this.cancelBtn);
            this.inputPnl.Controls.Add(this.inputOKBtn);
            this.inputPnl.Controls.Add(this.prodNameTxt);
            this.inputPnl.Controls.Add(this.label1);
            this.inputPnl.Location = new System.Drawing.Point(59, 51);
            this.inputPnl.Name = "inputPnl";
            this.inputPnl.Size = new System.Drawing.Size(421, 385);
            this.inputPnl.TabIndex = 272;
            this.inputPnl.Visible = false;
            // 
            // NudRoldis
            // 
            this.NudRoldis.DecimalPlaces = 2;
            this.NudRoldis.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NudRoldis.Location = new System.Drawing.Point(219, 266);
            this.NudRoldis.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NudRoldis.Name = "NudRoldis";
            this.NudRoldis.Size = new System.Drawing.Size(76, 26);
            this.NudRoldis.TabIndex = 228;
            // 
            // NudColdis
            // 
            this.NudColdis.DecimalPlaces = 2;
            this.NudColdis.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NudColdis.Location = new System.Drawing.Point(219, 234);
            this.NudColdis.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NudColdis.Name = "NudColdis";
            this.NudColdis.Size = new System.Drawing.Size(76, 26);
            this.NudColdis.TabIndex = 227;
            // 
            // NudRol
            // 
            this.NudRol.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NudRol.Location = new System.Drawing.Point(219, 202);
            this.NudRol.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NudRol.Name = "NudRol";
            this.NudRol.Size = new System.Drawing.Size(76, 26);
            this.NudRol.TabIndex = 226;
            // 
            // NudCol
            // 
            this.NudCol.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NudCol.Location = new System.Drawing.Point(219, 170);
            this.NudCol.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NudCol.Name = "NudCol";
            this.NudCol.Size = new System.Drawing.Size(76, 26);
            this.NudCol.TabIndex = 225;
            // 
            // NudOffset_Y
            // 
            this.NudOffset_Y.DecimalPlaces = 2;
            this.NudOffset_Y.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NudOffset_Y.Location = new System.Drawing.Point(220, 138);
            this.NudOffset_Y.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NudOffset_Y.Name = "NudOffset_Y";
            this.NudOffset_Y.Size = new System.Drawing.Size(76, 26);
            this.NudOffset_Y.TabIndex = 224;
            // 
            // NudOffset_X
            // 
            this.NudOffset_X.DecimalPlaces = 2;
            this.NudOffset_X.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.NudOffset_X.Location = new System.Drawing.Point(220, 106);
            this.NudOffset_X.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.NudOffset_X.Name = "NudOffset_X";
            this.NudOffset_X.Size = new System.Drawing.Size(76, 26);
            this.NudOffset_X.TabIndex = 223;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(71, 267);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(133, 23);
            this.label6.TabIndex = 222;
            this.label6.Text = "料盘行间距:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(71, 235);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(133, 23);
            this.label7.TabIndex = 221;
            this.label7.Text = "料盘列间距:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(71, 203);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 23);
            this.label4.TabIndex = 220;
            this.label4.Text = "料盘行数量:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(71, 171);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 23);
            this.label5.TabIndex = 219;
            this.label5.Text = "料盘列数量:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(71, 139);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 23);
            this.label3.TabIndex = 218;
            this.label3.Text = "料盘Mark点偏移Y:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(71, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 23);
            this.label2.TabIndex = 217;
            this.label2.Text = "料盘Mark点偏移X:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cancelBtn
            // 
            this.cancelBtn.BackColor = System.Drawing.Color.Transparent;
            this.cancelBtn.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(235)))));
            this.cancelBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0)))));
            this.cancelBtn.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.cancelBtn.DownBack = null;
            this.cancelBtn.DownBaseColor = System.Drawing.Color.Gray;
            this.cancelBtn.Font = new System.Drawing.Font("楷体", 10F, System.Drawing.FontStyle.Bold);
            this.cancelBtn.ForeColor = System.Drawing.Color.Black;
            this.cancelBtn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cancelBtn.IsDrawGlass = false;
            this.cancelBtn.Location = new System.Drawing.Point(227, 330);
            this.cancelBtn.MouseBack = null;
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.NormlBack = null;
            this.cancelBtn.Size = new System.Drawing.Size(93, 35);
            this.cancelBtn.TabIndex = 216;
            this.cancelBtn.Text = "取消";
            this.cancelBtn.UseVisualStyleBackColor = false;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // inputOKBtn
            // 
            this.inputOKBtn.BackColor = System.Drawing.Color.Transparent;
            this.inputOKBtn.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(235)))));
            this.inputOKBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0)))));
            this.inputOKBtn.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.inputOKBtn.DownBack = null;
            this.inputOKBtn.DownBaseColor = System.Drawing.Color.Gray;
            this.inputOKBtn.Font = new System.Drawing.Font("楷体", 10F, System.Drawing.FontStyle.Bold);
            this.inputOKBtn.ForeColor = System.Drawing.Color.Black;
            this.inputOKBtn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.inputOKBtn.IsDrawGlass = false;
            this.inputOKBtn.Location = new System.Drawing.Point(91, 330);
            this.inputOKBtn.MouseBack = null;
            this.inputOKBtn.Name = "inputOKBtn";
            this.inputOKBtn.NormlBack = null;
            this.inputOKBtn.Size = new System.Drawing.Size(93, 35);
            this.inputOKBtn.TabIndex = 215;
            this.inputOKBtn.Text = "确定";
            this.inputOKBtn.UseVisualStyleBackColor = false;
            this.inputOKBtn.Click += new System.EventHandler(this.inputOKBtn_Click);
            // 
            // prodNameTxt
            // 
            this.prodNameTxt.Font = new System.Drawing.Font("宋体", 15F);
            this.prodNameTxt.Location = new System.Drawing.Point(18, 57);
            this.prodNameTxt.Name = "prodNameTxt";
            this.prodNameTxt.Size = new System.Drawing.Size(376, 30);
            this.prodNameTxt.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("宋体", 15F);
            this.label1.Location = new System.Drawing.Point(3, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(415, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "请输入新建料盘型号,请勿与已有型号重名";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // modifyProBtn
            // 
            this.modifyProBtn.BackColor = System.Drawing.Color.Transparent;
            this.modifyProBtn.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(235)))));
            this.modifyProBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0)))));
            this.modifyProBtn.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.modifyProBtn.DownBack = null;
            this.modifyProBtn.DownBaseColor = System.Drawing.Color.Gray;
            this.modifyProBtn.Font = new System.Drawing.Font("楷体", 10F, System.Drawing.FontStyle.Bold);
            this.modifyProBtn.ForeColor = System.Drawing.Color.Black;
            this.modifyProBtn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.modifyProBtn.IsDrawGlass = false;
            this.modifyProBtn.Location = new System.Drawing.Point(391, 152);
            this.modifyProBtn.MouseBack = null;
            this.modifyProBtn.Name = "modifyProBtn";
            this.modifyProBtn.NormlBack = null;
            this.modifyProBtn.Size = new System.Drawing.Size(135, 35);
            this.modifyProBtn.TabIndex = 273;
            this.modifyProBtn.Text = "修改型号";
            this.modifyProBtn.UseVisualStyleBackColor = false;
            this.modifyProBtn.Click += new System.EventHandler(this.modifyProBtn_Click);
            // 
            // MarkManageWnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 491);
            this.Controls.Add(this.inputPnl);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.delProBtn);
            this.Controls.Add(this.addProBtn);
            this.Controls.Add(this.selectProBtn);
            this.Controls.Add(this.productListView);
            this.Controls.Add(this.modifyProBtn);
            this.Name = "MarkManageWnd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "料盘型号管理";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProManageWnd_FormClosing);
            this.Load += new System.EventHandler(this.ProManageWnd_Load);
            this.inputPnl.ResumeLayout(false);
            this.inputPnl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NudRoldis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudColdis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudRol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudCol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudOffset_Y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudOffset_X)).EndInit();
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
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown NudRoldis;
        private System.Windows.Forms.NumericUpDown NudColdis;
        private System.Windows.Forms.NumericUpDown NudRol;
        private System.Windows.Forms.NumericUpDown NudCol;
        private System.Windows.Forms.NumericUpDown NudOffset_Y;
        private System.Windows.Forms.NumericUpDown NudOffset_X;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
    }
}