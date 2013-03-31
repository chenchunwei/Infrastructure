using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain;
using Fluent.Permission.Privileges;
using Fluent.Permission.Roles;

namespace Fluent.Permission.RoleUsers
{
    public class RoleUser : IAggregateRoot
    {
        public RoleUser()
        {
            Privileges = new List<Privilege>();
            Roles = new List<Role>();

            _effectivePrivileges = new List<Privilege>();

        }
        private readonly object _locker = new object();
        private List<Privilege> _effectivePrivileges;
        public virtual Guid Id { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual IList<Privilege> Privileges { get; set; }
        public virtual IList<Role> Roles { get; set; }
        /// <summary>
        /// 包含角色内的所有权限及个人权限
        /// </summary>
        public virtual IList<Privilege> EffectivePrivileges
        {
            get
            {
                if (_effectivePrivileges == null)
                {
                    lock (_locker)
                    {
                        if (_effectivePrivileges == null)
                        {
                            _effectivePrivileges = new List<Privilege>();
                            _effectivePrivileges.AddRange(Privileges);
                            Roles.ToList().ForEach(role => _effectivePrivileges.AddRange(role.Privileges));
                        }
                    }
                }
                return _effectivePrivileges;
            }
        }
        /// <summary>
        /// 判断是否有权限
        /// </summary>
        /// <param name="privilegeCode"></param>
        /// <returns></returns>
        public virtual bool Validate(string privilegeCode)
        {
            return EffectivePrivileges.Any(o => o.Code == privilegeCode);
        }
    }
}
