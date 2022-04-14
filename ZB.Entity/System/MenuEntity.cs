using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZB.Entity.System
{
    public class MenuEntity
    {
        public int MnuId{get;set;}
        public string MnuName { get; set; }
        public string MnuUrl { get; set; }

        public List<MenuEntity> Children { get; set; }
    
    }
}
