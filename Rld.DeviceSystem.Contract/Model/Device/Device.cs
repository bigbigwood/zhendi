using System;
using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Model.Device
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class Device
    {
        [DataMember]
        public Int32 DeviceId { get; set; }
        [DataMember]
        public String SerialNumber { get; set; }
        [DataMember]
        public String DeviceModel { get; set; }
    }
}
