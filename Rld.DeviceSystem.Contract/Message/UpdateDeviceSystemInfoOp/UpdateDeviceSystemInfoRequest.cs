using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.DeviceSystem.Contract.Message.UpdateDeviceSystemInfoOp
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class UpdateDeviceSystemInfoRequest : RequestBase
    {
        [DataMember]
        public DeviceSystemInfo DeviceInfo { get; set; }
    }
}
