namespace BAR.Windows
{
    partial class AlarmRecWnd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AlarmRecWnd));
            this.panel1 = new System.Windows.Forms.Panel();
            this.alarmList = new System.Windows.Forms.ListView();
            this.tabBtn1 = new CCWin.SkinControl.SkinButton();
            this.tabBtn2 = new CCWin.SkinControl.SkinButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.alarmList);
            this.panel1.Name = "panel1";
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
            // tabBtn1
            // 
            resources.ApplyResources(this.tabBtn1, "tabBtn1");
            this.tabBtn1.BackColor = System.Drawing.Color.Transparent;
            this.tabBtn1.BaseColor = System.Drawing.Color.Gray;
            this.tabBtn1.BorderColor = System.Drawing.Color.Black;
            this.tabBtn1.BorderInflate = new System.Drawing.Size(0, 0);
            this.tabBtn1.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.tabBtn1.DownBack = null;
            this.tabBtn1.DownBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(56)))), ((int)(((byte)(56)))));
            this.tabBtn1.FadeGlow = false;
            this.tabBtn1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.tabBtn1.IsDrawGlass = false;
            this.tabBtn1.MouseBack = null;
            this.tabBtn1.MouseBaseColor = System.Drawing.Color.DimGray;
            this.tabBtn1.Name = "tabBtn1";
            this.tabBtn1.NormlBack = null;
            this.tabBtn1.UseVisualStyleBackColor = false;
            this.tabBtn1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tabBtn_MouseUp);
            // 
            // tabBtn2
            // 
            resources.ApplyResources(this.tabBtn2, "tabBtn2");
            this.tabBtn2.BackColor = System.Drawing.Color.Transparent;
            this.tabBtn2.BaseColor = System.Drawing.Color.Gray;
            this.tabBtn2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.tabBtn2.BorderInflate = new System.Drawing.Size(0, 0);
            this.tabBtn2.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.tabBtn2.DownBack = null;
            this.tabBtn2.DownBaseColor = System.Drawing.Color.DimGray;
            this.tabBtn2.FadeGlow = false;
            this.tabBtn2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.tabBtn2.IsDrawGlass = false;
            this.tabBtn2.MouseBack = null;
            this.tabBtn2.MouseBaseColor = System.Drawing.Color.DimGray;
            this.tabBtn2.Name = "tabBtn2";
            this.tabBtn2.NormlBack = null;
            this.tabBtn2.UseVisualStyleBackColor = false;
            this.tabBtn2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tabBtn_MouseUp);
            // 
            // AlarmRecWnd
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabBtn2);
            this.Controls.Add(this.tabBtn1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "AlarmRecWnd";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.AlarmRecWnd_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private CCWin.SkinControl.SkinButton tabBtn1;
        private CCWin.SkinControl.SkinButton tabBtn2;
        private System.Windows.Forms.ListView alarmList;
    }
}