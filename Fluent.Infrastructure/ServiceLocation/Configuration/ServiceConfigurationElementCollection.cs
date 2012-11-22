using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.ServiceLocation.Configuration
{
    public class ServiceConfigurationElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            var ele = element as ServiceConfigurationElement;
            if (ele == null)
                throw new ArgumentNullException("element", "element不能转换成 ServiceConfigurationElement");
            return ele.Key;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return ServiceLocationConst.ServiceName; }
        }
    }
}
