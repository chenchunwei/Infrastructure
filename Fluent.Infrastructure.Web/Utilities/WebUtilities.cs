using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace Fluent.Infrastructure.Web.Utilities
{
    public class WebUtilities
    {
        public static string GetRelativePathWithApplicationHost(string url)
        {
            var virtualPath = HostingEnvironment.ApplicationHost.GetVirtualPath();
            if (url.StartsWith("~/"))
            {
                if (!virtualPath.EndsWith("/"))
                    virtualPath = virtualPath + "/";
                return virtualPath + url.TrimStart(new char[] { '~', '/' });
            }
            return url;
        }
    }
}
