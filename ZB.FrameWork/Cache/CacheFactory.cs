using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZB.Common.Handler;

namespace ZB.FrameWork.Cache
{
    public static class CacheFactoryHelper
    {
        private static ICacheHelper _current = null;
        public static ICacheHelper Current
        {
            get
            {
                if (_current != null)
                    return _current;
                var memcachedClientType = Config.CacheType;
                switch (memcachedClientType)
                {
                    case "Enyim":
                        _current = new EnyimMemcachedHelper();
                        break;
                    case "AspNet":
                        _current = new AspNetCacheHelper();
                        break;
                    default:
                        _current = new DotNetMemcachedHelper();
                        break;
                }
                return _current;
            }

        }

        private static string FormatKey(string key)
        {
            if (key == null)
                return Guid.NewGuid().ToString();
            var tempKey = key.Replace(" ", "_");

            if (!IsEnglishStatement(tempKey))  
            {
                tempKey = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(tempKey));
            }

            var splitLength = 150;
            if (tempKey.Length > splitLength)
            {
                var part1 = tempKey.Substring(0, splitLength);
                var part2 = tempKey.Substring(splitLength);
                tempKey = string.Format("{0}_{1}_{2}", Md5Hash(part1), Md5Hash(part2), Md5Hash(tempKey));
            }
            return tempKey;
        }

        private static string Md5Hash(string input)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                return BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(input))).Replace("-", "");
            }
        }

        /// <summary>
        /// 是否英文字符串
        /// </summary>
        /// <param name="str"></param>
        private static bool IsEnglishStatement(string str)
        {
            var regEx = new Regex("^[a-zA-Z0-9_ ./\\-]+s*$");
            return regEx.IsMatch(str);
        }


        public static void Store(string key, object value)
        {
            var newKey = FormatKey(key);
            Current.Remove(newKey);
            Current.Store(newKey, value);
        }

        public static void Store(string key, object value, TimeSpan expiresAt)
        {
            var newKey = FormatKey(key);
            Current.Remove(newKey);
            Current.Store(newKey, value, expiresAt);
        }

        public static void Store(string key, object value, DateTime expiresAt)
        {
            var newKey = FormatKey(key);
            Current.Remove(newKey);
            Current.Store(newKey, value, expiresAt);
        }

        public static void Update(string key, object value)
        {
            var newKey = FormatKey(key);
            Current.Update(newKey, value);
        }

        public static object Get(string key)
        {
            var newKey = FormatKey(key);
            return Current.Get(newKey);
        }

        public static T Get<T>(string key)
        {
            var newKey = FormatKey(key);
            var obj = Current.Get<T>(newKey);
            return (T)obj;
        }

        public static object Remove(string key)
        {
            var newKey = FormatKey(key);
            return Current.Remove(newKey);
        }
    }
}
