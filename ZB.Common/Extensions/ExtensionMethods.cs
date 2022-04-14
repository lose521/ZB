using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZB.Common.Extensions
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// 转换为 int，如果转换失败，返回 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int ToInt<T>(this T source)
        {
            if (source == null)
                return 0;
            if (source.Equals(DBNull.Value))
                return 0;

            int returnValue;

            if (int.TryParse(source.ToString(), out returnValue))
            {
                return returnValue;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 转换为 int?
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int? ToIntNull<T>(this T source)
        {
            if (source == null)
                return null;
            if (source.Equals(DBNull.Value))
                return null;

            return source.ToInt();
        }


        /// <summary>
        /// 转换为 long，如果转换失败，返回 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static long ToLong<T>(this T source)
        {
            if (source == null)
                return 0;
            if (source.Equals(DBNull.Value))
                return 0;

            long returnValue;

            if (long.TryParse(source.ToString(), out returnValue))
            {
                return returnValue;
            }
            else
            {
                return 0;
            }
        }

        public static bool ToBool<T>(this T source)
        {
            if (source == null)
                return false;
            if (source.Equals(DBNull.Value))
                return false;

            bool returnValue;

            if (bool.TryParse(source.ToString(), out returnValue))
            {
                return returnValue;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 转换为 decimal，如果转换失败，返回 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static decimal ToDecimal<T>(this T source)
        {
            if (source == null)
                return 0m;
            if (source.Equals(DBNull.Value))
                return 0m;
            decimal returnValue;
            if (decimal.TryParse(source.ToString(), out returnValue))
            {
                return returnValue;
            }
            else
            {
                return 0m;
            }
        }

        /// <summary>
        /// 转换为 decimal?
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static decimal? ToDecimalNull<T>(this T source)
        {
            if (source == null)
                return null;
            if (source.Equals(DBNull.Value))
                return null;

            return source.ToDecimal();
        }


        /// <summary>
        /// 转换为字符串，会做处理将单引号'转换为两个单引号''
        /// 会进行Anti XSS消毒处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToStr<T>(this T source)
        {
            if (source == null)
                return string.Empty;
            if (source.Equals(DBNull.Value))
                return string.Empty;
            return Convert.ToString(source);
        }
        /// <summary>
        /// 转换为日期，如果转换失败，返回 null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTime? ToDateNull<T>(this T source)
        {
            if (source == null)
                return null;
            if (source.Equals(DBNull.Value))
                return null;
            DateTime returnValue;
            if (DateTime.TryParse(source.ToString(), out returnValue))
            {
                return returnValue;
            }
            else
            {
                return null;
            }
        }
        public static Guid ToGuid(this object obj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
                return Guid.Empty;
            Guid gid;
            if (Guid.TryParse(obj.ToString(), out gid))
                return gid;

            return Guid.Empty;
        }

        public static Guid? ToGuidNull(this object obj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
                return null;
            Guid gid;
            if (Guid.TryParse(obj.ToString(), out gid))
                return gid;

            return null;
        }

        public static T ToEnum<T>(this object obj)
         where T : struct
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Type must is a enum");
            if (obj == null || obj == DBNull.Value || obj.ToString() == string.Empty)
                return default(T);

            var result = default(T);
            Enum.TryParse<T>(obj.ToStr(), out result);
            return (T)result;
        }
 
     


        public static bool EqualsIgnoreCase(this string str, string compareStr)
        {
            return str.ToStr().Equals(compareStr, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsGuid(this string str)
        {
            Match m = Regex.Match(str, @"^[0-9a-f]{8}(-[0-9a-f]{4}){3}-[0-9a-f]{12}$", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                //可以转换
                try
                {
                    Guid guid = new Guid(str);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                //不可转换 
                return false;
            }
        }


        /// <summary>
        /// 根据字符串进行分隔
        /// </summary>
        /// <param name="str"></param>
        /// <param name="splitString"></param>
        /// <returns></returns>
        public static string[] Split(this string str, string splitString)
        {
            if (string.IsNullOrEmpty(str.ToString()))
                return new string[] { };
            return str.Split(new[] { splitString }, StringSplitOptions.None);
        }

        /// <summary>
        /// 复制行集合到 Datable
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public static DataTable CopyToDataTable(this IEnumerable<DataRow> rows, DataTable table)
        {
            if (rows == null)
                return table.Clone();
            return rows.Any() ? rows.CopyToDataTable() : table.Clone();
        }

        public static string ToJsonString(this DataTable source)
        {
            var table = source.Copy();
            if (!table.Columns.Contains("DataRowState"))
                table.Columns.Add("DataRowState", typeof(string));
            foreach (DataRow row in table.Rows)
            {
                if (row.RowState == DataRowState.Deleted)
                {
                    row.RejectChanges();
                    row["DataRowState"] = DataRowState.Deleted.ToString();
                }
                else
                {
                    row["DataRowState"] = row.RowState.ToString();
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(table);

        }

 
    
      
 

        public static DataTable ToDataTable<T>(this IEnumerable<T> list) where T : class
        {
            List<PropertyInfo> pList = new List<PropertyInfo>(); //创建属性的集合 
            Type type = typeof(T);//获得反射的入口 
            DataTable dt = new DataTable();
            Array.ForEach<PropertyInfo>(type.GetProperties(), p => { pList.Add(p); dt.Columns.Add(p.Name, p.PropertyType); });//把所有的public属性加入到集合 并添加DataTable的列   
            foreach (var item in list)
            {
                DataRow row = dt.NewRow(); //创建一个DataRow实例  
                pList.ForEach(p => row[p.Name] = p.GetValue(item, null));//给row 赋值 
                dt.Rows.Add(row);//加入到DataTable 
            }
            return dt;
        }

 
 
 
    }
}
