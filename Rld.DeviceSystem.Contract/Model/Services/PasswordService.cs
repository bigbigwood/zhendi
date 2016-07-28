using System;
using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Model.Services
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class PasswordService : CredentialService
    {
        [DataMember]
        public String Password { get; set; }
    }
}
