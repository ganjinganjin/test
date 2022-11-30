using BAR.Commonlib;
using BAR.Commonlib.Connectors;
using BAR.Commonlib.Connectors.Protocols;
using BAR.Communicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BAR
{
    public class Download
    {
        public static BurnProxy proxy;
        private  Download_AK AK;
        private  Download_WG WG;
        private Download_DP DP;
        private Download_YED YED;
        private Download_RD RD;
        private Download_SFLY SFLY;
        private Download_XYDZ XYDZ;
        private Tester_SDSX SDSX;
        private static Download _instance = null;
        private static readonly object padlock = new object();

        private Download()
        {
            //非标准协议接口
            if ((UserTask.ProgrammerType == GlobConstData.Programmer_WG || UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY) && UserTask.Protocol_WG == GlobConstData.Protocol_WG_SDSX)
            {
                SDSX = new Tester_SDSX();
                return;
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_DP)
            {
                DP = new Download_DP();
                return;
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_SFLY)
            {
                SFLY = new Download_SFLY();
            }

            //标准协议接口
            proxy = BurnProxy.GetInstance();
            if (UserTask.ProgrammerType == GlobConstData.Programmer_AK)
            {
                AK = new Download_AK();
            }
            
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_WG || UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY)
            {
                WG = new Download_WG();
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_YED)
            {
                YED = new Download_YED();
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_RD)
            {
                RD = new Download_RD();
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_XYDZ)
            {
                XYDZ = new Download_XYDZ();
            }
        }

        public static Download GetInstance()
        {
            if (_instance == null)
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new Download();
                    }
                }
            }
            return _instance;
        }

        public static void Init()
        {
            if (UserTask.ProgrammerType == GlobConstData.Programmer_AK)
            {
                Download_AK.Init();
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_DP)
            {
                Download_DP.Init();
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_WG || UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY)
            {
                if (UserTask.Protocol_WG == GlobConstData.Protocol_WG_SDSX)
                {
                    Tester_SDSX.Init();
                }
                else
                {
                    Download_WG.Init();
                }
                
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_YED)
            {
                Download_YED.Init();
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_RD)
            {
                Download_RD.Init();
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_SFLY)
            {
                Download_SFLY.Init();
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_XYDZ)
            {
                Download_XYDZ.Init();
            }
        }

        public void Download_Program_Handle()
        {
            if (UserTask.ProgrammerType == GlobConstData.Programmer_AK)
            {
                AK.Program();
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_DP)
            {
                DP.Program();
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_WG || UserTask.ProgrammerType == GlobConstData.Programmer_WG_TY)
            {
                if (UserTask.Protocol_WG == GlobConstData.Protocol_WG_SDSX)
                {
                    SDSX.Program();
                }
                else
                {
                    WG.Program();
                }
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_YED)
            {
                YED.Program();
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_RD)
            {
                RD.Program();
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_SFLY)
            {
                SFLY.Program();
            }
            else if (UserTask.ProgrammerType == GlobConstData.Programmer_XYDZ)
            {
                XYDZ.Program();
            }
        }
    }
}
