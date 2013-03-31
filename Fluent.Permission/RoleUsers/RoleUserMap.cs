using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace Fluent.Permission.RoleUsers
{
    public class RoleUserMap : ClassMap<RoleUser>
    {
        public RoleUserMap()
        {
            Id(o => o.Id).GeneratedBy.GuidComb();
            Map(o => o.UserId);
            HasManyToMany(o => o.Privileges)
                .ParentKeyColumn("RoleUserId")
                .ChildKeyColumn("PrivilegeId")
                .Table("RoleUserPrivilegeMap");
            HasManyToMany(o => o.Roles)
                .ParentKeyColumn("RoleUserId")
                .ChildKeyColumn("RoleId")
                .Table("RoleUserRoleMap");
        }
    }
}
