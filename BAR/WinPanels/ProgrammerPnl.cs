using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CCWin.SkinControl;
using BAR.Commonlib;
using static System.Data.Entity.Infrastructure.Design.Executor;
using BAR.CommonLib_v1._0;

namespace BAR.WinPanels
{
    public partial class ProgrammerPnl : UserControl
    {
        private SkinPanel[] scketGroupPanel;
        private SkinButton[] scketGroupBtn;
        private SkinButton[] scketEnableBtn;
        private Label[] scketStateLabel;
        public Config g_config = Config.GetInstance();

        private string strEnable = "☑";//"使用中";
        private string strDisable = "□";//"未启用";

        public ProgrammerPnl()
        {
            InitializeComponent();
            _InitializeComponent();
            UserEvent.enableSeat += new UserEvent.EnableSeat(EnableBtn);
            UserEvent.productChange += new UserEvent.ProductChange(UpdateUI);
        }

        private void _InitializeComponent()
        {
            int left = 0, top = 0, nW = 0, nH = 0, offsetW = 0, offsetH = 0, btnW = 0, btnH = 0;
            float emSize0 = 8.25F, emSize1 = 11F;
            Size size0 = new Size(), size1 = new Size(), size2 = new Size();
            Point point0 = new Point(), point1 = new Point(), point2 = new Point();

            //烧录座子分组控件
            int groupW = 0, groupH = 0, layoutMode = 1;
            int rowC = 1, colC = 2, rowMaxC = 3, colMaxC = 4;//行列数
            if (UserTask.ProgrammerType == GlobConstData.Programmer_AK)
            {
                layoutMode = 2;//1：单行排列，2：双行排列
                colMaxC = 3;
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_DP)
            {
                layoutMode = UserConfig.ScketUnitC == 8 ? 1 : 2;//1：单行排列，2：双行排列
                colMaxC = UserConfig.ScketUnitC == 4 ? 8 : 2;
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_YED)
            {
                colMaxC = 8;
            }
            else if(UserTask.ProgrammerType == GlobConstData.Programmer_WG || UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY)
            {
                colMaxC = UserConfig.ScketGroupC;
            }
            if (UserConfig.ScketGroupC % colMaxC == 0)
            {
                rowC = UserConfig.ScketGroupC / colMaxC;
                colC = colMaxC;
            }
            else
            {
                rowC = UserConfig.ScketGroupC / colMaxC + 1;
                colC = rowC == 1 ? UserConfig.ScketGroupC : colMaxC;
            }
            groupW = Width / colC - 1;
            groupH = Height / rowC - 1;
            if (rowC == 1)
            {
                groupH -= 10;
                btnH = (UserTask.ProgrammerType == GlobConstData.Programmer_DP && UserConfig.ScketUnitC == 4) ? 20 : 40;
                btnW = 30;
                offsetH = 80;
                offsetW = 2;
                size0 = new Size(140, 40);
                point0 = new Point((groupW - 140) / 2, 20);
            }
            else
            {
                btnH = (UserTask.ProgrammerType == GlobConstData.Programmer_DP && UserConfig.ScketUnitC == 16) ? 14 : 25;
                btnW = 23;
                offsetW = 32;
                size0 = new Size(28, groupH - 2);
                point0 = new Point(1, 1);
            }

            Point[] groupPoint = new Point[colC * rowC];//分组位置
            for (int n = 0; n < rowC; n++)
            {
                for (int m = 0; m < colC; m++)
                {
                    int nIndex = n * colC + m;
                    left = Math.Abs((colC - 1) * 0 - m) * (groupW + 1);
                    top = Math.Abs((rowC - 1) * 1 - n) * (groupH + 1);
                    groupPoint[nIndex] = new Point(left, top);
                }
            }

            scketGroupPanel = new SkinPanel[UserConfig.ScketGroupC];
            scketGroupBtn = new SkinButton[UserConfig.ScketGroupC];
            for (int i = 0; i < UserConfig.ScketGroupC; i++)
            {
                int nIndex = i;
                if (UserTask.ProgrammerType == GlobConstData.Programmer_AK && UserConfig.ScketGroupC == 5)
                {
                    if (i > 0)
                    {
                        nIndex++;
                    }
                }
                scketGroupPanel[i] = new SkinPanel
                {
                    BackColor = Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(58))))),
                    ControlState = CCWin.SkinClass.ControlState.Normal,
                    DownBack = null,
                    Location = groupPoint[nIndex],
                    MouseBack = null,
                    Name = "scketGroupPanel" + i,
                    NormlBack = null,
                    Size = new Size(groupW, groupH),
                    TabIndex = i
                };

                scketGroupBtn[i] = new SkinButton
                {
                    BackColor = Color.Transparent,
                    BaseColor = Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(171)))), ((int)(((byte)(250))))),
                    BorderColor = Color.Black,
                    BorderInflate = new Size(0, 0),
                    ControlState = CCWin.SkinClass.ControlState.Normal,
                    DownBack = null,
                    Font = new Font("黑体", 9F),
                    ForeColor = Color.White,
                    GlowColor = Color.Transparent,
                    InnerBorderColor = Color.Black,
                    IsDrawGlass = false,
                    Location = point0,
                    MouseBack = null,
                    Name = "scketGroupBtn" + i,
                    NormlBack = null,
                    RoundStyle = CCWin.SkinClass.RoundStyle.All,
                    Size = size0,
                    TabIndex = i,
                    Tag = i * UserConfig.ScketUnitC,
                    Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English ? "G" + (i + 1) : "组" + (i + 1) + "设置",
                    UseVisualStyleBackColor = false
                };
                scketGroupBtn[i].MouseUp += new MouseEventHandler(groupBtn_MouseUp);

                scketGroupPanel[i].Controls.Add(scketGroupBtn[i]);
                Controls.Add(scketGroupPanel[i]);
                scketGroupPanel[i].SuspendLayout();
            }

            //烧录座子相关控件
            colC = UserConfig.ScketUnitC / layoutMode;
            if (colC == 0)
            {
                colC = 1;
            }
            nW = (groupW - offsetW) / colC;
            nH = (groupH - offsetH)  / layoutMode - 4;
            if (UserTask.ProgrammerType == GlobConstData.Programmer_AK || (UserTask.ProgrammerType == GlobConstData.Programmer_DP && UserConfig.ScketUnitC == 16))
            {
                size1 = new Size(btnW, nH);
                size2 = new Size(nW - btnW - 3, nH - 2);
            }
            else
            {
                size1 = new Size(nW - 1, btnH);
                size2 = new Size(nW - 1, nH - btnH - 2);
            }
            scketEnableBtn = new SkinButton[UserConfig.AllScketC];
            scketStateLabel = new Label[UserConfig.AllScketC];
            for (int i = 0; i < UserConfig.AllScketC; i++)
            {
                left = (i % colC) * ((groupW - offsetW) / colC) + offsetW;
                if (layoutMode == 1)
                {
                    top = 3 + offsetH;
                }
                else
                {
                    if (UserTask.ProgrammerType == GlobConstData.Programmer_DP && UserConfig.ScketUnitC == 16)
                    {
                        top = (i % UserConfig.ScketUnitC / colC) * (nH + 2) + 3 + offsetH;
                        emSize0 = 12F;
                        emSize1 = 10F;
                        strEnable = "☑";
                        strDisable = "□";
                    }
                    else if (UserTask.ProgrammerType == GlobConstData.Programmer_DP && UserConfig.ScketUnitC == 4)
                    {
                        top = (i % UserConfig.ScketUnitC / colC) * (nH + 2) + 3 + offsetH;
                    }
                    else
                    {
                        top = (1 - (i % UserConfig.ScketUnitC) / colC) * (nH + 2) + 3 + offsetH;
                    }  
                }
                if (UserTask.ProgrammerType == GlobConstData.Programmer_AK || (UserTask.ProgrammerType == GlobConstData.Programmer_DP && UserConfig.ScketUnitC == 16))
                {
                    point1 = new Point(left, top);
                    point2 = new Point(btnW + 1 + left, 1 + top);
                }
                else
                {
                    point1 = new Point(left, top);
                    point2 = new Point(left, btnH + 1 + top);
                }
                scketEnableBtn[i] = new SkinButton
                {
                    BackColor = Color.Transparent,
                    BaseColor = Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(80))))),
                    BorderColor = Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(171)))), ((int)(((byte)(250))))),
                    ControlState = CCWin.SkinClass.ControlState.Normal,
                    DownBack = null,
                    Font = new Font("宋体", emSize0, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134))),
                    ForeColor = Color.White,
                    GlowColor = Color.Transparent,
                    InnerBorderColor = Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(171)))), ((int)(((byte)(250))))),
                    IsDrawGlass = false,
                    Location = point1,
                    MouseBack = null,
                    Name = "scketEnableBtn" + i,
                    NormlBack = null,
                    RoundStyle = CCWin.SkinClass.RoundStyle.All,
                    Size = size1,
                    TabIndex = i,
                    Tag = i,
                    Text = strDisable,
                    UseVisualStyleBackColor = false
                };
                scketEnableBtn[i].MouseUp += new MouseEventHandler(seatBtn_MouseUp);

                scketStateLabel[i] = new Label
                {
                    BackColor = Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(68)))), ((int)(((byte)(80))))),
                    BorderStyle = BorderStyle.Fixed3D,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("黑体", emSize1),
                    ForeColor = Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226))))),
                    Location = point2,
                    Name = "scketStateLabel" + i,
                    Size = size2,
                    TabIndex = i,
                    Text = "NULL",
                    TextAlign = ContentAlignment.MiddleCenter
                };

                int group = i / UserConfig.ScketUnitC;
                scketGroupPanel[group].Controls.Add(scketEnableBtn[i]);
                scketGroupPanel[group].Controls.Add(scketStateLabel[i]);
            }
            ResumeLayout(false);
            PerformLayout();
        }

        private void Programmer_Load(object sender, EventArgs e)
        {
            tmrUpdateUI.Start();//打开定时器刷新状态
            UpdateUI();
        }

        private void UpdateUI()
        {
            for (int i = 0; i < UserConfig.AllScketC; i++)
            {
                if (Axis.Group[i / UserConfig.ScketUnitC].Unit[i % UserConfig.ScketUnitC].Flag_Open)
                {
                    scketEnableBtn[i].Text = strEnable;
                    scketEnableBtn[i].BaseColor = Color.FromArgb(45, 189, 84);
                }
                else
                {
                    scketEnableBtn[i].Text = strDisable;
                    scketEnableBtn[i].BaseColor = Color.FromArgb(68, 68, 80);
                }
            }
        }

        private void EnableBtn(int Group, int Unit)
        {
            int k = UserConfig.ScketUnitC;
            int ind = Group * k + Unit;
            if (Axis.Group[Group].Unit[Unit].Flag_Open)
            {
                scketEnableBtn[ind].Text = strEnable;
                scketEnableBtn[ind].BaseColor = Color.FromArgb(45, 189, 84);
            }
            else
            {
                scketEnableBtn[ind].Text = strDisable;
                scketEnableBtn[ind].BaseColor = Color.FromArgb(68, 68, 80);
            }
            g_config.WriteSocketEnabled(ind);
        }

        private void seatBtn_MouseUp(object sender, MouseEventArgs e)
        {
            int ind = Convert.ToInt32((sender as SkinButton).Tag);
            int k = UserConfig.ScketUnitC;
            Axis.Group[ind / k].Unit[ind % k].Flag_Open = !Axis.Group[ind / k].Unit[ind % k].Flag_Open;
            if (Axis.Group[ind / k].Unit[ind % k].Flag_Open)
            {
                scketEnableBtn[ind].Text = strEnable;
                scketEnableBtn[ind].BaseColor = Color.FromArgb(45, 189, 84); 
            }
            else
            {
                scketEnableBtn[ind].Text = strDisable;
                scketEnableBtn[ind].BaseColor = Color.FromArgb(68, 68, 80);
            }
            g_config.WriteSocketEnabled(ind);
        }

        private void groupBtn_MouseUp(object sender, MouseEventArgs e)
        {
            int ind = Convert.ToInt32((sender as SkinButton).Tag);
            int k = UserConfig.ScketUnitC;
            int index;
            bool flag = !Axis.Group[ind / k].Unit[ind % k].Flag_Open;
            for (int i = 0; i < UserConfig.ScketUnitC; i++)
            {
                index = ind + i;
                if (flag)
                {
                    Axis.Group[index / k].Unit[index % k].Flag_Open = true;
                    scketEnableBtn[index].Text = strEnable;
                    scketEnableBtn[index].BaseColor = Color.FromArgb(45, 189, 84); ;
                }
                else
                {
                    Axis.Group[index / k].Unit[index % k].Flag_Open = false;
                    scketEnableBtn[index].Text = strDisable;
                    scketEnableBtn[index].BaseColor = Color.FromArgb(68, 68, 80);
                }
                g_config.WriteSocketEnabled(index);
            }
            
        }

        private void __DrawBurnSeat()
        {
            int downResult;
            for (int i = 0; i < UserConfig.AllScketC; i++)
            {
                downResult = Axis.Group[i / UserConfig.ScketUnitC].Unit[i % UserConfig.ScketUnitC].DownResult;
                if (downResult == 0)
                {
                    scketStateLabel[i].BackColor = Color.FromArgb(68, 68, 80);
                    scketStateLabel[i].ForeColor = Color.FromArgb(226, 226, 226);
                    scketStateLabel[i].Text = "NULL";
                }
                else if (downResult == 1)
                {
                    scketStateLabel[i].BackColor = Color.Lime;
                    scketStateLabel[i].ForeColor = Color.Green;
                    scketStateLabel[i].Text = "PASS";
                }
                else if (downResult == 2)
                {
                    scketStateLabel[i].BackColor = Color.Red;
                    scketStateLabel[i].ForeColor = Color.DarkRed;
                    scketStateLabel[i].Text = "NG";
                }
                else if (downResult == 4)
                {
                    scketStateLabel[i].BackColor = Color.FromArgb(68, 68, 80);
                    scketStateLabel[i].ForeColor = Color.FromArgb(226, 226, 226);
                    scketStateLabel[i].Text = "---";
                }
                else if (downResult == 8)
                {
                    scketStateLabel[i].BackColor = Color.Lime;
                    scketStateLabel[i].ForeColor = Color.Green;
                    scketStateLabel[i].Text = "SPASS";
                }
                else if (downResult == 16)
                {
                    scketStateLabel[i].BackColor = Color.Red;
                    scketStateLabel[i].ForeColor = Color.DarkRed;
                    scketStateLabel[i].Text = "SNG";
                }
                else if (downResult == 32)
                {
                    scketStateLabel[i].BackColor = Color.FromArgb(68, 68, 80);
                    scketStateLabel[i].ForeColor = Color.FromArgb(226, 226, 226);
                    scketStateLabel[i].Text = "***";
                }
                else if (downResult == 64)
                {
                    scketStateLabel[i].BackColor = Color.Red;
                    scketStateLabel[i].ForeColor = Color.DarkRed;
                    scketStateLabel[i].Text = "ERROR";
                }
            }
        }

        private void tmrUpdateUI_Tick(object sender, EventArgs e)
        {
            __DrawBurnSeat();
        }
    }
}
