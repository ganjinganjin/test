using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace BAR.Commonlib.Events
{ 
    public class MsgEvent:EventArgs
    {
        public const int MSG_IMGPAINT = 1000;
        public const int MSG_BRAIDFLASH = 1001;
        public const int MSG_BURNFLASH = 1002;
        public const int MSG_TRAYSIZEFLASH = 1003;
        public const int MSG_TRAYCOLORFLASH = 1004;

        public int MsgType;
        public object Data;
        public MsgEvent(int msgType, object data)
        {
            this.MsgType = msgType;
            this.Data = data;
        }
    }

}
