using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Log;
using Fluent.Infrastructure.ServiceLocation.Configuration;
using log4net;

namespace Fluent.Infrastructure.ServiceLocation
{
    public class ServiceLocationHandler
    {
        private static readonly ILog Logger;
        private static int _times = 0;
        static ServiceLocationHandler()
        {
            Logger = new DefaultLoggerFactory().GetLogger();
        }
        private ServiceLocationHandler() { }
        public static readonly object Locker = new object();
        private static readonly Dictionary<Type, object> ServiceStore = new Dictionary<Type, object>();
        public static T Resolver<T>(bool isMust = true) where T : class
        {
            _times++;
            try
            {
                var type = typeof(T);
                Logger.DebugFormat("ServiceStore当前有{0}个对象,当前第{1}次访问", ServiceStore.Count, _times);
                if (!ServiceStore.ContainsKey(type))
                {
                    lock (Locker)
                    {
                        if (!ServiceStore.ContainsKey(type))
                        {
                            var serviceSection = (ServiceSection)ConfigurationManager.GetSection(ServiceLocationConst.ServiceSetionName);
                            foreach (ServiceConfigurationElement element in serviceSection.Services)
                            {
                                if (!string.Equals(type.ToString(), element.Key, StringComparison.CurrentCultureIgnoreCase))
                                    continue;
                                if (element.ClassName.IndexOf(",", StringComparison.CurrentCultureIgnoreCase) == -1)
                                    element.ClassName += "," + serviceSection.AssemblyName;
                                var classInfos = element.ClassName.Split(new[] { ',' });
                                var typeTarget = Type.GetType(element.ClassName);
                                if (typeTarget == null)
                                    throw new ArgumentException(element.ClassName + "无法初始化");
                                var service = Activator.CreateInstance(typeTarget) as T;
                                ServiceStore[type] = service;
                                break;
                            }
                        }
                    }
                }
                return (T)ServiceStore[type];
            }
            catch (Exception)
            {
                if (isMust)
                    throw;
                return null;
            }
        }
    }
}
