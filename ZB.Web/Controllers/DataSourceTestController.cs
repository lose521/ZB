using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZB.Common.Handler;
using ZB.EntityFramework.SqlServer;
using ZB.FrameWork.WebApi;
using ZB.Web.Controllers.Framework;
using ZB.EntityFramework.DataAccess;
using ZB.Common.Entity;
namespace ZB.Web.Controllers
{
     
    public class DataSourceTestController : BaseApiController
    {
        /*
        public class dataSource
        {
            public string value
            {
                get;
                set;
            }
            public string text
            {
                get;
                set;
            }
        }
        public class list
        {
            public int listid { get; set; }
            public int seqno { get; set; }
            public string code { get; set; }
            public string name { get; set; }
            public DateTime date { get; set; }
        }
        public virtual HttpResponseMessage GetTest()
        {
            try
            {
                List<dataSource> lst = new List<dataSource>();
                lst.Add(new dataSource { value = "1", text = "苹果" });
                lst.Add(new dataSource { value = "2", text = "香蕉" });
                lst.Add(new dataSource { value = "3", text = "葡萄" });
                return WebApi.GetSuccessHttpResponseMessage(lst);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public virtual HttpResponseMessage GetListTestObject()
        {
            try
            {
                List<list> lst = new List<list>();
                lst.Add(new list { listid = 1, seqno = 1, code = "apple", name = "苹果", date = DateTime.Now });
                lst.Add(new list { listid = 2, seqno = 2, code = "banana", name = "香蕉", date = DateTime.Parse("2018-12-22") });
                lst.Add(new list { listid = 3, seqno = 3, code = "grape", name = "葡萄", date = DateTime.Parse("2018-11-20") });
                lst.Add(new list { listid = 4, seqno = 4, code = "bbbb", name = "葡萄b", date = DateTime.Parse("2018-11-21") });
                lst.Add(new list { listid = 5, seqno = 1, code = "apple", name = "苹果", date = DateTime.Now });
                lst.Add(new list { listid = 6, seqno = 1, code = "banana", name = "香蕉", date = DateTime.Parse("2018-12-22") });
                lst.Add(new list { listid = 7, seqno = 1, code = "grape", name = "葡萄", date = DateTime.Parse("2018-11-20") });
                lst.Add(new list { listid = 8, seqno = 1, code = "bbbb", name = "葡萄b", date = DateTime.Parse("2018-11-21") });
                lst.Add(new list { listid = 9, seqno = 1, code = "apple", name = "苹果", date = DateTime.Now });
                lst.Add(new list { listid = 10, seqno = 1, code = "banana", name = "香蕉", date = DateTime.Parse("2018-12-22") });
                lst.Add(new list { listid = 11, seqno = 1, code = "grape", name = "葡萄", date = DateTime.Parse("2018-11-20") });
                lst.Add(new list { listid = 12, seqno = 1, code = "bbbb", name = "葡萄b", date = DateTime.Parse("2018-11-21") });
                IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
                timeFormat.DateTimeFormat = "yyyy-MM-dd";
                return WebApi.GetSuccessHttpResponseMessage(lst, timeFormat);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual HttpResponseMessage GetListTestSql()//int current, int pageSize,string condition
        {            
            var controller = new AntdTableController();
            //var customParameterDict = controller.GetCustomParameterDictionary(id);
            controller.PrimaryKey = "listid";
            controller.SelectCommandText = @"select a.ListId,a.Code,a.Name,a.Date,a.Seqno,a.Status,a.ProductCount,a.season as SeasonCode,b.name as SeasonName from test_list a
left join dbo.cm_typeDetail b on a.season=b.Code and Type='season'";

            return controller.GetListData();
            //using (EFContext entities = new EFContext())
            //{
            //    string sql = "select * from dbo.test_list";
            //    DataTable dt = entities.ExecuteDataTable(sql);
            //    IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            //    timeFormat.DateTimeFormat = "yyyy-MM-dd";
            //    return WebApi.GetSuccessHttpResponseMessage(dt, timeFormat);
            //}
        }
        public virtual HttpResponseMessage GetListEditTestSql()//int current, int pageSize,string condition
        {
            using (EFContext entities = new EFContext())
            {
                string sql = @"select a.ListId,a.Code,a.Name,a.Date,a.Seqno,a.Status,a.ProductCount,a.season as SeasonCode,b.name as SeasonName from test_list a
left join dbo.cm_typeDetail b on a.season=b.Code and Type='season'";
                DataTable dt = entities.ExecuteDataTable(sql);
                IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
                timeFormat.DateTimeFormat = "yyyy-MM-dd";
                return WebApi.GetSuccessHttpResponseMessage(dt, timeFormat);
            }
        }

        public virtual HttpResponseMessage GetType() 
        {
            using (EFContext entities = new EFContext())
            {
                var lst = entities.cm_typeDetail.Where(e => e.Type == "season" && e.Status == "active").Select(e => new { key = e.Code, text = e.Name }).ToList();
                return WebApi.GetSuccessHttpResponseMessage(lst);
            }
        }

        public virtual HttpResponseMessage GetTypeByValue(string text) 
        {
            using (EFContext entities = new EFContext())
            {
                var lst = entities.cm_typeDetail.Where(e => e.Type == "season" && e.Status == "active" && e.Name.Contains(text)).Select(e => new { key = e.Code, text = e.Name }).ToList();
                return WebApi.GetSuccessHttpResponseMessage(lst);
            }
        }

        public virtual HttpResponseMessage GetTestObject(int listId) 
        {
            using (EFContext entities = new EFContext())
            {
                var test_list = entities.test_list.SingleOrDefault(e => e.ListId == listId);
                return WebApi.GetSuccessHttpResponseMessage(test_list);
            }
        }
        public virtual HttpResponseMessage GetCascaderSync(int level,string value)
        {
            using (EFContext entities = new EFContext())
            {
                if (level == 0)
                {
                    var test_list = entities.cm_menu.Select(e => new { value = e.MnuNo, label = e.MnuName, level = 1, isLeaf=false });
                    return WebApi.GetSuccessHttpResponseMessage(test_list);
                }
                else if (level == 1)
                {
                    var test_list = entities.cm_menuModule.Where(e => e.MnuNo == value).Select(e => new { value = e.MnuModuleNo, label = e.MnuModeuleName, level = 2, isLeaf = false });
                    return WebApi.GetSuccessHttpResponseMessage(test_list);
                }
                else 
                {
                    var test_list = entities.cm_menuOperate.Where(e => e.MnuModuleNo == value).Select(e => new { value = e.MnuOperateNo, label = e.MnuOperateName, level = 3, isLeaf = true });
                    return WebApi.GetSuccessHttpResponseMessage(test_list);
                }
            }
        }
        public virtual HttpResponseMessage GetCascader()
        {
            using (EFContext entities = new EFContext())
            {
                List<ValueLabel> 省 = new List<ValueLabel>();
                List<ValueLabel> 市 = new List<ValueLabel>();
                List<ValueLabel> 区 = new List<ValueLabel>();
                区.Add(new ValueLabel("1","西陵区",true));
                区.Add(new ValueLabel("2", "夷陵区", true));
                市.Add(new ValueLabel("1","宜昌市",区));
                省.Add(new ValueLabel("1","湖北省",市));

                List<ValueLabel> 市1 = new List<ValueLabel>();
                List<ValueLabel> 区1 = new List<ValueLabel>();
                区1.Add(new ValueLabel("3", "天河区", true));
                区1.Add(new ValueLabel("4", "越秀区", true));
                市1.Add(new ValueLabel("2", "广州市", 区1));
                省.Add(new ValueLabel("2", "广东省", 市1));
                return WebApi.GetSuccessHttpResponseMessage(省);

            }
        }

        public virtual HttpResponseMessage GetAutoComplete()
        {
            using (EFContext entities = new EFContext())
            {
                var lst = entities.cm_typeDetail.Where(e => e.Type == "season" && e.Status == "active" ).Select(e => e.Name).ToList();
                return WebApi.GetSuccessHttpResponseMessage(lst);
            }
        }

        public  HttpResponseMessage GetTreeSync(string key,int level)
        {
            TestDataSource tds = new TestDataSource();
            if (level == 0)
            {
                var lst = tds.District.AsEnumerable().Where(dr => dr["parentid"] == DBNull.Value)
                    .Select(dr => new KeyTitle
                    {
                        key = dr["id"].ToString(),
                        title = dr["name"].ToString(),
                        level = Convert.ToInt32(dr["level"])
                    }
                        ).ToList();
                return WebApi.GetSuccessHttpResponseMessage(lst);
            }
            else
            {
                var lst = tds.District.AsEnumerable().Where(dr => dr["parentid"].ToString() == key)
                    .Select(dr => new KeyTitle
                    {
                        key = dr["id"].ToString(),
                        title = dr["name"].ToString(),
                        level = Convert.ToInt32(dr["level"])
                    }
                        ).ToList();
                return WebApi.GetSuccessHttpResponseMessage(lst);
            }
        }

        public virtual HttpResponseMessage GetListTree()
        {
            using (EFContext entities = new EFContext())
            {
                List<ValueLabel> 省 = new List<ValueLabel>();
                List<ValueLabel> 市 = new List<ValueLabel>();
                List<ValueLabel> 区 = new List<ValueLabel>();
                区.Add(new ValueLabel("c1", "西陵区", true));
                区.Add(new ValueLabel("c2", "夷陵区", true));
                市.Add(new ValueLabel("b1", "宜昌市", 区));
                省.Add(new ValueLabel("a1", "湖北省", 市));

                List<ValueLabel> 市1 = new List<ValueLabel>();
                List<ValueLabel> 区1 = new List<ValueLabel>();
                区1.Add(new ValueLabel("c3", "天河区", true));
                区1.Add(new ValueLabel("c4", "越秀区", true));
                市1.Add(new ValueLabel("b2", "广州市", 区1));
                省.Add(new ValueLabel("a2", "广东省", 市1));
                //List<dynamic> lst = new List<dynamic>();
                //lst.Add(new { value = 1, label = "aa", level = 1, isLeaf = false });
                return WebApi.GetSuccessHttpResponseMessage(new
                {
                    gridDataSource = 省
                });

            }
        }
    }

    public class TestDataSource
    {
        public DataTable District
        {
            get
            {
                using (EFContext entities = new EFContext())
                {
                    string sql = @"select 'A1' id, '湖北省'  name,1  level,null parentid
union
select 'A2' id, '广东省'  name,1  level,null parentid
union 
select 'A3' id, '湖南省'  name,1  level,null parentid
union 
select 'B1' id, '武汉市'  name,2  level,'A1' parentid
union 
select 'B2' id, '宜昌市'  name,2  level,'A1' parentid
union 
select 'B3' id, '荆州市'  name,2  level,'A1' parentid
union 
select 'C1' id, '洪山区'  name,3  level,'B1' parentid
union 
select 'C2' id, '武昌区'  name,3  level,'B1' parentid";
                    DataTable dt = entities.ExecuteDataTable(sql);
                    return dt;
                }
            }

        }
        */

    }
}
