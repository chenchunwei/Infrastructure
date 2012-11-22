using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Fluent.Infrastructure.Web.HttpMoudles.Configuration
{
    public class IgnorePathConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("path", IsRequired = true)]
        public string Path
        {
            get { return this["path"].ToString(); }
            set { this["path"] = value; }
        }
    }
}