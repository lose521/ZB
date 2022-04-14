using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZB.Common.Entity
{

    public class ResponsePage
    {
        public int pageCount { get; set; }
        public int totalCount { get; set; }
    }

    public class ListData<T>
    {
        public List<T> list { get; set; }
        public ResponsePage page { get; set; }
    }

    //public class RequestList
    //{
    //    public RequestPage page { get; set; }
    //}

    public class RequestPage
    {
        public int current { get; set; }
        public int pageSize { get; set; }
    }

}
 