using System;
using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Model.Services.DeviceConnection
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class SerialConnectionService : DeviceConnectionService
    {
        [DataMember]
        public Int32 Baudrate { get; set; }
        [DataMember]
        public Int32 Port { get; set; }
    }
}
