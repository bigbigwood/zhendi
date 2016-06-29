using System;
using System.Collections.Generic;
using System.Text;

namespace Rld.Acs.Model
{
    public class UserAuthentication
    {
        public virtual Int32 UserAuthenticationID { get; set; }
        public virtual Int32 DeviceUserID { get; set; }
        public virtual DeviceType DeviceType { get; set; }
        public virtual AuthenticationType AuthenticationType { get; set; }
        public virtual String AuthenticationData { get; set; }
        public virtual String Version { get; set; }
        public virtual Boolean IsDuress { get; set; }
        public virtual String Remark { get; set; }
        public virtual Int32 CreateUserID { get; set; }
        public virtual DateTime CreateDate { get; set; }
        public virtual GeneralStatus Status { get; set; }
        public virtual Int32? UpdateUserID { get; set; }
        public virtual DateTime? UpdateDate { get; set; }
    }
}
