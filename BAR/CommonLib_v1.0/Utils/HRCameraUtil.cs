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

namespace BAR.Commonlib.Utils
{
    public class HRCameraUtil
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
        private static HRCameraUtil instance = null;
        private static Dictionary<String, HRCameraUtil> instanceDir = new Dictionary<String, HRCameraUtil>();
        // 定义一个标识确保线程同步
        private static readonly object _objPadLock = new object();


        public delegate void MsgHandler(object sender, MsgEvent e);
        public event MsgHandler MsgEventHandler;

        public delegate void ShowMsgDelegate(string strMsg);
        public ShowMsgDelegate ShowMsg;


        Enumerator objEnumerator = null;                            ///<Factory对像
        IDevice objIDevice = null;                            ///<设备对像              
        private HRCameraUtil()
        {
            __InitEnumerator();
        }
        public static HRCameraUtil GetInstance(String cameraID)
        {
            lock (_objPadLock)
            {
                if (instanceDir.ContainsKey(cameraID))
                {
                    instance = instanceDir[cameraID];
                }
                else
                {
                    instance = new HRCameraUtil();
                    instanceDir[cameraID] = instance;
                }
            }
            return instance;
        }
        private void __InitEnumerator()
        {
            try
            {
                objEnumerator = new Enumerator();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "__InitEnumerator");
            }
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
                if (null != objIDevice)
                {
                    //获得图像宽度、高度等
                    using (IIntegraParameter p = objIDevice.ParameterCollection[ParametrizeNameSet.ImageWidth])
                    {
                        IWidth = (int)p.GetValue();
                    }
                    using (IIntegraParameter p = objIDevice.ParameterCollection[ParametrizeNameSet.ImageHeight])
                    {
                        IHeigh = (int)p.GetValue();
                    }

                    // 设置图像格式 
                    // set PixelFormat 
                    using (IEnumParameter p = objIDevice.ParameterCollection[ParametrizeNameSet.ImagePixelFormat])
                    {
                        p.SetValue("Mono8");
                    }

                    // 设置曝光 
                    // set ExposureTime 
                    using (IFloatParameter p = objIDevice.ParameterCollection[ParametrizeNameSet.ExposureTime])
                    {
                        if (ICameraID == 1)
                        {
                            p.SetValue(Config.Shutter);
                        }
                        else
                        {
                            p.SetValue(10000);
                        }
                    }

                    // 设置增益 
                    // set Gain 
                    using (IFloatParameter p = objIDevice.ParameterCollection[ParametrizeNameSet.GainRaw])
                    {
                        p.SetValue(1.0);
                    }

                    // 设置缓存个数为8（默认值为16） 
                    // set buffer count to 8 (default 16) 
                    objIDevice.StreamGrabber.SetBufferCount(8);

                }
            }
            catch(Exception e)
            {
                Console.Write(e.Message);
            }
        }
        public bool InitHRCard()
        {
            try
            {
                // 设备搜索 
                // device search 
                List<IDeviceInfo> li = Enumerator.EnumerateDevices();
                if (li.Count <= 0)
                {
                    //ShowMsg("未找到大华相机！");
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
        /// 打开华睿相机
        /// </summary>
        public string OpenHRDevice()
        {
            try
            {
                this.CloseStream();
                this.CloseDevice();
           
                if (objIDevice != null)
                {
                    objIDevice.Close();
                    objIDevice = null;
                }
                
                //根据IP地址打开相机
                objIDevice = Enumerator.GetDeviceByGigeIP(this.StrIP);
                // 打开设备 
                // open device 
                if (!objIDevice.Open())
                {
                    return "Open camera failed IP:" + StrIP;
                }
                objIDevice.ConnectionLost += OnConnectLoss;
                //初始化相机参数
                SetTriggerMode("On");
                __InitDeviceParam();

                // 注册码流回调事件 
                // register grab event callback 
                objIDevice.StreamGrabber.ImageGrabbed += __CaptureCallBackPro;

                // 开启码流 
                // start grabbing 
                if (!objIDevice.GrabUsingGrabLoopThread())
                {
                    return "开启码流失败";
                }

                // 更新设备打开标识
                IsOpen = true;
                return "相机[" + StrIP + "]连接成功";
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                string strErrorInfo = "【OpenDHDevice()】" + "错误描述信息为:" + ex.Message;
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
            //objIDevice.ShutdownGrab();
            //objIDevice.Dispose();
            //objIDevice = null;
            ////OpenHRDevice();
            //ShowMsg(OpenHRDevice());
        }

        /// <summary>
        /// 设置触发模式 软触发下设置Off后自由拉流（连续触发）On（单次触发）
        /// </summary>
        /// <param name="value"></param>
        public string SetTriggerMode(string value = "Off")
        {
            if (objIDevice == null)
            {
                return "Device is invalid";
            }
            if (!objIDevice.IsOpen)
            {
                return "相机未打开";
            }
            // 打开Software Trigger 
            // Set Software Trigger 
            objIDevice.TriggerSet.Open(TriggerSourceEnum.Software);
            using (IEnumParameter p = objIDevice.ParameterCollection[ParametrizeNameSet.TriggerMode])
            {
                p.SetValue(value);
            }
            return "";
        }

        public string SetLineMode(string value = "On")
        {
            if (objIDevice == null)
            {
                return "Device is invalid";
            }
            if (!objIDevice.IsOpen)
            {
                return "相机未打开";
            }
            // 打开Software Trigger 
            // Set Software Trigger 
            objIDevice.TriggerSet.Open(TriggerSourceEnum.Line1);
            using (IEnumParameter p = objIDevice.ParameterCollection[ParametrizeNameSet.TriggerMode])
            {
                p.SetValue(value);
            }
            return "";
        }

        public void SetShutter(double value)
        {
            try
            {
                double minNum, maxNum = 0.0;
                if (null != objIDevice)
                {
                    IFloatParameter p = objIDevice.ParameterCollection[ParametrizeNameSet.ExposureTime];
                    minNum = p.GetMinimum();
                    maxNum = p.GetMaximum();
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
                    p.SetValue(value);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }
        public void AutoShutter(String strValue)
        {
            try
            {
                using (IEnumParameter p = objIDevice.ParameterCollection[ParametrizeNameSet.ExposureAuto])
                {
                    p.SetValue(strValue);
                }
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
                if (null != objIDevice)
                {
                    IIntegraParameter Height = objIDevice.ParameterCollection[ParametrizeNameSet.ImageHeight];
                    IIntegraParameter Width = objIDevice.ParameterCollection[ParametrizeNameSet.ImageWidth];
                    if (ICameraID == 0)
                    {
                        GlobConstData.IMG500_HEIGHT = (short)Height.GetValue();
                        GlobConstData.IMG500_WIDTH = (short)Width.GetValue();
                    }
                    else if (ICameraID == 1)
                    {
                        GlobConstData.IMG130_HEIGHT = (short)Height.GetValue();
                        GlobConstData.IMG130_WIDTH = (short)Width.GetValue();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public void SetGain(double value)
        {
            try
            {
                if (null != objIDevice)
                {
                    double minNum, maxNum = 0.0;
                    if (null != objIDevice)
                    {
                        IFloatParameter p = objIDevice.ParameterCollection[ParametrizeNameSet.GainRaw];
                        minNum = p.GetMinimum();
                        maxNum = p.GetMaximum();
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
                        p.SetValue(value);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
        public void AutoGain(String strValue)
        {
            try
            {
                using (IEnumParameter p = objIDevice.ParameterCollection[ParametrizeNameSet.GainAuto])
                {
                    p.SetValue(strValue);
                }
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
                if (null != objIDevice)
                {
                    //发送开采命令
                    using (ICommandParameter p = objIDevice.ParameterCollection[new CommandName("AcquisitionStart")])
                    {
                        p.Execute();
                    }
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
                if (null != objIDevice)
                {
                    using (ICommandParameter p = objIDevice.ParameterCollection[new CommandName("AcquisitionStop")])
                    {
                        p.Execute();
                    }
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
                if (null != objIDevice)
                {
                    objIDevice.ExecuteSoftwareTrigger();
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
                if (null != objIDevice)
                {
                    if (isOpen)
                    {
                        // 打开Software Trigger 
                        // Set Software Trigger 
                        objIDevice.TriggerSet.Open(TriggerSourceEnum.Software);
                        using (IEnumParameter p = objIDevice.ParameterCollection[ParametrizeNameSet.TriggerMode])
                        {
                            p.SetValue("On");
                        }
                    }
                    else
                    {
                        using (IEnumParameter p = objIDevice.ParameterCollection[ParametrizeNameSet.TriggerMode])
                        {
                            p.SetValue("Off");
                        }
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
                    if (null != objIDevice)
                    {
                        StopSnap();
                        WaitDoEvent(200);
                        objIDevice.StreamGrabber.ImageGrabbed -= __CaptureCallBackPro;   // 反注册回调 | unregister grab event callback 
                        objIDevice.ShutdownGrab();                                       // 停止码流 | stop grabbing 
                        objIDevice.Close();                                              // 关闭相机 | close camera 
                        objIDevice = null;
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
        private void CloseStream()
        {
            try
            {
                //停止流通道、注销采集回调和关闭流
                if (null != objIDevice)
                {
                    StopSnap();
                    objIDevice.StreamGrabber.ImageGrabbed -= __CaptureCallBackPro;   // 反注册回调 | unregister grab event callback 
                    objIDevice.ShutdownGrab();                                       // 停止码流 | stop grabbing 
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

        }
        public void CloseDevice()
        {
            try
            {
                if (null != objIDevice)
                {
                    objIDevice.Close();                                              // 关闭相机 | close camera 
                    objIDevice = null;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}
