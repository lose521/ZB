using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Security.Principal;
using System.Web;
using ZB.FrameWork.Cache;
using ZB.Common.Handler;
namespace ZB.FrameWork.Access
{
    public class UserContextManager
    {
        private static int _maxLoginTimeoutMinutes = 60 * 24 * 1; // 1天

        // replace with cache provider
        private static ConcurrentDictionary<string, UserContext> _userContexts =
            new ConcurrentDictionary<string, UserContext>();

        /// <summary>
        /// 当前用户凭据
        /// </summary>
        [ThreadStatic]
        public static GenericPrincipal CurrentPrincipal;

        public static UserContext CurrentUserContext
        {
            get
            {
                var appId = CurrentAppId;
                if (string.IsNullOrEmpty(appId))
                    return null;
                return UserContextManager.GetUserContext(appId);
            }
        }

        private static string[] GetAutherizationHeaderValues(string rawAuthzHeader)
        {
            var credArray = rawAuthzHeader.Split(':');

            if (credArray.Length == 3)
            {
                return credArray;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 当前用户ID，如果找不到，返回-1
        /// </summary>
        public static int CurrentUserId
        {
            get
            {
                var currentUserContext = CurrentUserContext;
                if (currentUserContext == null)
                    return -1;
                return currentUserContext.UserId;
            }
        }

        /// <summary>
        /// 当前登录名，如果找不到，返回null
        /// </summary>
        public static string CurrentLoginName
        {
            get
            {
                var currentUserContext = CurrentUserContext;
                if (currentUserContext == null)
                    return null;
                return currentUserContext.LoginName;
            }
        }

        /// <summary>
        /// 当前用户名，如果找不到，返回null
        /// </summary>
        //public static string CurrentUserName
        //{
        //    get
        //    {
        //        var currentUserContext = CurrentUserContext;
        //        if (currentUserContext == null)
        //            return null;
        //        return currentUserContext.UserName;
        //    }
        //}
        /// <summary>
        /// 当前语言，如果找不到，返回cn
        /// </summary>
        public static string CurrentLanguage
        {
            get
            {
                var currentUserContext = CurrentUserContext;
                if (currentUserContext == null)
                    return null;
                var language = currentUserContext.CurrentLanguage;
                return language == "en" ? "en" : "cn";
            }
        }

        /// <summary>
        /// 当前语言，如果找不到，返回cn
        /// </summary>
        public static string CurrentAppId
        {
            get
            {
                if (HttpContext.Current == null)
                    return null;
                var request = HttpContext.Current.Request;
                var authorization = request.Headers["Authorization"];
                var scheme = string.Empty;
                var parameter = string.Empty;
                if (!string.IsNullOrEmpty(authorization) && authorization.Contains(" "))
                {
                    scheme = authorization.Split(' ').First();
                    parameter = authorization.Split(' ').Last();
                }

                if (authorization != null &&
                    scheme.Contains(Config.AppName)) // IFCA or IFCAMOBILE
                //Constant.AUTHENTICATION_SCHEME.Equals(scheme, StringComparison.OrdinalIgnoreCase))
                {
                    var autherizationHeaderArray = GetAutherizationHeaderValues(parameter);
                    if (autherizationHeaderArray != null)
                    {
                        string appId = autherizationHeaderArray[0];
                        return appId;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 注册用户上下文
        /// </summary>
        /// <param name="userContext"></param>
        public static void Register(UserContext userContext)
        {
            try
            {
                //var timeoutMinutes = SecurityConfiguration.CredentialValidPeriod; // 用户凭据有效期，单位：分钟
                //if (timeoutMinutes == 0) // 不过期
                //    timeoutMinutes = _maxLoginTimeoutMinutes;

                //// 保存进缓存
                //var expireTime = DateTime.Now.AddMinutes(timeoutMinutes);
                //CacheFactoryHelper.Store(userContext.AppId, userContext, expireTime);
                CacheFactoryHelper.Store(userContext.AppId, userContext, userContext.LastKeepTime);
               
               UserInfo.RegisterSession(userContext.AppId, userContext.UserId,userContext.LastKeepTime);
               // var context = HttpContext.Current;
               // var remark = string.Empty;
               //if (context != null)
               //{
               //    var remarkBuilder = new StringBuilder();
               //    remarkBuilder.AppendLine(string.Format("ClientType:{0} ", userContext.ClientType.ToString()));
               //    remarkBuilder.AppendLine(string.Format("UserAgent:{0} ", context.Request.UserAgent));
               //    remark = remarkBuilder.ToString();
               //}

                try
                {
                    // 保存进数据库
//                    using (var dbHelper = new DbHelper())
//                    {
//                        var sql = @"
//insert into Sys_UserContext(Userid,	Loginname,	Appid,	Apikey,	Createtime, Expiretime, Lastkeeptime, Remark,UserName)
//values(@Userid,	@Loginname,	@Appid,	@Apikey, @Createtime, @Expiretime, @Lastkeeptime, @Remark,@UserName)
//";
//                        if (dbHelper.IsOracle())
//                            sql = sql.Replace("@", ":");

//                        var cmd = dbHelper.CreateCommand(sql);
//                        dbHelper.AddInParameter(cmd, "Userid", DbType.Int32, userContext.UserId);
//                        dbHelper.AddInParameter(cmd, "LoginName", DbType.String, userContext.LoginName);
//                        dbHelper.AddInParameter(cmd, "Appid", DbType.String, userContext.AppId);
//                        dbHelper.AddInParameter(cmd, "Apikey", DbType.String, userContext.ApiKey);
//                        dbHelper.AddInParameter(cmd, "Createtime", DbType.DateTime, DateTime.Now);
//                        dbHelper.AddInParameter(cmd, "Expiretime", DbType.DateTime, expireTime);
//                        dbHelper.AddInParameter(cmd, "Lastkeeptime", DbType.DateTime, DateTime.Now);
//                        dbHelper.AddInParameter(cmd, "Remark", DbType.String, remark);
//                        dbHelper.AddInParameter(cmd, "UserName", DbType.String, userContext.UserName);
//                        var result = dbHelper.ExecuteNonQuery(cmd);
//                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static UserContext GetUserContext(string appId)
        {
            if (HttpContext.Current == null)
                return null;

            UserContext userContext;
            userContext = CacheFactoryHelper.Get<UserContext>(appId);
            if (userContext != null)
            {
                var currentLanguage = HttpContext.Current.Request.Headers["CurrentLanguage"];
                if (!string.IsNullOrEmpty(currentLanguage))
                    userContext.CurrentLanguage = (currentLanguage.Equals("en",StringComparison.OrdinalIgnoreCase)) ? "en" : "cn";
                return userContext;
            }
            else
            {
                return null;
//                try
//                {
//                    // 读数据库
//                    using (var dbHelper = new DbHelper())
//                    {
//                        var sql = @"
//select Userid, Loginname, Apikey, Expiretime,Username from Sys_UserContext where Appid = @Appid and Expiretime > getdate()";
//                        var cmd = dbHelper.CreateCommand(sql);
//                        dbHelper.AddInParameter(cmd, "Appid", System.Data.DbType.String, appId);
//                        var table = dbHelper.ExecuteDataTable(cmd);
//                        if (table == null || table.Rows.Count == 0)
//                        {
//                            return null;
//                        }
//                        var row = table.Rows[0];
//                        userContext = new UserContext(row["Loginname"].ToStr(), row["Userid"].ToInt(), appId, row["Apikey"].ToStr(), row["Username"].ToStr());
//                        userContext.UserName = row["Username"].ToStr();
//                        var expireTime = row["Expiretime"].ToDateNull();
//                        if (expireTime == null)
//                            expireTime = DateTime.Now.AddMinutes(_maxLoginTimeoutMinutes);

//                        CacheHelper.Store(userContext.AppId, userContext, expireTime.Value);

//                        var currentLanguage = HttpContext.Current.Request.Headers["CurrentLanguage"];
//                        if (!currentLanguage.IsEmpty())
//                            userContext.CurrentLanguage = "en".EqualsIgnoreCase(currentLanguage) ? "en" : "cn";

//                        return userContext;
//                    }
//                }
//                catch (Exception ex)
//                {
//                    _logger.Error(ex.Message + "\n" + ex.StackTrace);
//                    return null;
//                }
            }

        }

        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="appId"></param>
        public static void Unregister(string appId)
        {
            UserInfo.RemoveSession();
            CacheFactoryHelper.Remove(appId);
            
            //try
            //{
            //    // 读数据库
            //    using (var dbHelper = new DbHelper())
            //    {
            //        var sql = @"delete from Sys_UserContext where Appid = @Appid";
            //        if (dbHelper.IsOracle())
            //            sql = sql.Replace("@", ":");
            //        var cmd = dbHelper.CreateCommand(sql);
            //        dbHelper.AddInParameter(cmd, "Appid", System.Data.DbType.String, appId);
            //        var result = dbHelper.ExecuteNonQuery(cmd);

            //        sql = @"delete from Sys_UserContext where getdate() > Expiretime";
            //        dbHelper.ExecuteNonQuery(sql);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _logger.Error(ex.Message + "\n" + ex.StackTrace);
            //}
        }
        /// <summary>
        /// 保持状态
        /// </summary>
        /// <param name="appId"></param>
        public static void KeepSession(string appId)
        {
            var userContext = GetUserContext(appId);
            if (DateTime.Now.Subtract(userContext.LastKeepTime).Minutes < 2) // 少于2分钟，不处理
                return;
            userContext.LastKeepTime = DateTime.Now;
            //try
            //{
            //    // 读数据库
            //    using (var dbHelper = new DbHelper())
            //    {
            //        var sql = @"update Sys_UserContext set Lastkeeptime = getdate() where Appid = @Appid";
            //        if (dbHelper.IsOracle())
            //            sql = sql.Replace("@", ":");
            //        var cmd = dbHelper.CreateCommand(sql);
            //        dbHelper.AddInParameter(cmd, "Appid", System.Data.DbType.String, appId);
            //        var result = dbHelper.ExecuteNonQuery(cmd);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _logger.Error(ex.Message + "\n" + ex.StackTrace);
            //}
        }

    }
}
