using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using Nestle.WorkFlow.Framework.Helper;

namespace Nestle.WorkFlow.Framework
{
    public class JsonQuery
    {
        public NameValue[] Array
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
    }
}