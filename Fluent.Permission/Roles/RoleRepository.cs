using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;

namespace Fluent.Permission.Roles
{
    public class RoleRepository : AbstractNhibernateRepository<Guid, Role>
    {
        public IList<Role> GetAllRoleByUnitId(Guid unitId)
        {
            return Session.CreateSQLQuery("select * from role where unitId=:unitId")
                          .AddEntity(typeof(Role))
                          .SetGuid("unitId", unitId)
                          .List<Role>();
        }

        public bool IsCodeRepeat(string code, Guid? roleId)
        {
            if (roleId.HasValue)
            {
                return Session.CreateSQLQuery("select count(*) from Role where code =:code and id=:roleId")
                    .SetString("code", code.Trim())
                    .SetGuid("privilegeId", roleId.Value).UniqueResult<int>() > 0;
            }
            return Session.CreateSQLQuery("select count(*) from Role where code =:code")
                    .SetString("code", code.Trim())
                    .UniqueResult<int>() > 0;
        }

        public IList<Role> GetRolesByCodes(IList<string> codes)
        {
           if(codes==null||!codes.Any())
               return new List<Role>();
            return Session.CreateSQLQuery("select * from Role where code in(:codes)")
                        .AddEntity(typeof (Role))
                        .SetParameterList("codes", codes).List<Role>();
        }
    }
}
