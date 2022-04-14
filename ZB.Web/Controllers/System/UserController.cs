using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZB.Common.Handler;
using ZB.EntityFramework.SqlServer;
using ZB.FrameWork.Ioc;
using ZB.FrameWork.WebApi;
using ZB.IBusiness.System;
using ZB.Web.Controllers.Framework;
using ZB.FrameWork.Access;
using ZB.Common.Extensions;
using Newtonsoft.Json;

namespace ZB.Web.Controllers.System
{
    public class UserController : BaseApiController
    {
        /*
        public virtual HttpResponseMessage GetList()
        {
            var controller = new AntdTableController();
            controller.PrimaryKey = "userid";
            controller.SelectCommandText = @"select * from view_sys_user where  Status!='X'";
            return controller.GetListData();
        }
        public virtual HttpResponseMessage GetData(int id)
        {
            using (EFContext context = new EFContext())
            {
                //sys_user sysUser = context.sys_user.SingleOrDefault(e => e.UserId == id);
                var user = (from a in context.sys_user
                            join b in context.sys_employee on new { key = a.FkeyId, table = a.Ftable } equals new { key = b.EmployeeId, table = "sys_employee" }
                            where a.UserId == id
                            select new
                            {
                                User = a,
                                Employee = b
                            }).FirstOrDefault();

                if (user == null)
                {
                    throw new Exception("没找到用户");
                }
                return WebApi.GetSuccessHttpResponseMessage(user);
            }
        }
        [HttpPost]
        public virtual HttpResponseMessage Save(Dictionary<string, Dictionary<string, object>> entity)
        {
            try
            {
                var employeeBs = IocContainer.Resolve<IEmployee>();
                string strEmployee = JsonConvert.SerializeObject(entity["employee"]);
                sys_employee employee = JsonConvert.DeserializeObject<sys_employee>(strEmployee);
                employeeBs.Save(employee);

                var userBs = IocContainer.Resolve<IUser>();
                string strUser = JsonConvert.SerializeObject(entity["user"]);
                sys_user user = JsonConvert.DeserializeObject<sys_user>(strUser);
                if(user.UserId == 0)
                {
                    user.Ftable = "sys_employee";
                    user.FkeyId = employee.EmployeeId;
                    user.PassWord = "1";
                    user.Status = "A";
                    user.CreateDate = DateTime.Now;
                    user.CreateUserId = UserInfo.CurrentUserInfo.UserId;
                    user.ModifyDate = DateTime.Now;
                    user.ModifyUserId = UserInfo.CurrentUserInfo.UserId;                    
                }
                userBs.Save(user);
                return WebApi.GetSuccessHttpResponseMessage("ok");
            }
            catch(Exception ex)
            {
                return WebApi.GetExceptionHttpResponseMessage(ex);
            }
        }
        */
    }
}
