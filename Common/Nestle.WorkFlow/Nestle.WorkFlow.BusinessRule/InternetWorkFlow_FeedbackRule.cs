using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class InternetWorkFlow_FeedbackRule : System.Web.UI.Page
    {
        public static int InsertInternetWorkFlow_Feedback(InternetWorkFlow_Feedback model)
        {
            return InternetWorkFlow_FeedbackDao.InsertInternetWorkFlow_Feedback(model);
        }

        public static int UpdateInternetWorkFlow_Feedback(InternetWorkFlow_Feedback model)
        {
            return InternetWorkFlow_FeedbackDao.UpdateInternetWorkFlow_Feedback(model);
        }

        public static List<InternetWorkFlow_Feedback> GetInternetWorkFlow_Feedback(string procedureName, SqlParameter[] sqlParamters)
        {
            return InternetWorkFlow_FeedbackDao.GetInternetWorkFlow_Feedback(procedureName, sqlParamters);
        }


        public static QueryResult<InternetWorkFlow_Feedback> GetInternetWorkFlow_FeedbackResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return InternetWorkFlow_FeedbackDao.GetInternetWorkFlow_FeedbackResult(procedureName, sqlParamters);
        }
    }
}
