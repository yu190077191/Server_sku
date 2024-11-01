using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class EventContainerRule : System.Web.UI.Page
    {
        public static int InsertEventContainer(EventContainer model)
        {
            return EventContainerDao.InsertEventContainer(model);
        }

        public static int UpdateEventContainer(EventContainer model)
        {
            return EventContainerDao.UpdateEventContainer(model);
        }

        public static List<EventContainer> GetEventContainer(string procedureName, SqlParameter[] sqlParamters)
        {
            return EventContainerDao.GetEventContainer(procedureName, sqlParamters);
        }


        public static QueryResult<EventContainer> GetEventContainerResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return EventContainerDao.GetEventContainerResult(procedureName, sqlParamters);
        }
    }
}
