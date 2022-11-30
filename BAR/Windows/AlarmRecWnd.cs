using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using BAR.Commonlib;
using CCWin.SkinControl;
using System.Collections;

namespace BAR.Windows
{
    public partial class AlarmRecWnd : Form
    {
        Config          g_config = Config.GetInstance();
        Act             g_act = Act.GetInstance();
        SkinButton[]    ArrTabBtns;
        public AlarmRecWnd()
        {
            InitializeComponent();
            ArrTabBtns = new SkinButton[2];
            ArrTabBtns[0] = this.tabBtn1;
            ArrTabBtns[1] = this.tabBtn2;
        }
        private void AlarmRecWnd_Load(object sender, EventArgs e)
        {
            this.__ReadHistoryAlarm();
            this.__InitWndUI();
        }
        private void __ReadHistoryAlarm()
        {
            g_act.ArrHisAlarms.Clear();
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\Log\" + "AlarmLog.log";
            StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding("gb2312"));
            
            while(!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                Act.AlarmInfo info = new Act.AlarmInfo();
                string[] arrSubStr = line.Split(" ".ToCharArray());
                info.StrDate = arrSubStr[0];
                info.StrTime = arrSubStr[1];
                info.StrDetail = arrSubStr[2];

                g_act.ArrHisAlarms.Add(info);
            }

            sr.Close();
        }
        private void __ChangeTabBtnState(SkinButton chooseBtn)
        {
            for (int i = 0; i< ArrTabBtns.Length; i++){
                if(chooseBtn == ArrTabBtns[i])
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
            __ChangeTabBtnState(this.tabBtn2);
            __FlashHisToryList();
        }
        private void __InitListView()
        {
            alarmList.Clear();

            ColumnHeader ch = new ColumnHeader();
            ch.Text = "日期(Date)";
            ch.Width = 0;
            ch.TextAlign = HorizontalAlignment.Left;
            this.alarmList.Columns.Add(ch);

            ColumnHeader ch1 = new ColumnHeader();
            ch1.Text = "时间(Time)";
            ch1.Width = 120;
            ch1.TextAlign = HorizontalAlignment.Left;
            this.alarmList.Columns.Add(ch1);

            ColumnHeader ch2 = new ColumnHeader();
            ch2.Text = "告警描述(Detail)";
            ch2.Width = 400;
            ch2.TextAlign = HorizontalAlignment.Left;
            this.alarmList.Columns.Add(ch2);
        }
        private void __FlashAlarmList()
        {
            this.alarmList.Items.Clear();
            this.alarmList.BeginUpdate();
            this.alarmList.Columns[0].Width = 0;
            for (int n = 0; n < g_act.ArrRealAlarms.Count; n++)
            {
                ListViewItem lvi1 = new ListViewItem();
                Act.AlarmInfo alarmInfo = (Act.AlarmInfo)g_act.ArrRealAlarms[n];
                lvi1.Text = "";
                lvi1.SubItems.Add(Convert.ToString(alarmInfo.StrTime));
                lvi1.SubItems.Add(Convert.ToString(alarmInfo.StrDetail));

                alarmList.Items.Add(lvi1);
            }
            this.alarmList.EndUpdate();
        }
        private void __FlashHisToryList()
        {
            this.alarmList.Items.Clear();
            this.alarmList.BeginUpdate();
            this.alarmList.Columns[0].Width = 120;
            for (int n = 0; n < g_act.ArrHisAlarms.Count; n++)
            {
                ListViewItem lvi1 = new ListViewItem();
                Act.AlarmInfo alarmInfo = (Act.AlarmInfo)g_act.ArrHisAlarms[n];
                lvi1.Text = alarmInfo.StrDate;
                lvi1.SubItems.Add(Convert.ToString(alarmInfo.StrTime));
                lvi1.SubItems.Add(Convert.ToString(alarmInfo.StrDetail));

                alarmList.Items.Add(lvi1);
            }
            this.alarmList.EndUpdate();
        }
        private void tabBtn_MouseUp(object sender, MouseEventArgs e)
        {
            SkinButton btn = sender as SkinButton;
            if(btn == this.tabBtn1)
            {
                __ChangeTabBtnState(this.tabBtn1);
                __FlashAlarmList();
            }
            else if(btn == this.tabBtn2)
            {
                __ChangeTabBtnState(this.tabBtn2);
                __FlashHisToryList();
            }
        }
    }
}
