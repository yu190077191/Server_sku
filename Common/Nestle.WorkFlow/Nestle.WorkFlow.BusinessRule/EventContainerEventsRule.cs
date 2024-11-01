using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class EventContainerEventsRule : System.Web.UI.Page
    {
        public static int InsertEventContainerEvents(EventContainerEvents model)
        {
            return EventContainerEventsDao.InsertEventContainerEvents(model);
        }

        public static int UpdateEventContainerEvents(EventContainerEvents model)
        {
            return EventContainerEventsDao.UpdateEventContainerEvents(model);
        }

        public static List<EventContainerEvents> GetEventContainerEvents(string procedureName, SqlParameter[] sqlParamters)
        {
            return EventContainerEventsDao.GetEventContainerEvents(procedureName, sqlParamters);
        }


        public static QueryResult<EventContainerEvents> GetEventContainerEventsResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return EventContainerEventsDao.GetEventContainerEventsResult(procedureName, sqlParamters);
        }
    }
}
