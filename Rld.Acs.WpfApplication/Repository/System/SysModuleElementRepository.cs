using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.WpfApplication.Repository.System
{
    public class SysModuleElementRepository : BaseRepository<SysModuleElement, int>, ISysModuleElementRepository
    {
        public SysModuleElementRepository()
        {
            RelevantUri = "/api/SysModuleElements";
        }

        public override bool Update(SysModuleElement sysModuleElement)
        {
            return Update(sysModuleElement, sysModuleElement.ElementID);
        }
    }
}
