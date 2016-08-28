using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rld.Acs.Model
{
    public static class SysRoleExtension
    {
        public static String GetModuleString(this SysRole sysRole)
        {
            if (sysRole == null || sysRole.SysRolePermissions == null || sysRole.SysRolePermissions.Count == 0)
                return string.Empty;

            var modulePermissions = sysRole.SysRolePermissions.Where(x => x.ModuleInfo != null && x.ModuleInfo.ModuleLevel == 2);
            var modules = modulePermissions.Select(x => x.ModuleInfo);

            var moduleNames = modules.Select(x => x.ModuleName);
            return string.Join(", ", moduleNames);
        }

    }
}
