using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ZB.Common.Extensions;
using ZB.Common.Handler;
using ZB.EntityFramework.SqlServer;
using ZB.EntityFramework.DataAccess;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Converters;
namespace ZB.Web.Controllers.Framework
{
    public class AntdTableController : ApiController
    {
        public delegate DataTable PagedGetDataMethod(int pageIndex, int pageSize, ref int recordCount, string whereT, string order);
        public PagedGetDataMethod PagedGetData = null;

        public delegate DataTable GetDataSourceMethod();
        public GetDataSourceMethod GetDataSource = null;

        private bool _allowCustomPaging = true;
        /// <summary>
        /// 允许数据库分页
        /// </summary>
        public bool AllowCustomPaging
        {
            get
            {
                return _allowCustomPaging;
            }
            set
            {
                _allowCustomPaging = value;
            }
        }
        public string PrimaryKey { get; set; }
        public string SelectCommandText { get; set; }

        string GetCondition(List<List<object>> lstCondition)
        {
            string conditon = "";
            foreach (List<object> lst in lstCondition)
            {
                string fldKey = lst[0].ToStr();
                string operate = lst[1].ToStr();
                
                string where = "";
                switch (operate.ToLower())
                {
                    case "c":
                        string value1 = lst[2].ToStr();
                        where = string.Format("{0} like '%{1}%'", fldKey, value1);
                        break;
                    case "<>":
                        string[] value2 = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(lst[2].ToStr());
                        where = string.Format("{0} between '{1}' and '{2}'", fldKey, value2[0], value2[1]);
                        break; 
                    default:
                        string value3 = lst[2].ToStr();
                        where = string.Format("{0}{1}'{2}'", fldKey,operate, value3);
                        break;
                }
                conditon = conditon == "" ? where : conditon + " and " + where;
            }
            return conditon;
        }
        public virtual HttpResponseMessage GetListData()
        {
            try
            {
                var context = HttpContext.Current;
                var condition = context.Request["condition"].ToStr();//查询控件
                var filter = context.Request["filter"].ToStr();//自定义过滤条件
               
                var current = context.Request["current"].ToInt();//当前页
                var pageSize = context.Request["pageSize"].ToInt();//每页显示数chaxd

                if (pageSize == 0)
                    pageSize = 100000;
                string where = "";
                if (!string.IsNullOrEmpty(condition))
                {
                    List<List<object>> lstCondition = Newtonsoft.Json.JsonConvert.DeserializeObject<List<List<object>>>(condition);
                    where = GetCondition(lstCondition);
                }
                
                if(!string.IsNullOrEmpty(filter.Trim()))
                {
                    if(!string.IsNullOrEmpty(where.Trim()))
                    {
                        where = where + string.Format("({0})", filter);
                    }
                    else
                    {
                        where = filter;
                    }
                }
                var jsonString = GetDataSourceJsonString(current, pageSize, where, "");
                //var result = new HttpResponseMessage { Content = new StringContent(jsonString, Encoding.GetEncoding("UTF-8"), "application/json") };
                //return result;
                IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
                timeFormat.DateTimeFormat = "yyyy-MM-dd";
                return WebApi.GetSuccessHttpResponseMessage(jsonString, timeFormat);
            }
            catch (Exception ex)
            {
                //_logger.Error(ex.Message + "\n" + ex.StackTrace);
                var result = new HttpResponseMessage { Content = new StringContent(ex.Message + "\n" + ex.StackTrace, Encoding.GetEncoding("UTF-8"), "application/json") };
                return result;
            }
        }

