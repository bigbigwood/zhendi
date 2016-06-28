using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DeviceRolePermission
    {
        public Int32 DeviceRolePermissionID { get; set; }
        public Int32 DeviceRoleID { get; set; }
        public Int32 DevicePermissionID { get; set; }
    }
}
