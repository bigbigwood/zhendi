using System.Runtime.Serialization;
using Rld.DeviceSystem.Contract.Model;

namespace Rld.DeviceSystem.Contract.Message.ModifyDeviceOperation
{
    [DataContract(Namespace = Declarations.NameSpace)]
    public class ModifyDeviceRequest : RequestBase
    {
        [DataMember]
        public DeviceSystemInfo DeviceInfo { get; set; }
    }
}
