using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.Utilities
{
    public static class StringExtensions
    {
        public static string ToHtml(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            str = str.Replace("\r\n", "<br/>");
            str = str.Replace("\n", "<br/>");
            return str.Replace(" ", "&nbsp;");
        }
    }
}
