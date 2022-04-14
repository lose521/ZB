using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZB.EntityFramework.SqlServer;
using System.ComponentModel.DataAnnotations;

namespace ZB.EntityFramework.DataAccess
{
    public abstract class EFBaseDAL<T> where T : class,new()
    {
        public EFContext context { get; set; }
        public EFBaseDAL() : this(new EFContext())
        {

        }

        public EFBaseDAL(EFContext efContext)
        {
            this.context = efContext;
        }

        //public int Add(T model)
        //{
        //    context.Set<T>().Add(model);
        //    //保存成功后，会将自增的id设置给model的主键属性，并返回受影响的行数。
        //    return context.SaveChanges();
        //}

        string GetKeyProperty(Type entityType)
        {
            foreach (var prop in entityType.GetProperties())
            {
                //var d = typeof(System.ComponentModel.DataAnnotations.kSchema.IndexAttribute);
               // var attr1 = prop.GetCustomAttributes(false);
               // var properties = entityType.GetProperties().Where(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Length == 1).Select(p => p);
                var attr = prop.GetCustomAttributes(typeof(KeyAttribute), false).FirstOrDefault()
                    as KeyAttribute;
                if (attr != null)
                    return prop.Name;
            }
            return null;
        }

        public virtual T Save(T model)
        {
            string pkey = GetKeyProperty(model.GetType());
            DbEntityEntry entry = this.context.Entry<T>(model);
            DbPropertyEntry propertyEntry = entry.Property(pkey);
            if (propertyEntry != null)
            {
                //修改最后更新时间
                DbPropertyEntry propertyEntryModifyDate = entry.Property("modifyDate");
                if (propertyEntryModifyDate != null)
                {
                    propertyEntryModifyDate.CurrentValue = DateTime.Now;
                }
                
                object value = propertyEntry.CurrentValue;
                if (Convert.ToInt32(value) == 0)
                {
                    DbPropertyEntry propertyEntryCreateDate = entry.Property("createDate");
                    if (propertyEntryCreateDate != null)
                    {
                        propertyEntryCreateDate.CurrentValue = DateTime.Now;
                    }
                    DbPropertyEntry propertyEntryStatus = entry.Property("status");
                    if (propertyEntryStatus != null)
                    {
                        propertyEntryStatus.CurrentValue = "A";
                    }
                    return Add(model);
                }
                    
                else
                    return Modify(model);
            }
            return Add(model);
        }
        public virtual T Add(T model)
        {
            context.Set<T>().Add(model);
            context.SaveChanges();
            return model;
        }

        public virtual bool Delete(T model)
        {
            return Delete(model, false);
        }
        public virtual bool Delete(T model, bool isDelete)
        {
            if (isDelete)
            {
                context.Set<T>().Attach(model);
                context.Set<T>().Remove(model);
                context.SaveChanges();
            }
            else
            {
                DbEntityEntry entry = this.context.Entry<T>(model);
                DbPropertyEntry propertyEntryStatus = entry.Property("status");
                if (propertyEntryStatus == null)
                {
                    throw new Exception("没有找到字段Status,无法逻辑删除");
                }
                DbPropertyEntry propertyEntryModifyDate = entry.Property("modifyDate");
                if (propertyEntryModifyDate != null)
                {
                    propertyEntryModifyDate.CurrentValue = DateTime.Now;
                }
                propertyEntryStatus.CurrentValue = "X";
                Modify(model,  "status" );               
            }
            return true;
        }
        public virtual int DeleteBy(Expression<Func<T, bool>> delWhere)
        {
            return DeleteBy(delWhere, false);
        }
        public virtual int DeleteBy(Expression<Func<T, bool>> delWhere,bool isDelete)
        {
            if (isDelete)
            {
                //2.1.1 查询要删除的数据
                List<T> listDeleting = context.Set<T>().Where(delWhere).ToList();
                //2.1.2 将要删除的数据 用删除方法添加到 EF 容器中
                listDeleting.ForEach(u =>
                {
                    context.Set<T>().Attach(u);  //先附加到EF 容器
                    context.Set<T>().Remove(u); //标识为删除状态
                });
                //2.1.3 一次性生成sql语句 到数据库执行删除
                return context.SaveChanges();
            }
            else
            {
                T model = new T();
                DbEntityEntry entry = this.context.Entry<T>(model);
                DbPropertyEntry propertyEntryStatus = entry.Property("status");
                if (propertyEntryStatus == null)
                {
                    throw new Exception("没有找到字段Status,无法逻辑删除");
                }
                propertyEntryStatus.CurrentValue = "X";
                DbPropertyEntry propertyEntryModifyDate = entry.Property("modifyDate");
                if (propertyEntryModifyDate != null)
                {
                    propertyEntryModifyDate.CurrentValue = DateTime.Now;
                }
                return ModifyListBy(model, delWhere, "status", "modifyDate");
            }
        }

