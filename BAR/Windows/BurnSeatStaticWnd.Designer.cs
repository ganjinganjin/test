namespace BAR.Windows
{
    partial class BurnSeatStaticWnd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BurnSeatStaticWnd));
            this.scketPosPnl = new CCWin.SkinControl.SkinPanel();
            this.clearBtn1 = new CCWin.SkinControl.SkinButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // scketPosPnl
            // 
            resources.ApplyResources(this.scketPosPnl, "scketPosPnl");
            this.scketPosPnl.BackColor = System.Drawing.Color.Transparent;
            this.scketPosPnl.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.scketPosPnl.DownBack = null;
            this.scketPosPnl.MouseBack = null;
            this.scketPosPnl.Name = "scketPosPnl";
            this.scketPosPnl.NormlBack = null;
            // 
            // clearBtn1
            // 
            resources.ApplyResources(this.clearBtn1, "clearBtn1");
            this.clearBtn1.BackColor = System.Drawing.Color.Transparent;
            this.clearBtn1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(235)))));
            this.clearBtn1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.clearBtn1.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.clearBtn1.DownBack = null;
            this.clearBtn1.DownBaseColor = System.Drawing.Color.Gray;
            this.clearBtn1.ForeColor = System.Drawing.Color.Black;
            this.clearBtn1.IsDrawGlass = false;
            this.clearBtn1.MouseBack = null;
            this.clearBtn1.MouseBaseColor = System.Drawing.Color.Gray;
            this.clearBtn1.Name = "clearBtn1";
            this.clearBtn1.NormlBack = null;
            this.clearBtn1.UseVisualStyleBackColor = false;
            this.clearBtn1.Click += new System.EventHandler(this.clearBtn1_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 800;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // BurnSeatStaticWnd
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.clearBtn1);
            this.Controls.Add(this.scketPosPnl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "BurnSeatStaticWnd";
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BurnSeatStaticWnd_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CCWin.SkinControl.SkinPanel scketPosPnl;
        private CCWin.SkinControl.SkinButton clearBtn1;
        public System.Windows.Forms.Timer timer1;
    }
}