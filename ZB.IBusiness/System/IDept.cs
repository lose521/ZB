using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZB.EntityFramework.SqlServer;

namespace ZB.IBusiness.System
{
    public interface IDept
    {
        sys_dept Save(sys_dept user);
        int Modify(sys_dept model, params string[] propertyNames);
    }
}
