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
using ZB.Entity.LW;
using ZB.IBusiness.LW;

namespace ZB.Web.Controllers
{
    public class ContractController : BaseApiController
    {
        private static Logger _logger = LogManager.GetLogger("default");
        public virtual HttpResponseMessage GetPageList([FromUri] RequestPage page, [FromUri] ContractListEntity filter)
        {
            try
            {
                EFContext ef = new EFContext();
                var bs = IocContainer.Resolve<IContractList>();
                int count = 0;

                var sql = (from a in ef.bl_contract
                           join b in ef.bl_customer on a.customerId equals b.customerId
                           where a.status == "A" && (b.customerName.Contains(filter.customerName) || string.IsNullOrEmpty(filter.customerName))
                                                 && (a.contractName.Contains(filter.contractName) || string.IsNullOrEmpty(filter.contractName))
                           select
                           new ContractListEntity
                           {
                               contractId = a.contractId,
                               contractNo = a.contractNo,
                               contractName = a.contractName,
                               contractAmt = a.contractAmt,
                               signDate = a.signDate,
                               customerId = a.customerId,
                               customerName = b.customerName
                           });

                List<ContractListEntity> lst = bs.GetPagedList<DateTime>(sql,page.current, page.pageSize,  (c) => c.signDate, out count);
                ResponsePage rp = new ResponsePage { totalCount = count };
                return WebApi.GetSuccessHttpResponseMessage(new ListData<ContractListEntity> { list = lst, page = rp });
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
                var bs = IocContainer.Resolve<IContract>();
                var t = bs.GetModel(c => c.contractId == key);

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
                EFContext ef = new EFContext();
                var bs1 = IocContainer.Resolve<IContract>();
                var t1 = bs1.GetModel(c => c.contractId == key);
                var bs2 = IocContainer.Resolve<ICustomer>();
                var t2 = bs2.GetModel(c => c.customerId == t1.customerId);
                ContractViewEntity t = new ContractViewEntity(t1, t2);
                return WebApi.GetSuccessHttpResponseMessage(t);
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
                var bs = IocContainer.Resolve<IContract>();
                var t = bs.GetListBy(c => c.status == "A" && (c.contractName.Contains(name) || string.IsNullOrEmpty(name)));

                return WebApi.GetSuccessHttpResponseMessage(t);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual HttpResponseMessage Add(bl_contract rqt)
        {
            try
            {
                EFContext ef = new EFContext();
                var bs = IocContainer.Resolve<IContract>();
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
        public virtual HttpResponseMessage Update(bl_contract rqt)
        {
            try
            {
                EFContext ef = new EFContext();
                var bs = IocContainer.Resolve<IContract>();
                //方式 1
                var newt = ef.bl_contract.Single(c => c.contractId == rqt.contractId);
                newt.contractNo = rqt.contractNo;
                newt.contractName = rqt.contractName;
                newt.contractAmt = rqt.contractAmt;
                newt.signDate = rqt.signDate;
                newt.remark = rqt.remark;
                newt.customerId = rqt.customerId;
                newt.modifyUserId = 1;
                newt.modifyDate = DateTime.Now;
                bs.Modify(newt);
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
                var bs = IocContainer.Resolve<IContract>();
                //方式 1
                Expression<Func<bl_contract, bool>> where = (c) => c.contractId == key;
                bs.DeleteBy(where);
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
                var bs = IocContainer.Resolve<IContract>();
                //方式 1
                Expression<Func<bl_contract, bool>> where = (c) => keys.Contains(c.contractId);
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