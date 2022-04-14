using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZB.EntityFramework.SqlServer;

namespace ZB.Entity.LW
{
    public class InvoiceListEntity : bl_invoice
    {
        public string contractName { get; set; }
        public InvoiceListEntity()
        {

        }


    }
}
