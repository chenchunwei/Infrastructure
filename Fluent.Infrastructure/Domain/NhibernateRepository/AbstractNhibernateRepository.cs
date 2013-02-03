using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Fluent.Infrastructure.Domain.NhibernateRepository
{
    public class AbstractNhibernateRepository<TKey, TAggregateRoot> : IRepository<TKey, TAggregateRoot>
        where TKey : IComparable
        where TAggregateRoot : IAggregateRoot
    {
        private readonly DefaultSessionFactoryHelper _defaultSessionFactoryHelper;

        private readonly ISessionManagerFactory _sessionManagerFactory;

        protected ISession Session { get { return _sessionManagerFactory.CreateManager().OpenSession(); } }

        public AbstractNhibernateRepository()
        {
            _defaultSessionFactoryHelper = new DefaultSessionFactoryHelper();
            _sessionManagerFactory = new DefaultSessionManagerFactory(_defaultSessionFactoryHelper.GetSessionFactory());
        }

        public TAggregateRoot Find(TKey key)
        {
            return Session.Get<TAggregateRoot>(key);
        }

        public void Update(TAggregateRoot aggregateRoot)
        {
            Session.Update(aggregateRoot);
            Flush();
        }

        public void Save(TAggregateRoot aggregateRoot)
        {
            Session.Save(aggregateRoot);
            Flush();
        }

        public void SaveOrUpdate(TAggregateRoot aggregateRoot)
        {
            Session.SaveOrUpdate(aggregateRoot);
            Flush();
        }

        private void Flush()
        {
            Session.Flush();
        }

        protected void SetParameters(IQuery query, Hashtable parameterValues)
        {
            foreach (DictionaryEntry entry in parameterValues)
            {
                if (entry.Value is ICollection)
                    query.SetParameterList(entry.Key.ToString(), (ICollection)entry.Value);
                else
                    query.SetParameter(entry.Key.ToString(), entry.Value);
            }
        }
    }
}
