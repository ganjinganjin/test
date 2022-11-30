using BAR.Commonlib;
using BAR.CommonLib_v1._0;
using CCWin.SkinClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BAR.Windows
{
    public partial class TicketsWnd : Form
    {
        public Act g_act = Act.GetInstance();

        public TicketsWnd()
        {
            InitializeComponent();
        }

        private void TicketsWnd_Load(object sender, EventArgs e)
        {
            _InitUI();
        }

        public void _InitUI()
        {
            g_act.ReadLotInfo();
            txtCustomer.Text = Mes.Customer;
            txtWorkNo.Text = Mes.LotSN;
            nudLotNum.Value = Mes.Count;
            txtICBrand.Text = Mes.Brand;
            txtICModel.Text = Mes.Device;
            txtChecksum.Text = Mes.Checksum_File;
            nudExpectInput.Value = Mes.TIC_DoneC;
            nudExpectOuput.Value = Mes.OKDoneC;
            txtOperator.Text = Mes.OperatorID;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Auto_Flag.AutoRunBusy)
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "设备运行中,不允许修改", "Tickets");
                return;
            }
            Mes.LotSN = txtWorkNo.Text;
            Auto_Flag.RemoteStart = false;
            if (nudLotNum.Value.ToInt32() == 0)
            {
                MessageBox.Show(MultiLanguage.GetString("工单批量数不能为零"), "Information：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (g_act.ReadLotInfo())
            {
                if ((Mes.Exit == 1 && Mes.OKDoneC >= Mes.Count) || (Mes.Exit == 2 && Mes.TIC_DoneC >= Mes.Count))
                {
                    MessageBox.Show("工单[" + Mes.LotSN + "]已完成,请添加新工单", "Information：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (Mes.Count != Convert.ToInt32(nudLotNum.Value))
                {
                    if ((Mes.Exit == 1 && Mes.OKDoneC >= Convert.ToInt32(nudLotNum.Value)) || (Mes.Exit == 2 && Mes.TIC_DoneC >= Convert.ToInt32(nudLotNum.Value)))
                    {
                        MessageBox.Show("存在未完成工单,且工单批量数小于已完成数", "Error：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    DialogResult result = MessageBox.Show("存在未完成工单[" + Mes.LotSN + "]\r\n\r\n是否将批量数由 " + Mes.Count +" 更改为 " + nudLotNum.Value.ToString() + " ?", "提示：", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (DialogResult.No == result)
                    {
                        nudLotNum.Value = Mes.Count;
                    }
                }
                Mes.Count = UserTask.TargetC = Convert.ToInt32(nudLotNum.Value) == 0 ? 2000 : Convert.ToInt32(nudLotNum.Value);
                UserTask.TIC_DoneC = Mes.TIC_DoneC;
                UserTask.OKDoneC = Mes.OKDoneC;
                if (Mes.Exit == 0)
                {
                    Auto_Flag.Production = false;
                }
                else
                {
                    Auto_Flag.Production = true;
                    Auto_Flag.ProductionOK = Mes.Exit == 1 ? true : false;
                }
            }
            else//无工单记录
            {
                Auto_Flag.Production = true;
                Auto_Flag.ProductionOK = true;
                Mes.Count = UserTask.TargetC = Convert.ToInt32(nudLotNum.Value) == 0 ? 2000 : Convert.ToInt32(nudLotNum.Value);
                Mes.TIC_DoneC = UserTask.TIC_DoneC = 0;
                Mes.OKDoneC = UserTask.OKDoneC = 0;
            }
            Auto_Flag.RemoteStart = true;
            Mes.Customer = txtCustomer.Text;
            //Mes.Brand = txtICBrand.Text;
            //Mes.Device = txtICModel.Text;
            //Mes.Checksum_File = txtChecksum.Text;
            Mes.OperatorID = txtOperator.Text;
            g_act.WriteLotInfo(true);
            _InitUI();
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "保存工单信息成功", "Tickets");
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TicketsWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
