using System.Linq;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.WpfApplication.Repository.System
{
    public class SysRoleRepository : CacheableRepository<SysRole, int>, ISysRoleRepository
    {
        public SysRoleRepository()
        {
            RelevantUri = "/api/SysRoles";
            CacheKey = "CacheKey_SysRoles";
            CacheExpireMinutes = SystemCacheExpireMinutes;
        }

        public override bool Update(SysRole sysRole)
        {
            return Update(sysRole, sysRole.RoleID);
        }

        public override SysRole GetByKey(int key)
        {
            return CacheableQuery().FirstOrDefault(x => x.RoleID == key);
        }
    }
}
