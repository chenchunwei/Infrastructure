using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fluent.Infrastructure.Web.HttpMoudles
{
    public class AuthenticationClinet
    {
        public static User CurrentUser
        {
            get
            {
                if (HttpContext.Current.Items[HttpMoudlesConst.HttpUserKey] != null)
                    return HttpContext.Current.Items[HttpMoudlesConst.HttpUserKey] as User;
                return null;
            }
        }
    }
}