using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading;
using System.Linq.Expressions;
using System.Transactions;
using ZB.Common.Entity;
using ZB.Common.Handler;
using ZB.EntityFramework.SqlServer;
using ZB.FrameWork.Access;
using ZB.FrameWork.Ioc;
using ZB.FrameWork.WebApi;
using ZB.IBusiness.System;
using ZB.EntityFramework.DataAccess;
using System.Data;

namespace ZB.Web.Controllers.System
{
    //[TokenAuthentication]
    public class CompanyController : BaseApiController
    {
        public virtual HttpResponseMessage GetData(int id)
        {
            try
            {
                using (EFContext ef = new EFContext())
                {
                    sys_company company = ef.sys_company.Single(c=>c.CompanyId == id);
                    return WebApi.GetSuccessHttpResponseMessage(company);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual HttpResponseMessage GetTree()
        {
            try
            {
                using (EFContext ef = new EFContext())
                {
                    DataTable dt = ef.ExecuteDataTable("select * from sys_company where Status = 'A'");
                    List<KeyTitle> lstKeyTitle = KeyTitle.ToKeyTitle(dt, "CompanyId", "ParentId", "CompanyName");
                    return WebApi.GetSuccessHttpResponseMessage(lstKeyTitle);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public virtual HttpResponseMessage GetTree()
        //{
        //    List<KeyTitle> lstKeyTitle = new List<KeyTitle>();
        //    try
        //    {
        //        using (EFContext ef = new EFContext())
        //        {
        //            List<sys_company> lstALlCompany = ef.sys_company.Where(c => c.Status == "A").ToList();//一次性取出所有公司
        //            List<sys_company> lstCompany = lstALlCompany.Where(c => (c.ParentId == null || c.ParentId == 0) && c.Status == "A").ToList();
        //            foreach (sys_company c in lstCompany)
        //            {
        //                KeyTitle kt = new KeyTitle { key = c.CompanyId.ToString(), title = c.CompanyName };                       
        //                GetChildren(lstALlCompany,kt, c,ef);
        //                lstKeyTitle.Add(kt);
        //            }
        //            return WebApi.GetSuccessHttpResponseMessage(lstKeyTitle);
        //        }

        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        void GetChildren(List<sys_company> lstALlCompany, KeyTitle kt,sys_company company, EFContext ef)
        {
            kt.children = new List<KeyTitle>();
            List<sys_company> lstCompany = lstALlCompany.Where(c => c.ParentId == company.CompanyId && c.Status == "A").ToList();
            foreach (sys_company c in lstCompany)
            {
                KeyTitle ktChildren = new KeyTitle { key = c.CompanyId.ToString(), title = c.CompanyName };
                GetChildren(lstALlCompany,ktChildren, c, ef);
                kt.children.Add(ktChildren);
            }
        }
        [HttpPost]
        public virtual HttpResponseMessage Save(sys_company company)
        {
            try
            {
                //Thread.Sleep(3000);
                var bs = IocContainer.Resolve<ICompany>();
                company.CreateDate = DateTime.Now;
                company.CreateUserId = UserInfo.CurrentUserInfo.UserId;
                company.ModifyDate = DateTime.Now;
                company.ModifyUserId = UserInfo.CurrentUserInfo.UserId;
                company.Status = "A";
                bs.Save(company);
                return WebApi.GetSuccessHttpResponseMessage(company.CompanyId);
            }
            catch (Exception ex)
            {
                return WebApi.GetExceptionHttpResponseMessage(ex);
            }
        }
        [HttpPost]
        public virtual HttpResponseMessage Delete(Dictionary<string, int> company)
        {

            try
            {
                using (TransactionScope tran = new TransactionScope())
                {
                    int companyId = company["companyId"];
                    bool isforce = false;
                    if (company.ContainsKey("force"))
                    {
                        isforce = company["force"] == 1;
                    }
                    ResponseMessage rm = new ResponseMessage { Ok = true };
                    using (EFContext ef = new EFContext())
                    {
                        var bs = IocContainer.Resolve<ICompany>(ef);
                        //判断是否主公司
                        sys_company model = ef.sys_company.Single(c => c.CompanyId == companyId);
                        if (model.CompanyGoup == "main")
                        {
                            rm.Ok = false;
                            rm.Message = "主公司不能删除";
                            return WebApi.GetHttpResponseMessage(rm);
                        }
                        //判断是否有下级公司
                        int modelChildren = ef.sys_company.Where(c => c.ParentId == companyId).Count();
                        if (modelChildren > 0 && !isforce)
                        {
                            rm.Ok = false;
                            rm.Data = true;
                            rm.Message = model.CompanyName + "有子级公司";
                            return WebApi.GetHttpResponseMessage(rm);
                        }
                        else if (modelChildren > 0 && isforce)
                        {
                            Expression<Func<sys_company, bool>> exp = (c) => c.ParentId == companyId;
                            //删除子公司
                            model.Status = "X";
                            bs.ModifyListBy(model, exp, new string[] { "Status" });
                        }
                        model.Status = "X";
                        bs.Modify(model, new string[] { "Status" });
                    }
                    tran.Complete();
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
