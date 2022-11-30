using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BAR.Windows;
using CCWin.SkinControl;
using BAR.Commonlib.Connectors;
using BAR.Commonlib;

namespace BAR.ControlPanels
{
    public partial class ICShapePnl : UserControl
    {
        ConfigInitWnd _ConInitWnd;
        Config g_config = Config.GetInstance();
        Act g_act = Act.GetInstance();
        public ICShapePnl()
        {
            InitializeComponent();
            _InitializeComponent();
        }

        public ICShapePnl(ConfigInitWnd configWnd) : this()
        {
            this._ConInitWnd = configWnd;
        }
        
        private void _InitializeComponent()
        {
            //吸笔
            for (int i = 0; i < UserConfig.VacuumPenC; i++)
            {
                String str = "吸笔" + (i + 1);
                this.penCBox.Items.Add(str);
            }
            penCBox.SelectedIndex = 0;

            TxtLocation.Text = string.Format("X：{0:f2} Y：{1:f2}\r\nZ：{2:f2}", Vision_3D.X, Vision_3D.Y, Vision_3D.Z);
        }
        
        private void ICShapePnl_Load(object sender, EventArgs e)
        {
            if (Vision_3D.ICType == 1)
            {
                RbtnBGA.Checked = true;
            }
            else if (Vision_3D.ICType == 2)
            {
                RbtnSOP.Checked = true;
            }
            else
            {
                RbtnQFP.Checked = true;
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            BAR.tcpClient_3D.Connect(Vision_3D.IP, Vision_3D.Port);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            BAR.tcpClient_3D.Send("A");
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            BAR.tcpClient_3D.DisConnect();
        }
        
        private void BtnGoLocation_Click(object sender, EventArgs e)
        {
            double setx, sety;
            if (!Auto_Flag.SystemBusy)
            {
                setx = Vision_3D.X;
                sety = Vision_3D.Y;
                setx -= Axis.Pen[penCBox.SelectedIndex].Offset_TopCamera_X;
                sety -= Axis.Pen[penCBox.SelectedIndex].Offset_TopCamera_Y;
                TeachAction.GO_Start(setx, sety);
            }
        }

        private void BtnSaveLocation_Click(object sender, EventArgs e)
        {
            Vision_3D.X = Axis.trapPrm_X.getPos + Axis.Pen[penCBox.SelectedIndex].Offset_TopCamera_X;
            Vision_3D.Y = Axis.trapPrm_Y.getPos + Axis.Pen[penCBox.SelectedIndex].Offset_TopCamera_Y;
            Vision_3D.Z = Axis.trapPrm_Z[penCBox.SelectedIndex].getPos;
            TxtLocation.Text = string.Format("X：{0:f2} Y：{1:f2}\r\nZ：{2:f2}", Vision_3D.X, Vision_3D.Y, Vision_3D.Z);
            g_config.Save3DPar();
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "保存2.5D拍照位置成功", "Modify");
        }

        private void RbtnQFP_Click(object sender, EventArgs e)
        {
            Vision_3D.ICType = 0;
            g_config.Save3DPar();
        }

        private void RbtnBGA_Click(object sender, EventArgs e)
        {
            Vision_3D.ICType = 1;
            g_config.Save3DPar();
        }

        private void RbtnSOP_Click(object sender, EventArgs e)
        {
            Vision_3D.ICType = 2;
            g_config.Save3DPar();
        }

        private void BtnGo3DCamera_Click(object sender, EventArgs e)
        {
            int pen = penCBox.SelectedIndex;
            TeachAction.Detection_Start(pen);
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            if (Auto_Flag.LearnBusy && (Auto_Flag.ALarmPause || Auto_Flag.RunPause))
            {
                Auto_Flag.Next = true;
            }
        }

        private void BtnLayIC_Click(object sender, EventArgs e)
        {
            if (Auto_Flag.LearnReady)
            {
                Button.replaceIC = true;
            }
        }

        private void BtnRotateIC_Click(object sender, EventArgs e)
        {
            if (Auto_Flag.LearnReady)
            {
                Button.rotateIC = true;
            }
        }

        private void btnLight1_Click(object sender, EventArgs e)
        {
            In_Output._3DLightO[0].M = !In_Output._3DLightO[0].M;
        }

        private void btnLight2_Click(object sender, EventArgs e)
        {
            In_Output._3DLightO[1].M = !In_Output._3DLightO[1].M;
        }
    }
}
