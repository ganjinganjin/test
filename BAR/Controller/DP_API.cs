using System;
using System.Runtime.InteropServices;

    
namespace BAR
{
    
    public static class DP_API
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ProgressCallback(IntPtr value);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void ConnCallback(IntPtr value);

        public const string _dedinetPath = @"C:\Program Files (x86)\Dediprog\Client\DediNetLinker.dll";

        [DllImport(_dedinetPath, EntryPoint = "SetCallBack", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetCallBack([MarshalAs(UnmanagedType.FunctionPtr)] ProgressCallback callbackPointer);

        [DllImport(_dedinetPath, EntryPoint = "SetConnCallBack", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetConnCallBack([MarshalAs(UnmanagedType.FunctionPtr)] ConnCallback callbackPointer);

        [DllImport(_dedinetPath, EntryPoint = "CreateInstance", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateInstance();


        [DllImport(_dedinetPath, EntryPoint = "ExLogin", CallingConvention = CallingConvention.Cdecl)]

        /// _progServer 0 = Progmaster , 1 = NuProg
        public static extern int ExLogin([MarshalAs(UnmanagedType.AnsiBStr)] string _IpAddress, [MarshalAs(UnmanagedType.AnsiBStr)] string port, IntPtr _instance, int _progServer);


        [DllImport(_dedinetPath, EntryPoint = "ExGetProjectInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ExGetProjectInfo(IntPtr _instance, ref int _cLength);

        /// <summary>
        /// 取回NuProg chip info
        /// {Ready:Yes;
        /// FileSize:1234;
        /// Brand:aaa;
        /// Model:bbb;
        /// NPVR-M:1.1.1;
        /// NPVR-S:{n,n,n};
        /// Batch:BPV}
        /// 
        /// Ready , Batch
        /// </summary>
        /// <param name="_instance"></param>
        /// <param name="_cLength"></param>
        /// <returns></returns>
        [DllImport(_dedinetPath, EntryPoint = "ExGetNuProgChipInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ExGetNuProgChipInfo(IntPtr _instance, ref int _cLength);

        [DllImport(_dedinetPath, EntryPoint = "ExDediNet_GetProgrammerSN", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ExDediNet_GetProgrammerSN(int OrderIndex, ref uint len);

        [DllImport(_dedinetPath, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ExDediNet_GetProjectCnt(ref int _cnt);

        [DllImport(_dedinetPath, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ExDediNet_GetProjectTbl(int _index, int _maxCnt, ref int _len);


        [DllImport(_dedinetPath, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ExDediNet_SelectProject([MarshalAs(UnmanagedType.LPWStr)] string _prjName);

        [DllImport(_dedinetPath, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ExDediNet_RunPrj();

        [DllImport(_dedinetPath, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ExDediNet_DownPrj([MarshalAs(UnmanagedType.LPWStr)] string _path);

        [DllImport(_dedinetPath, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ExDediNet_StopPrj();

        [DllImport(_dedinetPath, EntryPoint = "ExDediNet_StartSite", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ExDediNet_StartSite(int _ctgIndex, int _sktIndex);

        /// <summary>
        /// Mask Start
        /// </summary>
        /// <param name="_ctgIndex"></param>
        /// <param name="_sktIndex"></param>
        /// <returns></returns>
        [DllImport(_dedinetPath, EntryPoint = "ExDediNet_MaskStart", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ExDediNet_MaskStart(int _ctgIndex, int _mask);


        [DllImport(_dedinetPath, EntryPoint = "ExGetSocketInfo",
            CallingConvention = CallingConvention.Cdecl)]

        public static extern IntPtr ExGetSocketInfo(int _cIndex, int _sIndex);//,ref IntPtr _dataStruct);

        [DllImport(_dedinetPath, EntryPoint = "ExGetSocketInfo1",
    CallingConvention = CallingConvention.Cdecl)]

        public static extern IntPtr ExGetSocketInfo1(int _cIndex, int _sIndex, ref int _pass, ref int _fail, ref int _limit);//,ref IntPtr _dataStruct);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_retString">len =64</param>
        /// <returns></returns>

        [DllImport(_dedinetPath, EntryPoint = "ExGetServerVer", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ExGetServerVer();

        [DllImport(_dedinetPath, EntryPoint = "ExGetFileName", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ExGetFileName(ref int _length);

        [DllImport(_dedinetPath, EntryPoint = "ExDismountCallBack")]
        public static extern void ExDismountCallBack(IntPtr _instance);


        #region For NuProg 新增的命令

        /// <summary>
        /// Start NuProg 
        /// </summary>
        /// <param name="_ctgIndex"></param>
        /// <param name="_mask">All = -1 </param>
        /// <returns></returns>
        [DllImport(_dedinetPath, EntryPoint = "ExDediNet_NuProgMaskStart", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ExDediNet_NuProgMaskStart(int _ctgIndex, int _mask);


        ///// <summary>
        ///// Get UFS chip information
        ///// </summary>
        ///// <param name="_instance"></param>
        ///// <param name="_cLength"></param>
        ///// <returns></returns>
        //[DllImport(_dedinetPath, CallingConvention = CallingConvention.Cdecl)]
        //public static extern IntPtr ExGetNuProgChipInfo(IntPtr _instance, ref int _cLength);



        [DllImport(_dedinetPath, EntryPoint = "SetNuProgMasterStatusCallBack", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SetNuProgMasterStatusCallBack([MarshalAs(UnmanagedType.FunctionPtr)] ProgressCallback callbackPointer);

        [DllImport(_dedinetPath, EntryPoint = "ExDediNet_NuProgMaskStop", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ExDediNet_NuProgMaskStop(int ctgIndex, int _mask);


        [DllImport(_dedinetPath, EntryPoint = "ExDediNet_NuProgPlusMaskStop", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ExDediNet_NuProgPlusMaskStop(int ctgIndex, int _mask);




        [DllImport(_dedinetPath, EntryPoint = "SelectIC", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SelectIC(IntPtr _instance,
            [MarshalAs(UnmanagedType.LPWStr)] string _type,
            [MarshalAs(UnmanagedType.LPWStr)] string _mfg,
       [MarshalAs(UnmanagedType.LPWStr)] string _partName);

        [DllImport(_dedinetPath, EntryPoint = "LoadFile", CallingConvention = CallingConvention.Cdecl)]
        public static extern void LoadFile(IntPtr _instance,
         [MarshalAs(UnmanagedType.LPWStr)] string _fileName,
          [MarshalAs(UnmanagedType.LPWStr)] string _format);


        [DllImport(_dedinetPath, EntryPoint = "CreateProject", CallingConvention = CallingConvention.Cdecl)]
        public static extern void CreateProject(IntPtr _instance,
        [MarshalAs(UnmanagedType.LPWStr)] string _filePath,
       [MarshalAs(UnmanagedType.LPWStr)] string batch);


        [DllImport(_dedinetPath, EntryPoint = "CheckICExist", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CheckICExist(int usbIndex, int sktIndex);


        [DllImport(_dedinetPath, EntryPoint = "EnableLogFile", CallingConvention = CallingConvention.Cdecl)]
        public static extern int EnableLogFile(bool bEnable);

        [DllImport(_dedinetPath, EntryPoint = "ExGetChipOptionChecksum", CallingConvention = CallingConvention.Cdecl)]
        public static extern long ExGetChipOptionChecksum();

        [DllImport(_dedinetPath, EntryPoint = "ExGetChipChecksum", CallingConvention = CallingConvention.Cdecl)]
        public static extern long ExGetChipChecksum();

        [DllImport(_dedinetPath, EntryPoint = "ExGetFileInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ExGetFileInfo(ref int Length);

        #endregion

        [DllImport(_dedinetPath, EntryPoint = "FreeArrray", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FreeArrray(IntPtr ptr);

        [DllImport(_dedinetPath, EntryPoint = "FreePointer", CallingConvention = CallingConvention.Cdecl)]
        public static extern void FreePointer(IntPtr ptr);

        [DllImport(_dedinetPath, EntryPoint = "ExGetSerialNoInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ExGetSerialNoInfo(ref int _length);


        [DllImport(_dedinetPath, EntryPoint = "ExChecknGetSocketInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ExChecknGetSocketInfo(int _cIndex, int _sIndex, ref int len);



        [DllImport(_dedinetPath, EntryPoint = "ExChecknGetSocketInfo_buff", CallingConvention = CallingConvention.Cdecl)]

        public static extern IntPtr ExChecknGetSocketInfo_buff(int _cIndex, int _sIndex, ref int len);


        [DllImport(_dedinetPath, EntryPoint = "ExDediNet_CheckSocketExist", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint ExDediNet_CheckSocketExist(int _usbIndex, int slot);


        /// <summary>
        /// 決定是否收到Wait Start 的命令
        /// </summary>
        /// <param name="enable"></param>
        /// <returns></returns>
        [DllImport(_dedinetPath, EntryPoint = "EnableWaitStartMessage", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr EnableWaitStartMessage(bool enable);


        /// <summary>
        /// 檢查燒錄專案是否會有乒乓狀況
        /// </summary>
        /// <returns></returns>
        [DllImport(_dedinetPath, EntryPoint = "CheckProjectIsPinPang", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CheckProjectIsPinPang();
    }
}

//public class DEMO
//    {


//        public void Init()
//        {

//        DP_API.SetConnCallBack(CCB);
//        DP_API.SetCallBack(PCB);
//        }

//        private void CCB(IntPtr value)
//        {
//            /// ret is connect event message
//            string ret = Marshal.PtrToStringAnsi(value);
//        }

//        private void PCB(IntPtr value)
//        {
//            /// _ret is programmer message
//            string ret = string.Copy(Marshal.PtrToStringAnsi(value));

//        }
//    }



    