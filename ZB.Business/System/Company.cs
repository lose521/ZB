using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZB.EntityFramework.DataAccess;
using ZB.EntityFramework.SqlServer;
using ZB.IBusiness.System;

namespace ZB.Business.System
{
    public class Company : EFBaseDAL<sys_company>, ICompany
    {
        public void Delete(int companyId)
        {  
        }
    }
}
