using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace TensionDetectionDLL
{
    public static class TDMethod
    {
        static TDMethod()
        {
            string dbpath = AppDomain.CurrentDomain.BaseDirectory + "DB\\TensionDB.db";
            StaticInfo.sqlhelper = new SQLiteHelper(dbpath, ""); //参数1: db的绝对路径 参数2: 数据库密码
        }

        /// <summary>
        /// 登录
        /// </summary>
        public static void TDLogin(string TDUserName, string TDUserPass)
        {
            string sql = "select TDUserName,TDLevel FROM TDUserTable where TDUserName=@TDUserName and TDUserPass=@TDUserPass";
            SQLiteParameter[] param = new SQLiteParameter[]{
                new SQLiteParameter("@TDUserName",TDUserName),
                new SQLiteParameter("@TDUserPass",TDUserPass)
             };
            SQLiteDataReader dr = StaticInfo.sqlhelper.ExecuteReader(sql, param);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    StaticInfo.TDUser = dr["TDUserName"].ToString();
                    StaticInfo.TDLevel = Convert.ToInt16(dr["TDLevel"]);
                }
            }
        }

        /// <summary>
        /// 注册用户名
        /// </summary>
        public static int Reg(string TDUserName, string TDUserPass)
        {
            string sql = "INSERT INTO TDUserTable (TDUserName,TDUserPass) VALUES (@TDUserName,@TDUserPass);";

            SQLiteParameter[] param = new SQLiteParameter[]{
                new SQLiteParameter("@TDUserName",TDUserName),
                 new SQLiteParameter("@TDUserPass",TDUserPass)
             };
            int res = StaticInfo.sqlhelper.ExecuteNonQuery(sql, param);
            return res;
        }

        /// <summary>
        /// 检查用户名（注册界面）
        /// </summary>
        public static int CheckUserName(string TDUserName)
        {
            string sql = "select count(*) FROM TDUserTable where TDUserName=@TDUserName";
            SQLiteParameter[] param = new SQLiteParameter[]{
                new SQLiteParameter("@TDUserName",TDUserName)
             };
            int res = Convert.ToInt32(StaticInfo.sqlhelper.ExecuteScalar(sql, param));
            return res;
        }

        /// <summary>
        /// 读取所有用户名
        /// </summary>
        public static void TDReadDate(int TDRead)
        {
            int i = 0;

            string sql = "select TDUserName FROM TDUserTable where TDRead=@TDRead";
            SQLiteParameter[] param = new SQLiteParameter[]{
                new SQLiteParameter("@TDRead",TDRead)
             };
            SQLiteDataReader dr = StaticInfo.sqlhelper.ExecuteReader(sql, param);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    StaticInfo.TDDate[i]= dr["TDUserName"].ToString();
                    i++;
                }
                StaticInfo.nDate = i;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        public static int TDDelete(string TDUserName)
        {
            string sql = "DELETE FROM TDUserTable where TDUserName=@TDUserName";

            SQLiteParameter[] param = new SQLiteParameter[]{
                new SQLiteParameter("@TDUserName",TDUserName)
             };
            int res = StaticInfo.sqlhelper.ExecuteNonQuery(sql, param);
            return res;
        }

        /// <summary>
        /// 修改
        /// </summary>
        public static int Modify(string TDUserName, string TDUserPass)
        {
            string sql = "UPDATE TDUserTable SET TDUserPass=@TDUserPass where TDUserName=@TDUserName";

            SQLiteParameter[] param = new SQLiteParameter[]{
                new SQLiteParameter("@TDUserName",TDUserName),
                new SQLiteParameter("@TDUserPass",TDUserPass)
             };
            int res = StaticInfo.sqlhelper.ExecuteNonQuery(sql, param);
            return res;
        }
    }
}
