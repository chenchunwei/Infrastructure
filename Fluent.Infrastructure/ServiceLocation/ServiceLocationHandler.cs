using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.ServiceLocation.Configuration;

namespace Fluent.Infrastructure.ServiceLocation
{
    public class ServiceLocationHandler
    {
        private ServiceLocationHandler() { }
        public static readonly object Locker = new object();
        private static readonly Dictionary<Type, object> ServiceStore = new Dictionary<Type, object>();
        public static T Resolver<T>()
            where T : class
        {
            var type = typeof(T);
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
    }
}
