using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Fluent.Infrastructure.Utilities
{
    public class EnumHelper
    {
        public static string GetDescription(Enum e)
        {
            FieldInfo enumInfo = e.GetType().GetField(e.ToString());
            var enumAttributes = (DescriptionAttribute[])enumInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return enumAttributes.Length > 0 ? enumAttributes[0].Description : e.ToString();
        }
    }
}