        //public virtual HttpResponseMessage GetOperateListData()
        //{
        //    try
        //    {
        //        var context = HttpContext.Current;
        //        var mnuOperateOpsition = context.Request["mnuOperateOpsition"].ToStr(); 
        //        var mnuModuleNo = context.Request["mnuModuleNo"].ToStr();//自定义过滤条件
        //        using (EFContext efContext = new EFContext())
        //        {
        //            List<cm_menuOperate> lstMenuOperate = efContext.cm_menuOperate.Where(e => e.MnuModuleNo == mnuModuleNo && e.MnuOperatePosition == mnuOperateOpsition).OrderBy(e=>e.MnuOperateOrder).ToList();
        //            return WebApi.GetSuccessHttpResponseMessage(lstMenuOperate);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        private dynamic GetDataSourceJsonString(int current, int pageSize, string filter = "", string sort = "")
        {
            var skip = current * pageSize + 1;
            int recordCount = 0;
            DataTable table = null;

            if (AllowCustomPaging)
            {
                if (PagedGetData != null)
                {
                    table = PagedGetData(current, pageSize, ref recordCount, filter, sort);
                }
                else
                {
                    if (string.IsNullOrEmpty(this.SelectCommandText))
                        throw new Exception("未设置SelectCommandText");
                    if (string.IsNullOrEmpty(this.PrimaryKey))
                        throw new Exception("未设置PrimaryKey");

                    table = SysList.GetDataByPage(this.SelectCommandText, this.PrimaryKey, current, pageSize, ref recordCount, filter, sort);
                }
            }
            else
            {
                if (GetDataSource != null)
                {
                    table = GetDataSource();

                    if (!string.IsNullOrEmpty(filter))
                    {
                        if (string.IsNullOrEmpty(sort))
                            table = table.Select(filter).CopyToDataTable(table);
                        else
                            table = table.Select(filter, sort).CopyToDataTable(table);
                    }
                    else if (!string.IsNullOrEmpty(sort))
                    {
                        table = table.Select("1=1", sort).CopyToDataTable(table);
                    }

                }
                else
                {
                    if (string.IsNullOrEmpty(this.SelectCommandText))
                        throw new Exception("未设置SelectCommandText");
                    if (string.IsNullOrEmpty(this.PrimaryKey))
                        throw new Exception("未设置PrimaryKey");
                    using (var context = new EFContext())
                    {
                        var sql = this.SelectCommandText;
                        int sortIndex = sql.IndexOf("order by", StringComparison.OrdinalIgnoreCase);
                        if (sortIndex > 0)
                        {
                            if (string.IsNullOrEmpty(sort))
                                sort = sql.Substring(sortIndex + "order by".Length + 1);
                            sql = sql.Substring(0, sortIndex);
                        }

                        if (!string.IsNullOrEmpty(filter))
                        {
                            if (string.IsNullOrEmpty(sort))
                                sql = string.Format("select * from ({0}) a where {1}",sql, filter);
                            else
                                sql = string.Format("select * from ({0}) a where {1} order by {2}",sql, filter, sort);
                        }
                        else if (!string.IsNullOrEmpty(sort))
                        {
                            sql = string.Format("select * from ({0}) a order by {1}",sql, sort);
                        }

                        table = context.ExecuteDataTable(sql);
                        recordCount = table.Rows.Count;
                    }
                }

                // 列表数据量太大写日志，超过1000行
                if (table.Rows.Count >= 1000)
                {
                    //_slowQueryLogger.Warn("列表数据集过大：行数：{0}，列数：{1}".FormatString(table.Rows.Count, table.Columns.Count));
                }
                // 分页
                recordCount = table.Rows.Count;
                if (table.Rows.Count > 0)
                    table = table.AsEnumerable().Skip(skip).Take(pageSize).CopyToDataTable();

            }

            

            //table = table.FormatDataTableFieldNameAsR51Standard();

            //var jsonString = JsonConvert.SerializeObject(new
            //{
            //    totalCount = recordCount,
            //    currentCount = table.Rows.Count,
            //    gridDataSource = table,
            //});
            //return jsonString;
            return new
            {
                totalCount = recordCount,
                currentCount = table.Rows.Count,
                gridDataSource = table
            };
        }
    }
}
