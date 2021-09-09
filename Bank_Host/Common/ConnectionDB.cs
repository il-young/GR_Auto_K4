using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.SQLite;
using System.Threading;
using MICube.SmartDriver.Base.Log4net;

namespace ASMTowerInterface
{
    public class SqliteManager
    {
        private string sqlConnectionString;
        //private SQLiteConnection sqliteConnection;

        public SqliteManager(string conStr)
        {
            this.sqlConnectionString = conStr;
        }

        //public void SQLiteDBManger()
        //{
        //    this.sqlConnectionString = String.Format("Data Source={0}\\AmkorDB.db; version=3", System.IO.Directory.GetCurrentDirectory());
        //}

        int SetRetryMax = 5;
        public int SetData(string queryString, SQLiteParameter[] sqlParam = null, int retry = 1)
        {
            int ret = 0;
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(sqlConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(queryString, c))
                    {
                        if (sqlParam != null)
                            cmd.Parameters.AddRange(sqlParam);

                        ret = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                if (retry == SetRetryMax)
                {
                    Log.WriteLog(Log4net.EnumLogLevel.ERROR, queryString);
                    Log.WriteLog(Log4net.EnumLogLevel.ERROR, ex.ToString());
                }
                else
                {
                    System.Threading.Thread.Sleep(100);
                    Log.WriteLog(Log4net.EnumLogLevel.ERROR, "SQLITE Retry:" + retry);
                    return SetData(queryString, sqlParam, ++retry);
                }
            }

            return ret;
        }

        public int SetData(List<string> queryList, int retry = 1)
        {
            if (queryList.Count == 0)
                return 0;

            int ret = 0;
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(sqlConnectionString))
                {
                    Log.WriteLog(Log4net.EnumLogLevel.INFO, "SQLITE SetData List Start");
                    c.Open();
                    BeginTran(c);
                    foreach (var query in queryList)
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand(query, c))
                        {
                            ret = cmd.ExecuteNonQuery();
                        }
                    }
                    CommitTran(c);
                    Log.WriteLog(Log4net.EnumLogLevel.INFO, "SQLITE SetData List End");
                }
            }
            catch (Exception ex)
            {
                if (retry == SetRetryMax)
                {
                    Log.WriteLog(Log4net.EnumLogLevel.ERROR, queryList.Count.ToString() + "/" + queryList[0]);
                    Log.WriteLog(Log4net.EnumLogLevel.ERROR, ex.ToString());
                }
                else
                {
                    System.Threading.Thread.Sleep(100);
                    Log.WriteLog(Log4net.EnumLogLevel.ERROR, "SQLITE Retry:" + retry);
                    return SetData(queryList, ++retry);
                }
            }

            return ret;
        }

        public bool OpenTest()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(sqlConnectionString))
                    c.Open();

                return true;
            }
            catch (Exception ex)
            {
                Log.WriteLog(Log4net.EnumLogLevel.ERROR, ex.ToString());
            }

            return false;
        }

            int GetRetryMax = 5;
        public DataTable GetData(string queryString, int retry = 1)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SQLiteConnection c = new SQLiteConnection(sqlConnectionString))
                {
                    c.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(queryString, c))
                    {
                        using (SQLiteDataReader rdr = cmd.ExecuteReader())
                        {
                            dt.Load(rdr);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                if (retry == GetRetryMax)
                {
                    Log.WriteLog(Log4net.EnumLogLevel.ERROR, queryString);
                    Log.WriteLog(Log4net.EnumLogLevel.ERROR, ex.ToString());
                }
                else
                {
                    System.Threading.Thread.Sleep(100);
                    Log.WriteLog(Log4net.EnumLogLevel.ERROR, "SQLITE Get Retry:" + retry);
                    return GetData(queryString, ++retry);
                }
            }
            return dt;
        }

        private void BeginTran(SQLiteConnection conn)
        {
            using (SQLiteCommand cmd = new SQLiteCommand("Begin", conn))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void CommitTran(SQLiteConnection conn)
        {
            using (SQLiteCommand cmd = new SQLiteCommand("Commit", conn))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
