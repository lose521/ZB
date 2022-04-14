using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using ZB.Business;
using ZB.Common.Handler;
using ZB.EntityFramework.SqlServer;
using ZB.FrameWork;
using ZB.FrameWork.Access;
using ZB.FrameWork.WebApi;
using NLog;
namespace ZB.Web.Controllers
{
    /// <summary>
    /// 后台验证不通过 WebApi.GetErrorHttpResponseMessage("验证信息不通过")
    /// 成功WebApi.GetSuccessHttpResponseMessage(lst)
    /// </summary>
   // [TokenAuthentication]
    public class TestController : BaseApiController
    {
        private static Logger _logger = LogManager.GetLogger("default");
        public virtual HttpResponseMessage GetTest()
        {
            try
            {
                EFContext entities = new EFContext();
                var lst = entities.sys_user.ToList();
                //return WebApi.GetErrorHttpResponseMessage("Dddd");
                //return WebApi.GetSuccessHttpResponseMessage(lst);
                //return Request.CreateResponse<List<sys_user>>(HttpStatusCode.OK, lst);
                //HttpResponseMessage httpResponseMessage = new HttpResponseMessage();
                //string jsonString = JsonConvert.SerializeObject(new { Success = true, Data = lst });
                //httpResponseMessage.Content = new StringContent(jsonString, Encoding.GetEncoding("UTF-8"), "application/json");
                //return httpResponseMessage;
                var t = new sys_user { userId=1,loginName="admin"};
                return WebApi.GetSuccessHttpResponseMessage(t);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //[TokenAuthentication]
        public virtual HttpResponseMessage GetTest1(string name)
        {
            try
            {
                var t = new sys_user { userId = 1, loginName = name };
                return WebApi.GetSuccessHttpResponseMessage(t);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        
        [HttpPost]
        public  HttpResponseMessage Login(sys_user user)
        {
            try
            {
                //var cookieValues = Request.Headers.GetCookies();
                //var cookie = Request.Headers.GetCookies("session").FirstOrDefault();
                //if (cookie != null)
                //{
                //    string sessionId = cookie["session"].Value;
                //}
                bool isEver = true;
                string name = user.loginName;
                string pw = user.password;
                using (EFContext context = new EFContext())
                {
                    sys_user sysUser = context.sys_user.SingleOrDefault(e => e.loginName == name);
                    if (sysUser == null)
                    {
                        throw new Exception("没找到用户");
                    }
                    UserContext model = RegisterUserContext(sysUser, isEver);
                    return WebApi.GetSuccessHttpResponseMessage(model);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        UserContext RegisterUserContext(sys_user user, bool isEver)
        {
            //服务端注册用户信息
            string appId = RondomValue.CreateAppId();
            string apiKey = RondomValue.CreateApiKey();
            int userId = user.userId;
            var loginName = user.loginName;
            DateTime expire = isEver ? DateTime.Now.AddYears(1) : DateTime.Now.AddSeconds(30);
            var userContext = new UserContext(loginName, userId, appId, apiKey);
            userContext.LastKeepTime = expire;
            UserContextManager.Register(userContext);
            //返回客户端用户信息
            //var timeoutMinutes = SecurityConfiguration.CredentialValidPeriod; // 用户凭据有效期，单位：分钟
            //if (timeoutMinutes == 0) // 默认一天
            //{
            //    var maxLoginTimeoutMinutes = 60 * 24 * 1; // 1天
            //    timeoutMinutes = maxLoginTimeoutMinutes;
            //}
            // 清权限缓存
            //AccessHelper.ReloadAccessCache(userContext.UserId);
            var model = new UserContext
            {
                LoginName = userContext.LoginName,
                UserId = userContext.UserId,
                AppId = userContext.AppId,
                ApiKey = userContext.ApiKey,
                LastKeepTime = expire
            };
            return model;
        }

        [HttpPost]
        public HttpResponseMessage Logout()
        {
            // 清除当前用户登录凭据，用户需重新登录才能正常调用API
            if (CurrentUserContext != null)
                UserContextManager.Unregister(CurrentUserContext.AppId);

            return WebApi.GetSuccessHttpResponseMessage(true);
        }

        [HttpGet]
        public HttpResponseMessage GetAccessNo()
        {
            _logger.Info("111");
            return WebApi.GetSuccessHttpResponseMessage("ok");
        }

        [TokenAuthentication]
        [HttpGet]
        public HttpResponseMessage GetAccess()
        {
            return WebApi.GetSuccessHttpResponseMessage("ok");
        }

        [HttpPost]
        public HttpResponseMessage PostAccessNo(sys_user user)
        {
            return WebApi.GetSuccessHttpResponseMessage("ok");
        }

        [TokenAuthentication]
        [HttpPost]
        public HttpResponseMessage PostAccess(sys_user user)
        {
            return WebApi.GetSuccessHttpResponseMessage("ok");
        }
    }
}
