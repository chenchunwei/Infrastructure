using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain;

namespace Fluent.Permission.Privileges
{
    public class Privilege : IAggregateRoot
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Code { get; set; }
        public virtual string Description { get; set; }
        public virtual Guid UnitId { get; set; }

    }
}
