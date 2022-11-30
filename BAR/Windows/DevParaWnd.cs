using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CCWin.SkinControl;
using BAR.Commonlib;
using System.Threading.Tasks;
using System.Threading;
using BAR.CommonLib_v1._0;

namespace BAR.Windows
{
    public partial class DevPara : Form
    {
        Config g_config = Config.GetInstance();
        Act g_act = Act.GetInstance();

        NumericUpDown[]     ArrNumTxt;

        public DevPara()
        {
            MultiLanguage.SetDefaultLanguage();
            InitializeComponent();
            if (Config.SyncTakeLay && UserTask.ProgrammerType != GlobConstData.Programmer_YED)
            {
                label9.Text = MultiLanguage.DefaultLanguage == GlobConstData.ST_English? "Put errors(mm)" : "同放误差值(mm)：";
            }

            ArrNumTxt = new NumericUpDown[30];
            ArrNumTxt[0] = this.numericUpDown1;
            ArrNumTxt[1] = this.numericUpDown2;
            ArrNumTxt[2] = this.numericUpDown3;
            ArrNumTxt[3] = this.numericUpDown4;
            ArrNumTxt[4] = this.numericUpDown5;
            ArrNumTxt[5] = this.numericUpDown6;
            ArrNumTxt[6] = this.numericUpDown7;
            ArrNumTxt[7] = this.numericUpDown8;
            ArrNumTxt[8] = this.numericUpDown9;
            ArrNumTxt[9] = this.numericUpDown10;
            ArrNumTxt[10] = this.numericUpDown11;
            ArrNumTxt[11] = this.numericUpDown12;
            ArrNumTxt[12] = this.numericUpDown13;
            ArrNumTxt[13] = this.numericUpDown14;
            ArrNumTxt[14] = this.numericUpDown15;
            ArrNumTxt[15] = this.numericUpDown16;
            ArrNumTxt[16] = this.numericUpDown17;
            ArrNumTxt[17] = this.numericUpDown18;
            ArrNumTxt[18] = this.numericUpDown19;
            ArrNumTxt[19] = this.numericUpDown20;
            ArrNumTxt[20] = this.numericUpDown21;
            ArrNumTxt[21] = this.numericUpDown22;
            ArrNumTxt[22] = this.numericUpDown23;
            ArrNumTxt[23] = this.numericUpDown24;
            ArrNumTxt[24] = this.numericUpDown25;
            ArrNumTxt[25] = this.numericUpDown26;
            ArrNumTxt[26] = this.numericUpDown27;
            ArrNumTxt[27] = this.numericUpDown28;
            ArrNumTxt[28] = this.numericUpDown29;
            ArrNumTxt[29] = this.numericUpDown30;
        }
        private void DevPara_Load(object sender, EventArgs e)
        {
            //this.__InitButton();
            this.__InitParaText();
        }
 
