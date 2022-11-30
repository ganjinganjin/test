using BAR.Commonlib;
using BAR.Commonlib.Connectors;
using BAR.Commonlib.Events;
using System;
using System.Collections.Concurrent;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BAR.Commonlib.Config;

namespace BAR.Communicators
{
    public class TesterProxy
    {
        public delegate void ProxyEventNotifyHander(int index, string frame);
        public event ProxyEventNotifyHander ProxyNotifyEvent;
        public SerialPort seriaPort;
        public volatile BlockingCollection<string> CommandsCollection = new BlockingCollection<string>(new ConcurrentQueue<string>());

        public const string NewLine = "#";
        private readonly object _syncLock = new object();

        private int Index ;

        public TesterProxy(int index)
        {
            Index = index;
        }

        public void OpenConnection(TagLamp tagLamp)
        {
            string[] str = SerialPort.GetPortNames();
            if (str == null)
            {
                MessageBox.Show("本机没有串口！", "Error");
            }

            seriaPort = new SerialPort();
            try
            {
                if (!seriaPort.IsOpen)
                {
                    seriaPort.BaudRate = tagLamp.IBaud;
                    seriaPort.DataBits = tagLamp.IDataBits;
                    seriaPort.PortName = "COM" + tagLamp.ICom;
                    seriaPort.Parity = (Parity)tagLamp.IParity;
                    seriaPort.StopBits = (StopBits)tagLamp.IStopBits;
                    seriaPort.NewLine = NewLine;
                    seriaPort.ReadTimeout = 500;
                    seriaPort.WriteTimeout = 500;
                    seriaPort.Open();
                    seriaPort.DataReceived += new SerialDataReceivedEventHandler(ReceivedData);
                    CommandDequeue();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "TesterOpenConnection");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagLamp"></param>
        public void CloseConnection()
        {
            try
            {
                if (CommandsCollection != null)
                {
                    CommandsCollection.Add("End");
                }
                seriaPort.Close();
                seriaPort.DataReceived -= new SerialDataReceivedEventHandler(ReceivedData);
                seriaPort.Dispose();
                seriaPort = null;
                GC.Collect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "TesterCloseConnection");
            }
        }

        /// <summary>
        /// 命令入队
        /// </summary>
        /// <param name="cmd"></param>
        public void CommandEnQueue(string cmd)
        {
            lock (this)
            {
                if (seriaPort.IsOpen)
                {
                    CommandsCollection.Add(cmd);
                }
                else
                {
                    MessageBox.Show("串口未打开！", "Error");
                }
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
                foreach (var cmd in CommandsCollection.GetConsumingEnumerable())
                {
                    try
                    {
                        if (cmd == "End")
                        {
                            CommandsCollection.Dispose();
                            break;
                        }
                        seriaPort.WriteLine(cmd);
                        while (seriaPort.BytesToWrite != 0) ;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "TesterCommandDequeue");
                    }
                    Thread.Sleep(5);
                }
            }, TaskCreationOptions.LongRunning);
        }

        public void ReceivedData(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                lock (_syncLock)
                {
                    string frame = seriaPort.ReadLine();
                    if (ProxyNotifyEvent != null)
                    {
                        Task.Run(() => ProxyNotifyEvent(Index, frame));//开启检测线程
                    }
                }
            }
            catch (Exception ex)
            {
                if (seriaPort.IsOpen)
                {
                    seriaPort.DiscardInBuffer();
                } 
            }
        }
    } 
}
