using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Nestle.WorkFlow.DataAccess
{
    public static class SqlDbTypeConvert
    {
        // sql server数据类型（如：varchar）
        // 转换为SqlDbType类型
        public static SqlDbType SqlTypeString2SqlType(string sqlTypeString)
        {
            SqlDbType dbType = SqlDbType.Variant;//默认为Object

            switch (sqlTypeString)
            {
                case "int":
                    dbType = SqlDbType.Int;
                    break;
                case "varchar":
                    dbType = SqlDbType.VarChar;
                    break;
                case "bit":
                    dbType = SqlDbType.Bit;
                    break;
                case "datetime":
                    dbType = SqlDbType.DateTime;
                    break;
                case "decimal":
                    dbType = SqlDbType.Decimal;
                    break;
                case "float":
                    dbType = SqlDbType.Float;
                    break;
                case "image":
                    dbType = SqlDbType.Image;
                    break;
                case "money":
                    dbType = SqlDbType.Money;
                    break;
                case "ntext":
                    dbType = SqlDbType.NText;
                    break;
                case "nvarchar":
                    dbType = SqlDbType.NVarChar;
                    break;
                case "smalldatetime":
                    dbType = SqlDbType.SmallDateTime;
                    break;
                case "smallint":
                    dbType = SqlDbType.SmallInt;
                    break;
                case "text":
                    dbType = SqlDbType.Text;
                    break;
                case "bigint":
                    dbType = SqlDbType.BigInt;
                    break;
                case "binary":
                    dbType = SqlDbType.Binary;
                    break;
                case "char":
                    dbType = SqlDbType.Char;
                    break;
                case "nchar":
                    dbType = SqlDbType.NChar;
                    break;
                case "numeric":
                    dbType = SqlDbType.Decimal;
                    break;
                case "real":
                    dbType = SqlDbType.Real;
                    break;
                case "smallmoney":
                    dbType = SqlDbType.SmallMoney;
                    break;
                case "sql_variant":
                    dbType = SqlDbType.Variant;
                    break;
                case "timestamp":
                    dbType = SqlDbType.Timestamp;
                    break;
                case "tinyint":
                    dbType = SqlDbType.TinyInt;
                    break;
                case "uniqueidentifier":
                    dbType = SqlDbType.UniqueIdentifier;
                    break;
                case "varbinary":
                    dbType = SqlDbType.VarBinary;
                    break;
                case "xml":
                    dbType = SqlDbType.Xml;
                    break;
            }
            return dbType;
        }

        // SqlDbType转换为C#数据类型
        public static string SqlType2CsharpType(SqlDbType sqlType)
        {
            switch (sqlType)
            {
                case SqlDbType.BigInt:
                    return "int";
                case SqlDbType.Binary:
                    return "byte[]";
                case SqlDbType.Bit:
                    return "bool";
                case SqlDbType.Char:
                    return "string";
                case SqlDbType.DateTime:
                    return "DateTime";
                case SqlDbType.Decimal:
                    return "decimal";
                case SqlDbType.Float:
                    return "double";
                case SqlDbType.Image:
                    return "byte[]";
                case SqlDbType.Int:
                    return "int";
                case SqlDbType.Money:
                    return "decimal";
                case SqlDbType.NChar:
                    return "string";
                case SqlDbType.NText:
                    return "string";
                case SqlDbType.NVarChar:
                    return "string";
                case SqlDbType.Real:
                    return "Single";
                case SqlDbType.SmallDateTime:
                    return "DateTime";
                case SqlDbType.SmallInt:
                    return "int";
                case SqlDbType.SmallMoney:
                    return "decimal";
                case SqlDbType.Text:
                    return "string";
                case SqlDbType.Timestamp:
                    return "string";
                case SqlDbType.TinyInt:
                    return "int";
                case SqlDbType.Udt://自定义的数据类型
                    return "Object";
                case SqlDbType.UniqueIdentifier:
                    return "Guid";
                case SqlDbType.VarBinary:
                    return "byte[]";
                case SqlDbType.VarChar:
                    return "string";
                case SqlDbType.Variant:
                    return "Object";
                case SqlDbType.Xml:
                    return "Object";
                default:
                    return null;
            }
        }

        public static string SqlTypeString2CsharpTypeString(string sqlDbTypeString)
        {
            return SqlType2CsharpType(SqlTypeString2SqlType(sqlDbTypeString));
        }

        public static string CSharpSystemTypeToSqlTypeString(string csharpTypeString)
        {
            switch (csharpTypeString)
            {
                case "System.String": return "NVARCHAR(MAX)";
                case "System.Int64": return "bigint";
                case "System.Object": return "varbinary";
                case "System.Boolean": return "bit";
                case "System.DateTime": return "datetime";
                case "System.Decimal": return "decimal";
                case "System.Double": return "float";
                case "System.Byte[]": return "varbinary";
                case "System.Int32": return "int";
                case "System.Single": return "real";
                case "System.Int16": return "smallint";
                case "System.Byte": return "tinyint";
                case "System.Guid": return "uniqueidentifier";
            }

            return "NVARCHAR(MAX)";
        }
    }
}