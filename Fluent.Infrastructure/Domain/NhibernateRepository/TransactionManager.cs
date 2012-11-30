using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Fluent.Infrastructure.Domain.NhibernateRepository
{
    public class TransactionManager
    {
        public static ITransaction BeginTransaction()
        {
            var sessionFactoryHelper = new DefaultSessionFactoryHelper();
            var defaultSessionFactory = new DefaultSessionManagerFactory(sessionFactoryHelper.GetSessionFactory());
            var session = defaultSessionFactory.CreateManager().OpenSession();
            return session.BeginTransaction();
        }
    }
}
