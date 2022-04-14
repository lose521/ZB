using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZB.EntityFramework.SqlServer;

namespace ZB.IBusiness.System
{
    public interface IRole
    {
        sys_role Save(sys_role entity);
        
        bool Delete(sys_role entity);
    }
}
