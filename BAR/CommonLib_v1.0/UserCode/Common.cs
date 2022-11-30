using DllInterface;
using DllInterface.CodeFirst;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BAR
{
    public class Auto_Flag
    {
        /// <summary>
        /// 系统忙标志
        /// </summary>
        public static bool SystemBusy;          //系统忙标志
        /// <summary>
        /// 回原点忙标志
        /// </summary>
        public static bool HomeBusy;            //回原点忙标志
        /// <summary>
        /// GO位置忙标志
        /// </summary>
        public static bool GOBusy;              //GO位置忙标志
        /// <summary>
        /// 反学忙标志
        /// </summary>
        public static bool LearnBusy;           //反学忙标志
        /// <summary>
        /// 准备忙标志
        /// </summary>
        public static bool PrepareBusy;         //准备忙标志
        /// <summary>
        /// 自动运行忙标志
        /// </summary>
        public static bool AutoRunBusy;         //自动运行忙标志
        /// <summary>
        /// 远程启动运行标志
        /// </summary>
        public static bool RemoteStart;
        /// <summary>
        /// 运行到位标志
        /// </summary>
        public static bool Run_InPlace;         //运行到位标志
        /// <summary>
        /// 相机运行到位标志
        /// </summary>
        public static bool Camera_InPlace;
        /// <summary>
        /// 检查自动运行初始化标志
        /// </summary>
        public static bool CheckInitOK;         //检查自动运行初始化标志
        /// <summary>
        /// 安全打开标志
        /// </summary>
        public static bool Safety_Gate;         //安全打开标志
        /// <summary>
        /// 异常标志
        /// </summary>
        public static bool Exception;           //异常标志
        /// <summary>
        /// 报警标志
        /// </summary>
        public static bool ALarm;               //报警标志
        /// <summary>
        /// 编带报警标志
        /// </summary>
        public static bool BredeALarm;          //编带报警标志
        /// <summary>
        /// 报警暂停标志
        /// </summary>
        public static bool ALarmPause;          //报警暂停标志
        /// <summary>
        /// 运行暂停标志
        /// </summary>
        public static bool RunPause;            //运行暂停标志
        /// <summary>
        /// 下光源打开标志
        /// </summary>
        public static bool DownLightOn;         //下光源打开标志
        /// <summary>
        /// 重烧标志
        /// </summary>
        public static bool Burn_Again;          //重烧标志
        /// <summary>
        /// 烧录联机标志
        /// </summary>
        public static bool BurnOnline;          //烧录联机标志
        /// <summary>
        /// 烧录联机标志
        /// </summary>
        public static bool BurnReady;          //烧录就绪标志
        /// <summary>
        /// 编带联机标志
        /// </summary>
        public static bool BredeOnline;         //编带联机标志
        /// <summary>
        /// 自动盘联机标志
        /// </summary>
        public static bool AutoTrayOnline;      //自动盘联机标志
        /// <summary>
        /// 烧录座取料标志
        /// </summary>
        public static bool BurnSeat_TakeIC;     //烧录座取料标志
        /// <summary>
        /// 烧录座取料无状态IC标志
        /// </summary>
        public static bool BurnSeat_TakeNullIC; //烧录座取料无状态IC标志
        /// <summary>
        /// 管装全空标志
        /// </summary>
        public static bool Tube_AllEmptying;    //管装全空标志
        /// <summary>
        /// 管装取料超时标志
        /// </summary>
        public static bool TubeTimeOut;         //管装取料超时标志
        /// <summary>
        /// 更新料盘标志
        /// </summary>
        public static bool Update_Tray;         //更新料盘标志	
        /// <summary>
        /// 盘号改变标志
        /// </summary>
        public static bool TrayNum_Change;      //盘号改变标志
        /// <summary>
        /// 自动回原点结束标志
        /// </summary>
        public static bool RunHome_End;         //自动回原点结束标志
        /// <summary>
        /// 自动送盘结束标志
        /// </summary>
        public static bool AutoTray_End;      	//自动送盘结束标志
        /// <summary>
        /// 自动送盘就绪标志
        /// </summary>
        public static bool AutoTrayReady;      	//自动送盘就绪标志
        /// <summary>
        /// 反学就绪标志
        /// </summary>
        public static bool LearnReady;          //反学就绪标志
        /// <summary>
        /// 精度测试结束标志
        /// </summary>
        public static bool Precision_Test_End;  //精度测试结束标志	
        /// <summary>
        /// 排空标志
        /// </summary>
        public static bool Emptying;            //排空标志	
        /// <summary>
        /// 烧录后定位拍照
        /// </summary>
        public static bool Cam_2D_Mode_II;            //烧录后定位拍照
        /// <summary>
        /// 烧录后3D检测
        /// </summary>
        public static bool Cam_3D_Mode_II;            //烧录后3D检测
        /// <summary>
        /// 烧录座取料结束
        /// </summary>
        public static bool Seat_EndTakeIC;      //烧录座取料结束
        /// <summary>
        /// 烧录座放料结束
        /// </summary>
        public static bool Seat_EndLayIC;		//烧录座放料结束
        /// <summary>
        /// 烧录座取料循环
        /// </summary>
        public static bool Seat_TIC_Cycle;      //烧录座取料循环
        /// <summary>
        /// 获取水平位置
        /// </summary>
        public static bool Get_Horizontal;		//获取水平位置

        /// <summary>
        /// 固定盘取料标志
        /// </summary>
        public static bool FixedTray_TakeIC;    //固定盘取料标志
        /// <summary>
        /// 自动盘取料标志
        /// </summary>
        public static bool AutoTray_TakeIC;     //自动盘取料标志
        /// <summary>
        /// 编带取料标志
        /// </summary>
        public static bool Brede_TakeIC;        //编带取料标志
        /// <summary>
        /// 固定管装取料标志
        /// </summary>
        public static bool FixedTube_TakeIC;         //固定管装取料标志
        /// <summary>
        /// 自动管装取料标志
        /// </summary>
        public static bool AutoTube_TakeIC;         //自动管装取料标志

        /// <summary>
        /// 固定盘放料标志
        /// </summary>
        public static bool FixedTray_LayIC;     //固定盘放料标志
        /// <summary>
        /// 自动盘放料标志
        /// </summary>
        public static bool AutoTray_LayIC;      //自动盘放料标志
        /// <summary>
        /// 编带放料标志
        /// </summary>
        public static bool Brede_LayIC;         //编带放料标志
        /// <summary>
        /// 固定管装放料标志
        /// </summary>
        public static bool FixedTube_LayIC;          //固定管装放料标志

        /// <summary>
        /// 自动管装放料标志
        /// </summary>
        public static bool AutoTube_LayIC;          //自动管装放料标志

        /// <summary>
        /// 编带检测标志
        /// </summary>
        public static bool Brede_Check;         //编带检测标志
        /// <summary>
        /// 盖膜检测标志
        /// </summary>
        public static bool Velum_Check;         //盖膜检测标志
        /// <summary>
        /// 空料检测标志
        /// </summary>
        public static bool Empty_Check;
        /// <summary>
        /// 编带CCD检测标志
        /// </summary>
        public static bool BredeCCD_Check;
        /// <summary>
        /// 打点A标志
        /// </summary>
        public static bool DotA;                 //打点A标志
        /// <summary>
        /// 打点B标志
        /// </summary>
        public static bool DotB;                 //打点B标志
        /// <summary>
        /// 翻盖标志
        /// </summary>
        public static bool Flip;			    //翻盖标志
        /// <summary>
        /// 自动盘标志
        /// </summary>
        public static bool AutoTray;            //自动盘标志
        /// <summary>
        /// 调试模式标志
        /// </summary>
        public static bool DebugMode;           //调试模式标志
        /// <summary>
        /// 测试模式标志
        /// </summary>
        public static bool TestMode;			//测试模式标志
        /// <summary>
        /// 烧录模式标志
        /// </summary>
        public static bool BurnMode;			//烧录模式标志
        /// <summary>
        /// 暂停标志
        /// </summary>
        public static bool Pause;			    //暂停标志
        /// <summary>
        /// 下一步标志
        /// </summary>
        public static bool Next;			    //下一步标志
        /// <summary>
        /// 量产标志
        /// </summary>
        public static bool Production;			//量产标志  
        /// <summary>
        /// 量产OK数标志
        /// </summary>
        public static bool ProductionOK;	    //量产OK数标志
        /// <summary>
        /// 量产完成标志
        /// </summary>
        public static bool ProductionFinish;    //量产完成标志

        /// <summary>
        /// 旋转改变标志
        /// </summary>
        public static bool RotateChange;		//旋转改变标志
        /// <summary>
        /// 检测标志
        /// </summary>
        public static bool Detection;			//检测标志
        /// <summary>
        /// NG盘标志
        /// </summary>
        public static bool NGTray;			    //NG盘标志
        /// <summary>
        /// NG盘保持标志
        /// </summary>
        public static bool NGTrayKeep;			//NG盘保持标志
        /// <summary>
        /// 手动收尾标志
        /// </summary>
        public static bool ManualEnd;			//手动收尾标志
        /// <summary>
        /// 收尾标志
        /// </summary>
        public static bool Ending;			    //收尾标志
        /// <summary>
        /// 强制收尾标志
        /// </summary>
        public static bool ForceEnd;            //强制收尾标志
        /// <summary>
        /// 料盘取IC视觉定位使能
        /// </summary>
        public static bool Enabled_TakeICPos;   //料盘取IC视觉定位使能
        /// <summary>
        /// 料盘放IC视觉定位使能
        /// </summary>
        public static bool Enabled_LayICPos;    //料盘放IC视觉定位使能
        /// <summary>
        /// 叠料使能
        /// </summary>
        public static bool Enabled_Overlay;    //叠料使能
        /// <summary>
        /// 吸笔交替模式
        /// </summary>
        public static bool PenAltMode;
        /// <summary>
        /// 同步取放料使能
        /// </summary>
        public static bool Enabled_Sync;
        /// <summary>
        /// 吸笔异常
        /// </summary>
        public static bool PenAbnormal;
        /// <summary>
        /// NCC模板匹配使能
        /// </summary>
        public static bool Enabled_NCCModel;
        /// <summary>
        /// 自动校准坐标标志
        /// </summary>
        public static bool AutoRevisePos;
        /// <summary>
        /// 吸笔正序IC定位
        /// </summary>
        public static bool Ascending_ICPos;
        /// <summary>
        /// 烧录座同步取放料标志
        /// </summary>
        public static bool BurnSeat_Sync;
        /// <summary>
        /// 偏差过大标志
        /// </summary>
        public static bool OverDeviation;
        /// <summary>
        /// 速度模式
        /// </summary>
        public static int RunModel;        
        /// <summary>
        /// 吸笔交替标志
        /// </summary>
        public static bool PenAlt_Flag;
        /// <summary>
        /// 跳过编带步序标志
        /// </summary>
        public static bool JumpBredeStep_Flag;
        /// <summary>
        /// 跳过主逻辑步序标志
        /// </summary>
        public static bool JumpMainStep_Flag;
    }

    public class TrayEndFlag
    {
        /// <summary>
        /// 盘2烧录结束标志
        /// </summary>
        public static bool tray2Burn;                       //盘2烧录结束标志
        /// <summary>
        /// 盘取料结束标志
        /// </summary>
        public static bool[] takeIC = new bool[2];          //盘取料结束标志
        /// <summary>
        /// 盘取放料结束标志
        /// </summary>
        public static bool[] takeLayIC = new bool[2];       //盘取放料结束标志
        /// <summary>
        /// 盘放料结束标志
        /// </summary>
        public static bool[] layIC = new bool[3];           //盘放料结束标志
    }

    public class Button
    {
        /// <summary>
        /// 放回IC
        /// </summary>
        public static bool replaceIC;           //放回IC
        /// <summary>
        /// 旋转IC
        /// </summary>
        public static bool rotateIC;           //旋转IC
        /// <summary>
        /// 结束取IC
        /// </summary>
        public static bool endTakeIC;           //结束取IC
        /// <summary>
        /// 重新启动按钮
        /// </summary>
        public static bool Start_Restart;       //重新启动按钮
        /// <summary>
        /// 记忆启动按钮
        /// </summary>
        public static bool Start_memory;        //记忆启动按钮
        /// <summary>
        /// 继续按钮
        /// </summary>
        public static bool Resume;              //继续按钮
        /// <summary>
        /// 暂停按钮
        /// </summary>
        public static bool Pause;               //暂停按钮
        /// <summary>
        /// 自动盘初始化按钮
        /// </summary>
        public static bool AutoTray_Init;       //自动盘初始化按钮
    }

    public class TrayD
    {
        /// <summary>
        /// 列数
        /// </summary>
        public static int ColC;    				            //列数
        /// <summary>
        /// 行数
        /// </summary>
        public static int RowC;                             //行数
        /// <summary>
        /// 取料盘号
        /// </summary>
        public static int TIC_TrayN;    				    //取料盘号
        /// <summary>
        /// 放料盘号
        /// </summary>
        public static int LIC_TrayN;                        //放料盘号
        /// <summary>
        /// 料盘取料列计数
        /// </summary>
        public static int[] TIC_ColN = new int[2];          //料盘取料列计数
        /// <summary>
        /// 料盘取料行计数
        /// </summary>
        public static int[] TIC_RowN = new int[2];          //料盘取料行计数
        /// <summary>
        /// 料盘放料列计数
        /// </summary>
        public static int[] LIC_ColN = new int[3];          //料盘放料列计数
        /// <summary>
        /// 料盘放料行计数
        /// </summary>
        public static int[] LIC_RowN = new int[3];          //料盘放料行计数
        /// <summary>
        /// 料盘结束取料列号
        /// </summary>
        public static int[] TIC_EndColN = new int[2];       //料盘结束取料列号
        /// <summary>
        /// 料盘结束取料行号
        /// </summary>
        public static int[] TIC_EndRowN = new int[2];       //料盘结束取料行号

        /// <summary>
        /// 托盘列向间距
        /// </summary>
        public static double Col_Space;                      //托盘列向间距
        /// <summary>
        /// 托盘行向间距
        /// </summary>
        public static double Row_Space;                      //托盘行向间距
    }

    public class Position
    {
        /// <summary>
        /// 料盘第一个料坐标
        /// </summary>
        public double[] trayFirst = new double[3];     //料盘第一个料坐标
        /// <summary>
        /// 料盘最后一个料坐标
        /// </summary>
        public double[] trayEnd = new double[3];  	   //料盘最后一个料坐标
        //public double bredeIn;     	                   //编带进坐标
        /// <summary>
        /// 飞达坐标
        /// </summary>
        public double[] Feeder = new double[2];        //飞达坐标
        /// <summary>
        /// 编带出坐标
        /// </summary>
        public double bredeOut;    	                   //编带出坐标
        /// <summary>
        /// NG杯坐标
        /// </summary>
        public double NGCup;       	                   //NG杯坐标
        /// <summary>
        /// 3D检测坐标
        /// </summary>
        public double detection;       	               //3D检测坐标
        /// <summary>
        /// 精度测试
        /// </summary>
        public double Precision_Test;                  //精度测试 
        /// <summary>
        /// 管装进坐标
        /// </summary>
        public double tubeIn;          	               //管装进坐标
        /// <summary>
        /// 料管间距
        /// </summary>
        public double tubespace;	                   //料管间距	
        /// <summary>
        /// 编带间距坐标
        /// </summary>
        public double bredespace;                      //编带间距坐标 
        /// <summary>
        /// 上相机对位坐标
        /// </summary>
        public double topCamera; 		               //上相机对位坐标
        /// <summary>
        /// 轴坐标缓存
        /// </summary>
        public double Buffer;   			           //轴坐标缓存
        /// <summary>
        /// 最大值
        /// </summary>
        public double MaxVal;                          //最大值
        /// <summary>
        /// 最小值
        /// </summary>
        public double MinVal;                          //最小值
    }

    public class HeightVal
    {
        public static double Tray_TakeIC;      //托盘取料高度
        public static double Tray_LayIC;       //托盘放料高度
        public static double DownSeat_TakeIC;  //烧录座取料高度
        public static double DownSeat_LayIC;   //烧录座放料高度
        public static double Brede_TakeIC;     //编带取料高度
        public static double Brede_LayIC;      //编带放料高度
        public static double Tube_TakeIC;      //管装取料高度
        public static double Detection;        //3D检测高度
        public static double Precision_Test;   //精度测试
        public static double Safe;             //Z轴安全高度
        public static double Buffer;           //高度缓存
    }

    public class HeightVal_Altimeter
    {
        /// <summary>
        /// 托盘IC与测高仪高度
        /// </summary>
        public static double Tray;
        /// <summary>
        /// 编进IC与测高仪高度
        /// </summary>
        public static double BredeIn;
        /// <summary>
        /// 编出IC与测高仪高度
        /// </summary>
        public static double BredeOut;
        /// <summary>
        /// 管进IC与测高仪高度
        /// </summary>
        public static double TubeIn;
        /// <summary>
        /// 高度缓存
        /// </summary>
        public static double Buffer;
    }

    public class RotateAngle
    {
        /// <summary>
        /// 烧录座基准角度
        /// </summary>
        public static double Base_Socket;
        public static double TIC_Brede;      	              //编带取料旋转角度
        public static double LIC_Brede;      	              //编带放料旋转角度
        public static double TIC_Tube;      	              //管装取料旋转角度
        public static double LIC_Tube;      	              //管装放料旋转角度
        public static double[] TIC_Tray = new double[2];      //料盘取料旋转角度
        public static double[] LIC_Tray = new double[3];      //料盘放料旋转角度
    }

    //时间
    public class AutoTiming
    {
        public static UInt32 DownDelay;         //延迟烧录
        public static UInt32 VacuumDuration;    //真空保持（取料）
        public static UInt32 BlowDelay;         //延迟吹气
        public static UInt32 BlowDuration;      //吹气保持
        public static UInt32 BuzzerDuration;    //蜂鸣器保持
        public static UInt32 DownTimeOut;       //烧录超时
        public static UInt32 ScanTimeOut;       //扫码超时
        public static UInt32 SeatTakeDelay;     //烧录座取料延迟
        public static UInt32 BredeTakeDelay;    //飞达取料延迟
        public static UInt32 TubeTakeDelay;     //料管取料延迟
        public static UInt32 TubeTimeOut;       //料管超时
    }

    //定时器
    public class AutoTimer
    {
        public static UInt64 DownDelay;         //延迟烧录
        public static UInt64 VacuumDuration;    //真空保持（取料）
        public static UInt64 BlowDelay;         //延迟吹气
        public static UInt64 BlowDuration;      //吹气保持
        public static UInt64 BuzzerDuration;    //蜂鸣器保持
        public static UInt64 DownTimeOut;       //烧录超时
        public static UInt64 TubeTimeOut;       //料管超时
        public static UInt64 SeatTakeDelay;     //烧录座取料延迟
        public static UInt64 BredeTakeDelay;    //飞达取料延迟
        public static UInt64 OpenSocket;        //打开烧录座
        public static UInt64 OriginZ_Check;     //原点信号查询
        public static UInt64 LostIC;            //掉料
        /// <summary>
        /// 编带CCD触发延迟关闭
        /// </summary>
        public static UInt64 BredeCCDOffDelay;
    }

    public class CCD
    {
        /// <summary>
        /// 拍照图像
        /// </summary>
        public static HObject[] Image = new HObject[4];
        /// <summary>
        /// 图像Xld
        /// </summary>
        public static HObject[] ImageXld = new HObject[4];
        /// <summary>
        /// X偏移
        /// </summary>
        public static double[] Offset_X = new double[4];
        /// <summary>
        /// Y偏移
        /// </summary>
        public static double[] Offset_Y = new double[4];
        /// <summary>
        /// R偏移
        /// </summary>
        public static double[] Offset_R = new double[4];
    }

    public struct SocketUnit_Struct
    {
        /// <summary>
        /// 首次使用标志
        /// </summary>
        public bool Flag_First;				  //首次使用标志
        /// <summary>
        /// 打开标志
        /// </summary>
        public bool Flag_Open;				  //打开标志
        /// <summary>
        /// 取料标志
        /// </summary>
        public bool Flag_TakeIC;			  //取料标志
        /// <summary>
        /// 放料标志
        /// </summary>
        public bool Flag_LayIC;				  //放料标志
        /// <summary>
        /// 新料标志
        /// </summary>
        public bool Flag_NewIC;				  //新料标志
        /// <summary>
        /// 烧录错误停机标志
        /// </summary>
        public bool Flag_Error;

        /// <summary>
        /// 烧录结果
        /// </summary>
        public int DownResult;				  //烧录结果
        /// <summary>
        /// 重烧计数器
        /// </summary>
        public int Counter_Burn;			  //重烧计数器
        /// <summary>
        /// 连续NG计数器
        /// </summary>
        public int Counter_NG;			      //连续NG计数器
        /// <summary>
        /// 连续NG关闭烧录座计数器
        /// </summary>
        public int NGCounter_Shut;
        /// <summary>
		/// OK总数
		/// </summary>
		public int OKAllC;
        /// <summary>
        /// NG总数
        /// </summary>
        public  int NGAllC;
        /// <summary>
        /// NG总数关闭烧录座计数器
        /// </summary>
        public int NGAllC_Shut;
        /// <summary>
        /// 上相机X轴
        /// </summary>
        public double TopCamera_X;            //上相机X轴
        /// <summary>
        /// 上相机Y轴
        /// </summary>
        public double TopCamera_Y;            //上相机Y轴
        /// <summary>
        /// 烧录座与测高仪高度
        /// </summary>
        public double HeightVal;
    }

    public struct Down_Struct
    {
        /// <summary>
        /// 烧录触发标志
        /// </summary>
        public bool Trigger;             //烧录触发标志
        /// <summary>
        /// 烧录忙标志
        /// </summary>
        public bool Busy;                //烧录忙标志  
    }

    public struct Scan_Struct
    {
        /// <summary>
        /// 扫码触发标志
        /// </summary>
        public bool Trigger;             //扫码触发标志
        /// <summary>
        /// 扫码忙标志
        /// </summary>
        public bool Busy;                //扫码忙标志  
    }

    public class SocketGroup
    {
        /// <summary>
        /// 待取标志
        /// </summary>
        public bool Waiting_To_Take;		  //待取标志
        /// <summary>
        /// 待放标志
        /// </summary>
        public bool Waiting_To_Lay;		  //待放标志
        /// <summary>
        /// 待烧标志
        /// </summary>
        public bool Waiting_To_Burn;		  //待烧标志
        /// <summary>
        /// 连续NG标志
        /// </summary>
        public bool Continue_NG;              //连续NG标志
        /// <summary>
        /// 烧录错误停机标志
        /// </summary>
        public bool Flag_Error;

        /// <summary>
        /// 放料座子个数
        /// </summary>
        public int LIC_UnitC;			      //放料座子个数
        /// <summary>
        /// 组号
        /// </summary>
        public int GroupNum;

        public Down_Struct Down;
        public Scan_Struct Scan;
        public SocketUnit_Struct[] Unit = new SocketUnit_Struct[16];        
    }

    public class Mes
    {
        public static FunctionDll _dll_KJD = new FunctionDll();
        public static ProductReCord record_KJD = new ProductReCord();
        public static List<FileInfo> lstFiles;

        public static int Type;//对接的客户
        public static string IP;//欣旺达IP地址
        public static string IP_XC;//协创IP地址
        public static string Port;//端口号
        public static string ProgFilePath;//烧录文档目录
        public static string ProgFileName;//烧录文档路径
        public static string ProgFileFirstName;//烧录文档名
        public static string FuncMode;//烧录模式
        public static string FuncModeIndex;//烧录模式索引
        public static bool IsRun;//MES系统运行状态
        public static int Exit;//退出条件
        public static string ComputerName;//电脑名称

        /// <summary>
        /// 客户
        /// </summary>
        public static string Customer;//客户
        /// <summary>
        /// 设备编号
        /// </summary>
        public static string DeviceSN;//设备编号
        /// <summary>
        /// 设备描述
        /// </summary>
        public static string DeviceDes;//设备描述
        /// <summary>
        /// IC厂牌
        /// </summary>
        public static string Brand = "NonSupport";//IC厂牌
        /// <summary>
        /// 芯片型号
        /// </summary>
        public static string Device = "NonSupport";//芯片型号
        public static string BuildVersion;//驱动程序版本
        public static string Version;//软件版本
        public static string Version_File;//烧录文档版本号
        /// <summary>
        /// 操作人员ID
        /// </summary>
        public static string OperatorID;
        /// <summary>
        /// 工单、SN码
        /// </summary>
        public static string LotSN;
        /// <summary>
        /// 物料名稱（配方名）
        /// </summary>
        public static string ChipName;
        /// <summary>
        /// 料号
        /// </summary>
        public static string ItemCode;
        /// <summary>
        /// 工单批次数量
        /// </summary>
        public static int Count;
        /// <summary>
        /// 工单取料完成数
        /// </summary>
        public static int TIC_DoneC;
        /// <summary>
        /// 工单OK料完成数
        /// </summary>
        public static int OKDoneC;
        public static string Checksum_Mes;//数据库校验值
        /// <summary>
        /// 烧录文件校验值
        /// </summary>
        public static string Checksum_File = "NonSupport";//烧录文件校验值
        /// <summary>
        /// 芯片校验值
        /// </summary>
        public static string Checksum_Chip = "NonSupport";
        public static string MD5;
        public static string DataValue_XC;//协创提交信息包
        public static string DatabasePath_XC;//协创共享数据库路径
        public static string LogPath_XC;//协创共享昂科烧录Log路径
        public static string ProgFilePath_XC;//协创烧录文档目录
    }

    public class BurnInfo
    {
        public static string SiteSN;//当前产量发生变化的烧录器站点序列号
        public static string SiteAlias;//该站点别名
        public static string Total;//从批量开始到现在该站点的总产出个数
        public static string Fail;//从批量开始到现在该站点的失败总个数
        public static string Pass;//从批量开始到现在该站点的成功总个数
        public static string CurTotal;//本次烧录该站点总共烧录的多少颗芯片
        public static string CurFail;//本次烧录该站点烧录的芯片中有几个 Fail 的
        public static string CurPass;//本次烧录该站点烧录的芯片中有几个 Pass 的
        public static FailReason_Struct[] FailReason = new FailReason_Struct[8];//本次烧录失败的原因，看 FailReason 说明
        public static Group_class[] Group = new Group_class[6];

        public static string AllFail;//从批量开始到现在失败总个数
        public static string AllPass;//从批量开始到现在成功总个数
    }

    /// <summary>
    /// 外挂烧录参数
    /// </summary>
    public class BurnPara
    {
        public static string[] Name = new string[10];
    }

    /// <summary>
    /// 协创MES信息
    /// </summary>
    public class MesInfo_XC
    {
        /// <summary>
        /// 烧录机型数量
        /// </summary>
        public static int Count_Model;
        
        public static Model_class[] Model = new Model_class[100];
    }

    /// <summary>
    /// 协创烧录机型节点
    /// </summary>
    public class Model_class
    {
        /// <summary>
        /// 烧录机型
        /// </summary>
        public string Model;
        /// <summary>
        /// 烧录版本数量
        /// </summary>
        public int Count_Version;
        public Version_Struct[] Version = new Version_Struct[100];
    }

    /// <summary>
    /// 协创烧录版本节点
    /// </summary>
    public struct Version_Struct
    {
        /// <summary>
        /// 协创烧录版本
        /// </summary>
        public string Version;
        /// <summary>
        /// 协创服务器校验值
        /// </summary>
        public string Checksum;
    }


    public class Vision_3D
    {
        public static bool Function;//功能开关
        public static bool Enabled_I;//使能开关1
        public static bool Enabled_II;//使能开关2
        public static int ICType;//封装类型
        public static string IP;//IP地址
        public static int Port;//端口号
        public static double X;
        public static double Y;
        public static double Z;
    }

    public class Inks
    {
        public static bool Function;//油墨功能开关
        public static bool Enabled_AutoTray;//自动盘油墨使能
        public static bool Enabled_Braid;//编带油墨使能
        public static double DotCount_AutoTray;//自动盘油墨打点数
        public static double TimeCount_AutoTray;//自动盘油墨时长
        public static DateTime DateTime_AutoTray;//自动盘换油墨时间
        public static TimeSpan TimeSpan_AutoTray;//自动盘已用油墨时间
        public static double DotCount_Braid;//编带油墨打点数
        public static double TimeCount_Braid;//编带油墨时长
        public static DateTime DateTime_Braid;//编带换油墨时间
        public static TimeSpan TimeSpan_Braid;//编带已用油墨时间
    }

    public class GlobalParam
    {
        /// <summary>
        /// 服务器IP（瑞德）
        /// </summary>
        public static string ServerIP;
        /// <summary>
        /// 服务器端口（瑞德）
        /// </summary>
        public static int ServerPort;
        /// <summary>
        /// RPC服务器IP
        /// </summary>
        public static string RPCServerIP;
        /// <summary>
        /// RPC服务器端口
        /// </summary>
        public static int RPCServerPort;
    }

    /// <summary>
    /// 方块信息
    /// </summary>
    public class RectangleInfo
    {
        /// <summary>
        /// 行
        /// </summary>
        public int Row;
        /// <summary>
        /// 列
        /// </summary>
        public int Col;
    }

    public class MarkPos_class
    {
        public Tray_Struct[] Tray = new Tray_Struct[4];
        public Correction_Struct[] Correction = new Correction_Struct[4];
    }

    /// <summary>
    /// 运行模式参数
    /// </summary>
    public class RunModel_class
    {
        public double SpeedX;
        public double SpeedY;
        public double SpeedZ;
        public double SpeedC;
        public double LongAcceSpeedX;
        public double LongAcceSpeedY;
        public double LongDecSpeedX;
        public double LongDecSpeedY;
        public double ShortAcceSpeedX;
        public double ShortAcceSpeedY;
        public double ShortDecSpeedX;
        public double ShortDecSpeedY;
        public double AcceSpeedZ;
        public double DecSpeedZ;
        /// <summary>
        /// 真空保持
        /// </summary>
        public UInt32 VacuumDuration;    //真空保持（取料）
        /// <summary>
        /// 延迟吹气
        /// </summary>
        public UInt32 BlowDelay;         //延迟吹气
        /// <summary>
        /// 吹气保持
        /// </summary>
        public UInt32 BlowDuration;      //吹气保持
    }

    public struct Tray_Struct
    {
        public double X;
        public double Y;
    }

    public struct Correction_Struct
    {
        public double X;
        public double Y;
    }

    public class MarkOffset
    {
        public static double Tray_X;
        public static double Tray_Y;
        public static int Tray_Col;
        public static int Tray_Rol;
        public static double Tray_Coldis;
        public static double Tray_Roldis;
    }


    public struct FailReason_Struct
    {
        public string SKTIDX;
        public string ErrMsg;
    }

    public class Group_class
    {
        public Unit_Struct[] unit = new Unit_Struct[8];
    }

    public struct Unit_Struct
    {
        public string ID;
        public string Status;
    }

    public struct Rotate_Struct
    {
        /// <summary>
        /// 旋转中标志
        /// </summary>
	    public bool Busy;           //旋转中标志
        public bool mode;
        public double AngleVal;     //角度旋转
        public UInt64 TimeOut;      //旋转超时
    }

    public struct VacuumPen_Struct        //吸笔
    {
        /// <summary>
        /// 使能标志
        /// </summary>
        public bool Enable;			      //使能标志
        /// <summary>
        /// 有料标志
        /// </summary>
        public bool ExistIC;			  //有料标志
        /// <summary>
        /// 有料标志
        /// </summary>
        public bool ExistRawIC;			  //有原始料标志
        /// <summary>
        /// 触发烧录标志
        /// </summary>
        public bool DownTrigger;          //触发烧录标志
        /// <summary>
        /// 图像匹配忙标志
        /// </summary>
        public bool Image_Busy;           //图像匹配忙标志

        /// <summary>
        /// 图像匹配结果
        /// </summary>
        public bool ImageResult;


        /// <summary>
        /// 烧录结果
        /// </summary>
        public int DownResult;			  //烧录结果
        /// <summary>
        /// 3D检测结果
        /// </summary>
        public int DetectionResult;	      //3D检测结果

        /// <summary>
        /// 检测次数
        /// </summary>
        public int Detection_Num;		  //检测次数
        /// <summary>
        /// 重烧计数器
        /// </summary>
        public int Counter_Burn;		  //重烧计数器


        /// <summary>
        /// 与上相机偏移值X
        /// </summary>
        public double Offset_TopCamera_X;  //与上相机偏移值X
        /// <summary>
        /// 与上相机偏移值Y
        /// </summary>
        public double Offset_TopCamera_Y;  //与上相机偏移值Y
        /// <summary>
        /// 下相机X
        /// </summary>
        public double LowCamera_X;         //下相机X
        /// <summary>
        /// 下相机Y
        /// </summary>
        public double LowCamera_Y;         //下相机Y
        /// <summary>
        /// 与校准点高度
        /// </summary>
        public double Calibrating_Z;
        /// <summary>
        /// 与测高仪高度差Z
        /// </summary>
        public double Altimeter_Z;

        public Rotate_Struct Rotate;

        /// <summary>
        /// 与载体取放料位置偏移值X
        /// </summary>
        public double Offset_Carrier_X;
        /// <summary>
        /// 与载体取放料位置偏移值Y
        /// </summary>
        public double Offset_Carrier_Y;
        /// <summary>
        /// 取放料位置偏移值X
        /// </summary>
        public double Offset_Base_X;
        /// <summary>
        /// 取放料位置偏移值Y
        /// </summary>
        public double Offset_Base_Y;
    }

    public struct SyncPen_Struct           //同步吸笔
    {
        /// <summary>
        /// 测高仪排空状态
        /// </summary>
        public static bool State_Emptying;
        /// <summary>
        /// 取料状态
        /// </summary>
        public static bool State_TakeIC;
        /// <summary>
        /// 放料状态
        /// </summary>
        public static bool State_LayIC;
        /// <summary>
        /// 烧录座取料循环
        /// </summary>
        public static bool Seat_TIC_Cycle;
        /// 偏差过大标志
        /// </summary>
        public bool OverDeviation;
        /// <summary>
        /// 首次使用标志
        /// </summary>
        public bool Flag_First;
        /// <summary>
        /// 取料标志
        /// </summary>
        public bool Flag_TakeIC;
        /// <summary>
        /// 放料标志
        /// </summary>
        public bool Flag_LayIC;
        /// <summary>
        /// 重烧标志
        /// </summary>
        public bool Burn_Again;
        /// <summary>
		/// 烧录座单元号
		/// </summary>
		public int UnitN;
		/// <summary>
		/// 烧录座组号
		/// </summary>
		public int GroupN;
        /// <summary>
		/// 吸笔号
		/// </summary>
		public int PenN;
        /// <summary>
        /// 高度值
        /// </summary>
        public double HeightVal;
        /// <summary>
        /// 步骤
        /// </summary>
        public int Step;
        /// <summary>
        /// 物料丢失时长
        /// </summary>
        public UInt64 TimeLostIC;
        /// <summary>
        /// 超时计数
        /// </summary>
        public UInt64 TimeOut;
        /// <summary>
        /// 真空保持（取料）
        /// </summary>
        public UInt64 VacuumDuration;
        /// <summary>
        /// 延迟吹气
        /// </summary>
        public UInt64 BlowDelay;
        /// <summary>
        /// 吹气保持
        /// </summary>
        public UInt64 BlowDuration;
    }

    public struct Feeder_Struct           //飞达
    {
        /// <summary>
        /// 飞达使能标志
        /// </summary>
        public bool Enable;			      //使能标志
        /// <summary>
        /// 飞达触发标志
        /// </summary>
        public bool Trigger;              //触发烧录标志
        public UInt64 FeederOffDelay;    //飞达延迟关闭
        public UInt64 FeederTakeDelay;    //飞达取料延迟
    }

    public struct Scanner_Struct        //扫描器
    {
        /// <summary>
        /// 与上相机偏移值X
        /// </summary>
        public double Offset_TopCamera_X;  //与上相机偏移值X
        /// <summary>
        /// 与上相机偏移值Y
        /// </summary>
        public double Offset_TopCamera_Y;  //与上相机偏移值Y
        /// <summary>
        /// 校准X
        /// </summary>
        public double Calibrating_X;       //校准X
        /// <summary>
        /// 校准Y
        /// </summary>
        public double Calibrating_Y;       //校准Y
    }

    public struct Altimeter_Struct        //测高仪
    {
        /// <summary>
        /// 与上相机偏移值X
        /// </summary>
        public double Offset_TopCamera_X;  //与上相机偏移值X
        /// <summary>
        /// 与上相机偏移值Y
        /// </summary>
        public double Offset_TopCamera_Y;  //与上相机偏移值Y
        /// <summary>
        /// 与烧录座偏移值X
        /// </summary>
        public double Offset_Socket_X;
        /// <summary>
        /// 与烧录座偏移值Y
        /// </summary>
        public double Offset_Socket_Y;
        /// <summary>
        /// 校准孔X
        /// </summary>
        public double Calibrating_X;
        /// <summary>
        /// 校准孔Y
        /// </summary>
        public double Calibrating_Y;
        /// <summary>
        /// 校准孔Z
        /// </summary>
        public double Calibrating_Z;
        /// <summary>
        /// 无IC高度值
        /// </summary>
        public double ValueEmptyIC;
        /// <summary>
        /// 有IC高度值
        /// </summary>
        public double ValueExistIC;
        /// <summary>
        /// IC厚度
        /// </summary>
        public double Thickness;
        /// <summary>
        /// 取放料高度差
        /// </summary>
        public double HeightDifference;
        /// <summary>
        /// 读测高仪模拟量忙
        /// </summary>
        public bool ReadAI_Busy;
        /// <summary>
        /// 读测高仪模拟量成功
        /// </summary>
        public bool ReadAI_Online;
    }

    /// <summary>
    /// 变焦镜头参数
    /// </summary>
    public struct ZoomLens_Struct        
    {
        /// <summary>
        /// 烧录座焦距
        /// </summary>
        public double Socket;
        /// <summary>
        /// 料盘焦距
        /// </summary>
        public double Tray;
        /// <summary>
        /// 飞达焦距
        /// </summary>
        public double BredeIn;
        /// <summary>
        /// 编带焦距
        /// </summary>
        public double BredeOut;
        /// <summary>
        /// 料管焦距
        /// </summary>
        public double TubeIn;
        /// <summary>
        /// 读变焦镜头忙
        /// </summary>
        public bool ReadStatus_Busy;
        /// <summary>
        /// 读变焦镜头成功
        /// </summary>
        public bool ReadStatus_Online;
        /// <summary>
        /// 焦镜头零位
        /// </summary>
        public bool Home;
        /// <summary>
        /// 定位完成
        /// </summary>
        public bool IsDone;
        /// <summary>
        /// 忙信号
        /// </summary>
        public bool IsBusy;
        /// <summary>
        /// GO超时时间
        /// </summary>
        public UInt64 GODelay;

        internal void MODBUS_DST()
        {
            throw new NotImplementedException();
        }
    }

    public struct PenRectify
    {
        /// <summary>
        /// X纠偏
        /// </summary>
        public double AxisX;       //X纠偏
        /// <summary>
        /// Y纠偏
        /// </summary>
        public double AxisY;       //Y纠偏
    };

    public enum RUNSTEP
    {
        /// <summary>
        /// 待机
        /// </summary>
        IDLE = 0,              	//待机
        /// <summary>
        /// 开始
        /// </summary>
        START,					//开始
        /// <summary>
        /// 获取水平位置前
        /// </summary>
        BEFORE_HP_GET,        	//获取水平位置前
        /// <summary>
        /// 获取水平位置
        /// </summary>
        NOW_HP_GET,           	//获取水平位置
        /// <summary>
        /// 料管等待
        /// </summary>
        TUBE_WAIT,           	//料管等待
        /// <summary>
        /// 获取水平位置后
        /// </summary>
        AFTER_HP_GET,        	//获取水平位置后
        /// <summary>
        /// 获取垂直位置前
        /// </summary>
        BEFORE_VP_GET,        	//获取垂直位置前
        /// <summary>
        /// 获取垂直位置
        /// </summary>
        NOW_VP_GET,           	//获取垂直位置
        /// <summary>
        /// 获取垂直位置后
        /// </summary>
        AFTER_VP_GET,        	//获取垂直位置后
        /// <summary>
        /// 垂直移动
        /// </summary>
        MOVE_VP,        		//垂直移动
        /// <summary>
        /// 取放料前
        /// </summary>
        BEFORE_TLIC,            //取放料前
        /// <summary>
        /// 取料
        /// </summary>
        NOW_TIC,           		//取料
        /// <summary>
        /// 放料
        /// </summary>
        NOW_LIC,           		//放料
        /// <summary>
        /// 取放料后
        /// </summary>
        AFTER_TLIC,        		//取放料后
        /// <summary>
        /// 安全高度
        /// </summary>
        HEIGHT_SAFE,			//安全高度
        /// <summary>
        /// 数据运算工作模式
        /// </summary>
        DATA_WORK,				//数据运算工作模式
        /// <summary>
        /// 报警取料失败
        /// </summary>
        ALARM_TIC_FAILURE,		//报警取料失败
        /// <summary>
        /// 数据运算调试模式
        /// </summary>
        DATA_DEBUG,				//数据运算调试模式
        /// <summary>
        /// 报警动作
        /// </summary>
        ALARM_ACTION,			//报警动作
        /// <summary>
        /// 报警等待
        /// </summary>
        ALARM_WAIT,				//报警等待
        /// <summary>
        /// 烧录座取放料前
        /// </summary>
        BEFORE_SEAT_TLIC,       //烧录座取放料前
        /// <summary>
        /// 主控逻辑
        /// </summary>
        MASTERLOGIC,			//主控逻辑
        /// <summary>
        /// 编带等待
        /// </summary>
        BREDE_WAIT,				//编带等待
        /// <summary>
        /// 编带查询
        /// </summary>
        BREDE_CHECK,			//编带查询
        /// <summary>
        /// 编带报警
        /// </summary>
        ALARM_BREDE,			//编带报警
        /// <summary>
        /// 结束
        /// </summary>
        END,					//结束
    }

    public enum RUNSTATE
    {
        /// <summary>
        /// 载体取料
        /// </summary>
        Carrier_TakeIC,		//载体取料
        /// <summary>
        /// 载体放OK料
        /// </summary>
        Carrier_LayOKIC,	//载体放OK料
        /// <summary>
        /// 载体放NG料
        /// </summary>
        Carrier_LayNGIC,	//载体放NG料
        /// <summary>
        /// 烧录座取料
        /// </summary>
	    BurnSeat_TakeIC,	//烧录座取料
        /// <summary>
        /// 排空
        /// </summary>
        Emptying,           //排空
        /// <summary>
        /// 烧录座放料
        /// </summary>
        BurnSeat_LayIC,		//烧录座放料
        /// <summary>
        /// 重烧
        /// </summary>
	    Burn_Again,			//重烧
        /// <summary>
        /// 拍照
        /// </summary>
	    Photograph,			//拍照
        /// <summary>
        /// 检测
        /// </summary>
	    Detection,		    //检测
        /// <summary>
        /// 扫码
        /// </summary>
        ScanCode,           //扫码
        /// <summary>
        /// 烧录座取扫码NG料
        /// </summary>
        TakeSNGIC,	        //烧录座取扫码NG料
        /// <summary>
        /// 载体放检测NG料
        /// </summary>
        Carrier_LayDNGIC,	//载体放检测NG料
        /// <summary>
        /// 载体放未烧录料
        /// </summary>
	    Carrier_LayIC,		//载体放未烧录料
        /// <summary>
        /// 结束
        /// </summary>
	    End,				//结束
    }

}

