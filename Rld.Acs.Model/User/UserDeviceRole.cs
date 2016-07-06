using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class UserDeviceRole
    {
        public virtual Int32 UserDeviceRoleID { get; set; }
        public virtual Int32 UserID { get; set; }
        public virtual Int32 DeviceRoleID { get; set; }
    }
}
