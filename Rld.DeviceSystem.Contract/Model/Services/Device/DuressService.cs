using System;
using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Model.Services.Device
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class DuressService : ServiceBase
    {
        [DataMember]
        public Int32? FingerPrintIndex { get; set; }
        [DataMember]
        public String Password { get; set; }
        [DataMember]
        public Boolean? IsOpenDoor { get; set; }
        [DataMember]
        public Boolean? IsTriggerAlarm { get; set; }
    }
}
