using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DevicePermission
    {
        public virtual Int32 DevicePermissionID { get; set; }
        public virtual DeviceController Device { get; set; }
        public virtual Boolean Enable { get; set; }
        public virtual String Remark { get; set; }
        public virtual DevicePermissionAction PermissionAction { get; set; }
        public virtual String UserGroupVM { get; set; }
        public virtual TimeZone AllowedAccessTimeZone { get; set; }
        public virtual DateTime STARTDATE { get; set; }
        public virtual DateTime? Enddate { get; set; }
    }
}
