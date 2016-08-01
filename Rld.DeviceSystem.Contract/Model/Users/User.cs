using System;
using Rld.DeviceSystem.Contract.Model.Services;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Rld.DeviceSystem.Contract.Model.Users
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class User
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

        public User()
        {
            CredentialServices = new List<CredentialService>();
        }
    }
}
