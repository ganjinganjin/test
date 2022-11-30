using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BAR.Commonlib;
using BAR.Commonlib.Connectors;
using BAR.Commonlib.Connectors.Protocols;
using BAR.Commonlib.FrameWork.Patterns.Command;
using BAR.Commonlib.FrameWork.Patterns.Proxy;
using BAR.Communicates.Commands;

namespace BAR.Communicates
{
    public abstract class BaseProxy : Proxy
    {
        public CommandManager CmdManger;

        /// <summary>
        /// 命令发送
        /// </summary>
        public void SendCmd(Command cmd, bool isNoRepeatCmd)
        {
            if(!isNoRepeatCmd)
            {
                if (CmdManger.CommandsCollection.Contains(cmd))
                    return;
            }
            CmdManger.CommandEnQueue(cmd);
        }
    } 
}
