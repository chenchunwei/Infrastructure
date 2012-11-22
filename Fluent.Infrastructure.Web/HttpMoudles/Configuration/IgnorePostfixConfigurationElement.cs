using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Fluent.Infrastructure.Web.HttpMoudles.Configuration
{
    public class IgnorePostfixConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("postfix", IsRequired = true)]
        public string Postfix
        {
            get { return this["postfix"].ToString(); }
            set { this["postfix"] = value; }
        }
    }
}