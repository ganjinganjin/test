using BAR.Commonlib.Connectors;
using CCWin.SkinClass;
using Packet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BAR.Commonlib.Connectors
{
    /// <summary>
    /// 凤凰平台接口
    /// </summary>
    public class PhenixIOS
    {
        private static PhenixIOS _instance = null;
        private static readonly object padlock = new object();

        public Config g_config = Config.GetInstance();
        public Act g_act = Act.GetInstance();
        public DP1KAPIENTRANCE.API _api;

        public static PhenixIOS GetInstance()
        {
            if (_instance == null)
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new PhenixIOS();

                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// 注册
        /// </summary>
        public void Register()
        {
            _api = new DP1KAPIENTRANCE.API();
            _api.eRequest_SetWorkInfo += _api_eRequest_SetWorkInfo;
            _api.eRequest_AutorunStart += _api_eRequest_AutorunStart;
            _api.eRequest_AutorunStop += _api_eRequest_AutorunStop;
        }

        /// <summary>
        /// 连接
        /// </summary>
        public void Connect()
        {
            _api.Connect();
            if (_api.IsConnected)
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD,"成功连接到凤凰平台服务器", "Link_Phenixl");
            }
            else
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "连接凤凰平台服务器失败!", "Link_Phenixl");
            }
        }

        public void QueryJob()
        {
            _api.QueryJob();
        }

        private void _api_eRequest_SetWorkInfo(object sender, PacketDef.WorkInfo e)
        {
            if (Auto_Flag.AutoRunBusy)
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "设备运行中,不允许修改", "Link_Phenixl");
                return;
            }
            Auto_Flag.RemoteStart = false;
            string[] strArray = e.ToString().Split(new char[2] { '=', '\n' });
            for (int i = 0; i < strArray.Length; i++)
            {
                strArray[i] = strArray[i].Replace(" ", "");
            }
            Mes.LotSN = strArray[1];
            Mes.ChipName = strArray[3];
            if (!g_config.LoadConfigProd(Mes.ChipName))//加载配方
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "配方[" + Mes.ChipName + "]不存在,加载失败", "Link_Phenixl");
                return;
            }
            UserEvent evt = new UserEvent();
            evt.ProductChange_Click();
            evt.RenovateBtn_Click();
            TrayState.TrayStateUpdate(true);
            //g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "配方：【" + Mes.ChipName + "】加载成功", "Link_FK");

            if (string.IsNullOrEmpty(Mes.LotSN))//工单号为空
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "工单号为空", "Link_Phenixl");
                return;
            }

            if (g_act.ReadLotInfo())//存在相同工单
            {
                if ((Mes.Exit == 1 && Mes.OKDoneC >= Mes.Count) || (Mes.Exit == 2 && Mes.TIC_DoneC >= Mes.Count))
                {
                    g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "工单[" + Mes.LotSN + "]已完成,请添加新工单", "Link_Phenixl");
                    return;
                }
                //赋值
                UserTask.TargetC = Mes.Count;
                UserTask.TIC_DoneC = Mes.TIC_DoneC;
                UserTask.OKDoneC = Mes.OKDoneC;
                if (Mes.Exit == 0)
                {
                    Auto_Flag.Production = false;
                }
                else
                {
                    Auto_Flag.Production = true;
                    Auto_Flag.ProductionOK = Mes.Exit == 1 ? true : false;
                }
            }
            else//无工单记录
            {
                Auto_Flag.Production = true;
                Auto_Flag.ProductionOK = true;
                Mes.Count = UserTask.TargetC = strArray[9].Replace(" ", "").ToInt32();
                Mes.TIC_DoneC = UserTask.TIC_DoneC = 0;
                Mes.OKDoneC = UserTask.OKDoneC = 0;
            }
            Auto_Flag.RemoteStart = true;
            g_act.WriteLotInfo();
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "工单准备完成", "Link_Phenixl");
        }
        
        /// <summary>
        /// 停止运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _api_eRequest_AutorunStop(object sender, PacketDef.AutorunStop e)
        {
            if (!Auto_Flag.AutoRunBusy)
            {
                return;
            }
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "停止运行", "Link_Phenixl");
            Auto_Flag.Next = true;
            Auto_Flag.ManualEnd = true;
            Auto_Flag.Pause = false;
            Auto_Flag.Ending = true;
            Auto_Flag.ForceEnd = true;
            UserEvent evt = new UserEvent();
            evt.StaticalWndUI_Click();
        }

        /// <summary>
        /// 启动运行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _api_eRequest_AutorunStart(object sender, PacketDef.AutorunStart e)
        {
            if (Auto_Flag.AutoRunBusy)
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "机台已在运作中", "Link_Phenixl");
                return;
            }
            if (!Auto_Flag.RemoteStart)
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "工单导入失败,无法启动", "Link_Phenixl");
                return;
            }
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "启动运行", "Link_Phenixl");
            UserEvent evt = new UserEvent();
            evt.BtnAutoRun_Click();
        }
    }
}
