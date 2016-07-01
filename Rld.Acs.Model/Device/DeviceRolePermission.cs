using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DeviceRolePermission
    {
        public virtual Int32 DeviceRolePermissionID { get; set; }
        public virtual Int32 DeviceRoleID { get; set; }
        public virtual Int32 DevicePermissionID { get; set; }
    }
}
