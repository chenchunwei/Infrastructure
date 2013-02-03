using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Fluent.Infrastructure.Log;
using NHibernate;
using log4net;

namespace Fluent.Infrastructure.Domain.NhibernateRepository
{
    public class ThreadSessionManager : ISessionManager
    {
        [ThreadStatic]
        private static ISession _session;

        private readonly ISessionFactory _sessionFactory;
        private readonly ILog _log;
        private static readonly object Locker = new object();

        public ThreadSessionManager(ISessionFactory sessionFactory)
        {
            _log = new DefaultLoggerFactory().GetLogger();
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
                        _log.InfoFormat("在线程中打开了Session,GetHashCode={0}", _session.GetHashCode());
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
