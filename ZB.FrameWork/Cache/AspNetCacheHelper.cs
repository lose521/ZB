using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using ZB.Common.Handler;

namespace ZB.FrameWork.Cache
{
    public class AspNetCacheHelper : ICacheHelper
    {
        //private static ILog _logger = LogManager.GetLogger("AspNetTrace");
        private System.Web.Caching.Cache _cache;
        internal static string _cachePoolName;
        internal static string _keyTemplate;

        public AspNetCacheHelper()
        {
            InitKeyTemplate();
            _cache = HttpRuntime.Cache;

        }

        private void InitKeyTemplate()
        {
            // HttpContext.Current is not available in the application_start event.
            _cachePoolName = HttpRuntime.AppDomainAppVirtualPath;
            try
            {
                _cachePoolName = _cachePoolName.Replace("/", "");
            }
            catch
            {
                _cachePoolName = Config.CachePoolName;
            }

            _keyTemplate = _cachePoolName + "_{0}";
        }

        public void Store(string key, object value)
        {
            try
            {
                if (value == null)
                {
                    this.Remove(key);
                    return;
                }
                _cache.Insert(string.Format(_keyTemplate, key), value);
            }
            catch (Exception ex)
            {
            }
        }

        public void Store(string key, object value, TimeSpan expiresAt)
        {
            Store(key, value, DateTime.Now.Add(expiresAt));
        }

        public void Store(string key, object value, DateTime expiresAt)
        {
            try
            {
                if (value == null)
                {
                    this.Remove(key);
                    return;
                }
                _cache.Insert(string.Format(_keyTemplate, key), value, null, expiresAt, System.Web.Caching.Cache.NoSlidingExpiration);
            }
            catch (Exception ex)
            {
            }
        }

        public void Update(string key, object value)
        {
            Store(key, value);
        }

        public object Get(string key)
        {
            try
            {
                return _cache.Get(string.Format(_keyTemplate, key));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public object Get<T>(string key)
        {
            return Get(key);
        }

        public object Remove(string key)
        {
            try
            {
                object obj = _cache.Remove(string.Format(_keyTemplate, key));
                return obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
