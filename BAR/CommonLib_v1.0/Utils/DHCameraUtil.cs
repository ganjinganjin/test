using BAR.Commonlib.Events;
using GxIAPINET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace BAR.Commonlib.Utils
{
    public class DHCameraUtil
    {
        #region 定义变量
        const uint PIXEL_FORMATE_BIT = 0x00FF0000;          ///<用于与当前的数据格式进行与运算得到当前的数据位数
        const uint GX_PIXEL_8BIT = 0x00080000;          ///<8位数据图像格式
                                                        ///
        const int COLORONCOLOR = 3;
        const uint DIB_RGB_COLORS = 0;
        const uint SRCCOPY = 0x00CC0020;

        public int ICameraID = 0;                   ///<相机ID
        public String StrIP = null;
        public int IPayloadSize = 0;                   ///<图像数据大小
        public int IWidth = 0;                   ///<图像宽度
        public int IHeigh = 0;                   ///<图像高度
        public bool IsColor = false;               ///<是否彩色相机 
        public bool IsOpen = false;               ///<是否打开相机
        public bool IsSnap = false;               ///<是否正在连续采图
        public bool IsInit = false;               ///<相机是否初始化结束 
        public byte[] ArrRawBuffer = null;                ///<用于存储Raw图的Buffer
        public byte[] ArrImgBuffer = null;                ///<用于存储Raw图的Buffer                                                                      
        public IntPtr PtrBufferRaw = IntPtr.Zero;         ///<用于存储Raw图Buffer的指针

        public int SelectedWnd = 0;                   ///<当前打开的窗口
        private static DHCameraUtil instance = null;
        private static Dictionary<String, DHCameraUtil> instanceDir = new Dictionary<String, DHCameraUtil>();
        // 定义一个标识确保线程同步
        private static readonly object _objPadLock = new object();

        public delegate void MsgHandler(object sender, MsgEvent e);
        public event MsgHandler MsgEventHandler;

        public delegate void ShowMsgDelegate(string strMsg);
        public ShowMsgDelegate ShowMsg;


        IGXFactory objIGXFactory = null;                            ///<Factory对像
        IGXDevice objIGXDevice = null;                            ///<设备对像                                                                                   
        IGXStream objIGXStream = null;                            ///<流对像 
        IGXFeatureControl objIGXFeatureControl = null;                            ///<远端设备属性控制器对像                                                                                            
        IGXFeatureControl objIGXStreamFeatureControl = null;                            ///<流层属性控制器对象
        Bitmap objBitmapForSave = null;                            ///<bitmap对象,仅供存储图像使用        

        GX_DEVICE_OFFLINE_CALLBACK_HANDLE hDeviceOffline = null;//掉线事件

        IntPtr pBitmapInfo = IntPtr.Zero;
        #endregion


        private DHCameraUtil()
        {
            __InitGXFactory();
        }
        public static DHCameraUtil GetInstance(String cameraID)
        {
            lock (_objPadLock)
            {
                if (instanceDir.ContainsKey(cameraID))
                {
                    instance = instanceDir[cameraID];
                }
                else
                {
                    instance = new DHCameraUtil();
                    instanceDir[cameraID] = instance;
                }

            }
            return instance;
        }
        private void __InitGXFactory()
        {
            try
            {
                objIGXFactory = IGXFactory.GetInstance();
                objIGXFactory.Init();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "__InitGXFactory");
            }
        }
        private void __InitDevice()
        {
            if (null != objIGXFeatureControl)
            {
                //设置采集模式连续采集
                objIGXFeatureControl.GetEnumFeature("AcquisitionMode").SetValue("Continuous");
            }
        }
        /// <summary>
        /// 判断PixelFormat是否为8位
        /// </summary>
        /// <param name="emPixelFormatEntry">图像数据格式</param>
        /// <returns>true为8为数据，false为非8位数据</returns>
        private bool __IsPixelFormat8(GX_PIXEL_FORMAT_ENTRY emPixelFormatEntry)
        {
            bool bIsPixelFormat8 = false;
            uint uiPixelFormatEntry = (uint)emPixelFormatEntry;
            if ((uiPixelFormatEntry & PIXEL_FORMATE_BIT) == GX_PIXEL_8BIT)
            {
                bIsPixelFormat8 = true;
            }
            return bIsPixelFormat8;
        }
        private void __CaptureCallBackPro(object objUserParam, IFrameData objIFrameData)
        {
            try
            {
                if (objIFrameData.GetStatus() == GX_FRAME_STATUS_LIST.GX_FRAME_STATUS_SUCCESS)
                {
                    GX_VALID_BIT_LIST emValidBits = GX_VALID_BIT_LIST.GX_BIT_0_7;

                    //检查图像是否改变并更新Buffer
                    //__UpdateBufferSize(objIFrameData);

                    //设置相机的数据格式为8bit
                    emValidBits = __GetBestValudBit(objIFrameData.GetPixelFormat());
                    if (GX_FRAME_STATUS_LIST.GX_FRAME_STATUS_SUCCESS == objIFrameData.GetStatus())
                    {
                        if (IsColor)
                        {
                            IntPtr pBufferColor = objIFrameData.ConvertToRGB24(emValidBits, GX_BAYER_CONVERT_TYPE_LIST.GX_RAW2RGB_NEIGHBOUR, false);
                            Marshal.Copy(pBufferColor, ArrImgBuffer, 0, __GetStride(IWidth, IsColor) * IHeigh);
                        }
                        else
                        {
                            IntPtr pBufferMono = IntPtr.Zero;
                            if (__IsPixelFormat8(objIFrameData.GetPixelFormat()))
                            {
                                PtrBufferRaw = pBufferMono = objIFrameData.GetBuffer();
                            }
                            else
                            {
                                PtrBufferRaw = pBufferMono = objIFrameData.ConvertToRaw8(emValidBits);
                            }
                            Marshal.Copy(pBufferMono, ArrImgBuffer, 0, __GetStride(IWidth, IsColor) * IHeigh);
                        }
                    }
                }
                MsgEventHandler.Invoke(this, new MsgEvent(MsgEvent.MSG_IMGPAINT, null));
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 掉线回调函数
        /// </summary>
        /// <param name="obj"></param>
        public void OnDeviceOfflineEvent(object obj)
        {
            //obj为用户私有参数，在注册回调函数的时候传入，用户可在此处将其还原
            //用户在掉线回调函数内部不允许执行关闭设备动作，否则抛出非法调用异常
            ShowMsg("大恒相机：" + StrIP + "掉线");
            if (Config.CCDModel == 1)
            {
                Task.Run(() =>
                {
                    Console.WriteLine("Thread.CurrentThread.IsBackground=" + Thread.CurrentThread.IsBackground);
                    // 置为后台线程
                    Thread.CurrentThread.IsBackground = true;
                    Console.WriteLine("Thread.CurrentThread.IsBackground=" + Thread.CurrentThread.IsBackground);
                    ShowMsg("重启相机");
                    InitDHCard();
                    string strErrorInfo = OpenDHDevice();
                    ShowMsg(strErrorInfo);
                });
            }
            
        }



        /// <summary>
        /// 通过GX_PIXEL_FORMAT_ENTRY获取最优Bit位
        /// </summary>
        /// <param name="em">图像数据格式</param>
        /// <returns>最优Bit位</returns>
        private GX_VALID_BIT_LIST __GetBestValudBit(GX_PIXEL_FORMAT_ENTRY emPixelFormatEntry)
        {
            GX_VALID_BIT_LIST emValidBits = GX_VALID_BIT_LIST.GX_BIT_0_7;
            switch (emPixelFormatEntry)
            {
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO8:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GR8:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_RG8:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GB8:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_BG8:
                    {
                        emValidBits = GX_VALID_BIT_LIST.GX_BIT_0_7;
                        break;
                    }
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO10:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GR10:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_RG10:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GB10:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_BG10:
                    {
                        emValidBits = GX_VALID_BIT_LIST.GX_BIT_2_9;
                        break;
                    }
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO12:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GR12:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_RG12:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GB12:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_BG12:
                    {
                        emValidBits = GX_VALID_BIT_LIST.GX_BIT_4_11;
                        break;
                    }
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO14:
                    {
                        //暂时没有这样的数据格式待升级
                        break;
                    }
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_MONO16:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GR16:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_RG16:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_GB16:
                case GX_PIXEL_FORMAT_ENTRY.GX_PIXEL_FORMAT_BAYER_BG16:
                    {
                        //暂时没有这样的数据格式待升级
                        break;
                    }
                default:
                    break;
            }
            return emValidBits;
        }

        /// <summary>
        /// 检查图像是否改变并更新Buffer
        /// </summary>
        /// <param name="objIBaseData">图像数据对象</param>
        private void __UpdateBufferSize(IBaseData objIBaseData)
        {
            if (null != objIBaseData)
            {
                if (__IsCompatible(objBitmapForSave, IWidth, IHeigh, IsColor))
                {
                    IPayloadSize = (int)objIBaseData.GetPayloadSize();
                    IWidth = (int)objIBaseData.GetWidth();
                    IHeigh = (int)objIBaseData.GetHeight();
                }
                else
                {
                    IPayloadSize = (int)objIBaseData.GetPayloadSize();
                    IWidth = (int)objIBaseData.GetWidth();
                    IHeigh = (int)objIBaseData.GetHeight();

                    ArrRawBuffer = new byte[IPayloadSize];
                    ArrImgBuffer = new byte[__GetStride(IWidth, IsColor) * IHeigh];
                    //m_byColorBuffer = new byte[__GetStride(IWidth, IsColor) * IHeigh];
                }
            }
        }

        /// <summary>
        /// 判断是否兼容
        /// </summary>
        /// <param name="bitmap">Bitmap对象</param>
        /// <param name="nWidth">图像宽度</param>
        /// <param name="nHeight">图像高度</param>
        /// <param name="bIsColor">是否是彩色相机</param>
        /// <returns>true为一样，false不一样</returns>
        private bool __IsCompatible(Bitmap bitmap, int nWidth, int nHeight, bool bIsColor)
        {
            if (bitmap == null
                || bitmap.Height != nHeight
                || bitmap.Width != nWidth
                || bitmap.PixelFormat != __GetFormat(bIsColor)
             )
            {
                return false;
            }
            return true;
        }

        public void __InitDeviceParam()
        {
            try
            {
                string strValue = null;
                if (null != objIGXDevice)
                {
                    //设置曝光时间
                    SetShutter(Config.Shutter);
                    //获得图像原始数据大小、宽度、高度等
                    IPayloadSize = (int)objIGXDevice.GetRemoteFeatureControl().GetIntFeature("PayloadSize").GetValue();
                    IWidth = (int)objIGXDevice.GetRemoteFeatureControl().GetIntFeature("Width").GetValue();
                    IHeigh = (int)objIGXDevice.GetRemoteFeatureControl().GetIntFeature("Height").GetValue();

                    //获取是否为彩色相机
                    if (objIGXDevice.GetRemoteFeatureControl().IsImplemented("PixelColorFilter"))
                    {
                        strValue = objIGXDevice.GetRemoteFeatureControl().GetEnumFeature("PixelColorFilter").GetValue();

                        if ("None" != strValue)
                        {
                            IsColor = true;
                        }
                    }
                    //申请用于缓存图像数据的buffer
                    ArrRawBuffer = new byte[IPayloadSize];
                    ArrImgBuffer = new byte[__GetStride(IWidth, IsColor) * IHeigh];

                    //__CreateBitmap(out objBitmapForSave, IWidth, IHeigh, IsColor);
                }
            }
            catch (CGalaxyException ex)
            {
                string strErrorInfo = "错误码为:" + ex.GetErrorCode().ToString() + "错误描述信息为:" + ex.Message;
                MessageBox.Show(strErrorInfo, "__InitDeviceParam");
            }
        }
        public bool InitDHCard()
        {
            try
            {
                List<IGXDeviceInfo> listGXDeviceInfo = new List<IGXDeviceInfo>();
                objIGXFactory.UpdateDeviceList(2000, listGXDeviceInfo);
                if (listGXDeviceInfo.Count <= 0)
                {
                    //MessageBox.Show("Device no found!");
                    //ShowMsg("未找到大恒相机！");
                    this.IsInit = false;
                    return false;
                }
                //for (int i = 0; i < listGXDeviceInfo.Count; i++)
                //{
                //    ShowMsg("已找到大恒相机：" + listGXDeviceInfo[i].GetIP());
                    
                //}
                this.IsInit = true;
            }
            catch (CGalaxyException ex)
            {
                return false;
            }
            return true;
        }

        public string OpenDHDevice()
        {
            try
            {
                this.CloseStream();
                this.CloseDevice();

                if (objIGXDevice != null)
                {
                    objIGXDevice.Close();
                    objIGXDevice = null;
                }

                //根据IP地址打开相机
                objIGXDevice = objIGXFactory.OpenDeviceByIP(this.StrIP, GX_ACCESS_MODE.GX_ACCESS_EXCLUSIVE);
                objIGXFeatureControl = objIGXDevice.GetRemoteFeatureControl();

                if (null != objIGXDevice)
                {
                    objIGXStream = objIGXDevice.OpenStream(0);
                    objIGXStreamFeatureControl = objIGXStream.GetFeatureControl();
                    objIGXStream.RegisterCaptureCallback(this, __CaptureCallBackPro);
                    //objIGXDevice为IGXDevice设备对象，设备已经打开
                    //第一个参数可以传入一个字符串，例如设备名称，此参数为用户私有参数，用户可以在回调函数内部将其还原然后打印
                    //如果不需要则可传入null即可
                    hDeviceOffline = objIGXDevice.RegisterDeviceOfflineCallback(null, OnDeviceOfflineEvent);


                }

                //初始化相机参数
                __InitDevice();
                __InitDeviceParam();

                if (null != objIGXStreamFeatureControl)
                {
                    //设置采集buffer个数
                    objIGXStream.SetAcqusitionBufferNumber(4);
                    //设置流层Buffer处理模式为OldestFrist
                    objIGXStreamFeatureControl.GetEnumFeature("StreamBufferHandlingMode").SetValue("OldestFirst");
                }

                // 更新设备打开标识
                IsOpen = true;
                return "相机[" + StrIP + "]连接成功";
            }
            catch (CGalaxyException ex)
            {
                string strErrorInfo = "【OpenDHDevice()】错误码为:" + ex.GetErrorCode().ToString() + "错误描述信息为:" + ex.Message;
                return strErrorInfo;
            }

        }
        public void SetEnumValue(String strFeatrueName, String strValue)
        {
            try
            {
                if (null != objIGXFeatureControl)
                {
                    if (strValue == "Line2")
                    {
                        //设置触发模式为开

                    }
                    objIGXFeatureControl.GetEnumFeature("TriggerMode").SetValue("On");
                    objIGXFeatureControl.GetEnumFeature(strFeatrueName).SetValue(strValue);
                }
            }
            catch (Exception ex)
            {
                ShowMsg("大恒相机：" + StrIP + ex.Message);
            }
            
        }
        /// <summary>
        /// 设置曝光时间
        /// </summary>
        /// <param name="value"></param>
        public void SetShutter(double value)
        {
            try
            {
                double minNum, maxNum = 0.0;
                if (null != objIGXFeatureControl)
                {
                    minNum = objIGXFeatureControl.GetFloatFeature("ExposureTime").GetMin();
                    maxNum = objIGXFeatureControl.GetFloatFeature("ExposureTime").GetMax();
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
                    if (ICameraID == 1)
                    {
                        objIGXFeatureControl.GetFloatFeature("ExposureTime").SetValue(value);
                    }
                    else
                    {
                        objIGXFeatureControl.GetFloatFeature("ExposureTime").SetValue(10000);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMsg("大恒相机：" + StrIP + ex.Message);
            }
        }
        public void AutoShutter(String strValue)
        {
            try
            {
                objIGXFeatureControl.GetEnumFeature("ExposureAuto").SetValue(strValue);
            }
            catch (Exception ex)
            {
                ShowMsg("大恒相机：" + StrIP + ex.Message);
            }
        }
        public void SetGain(double value)
        {
            try
            {
                double minNum, maxNum = 0.0;
                if (null != objIGXFeatureControl)
                {
                    minNum = objIGXFeatureControl.GetFloatFeature("Gain").GetMin();
                    maxNum = objIGXFeatureControl.GetFloatFeature("Gain").GetMax();
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
                    objIGXFeatureControl.GetFloatFeature("Gain").SetValue(value);
                }

            }
            catch (Exception ex)
            {
                ShowMsg("大恒相机：" + StrIP + ex.Message);
            }
        }
        public void AutoGain(String strValue)
        {
            try
            {
                objIGXFeatureControl.GetEnumFeature("GainAuto").SetValue(strValue);
            }
            catch (Exception ex)
            {
                ShowMsg("大恒相机：" + StrIP + ex.Message);
            }
        }
        public void StartGrap()
        {
            try
            {
                // 开启采集流通道
                if (null != objIGXStream)
                {
                    objIGXStream.StartGrab();
                }
                //发送开采命令
                if (null != objIGXFeatureControl)
                {
                    objIGXFeatureControl.GetCommandFeature("AcquisitionStart").Execute();
                }

                IsSnap = true;
            }
            catch (Exception ex)
            {
                ShowMsg("大恒相机：" + StrIP + ex.Message);
            }
        }
        /// <summary>
        /// 创建Bitmap
        /// </summary>
        /// <param name="bitmap">Bitmap对象</param>
        /// <param name="nWidth">图像宽度</param>
        /// <param name="nHeight">图像高度</param>
        /// <param name="bIsColor">是否是彩色相机</param>
        private void __CreateBitmap(out Bitmap bitmap, int nWidth, int nHeight, bool bIsColor)
        {
            bitmap = new Bitmap(nWidth, nHeight, __GetFormat(bIsColor));
            if (bitmap.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                ColorPalette colorPalette = bitmap.Palette;
                for (int i = 0; i < 256; i++)
                {
                    colorPalette.Entries[i] = Color.FromArgb(i, i, i);
                }
                bitmap.Palette = colorPalette;
            }
        }
        /// <summary>
        /// 获取图像显示格式
        /// </summary>
        /// <param name="bIsColor">是否为彩色相机</param>
        /// <returns>图像的数据格式</returns>
        private PixelFormat __GetFormat(bool bIsColor)
        {
            return bIsColor ? PixelFormat.Format24bppRgb : PixelFormat.Format8bppIndexed;
        }

        /// <summary>
        /// 计算宽度所占的字节数
        /// </summary>
        /// <param name="nWidth">图像宽度</param>
        /// <param name="bIsColor">是否是彩色相机</param>
        /// <returns>图像一行所占的字节数</returns>
        private int __GetStride(int nWidth, bool bIsColor)
        {
            return bIsColor ? nWidth * 3 : nWidth;
        }

        public void StopSnap()
        {
            try
            {
                if (!IsSnap) return;
                //发送停止命令
                if (null != objIGXFeatureControl)
                {
                    objIGXFeatureControl.GetCommandFeature("AcquisitionStop").Execute();
                }
                if (null != objIGXStream)
                {
                    objIGXStream.StopGrab();
                }
                IsSnap = false;
            }
            catch (Exception ex)
            {
                ShowMsg("大恒相机：" + StrIP + ex.Message);
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
                if (null != objIGXFeatureControl)
                {
                    objIGXFeatureControl.GetCommandFeature("TriggerSoftware").Execute();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "SoftTrigger");
            }
        }
        public void TriggerMode(bool isOpen)
        {
            if (null != objIGXFeatureControl)
            {
                if (isOpen)
                {
                    //设置触发模式为开
                    objIGXFeatureControl.GetEnumFeature("TriggerMode").SetValue("On");
                    //选择触发源为软触发
                    objIGXFeatureControl.GetEnumFeature("TriggerSource").SetValue("Software");
                }
                else
                {
                    objIGXFeatureControl.GetEnumFeature("TriggerMode").SetValue("Off");
                }
            }
        }
        public void ReleaseDevices()
        {
            try
            {
                // 如果未停采则先停止采集
                if (IsSnap)
                {
                    if (null != objIGXFeatureControl)
                    {
                        objIGXFeatureControl.GetCommandFeature("AcquisitionStop").Execute();
                    }
                }
            }
            catch (Exception)
            {

            }

            try
            {
                //停止流通道、注销采集回调和关闭流
                if (null != objIGXStream)
                {
                    objIGXStream.StopGrab();
                    objIGXStream.UnregisterCaptureCallback();
                    objIGXStream.Close();
                    objIGXStream = null;
                    objIGXStreamFeatureControl = null;
                }
            }
            catch (Exception)
            {

            }

            try
            {
                //关闭设备
                if (null != objIGXDevice)
                {
                    objIGXDevice.UnregisterDeviceOfflineCallback(hDeviceOffline);
                    objIGXDevice.Close();
                    objIGXDevice = null;
                }
            }
            catch (Exception)
            {

            }

            try
            {
                //反初始化
                if (null != objIGXFactory)
                {
                    objIGXFactory.Uninit();
                }
            }
            catch (Exception)
            {

            }
        }
        private void CloseStream()
        {
            try
            {
                if (null != objIGXStream)
                {
                    objIGXStream.UnregisterCaptureCallback();
                    objIGXStream.Close();
                    objIGXStream = null;
                }
            }
            catch (Exception ex)
            {

            }

        }
        public void CloseDevice()
        {
            try
            {
                if (null != objIGXDevice)
                {
                    objIGXDevice.UnregisterDeviceOfflineCallback(hDeviceOffline);
                    objIGXDevice.Close();
                    objIGXDevice = null;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
