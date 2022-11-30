using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TensionDetectionDLL
{
    /// <summary>
    /// 静态类，保存所有固定信息
    /// </summary>
    public static class StaticInfo
    {
        public static string TDUser { get; set; }//当前的登录用户
        public static SQLiteHelper sqlhelper;
        public static string[] TDDate=new string[20];//接收用户名的数组
        public static int nDate;//数据库中用户名的个数
        /// <summary>
        /// 当前用户权限等级 供应商：1，管理员：2，操作员：3
        /// </summary>
        public static int TDLevel;//当前用户权限等级
        public static string[] Rows = new string[] { "时间", "上限", "下限", "监控值" };
        public static string[,] Date = new string[12,3];//12路张力上限、下限以及传感器实时监控值
        public static int[] iYCoordinate = new int[4] { 40, 80, 35, 65};//标定参数设置值
        public static bool bIsSave { get; set; }
        //检查用户名是否存在 1：存在、0：不存在
        public static bool CheckUserName()
        {
            return !string.IsNullOrEmpty(StaticInfo.TDUser);
        }
    }
}
