using System.Runtime.InteropServices;
using System;

namespace PLC
{

    public class lhmtc
    {
        #region lhmtc接口中用到的结构体
        public struct TVersion
        {
            public short year;
            public short month;
            public short day;
            public short version;//版本号
            public short chip;//芯片代码
            public short reserve1;
            public short reserve2;
        } ;
        /*运动模式*/
        public enum MotionMode
        {
            Trap = 0,
            Jog = 1,
            Pt = 2,
            Gear = 3,
            Follow = 4,
            Crd = 5,
            Pvt = 6,
            Home = 7
        }
        /*点位模式运动参数*/
        public struct TrapPrfPrm
        {
            public double acc;
            public double dec;
            public double velStart;
            public short smoothTime;
        }
        /*JOG模式运动参数*/
        public struct JogPrfPrm
        {
            public double acc;
            public double dec;
            public double smooth;
        }
        //public struct THomeStatus
        //{
        //    public short run;
        //    public short stage;
        //    public short error;
        //    public int capturePos;
        //    public int targetPos;
        //}
        /*PID参数*/
        public struct PidParam
        {
            public double kp;
            public double ki;
            public double kd;
            public double kvff;
            public double kaff;
            public int integralLimit;
            public int derivativeLimit;
            public short limit;
        }
        /*插补运动坐标系参数*/
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct CrdCfg
        {
            /// short
            public short dimension;
            /// short[8]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I2)]
            public short[] profile;
            /// short
            public short setOriginFlag;
            /// int[8]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I4)]
            public int[] orignPos;
            /// short
            public short evenTime;
            /// double
            public double synVelMax;
            /// double
            public double synAccMax;
            /// double
            public double synDecSmooth;
            /// double
            public double synDecAbrupt;
        }

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct TCrdBufOperation
        {
            public ushort delay;                         // 延时时间
            public short doType;                        // 缓存区IO的类型,0:不输出IO
            public ushort doAddress;					 // IO模块地址
            public ushort doMask;                        // 缓存区IO的输出控制掩码
            public ushort doValue;                       // 缓存区IO的输出值
            public short dacChannel;					 // DAC输出通道
            public short dacValue;					     // DAC输出值
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U2)]
            public ushort[] dataExt;               // 辅助操作扩展数据
        }


        //前瞻缓冲区；与前瞻相关的数据结构
        public struct TCrdBlockData
        {
            public short iMotionType;                             // 运动类型,0为直线插补,1为2D圆弧插补,2为3D圆弧插补,6为IO,7为延时，8位DAC
            public short iCirclePlane;                            // 圆弧插补的平面;XY—1，YZ-2，ZX-3
            public short arcPrmType;							   // 1-圆心表示法；2-半径表示法
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I4)]
            public int[] lPos;            // 当前段各轴终点位置

            public double dRadius;                                // 圆弧插补的半径
            public short iCircleDir;                             // 圆弧旋转方向,0:顺时针;1:逆时针
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = System.Runtime.InteropServices.UnmanagedType.R8)]
            public double[] dCenter;                             // 2维圆弧插补的圆心相对坐标值，即圆心相对于起点位置的偏移量
            // 3维圆弧插补的圆心在用户坐标系下的坐标值
            public int height;								   // 螺旋线的高度
            public double pitch;	// 螺旋线的螺距
            //double[3]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = System.Runtime.InteropServices.UnmanagedType.R8)]
            public double[] beginPos;
            //double[3]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = System.Runtime.InteropServices.UnmanagedType.R8)]
            public double[] midPos;
            //double[3]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = System.Runtime.InteropServices.UnmanagedType.R8)]
            public double[] endPos;
            //double[3][3]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = System.Runtime.InteropServices.UnmanagedType.R8)]
            public double[] R_inv;
            //double[3][3]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 9, ArraySubType = System.Runtime.InteropServices.UnmanagedType.R8)]
            public double[] R;

            public double dVel;                                   // 当前段合成目标速度
            public double dAcc;                                   // 当前段合成加速度
            public short loop;
            public short iVelEndZero;                             // 标志当前段的终点速度是否强制为0,值0——不强制为0;值1——强制为0
            public TCrdBufOperation operation;
            public double dVelEnd;                                // 当前段合成终点速度
            public double dVelStart;                              // 当前段合成的起始速度
            public double dResPos;                                // 当前段合成位移量

        }
        //位置比较
        public struct TMDComparePrm
        {
            public short encx;             //轴号
            public short ency;
            public short encz;
            public short enca;
            public short source;          //比较源： 0：规划   1:反馈
            public short outputType;      //输出方式：0：脉冲  1：电平
            public short startLevel;      //起始电平
            public short time;            //比较输出脉冲上升沿宽度   单位100us
            public short maxerr;          //比较范围最大误差
            public short threshold;       //最优算法阈值
            public short pluseCount;      //输出脉冲个数
            public short spacetime;       //脉冲下降沿宽度
            public short delaytime;       //输出延时时间
        };
        public struct TMDCompareData
        {
            public int px;              //比较位置
            public int py;
            public int pz;
            public int pa;
        };

        public struct TMDCompareDataEX
        {
            public int px;              //比较位置
            public int py;
            public int pz;
            public int pa;
            public short time;           //上升沿宽度  单位100us
            public short spacetime;      //下降沿宽度
            public short delaytime;      //延时时间
            public short pluseCount;     //脉冲个数
        };

        //数据采集
       [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct TSampParam
        {
	        public short rate; //Sampling rate [ 1 ~ 65535 times of cycle]
	        public short edge; //Trigger edge [ 0: Rising edge, 1: Faling edge ]
	        public short startType;//0-start sample    1-trigger sample
	        public int level; //Trigger level [ -214743648 ~ 2147483647 ]
	        public short trigCh; //Trigger channel [ 0 ~ 7 ]
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I2)]
            public short[] sourceByCh;//short[8*2]  sourceByCh[0-1}:Channe-0  sourceByCh[2-3]:channel-1 .... 
            /* Channel
                #define SAMP_SRC_PRF_POS             0  规划位置
                #define SAMP_SRC_ENC_POS             1  反馈位置
                #define SAMP_SRC_ERR_POS             2  跟随误差
                #define SAMP_SRC_PRF_VEL             3  规划速度
                #define SAMP_SRC_ENC_VEL             4  反馈速度
                #define SAMP_SRC_PRF_ACC             5  规划加速度  
                #define SAMP_SRC_ENC_ACC             6  反馈加速度
                #define SAMP_SRC_VALUE_ADC			 7  ADC值
                #define SAMP_SRC_VALUE_DAC			 8  DAC值
            */
            //b: 1-8
            //
        };

        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct TSampData8Ch
        {
            public uint tick;
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = System.Runtime.InteropServices.UnmanagedType.R8)]
            public double[] data;//double[8]
        }
        #endregion

        /*初始化部分***********************************************************************************************************/
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_Open(short channel, short param,short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_Close(short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_Reset(short cardNum);
        [DllImport("lhmtc.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetVersion(short type, ref TVersion pVersion,short cardNum);
        [DllImport("lhmtc.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetCardNum(short cardNum);
        [DllImport("lhmtc.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_LoadConfig(string pFile,short cardNum);
        /*轴基本操作*************************************************************************************************************/
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetSts(short axis, out int pSts, short count, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_ClrSts(short axis, short count, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_AxisOn(short axis, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_AxisOff(short axis, short cardNum);

        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_Stop(short mask, short option, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_ZeroPos(short axis, short count, short cardNum);

        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_AlarmOn(short axis,  short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_AlarmOff(short axis, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_LmtsOn(short axis, short limitType, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_LmtsOff(short axis, short limitType,short cardNum);

        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetPrfPos(short profile, int prfPos, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetPrfMode(short profile, out short pValue, short count, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetPrfPos(short profile, out double pValue, short count, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetPrfVel(short profile, out double pValue, short count, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetPrfAcc(short profile, out double pValue, short count, short cardNum);
        //访问编码器
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetEncPos(short encoder, out double pValue, short count, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetEncVel(short encoder, out double pValue, short count, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetEncPos(short encoder, int encPos, short cardNum);
        //系统参数配置
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetControlPid(short control, short index, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetControlPid(short control, out short pIndex, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetPid(short control, short index, ref PidParam pPid, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetPid(short control, short index, out PidParam pPid, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetFollowError(short control, int error, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetFollowError(short control, out int pError, short cardNum);

        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetMtrBias(short dac, short bias, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetMtrBias(short dac, out short bias, short cardNum);
        
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetMtrLimit(short dac, short bias, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetMtrLimit(short dac, out short bias, short cardNum);

        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetCtrlMode(short axis, short mode, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetCtrlMode(short axis, out short mode, short cardNum);
        
        //软限位
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetSoftLimit(short axis, int positive, int negative, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetSoftLimit(short axis, out int pPositive, out int pNegative, short cardNum);

        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetEncSns(uint sense, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetEncSns(out uint sense,short cardNum);

        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetEncSrc(short axis, short sense, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetEncSrc(short axis, out short sense, short cardNum);

        //点位运动指令
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_PrfTrap(short profile, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetTrapPrm(short profile, ref TrapPrfPrm pPrm, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetTrapPrm(short profile, out TrapPrfPrm pPrm, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetPos(short profile, int pos, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetPos(short profile, out int pPos, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetVel(short profile, double vel, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetVel(short profile, out double pVel, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_Update(int mask, short cardNum);

        //Jog指令
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_PrfJog(short profile, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetJogPrm(short profile, ref JogPrfPrm pPrm, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetJogPrm(short profile, out JogPrfPrm pPrm, short cardNum);
         //PT模式
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_PrfPt(short profile, short mode, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_PtSpace(short profile, out short pSpace, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_PtData(short profile, int pos, int time, short type, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_PtClear(short profile, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_PtStart(int mask, int option, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetPtLoop(short profile, int loop, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetPtLoop(short profile, out int loop, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetPtMemory(short profile, short memory, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetPtMemory(short profile, out short memory, short cardNum);

        //Gear 运动
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_PrfGear(short profile, short dir, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetGearMaster(short profile, short masterindex, short masterType, short masterItem, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetGearMaster(short profile, out short masterindex, out short masterType, out short masterItem, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetGearRatio(short profile, int masterEven, int slaveEven, int masterSlope, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetGearRatio(short profile, out int masterEven, out int slaveEven, out int masterSlope, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GearStart(int mask, short cardNum);

        //Follow模式
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_PrfFollow(short profile, short dir, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetFollowMaster(short profile, short masterIndex, short masterType, short masterItem, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetFollowMaster(short profile, out short MasterIndex, out short MasterType, out short MasterItem, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetFollowLoop(short profile, short loop, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetFollowLoop(short profile, out int pLoop, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetFollowEvent(short profile, short even, short masterDir, int pos, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetFollowEvent(short profile, out short pEvent, out short pMasterDir, out int pPos, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_FollowSpace(short profile, out short pSpace, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_FollowData(short profile, int masterSegment, int slaveSegment, short type, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_FollowClear(short profile, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_FollowStart(int mask, int option, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_FollowSwitch(int mask, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetFollowMemory(short profile, short memory, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetFollowMemory(short profile, out short pMemory, short cardNum);

        //访问数字IO
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetDi(short diType, out int pValue, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetDiRaw(short diType, out int pValue, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetDo(short doType,int value, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetDoBit(short doType, short doIndex, short value, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetDo(short doType, out int pValue, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        //访问扩展数字IO
        public static extern short LH_GetExtendDi(short address, out int pValue, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetExtendDiBit(short address, short diIndex, ref short value, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetExtendDo(short address, int value, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetExtendDoBit(short address, short doIndex, short value, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetExtendDo(short address, out int pValue, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetExtendCardCount(short count, short cardNum); //设置IO扩展板的个数
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetDoBitReverse(short doType, short doIndex, short value, short reverseTime, short cardNum);

        //访问DAC
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetDacValue(short dac, ref short pValue, short count, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetDacValue(short dac, ref short pValue, short count, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetAdcValue(short channel, ref short pValue, short count, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetAdcValue(short channel, ref double pValue, short count, short cardNum);
        
        //Home/Index硬件捕获
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetCaptureMode(short encoder, short mode, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetCaptureMode(short encoder, out short pMode, short count, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetCaptureStatus(short encoder, out short pStatus, out int pValue, short count, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetCaptureSense(short encoder, short mode, short sense, short cardNum);
        
		//Smart Home
        public const short HOME_STAGE_IDLE=0;
        public const short HOME_STAGE_START=1;
        public const short HOME_STAGE_ON_HOME_LIMIT_ESCAPE=2;
        public const short HOME_STAGE_SEARCH_LIMIT=10;
        public const short HOME_STAGE_SEARCH_LIMIT_STOP=11;
        public const short HOME_STAGE_SEARCH_LIMIT_ESCAPE = 13;
        public const short HOME_STAGE_SEARCH_LIMIT_RETURN=15;
        public const short HOME_STAGE_SEARCH_LIMIT_RETURN_STOP=16;
        public const short HOME_STAGE_SEARCH_HOME=20;
        public const short HOME_STAGE_SEARCH_HOME_STOP=22;
        public const short HOME_STAGE_SEARCH_HOME_RETURN=25;
        public const short HOME_STAGE_SEARCH_INDEX=30;
        public const short HOME_STAGE_SEARCH_GPI=40;
        public const short HOME_STAGE_SEARCH_GPI_RETURN=45;
        public const short HOME_STAGE_GO_HOME=80;
        public const short HOME_STAGE_END=100;
        public const short HOME_ERROR_NONE=0;
        public const short HOME_ERROR_NOT_TRAP_MODE=1;
        public const short HOME_ERROR_DISABLE=2;
        public const short HOME_ERROR_ALARM=3;
        public const short HOME_ERROR_STOP=4;
        public const short HOME_ERROR_STAGE=5;
        public const short HOME_ERROR_HOME_MODE=6;
        public const short HOME_ERROR_SET_CAPTURE_HOME=7;
        public const short HOME_ERROR_NO_HOME=8;
        public const short HOME_ERROR_SET_CAPTURE_INDEX=9;
        public const short HOME_ERROR_NO_INDEX=10;
        public const short HOME_MODE_LIMIT=10;
        public const short HOME_MODE_LIMIT_HOME=11;
        public const short HOME_MODE_LIMIT_INDEX=12;
        public const short HOME_MODE_LIMIT_HOME_INDEX=13;
        public const short HOME_MODE_HOME=20;
        public const short HOME_MODE_HOME_INDEX=22;
        public const short HOME_MODE_INDEX = 30;
        public const short HOME_MODE_FORCED_HOME=40;
        public const short HOME_MODE_FORCED_HOME_INDEX=41;
        
        public struct THomePrm
        {
	        public short mode;						
	        public short moveDir;					
	        public short indexDir;					
	        public short edge;					
	        public short triggerIndex;			
			public short pad1_1;
	        public short pad1_2;
            public short pad1_3;
	        public double velHigh;				
	        public double velLow;				
	        public double acc;					
	        public double dec;
	        public short smoothTime;
			public short pad2_1;
		    public short pad2_2;
            public short pad2_3;
	        public int homeOffset;				
	        public int searchHomeDistance;	
	        public int searchIndexDistance;	
	        public int escapeStep;			
            public int pad3_1;
            public int pad3_2;
            public int pad3_3;
        } 
        public struct THomeStatus
        {
	        public short run;
	        public short stage;
            public short error;
            public short pad1;
	        public int capturePos;
	        public int targetPos;
        }
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short GT_GoHome(short cardNum,short axis, ref THomePrm pHomePrm);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short GT_GetHomePrm(short cardNum,short axis, out THomePrm pHomePrm);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short GT_GetHomeStatus(short cardNum,short axis, out THomeStatus pHomeStatus);
		//自动回零
		[DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_HomeInitAxis(short axis,short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_HomeInit(short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_Home(short axis, int pos, double vel, double acc, int offset, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_Index(short axis, int pos, int offset, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_HomeStop(short axis, int pos, double vel, double acc, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_HomeSts(short axis, out ushort pStatus, short cardNum);
        //位置比较功能
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_MDCompareMode(short chn,short dimMode,short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_MDComparePulse(short chn, short level, short outputType, short time,int lPluseCount,short spacetime,short delayTime,short cardNum);
        //chn:通道0-3   chnMode:0-脉冲引脚  1-方向引脚    Hsio:HSIO触发 0-1   delayTime:延时时间   time：上升沿宽度 单位100us    spacetime：下降沿宽度  pluseCount：输出个数 1-255
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_MDComparePulseEx(short chn, short chnMode, short hsio, short delayTime, short time,short spacetime,short pluseCount,short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_MDComparePulseExStop(short chn, short chnMode,short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_MDCompareBindOn(short chn, short time,int lPluseCount,short spacetime,short inputIo,short senselevel,short delayTime,short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)] 
        public static extern short LH_MDCompareGetBindPrm(short chn, out short bind,out short time,out int lPluseCount,out short spacetime,out short inputIo,out short senselevel,out short delayTime,short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_MDCompareBindOff(short chn,short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_MDCompareSetPrm(short chn, ref TMDComparePrm pPrm,short mode ,short cardNum);  // mode: 0-LH_MDCompareData   ;1 - LH_MDCompareDataEx
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)] 
        public static extern short  LH_MDCompareData(short chn, short count, ref  TMDCompareData pBuf, short fifo ,short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short  LH_MDCompareDataEx(short chn, short count,ref TMDCompareDataEX pBuf, short fifo ,short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_MDCompareClear(short chn,short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_MDCompareStatus(short chn, out short pStatus,out int pCount, out short pFifo, out short pFifoCount, out short pBufCount,out int pTriggerPos,short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_MDCompareStart(short chn,short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_MDCompareStop(short chn,short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short  LH_SetComparePort(short chn, short hsio0, short hsio1,short cardNum);
        //插补
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetCrdPrm(short crd, ref CrdCfg pCrdPrm, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetCrdPrm(short crd, out CrdCfg pCrdPrm, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_CrdSpace(short crd, out int pSpace, short count, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_CrdClear(short crd, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_LnXY(short crd, int x, int y, double synVel, double synAcc, double velEnd, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_LnXYZ(short crd, int x, int y, int z, double synVel, double synAcc, double velEnd, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_LnXYZA(short crd, int x, int y, int z, int a, double synVel, double synAcc, double velEnd, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_ArcXYR(short crd, int x, int y, double radius, short circleDir, double synVel, double synAcc, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_ArcYZR(short crd, int y, int z, double radius, short circleDir, double synVel, double synAcc, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_ArcZXR(short crd, int z, int x, double radius, short circleDir, double synVel, double synAcc, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_ArcXYC(short crd, int x, int y, double xCenter, double yCenter, short circleDir, double synVel, double synAcc, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_ArcYZC(short crd, int y, int z, double yCenter, double zCenter, short circleDir, double synVel, double synAcc, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_ArcZXC(short crd, int z, int x, double zCenter, double xCenter, short circleDir, double synVel, double synAcc, short fifo, short cardNum);

        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetLastCrdPos(short crd, out int position, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_BufIO(short crd, ushort address, ushort doMask, ushort doValue, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_BufDelay(short crd, uint delayTime, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_BufDac(short crd, short chn, short daValue, bool bGear, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_BufGear(short crd, short axis, int pos, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_BufMove(short crd, short moveAxis, int pos, double vel, double acc, short modal, short fifo, short cardNum);

        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_BufLmtsOn(short crd, short axis, short limitType, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_BufLmtsOff(short crd, short axis, short limitType, short fifo, short cardNum);

        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_BufSetStopIo(short crd, short axis, short stoptype, short inputtype, short inputindex, short fifo, short cardNum);

        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_CrdStart(short mask, short option, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_CrdStop(short mask, short option, short cardNum);

        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_CrdStatus(short crd, out short pSts, out short pCmdNum, out int pSpace, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetCrdPos(short crd, out double pPos, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetCrdVel(short crd, out double pSynVel, short cardNum);

        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetCrdStopDec(short crd, double decSmoothStop, double decAbruptStop, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetCrdStopDec(short crd, out double decSmoothStop, out double decAbruptStop, short cardNum);

        //前瞻部分
        // x y z是终点坐标， interX, interY, interZ是中间点坐标
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_ArcXYZ(short crd, double x, double y, double z, double interX, double interY, double interZ, double synVel, double synAcc, double velEnd, short fifo, short cardNum);
        //基于2维圆弧半径加终点的输入方式的螺旋线插补  xyz是终点坐标 终点坐标要跟螺距信息匹配
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_HelicalLineXYR(short crd, int x, int y, int z, double radius, short circleDir, double pitch, double synVel, double synAcc, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_HelicalLineYZR(short crd, int y, int z, int x, double radius, short circleDir, double pitch, double synVel, double synAcc, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_HelicalLineZXR(short crd, int z, int x, int y, double radius, short circleDir, double pitch, double synVel, double synAcc, short fifo, short cardNum);

        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        //基于2维圆弧圆心和终点的输入方式的螺旋线插补	xyz是终点坐标 终点坐标要跟螺距信息匹配
        public static extern short LH_HelicalLineXYC(short crd, int x, int y, int z, double xCenter, double yCenter, short circleDir, double pitch, double synVel, double synAcc, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_HelicalLineYZC(short crd, int y, int z, int x, double yCenter, double zCenter, short circleDir, double pitch, double synVel, double synAcc, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_HelicalLineZXC(short crd, int z, int x, int y, double zCenter, double xCenter, short circleDir, double pitch, double synVel, double synAcc, short fifo, short cardNum);

        //基于空间圆弧的螺旋线插补，主要输入参数为定义螺旋线圆柱底面的圆弧的两个点（加上当前起点构成三点圆弧），螺旋线的高度（有正负号，根据右手定则判断螺旋线虚拟z轴的正向），螺旋线的螺距（正数），函数会自动计算螺旋线的终点，用户需要有终点停在哪里的意识
        // x y z是终点坐标， interX, interY, interZ是中间点坐标        
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_HelicalLineXYZ(short crd, double x, double y, double z, double interX, double interY, double interZ, int height, double pitch, double synVel, double synAcc, double velEnd, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_InitLookAhead(short crd, short fifo, double T, double accMax, short n, ref TCrdBlockData pLookAheadBuf, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_CrdData(short crd, ref TCrdBlockData pCrdData, short fifo, short cardNum);

        //数据采样
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetSamplePrm(ref TSampParam pPrm, short cardNum);

        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_StartSample(short start, short cardNum);

        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetSampleData(ref short pGetLen, IntPtr DataArr, ref short Status, short cardNum);
    }
    
}
