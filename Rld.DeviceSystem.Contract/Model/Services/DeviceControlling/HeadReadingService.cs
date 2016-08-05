using System;
using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.Services.UserCredential;

namespace Rld.DeviceSystem.Contract.Model.Services.DeviceControlling
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class HeadReadingService : ServiceBase
    {
        [DataMember]
        public String Mac { get; set; }
        [DataMember]
        public String SerialNumber { get; set; }
        [DataMember]
        public Int32? Type { get; set; }
        [DataMember]
        public String Performance { get; set; }
    }
}
