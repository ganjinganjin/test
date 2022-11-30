using BAR.Commonlib;
using BAR.CommonLib_v1._0;
using Microsoft.Win32;
using PLC;
using Spire.Xls.Core.Converter.General.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BAR.Windows
{
    public partial class EngineerSettingWnd : Form
    {
        Config g_config = Config.GetInstance();
        BAR main = null;
        
        string[] strProgrammerType;
        string[] strPenType;
        string[] strLightType;
        string[] strLightCType;
        string[] strDir;
        string[] strTrayModel;
        string[] strTray1Start;
        string[] strTray1MoveDir;
        string[] strFunction;
        string[] strCardNum;
        string[] strLogo;
        string[] strMesType;
        string[] strFeederCount;
        string[] strBredeDotCount;
        string[] strCardType;
        string[] strCameraType;
        string[] strAltimeter;
        string[] strCameraIP;
        string[] strCCDModel;
        string[] strZoomLens;
        string[] strEfficiency;
        string[] strProtocol_WG;
        string[] strLanguage;

        public EngineerSettingWnd()
        {
            InitializeComponent();

            #region --------------------赋值下拉选项名-----------------------
            strProgrammerType = new string[]
            {
                "昂科烧录器","岱镨烧录器","外挂烧录器","易而达烧录器","瑞德烧录器","硕飞烧录器","推压式外挂烧录器","欣扬烧录器"
            };
            strProtocol_WG = new string[]
            {
                "标准","时代速信"
            };
            strPenType = new string[]
            {
                "电机吸笔","气缸吸笔"
            };
            strLightType = new string[]
            {
                "裸板","大恒1","大恒2","创视"
            };
            strLightCType = new string[]
            {
                "串口","网口"
            };
            strDir = new string[]
            {
                "正常","取反"
            };
            strTrayModel = new string[]
            {
                "三横盘","两竖一横盘1","两横一竖盘1","两横一竖盘2","两竖一横盘2","三竖盘"
            };
            strTray1Start = new string[]
            {
                "左上角","左下角"
            };
            strTray1MoveDir = new string[]
            {
                "列方向","行方向"
            };
            strFunction = new string[]
            {
                "关闭","启用"
            };
            strCardNum = new string[]
            {
                "正常","取反"
            };
            strLogo = new string[]
            {
                "金创图","自动烧录机","昂科","海洛","嘉多利","易而达","義辉电子","德爱思","岱镨","志浩航","联阳电子"
            };
            strMesType = new string[]
            {
                "关闭","欣旺达","快捷达","协创","RPC服务器"
            };
            strFeederCount = new string[]
            {
                "单飞达","双飞达"
            };
            strBredeDotCount = new string[]
            {
                "单打点","双打点"
            };
            strCardType = new string[]
            {
                "固高","维精"
            };
            strCameraType = new string[]
            {
                "大恒","大华"
            };
            strAltimeter = new string[]
            {
                "关闭","远向","舟正&HG-C1030","松下","舟正&HG-C1100"
            };
            strCameraIP = new string[]
            {
                "自动获取","固定设置"
            };
            strCCDModel = new string[]
            {
                "定拍","飞拍"
            };
            strZoomLens = new string[]
            {
                "定焦","镜头变焦","电机变焦"
            };
            strEfficiency = new string[]
            {
                "理论","实际"
            };
            strEfficiency = new string[]
            {
                "理论","实际"
            };
            strLanguage = new string[]
            {
                "中文","英文"
            };
            #endregion

        }

        private void EngineerSettingWnd_Load(object sender, EventArgs e)
        {
            main = (BAR)this.Owner;
            GetComList(CboLightPort, g_config.ArrStrutCom[0].ICom);
            GetComList(CboBraidPort, g_config.ArrStrutCom[1].ICom);
            GetComList(CboAutoTrayPort, g_config.ArrStrutCom[2].ICom);
            GetComList(CboProgrammerPort, g_config.ArrStrutCom[3].ICom);
            GetComList(CboCathetometerPort,g_config.ArrStrutCom[4].ICom);
            GetComList(CboZoomLensPort, g_config.ArrStrutCom[5].ICom);

            InitCbo();
            InitText();
            NudShutter.Value = Convert.ToDecimal(Config.Shutter);
        }


        /// <summary>
        /// 获取主机可以COM端口
        /// </summary>
        /// <param name="comboBox">下拉控件</param>
        public void GetComList(ComboBox comboBox,int select)
        {
            RegistryKey keyCom = Registry.LocalMachine.OpenSubKey("Hardware\\DeviceMap\\SerialComm");
            if (keyCom != null)
            {
                string[] sSubKeys = keyCom.GetValueNames();
                comboBox.Items.Clear();
                foreach (string sName in sSubKeys)
                {
                    string sValue = (string)keyCom.GetValue(sName);
                    comboBox.Items.Add(sValue);
                }
                comboBox.Text = "COM" + select;
            }
            else
            {
                comboBox.Text = "COM" + select;
            }
        }

        /// <summary>
        /// 初始化下拉框
        /// </summary>
        private void InitCbo()
        {
            AddCboItems(CboProgrammerType, strProgrammerType, g_config.ProgrammerType);
            AddCboItems(CboPenType, strPenType, g_config.PenType);
            AddCboItems(CboLightType, strLightType, g_config.ILightType);
            AddCboItems(CboLightCType, strLightCType, g_config.ILightCType);
            AddCboItems(CboXDir, strDir, g_config.ArrCamDir[0]);
            AddCboItems(CboYDir, strDir, g_config.ArrCamDir[1]);
            AddCboItems(CboRDir, strDir, g_config.ArrCamDir[2]);
            AddCboItems(CboTrayModel, strTrayModel, g_config.TrayModel);
            AddCboItems(CboTray1Start, strTray1Start, g_config.Tray1Start);
            AddCboItems(CboTray1MoveDir, strTray1MoveDir, g_config.Tray1MoveDir);
            AddCboItems(CboNGTrayStart, strFunction, g_config.NGTrayStart);
            AddCboItems(CboAutoTrayStart, strFunction, g_config.AutoTrayStart);
            AddCboItems(CboCardNum, strCardNum, PLC1.card[0].cardNum);
            AddCboItems(CboLogo, strLogo,g_config.ILogo);
            AddCboItems(CboMesType, strMesType, Mes.Type);
            AddCboItems(CboAutoTrayDir, strDir, Config.AutoTrayDir);
            AddCboItems(CboAutoTrayDot, strFunction, Config.AutoTrayDot);
            AddCboItems(CboVision_3D, strFunction, Vision_3D.Function == true ? 1 : 0);
            AddCboItems(CboAltimeter, strAltimeter, Config.Altimeter);
            AddCboItems(CboInks, strFunction, Inks.Function == true ? 1 : 0);
            AddCboItems(CboFeederCount, strFeederCount, Config.FeederCount);
            AddCboItems(CboBredeDotCount, strBredeDotCount, Config.BredeDotCount);
            AddCboItems(CboCardType, strCardType, Config.CardType);
            AddCboItems(CboxCameraType, strCameraType, Config.CameraType);
            AddCboItems(CboxCameraIP, strCameraIP, Config.CameraIP);
            AddCboItems(CboCCDModel, strCCDModel, Config.CCDModel);
            AddCboItems(CboICDirAndFlaw, strFunction, Config.ICDirAndFlaw);
            AddCboItems(CboZoomLens, strZoomLens, Config.ZoomLens);
            AddCboItems(CboSyncTakeLay, strFunction, Config.SyncTakeLay == true ? 1 : 0);
            AddCboItems(CboEfficiency, strEfficiency, Config.Efficiency);
            AddCboItems(CboPhenixIOS, strFunction, Config.PhenixIOS == true ? 1 : 0);
            AddCboItems(CboProtocol_WG, strProtocol_WG, Config.Protocol_WG);
            AddCboItems(CboLanguage, strLanguage, MultiLanguage.IsEnglish() == true ? 1 : 0);

            if (g_config.ProgrammerType == GlobConstData.Programmer_DP)
            {
                CboProgrammerType.Enabled = false;
            }
            if (g_config.ILogo == GlobConstData.Logo_DP)
            {
                CboLogo.Enabled = false;
            }
            if (g_config.ProgrammerType == GlobConstData.Programmer_WG || g_config.ProgrammerType == GlobConstData.Programmer_WG_TY)
            {
                CboProtocol_WG.Visible = true;
                skinLabel44.Visible = true;
            }
            
        }

        private void InitText()
        {
            TxtServerIP.Text = GlobalParam.ServerIP;
            TxtServerPort.Text = GlobalParam.ServerPort.ToString();
            TxtRPCServerIP.Text = GlobalParam.RPCServerIP;
            TxtRPCServerPort.Text = GlobalParam.RPCServerPort.ToString();
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
                string strGet = MultiLanguage.GetString(sName);
                comboBox.Items.Add(strGet);
            }
            comboBox.SelectedIndex = select;
        }

        /// <summary>
        /// 保存参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            Config.Index_Z = Convert.ToInt32(NudIndex_Z.Value);
            g_config.ArrStrutCom[0].ICom = Convert.ToInt32(CboLightPort.Text.Substring(3));
            g_config.ArrStrutCom[1].ICom = Convert.ToInt32(CboBraidPort.Text.Substring(3));
            g_config.ArrStrutCom[2].ICom = Convert.ToInt32(CboAutoTrayPort.Text.Substring(3));
            g_config.ArrStrutCom[3].ICom = Convert.ToInt32(CboProgrammerPort.Text.Substring(3));
            g_config.ArrStrutCom[4].ICom = Convert.ToInt32(CboCathetometerPort.Text.Substring(3));
            g_config.ArrStrutCom[5].ICom = Convert.ToInt32(CboZoomLensPort.Text.Substring(3));
            g_config.ProgrammerType = CboProgrammerType.SelectedIndex;
            g_config.PenType = CboPenType.SelectedIndex;
            g_config.ILightType = CboLightType.SelectedIndex;
            g_config.ILightCType = CboLightCType.SelectedIndex;
            g_config.ArrCamDir[0] = CboXDir.SelectedIndex;
            g_config.ArrCamDir[1] = CboYDir.SelectedIndex;
            g_config.ArrCamDir[2] = CboRDir.SelectedIndex;
            g_config.TrayModel = CboTrayModel.SelectedIndex;
            g_config.Tray1Start = CboTray1Start.SelectedIndex;
            g_config.Tray1MoveDir = CboTray1MoveDir.SelectedIndex;
            g_config.NGTrayStart = CboNGTrayStart.SelectedIndex;
            g_config.AutoTrayStart = CboAutoTrayStart.SelectedIndex;
            g_config.ILogo = CboLogo.SelectedIndex;
            Mes.Type = CboMesType.SelectedIndex;
            Config.AutoTrayDir = CboAutoTrayDir.SelectedIndex;
            Config.AutoTrayDot = CboAutoTrayDot.SelectedIndex;
            Vision_3D.Function = CboVision_3D.SelectedIndex == 1 ? true : false;
            Config.Altimeter = CboAltimeter.SelectedIndex;
            Inks.Function = CboInks.SelectedIndex == 1 ? true : false;
            Config.FeederCount = CboFeederCount.SelectedIndex;
            Config.BredeDotCount = CboBredeDotCount.SelectedIndex;
            Config.CardType = CboCardType.SelectedIndex;
            Config.CameraType = CboxCameraType.SelectedIndex;
            Config.CameraIP = CboxCameraIP.SelectedIndex;
            Config.CCDModel = CboCCDModel.SelectedIndex;
            Config.ICDirAndFlaw = CboICDirAndFlaw.SelectedIndex;
            Config.Shutter = Convert.ToInt32(NudShutter.Value);
            Config.ZoomLens = CboZoomLens.SelectedIndex;
            Config.SyncTakeLay = CboSyncTakeLay.SelectedIndex == 1 ? true : false;
            Config.Efficiency = CboEfficiency.SelectedIndex;
            Config.PhenixIOS = CboPhenixIOS.SelectedIndex == 1 ? true : false;
            Config.Protocol_WG = CboProtocol_WG.SelectedIndex;
            MultiLanguage.DefaultLanguage = CboLanguage.SelectedIndex == 1 ? "en-US" : "zh-CN";

            GlobalParam.ServerIP = Regex.Replace(TxtServerIP.Text, @"\s", "");
            GlobalParam.ServerPort = Convert.ToInt32(Regex.Replace(TxtServerPort.Text, @"\s", ""));
            GlobalParam.RPCServerIP = Regex.Replace(TxtRPCServerIP.Text, @"\s", "");
            GlobalParam.RPCServerPort = Convert.ToInt32(Regex.Replace(TxtRPCServerPort.Text, @"\s", ""));

            #region ------------------配置烧录器串口----------------------
            if (g_config.ProgrammerType == 0)//昂科烧录器串口配置
            {
                g_config.ArrStrutCom[3].IBaud = 115200;
                g_config.ArrStrutCom[3].IParity = 1;
            }
            else if (g_config.ProgrammerType == 1)//岱镨烧录器串口配置
            {
                g_config.ArrStrutCom[3].IBaud = 115200;
                g_config.ArrStrutCom[3].IParity = 1;
            }
            else if (g_config.ProgrammerType == 2)//外挂烧录器串口配置
            {
                g_config.ArrStrutCom[3].IBaud = 19200;
                g_config.ArrStrutCom[3].IParity = 0;
            }
            else if (g_config.ProgrammerType == 3)//易而达烧录器串口配置
            {
                g_config.ArrStrutCom[3].IBaud = 115200;
                g_config.ArrStrutCom[3].IParity = 0;
            }
            #endregion

            #region -------------------配置料盘摆向-----------------------
            if (g_config.TrayModel == 0)//料盘1、2、3横
            {
                g_config.TrayRotateDir[1] = 0;
                g_config.TrayRotateDir[2] = 0;
            }
            else if (g_config.TrayModel == 1)//料盘1、2竖，料盘3横
            {
                g_config.TrayRotateDir[1] = 0;
                g_config.TrayRotateDir[2] = 2;
            }
            else if (g_config.TrayModel == 2)//料盘1、3横，料盘2竖
            {
                g_config.TrayRotateDir[1] = 1;
                g_config.TrayRotateDir[2] = 0;
            }
            #endregion

            #region --------------------配置板卡序号---------------------
            if (CboCardNum.SelectedIndex == 0)
            {
                PLC1.card[0].cardNum = 0;
                PLC1.card[1].cardNum = 1;
            }
            else
            {
                PLC1.card[0].cardNum = 1;
                PLC1.card[1].cardNum = 0;
            }
            #endregion


            g_config.SaveEngineerSetting();
            MultiLanguage.SaveDefaultLanguage();
            MessageBox.Show("保存工程参数成功\r\n【重启软件生效】", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void EngineerSettingWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
            main.Activate();
        }

        private void CboProgrammerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CboProgrammerType.SelectedIndex == GlobConstData.Programmer_WG || CboProgrammerType.SelectedIndex == GlobConstData.Programmer_WG_TY)
            {
                CboProtocol_WG.Visible = true;
                skinLabel44.Visible = true;
            }
            else
            {
                CboProtocol_WG.Visible = false;
                skinLabel44.Visible = false;
            }
        }
    }
}
