using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.Domain.NhibernateRepository
{
    public interface ISessionManagerFactory
    {
        ISessionManager CreateManager();
    }
}
