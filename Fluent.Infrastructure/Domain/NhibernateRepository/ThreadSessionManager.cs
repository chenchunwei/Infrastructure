using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Fluent.Infrastructure.Domain.NhibernateRepository
{
    public class ThreadSessionManager : ISessionManager
    {
        [ThreadStatic]
        private static ISession _session;

        private readonly ISessionFactory _sessionFactory;

        private static readonly object Locker = new object();

        public ThreadSessionManager(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public ISession OpenSession()
        {
            return CreateSessionThreadSafely();
        }

        private ISession CreateSessionThreadSafely()
        {
            if (_session == null)
                lock (Locker)
                {
                    if (_session == null)
                    {
                        _session = _sessionFactory.OpenSession();
                    }
                }
            return _session;
        }

        public void Dispose()
        {
            if (_session == null)
                return;
            _session.Close();
            _session.Dispose();
        }
    }
}
