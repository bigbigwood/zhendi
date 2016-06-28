using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class UserAuthentication
    {
        public Int32 UserAuthenticationID { get; set; }
        public Int32 DeviceUserID { get; set; }
        public DeviceType DeviceType { get; set; }
        public AuthenticationType AuthenticationType { get; set; }
        public String AuthenticationData { get; set; }
        public String Version { get; set; }
        public Boolean IsDuress { get; set; }
        public String Remark { get; set; }
        public Int32 CreateUserID { get; set; }
        public DateTime CreateDate { get; set; }
        public GeneralStatus Status { get; set; }
        public Int32? UpdateUserID { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
