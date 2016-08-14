using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.Contract.Model.Logs;

namespace Rld.DeviceSystem.Contract.Message
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetDeviceTrafficLogResponse : ResponseBase
    {
        [DataMember]
        public IList<DeviceTrafficLog> Logs { get; set; }

        public GetDeviceTrafficLogResponse()
        {
            Logs = new List<DeviceTrafficLog>();
        }
    }
}
