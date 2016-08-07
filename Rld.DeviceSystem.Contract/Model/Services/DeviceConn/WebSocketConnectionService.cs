using System;
using System.Runtime.Serialization;

namespace Rld.DeviceSystem.Contract.Model.Services.DeviceConn
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class WebSocketConnectionService : DeviceConnectionService
    {
        [DataMember]
        public String ServerUrl { get; set; }
    }
}
