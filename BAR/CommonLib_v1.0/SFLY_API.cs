using System.Runtime.InteropServices;

namespace BAR
{
    class SFLY_API
    {
        public const short RESULT_OK = 0;
        public const short RESULT_ERR_MCP = 1;
        public const short RESULT_ERR_MSG = 2;
        public const short RESULT_ERR_INIT = 3;
        public const short RESULT_ERR_PARAM = 4;
        public const short RESULT_ERR_BUSY = 5;

        [DllImport("rmc.dll")]
        public static extern short RMC_Init();
        [DllImport("rmc.dll")]
        public static extern short RMC_Start(int iSocket);
        [DllImport("rmc.dll")]
        public static extern short RMC_GetSocketStatus(byte[] pStatus);
    }
}
