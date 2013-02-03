using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Fluent.Infrastructure.Domain.NhibernateRepository
{
    public class TransactionManager
    {
        //HACK:陈春伟 陈春伟 暂不支持 多个事务
        public static ITransaction BeginTransaction()
        {
            var sessionFactoryHelper = new DefaultSessionFactoryHelper();
            var defaultSessionFactory = new DefaultSessionManagerFactory(sessionFactoryHelper.GetSessionFactory());
            var session = defaultSessionFactory.CreateManager().OpenSession();
            return session.Transaction.IsActive ? new EmptySessionTransaction() : session.BeginTransaction();
        }
    }

    #region EmptySessionTransaction
    public class EmptySessionTransaction : ITransaction
    {
        public void Begin(System.Data.IsolationLevel isolationLevel)
        {
            return;
        }

        public void Begin()
        {
            return;

        }

        public void Commit()
        {
            return;
        }

        public void Enlist(System.Data.IDbCommand command)
        {
            return;
        }

        public bool IsActive
        {
            get { return true; }
        }

        public void RegisterSynchronization(NHibernate.Transaction.ISynchronization synchronization)
        {
            return;
        }

        public void Rollback()
        {
            return;
        }

        public bool WasCommitted
        {
            get { return true; }
        }

        public bool WasRolledBack
        {
            get { return true; }
        }

        public void Dispose()
        {
            return;
        }
    }
    #endregion
}
