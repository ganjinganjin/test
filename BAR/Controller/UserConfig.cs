#define  AXIS_YS

#if  AXIS_YS
#else
#define  AXIS_YP
#endif

#define  AXIS_XS

#if  AXIS_XS
#else
#define  AXIS_XP
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PLC;

namespace BAR
{
    public class UserConfig
    {
        public static int CardC = 2;          //板卡数量
        public static int GLinkC = 0;         //板卡1扩展IO模块数量  四吸笔：0；压座、推压2工位：1；推压：2
        public static int AxisZC = 1;         //Z轴数量
        public const int ScketUnitC = 16;      //每组座子数量
        public const int ScketMotionC = 16;    //每个动作座子数量

        public const int ScketGroupC = 6;     //分组数量
        public const int VacuumPenC = 4;      //吸笔数量
        public const int TubeC = 6;           //料管通道数量
        public const int FeederC = 2;         //飞达数量

        public const int AllScketC = ScketUnitC * ScketGroupC;         //烧录座总数
        public const int MotionGroupC = ScketUnitC / ScketMotionC;     //每组动作次数
        public const int AllMotionC = MotionGroupC * ScketGroupC;      //总动作次数

        /// <summary>
        /// 程序人员调试模式
        /// </summary>
        public static bool IsProgrammer = false;
    }

    public class Mm_Per_Pulse
    {
#if AXIS_XS		
		public static double axisX = 20.0d / 10000d; //X轴位移每脉冲
#elif AXIS_XP	
        public static double axisX = 0.01d;
#endif

#if AXIS_YS
        public static double axisY = 40.0d / 20000d; //Y轴位移每脉冲
#elif AXIS_YP	
        public static double axisY = 0.01d
#endif

        public const double axisZ = 80d / 4000d; //Z轴位移每脉冲
        public const double axisC = 360d / 10000d; //C轴位移每脉冲
    }
}
