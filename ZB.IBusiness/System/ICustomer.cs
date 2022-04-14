using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZB.EntityFramework.SqlServer;

namespace ZB.IBusiness.System
{
   public interface ICustomer
    {
        bl_customer Save(bl_customer mode);
        bl_customer Add(bl_customer mode);
        bl_customer Modify(bl_customer mode);
        int Modify(bl_customer mode, params string[] propertyNames);
        int DeleteBy(Expression<Func<bl_customer, bool>> delWhere);
        List<bl_customer> GetPagedList<TKey>(int pageIndex, int pageSize, Expression<Func<bl_customer, bool>> whereLambda, Expression<Func<bl_customer, TKey>> orderByLambda, out int rowCount, bool isAsc = true);
        bool Delete(bl_customer model);
        bl_customer GetModel(Expression<Func<bl_customer, bool>> whereLambda);
        List<bl_customer> GetListBy(Expression<Func<bl_customer, bool>> whereLambda);
    }
}
