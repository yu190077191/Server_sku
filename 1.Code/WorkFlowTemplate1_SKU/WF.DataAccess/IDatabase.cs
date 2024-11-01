using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace WF.DataAccess
{
    public interface IDatabase : IDisposable
    {
        bool inTransaction { get; set; }
        DbTransaction BeginTrans();
        void Commit();
        void Rollback();
        void Close();

        int ExecuteByProc(string procName, DbParameter[] parameters, DbTransaction isOpenTrans2);

        int ExecuteBySql(StringBuilder strSql, DbParameter[] parameters, DbTransaction isOpenTrans2);

        DataTable FindTableBySql(string strSql);

        DataTable FindTableByProc(string procName);
        DataTable FindTableByProc(string procName, DbParameter[] parameters);

        object FindScalarBySql(string strSql, DbParameter[] parameters);


        object FindScalarBySql(string strSql, DbParameter[] parameters, DbTransaction isOpenTrans);
    }

}
