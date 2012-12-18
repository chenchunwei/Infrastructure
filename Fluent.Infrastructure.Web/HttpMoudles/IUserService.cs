using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.Web.HttpMoudles
{
    public interface IUserService
    {
        User Authencation(string account, string password);
        User GetAuthencationUser(string account, string password);
        User GetAuthencationUser(string account);
    }
}
