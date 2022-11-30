using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace TensionDetectionDLL
{
    public class SQLiteHelper
    {
        public SQLiteHelper(string conn)
        {
            strConn = conn;
        }
        public SQLiteHelper(string path, string pass)
        {
            strConn = string.Format("Data Source={0};Pooling=true;FailIfMissing=false", path, pass);
        }

        /// <summary>
        /// 创建链接字符串
        private string strConn = string.Empty;

        #region  执行Sql语句，增删改
        /// <summary>
        /// 执行Sql语句，增删改
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parms">参数</param>
        /// <returns>影响行数</returns>
        public int ExecuteNonQuery(string sql, params SQLiteParameter[] parms)
        {
            using (SQLiteConnection conn = new SQLiteConnection(strConn))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    try
                    {
                        if (parms != null)
                        {
                            cmd.Parameters.AddRange(parms);
                        }
                        conn.Open();
                        return cmd.ExecuteNonQuery();
                    }
                    catch
                    {

                        return 0;
                    }

                }
            }
        }
        #endregion

        #region 执行Sql语句，1个返回值

        /// <summary>
        /// 执行一个Sql语句，1个返回值
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parms">sql参数</param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, params SQLiteParameter[] parms)
        {
            using (SQLiteConnection conn = new SQLiteConnection(strConn))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    if (parms != null)
                    {
                        cmd.Parameters.AddRange(parms);
                    }
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }
        #endregion

        #region 执行sql语句，返回结果集

        public SQLiteDataReader ExecuteReader(string sql, params SQLiteParameter[] parms)
        {
            SQLiteConnection conn = new SQLiteConnection(strConn);
            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            {
                if (parms != null)
                {
                    cmd.Parameters.AddRange(parms);
                }
                conn.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }
        #endregion
    }
}
