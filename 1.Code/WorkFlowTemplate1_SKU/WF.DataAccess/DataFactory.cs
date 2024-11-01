using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.DataAccess
{
    /// <summary>
    /// 操作数据库工厂
    /// </summary>
    public class DataFactory
    {
        /// <summary>
        /// 当前数据库类型
        /// </summary>
        private static string DbType = "SqlServer";

        /// <summary>
        /// 获取指定的数据库连接
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static IDatabase Database(string connString)
        {
            return new Database(connString);
        }
        /// <summary>
        /// 获取指定的数据库连接
        /// </summary>
        /// <returns></returns>
        public static IDatabase Database()
        {
            switch (DbType)
            {
                case "SqlServer":
                    return Database("ProjectManagementDatabase2");
                case "MySql":
                    return Database("MySql");
                case "Oracle":
                    return Database("Oracle");
                default:
                    return null;
            }
        }
    }
}
