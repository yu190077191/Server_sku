using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;


namespace WF.DataAccess
{
    public class DatabaseReader
    {
        /// <summary>
        ///  将IDataReader转换为DataTable
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static DataTable ReaderToDataTable(IDataReader dr)
        {
            using (dr)
            {
                DataTable objDataTable = new DataTable("Table");
                int intFieldCount = dr.FieldCount;
                for (int intCounter = 0; intCounter < intFieldCount; ++intCounter)
                {
                    objDataTable.Columns.Add(dr.GetName(intCounter).ToLower(), dr.GetFieldType(intCounter));
                }
                objDataTable.BeginLoadData();
                object[] objValues = new object[intFieldCount];
                while (dr.Read())
                {
                    dr.GetValues(objValues);
                    objDataTable.LoadDataRow(objValues, true);
                }
                dr.Close();
                dr.Dispose();
                objDataTable.EndLoadData();
                return objDataTable;
            }
        }

    }
}
