using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using WF.Framework;

namespace WF.DataAccess
{
    public class Database : IDatabase, IDisposable
    {
        #region 构造函数
        public static string connString { get; set; }
        /// <summary>
        /// 构造方法
        /// </summary>
        public Database(string connstring)
        {
            DbHelper dbhelper = new DbHelper(connstring);
        }
        /// <summary>
        /// 数据库连接对象
        /// </summary>
        private DbConnection dbConnection { get; set; }
        /// <summary>
        /// 事务对象
        /// </summary>
        private DbTransaction isOpenTrans { get; set; }
        /// <summary>
        /// 是否已在事务之中
        /// </summary>
        public bool inTransaction { get; set; }
        /// <summary>
        /// 事务开始
        /// </summary>
        /// <returns></returns>
        public DbTransaction BeginTrans()
        {
            if (!this.inTransaction)
            {
                dbConnection = DbFactory.CreateDbConnection(DbHelper.ConnectionString);
                if (dbConnection.State == ConnectionState.Closed)
                {
                    dbConnection.Open();
                }
                inTransaction = true;
                isOpenTrans = dbConnection.BeginTransaction();
            }
            return isOpenTrans;
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            if (this.inTransaction)
            {
                this.inTransaction = false;
                this.isOpenTrans.Commit();
                this.Close();
            }
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            if (this.inTransaction)
            {
                this.inTransaction = false;
                this.isOpenTrans.Rollback();
                this.Close();
            }
        }
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            if (this.dbConnection != null)
            {
                this.dbConnection.Close();
                this.dbConnection.Dispose();
            }
            if (this.isOpenTrans != null)
            {
                this.isOpenTrans.Dispose();
            }
            this.dbConnection = null;
            this.isOpenTrans = null;
        }
        /// <summary>
        /// 内存回收
        /// </summary>
        public void Dispose()
        {
            if (this.dbConnection != null)
            {
                this.dbConnection.Dispose();
            }
            if (this.isOpenTrans != null)
            {
                this.isOpenTrans.Dispose();
            }
        }
        #endregion


        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <param name="isOpenTrans">事务对象</param>
        /// <returns></returns>
        public int ExecuteBySql(StringBuilder strSql, DbParameter[] parameters, DbTransaction isOpenTrans)
        {
            return DbHelper.ExecuteNonQuery(isOpenTrans, CommandType.Text, strSql.ToString(), parameters);
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <param name="isOpenTrans">事务对象</param>
        /// <returns></returns>
        public int ExecuteByProc(string procName, DbParameter[] parameters, DbTransaction isOpenTrans2)
        {
            return DbHelper.ExecuteNonQuery(isOpenTrans2, CommandType.StoredProcedure, procName, parameters);
        }

        /// <summary>
        /// 查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="strSql">Sql语句</param>
        /// <returns></returns>
        public DataTable FindTableBySql(string strSql)
        {
            IDataReader dr = DbHelper.ExecuteReader(CommandType.Text, strSql.ToString());
            return DatabaseReader.ReaderToDataTable(dr);
        }

        /// <summary>
        /// 查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="procName">存储过程</param>
        /// <returns></returns>
        public DataTable FindTableByProc(string procName)
        {
            IDataReader dr = DbHelper.ExecuteReader(CommandType.StoredProcedure, procName);
            return DatabaseReader.ReaderToDataTable(dr);
        }
        /// <summary>
        /// 查询数据列表、返回 DataTable
        /// </summary>
        /// <param name="procName">存储过程</param>
        /// <param name="parameters">sql语句对应参数</param>
        /// <returns></returns>
        public DataTable FindTableByProc(string procName, DbParameter[] parameters)
        {
            IDataReader dr = DbHelper.ExecuteReader(CommandType.StoredProcedure, procName, parameters);
            return DatabaseReader.ReaderToDataTable(dr);
        }

        public object FindScalarBySql(string strSql, DbParameter[] parameters)
        {
            return DbHelper.ExecuteScalar(CommandType.Text, strSql, parameters);
        }


        public object FindScalarBySql(string strSql, DbParameter[] parameters,DbTransaction isOpenTrans)
        {
            return DbHelper.ExecuteScalar(isOpenTrans,CommandType.Text, strSql, parameters);
        }


    }
}
