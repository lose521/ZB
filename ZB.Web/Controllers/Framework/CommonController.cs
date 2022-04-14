using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZB.Common.Handler;
using ZB.EntityFramework.SqlServer;

namespace ZB.Web.Controllers.Framework
{
    public class CommonController : ApiController
    {
        //public virtual HttpResponseMessage GetType(string type)//int current, int pageSize,string condition
        //{
        //    using (EFContext entities = new EFContext())
        //    {
        //        var lst = entities.cm_typeDetail.Where(e => e.Type == type && e.Status == "active").Select(e => new { e.Code, e.Name }).ToList();
        //        return WebApi.GetSuccessHttpResponseMessage(lst);
        //    }
        //}
    }
}
