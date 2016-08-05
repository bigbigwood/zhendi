using System;
using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Model.Services.UserCredential
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class CredentialCardService : CredentialService
    {
        [DataMember]
        public String CardNumber { get; set; }
    }
}
