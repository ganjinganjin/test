using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using BAR.Commonlib.Utils;
using BAR.Commonlib.Connectors;
using System.Threading;
using HalconDotNet;
using CCWin.SkinClass;
using CCWin.SkinControl;
using System.IO;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using BAR.Windows;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using PLC;
using Spire.Xls.Core.Converter.General.Word;
using System.ComponentModel;
using BAR.CommonLib_v1._0;

namespace BAR.Commonlib
{
    public class Act
    {
        const int _MAX_FNAME = 512;

        [DllImport("kernel32")]//返回0表示失败，非0为成功
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        // 定义一个静态变量来保存类的实例
        private static Act _instance = null;
        // 定义一个标识确保线程同步
        private static readonly object _objPadLock = new object();
        private static readonly object _objCopyLock = new object();
        private static readonly object _objAlarmLock = new object();
        private static readonly object _objLogLock = new object();
        private static readonly object _objMesLogLock = new object();
        private static readonly object _objProgrammerLock = new object();
        private static readonly object _objDateLock = new object();

        public bool IsSoftExit;
        public bool IsCamSnapMode;
        public volatile bool IsSnapOver;

        public BAR MainWnd;
        public MesWnd mesWnd;
        public HTuple[] ArrWndID;
        public HObject[] ArrSourceImage;
        public HObject[] ArrSourceImageBuffer;
        public DHCameraUtil[] ArrDHCameraUtils;
        public HRCameraUtil[] ArrHRCameraUtils;

        public NetConnector NetLightCtrl;
        public SerialConector SerialLightCtrl;
        Config g_config = Config.GetInstance();
        HalconImgUtil _HalconUtil = HalconImgUtil.GetInstance();

        FileStream      _LogFileStream;
        StreamWriter    _LogWriter;
        FileStream      _MesLogFileStream;
        StreamWriter    _MesLogWriter;
        FileStream _ProgrammerFileStream;
        StreamWriter _ProgrammerWriter;
        StreamWriter _AlarmWriter;
        StreamWriter _DateWriter;

        public struct AlarmInfo
        {
            public String StrDate;
            public String StrTime;
            public String StrDetail;
        }
        public List<AlarmInfo> ArrRealAlarms;
        public List<AlarmInfo> ArrHisAlarms;

        

        ///<当前选择的相机
        public int ISelectCam;
        ///<当前打开的窗口
        public int ISelectWnd;

        public int DispLineType = GlobConstData.DISPLAYLINETYPE_CROSS;                 ///<画线的类型