        public void __InitParaText()
        {
            this.ArrNumTxt[0].Value = new decimal(HeightVal.Tray_LayIC);

            this.ArrNumTxt[1].Value = new decimal(HeightVal.Tray_TakeIC);

            this.ArrNumTxt[2].Value = new decimal(HeightVal.DownSeat_LayIC);

            this.ArrNumTxt[3].Value = new decimal(HeightVal.DownSeat_TakeIC);

            this.ArrNumTxt[4].Value = new decimal(HeightVal.Tube_TakeIC);

            this.ArrNumTxt[5].Value = new decimal(HeightVal.Brede_TakeIC);

            this.ArrNumTxt[6].Value = new decimal(HeightVal.Brede_LayIC);

            this.ArrNumTxt[7].Value = new decimal(Axis.Position_X.tubespace);

            this.ArrNumTxt[8].Value = new decimal((double)AutoTiming.SeatTakeDelay / 1000);

            this.ArrNumTxt[9].Value = new decimal((double)AutoTiming.BuzzerDuration / 1000);

            this.ArrNumTxt[10].Value = new decimal((double)AutoTiming.DownTimeOut / 1000);

            this.ArrNumTxt[11].Value = new decimal(TrayD.ColC);

            this.ArrNumTxt[12].Value = new decimal(TrayD.RowC);

            this.ArrNumTxt[13].Value = new decimal(TrayD.Col_Space);

            this.ArrNumTxt[14].Value = new decimal(TrayD.Row_Space);

            this.ArrNumTxt[15].Value = new decimal(HeightVal.Safe);

            this.ArrNumTxt[16].Value = new decimal(UserTask.Deviate);

            this.ArrNumTxt[17].Value = new decimal((double)AutoTiming.DownDelay / 1000);

            this.ArrNumTxt[18].Value = new decimal((double)AutoTiming.VacuumDuration / 1000);

            this.ArrNumTxt[19].Value = new decimal((double)AutoTiming.BlowDelay / 1000);

            this.ArrNumTxt[20].Value = new decimal(UserTask.NGContinueC);

            this.ArrNumTxt[21].Value = new decimal(UserTask.NGReBurnC);

            this.ArrNumTxt[22].Value = new decimal((double)AutoTiming.BlowDuration / 1000);

            this.ArrNumTxt[23].Value = new decimal((double)AutoTiming.BredeTakeDelay / 1000);

            this.ArrNumTxt[24].Value = new decimal(UserTask.NGScketC_Shut);

            this.ArrNumTxt[25].Value = new decimal(UserTask.NGContinueC_Shut);

            this.ArrNumTxt[26].Value = new decimal(UserTask.NGAllC_Shut);

            this.ArrNumTxt[27].Value = new decimal((double)AutoTiming.TubeTakeDelay / 1000);

            this.ArrNumTxt[28].Value = new decimal((double)AutoTiming.TubeTimeOut / 1000);

            this.ArrNumTxt[29].Value = new decimal(UserTask.MPa);

        }
       
