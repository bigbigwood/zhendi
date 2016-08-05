using System;
using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Model.Services.UserCredential
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class PasswordService : CredentialService
    {
        [DataMember]
        public String Password { get; set; }
    }
}
