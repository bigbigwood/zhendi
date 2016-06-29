using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class UserDeviceRole
    {
        public virtual Int32 UserDeviceRoleID { get; set; }
        public virtual User User { get; set; }
        public virtual DeviceRole DeviceRole { get; set; }
        public virtual Int32 CreateUserID { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual GeneralStatus Status { get; set; }
        public virtual DateTime? UpdateUserID { get; set; }
        public virtual DateTime? UpdateDate { get; set; }
    }
}
