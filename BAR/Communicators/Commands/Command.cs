using BAR.Commonlib.Connectors;
using BAR.Commonlib.FrameWork.Patterns.Command;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace BAR.Communicates.Commands
{
    public class Command : BaseCmd
    {
        public int IRevByteNum { get; set; } = 0;

        public object RawData { set; get; }
        public object SendData { set; get; }
        public object RevData { set; get; }

        public object StrSendData { set; get; }
        public object StrRevData { set; get; }
        public Command(string name)
        {
            CommandName = name;
        }

        public override bool Equals(object obj)
        {
            //按需求定制自己需要的比较方式
            Command cmd = obj as Command;
            return (CommandName == cmd.CommandName);
        }

        public override bool Write()
        {
            byte[] sData = (byte[])SendData;
            return SerialCtrl.SendData(sData);
        }

        public override bool Execute()
        {
            return true;
        }

        public override bool Write_Net()
        {
            byte[] sData = (byte[])SendData;
            return NetCtrl.Broadcast(sData);
        }

        public override bool Execute_Net()
        {
            return true;
        }
    }
}
