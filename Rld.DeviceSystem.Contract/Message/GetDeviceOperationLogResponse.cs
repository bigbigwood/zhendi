using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.Contract.Model.Logs;

namespace Rld.DeviceSystem.Contract.Message
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetDeviceOperationLogResponse : ResponseBase
    {
        [DataMember]
        public IList<DeviceOperationLog> Logs { get; set; }

        public GetDeviceOperationLogResponse()
        {
            Logs = new List<DeviceOperationLog>();
        }
    }
}
