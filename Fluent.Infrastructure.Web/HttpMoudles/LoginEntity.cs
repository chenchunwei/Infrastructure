using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.Web.HttpMoudles
{
    public class LoginEntity
    {
        public string ReferrerUrl { get; set; }
        public string RedirectUrl { get; set; }
        public bool IsLoginSuccess { get; set; }
        public string UserName { get; set; }
        public string Pwd { get; set; }
        public string Extension { get; set; }
    }
}
