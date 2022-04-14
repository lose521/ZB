using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ZB.FrameWork.Access;

namespace ZB.FrameWork.WebApi
{
    public class BaseApiController : ApiController
    {
        public UserContext CurrentUserContext
        {
            get
            {
                return UserContextManager.CurrentUserContext;
            }
        }
    }
}
