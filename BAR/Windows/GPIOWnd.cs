using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BAR.CommonLib_v1._0;
using LBSoft.IndustrialCtrls.Leds;
using PLC;

namespace BAR.Windows
{
    public partial class GPIOWnd : Form
    {
        BAR man = null;  //声明父窗体
        int IOIndex;
        GPIO[][] InPut, OutPut;

        public GPIOWnd()
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();

            

        }

        private void 普通IO_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Enabled = false;
            e.Cancel = true;
            this.Hide();
        }

        private void 普通IO_Load(object sender, EventArgs e)
        {
            man = (BAR)this.Owner;//实例化父窗体
            SetIOTpye();
        }

        /// <summary>
        /// 设置IO类型
        /// </summary>
        private void SetIOTpye()
        {
            #region --------------------赋值IO名-----------------------
            string[,] strIO_Axis6_AK = new string[,]//2吸笔（6轴）昂科
            {
                {" EXI0   急停"," EXI1   门感应"," EXI2   备用"," EXI3   备用"," EXI4   真空开关"," EXI5   料盘1感应信号",
                    " EXI6   料盘2感应信号"," EXI7   料盘3感应信号"," EXI8    Z1真空检测"," EXI9    Z2真空检测"," EXI10   备用",
                    " EXI11   备用"," EXI12   备用"," EXI13   备用"," EXI14   备用"," EXI15   备用"},
                {" EXI0   压座1原位"," EXI1   压座2原位"," EXI2   压座3原位"," EXI3   压座4原位"," EXI4   压座5原位"," EXI5   压座6原位",
                    " EXI6   压座7原位"," EXI7   压座8原位"," EXI8    翻盖座1原位"," EXI9    翻盖座2原位"," EXI10   翻盖座3原位",
                    " EXI11   翻盖座4原位"," EXI12   翻盖座5原位"," EXI13   翻盖座6原位"," EXI14   翻盖座7原位"," EXI15   翻盖座8原位"},
                {" EXO0   Z1吸嘴真空"," EXO1   Z1吸嘴吹气"," EXO2   Z2吸嘴真空"," EXO3   Z2吸嘴吹气"," EXO4   备用"," EXO5   备用",
                    " EXO6   备用"," EXO7   备用"," EXO8    料盘1指示灯"," EXO9    料盘2指示灯"," EXO10   料盘3指示灯",
                    " EXO11   真空泵电源"," EXO12   红色指示灯"," EXO13   黄色指示灯"," EXO14   绿色指示灯"," EXO15   蜂鸣器"},
                {" EXO0   压座1电磁阀"," EXO1   压座2电磁阀"," EXO2   压座3电磁阀"," EXO3   压座4电磁阀"," EXO4   压座5电磁阀",
                    " EXO5   压座6电磁阀"," EXO6   压座7电磁阀"," EXO7   压座8电磁阀"," EXO8    翻盖座1电磁阀"," EXO9    翻盖座2电磁阀",
                    " EXO10   翻盖座3电磁阀"," EXO11   翻盖座4电磁阀"," EXO12   翻盖座5电磁阀"," EXO13   翻盖座6电磁阀",
                    " EXO14   翻盖座7电磁阀"," EXO15   翻盖座8电磁阀"}
            };
            string[,] strIO_Axis7_AK = new string[,]//4吸笔（7轴）昂科
            {
                {" EXI0   急停"," EXI1   门感应"," EXI2   备用"," EXI3   备用"," EXI4   真空开关"," EXI5   料盘1感应信号",
                    " EXI6   料盘2感应信号"," EXI7   料盘3感应信号"," EXI8   Z1真空检测"," EXI9   Z2真空检测"," EXI10   Z3真空检测",
                    " EXI11   Z4真空检测"," EXI12   备用"," EXI13   备用"," EXI14   备用"," EXI15   备用"},
                {" EXI0   压座1原位"," EXI1   压座2原位"," EXI2   压座3原位"," EXI3   压座4原位"," EXI4   压座5原位"," EXI5   压座6原位",
                    " EXI6   压座7原位"," EXI7   压座8原位"," EXI8   备用"," EXI9   备用"," EXI10   备用"," EXI11   备用"," EXI12   备用",
                    " EXI13   备用"," EXI14   备用"," EXI15   备用"},
                {" EXO0   Z1吸嘴真空"," EXO1   Z1吸嘴吹气"," EXO2   Z2吸嘴真空"," EXO3   Z2吸嘴吹气"," EXO4   Z3吸嘴真空"," EXO5   Z3吸嘴吹气",
                    " EXO6   Z4吸嘴真空"," EXO7   Z4吸嘴吹气"," EXO8    料盘1指示灯"," EXO9    料盘2指示灯"," EXO10   料盘3指示灯",
                    " EXO11   真空泵电源"," EXO12   红色指示灯"," EXO13   黄色指示灯"," EXO14   绿色指示灯"," EXO15   蜂鸣器"},
                {" EXO0   压座1电磁阀"," EXO1   压座2电磁阀"," EXO2   压座3电磁阀"," EXO3   压座4电磁阀"," EXO4   压座5电磁阀",
                    " EXO5   压座6电磁阀"," EXO6   备用"," EXO7   备用"," EXO8    Z1吸笔电磁阀"," EXO9    Z2吸笔电磁阀",
                    " EXO10   Z3吸笔电磁阀"," EXO11   Z4吸笔电磁阀"," EXO12   飞达1电磁阀"," EXO13   飞达2电磁阀",
                    " EXO14   备用"," EXO15   备用"}
            };
            string[,] strIO_Axis10_DP16X6 = new string[,]//4吸笔（10轴）岱镨
            {
                {" EXI0   E-STOP"," EXI1   Door"," EXI2   Tube_5"," EXI3   Tube_6"," EXI4   VacuumSwitch"," EXI5   Tray_1",
                    " EXI6   Tray_2"," EXI7   Tray_3"," EXI8   Z1_Vacuum"," EXI9   Z2_Vacuum"," EXI10   Z3_Vacuum",
                    " EXI11   Z4_Vacuum"," EXI12   Tube_1"," EXI13   Tube_2"," EXI14   Tube_3"," EXI15   Tube_4"},
                {" EXI0   Prog_1"," EXI1   Prog_2"," EXI2   Prog_3"," EXI3   Prog_4"," EXI4   Prog_5"," EXI5   Prog_6",
                    " EXI6   RES"," EXI7   RES"," EXI8   RES"," EXI9   RES"," EXI10   RES"," EXI11   RES"," EXI12   RES",
                    " EXI13   StartButton"," EXI14   PauseButton"," EXI15   ResetButton"},
                {" EXO0   Z1_Vacuum"," EXO1   Z1_Blow"," EXO2   Z2_Vacuum"," EXO3   Z2_Blow"," EXO4   Z3_Vacuum"," EXO5   Z3_Blow",
                    " EXO6   Z4_Vacuum"," EXO7   Z4_Blow"," EXO8    Tray_1"," EXO9    Tray_2"," EXO10   Tray_3",
                    " EXO11   VacuumPower"," EXO12   RedLED"," EXO13   YellowLED"," EXO14   GreenLED"," EXO15   Buzzer"},
                {" EXO0   Prog_1"," EXO1   Prog_2"," EXO2   Prog_3"," EXO3   Prog_4"," EXO4   Prog_5",
                    " EXO5   Prog_6"," EXO6   RES"," EXO7   RES"," EXO8    RES"," EXO9    RES",
                    " EXO10   RES"," EXO11   RES"," EXO12   RES"," EXO13   RES",
                    " EXO14   PauseLED"," EXO15   ResetLED"}
            };
            #endregion

            int PenCount = UserConfig.VacuumPenC;
            if (UserTask.PenType == 0)//电机吸笔
            {
                if (PenCount == 4)//4吸笔
                {
                    IOIndex = 1;
                    InPut = In_Output.EXI;
                    OutPut = In_Output.EXO;
                    if (MultiLanguage.DefaultLanguage == GlobConstData.ST_English)
                    {
                        SetIOTitle(strIO_Axis10_DP16X6);
                    }
                }
                if (PenCount == 2)//2吸笔
                {
                    IOIndex = 0;
                    InPut = In_Output.EXGLI;
                    OutPut = In_Output.EXGLO;
                    SetIOTitle(strIO_Axis6_AK);
                }
            }
            else if (UserTask.PenType == 1)//气缸吸笔
            {
                IOIndex = 0;
                InPut = In_Output.EXGLI;
                OutPut = In_Output.EXGLO;

                if (PenCount == 4)//4吸笔
                {
                    SetIOTitle(strIO_Axis7_AK);
                }
                if (PenCount == 2)//2吸笔
                {
                }

            }
        }

        public void InitUI()
        {
            timer1.Enabled = true;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            String ctlName;
            Control[] tagAry;
            for (int i = 0; i < 16; i++)
            {
                ctlName = "lbLed" + i + "_0_IN";
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    IO_Monitor((tagAry.First() as LBLed), In_Output.EXI[0][i].M);
                }

                ctlName = "lbLed" + i + "_1_IN";
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    IO_Monitor((tagAry.First() as LBLed), InPut[IOIndex][i].M);
                }

                ctlName = "lbLed" + i + "_0_OUT";
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    IO_Monitor((tagAry.First() as LBLed), In_Output.EXO[0][i].M);
                }
                

                ctlName = "lbLed" + i + "_1_OUT";
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    IO_Monitor((tagAry.First() as LBLed), OutPut[IOIndex][i].M);
                }
            }
        }

        /// <summary>
        /// IO监控
        /// </summary>
        /// <param name="lBLed">控件</param>
        /// <param name="IsBright">监控值</param>
        public void IO_Monitor(LBLed lBLed, bool IsBright)
        {
            if (IsBright)
            {
                lBLed.State = LBLed.LedState.On;
            }
            else
            {
                lBLed.State = LBLed.LedState.Off;
            }
        }

        /// <summary>
        /// 设置IO名称
        /// </summary>
        /// <param name="str">名称</param>
        private void SetIOTitle(string[,] str)
        {
            String ctlName;
            Control[] tagAry;
            for (int i = 0; i < 16; i++)
            {
                ctlName = "lbLed" + i + "_0_IN";
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    (tagAry.First() as LBLed).Label = str[0,i];
                }

                ctlName = "lbLed" + i + "_1_IN";
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    (tagAry.First() as LBLed).Label = str[1,i];
                }
                
                ctlName = "lbLed" + i + "_0_OUT";
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    (tagAry.First() as LBLed).Label = str[2,i];
                }

                ctlName = "lbLed" + i + "_1_OUT";
                tagAry = Controls.Find(ctlName, true);
                if (tagAry.Length != 0)
                {
                    (tagAry.First() as LBLed).Label = str[3,i];
                }
            }
        }
    }
}
