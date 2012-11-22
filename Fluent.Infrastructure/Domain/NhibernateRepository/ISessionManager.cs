using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Fluent.Infrastructure.Domain.NhibernateRepository
{
    public interface ISessionManager : IDisposable
    {
        ISession OpenSession();
    }
}
