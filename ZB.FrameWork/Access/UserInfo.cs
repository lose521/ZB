using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZB.EntityFramework.SqlServer;
using ZB.FrameWork.Cache;

namespace ZB.FrameWork.Access
{
    public class UserInfo
    {
        //private int userId;
        //public UserInfo(int userId)
        //{
        //    this.userId = userId;
        //}
        public static void RegisterSession(string appId,int userId,DateTime keepTime)
        {
            //using (EFContext ef = new EFContext())
            //{
            //    var info = (from a in ef.sys_user
            //                join b in ef.sys_employee on a.FkeyId equals b.EmployeeId
            //                join c in ef.sys_company on b.CompanyId equals c.CompanyId
            //                join d in ef.sys_dept on b.DeptId equals d.DeptId
            //                where a.UserId == userId
            //                select new UserSession
            //                {
            //                    UserId = a.UserId,
            //                    LoginName = a.LoginName,
            //                    UserName = b.EmployeeName,
            //                    CompanyId = b.CompanyId,
            //                    CompanyName = c.CompanyName,
            //                    DeptId = d.DeptId,
            //                    DeptName = d.DeptName,
            //                    Ftable = a.Ftable
            //                }).Single();
            //    CacheFactoryHelper.Store("session_"+ appId, info, keepTime);
            //}

        }
        public static void RemoveSession()
        {
            CacheFactoryHelper.Remove("session_" + UserContextManager.CurrentAppId);
        }
        public static UserSession CurrentUserInfo
        {
            get
            {
                
                object obj = CacheFactoryHelper.Get("session_"+ UserContextManager.CurrentAppId);
                if (obj != null)
                {
                    UserSession session = (UserSession)obj;
                    return session;
                }
                return null;
            }
        }
    }

    [Serializable]
    public class UserSession
    {
        public int UserId { get; set; }
        public string LoginName { get; set; }
        public string Ftable { get; set; }
        public string UserName { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int DeptId { get; set; }
        public string DeptName { get; set; }
    }
}