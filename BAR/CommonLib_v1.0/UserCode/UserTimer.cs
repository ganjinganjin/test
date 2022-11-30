using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAR
{
    public class UserTimer
    {
        private static UInt64 SysTick;
        private static DateTime openTime = DateTime.Now;
        private static UInt64 run_Toggle;
        public static bool blink;

        public static void Blink()
        {
            if (GetSysTime() >= run_Toggle)
            {
                blink = !blink;
                run_Toggle = GetSysTime() + 500;
            }
        }

        public static UInt64 GetSysTime()
        {
            SysTick = (ulong)(DateTime.Now - openTime).TotalMilliseconds;
            return SysTick;
        }
    }
}
