using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.Services.UserCredential;

namespace Rld.DeviceSystem.Contract.Model
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class UserInfo
    {
        [DataMember]
        public Int32 UserId { get; set; }
        [DataMember]
        public String ExternalUserCode { get; set; }
        [DataMember]
        public UserRole? Role { get; set; }
        [DataMember]
        public String UserName { get; set; }
        [DataMember]
        public Boolean? UserStatus { get; set; }
        [DataMember]
        public Int32? DepartmentId { get; set; }
        [DataMember]
        public String Comment { get; set; }
        [DataMember]
        public Int32? AccessTimeZoneId { get; set; }
        [DataMember]
        public IList<CredentialService> CredentialServices { get; set; }

        public UserInfo()
        {
            CredentialServices = new List<CredentialService>();
        }
    }
}
