using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZB.EntityFramework.SqlServer;

namespace ZB.IBusiness.System
{
    public interface IUser
    {
        sys_user Save(sys_user user);
        //sys_user GetModel(Expression<Func<sys_user, bool>> whereLambda);
    }
}
