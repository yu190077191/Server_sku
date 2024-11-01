using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using WF.Framework;
using System.Data.SqlClient;

namespace WF.DataAccess
{

    /// <summary>
    /// 数据库类型枚举
    /// </summary>
    public enum DatabaseTypeEnum
    {
        /// <summary>
        /// 数据库类型：Oracle
        /// </summary>
        Oracle,
        /// <summary>
        /// 数据库类型：SqlServer
        /// </summary>
        SqlServer,
        /// <summary>
        /// 数据库类型：Access
        /// </summary>
        Access,
        /// <summary>
        /// 数据库类型：MySql
        /// </summary>
        MySql,
        /// <summary>
        /// 数据库类型：SQLite
        /// </summary>
        SQLite
    }
    /// <summary>
    /// 数据库操作基类
    /// </summary>
    public class DbHelper
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string ConnectionString { get; set; }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public static DatabaseTypeEnum DbType { get; set; }
        /// <summary>
        /// 数据库命名参数符号
        /// </summary>
        public static string DbParmChar { get; set; }
        public DbHelper(string connstring)
        {
            ConnectionString = ConfigurationManager.ConnectionStrings[connstring].ConnectionString;
            this.DatabaseTypeEnumParse(ConfigurationManager.ConnectionStrings[connstring].ProviderName);
            DbParmChar = DbFactory.CreateDbParmCharacter();
        }
        /// <summary>
        /// 执行 SQL 语句，并返回受影响的行数。
        /// </summary>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText)
        {
            int num = 0;
            DbCommand cmd = DbFactory.CreateDbCommand();
            try
            {
                using (DbConnection conn = DbFactory.CreateDbConnection(ConnectionString))
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, null);
                    num = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                num = -1;
                WriteLog(cmdText, null, ex);
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
            return num;
        }
        /// <summary>
        /// 执行 SQL 语句，并返回受影响的行数。
        /// </summary>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <param name="parameters">执行命令所需的sql语句对应参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            int num = 0;
            DbCommand cmd = DbFactory.CreateDbCommand();
            try
            {
                using (DbConnection conn = DbFactory.CreateDbConnection(ConnectionString))
                {
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, parameters);
                    num = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                num = -1;
                WriteLog(cmdText, parameters, ex);
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
            return num;
        }
        /// <summary>
        /// 执行 SQL 语句，并返回受影响的行数。
        /// </summary>
        /// <param name="isOpenTrans">事务对象</param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行</param>
        /// <param name="parameters">执行命令所需的sql语句对应参数</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(DbTransaction isOpenTrans, CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            int num = 0;
            try
            {
                DbCommand cmd = DbFactory.CreateDbCommand();
                if (isOpenTrans == null || isOpenTrans.Connection == null)
                {
                    using (DbConnection conn = DbFactory.CreateDbConnection(ConnectionString))
                    {
                        PrepareCommand(cmd, conn, isOpenTrans, cmdType, cmdText, parameters);
                        num = cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    PrepareCommand(cmd, isOpenTrans.Connection, isOpenTrans, cmdType, cmdText, parameters);
                    num = cmd.ExecuteNonQuery();
                }
                cmd.Parameters.Clear();
            }
            catch (Exception ex)
            {
                num = -1;
                throw;
            }
            return num;
        }
        /// <summary>
        ///使用提供的参数，执行有结果集返回的数据库操作命令、并返回SqlDataReader对象
        /// </summary>
        /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="commandText">存储过程名称或者T-SQL命令行<</param>
        /// <returns>返回SqlDataReader对象</returns>
        public static IDataReader ExecuteReader(CommandType cmdType, string cmdText)
        {
            DbCommand cmd = DbFactory.CreateDbCommand();
            DbConnection conn = DbFactory.CreateDbConnection(ConnectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, null);
                IDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch (Exception ex)
            {
                conn.Close();
                cmd.Dispose();
                WriteLog(cmdText, null, ex);
                throw;
            }
        }
        /// <summary>
        /// 使用提供的参数，执行有结果集返回的数据库操作命令、并返回SqlDataReader对象
        /// </summary>
        /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="commandText">存储过程名称或者T-SQL命令行<</param>
        /// <param name="parameters">执行命令所需的sql语句对应参数</param>
        /// <returns>返回SqlDataReader对象</returns>
        public static IDataReader ExecuteReader(CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            DbCommand cmd = DbFactory.CreateDbCommand();
            DbConnection conn = DbFactory.CreateDbConnection(ConnectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, parameters);
                IDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch (Exception ex)
            {
                conn.Close();
                cmd.Dispose();
                WriteLog(cmdText, parameters, ex);
                throw;
            }
        }
        /// <summary>
        /// 查询数据填充到数据集DataSet中
        /// </summary>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">命令文本</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns>数据集DataSet对象</returns>
        public static DataSet GetDataSet(CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            DataSet ds = new DataSet();
            DbCommand cmd = DbFactory.CreateDbCommand();
            DbConnection conn = DbFactory.CreateDbConnection(ConnectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, parameters);
                IDbDataAdapter sda = DbFactory.CreateDataAdapter(cmd);
                sda.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                cmd.Dispose();
                WriteLog(cmdText, parameters, ex);
                throw;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
        }
        /// <summary>
        /// 查询数据填充到数据集DataSet中
        /// </summary>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">命令文本</param>
        /// <returns>数据集DataSet对象</returns>
        public static DataSet GetDataSet(CommandType cmdType, string cmdText)
        {
            DataSet ds = new DataSet();
            DbCommand cmd = DbFactory.CreateDbCommand();
            DbConnection conn = DbFactory.CreateDbConnection(ConnectionString);
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, null);
                IDbDataAdapter sda = DbFactory.CreateDataAdapter(cmd);
                sda.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                cmd.Dispose();
                WriteLog(cmdText, null, ex);
                throw;
            }
            finally
            {
                conn.Close();
                cmd.Dispose();
            }
        }
        /// <summary>
        /// 依靠数据库连接字符串connectionString,
        /// 使用所提供参数，执行返回首行首列命令
        /// </summary>
        /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="commandText">存储过程名称或者T-SQL命令行</param>
        /// <param name="parameters">执行命令所需的sql语句对应参数</param>
        /// <returns>返回一个对象，使用Convert.To{Type}将该对象转换成想要的数据类型。</returns>
        public static object ExecuteScalar(CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            DbCommand cmd = DbFactory.CreateDbCommand();
            try
            {
                using (DbConnection connection = DbFactory.CreateDbConnection(ConnectionString))
                {
                    PrepareCommand(cmd, connection, null, cmdType, cmdText, parameters);
                    object val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
            catch (Exception ex)
            {
                WriteLog(cmdText, parameters, ex);
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
        }
        private static void WriteLog(string cmdText, DbParameter[] parameters, Exception ex = null)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("SQL:" + cmdText);
            if (parameters != null)
            {
                sb.AppendLine("parameters:");
                foreach (DbParameter item in parameters)
                {
                    sb.AppendFormat("[{0}]:[{1}],\n", item.ParameterName, item.Value);
                }
            }
            // ErrorLogHelper.Error(sb.ToString(), ex);
        }
        /// <summary>
        /// 依靠数据库连接字符串connectionString,
        /// 使用所提供参数，执行返回首行首列命令
        /// </summary>
        /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="commandText">存储过程名称或者T-SQL命令行</param>
        /// <param name="parameters">执行命令所需的sql语句对应参数</param>
        /// <returns>返回一个对象，使用Convert.To{Type}将该对象转换成想要的数据类型。</returns>
        public static object ExecuteScalar(DbTransaction isOpenTrans, CommandType cmdType, string cmdText, params DbParameter[] parameters)
        {
            DbCommand cmd = DbFactory.CreateDbCommand();
            try
            {
                object val;

                if (isOpenTrans == null || isOpenTrans.Connection == null)
                {
                    using (DbConnection conn = DbFactory.CreateDbConnection(ConnectionString))
                    {
                        PrepareCommand(cmd, conn, isOpenTrans, cmdType, cmdText, parameters);
                        val = cmd.ExecuteScalar();
                    }
                }
                else
                {
                    PrepareCommand(cmd, isOpenTrans.Connection, isOpenTrans, cmdType, cmdText, parameters);
                    val = cmd.ExecuteScalar();
                }
                cmd.Parameters.Clear();
                return val;
            }
            catch (Exception ex)
            {
                WriteLog(cmdText, parameters, ex);
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
        }
        /// <summary>
        /// 依靠数据库连接字符串connectionString,
        /// 使用所提供参数，执行返回首行首列命令
        /// </summary>
        /// <param name="commandType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="commandText">存储过程名称或者T-SQL命令行</param>
        /// <returns>返回一个对象，使用Convert.To{Type}将该对象转换成想要的数据类型。</returns>
        public static object ExecuteScalar(CommandType cmdType, string cmdText)
        {
            DbCommand cmd = DbFactory.CreateDbCommand();
            try
            {
                using (DbConnection connection = DbFactory.CreateDbConnection(ConnectionString))
                {
                    PrepareCommand(cmd, connection, null, cmdType, cmdText, null);
                    object val = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
            catch (Exception ex)
            {
                WriteLog(cmdText, null, ex);
                throw;
            }
            finally
            {
                cmd.Dispose();
            }
        }
        /// <summary>
        /// 为即将执行准备一个命令
        /// </summary>
        /// <param name="cmd">SqlCommand对象</param>
        /// <param name="conn">SqlConnection对象</param>
        /// <param name="isOpenTrans">DbTransaction对象</param>
        /// <param name="cmdType">执行命令的类型（存储过程或T-SQL，等等）</param>
        /// <param name="cmdText">存储过程名称或者T-SQL命令行, e.g. Select * from Products</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        private static void PrepareCommand(DbCommand cmd, DbConnection conn, DbTransaction isOpenTrans, CommandType cmdType, string cmdText, DbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = int.Parse("180");
            if (isOpenTrans != null)
                cmd.Transaction = isOpenTrans;
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                cmd.Parameters.AddRange(cmdParms);
            }
        }
        /// <summary>
        /// 用于数据库类型的字符串枚举转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public void DatabaseTypeEnumParse(string value)
        {
            try
            {
                switch (value)
                {
                    case "System.Data.SqlClient":
                        DbType = DatabaseTypeEnum.SqlServer;
                        break;
                    case "System.Data.OracleClient":
                        DbType = DatabaseTypeEnum.Oracle;
                        break;
                    case "MySql.Data.MySqlClient":
                        DbType = DatabaseTypeEnum.MySql;
                        break;
                    case "System.Data.OleDb":
                        DbType = DatabaseTypeEnum.Access;
                        break;
                    case "System.Data.SQLite":
                        DbType = DatabaseTypeEnum.SQLite;
                        break;
                    default:
                        break;
                }
            }
            catch
            {
                throw new Exception("数据库类型\"" + value + "\"错误，请检查！");
            }
        }
    }
}
