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
    }
}
