using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace BAR.Commonlib.FrameWork.Patterns.Proxy
{
    public abstract class Proxy
    {
        public static String NAME = "Proxy";

        ///<proxy名字
        protected String StrProxyName;
        ///<proxy数据
        protected Object ObjData;

        public Proxy(String proxyName = null, Object data = null)
        {
            this.StrProxyName = proxyName;
            this.ObjData = data;
        }
        public String GetProxyName()
        {
            return StrProxyName;
        }
        public void SetData( Object data)
        {
            this.ObjData = data;
        }
        public Object GetData()
        {
            return this.ObjData;
        }
    }
}
