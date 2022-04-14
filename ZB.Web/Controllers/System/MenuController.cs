using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZB.Common.Handler;
using ZB.Entity.System;
using ZB.EntityFramework.SqlServer;
using ZB.FrameWork.Access;
using ZB.FrameWork.WebApi;

namespace ZB.Web.Controllers.System
{
   // [TokenAuthentication]
    public class MenuController : BaseApiController
    {
        public virtual HttpResponseMessage GetMenuList()
        {
            using (EFContext ef = new EFContext())
            {
                List<MenuEntity> lstMenuEntity = new List<MenuEntity>();
                List<cm_menu> lstMenu = ef.cm_menu.Where(e => e.Status == "a").ToList(); 
                List<cm_menu> lstParentMenu = lstMenu.Where(e => string.IsNullOrEmpty(e.MnuParentNo)).ToList();
                foreach (cm_menu m in lstParentMenu)
                {
                    List<MenuEntity> lstChildrenMenuEntity = new List<MenuEntity>();
                    List<cm_menu> lstChildrenMenu = lstMenu.Where(e => e.MnuParentNo == m.MnuNo).ToList();
                    foreach(cm_menu cm in lstChildrenMenu)
                    {
                        lstChildrenMenuEntity.Add(new MenuEntity
                        {
                            MnuId = cm.MnuId,
                            MnuName = cm.MnuName,
                            MnuUrl = cm.MnuUrl
                        });
                    }
                    lstMenuEntity.Add(new MenuEntity
                    {
                        MnuId = m.MnuId,
                        MnuName = m.MnuName,
                        MnuUrl = m.MnuUrl,
                        Children = lstChildrenMenuEntity
                    });
                }
                return WebApi.GetSuccessHttpResponseMessage(lstMenuEntity);
            }
        }
    }
}
