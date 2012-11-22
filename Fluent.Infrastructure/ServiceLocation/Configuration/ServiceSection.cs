using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.ServiceLocation.Configuration
{
    public class ServiceSection : ConfigurationSection
    {
        [ConfigurationProperty(ServiceLocationConst.AssemblyNameOfSection)]
        public string AssemblyName
        {
            get { return (string)this[ServiceLocationConst.AssemblyNameOfSection]; }
            set { this[ServiceLocationConst.AssemblyNameOfSection] = value; }
        }

        [ConfigurationProperty(ServiceLocationConst.ServiceCollectionName, IsRequired = false)]
        public ServiceConfigurationElementCollection IgnorePaths
        {
            get { return (ServiceConfigurationElementCollection)(base[ServiceLocationConst.ServiceCollectionName]); }
            set { this[ServiceLocationConst.ServiceCollectionName] = value; }
        }
    }
}
