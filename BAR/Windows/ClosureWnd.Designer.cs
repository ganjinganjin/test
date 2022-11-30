namespace BAR.Windows
{
    partial class ClosureWnd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClosureWnd));
            this.btnNo = new CCWin.SkinControl.SkinButton();
            this.btnYes = new CCWin.SkinControl.SkinButton();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSure = new CCWin.SkinControl.SkinButton();
            this.SuspendLayout();
            // 
            // btnNo
            // 
            resources.ApplyResources(this.btnNo, "btnNo");
            this.btnNo.BackColor = System.Drawing.Color.Transparent;
            this.btnNo.BaseColor = System.Drawing.Color.Red;
            this.btnNo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(77)))), ((int)(((byte)(0)))));
            this.btnNo.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnNo.DownBack = null;
            this.btnNo.DownBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnNo.ForeColor = System.Drawing.Color.White;
            this.btnNo.IsDrawGlass = false;
            this.btnNo.MouseBack = null;
            this.btnNo.MouseBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnNo.Name = "btnNo";
            this.btnNo.NormlBack = null;
            this.btnNo.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // btnYes
            // 
            resources.ApplyResources(this.btnYes, "btnYes");
            this.btnYes.BackColor = System.Drawing.Color.Transparent;
            this.btnYes.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnYes.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnYes.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnYes.DownBack = null;
            this.btnYes.DownBaseColor = System.Drawing.Color.Green;
            this.btnYes.ForeColor = System.Drawing.Color.White;
            this.btnYes.IsDrawGlass = false;
            this.btnYes.MouseBack = null;
            this.btnYes.MouseBaseColor = System.Drawing.Color.Lime;
            this.btnYes.Name = "btnYes";
            this.btnYes.NormlBack = null;
            this.btnYes.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnYes.UseVisualStyleBackColor = false;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnSure
            // 
            resources.ApplyResources(this.btnSure, "btnSure");
            this.btnSure.BackColor = System.Drawing.Color.Transparent;
            this.btnSure.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnSure.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnSure.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnSure.DownBack = null;
            this.btnSure.DownBaseColor = System.Drawing.Color.Green;
            this.btnSure.ForeColor = System.Drawing.Color.White;
            this.btnSure.IsDrawGlass = false;
            this.btnSure.MouseBack = null;
            this.btnSure.MouseBaseColor = System.Drawing.Color.Lime;
            this.btnSure.Name = "btnSure";
            this.btnSure.NormlBack = null;
            this.btnSure.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnSure.UseVisualStyleBackColor = false;
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // ClosureWnd
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ControlBox = false;
            this.Controls.Add(this.btnSure);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ClosureWnd";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClosureWnd_FormClosing);
            this.Load += new System.EventHandler(this.ClosureWnd_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CCWin.SkinControl.SkinButton btnNo;
        private CCWin.SkinControl.SkinButton btnYes;
        private System.Windows.Forms.Label label1;
        private CCWin.SkinControl.SkinButton btnSure;
    }
}