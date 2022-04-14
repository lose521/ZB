using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using ZB.Common.Handler;
using ZB.FrameWork.WebApi;


namespace ZB
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var globalCors = new System.Web.Http.Cors.EnableCorsAttribute("*", "*", "*");
            config.EnableCors(globalCors);
            // Web API 配置和服务
            // 将 Web API 配置为仅使用不记名令牌身份验证。
           // config.SuppressDefaultHostAuthentication();
           // config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API 路由
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
            config.Routes.MapHttpRoute("ActionApi", "api/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional, action = RouteParameter.Optional });
            config.Filters.Add(new WebApiExceptionFilterAttribute());
            config.Filters.Add(new CrossSiteAttribute());

        }
    }
}
