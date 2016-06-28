using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DevicePermission
    {
        public Int32 DevicePermissionID { get; set; }
        public DeviceController Device { get; set; }
        public Boolean Enable { get; set; }
        public String Remark { get; set; }
        public DevicePermissionAction PermissionAction { get; set; }
        public String UserGroupVM { get; set; }
        public TimeZone AllowedAccessTimeZone { get; set; }
        public DateTime STARTDATE { get; set; }
        public DateTime? Enddate { get; set; }
    }
}
