using System;
using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Model.Services.DeviceConn
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class TcpConnectionService : DeviceConnectionService
    {
        [DataMember]
        public String IpAddress { get; set; }
        [DataMember]
        public Int32 Port { get; set; }
    }
}
