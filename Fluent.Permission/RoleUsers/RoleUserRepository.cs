using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;
using Fluent.Permission.RoleUsers;
using Fluent.Permission.Roles;

namespace Fluent.Permission.RoleUsers
{
    public class RoleUserRepository : AbstractNhibernateRepository<Guid, RoleUser>
    {
        public RoleUser GetRoleUserByUserId(Guid userId)
        {
            return Session.CreateSQLQuery("select top 1 * from RoleUser where userId=:userId")
                          .AddEntity(typeof(RoleUser))
                          .SetGuid("userId", userId)
                          .UniqueResult<RoleUser>();
        }

        public IList<RoleUser> GetRoleUsersByUserIds(IEnumerable<Guid> userIds)
        {
            if (userIds == null)
                return new List<RoleUser>();
            return Session.CreateSQLQuery("select * from RoleUser where userId in (:userIds)")
                          .AddEntity(typeof(RoleUser))
                          .SetParameterList("userIds", userIds.ToList())
                          .List<RoleUser>();
        }
    }
}
