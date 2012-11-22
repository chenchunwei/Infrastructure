using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.Web.HttpMoudles
{
    public interface IUserService
    {
        IUser Authencation(string account, string password);
        IUser GetAuthencationUser(string account, string password);
        IUser GetAuthencationUser(string account);
    }
}
