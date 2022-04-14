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
using ZB.IBusiness.LW;
using ZB.Entity.LW;
using System.Web;
using System.IO;
using System.Net.Http.Headers;

namespace ZB.Web.Controllers
{
    public class InvoiceController : BaseApiController
    {
        private static Logger _logger = LogManager.GetLogger("default");

        public virtual HttpResponseMessage GetPageList([FromUri] RequestPage page, [FromUri] InvoiceListEntity filter)
        {
            try
            {
                EFContext ef = new EFContext();

                var bs = IocContainer.Resolve<IInvoiceList>();
                int count = 0;
                var sql = (from a in ef.bl_invoice
                           join b in ef.bl_contract on a.contractId equals b.contractId
                           where a.status == "A" && (a.invoiceName.Contains(filter.invoiceName) || string.IsNullOrEmpty(filter.invoiceName))
                                                 && (b.contractName.Contains(filter.contractName) || string.IsNullOrEmpty(filter.contractName))
                           select
                           new InvoiceListEntity
                           {
                               invoiceId = a.invoiceId,
                               contractId = a.contractId,
                               invoiceNo = a.invoiceNo,
                               invoiceName = a.invoiceName,
                               makeDate = a.makeDate,
                               contractName = b.contractName,
                               invoiceAmt = a.invoiceAmt,
                               invoiceTaxAmt = a.invoiceTaxAmt,
                               imageUrl = a.imageUrl,
                           });

                List<InvoiceListEntity> lst = bs.GetPagedList<string>(sql, page.current, page.pageSize, (c) => c.contractName, out count, true);
                ResponsePage rp = new ResponsePage { totalCount = count };
                return WebApi.GetSuccessHttpResponseMessage(new ListData<InvoiceListEntity> { list = lst, page = rp });
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
                var bs = IocContainer.Resolve<IInvoice>();
                var t = bs.GetModel(c => c.invoiceId == key);
                Files f = new Files(t.imageUrl);
                FileEntity fileObject = f.GetFile();
                return WebApi.GetSuccessHttpResponseMessage(new { t=t,f= new List<FileEntity> { fileObject } });
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



        public virtual HttpResponseMessage Add(bl_invoice rqt)
        {
            try
            {
                EFContext ef = new EFContext();
                var bs = IocContainer.Resolve<IInvoice>();
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
        public virtual HttpResponseMessage Update(bl_invoice rqt)
        {
            try
            {
                EFContext ef = new EFContext();
                var bs = IocContainer.Resolve<IInvoice>();
                //方式 1
                var newt = ef.bl_invoice.Single(c => c.invoiceId == rqt.invoiceId);
                newt.invoiceName = rqt.invoiceName;
                newt.invoiceNo = rqt.invoiceNo;
                newt.makeDate = rqt.makeDate;
                newt.invoiceAmt = rqt.invoiceAmt;
                newt.invoiceTaxAmt = rqt.invoiceTaxAmt;
                newt.imageUrl = rqt.imageUrl;
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
                var bs = IocContainer.Resolve<IInvoice>();
                //方式 1
                Expression<Func<bl_invoice, bool>> where = (c) => c.invoiceId == key;
                bs.DeleteBy(where);
                //方式 2
                //var newt = ef.bl_invoice.Single(c => c.customerId == key);
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
                var bs = IocContainer.Resolve<IInvoice>();
                //方式 1
                Expression<Func<bl_invoice, bool>> where = (c) => keys.Contains(c.invoiceId);
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