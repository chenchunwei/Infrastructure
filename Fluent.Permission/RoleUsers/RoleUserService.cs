using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Permission.Privileges;
using Fluent.Permission.Roles;

namespace Fluent.Permission.RoleUsers
{
    public class RoleUserService
    {
        private readonly RoleUserRepository _roleUserRepository;

        public RoleUserService()
        {
            _roleUserRepository = new RoleUserRepository();
        }

        public bool Validate(Guid userId, string privilegeCode)
        {
            var roleUser = _roleUserRepository.GetRoleUserByUserId(userId);
            return roleUser.Validate(privilegeCode); ;
        }

        public void AssignPrivilegeToUser(RoleUser roleUser, Privilege privilege)
        {
            roleUser.Privileges.Add(privilege);
        }

        public void AssignRole(RoleUser roleUser, Role role)
        {
            roleUser.Roles.Add(role);
        }
    }
}
