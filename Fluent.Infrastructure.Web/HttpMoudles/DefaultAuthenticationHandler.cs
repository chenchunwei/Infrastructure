using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.Web.HttpMoudles
{
    public class DefaultAuthenticationHandler : IAuthenticationHandler
    {
        public void OnLoginStart()
        {
            return;
        }

        public void OnLoginEnd(LoginEntity loginEntity)
        {
            return;
        }

        public void OnLogoutBegin()
        {
            return;
        }

        public void OnLogoutEnd()
        {
            return;
        }
    }
}
