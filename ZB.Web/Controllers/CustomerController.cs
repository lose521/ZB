using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZB.Common.Handler;
using ZB.EntityFramework.SqlServer;
using ZB.FrameWork.Ioc;
using ZB.FrameWork.WebApi;
using ZB.IBusiness.System;
using System.Threading;
using ZB.Entity.System;
using ZB.Common.Entity;
using System.Linq.Expressions;

namespace ZB.Web.Controllers
{
    public class CustomerController : BaseApiController
    {
        private static Logger _logger = LogManager.GetLogger("default");

        public virtual HttpResponseMessage GetPageList([FromUri] RequestPage page, [FromUri] bl_customer filter)
        {
            try
            {
                //return WebApi.GetSuccessHttpResponseMessage(page);
                EFContext ef = new EFContext();

                var bs = IocContainer.Resolve<ICustomer>();
                int count = 0;
                Expression<Func<bl_customer, bool>> where = (c) => c.status == "A"
               && (c.customerName.Contains(filter.customerName) || string.IsNullOrEmpty(filter.customerName))
               && (c.telephone.Contains(filter.telephone) || string.IsNullOrEmpty(filter.telephone));

                List<bl_customer> lst = bs.GetPagedList<string>(page.current, page.pageSize,  where, (c) => c.customerName, out count);
                ResponsePage rp = new ResponsePage { totalCount = count };

                return WebApi.GetSuccessHttpResponseMessage(new ListData<bl_customer> { list = lst, page = rp });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual HttpResponseMessage Get(int key)
        {
            try
            {
                EFContext ef = new EFContext();
                var bs = IocContainer.Resolve<ICustomer>();
                var t = bs.GetModel(c => c.customerId == key);

                return WebApi.GetSuccessHttpResponseMessage(t);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual HttpResponseMessage GetView(int key)
        {
            try
            {
                return Get(key);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual HttpResponseMessage GetByName(string name)
        {
            try
            {
                EFContext ef = new EFContext();
                var bs = IocContainer.Resolve<ICustomer>();
                var t = bs.GetListBy(c => c.status == "A" && (c.customerName.Contains(name) || string.IsNullOrEmpty(name)));

                return WebApi.GetSuccessHttpResponseMessage(t);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual HttpResponseMessage Add(bl_customer rqt)
        {
            try
            {
                EFContext ef = new EFContext();
                var bs = IocContainer.Resolve<ICustomer>();
                rqt.status = "A";
                rqt.createUserId = 1;
                rqt.modifyUserId = 1;
                rqt.createDate = DateTime.Now;
                rqt.modifyDate = DateTime.Now;
                bs.Add(rqt);
                return WebApi.GetSuccessHttpResponseMessage();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public virtual HttpResponseMessage Update(bl_customer rqt)
        {
            try
            {
                EFContext ef = new EFContext();
                var bs = IocContainer.Resolve<ICustomer>();
                //方式 1
                var newt = ef.bl_customer.Single(c => c.customerId == rqt.customerId);
                newt.customerName = rqt.customerName;
                newt.customerNo = rqt.customerNo;
                newt.telephone = rqt.telephone;
                newt.email = rqt.email;
                newt.sex = rqt.sex;
                newt.address = rqt.address;
                newt.birthday = rqt.birthday;
                newt.remark = rqt.remark;

                newt.modifyUserId = 1;
                newt.modifyDate = DateTime.Now;
                bs.Modify(newt);

                
                //Thread.Sleep(10000);
                return WebApi.GetSuccessHttpResponseMessage();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual HttpResponseMessage Delete(int key)
        {
            try
            {
                EFContext ef = new EFContext();
                var bs = IocContainer.Resolve<ICustomer>();
                //方式 1
                Expression<Func<bl_customer, bool>> where = (c) => c.customerId == key;
                bs.DeleteBy(where);
                //方式 2
                //var newt = ef.bl_customer.Single(c => c.customerId == key);
                //newt.status = "X";
                //newt.modifyUserId = 1;
                //newt.modifyDate = DateTime.Now;
                //customerBs.Modify(newt);
                return WebApi.GetSuccessHttpResponseMessage();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public virtual HttpResponseMessage DeleteBatch(List<int> keys)
        {
            try
            {
                EFContext ef = new EFContext();
                var bs = IocContainer.Resolve<ICustomer>();
                //方式 1
                Expression<Func<bl_customer, bool>> where = (c) => keys.Contains(c.customerId);
                bs.DeleteBy(where);
                return WebApi.GetSuccessHttpResponseMessage();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
 
}