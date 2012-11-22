using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.Domain
{
    public interface IRepository<TKey, TAggregateRoot>
        where TKey : IComparable
        where TAggregateRoot : IAggregateRoot
    {
        TAggregateRoot Find(TKey key);
        void Update(TAggregateRoot aggregateRoot);
        void Save(TAggregateRoot aggregateRoot);
        void SaveOrUpdate(TAggregateRoot aggregateRoot);
    }
}
