using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Xml;
using WF.Framework.Helper;
using WF.Model;

namespace WF.Framework
{
    public static class Operation
    {
        public static Employee CreateByName(string userName)
        {
            string sessionName = "EmployeeInfo";
            string procedureName = "GetEmployee";
            using (var connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ProjectManagementDatabase2"].ConnectionString))
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
        public static string UserName
        {
            get
            {
                if (HttpContext.Current.Session["debugAccountName"] != null)
                {
                    return "NESTLE\\" + HttpContext.Current.Session["debugAccountName"].ToString();
                }

                if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    return "CNSunJi1";
                }
                return HttpContext.Current.User.Identity.Name;
            }
        }

        public static void Simulate(string userName)
        {
            HttpContext.Current.Session.Remove("EmployeeInfo");
            HttpContext.Current.Session.Remove("OperationBy");
            HttpContext.Current.Session.Remove(Constants.DelegatedBy);
            HttpContext.Current.Session.Remove("Nestle.WorkFlow.Employee");
            HttpContext.Current.Session["debugAccountName"] = userName;
            Employee e = GetMyEmployeeInfo(false, true);
        }

        public static bool IsInDebugMode()
        {
            return HttpContext.Current.User.Identity.Name != UserName;
        }

        public static Guid? OperationBy
        {
            get
            {
                return Employee.Id;
            }
        }

        public static Employee Employee
        {
            get
            {
                return GetMyEmployeeInfo();
            }
        }

        public static Employee RealUser
        {
            get
            {
                return GetMyEmployeeInfo(true, false, "RealUserEmployeeInfo");
            }
        }

        private static SqlParameter[] GetSqlParameter(bool isWindowsIdentity)
        {
            string userName = !isWindowsIdentity ? UserName : HttpContext.Current.User.Identity.Name;
            return new[] { new SqlParameter("@AccountName", userName) };
        }

        const string delegateLoadCompleted = "delegateLoadCompleted";
        private static bool IsSessionValid(string sessionName)
        {
            if (HttpContext.Current.Session != null && HttpContext.Current.Session[sessionName] != null)
            {
                if (HttpContext.Current.Session[Constants.DelegatedBy] != null && HttpContext.Current.Session[delegateLoadCompleted] == null)
                {
                    HttpContext.Current.Session.Remove("EmployeeInfo");
                    HttpContext.Current.Session.Remove("OperationBy");
                    HttpContext.Current.Session.Remove("Nestle.WorkFlow.Employee");
                    Employee e = GetMyEmployeeInfo(false, true);
                    HttpContext.Current.Session[delegateLoadCompleted] = true;
                }
                else if (HttpContext.Current.Session[Constants.DelegatedBy] == null && HttpContext.Current.Session[delegateLoadCompleted] != null)
                {
                    HttpContext.Current.Session.Remove(delegateLoadCompleted);
                }

                return true;
            }

            return false;
        }

        public static Employee GetMyEmployeeInfo(bool isWindowsIdentity = false, bool refresh = false, string sessionName = "EmployeeInfo")
        {
            if (IsSessionValid(sessionName) && !refresh)
            {
                return (Employee)HttpContext.Current.Session[sessionName];
            }

            using (var connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ProjectManagementDatabase2"].ConnectionString))
            {
                XmlReader reader = SqlHelper.ExecuteXmlReader(connection, CommandType.StoredProcedure, "GetEmployee", GetSqlParameter(isWindowsIdentity));
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

        public static string DelegatedBy
        {
            get
            {
                string sessionName = Constants.DelegatedBy;
                if (HttpContext.Current.Session != null && HttpContext.Current.Session[sessionName] != null)
                {
                    return HttpContext.Current.Session[sessionName].ToString();
                }

                return string.Empty;
            }
        }

        public static bool IsDelegated
        {
            get
            {
                return !string.IsNullOrEmpty(DelegatedBy);
            }
        }
    }
}