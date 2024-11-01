using System.Collections.Generic;

namespace Nestle.WorkFlow.Framework
{
    public class QueryResult<T>
    {
        public T[] DataArray
        {
            get;
            set;
        }

        public List<T> DataList { get; set; }

        public int Count
        {
            get;
            set;
        }
    }
}
