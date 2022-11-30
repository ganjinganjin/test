using RRQMCore.XREF.Newtonsoft.Json.Linq;
using RRQMSocket;
using RRQMSocket.RPC;
using RRQMSocket.RPC.JsonRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BAR.Commonlib.Connectors
{
    public class RPCServer
    {
        private static RPCServer _instance = null;
        private static readonly object padlock = new object();

        public SimpleSocketClient simpleSocketClient = new SimpleSocketClient();
        public bool IsConnected;

        public static RPCServer GetInstance()
        {
            if (_instance == null)
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new RPCServer();

                    }
                }
            }
            return _instance;
        }
        /// <summary>
        /// 创建服务
        /// </summary>
        public void Create()
        {
            try
            {
                string temp= String.Format("{0}{1}{2}", GlobalParam.RPCServerIP, ":", GlobalParam.RPCServerPort);
                RPCService rpcService = new RPCService();
                JsonRpcParser jsonRpcParser = new JsonRpcParser();
                var jsonRpcConfig = new ServiceConfig();
                jsonRpcParser.ClientConnected += TcpRPCParser_ClientConnected;
                jsonRpcParser.ClientDisconnected += TcpRPCParser_ClientDisconnected;
                jsonRpcConfig.SetValue(JsonRpcParserConfig.ListenIPHostsProperty, new IPHost[] { new IPHost(temp) })
                    .SetValue(TcpServiceConfig.ClearIntervalProperty, -1)
                    .SetValue(JsonRpcParserConfig.ProtocolTypeProperty, JsonRpcProtocolType.Tcp);
                jsonRpcParser.Setup(jsonRpcConfig);
                jsonRpcParser.Start();
                rpcService.AddRPCParser("jsonRpcParser", jsonRpcParser);
                rpcService.RegisterServer<Server>();//注册服务
                Console.WriteLine("RPC服务已启动");
            }
            catch (Exception ex)
            {
                MessageBox.Show("RPC服务启动失败：" + ex.Message);
            }
            
        }

        /// <summary>
        /// 有客户连接时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TcpRPCParser_ClientConnected(object sender, MesEventArgs e)
        {
            simpleSocketClient = (SimpleSocketClient)sender;
            IsConnected = true;
        }

        /// <summary>
        /// 有客户断开连接时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TcpRPCParser_ClientDisconnected(object sender, MesEventArgs e)
        {
            simpleSocketClient = (SimpleSocketClient)sender;
            IsConnected = false;
        }

        /// <summary>
        /// 自动化报警推送，通知类型
        /// </summary>
        public void AlertNotify(string Msg)
        {
            try
            {
                if (!IsConnected)
                {
                    return;
                }
                JObject respondJObject = new JObject();
                JObject paramsJObject = new JObject();
                respondJObject.Add("jsonrpc", "2.0");
                respondJObject.Add("method", "AlertNotify");
                paramsJObject.Add("MachineSN", "IPS5000-001");
                paramsJObject.Add("AlertModule", "--");
                paramsJObject.Add("AlertNo", "--");
                paramsJObject.Add("AlertMessage", Msg);
                paramsJObject.Add("AlertTime", DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
                respondJObject.Add("params", paramsJObject);
                byte[] byteData = Encoding.UTF8.GetBytes(respondJObject.ToString(RRQMCore.XREF.Newtonsoft.Json.Formatting.None));
                simpleSocketClient.Send(byteData);
            }
            catch (Exception)
            {

            }
            
        }

        /// <summary>
        /// 自动化产能推送，通知类型
        /// </summary>
        public void YieldNotify()
        {
            try
            {
                if (!IsConnected)
                {
                    return;
                }
                JObject respondJObject = new JObject();
                JObject paramsJObject = new JObject();

                if (UserTask.ProgrammerType == 2)//外挂烧录器
                {
                    JArray SKTStatusJArray = new JArray();
                    JObject[] SKT = new JObject[UserConfig.AllScketC];
                    int k = UserConfig.ScketUnitC, total;

                    respondJObject.Add("jsonrpc", "2.0");
                    respondJObject.Add("method", "YieldNotify");
                    paramsJObject.Add("MachineSN", "IPS5000-001");
                    paramsJObject.Add("Pass", Convert.ToString(UserTask.OKAllC));
                    paramsJObject.Add("Fail", Convert.ToString(UserTask.NGAllC));
                    paramsJObject.Add("Time", DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
                    for (int i = 0; i < UserConfig.AllScketC; i++)
                    {
                        SKT[i] = new JObject();
                        total = Axis.Group[i / k].Unit[i % k].OKAllC + Axis.Group[i / k].Unit[i % k].NGAllC;
                        SKT[i].Add("SKTIDX", i + 1);
                        SKT[i].Add("Status", Convert.ToString(Axis.Group[i / k].Unit[i % k].DownResult));//1:Pass;2:Fail;4:烧录中
                        SKT[i].Add("Pass", Convert.ToString(Axis.Group[i / k].Unit[i % k].OKAllC));
                        SKT[i].Add("Fail", Convert.ToString(Axis.Group[i / k].Unit[i % k].NGAllC));
                        SKT[i].Add("Total", Convert.ToString(total));
                        SKTStatusJArray.Add(SKT[i]);
                    }
                    paramsJObject.Add("SKTStatus", SKTStatusJArray);
                    respondJObject.Add("params", paramsJObject);
                }
                else
                {
                    respondJObject.Add("jsonrpc", "2.0");
                    respondJObject.Add("method", "YieldNotify");
                    paramsJObject.Add("MachineSN", "IPS5000-001");
                    paramsJObject.Add("Pass", Convert.ToString(UserTask.OKAllC));
                    paramsJObject.Add("Fail", Convert.ToString(UserTask.NGAllC));
                    paramsJObject.Add("Time", DateTime.Now.ToString("yyyyMMdd HH:mm:ss"));
                    respondJObject.Add("params", paramsJObject);
                }

                byte[] byteData = Encoding.UTF8.GetBytes(respondJObject.ToString(RRQMCore.XREF.Newtonsoft.Json.Formatting.None));
                simpleSocketClient.Send(byteData);
            }
            catch (Exception)
            {

            }
            
        }
    }

    /// <summary>
    /// 客户调用
    /// </summary>
    public class Server : ServerProvider
    {
        Config g_config = Config.GetInstance();
        [JsonRpc]
        public JObject GetAutoStatus()
        {
            String machineType = "IPS5000";
            String OEM = "Acroview";
            String machineSN = "IPS5000-001";
            string version = "1.0";

            JObject respondJObject = new JObject();
            respondJObject.Add("MachineType", machineType);
            respondJObject.Add("OEM", OEM);
            respondJObject.Add("MachineSN", machineSN);
            respondJObject.Add("Version", version);
            return respondJObject;
        }

        [JsonRpc]
        public JObject GetSetting(string MachineSN, string Module)
        {
            JObject respondJObject = new JObject();
            respondJObject.Add("MachineSN", MachineSN);
            respondJObject.Add("Module", Module);
            respondJObject.Add("RotateAngle", "90");
            return respondJObject;
        }

        [JsonRpc]
        public JObject GetAutoRunningInfo(string MachineSN)
        {
            TimeSpan tsR = Run.RunStopWatch.Elapsed;
            TimeSpan tsP = Run.PauseStopWatch.Elapsed;
            JObject respondJObject = new JObject();
            string MachineStatus;
            if (Auto_Flag.AutoRunBusy)
            {
                if (Auto_Flag.ALarmPause || Auto_Flag.RunPause)
                {
                    MachineStatus = "Pause";
                }
                else
                {
                    MachineStatus = "RunBusy";
                }
            }
            else
            {
                MachineStatus = "Standby";
            }
            
            respondJObject.Add("MachineSN", MachineSN);
            respondJObject.Add("MachineStatus", MachineStatus);
            respondJObject.Add("RunningTimeSpan", String.Format("{0:0}:{1:00}:{2:00}:{3:00}", tsR.Days, tsR.Hours, tsR.Minutes, tsR.Seconds));
            respondJObject.Add("FaultTimeSpan", String.Format("{0:0}:{1:00}:{2:00}:{3:00}", tsP.Days, tsP.Hours, tsP.Minutes, tsP.Seconds));
            return respondJObject;
            
        }

        [JsonRpc]
        public JObject SetTargetQuantity(int Quantity)
        {
            UserTask.TargetC = Quantity;
            g_config.WriteStaticVal();
            JObject respondJObject = new JObject();
            respondJObject.Add("Quantity", Quantity);
            return respondJObject;
        }
    }
}
