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
    public interface IContract
    {
        bl_contract Save(bl_contract mode);
        bl_contract Add(bl_contract mode);
        bl_contract Modify(bl_contract mode);
        int Modify(bl_contract mode, params string[] propertyNames);
        int DeleteBy(Expression<Func<bl_contract, bool>> delWhere);      
        bool Delete(bl_contract model);
        bl_contract GetModel(Expression<Func<bl_contract, bool>> whereLambda);
        List<bl_contract> GetListBy(Expression<Func<bl_contract, bool>> whereLambda);
    }

    public interface IContractList
    {
        
        List<ContractListEntity> GetPagedList<TKey>(IEnumerable<ContractListEntity> source, int pageIndex, int pageSize, Expression<Func<ContractListEntity, TKey>> orderByLambda, out int rowCount, bool isAsc = true);
    }
}
