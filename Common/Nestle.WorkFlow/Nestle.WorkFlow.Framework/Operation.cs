using Nestle.WorkFlow.Framework.Helper;
using Nestle.WorkFlow.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Xml;

namespace Nestle.WorkFlow.Framework
{
    public static class Operation
    {
        public static bool IsSystemAdmin
        {
            get
            {
                string admins = "," + System.Configuration.ConfigurationManager.AppSettings["SystemAdmins"] + ",";
                return admins.ToLower().Contains("," + HttpContext.Current.User.Identity.Name.Replace("NESTLE\\", "").ToLower() + ",");
            }
        }

        public static string UserName
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return null;
                }

                if (HttpContext.Current.Session["debugAccountName"] != null)
                {
                    return "NESTLE\\" + HttpContext.Current.Session["debugAccountName"].ToString();
                }

                return HttpContext.Current.User.Identity.Name;
            }
        }

        public static Guid? OperationBy
        {
            get
            {
                return Guid.Parse("CA3B880B-531D-497A-BD35-3E592E2E4C81");
                if (Employee != null)
                {
                    return Employee.Id;
                }
                else
                {
                    return null;
                }
            }
        }

        public static Employee Employee
        {
            get
            {
                return GetEmployeeInfo();
            }
        }

        private static SqlParameter[] GetSqlParameter()
        {
            string userName = HttpContext.Current.User.Identity.Name;
            return new[] { new SqlParameter("@AccountName", UserName) };
        }

        private static bool IsSessionValid(string sessionName)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session[sessionName] != null)
            {
                return true;
            }

            return false;
        }

        public static Employee GetEmployeeInfo()
        {
            string sessionName = Constants.EmployeeSessionName;
            if (IsSessionValid(sessionName))
            {
                return (Employee)HttpContext.Current.Session[sessionName];
            }
            else if (HttpContext.Current == null)
            {
                return null;
            }

            using (var connection = new SqlConnection(Constants.ConnectionString))
            {
                XmlReader reader = SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, "GetEmployee", GetSqlParameter());
                XmlDocument xmlDocument = XmlHelper.LoadXml(reader);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNode node = xmlDocument.FirstChild.SelectSingleNode("//Employee");
                    Employee employee = SerializationHelper.Deserialize<Employee>(node.OuterXml);
                    if (HttpContext.Current.Session != null)
                    {
                        HttpContext.Current.Session[sessionName] = employee;
                    }

                    return employee;
                }
            }

            return null;
        }

        public static Employee CreateByName(string userName)
        {
            string sessionName = "EmployeeInfo";
            string procedureName = "GetEmployee";
            using (var connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CreditReleaseRequestDatabase"].ConnectionString))
            {
                XmlReader reader = SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, procedureName, new[] { new SqlParameter("@AccountName", userName) });
                XmlDocument xmlDocument = XmlHelper.LoadXml(reader);
                if (xmlDocument.HasChildNodes)
                {
                    XmlNode node = xmlDocument.FirstChild.SelectSingleNode("//Employee");
                    Employee employee = SerializationHelper.Deserialize<Employee>(node.OuterXml);
                    if (HttpContext.Current.Session != null)
                    {
                        HttpContext.Current.Session[sessionName] = employee;
                    }

                    return employee;
                }
            }

            return null;
        }
    }
}