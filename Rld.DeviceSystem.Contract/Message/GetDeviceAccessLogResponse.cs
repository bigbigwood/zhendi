using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.Contract.Model.Logs;

namespace Rld.DeviceSystem.Contract.Message
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetDeviceAccessLogResponse : ResponseBase
    {
        [DataMember]
        public IList<DeviceAccessLog> Logs { get; set; }

        public GetDeviceAccessLogResponse()
        {
            Logs = new List<DeviceAccessLog>();
        }
    }
}
