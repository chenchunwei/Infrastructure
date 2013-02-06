using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.Web.HttpMoudles
{
    public interface IAuthenticationHandler
    {
        void OnLoginStart();
        void OnLoginEnd(LoginEntity loginEntity);
        void OnLogoutBegin();
        void OnLogoutEnd();
    }
}
