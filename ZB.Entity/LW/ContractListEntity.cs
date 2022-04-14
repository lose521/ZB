using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZB.Common.Tool;
using ZB.EntityFramework.SqlServer;

namespace ZB.Entity.LW
{
    public class ContractListEntity:bl_contract
    {
        public string customerName { get; set; }
        public ContractListEntity()
        {

        }

        
    }

    public class ContractViewEntity : bl_contract
    {
        public string customerName { get; set; }
        public ContractViewEntity()
        {

        }
        public ContractViewEntity(bl_contract contract)
        {
            Tools.mapping(contract, this);
        }
        public ContractViewEntity(bl_contract contract, bl_customer customer)
        {
            Tools.mapping(contract, this);
            this.customerName = customer.customerName;
        }

    }
}