        Act()
        {
            IsSoftExit = false;
            ISelectCam = -1;
            ISelectWnd = -1;
            IsSnapOver = false;
            NetLightCtrl = NetConnector.GetInstance("NetLight");
            SerialLightCtrl = SerialConector.GetInstance("Light");
            ArrWndID = new HTuple[2];
            ArrSourceImage = new HObject[2];
            ArrSourceImageBuffer = new HObject[4];
            //ArrDHCameraUtils = new DHCameraUtil[2];
            //ArrDHCameraUtils[GlobConstData.ST_CCDUP] = DHCameraUtil.GetInstance(GlobConstData.ST_CCDUP.ToString());
            //ArrDHCameraUtils[GlobConstData.ST_CCDDOWN] = DHCameraUtil.GetInstance(GlobConstData.ST_CCDDOWN.ToString());

            //ArrHRCameraUtils = new HRCameraUtil[2];
            //ArrHRCameraUtils[GlobConstData.ST_CCDUP] = HRCameraUtil.GetInstance(GlobConstData.ST_CCDUP.ToString());
            //ArrHRCameraUtils[GlobConstData.ST_CCDDOWN] = HRCameraUtil.GetInstance(GlobConstData.ST_CCDDOWN.ToString());

            ArrRealAlarms = new List<AlarmInfo>();
            ArrHisAlarms = new List<AlarmInfo>();

            _HalconUtil.ShowMsg = new HalconImgUtil.ShowMsgDelegate(ShowMsg);
        }
        public static Act GetInstance()
        {
            if (_instance == null)
            {
                lock (_objPadLock)
                {
                    if (_instance == null)
                    {
                        _instance = new Act();
                    }
                }
            }
            return _instance;
        }
        /// <summary>
        /// 初始化CCD相机
        /// </summary>
        public void InitCCD()
        {
            String[] ip;
            if (Config.CameraIP == GlobConstData.CameraIP_Fixed)
            {
                ip = new String[] { "169.254.231.101", "169.254.232.101" };
            }
            else
            {
                ip = new String[] { "169.254.232.1", "169.254.232.2" };
            }
            for (int ind = 0; ind < GlobConstData.ST_CCDCount; ind++)
            {
                if (Config.CameraType == GlobConstData.Camera_DH)
                {
                    ArrDHCameraUtils = new DHCameraUtil[2];
                    ArrDHCameraUtils[GlobConstData.ST_CCDUP] = DHCameraUtil.GetInstance(GlobConstData.ST_CCDUP.ToString());
                    ArrDHCameraUtils[GlobConstData.ST_CCDDOWN] = DHCameraUtil.GetInstance(GlobConstData.ST_CCDDOWN.ToString());

                    bool bRet = ArrDHCameraUtils[ind].InitDHCard();
                    if (!bRet)
                    {
                        throw (new Exception("Camera No found"));
                    }
                    ArrDHCameraUtils[ind].ICameraID = ind;
                    ArrDHCameraUtils[ind].StrIP = ip[ind];
                    string strErrorInfo = ArrDHCameraUtils[ind].OpenDHDevice();
                    GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, strErrorInfo, "Camera");
                    ArrDHCameraUtils[ind].AutoShutter("Off");
                    ArrDHCameraUtils[ind].AutoGain("Off");
                    ArrDHCameraUtils[ind].ShowMsg = new DHCameraUtil.ShowMsgDelegate(ShowMsg);
                }
                else
                {
                    ArrHRCameraUtils = new HRCameraUtil[2];
                    ArrHRCameraUtils[GlobConstData.ST_CCDUP] = HRCameraUtil.GetInstance(GlobConstData.ST_CCDUP.ToString());
                    ArrHRCameraUtils[GlobConstData.ST_CCDDOWN] = HRCameraUtil.GetInstance(GlobConstData.ST_CCDDOWN.ToString());

                    bool bRet = ArrHRCameraUtils[ind].InitHRCard();
                    if (!bRet)
                    {
                        throw (new Exception("Camera No found"));
                    }
                    ArrHRCameraUtils[ind].ICameraID = ind;
                    ArrHRCameraUtils[ind].StrIP = ip[ind];
                    string strErrorInfo = ArrHRCameraUtils[ind].OpenHRDevice();
                    GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, strErrorInfo, "Camera");
                    ArrHRCameraUtils[ind].GetImageSize();
                    ArrHRCameraUtils[ind].AutoShutter("Off");
                    ArrHRCameraUtils[ind].AutoGain("Off");
                    ArrHRCameraUtils[ind].ShowMsg = new HRCameraUtil.ShowMsgDelegate(ShowMsg);
                }
                
            }
        }

        public void ShowMsg(string strMsg)
        {
            GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, strMsg, "Camera");
        }

        /// <summary>
        /// 打开相机触发采集
        /// </summary>
        /// <param name="camID">相机ID</param>
        /// <param name="isChangeMode">是否切换模式</param>
        public void CCDSnap(int camID, bool isChangeMode = true)
        {
            int count = 0;
            __start:
            try
            {
                if (count == 2)
                {
                    GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "重启相机失败,请检查相机电源及网线");
                    return;
                }
                if (Config.CameraType == GlobConstData.Camera_DH)
                {
                    #region ---------------大恒相机--------------------
                    if (ISelectCam == GlobConstData.ST_CCDUP)
                    {
                        ArrDHCameraUtils[GlobConstData.ST_CCDDOWN].SelectedWnd = -1;
                        ArrDHCameraUtils[GlobConstData.ST_CCDUP].SelectedWnd = this.ISelectWnd;
                        if (!IsCamSnapMode && isChangeMode)
                        {
                            ArrDHCameraUtils[GlobConstData.ST_CCDUP].TriggerMode(true);
                            IsCamSnapMode = true;
                        }
                        IsSnapOver = false;
                        ArrDHCameraUtils[GlobConstData.ST_CCDUP].SoftTrigger();
                    }
                    else if (ISelectCam == GlobConstData.ST_CCDDOWN)
                    {
                        ArrDHCameraUtils[GlobConstData.ST_CCDUP].SelectedWnd = -1;
                        ArrDHCameraUtils[GlobConstData.ST_CCDDOWN].SelectedWnd = this.ISelectWnd;
                        if (!IsCamSnapMode && isChangeMode)
                        {
                            ArrDHCameraUtils[GlobConstData.ST_CCDDOWN].TriggerMode(true);
                            IsCamSnapMode = true;
                        }
                        IsSnapOver = false;
                        ArrDHCameraUtils[GlobConstData.ST_CCDDOWN].SoftTrigger();
                    }
                    #endregion
                }
                else
                {
                    #region ---------------------大华相机-------------------
                    if (ISelectCam == GlobConstData.ST_CCDUP)
                    {
                        ArrHRCameraUtils[GlobConstData.ST_CCDDOWN].SelectedWnd = -1;
                        ArrHRCameraUtils[GlobConstData.ST_CCDUP].SelectedWnd = this.ISelectWnd;
                        if (!IsCamSnapMode && isChangeMode)
                        {
                            ArrHRCameraUtils[GlobConstData.ST_CCDUP].TriggerMode(true);
                            IsCamSnapMode = true;
                        }
                        IsSnapOver = false;
                        ArrHRCameraUtils[GlobConstData.ST_CCDUP].SoftTrigger();
                    }
                    else if (ISelectCam == GlobConstData.ST_CCDDOWN)
                    {
                        ArrHRCameraUtils[GlobConstData.ST_CCDUP].SelectedWnd = -1;
                        ArrHRCameraUtils[GlobConstData.ST_CCDDOWN].SelectedWnd = this.ISelectWnd;
                        if (!IsCamSnapMode && isChangeMode)
                        {
                            ArrHRCameraUtils[GlobConstData.ST_CCDDOWN].TriggerMode(true);
                            IsCamSnapMode = true;
                        }
                        IsSnapOver = false;
                        ArrHRCameraUtils[GlobConstData.ST_CCDDOWN].SoftTrigger();
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "相机采图超时,重启相机" + "\r" + ex.Message);
                if (Config.CameraType == GlobConstData.Camera_DH)
                {
                    ArrDHCameraUtils[ISelectCam].InitDHCard();
                    string strErrorInfo = ArrDHCameraUtils[ISelectCam].OpenDHDevice();
                    GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, strErrorInfo, "Camera");
                    WaitDoEvent(700);
                }
                else
                {
                    ArrHRCameraUtils[ISelectCam].InitHRCard();
                    string strErrorInfo = ArrHRCameraUtils[ISelectCam].OpenHRDevice();
                    GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, strErrorInfo, "Camera");
                    WaitDoEvent(700);
                }
                count++;
                goto __start;
            }
        }
        /// <summary>
        /// 打开相机连续采集
        /// </summary>
        /// <param name="camID">相机ID</param>
        /// <param name="isChangeMode">是否切换模式</param>
        public void CCDCap(int camID, bool isChangeMode = false)
        {
            int count = 0;
            __start:
            try
            {
                if (count == 2)
                {
                    GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "重启相机失败,请检查相机电源及网线");
                    return;
                }
                if (Config.CameraType == GlobConstData.Camera_DH)
                {
                    #region ---------------------大恒相机--------------------
                    if (camID == GlobConstData.ST_CCDUP)
                    {
                        if (ISelectCam == GlobConstData.ST_CCDUP && !isChangeMode) { ArrDHCameraUtils[GlobConstData.ST_CCDUP].SelectedWnd = this.ISelectWnd; return; }
                        if (!isChangeMode)
                        {
                            ArrDHCameraUtils[GlobConstData.ST_CCDDOWN].StopSnap();
                            Thread.Sleep(10);
                            CCDSnap(GlobConstData.ST_CCDDOWN);
                            ArrDHCameraUtils[GlobConstData.ST_CCDDOWN].SelectedWnd = -1;
                            ArrDHCameraUtils[GlobConstData.ST_CCDUP].SelectedWnd = this.ISelectWnd;
                        }
                        ArrDHCameraUtils[GlobConstData.ST_CCDUP].TriggerMode(false);
                        IsCamSnapMode = false;
                        ArrDHCameraUtils[GlobConstData.ST_CCDUP].StartGrap();
                        ISelectCam = GlobConstData.ST_CCDUP;
                    }
                    else if (camID == GlobConstData.ST_CCDDOWN)
                    {
                        if (ISelectCam == GlobConstData.ST_CCDDOWN && !isChangeMode) { ArrDHCameraUtils[GlobConstData.ST_CCDDOWN].SelectedWnd = this.ISelectWnd; return; }
                        if (!isChangeMode)
                        {
                            ArrDHCameraUtils[GlobConstData.ST_CCDUP].StopSnap();
                            Thread.Sleep(10);
                            CCDSnap(GlobConstData.ST_CCDUP);
                            ArrDHCameraUtils[GlobConstData.ST_CCDUP].SelectedWnd = -1;
                            ArrDHCameraUtils[GlobConstData.ST_CCDDOWN].SelectedWnd = this.ISelectWnd;
                        }
                        ArrDHCameraUtils[GlobConstData.ST_CCDDOWN].TriggerMode(false);
                        IsCamSnapMode = false;
                        ArrDHCameraUtils[GlobConstData.ST_CCDDOWN].StartGrap();
                        ISelectCam = GlobConstData.ST_CCDDOWN;
                    }
                    #endregion
                }
                else
                {
                    #region ---------------------大华相机-------------------
                    if (camID == GlobConstData.ST_CCDUP)
                    {
                        if (ISelectCam == GlobConstData.ST_CCDUP && !isChangeMode) { ArrHRCameraUtils[GlobConstData.ST_CCDUP].SelectedWnd = this.ISelectWnd; return; }
                        if (!isChangeMode)
                        {
                            ArrHRCameraUtils[GlobConstData.ST_CCDDOWN].StopSnap();
                            Thread.Sleep(10);
                            CCDSnap(GlobConstData.ST_CCDDOWN);
                            ArrHRCameraUtils[GlobConstData.ST_CCDDOWN].SelectedWnd = -1;
                            ArrHRCameraUtils[GlobConstData.ST_CCDUP].SelectedWnd = this.ISelectWnd;
                        }
                        ArrHRCameraUtils[GlobConstData.ST_CCDUP].TriggerMode(false);
                        IsCamSnapMode = false;
                        ArrHRCameraUtils[GlobConstData.ST_CCDUP].StartGrap();
                        ISelectCam = GlobConstData.ST_CCDUP;
                    }
                    else if (camID == GlobConstData.ST_CCDDOWN)
                    {
                        if (ISelectCam == GlobConstData.ST_CCDDOWN && !isChangeMode) { ArrHRCameraUtils[GlobConstData.ST_CCDDOWN].SelectedWnd = this.ISelectWnd; return; }
                        if (!isChangeMode)
                        {
                            ArrHRCameraUtils[GlobConstData.ST_CCDUP].StopSnap();
                            Thread.Sleep(10);
                            CCDSnap(GlobConstData.ST_CCDUP);
                            ArrHRCameraUtils[GlobConstData.ST_CCDUP].SelectedWnd = -1;
                            ArrHRCameraUtils[GlobConstData.ST_CCDDOWN].SelectedWnd = this.ISelectWnd;
                        }
                        ArrHRCameraUtils[GlobConstData.ST_CCDDOWN].TriggerMode(false);
                        IsCamSnapMode = false;
                        ArrHRCameraUtils[GlobConstData.ST_CCDDOWN].StartGrap();
                        ISelectCam = GlobConstData.ST_CCDDOWN;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "相机采图超时,重启相机" + "\r" + ex.Message);
                if (Config.CameraType == GlobConstData.Camera_DH)
                {
                    ArrDHCameraUtils[camID].InitDHCard();
                    string strErrorInfo = ArrDHCameraUtils[camID].OpenDHDevice();
                    GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, strErrorInfo, "Camera");
                    WaitDoEvent(700);
                }
                else
                {
                    ArrHRCameraUtils[camID].InitHRCard();
                    string strErrorInfo = ArrHRCameraUtils[camID].OpenHRDevice();
                    GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, strErrorInfo, "Camera");
                    WaitDoEvent(700);
                }
                count++;
                goto __start;
            }
            
        }

        /// <summary>
        /// 软触发下设置On后不拉流
        /// </summary>
        public void TriggerMode()
        {
            try
            {
                if (Config.CameraType == GlobConstData.Camera_DH)
                {
                    ArrDHCameraUtils[GlobConstData.ST_CCDUP].TriggerMode(true);
                    ArrDHCameraUtils[GlobConstData.ST_CCDDOWN].TriggerMode(true);
                }
                else
                {
                    ArrHRCameraUtils[GlobConstData.ST_CCDUP].TriggerMode(true);
                    ArrHRCameraUtils[GlobConstData.ST_CCDDOWN].TriggerMode(true);
                }
            }
            catch (Exception ex)
            {
                GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "停止上相机采集流失败" + "\r" + ex.Message);
            }
        }

        /// <summary>
        /// 发送光源指令
        /// </summary>
        /// <param name="lightID">光源ID</param>
        /// <param name="data">光源亮度</param>
        public bool SendCmd(int lightID, int data)
        {
            bool ret = false;
            if (g_config.ILightCType == GlobConstData.LightCtl_WAN)
            {
                if (g_config.ILightType == 1)
                {
                    String str = "";
                    if (lightID == 0)
                        str = String.Format("SA{0:D4}#", data);
                    else if (lightID == 1)
                        str = String.Format("SB{0:D4}#", data);
                    char cr = Convert.ToChar(0x0D);
                    char lf = Convert.ToChar(0x0A);
                    str += cr;
                    str += lf;
                    byte[] msg = Encoding.ASCII.GetBytes(str);
                    ret = NetLightCtrl.SendUDP(msg, msg.Length);
                    WaitDoEvent(5);
                }
                else if (g_config.ILightType == 2)
                {
                    String str = "", str2 = "";
                    if (lightID == 0)
                    {
                        str = String.Format("$310{0:x2}", data);
                    }
                    else if (lightID == 1)
                    {
                        str = String.Format("$320{0:x2}", data);
                    }
                    char ed = Convert.ToChar(0x00);
                    for (int n = 0; n < str.Length; n++)
                    {
                        ed ^= str.ToCharArray()[n];
                    }
                    str2 = String.Format("{0:x2}", ed.ToInt32());
                    str = str + str2;
                    byte[] msg = Encoding.ASCII.GetBytes(str);
                    ret = NetLightCtrl.SendUDP(msg, msg.Length);
                    WaitDoEvent(5);
                }
                else if (g_config.ILightType == 3)//创视
                {
                    String str = "";
                    if (g_config.PenType == 0 )
                    {
                        if (lightID == 0)
                            str = String.Format("WA{0:D4}TB{1:D4}TC{2:D4}TD{3:D4}T", data, 0, 0, 0);
                        else if (lightID == 1)
                            str = String.Format("WA{0:D4}TB{1:D4}TC{2:D4}TD{3:D4}T", 0, data, data, data);
                    }
                    else
                    {
                        if (lightID == 0)
                            str = String.Format("WA{0:D4}TB{1:D4}TC{2:D4}TD{3:D4}T", data, 0, 0, 0);
                        else if (lightID == 1)
                            str = String.Format("WA{0:D4}TB{1:D4}TC{2:D4}TD{3:D4}T", 0, data, data, data);
                    }
                    

                    byte[] msg = Encoding.ASCII.GetBytes(str);
                    ret = NetLightCtrl.SendUDP(msg, msg.Length);
                    WaitDoEvent(5);
                }
            }
            else
            {
                if (g_config.ILightType == 1)
                {
                    String str = "";
                    if (lightID == 0)
                        str = String.Format("SA{0:D4}#", data);
                    else if (lightID == 1)
                        str = String.Format("SB{0:D4}#", data);
                    char cr = Convert.ToChar(0x0D);
                    char lf = Convert.ToChar(0x0A);
                    str += cr;
                    str += lf;
                    ret = SerialLightCtrl.SendData(str);
                    WaitDoEvent(5);
                }
                else if (g_config.ILightType == 2)
                {
                    String str = "", str2 = "";
                    if (lightID == 0)
                    {
                        str = String.Format("$310{0:x2}", data);
                    }
                    else if (lightID == 1)
                    {
                        str = String.Format("$320{0:x2}", data);
                    }
                    char ed = Convert.ToChar(0x00);
                    for (int n = 0; n < str.Length; n++)
                    {
                        ed ^= str.ToCharArray()[n];
                    }
                    str2 = String.Format("{0:x2}", ed.ToInt32());
                    str = str + str2;
                    ret = SerialLightCtrl.SendData(str);
                    WaitDoEvent(5);
                }
                else if (g_config.ILightType == 3)//创视
                {
                    String str = "";
                    if (lightID == 0)
                        str = String.Format("SA{0:D4}T"+ "B0000FC#", data);
                    else if (lightID == 1)
                        str = String.Format("SA0000F"+"B{0:D4}TC#", data);
                    ret = SerialLightCtrl.SendData(str);
                    WaitDoEvent(5);
                }
            }


            return ret;
        }

        /// <summary>
        /// 获得图像偏移
        /// </summary>
        /// <param name="ind">查找的模板匹配类型</param>
        /// <param name="dx">X方向偏移</param>
        /// <param name="dy">Y方向偏移</param>
        /// <param name="dr">偏移角度</param>
        /// <returns>查找是否成功</returns>
        public bool FindIndexImage(int ind, ref double dx, ref double dy, ref double dr)
        {
            HTuple resYc, resXc, resAng, resScore;
            double temDX, temDY, temDR, temDS;
            bool retBool;
            g_config.IsDispMatch = true;
            if (ind == GlobConstData.ST_MODELICPOS_NCC)
            {
                retBool = _HalconUtil.NCCModelPar(g_config.IsDispMatch, ArrWndID[ISelectWnd], ArrSourceImage[ISelectCam], g_config.ArrModID[ind],
                g_config.ArrMod[ind].SeachBox.Cy, g_config.ArrMod[ind].SeachBox.Cx, g_config.ArrMod[ind].SeachBox.Phi, g_config.ArrMod[ind].SeachBox.len1, g_config.ArrMod[ind].SeachBox.len2,
                g_config.DScore, g_config.DAngLimit, g_config.DMaxOverlap, 1, out resYc, out resXc, out resAng, out resScore);
            }
            else
            {
                retBool = _HalconUtil.FindModelPar(g_config.IsDispMatch, ArrWndID[ISelectWnd], ArrSourceImage[ISelectCam], g_config.ArrModID[ind],
                g_config.ArrMod[ind].SeachBox.Cy, g_config.ArrMod[ind].SeachBox.Cx, g_config.ArrMod[ind].SeachBox.Phi, g_config.ArrMod[ind].SeachBox.len1, g_config.ArrMod[ind].SeachBox.len2,
                g_config.DScore, g_config.DAngLimit, g_config.DMaxOverlap, g_config.DGreediness, 1, out resYc, out resXc, out resAng, out resScore);
            }
            

            if (retBool)
            {
                temDX = resXc.D;
                temDY = resYc.D;
                temDR = resAng.D;
                temDS = resScore.D;

                int dirX,dirY,dirR;
                if (g_config.ArrCamDir[0] == 0)
                    dirX = 1;
                else
                    dirX = -1;
                if (g_config.ArrCamDir[1] == 0)
                    dirY = 1;
                else
                    dirY = -1;
                if (g_config.ArrCamDir[2] == 0)
                    dirR = 1;
                else
                    dirR = -1;

                if (ind == GlobConstData.ST_MODELICPOS_NCC)
                {
                    dx = (temDX - g_config.DModelCx2) * g_config.ArrCCDPrec[GlobConstData.ST_MODELPENACC].X * dirX;
                    dy = (temDY - g_config.DModelCy2) * g_config.ArrCCDPrec[GlobConstData.ST_MODELPENACC].Y * dirY;
                    dr = dirR * (temDR - g_config.DModelCr2);
                }
                else
                {
                    dx = (temDX - g_config.DModelCx) * g_config.ArrCCDPrec[GlobConstData.ST_MODELPENACC].X * dirX;
                    dy = (temDY - g_config.DModelCy) * g_config.ArrCCDPrec[GlobConstData.ST_MODELPENACC].Y * dirY;
                    dr = dirR * (temDR - g_config.DModelCr);
                }

                String str = String.Format("Y:{0:f1},X:{1:f1},A:{2:f1},S:{3:f1}", dx, dy, dr, temDS);
                _HalconUtil.DrawTxt(ArrWndID[ISelectWnd], 50, 50, str, "green");

                resXc?.Dispose();
                resYc?.Dispose();
                resAng?.Dispose();
                resScore?.Dispose();
                retBool = true;
            }
            else
            {
                dx = 0;
                dy = 0;
                dr = 0;
                retBool = false;
            }
            resXc?.Dispose();
            resYc?.Dispose();
            resAng?.Dispose();
            resScore?.Dispose();
            return retBool;
        }

        /// <summary>
        /// 获得图像偏移
        /// </summary>
        /// <param name="ind">查找的模板匹配类型</param>
        /// <param name="dx">X方向偏移</param>
        /// <param name="dy">Y方向偏移</param>
        /// <param name="dr">偏移角度</param>
        /// <returns>查找是否成功</returns>
        public bool FindIndexImage(int ind, int ImageIndex, ref double dx, ref double dy, ref double dr)
        {
            HTuple resYc, resXc, resAng, resScore;
            double temDX, temDY, temDR, temDS;
            bool retBool;
            g_config.IsDispMatch = true;
            if (ind == GlobConstData.ST_MODELICPOS_NCC)
            {
                retBool = _HalconUtil.NCCModelPar(g_config.IsDispMatch, ArrWndID[ISelectWnd], ArrSourceImageBuffer[ImageIndex], g_config.ArrModID[ind],
                g_config.ArrMod[ind].SeachBox.Cy, g_config.ArrMod[ind].SeachBox.Cx, g_config.ArrMod[ind].SeachBox.Phi, g_config.ArrMod[ind].SeachBox.len1, g_config.ArrMod[ind].SeachBox.len2,
                g_config.DScore, g_config.DAngLimit, g_config.DMaxOverlap, 1, out resYc, out resXc, out resAng, out resScore);
            }
            else
            {
                retBool = _HalconUtil.FindModelPar(g_config.IsDispMatch, ArrWndID[ISelectWnd], ArrSourceImageBuffer[ImageIndex], g_config.ArrModID[ind],
                g_config.ArrMod[ind].SeachBox.Cy, g_config.ArrMod[ind].SeachBox.Cx, g_config.ArrMod[ind].SeachBox.Phi, g_config.ArrMod[ind].SeachBox.len1, g_config.ArrMod[ind].SeachBox.len2,
                g_config.DScore, g_config.DAngLimit, g_config.DMaxOverlap, g_config.DGreediness, 1, out resYc, out resXc, out resAng, out resScore);
            }


            if (retBool)
            {
                temDX = resXc.D;
                temDY = resYc.D;
                temDR = resAng.D;
                temDS = resScore.D;

                int dirX, dirY, dirR;
                if (g_config.ArrCamDir[0] == 0)
                    dirX = 1;
                else
                    dirX = -1;
                if (g_config.ArrCamDir[1] == 0)
                    dirY = 1;
                else
                    dirY = -1;
                if (g_config.ArrCamDir[2] == 0)
                    dirR = 1;
                else
                    dirR = -1;

                if (ind == GlobConstData.ST_MODELICPOS_NCC)
                {
                    dx = (temDX - g_config.DModelCx2) * g_config.ArrCCDPrec[GlobConstData.ST_MODELPENACC].X * dirX;
                    dy = (temDY - g_config.DModelCy2) * g_config.ArrCCDPrec[GlobConstData.ST_MODELPENACC].Y * dirY;
                    dr = dirR * (temDR - g_config.DModelCr2);
                }
                else
                {
                    dx = (temDX - g_config.DModelCx) * g_config.ArrCCDPrec[GlobConstData.ST_MODELPENACC].X * dirX;
                    dy = (temDY - g_config.DModelCy) * g_config.ArrCCDPrec[GlobConstData.ST_MODELPENACC].Y * dirY;
                    dr = dirR * (temDR - g_config.DModelCr);
                }

                String str = String.Format("X:{0:f1},Y:{1:f1},A:{2:f1},S:{3:f1}", dx, dy, dr, temDS);
                _HalconUtil.DrawTxt(ArrWndID[ISelectWnd], 50, 50, str, "green");
                str = String.Format("X:{0:f3},Y:{1:f3},A:{2:f3},S:{3:f3}", temDX, temDY, temDR, temDS);
                _HalconUtil.DrawTxt(ArrWndID[ISelectWnd], 100, 50, str, "green");

                resXc?.Dispose();
                resYc?.Dispose();
                resAng?.Dispose();
                resScore?.Dispose();
                retBool = true;
            }
            else
            {
                dx = 0;
                dy = 0;
                dr = 0;
                retBool = false;
            }
            resXc?.Dispose();
            resYc?.Dispose();
            resAng?.Dispose();
            resScore?.Dispose();
            return retBool;
        }
        /// <summary>
        /// CCD图像处理流程
        /// </summary>
        /// <param name="dx">X方向偏移</param>
        /// <param name="dy">Y方向偏移</param>
        /// <param name="dr">偏移角度</param>
        /// <returns>查找是否成功</returns>
        public bool CCDProccess(ref double dx, ref double dy, ref double dr, double penAngle = 0)
        {
            GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "CCD处理流程开始");
            while (!IsSoftExit)
            {
                int ModelIndex = Auto_Flag.Enabled_NCCModel == true ? GlobConstData.ST_MODELICPOS_NCC : GlobConstData.ST_MODELICPOS;
                bool ret;
                ret = SendCmd(GlobConstData.ST_LIGHTDOWN, g_config.ArrLampBox[ModelIndex].ILightDown);
                if (!ret)
                {
                    GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "下光源打开失败", "error");
                }

                Thread.Sleep(g_config.ISnapDelay);

                CCDSnap(GlobConstData.ST_CCDDOWN);
                GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "相机第一次拍照采图");
                int findFailCount = 0;
                DateTime StTime1 = DateTime.Now;
                while (!IsSnapOver && !IsSoftExit)
                {
                    Thread.Sleep(20);
                    if ((DateTime.Now - StTime1).TotalMilliseconds > 1000)
                    {
                        findFailCount++;
                        if (findFailCount > 2)
                        {
                            findFailCount = 0;
                            GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "相机采集图象超时", "error");
                            if (MessageBox.Show("[YES]等待 [NO]中止程序", "相机采集图象超时,是否继续等待", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                StTime1 = DateTime.Now;
                                CCDSnap(GlobConstData.ST_CCDDOWN);
                            }
                            else
                            {
                                GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "相机采集图象失败,中止", "error");
                                return false;
                            }
                        }
                        else
                        {
                            GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "相机采图超时,重启相机");
                            if (Config.CameraType == GlobConstData.Camera_DH)
                            {
                                ArrDHCameraUtils[GlobConstData.ST_CCDDOWN].InitDHCard();
                                string strErrorInfo = ArrDHCameraUtils[GlobConstData.ST_CCDDOWN].OpenDHDevice();
                                GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, strErrorInfo, "Camera");
                            }
                            else
                            {
                                ArrHRCameraUtils[GlobConstData.ST_CCDDOWN].InitHRCard();
                                string strErrorInfo = ArrHRCameraUtils[GlobConstData.ST_CCDDOWN].OpenHRDevice();
                                GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, strErrorInfo, "Camera");
                            }
                            CCDCap(GlobConstData.ST_CCDDOWN, true);
                            WaitDoEvent(700);
                            GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "相机再次采集图象");
                            //再次发送采图命令
                            CCDSnap(GlobConstData.ST_CCDDOWN);
                            StTime1 = DateTime.Now;
                        }
                    }
                }
                //模板匹配图象
                GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "相机采集图象完成");
                 if (Auto_Flag.Enabled_LayICPos)
                {
                    if ((int)penAngle == 90)
                    {
                        ModelIndex = GlobConstData.ST_MODELICPOS_BACK1;
                    }
                    else if ((int)penAngle == -90)
                    {
                        ModelIndex = GlobConstData.ST_MODELICPOS_BACK2;
                    }
                    else if ((int)penAngle == 180) 
                    {
                        ModelIndex = GlobConstData.ST_MODELICPOS_BACK3;
                    }
                }
                bool retBool = FindIndexImage(ModelIndex, ref dx, ref dy, ref dr);
                ArrSourceImage[ISelectCam].Dispose();
                if (retBool)
                {
                    return true;
                }
                else
                {
                    dx = 0;
                    dy = 0;
                    dr = 0;
                    return false;
                }

            }
            return false;
        }

        /// <summary>
        /// CCD飞拍图像处理流程
        /// </summary>
        public void CCDProccess_Fast(int index, double penAngle = 0)
        {
            int indexbuf = index;
            if (Config.CCDModel == GlobConstData.CCDModel_Slow)
            {
                return;
            }
            if (Auto_Flag.AutoRunBusy)
            {
                double retX = 0, retY = 0, retR = 0;
                bool ret;
                string str;
                int ModelIndex = Auto_Flag.Enabled_NCCModel == true ? GlobConstData.ST_MODELICPOS_NCC : GlobConstData.ST_MODELICPOS;
                if (Auto_Flag.Enabled_LayICPos)
                {
                    if (penAngle == 90)
                    {
                        ModelIndex = GlobConstData.ST_MODELICPOS_BACK1;
                    }
                    else if (penAngle == -90)
                    {
                        ModelIndex = GlobConstData.ST_MODELICPOS_BACK2;
                    }
                    else if (penAngle == 180)
                    {
                        ModelIndex = GlobConstData.ST_MODELICPOS_BACK3;
                    }
                }
                ret = FindIndexImage(ModelIndex, index, ref retX, ref retY, ref retR);

                if (ret)
                {
                    Axis.rectify[indexbuf].AxisX = -retX;
                    Axis.rectify[indexbuf].AxisY = retY + Axis.Pen[0].LowCamera_Y - Axis.Pen[indexbuf].LowCamera_Y;
                    UserTask.RotateTrigge(false, indexbuf, retR);
                }
                Axis.Pen[indexbuf].Image_Busy = false;
                Axis.Pen[indexbuf].ImageResult = ret;
                indexbuf++;
                if (ret)
                {
                    str = "吸笔[" + indexbuf + "]纠偏成功[X：" + (-retX).ToString("f3") + "mm，Y：" + retY.ToString("f3") + "mm，C：" + retR.ToString("f2") + "°]";

                }
                else
                {
                    str = "吸笔[" + indexbuf + "]纠偏失败";
                }
                GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
            }
        }

        public void WaitDoEvent(int delayTime)
        {
            DateTime dt1 = DateTime.Now;
            while ((DateTime.Now - dt1).TotalMilliseconds < delayTime) Application.DoEvents();
        }
        

        /// <summary>
        /// 记录告警信息
        /// </summary>
        /// <param name="msg">告警信息</param>
        public void RecordAlarmsInfo(String msg)
        {
            String alarmMsg;
            lock (_objAlarmLock)
            {
                DateTime curTime = DateTime.Now;
                String path = AppDomain.CurrentDomain.BaseDirectory;
                if (!path.IsNullOrEmpty())
                {
                    path = AppDomain.CurrentDomain.BaseDirectory + @"Log\" + curTime.ToString("yyyyMMdd") + "\\" + "AlarmLog.log";
                    if (!File.Exists(path))
                    {
                        FileStream fs = File.Create(path);
                        fs.Close();
                    }
                    if (File.Exists(path))
                    {
                        _AlarmWriter = new StreamWriter(path, true, System.Text.Encoding.Default);
                        alarmMsg = curTime.ToString("yyyyMMdd HH:mm:ss") + " " + msg;
                        _AlarmWriter.WriteLine(alarmMsg);
                        _AlarmWriter.Flush();
                        _AlarmWriter.Close();
                    }
                }
            }
        }

        public void RecordDateInfo(int Axis, String msg)
        {
            //String alarmMsg;
            //lock (_objDateLock)
            //{
            //    DateTime curTime = DateTime.Now;
            //    String path = AppDomain.CurrentDomain.BaseDirectory;
            //    if (!path.IsNullOrEmpty())
            //    {
            //        path = AppDomain.CurrentDomain.BaseDirectory + @"Log\" + curTime.ToString("yyyyMMdd") + "\\" + "Z"+ Axis + ".log";
            //        if (!File.Exists(path))
            //        {
            //            FileStream fs = File.Create(path);
            //            fs.Close();
            //        }
            //        if (File.Exists(path))
            //        {
            //            _DateWriter = new StreamWriter(path, true, System.Text.Encoding.Default);
            //            alarmMsg = curTime.ToString("yyyyMMdd HH:mm:ss") + "," + msg;
            //            _DateWriter.WriteLine(alarmMsg);
            //            _DateWriter.Flush();
            //            _DateWriter.Close();
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 记录烧录器烧录信息
        /// </summary>
        /// <param name="msg">烧录信息</param>
        /// <param name="msgType">信息类型</param>
        public void RecordProgrammerInfo(String msg, String msgType = "Recv")
        {
            //lock (_objProgrammerLock)
            //{
            //    DateTime curTime = DateTime.Now;
            //    String path = AppDomain.CurrentDomain.BaseDirectory;
            //    string file;
            //    if (string.IsNullOrEmpty(Mes.LotSN))
            //    {
            //        file = curTime.ToString("yyyyMMdd");
            //    }
            //    else
            //    {
            //        file = Mes.LotSN;
            //    }
            //    string LogDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log", file);
            //    String logIni = Path.Combine(LogDir, "Burn" + "_" + file + ".log");
            //    if (!path.IsNullOrEmpty())
            //    {
            //        if (!Directory.Exists(LogDir))
            //        {
            //            Directory.CreateDirectory(LogDir);
            //        }

            //        if (!File.Exists(logIni))
            //        {
            //            _ProgrammerFileStream = File.Create(logIni);
            //            _ProgrammerFileStream.Close();
            //            _ProgrammerWriter = new StreamWriter(logIni, true, System.Text.Encoding.Default);
            //        }
            //        if (File.Exists(logIni))
            //        {
            //            if (_ProgrammerWriter == null) _ProgrammerWriter = new StreamWriter(logIni, true, System.Text.Encoding.Default);
            //            String programmerMsg = curTime.ToString("yyyy-MM-dd  HH:mm:ss:fff") + "[" + msgType + "]" + " " + msg;
            //            _ProgrammerWriter.WriteLine(programmerMsg);
            //            _ProgrammerWriter.Flush();
            //        }
            //    }
            //}

        }
        
        /// <summary>
        /// 生成打印日志信息
        /// </summary>
        /// <param name="logLev">log等级</param>
        /// <param name="msg">日志信息</param>
        /// <param name="fileName">log文件名</param>
        /// <param name="msgType">信息类型</param>
        /// <param name="threadType">执行线程</param>
        public void GenLogMessage(int logLev, String msg, String msgType = "Flow", String threadType = "Main", String fileName = "log")
        {
            if (g_config.IsSwWorkFlowInfo == false)
            {
                lock (_objLogLock)
                {
                    msg = MultiLanguage.GetString(msg);
                    DateTime curTime = DateTime.Now;
                    String path = AppDomain.CurrentDomain.BaseDirectory;
                    string file;
                    if (string.IsNullOrEmpty(Mes.LotSN))
                    {
                        file = curTime.ToString("yyyyMMdd");
                    }
                    else
                    {
                        file = Mes.LotSN;
                    }
                    string LogDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log", file);
                    String logIni = Path.Combine(LogDir, fileName + "_" + file + ".log");
                    String logMsg = "[" + threadType + "]" + " " + msgType + " " + msg;
                    if (logLev == GlobConstData.ST_LOG_PRINT)
                    {
                        logMsg = curTime.ToString("HH:mm:ss:fff") + ", " + logMsg;
                        MainWnd.PrintLogInfo(logMsg);
                    }
                    else if (logLev == GlobConstData.ST_LOG_RECORD)
                    {
                        if (!path.IsNullOrEmpty())
                        {
                            //path = AppDomain.CurrentDomain.BaseDirectory + @"\Log\" + curTime.ToString("yyyyMMdd");
                            if (!Directory.Exists(LogDir))
                            {
                                Directory.CreateDirectory(LogDir);
                            }
                            
                            if (!File.Exists(logIni))
                            {
                                _LogFileStream = File.Create(logIni);
                                _LogFileStream.Close();
                                _LogWriter = new StreamWriter(logIni, true, System.Text.Encoding.Default);
                            }
                            if (File.Exists(logIni))
                            {
                                if (_LogWriter == null) _LogWriter = new StreamWriter(logIni, true, System.Text.Encoding.Default);
                                logMsg = curTime.ToString("yyyy-MM-dd") + ", " + logMsg;
                                _LogWriter.WriteLine(logMsg);
                                _LogWriter.Flush();
                            }
                        }
                    }
                    else if (logLev == GlobConstData.ST_LOG_PRINTANDRECORD)
                    {
                        logMsg = curTime.ToString("HH:mm:ss:fff") + ", " + logMsg;
                        MainWnd.PrintLogInfo(logMsg);
                        if (!path.IsNullOrEmpty())
                        {
                            
                            if (!Directory.Exists(LogDir))
                            {
                                Directory.CreateDirectory(LogDir);
                            }
                            
                            if (!File.Exists(logIni))
                            {
                                _LogFileStream = File.Create(logIni);
                                _LogFileStream.Close();
                                _LogWriter = new StreamWriter(logIni, true, System.Text.Encoding.Default);
                            }
                            if (File.Exists(logIni))
                            {
                                if (_LogWriter == null) _LogWriter = new StreamWriter(logIni, true, System.Text.Encoding.Default);
                                logMsg = curTime.ToString("yyyy-MM-dd") + ", " + logMsg;
                                _LogWriter.WriteLine(logMsg);
                                _LogWriter.Flush();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 生成MES打印日志信息
        /// </summary>
        /// <param name="logLev">log等级</param>
        /// <param name="msg">日志信息</param>
        /// <param name="fileName">log文件名</param>
        /// <param name="msgType">信息类型</param>
        /// <param name="threadType">执行线程</param>
        public void GenMesLogMessage(int logLev, String msg, String msgType = "Flow", String threadType = "MES", String fileName = "Meslog")
        {
            if (g_config.IsSwWorkFlowInfo == false)
            {
                lock (_objMesLogLock)
                {
                    DateTime curTime = DateTime.Now;
                    String path = AppDomain.CurrentDomain.BaseDirectory;
                    String logMsg = "[" + threadType + "]" + " " + msgType + " " + msg;
                    if (logLev == GlobConstData.ST_LOG_PRINT)
                    {
                        logMsg = curTime.ToString("HH:mm:ss:fff") + ", " + logMsg;
                        mesWnd.PrintLogInfo(logMsg);
                    }
                    else if (logLev == GlobConstData.ST_LOG_RECORD)
                    {
                        if (!path.IsNullOrEmpty())
                        {
                            path = AppDomain.CurrentDomain.BaseDirectory + @"\MesLog\" + curTime.ToString("yyyyMMdd");
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            String logPath = path + "\\" + fileName + "_" + curTime.ToString("yyyyMMdd") + ".log";
                            if (!File.Exists(logPath))
                            {
                                _MesLogFileStream = File.Create(logPath);
                                _MesLogFileStream.Close();
                                _MesLogWriter = new StreamWriter(logPath, true, System.Text.Encoding.Default);
                            }
                            if (File.Exists(logPath))
                            {
                                if (_MesLogWriter == null) _MesLogWriter = new StreamWriter(logPath, true, System.Text.Encoding.Default);
                                logMsg = curTime.ToString("yyyy-MM-dd") + ", " + logMsg;
                                _MesLogWriter.WriteLine(logMsg);
                                _MesLogWriter.Flush();
                            }
                        }
                    }
                    else if (logLev == GlobConstData.ST_LOG_PRINTANDRECORD)
                    {
                        logMsg = curTime.ToString("HH:mm:ss:fff") + ", " + logMsg;
                        mesWnd.PrintLogInfo(logMsg);
                        if (!path.IsNullOrEmpty())
                        {
                            path = AppDomain.CurrentDomain.BaseDirectory + @"\MesLog\" + curTime.ToString("yyyyMMdd");
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            String logPath = path + "\\" + fileName + "_" + curTime.ToString("yyyyMMdd") + ".log";
                            if (!File.Exists(logPath))
                            {
                                _MesLogFileStream = File.Create(logPath);
                                _MesLogFileStream.Close();
                                _MesLogWriter = new StreamWriter(logPath, true, System.Text.Encoding.Default);
                            }
                            if (File.Exists(logPath))
                            {
                                if (_MesLogWriter == null) _MesLogWriter = new StreamWriter(logPath, true, System.Text.Encoding.Default);
                                logMsg = curTime.ToString("yyyy-MM-dd") + ", " + logMsg;
                                _MesLogWriter.WriteLine(logMsg);
                                _MesLogWriter.Flush();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 在自动烧录流程已退出并指定退出原因前发生
        /// </summary>
        public void AutoRunShutoff()
        {
            if (Config.CCDModel == 1)
            {
                if (Config.CameraType == GlobConstData.Camera_DH)
                {
                    ArrDHCameraUtils[GlobConstData.ST_CCDDOWN].SetEnumValue("TriggerSource", "Software");
                }
                else
                {
                    ArrHRCameraUtils[GlobConstData.ST_CCDDOWN].SetTriggerMode();
                }
            }
            CopyAKLog_XC();
        }
        /// <summary>
        /// 删除日志文件
        /// </summary>
        /// <param name="logLev">log等级</param>
        public void DelLogFile(String threadType = "main", String fileName = "log")
        {
            DateTime curTime = DateTime.Now;
            String path = AppDomain.CurrentDomain.BaseDirectory + @"\Log" ;
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            int fileNum = GetFilesCount(dirInfo);

            Console.WriteLine(fileNum);
        }
        #region 获取当前目录下的文件总数
        /// <summary>
        /// 获取文件夹下的文件总数
        /// </summary>
        /// <param name="dirInfo"></param>
        /// <returns></returns>
        public static int GetFilesCount(DirectoryInfo dirInfo)
        {
            int totalFile = 0;
            totalFile += dirInfo.GetFiles().Length;
            foreach (DirectoryInfo f in dirInfo.GetDirectories())
            {
                totalFile += GetFilesCount(f);
            }
            return totalFile;
        }
        #endregion
        #region 删除文件夹
        /// 删除文件夹
        /// </summary>
        /// <param name="srcPath"> 文件夹路径 </param>
        public static void DelectDir(String srcPath)
        {
            try
            {
                if (Directory.Exists(srcPath))
                {
                    Directory.Delete(srcPath, true);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "DelectDir");
                throw;
            }
        }
        #endregion

        #region 修改文件夹名
        /// <summary>
        /// 修改文件夹名
        /// </summary>
        /// <param name="oldPath"></param>
        /// <param name="newPath"></param>
        public static void ModifyDir(String oldPath, string newPath)
        {
            try
            {
                if (Directory.Exists(oldPath))
                {
                    Directory.Move(oldPath, newPath);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "ModifyDir");
            }
        }
        #endregion

        #region 复制文件
        /// <summary>
        /// 复制昂科烧录器Log到协创
        /// </summary>
        public void CopyAKLog_XC()
        {
            if (Mes.Type != GlobConstData.ST_MESTYPE_XC)
            {
                return;
            }
            Task.Run(() =>
            {
                lock (_objCopyLock)
                {
                    string srcPath = @"C:\ACROVIEW\MultiAprog\log";//昂科烧录器Log位置
                    string destDir = Path.Combine(Mes.LogPath_XC, Mes.ComputerName);//协创保存Log网络路径
                    CopyFilesToDirKeepSrcDirName(srcPath, destDir);
                }
            });
        }

        /// <summary>
        /// 复制文件夹（保留源文件夹名）
        /// </summary>
        /// <param name="srcPath">源路径</param>
        /// <param name="destDir">目标路径</param>
        public void CopyFilesToDirKeepSrcDirName(string srcPath, string destDir)
        {
            DirectoryInfo destDirectory = new DirectoryInfo(Mes.LogPath_XC);

            if (!destDirectory.Exists)
            {
                destDirectory.Create();
            }

            if (!destDirectory.Exists)
            {
                GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "未找到共享路径: " + Mes.LogPath_XC, "Error");
                return;
            }
            if (Directory.Exists(srcPath))
            {
                DirectoryInfo srcDirectory = new DirectoryInfo(srcPath);
                CopyDirectory(srcPath, destDir + @"\" + srcDirectory.Name);
            }
            else
            {
                CopyFile(srcPath, destDir);
            }
        }

        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="srcPath">源路径</param>
        /// <param name="destDir">目标路径</param>
        public void CopyFilesToDir(string srcPath, string destDir)
        {
            if (Directory.Exists(srcPath))
            {
                CopyDirectory(srcPath, destDir);
            }
            else
            {
                CopyFile(srcPath, destDir);
            }
        }

        string[] strDirector = new string[4] { DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"), "Report", "UID" };
        private void CopyDirectory(string srcDir, string destDir, bool IsCopyFile = false)
        {
            DirectoryInfo srcDirectory = new DirectoryInfo(srcDir);
            DirectoryInfo destDirectory = new DirectoryInfo(destDir);

            if (destDirectory.FullName.StartsWith(srcDirectory.FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new Exception("Cannot copy parent to child directory.");
            }

            if (!srcDirectory.Exists)
            {
                GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "CopyDirectory: 未找到源路径" + srcDir, "Error");
                return;
            }

            if (!destDirectory.Exists)
            {
                destDirectory.Create();
            }

            if (IsCopyFile)
            {
                FileInfo[] files = srcDirectory.GetFiles();
                for (int i = 0; i < files.Length; i++)
                {
                    CopyFile(files[i].FullName, destDirectory.FullName);
                }
            }

            DirectoryInfo[] dirs = srcDirectory.GetDirectories();
            for (int j = 0; j < dirs.Length; j++)
            {
                for (int i = 0; i < strDirector.Length; i++)
                {
                    if (dirs[j].Name == strDirector[i])
                    {
                        CopyDirectory(dirs[j].FullName, destDirectory.FullName + @"\" + dirs[j].Name, true);
                    }
                }
            }
        }

        private void CopyFile(string srcFile, string destDir)
        {
            try
            {
                DirectoryInfo destDirectory = new DirectoryInfo(destDir);
                string fileName = Path.GetFileName(srcFile);
                if (!File.Exists(srcFile))
                {
                    return;
                }

                if (!destDirectory.Exists)
                {
                    destDirectory.Create();
                }

                File.Copy(srcFile, destDirectory.FullName + @"\" + fileName, true);
            }
            catch (Exception ex)
            {
                GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "CopyFile: " + ex.Message, "Error");
            }
            

        }
        #endregion

        /// <summary>
        /// 检查工单号日志
        /// </summary>
        public bool ReadLotInfo(String fileName = "LotInfo")
        {
            if (string.IsNullOrEmpty(Mes.LotSN))//工单号为空
            {
                return false;
            }
            string logIni = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log", Mes.LotSN, fileName + "_" + Mes.LotSN + ".ini");
            if (!File.Exists(logIni))//判断工单是否存在
            {
                return false;
            }
            String appName = "LotInfo";
            StringBuilder tempStr = new StringBuilder();
            string strBuffer;

            GetPrivateProfileString(appName, "Customer", "", tempStr, _MAX_FNAME, logIni);
            strBuffer = tempStr.ToString();
            Mes.Customer = strBuffer;
            tempStr.Clear();

            GetPrivateProfileString(appName, "LotSN", "", tempStr, _MAX_FNAME, logIni);
            strBuffer = tempStr.ToString();
            Mes.LotSN = strBuffer;
            tempStr.Clear();

            GetPrivateProfileString(appName, "Exit", "1", tempStr, _MAX_FNAME, logIni);
            strBuffer = tempStr.ToString();
            Mes.Exit = Convert.ToInt32(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "TargetC", "3000", tempStr, _MAX_FNAME, logIni);
            strBuffer = tempStr.ToString();
            Mes.Count = Convert.ToInt32(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "Brand", "", tempStr, _MAX_FNAME, logIni);
            strBuffer = tempStr.ToString();
            Mes.Brand = strBuffer;
            tempStr.Clear();

            GetPrivateProfileString(appName, "Device", "", tempStr, _MAX_FNAME, logIni);
            strBuffer = tempStr.ToString();
            Mes.Device = strBuffer;
            tempStr.Clear();

            GetPrivateProfileString(appName, "Checksum_File", "", tempStr, _MAX_FNAME, logIni);
            strBuffer = tempStr.ToString();
            Mes.Checksum_File = strBuffer;
            tempStr.Clear();

            GetPrivateProfileString(appName, "TIC_DoneC", "0", tempStr, _MAX_FNAME, logIni);
            strBuffer = tempStr.ToString();
            Mes.TIC_DoneC = Convert.ToInt32(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "OKDoneC", "0", tempStr, _MAX_FNAME, logIni);
            strBuffer = tempStr.ToString();
            Mes.OKDoneC = Convert.ToInt32(strBuffer);
            tempStr.Clear();

            GetPrivateProfileString(appName, "OperatorID", "", tempStr, _MAX_FNAME, logIni);
            strBuffer = tempStr.ToString();
            Mes.OperatorID = strBuffer;
            tempStr.Clear();

            return true;
        }

        /// <summary>
        /// 写工单信息
        /// </summary>
        /// <param name="fileName"></param>
        public void WriteLotInfo(bool IsManual = false, String fileName = "LotInfo")
        {
            DateTime curTime = DateTime.Now;
            String path = AppDomain.CurrentDomain.BaseDirectory;
            string file;
            if (string.IsNullOrEmpty(Mes.LotSN))
            {
                file = curTime.ToString("yyyyMMdd");
            }
            else
            {
                file = Mes.LotSN;
            }
            string LogDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log", file);
            String logIni = Path.Combine(LogDir, fileName + "_" + file + ".ini");
            
            if (!Directory.Exists(LogDir))
            {
                Directory.CreateDirectory(LogDir);
            }
            
            if (!File.Exists(logIni))
            {
                FileStream fileStream = File.Create(logIni);
                fileStream.Close();
            }
            String appName = "LotInfo";
            int temp = 1;
            if (Auto_Flag.Production)
            {
                temp = Auto_Flag.ProductionOK == true ? 1 : 2;
            }
            else
            {
                temp = 0;
            }
            WritePrivateProfileString(appName, "Customer", Mes.Customer, logIni);
            WritePrivateProfileString(appName, "LotSN", Mes.LotSN, logIni);
            WritePrivateProfileString(appName, "Exit", temp.ToString(), logIni);
            WritePrivateProfileString(appName, "TargetC", UserTask.TargetC.ToString(), logIni);
            WritePrivateProfileString(appName, "Brand", Mes.Brand, logIni);
            WritePrivateProfileString(appName, "Device", Mes.Device, logIni);
            WritePrivateProfileString(appName, "Checksum_File", Mes.Checksum_File, logIni);
            if (!IsManual)//手动保存时不更新数据
            {
                WritePrivateProfileString(appName, "TIC_DoneC", UserTask.TIC_DoneC.ToString(), logIni);
                WritePrivateProfileString(appName, "OKDoneC", UserTask.OKDoneC.ToString(), logIni);
            }
            WritePrivateProfileString(appName, "OperatorID", Mes.OperatorID, logIni);
        }

        public void SetDac(short dac,double mpa)
        {
            // 指令返回值
            short sRtn;
            // 电压值
            short sSetValue;
            short sGetValue;
            sSetValue = (short)(mpa * (10d / 0.9d) * (32767d / 10d));
            sRtn = Gts.GT_SetDac(Axis.jogPrm_X.cardPrm.cardNum, dac, ref sSetValue, 1);
            sRtn = Gts.GT_GetDac(Axis.jogPrm_X.cardPrm.cardNum, dac, out sGetValue, 1,out uint pClock);
        }
    }
}