        private void savaAllBtn_Click(object sender, EventArgs e)
        {
            HeightVal.Tray_LayIC = g_config.ArrDevParas[0].DData;
            HeightVal.Tray_TakeIC = g_config.ArrDevParas[1].DData;
            HeightVal.DownSeat_LayIC = g_config.ArrDevParas[2].DData;
            HeightVal.DownSeat_TakeIC = g_config.ArrDevParas[3].DData;
            HeightVal.Tube_TakeIC = g_config.ArrDevParas[4].DData;
            HeightVal.Brede_TakeIC = g_config.ArrDevParas[5].DData;
            HeightVal.Brede_LayIC = g_config.ArrDevParas[6].DData;
            Axis.Position_X.tubespace = g_config.ArrDevParas[7].DData;
            AutoTiming.SeatTakeDelay = g_config.ArrDevParas[8].IData;
            AutoTiming.BuzzerDuration = g_config.ArrDevParas[9].IData;
            AutoTiming.DownTimeOut = g_config.ArrDevParas[10].IData;
            AutoTiming.BlowDuration = g_config.ArrDevParas[11].IData;


            HeightVal.Tray_LayIC = g_config.ArrDevParas[0].DData = Convert.ToDouble(this.ArrNumTxt[0].Value);

            HeightVal.Tray_TakeIC = g_config.ArrDevParas[1].DData = Convert.ToDouble(this.ArrNumTxt[1].Value);

            HeightVal.DownSeat_LayIC = g_config.ArrDevParas[2].DData = Convert.ToDouble(this.ArrNumTxt[2].Value);

            HeightVal.DownSeat_TakeIC = g_config.ArrDevParas[3].DData = Convert.ToDouble(this.ArrNumTxt[3].Value);

            HeightVal.Tube_TakeIC = g_config.ArrDevParas[4].DData = Convert.ToDouble(this.ArrNumTxt[4].Value);

            HeightVal.Brede_TakeIC = g_config.ArrDevParas[5].DData = Convert.ToDouble(this.ArrNumTxt[5].Value);

            HeightVal.Brede_LayIC = g_config.ArrDevParas[6].DData = Convert.ToDouble(this.ArrNumTxt[6].Value);

            Axis.Position_X.tubespace = g_config.ArrDevParas[7].DData = Convert.ToDouble(this.ArrNumTxt[7].Value);

            AutoTiming.SeatTakeDelay = g_config.ArrDevParas[8].IData = (uint)(Convert.ToDouble(this.ArrNumTxt[8].Value) * 1000);

            AutoTiming.BuzzerDuration = g_config.ArrDevParas[9].IData = (uint)(Convert.ToDouble(this.ArrNumTxt[9].Value) * 1000);

            AutoTiming.DownTimeOut = g_config.ArrDevParas[10].IData = (uint)(Convert.ToDouble(this.ArrNumTxt[10].Value) * 1000);

            AutoTiming.BlowDuration = g_config.ArrDevParas[11].IData = (uint)(Convert.ToDouble(this.ArrNumTxt[22].Value) * 1000);


            TrayD.ColC = (int)Convert.ToSingle(this.ArrNumTxt[11].Value);
    
            TrayD.RowC = (int)Convert.ToSingle(this.ArrNumTxt[12].Value);
         
            TrayD.Col_Space = Convert.ToDouble(this.ArrNumTxt[13].Value);
        
            TrayD.Row_Space = Convert.ToDouble(this.ArrNumTxt[14].Value);


            HeightVal.Safe = g_config.ArrDevParas[12].DData = Convert.ToDouble(this.ArrNumTxt[15].Value);

            UserTask.Deviate = Convert.ToDouble(this.ArrNumTxt[16].Value);

            AutoTiming.DownDelay = g_config.ArrDevParas[14].IData = (uint)(Convert.ToDouble(this.ArrNumTxt[17].Value) * 1000);

            AutoTiming.VacuumDuration = g_config.ArrDevParas[15].IData = (uint)(Convert.ToDouble(this.ArrNumTxt[18].Value) * 1000);

            AutoTiming.BlowDelay = g_config.ArrDevParas[16].IData = (uint)(Convert.ToDouble(this.ArrNumTxt[19].Value) * 1000);
                
            g_config.ArrDevParas[17].IData = Convert.ToUInt32(this.ArrNumTxt[20].Value);
            UserTask.NGContinueC = (int)g_config.ArrDevParas[17].IData;

            g_config.ArrDevParas[18].IData = Convert.ToUInt32(this.ArrNumTxt[21].Value);
            UserTask.NGReBurnC = (int)g_config.ArrDevParas[18].IData;

            AutoTiming.BredeTakeDelay = g_config.ArrDevParas[19].IData = (uint)(Convert.ToDouble(this.ArrNumTxt[23].Value) * 1000);

            UserTask.NGScketC_Shut = Convert.ToInt32(this.ArrNumTxt[24].Value);
            UserTask.NGContinueC_Shut = Convert.ToInt32(this.ArrNumTxt[25].Value);
            UserTask.NGAllC_Shut = Convert.ToInt32(this.ArrNumTxt[26].Value);
            AutoTiming.TubeTakeDelay = (uint)(Convert.ToDouble(this.ArrNumTxt[27].Value) * 1000);
            AutoTiming.TubeTimeOut = (uint)(Convert.ToDouble(this.ArrNumTxt[28].Value) * 1000);
            UserTask.MPa = Convert.ToDouble(this.ArrNumTxt[29].Value);

            g_act.SetDac(12, UserTask.MPa);
            g_config.SaveDevParaVal();
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "保存设备参数成功", "Modify");
        }

        private void skinButton7_Click(object sender, EventArgs e)
        {
            double setx, sety;
            setx = Axis.Position_X.trayFirst[0] - Axis.Pen[0].Offset_TopCamera_X;
            sety = Axis.Position_Y.trayFirst[0] - Axis.Pen[0].Offset_TopCamera_Y;
            TeachAction.GO_Start(setx, sety);

            if (Config.ZoomLens == 1)
            {
                ZoomLens.MODBUS_ReadStatus();
                Task.Run(() =>
                {
                    do
                    {
                        Thread.Sleep(5);
                    } while (Axis.ZoomLens_S.ReadStatus_Busy);

                    if (Axis.ZoomLens_S.ReadStatus_Online)
                    {
                        ZoomLens.SetPos = ZoomLens.OriginPos - Axis.ZoomLens_S.Tray - ZoomLens.NowPos;
                        ZoomLens.MODBUS_DST();
                    }
                    else
                    {
                        MessageBox.Show("通讯异常");
                    }
                });
            }
        }

