using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NHibernate;

namespace Fluent.Infrastructure.Domain.NhibernateRepository
{
    public class DefaultSessionManagerFactory : ISessionManagerFactory
    {
        //HACK:陈春伟 实现多session仓储管理和生命周期管理，现在只支持单个 
        private static ISessionManager _sessionManagerInstance;
        private readonly object _locker = new object();
        private readonly ISessionFactory _sessionFactory;
        public DefaultSessionManagerFactory(ISessionFactory sessionFactory) { _sessionFactory = sessionFactory; }
        public ISessionManager CreateManager()
        {
            if (_sessionManagerInstance != null)
                return _sessionManagerInstance;
            lock (_locker)
            {
                if (_sessionManagerInstance != null)
                    return _sessionManagerInstance;
                if (HttpContext.Current == null)
                {
                    _sessionManagerInstance = new ThreadSessionManager(_sessionFactory);
                }
                else
                {
                    _sessionManagerInstance = new WebSessionManager(_sessionFactory);
                }
            }
            return _sessionManagerInstance;
        }
    }
}
