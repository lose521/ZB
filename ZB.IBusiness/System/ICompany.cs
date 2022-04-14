using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZB.EntityFramework.SqlServer;

namespace ZB.IBusiness.System
{
    public interface ICompany
    {
        sys_company Save(sys_company company);
        int Modify(sys_company model, params string[] propertyNames);
        int ModifyListBy(sys_company model, Expression<Func<sys_company, bool>> whereLambda, params string[] modifiedPropertyNames);
        void Delete(int companyId);
    }
}
