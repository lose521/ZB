using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using ZB.Common.Entity;

namespace ZB.Common.Handler
{
    public static class WebApi
    {
        public static HttpResponseMessage GetSuccessHttpResponseMessage()
        {
            return GetSuccessHttpResponseMessage("", null);
        }
        /// <summary>
        /// OK响应
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static HttpResponseMessage GetSuccessHttpResponseMessage<T>(T t)
        {
            return GetSuccessHttpResponseMessage(t, null);
        }
        public static HttpResponseMessage GetSuccessHttpResponseMessage<T>(T t, JsonConverter jsonConverter)
        {
            return GetHttpResponseMessage(t, HttpStatusCode.OK, jsonConverter);
        }
        /// <summary>
        /// 错误响应
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static HttpResponseMessage GetErrorHttpResponseMessage(string t)
        {
            return GetHttpResponseMessage(t, HttpStatusCode.BadRequest,null);
        }
        public static HttpResponseMessage GetExceptionHttpResponseMessage(Exception ex)
        {

            string message = ex.Message + "-》" + ex.StackTrace;
            
            return GetHttpResponseMessage(message, HttpStatusCode.InternalServerError,null);
        }

        public static HttpResponseMessage GetHttpResponseMessage(ResponseMessage rm)
        {
            return GetHttpResponseMessage(rm, null);
        }
        public static HttpResponseMessage GetHttpResponseMessage(ResponseMessage rm, JsonConverter jsonConverter)
        {
            string jsonString;
            HttpStatusCode httpStatusCode;
            if (rm.ok)
            {
                 httpStatusCode = HttpStatusCode.OK;
            }
            else
            {
                 httpStatusCode = HttpStatusCode.BadRequest;
            }
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(httpStatusCode);
            if (jsonConverter == null)
                jsonString = JsonConvert.SerializeObject(rm);//Convert.ToString(t);
            else
                jsonString = JsonConvert.SerializeObject(rm, Newtonsoft.Json.Formatting.Indented, jsonConverter);
            httpResponseMessage.Content = new StringContent(jsonString, Encoding.GetEncoding("UTF-8"), "application/json");
            return httpResponseMessage;
        }
        /// <summary>
        /// 响应
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="httpStatusCode"></param>
        /// <returns></returns>
        public static HttpResponseMessage GetHttpResponseMessage<T>(T t, HttpStatusCode httpStatusCode, JsonConverter jsonConverter)
        {
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(httpStatusCode);
            bool success = httpResponseMessage.IsSuccessStatusCode;
            //Newtonsoft.Json.Converters.IsoDateTimeConverter timeFormat = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
            //timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            string jsonString;
            if (typeof(T) == typeof(string) || typeof(T).IsEnum)
            {
                if (success)
                {
                    if (jsonConverter == null)
                        jsonString = JsonConvert.SerializeObject(new  { ok = true, data = t.ToString() });//Convert.ToString(t);
                    else
                        jsonString = JsonConvert.SerializeObject(new  { ok = true, data = t.ToString() }, Newtonsoft.Json.Formatting.Indented, jsonConverter);
                }
                else
                    jsonString = JsonConvert.SerializeObject(new  { ok = false, message = t.ToString()});
            }
            else
            {
                if (success)
                {
                    if (jsonConverter == null)
                        jsonString = JsonConvert.SerializeObject(new { ok = true, data = t });
                    else
                        jsonString = JsonConvert.SerializeObject(new { ok = true, data = t }, Newtonsoft.Json.Formatting.Indented, jsonConverter);
                }
                else
                {
                    jsonString = JsonConvert.SerializeObject(new { ok = false, message = t.ToString() });
                }
            }
            
            
            httpResponseMessage.Content = new StringContent(jsonString, Encoding.GetEncoding("UTF-8"), "application/json");
            return httpResponseMessage;
        }

        
    }
}
