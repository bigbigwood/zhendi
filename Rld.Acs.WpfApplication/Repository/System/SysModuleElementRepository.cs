using System.Linq;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.WpfApplication.Repository.System
{
    public class SysModuleElementRepository : CacheableRepository<SysModuleElement, int>, ISysModuleElementRepository
    {
        public SysModuleElementRepository()
        {
            RelevantUri = "/api/SysModuleElements";
            CacheKey = "CacheKey_SysModuleElements";
            CacheExpireMinutes = SystemCacheExpireMinutes;
        }

        public override bool Update(SysModuleElement sysModuleElement)
        {
            return Update(sysModuleElement, sysModuleElement.ElementID);
        }

        public override SysModuleElement GetByKey(int key)
        {
            return CacheableQuery().FirstOrDefault(x => x.ElementID == key);
        }
    }
}