        private void skinButton8_Click(object sender, EventArgs e)
        {
            double setx, sety;
            setx = Axis.Group[0].Unit[0].TopCamera_X - Axis.Pen[0].Offset_TopCamera_X;
            sety = Axis.Group[0].Unit[0].TopCamera_Y - Axis.Pen[0].Offset_TopCamera_Y;
            TeachAction.GO_Start(setx, sety);

            if (Config.ZoomLens == 1)
            {
                ZoomLens.MODBUS_ReadStatus();
                Task.Run(() =>
                {
                    do
                    {
                        Thread.Sleep(5);
                    } while (Axis.ZoomLens_S.ReadStatus_Busy);

                    if (Axis.ZoomLens_S.ReadStatus_Online)
                    {
                        ZoomLens.SetPos = ZoomLens.OriginPos - Axis.ZoomLens_S.Socket - ZoomLens.NowPos;
                        ZoomLens.MODBUS_DST();
                    }
                    else
                    {
                        MessageBox.Show("通讯异常");
                    }
                });
            }
        }

        private void skinButton1_Click(object sender, EventArgs e)
        {
            double setx, sety;
            setx = Axis.Position_X.tubeIn - Axis.Pen[0].Offset_TopCamera_X;
            sety = Axis.Position_Y.tubeIn - Axis.Pen[0].Offset_TopCamera_Y;
            TeachAction.GO_Start(setx, sety);

            if (Config.ZoomLens == 1)
            {
                ZoomLens.MODBUS_ReadStatus();
                Task.Run(() =>
                {
                    do
                    {
                        Thread.Sleep(5);
                    } while (Axis.ZoomLens_S.ReadStatus_Busy);

                    if (Axis.ZoomLens_S.ReadStatus_Online)
                    {
                        ZoomLens.SetPos = ZoomLens.OriginPos - Axis.ZoomLens_S.TubeIn - ZoomLens.NowPos;
                        ZoomLens.MODBUS_DST();
                    }
                    else
                    {
                        MessageBox.Show("通讯异常");
                    }
                });
            }
        }

        private void skinButton2_Click(object sender, EventArgs e)
        {
            double setx, sety;
            setx = Axis.Position_X.bredeOut - Axis.Pen[0].Offset_TopCamera_X;
            sety = Axis.Position_Y.bredeOut - Axis.Pen[0].Offset_TopCamera_Y;
            TeachAction.GO_Start(setx, sety);

            if (Config.ZoomLens == 1)
            {
                ZoomLens.MODBUS_ReadStatus();
                Task.Run(() =>
                {
                    do
                    {
                        Thread.Sleep(5);
                    } while (Axis.ZoomLens_S.ReadStatus_Busy);

                    if (Axis.ZoomLens_S.ReadStatus_Online)
                    {
                        ZoomLens.SetPos = ZoomLens.OriginPos - Axis.ZoomLens_S.BredeOut - ZoomLens.NowPos;
                        ZoomLens.MODBUS_DST();
                    }
                    else
                    {
                        MessageBox.Show("通讯异常");
                    }
                });
            }
        }

        private void skinButton3_Click(object sender, EventArgs e)
        {
            double setx, sety;
            setx = Axis.Position_X.Feeder[0] - Axis.Pen[0].Offset_TopCamera_X;
            sety = Axis.Position_Y.Feeder[0] - Axis.Pen[0].Offset_TopCamera_Y;
            TeachAction.GO_Start(setx, sety);

            if (Config.ZoomLens == 1)
            {
                ZoomLens.MODBUS_ReadStatus();
                Task.Run(() =>
                {
                    do
                    {
                        Thread.Sleep(5);
                    } while (Axis.ZoomLens_S.ReadStatus_Busy);

                    if (Axis.ZoomLens_S.ReadStatus_Online)
                    {
                        ZoomLens.SetPos = ZoomLens.OriginPos - Axis.ZoomLens_S.BredeIn - ZoomLens.NowPos;
                        ZoomLens.MODBUS_DST();
                    }
                    else
                    {
                        MessageBox.Show("通讯异常");
                    }
                });
            }
        }

        private void DevPara_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void savePanPutBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
