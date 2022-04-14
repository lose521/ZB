using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZB.Common.Tool
{
    public class Tools
    {
        public static void mapping(object obj1, object obj2)
        {
            Type type1 = obj1.GetType();
            Type type2 = obj2.GetType();

            var properties = (from p1 in type1.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                              where p1.CanRead && p1.CanWrite
                              from p2 in type2.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                              where p2.CanRead && p2.CanWrite
                              where p1.Name == p2.Name && p1.PropertyType == p2.PropertyType
                              select new { Property1 = p1, Property2 = p2 }).ToList();

            foreach (var props in properties)
            {
                object value1 = props.Property1.GetValue(obj1, null);
                props.Property2.SetValue(obj2, value1, null);
            }
        }
        //public static D CloneProperty<D, S>(S s)
        //{
        //    D d = Activator.CreateInstance<D>();
        //    try
        //    {
        //        var Types = s.GetType();//获得类型
        //        var Typed = typeof(D);
        //        foreach (System.Reflection.PropertyInfo sp in Types.GetProperties())//获得类型的属性字段
        //        {
        //            foreach (System.Reflection.PropertyInfo dp in Typed.GetProperties())
        //            {
        //                if (dp.Name == sp.Name)//判断属性名是否相同
        //                {
        //                    dp.SetValue(d, sp.GetValue(s, null), null);//获得s对象属性的值复制给d对象的属性
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return d;
        //}
    }
}
