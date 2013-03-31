using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain;
using Fluent.Permission.Privileges;

namespace Fluent.Permission.Roles
{
    public class Role : IAggregateRoot
    {
        public Role()
        {
            Privileges = new List<Privilege>();
        }
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Code { get; set; }
        public virtual IList<Privilege> Privileges { get; set; }
        public virtual string Description { get; set; }
        public virtual Guid UnitId { get; set; }
    }
}
