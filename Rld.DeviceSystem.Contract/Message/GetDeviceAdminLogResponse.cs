using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model;
using Rld.DeviceSystem.Contract.Model.Logs;

namespace Rld.DeviceSystem.Contract.Message
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class GetDeviceAdminLogResponse : ResponseBase
    {
        [DataMember]
        public IList<DeviceAdminLog> Logs { get; set; }

        public GetDeviceAdminLogResponse()
        {
            Logs = new List<DeviceAdminLog>();
        }
    }
}
