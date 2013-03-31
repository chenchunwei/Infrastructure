using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluent.Permission.Roles
{
    public class RoleService
    {
        private readonly RoleRepository _roleRepository;
        public RoleService()
        {
            _roleRepository=new RoleRepository();
        }

        public void Add(Guid unitId, string roleName, string roleCode, string description)
        {
            var role = new Role();
            role.Name = roleName;
            role.Code = roleCode;
            role.Description = description;
            role.UnitId = unitId;
            _roleRepository.Save(role);
        }
    }
}
