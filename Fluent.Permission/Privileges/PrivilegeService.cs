using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fluent.Permission.Privileges
{
    public class PrivilegeService
    {
        private readonly PrivilegeRepository _privilegeRepository;
        public PrivilegeService()
        {
            _privilegeRepository = new PrivilegeRepository();
        }
        public void AddPrivilege(Guid unitId, string name, string code, string description)
        {
            var privilege = new Privilege();
            privilege.Code = code;
            privilege.Name = name;
            privilege.Description = description;
            privilege.UnitId = unitId;
            _privilegeRepository.Save(privilege);
        }
    }
}
