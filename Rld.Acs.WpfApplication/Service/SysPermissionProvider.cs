using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using Rld.Acs.Model;
using Rld.Acs.Repository.Interfaces;
using Rld.Acs.WpfApplication.Repository;

namespace Rld.Acs.WpfApplication.Service
{
    public class SysPermissionProvider
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ISysRoleRepository _sysRoleRepo = NinjectBinder.GetRepository<ISysRoleRepository>();
        private ISysModuleRepository _sysModuleRepo = NinjectBinder.GetRepository<ISysModuleRepository>();
        private ISysModuleElementRepository _sysModuleElementRepo = NinjectBinder.GetRepository<ISysModuleElementRepository>();

        private static SysPermissionProvider _instance = null;

        public static SysPermissionProvider GetInstance()
        {
            if (_instance == null)
                _instance = new SysPermissionProvider();

            return _instance;
        }
        private SysPermissionProvider()
        {
        }


        public List<SysRole> AllSysRoles
        {
            get { return _sysRoleRepo.Query(new Hashtable()).ToList(); }
        }
        public List<SysModule> AllSysModules
        {
            get { return _sysModuleRepo.Query(new Hashtable()).ToList(); }
        }
        public List<SysModuleElement> AllSysModuleElements
        {
            get { return _sysModuleElementRepo.Query(new Hashtable()).ToList(); }
        }
    }
}