        public virtual T Modify(T model)
        {
            DbEntityEntry entry = context.Entry<T>(model);
            entry.State = EntityState.Modified;
            context.SaveChanges();
            return model;
        }
        /// <summary>
        /// 指定修改列
        /// </summary>
        /// <param name="model"></param>
        /// <param name="propertyNames">指定要修改的列</param>
        /// <returns></returns>
        public virtual int Modify(T model, params string[] propertyNames)
        {
            //3.1.1 将对象添加到EF中
            DbEntityEntry entry = context.Entry<T>(model);
            //3.1.2 先设置对象的包装状态为 Unchanged
            entry.State = EntityState.Unchanged;
            //3.1.3 循环被修改的属性名数组
            foreach (string propertyName in propertyNames)
            {
                //将每个被修改的属性的状态设置为已修改状态；这样在后面生成的修改语句时，就只为标识为已修改的属性更新
                entry.Property(propertyName).IsModified = true;
            }
            return context.SaveChanges();
        }

        public virtual int ModifyListBy(T model, Expression<Func<T, bool>> whereLambda, params string[] modifiedPropertyNames)
        {
            //3.2.1 查询要修改的数据
            List<T> listModifing = context.Set<T>().Where(whereLambda).ToList();
            //3.2.2 获取实体类类型对象
            Type t = typeof(T);
            //3.2.3 获取实体类所有的公共属性
            List<PropertyInfo> propertyInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            //3.2.4 创建实体属性字典集合
            Dictionary<string, PropertyInfo> dicPropertys = new Dictionary<string, PropertyInfo>();
            //3.2.5 将实体属性中要修改的属性名 添加到字典集合中  键：属性名  值：属性对象
            propertyInfos.ForEach(p =>
            {
                if (modifiedPropertyNames.Contains(p.Name))
                {
                    dicPropertys.Add(p.Name, p);
                }
            });
            //3.2.6 循环要修改的属性名
            foreach (string propertyName in modifiedPropertyNames)
            {
                //判断要修改的属性名是否在实体类的属性集合中存在
                if (dicPropertys.ContainsKey(propertyName))
                {
                    //如果存在，则取出要修改的属性对象
                    PropertyInfo proInfo = dicPropertys[propertyName];
                    //取出要修改的值
                    object newValue = proInfo.GetValue(model, null);
                    //批量设置要修改对象的属性
                    foreach (T item in listModifing)
                    {
                        //为要修改的对象的要修改的属性设置新的值
                        proInfo.SetValue(item, newValue, null);
                    }
                }
            }
            //一次性生成sql语句 到数据库执行
            return context.SaveChanges();
        }

        /// <summary>
        /// 获取对象AsNoTracking不能修改属性
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public virtual T GetModel(Expression<Func<T, bool>> whereLambda)
        {
            return context.Set<T>().Where(whereLambda).AsNoTracking().FirstOrDefault();
        }

        public virtual T GetModel<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc = true)
        {
            if (isAsc)
            {
                return context.Set<T>().Where(whereLambda).OrderBy(orderLambda).AsNoTracking().FirstOrDefault();
            }
            else
            {
                return context.Set<T>().Where(whereLambda).OrderByDescending(orderLambda).AsNoTracking().FirstOrDefault();
            }
        }

        public virtual List<T> GetListBy(Expression<Func<T, bool>> whereLambda)
        {
            return context.Set<T>().Where(whereLambda).AsNoTracking().ToList();
        }

        public virtual List<T> GetListBy<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc = true)
        {
            if (isAsc)
            {
                return context.Set<T>().Where(whereLambda).OrderBy(orderLambda).AsNoTracking().ToList();
            }
            else
            {
                return context.Set<T>().Where(whereLambda).OrderByDescending(orderLambda).AsNoTracking().ToList();
            }
        }

        public virtual List<T> GetListBy<TKey>(int top, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda, bool isAsc = true)
        {
            if (isAsc)
            {
                return context.Set<T>().Where(whereLambda).OrderBy(orderLambda).Take(top).AsNoTracking().ToList();
            }
            else
            {
                return context.Set<T>().Where(whereLambda).OrderByDescending(orderLambda).Take(top).AsNoTracking().ToList();
            }
        }

        /// <summary>
        /// 分页查询 + List<T> GetPagedList
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页容量</param>
        /// <param name="whereLambda">条件 lambda表达式</param>
        /// <param name="orderBy">排序 lambda表达式</param>
        /// <returns></returns>
        public virtual List<T> GetPagedList<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderByLambda, bool isAsc = true)
        {
            // 分页 一定注意： Skip 之前一定要 OrderBy
            if (isAsc)
            {
                return context.Set<T>().Where(whereLambda).OrderBy(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
            }
            else
            {
                return context.Set<T>().Where(whereLambda).OrderByDescending(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
            }
        }

        /// <summary>
        /// 分页查询 带输出
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="rowCount"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderBy"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public virtual List<T> GetPagedList<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderByLambda,  out int rowCount,bool isAsc = false)
        {
            rowCount = context.Set<T>().Where(whereLambda).Count();
            if (isAsc)
            {
                return context.Set<T>().OrderBy(orderByLambda).Where(whereLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
            }
            else
            {
                return context.Set<T>().OrderByDescending(orderByLambda).Where(whereLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsNoTracking().ToList();
            }
        }

        public virtual List<T> GetPagedList<TKey>(IEnumerable<T> source, int pageIndex, int pageSize,  Expression<Func<T, TKey>> orderByLambda,out int rowCount, bool isAsc = false)
        {
            rowCount = source.Count();
            if (isAsc)
            {
                return source.AsQueryable().OrderBy(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return source.AsQueryable().OrderByDescending(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            }
        }
    }
}
