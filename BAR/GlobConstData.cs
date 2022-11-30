using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAR
{
    public sealed class GlobConstData
    {
        //烧录器类型
        public const short Programmer_AK = 0;           //昂科
        public const short Programmer_DP = 1;           //岱镨
        public const short Programmer_WG = 2;           //外挂
        public const short Programmer_YED = 3;          //易而达
        public const short Programmer_RD = 4;           //瑞德
        public const short Programmer_SFLY = 5;         //硕飞
        public const short Programmer_WG_TY = 6;        //推压式外挂
        /// <summary>
        /// 欣扬电子
        /// </summary>
        public const short Programmer_XYDZ = 7;        //欣扬电子

        //外挂通讯协议
        /// <summary>
        /// 外挂标准协议
        /// </summary>
        public const short Protocol_WG = 0;
        /// <summary>
        /// 外挂成都时代速信协议
        /// </summary>
        public const short Protocol_WG_SDSX = 1;

        ///<光源类型
        public const short LightCtl_COM = 0;
        public const int LightCtl_WAN = 1;

        /// <summary>
        /// 大恒相机
        /// </summary>
        public const int Camera_DH = 0;
        /// <summary>
        /// 大华相机
        /// </summary>
        public const int Camera_HR = 1;

        /// <summary>
        /// 岱镨Logo
        /// </summary>
        public const int Logo_DP = 8;

        /// <summary>
        /// 湖南志浩航Logo
        /// </summary>
        public const int Logo_ZHH = 9;

        /// <summary>
        /// 联阳电子Logo
        /// </summary>
        public const int Logo_LYDZ = 10;

        /// <summary>
        /// PC端自动获取相机IP
        /// </summary>
        public const int CameraIP_Auto = 0;
        /// <summary>
        /// PC端固定设置相机IP
        /// </summary>
        public const int CameraIP_Fixed = 1;

        /// <summary>
        /// 定拍
        /// </summary>
        public const int CCDModel_Slow = 0;
        /// <summary>
        /// 飞拍
        /// </summary>
        public const int CCDModel_Fast = 1;

        /// <summary>
        /// 固高运动卡
        /// </summary>
        public const int MotionCard_GTS = 0;
        /// <summary>
        /// 唯精运动卡
        /// </summary>
        public const int MotionCard_LH = 1;

        //130万下相机图像大小 720 540
        public static short IMG130_WIDTH = 1280;//1280;
        public static short IMG130_HEIGHT = 1024;//1024;

        //500万上相机图像大小
        public static short IMG500_WIDTH = 2592;//2592;
        public static short IMG500_HEIGHT = 1944;//1944;

        //当前打开的界面
        public static short SELECT_MAIN_WND = 0;
        public static short SELECT_CONFIGINIT_WND = 1;

        //上光源
        public static short ST_LIGHTUP = 0;
        //下光源
        public static short ST_LIGHTDOWN = 1;
        public static short ST_CCDCount = 2;
        //上相机
        public const short ST_CCDUP = 0;
        //下相机
        public const short ST_CCDDOWN = 1;

        //图像画线类型
        public static short DISPLAYLINETYPE_CROSS = 0;
        public static short DISPLAYLINETYPE_SCALE = 1;
        public static short DISPLAYLINETYPE_BACKRECT = 2;

        //示教模板类型
        /// <summary>
        /// 上相机 IC座精度模板
        /// </summary>
        public const short ST_MODELICSTATACC = 0;
        /// <summary>
        /// 上相机	料盘精度模板
        /// </summary>
        public const short ST_ModelTrayACC = 1;
        /// <summary>
        /// 下相机	吸头精度模板
        /// </summary>
        public const short ST_MODELPENACC = 2;
        /// <summary>
        /// 上相机	IC座定位模板
        /// </summary>
        public const short ST_MODELICSTATPOS = 3;
        /// <summary>
        /// 上相机	料盘定位模板
        /// </summary>
        public const short ST_ModelTrayPOS = 4;
        /// <summary>
        /// 下相机	IC前拍定位模板
        /// </summary>
        public const short ST_MODELICPOS = 5;
        /// <summary>
        /// 下相机	IC后拍定位模板1
        /// </summary>
        public const short ST_MODELICPOS_BACK1 = 6;
        /// <summary>
        /// 下相机	IC后拍定位模板2
        /// </summary>
        public const short ST_MODELICPOS_BACK2 = 7;
        /// <summary>
        /// 下相机	IC后拍定位模板3
        /// </summary>
        public const short ST_MODELICPOS_BACK3 = 8;
        /// <summary>
        /// IC前拍NCC定位模板
        /// </summary>
        public const short ST_MODELICPOS_NCC = 10;
        /// <summary>
        /// 上相机	飞达精度模板
        /// </summary>
        public const short ST_ModelBredeInACC = 11;
        /// <summary>
        /// 上相机	编带精度模板
        /// </summary>
        public const short ST_ModelBredeOutACC = 12;
        /// <summary>
        /// 上相机	料管精度模板
        /// </summary>
        public const short ST_ModelTubeInACC = 13;
        /// <summary>
        /// 上相机	飞达定位模板
        /// </summary>
        public const short ST_ModelBredeInPOS = 14;
        /// <summary>
        /// 上相机	编带定位模板
        /// </summary>
        public const short ST_ModelBredeOutPOS = 15;
        /// <summary>
        /// 上相机	料管定位模板
        /// </summary>
        public const short ST_ModelTubeInPOS = 16;

        /// <summary>
        /// 功能关闭
        /// </summary>
        public const short CLOSE = 0;
        /// <summary>
        /// 功能开启
        /// </summary>
        public const short OPEN = 1;

        public const   short   ST_XAXIS = 0;
        public  const   short   ST_YAXIS = 1;
        public  const   short   ST_Z1AXIS = 2;
        public  const   short   ST_Z2AXIS = 3;
        public  const   short   ST_Z3AXIS = 4;
        public  const   short   ST_Z4AXIS = 5;


        public  const   short   ST_DISCCDTOZ1 = 0;
        public  const   short   ST_DISCCDTOZ2 = 1;
        public  const   short   ST_DISCCDTOZ3 = 2;
        public  const   short   ST_DISCCDTOZ4 = 3;
        public  const   short   ST_DISZ1TOZ2 = 4;
        public  const   short   ST_DISZ2TOZ3 = 5;
        public  const   short   ST_DISZ3TOZ4 = 6;

        public  const   short   ST_PICKZ1   =   2;
        public  const   short   ST_PICKZ2   =   3;
        public  const   short   ST_PICKZ3   =   4;
        public  const   short   ST_PICKZ4   =   5;


        public  const   short   ST_LOG_PRINT    =   1;
        public  const   short   ST_LOG_RECORD   =   2;
        public  const   short   ST_LOG_PRINTANDRECORD    =   3;

        public  const   short   ST_MESEXIT_WITHOK = 0;
        public  const   short   ST_MESEXIT_WITHTOTAL = 1;

        public const short ST_MESTYPE_XWD = 1;
        public const short ST_MESTYPE_KJD = 2;
        public const short ST_MESTYPE_XC = 3;
        public const short ST_MESTYPE_RPC = 4;

        /// <summary>
        /// 远向采集模块
        /// </summary>
        public const short ST_Altimeter_YX = 1;
        /// <summary>
        /// 舟正采集模块 + HG_C1030
        /// </summary>
        public const short ST_Altimeter_ZZ_C1030 = 2;
        /// <summary>
        /// 内置通讯模块松下激光传感器
        /// </summary>
        public const short ST_Altimeter_SX = 3;
        /// <summary>
        /// 舟正采集模块 + HG_C1100
        /// </summary>
        public const short ST_Altimeter_ZZ_C1100 = 4;
        /// <summary>
        /// 英文
        /// </summary>
        public const string ST_English = "en-US";

    }
}
