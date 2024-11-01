using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class EventRule : System.Web.UI.Page
    {
        public static int InsertEvent(Event model)
        {
            return EventDao.InsertEvent(model);
        }

        public static int UpdateEvent(Event model)
        {
            return EventDao.UpdateEvent(model);
        }

        public static List<Event> GetEvent(string procedureName, SqlParameter[] sqlParamters)
        {
            return EventDao.GetEvent(procedureName, sqlParamters);
        }


        public static QueryResult<Event> GetEventResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return EventDao.GetEventResult(procedureName, sqlParamters);
        }
    }
}
