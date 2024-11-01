using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using WF.Model;

namespace WF.DataAccess
{
    public class PackagingDAL
    {
        public static int AddPackagingJson(PackagingJson model)
        {
            IDatabase db = DataFactory.Database();
            DbTransaction dbTran = db.BeginTrans();
            try
            {
                int nRet = 0;
                if (!string.IsNullOrEmpty(model.RequestID.ToString()) && model.PackagingList.Count() > 0)
                {
                    if (isExistPackaging(db, model.RequestID) > 0)
                    {
                        DelPackaging(db, model.RequestID);
                    }
                    nRet = AddPackagingList(db, dbTran, model.RequestID, model.PackagingList);
                    if (nRet == 0 )
                    {
                        db.Rollback();
                        return 0;
                    }
                }
                dbTran.Commit();
                return 1;
            }
            catch (Exception ex)
            {
                dbTran.Rollback();
                return 0;
                throw ex;
            }
        }

        /// <summary>
        /// 包装信息 表单信息一条
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dbTran"></param>
        /// <param name="RequestID"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        private static int AddPackagingList(IDatabase db, DbTransaction dbTran, int RequestID, List<PackagingDetails> list)
        {

            int next = 0;
            foreach (var info in list)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into [Packaging] (");
                strSql.Append("RequestID,Denominator,AUoM,Numerator,BUoM,Length,Width,Height,UOMUnit,GrossWeight,GrossUnit)");
                strSql.Append(" values (");
                strSql.Append("@RequestID,@Denominator,@AUoM,@Numerator,@BUoM,@Length,@Width,@Height,@UOMUnit,@GrossWeight,@GrossUnit)");
                SqlParameter[] parameters = {
                    //new SqlParameter("@RequestID", SqlDbType.Int,4),
                    //new SqlParameter("@Denominator", SqlDbType.Int,4),
                    //new SqlParameter("@AUoM", SqlDbType.NVarChar,255),
                    //new SqlParameter("@Numerator", SqlDbType.Int,4),
                    //new SqlParameter("@BUoM", SqlDbType.NVarChar,255),
                    //new SqlParameter("@Length", SqlDbType.Int,4),
                    //new SqlParameter("@Width", SqlDbType.Int,4),
                    //new SqlParameter("@Height", SqlDbType.Int,4),
                    //new SqlParameter("@UOMUnit", SqlDbType.NVarChar,255),
                    //new SqlParameter("@GrossWeight", SqlDbType.Float,8),
                    //new SqlParameter("@GrossUnit", SqlDbType.NVarChar,255)};
                       new SqlParameter("@RequestID", SqlDbType.Int,4),
                    new SqlParameter("@Denominator", SqlDbType.VarChar,10),
                    new SqlParameter("@AUoM", SqlDbType.NVarChar,255),
                    new SqlParameter("@Numerator", SqlDbType.VarChar,10),
                    new SqlParameter("@BUoM", SqlDbType.NVarChar,255),
                    new SqlParameter("@Length", SqlDbType.VarChar,10),
                    new SqlParameter("@Width", SqlDbType.VarChar,10),
                    new SqlParameter("@Height", SqlDbType.VarChar,10),
                    new SqlParameter("@UOMUnit", SqlDbType.NVarChar,255),
                    new SqlParameter("@GrossWeight",SqlDbType.VarChar,10),
                    new SqlParameter("@GrossUnit", SqlDbType.NVarChar,255)};
                parameters[0].Value = RequestID;
                parameters[1].Value = info.Denominator;
                parameters[2].Value = info.AUoM;
                parameters[3].Value = info.Numerator;
                parameters[4].Value = info.BUoM;
                parameters[5].Value = info.Length;
                parameters[6].Value = info.Width;
                parameters[7].Value = info.Height;
                parameters[8].Value = info.UOMUnit;
                parameters[9].Value = info.GrossWeight;
                parameters[10].Value = info.GrossUnit;
                next = db.ExecuteBySql(strSql, parameters, dbTran);
                if (next == 0)
                {
                    return 0;
                }
            }

            return next;
        }

        /// <summary>
        /// requestid 是否有包装信息
        /// </summary>
        /// <param name="db"></param>
        /// <param name="dbTran"></param>
        /// <param name="RequestID"></param>
        /// <returns></returns>
        public static int isExistPackaging(IDatabase db,int RequestID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"select count(*) from [Packaging] where requestID=@RequestID
             ");
            SqlParameter[] parameters = {
                    new SqlParameter("@RequestID", SqlDbType.Int,4) };
            parameters[0].Value = RequestID;
            object obj = db.FindScalarBySql(strSql.ToString(), parameters);
            if (obj != null && obj != DBNull.Value)
            {
                return Convert.ToInt32(obj);
            }
            return 0;
        }


        public static int DelPackaging(IDatabase db, int RequestID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(@"delete  from  [Packaging] where requestID=@RequestID
             ");
            SqlParameter[] parameters = {
                    new SqlParameter("@RequestID", SqlDbType.Int,4) };
            parameters[0].Value = RequestID;
            object obj = db.FindScalarBySql(strSql.ToString(), parameters);
            if (obj != null && obj != DBNull.Value)
            {
                return Convert.ToInt32(obj);
            }
            return 0;
        }
    }
}
