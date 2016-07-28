using System;
using Rld.DeviceSystem.Contract.Model.Services;
using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Model.User
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
        public Boolean UserStatus { get; set; }
        [DataMember]
        public Int32 DepartmentId { get; set; }
        [DataMember]
        public String Comment { get; set; }
        [DataMember]
        public CredentialService[] CredentialServices { get; set; }
    }
}
