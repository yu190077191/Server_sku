using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class EmailTemplateRule : System.Web.UI.Page
    {
        public static int InsertEmailTemplate(EmailTemplate model)
        {
            return EmailTemplateDao.InsertEmailTemplate(model);
        }

        public static int UpdateEmailTemplate(EmailTemplate model)
        {
            return EmailTemplateDao.UpdateEmailTemplate(model);
        }

        public static List<EmailTemplate> GetEmailTemplate(string procedureName, SqlParameter[] sqlParamters)
        {
            return EmailTemplateDao.GetEmailTemplate(procedureName, sqlParamters);
        }


        public static QueryResult<EmailTemplate> GetEmailTemplateResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return EmailTemplateDao.GetEmailTemplateResult(procedureName, sqlParamters);
        }
    }
}
