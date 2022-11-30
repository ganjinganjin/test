using BAR.Commonlib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR
{
    public class Btn
    {

        #region MyRegion
        public static Act g_act = Act.GetInstance();

        int BtnReset_Step = 0;
        public static bool PauseFlag;

        private UInt64 Timer_BtnResetDelay;
        #endregion

        /// <summary>
        /// 外部按钮处理函数
        /// </summary>
        public void Button_Handle()
        {
            BtnReset_Handle();
        }

        private void BtnReset_Handle()
        {
            switch (BtnReset_Step)
            {
                case 0:
                    if (In_Output.BtnResetI.M)
                    {
                        Timer_BtnResetDelay = UserTimer.GetSysTime() + 2000;
                        BtnReset_Step = 1;
                    }
                    break;

                case 1:
                    if (In_Output.BtnResetI.M)
                    {
                        if (UserTimer.GetSysTime() > Timer_BtnResetDelay)
                        {
                            BtnReset_Step = 2;
                        }
                    }
                    else
                    {
                        BtnReset_Step = 0;
                    }
                    break;

                case 2:
                    In_Output.BtnResetO.M = true;
                    Reset.ResetState(true);
                    Axis.Home_Start();//回原点
                    BtnReset_Step = 3;
                    break;

                case 3:
                    if (!Auto_Flag.HomeBusy)
                    {
                        In_Output.BtnResetO.M = false;
                        g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "复位成功", "Flow");
                        BtnReset_Step = 0;
                    }
                    break;
                default:
                    break;
            }
        }
    }

}
