using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZB.Common.Handler;
using ZB.EntityFramework.SqlServer;
using ZB.FrameWork.Access;
using ZB.FrameWork.Ioc;
using ZB.FrameWork.WebApi;
using ZB.IBusiness.System;
using ZB.Web.Controllers.Framework;
using ZB.EntityFramework.DataAccess;
using ZB.Common.Entity;

namespace ZB.Web.Controllers.System
{
   // [TokenAuthentication]
    public class DeptController : BaseApiController
    {
        public virtual HttpResponseMessage GetList()
        {
            var controller = new AntdTableController();
            controller.PrimaryKey = "deptid";
            controller.SelectCommandText = @"select * from  sys_dept where  Status!='X' order by modifydate desc";
            return controller.GetListData();
        }

        public virtual HttpResponseMessage GetData(int id)
        {
            using (EFContext ef = new EFContext())
            {
                sys_dept dept = ef.sys_dept.Single(e => e.DeptId == id);
                return WebApi.GetSuccessHttpResponseMessage(dept);
            }
        }

        public virtual HttpResponseMessage GetTree()
        {
            try
            {
                using (EFContext ef = new EFContext())
                {
                    DataTable dt = ef.ExecuteDataTable(@"select * from view_sys_companyDeptTree");
                   // List<KeyTitle> lstKeyTitle = KeyTitle.ToKeyTitle(dt, "KeyId", "ParentKeyId", "Name");
                    List<Dictionary<string, object>> lstKeyTitle = KeyTitle.ToKeyTitleDictionary(dt, "KeyId", "ParentKeyId", "Name");
                    return WebApi.GetSuccessHttpResponseMessage(lstKeyTitle);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public virtual HttpResponseMessage GetTree1()
        //{
        //    try
        //    {
        //        using (EFContext ef = new EFContext())
        //        {
        //            DataTable dt = ef.ExecuteDataTable(@"select * from view_sys_companyDeptTree");
        //            List<Dictionary<string, object>> lstKeyTitle = KeyTitle.ToKeyTitle(dt, "KeyId", "ParentKeyId", "Name");
        //            return WebApi.GetSuccessHttpResponseMessage(lstKeyTitle);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        [HttpPost]
        public virtual HttpResponseMessage Save(sys_dept dept)
        {
            try
            {
                //string s = "dd".Split('f')[2];
                var bs = IocContainer.Resolve<IDept>();
                dept.CreateDate = DateTime.Now;
                dept.CreateUserId = UserInfo.CurrentUserInfo.UserId;
                dept.ModifyDate = DateTime.Now;
                dept.ModifyUserId = UserInfo.CurrentUserInfo.UserId;
                dept.Status = "A";
                bs.Save(dept);
                return WebApi.GetSuccessHttpResponseMessage("ok");
            }
            catch (Exception ex)
            {
                return WebApi.GetExceptionHttpResponseMessage(ex);
            }
        }

        [HttpPost]
        public virtual HttpResponseMessage Delete(Dictionary<string, int> dept)
        {
            try
            {
                using (EFContext ef = new EFContext())
                {
                    var bs = IocContainer.Resolve<IDept>(ef);
                    int deptId = dept["deptId"];
                    sys_dept model = ef.sys_dept.Single(c => c.DeptId == deptId);
                    model.Status = "X";
                    bs.Save(model);
                    return WebApi.GetSuccessHttpResponseMessage("ok");
                }
            }
            catch (Exception ex)
            {
                return WebApi.GetExceptionHttpResponseMessage(ex);
            }
        }
    }
}
