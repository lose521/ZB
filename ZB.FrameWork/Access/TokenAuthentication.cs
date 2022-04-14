using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;

using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Net;
using System.Security.Principal;
using System.Net.Http;
using ZB.FrameWork;
using ZB.Common.Handler;

namespace ZB.FrameWork.Access
{
    public class TokenAuthentication : Attribute, IAuthenticationFilter
    {
        private readonly UInt64 requestMaxAgeInSeconds = Convert.ToUInt64(SecurityConfiguration.ApiRequestMaxAge);//
        private readonly bool enableHMACAuthentication = SecurityConfiguration.IsHMACAuthenticationEnabled;

        public TokenAuthentication()
        {

        }

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            // TODO:增加开关控制验证
            // IsHMACAuthenticationEnabled = true/false

            // 压力测试检查
            var req = context.Request;

            if (req.Headers.Authorization != null &&
               Config.AppName.Equals(req.Headers.Authorization.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                var rawAuthzHeader = req.Headers.Authorization.Parameter;

                var autherizationHeaderArray = GetAutherizationHeaderValues(rawAuthzHeader);

                if (autherizationHeaderArray != null)
                {
                    var appId = autherizationHeaderArray[0];
                    var incomingBase64Signature = autherizationHeaderArray[1];
                    var requestTimeStamp = autherizationHeaderArray[2];
                    var userContext = UserContextManager.GetUserContext(appId);
                    if (userContext == null)
                    {
                        context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
                    }
                    else
                    {
                        var apiKey = userContext.ApiKey;

                        var isValid = isValidRequest(req, appId, apiKey, incomingBase64Signature, requestTimeStamp);
                        if (isValid.Result)
                        {
                            var currentPrincipal = new GenericPrincipal(new GenericIdentity(appId), null);
                            //context.Principal = currentPrincipal;
                            //Thread.CurrentPrincipal = currentPrincipal;

                            UserContextManager.CurrentPrincipal = currentPrincipal;
                        }
                        else
                        {
                            context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
                        }
                    }


                }
                else
                {
                    context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
                }
            }
            else
            {
                context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
            }

            return Task.FromResult(0);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            context.Result = new ResultWithChallenge(context.Result);
            return Task.FromResult(0);
        }

        public bool AllowMultiple
        {
            get { return false; }
        }

        private string[] GetAutherizationHeaderValues(string rawAuthzHeader)
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

        private async Task<bool> isValidRequest(HttpRequestMessage req, string appId, string apiKey, string incomingBase64Signature, string requestTimeStamp)
        {
            if (!CheckUrlSqlInjection(req))
                return false;

            // 关闭验证
            if (!SecurityConfiguration.IsHMACAuthenticationEnabled)
            {
                return true;
            }

            string requestContentBase64String = "";
            //string requestUri = HttpUtility.UrlEncode(req.RequestUri.AbsoluteUri.ToLower());
            string requestHttpMethod = req.Method.Method;

            //var sharedKey = CacheHelper  allowedApps[appId];
            //var userContext = CacheHelper.Get<UserContext>(appId);


            if (IsReplayRequest(requestTimeStamp))
            {
                return false;
            }

            byte[] hash = await ComputeHash(req.Content);

            if (hash != null)
            {
                requestContentBase64String = ByteArrayToHexString(hash);// Convert.ToBase64String(hash);
            }

            string data = String.Format("{0}{1}{2}{3}", appId, requestHttpMethod, requestTimeStamp, requestContentBase64String);

            //var b = Base64.encode('apiKey');
            //var hmacObj1 = new jsSHA('SHA-256', 'TEXT');
            //hmacObj1.setHMACKey('apiKey', 'B64');
            //var dd = hmacObj1.update('hmacText');
            //var hmacOutput1 = hmacObj1.getHMAC('B64');

            //var secretKeyBytes1 = Encoding.UTF8.GetBytes("apiKey");

            //byte[] signature1 = Encoding.UTF8.GetBytes("hmacText");

            //using (HMACSHA256 hmac1 = new HMACSHA256(secretKeyBytes1))
            //{
            //    byte[] signatureBytes1 = hmac1.ComputeHash(signature1);

            //    string d = Convert.ToBase64String(signatureBytes1);
            //}

            //var secretKeyBytes = Convert.FromBase64String(apiKey);
            var secretKeyBytes = Encoding.UTF8.GetBytes(apiKey);
            byte[] signature = Encoding.UTF8.GetBytes(data);

            using (HMACSHA256 hmac = new HMACSHA256(secretKeyBytes))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);

                return (incomingBase64Signature.Equals(Convert.ToBase64String(signatureBytes), StringComparison.Ordinal));
            }

        }

        /// <summary>
        /// Check Url for sql injection
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private bool CheckUrlSqlInjection(HttpRequestMessage req)
        {
            var url = HttpUtility.UrlDecode(req.RequestUri.AbsoluteUri).ToLower();

            var dangerWords = new string[] { "select ", "insert ", "update ", "delete ", "truncate ", "exec " };
            foreach (var dangerWord in dangerWords)
            {
                if (url.IndexOf(dangerWord) >= 0)
                {
                    return false;
                }
            }
            return true;
        }


        private string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }


        private bool IsReplayRequest(string requestTimeStamp)
        {
            //if (System.Runtime.Caching.MemoryCache.Default.Contains(nonce))
            //{
            //    return true;
            //}

            DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan currentTs = DateTime.UtcNow - epochStart;

            var serverTotalSeconds = Convert.ToUInt64(currentTs.TotalSeconds);
            var requestTotalSeconds = Convert.ToUInt64(requestTimeStamp);

            if ((serverTotalSeconds - requestTotalSeconds) > requestMaxAgeInSeconds)
            {
                return true;
            }

            //System.Runtime.Caching.MemoryCache.Default.Add(nonce, requestTimeStamp, DateTimeOffset.UtcNow.AddSeconds(requestMaxAgeInSeconds));

            return false;
        }

        private static async Task<byte[]> ComputeHash(HttpContent httpContent)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = null;
                var content = await httpContent.ReadAsByteArrayAsync();
                if (content.Length != 0)
                {
                    hash = md5.ComputeHash(content);
                }
                return hash;
            }
        }
    }

    public class ResultWithChallenge : IHttpActionResult
    {
        private readonly IHttpActionResult next;

        public ResultWithChallenge(IHttpActionResult next)
        {
            this.next = next;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = await next.ExecuteAsync(cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(Config.AppName));//Constant.AUTHENTICATION_SCHEME
            }

            return response;
        }
    }
}
