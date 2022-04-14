using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZB.Entity.LW;
using ZB.EntityFramework.DataAccess;
using ZB.EntityFramework.SqlServer;
using ZB.IBusiness.LW;

namespace ZB.Business.LW
{
    public class Invoice : EFBaseDAL<bl_invoice>, IInvoice
    {
    }
    public class InvoiceList : EFBaseDAL<InvoiceListEntity>, IInvoiceList
    {

    }
}
