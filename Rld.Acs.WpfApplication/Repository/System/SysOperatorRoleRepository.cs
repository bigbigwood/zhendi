using System;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.WpfApplication.Repository.System
{
    public class SysOperatorRoleRepository : BaseRepository<SysOperatorRole, int>, ISysOperatorRoleRepository
    {
        public SysOperatorRoleRepository()
        {
            RelevantUri = "/api/SysOperatorRoles";
        }
    }
}
