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


        public List<SysRole> AllSysRoles { get; set; }
        public List<SysModule> AllSysModules { get; set; }
        public List<SysModuleElement> AllSysModuleElements { get; set; }

        private static SysPermissionProvider _instance = null;

        public static SysPermissionProvider GetInstance()
        {
            if (_instance == null)
                _instance = new SysPermissionProvider();

            return _instance;
        }
        private SysPermissionProvider()
        {
            InitResource();
        }


        private void InitResource()
        {
            AllSysRoles = _sysRoleRepo.Query(new Hashtable()).ToList();
            AllSysModules = _sysModuleRepo.Query(new Hashtable()).ToList();
            AllSysModuleElements = _sysModuleElementRepo.Query(new Hashtable()).ToList();
        }
    }
}
