using BAR.Commonlib;
using BAR.Commonlib.Events;
using BAR.CommonLib_v1._0;
using BAR.Windows;
using CCWin.SkinControl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BAR
{
    public class TrayState
    {
        public delegate void TrayColorEventHander(MsgEvent evt);
        public static event TrayColorEventHander TrayColorEvent;
        public Config g_config = Config.GetInstance();
        public Act g_act = Act.GetInstance();
        private const UInt64 delayTimer = 1000;
        private UInt64[] timer = new UInt64[3];
        private bool[] resetSuccess = new bool[3];
        private RectangleF[,] _TrayRects;
        public static SkinPanel[] _RectPanels;
        public static SkinPanel _PlatePanel;
        /// <summary>
        /// 鼠标选择盘索引
        /// </summary>
        public static int selectPanIndex;
        public static int displayPanIndex;

        /// <summary>
        /// 鼠标选择方块索引
        /// </summary>
        public static int selectRectIndex;
        /// <summary>
        /// 鼠标选择行
        /// </summary>
        public static int selectRow;
        /// <summary>
        /// 鼠标选择列
        /// </summary>
        public static int selectCol;
        /// <summary>
        /// 选中过程中
        /// </summary>
        public static bool IsSelect;
        public Point oldMousePos;
        public static RectangleInfo[][] _RectangleInfos = new RectangleInfo[3][];
        Bitmap bitmap;
        Graphics graphics;
        Font font;
        LinearGradientBrush brush;

        public TrayState()
        {
            if (TrayColorEvent == null)
            {
                TrayColorEvent += OnMsgEvenHandle;
            }
        }

        public static void TrayStateUpdate(bool flag = false)
        {
            if (TrayColorEvent != null)
            {
                MsgEvent evt;
                if (flag)
                {
                    evt = new MsgEvent(MsgEvent.MSG_TRAYSIZEFLASH, null);
                }
                else
                {
                    evt = new MsgEvent(MsgEvent.MSG_TRAYCOLORFLASH, null);
                }
                Task.Run(() => TrayColorEvent(evt));
            }
        }

        private void OnMsgEvenHandle(MsgEvent e)
        {
            switch (e.MsgType)
            {
                case MsgEvent.MSG_TRAYSIZEFLASH:
                    __CreatDrawBox2Tray(3, TrayD.RowC, TrayD.ColC);
                    __GetRealPanPosData();
                    break;

                case MsgEvent.MSG_TRAYCOLORFLASH:
                    __GetRealPanPosData();
                    break;
            }

        }

        /// <summary>
        /// 画出内框
        /// </summary>
        private void __CreatDrawBox2Tray(int nTray/*盘可用数*/, int nRow/*盘行数*/, int nCol/*盘列数*/)
        {
            Config.TrayStartDir dir;
            int border_g = 10;
            int rowC, colC;//行列数
            int dirRow, dirCol;//行列方向
            float nW, nH;//方块的宽、高
            float left, top;//方块的左上角位置

            if (nRow <= 0 || nCol <= 0)
            {
                return;
            }
            for (int i = 0; i < nTray; i++)
            {
                _RectangleInfos[i] = new RectangleInfo[nRow * nCol + 1];
                for (int j = 0; j < nRow * nCol + 1; j++)
                {
                    _RectangleInfos[i][j] = new RectangleInfo();
                }
            }
            _TrayRects = new RectangleF[nTray, nRow * nCol + 1];
            for (int i = 0; i < nTray; i++)
            {
                
                if (g_config.TrayRotateDir[i] != 0)//行列是否对调
                {
                    rowC = nCol;
                    colC = nRow;
                }
                else
                {
                    rowC = nRow;
                    colC = nCol;
                }
                dir = g_config.Tray_start(i);
                dirCol = dir.dx ? 1 : 0;
                dirRow = dir.dy ? 1 : 0;
                nW = ((float)(_RectPanels[i].Width) - (border_g + colC)) / colC;
                nH = ((float)(_RectPanels[i].Height) - (border_g + rowC)) / rowC;

                for (int n = 0; n < colC; n++)
                {
                    for (int m = 0; m < rowC; m++)
                    {
                        int nIndex = g_config.Tray_Col_Add(i) ? m * colC + n : n * rowC + m;
                        _RectangleInfos[i][nIndex].Row = m + 1;
                        _RectangleInfos[i][nIndex].Col = n + 1;
                        left = Math.Abs((colC - 1) * dirCol - n) * (nW + 1);
                        top = Math.Abs((rowC - 1) * dirRow - m) * (nH + 1);
                        _TrayRects[i, nIndex] = new RectangleF(left, top, nW, nH);
                    }
                }
            }
        }

        /// <summary>
        /// 获取料盘实时状态
        /// </summary>
        private void __GetRealPanPosData()
        {
            int takeID, layID, end_takeID, allC = TrayD.RowC * TrayD.ColC;
            int[] plateStats = new int[allC];

            if (allC <= 0)
            {
                return;
            }

            for (int k = 0; k < 2; k++)
            {
                takeID = Get_RectangleF_ID(k, TrayD.TIC_RowN[k], TrayD.TIC_ColN[k]);
                layID = Get_RectangleF_ID(k, TrayD.LIC_RowN[k], TrayD.LIC_ColN[k]);
                end_takeID = Get_RectangleF_ID(k, TrayD.TIC_EndRowN[k], TrayD.TIC_EndColN[k], false);

                for (int i = 0; i < allC; i++)
                {
                    //待测料
                    plateStats[i] = 1;
                }

                if (takeID > 0)
                {
                    for (int i = 0; i < takeID; i++)
                    {
                        //空料
                        plateStats[i] = -1;
                    }
                }
                if (end_takeID < allC - 1)
                {
                    for (int i = end_takeID + 1; i < allC; i++)
                    {
                        //空料
                        plateStats[i] = -1;
                    }
                }

                if (layID > 0)
                {
                    for (int i = 0; i < layID; i++)
                    {
                        //OK料
                        plateStats[i] = 2;
                    }
                }
                DrawTrayPanel(k, plateStats);
            }
            //NG盘放置状态
            for (int i = 0; i < allC; i++)
            {
                //空料料
                plateStats[i] = -1;
            }
            layID = Get_RectangleF_ID(2, TrayD.LIC_RowN[2], TrayD.LIC_ColN[2]);
            if (layID > 0)
            {
                for (int i = 0; i < layID; i++)
                {
                    //OK料
                    plateStats[i] = 3;
                }
            }
            DrawTrayPanel(2, plateStats);
        }
        
        private void DrawTrayPanel(int nTray, int[] plateStats)
        {
            int allC = TrayD.RowC * TrayD.ColC;
            string[] strArr = new string[3] { MultiLanguage.GetString("料盘1"), MultiLanguage.GetString("料盘2"), MultiLanguage.GetString("NG盘") };
            graphics?.Dispose();
            bitmap = new Bitmap(_RectPanels[nTray].Width - 10, _RectPanels[nTray].Height - 10);
            graphics = Graphics.FromImage(bitmap);
            graphics.Clear(Color.White);
            graphics.SmoothingMode = SmoothingMode.HighQuality; //高质量
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality; //高像素偏移质量
            for (int n = 0; n < allC; n++)
            {
                brush?.Dispose();
                if (plateStats[n] == -1)
                {
                    brush = new LinearGradientBrush(_TrayRects[nTray, n], Color.Gray, Color.DarkGray, LinearGradientMode.ForwardDiagonal);
                }
                else if (plateStats[n] == 1)
                {
                    brush = new LinearGradientBrush(_TrayRects[nTray, n], Color.DarkBlue, Color.FromArgb(43, 180, 255), LinearGradientMode.ForwardDiagonal);
                }
                else if (plateStats[n] == 3)
                {
                    brush = new LinearGradientBrush(_TrayRects[nTray, n], Color.DarkRed, Color.FromArgb(255, 180, 43), LinearGradientMode.ForwardDiagonal);
                }
                else if (plateStats[n] == 2)
                {
                    brush = new LinearGradientBrush(_TrayRects[nTray, n], Color.Green, Color.FromArgb(45, 223, 83), LinearGradientMode.ForwardDiagonal);
                }
                else
                {
                    brush = new LinearGradientBrush(_TrayRects[nTray, n], Color.Green, Color.FromArgb(45, 223, 83), LinearGradientMode.ForwardDiagonal);
                }
                brush.SetSigmaBellShape(0.0f, 0.35f);
                graphics.FillRectangle(brush, _TrayRects[nTray, n]);
                if (n == 0)
                {
                    RectangleF rec = _TrayRects[nTray, n];
                    graphics.DrawLine(new Pen(Color.LightGray), rec.Location, new PointF(rec.Right, rec.Bottom));
                    graphics.DrawLine(new Pen(Color.LightGray), new PointF(rec.X, rec.Bottom), new PointF(rec.Right, rec.Y));
                }
                brush.Dispose();
            }
            font?.Dispose();
            font = new Font("楷体", 50, FontStyle.Bold, GraphicsUnit.Point);
            graphics.DrawString(strArr[nTray], font, new SolidBrush(Color.FromArgb(120, 255, 255, 255)),
                new Point((_RectPanels[nTray].Width / 2 - 100), (_RectPanels[nTray].Height / 2 - 45)));

            //graphics = _RectPanels[nTray].CreateGraphics();
            //graphics.DrawImage(bitmap, 5, 5);

            _RectPanels[nTray].BackgroundImage = bitmap;
            graphics.Dispose();
            font.Dispose();
            //bitmap.Dispose();
            GC.Collect();
        }

        /// <summary>
        /// 获取ID
        /// </summary>
        /// <param name="nTray"></param>
        /// <param name="nRow"></param>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public int Get_RectangleF_ID(int nTray, int nRow, int nCol, bool mode = true)
        {
            int rowC = TrayD.RowC;
            int colC = TrayD.ColC;

            if (nTray >= 1 && g_config.TrayRotateDir[nTray] != 0)
            {
                rowC = TrayD.ColC;
                colC = TrayD.RowC;
            }
            nRow = nRow > rowC ? rowC : nRow;
            nCol = nCol > colC ? colC : nCol;
            if (nRow == 0 || nCol == 0)
            {
                if (mode)
                {
                    return rowC * colC;
                }
                else
                {
                    nRow = 1;
                    nCol = 1;
                }
            }
            nRow--;
            nCol--;
            return g_config.Tray_Col_Add(nTray) ? nRow * colC + nCol : nCol * rowC + nRow;
        }

        public void TrayState_Handle()
        {
            if (!In_Output.tray_Sig[0].M)//取托盘1信号处理
            {
                if (!In_Output.tray_Sig[0].FM)
                {
                    In_Output.tray_Sig[0].FM = true;
                    timer[0] = UserTimer.GetSysTime() + delayTimer;
                    resetSuccess[0] = false;
                }
            }
            else
            {
                In_Output.tray_Sig[0].FM = false;
            }

            if (In_Output.tray_Sig[0].FM && (UserTimer.GetSysTime() > timer[0]) && !resetSuccess[0])
            {
                if (Auto_Flag.FixedTray_TakeIC && Auto_Flag.FixedTray_LayIC)
                {
                    //取料盘号为0或1或取料盘号为2且还没开始取或取料结束
                    if (TrayD.TIC_TrayN == 0 || TrayD.TIC_TrayN == 1 ||
                        (TrayD.TIC_TrayN == 2 && TrayD.TIC_ColN[1] == 1 && TrayD.TIC_RowN[1] == 1))
                    {
                        //放料盘号为0或1或放料盘号为2且还没开始放
                        if (TrayD.LIC_TrayN == 0 || TrayD.LIC_TrayN == 1 ||
                            (TrayD.LIC_TrayN == 2 && TrayD.LIC_ColN[1] == 1 && TrayD.LIC_RowN[1] == 1 &&
                            ((TrayD.TIC_TrayN == 2 && TrayD.TIC_ColN[1] == 1 && TrayD.TIC_RowN[1] == 1) || TrayEndFlag.takeIC[1])))
                        {
                            TrayD.LIC_TrayN = 1;   //放料盘号
                        }
                        TrayD.TIC_TrayN = 1;  //取料盘号
                    }
                    else
                    {
                        //放料盘号为0或1
                        if ((TrayD.LIC_TrayN == 0) || (TrayD.LIC_TrayN == 1))
                        {
                            TrayD.LIC_TrayN = 2;   //放料盘号
                        }
                    }
                }
                else if (Auto_Flag.FixedTray_TakeIC)
                {
                    if (TrayD.TIC_TrayN == 0 || TrayD.TIC_TrayN == 1 ||
                        (TrayD.TIC_TrayN == 2 && TrayD.TIC_ColN[1] == 1 && TrayD.TIC_RowN[1] == 1))
                    {
                        TrayD.TIC_TrayN = 1;  //取料盘号
                    }
                }
                else if (Auto_Flag.FixedTray_LayIC)
                {
                    //放料盘号为0或1或放料盘号为2且还没开始放
                    if (TrayD.LIC_TrayN == 0 || TrayD.LIC_TrayN == 1 ||
                        (TrayD.LIC_TrayN == 2 && TrayD.LIC_ColN[1] == 1 && TrayD.LIC_RowN[1] == 1))
                    {
                        TrayD.LIC_TrayN = 1;   //放料盘号
                    }
                }
                else if (Auto_Flag.AutoTray_TakeIC && Auto_Flag.AutoTray_LayIC)//自动盘，盘转盘模式
                {
                    //取料盘号为0
                    if (TrayD.TIC_TrayN == 0)
                    {
                        TrayD.TIC_TrayN = 1;   //取料盘号
                    }
                }

                if (TrayEndFlag.tray2Burn && Auto_Flag.AutoTray_TakeIC && (AutoTray.StatusAlarmWord & AutoTray.Status_OverplusTrayInit) != 0)
                {
                    TrayEndFlag.takeIC[0] = true;
                    TrayEndFlag.takeLayIC[0] = true;
                    TrayD.TIC_ColN[0] = 0;
                    TrayD.TIC_RowN[0] = 0;
                }
                else
                {
                    TrayEndFlag.takeIC[0] = false;
                    TrayEndFlag.takeLayIC[0] = false;
                    TrayD.TIC_ColN[0] = 1;
                    TrayD.TIC_RowN[0] = 1;
                }
                TrayEndFlag.layIC[0] = false;
                TrayD.LIC_ColN[0] = 1;
                TrayD.LIC_RowN[0] = 1;
                TrayD.TIC_EndColN[0] = TrayD.ColC;
                TrayD.TIC_EndRowN[0] = TrayD.RowC;
                resetSuccess[0] = true;
                TrayStateUpdate();
            }

            if (!In_Output.tray_Sig[1].M)//取托盘2信号处理
            {
                if (!In_Output.tray_Sig[1].FM)
                {
                    In_Output.tray_Sig[1].FM = true;
                    timer[1] = UserTimer.GetSysTime() + delayTimer;
                    resetSuccess[1] = false;
                }
            }
            else
            {
                In_Output.tray_Sig[1].FM = false;
            }

            if (In_Output.tray_Sig[1].FM && (UserTimer.GetSysTime() > timer[1]) && !resetSuccess[1])
            {
                if (Auto_Flag.FixedTray_TakeIC && Auto_Flag.FixedTray_LayIC)
                {
                    //取料盘号为0或2
                    if ((TrayD.TIC_TrayN == 0) || (TrayD.TIC_TrayN == 2))
                    {
                        //1号盘取料结束
                        if (TrayEndFlag.takeIC[0])
                        {
                            TrayD.TIC_TrayN = 2;  //取料盘号
                                                        //放料盘号为0或2或放料盘号为1且还没开始放
                            if (((TrayD.LIC_TrayN == 0) || (TrayD.LIC_TrayN == 2)) ||
                                ((TrayD.LIC_TrayN == 1) && (TrayD.LIC_ColN[0] == 1) && (TrayD.LIC_RowN[0] == 1)))
                            {
                                TrayD.LIC_TrayN = 2;   //放料盘号
                            }
                        }
                        else
                        {
                            TrayD.TIC_TrayN = 1;  //取料盘号
                            TrayD.LIC_TrayN = 1;   //放料盘号
                        }
                    }
                    else
                    {
                        //放料盘号为0或2
                        if ((TrayD.LIC_TrayN == 0) || (TrayD.LIC_TrayN == 2))
                        {
                            TrayD.LIC_TrayN = 1;   //放料盘号
                        }
                    }
                }
                else if (Auto_Flag.FixedTray_TakeIC)
                {
                    if ((TrayD.TIC_TrayN == 0) || (TrayD.TIC_TrayN == 2))
                    {
                        //1号盘取料结束
                        if (TrayEndFlag.takeIC[0])
                        {
                            TrayD.TIC_TrayN = 2;  //取料盘号
                        }
                        else
                        {
                            TrayD.TIC_TrayN = 1;  //取料盘号
                        }
                    }
                }
                else if (Auto_Flag.FixedTray_LayIC)
                {
                    //放料盘号为0或2
                    if ((TrayD.LIC_TrayN == 0) || (TrayD.LIC_TrayN == 2))
                    {
                        //1号盘放料结束
                        if (TrayEndFlag.layIC[0])
                        {
                            TrayD.LIC_TrayN = 2;   //放料盘号
                        }
                        else
                        {
                            TrayD.LIC_TrayN = 1;   //放料盘号
                        }
                    }
                }
                if (Auto_Flag.FixedTray_TakeIC || Auto_Flag.FixedTray_LayIC)
                {
                    TrayEndFlag.takeIC[1] = false;
                    TrayEndFlag.layIC[1] = false;
                    TrayEndFlag.takeLayIC[1] = false;
                    TrayD.TIC_ColN[1] = 1;
                    TrayD.TIC_RowN[1] = 1;
                    TrayD.LIC_ColN[1] = 1;
                    TrayD.LIC_RowN[1] = 1;
                }
                TrayD.TIC_EndColN[1] = g_config.TrayRotateDir[1] != 0 ? TrayD.RowC : TrayD.ColC;
                TrayD.TIC_EndRowN[1] = g_config.TrayRotateDir[1] != 0 ? TrayD.ColC : TrayD.RowC;
                resetSuccess[1] = true;
                TrayStateUpdate();
            }

            if (!In_Output.tray_Sig[2].M)//取托盘3信号处理
            {
                if (!In_Output.tray_Sig[2].FM)
                {
                    In_Output.tray_Sig[2].FM = true;
                    timer[2] = UserTimer.GetSysTime() + delayTimer;
                    resetSuccess[2] = false;
                }
            }
            else
            {
                In_Output.tray_Sig[2].FM = false;
            }

            if (In_Output.tray_Sig[2].FM && (UserTimer.GetSysTime() > timer[2]) && !resetSuccess[2])
            {
                TrayEndFlag.layIC[2] = false;
                TrayD.LIC_ColN[2] = 1;
                TrayD.LIC_RowN[2] = 1;
                resetSuccess[2] = true;
                TrayStateUpdate();
            }
        }

        public void LED()
        {
            if (In_Output.tray_Sig[0].M && TrayEndFlag.takeLayIC[0] && (Auto_Flag.FixedTray_LayIC || Auto_Flag.FixedTray_TakeIC ||
               (Auto_Flag.AutoTray_TakeIC && Auto_Flag.AutoTray_LayIC)))
            {
                In_Output.tray_LED[0].M = true;
            }
            else
            {
                In_Output.tray_LED[0].M = false;
            }

            if (In_Output.tray_Sig[1].M && TrayEndFlag.takeLayIC[1] && (Auto_Flag.FixedTray_LayIC || Auto_Flag.FixedTray_TakeIC ||
                (Auto_Flag.AutoTray_TakeIC && Auto_Flag.AutoTray_LayIC)))
            {
                In_Output.tray_LED[1].M = true;
            }
            else
            {
                In_Output.tray_LED[1].M = false;
            }

            if (In_Output.tray_Sig[2].M && TrayEndFlag.layIC[2])
            {
                In_Output.tray_LED[2].M = true;
            }
            else
            {
                In_Output.tray_LED[2].M = false;
            }
        }

        /// <summary>
        /// 鼠标右键按下
        /// </summary>
        public void RightMouseDown()
        {
            if (IsSelect)
            {
                return;
            }
            if (Auto_Flag.AutoRunBusy)
            {
                g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "设备运行中,不允许修改", "Flow");
                return;
            }
            MouseButtons mb = Control.MouseButtons;
            //  获取鼠标动作（按下了 那个按键)
            if (mb == MouseButtons.Right)
            {
                //IsSelect = true;

                Point temp = Control.MousePosition;
                //判断点击位置是否在料盘中
                if (_RectPanels[0].Region.IsVisible(_RectPanels[0].PointToClient(temp)))
                {
                    selectPanIndex = 0;
                }
                else if (_RectPanels[1].Region.IsVisible(_RectPanels[1].PointToClient(temp)))
                {
                    selectPanIndex = 1;
                }
                else if (_RectPanels[2].Region.IsVisible(_RectPanels[2].PointToClient(temp)))
                {
                    selectPanIndex = 2;
                }
                else
                {
                    IsSelect = false;
                    return;
                }
                var ClientPoint = _RectPanels[selectPanIndex].PointToClient(new Point(temp.X - 4, temp.Y - 4));//鼠标点击位置在料盘Panel中的坐标
                for (int i = 0; i < TrayD.RowC * TrayD.ColC + 1; i++)
                {
                    if (_TrayRects[selectPanIndex, i].Contains(ClientPoint))
                    {
                        selectRectIndex = i;
                        selectRow = _RectangleInfos[selectPanIndex][i].Row;
                        selectCol = _RectangleInfos[selectPanIndex][i].Col;
                        IsSelect = true;
                        BAR._ModifyICPosWnd.__InitUI();
                        BAR._ModifyICPosWnd.Show();
                        break;
                    }
                }
                
            }
        }

        /// <summary>
        /// 实时显示当前选中行列
        /// </summary>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public bool DisplayRowAndCol(out string strMsg)
        {
            if (Control.MousePosition == oldMousePos || !Config.IsLoaded)
            {
                strMsg = null;
                return false;
            }
            oldMousePos = Control.MousePosition;
            //判断点击位置是否在料盘中
            if (_RectPanels[0].Region.IsVisible(_RectPanels[0].PointToClient(Control.MousePosition)))
            {
                displayPanIndex = 0;
            }
            else if (_RectPanels[1].Region.IsVisible(_RectPanels[1].PointToClient(Control.MousePosition)))
            {
                displayPanIndex = 1;
            }
            else if (_RectPanels[2].Region.IsVisible(_RectPanels[2].PointToClient(Control.MousePosition)))
            {
                displayPanIndex = 2;
            }
            else
            {
                strMsg = null;
                return false;
            }
            var ClientPoint = _RectPanels[displayPanIndex].PointToClient(new Point(Control.MousePosition.X - 4, Control.MousePosition.Y - 4));//鼠标点击位置在料盘Panel中的坐标
            string str = "----";
            for (int i = 0; i < TrayD.RowC * TrayD.ColC + 1; i++)
            {
                if (_TrayRects[displayPanIndex, i].Contains(ClientPoint))
                {
                    if (MultiLanguage.IsEnglish())
                    {
                        str = string.Format("{0:d}row {1:d}Column\r(Right click Setting)", _RectangleInfos[displayPanIndex][i].Row, _RectangleInfos[displayPanIndex][i].Col);
                    }
                    else
                    {
                        str = string.Format("{0:d}行 {1:d}列\r(右键设置)", _RectangleInfos[displayPanIndex][i].Row, _RectangleInfos[displayPanIndex][i].Col);
                    }
                    
                    break;
                }
            }
            strMsg = str;
            return true;
        }

        /// <summary>
        /// 校验设置行列数
        /// </summary>
        /// <param name="CheckType">0：取料行列，1：放料行列，2：结束行列</param>
        public bool Check_RectangleF_ID(int CheckType)
        {
            string strMsg = "修改料盘取放料位置失败";
            int takeID = 0, layID, endID = 0;
            if (selectPanIndex == 2)
            {
                layID = Get_RectangleF_ID(selectPanIndex, TrayD.LIC_RowN[selectPanIndex], TrayD.LIC_ColN[selectPanIndex]);
            }
            else
            {
                takeID = Get_RectangleF_ID(selectPanIndex, TrayD.TIC_RowN[selectPanIndex], TrayD.TIC_ColN[selectPanIndex]);
                layID = Get_RectangleF_ID(selectPanIndex, TrayD.LIC_RowN[selectPanIndex], TrayD.LIC_ColN[selectPanIndex]);
                endID = Get_RectangleF_ID(selectPanIndex, TrayD.TIC_EndRowN[selectPanIndex], TrayD.TIC_EndColN[selectPanIndex]);
            }
            if (CheckType == 0 && selectPanIndex != 2)//取料行列
            {
                if (selectRectIndex < layID)
                {
                    TrayD.LIC_RowN[selectPanIndex] = _RectangleInfos[selectPanIndex][selectRectIndex].Row;
                    TrayD.LIC_ColN[selectPanIndex] = _RectangleInfos[selectPanIndex][selectRectIndex].Col;
                }
                if (selectRectIndex > endID)
                {
                    TrayD.TIC_EndRowN[selectPanIndex] = _RectangleInfos[selectPanIndex][TrayD.RowC * TrayD.ColC - 1].Row;
                    TrayD.TIC_EndColN[selectPanIndex] = _RectangleInfos[selectPanIndex][TrayD.RowC * TrayD.ColC - 1].Col;
                }
                TrayD.TIC_RowN[selectPanIndex] = selectRow;
                TrayD.TIC_ColN[selectPanIndex] = selectCol;
                if (MultiLanguage.IsEnglish())
                {
                    strMsg = string.Format("Modify the take position of tray {0:d} to {1:d} row and {2:d} columns", TrayState.selectPanIndex + 1, TrayState.selectRow, TrayState.selectCol);
                }
                else
                {
                    strMsg = string.Format("成功修改料盘[{0:d}]取料位置为[{1:d}]行[{2:d}]列", TrayState.selectPanIndex + 1, TrayState.selectRow, TrayState.selectCol);
                }
                    
            }
            else if(CheckType == 1)//放料行列
            {
                if (selectRectIndex > takeID && selectPanIndex != 2)
                {
                    TrayD.TIC_RowN[selectPanIndex] = _RectangleInfos[selectPanIndex][selectRectIndex].Row;
                    TrayD.TIC_ColN[selectPanIndex] = _RectangleInfos[selectPanIndex][selectRectIndex].Col;
                }
                if (selectRectIndex > endID && selectPanIndex != 2)
                {
                    TrayD.TIC_EndRowN[selectPanIndex] = _RectangleInfos[selectPanIndex][TrayD.RowC * TrayD.ColC - 1].Row;
                    TrayD.TIC_EndColN[selectPanIndex] = _RectangleInfos[selectPanIndex][TrayD.RowC * TrayD.ColC - 1].Col;
                }
                TrayD.LIC_RowN[selectPanIndex] = selectRow;
                TrayD.LIC_ColN[selectPanIndex] = selectCol;
                if (MultiLanguage.IsEnglish())
                {
                    strMsg = string.Format("Modify the lay position of tray {0:d} to {1:d} row and {2:d} columns", TrayState.selectPanIndex + 1, TrayState.selectRow, TrayState.selectCol);
                }
                else
                {
                    strMsg = string.Format("成功修改料盘{0:d}放料位置为{1:d}行{2:d}列", TrayState.selectPanIndex + 1, TrayState.selectRow, TrayState.selectCol);
                }
                
            }
            else if (CheckType == 2 && selectPanIndex != 2)//结束行列
            {
                if (selectRectIndex < takeID)
                {
                    TrayD.TIC_RowN[selectPanIndex] = _RectangleInfos[selectPanIndex][selectRectIndex].Row;
                    TrayD.TIC_ColN[selectPanIndex] = _RectangleInfos[selectPanIndex][selectRectIndex].Col;
                }
                if (selectRectIndex < layID)
                {
                    TrayD.LIC_RowN[selectPanIndex] = _RectangleInfos[selectPanIndex][selectRectIndex].Row;
                    TrayD.LIC_ColN[selectPanIndex] = _RectangleInfos[selectPanIndex][selectRectIndex].Col;
                }
                TrayD.TIC_EndRowN[selectPanIndex] = selectRow;
                TrayD.TIC_EndColN[selectPanIndex] = selectCol;
                if (MultiLanguage.IsEnglish())
                {
                    strMsg = string.Format("Modify the end position of tray {0:d} to {1:d} row and {2:d} columns", TrayState.selectPanIndex + 1, TrayState.selectRow, TrayState.selectCol);
                }
                else
                {
                    strMsg = string.Format("成功修改料盘[{0:d}]结束位置为[{1:d}]行[{2:d}]列", TrayState.selectPanIndex + 1, TrayState.selectRow, TrayState.selectCol);
                }
                
            }
            g_config.WriteStaticVal();
            TrayStateUpdate();
            g_act.GenLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, strMsg, "Flow");
            return true;
        }
    }
}
