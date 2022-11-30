using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BAR
{
    class AC_API
    {
        const string DLLPath = "C:\\ACROVIEW\\MultiAprog\\ProgCtrl.dll";
        [DllImport(DLLPath, CallingConvention = CallingConvention.StdCall)]
        public static extern bool AC_StartService();

        [DllImport(DLLPath)]
        public static extern bool AC_StopService();

        [DllImport(DLLPath)]
        public static extern bool AC_GetChecksum(byte[] name);

        [DllImport(DLLPath)]
        public static extern bool AC_GetProjectInfo_Json(byte[] Buffer, int Size, IntPtr pSizeNeed);

        [DllImport(DLLPath)]
        public static extern bool AC_LoadProject(string FilePath);

        [DllImport(DLLPath)]
        public static extern int AC_LoadProjectWithLot(string ProjectFile, string FuncMode, int Lot);

        [DllImport(DLLPath)]
        public static extern bool AC_GetResult(byte[] Buffer, int Size);

        [DllImport(DLLPath)]
        public static extern bool AC_StopProject();

        [DllImport(DLLPath)]
        public static extern bool AC_StartProject();

        [DllImport(DLLPath)]
        public static extern bool AC_GetStatus_Json(byte[] Buffer, int Size);

        public delegate int MsgHandler(IntPtr Para, string Msg, string MsgData);
        [DllImport(DLLPath)]
        public static extern int AC_SetMsgHandle(MsgHandler MsgHandle, IntPtr Para);
    }
}
