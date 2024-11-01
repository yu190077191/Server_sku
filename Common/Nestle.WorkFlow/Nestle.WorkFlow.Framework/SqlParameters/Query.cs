using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using Nestle.WorkFlow.Framework.Helper;

namespace Nestle.WorkFlow.Framework
{
    public class Query
    {
        private int? pageIndex;
        private string json;
        private bool addUserId;
        private bool addPageIndexAndPageSize;
        public Query(int? pageIndex = null, string json = null, bool addUserId = true, bool addPageIndexAndPageSize = true)
        {
            Conditions = new NameValueCollection();
            this.pageIndex = pageIndex;
            this.json = json;
            this.addUserId = addUserId;
            this.addPageIndexAndPageSize = addPageIndexAndPageSize;
        }

        public Query(string json)
        {
            Conditions = new NameValueCollection();
            this.json = json;
            this.addUserId = true;
            this.addPageIndexAndPageSize = false;
        }

        public NameValueCollection Conditions
        {
            get;
            set;
        }

        public System.Guid? OperationBy
        {
            get;
            set;
        }

        public int PageSize
        {
            get;
            set;
        }

        public int PageIndex
        {
            get;
            set;
        }

        public int? RecordStatus
        {
            get;
            set;
        }

        public string Keyword
        {
            get;
            set;
        }

        public SqlParameter[] GetParameters(string siteName = null, System.Guid? siteGuid = null, int? _pageIndex = 0, string _json = null)
        {
            if (_pageIndex != null && _pageIndex > 0)
            {
                pageIndex = _pageIndex;
            }

            if (!string.IsNullOrEmpty(_json))
            {
                json = _json;
            }

            List<SqlParameter> li = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(json))
            {
                JsonQuery query = JsonHelper.DeserializeJson<JsonQuery>(json);
                this.PageSize = query.PageSize;
                this.PageIndex = query.PageIndex;
                this.Keyword = query.Keyword;
                this.RecordStatus = query.RecordStatus;
                if (query.Array != null)
                {
                    foreach (NameValue item in query.Array)
                    {
                        var para = new SqlParameter(item.Name, item.Value);
                        li.Add(para);
                    }
                }
            }

            if (RecordStatus != null)
            {
                li.Add(new SqlParameter("@recordStatus", RecordStatus));
            }

            if (OperationBy != null)
            {
                li.Add(new SqlParameter("@CreatedBy", OperationBy));
            }
            else if (addUserId)
            {
                li.Add(new SqlParameter("@CreatedBy", Operation.OperationBy));
            }

            if (pageIndex != null && pageIndex > 0)
            {
                PageIndex = (int)pageIndex;
            }

            if (PageIndex == 0 && addPageIndexAndPageSize)
            {
                PageIndex = 1;
            }

            if (PageIndex > 0)
            {
                li.Add(new SqlParameter("@pageIndex", PageIndex));
            }

            if (PageSize == 0 && addPageIndexAndPageSize)
            {
                PageSize = Constants.PageSize;
            }

            if (PageSize > 0)
            {
                li.Add(new SqlParameter("@pageSize", PageSize));
            }

            if (!string.IsNullOrEmpty(Keyword))
            {
                li.Add(new SqlParameter("@keyword", Keyword));
            }

            if (!string.IsNullOrEmpty(siteName))
            {
                li.Add(new SqlParameter("@SiteName", siteName));
            }

            if (siteGuid != null)
            {
                li.Add(new SqlParameter("@SiteGuid", siteGuid));
            }

            if (Conditions != null)
            {
                foreach (string key in Conditions.Keys)
                {
                    string value = Conditions.Get(key);
                    if (!string.IsNullOrEmpty(value))
                    {
                        var para = new SqlParameter(key, value);
                        li.Add(para);
                    }
                }
            }

            return li.ToArray();
        }
    }
}