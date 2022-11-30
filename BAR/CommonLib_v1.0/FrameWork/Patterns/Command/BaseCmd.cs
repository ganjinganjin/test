using BAR.Commonlib.Connectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BAR.Commonlib.FrameWork.Patterns.Command
{
    public abstract class BaseCmd
    {
        public int IOutTime;
        public SerialConector SerialCtrl;
        public NetConnector NetCtrl;
        /// <summary>
        /// 是否重执行
        /// </summary>
        public bool IsCanRedo { set; get; }

        public String CommandName;
        /// <summary>
        /// 串口写数据
        /// </summary>
        /// <returns></returns>
        public abstract bool Write();
        /// <summary>
        /// 串口命令执行
        /// </summary>
        public abstract bool Execute();

        /// <summary>
        /// 网口写数据
        /// </summary>
        /// <returns></returns>
        public abstract bool Write_Net();
        /// <summary>
        /// 网口命令执行
        /// </summary>
        public abstract bool Execute_Net();
    }
}
