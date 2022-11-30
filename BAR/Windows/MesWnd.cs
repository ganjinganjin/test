using BAR.Commonlib;
using CCWin.SkinControl;
using DllInterface;
using DllInterface.CodeFirst;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using TensionDetectionDLL;

namespace BAR.Windows
{
    public partial class MesWnd : Form
    {
        #region
        private SkinTextBox[] txtMes;
        private Label[] lblMes;
        private SkinComboBox[] cobMes;
        private int txtCount;
        private int cobCount;
        private string[] strLblText;

        public Act g_act = Act.GetInstance();
        Config g_config = Config.GetInstance();
        MESModule _MESModule = new MESModule();

        bool m_bMESIsOpen;
        string m_strProgFileDir;
        string m_strServerPath;
        bool m_bMESDataReady = false;///MES数据是否Ready

        Thread m_Worker = null;
        private static readonly object _objYieldChange = new object();
        private static readonly object _objUIDFetched = new object();
        public struct StSetting
        {
            public string strTemplateDir;  ///工程模板文件夹
            public string strProgFilesDir; ///烧录档案文件夹
            public string strServerDir; ///multiAprog服务器的路径
            public string strFuncMode;///烧录命令
            public int DllTestEn; ///是否使能动态库测试
            public string DllName; ///动态库名称
            public string DllPath;//动态库路径
            //INT DllUseCSharp; //是否使用C#DLL
            //tDebug Debug;
            public string strCurFactory;
            //std::vector<tFactorySetting> vFactorySetting;
            public int ChipInspectionEn;
            public string strDevTypeFile; ////器件表文件名称
            public uint MesExitCondition;  //MES退出条件，0表示采用OK数作为退出条件，1表示用OK+NG数作为退出条件
            public string strMesExitCond; /// MES退出条件字符串
            public int nTargetNum;//目标产量
        }


        public FunctionDll _dll_KJD = new FunctionDll();
        public ProgramInfo info_KJD = new ProgramInfo();

        public StSetting m_Setting;

        SelectProgFile _SelectProgFile;

        #endregion
        public MesWnd()
        {
            InitializeComponent();
            _InitializeComponent();

            g_act.mesWnd = this;
            m_Setting = new StSetting();
            
            m_Setting.strServerDir = "C:\\ACROVIEW\\MultiAprog";
            m_Setting.DllName = "ProgCtrl.dll";

            for (int i = 0; i < 6; i++)
            {
                BurnInfo.Group[i] = new Group_class();
            }

            for (int j = 0; j < 100; j++)
            {
                MesInfo_XC.Model[j] = new Model_class();
            }
        }

        private void _InitializeComponent()
        {
            CheckMesType();
            int nW = 190, nH = 34;
            int left = 0, top = 0;

            //TxtBox
            txtMes = new SkinTextBox[txtCount];
            lblMes = new Label[txtCount];
            cobMes = new SkinComboBox[cobCount];
            for (int i = 0; i < txtCount; i++)
            {
                left = (i % 2) * 350 + 200;
                top = (i / 2) * (2 + nH) + 17;
                txtMes[i] = new SkinTextBox
                {
                    BackColor = System.Drawing.Color.Transparent,
                    BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D,
                    DownBack = null,
                    Icon = null,
                    IconIsButton = false,
                    IconMouseState = CCWin.SkinClass.ControlState.Normal,
                    IsPasswordChat = '\0',
                    IsSystemPasswordChar = false,
                    Lines = new string[0],
                    Location = new System.Drawing.Point(left, top),
                    Margin = new System.Windows.Forms.Padding(0),
                    MaxLength = 32767,
                    MinimumSize = new System.Drawing.Size(28, 28),
                    MouseBack = null,
                    MouseState = CCWin.SkinClass.ControlState.Normal,
                    Multiline = true,
                    Name = "txtMes" + i,
                    NormlBack = null,
                    Padding = new System.Windows.Forms.Padding(5),
                    ReadOnly = true,
                    ScrollBars = System.Windows.Forms.ScrollBars.None,
                    Size = new System.Drawing.Size(nW, nH)
                };

                txtMes[i].SkinTxt.BorderStyle = System.Windows.Forms.BorderStyle.None;
                txtMes[i].SkinTxt.Dock = System.Windows.Forms.DockStyle.Fill;
                txtMes[i].SkinTxt.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                txtMes[i].SkinTxt.Location = new System.Drawing.Point(5, 5);
                txtMes[i].SkinTxt.Multiline = true;
                txtMes[i].SkinTxt.Name = "BaseText";
                txtMes[i].SkinTxt.ReadOnly = true;
                txtMes[i].SkinTxt.Size = new System.Drawing.Size(116, 20);
                txtMes[i].SkinTxt.TabIndex = 0;
                txtMes[i].SkinTxt.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
                txtMes[i].SkinTxt.WaterText = "";
                txtMes[i].TabIndex = i;
                txtMes[i].TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
                txtMes[i].WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
                txtMes[i].WaterText = "";
                txtMes[i].WordWrap = true;

                

                lblMes[i] = new Label
                {
                    Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134))),
                    ForeColor = System.Drawing.Color.Black,
                    ImeMode = System.Windows.Forms.ImeMode.NoControl,
                    Location = new System.Drawing.Point(left - 140, top),
                    Name = "label" + i,
                    Size = new System.Drawing.Size(140, nH),
                    TabIndex = i,
                    Text = strLblText[i],
                    TextAlign = System.Drawing.ContentAlignment.MiddleRight
                };
                
