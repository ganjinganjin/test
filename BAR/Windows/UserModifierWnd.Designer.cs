namespace BAR.Windows
{
    partial class UserModifierWnd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserModifierWnd));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textUserName = new System.Windows.Forms.TextBox();
            this.textUserPass = new System.Windows.Forms.TextBox();
            this.btnModify = new System.Windows.Forms.Button();
            this.textUserCoPass = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // textUserName
            // 
            resources.ApplyResources(this.textUserName, "textUserName");
            this.textUserName.Name = "textUserName";
            this.textUserName.ReadOnly = true;
            this.textUserName.TabStop = false;
            // 
            // textUserPass
            // 
            resources.ApplyResources(this.textUserPass, "textUserPass");
            this.textUserPass.Name = "textUserPass";
            // 
            // btnModify
            // 
            resources.ApplyResources(this.btnModify, "btnModify");
            this.btnModify.Name = "btnModify";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // textUserCoPass
            // 
            resources.ApplyResources(this.textUserCoPass, "textUserCoPass");
            this.textUserCoPass.Name = "textUserCoPass";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // UserModifierWnd
            // 
            this.AcceptButton = this.btnModify;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textUserCoPass);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnModify);
            this.Controls.Add(this.textUserPass);
            this.Controls.Add(this.textUserName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserModifierWnd";
            this.ShowInTaskbar = false;
            this.Activated += new System.EventHandler(this.UserModifierWnd_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UserModifierWnd_FormClosing);
            this.Load += new System.EventHandler(this.UserModifierWnd_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textUserName;
        private System.Windows.Forms.TextBox textUserPass;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.TextBox textUserCoPass;
        private System.Windows.Forms.Label label3;
    }
}