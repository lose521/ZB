using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZB.EntityFramework.SqlServer;
using ZB.EntityFramework.DataAccess;
using ZB.IBusiness.System;

namespace ZB.Business.System
{
    public class User :EFBaseDAL<sys_user>,IUser
    {
    }
}
