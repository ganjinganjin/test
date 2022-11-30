using BAR.Commonlib;
using BAR.Commonlib.Connectors;
using BAR.Commonlib.Connectors.Protocols;
using BAR.Communicators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR
{
    class Cathetometer
    {
        public Config g_config = Config.GetInstance();
        private static Cathetometer _instance = null;
        private static readonly object padlock = new object();
        private SerialConector SerialCtrl;
        private static CathetometerProxy proxy = CathetometerProxy.GetInstance();
        public static double Value;

        public Cathetometer()
        {
            if (Config.Altimeter != 0)
            {
                SerialCtrl = SerialConector.GetInstance("CathetometerSerialCtl");
                if (SerialCtrl.CreateSerialPort() && !SerialCtrl.IsOpen)
                {
                    SerialCtrl.OpenConnection(g_config.ArrStrutCom[4].ICom, g_config.ArrStrutCom[4].IBaud, g_config.ArrStrutCom[4].IDataBits, g_config.ArrStrutCom[4].IParity, g_config.ArrStrutCom[4].IStopBits, "Cathetometer Ports");
                }
                SerialCtrl.AutoResetEventSet();
            }
        }

        public static Cathetometer GetInstance()
        {
            if (_instance == null)
            {
                lock (padlock)
                {
                    _instance = new Cathetometer();
                }
            }
            return _instance;
        }

        public static void MODBUS_ReadAI()
        {
            Axis.Altimeter.ReadAI_Busy = true;
            if (Config.Altimeter == 3)//内置通讯模块松下激光传感器
            {
                string strDate = "%01#RMD";
                var DateBuffer = Encoding.UTF8.GetBytes(strDate.ToCharArray());
                strDate += Modbus.BCC(DateBuffer, DateBuffer.Length);
                strDate += SerialConector.NewLine;
                proxy.SendCmd("ReadMD", strDate, 0);
            }
            else 
            {
                int CRCtemp;
                byte[] TxBuffer = new byte[8];
                TxBuffer[0] = 0x01;
                TxBuffer[1] = 0x03;
                TxBuffer[2] = 0x00;
                TxBuffer[3] = 0x00;
                TxBuffer[4] = 0x00;
                TxBuffer[5] = 0x02;
                CRCtemp = Modbus.Crc16(TxBuffer, TxBuffer.Length - 2);
                TxBuffer[6] = (byte)(CRCtemp & 0x00FF);
                TxBuffer[7] = (byte)((CRCtemp & 0xFF00) >> 8);
                proxy.SendCmd("ReadAI", TxBuffer, 9, true, 50);
            }
            
        }
    }
}
