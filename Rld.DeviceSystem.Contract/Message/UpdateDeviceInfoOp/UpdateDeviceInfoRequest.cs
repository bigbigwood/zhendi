using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.DeviceSystem.Contract.Message.UpdateDeviceInfoOp
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class UpdateDeviceInfoRequest : RequestBase
    {
        [DataMember]
        public DeviceInfo DeviceInfo { get; set; }
    }
}
