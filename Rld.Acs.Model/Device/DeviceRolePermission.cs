using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DeviceRolePermission
    {
        public virtual Int32 DeviceRolePermissionID { get; set; }
        public virtual Int32 DeviceRoleID { get; set; }
        public virtual Int32 DeviceID { get; set; }
        public virtual Boolean Enable { get; set; }
        public virtual String Remark { get; set; }
        public virtual DevicePermissionAction PermissionAction { get; set; }
        public virtual String UserGroupVM { get; set; }
        public virtual Int32 AllowedAccessTimeZoneID { get; set; }
        public virtual DateTime STARTDATE { get; set; }
        public virtual DateTime? Enddate { get; set; }
    }
}
