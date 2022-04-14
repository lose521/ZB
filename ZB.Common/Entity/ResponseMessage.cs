using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZB.Common.Entity
{
    public class ResponseMessage //<T> where T : class, new()
    {
        public bool ok { get; set; }
        public object data { get; set; }
        public string message { get; set; }
    }
}
