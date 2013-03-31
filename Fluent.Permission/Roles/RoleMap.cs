using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Fluent.Permission.Roles
{
    public class RoleMap : ClassMap<Role>
    {
        public RoleMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.Name);
            Map(o => o.Code);
            Map(o => o.Description);
            Map(o => o.UnitId);
            HasManyToMany(o => o.Privileges).ChildKeyColumn("PrivilegeId")
                .ParentKeyColumn("RoleId")
                .Table("RolePrivilegeMap");
        }
    }
}
