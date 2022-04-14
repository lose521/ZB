using System;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using System.Configuration;
using System.Web;

namespace ZB.FrameWork.Cache
{
    public class EnyimMemcachedHelper : ICacheHelper
    {
        internal static string _cachePoolName;// = ConfigurationManager.AppSettings["CachePoolName"];
        internal static string _keyTemplate;// = _cachePoolName + "_{0}";
        internal static MemcachedClient mc;
        internal static TimeSpan _maxExpiresAt = new TimeSpan(0, 43100,0);

        public EnyimMemcachedHelper()
        {
            if (mc == null)
            {
                InitKeyTemplate();
                mc = new MemcachedClient();
            }
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
                _cachePoolName = ConfigurationManager.AppSettings["CachePoolName"];
            }

            _keyTemplate = _cachePoolName + "_E_{0}";
        }

        public void Store(string key, object value)
        {
            mc.Store(StoreMode.Set, string.Format(_keyTemplate, key), value);
        }

        public void Store(string key, object value, TimeSpan expiresAt)
        {
            if (expiresAt > _maxExpiresAt)
                expiresAt = _maxExpiresAt;
            mc.Store(StoreMode.Set, string.Format(_keyTemplate, key), value, expiresAt);
        }

        public void Store(string key, object value, DateTime expiresAt)
        {
            mc.Store(StoreMode.Set, string.Format(_keyTemplate, key), value, expiresAt);
        }

        public void Update(string key, object value)
        {
            mc.Store(StoreMode.Replace, key, value);
        }

        public object Get(string key)
        {
            return mc.Get(string.Format(_keyTemplate, key));
        }

        public object Get<T>(string key)
        {
            return mc.Get<T>(string.Format(_keyTemplate, key));
        }

        public object Remove(string key)
        {
            return mc.Remove(string.Format(_keyTemplate, key));
        }
    }
}