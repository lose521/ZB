using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace ZB.FrameWork.WebApi
{
    public class CrossSiteAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
    //    app.all('*', function(req, res, next) {  
    //res.header("Access-Control-Allow-Origin", "*");  
    //res.header("Access-Control-Allow-Headers", "X-Requested-With");  
    //res.header("Access-Control-Allow-Methods","PUT,POST,GET,DELETE,OPTIONS");  
    //res.header("X-Powered-By",' 3.2.1')  
    //res.header("Content-Type", "application/json;charset=utf-8");  
    //next();  
  //app.all('*', function(req, res, next) {  
  //  res.header("Access-Control-Allow-Origin", "*");  
  //  res.header("Access-Control-Allow-Headers", "Content-Type");  
  //  res.header("Access-Control-Allow-Methods","PUT,POST,GET,DELETE,OPTIONS");
  //  next();  
 
        private const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
        private const string originHeaderdefault = "http://localhost:8000";
        public  void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
            actionExecutedContext.Response.Headers.Add("X-Powered-By", "PUT,POST,GET,DELETE,OPTIONS");
            //actionExecutedContext.Response.Headers.Add("Access-Control-Allow-Methods", "3.2.1");
            //actionExecutedContext.Response.Headers.Add("Content-Type", "application/json;charset=utf-8");
        }
    }
}
