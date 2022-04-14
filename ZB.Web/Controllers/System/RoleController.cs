using System;
using System.Collections.Generic;
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

namespace ZB.Web.Controllers.System
{
    public class RoleController : BaseApiController
    {
        public virtual HttpResponseMessage GetList()
        {
            var controller = new AntdTableController();
            controller.PrimaryKey = "roleid";
            controller.SelectCommandText = @"select * from sys_role where  Status!='X'";
            return controller.GetListData();
        }

        public virtual HttpResponseMessage GetData(int id)
        {
            using (EFContext ef = new EFContext())
            {
                sys_role dept = ef.sys_role.Single(e => e.RoleId == id);
                return WebApi.GetSuccessHttpResponseMessage(dept);
            }
        }

        [HttpPost]
        public virtual HttpResponseMessage Save(sys_role model)
        {
            try
            {
                var bs = IocContainer.Resolve<IRole>();
                model.CreateUserId = UserInfo.CurrentUserInfo.UserId;
                model.ModifyUserId = UserInfo.CurrentUserInfo.UserId;
                bs.Save(model);
                return WebApi.GetSuccessHttpResponseMessage("ok");
            }
            catch (Exception ex)
            {
                return WebApi.GetExceptionHttpResponseMessage(ex);
            }
        }

        [HttpPost]
        public virtual HttpResponseMessage Delete(sys_role model)
        {
            using (EFContext ef = new EFContext())
            {
                sys_role role = ef.sys_role.Single(e => e.RoleId == model.RoleId);
                var bs = IocContainer.Resolve<IRole>();
                bs.Delete(role);
                return WebApi.GetSuccessHttpResponseMessage("ok");
            }
        }
    }
}
