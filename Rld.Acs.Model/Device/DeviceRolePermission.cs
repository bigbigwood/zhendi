using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DeviceRolePermission
    {
        public Int32 DeviceRolePermissionID { get; set; }
        public DeviceRole DeviceRole { get; set; }
        public DevicePermission DevicePermission { get; set; }
    }
}
