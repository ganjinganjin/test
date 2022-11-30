using BAR.Commonlib;
using BAR.Commonlib.Connectors;
using BAR.CommonLib_v1._0;
using CCWin.SkinControl;
using DllInterface.CodeFirst;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BAR.Windows
{
    public partial class ToolTipWnd : Form
    {
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public Act g_act = Act.GetInstance();
        Config g_config = Config.GetInstance();
        public RPCServer rpcServer = RPCServer.GetInstance();
        SkinButton[] ArrTabBtns;

        public bool IsShow;
        BAR main = null;
        public ToolTipWnd(BAR main)
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();
            ArrTabBtns = new SkinButton[2];
            ArrTabBtns[0] = this.tabBtn1;
            ArrTabBtns[1] = this.tabBtn2;
            this.ControlBox = false;
            this.main=main;
            //main = (BAR)this.Owner;
        }
        private void ToolTipWnd_Load(object sender, EventArgs e)
        {
            this.__ReadHistoryAlarm();
            this.__InitWndUI();
            
        }
        private void __ReadHistoryAlarm()
        {
            g_act.ArrHisAlarms.Clear();
            string logPath = AppDomain.CurrentDomain.BaseDirectory + @"\Log\";
            DirectoryInfo dirInfo = new DirectoryInfo(logPath);
            DirectoryInfo[] dirs = dirInfo.GetDirectories();
            int len = dirs.Length;
            if (len > 5) len = 5;
            
            for (int i = 1; i <= len; i++)
            {
                string filePath = logPath + dirs[dirs.Length - i].Name+ "\\" + "AlarmLog.log";
                var temList = new List<Act.AlarmInfo>();
                if (File.Exists(filePath))
                {
                    try
                    {
                        FileStream fs = new FileStream(filePath, FileMode.Open);
                        StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("gb2312"));

                        while (!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            Act.AlarmInfo info = new Act.AlarmInfo();
                            string[] arrSubStr = line.Split(" ".ToCharArray());
                            info.StrDate = arrSubStr[0];
                            info.StrTime = arrSubStr[1];
                            for (int j = 2; j < arrSubStr.Length; j++)
                            {
                                info.StrDetail += arrSubStr[j] + " ";
                            }
                            temList.Add(info);
                        }
                        sr.Close();
                        temList.Reverse();
                    }
                    catch(Exception)
                    {
                        continue;
                    }
                    
                }
                g_act.ArrHisAlarms.AddRange(temList);
            }
        }
        private void __InitListView()
        {
            alarmList.Clear();

            ColumnHeader ch = new ColumnHeader
            {
                Text = MultiLanguage.GetString("日期"),
                Width = 0,
                TextAlign = HorizontalAlignment.Left
            };
            this.alarmList.Columns.Add(ch);

            ColumnHeader ch1 = new ColumnHeader
            {
                Text = MultiLanguage.GetString("时间"),
                Width = 120,
                TextAlign = HorizontalAlignment.Left
            };
            this.alarmList.Columns.Add(ch1);

            ColumnHeader ch2 = new ColumnHeader
            {
                Text = MultiLanguage.GetString("告警描述"),
                Width = 900,
                TextAlign = HorizontalAlignment.Left
            };
            this.alarmList.Columns.Add(ch2);
        }
        private void __ChangeTabBtnState(SkinButton chooseBtn)
        {
            for (int i = 0; i < ArrTabBtns.Length; i++)
            {
                if (chooseBtn == ArrTabBtns[i])
                {
                    ArrTabBtns[i].BaseColor = Color.FromArgb(56, 56, 56);
                }
                else
                {
                    ArrTabBtns[i].BaseColor = Color.Gray;
                }
            }
        }
        private void __InitWndUI()
        {
            //__ChangeTabBtnState(this.tabBtn1);
            __InitListView();
            __ChangeTabBtnState(this.tabBtn1);
            __FlashHisToryList();
        }
        private void __FlashHisToryList()
        {
            this.alarmList.Items.Clear();
            this.alarmList.BeginUpdate();
            this.alarmList.Columns[0].Width = 120;
            for (int n = 0; n < g_act.ArrHisAlarms.Count; n++)
            {
                ListViewItem lvi1 = new ListViewItem();
                Act.AlarmInfo alarmInfo = g_act.ArrHisAlarms[n];
                lvi1.Text = alarmInfo.StrDate;
                lvi1.SubItems.Add(Convert.ToString(alarmInfo.StrTime));
                lvi1.SubItems.Add(Convert.ToString(alarmInfo.StrDetail));

                alarmList.Items.Add(lvi1);
            }
            this.alarmList.EndUpdate();
        }
        public void WriteToolTipStr(string str)
        {
            str = MultiLanguage.GetString(str);
            __ChangeTabBtnState(this.tabBtn1);
            this.lvToolTip.Visible = true;
            this.alarmList.Visible = false;
            //if (Auto_Flag.PenAlt_Flag)
            //{
            //    btnConfirm.Location = new Point(230, 417);
            //    btnJump.Visible = false;
            //}
            //else
            {
                btnConfirm.Location = new Point(127, 417);
                btnJump.Visible = true;
            }
            string ID = (lvToolTip.Items.Count + 1).ToString();
            ListViewItem lvItem = new ListViewItem();//新建行
            lvItem.SubItems.Add(ID);//写入序号
            lvItem.SubItems.Add(str);//写入告警内容
            lvToolTip.Items.Add(lvItem);//更新控件显示
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Alarm");
            g_act.RecordAlarmsInfo(str);
            rpcServer.AlertNotify(str);
        }
        public void btnConfirm_Click(object sender, EventArgs e)
        {
            lvToolTip.Items.Clear();
            Auto_Flag.ALarm = false;
            if (Auto_Flag.BredeALarm && Auto_Flag.Brede_LayIC)
            {
                Brede.Send_Cmd(Brede.Cmd_ClearAlarm);   //清除报警命令
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "发送编带清除报警命令", "Flow");
            }
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "清除报警", "Flow");
            this.Hide();
            main.Activate();
        }

        private void tabBtn_MouseUp(object sender, EventArgs e)
        {
            SkinButton btn = sender as SkinButton;
            if (btn == this.tabBtn1)
            {
                __ChangeTabBtnState(this.tabBtn1);
                this.lvToolTip.Visible = true;
                this.alarmList.Visible = false;
            }
            else if (btn == this.tabBtn2)
            {
                __ChangeTabBtnState(this.tabBtn2);
                this.lvToolTip.Visible = false;
                this.alarmList.Visible = true;
                this.__ReadHistoryAlarm();
                __FlashHisToryList();
            }
        }

        private void ToolTipWnd_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
            IsShow = false;
            
        }

        public void ToolTipWnd_FormClosed(object sender, FormClosedEventArgs e)
        {
            //IntPtr Hwnd = FindWindow(null, main.Text);
            //SetForegroundWindow(Hwnd);
        }

        private void btnJump_Click(object sender, EventArgs e)
        {
            Auto_Flag.JumpMainStep_Flag = true;
            Auto_Flag.JumpBredeStep_Flag = true;
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "跳过当前动作", "Flow");
            btnConfirm_Click(sender, e);
        }

    }
}
