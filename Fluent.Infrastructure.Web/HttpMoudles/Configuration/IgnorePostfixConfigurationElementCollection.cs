using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Fluent.Infrastructure.Web.HttpMoudles.Configuration
{
    public class IgnorePostfixConfigurationElementCollection : ConfigurationElementCollection
    {
        #region 暂时用不上的代码
        //public IgnorePostfixConfigurationElement this[int index]
        //{
        //    get
        //    {
        //        return base.BaseGet(index) as IgnorePostfixConfigurationElement;
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
            return new IgnorePostfixConfigurationElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            var ele = element as IgnorePostfixConfigurationElement;
            if (ele == null)
                throw new ArgumentNullException("element", "element不能转换成 IgnoreConfigurationElement");
            return ele.Postfix;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "ignorePostfix"; }
        }
    }
}