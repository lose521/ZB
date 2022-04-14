using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZB.EntityFramework.SqlServer;

namespace ZB.IBusiness.System
{
    public interface ITest
    {
        test_list GetModel(Expression<Func<test_list, bool>> whereLambda);
        test_list Save(test_list model);
        int ModifyListBy(test_list model, Expression<Func<test_list, bool>> whereLambda, params string[] modifiedPropertyNames);
        int Modify(test_list model, params string[] propertyNames);
        bool Delete(test_list model);
        void Other(int id);
        void OtherTran(int id);
    }
}
