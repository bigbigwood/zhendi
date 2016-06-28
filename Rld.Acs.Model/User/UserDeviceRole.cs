using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class UserDeviceRole
    {
        public Int32 UserDeviceRoleID { get; set; }
        public User User { get; set; }
        public DeviceRole DeviceRole { get; set; }
        public Int32 CreateUserID { get; set; }
        public DateTime CreateDate { get; set; }
        public GeneralStatus Status { get; set; }
        public DateTime? UpdateUserID { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
