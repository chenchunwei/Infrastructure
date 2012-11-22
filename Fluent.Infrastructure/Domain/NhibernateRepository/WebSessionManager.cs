using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Web;

namespace Fluent.Infrastructure.Domain.NhibernateRepository
{
    public class WebSessionManager : ISessionManager
    {
        private const string NhibernateSessionKey = "__nhibernateSessionKey__";

        private readonly ISessionFactory _sessionFactory;

        private static readonly object Locker = new object();

        public WebSessionManager(ISessionFactory sessionFactory)
        {
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
                lock (Locker)
                {
                    if (ObtainSessionInHttpContext() == null)
                    {
                        HttpContext.Current.Items[NhibernateSessionKey] = _sessionFactory.OpenSession();
                    }
                }
            return ObtainSessionInHttpContext();
        }

        private ISession ObtainSessionInHttpContext()
        {
            return HttpContext.Current.Items[NhibernateSessionKey] as ISession;
        }

        public void Dispose()
        {
            var session = ObtainSessionInHttpContext();
            if (session == null)
                return;
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
