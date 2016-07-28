using System;
using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Model.Services
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class CredentialService : BaseService
    {
        [DataMember]
        public Boolean UseForDuress { get; set; }
    }
}