                groupBox2.Controls.Add(txtMes[i]);
                groupBox2.Controls.Add(lblMes[i]);

            }

            for (int j = 0; j < cobCount; j++)
            {
                left = (j % 2) * 350 + 200;
                top = (j / 2) * (2 + nH) + 17;
                cobMes[j] = new SkinComboBox
                {
                    BorderColor = System.Drawing.Color.Gray,
                    Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134))),
                    Location = new System.Drawing.Point(left, top),
                    Name = "cobMes" + j,
                    Size = new System.Drawing.Size(189, 30),
                    TabIndex = j,
                    Tag = j,
                    Visible = false,
                };
                cobMes[j].DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
                cobMes[j].DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                cobMes[j].FormattingEnabled = true;
                cobMes[j].WaterText = "";
                groupBox2.Controls.Add(cobMes[j]);
            }
            
            SetControlState();
        }

        /// <summary>
        /// 检查MES客户
        /// </summary>
        private void CheckMesType()
        {
            if (Mes.Type == GlobConstData.ST_MESTYPE_XWD)//欣旺达
            {
                txtCount = 9;
                strLblText = new string[]
                {
                    "设备编号：","设备描述：","烧录版本：","操作人员ID：","SN码：","料号：","数据库校验值：","烧录文件校验值：","MD5值："
                };

                UserEvent.initMesUI += new UserEvent.InitMesUI(InitMesUI_XWD);
                UserEvent.getMesInfo += new UserEvent.GetMesInfo(GetMesInfo_XWD);
                UserEvent.startMesSystem += new UserEvent.StartMesSystem(StartMesSystem_XWD);
                UserEvent.saveSmtBurnLog += new UserEvent.SaveSmtBurnLog(_MESModule.SaveSmtBurnLog_XWD);
                UserEvent.getMesDate += new UserEvent.GetMesDate(GetMesDate_XWD);
                UserEvent.updateControls += new UserEvent.UpdateControls(UpdateControls_XWD);
            }
            else if (Mes.Type == GlobConstData.ST_MESTYPE_KJD)//快捷达
            {
                txtCount = 6;
                strLblText = new string[]
                {
                    "站点名称：","操作人员ID：","工单号：","料号：","数据库校验值：","烧录文件校验值："
                };

                UserEvent.initMesUI += new UserEvent.InitMesUI(InitMesUI_KJD);
                UserEvent.getMesInfo += new UserEvent.GetMesInfo(GetMesInfo_KJD);
                UserEvent.startMesSystem += new UserEvent.StartMesSystem(StartMesSystem_KJD);
                UserEvent.saveSmtBurnLog += new UserEvent.SaveSmtBurnLog(_MESModule.SaveSmtBurnLog_KJD);
                UserEvent.getMesDate += new UserEvent.GetMesDate(GetMesDate_KJD);
                UserEvent.updateControls += new UserEvent.UpdateControls(UpdateControls_KJD);
            }
            else if (Mes.Type == GlobConstData.ST_MESTYPE_XC)//协创
            {
                txtCount = 6;
                cobCount = 2;
                strLblText = new string[]
                {
                    "产品机型：","固件版本：","工单号：","芯片型号：","烧录文件校验值：","服务器校验值"
                };

                UserEvent.initMesUI += new UserEvent.InitMesUI(InitMesUI_XC);
                UserEvent.getMesInfo += new UserEvent.GetMesInfo(GetMesInfo_XC);
                UserEvent.startMesSystem += new UserEvent.StartMesSystem(StartMesSystem_XC);
                UserEvent.saveSmtBurnLog += new UserEvent.SaveSmtBurnLog(_MESModule.SaveSmtBurnLog_XC);
                UserEvent.getMesDate += new UserEvent.GetMesDate(GetMesDate_XC);
                UserEvent.updateControls += new UserEvent.UpdateControls(UpdateControls_XC);
            }
        }

        /// <summary>
        /// 控件使能以及布局调整
        /// </summary>
        private void SetControlState()
        {
            if (Mes.Type == GlobConstData.ST_MESTYPE_XWD)//欣旺达
            {
                for (int i = 0; i < 3; i++)
                {
                    txtMes[i].ReadOnly = false;
                }
                txtMes[4].ReadOnly = false;
                nudTargetNum.ReadOnly = true;
                txtMes[8].Width = 540;
            }
            else if (Mes.Type == GlobConstData.ST_MESTYPE_KJD)//快捷达
            {
                txtMes[0].ReadOnly = false;
                txtMes[2].ReadOnly = false;
            }
            else if (Mes.Type == GlobConstData.ST_MESTYPE_XC)//协创
            {
                cobMes[0].SelectedIndexChanged += new System.EventHandler(this.cobMes_SelectedIndexChanged);
                cobMes[1].SelectedIndexChanged += new System.EventHandler(this.cobMes_SelectedIndexChanged);
                cobMes[0].Visible = true;
                cobMes[1].Visible = true;
                txtMes[0].Visible = false;
                txtMes[1].Visible = false;
                txtMes[2].ReadOnly = false;
                label5.Visible = false;
                txtFilePath.Visible = false;
                btnBrowse2.Visible = false;
                btnBrowse3.Visible = false;
            }
        }

        /// <summary>
        /// 更新UI
        /// </summary>
        private void UpdateControls_XWD()
        {
            txtMes[7].Text = Mes.Checksum_File;
            txtMes[8].Text = Mes.MD5;
        }

        private void UpdateControls_KJD()
        {
            txtMes[5].Text = Mes.Checksum_File;
        }

        private void UpdateControls_XC()
        {
            txtMes[4].Text = Mes.Checksum_File;
            txtMes[1].Text = Mes.BuildVersion;
            txtMes[3].Text = Mes.Device;
        }

        private void btnBrowse1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dilog = new FolderBrowserDialog();
            dilog.Description = "请选择MultiAprog.exe所在路径";
            if (dilog.ShowDialog() == DialogResult.OK)
            {
                txtAppPath.Text = dilog.SelectedPath;
            }
        }

        private void btnBrowse2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dilog = new FolderBrowserDialog();
            dilog.Description = "请选择烧录文件所在目录";
            if (dilog.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = Mes.ProgFilePath = dilog.SelectedPath;
            }

            //OpenFileDialog dilog = new OpenFileDialog();
            //dilog.Title = "请选择烧录文件";
            //if (dilog.ShowDialog() == DialogResult.OK)
            //{
            //    txtFilePath.Text = dilog.FileName;
            //}
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            if (Mes.IsRun)
            {
                MessageBox.Show("MES系统运行中，请先中止再获取信息", "温馨提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            m_bMESDataReady = false;
            UserEvent evt = new UserEvent();
            evt.GetMesInfo_Click();
        }

        /// <summary>
        /// 获取快捷达MES信息
        /// </summary>
        private void GetMesInfo_KJD()
        {
            try
            {
                Mes.LotSN = txtMes[2].Text;
                if (string.IsNullOrEmpty(Mes.LotSN))
                {
                    MessageBox.Show("工单号不能为空", "温馨提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (!_dll_KJD.Get_ProgrammInfo(Mes.LotSN, out info_KJD, out string err))//debug为测试工单号
                {
                    MessageBox.Show("获取料号信息失败！错误信息：" + err);
                    return;
                }

                //Mes.ProgFilePath = "E:\\ProgramFile";
                Mes.ProgFileName = Path.Combine(Mes.ProgFilePath, info_KJD.FileName);
                if (!Directory.Exists(Mes.ProgFilePath))
                {
                    Directory.CreateDirectory(Mes.ProgFilePath);
                }
                Bytes_To_File(info_KJD.BinFile, Mes.ProgFileName);
                //更新控件显示
                txtMes[1].Text = info_KJD.Id.ToString();
                txtMes[3].Text = info_KJD.PnId.ToString();
                txtMes[4].Text = Mes.Checksum_Mes = info_KJD.CheckSum;

                txtFilePath.Text = Mes.ProgFilePath;
                txtFileName.Text = Mes.ProgFileName;

                m_bMESDataReady = true;
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "获取Mes数据成功!!!");
            }
            catch (Exception ex)
            {
                m_bMESDataReady = false;
                MessageBox.Show(ex.Message);
            }
            
        }

        /// <summary>
        /// 获取欣旺达MES信息
        /// </summary>
        private void GetMesInfo_XWD()
        {
            Mes.Version_File = txtMes[2].Text;
            Mes.LotSN = txtMes[4].Text;

            if (Mes.Version_File == "")
            {
                MessageBox.Show("版本号不能为空，请填入", "温馨提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else if (Mes.LotSN == "")
            {
                MessageBox.Show("SN码不能为空，请填入", "温馨提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            String strResp;
            if (!_MESModule.GetNumByLotSN_XWD(Mes.LotSN, out strResp))
            {
                //g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "获取Mes数据错误!!!", "Error");
                //MessageBox.Show("获取Mes数据错误!!!", "错误提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // 解析MES返回的数据
                if (!_MESModule.AnalyseLotSNResp_XWD(0, strResp, "GetNumByLotSNResponse", "GetNumByLotSNResult"))
                {
                    g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "解析MES返回数据失败", "Error");
                    return;
                }
                else
                {
                    txtMes[5].Text = Mes.ItemCode;
                    nudTargetNum.Value = Convert.ToInt32(Mes.Count);
                    // 检索烧录文档
                    if (!_MESModule.CheckProgFiles_XWD())
                    {
                        return;
                    }
                    else
                    {
                        txtMes[6].Text = Mes.Checksum_Mes;
                        txtFileName.Text = Mes.ProgFileName;
                        m_bMESDataReady = true;
                    }
                }
            }
        }

        /// <summary>
        /// 获取协创服务器信息
        /// </summary>
        public void GetMesInfo_XC()
        {
            try
            {
                InitMesInfo_XC();//初始化协创MES信息
                Mes.LotSN = txtMes[2].Text;
                if (string.IsNullOrEmpty(Mes.LotSN))
                {
                    MessageBox.Show("工单号不能为空", "温馨提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                int ind_name = 0, ind_Model = 0, ind_Version = 0;
                //string jsonPath = @"\\192.168.20.22\i\db.json";//共享JSON文件路径
                bool flag = false;
                if (!File.Exists(Mes.DatabasePath_XC))
                {
                    MessageBox.Show( "配置文件不存在：" + Mes.DatabasePath_XC, "温馨提示：", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string MESDate = File.ReadAllText(Mes.DatabasePath_XC, Encoding.Default);
                MESDate = MESDate.Insert(0, "{\"db\":");
                MESDate = MESDate.Insert(MESDate.Length, "}");
                JObject obj = Newtonsoft.Json.Linq.JObject.Parse(MESDate);

                //根据工单号获取烧录机型数组
                var result = obj["db"];
                bool flag_Check = false;
                if (result != null)
                {
                    int length = result.Count();
                    for (int i = 0; i < length; i++)
                    {

                        if (Mes.LotSN != obj["db"][i]["name"].ToString())//检查工单号
                        {
                            continue;
                        }
                        flag_Check = true;
                        //获取烧录机型组
                        for (int j = 0; j < ind_Model; j++)//如果烧录机型重复则跳过
                        {
                            if (MesInfo_XC.Model[j].Model == obj["db"][i]["model"].ToString())
                            {
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            continue;
                        }
                        MesInfo_XC.Model[ind_Model].Model = obj["db"][i]["model"].ToString();
                        ind_Model++;
                        MesInfo_XC.Count_Model = ind_Model;

                    }

                    if (!flag_Check)
                    {
                        string strMsg = "未能匹配到工单号：【" + Mes.LotSN + "】 请检查工单号是否填写错误...";
                        g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, strMsg);
                        MessageBox.Show(strMsg, "温馨提示：", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    //根据烧录机型获取版本号数组
                    for (int i = 0; i < ind_Model; i++)
                    {
                        ind_Version = 0;//复位烧录版本索引
                        for (int j = 0; j < length; j++)
                        {
                            if (Mes.LotSN != obj["db"][j]["name"].ToString())//检查工单号
                            {
                                continue;
                            }
                            if (MesInfo_XC.Model[i].Model != obj["db"][j]["model"].ToString())//检查烧录机型
                            {
                                continue;
                            }
                            //获取烧录版本和校验码
                            MesInfo_XC.Model[i].Version[ind_Version].Version = obj["db"][j]["Version"].ToString();
                            MesInfo_XC.Model[i].Version[ind_Version].Checksum = obj["db"][j]["checksum"].ToString();
                            ind_Version++;
                            MesInfo_XC.Model[i].Count_Version = ind_Version;
                        }
                    }
                }
                InitComboBox();
                m_bMESDataReady = true;
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "获取Mes数据成功!!!");
            }
            catch (Exception ex)
            {
                m_bMESDataReady = false;
                MessageBox.Show(ex.Message);
            }
            
        }

        /// <summary>
        /// 初始化下拉框选择项
        /// </summary>
        private void InitComboBox()
        {
            cobMes[0].Items.Clear();
            cobMes[1].Items.Clear();
            for (int i = 0; i < MesInfo_XC.Count_Model; i++)
            {
                cobMes[0].Items.Add(MesInfo_XC.Model[i].Model);
            }
            if (MesInfo_XC.Count_Model != 0)
            {
                cobMes[0].SelectedIndex = 0;
            }
            
            
        }

        /// <summary>
        /// 初始化协创MES信息
        /// </summary>
        private void InitMesInfo_XC()
        {
            cobMes[0].Items.Clear();
            cobMes[1].Items.Clear();
            MesInfo_XC.Count_Model = 0;
            for (int i = 0; i < 100; i++)
            {
                MesInfo_XC.Model[i].Count_Version = 0;
                MesInfo_XC.Model[i].Model = "";
                for (int j = 0; j < 100; j++)
                {
                    MesInfo_XC.Model[i].Version[j].Version = "";
                    MesInfo_XC.Model[i].Version[j].Checksum = "";
                }
            }
            
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            Mes.FuncMode = cobFuncMode.Text;
            Mes.Count = Convert.ToInt32(nudTargetNum.Value);
            Mes.Exit = cobMesExit.SelectedIndex;

            _MESModule.RstUID();
            if (!m_bMESDataReady)
            {
                string strErrMsg;
                string strText;
                strText = btnGet.Text;
                strErrMsg = string.Format("请先点击【{0:s}】按钮进行信息获取", strText);
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, strErrMsg, "Flow");
                MessageBox.Show(strErrMsg, "温馨提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            UserEvent evt = new UserEvent();
            if (!evt.StartMesSystem_Click())
            {
                return;
            }

            if (DialogResult.No == MessageBox.Show("目标产量为：" + Mes.Count.ToString() + "\r\n是否启动MES系统", 
                "温馨提示：", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
            {
                return;
            }
            
            PrintMesExitCondition();

            m_bMESIsOpen = true;
            btnStart.Enabled = false;
            if (!CheckDllPath())
            {
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "请确认MultiAprog.exe是否在" + m_Setting.strServerDir + "文件夹下", "Error");
                return;
            }

            //启动服务器
            var Ret = StartService();
            if (Ret == -1)//启动失败
            {
                return;
            }
            m_Worker = new Thread(DoTask);
            m_Worker.IsBackground = true;
            m_Worker.Start();

            return;
        }

        private bool StartMesSystem_KJD()
        {
            Mes.record_KJD.PositionNum = Mes.DeviceSN = txtMes[0].Text;
            Mes.record_KJD.JobNum = Mes.LotSN = txtMes[2].Text;
            Mes.record_KJD.Check_Sum = info_KJD.CheckSum;
            Mes.record_KJD.Pn_Id = info_KJD.PnId;
            Mes.record_KJD.FileName = info_KJD.FileName;
            Mes.record_KJD.JobProductionStartTime = DateTime.Now;
            Mes.record_KJD.Quantity = Mes.Count;
            Mes.record_KJD.UpdateBy = 0;

            if (string.IsNullOrEmpty(Mes.LotSN))
            {
                MessageBox.Show("工单号不能为空", "温馨提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(Mes.DeviceSN))
            {
                MessageBox.Show("站位名称不能为空", "温馨提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private bool StartMesSystem_XWD()
        {
            Mes.DeviceSN = txtMes[0].Text;
            Mes.DeviceDes = txtMes[1].Text;
            if (string.IsNullOrEmpty(Mes.DeviceSN))
            {
                MessageBox.Show("设备编号不能为空", "温馨提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(Mes.DeviceDes))
            {
                MessageBox.Show("设备描述不能为空", "温馨提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        private bool StartMesSystem_XC()
        {
            
            Mes.DeviceSN = cobMes[0].Text;
            Mes.Version = cobMes[1].Text;
            if (string.IsNullOrEmpty(Mes.DeviceSN))
            {
                MessageBox.Show("产品机型不能为空", "温馨提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            if (string.IsNullOrEmpty(Mes.Version))
            {
                MessageBox.Show("烧录版本不能为空", "温馨提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            List<FileInfo> lst = new List<FileInfo>();
            Mes.ProgFilePath = Mes.ProgFilePath_XC;//协创共享文件路径
            Mes.ProgFilePath = Path.Combine(Mes.ProgFilePath, Mes.DeviceSN, Mes.Version);
            Mes.lstFiles = GetFile(Mes.ProgFilePath, ".apr", lst);
            if (Mes.lstFiles.Count <= 0)
            {
                MessageBox.Show("请确认路径：" + Mes.ProgFilePath + "是否含有.apr烧录文档\r\n对接MES失败", "温馨提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            else if (Mes.lstFiles.Count >= 2)
            {
                _SelectProgFile.ShowDialog();
                txtFileName.Text = Mes.ProgFileName;
            }
            else
            {
                Mes.ProgFileName = txtFileName.Text = Mes.lstFiles[0].FullName;
            }
            g_act.CopyAKLog_XC();
            return true;
        }

        /// <summary>
        /// 获得目录下所有文件或指定文件类型文件(包含所有子文件夹)
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <param name="extName">扩展名可以多个 例如 .mp3.wma.rm</param>
        /// <returns>List<FileInfo></returns>
        public static List<FileInfo> GetFile(string path, string extName, List<FileInfo> lst)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    return lst;
                }
                string[] dir = Directory.GetDirectories(path); //文件夹列表  
                DirectoryInfo fdir = new DirectoryInfo(path);
                FileInfo[] file = fdir.GetFiles();
                //FileInfo[] file = Directory.GetFiles(path); //文件列表  
                if (file.Length != 0 || dir.Length != 0) //当前目录文件或文件夹不为空          
                {
                    foreach (FileInfo f in file) //显示当前目录所有文件  
                    {
                        if (extName.ToLower().IndexOf(f.Extension.ToLower()) >= 0)
                        {
                            lst.Add(f);
                        }
                    }
                    foreach (string d in dir)
                    {
                        GetFile(d, extName, lst);//递归  
                    }
                }
                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("将要中止对接MES系统", "是否继续？", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                m_bMESIsOpen = false;
                btnStart.Enabled = true;
            }
                
        }

        /// <summary>
        /// 清零烧录OK、NG数
        /// </summary>
        public void RstCount()
        {
            UserTask.OKDoneC = 0;
            UserTask.NGDoneC = 0;
            UserTask.TIC_DoneC = 0;
        }

        /// <summary>
        /// 打印MES退出条件
        /// </summary>
        public void PrintMesExitCondition()
        {
            m_Setting.strMesExitCond = cobMesExit.Text;
            g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "当前MES批量自动结束条件:" + m_Setting.strMesExitCond, "Flow");
            if (cobMesExit.SelectedIndex == GlobConstData.ST_MESEXIT_WITHOK)
            {
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "批量自动结束时OK数产值为:" + m_Setting.nTargetNum, "Flow");
            }
            else
            {
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "批量自动结束时OK+NG数产值为:" + m_Setting.nTargetNum, "Flow");
            }
        }

        /// <summary>
        /// 检查DLL路径
        /// </summary>
        /// <returns></returns>
        public bool CheckDllPath()
        {
            m_Setting.DllPath = Path.Combine(m_Setting.strServerDir, m_Setting.DllName);
            if (File.Exists(m_Setting.DllPath))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 数据写入文档（创建实例文档）
        /// </summary>
        /// <param name="buff">二进制数据</param>
        /// <param name="savepath">保存路径</param>
        public void Bytes_To_File(byte[] buff, string savepath)
        {
            if (System.IO.File.Exists(savepath))
            {
                System.IO.File.Delete(savepath);
            }

            FileStream fs = new FileStream(savepath, FileMode.CreateNew);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(buff, 0, buff.Length);
            bw.Close();
            fs.Close();
        }


        public void DoTask()
        {
            int Ret = 0;
            bool bRtn;
            int PreTotal = -1;
            string strMsg;
            
            int Total = 0, Pass = 0, Fail = 0;

            //设置消息处理函数
            Ret = AC_API.AC_SetMsgHandle(MsgEvent, IntPtr.Zero);

            //加载工程
            Ret = LoadProject();
            if (Ret != 1)
            {
                goto __end;
            }

            //获取昂科烧录工程文件基础信息
            if (!GetProjectInfo())
            {
                goto __end;
            }

            //比对校验值
            Ret = CompareChecksum();
            if (Ret != 0)
            {
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "批量生产终止", "Flow");
                goto __end;
            }


            g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "启动工程任务...", "Flow");
            bRtn = AC_API.AC_StartProject();
            if (!bRtn)
            {
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "启动工程任务失败", "Error");
                Ret = -1; goto __end;
            }
            Mes.IsRun = true;
            RstCount();
            SetRunTargetNum();
            g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "启动工程成功，可以进入自动化界面启动", "Flow");
            while (true)
            {
                //Thread.Sleep(200); ///两次取状态的间隔为200ms
                g_act.WaitDoEvent(200);
                if (!m_bMESIsOpen)
                {///客户需要退出
                    g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "操作员主动取消批量操作", "Flow");
                    break;
                }

                //更新烧录IC数
                Pass = UserTask.OKDoneC; Fail = UserTask.NGDoneC; Total = Fail + Pass;
                if (!bRtn)
                {
                    g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "获取状态结果失败", "Error");
                    break;
                }
                ///退出循环的条件应该自己判断，一般情况下是Total已经达到需要的总数
                if (PreTotal != Total)
                {///有可能两次取得的结果是一样的，不需要重复打印
                    strMsg = string.Format("当前烧录总数:{0:d}, 成功个数:{1:d}, 失败个数:{2:d}", Total, Pass, Fail);
                    g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, strMsg, "Flow");
                    PreTotal = Total;
                }

                if (Mes.Exit == GlobConstData.ST_MESEXIT_WITHOK)
                {
                    if (Pass >= Mes.Count)
                    {///只循环到Pass为Count截止。///打印最后一次
                        strMsg = string.Format("批量任务达成，生产总个数:{0:d}, 成功个数:{1:d}, 失败个数:{2:d}", Total, Pass, Fail);
                        g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, strMsg, "Flow");
                        break;
                    }
                }
                else
                {
                    if (Total >= Mes.Count)
                    {///循环到Pass+NG为Count截止。///打印最后一次
                        strMsg = string.Format("批量任务达成，生产总个数:{0:d}, 成功个数:{1:d}, 失败个数:{2:d}", Total, Pass, Fail);
                        g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, strMsg, "Flow");
                        break;
                    }
                }
            }
            

        __end:
            g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "结束工程任务中...", "Flow");
            if (AC_API.AC_StopService())
            {
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "工程任务结束", "Flow");
            }
            btnStart.Enabled = true;
            Mes.IsRun = false;
            g_act.CopyAKLog_XC();
        }

        /// <summary>
        /// 启动昂科烧录器
        /// </summary>
        /// <returns></returns>
        public int StartService()
        {
            int Ret = 0;
            bool bRtn;
            g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "启动服务器...", "Flow");
            bRtn = AC_API.AC_StartService();
            if (bRtn)
            {
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "启动服务器成功", "Flow");
                //Mes.IsRun = true;
            }
            else
            {
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "启动服务器失败", "Error");
                Ret = -1;
                btnStart.Enabled = true;
            }

            return Ret;
        }

        /// <summary>
        /// 加载烧录工程
        /// </summary>
        /// <returns></returns>
        public int LoadProject()
        {
            string strErrmsg;
            int Ret = 0, nRetCode;
            g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "加载工程...", "Flow");
            //Mes.ProgFileName = "C:\\Users\\Administrator\\Desktop\\11.apr";
            Ret = AC_API.AC_LoadProjectWithLot(Mes.ProgFileName, Mes.FuncMode, Mes.Count);

            if (Ret != 1)
            {
                strErrmsg = "加载工程失败!";
                Ret = -1;
                goto __end;
            }

            nRetCode = WaitJobDone();
            if (nRetCode != 1)
            {
                strErrmsg = "加载工程失败!";
                Ret = -1; goto __end;
            }
            else
            {
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "加载工程成功!", "Flow");
            }

        __end:
            if (Ret != 1)
            {
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "加载工程失败!", "Error");
            }
            return Ret;
        }

        /// <summary>
        /// 等待命令执行完成，对GetResult的封装
        /// </summary>
        /// <returns></returns>
        public int WaitJobDone()
        {
            int Ret = 0;
            bool bRtn;
            byte[] byteResult = new byte[64];
            while (true)
            {
                Thread.Sleep(300); ////300ms查询一次
                bRtn = AC_API.AC_GetResult(byteResult, 64);
                string str = Encoding.Default.GetString(byteResult);
                if (!bRtn)
                {
                    Ret = -1;
                    break;
                }
                if (string.Compare(str, "OK") == 0)
                {///命令执行成功
                    Ret = 1;
                    break;
                }
                else if (string.Compare(str, "ERROR") == 0)
                {///命令执行失败
                    g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "获取执行状态失败：" + byteResult, "Error");
                    break;
                }
            }

            return Ret;
        }

        
        /// <summary>
        /// 回调函数
        /// </summary>
        /// <param name="Para"></param>
        /// <param name="Msg"></param>
        /// <param name="MsgData"></param>
        /// <returns></returns>
        public int MsgEvent(IntPtr Para, string Msg, string MsgData)
        {
            if (string.Compare(Msg, "StatusChange") == 0)
            {
                //g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "StatusChang", "Flow");
            }
            else if (string.Compare(Msg, "YieldChange") == 0)
            {
                Task.Run(() =>
                {
                    lock (_objYieldChange)
                    {
                        g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "YieldChange", "Flow");
                        _MESModule.GetBurnData();
                        _MESModule.AnalyseYieldChangeData(MsgData);
                        UserEvent evt = new UserEvent();
                        evt.SaveSmtBurnLog_Click();
                    }
                });
                
            }
            else if (string.Compare(Msg, "UIDFetched") == 0)
            {
                Task.Run(() =>
                {
                    lock (_objUIDFetched)
                    {
                        g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "UIDFetched", "Flow");
                        _MESModule.AnalyseUIDFetchedData(MsgData);
                    }
                });
                
            }
            return 0;
        }

        /// <summary>
        /// 获取昂科烧录工程文件基础信息
        /// </summary>
        /// <returns></returns>
        private bool GetProjectInfo()
        {
            byte[] byteResult = new byte[5000];
            if (!AC_API.AC_GetProjectInfo_Json(byteResult, 5000, IntPtr.Zero))
            {
                string Msg = "获取昂科烧录工程文件基础信息失败！！！";
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, Msg, "Error");
                MessageBox.Show(Msg, "提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                string Result = Encoding.Default.GetString(byteResult);
                JObject obj = JObject.Parse(Result);
                Mes.Device = obj["ChipName"].ToString();
                Mes.BuildVersion = obj["BuildVersion"].ToString();
                Mes.MD5 = obj["Description"].ToString();
                Mes.Checksum_File = obj["CheckSum"].ToString();

                //更新控件
                UserEvent evt = new UserEvent();
                evt.UpdateControls_Click();
                return true;
            }
            
        }

        /// <summary>
        /// 比对校验值
        /// </summary>
        /// <returns></returns>
        public int CompareChecksum()
        {
            int Ret = 0;
            bool bRtn = false;
            string strChecksum = null;

            g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "比对校验值中...", "Flow");
            strChecksum = Mes.Checksum_File.Substring(2);//截取长度
            if (string.Compare(strChecksum, Mes.Checksum_Mes) != 0)
            {
                string Msg = "校验值比对错误: 烧录工程文件校验值为:" + strChecksum + ", MES系统校验值为:" + Mes.Checksum_Mes;
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, Msg, "Error");
                MessageBox.Show(Msg, "提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Ret = -1; goto __end;
            }
            else
            {
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "校验值比对成功", "Flow");
            }
        __end:
            return Ret;
        }

        /// <summary>
        /// 设置产量
        /// </summary>
        public void SetRunTargetNum()
        {
            if (cobMesExit.SelectedIndex == GlobConstData.ST_MESEXIT_WITHOK)
            {
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "批量自动结束时OK数产值为:" + Mes.Count, "Flow");
                UserTask.TargetC = Mes.Count;
                //定量开启
                Auto_Flag.Production = true;
                Auto_Flag.ProductionOK = true;
            }
            else
            {
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "批量自动结束时OK+NG数产值为:" + Mes.Count, "Flow");
                UserTask.TargetC = Mes.Count;
                //定量开启
                Auto_Flag.Production = true;
                Auto_Flag.ProductionOK = false;
            }

            UserEvent evt = new UserEvent();
            evt.GetMesDate_Click();
            Mes.FuncModeIndex = cobFuncMode.SelectedIndex.ToString();
            g_config.WriteMesDate();
            g_config.WriteFuncSwitVal();
            g_config.WriteStaticVal();
        }

        private void GetMesDate_XWD()
        {
            Mes.DeviceSN = txtMes[0].Text;
            Mes.DeviceDes = txtMes[1].Text;
        }

        private void GetMesDate_KJD()
        {
            Mes.DeviceSN = txtMes[0].Text;
        }

        private void GetMesDate_XC()
        {
            Mes.DeviceSN = txtMes[0].Text;
        }

        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="byteDatas"></param>
        /// <returns></returns>
        public string ToHexStrFromByte(byte[] byteDatas)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = byteDatas.Length - 1; i >= 0; i--)
            {
                builder.Append(string.Format("{0:X2}", byteDatas[i]));
            }
            return builder.ToString().Trim();

        }

        /// <summary>
        /// 打印日志信息
        /// </summary>
        public void PrintLogInfo(String msg)
        {
            if (txtMesLog.Lines.Length > 50)
                txtMesLog.Lines = txtMesLog.Lines.Skip(1).ToArray();
            txtMesLog.Text += Environment.NewLine;
            txtMesLog.AppendText(msg);
        }

        private void MesWnd_Load(object sender, EventArgs e)
        {
            _SelectProgFile = new SelectProgFile();
            _SelectProgFile.Owner = this;
            InitWndUI();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        public void InitWndUI()
        {
            txtAppPath.Text = m_Setting.strServerDir;
            txtFilePath.Text = Mes.ProgFilePath;
            nudTargetNum.Value = Mes.Count = UserTask.TargetC;
            __InitComboBtn();

            UserEvent evt = new UserEvent();
            evt.InitMesUI_Click();
        }

        private void InitMesUI_XWD()
        {
            txtMes[0].Text = Mes.DeviceSN;
            txtMes[1].Text = Mes.DeviceDes;
            txtMes[2].Text = Mes.Version_File;
            txtMes[3].Text = StaticInfo.TDUser;
        }

        private void InitMesUI_KJD()
        {
            txtMes[0].Text = Mes.DeviceSN;
        }

        private void InitMesUI_XC()
        {
            txtMes[0].Text = Mes.DeviceSN;
        }

        /// <summary>
        /// 初始化下拉框
        /// </summary>
        private void __InitComboBtn()
        {
            //默认执行命令
            cobFuncMode.Items.Clear();
            cobFuncMode.Items.Add("BlankCheck");
            cobFuncMode.Items.Add("Verify");
            cobFuncMode.Items.Add("Program");
            cobFuncMode.Items.Add("Erase");
            cobFuncMode.SelectedIndex = Convert.ToUInt16(Mes.FuncModeIndex);

            //MES结束判定条件
            cobMesExit.Items.Clear();
            cobMesExit.Items.Add("采用OK数作为批量自动结束条件");
            cobMesExit.Items.Add("采用OK+NG数作为批量自动结束条件");
            cobMesExit.SelectedIndex = Auto_Flag.ProductionOK == true ? 0 : 1;
        }

        private void MesWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void btnBrowse3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dilog = new OpenFileDialog();
            dilog.Title = "请选择烧录文件";
            if (dilog.ShowDialog() == DialogResult.OK)
            {
                txtFileName.Text = dilog.FileName;
            }
        }

        private void cobMes_SelectedIndexChanged(object sender, EventArgs e)
        {
            SkinComboBox SelectedBox = sender as SkinComboBox;
            int ind = Convert.ToInt32(SelectedBox.Tag);
            int ind_Model = cobMes[0].SelectedIndex;

            if (ind == 0)//烧录机型被选择
            {
                cobMes[1].Items.Clear();
                for (int i = 0; i < MesInfo_XC.Model[ind_Model].Count_Version; i++)
                {
                    cobMes[1].Items.Add(MesInfo_XC.Model[ind_Model].Version[i].Version);
                }
                if (MesInfo_XC.Model[ind_Model].Count_Version != 0)
                {
                    cobMes[1].SelectedIndex = 0;
                    Mes.Checksum_Mes = MesInfo_XC.Model[ind_Model].Version[0].Checksum.Replace(" ", "");
                    txtMes[5].Text = Mes.Checksum_Mes;
                }
            }
            else if (ind == 1)//烧录版本被选择
            {
                int ind_Version = cobMes[1].SelectedIndex;
                Mes.Checksum_Mes = MesInfo_XC.Model[ind_Model].Version[ind_Version].Checksum.Replace(" ", "");
                txtMes[5].Text = Mes.Checksum_Mes;
            }
            
            
        }
    }
}
