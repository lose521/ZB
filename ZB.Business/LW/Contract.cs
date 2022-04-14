using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZB.Entity.LW;
using ZB.EntityFramework.DataAccess;
using ZB.EntityFramework.SqlServer;
using ZB.IBusiness.LW;

namespace ZB.Business.LW
{
    public class Contract : EFBaseDAL<bl_contract>,  IContract
    {
     
    }
    public class ContractList : EFBaseDAL<ContractListEntity>, IContractList
    {

    }
}
