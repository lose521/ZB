using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ZB.EntityFramework.SqlServer;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Common;

namespace ZB.EntityFramework.DataAccess
{
    public static class EntityExtension
    {
        public static object ExecuteScalar(this EFContext context, string sql, Dictionary<string, Object> parameters = null)
        {
            var row = ExecuteDataRow(context, sql, parameters);
            if (row == null)
                return null;
            return row[0];
        }


        public static DataRow ExecuteDataRow(this EFContext context, string sql, Dictionary<string, Object> parameters = null)
        {
            var table = ExecuteDataTable(context, sql, CommandType.Text, parameters);
            if (table == null || table.Rows.Count == 0)
                return null;
            return table.Rows[0];
        }

        public static DataTable ExecuteDataTable(this EFContext context, string sql, CommandType commandType = CommandType.Text, Dictionary<string, Object> parameters = null)
        {
            var set = ExecuteDataSet(context, sql, commandType, parameters);
            if (set == null || set.Tables.Count == 0)
                return null;
            return set.Tables[0];
        }


        public static DataSet ExecuteDataSet(this EFContext context, string sql, CommandType commandType = CommandType.Text, Dictionary<string, Object> parameters = null)
        {
            // creates resulting dataset
            var result = new DataSet();

            //var logBuilder = new StringBuilder();

            // creates a data access context (DbContext descendant)
            // creates a Command 
            var cmd = context.Database.Connection.CreateCommand();
            cmd.CommandType = commandType;
            cmd.CommandText = sql;

            //logBuilder.AppendLine(MethodBase.GetCurrentMethod().Name);
            //logBuilder.AppendLine(string.Format("CommandType:{0}", commandType));
            //logBuilder.AppendLine(string.Format("CommandText:{0}", sql));
            //logBuilder.AppendLine(string.Format("Parameters:"));


            if (parameters != null)
            {
                // adds all parameters
                foreach (var pr in parameters)
                {
                    var p = cmd.CreateParameter();
                    p.ParameterName = pr.Key;
                    p.Value = pr.Value;
                    cmd.Parameters.Add(p);

                    //logBuilder.AppendLine(string.Format("{0}={1}", p.ParameterName, Convert.ToString(p.Value)));

                }
            }
            //_logger.Trace(logBuilder.ToString());

            try
            {
                // executes
                context.Database.Connection.Open();

                // 统一使用此方法执行，拦截器才会输出SQL
                var reader = DbInterception.Dispatch.Command.Reader(cmd, new DbCommandInterceptionContext());
                //var reader = cmd.ExecuteReader();

                // loop through all resultsets (considering that it's possible to have more than one)
                do
                {
                    // loads the DataTable (schema will be fetch automatically)
                    var tb = new DataTable();
                    tb.Load(reader);
                    result.Tables.Add(tb);

                    //if (tb != null)
                    //    _logger.Trace(string.Format("Return Rows:{0}", tb.Rows.Count));

                } while (!reader.IsClosed);

            }
            catch (Exception ex)
            {
                throw;
                //_logger.Error(ex.Message + "\n" + ex.StackTrace);
                //if (ex.InnerException != null)
                //    _logger.Error(ex.InnerException.Message + "\n" + ex.InnerException.StackTrace);

            }
            finally
            {
                // closes the connection
                context.Database.Connection.Close();
            }

            // 列表数据量太大写日志，超过1000行
            if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count >= 1000)
            {
                var table = result.Tables[0];
                
            }


            // returns the DataSet
            return result;
        }
        public static DataSet ExecuteDataSet(this EFContext context, string sql, CommandType commandType = CommandType.Text, DbParameter[] parameters = null)
        {
            // creates resulting dataset
            var result = new DataSet();

            //var logBuilder = new StringBuilder();

            // creates a data access context (DbContext descendant)
            // creates a Command 
            var cmd = context.Database.Connection.CreateCommand();
            cmd.CommandType = commandType;
            cmd.CommandText = sql;

            //logBuilder.AppendLine(MethodBase.GetCurrentMethod().Name);
            //logBuilder.AppendLine(string.Format("CommandType:{0}", commandType));
            //logBuilder.AppendLine(string.Format("CommandText:{0}", sql));
            //logBuilder.AppendLine(string.Format("Parameters:"));


            if (parameters != null)
            {
                // adds all parameters
                foreach (var pr in parameters)
                {
                    cmd.Parameters.Add(pr);

                    //logBuilder.AppendLine(string.Format("{0}={1}", p.ParameterName, Convert.ToString(p.Value)));

                }
            }
            //_logger.Trace(logBuilder.ToString());

            try
            {
                // executes
                context.Database.Connection.Open();

                // 统一使用此方法执行，拦截器才会输出SQL
                var reader = DbInterception.Dispatch.Command.Reader(cmd, new DbCommandInterceptionContext());
                //var reader = cmd.ExecuteReader();

                // loop through all resultsets (considering that it's possible to have more than one)
                do
                {
                    // loads the DataTable (schema will be fetch automatically)
                    var tb = new DataTable();
                    tb.Load(reader);
                    result.Tables.Add(tb);

                    //if (tb != null)
                    //    _logger.Trace(string.Format("Return Rows:{0}", tb.Rows.Count));

                } while (!reader.IsClosed);

            }
            catch (Exception ex)
            {
                throw;
                //_logger.Error(ex.Message + "\n" + ex.StackTrace);
                //if (ex.InnerException != null)
                //    _logger.Error(ex.InnerException.Message + "\n" + ex.InnerException.StackTrace);

            }
            finally
            {
                // closes the connection
                context.Database.Connection.Close();
            }

            // 列表数据量太大写日志，超过1000行
            if (result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count >= 1000)
            {
                var table = result.Tables[0];
               // _slowQueryLogger.Warn(string.Format("SQL:{0}\n数据集过大：行数：{1}，列数：{2}", sql, table.Rows.Count, table.Columns.Count));
            }


            // returns the DataSet
            return result;
        }
        public static string GetPrimaryKey<T>(this DbContext context, T entity)
        {
            var primaryKeys = GetPrimaryKeys(context, entity);
            if (primaryKeys.Count == 0)
                return null;
            return primaryKeys[0];
        }
        public static List<string> GetPrimaryKeys<T>(this DbContext context, T entity)
        {
            return ((IObjectContextAdapter)context).ObjectContext.CreateEntityKey(typeof(T).Name, entity).EntityKeyValues.Select(kv => kv.Key).ToList();
        }

    }
}
