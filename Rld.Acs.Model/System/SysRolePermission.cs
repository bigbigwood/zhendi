using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class SysRolePermission
    {
        public virtual Int32 SysRolePermissionID { get; set; }
        public virtual SysRole RoleInfo { get; set; }
        public virtual SysModule ModuleInfo { get; set; }
        public virtual SysModuleElement ElementInfo { get; set; }
        public virtual Boolean Visible { get; set; }
        public virtual Boolean Enabled { get; set; }
    }
}
