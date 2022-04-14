using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using ZB.Common.Handler;

namespace ZB.Web.Controllers.Framework
{
    public class UploadController : ApiController
    {
        public HttpResponseMessage GetImage(string url)
        {

            //var imgPath = @"D:\学习\ZB\ZB.Web\Upload\invoice\hnjqrftm.3bg.jpeg";

            var imgPath = Path.Combine(Config.UploadBaseUrl,url);
            //从图片中读取byte  
            var imgByte = File.ReadAllBytes(imgPath);
            //从图片中读取流  
            var imgStream = new MemoryStream(File.ReadAllBytes(imgPath));
            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(imgByte)
                //或者  
                //Content = new StreamContent(stream)  
            };
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            return resp;
        }

        public virtual HttpResponseMessage UploadPhoto()
        {
            string saveUrl = "";
            try
            {
                //获取上传文件
                HttpFileCollection files = HttpContext.Current.Request.Files;
                string floder = HttpContext.Current.Request.Form["floder"];
                //判断是否有文件上传
                //if (files.Count == 0)
                //{
                //    DataTable dt = new DataTable();
                //    resultMsg = new ResultMsg();
                //    resultMsg.StatusCode = (int)StatusCodeEnum.Success;
                //    resultMsg.Info = "请选择要上传的文件！";
                //    resultMsg.Data = "";
                //    return HttpResponseExtension.toJson(JsonConvert.SerializeObject(resultMsg));
                //}
                //得到上传文件格式
                string FileEextension = Path.GetExtension(files[0].FileName);
                string[] LimitPictureType = { ".jpg", ".png", ".jpeg" };
                if (LimitPictureType.Contains(FileEextension))
                {
                    //设置文件上传路径
                    string appPath = Config.UploadBaseUrl;
                    string fileName = Path.GetRandomFileName() + FileEextension;
                    string fullFloder = Path.Combine(appPath, floder);
                    string fullFileName = Path.Combine(fullFloder, fileName);
                    //配置数据库保存格式
                    saveUrl = Path.Combine(floder, fileName);
                    ////创建文件夹，保存文件
                    string path = Path.GetDirectoryName(fullFileName);
                    #region 检查上传的物理路径是否存在，不存在则创建
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    #endregion
                    //保存文件  文件存在则先删除原来的文件
                    if (File.Exists(fullFileName))
                    {
                        File.Delete(fullFileName);
                    }
                    files[0].SaveAs(fullFileName);

                    //修改数据库
                    //var _data = DAL.UploadPhoto(newFileName, SavePhotoUrl);
                    //if (_data != null && _data != (object)"-1")
                    //{
                    //    resultMsg = new ResultMsg();
                    //    resultMsg.StatusCode = (int)StatusCodeEnum.Success;
                    //    resultMsg.Info = StatusCodeEnum.Success.GetEnumText();
                    //    resultMsg.Data = 1;
                    //}
                    //else
                    //{
                    //    DataTable dt = new DataTable();
                    //    resultMsg = new ResultMsg();
                    //    resultMsg.StatusCode = (int)StatusCodeEnum.Success;
                    //    resultMsg.Info = "文件存储异常，请稍后重试";
                    //    resultMsg.Data = -1;
                    //}
                }
                else
                {
                    //DataTable dt = new DataTable();
                    //resultMsg = new ResultMsg();
                    //resultMsg.StatusCode = (int)StatusCodeEnum.Success;
                    //resultMsg.Info = "图片上传操作失败，请选择扩展名为：.jpg, .png, 等类型图片！";
                    //resultMsg.Data = -1;
                }
                return WebApi.GetSuccessHttpResponseMessage(saveUrl);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}