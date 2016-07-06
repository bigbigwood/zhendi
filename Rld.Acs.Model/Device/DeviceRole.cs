using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class DeviceRole
    {
        public virtual Int32 DeviceRoleID { get; set; }
        public virtual String RoleName { get; set; }
        public virtual Int32 CreateUserID { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual GeneralStatus Status { get; set; }
        public virtual Int32? UpdateUserID { get; set; }
        public virtual DateTime? UpdateDate { get; set; }
        public virtual IList<DeviceRolePermission> DeviceRolePermissions { get; set; }

        public DeviceRole()
        {
            DeviceRolePermissions = new List<DeviceRolePermission>();
        }
    }
}
