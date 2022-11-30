using CCWin.SkinControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace BAR.Commonlib.Connectors.Protocols
{
    public class Modbus
    {
        #region 常量定义
        public const byte MB_READ_COILS = 0x01;             //读线圈寄存器
        public const byte MB_READ_DISCRETE = 0x02;          //读离散输入寄存器
        public const byte MB_READ_HOLD_REG = 0x03;          //读保持寄存器
        public const byte MB_READ_INPUT_REG = 0x04;         //读输入寄存器
        public const byte MB_WRITE_SINGLE_COIL = 0x05;      //写单个线圈
        public const byte MB_WRITE_SINGLE_REG = 0x06;       //写单寄存器
        public const byte MB_WRITE_MULTIPLE_COILS = 0x0f;   //写多线圈
        public const byte MB_WRITE_MULTIPLE_REGS = 0x10;    //写多寄存器
        public const byte MB_READ_MULTIPLE_REGS = 0x17;     //读多寄存器
        /// <summary>
        /// 变焦镜头电机控制指令
        /// </summary>
        public const byte MB_READ_ZOOMLENS_DST = 0x64;
        /// <summary>
        /// 读变焦镜头电机状态
        /// </summary>
        public const byte MB_READ_ZOOMLENS_STATUS = 0x65;

        private const int MB_MAX_LENGTH = 255;               //最大数据长度
        private const int MB_SCI_MAX_COUNT = 15;             //指令管道最大存放的指令各数
        private const int MB_MAX_REPEAT_COUNT = 3;           //指令最多发送次数
        #endregion


        /// 获取寄存器或线圈 分组后的成员各数
        /// </summary>
        /// <param name="addr">首地址</param>
        /// <returns>成员各数</returns>
        public static int GetAddressValueLength(int addr)
        {
            int res = 0;
            return res;
        }
        /// <summary>
        /// 获取地址所对应的数据
        /// </summary>
        /// <param name="addr">地址</param>
        /// <param name="type">类型</param>
        /// <returns>获取到的数据</returns>
        public static object GetAddressValue(int addr, byte type)
        {
            switch (type)       //功能码类型判断
            {
                case MB_READ_COILS:
                case MB_READ_DISCRETE:
                case MB_READ_HOLD_REG:
                case MB_READ_INPUT_REG: break;
                case MB_WRITE_SINGLE_COIL:
                case MB_WRITE_MULTIPLE_COILS: type = MB_READ_DISCRETE; break;
                case MB_WRITE_SINGLE_REG:
                case MB_WRITE_MULTIPLE_REGS: type = MB_READ_HOLD_REG; break;
                default: return null;
            }
            return null;
        }
        /// <summary>
        /// 设置地址所对应的数据
        /// </summary>
        /// <param name="addr">地址</param>
        /// <param name="type">类型</param>
        /// <param name="data">数据</param>
        /// <returns>是否成功</returns>
        public static object SetAddressValue(int addr, byte type, object data)
        {
       
            return null;
        }
        /// <summary>
        /// 获取一连串数据
        /// </summary>
        /// <param name="addr">首地址</param>
        /// <param name="type">功能码</param>
        /// <param name="len">长度</param>
        /// <returns>转换后的字节数组</returns>
        public static byte[] GetAddressValues(int addr, byte type, int len)
        {
            byte[] arr = null;
            object obj;
            byte temp;
            int temp2;

            switch (type)
            {
                case MB_WRITE_MULTIPLE_COILS:
                    arr = new byte[(len % 8 == 0) ? (len / 8) : (len / 8 + 1)];
                    for (int i = 0; i < arr.Length; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {   //获取地址所对应的数据 并判断所读数据 是否被指定，有没被指定的数据 直接返回null
                            obj = GetAddressValue(addr + i * 8 + j, MB_READ_COILS);
                            if (obj == null)
                                return null;
                            else
                                temp = Convert.ToByte(obj);
                            arr[i] |= (byte)((temp == 0 ? 0 : 1) << j);
                        }
                    }
                    break;
                case MB_WRITE_MULTIPLE_REGS:
                    arr = new byte[len * 2];
                    for (int i = 0; i < len; i++)
                    {
                        obj = GetAddressValue(addr + i, MB_READ_HOLD_REG);
                        if (obj == null)
                            return null;
                        else
                            temp2 = Convert.ToInt32(obj);
                        arr[i * 2] = (byte)(temp2 >> 8);
                        arr[i * 2 + 1] = (byte)(temp2 & 0xFF);
                    }
                    break;
                default: break;
            }
            return arr;
        }

        #region 校验
        private static readonly byte[] aucCRCHi = {
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
        0x00, 0xC1, 0x81, 0x40
    };
        private static readonly byte[] aucCRCLo = {
        0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2, 0xC6, 0x06, 0x07, 0xC7,
        0x05, 0xC5, 0xC4, 0x04, 0xCC, 0x0C, 0x0D, 0xCD, 0x0F, 0xCF, 0xCE, 0x0E,
        0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09, 0x08, 0xC8, 0xD8, 0x18, 0x19, 0xD9,
        0x1B, 0xDB, 0xDA, 0x1A, 0x1E, 0xDE, 0xDF, 0x1F, 0xDD, 0x1D, 0x1C, 0xDC,
        0x14, 0xD4, 0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13, 0xD3,
        0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3, 0xF2, 0x32,
        0x36, 0xF6, 0xF7, 0x37, 0xF5, 0x35, 0x34, 0xF4, 0x3C, 0xFC, 0xFD, 0x3D,
        0xFF, 0x3F, 0x3E, 0xFE, 0xFA, 0x3A, 0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38,
        0x28, 0xE8, 0xE9, 0x29, 0xEB, 0x2B, 0x2A, 0xEA, 0xEE, 0x2E, 0x2F, 0xEF,
        0x2D, 0xED, 0xEC, 0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
        0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60, 0x61, 0xA1,
        0x63, 0xA3, 0xA2, 0x62, 0x66, 0xA6, 0xA7, 0x67, 0xA5, 0x65, 0x64, 0xA4,
        0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F, 0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB,
        0x69, 0xA9, 0xA8, 0x68, 0x78, 0xB8, 0xB9, 0x79, 0xBB, 0x7B, 0x7A, 0xBA,
        0xBE, 0x7E, 0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C, 0xB4, 0x74, 0x75, 0xB5,
        0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71, 0x70, 0xB0,
        0x50, 0x90, 0x91, 0x51, 0x93, 0x53, 0x52, 0x92, 0x96, 0x56, 0x57, 0x97,
        0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C, 0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E,
        0x5A, 0x9A, 0x9B, 0x5B, 0x99, 0x59, 0x58, 0x98, 0x88, 0x48, 0x49, 0x89,
        0x4B, 0x8B, 0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
        0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42, 0x43, 0x83,
        0x41, 0x81, 0x80, 0x40
    };
        /// <summary>
        /// CRC效验
        /// </summary>
        /// <param name="pucFrame">效验数据</param>
        /// <param name="usLen">数据长度</param>
        /// <returns>效验结果</returns>
        public static int Crc16(byte[] pucFrame, int usLen)
        {
            int i = 0;
            byte ucCRCHi = 0xFF;
            byte ucCRCLo = 0xFF;
            UInt16 iIndex = 0x0000;

            while (usLen-- > 0)
            {
                iIndex = (UInt16)(ucCRCLo ^ pucFrame[i++]);
                ucCRCLo = (byte)(ucCRCHi ^ aucCRCHi[iIndex]);
                ucCRCHi = aucCRCLo[iIndex];
            }
            return (ucCRCHi << 8 | ucCRCLo);
        }
        /// <summary>
        /// LRC效验
        /// </summary>
        /// <param name="pucFrame">效验数据</param>
        /// <param name="usLen">数据长度</param>
        /// <returns>效验结果</returns>
        public static int LRC(byte[] pucFrame, int usLen)
        {
            int i = 0;
            byte ucCRCHi = 0xFF;
            byte ucCRCLo = 0xFF;
            UInt16 iIndex = 0x0000;

           
            return (ucCRCHi << 8 | ucCRCLo);
        }

        /// <summary>
        /// BCC和校验代码返回16进制
        /// </summary>
        /// <param name="data">需要校验的数据包</param>
        /// <returns></returns>
        public static string BCC(byte[] data, int len)
        {
            var CheckCode = new byte[1];
            CheckCode[0] = 0;
            for (int i = 0; i < len; i++)
            {
                CheckCode[0] ^= data[i];
            }
            var CheckCodeAcii = Encoding.UTF8.GetBytes(CheckCode.SelectMany(n => n.ToString("X2")).ToArray());
            return Encoding.UTF8.GetString(CheckCodeAcii);
        }

        #endregion

        #region 发送指命操作
        #endregion

        #region 回传数据操作
        /// <summary>
        /// 存储回传的线圈
        /// </summary>
        /// <param name="data">回传的数组</param>
        /// <param name="addr">首地址</param>
        /// <returns>存储是否正确</returns>
        private static bool ReadDiscrete(byte[] data, int addr)
        {
            bool res = true;
            int len = data[2];

            if (len != (data.Length - 5))  //数据长度不正确 直接退出
                return false;

            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (SetAddressValue(addr + i * 8 + j, data[1], data[i + 3] & (0x01 << j)) == null)
                    {
                        return false;
                    }
                }
            }
            return res;
        }
        /// <summary>
        /// 读回传的线圈
        /// </summary>
        /// <param name="data">回传的数组</param>
        /// <param name="addr">首地址</param>
        /// <returns>存储是否正确</returns>
        private static bool ReadDiscrete(byte[] data, out bool[] retData)
        {
            bool res = true;
            int len = data[2];
            retData = new bool[len];


            UInt32 temData = 0;
            for(int i = 0; i < len; i++)
            {
                if(len == 1)
                {
                    temData = temData << 8 | data[3 + i];
                }
                else
                {
                    temData = temData << 8 | data[4 - i];
                }
            }
            for(int n = 0; n< len; n++)
            {
                var b = temData & 0x01;
                retData[n] = Convert.ToBoolean(b);
                temData = temData >> 1;
            }

            return res;
        }
        /// <summary>
        /// 读回传的寄存器
        /// </summary>
        /// <param name="data">回传的数组</param>
        /// <param name="addr">首地址</param>
        /// <returns>存储是否正确</returns>
        private static bool ReadReg(byte[] data, int addr)
        {
            bool res = true;
            int len = data[2];

            for (int i = 0; i < len; i += 2)
            {
                if (SetAddressValue(addr + i / 2, data[1], (data[i + 3] << 8) | data[i + 4]) == null)
                {
                    res = false;
                    break;
                }
            }
            return res;
        }
        /// <summary>
        /// 读回传的寄存器
        /// </summary>
        /// <param name="data">回传的数组</param>
        /// <param name="addr">首地址</param>
        /// <returns>存储是否正确</returns>
        private static bool ReadReg(byte[] data, out byte[] retData)
        {
            bool res = true;
            int len = data[2];
            retData = new byte[len];

            if (len != (data.Length - 5))  //数据长度不正确 直接退出
                return false;

            Array.Copy(data, 3, retData, 0, len);
            return res;
        }

        /// <summary>
        /// 读变焦镜头回传的寄存器
        /// </summary>
        /// <param name="data">回传的数组</param>
        /// <param name="addr">首地址</param>
        /// <returns>存储是否正确</returns>
        private static bool ReadReg_ZoomLens(byte[] data, out byte[] retData)
        {
            bool res = true;
            int len = 11;
            retData = new byte[len];

            if (len != (data.Length - 5))  //数据长度不正确 直接退出
                return false;

            Array.Copy(data, 3, retData, 0, len);
            return res;
        }

        /// <summary>
        /// 回传的数据处理
        /// </summary>
        /// <param name="buff">回传的整帧数据</param>
        /// <param name="addr">当前所操作的首地址</param>
        /// <returns></returns>
        private static bool __ReceiveDataProcess(byte[] buff, int addr)
        {
            if (buff == null)
                return false;
            if (buff.Length < 5)    //回传的数据 地址+功能码+长度+2效验 = 5字节
                return false;

            bool res = true;
            switch (buff[1])
            {
                case MB_READ_COILS: ReadDiscrete(buff, addr); break;
                case MB_READ_DISCRETE: ReadDiscrete(buff, addr); break;
                case MB_READ_HOLD_REG: ReadReg(buff, addr); break;
                case MB_READ_INPUT_REG: ReadReg(buff, addr); break;
                case MB_WRITE_SINGLE_COIL:
                case MB_WRITE_SINGLE_REG:
                case MB_WRITE_MULTIPLE_COILS:
                case MB_WRITE_MULTIPLE_REGS: break;
                default: res = false; break;
            }
            return res;
        }
        /// <summary>
        /// 回传的数据处理
        /// </summary>
        /// <param name="buff">回传的整帧数据</param>
        /// <param name="addr">当前所操作的首地址</param>
        /// <returns></returns>
        private static bool __ReceiveDataProcess(byte[] buff, ref object retData)
        {
            if (buff == null)
                return false;
            if (buff.Length < 5)    //回传的数据 地址+功能码+长度+2效验 = 5字节
                return false;

            bool res = true;
            bool[] retArrBool;
            byte[] retArrByte;
            switch (buff[1])
            {
                case MB_READ_COILS: ReadDiscrete(buff,out retArrBool); retData = retArrBool; break;
                case MB_READ_DISCRETE: ReadDiscrete(buff,out retArrBool); retData = retArrBool; break;
                case MB_READ_HOLD_REG: ReadReg(buff, out retArrByte); retData = retArrByte; break;
                case MB_READ_INPUT_REG: ReadReg(buff, out retArrByte); retData = retArrByte; break;
                case MB_WRITE_SINGLE_COIL:
                case MB_WRITE_SINGLE_REG:
                case MB_WRITE_MULTIPLE_COILS:
                case MB_WRITE_MULTIPLE_REGS: break;
                case MB_READ_ZOOMLENS_DST: ReadReg_ZoomLens(buff, out retArrByte); retData = retArrByte; break;
                case MB_READ_ZOOMLENS_STATUS: ReadReg_ZoomLens(buff, out retArrByte); retData = retArrByte; break;
                default: res = false; retData = null; break;
            }
            return res;
        }

        #endregion

        #region 接口函数
        /// <summary>
        /// RTU协议回传的数据处理
        /// </summary>
        /// <param name="buff">回传的字节数据</param>
        /// <param name="len">判断整帧数据的长度</param>
        /// <returns></returns>
        public static bool GetRTUFrameLength(byte[] buff, ref int len)
        {
            if (buff == null)
                return false;
            if (buff.Length < 5)    //回传的数据 地址+功能码+长度+2效验 = 5字节
                return false;

            byte functionCode = buff[1];
            bool res = true;
            switch (functionCode)
            {
                case MB_READ_COILS: 
                case MB_READ_DISCRETE:
                case MB_READ_HOLD_REG: 
                case MB_READ_INPUT_REG:
                    len = buff[2] + 5;
                    break;
                case MB_WRITE_SINGLE_COIL:
                case MB_WRITE_SINGLE_REG:
                case MB_WRITE_MULTIPLE_COILS:
                case MB_WRITE_MULTIPLE_REGS:
                    len = 8;
                    break;
                default: res = false;  break;
            }
            return res;
        }
        /// <summary>
        /// ASCII协议回传的数据处理
        /// </summary>
        /// <param name="buff">回传的字节数据</param>
        /// <param name="len">判断整帧数据的长度</param>
        /// <returns></returns>
        public static bool GetASCIIFrameLength(byte[] buff, ref int len)
        {
            if (buff == null)
                return false;
            if (buff.Length < 5)    //回传的数据 地址+功能码+长度+2效验 = 5字节
                return false;

            byte functionCode = buff[1];
            bool res = true;
            switch (functionCode)
            {
                case MB_READ_COILS:
                case MB_READ_DISCRETE:
                case MB_READ_HOLD_REG:
                case MB_READ_INPUT_REG:
                    len = buff[2] + 5;
                    break;
                case MB_WRITE_SINGLE_COIL:
                case MB_WRITE_SINGLE_REG:
                case MB_WRITE_MULTIPLE_COILS:
                case MB_WRITE_MULTIPLE_REGS:
                    len = 8;
                    break;
                default: res = false; break;
            }
            return res;
        }
        /// <summary>
        /// 对数据进行预处理
        /// </summary>
        /// <param name="comm">所用到的串口</param>
        public static bool ReceiveDataProcess(byte[] buffer, out object data)
        {
            bool ret = false;
            data = null;
            ret = __ReceiveDataProcess(buffer,ref data);
           
            return ret;
        }
        #endregion


    }
}
