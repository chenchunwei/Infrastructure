using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Fluent.Permission.Privileges
{
    public class PrivilegeMap : ClassMap<Privilege>
    {
        public PrivilegeMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.Name);
            Map(o => o.Code);
            Map(o => o.UnitId);
            Map(o => o.Description);
        }
    }
}
