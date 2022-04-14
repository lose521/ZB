using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;
using ZB.Common.Entity;
using ZB.Common.Handler;
using ZB.EntityFramework.SqlServer;
using ZB.FrameWork.Ioc;
using ZB.IBusiness.System;

namespace ZB.Web.Controllers
{
    public class TestEFController : ApiController
    {
        /*
        [HttpGet]
        public virtual HttpResponseMessage TestResponseMessage()
        {
            ResponseMessage rm = new ResponseMessage { Ok=true };//,Data= new { a="1",b="2"}
            try
            {
                using (EFContext ef = new EFContext())
                {
                    var bs = IocContainer.Resolve<ITest>(ef);
                    Expression<Func<test_list, bool>> exp = (c) => c.ListId == 2;
                    test_list test_list = bs.GetModel(exp);
                    if (test_list.Name == "苹果")
                    {
                        rm.Ok = false;
                        rm.Message = "测试验证提示信息：不能是苹果";
                    }
                    else
                    {
                        rm.Data = test_list; 
                    }
                }
                return WebApi.GetHttpResponseMessage(rm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        [HttpGet]
        public virtual HttpResponseMessage TestTransaction()
        {
            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    var bs = IocContainer.Resolve<ITest>();
                    test_list test = new test_list();
                    test.Name = "西瓜";
                    test.Code = "xigua";
                    test.Date = DateTime.Now;
                    test.Seqno = 1;
                    test.Season = "summer";
                    test.ProductCount = 10;
                    test.Status = "A";
                    bs.Save(test);

                    test_list test1 = new test_list();
                    test1.Name = "南瓜";
                    test1.Code = "nangua";
                    test1.Date = DateTime.Now;
                    test1.Seqno = 2;
                    test1.Season = "summer";
                    test1.ProductCount = 1;
                    test1.Status = "A";
                    bs.Save(test1);

                    // if (test1.Name == "南瓜")
                    //     throw new Exception("roll"); 

                    tran.Complete();
                    return WebApi.GetSuccessHttpResponseMessage("ok");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        [HttpGet]
        public virtual HttpResponseMessage TestModify()
        {
            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    
                    using (EFContext ef = new EFContext())
                    {
                        var bs = IocContainer.Resolve<ITest>(ef);
                        test_list model = ef.test_list.Single(c=>c.ListId == 2);
                        model.ProductCount = 1000;
                        bs.Modify(model, new string[] { "ProductCount" });
                    }
                    tran.Complete();
                    return WebApi.GetSuccessHttpResponseMessage("ok");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [HttpGet]
        public virtual HttpResponseMessage TestModifyList()
        {
            using (TransactionScope tran = new TransactionScope())
            {
                try
                {

                    var bs = IocContainer.Resolve<ITest>();
                    test_list model = new test_list();
                    model.ProductCount = 10;
                    Expression<Func<test_list, bool>> exp = (c) => c.ListId == 2 || c.ListId == 4 || c.ListId == 5;
                    bs.ModifyListBy(model, exp, new string[] { "ProductCount" });

                    tran.Complete();
                    return WebApi.GetSuccessHttpResponseMessage("ok");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [HttpGet]
        public virtual HttpResponseMessage TestDelete()
        {
            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    
                    using (EFContext ef = new EFContext())
                    {
                        var bs = IocContainer.Resolve<ITest>();
                        test_list model = ef.test_list.Single(c => c.ListId == 17);
                        bs.Delete(model);
                    }
                    tran.Complete();
                    return WebApi.GetSuccessHttpResponseMessage("ok");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [HttpGet]
        public virtual HttpResponseMessage TestOther()
        {
            using (TransactionScope tran = new TransactionScope())
            {
                try
                {
                    using (EFContext ef = new EFContext())
                    {
                        var bs = IocContainer.Resolve<ITest>(ef);                       
                        bs.Other(2);
                    }
                    tran.Complete();
                    return WebApi.GetSuccessHttpResponseMessage("ok");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        [HttpGet]
        public virtual HttpResponseMessage TestOtherTran()
        {

            try
            {
                using (EFContext ef = new EFContext())
                {
                    var bs = IocContainer.Resolve<ITest>(ef);
                    bs.OtherTran(2);
                }
                return WebApi.GetSuccessHttpResponseMessage("ok");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        */
    }
}
