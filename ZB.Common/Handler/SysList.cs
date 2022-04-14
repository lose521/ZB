using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZB.EntityFramework.SqlServer;
using ZB.EntityFramework.DataAccess;
using System.Data.SqlClient;
namespace ZB.Common.Handler
{
    public class SysList
    {
        public static DataTable GetDataByPage(string selectCommandText, string primaryKey, int pageindex, int pageSize, ref int recordCount, string sWhere, string sOrder)
        {
            using (EFContext ef = new EFContext())
            {
                //string select = selectCommandText;
                ////判断当前最大页  删除最后一条数据 可能造成页码不对
                //int index = selectCommandText.IndexOf(" order ", StringComparison.OrdinalIgnoreCase);
                //if (index > 0)
                //{
                //    select = selectCommandText.Substring(0, index);
                //}
                //select = string.Format(@"select count(*) from ({0}) v where {1}", select, sWhere);
                //int count = Convert.ToInt32(ef.ExecuteScalar(select));

                string sql = @"DECLARE	
                             @pageCount int,
                             @recordCount int

                        EXEC	[Prc_splitpage2]
                             @sql = N'{0}',
                                @idField = '{5}',
                             @page = {1},
                             @pageSize = {2},
                             @pageCount = @pageCount OUTPUT,
                             @recordCount = @recordCount OUTPUT,
                             @where = N'{3}',
                             @order = N'{4}'

                        SELECT	@recordCount as RecordCount";
                if (!string.IsNullOrEmpty(sOrder))
                {
                    int sortIndex = selectCommandText.IndexOf("order by", StringComparison.OrdinalIgnoreCase);
                    if (sortIndex < 0)
                    {
                        selectCommandText += " order by " + sOrder;
                    }
                    else
                    {

                        if (selectCommandText.IndexOf('$') > 0)
                        {
                            selectCommandText = selectCommandText.Substring(0, selectCommandText.IndexOf('$')) +
                                               " order by " + sOrder;
                        }

                    }
                }

              string  sql1 = string.Format(sql, new object[] {
               selectCommandText.Replace('$', ' ').Replace("'","''"),
                pageindex,
                pageSize,
                sWhere.Replace("'","''"),
                sOrder??"".Replace("'","''"),
                primaryKey
            });

                // 严谨性检查
                if (string.IsNullOrEmpty(primaryKey))
                    throw new Exception("未配置主键，请检查！");
                DataSet ds;

                ds = ef.ExecuteDataSet(sql1, CommandType.Text, new Dictionary<string, object>());

                var dataSource = ds.Tables[0];
                recordCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                dataSource.PrimaryKey = new DataColumn[] { dataSource.Columns[primaryKey] };

                if (recordCount > 0 && dataSource.Rows.Count == 0) //判断当前最大页 删除最后一条数据 可能造成页码不对,造成无数据
                {
                    string sql2 = string.Format(sql, new object[] {
                       selectCommandText.Replace('$', ' ').Replace("'","''"),
                        pageindex-1,
                        pageSize,
                        sWhere.Replace("'","''"),
                        sOrder??"".Replace("'","''"),
                        primaryKey
                    });
                    ds = ef.ExecuteDataSet(sql2, CommandType.Text, new Dictionary<string, object>());
                    dataSource = ds.Tables[0];
                }
                return dataSource;
            }
        }
        //public static DataTable GetDataByPage(string selectCommandText, string primaryKey, int pageindex, int pageSize, ref int recordCount, string sWhere, string sOrder)
        //{
        //    // 严谨性检查
        //    if (string.IsNullOrEmpty(primaryKey))
        //        throw new Exception("未配置主键，请检查！");

        //    if (!string.IsNullOrEmpty(sOrder))
        //    {
        //        int sortIndex = selectCommandText.IndexOf("order by", StringComparison.OrdinalIgnoreCase);
        //        if (sortIndex < 0)
        //        {
        //            selectCommandText += " order by " + sOrder;
        //        }
        //        else
        //        {

        //            if (selectCommandText.IndexOf('$') > 0)
        //            {
        //                selectCommandText = selectCommandText.Substring(0, selectCommandText.IndexOf('$')) +
        //                                   " order by " + sOrder;
        //            }

        //        }
        //    }

        //    DataSet ds;
        //    var context = new EFContext();
            
        //        var parameters = new SqlParameter[] {
        //                new SqlParameter { ParameterName = "PINDEX", SqlDbType = SqlDbType.Decimal, Value=pageindex },
        //                new SqlParameter { ParameterName = "PSQL", SqlDbType = SqlDbType.NVarChar, Value = selectCommandText },
        //                new SqlParameter { ParameterName = "PSIZE", SqlDbType = SqlDbType.Decimal, Value=pageSize },
        //                new SqlParameter { ParameterName = "PWHERE", SqlDbType = SqlDbType.NVarChar, Value=sWhere},
        //                new SqlParameter { ParameterName = "PORDFLD", SqlDbType = SqlDbType.NVarChar, Value=sOrder },
        //                new SqlParameter { ParameterName = "PCOUNT", SqlDbType = SqlDbType.Decimal, Direction = ParameterDirection.Output },
        //                new SqlParameter { ParameterName = "V_CUR", SqlDbType = SqlDbType.RefCursor, Direction = ParameterDirection.Output },
        //                new SqlParameter { ParameterName = "PRCOUNT", SqlDbType = SqlDbType.Decimal, Direction = ParameterDirection.Output },
        //            };

        //        var sql = "PAGINATION_PK.PAGINATION_PROC";


        //        ds = context.ExecuteDataSet(sql, CommandType.StoredProcedure, parameters);
        //        var dataSource = ds.Tables[0];
        //        var recordCountValue = parameters.First(c => c.ParameterName == "PRCOUNT").Value.ToString();
        //        recordCount = Convert.ToInt32(recordCountValue);
        //        dataSource.PrimaryKey = new DataColumn[] { dataSource.Columns[primaryKey] };
        //        return dataSource;
            

        //}
    }
}
