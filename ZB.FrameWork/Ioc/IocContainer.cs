using Autofac;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using ZB.EntityFramework.SqlServer;

namespace ZB.FrameWork.Ioc
{
    public class IocContainer
    {
        private static IContainer _container;
        public static IContainer Container
        {
            get
            {
                if (_container != null)
                    return _container;
                _container = new ContainerBuilder().Build();
                return _container;
            }
        }
        //private static string _currentCustomerCode = null;
        /// <summary>
        /// 当前客户编码
        /// </summary>
        //public static string CurrentCustomerCode
        //{
        //    get
        //    {
        //        if (_currentCustomerCode != null)
        //            return _currentCustomerCode;

        //        _currentCustomerCode = "STD"; // 默认标准版
        //        var temp = ConfigurationManager.AppSettings["CustomerCode"];
        //        _currentCustomerCode = temp;
        //        return _currentCustomerCode;
        //    }
        //}

        public static object Resolve(Type interfaceType)
        {
            if (Container.IsRegistered(interfaceType))
                return Container.Resolve(interfaceType);

            var fullTypeName = interfaceType.FullName;
            var pos = fullTypeName.LastIndexOf('.');
            if (pos < 0)
                return null;
            var classFullTypeName = fullTypeName.Substring(0, pos + 1) + fullTypeName.Substring(pos + 2);
            classFullTypeName = classFullTypeName.Replace(".IWebService.", ".WebService.");
            classFullTypeName = classFullTypeName.Replace(".IService.", ".Service.");
            classFullTypeName = classFullTypeName.Replace(".IBusiness.", ".Business.");
            var type = GetClassType(classFullTypeName);
            if (type == null)
                return null;

            //return Activator.CreateInstance(type);

            var builder = new ContainerBuilder();
            builder.RegisterType(type).As(interfaceType);
            builder.Update(Container);
            return Container.Resolve(interfaceType);
        }

        public static TInterface Resolve<TInterface>()
        {
            if (Container.IsRegistered<TInterface>())
                return Container.Resolve<TInterface>();

            var fullTypeName = typeof(TInterface).FullName;
            var pos = fullTypeName.LastIndexOf('.');
            if (pos < 0)
                return default(TInterface);
            var classFullTypeName = fullTypeName.Substring(0, pos + 1) + fullTypeName.Substring(pos + 2);
            classFullTypeName = classFullTypeName.Replace(".IBusiness.", ".Business.");
            var type = GetClassType(classFullTypeName);
            if (type == null)
                return default(TInterface);

            //return (TInterface)Activator.CreateInstance(type);

            var builder = new ContainerBuilder();
            builder.RegisterType(type).As<TInterface>();
            builder.Update(Container);
            return Container.Resolve<TInterface>();
        }

        public static TInterface Resolve<TInterface>(EFContext ef)
        {
            if (Container.IsRegistered<TInterface>())
                return Container.Resolve<TInterface>();

            var fullTypeName = typeof(TInterface).FullName;
            var pos = fullTypeName.LastIndexOf('.');
            if (pos < 0)
                return default(TInterface);
            var classFullTypeName = fullTypeName.Substring(0, pos + 1) + fullTypeName.Substring(pos + 2);
            classFullTypeName = classFullTypeName.Replace(".IBusiness.", ".Business.");
            var type = GetClassType(classFullTypeName);
            if (type == null)
                return default(TInterface);

            //return (TInterface)Activator.CreateInstance(type);

            var builder = new ContainerBuilder();
            builder.RegisterType(type).As<TInterface>();
            builder.Update(Container);
            return Container.Resolve<TInterface>(new NamedParameter("efContext",ef));
        }

        /// <summary>
        /// 根据类型全名获取对应的类型
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public static Type GetClassType(string fullTypeName)
        {
            var items = fullTypeName.Split('.');
            if (items.Length > 3)
            {
                //var dllName = string.Format("{0}.{1}.{2}", items[0], items[1], items[2]);
                var dllName = string.Format("{0}.{1}", items[0], items[1]);
                var binPath = System.IO.Path.Combine(WebAppDirectory, "Bin");

                //if (CurrentCustomerCode != "STD" && items[0] == "IFCA")
                //{
                //    var customerDllName = string.Format("{0}.{1}.{2}", CurrentCustomerCode, items[1], items[2]);
                //    var customerFullTypeName = string.Format("{0}.{1}", CurrentCustomerCode, fullTypeName.Substring(5));
                //    if (System.IO.File.Exists(System.IO.Path.Combine(binPath, customerDllName + ".dll")))
                //    {
                //        return Type.GetType(string.Format("{0}, {1}", customerFullTypeName, customerDllName));
                //    }
                //}

                //dllName = dllName.Replace(customerCode + ".", "*."); // 如果类名是 SHHL.Business.SM.SupplierService，只找 *.Business.SM.*.dll，这样就能找到

                var files = System.IO.Directory.GetFiles(binPath, dllName + "*.dll");
                foreach (var file in files)
                {
                    var fileName = System.IO.Path.GetFileName(file);
                    var type = Type.GetType(string.Format("{0}, {1}", fullTypeName, fileName.Substring(0, fileName.Length - 4)));
                    if (type != null)
                        return type;
                }

            }
            return Type.GetType(fullTypeName);
        }

        /// <summary>
        /// 网站的根目录物理路径，结果有带/
        /// </summary>
        private static string WebAppDirectory
        {
            get
            {
                var path = System.AppDomain.CurrentDomain.BaseDirectory;
                if (!path.EndsWith(@"\"))
                    path = path + @"\";
                return path;
            }
        }



    }
}
