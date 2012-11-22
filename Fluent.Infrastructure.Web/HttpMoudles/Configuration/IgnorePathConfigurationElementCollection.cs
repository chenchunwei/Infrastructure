using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Fluent.Infrastructure.Web.HttpMoudles.Configuration
{
    public class IgnorePathConfigurationElementCollection : ConfigurationElementCollection
    {
        #region 暂时用不上的代码
        //public IgnoreConfigurationElement this[int index]
        //{
        //    get
        //    {
        //        return base.BaseGet(index) as IgnoreConfigurationElement;
        //    }
        //    set
        //    {
        //        if (base.BaseGet(index) != null)
        //        {
        //            base.BaseRemoveAt(index);
        //        }
        //        this.BaseAdd(index, value);
        //    }
        //}
        #endregion

        protected override ConfigurationElement CreateNewElement()
        {
            return new IgnorePathConfigurationElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            var ele = element as IgnorePathConfigurationElement;
            if (ele == null)
                throw new ArgumentNullException("element", "element不能转换成 IgnoreConfigurationElement");
            return ele.Path;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "ignorePath"; }
        }
    }
}