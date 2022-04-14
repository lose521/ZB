using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZB.EntityFramework.SqlServer;

namespace ZB.IBusiness.System
{
    public interface IEmployee
    {
        sys_employee Save(sys_employee entity);
    }
}
