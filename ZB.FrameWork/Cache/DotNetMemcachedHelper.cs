using Memcached.ClientLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZB.Common.Handler;

namespace ZB.FrameWork.Cache
{
    public class DotNetMemcachedHelper : ICacheHelper
    {
        internal static string _cachePoolName;
        internal static string _keyTemplate;
        internal static MemcachedClient _mc;
        internal static TimeSpan _maxExpiresAt = new TimeSpan(0, 43100, 0);
        private static SockIOPool _pool;
        private static int _defaultCacheHours = 24; 


        public static SockIOPool Pool
        {
            get { return DotNetMemcachedHelper._pool; }
        }

        public DotNetMemcachedHelper()
        {
            if (_mc == null)
            {
                InitKeyTemplate();

                var temp = Config.DotNetMemcachedServer;
                if (string.IsNullOrEmpty(temp))
                    temp = "127.0.0.1:11211";

                //String[] serverlist = { "127.0.0.1:11211", "127.0.0.1:11211" };
                string[] serverlist = temp.Split(',');

                // initialize the pool for memcache servers
                _pool = SockIOPool.GetInstance(_cachePoolName);
                _pool.SetServers(serverlist);
                _pool.Initialize();

                _mc = new MemcachedClient();
                _mc.PoolName = _cachePoolName;
                _mc.EnableCompression = false;

                //temp = ConfigurationManager.AppSettings["DefaultCacheHours"];
                //if (!string.IsNullOrEmpty(temp))
                //{
                //    _defaultCacheHours = Convert.ToInt32(temp);
                //}

            }
        }

        /// <summary>
        /// 关闭池
        /// </summary>
        public static void Shutdown()
        {
            if (_pool != null)
                _pool.Shutdown();
        }

        private void InitKeyTemplate()
        {
            // HttpContext.Current is not available in the application_start event.
            _cachePoolName = HttpRuntime.AppDomainAppVirtualPath;
            try
            {
                _cachePoolName = Config.CachePoolName + "_" + _cachePoolName.Replace("/", "");
            }
            catch
            {
                _cachePoolName = Config.CachePoolName;
            }

            _keyTemplate = _cachePoolName + "_D_{0}";
        }

        public void Store(string key, object value)
        {
            Store(key, value, TimeSpan.FromHours(_defaultCacheHours));
        }

        public void Store(string key, object value, TimeSpan expiresAt)
        {
            if (expiresAt > _maxExpiresAt)
                expiresAt = _maxExpiresAt;
            var expireDateTime = DateTime.Now.Add(expiresAt);

            try
            {
                _mc.Set(string.Format(_keyTemplate, key), value, expireDateTime);
            }
            catch (Exception ex)
            {
            }
        }

        public void Store(string key, object value, DateTime expiresAt)
        {
            try
            {
                _mc.Set(string.Format(_keyTemplate, key), value, expiresAt);
            }
            catch (Exception ex)
            {
            }
        }

        public void Update(string key, object value)
        {
            try
            {
                _mc.Replace(key, value);
            }
            catch (Exception ex)
            {
                
            }
        }

        public object Get(string key)
        {
            try
            {
                return _mc.Get(string.Format(_keyTemplate, key));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public object Get<T>(string key)
        {
            try
            {
                return _mc.Get(string.Format(_keyTemplate, key));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public object Remove(string key)
        {
            try
            {
                return _mc.Delete(string.Format(_keyTemplate, key));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
