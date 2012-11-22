using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.Web.HttpMoudles
{
    public interface IUser
    {
        string Account { get; set; }
        string Pwd { get; set; }
    }
}
