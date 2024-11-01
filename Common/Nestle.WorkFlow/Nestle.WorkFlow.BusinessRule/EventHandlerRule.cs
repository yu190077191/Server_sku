using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Nestle.WorkFlow.DataAccess;
using Nestle.WorkFlow.Framework;
using Nestle.WorkFlow.Model;

namespace Nestle.WorkFlow.BusinessRule
{
    public class EventHandlerRule : System.Web.UI.Page
    {
        public static int InsertEventHandler(Nestle.WorkFlow.Model.EventHandler model)
        {
            return EventHandlerDao.InsertEventHandler(model);
        }

        public static int UpdateEventHandler(Nestle.WorkFlow.Model.EventHandler model)
        {
            return EventHandlerDao.UpdateEventHandler(model);
        }

        public static List<Nestle.WorkFlow.Model.EventHandler> GetEventHandler(string procedureName, SqlParameter[] sqlParamters)
        {
            return EventHandlerDao.GetEventHandler(procedureName, sqlParamters);
        }


        public static QueryResult<Nestle.WorkFlow.Model.EventHandler> GetEventHandlerResult(string procedureName, SqlParameter[] sqlParamters)
        {
            return EventHandlerDao.GetEventHandlerResult(procedureName, sqlParamters);
        }
    }
}
