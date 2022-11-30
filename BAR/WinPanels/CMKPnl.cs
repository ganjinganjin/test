using BAR.Commonlib;
using BAR.CommonLib_v1._0;
using BAR.Windows;
using CCWin.SkinClass;
using PLC;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BAR.ControlPanels
{
    public partial class CMKPnl : UserControl
    {
        ConfigInitWnd _ConInitWnd;
        string[] strAxisType, strDisplayType;
        double Pos_X, Pos_Y, Pos_Z;
        int AxisIndex;
        string AxisType;
        int DisplayType;
        //TrapPrm tempTrapPrm;
        public CMKPnl()
        {
            MultiLanguage.SetDefaultLanguage();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
            InitializeComponent();
            strAxisType = new string[]
            {
                "X","Y","Z1"
            };
            strDisplayType = new string[]
            {
                "pulse","mm"
            };
        }

        public CMKPnl(ConfigInitWnd conInitWnd) : this()
        {
            this._ConInitWnd = conInitWnd;
        }

        private void CMKPnl_Load(object sender, EventArgs e)
        {
            AddCboItems(CboAxisType, strAxisType, 0);
            AddCboItems(CboDisplayType, strDisplayType, 0);
        }

        /// <summary>
        /// 添加下拉框选项
        /// </summary>
        /// <param name="comboBox">下拉框</param>
        /// <param name="str">选项组</param>
        /// <param name="select">选择项</param>
        private void AddCboItems(ComboBox comboBox, string[] str, int select)
        {
            comboBox.Items.Clear();
            foreach (string sName in str)
            {
                comboBox.Items.Add(sName);
            }
            comboBox.SelectedIndex = select;
        }

        private void CboAxisType_SelectedValueChanged(object sender, EventArgs e)
        {
            AxisIndex = CboAxisType.SelectedIndex;
            AxisType = CboAxisType.SelectedText;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Pos_X = Axis.trapPrm_X.getPos;
            Pos_Y = Axis.trapPrm_Y.getPos;
            Pos_Z = Axis.trapPrm_Z[0].getPos;
            NudPos_X.Value = Convert.ToDecimal(Pos_X);
            NudPos_Y.Value = Convert.ToDecimal(Pos_Y);
            NudPos_Z.Value = Convert.ToDecimal(Pos_Z);
        }

        private void CboDisplayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayType = CboDisplayType.SelectedIndex;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string strShow;
            if (Auto_Flag.LearnBusy)
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Parameter modification is not allowed in the teaching process." : "示教流程中，不允许修改参数";
                MessageBox.Show(strShow, "Information：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Whether to measure the " + AxisType + "-axis" : "是否测量" + AxisType +"轴";
            if (DialogResult.No == MessageBox.Show(strShow, "Information：", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
            {
                return;
            }
            if (Pos_X == 0 && Pos_Y == 0 && Pos_Z == 0)//无坐标值
            {
                strShow = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "Measuring position coordinates cannot be empty!" : "测量位置坐标不能为空！！！";
                MessageBox.Show(strShow, "Information：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            TeachAction.CMK_Start(Pos_X, Pos_Y, Pos_Z, AxisIndex, NudCount.Value.ToInt32());
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            TeachAction.CMK_Stop();
        }

        /// <summary>
        /// 实时显示轴读数
        /// </summary>
        public void Display()
        {
            if (DisplayType == 0)
            {
                LabPos_X.Text = string.Format("{0:f0}", Axis.trapPrm_X.getEncPosPul);
                LabPos_Y.Text = string.Format("{0:f0}", Axis.trapPrm_Y.getEncPosPul);
                LabPos_Z.Text = string.Format("{0:f0}", Axis.trapPrm_Z[0].getPosPul);
            }
            else
            {
                LabPos_X.Text = string.Format("{0:f3}", Axis.trapPrm_X.getEncPos);
                LabPos_Y.Text = string.Format("{0:f3}", Axis.trapPrm_Y.getEncPos);
                LabPos_Z.Text = string.Format("{0:f3}", Axis.trapPrm_Z[0].getPos);
            }
        }
    }
}
