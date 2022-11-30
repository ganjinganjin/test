using BAR.CommonLib_v1._0;
using BAR.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using TensionDetectionDLL;

namespace BAR
{
    static class Program
    {
        static BAR mainForm;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            MultiLanguage.SetDefaultLanguage();
            string processName = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(processName);

            //处理未捕获的异常
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //处理UI线程异常
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //处理非UI线程异常
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            
            //如果该数组长度大于1，说明多次运行
            if (processes.Length > 1)
            {
                MessageBox.Show(MultiLanguage.GetString("程序已运行,不能再次打开"), "Warning：", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Environment.Exit(1);
            }
            else
            {
                try
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    UserLoginWnd fl = new UserLoginWnd();
                    fl.ShowDialog();
                    if (StaticInfo.CheckUserName())
                    {
                        mainForm = new BAR();
                        mainForm.Text += Application.ProductVersion.ToString()+ "     " + MultiLanguage.GetString(StaticInfo.TDUser);
                        mainForm.ShowDialog();
                    }
                    else
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.InnerException.ToString());
                }

            }
        }

        /// <summary>
        /// 处理未捕获的异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            mainForm.ReleseDevices();
            Exception ex = e.ExceptionObject as Exception;
            string str = string.Format("An unhandled exception was caught（ErrorCode：{0}）：{1}\r\nCLR exit：{2}", Marshal.GetLastWin32Error(), ex.GetBaseException(),e.IsTerminating);
            mainForm.g_act.GenLogMessage(GlobConstData.ST_LOG_RECORD, str, "Error");
            MessageBox.Show(str);
        }

        /// <summary>
        /// 处理UI线程异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            //mainForm.ReleseDevices();
            Exception ex = e.Exception;
            string str = string.Format("An unhandled exception was caught（ErrorCode：{0}）：{1}", Marshal.GetLastWin32Error(), ex.GetBaseException());
            mainForm.g_act.GenLogMessage(GlobConstData.ST_LOG_RECORD, str, "Error");
            MessageBox.Show(str);
        }
    } 
}
