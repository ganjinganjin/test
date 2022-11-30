using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace BAR.Commonlib.FrameWork.Patterns.Command
{

    public class CommandManager
    {
        // 定义一个静态变量来保存类的实例
        private static CommandManager   _instance = null;
        private static Dictionary<String, CommandManager> instanceDir = new Dictionary<String, CommandManager>();
        // 定义一个标识确保线程同步
        private static readonly object _objPadLock = new object();

        public delegate void CommandQueueEventNotifyHander(BaseCmd cmdName);
        public event CommandQueueEventNotifyHander CmdNotifyEvent;

        public volatile BlockingCollection<BaseCmd> CommandsCollection = new BlockingCollection<BaseCmd>(new ConcurrentQueue<BaseCmd>());

        //Act             g_act;
        int             Interval;
        public CommandManager()
        {
            //g_act = Act.GetInstance();
            Interval = 0;
        }
        public void SetTriggerTimer(int triggerTime)
        {
            this.Interval = triggerTime;
        }
        public static CommandManager GetInstance(String managerID = "Main")
        {
            lock (_objPadLock)
            {
                if (instanceDir.ContainsKey(managerID))
                {
                    _instance = instanceDir[managerID];
                }
                else
                {
                    instanceDir[managerID] = new CommandManager();
                    _instance = instanceDir[managerID];
                }
            }
            return _instance;
        }

        /// <summary>
        /// 命令入队
        /// </summary>
        /// <param name="cmd"></param>
        public void CommandEnQueue(BaseCmd cmd)
        {
            lock (this)
            {
                CommandsCollection.Add(cmd);
            }
        }

        /// <summary>
        /// 命令出队
        /// </summary>
        /// <param name="cmd"></param>
        public void CommandDequeue()
        {

            Task.Factory.StartNew(() =>
            {
                foreach (BaseCmd cmd in CommandsCollection.GetConsumingEnumerable())
                {
                    int tryCount = 3;
                    bool state = false;
                    for (int i = 0; i < tryCount; i++)
                    {
                        if (!cmd.Write())
                            continue;
                        if (!cmd.SerialCtrl.autoEvent.WaitOne(cmd.IOutTime))
                        {
                            //Debug.WriteLine($"Send Command 接收超时 - {cmd.CommandName} trycount:" + i);
                            continue;
                        }
                        Thread.Sleep(5);

                        if (cmd.Execute())
                        {
                            if (CmdNotifyEvent != null)
                            {
                                this.CmdNotifyEvent(cmd);
                            }
                            state = true;//通讯成功
                            break;
                        }
                    }
                    //判断通讯状态
                    if (cmd.CommandName.Contains("ReadDownloadStatus_Result"))
                    {
                        Auto_Flag.BurnOnline = state;
                    }
                    else if (cmd.CommandName == "ReadBredeStateAlarm")
                    {
                        Auto_Flag.BredeOnline = state;
                    }
                    else if (cmd.CommandName == "ReadTrayStateAlarm")
                    {
                        Auto_Flag.AutoTrayOnline = state;
                    }
                    else if (cmd.CommandName == "ReadAI" || cmd.CommandName == "ReadMD")
                    {
                        Axis.Altimeter.ReadAI_Online = state;
                        Axis.Altimeter.ReadAI_Busy = false;
                    }
                    else if (cmd.CommandName == "ReadZoomLensStatus")
                    {
                        Axis.ZoomLens_S.ReadStatus_Online = state;
                        if (state == false)
                        {
                            Axis.ZoomLens_S.Home = false;
                        }
                        Axis.ZoomLens_S.ReadStatus_Busy = false;
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        /// 网口命令出队
        /// </summary>
        /// <param name="cmd"></param>
        public void CommandDequeue_Net()
        {
            Task.Factory.StartNew(() =>
            {
                foreach (BaseCmd cmd in CommandsCollection.GetConsumingEnumerable())
                {
                    int tryCount = 3;
                    for (int i = 0; i < tryCount; i++)
                    {
                        if (!cmd.Write_Net())
                            continue;
                        if (!cmd.NetCtrl.autoEvent.WaitOne(cmd.IOutTime))
                        {
                            //Debug.WriteLine($"Send Command 接收超时 - {cmd.CommandName} trycount:" + i);
                            continue;
                        }
                        Thread.Sleep(5);
                        if (cmd.Execute_Net())
                        {
                            if (CmdNotifyEvent != null)
                            {
                                this.CmdNotifyEvent(cmd);
                            }
                            break;
                        }
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }
    }
}
