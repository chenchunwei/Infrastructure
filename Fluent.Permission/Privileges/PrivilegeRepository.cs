using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fluent.Infrastructure.Domain.NhibernateRepository;

namespace Fluent.Permission.Privileges
{
    public class PrivilegeRepository : AbstractNhibernateRepository<Guid, Privilege>
    {
        public IList<Privilege> GetAllPrivileges()
        {
            return Session.CreateSQLQuery("select * from Privilege").AddEntity(typeof(Privilege))
                .List<Privilege>();
        }

        public bool IsCodeRepeat(string code, Guid? privilegeId)
        {
            if (privilegeId.HasValue)
            {
                return Session.CreateSQLQuery("select count(*) from Privilege where code =:code and id=:privilegeId")
                    .SetString("code",code.Trim())
                    .SetGuid("privilegeId", privilegeId.Value).UniqueResult<int>() > 0;
            }
            return Session.CreateSQLQuery("select count(*) from Privilege where code =:code")
                    .SetString("code", code.Trim())
                    .UniqueResult<int>() > 0;
        }
    }
}
