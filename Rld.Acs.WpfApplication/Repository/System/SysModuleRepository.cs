using System.Linq;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.WpfApplication.Repository.System
{
    public class SysModuleRepository : CacheableRepository<SysModule, int>, ISysModuleRepository
    {
        public SysModuleRepository()
        {
            RelevantUri = "/api/SysModules";
            CacheKey = "CacheKey_SysModules";
            CacheExpireMinutes = SystemCacheExpireMinutes;
        }

        public override bool Update(SysModule sysModule)
        {
            return Update(sysModule, sysModule.ModuleID);
        }

        public override SysModule GetByKey(int key)
        {
            return CacheableQuery().FirstOrDefault(x => x.ModuleID == key);
        }
    }
}
