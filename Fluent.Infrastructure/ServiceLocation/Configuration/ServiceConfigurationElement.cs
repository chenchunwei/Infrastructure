using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.ServiceLocation.Configuration
{
    public class ServiceConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty(ServiceLocationConst.ClassNameOfService, IsRequired = true)]
        public string ClassName
        {
            get { return this[ServiceLocationConst.ClassNameOfService].ToString(); }
            set { this[ServiceLocationConst.ClassNameOfService] = value; }
        }

        [ConfigurationProperty(ServiceLocationConst.KeyOfService, IsRequired = true)]
        public string Key
        {
            get { return this[ServiceLocationConst.KeyOfService].ToString(); }
            set { this[ServiceLocationConst.KeyOfService] = value; }
        }
    }
}
