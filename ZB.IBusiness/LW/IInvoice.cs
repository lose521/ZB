using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZB.Entity.LW;
using ZB.EntityFramework.SqlServer;

namespace ZB.IBusiness.LW
{
    public interface IInvoice
    {
        List<bl_invoice> GetPagedList<TKey>(int pageIndex, int pageSize, Expression<Func<bl_invoice, bool>> whereLambda, Expression<Func<bl_invoice, TKey>> orderByLambda, out int rowCount, bool isAsc = true);
        bl_invoice Save(bl_invoice mode);
        bl_invoice Add(bl_invoice mode);
        bl_invoice Modify(bl_invoice mode);
        int Modify(bl_invoice mode, params string[] propertyNames);
        int DeleteBy(Expression<Func<bl_invoice, bool>> delWhere);
        bool Delete(bl_invoice model);
        bl_invoice GetModel(Expression<Func<bl_invoice, bool>> whereLambda);
        List<bl_invoice> GetListBy(Expression<Func<bl_invoice, bool>> whereLambda);
    }

    public interface IInvoiceList
    {
        List<InvoiceListEntity> GetPagedList<TKey>(IEnumerable<InvoiceListEntity> source, int pageIndex, int pageSize, Expression<Func<InvoiceListEntity, TKey>> orderByLambda, out int rowCount, bool isAsc = true);
    }
}
