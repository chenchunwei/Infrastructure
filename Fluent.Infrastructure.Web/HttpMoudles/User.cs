using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.Web.HttpMoudles
{
    public class User
    {
        public Guid Id { get; set; }
        public string Account { get; set; }
        public string Pwd { get; set; }
    }
}
