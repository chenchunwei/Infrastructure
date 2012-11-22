using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Fluent.Infrastructure.Web.HttpMoudles.Configuration
{
    public class AuthencationSection : ConfigurationSection
    {
        [ConfigurationProperty("ignorePaths", IsRequired = false)]
        public IgnorePathConfigurationElementCollection IgnorePaths
        {
            get { return (IgnorePathConfigurationElementCollection)(base["ignorePaths"]); }
            set { this["ignorePaths"] = value; }
        }

              [ConfigurationProperty("ignorePostfixs", IsRequired = false)]
        public IgnorePostfixConfigurationElementCollection IgnorePostfixs
        {
            get { return (IgnorePostfixConfigurationElementCollection)(base["ignorePostfixs"]); }
            set { this["ignorePostfixs"] = value; }
        }
    }
}