using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.WpfApplication.Repository.System
{
    public class SysRoleRepository : BaseRepository<SysRole, int>, ISysRoleRepository
    {
        public SysRoleRepository()
        {
            RelevantUri = "/api/SysRoles";
        }

        public override bool Update(SysRole sysRole)
        {
            return Update(sysRole, sysRole.RoleID);
        }
    }
}
