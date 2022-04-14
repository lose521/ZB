using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ZB.Common.Handler
{
    public static class Config
    {
        // AUTHENTICATION_SCHEME
        public static string AppName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["AppName"];
            }
        }

        public static string CacheType
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["CacheType"];
            }
        }

        public static string CachePoolName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["CachePoolName"];
            }
        }

        public static string DotNetMemcachedServer
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DotNetMemcachedServer"];
            }
        }

        public static string UploadBaseUrl
        {
            get
            {
                return Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "Upload");
                //return System.Configuration.ConfigurationManager.AppSettings["DotNetMemcachedServer"];
            }
        }

        public static string AppAddress
        {
            get
            {
                string siteAddress = System.Web.HttpContext.Current.Request.ApplicationPath.ToString();
                if (System.Web.HttpContext.Current.Request.ApplicationPath.ToString() == "/")
                {
                    siteAddress = "";
                }
                string port = System.Web.HttpContext.Current.Request.Url.Port.ToString();
                if (port == "80")
                    port = "";
                else
                {
                    port = ":"+ port;
                }
                return System.Web.HttpContext.Current.Request.Url.Scheme + "://" + System.Web.HttpContext.Current.Request.Url.Host+ port + siteAddress;

            }
        }
    }
}
