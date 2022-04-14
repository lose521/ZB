using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZB.FrameWork.Access
{
    [Serializable]
    public sealed class UserContext
    {

        public string LoginName { get;  set; }

        public int UserId { get;  set; }

        public string AppId { get;  set; }

        public string ApiKey { get;  set; }

        public string CurrentLanguage { get; set; }


        /// <summary>
        /// 最后保持状态时间
        /// </summary>
        public DateTime LastKeepTime { get; set; }


        public string UserName { get; set; }
        public UserContext()
        { }
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="userId"></param>
        /// <param name="appId"></param>
        /// <param name="apiKey"></param>
        public UserContext(string loginName, int userId, string appId, string apiKey)
        {
            this.LoginName = loginName;
            this.UserId = userId;
            this.AppId = appId;
            this.ApiKey = apiKey;

            this.LastKeepTime = DateTime.Now;
        }
        public UserContext(string loginName, int userId, string appId, string apiKey, string currentLanguage)
        {
            this.LoginName = loginName;
            this.UserId = userId;
            this.AppId = appId;
            this.ApiKey = apiKey;
            this.CurrentLanguage = currentLanguage;

            this.LastKeepTime = DateTime.Now;
        }
    }
}
