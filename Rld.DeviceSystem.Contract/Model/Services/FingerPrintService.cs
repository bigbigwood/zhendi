using System;
using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Model.Services
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class FingerPrintService : CredentialService
    {
        [DataMember]
        public Int32 Index { get; set; }
        [DataMember]
        public String FingerPrintData { get; set; }
    }
}
