using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using GxIAPINET;
using BAR.Commonlib.Events;
using HalconDotNet;
using ThridLibray;
using MvCamCtrl.NET;
using MvCamCtrl.NET.CameraParams;

namespace BAR.Commonlib.Utils
{
    public class MVCameraUtil
    {
        public  int         ICameraID                   = 0;                   ///<相机ID
        public  String      StrIP                       = null;
        public  int         IWidth                      = 0;                   ///<图像宽度
        public  int         IHeigh                      = 0;                   ///<图像高度
        public  bool        IsOpen                      = false;               ///<是否打开相机
        public  bool        IsSnap                      = false;               ///<是否正在连续采图
        public  bool        IsInit                      = false;               ///<相机是否初始化结束                                                                     
        public  IntPtr      PtrBufferRaw                = IntPtr.Zero;         ///<用于存储Raw图Buffer的指针
                                                                                 
        public  int         SelectedWnd                 = 0;                   ///<当前打开的窗口
        private static MVCameraUtil instance = null;
        private static Dictionary<String, MVCameraUtil> instanceDir = new Dictionary<String, MVCameraUtil>();
        // 定义一个标识确保线程同步
        private static readonly object _objPadLock = new object();


        public delegate void MsgHandler(object sender, MsgEvent e);
        public event MsgHandler MsgEventHandler;

        public delegate void ShowMsgDelegate(string strMsg);
        public ShowMsgDelegate ShowMsg;

        private CCamera m_MyCamera;
        // ch:用于从驱动获取图像的缓存 | en:Buffer for getting image from driver
        private static Object BufForDriverLock;
        CImage m_pcImgForDriver;        // 图像信息
        CFrameSpecInfo m_pcImgSpecInfo; // 图像的水印信息

        // ch:Bitmap | en:Bitmap
        Bitmap m_pcBitmap = null;
        PixelFormat m_enBitmapPixelFormat = PixelFormat.DontCare;
        private static cbOutputExdelegate ImageCallback;

        private MVCameraUtil()
        {
            __InitEnumerator();
        }
        public static MVCameraUtil GetInstance(String cameraID)
        {
            lock (_objPadLock)
            {
                if (instanceDir.ContainsKey(cameraID))
                {
                    instance = instanceDir[cameraID];
                }
                else
                {
                    instance = new MVCameraUtil();
                    instanceDir[cameraID] = instance;
                }
            }
            return instance;
        }
        private void __InitEnumerator()
        {
            try
            {
                m_MyCamera = new CCamera();
                BufForDriverLock = new Object();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "__InitEnumerator");
            }
        }

