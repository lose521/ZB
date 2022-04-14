using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using ZB.EntityFramework.DataAccess;
using ZB.EntityFramework.SqlServer;
using ZB.IBusiness.System;

namespace ZB.Business.System
{
    public class Test : EFBaseDAL<test_list>, ITest
    {
        public void Other(int id)
        {
            var test_list = base.context.test_list.Single(c => c.ListId == id);
            test_list.ProductCount = 0;
            base.Modify(test_list);
        }

        public void OtherTran(int id)
        {
            using (TransactionScope tran = new TransactionScope())
            {
                test_list model = new test_list();
                model.ProductCount = 1000;
                Expression<Func<test_list, bool>> exp = (c) => c.ListId == 2 || c.ListId == 4 || c.ListId == 5;
                base.ModifyListBy(model, exp, new string[] { "ProductCount" });

                var test_list = base.context.test_list.Single(c => c.ListId == id);
                test_list.ProductCount = 10000;
                if (test_list.ProductCount == 10000)
                    throw new Exception("测试事务异常,回滚");
                base.Modify(test_list);
                tran.Complete();
            }
        }
    }
}
