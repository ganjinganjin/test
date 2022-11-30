namespace BAR.Windows
{
    partial class UserLoginWnd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserLoginWnd));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cobLoginName = new System.Windows.Forms.ComboBox();
            this.btnLogin = new CCWin.SkinControl.SkinButton();
            this.btnExit = new CCWin.SkinControl.SkinButton();
            this.skinLabel1 = new CCWin.SkinControl.SkinLabel();
            this.textLoginPass = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.DimGray;
            this.label2.Name = "label2";
            // 
            // cobLoginName
            // 
            this.cobLoginName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cobLoginName, "cobLoginName");
            this.cobLoginName.FormattingEnabled = true;
            this.cobLoginName.Name = "cobLoginName";
            this.cobLoginName.TabStop = false;
            this.cobLoginName.SelectionChangeCommitted += new System.EventHandler(this.cobLoginName_SelectionChangeCommitted);
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.Transparent;
            this.btnLogin.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(112)))), ((int)(((byte)(226)))));
            this.btnLogin.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(112)))), ((int)(((byte)(226)))));
            this.btnLogin.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnLogin.DownBack = null;
            this.btnLogin.FadeGlow = false;
            resources.ApplyResources(this.btnLogin, "btnLogin");
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(112)))), ((int)(((byte)(226)))));
            this.btnLogin.IsDrawBorder = false;
            this.btnLogin.IsDrawGlass = false;
            this.btnLogin.MouseBack = null;
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.NormlBack = null;
            this.btnLogin.Radius = 20;
            this.btnLogin.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(112)))), ((int)(((byte)(226)))));
            this.btnExit.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(112)))), ((int)(((byte)(226)))));
            this.btnExit.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.DownBack = null;
            this.btnExit.FadeGlow = false;
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(112)))), ((int)(((byte)(226)))));
            this.btnExit.IsDrawBorder = false;
            this.btnExit.IsDrawGlass = false;
            this.btnExit.MouseBack = null;
            this.btnExit.Name = "btnExit";
            this.btnExit.NormlBack = null;
            this.btnExit.Radius = 20;
            this.btnExit.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // skinLabel1
            // 
            this.skinLabel1.BackColor = System.Drawing.Color.Transparent;
            this.skinLabel1.BorderColor = System.Drawing.Color.White;
            resources.ApplyResources(this.skinLabel1, "skinLabel1");
            this.skinLabel1.Name = "skinLabel1";
            // 
            // textLoginPass
            // 
            resources.ApplyResources(this.textLoginPass, "textLoginPass");
            this.textLoginPass.Name = "textLoginPass";
            this.textLoginPass.UseSystemPasswordChar = true;
            // 
            // UserLoginWnd
            // 
            this.AcceptButton = this.btnLogin;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(243)))));
            this.CancelButton = this.btnExit;
            this.Controls.Add(this.textLoginPass);
            this.Controls.Add(this.skinLabel1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.cobLoginName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserLoginWnd";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Login_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.LoginForm_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cobLoginName;
        private CCWin.SkinControl.SkinButton btnLogin;
        private CCWin.SkinControl.SkinButton btnExit;
        private CCWin.SkinControl.SkinLabel skinLabel1;
        private System.Windows.Forms.TextBox textLoginPass;
    }
}