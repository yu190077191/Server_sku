using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using WF.BusinessRule;
using WF.Framework;
using WF.Framework.Helper;
using WF.Model;

namespace WF.Web.Controllers
{
    public class VendorController : BaseController
    {
        public int Create(string type)
        {
            int id = RequestRule.InsertRequest(new Request { 
                Type = type
            });

            return id;
        }

        public ActionResult Index(int? id = null)
        {
            List<Field> list = null;
            if (id != null)
            {
                list = CommonRule.Get<Field>("[dbo].[GetRequest]", new[] { 
                new SqlParameter("@pagesize", 1000),
                new SqlParameter("@Id", id)
            });
            }

            return View(list);
        }

        public ActionResult Preview(int id)
        {
            List<Field> list = null;
            if (id != null)
            {
                list = CommonRule.Get<Field>("[dbo].[GetRequest]", new[] { 
                new SqlParameter("@pagesize", 1000),
                new SqlParameter("@Id", id)
            });
            }

            return View(list);
        }

        public int Save(int id, string json)
        {
            FieldNameValue[] array = JsonHelper.DeserializeJson<FieldNameValue[]>(json);
            string xml = SerializationHelper.Serialize(array);
            List<Request> li = CommonRule.Get<Request>("SaveRequest", new[]{
                new SqlParameter("@Id",id),
                new SqlParameter("@xml",xml),
                new SqlParameter("@CreatedBy", Operation.OperationBy)
            });

            return id;
        }
    }
}
