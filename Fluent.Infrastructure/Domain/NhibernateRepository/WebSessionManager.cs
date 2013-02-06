using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Log;
using NHibernate;
using System.Web;
using log4net;

namespace Fluent.Infrastructure.Domain.NhibernateRepository
{
    public class WebSessionManager : ISessionManager
    {
        public const string NhibernateSessionKey = "__nhibernateSessionKey__";
        private readonly ISessionFactory _sessionFactory;
        private readonly ILog _log;
        private static readonly object Locker = new object();

        public WebSessionManager(ISessionFactory sessionFactory)
        {
            _log = new DefaultLoggerFactory().GetLogger();
            _sessionFactory = sessionFactory;
        }

        public ISession OpenSession()
        {
            if (HttpContext.Current == null)
                throw new NotSupportedException("应用程序不支持HttpContext对象,无法使用该对象!");
            return CreateSessionThreadSafely();
        }

        private ISession CreateSessionThreadSafely()
        {
            if (ObtainSessionInHttpContext() == null)
            {
                lock (Locker)
                {
                    if (ObtainSessionInHttpContext() == null)
                    {
                        HttpContext.Current.Items[NhibernateSessionKey] = _sessionFactory.OpenSession();
                        _log.DebugFormat("在HttpContext中打开了Session,GetHashCode={0}", ObtainSessionInHttpContext().GetHashCode());
                    }
                }
            }
            var hc = ObtainSessionInHttpContext().GetHashCode();
            return ObtainSessionInHttpContext();
        }

        private ISession ObtainSessionInHttpContext()
        {
            var session = HttpContext.Current.Items[NhibernateSessionKey] as ISession;
            return session;
        }

        public void Dispose()
        {
            var session = ObtainSessionInHttpContext();
            if (session == null)
                return;
            _log.InfoFormat("HttpContext正回收Session,GetHashCode={0}", session.GetHashCode());
            session.Close();
            session.Dispose();
        }

        public static ISession Current
        {
            get
            {
                return HttpContext.Current.Items[NhibernateSessionKey] as ISession;
            }
        }
    }
}
