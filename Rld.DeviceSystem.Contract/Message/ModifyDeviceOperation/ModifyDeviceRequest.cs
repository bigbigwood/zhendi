using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model.Devices;
using Rld.DeviceSystem.Contract.Model.Users;

namespace Rld.DeviceSystem.Contract.Message.ModifyDeviceOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class ModifyDeviceRequest : RequestBase
    {
        [DataMember]
        public Device DeviceInfo { get; set; }
    }
}
