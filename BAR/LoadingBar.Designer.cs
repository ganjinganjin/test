namespace BAR
{
    partial class LoadingBar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadingBar));
            this.infoLab = new System.Windows.Forms.Label();
            this.loadBar = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.loadBar)).BeginInit();
            this.SuspendLayout();
            // 
            // infoLab
            // 
            resources.ApplyResources(this.infoLab, "infoLab");
            this.infoLab.ForeColor = System.Drawing.Color.DarkGoldenrod;
            this.infoLab.Name = "infoLab";
            // 
            // loadBar
            // 
            resources.ApplyResources(this.loadBar, "loadBar");
            this.loadBar.Image = global::BAR.Properties.Resources._5_121204193R1_50;
            this.loadBar.Name = "loadBar";
            this.loadBar.TabStop = false;
            // 
            // LoadingBar
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.loadBar);
            this.Controls.Add(this.infoLab);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoadingBar";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoadingBar_FormClosing);
            this.Load += new System.EventHandler(this.LoadingBar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.loadBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label infoLab;
        private System.Windows.Forms.PictureBox loadBar;
    }
}