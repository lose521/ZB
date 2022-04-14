using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZB.FrameWork
{
    public class SecurityConfiguration 
    {
        private const string IS_HMACAUTHENTICATION_ENABLED = "IsHMACAuthenticationEnabled";
        private const string CREDENTIAL_VALID_PERIOD = "CredentialValidPeriod";
        private const string API_REQUEST_MAX_AGE = "ApiRequestMaxAge";
        private const string STRESS_TEST_LOGINNAME = "StressTestLoginName";


        private static bool _isHMACAuthenticationEnabled = true;
        private static int _credentialValidPeriod = 1440;
        private static long _apiRequestMaxAge = 300;


        private static Dictionary<string, string> _configDictionary = new Dictionary<string, string>();


        static SecurityConfiguration()
        {
            NameValueCollection _config = ConfigurationManager.GetSection("ifcaSecurityConfiguration") as NameValueCollection;
            if (_config != null)
            {
                foreach (var nvc in _config.AllKeys)
                {
                    _configDictionary.Add(nvc, _config[nvc]);
                }
                //_configDictionary = _config.AllKeys.ToDictionary(t => t, t => _config[t]);
            }
            else
            {
                //Logging.Log.Instance.Warn("缺少配置文件，请检查网站目录中的 ~/ConfigFiles/Security.config 文件是否存在。");
            }

            InitializeSettingValue();
        }

        private static void InitializeSettingValue()
        {
            _isHMACAuthenticationEnabled = InitializeIsHMACAuthenticationEnabledValue();
            _credentialValidPeriod = InitializeCredentialValidPeriodValue();
            _apiRequestMaxAge = InitializeApiRequestMaxAgeValue();
        }

        private static bool InitializeIsHMACAuthenticationEnabledValue()
        {

            if (_configDictionary.ContainsKey(IS_HMACAUTHENTICATION_ENABLED))
            {
                bool result = false;
                if (bool.TryParse(_configDictionary[IS_HMACAUTHENTICATION_ENABLED], out result))
                {
                    return result;
                }
            }

            return true;
        }

        private static int InitializeCredentialValidPeriodValue()
        {
            if (_configDictionary.ContainsKey(CREDENTIAL_VALID_PERIOD))
            {
                int result = 0;
                Int32.TryParse(_configDictionary[CREDENTIAL_VALID_PERIOD], out result);

                return result;
            }

            return 1440; // 一天
        }

        private static long InitializeApiRequestMaxAgeValue()
        {
            if (_configDictionary.ContainsKey(API_REQUEST_MAX_AGE))
            {
                long result = 0;
                long.TryParse(_configDictionary[API_REQUEST_MAX_AGE], out result);

                return result;
            }

            return 300; // 5分钟

        }

        private static string GetKeyValue(string key)
        {
            if (!_configDictionary.ContainsKey(key))
                return null;
            var value = _configDictionary[key];
            return value;
        }


        /// <summary>
        /// 是否启用WEB API接口验证
        /// 对应配置文件值： /ConfigFiles/Security.config/IsHMACAuthenticationEnabled =true/false
        /// </summary>
        public static bool IsHMACAuthenticationEnabled
        {
            get
            {
                return _isHMACAuthenticationEnabled;
            }
        }

        /// <summary>
        /// 用户凭据有效期，过期后需重新登录获取APPID和APIKEY，否则调用接口无效
        /// 对应配置文件值： /ConfigFiles/Security.config/CredentialValidPeriod 值： 0时不设置过期时间， 单位：分钟
        /// </summary>
        public static int CredentialValidPeriod
        {
            get
            {
                return _credentialValidPeriod;
            }
        }

        /// <summary>
        /// 接口请求周期间隔
        /// 对应配置文件值： /ConfigFiles/Security.config/ApiRequestMaxAge 值： 0时不设置， 单位：秒
        /// </summary>
        public static long ApiRequestMaxAge
        {
            get
            {
                return _apiRequestMaxAge;
            }
        }

        public static string StressTestLoginName
        {
            get
            {
                return GetKeyValue(STRESS_TEST_LOGINNAME);
            }
        }
    }
}