        private void ImageCallbackFunc(IntPtr pData, ref MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
        {
            PtrBufferRaw = pData;
            MsgEventHandler.Invoke(this, new MsgEvent(MsgEvent.MSG_IMGPAINT, null));
            Console.WriteLine("Get one frame: Width[" + Convert.ToString(pFrameInfo.nWidth) + "] , Height[" + Convert.ToString(pFrameInfo.nHeight)
                                + "] , FrameNum[" + Convert.ToString(pFrameInfo.nFrameNum) + "]");
        }

        private void __CaptureCallBackPro(object objUserParam, GrabbedEventArgs e)
        {
            try
            {
                PtrBufferRaw = e.GrabResult.Raw;
                MsgEventHandler.Invoke(this, new MsgEvent(MsgEvent.MSG_IMGPAINT, null));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public void __InitDeviceParam()
        {
            try
            {
                string strValue = null;
                if (null != m_MyCamera)
                {
                    // 设置曝光 
                    // set ExposureTime 
                    if (ICameraID == 1)
                    {
                        SetShutter(Config.Shutter);
                    }
                    else
                    {
                        SetShutter(10000);
                    }

                    // 设置增益 
                    // set Gain 

                }
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
            }
        }
        public bool InitMVCard()
        {
            try
            {
                // ch:创建设备列表 | en:Create Device List
                System.GC.Collect();
                List<CCameraInfo> m_ltDeviceList = new List<CCameraInfo>();
                int nRet = CSystem.EnumDevices(CSystem.MV_GIGE_DEVICE, ref m_ltDeviceList);
                if (0 != nRet || m_ltDeviceList.Count <= 0)
                {
                    this.IsInit = false;
                    return false;
                }
                this.IsInit = true;
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 打开海康相机
        /// </summary>
        public string OpenMVDevice()
        {
            try
            {
                // 设备搜索 
                // device search 
                // ch:创建设备列表 | en:Create Device List
                System.GC.Collect();
                List<CCameraInfo> m_ltDeviceList = new List<CCameraInfo>();
                int nRet = CSystem.EnumDevices(CSystem.MV_GIGE_DEVICE, ref m_ltDeviceList);
                if (0 != nRet || m_ltDeviceList.Count <= 0)
                {
                    this.IsInit = false;
                    return "Camera No found";
                }
                this.IsInit = true;

                for (int i = 0; i < m_ltDeviceList.Count; i++)
                {
                    CGigECameraInfo gigeInfo = (CGigECameraInfo)m_ltDeviceList[i];

                    uint nIp1 = ((gigeInfo.nCurrentIp & 0xff000000) >> 24);
                    uint nIp2 = ((gigeInfo.nCurrentIp & 0x00ff0000) >> 16);
                    uint nIp3 = ((gigeInfo.nCurrentIp & 0x0000ff00) >> 8);
                    uint nIp4 = (gigeInfo.nCurrentIp & 0x000000ff);
                    string DevIP = nIp1 + "." + nIp2 + "." + nIp3 + "." + nIp4;
                    if (DevIP == StrIP) //根据IP地址打开相机
                    {
                        // ch:获取选择的设备信息 | en:Get selected device information
                        CCameraInfo device = m_ltDeviceList[i];
                        // ch:打开设备 | en:Open device
                        if (null == m_MyCamera)
                        {
                            m_MyCamera = new CCamera();
                            if (null == m_MyCamera)
                            {
                                return "fail to new CCamera()";
                            }
                        }

                        nRet = m_MyCamera.CreateHandle(ref device);
                        if (CErrorDefine.MV_OK != nRet)
                        {
                            return "fail to CreateHandle";
                        }
                    }
                }

                nRet = m_MyCamera.OpenDevice();
                if (CErrorDefine.MV_OK != nRet)
                {
                    m_MyCamera.DestroyHandle();
                    return "Device open fail IP:" + StrIP;
                }

                // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                int nPacketSize = m_MyCamera.GIGE_GetOptimalPacketSize();
                if (nPacketSize > 0)
                {
                    nRet = m_MyCamera.SetIntValue("GevSCPSPacketSize", (uint)nPacketSize);
                    if (nRet != CErrorDefine.MV_OK)
                    {
                        return "Set Packet Size failed!";
                    }
                }
                else
                {
                    return "Get Packet Size failed!";
                }

                

               
                //初始化相机参数
                SetTriggerMode("On");
                __InitDeviceParam();

                // ch:注册回调函数 | en:Register image callback
                ImageCallback = new cbOutputExdelegate(ImageCallbackFunc);
                nRet = m_MyCamera.RegisterImageCallBackEx(ImageCallback, IntPtr.Zero);

                // 开启码流 
                // ch:开启抓图 || en: start grab image
                nRet = m_MyCamera.StartGrabbing();
                if (CErrorDefine.MV_OK != nRet)
                {
                    Console.WriteLine("Start grabbing failed:{0:x8}", nRet);
                    return "开启码流失败";
                }
                // 更新设备打开标识
                IsOpen = true;
                return "相机[" + StrIP + "]连接成功";
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                string strErrorInfo = "【OpenMVDevice()】" + "错误描述信息为:" + ex.Message;
                return strErrorInfo;
            }

        }

        /// <summary>
        /// 相机丢失回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConnectLoss(object sender, EventArgs e)
        {
            ShowMsg("大华相机：" + StrIP + "丢失,重启相机");
        }

        /// <summary>
        /// 设置触发模式 软触发下设置Off后自由拉流（连续触发）On（单次触发）
        /// </summary>
        /// <param name="value"></param>
        public string SetTriggerMode(string value = "Off")
        {
            // ch:设置采集连续模式 | en:Set Continues Aquisition Mode
            //m_MyCamera.SetEnumValue("AcquisitionMode", (uint)MV_CAM_ACQUISITION_MODE.MV_ACQ_MODE_CONTINUOUS);
            m_MyCamera.SetEnumValue("TriggerSource", (uint)MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);
            m_MyCamera.SetEnumValue("TriggerMode", (uint)(value == "Off" ? MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF : MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON));
            
            return "";
        }

        public string SetLineMode(string value = "On")
        {
            if (m_MyCamera == null)
            {
                return "Device is invalid";
            }
            // 打开Software Trigger 
            // Set Software Trigger 
            m_MyCamera.SetEnumValue("TriggerSource", (uint)MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_LINE0);
            m_MyCamera.SetEnumValue("TriggerMode", (uint)(value == "Off" ? MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF : MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON));
            return "";
        }

        public void SetShutter(double value)
        {
            try
            {
                double minNum, maxNum = 0.0;
                if (null != m_MyCamera)
                {
                    CFloatValue pcFloatValue = new CFloatValue();
                    m_MyCamera.SetEnumValue("ExposureAuto", 0);
                    m_MyCamera.GetFloatValue("ExposureTime",ref pcFloatValue);
                    minNum = pcFloatValue.Min;
                    maxNum = pcFloatValue.Max;
                    //判断输入值是否在曝光时间的范围内
                    //若大于最大值则将曝光值设为最大值
                    if (value > maxNum)
                    {
                        value = maxNum;
                    }
                    //若小于最小值将曝光值设为最小值
                    if (value < minNum)
                    {
                        value = minNum;
                    }
                    m_MyCamera.SetFloatValue("ExposureTime", (float)value);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }
        public void AutoShutter()
        {
            try
            {
                m_MyCamera.SetEnumValue("ExposureAuto", (uint)MV_CAM_EXPOSURE_AUTO_MODE.MV_EXPOSURE_AUTO_MODE_OFF);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        /// <summary>
        /// 获取相机图像大小
        /// </summary>
        public void GetImageSize()
        {
            try
            {
                if (null != m_MyCamera)
                {
                    CIntValue Height = new CIntValue();
                    CIntValue Width = new CIntValue();
                    m_MyCamera.GetIntValue("Height", ref Height);
                    m_MyCamera.GetIntValue("Width", ref Width);
                    if (ICameraID == 0)
                    {
                        GlobConstData.IMG500_HEIGHT = (short)Height.CurValue;
                        GlobConstData.IMG500_WIDTH = (short)Width.CurValue;
                    }
                    else if (ICameraID == 1)
                    {
                        GlobConstData.IMG130_HEIGHT = (short)Height.CurValue;
                        GlobConstData.IMG130_WIDTH = (short)Width.CurValue;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        /// <summary>
        /// 设置增益值
        /// </summary>
        /// <param name="value"></param>
        public void SetGain(double value)
        {
            try
            {
                if (null != m_MyCamera)
                {
                    CFloatValue pcFloatValue = new CFloatValue();
                    double minNum, maxNum = 0.0;
                    m_MyCamera.GetFloatValue("Gain", ref pcFloatValue);
                    minNum = pcFloatValue.Min;
                    maxNum = pcFloatValue.Max;
                    //判断输入值是否在增益的范围内
                    //若大于最大值则将增益值设为最大值
                    if (value > maxNum)
                    {
                        value = maxNum;
                    }
                    //若小于最小值将增益值设为最小值
                    if (value < minNum)
                    {
                        value = minNum;
                    }
                    pcFloatValue.CurValue = (float)value;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
        public void AutoGain()
        {
            try
            {
                m_MyCamera.SetEnumValue("GainAuto", (uint)MV_CAM_GAIN_MODE.MV_GAIN_MODE_OFF);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
        public void StartGrap()
        {
            try
            {
                // 开启采集流通道
                if (null != m_MyCamera)
                {
                    //发送开采命令
                    m_MyCamera.SetEnumValue("AcquisitionMode", (uint)MV_CAM_ACQUISITION_MODE.MV_ACQ_MODE_CONTINUOUS);
                }
                
                IsSnap = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "StartGrap");
            }
        }
        
        public void StopSnap()
        {
            try
            {
                if (!IsSnap) return;
                if (null != m_MyCamera)
                {
                    m_MyCamera.SetEnumValue("AcquisitionMode", (uint)MV_CAM_ACQUISITION_MODE.MV_ACQ_MODE_SINGLE);
                }
                IsSnap = false;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "StopSnap");
            }
        }
        /// <summary>
        /// 相机触发采图
        /// </summary>
        public void SoftTrigger()
        {
            try
            {
                //发送软触发命令
                if (null != m_MyCamera)
                {
                    m_MyCamera.SetCommandValue("TriggerSoftware");
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "SoftTrigger");
            } 
        }
        public void TriggerMode(bool isOpen)
        {
            try
            {
                if (null != m_MyCamera)
                {
                    if (isOpen)
                    {
                        // 打开Software Trigger 
                        // Set Software Trigger 
                        m_MyCamera.SetEnumValue("TriggerSource", (uint)MV_CAM_TRIGGER_SOURCE.MV_TRIGGER_SOURCE_SOFTWARE);
                        m_MyCamera.SetEnumValue("TriggerMode", (uint)MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_ON);
                    }
                    else
                    {
                        m_MyCamera.SetEnumValue("TriggerMode", (uint)MV_CAM_TRIGGER_MODE.MV_TRIGGER_MODE_OFF);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            
        }

        object _object = new object();
        public void ReleaseDevices()
        {
            lock (_object)
            {
                try
                {
                    //停止流通道、注销采集回调和关闭流
                    if (null != m_MyCamera)
                    {
                        m_MyCamera.StopGrabbing();
                        WaitDoEvent(200);
                        m_MyCamera.CloseDevice();
                        m_MyCamera.DestroyHandle();
                        m_MyCamera = null;
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
            
        }

        public void WaitDoEvent(int delayTime)
        {
            DateTime dt1 = DateTime.Now;
            while ((DateTime.Now - dt1).TotalMilliseconds < delayTime) Application.DoEvents();
        }
        
    }
}
