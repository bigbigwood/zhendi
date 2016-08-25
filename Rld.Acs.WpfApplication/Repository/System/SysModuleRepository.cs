using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;

namespace Rld.Acs.WpfApplication.Repository.System
{
    public class SysModuleRepository : BaseRepository<SysModule, int>, ISysModuleRepository
    {
        public SysModuleRepository()
        {
            RelevantUri = "/api/SysModules";
        }

        public override bool Update(SysModule sysModule)
        {
            return Update(sysModule, sysModule.ModuleID);
        }
    }
}
