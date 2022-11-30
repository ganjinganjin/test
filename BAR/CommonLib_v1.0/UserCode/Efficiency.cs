using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using BAR.Commonlib;

namespace BAR
{
    public class Efficiency
    {
        private const double ratio = 3600d * 1000d;
        public static Stopwatch stopWatch = new Stopwatch();
        private static int step;
        public static int value;
        public static int freshCount;

        public static void Algorithm_Start()
        {
            step = 1;
        }

        public static void Algorithm()
        {
            if (Config.Efficiency == 0)//理论效率
            {
                Algorithm_theory();
            }
            else
            {
                Algorithm_Reality();
            }
        }

        /// <summary>
        /// 理论效率
        /// </summary>
        public static void Algorithm_theory()
        {
            if(!Auto_Flag.ALarmPause && !Auto_Flag.RunPause)
            {
                if (Run.PauseStopWatch.IsRunning)
                {
                    Run.PauseStopWatch.Stop();
                }
                if (!stopWatch.IsRunning)
                {
                    stopWatch.Start();
                }
            }
            else
            {
                if (!Run.PauseStopWatch.IsRunning)
                {
                    Run.PauseStopWatch.Start();
                }
                if (stopWatch.IsRunning)
                {
                    stopWatch.Stop();
                }
            }
            if (Auto_Flag.Ending && step != 0)
            {
                step = 0;
            }

            switch (step)
            {
                case 1:
                    step = 2;
                    break;

                case 2:
                    if (Run.ARP_Step == 70 && (UserTask.RunState == RUNSTATE.Carrier_LayOKIC ||
                        (UserTask.RunState == RUNSTATE.Carrier_LayNGIC && Axis.Pen[UserTask.LIC_PenN].DownResult == 2)))//将烧录完成的IC放入载体
                    {
                        step = 3;//进入预备状态
                    }
                    break;

                case 3:
                    if (Run.ARP_Step == 70 && UserTask.RunState == RUNSTATE.Carrier_TakeIC)
                    {
                        Auto_Flag.Seat_TIC_Cycle = false;
                        SyncPen_Struct.Seat_TIC_Cycle = false;
                        freshCount = 0;
                        stopWatch.Restart();
                        step = 4;//进入工作状态
                    }
                    break;

                case 4:
                    if (Run.ARP_Step == 6)
                    {
                        if (!Auto_Flag.BurnMode)//转移
                        {
                            if (UserTask.RunState == RUNSTATE.Carrier_LayOKIC)//将烧录完成的IC放入载体
                            {
                                step = 5;
                            }
                        }
                        else
                        {
                            if (UserTask.RunState == RUNSTATE.BurnSeat_TakeIC)
                            {
                                if (Auto_Flag.Seat_TIC_Cycle)
                                {                      
                                    Auto_Flag.Seat_TIC_Cycle = false;
                                    step = 5;
                                }
                            }
                        }
                    }
                    break;

                case 5:
                    if (Run.ARP_Step == 70 && UserTask.RunState == RUNSTATE.Carrier_TakeIC)
                    {
                        value = (ushort)(ratio * freshCount / stopWatch.ElapsedMilliseconds);
                        freshCount = 0;
                        stopWatch.Restart();
                        step = 4;//进入工作状态
                    }
                    break;

                default:
                    step = 0;
                    break;
            }
        }

        /// <summary>
        /// 实际效率
        /// </summary>
        public static void Algorithm_Reality()
        {
            if (!Auto_Flag.ALarmPause && !Auto_Flag.RunPause)
            {
                if (Run.PauseStopWatch.IsRunning)
                {
                    Run.PauseStopWatch.Stop();
                }
            }
            else
            {
                if (!Run.PauseStopWatch.IsRunning)
                {
                    Run.PauseStopWatch.Start();
                }
            }
            if (Auto_Flag.Ending && step != 0)
            {
                step = 0;
            }

            switch (step)
            {
                case 1:
                    step = 2;
                    break;

                case 2:
                    if (Run.ARP_Step == 70 && (UserTask.RunState == RUNSTATE.Carrier_LayOKIC ||
                        (UserTask.RunState == RUNSTATE.Carrier_LayNGIC && Axis.Pen[UserTask.LIC_PenN].DownResult == 2)))//将烧录完成的IC放入载体
                    {
                        step = 3;//进入预备状态
                    }
                    break;

                case 3:
                    if (Run.ARP_Step == 70 && UserTask.RunState == RUNSTATE.Carrier_TakeIC)
                    {
                        Auto_Flag.Seat_TIC_Cycle = false;
                        SyncPen_Struct.Seat_TIC_Cycle = false;
                        //freshCount = 0;
                        //stopWatch.Restart();
                        step = 4;//进入工作状态
                    }
                    break;

                case 4:
                    if (Run.ARP_Step == 6)
                    {
                        if (!Auto_Flag.BurnMode)//转移
                        {
                            if (UserTask.RunState == RUNSTATE.Carrier_LayOKIC)//将烧录完成的IC放入载体
                            {
                                step = 5;
                            }
                        }
                        else
                        {
                            if (UserTask.RunState == RUNSTATE.BurnSeat_TakeIC)
                            {
                                if (Auto_Flag.Seat_TIC_Cycle)
                                {
                                    Auto_Flag.Seat_TIC_Cycle = false;
                                    step = 5;
                                }
                            }
                        }
                    }
                    break;

                case 5:
                    if (Run.ARP_Step == 70 && UserTask.RunState == RUNSTATE.Carrier_TakeIC)
                    {
                        value = (ushort)(ratio * freshCount / Run.RunStopWatch.ElapsedMilliseconds);
                        //freshCount = 0;
                        //stopWatch.Restart();
                        step = 4;//进入工作状态
                    }
                    break;

                default:
                    step = 0;
                    break;
            }
        }
    }
}
