using System.Runtime.InteropServices;
using System;

namespace PLC
{

    public class lhmtc
    {
        #region lhmtc�ӿ����õ��Ľṹ��
        public struct TVersion
        {
            public short year;
            public short month;
            public short day;
            public short version;//�汾��
            public short chip;//оƬ����
            public short reserve1;
            public short reserve2;
        } ;
        /*�˶�ģʽ*/
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
        /*��λģʽ�˶�����*/
        public struct TrapPrfPrm
        {
            public double acc;
            public double dec;
            public double velStart;
            public short smoothTime;
        }
        /*JOGģʽ�˶�����*/
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
        /*PID����*/
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
        /*�岹�˶�����ϵ����*/
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
            public ushort delay;                         // ��ʱʱ��
            public short doType;                        // ������IO������,0:�����IO
            public ushort doAddress;					 // IOģ���ַ
            public ushort doMask;                        // ������IO�������������
            public ushort doValue;                       // ������IO�����ֵ
            public short dacChannel;					 // DAC���ͨ��
            public short dacValue;					     // DAC���ֵ
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 2, ArraySubType = System.Runtime.InteropServices.UnmanagedType.U2)]
            public ushort[] dataExt;               // ����������չ����
        }


        //ǰհ����������ǰհ��ص����ݽṹ
        public struct TCrdBlockData
        {
            public short iMotionType;                             // �˶�����,0Ϊֱ�߲岹,1Ϊ2DԲ���岹,2Ϊ3DԲ���岹,6ΪIO,7Ϊ��ʱ��8λDAC
            public short iCirclePlane;                            // Բ���岹��ƽ��;XY��1��YZ-2��ZX-3
            public short arcPrmType;							   // 1-Բ�ı�ʾ����2-�뾶��ʾ��
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = System.Runtime.InteropServices.UnmanagedType.I4)]
            public int[] lPos;            // ��ǰ�θ����յ�λ��

            public double dRadius;                                // Բ���岹�İ뾶
            public short iCircleDir;                             // Բ����ת����,0:˳ʱ��;1:��ʱ��
            [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 3, ArraySubType = System.Runtime.InteropServices.UnmanagedType.R8)]
            public double[] dCenter;                             // 2άԲ���岹��Բ���������ֵ����Բ����������λ�õ�ƫ����
            // 3άԲ���岹��Բ�����û�����ϵ�µ�����ֵ
            public int height;								   // �����ߵĸ߶�
            public double pitch;	// �����ߵ��ݾ�
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

            public double dVel;                                   // ��ǰ�κϳ�Ŀ���ٶ�
            public double dAcc;                                   // ��ǰ�κϳɼ��ٶ�
            public short loop;
            public short iVelEndZero;                             // ��־��ǰ�ε��յ��ٶ��Ƿ�ǿ��Ϊ0,ֵ0������ǿ��Ϊ0;ֵ1����ǿ��Ϊ0
            public TCrdBufOperation operation;
            public double dVelEnd;                                // ��ǰ�κϳ��յ��ٶ�
            public double dVelStart;                              // ��ǰ�κϳɵ���ʼ�ٶ�
            public double dResPos;                                // ��ǰ�κϳ�λ����

        }
        //λ�ñȽ�
        public struct TMDComparePrm
        {
            public short encx;             //���
            public short ency;
            public short encz;
            public short enca;
            public short source;          //�Ƚ�Դ�� 0���滮   1:����
            public short outputType;      //�����ʽ��0������  1����ƽ
            public short startLevel;      //��ʼ��ƽ
            public short time;            //�Ƚ�������������ؿ��   ��λ100us
            public short maxerr;          //�ȽϷ�Χ������
            public short threshold;       //�����㷨��ֵ
            public short pluseCount;      //����������
            public short spacetime;       //�����½��ؿ��
            public short delaytime;       //�����ʱʱ��
        };
        public struct TMDCompareData
        {
            public int px;              //�Ƚ�λ��
            public int py;
            public int pz;
            public int pa;
        };

        public struct TMDCompareDataEX
        {
            public int px;              //�Ƚ�λ��
            public int py;
            public int pz;
            public int pa;
            public short time;           //�����ؿ��  ��λ100us
            public short spacetime;      //�½��ؿ��
            public short delaytime;      //��ʱʱ��
            public short pluseCount;     //�������
        };

        //���ݲɼ�
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
                #define SAMP_SRC_PRF_POS             0  �滮λ��
                #define SAMP_SRC_ENC_POS             1  ����λ��
                #define SAMP_SRC_ERR_POS             2  �������
                #define SAMP_SRC_PRF_VEL             3  �滮�ٶ�
                #define SAMP_SRC_ENC_VEL             4  �����ٶ�
                #define SAMP_SRC_PRF_ACC             5  �滮���ٶ�  
                #define SAMP_SRC_ENC_ACC             6  �������ٶ�
                #define SAMP_SRC_VALUE_ADC			 7  ADCֵ
                #define SAMP_SRC_VALUE_DAC			 8  DACֵ
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

        /*��ʼ������***********************************************************************************************************/
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
        /*���������*************************************************************************************************************/
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
        //���ʱ�����
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetEncPos(short encoder, out double pValue, short count, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetEncVel(short encoder, out double pValue, short count, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetEncPos(short encoder, int encPos, short cardNum);
        //ϵͳ��������
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
        
        //����λ
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

        //��λ�˶�ָ��
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

        //Jogָ��
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_PrfJog(short profile, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetJogPrm(short profile, ref JogPrfPrm pPrm, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetJogPrm(short profile, out JogPrfPrm pPrm, short cardNum);
         //PTģʽ
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

        //Gear �˶�
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

        //Followģʽ
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

        //��������IO
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
        //������չ����IO
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
        public static extern short LH_SetExtendCardCount(short count, short cardNum); //����IO��չ��ĸ���
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetDoBitReverse(short doType, short doIndex, short value, short reverseTime, short cardNum);

        //����DAC
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetDacValue(short dac, ref short pValue, short count, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetDacValue(short dac, ref short pValue, short count, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetAdcValue(short channel, ref short pValue, short count, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetAdcValue(short channel, ref double pValue, short count, short cardNum);
        
        //Home/IndexӲ������
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
		//�Զ�����
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
        //λ�ñȽϹ���
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_MDCompareMode(short chn,short dimMode,short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_MDComparePulse(short chn, short level, short outputType, short time,int lPluseCount,short spacetime,short delayTime,short cardNum);
        //chn:ͨ��0-3   chnMode:0-��������  1-��������    Hsio:HSIO���� 0-1   delayTime:��ʱʱ��   time�������ؿ�� ��λ100us    spacetime���½��ؿ��  pluseCount��������� 1-255
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
        //�岹
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

        //ǰհ����
        // x y z���յ����꣬ interX, interY, interZ���м������
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_ArcXYZ(short crd, double x, double y, double z, double interX, double interY, double interZ, double synVel, double synAcc, double velEnd, short fifo, short cardNum);
        //����2άԲ���뾶���յ�����뷽ʽ�������߲岹  xyz���յ����� �յ�����Ҫ���ݾ���Ϣƥ��
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_HelicalLineXYR(short crd, int x, int y, int z, double radius, short circleDir, double pitch, double synVel, double synAcc, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_HelicalLineYZR(short crd, int y, int z, int x, double radius, short circleDir, double pitch, double synVel, double synAcc, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_HelicalLineZXR(short crd, int z, int x, int y, double radius, short circleDir, double pitch, double synVel, double synAcc, short fifo, short cardNum);

        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        //����2άԲ��Բ�ĺ��յ�����뷽ʽ�������߲岹	xyz���յ����� �յ�����Ҫ���ݾ���Ϣƥ��
        public static extern short LH_HelicalLineXYC(short crd, int x, int y, int z, double xCenter, double yCenter, short circleDir, double pitch, double synVel, double synAcc, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_HelicalLineYZC(short crd, int y, int z, int x, double yCenter, double zCenter, short circleDir, double pitch, double synVel, double synAcc, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_HelicalLineZXC(short crd, int z, int x, int y, double zCenter, double xCenter, short circleDir, double pitch, double synVel, double synAcc, short fifo, short cardNum);

        //���ڿռ�Բ���������߲岹����Ҫ�������Ϊ����������Բ�������Բ���������㣨���ϵ�ǰ��㹹������Բ�����������ߵĸ߶ȣ��������ţ��������ֶ����ж�����������z������򣩣������ߵ��ݾࣨ���������������Զ����������ߵ��յ㣬�û���Ҫ���յ�ͣ���������ʶ
        // x y z���յ����꣬ interX, interY, interZ���м������        
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_HelicalLineXYZ(short crd, double x, double y, double z, double interX, double interY, double interZ, int height, double pitch, double synVel, double synAcc, double velEnd, short fifo, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_InitLookAhead(short crd, short fifo, double T, double accMax, short n, ref TCrdBlockData pLookAheadBuf, short cardNum);
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_CrdData(short crd, ref TCrdBlockData pCrdData, short fifo, short cardNum);

        //���ݲ���
        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_SetSamplePrm(ref TSampParam pPrm, short cardNum);

        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_StartSample(short start, short cardNum);

        [DllImport("lhmtc.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern short LH_GetSampleData(ref short pGetLen, IntPtr DataArr, ref short Status, short cardNum);
    }
    
}
