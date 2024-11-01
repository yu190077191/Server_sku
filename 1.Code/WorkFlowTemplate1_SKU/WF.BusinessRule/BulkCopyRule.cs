using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WF.DataAccess;
using WF.Model;

namespace WF.BusinessRule
{
    public class BulkCopyRule : System.Web.UI.Page
    {
        public static List<T> ConvertToList<T>(string filePath, Object model, int minColumnCount, out string error, out string columnNames)
        {
            return BulkCopyDao.ConvertToList<T>(filePath, model, minColumnCount, out error, out columnNames);
        }

        public static DataTable ExcelToStandardDataTable(string Path, Guid guid, ref string error)
        {
            return BulkCopyDao.ExcelToStandardDataTable(Path, guid, ref error);
        }

        public static DataSet ExcelToDataSet(string Path, bool hasHeaderRow = true)
        {
            return BulkCopyDao.ExcelToDataSet(Path, hasHeaderRow);
        }

        public static List<UploadResult> GetUploadResultList(string procedureName, SqlParameter[] sqlParamters)
        {
            return BulkCopyDao.GetUploadResultList(procedureName, sqlParamters);
        }

        public static string Upload(string fileFullPath, Guid UniqueId, string tableNames = null, string destinationTableName = "Temp_ImportedData", string storedProcedureName = "UploadDataByExcel")
        {
            return BulkCopyDao.Upload(fileFullPath, UniqueId, tableNames, destinationTableName, storedProcedureName);
        }

        public static List<UploadResult> GetUploadResult(Guid UniqueId, string tableNames = null, string storedProcedureName = "UploadDataByExcel")
        {
            return BulkCopyDao.GetUploadResult(UniqueId, tableNames, storedProcedureName);
        }
    }
}
