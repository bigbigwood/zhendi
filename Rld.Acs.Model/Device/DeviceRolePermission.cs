using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DeviceRolePermission
    {
        public virtual Int32 DeviceRolePermissionID { get; set; }
        public virtual DeviceRole DeviceRole { get; set; }
        public virtual DevicePermission DevicePermission { get; set; }
    }
}
