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
        private ServiceLocationHandler(){}
        public static readonly object Locker = new object();
        private static readonly Dictionary<Type, object> ServiceStore = new Dictionary<Type, object>();
        public static T Resolver<T>()
        {
            var type = typeof(T);
            if (!ServiceStore.ContainsKey(type))
            {
                lock (Locker)
                {
                    if (!ServiceStore.ContainsKey(type))
                    {
                        var services = (ServiceSection)ConfigurationManager.GetSection(ServiceLocationConst.ServiceSetionName);
                        var service = Activator.CreateInstance<T>();
                        if (service == null)
                            throw new ArgumentException(string.Format("未找到{0}类型的服务配置节点！", type.ToString()));
                    }
                }
            }
            return (T)ServiceStore[type];
        }
    }
}
