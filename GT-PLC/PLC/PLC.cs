using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PLC
{ 
    public class PLC1
    {
        public static CardPrm[] card = new CardPrm[2];
        public PLC1()
        {
            if (card[0] == null)
            {
                card[0] = new CardPrm();
                card[1] = new CardPrm();
                card[0].index = 0;  
                card[0].axisCount = 8;
                card[0].GPIOCount = 16;

                card[1].index = 1;   
                card[1].axisCount = 4;
                card[1].GPIOCount = 16;
            }
        }
        /// <summary>
        /// 获取主卡输入口
        /// </summary>
        /// <param name="reverse"></param>
        /// <param name="diType"></param>
        /// <param name="GPIO"></param>
        /// <param name="leng"></param>
        public void GetDInput(int cardType, short cardNum, short diType, short reverse, GPIO[] GPIO, int leng)
        {
            int pValue;
            if (cardType == 0)
            {
                Gts.GT_GetDiRaw(cardNum, diType, out pValue);
            }
            else
            {
                lhmtc.LH_GetDiRaw(diType, out pValue, cardNum);
            }
            
            for (int i = 0; i < leng; i++)
            {
                if ((pValue >> i & 0x1) == reverse)
                {
                    GPIO[i].M = true;
                }
                else
                {
                    GPIO[i].M = false;
                }
            }
        }

        /// <summary>
        /// 获取扩展输入口
        /// </summary>
        /// <param name="mdl"></param>
        /// <param name="reverse"></param>
        /// <param name="GPIO"></param>
        /// <param name="leng"></param>
        public void GetExtInput(int cardType, short cardNum, short mdl, short reverse, GPIO[] GPIO, int leng)
        {
            int pValue;
            if (cardType == 0)
            {
                Gts.GT_GetExtIoValue(cardNum, mdl, out ushort buffer);
                pValue = buffer;
            }
            else
            {
                lhmtc.LH_GetExtendDi(mdl, out pValue, cardNum);
            }
            
            for (int i = 0; i < leng; i++)
            {
                if ((pValue >> i & 0x1) == reverse)
                {
                    GPIO[i].M = true;
                }
                else
                {
                    GPIO[i].M = false;
                }
            }
        }

        public void GetOutput(int cardType, short cardNum, short mdl, short reverse, GPIO[] GPIO, int leng)
        {
            int pValue;
            if (cardType == 0)
            {
                Gts.GT_GetDo(cardNum, mdl, out pValue);
            }
            else
            {
                lhmtc.LH_GetDo(mdl, out pValue, cardNum);
            }
            
            for (int i = 0; i < leng; i++)
            {
                if ((pValue >> i & 0x1) == reverse)
                {
                    GPIO[i].M = true;
                }
                else
                {
                    GPIO[i].M = false;
                }
            }
        }
        /// <summary>
        /// 智能回原点
        /// </summary>
        /// <param name="Card"></param>卡号
        /// <param name="Axis"></param>轴号
        /// <param name="m"></param>驱动状态
        /// <param name="postion"></param>原点搜索距离
        /// <param name="speed"></param>回原点速度
        /// <param name="add_speed"></param>加速度
        public bool SmartHome(int cardType, LimitHomePrm homePrm)
        {
            bool ret;
            if (cardType == 0)
            {
                Gts.GT_GetSts(homePrm.cardPrm.cardNum, homePrm.index, out int pSts, 1, out uint pClock);
                if ((pSts >> 10 & 0x1) == 0)
                {
                    Gts.GT_GetDi(homePrm.cardPrm.cardNum, 1, out int Input);
                    if ((Input >> (homePrm.index - 1) & 0x1) == 1)
                    {
                        Gts.GT_ClrSts(homePrm.cardPrm.cardNum, homePrm.index, 1);
                        Gts.GT_LmtsOff(homePrm.cardPrm.cardNum, homePrm.index, 1);//负限位无效
                        JogPrm jogPrm = new JogPrm
                        {
                            cardPrm = homePrm.cardPrm,
                            index = homePrm.index,
                            pulFactor = homePrm.pulFactor,
                            acc = homePrm.acc,
                            dec = homePrm.dec
                        };
                        JOG(cardType, jogPrm, homePrm.velHigh);
                        do
                        {
                            Gts.GT_GetDi(homePrm.cardPrm.cardNum, 1, out Input);
                        } while ((Input >> (homePrm.index - 1) & 0x1) == 1);

                        Gts.GT_SetStopDec(homePrm.cardPrm.cardNum, homePrm.index, 1, 1);
                        Gts.GT_Stop(homePrm.cardPrm.cardNum, 1 << (homePrm.index - 1), 1);
                        Gts.GT_ClrSts(homePrm.cardPrm.cardNum, homePrm.index, 1);
                        Gts.GT_ZeroPos(homePrm.cardPrm.cardNum, homePrm.index, 1);

                        do
                        {
                            Gts.GT_GetSts(homePrm.cardPrm.cardNum, homePrm.index, out pSts, 1, out pClock);
                        } while ((pSts >> 10 & 0x1) == 1);

                        TrapPrm trapPrm = new TrapPrm
                        {
                            cardPrm = homePrm.cardPrm,
                            index = homePrm.index,
                            pulFactor = homePrm.pulFactor,
                            speed = homePrm.velLow,
                            setPosPul = (int)(homePrm.escapeStep / homePrm.pulFactor),
                            acc = homePrm.acc,
                            dec = homePrm.dec,
                            smoothTime = homePrm.smoothTime,
                            velStart = 0
                        };

                        UpdateTrap(cardType, trapPrm);
                        do
                        {
                            Gts.GT_GetSts(homePrm.cardPrm.cardNum, homePrm.index, out pSts, 1, out pClock);
                        } while ((pSts >> 10 & 0x1) == 1);
                    }

                    Gts.THomeStatus tHomeSts;

                    Gts.GT_AlarmOff(homePrm.cardPrm.cardNum, homePrm.index);
                    Gts.GT_LmtsOn(homePrm.cardPrm.cardNum, homePrm.index, 1);//负限位有效
                                                                             //Gts.GT_LmtSns(homePrm.cardNum, 0xFFFF); //限位为低电平触发（按实际情况设置）
                    Gts.GT_EncSns(homePrm.cardPrm.cardNum, 1); //编码器方向设置（按实际编码器设置，保证规划位置与实际位置方向一致）
                    Gts.GT_ClrSts(homePrm.cardPrm.cardNum, homePrm.index, 1);
                    //Gts.GT_ZeroPos(homePrm.cardPrm.cardNum, homePrm.index, 1);
                    //设置 Smart Home 回原点参数
                    Gts.GT_GetHomePrm(homePrm.cardPrm.cardNum, homePrm.index, out Gts.THomePrm tHomePrm);
                    tHomePrm.mode = Gts.HOME_MODE_LIMIT;//限位回原点模式
                    tHomePrm.moveDir = homePrm.moveDir;// 设置启动搜索原点时的运动方向（如果回原点运动包含搜索 Limit 则为搜索 Limit 的运动方向）：-1-负方向， 1-正方向
                    tHomePrm.indexDir = homePrm.indexDir;// 设置搜索 Index 的运动方向： -1-负方向，1-正方向，在限位+Index 回原点模式下 moveDir 与indexDir 应该相异
                    tHomePrm.edge = homePrm.edge;// 设置捕获沿： 0-下降沿， 1-上升沿
                    tHomePrm.velHigh = homePrm.velHigh / (homePrm.pulFactor * 1000);// 回原点运动的高速速度（单位： pulse/ms）
                    tHomePrm.velLow = homePrm.velLow / (homePrm.pulFactor * 1000);// 回原点运动的低速速度（单位： pulse/ms）
                    tHomePrm.acc = homePrm.acc / (homePrm.pulFactor * 1000);// 回原点运动的加速度（单位： pulse/ms^2）
                    tHomePrm.dec = homePrm.dec / (homePrm.pulFactor * 1000);// 回原点运动的减速度（单位： pulse/ms^2）
                    tHomePrm.smoothTime = homePrm.smoothTime;//回原点运动的平滑时间：取值[0,50]
                    tHomePrm.pad2_1 = 0;
                    tHomePrm.homeOffset = (int)(homePrm.homeOffset / homePrm.pulFactor); // 最终停止的位置相对于原点的偏移量
                    tHomePrm.searchHomeDistance = (int)(homePrm.searchHomeDistance / homePrm.pulFactor);// 设定的搜索 Home 的搜索范围， 0 表示搜索距离为 805306368
                    tHomePrm.searchIndexDistance = (int)(homePrm.searchIndexDistance / homePrm.pulFactor);// 设定的搜索 Index 的搜索范围， 0 表示搜索距离为 805306368
                    tHomePrm.escapeStep = (int)(homePrm.escapeStep / homePrm.pulFactor);// 采用“限位回原点” 方式时，反方向离开限位的脱离步长
                    Gts.GT_GoHome(homePrm.cardPrm.cardNum, homePrm.index, ref tHomePrm); //启动 Smart Home 回原点
                    do
                    {
                        Gts.GT_GetHomeStatus(homePrm.cardPrm.cardNum, homePrm.index, out tHomeSts); //获取回原点状态
                    } while (tHomeSts.run == 1); //等待搜索原点停止
                    ret = tHomeSts.error == 0;
                    Thread.Sleep(500);
                    Gts.GT_ZeroPos(homePrm.cardPrm.cardNum, homePrm.index, 1);
                    Gts.GT_ClrSts(homePrm.cardPrm.cardNum, homePrm.index, 1);//清楚报警标志
                    if (ret && homePrm.strMessage.Substring(0, 1) == "Z")
                    {
                        Gts.GT_LmtsOff(homePrm.cardPrm.cardNum, homePrm.index, 1);//负限位无效
                        TrapPrm trapPrm = new TrapPrm
                        {
                            cardPrm = homePrm.cardPrm,
                            index = homePrm.index,
                            pulFactor = homePrm.pulFactor,
                            speed = homePrm.velHigh,
                            setPosPul = (int)(-7 / homePrm.pulFactor),
                            acc = homePrm.acc,
                            dec = homePrm.dec,
                            smoothTime = homePrm.smoothTime,
                            velStart = 0
                        };
                        UpdateTrap(cardType, trapPrm);
                        do
                        {
                            Gts.GT_GetSts(homePrm.cardPrm.cardNum, homePrm.index, out pSts, 1, out pClock);
                        } while ((pSts >> 10 & 0x1) == 1);
                    }
                    return ret;
                }
            }
            else
            {
                lhmtc.LH_GetSts(homePrm.index, out int pSts, 1, homePrm.cardPrm.cardNum);
                if ((pSts >> 10 & 0x1) == 0)
                {
                    lhmtc.LH_GetDi(1, out int Input, homePrm.cardPrm.cardNum);
                    if ((Input >> (homePrm.index - 1) & 0x1) == 1)
                    {
                        lhmtc.LH_ClrSts(homePrm.index, 1, homePrm.cardPrm.cardNum);
                        lhmtc.LH_LmtsOff(homePrm.index, 1, homePrm.cardPrm.cardNum);//负限位无效
                        JogPrm jogPrm = new JogPrm
                        {
                            cardPrm = homePrm.cardPrm,
                            index = homePrm.index,
                            pulFactor = homePrm.pulFactor,
                            acc = homePrm.acc,
                            dec = homePrm.dec
                        };
                        JOG(cardType, jogPrm, homePrm.velHigh);
                        do
                        {
                            lhmtc.LH_GetDi(1, out Input, homePrm.cardPrm.cardNum);
                        } while ((Input >> (homePrm.index - 1) & 0x1) == 1);
                        lhmtc.LH_LmtsOn(homePrm.index, 1, homePrm.cardPrm.cardNum);//负限位有效
                        lhmtc.LH_Stop((short)(1 << (homePrm.index - 1)), 0, homePrm.cardPrm.cardNum);
                        lhmtc.LH_ClrSts(homePrm.index, 1, homePrm.cardPrm.cardNum);
                        lhmtc.LH_ZeroPos(homePrm.index, 1, homePrm.cardPrm.cardNum);
                        do
                        {
                            lhmtc.LH_GetSts(homePrm.index, out pSts, 1, homePrm.cardPrm.cardNum);
                        } while ((pSts >> 10 & 0x1) == 1);

                        TrapPrm trapPrm = new TrapPrm
                        {
                            cardPrm = homePrm.cardPrm,
                            index = homePrm.index,
                            pulFactor = homePrm.pulFactor,
                            speed = homePrm.velHigh,
                            setPosPul = (int)(homePrm.escapeStep / homePrm.pulFactor),
                            acc = homePrm.acc,
                            dec = homePrm.dec,
                            smoothTime = homePrm.smoothTime,
                            velStart = 0
                        };

                        UpdateTrap(cardType, trapPrm);
                        do
                        {
                            lhmtc.LH_GetSts(homePrm.index, out pSts, 1, homePrm.cardPrm.cardNum);
                        } while ((pSts >> 10 & 0x1) == 1);
                    }

                    ushort tHomeSts;

                    lhmtc.LH_AlarmOff(homePrm.cardPrm.cardNum, homePrm.index);
                    lhmtc.LH_LmtsOn(homePrm.cardPrm.cardNum, homePrm.index, 1);//负限位有效
                    lhmtc.LH_ClrSts(homePrm.cardPrm.cardNum, homePrm.index, 1);
                    //设置 Smart Home 回原点参数

                    lhmtc.LH_Home(homePrm.index, (int)homePrm.searchHomeDistance, homePrm.velHigh, homePrm.acc, (int)homePrm.homeOffset, homePrm.cardPrm.cardNum); //启动 Smart Home 回原点
                    do
                    {
                        lhmtc.LH_HomeSts(homePrm.index, out tHomeSts, homePrm.cardPrm.cardNum); //获取回原点状态
                    } while (tHomeSts == 1); //等待搜索原点停止
                    ret = tHomeSts == 0;
                    lhmtc.LH_ZeroPos(homePrm.index, 1, homePrm.cardPrm.cardNum);
                    lhmtc.LH_ClrSts(homePrm.index, 1, homePrm.cardPrm.cardNum);//清楚报警标志
                    if (ret && homePrm.strMessage.Substring(0, 1) == "Z")
                    {
                        lhmtc.LH_LmtsOff(homePrm.index, 1, homePrm.cardPrm.cardNum);//负限位无效
                        TrapPrm trapPrm = new TrapPrm
                        {
                            cardPrm = homePrm.cardPrm,
                            index = homePrm.index,
                            pulFactor = homePrm.pulFactor,
                            speed = homePrm.velHigh,
                            setPosPul = (int)(-7 / homePrm.pulFactor),
                            acc = homePrm.acc,
                            dec = homePrm.dec,
                            smoothTime = homePrm.smoothTime,
                            velStart = 0
                        };
                        UpdateTrap(cardType, trapPrm);
                        do
                        {
                            lhmtc.LH_GetSts(homePrm.index, out pSts, 1, homePrm.cardPrm.cardNum);
                        } while ((pSts >> 10 & 0x1) == 1);
                    }
                    return ret;
                }
            }
            
            return false;
        }
        /// <summary>
        /// 更新位点
        /// </summary>
        /// <param name="trapPrm">点位运动参数</param>
        public void UpdateTrap(int cardType, TrapPrm trapPrm)
        {
            double speed = trapPrm.speed / (trapPrm.pulFactor * 1000);
            if (cardType == 0)
            {
                Gts.TTrapPrm tTrapPrm = new Gts.TTrapPrm
                {
                    acc = trapPrm.acc / (trapPrm.pulFactor * 1000),
                    dec = trapPrm.dec / (trapPrm.pulFactor * 1000),
                    velStart = trapPrm.velStart / (trapPrm.pulFactor * 1000),
                    smoothTime = trapPrm.smoothTime
                };

                Gts.GT_PrfTrap(trapPrm.cardPrm.cardNum, trapPrm.index);
                Gts.GT_SetTrapPrm(trapPrm.cardPrm.cardNum, trapPrm.index, ref tTrapPrm);//设置点位运动参数
                Gts.GT_SetVel(trapPrm.cardPrm.cardNum, trapPrm.index, speed);//设置目标速度
                Gts.GT_SetPos(trapPrm.cardPrm.cardNum, trapPrm.index, trapPrm.setPosPul);//设置目标位置
                Gts.GT_Update(trapPrm.cardPrm.cardNum, 1 << (trapPrm.index - 1));//更新轴运动      
            }
            else
            {
                lhmtc.TrapPrfPrm tTrapPrm = new lhmtc.TrapPrfPrm
                {
                    acc = trapPrm.acc / (trapPrm.pulFactor * 1000),
                    dec = trapPrm.dec / (trapPrm.pulFactor * 1000),
                    velStart = trapPrm.velStart / (trapPrm.pulFactor * 1000),
                    smoothTime = trapPrm.smoothTime
                };

                lhmtc.LH_PrfTrap(trapPrm.index, trapPrm.cardPrm.cardNum);
                lhmtc.LH_SetTrapPrm(trapPrm.index, ref tTrapPrm, trapPrm.cardPrm.cardNum);//设置点位运动参数
                lhmtc.LH_SetVel(trapPrm.index, speed, trapPrm.cardPrm.cardNum);//设置目标速度
                lhmtc.LH_SetPos(trapPrm.index, trapPrm.setPosPul, trapPrm.cardPrm.cardNum);//设置目标位置
                lhmtc.LH_Update(1 << (trapPrm.index - 1), trapPrm.cardPrm.cardNum);//更新轴运动  
            }
        }

        /// <summary>
        /// 各轴JOG
        /// </summary>
        /// <param name="jogPrm"></param>
        /// <param name="speed"></param>
        public void JOG(int cardType, JogPrm jogPrm, double speed)
        {
            if (cardType == 0)
            {
                speed = speed / (jogPrm.pulFactor * 1000);
                Gts.TJogPrm jog = new Gts.TJogPrm
                {
                    acc = jogPrm.acc / (jogPrm.pulFactor * 1000),
                    dec = jogPrm.dec / (jogPrm.pulFactor * 1000),
                    smooth = 0   //只有为0，JOG才有效
                };

                Gts.GT_Stop(jogPrm.cardPrm.cardNum, 1 << (jogPrm.index - 1), 0); //停止该轴
                Gts.GT_PrfJog(jogPrm.cardPrm.cardNum, jogPrm.index); //设置JOG模式
                Gts.GT_SetJogPrm(jogPrm.cardPrm.cardNum, jogPrm.index, ref jog); //设置jog的加减速时间
                Gts.GT_SetVel(jogPrm.cardPrm.cardNum, jogPrm.index, speed);//设置目标速度
                Gts.GT_Update(jogPrm.cardPrm.cardNum, 1 << (jogPrm.index - 1));//更新轴运动
            }
            else
            {
                speed = speed / (jogPrm.pulFactor * 1000);
                lhmtc.JogPrfPrm jog = new lhmtc.JogPrfPrm
                {
                    acc = jogPrm.acc / (jogPrm.pulFactor * 1000),
                    dec = jogPrm.dec / (jogPrm.pulFactor * 1000),
                    smooth = 0   //只有为0，JOG才有效
                };

                lhmtc.LH_Stop((short)(1 << (jogPrm.index - 1)), 0, jogPrm.cardPrm.cardNum); //停止该轴
                lhmtc.LH_PrfJog(jogPrm.index, jogPrm.cardPrm.cardNum); //设置JOG模式
                lhmtc.LH_SetJogPrm(jogPrm.index, ref jog, jogPrm.cardPrm.cardNum); //设置jog的加减速时间
                lhmtc.LH_SetVel(jogPrm.index, speed, jogPrm.cardPrm.cardNum);//设置目标速度
                lhmtc.LH_Update(1 << (jogPrm.index - 1), jogPrm.cardPrm.cardNum);//更新轴运动
            }
            
        }

        public void StopAxis(int cardType)
        {
            Gts.GT_Stop(card[0].cardNum, 255, 1); //停止所有轴
        }

        /// <summary>
        /// 输出线圈函数
        /// </summary>
        /// <param name="Card"></param>卡号
        /// <param name="Y_Address"></param>输出线圈地址
        /// <param name="Bit_Number"></param>0为地电平  1为高电平
        public void OUT(short cardNum, bool m, short Y_Address)
        {
            if (m == true)
            {
                Gts.GT_SetDoBit(cardNum, 12, Y_Address, 0);
            }
            else
            {
                Gts.GT_SetDoBit(cardNum, 12, Y_Address, 1);
            }
        }

        /// <summary>
        /// 比较大于
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool 大于(int a, int b)
        {
            bool m = false;
            if (a > b)
            {
                m = true;
            }
            else
            {
                m = false;
            }

            return m;
        }

        /// <summary>
        /// 比较大于或等于
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool 大于或等于(int a, int b)
        {
            bool m = false;
            if (a >= b)
            {
                m = true;
            }
            else
            {
                m = false;
            }

            return m;
        }

        /// <summary>
        /// 比较小于
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool 小于(int a, int b)
        {
            bool m = false;
            if (a < b)
            {
                m = true;
            }
            else
            {
                m = false;
            }
            return m;
        }

        /// <summary>
        /// 比较小于或等于
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool 小于或等于(int a, int b)
        {
            bool m = false;
            if (a <= b)
            {
                m = true;
            }
            else
            {
                m = false;
            }
            return m;
        }

        /// <summary>
        /// 比较等于
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool 等于(int a, int b)
        {
            bool m = false;
            if (a == b)
            {
                m = true;
            }
            else
            {
                m = false;
            }
            return m;
        }

        /// <summary>
        /// 轴的位置比较
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="axis_busy"></param>轴是否忙信号
        /// <returns></returns>
        public bool Axis_等于(int a, int b, short axis_busy)
        {

            bool m = false;
            if ((a == b) && (axis_busy != 1))
            {
                m = true;
            }
            else
            {
                m = false;
            }
            return m;

        }
        /// <summary>
        /// 比较不等于
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public bool 不等于(int a, int b)
        {
            bool m = false;
            if (a != b)
            {
                m = true;
            }
            else
            {
                m = false;
            }
            return m;
        }

        /// <summary>
        /// int转二进制
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string[] Int_to_binary(int number) //int16位转二进制
        {
            string[] arrystr = null;

            string k = Convert.ToString(number, 2);//转成2进制
            string[] arrk = new string[16];
            string k2 = string.Format("{0:0000000000000000}", Convert.ToInt64(k));//补位0     
            for (int i = 0; i < k2.Length; i++)
            {
                arrk[i] = k2.Substring(i, 1);
            }
            //字符串数组反序
            string[] str = arrk;
            try
            {
                for (int n = 0; n < str.Length; n++)
                {
                    Console.Write("{0}  ", str[n]);
                }
                arrystr = str.ToArray();
                Array.Reverse(arrystr);
                for (int i = 0; i < arrystr.Length; i++)
                {
                    Console.Write(arrystr[i]);
                }
            }
            catch
            {
                for (int n = 0; n < str.Length; n++)
                {
                    arrystr[n] = "0";

                }
            }
            return arrystr;
        }


    }

    //限位回原点参数
    public class LimitHomePrm
    {
        public CardPrm cardPrm;
        public short index;
        public double pulFactor;
        public string strMessage;
        public short moveDir;
        public short indexDir;
        public short edge;             
        public double velHigh;
        public double velLow;
        public double acc;
        public double dec;
        public short smoothTime;
        public double searchHomeDistance;
        public double searchIndexDistance;	
        public double homeOffset;    
        public double escapeStep;      
    }

    //点位运动参数
    public class TrapPrm
    {
        public CardPrm cardPrm;
        public short index;
        public double pulFactor;
        public double setPos;
        public double getPos;
        public double getEncPos;
        public int setPosPul;
        public int getPosPul;
        public int getEncPosPul;
        public double speed;
        public double acc;
        public double dec;
        public double velStart;
        public short smoothTime;
    }

    //点位运动参数
    public class JogPrm
    {
        public CardPrm cardPrm;
        public short index;
        public double pulFactor;      
        public double velHigh;
        public double velLow;
        public double acc;
        public double dec;
    }

    //通用IO监控
    public class GPIO
    {
        public bool M;
        public bool new_State;
        public bool old_State;
        public bool RM;
        public bool FM;
    }

    //板卡参数
    public class CardPrm
    {
        public short cardNum;
        public short index;
        public short axisCount;
        public short GPIOCount;
        public short GLIOCount;     //gLinkIO总数
    }
}